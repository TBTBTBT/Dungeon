using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ターゲットを取り、移動するタイプ
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract class ARPGCharacterBase<T> : MonoBehaviour where T : struct
{
    public enum DirectionState
    {
        Left,
        Up,
        Right,
        Down
    }
    //ステートの種類は２つ
    //向き と 状態

    protected T                     _behaviourState;                            //動きのステート 
    protected DirectionState        _directionState         = 0;                //現在の向きステート 0 左 1 上 2 右 3 下
    protected GameObject            _target;
    Rigidbody2D                     _rigidbody;
    protected Vector2               _moveTo                 = new Vector2(0, 0);
    protected float                 _targetRange            = 0.2f;             //近接判定 ターゲットからの半径距離 これ以上は近づかない

    public    Vector2               KnockBack { get; set; } 
    //public (protected) functions

    protected T CurrentBehaviourState
    {
        get
        {
            return _behaviourState;
        }
        set
        {
            if (value.Equals(_behaviourState))
            {
                _behaviourState = value;
                OnChangeBehaviourState.Invoke(value);
            }
        }
    }
    protected DirectionState CurrentDirectionState
    {
        get
        {
            return _directionState;
        }
        set
        {
            if (value != _directionState && value >= 0)
            {
                _directionState = value;
                OnChangeDirectionState.Invoke(value);
            }
        }
    }

    //events

    public class DirectionEvent : UnityEvent<DirectionState>{}//向きステートが変更されたときに発動するイベント
    public class BehaviourEvent : UnityEvent<T>  {}
    [NonSerialized]
    public DirectionEvent OnChangeDirectionState = new DirectionEvent();
    [NonSerialized]
    public BehaviourEvent OnChangeBehaviourState = new BehaviourEvent();
    protected void AimToTarget()
    {
        if (_target == null)
        {
            return;
        }
        _moveTo = new Vector2(_target.transform.position.x,_target.transform.position.y);
    }
    protected void MoveToAim(float speed){
       
        Vector2 _distance = _moveTo - (Vector2)transform.position;
        if (_distance.magnitude < _targetRange)
        {
            _rigidbody.velocity = Vector2.zero;
            return;
        }
        _rigidbody.velocity = _distance.normalized * speed;
    }
    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
}
