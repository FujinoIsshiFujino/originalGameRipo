public abstract class BossStateBase
{
    public virtual void OnEnter(
        BossController owner,
        BossStateBase previousState)
    {
    }

    public virtual void OnUpdate(BossController owner)
    {
    }

    public virtual void OnExit(
        BossController owner,
        BossStateBase nextState)
    {
    }
}