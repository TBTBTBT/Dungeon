using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
public class Test{
    public static IEnumerator CMaze(int i)
    {
        Debug.Log("IN");
        yield return null;
    }
}
public class FieldManager : SingletonMonoBehaviour<FieldManager> {
    public enum FieldType
    {
        None,
        Floor,
        EnemySpawn,
        Tresure,
        Shop
    }
    public class FieldState{
        public bool isPassable { get; set; }
        public FieldType[,] State;//道、部屋の間取り
        public int RoomSize { get; set; }
        public int RoadSize { get; set; }
        public FieldState(bool passable){
            isPassable = passable;
            RoomSize = FieldManager.Instance.RoomSize;
            RoadSize = FieldManager.Instance.RoadSize;
            int s = FieldManager.Instance.SectionSize;
            State = new FieldType[s,s];
        }
    }

    //param

    [SerializeField]int             _floorWidth = 10; //全体の大きさ(x)
    [SerializeField]int             _floorHeight = 10;//全体の大きさ(y)
    [SerializeField]int             _roadSize = 3;    //道の大きさ
    [SerializeField]int             _roomSize = 3;    //部屋の大きさ
    [SerializeField]int             _sectionSize = 5;    //部屋の大きさ
    private FieldState[,]           _field;
    private Statemachine<State>     _statemachine = new Statemachine<State>();

    //event

    //public UnityEvent OnChangeFieldState = new UnityEvent ();

    // statemachine delegate

    public IStatemachine CreateFieldSubstate = null;

    // publicmethod

    public FieldState[,] Field() => _field;
    public int SectionSize => _sectionSize;
    public int RoadSize => _roadSize;
    public int RoomSize => _roomSize;
    //eventmethod

    protected override void Awake()
	{
        base.Awake();
        _statemachine.Init(this);
	}
	private void Update()
	{
        //Debug.Log("---------------");
        _statemachine.Update();

	}



    // statemachine

    public enum State
    {
        Init,
        CreateField,
        Setting,
        Wait

    }

	IEnumerator Init(){
        yield return FieldInit();
        _statemachine.Next(State.CreateField);
        yield return null;
    }
    IEnumerator CreateField(){
       // yield return new WaitForSeconds(5);
        if(CreateFieldSubstate != null){
            while(CreateFieldSubstate.Update()){
                yield return null;
            }
        }
        _statemachine.Next(State.Setting);
        yield return null;
    }
    IEnumerator Setting(){
        _statemachine.Next(State.Wait);
        yield return null;
    }
    IEnumerator FieldInit(){
        Debug.Log("asdf");
       _field = new FieldState[_floorWidth, _floorHeight];

        yield return DungeonGenerator.CreateMaze(_field, _floorWidth, _floorHeight);
        //yield return DungeonGenerator.CreateMaze(_field,_floorWidth, _floorHeight);
        Debug.Log("OUT");
    }
   
}
class DungeonGenerator{
    static List<Vector2Int> _direction = new List<Vector2Int>(){
            new Vector2Int(-1,0),
            new Vector2Int(0,-1),
            new Vector2Int(1,0),
            new Vector2Int(0,1)
        };
    /// <summary>
    /// 穴掘り法
    /// </summary>
    /// <returns>The maze.</returns>
    public static IEnumerator CreateMaze(FieldManager.FieldState[,] ret,int w,int h){
        Debug.Log("IN");
        for (int i = 0; i < ret.Length; i++)
        {
            ret[i % ret.GetLength(0), (i / ret.GetLength(0)) % ret.GetLength(1)] = new FieldManager.FieldState(false);
        }
        Dig(ret, new Vector2Int(1, 1));
        yield return null;
        AdjustMaze(ret);//少し繋ぐ
        yield return null;
        SetState(ret);
        //input = ret;
    }

    static bool Dig(FieldManager.FieldState[,] ret, Vector2Int pos)
    {
        if (!isExist(ret, pos))
        {
            return false;
        }
        if (ret[pos.x, pos.y].isPassable)
        {
            return false;
        }

        ret[pos.x, pos.y].isPassable = true;


        //次へ(全ての方向をチェック)
        //確認する順番はランダム
        Vector2Int[] direction = _direction.OrderBy((cur) => System.Guid.NewGuid()).ToArray().Clone() as Vector2Int[];
        for (int i = 0; i < direction.Length; i++)
        {
            Vector2Int next = direction[i] + pos;
            Vector2Int nextnext = direction[i] * 2 + pos;
            /*if(!isExist(ret,nextnext)){
                continue;
            }
            if(ret[nextnext.x, nextnext.y].isPassable){
                continue;
            }*/
            if (!Dig(ret, nextnext))
            {
                continue;
            }
            ret[next.x, next.y].isPassable = true;
        }
        return true;

    }
    //調整
    static void AdjustMaze(FieldManager.FieldState[,] ret){
        foreach(var r in ret){
            if(!r.isPassable){
                if(Random.Range(0,10) < 1){
                    r.isPassable = true;
                }
            }
        }
    }
    static void SetRoad(FieldManager.FieldState[,] ret,Vector2Int pos){
        int maxX = ret[pos.x, pos.y].State.GetLength(0);
        int maxY = ret[pos.x, pos.y].State.GetLength(1);
        int size = ret[pos.x, pos.y].RoadSize;
        List<bool> isLink = new List<bool>();
        foreach (var d in _direction){
            bool ispassable = false;
            if(isExist(ret, pos + d)){
                ispassable = ret[pos.x + d.x, pos.y + d.y].isPassable;
            }

            isLink.Add(ispassable);
        }
        //bool condition = true;
        //for ()
        for (int i = 0; i < maxX; i++)
        {
            for (int j = 0; j < maxY; j++)
            {
                bool condition = i >= (maxX / 2) - size / 2 && i <= (maxX / 2) + size / 2;
                condition |= j >= (maxY / 2) - size / 2 && j <= (maxY / 2) + size / 2;
                condition &= i >= (maxX / 2) - size / 2 || isLink[0];
                condition &= j >= (maxY / 2) - size / 2 || isLink[1];
                condition &= i <= (maxX / 2) + size / 2 || isLink[2];
                condition &= j <= (maxY / 2) + size / 2 || isLink[3];

                    if(condition)
                {
                    ret[pos.x, pos.y].State[i, j] = FieldManager.FieldType.Floor;
                }

            }
        }
    }
    static void SetState(FieldManager.FieldState[,] ret){
        for (int i = 0; i < ret.GetLength(0); i++)
        {
            for (int j = 0; j < ret.GetLength(1); j++)
            {
                if (ret[i, j].isPassable)
                {
                    SetRoad(ret,new Vector2Int(i,j));
                }
            }
        }

    }
   
    static bool isExist(FieldManager.FieldState[,] ret, Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 && ret.GetLength(0) > pos.x && ret.GetLength(1) > pos.y;

    }
    static void StickDown(){
        
    }
}