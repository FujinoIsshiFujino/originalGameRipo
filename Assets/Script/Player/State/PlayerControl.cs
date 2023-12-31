using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class PlayerControl : MonoBehaviour
{

    [SerializeField] public float moveSpeed = 3;
    public CharacterController characterController;
    public Vector3 moveDirection;
    public GameObject Camera;
    public float initialMoveSpeed;

    // private Vector3 startPosition; // ジャンプ時の初期位置
    private float time;
    private float groundtime;
    public bool isGrounded;

    [SerializeField] float apex;
    [SerializeField] float apexTime;
    public double jumpPower;
    public float virtualGra;

    // 1人称視点
    CameraFollow _cameraFollow;

    public Vector3 cameraForward;


    public bool isDash;

    public float inputHorizontal;
    public float inputVertical;



    public int jumpCount;
    bool isDashJump;

    Animator _animator;

    public PlayerStatus _playerStatus;

    LockOn _lockOn;





    // Start is called before the first frame update
    public static readonly StateIdle stateIdle = new StateIdle();
    private static readonly StateJumping stateJumping = new StateJumping();
    private static readonly StateWalking stateWalking = new StateWalking();
    private static readonly StateAttacking stateAttacking = new StateAttacking();
    private static readonly StateMaking stateMaking = new StateMaking();
    private static readonly StateDead stateDead = new StateDead();

    public bool IsDead => currentState is StateDead;
    //isは型の比較　＝＝はメモリ位置（つまりガチで同じか）の比較

    private PlayerStateBase currentState = stateIdle;
    private void Start()
    {
        currentState.OnEnter(this, null);

        // 取得

        characterController = GetComponent<CharacterController>();
        initialMoveSpeed = moveSpeed;

        _cameraFollow = Camera.GetComponent<CameraFollow>();

        _animator = GetComponent<Animator>();
        // _status = GetComponent<PlayerStatus>();
        // _attack = GetComponent<MobAttack>();


        _lockOn = Camera.GetComponent<LockOn>();

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
    }

    private void Update()
    {

        _playerStatus = GetComponent<PlayerStatus>();

        if (_playerStatus.isDead)
        {
            ChangeState(stateDead);
        }


        //カメラ正面のベクトルのｘｚ成分を取得して、単位ベクトル化し、地面に平行なベクトルを取得
        cameraForward = Camera.transform.forward;
        cameraForward.y = 0;
        cameraForward = cameraForward.normalized;

        getInputMove();//各ステートで受け付けるべきかも。




        // isGrounded = isGroundDiscriminant();
        // isGrounded = characterController.isGrounded;

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


        freeFall();





        currentState.OnUpdate(this);
        // Debug.Log("attackCollider.enabled " + attackCollider.enabled);
        Debug.Log("currentState" + currentState);
        Debug.Log("isGrounded" + isGrounded);
        Debug.Log("mmmoveDirection" + moveDirection);

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

    private void getInputMove()
    {
        // // 入力を受け付ける
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
    }

    public bool isGroundDiscriminant()
    {
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

        return isGrounded;

    }

    public void freeFall()
    {
        time += Time.deltaTime;
        moveDirection = new Vector3(0, virtualGra * time, 0); // ジャンプではない自由落下? って過去の俺が書いてるけど、地面ついていないときこれがおこってるので、普通に自由落下では？
                                                              // ジャンプの時は方向が毎フレーム加算される　V=gt
                                                              //moveDirection.y = virtualGra * time;
        characterController.Move(moveDirection * Time.deltaTime);
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "ground")
        {
            isDashJump = false;
            time = 0;
            isGrounded = true;
            jumpCount = 0;

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

            if (currentState is StateJumping && isJump == true)
            {
                ChangeState(stateIdle);
            }



        }
    }
}
// //OnControllerColliderHitがアップデート後によばれてるのか？
