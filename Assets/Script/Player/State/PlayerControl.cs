using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class PlayerControl : MonoBehaviour
{
    public CharacterController characterController;
    public Vector3 moveDirection;
    public GameObject Camera;
    public float virtualGra;
    public float inputHorizontal;
    public float inputVertical;

    [SerializeField] public float moveSpeed = 3;
    [SerializeField] float apex;
    [SerializeField] float apexTime;


    double jumpPower;
    public float freeFallTime;
    CameraFollow _cameraFollow;
    Vector3 cameraForward;
    bool isDashJump;
    Animator _animator;
    PlayerStatus _playerStatus;
    LockOnCol _lockOnCol;
    [SerializeField] Collider lockOnCollider;


    //落下系
    [SerializeField] GameObject fadePanel;
    FadeController fadeController;
    float WaitTime = 2;
    public Vector3 lastGroundPosi;
    Rigidbody rb;
    public float checkDistance = 0.2f; // 地面との距離をチェックする閾値
    public bool isRayGrounded;
    public bool isGrounded;
    [SerializeField] int rayCount = 8; // 発射するRayの本数
    float radius;

    //メニュー系
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject recipeDialog;
    [SerializeField] MenuBase menuBase;

    // Start is called before the first frame update
    public static readonly StateIdle stateIdle = new StateIdle();
    private static readonly StateJumping stateJumping = new StateJumping();
    private static readonly StateWalking stateWalking = new StateWalking();
    private static readonly StateAttacking stateAttacking = new StateAttacking();
    private static readonly StateMaking stateMaking = new StateMaking();
    private static readonly StateRolling stateRolling = new StateRolling();
    private static readonly StateDead stateDead = new StateDead();

    public bool IsDead => currentState is StateDead;
    //isは型の比較　＝＝はメモリ位置（つまりガチで同じか）の比較

    private PlayerStateBase currentState = stateIdle;
    private void Start()
    {
        currentState.OnEnter(this, null);

        // 取得

        characterController = GetComponent<CharacterController>();


        _cameraFollow = Camera.GetComponent<CameraFollow>();

        _animator = GetComponent<Animator>();
        // _status = GetComponent<PlayerStatus>();
        // _attack = GetComponent<MobAttack>();


        _lockOnCol = lockOnCollider.GetComponent<LockOnCol>();

        //jumpPower = apex/apexTime + 0.5*9.81*apexTime;
        //jumpPower = jumpPower*apexTime/(jumpPower/9.81);
        // virtualGra = (float)(jumpPower/apexTime)*-1;


        // 任意の高さとその高さに達するまでの任意の秒数をインスペクター上から入力し、それによって加速度（ここでは重力加速度9.81とは違い変わりうる変数）と初速度を求める
        // 考えたとしては、まず鉛直上投げであっても、自由落下であっても下向きに重力が働いている。鉛直上の場合はそこに、上向きの初速がはっせいする。
        //なのでプログラム上では、地面についていないときは全部自由落下を組み込み、ジャンプするときだけ初速を上向きに加える。

        // プレイヤー用の重力加速度の計算　h = 1/2at^2 を変形　　y=1/2gt^2ともいう。　自由落下公式
        // また、鉛直上投げの場合、上に上がる時間と落下する時間は同じなので、apexTimeは本来頂点までの到達時間だが、自由落下式に使える
        virtualGra = 2 * apex / Mathf.Pow(apexTime, 2) * -1;

        // 初速度の計算　vo = at　←過去の俺がvo = atと書いているが、これは違う気がする。

        //上のはあっているが、厳密にはv=vo+atで頂点での速度は０になるはずなので、vo = atということがいいたかったのだと思う
        jumpPower = virtualGra * apexTime * -1;




        attackCollider.enabled = false;

        fadeController = fadePanel.GetComponent<FadeController>();
        rb = GetComponent<Rigidbody>();

        radius = characterController.radius + 0.1f;

        isJumpRayGrounded = true;
    }

    private void Update()
    {

        _playerStatus = GetComponent<PlayerStatus>();

        if (_playerStatus.isDead)
        {
            ChangeState(stateDead);
        }

        //メニュー画面
        if (Input.GetButtonDown("Start"))
        {
            // 同階層のunity上の他のメニューまで開いてしまうのでsetActiveでfalseにする
            recipeDialog.SetActive(false);
            menuBase.OpenMenu(mainMenuPanel);
        }



        //カメラ正面のベクトルのｘｚ成分を取得して、単位ベクトル化し、地面に平行なベクトルを取得
        cameraForward = Camera.transform.forward;
        cameraForward.y = 0;
        cameraForward = cameraForward.normalized;

        getInputMove(true);//各ステートで受け付けるべきかも。




        // レイが地面に接しているかをチェック
        isRayGrounded = CheckGroundedByRays();


        if (characterController.isGrounded)
        {
            //_characterController.isGroundedの精度が悪いため（フレーム毎に接地判定されたりされなかったりする。）
            //時間によって接地判定。0.1秒以上接地がなかったとすると空中判定となる
            groundtime = 0.0f;
            isGrounded = true;
        }
        else
        {
            groundtime += Time.deltaTime;
            if (groundtime >= 0.3f)
            { isGrounded = false; }
            else
            { isGrounded = true; }
        }

        if (isGrounded)
        {
            //めいくもここにはいる
            if (currentState is StateWalking || currentState is StateIdle)
            {
                // //1人称視点時
                if (_cameraFollow.isFirstPerson)
                {
                    if (_cameraFollow.isCameraMoveEnd)// １人称視点の時のカメラは常にプレイヤーの前にあるので、カメラを動かすのではなく、プレイヤーの向きを変える。
                    {
                        transform.forward = cameraForward;
                    }
                }
            }
        }
        // else
        // {
        //  freeFall();をステイト内の実行がされるまにしておくことで、接地を担保する
        // }
        _recipe = RecipieMenue.GetComponent<Recipe>();

        DetermaineLastPosi();

        SlopePush();

        freeFall();

        currentState.OnUpdate(this);
        // Debug.Log("attackCollider.enabled " + attackCollider.enabled);
        // Debug.Log("currentState" + currentState);
        // Debug.Log("mmmoveDirection" + moveDirection);

    }

    public void ChangeState(PlayerStateBase nextState)
    {
        currentState.OnExit(this, nextState);
        nextState.OnEnter(this, currentState);
        currentState = nextState;
    }

    // private void OnDeath()
    // {
    //     ChangeState(stateDead);
    // }

    // // 入力を受け付ける
    public void getInputMove(bool isGetInput)
    {
        if (isGetInput)
        {
            inputHorizontal = Input.GetAxis("Horizontal");
            inputVertical = Input.GetAxis("Vertical");
        }
        else
        {
            inputHorizontal = 0;
            inputVertical = 0;
        }
    }

    public void freeFall()
    {
        freeFallTime += Time.deltaTime;
        moveDirection = new Vector3(0, virtualGra * freeFallTime, 0);
        // ジャンプではない自由落下? って過去の俺が書いてるけど、地面ついていないときこれがおこってるので、普通に自由落下では？
        // ジャンプの時は方向が毎フレーム加算される　V=gt
        //moveDirection.y = virtualGra * time;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    // キャラクター直下にレイを発射すると、characterControllerは地面に接地しているのに、rayは接地していないという現象が起こるので、
    // characterControllerの円周上から下方向にrayを発射して、地面の接地を一致させている。
    // また、円周を少し大きくすることで、characterControllerの接地が確実になくなった後にisRayGroundedがfalseになるようにしている
    // characterController.isGroundedは下方向以外にも接地判定があってしまうので、例えば空中で壁などにキャラクターが当たっても
    // 接地判定されてしまうので、下方向にrayを打つ必要があったが、上記の理由によりcharacterController.radiusの円周上に配置
    private bool CheckGroundedByRays()
    {
        for (int i = 0; i < rayCount; i++)
        {
            // 円周上の角度を計算
            float angle = 360f / rayCount * i;
            // 円周上の座標を計算
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));
            Vector3 rayOrigin = transform.position + direction * radius;

            // RaycastHitを作成
            RaycastHit hit;

            // Rayを発射
            if (Physics.Raycast(rayOrigin, Vector3.down, out hit, checkDistance))
            {
                Debug.Log($"Ray {i}: Hit Object Name = {hit.collider.gameObject.name}");

                // ヒットしたRayを緑に描画
                Debug.DrawRay(rayOrigin, Vector3.down * checkDistance, Color.magenta);

                // 1つでもヒットしたら接地を判定して終了
                return true;
            }
            else
            {
                // ヒットしなかったRayを青に描画
                Debug.DrawRay(rayOrigin, Vector3.down * checkDistance, Color.blue);
            }
        }

        // すべてのRayがヒットしなかった場合
        return false;
    }

    //接地中ずっと呼ばれている
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "ground")
        {
            isDashJump = false;
            jumpCount = 0;

            if (isRayGrounded || isJumpRayGrounded)
            {
                freeFallTime = 0;
            }
            // moveDirection.y = 0;
            // if (currentState is StateIdle || currentState is StateWalking || currentState is StateJumping)
            // {
            //     ChangeState(stateIdle);
            // }
            // characterController.Move(moveDirection * Time.deltaTime);

            //この記述をなくすか、条件を付けるしかない
            // if (currentState is StateIdle || currentState is StateWalking || currentState is StateJumping)
            // {
            //     ChangeState(stateIdle);
            // }

            if (isLastGroundPosiForDown && isLastGroundPosiForSide)
            {
                lastGroundPosi = transform.position;
            }

            if (currentState is StateJumping && isJumpRayGrounded == true)
            {
                groundtime = 0.0f;

                if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
                {
                    _animator.SetFloat("Speed", 0);
                    ChangeState(stateIdle);
                }
                else
                {
                    _animator.SetFloat("Speed", moveDirection.magnitude);
                    ChangeState(stateWalking);
                }
            }

            Vector3 zeroRbVerocityX = rb.velocity;
            zeroRbVerocityX.x = 0;
            zeroRbVerocityX.z = 0;
            rb.velocity = zeroRbVerocityX;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hole")
        {
            fadeController.isFadeOut = true;//フェードアウト

            //同じフレームでフェードのOut/Inを行うと止まるのでコルーチンで時間をずらす。
            StartCoroutine(WarpFadeIn());
            //フェードアウトする前に座標移動しないようにコルーチン
            StartCoroutine(GetRespawnObjectPositionCoroutine());
        }
    }

    private IEnumerator GetRespawnObjectPositionCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        characterController.enabled = false;
        transform.position = lastGroundPosi;

        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; // プレイヤーの位置から少し上にレイを飛ばす

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayLength))
        {
            if (hit.collider.tag == "ground")
            {
                characterController.enabled = true;
            }
        }
    }

    private IEnumerator WarpFadeIn()
    {
        yield return new WaitForSeconds(WaitTime);
        fadeController.isFadeIn = true;//フェードイン
        characterController.enabled = true;
    }
}
