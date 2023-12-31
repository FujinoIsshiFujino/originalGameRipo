using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerStateBase
{
    public virtual void OnEnter(PlayerControl owner, PlayerStateBase preState) { }
    public virtual void OnUpdate(PlayerControl owner) { }
    public virtual void OnExit(PlayerControl owner, PlayerStateBase nextState) { }
}
