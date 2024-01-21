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


    [SerializeField] private float lifeMax = 10;//ライフ最大値
    protected Animator _animator;
    protected StateEnum _state = StateEnum.Normal;//ステイトの初期化
    private float _life;//現在のライフ



    //StateEnum.Normal == _state という式を評価して、その結果を返す、ラムダ式の変数初期化
    public bool IsMovable => StateEnum.Normal == _state;
    public bool IsAttackable => StateEnum.Normal == _state;
    public float LifeMax => lifeMax;
    // public float LifeMax
    // {
    //     get
    //     {
    //         return lifeMax;
    //     }
    // }
    //同じ意味

    //float currentLife = mobStatus.Life; // mobStatus は MobStatus クラスのインスタンスのような感じで、
    //Damage メソッドで _life の値が変更された場合、Life プロパティを介して _life の現在の値にアクセスすることができます。
    // プロパティの主な目的は、フィールドに対するアクセスを制御し、外部コードが直接フィールドにアクセスすることなく、安全かつ制御された方法でデータにアクセスできるようにすることです。
    //_life自体はプライべべーとだが、それをgetしているLifeはパブリックで外部からもアクセス可能ということ
    public float Life => _life;
    public bool damageble;
    protected virtual void Start()
    {
        _life = lifeMax;
        _animator = GetComponentInChildren<Animator>();
        damageble = true;
    }

    protected virtual void OnDie()
    {

    }

    public virtual void Damage(int damage)
    {
        if (damageble)
        {
            Debug.Log("damageble" + damageble);
            if (_state == StateEnum.Die) return;

            _life -= damage;
            _animator.SetTrigger("Damage");
            invincible();
            if (_life > 0) return;

            _state = StateEnum.Die;
            _animator.SetTrigger("Die");
            OnDie();
        }

    }

    public void GoToAttackStateIfPossible()
    {
        if (!IsAttackable) return;

        _state = StateEnum.Attack;
        _animator.SetTrigger("Attack");
    }

    public void GoToNormalStateIfPossible()
    {
        if (_state == StateEnum.Die) return;
        _state = StateEnum.Normal;
    }

    protected virtual void invincible()
    {
        //各継承先にて記述
    }
}
