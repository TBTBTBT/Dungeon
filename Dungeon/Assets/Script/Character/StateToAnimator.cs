using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
[Serializable]
public class ARPGCharactorAnimator<T> where T : struct
{
    [Header("ARPGCharactorAnimator")]
    private Animator _animator;

    public void Init(Animator a,ARPGCharacterBase<T> _chara)
    {
        _animator = a;
        _chara.OnChangeBehaviourState.AddListener(_ =>
        {
            Debug.Log(_);
            _animator.SetTrigger("Wait");//_.ToString());
        });
        _chara.OnChangeDirectionState.AddListener(_ =>
        {
            Debug.Log(_);
            _animator.SetFloat("Direction",(int)_);
        });
    }

}
/*
[Serializable]
public class StateToAnimator<T>  where T : struct
{
    private T _current;
    public T Current => _current ;
    [Header("StateToAnimator")]
    [SerializeField] private Animator _animator;
    public void Init()
    {

    }

    public void Next(T state)
    {
        if (_current.ToString() == state.ToString())
        {
           return;
        }
        _current = state;
        _animator.SetTrigger(state.ToString());
    }
}
[Serializable]
public class DirectionToAnimator
{
    [Header("DirectionToAnimator")]
    [SerializeField] private Animator _animator;

    private int _direction = 0;
    public int Direction
    {
        get { return _direction; }
        set
        {
            Next(value);
        }
    }

    void Next(int next)
    {
        if (next == _direction)
        {
            return;
        }

        _direction = next;
        _animator.SetInteger("Direction",next);
    }
    public void Init()
    {

    }
}*/