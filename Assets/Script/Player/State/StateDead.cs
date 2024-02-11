using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;


public partial class PlayerControl
{
    public class StateDead : PlayerStateBase
    {
        CameraFollow _cameraFollow;
        public override void OnEnter(PlayerControl owner, PlayerStateBase preState)
        {
            owner._lockOnCol.isLockOn = false;
        }

        public override void OnUpdate(PlayerControl owner)
        {
            owner.enabled = false;

            //死亡時にmakeオブジェの削除
            _cameraFollow = owner.Camera.GetComponent<CameraFollow>();
            Destroy(_cameraFollow.makeObj);
        }
    }

}
