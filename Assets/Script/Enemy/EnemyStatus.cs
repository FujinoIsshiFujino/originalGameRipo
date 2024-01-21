using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyStatus : MobStatus
{
    [SerializeField] GameObject HPPanel;
    [SerializeField] float HPverActiveDistance = 20;
    [SerializeField] float ForClosestCameraHPverActiveDistance = 3;
    private NavMeshAgent _agent;
    float HP;
    public Slider healthBar;

    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        healthBar.maxValue = LifeMax;
        healthBar.value = LifeMax;
    }

    private void Update()
    {
        _animator.SetFloat("MoveSpeed", _agent.velocity.magnitude);
        //Hpの表示をカメラとの距離によって切り替える。
        if (Vector3.Distance(Camera.main.transform.position, transform.position) <= HPverActiveDistance)
        {
            HPPanel.SetActive(true);
        }
        else
        {
            HPPanel.SetActive(false);
        }

        //Hpの表示をカメラが近すぎる場合に非表示にする
        if (Vector3.Distance(Camera.main.transform.position, transform.position) <= ForClosestCameraHPverActiveDistance)
        {
            HPPanel.SetActive(false);
        }
    }

    public override void Damage(int damage)
    {
        base.Damage(damage);
        healthBar.value = Life;
    }

    //無敵時間 今のところ実装予定なし
    // protected override void invincible()
    // {
    //     damageble = false;
    //     StartCoroutine(returnDamagebleState());
    // }

    // private IEnumerator returnDamagebleState()
    // {
    //     yield return new WaitForSeconds(invincibleTIme);
    //     damageble = true;
    //     for (int i = 0; i < childrenRenderer.Length; i++)
    //     {
    //         childrenRenderer[i].enabled = true;
    //     }
    // }

    protected override void OnDie()
    {
        base.OnDie();
        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
