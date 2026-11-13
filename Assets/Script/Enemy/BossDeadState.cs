



using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDeadState : BossStateBase
{
    public override void OnEnter(
        BossController owner,
        BossStateBase previousState)
    {
        owner._animator.SetTrigger("Die");
    }

    public override void OnUpdate(BossController owner)
    {
        //死亡時にmakeオブジェの削除 デストロイの処理はモブ共通の処理で行われている。
        // Destroy(_cameraFollow.makeObj);
    }
}
