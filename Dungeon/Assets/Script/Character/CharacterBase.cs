using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ターゲットを取り、移動するタイプ
/// </summary>

[RequireComponent(typeof(Collider2D))]
public abstract class ARPGCharacterBase<T> : MovableBase where T : struct
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


    //public (protected) functions
    //[SerializeField] private Collider2D _hitCollider;//攻撃との判定
    protected T CurrentBehaviourState
    {
        get
        {
            return _behaviourState;
        }
        set
        {
            if (!value.Equals(_behaviourState))
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



    public void CalcDirection()
    {
        if (Velocity.magnitude > 0.01f)
        {
            DirectionState d = _directionState;
            Vector2 v = Velocity;
            if ( Mathf.Abs(v.x) > Mathf.Abs(v.y))
            {
                if (v.x < 0)
                {
                    d = DirectionState.Left;
                }
                else
                {
                    d = DirectionState.Right;
                }
            }
            else
            {
                if (v.y > 0)
                {
                    d = DirectionState.Up;
                }
                else
                {
                    d = DirectionState.Down;
                }
            }
            CurrentDirectionState = d;
        }
    }
   

    protected virtual void OnTriggerStay2D(Collider2D c)
    {

    }

    

}
[RequireComponent(typeof(Rigidbody2D))]
public class MovableBase : MonoBehaviour
{
    protected GameObject _target;

    protected Vector2 _moveTo = new Vector2(0, 0);
    protected float _targetRange = 0.2f;             //近接判定 ターゲットからの半径距離 これ以上は近づかない

    protected Rigidbody2D _rigidbody;

    public bool IsNotKnockBack { get; set; }
    public Vector2 Power { get; set; }//勢い
    public Vector2 KnockBack { get; set; }

    public Vector2 Velocity { get; set; }
    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    protected void AimToTarget()
    {
        if (_target == null)
        {
            return;
        }
        _moveTo = new Vector2(_target.transform.position.x, _target.transform.position.y);
    }
    protected void MoveToAim(float speed)
    {

        Vector2 _distance = _moveTo - (Vector2)transform.position;
        if (_distance.magnitude < _targetRange)
        {
            Velocity = Vector2.zero;
            return;
        }
        Velocity = _distance.normalized * speed;
    }
    //ふっとび
    public void CalcKnockBack(MovableBase b)
    {
        KnockBack += b.Power;
    }
 
    //ぶつかっただけ
    public void CalcCollision(MovableBase b)
    {
        //KnockBack 
    }
    /// <summary>
    /// rigidbodyにVelocity流し込み
    /// </summary>
    public void CalcVelocity()
    {
        _rigidbody.velocity = Velocity + KnockBack;
    }

}