using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;


public partial class PlayerControl
{
    public class StateDead : PlayerStateBase
    {
        public override void OnEnter(PlayerControl owner, PlayerStateBase preState)
        {
            owner._lockOn.isLockOn = false;
        }

        public override void OnUpdate(PlayerControl owner)
        {
            owner.enabled = false;
        }
    }

}
