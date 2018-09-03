using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class FieldToTileMap : MonoBehaviour {
    enum State{
        TileUpdateAsync
    }
    public Tilemap _tileGrid;
    public TileBase _floortile;
    public TileBase _walltile;

    private Statemachine<State> _statemachine = new Statemachine<State>();

	private void Awake()
	{
        
        _statemachine.Init(this);
        FieldManager.Instance.CreateFieldSubstate = _statemachine;
	}
    IEnumerator TileUpdateAsync(){
        _tileGrid.RefreshAllTiles();
        FieldManager.FieldState[,] field = FieldManager.Instance.Field();
        int sectionSize = FieldManager.Instance.SectionSize;
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {

                for (int k = 0; k < sectionSize; k++)
                {
                    for (int l = 0; l < sectionSize; l++)
                    {
                        Vector3Int pos = new Vector3Int(i, j, 0) * sectionSize + new Vector3Int(k, l, 0);
                        if (!field[i, j].isPassable)
                        {
                            _tileGrid.SetTile(pos, _walltile);
                            continue;
                        }
                        switch(field[i, j].State[k, l]){
                            case FieldManager.FieldType.None:
                                _tileGrid.SetTile(pos, _walltile);
                                break;
                            case FieldManager.FieldType.Floor:
                                _tileGrid.SetTile(pos, _floortile);
                                break;
                            case FieldManager.FieldType.EnemySpawn:
                                break;
                        }

                    }

                }
            }
            yield return null;
        }
    }

}

/*
    /// <summary>
    /// Tiles the update.
    /// </summary>
    void TileUpdate()
    {
        _tileGrid.RefreshAllTiles();
        FieldManager.FieldState[,] field = FieldManager.Instance.Field();
        int sectionSize = FieldManager.Instance.SectionSize;
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {

                for (int k = 0; k < sectionSize; k++)
                {
                    for (int l = 0; l < sectionSize; l++)
                    {
                        Vector3Int pos = new Vector3Int(i, j, 0) * sectionSize + new Vector3Int(k, l, 0);
                        if (field[i, j].isPassable)
                        {
                            _tileGrid.SetTile(pos, _floortile);
                        }
                        else
                        {
                            _tileGrid.SetTile(pos, _walltile);
                        }
                    }

                }
            }
        }
    }
*/