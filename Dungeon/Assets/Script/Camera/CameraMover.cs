using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Camera cam;

    [NonSerialized] private GameObject _chaseObj;

    //Todo:PlayerManager作成してシングルトンからplayerを取得してくる
    void Start ()
	{
	    //_chaseObj = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    if (!_chaseObj)
	    {
	        _chaseObj = GameObject.FindWithTag("Player");
        }
	    else
	    {
            float y = _chaseObj.transform.position.y;
            //if (y > transform.position.y) y = transform.position.y;
	        transform.position = Vector3.Lerp(transform.position,
	            new Vector3(_chaseObj.transform.position.x,y,-10),
	            0.2f);
	    }
	}
}
