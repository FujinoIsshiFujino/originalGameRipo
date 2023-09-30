using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobStatus : MonoBehaviour
{
    protected enum StateEnum
    {
        Normal,
        Attack,
        Die
    }


    // [SerializeField] private float lifeMax = 10;//ライフ最大値
    // protected Animator _animator;
    // protected StateEnum _state = StateEnum.Normal;//ステイトの初期化
    // private float _life;//現在のライフ



    // //StateEnum.Normal == _state という式を評価して、その結果を返す、ラムダ式の変数初期化
    // public bool IsMovable => StateEnum.Normal == _state;
    // public bool IsAttackable => StateEnum.Normal == _state;
    // public float lifeMax => LifeMax;

    // public float Life => _life;
    // protected virtual void Start()
    // {
    //     _life = lifeMax;
    //     _animator = GetComponentInChildren<Animator>();

    // }

}
