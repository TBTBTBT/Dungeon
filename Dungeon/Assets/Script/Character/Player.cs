using System;
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
    [Header("スプライト切り替え")]
    [SerializeField] private List<MultipleSpriteAnimator> _spriteAnimator;

    [SerializeField] private Animator _animator;
    private ARPGCharactorAnimator<BehaviourState> _animation = new ARPGCharactorAnimator<BehaviourState>();

    //パラメーター
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

    private string[] SpritePath =
    {
        "Image/Player/Head/img_human_hair_{0:00}",
        "Image/Player/Face/img_human_face_{0:00}",
        "Image/Player/Body/img_human_body_{0:00}",
        "Image/Player/Leg/img_human_leg_{0:00}",
    };
    void Start()
    {
        
        InitParam();
        InitSprite();
        InitAnimation();
        TouchManager.Instance?.OnTouchBegin?.AddListener(TouchBegin);
        TouchManager.Instance?.OnTouchMove?.AddListener(TouchBegin);
    }
    
    void InitAnimation()
    {
        _animation.Init(_animator,this);
    }
    void InitSprite()
    {
        for (var i = 0; i < SpritePath.Length; i++)
        {
            if (_spriteAnimator.CheckIndex(i))
            {
                //Debug.Log(String.Format(SpritePath[i], 0));
                _spriteAnimator[i].Init("",String.Format(SpritePath[i],1));
            }
        }

    }
    void InitParam(){
        _Speed = 1;
    }
	protected void Update()
	{
        AimToTouch();
        MoveToAim(1.5f + (float)_Speed / 10f);
	    CalcDirection();
	    CalcState();//ステートを更新@アニメーションも
        CalcVelocity();//rigidbodyに流しこみ
	}
    void AimToTouch(){
        _moveTo = _touchPos;
    }
	void TouchBegin(int i){
        if(i == 0){
            _touchPos = TouchManager.Instance.GetTouchWorldPos(i);
        }
    }

    void CalcState()
    {
        BehaviourState b = CurrentBehaviourState;
        if (_rigidbody.velocity.magnitude > 0.01f)
        {
            b = BehaviourState.Walk;
        }
        else
        {
            b = BehaviourState.Wait;
        }
        CurrentBehaviourState = b;
    }
    protected override void OnTriggerStay2D(Collider2D c)
    {
        if (c.tag == "Enemy")
        {
            //if(c.GetComponent<>())
        }
    }
}
