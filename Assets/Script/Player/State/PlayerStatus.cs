using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MobStatus
{
    public bool isDead;
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
}
