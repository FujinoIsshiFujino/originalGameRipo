using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using TMPro;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStatus))]
public class BossController : MonoBehaviour
{
    public enum BossMode
    {
        Normal,
        Rage
    }
    public Animator _animator;

    [Header("Mode Settings")]
    [SerializeField] private BossModeSettings normalSettings;
    [SerializeField] private BossModeSettings rageSettings;

    [SerializeField] private BossMode currentMode = BossMode.Normal;

    [Header("Detection")]
    [SerializeField] private LayerMask raycastLayerMask;

    private NavMeshAgent _agent;
    private EnemyStatus _status;
    private Transform _target;
    private BossStateBase _currentState;

    public BossMode Mode => currentMode;
    public Transform Target => _target;
    public NavMeshAgent Agent => _agent;
    public EnemyStatus Status => _status;

    public BossModeSettings Settings =>
        currentMode == BossMode.Rage
            ? rageSettings
            : normalSettings;

    public float DistanceToTarget =>
        _target == null
            ? float.PositiveInfinity
            : Vector3.Distance(transform.position, _target.position);
    // public Collider AttackRangeDetecter;
    public SphereCollider AttackRangeDetecter;
    public Collider BasicAttackCollider;
    public Collider FlameAttackCollider;

    public SphereCollider CollisionDetecterBossLost;
    [SerializeField] float attackCoolDown = 0.5f;
    public bool attackFinished;//　クールダウンと攻撃判定可能か用のbool値

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _status = GetComponent<EnemyStatus>();
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();

        ApplyModeSettings();
        ChangeState(new BossIdleState());

        attackFinished = true;
    }

    public void ReturnToIdle()
    {
        ChangeState(new BossIdleState());
    }

    private void Update()
    {
        Debug.Log("_currentState" + _currentState + " : " + _target);

        //死亡
        if (_status.Life <= 0)
        {
            StopMovement();
            ChangeState(new BossDeadState());
            return;
        }

        //　怒りモード
        if (_status.Life <= _status.Life % 2)
        {
            EnterRageMode();
            _animator.SetTrigger("Scream");
            return;
        }

        if (_currentState != null)
        {
            _currentState.OnUpdate(this);
        }
    }

    // CollisionDetector から呼び出す
    public void OnDetectObject(Collider collider)
    {
        if (collider == null)
            return;

        if (!collider.CompareTag("Player"))
        {

            return;
        }

        if (_status.Life <= 0)
            return;

        if (!CanUseAgent())
            return;

        Vector3 diff = collider.transform.position - transform.position;
        float distance = diff.magnitude;

        // ボスとプレイヤーの距離がほぼゼロの場合に、Raycast（障害物判定）を行わずにターゲットだけ設定して処理を終了する
        if (distance <= 0.001f)
        {
            _target = collider.transform;
            return;
        }

        Vector3 direction = diff.normalized;

        // プレイヤーまでの間に障害物があるか確認
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distance, raycastLayerMask, QueryTriggerInteraction.Ignore))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                StopMovement();
                ReturnToIdle();
                return;
            }
        }

        _target = collider.transform;

        ChangeState(new BossChaseState());
    }

    //　コリジョンから呼ばれる
    public void OnLostTarget()
    {
        if (_target == null)
        { return; }
        _target = null;

        //　これがないとナブメッシュの慣性が残り続けて滑る
        StopMovement();

        ReturnToIdle();
    }

    // 例外処理
    public bool CanUseAgent()
    {
        if (_agent == null || !_agent.isActiveAndEnabled)
            return false;

        if (!_agent.isOnNavMesh)
        {
            Debug.LogWarning(
                $"BossがNavMesh上にいません: {transform.position}",
                this);
            return false;
        }

        return true;
    }

    // ターゲットを追跡
    public void MoveToTarget()
    {
        if (_target == null || !CanUseAgent())
            return;

        _agent.speed = Settings.moveSpeed;
        _agent.isStopped = false;
        _agent.SetDestination(_target.position);
    }

    public void StopMovement()
    {
        if (_agent == null || !_agent.isActiveAndEnabled)
            return;

        if (_agent.isOnNavMesh)
        {
            _agent.isStopped = true;
        }
    }

    public void ChangeState(BossStateBase nextState)
    {
        if (nextState == null)
            return;

        BossStateBase previousState = _currentState;

        if (previousState != null)
        {
            previousState.OnExit(this, nextState);
        }

        _currentState = nextState;
        _currentState.OnEnter(this, previousState);
    }

    // 怒りモードに移行したいタイミングで呼ぶ
    public void EnterRageMode()
    {
        SetMode(BossMode.Rage);
    }

    public void SetMode(BossMode mode)
    {
        if (currentMode == mode)
            return;

        currentMode = mode;
        ApplyModeSettings();
    }

    private void ApplyModeSettings()
    {
        if (_agent != null)
        {
            _agent.speed = Settings.moveSpeed;
        }
    }

    private void OnDisable()
    {
        if (_currentState != null)
        {
            _currentState.OnExit(this, null);
            _currentState = null;
        }
    }

    // 攻撃アニメーションから呼ばれる
    public void AttackCollisionAppear()
    {
        BasicAttackCollider.enabled = true;
    }

    // 攻撃コリジョン突入時に呼ばれる
    public void OnAttackStart()
    {
        if (_target != null && attackFinished == true)
        {
            ChangeState(new BossAttackState());
        }
    }
    // 攻撃アニメーションから呼ばれる
    public void OnAttackFinished()
    {
        BasicAttackCollider.enabled = false;
        ChangeState(new BossIdleState());

        StartCoroutine(CoolDownCoroutine());
    }

    public IEnumerator CoolDownCoroutine()
    {
        yield return new WaitForSeconds(attackCoolDown);
        attackFinished = true;
    }
}