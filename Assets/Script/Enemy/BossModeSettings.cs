using UnityEngine;

[System.Serializable]
public class BossModeSettings
{
    [Min(0f)]
    public float moveSpeed = 1.5f;

    [Min(0f)]
    public float meleeRange = 2f;

    [Min(0f)]
    public float rangedRange = 8f;

    [Min(0f)]
    public float meleeAttackDuration = 1.2f;

    [Min(0f)]
    public float rangedAttackDuration = 1.5f;
}