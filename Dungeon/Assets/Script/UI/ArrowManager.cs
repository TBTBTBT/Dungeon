using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : SingletonMonoBehaviour<ArrowManager> {
    public enum TargetState{
        None,
        Attackable,
        Talkable,
    }
    Vector2 _position {get; set;}
    [SerializeField]Arrow _arrow;
	// Use this for initialization
	void Start () {
        _position = new Vector2(0, 0);
        TouchManager.Instance?.OnTouchBegin?.AddListener(TouchBegin);
        TouchManager.Instance?.OnTouchMove?.AddListener(TouchBegin);
	}
    void TouchBegin(int i){
        if(i == 0){
            Vector2 pos = TouchManager.Instance.GetTouchWorldPos(i);
            _arrow.transform.position = pos;

                  
        }
    }
}
