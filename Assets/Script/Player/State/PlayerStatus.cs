using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MobStatus
{
    public bool isDead;
    [SerializeField] float invincibleTIme = 3;
    [SerializeField] private Renderer[] childrenRenderer;
    [SerializeField] private float _cycle = 0.2f;    // 点滅周期[s]
    private double _time;

    protected override void Start()
    {
        base.Start();
        childrenRenderer = GetComponentsInChildren<Renderer>();
    }
    protected override void OnDie()
    {
        base.OnDie();
        isDead = true;
        StartCoroutine(GoToGameOverCoroutine());
    }

    private IEnumerator GoToGameOverCoroutine()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("GameOverScene");
    }

    //無敵時間
    protected override void invincible()
    {
        damageble = false;
        StartCoroutine(returnDamagebleState());
    }

    private IEnumerator returnDamagebleState()
    {
        yield return new WaitForSeconds(invincibleTIme);
        damageble = true;
        for (int i = 0; i < childrenRenderer.Length; i++)
        {
            childrenRenderer[i].enabled = true;
        }
    }

    private void Update()
    {        // 内部時刻を経過させる
        if (isDead == false)
        {
            if (damageble == false)
            {
                _time += Time.deltaTime;

                // 周期cycleで繰り返す値の取得
                // 0～cycleの範囲の値が得られる
                var repeatValue = Mathf.Repeat((float)_time, _cycle);

                // 内部時刻timeにおける明滅状態を反映
                for (int i = 0; i < childrenRenderer.Length; i++)
                {
                    childrenRenderer[i].enabled = repeatValue >= _cycle * 0.5f;
                }
            }
        }
    }
}

