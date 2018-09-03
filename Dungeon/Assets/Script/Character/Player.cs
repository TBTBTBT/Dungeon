using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : ARPGCharacterBase<Player.BehaviourState>
{

    /// <summary>
    /// ステート
    /// </summary>
    public enum BehaviourState
    {
        Wait,
        Walk,
        Attack01,
        Attack02,
        Damage,
        Dead
    }

    private float _moveSpeed = 2.5f;

    float _attackRange = 1;


    private int damageTime = 0;
    private int attackTime = 0;

    Vector2 _touchPos { get; set; }

    public int _MaxHp { get; set; }
    public int _Hp { get; set; }
    public int _Attack { get; set; }
    public int _Speed { get; set; }
    public int _Deffence { get; set; }
    public int _Magic { get; set; }
    public int _HpRegen { get; set; }

    int _coin = 0;

    void Start()
    {
        InitParam();
        TouchManager.Instance?.OnTouchBegin?.AddListener(TouchBegin);
        TouchManager.Instance?.OnTouchMove?.AddListener(TouchBegin);
    }
    void InitParam(){
        _Speed = 1;
    }
	private void Update()
	{
        AimToTouch();
        MoveToAim(1.5f + (float)_Speed / 10f);
	}
    void AimToTouch(){
        _moveTo = _touchPos;
    }
	void TouchBegin(int i){
        if(i == 0){
            _touchPos = TouchManager.Instance.GetTouchWorldPos(i);
        }
    }

}
