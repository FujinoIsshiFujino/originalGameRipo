using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    [SerializeField] public float moveSpeed = 3;
    private CharacterController characterController;
    public Vector3 moveDirection;
    [SerializeField] GameObject Camera;
    private Vector3 cameraForward;
    public float inputHorizontal;
    public float inputVertical;
    public bool isDashJump;
    public float initialMoveSpeed;

    // jump
    [SerializeField] private float jumpForce;
    // private Vector3 startPosition; // ジャンプ時の初期位置
    private float time;
    public bool isJump;
    public int jumpCount;
    private float groundtime;
    public bool isGrounded;
    [SerializeField] float jumpAdjust;

    public float beforeJumpInputHorizontal;
    public float beforeJumpInputVertical;
    public Vector3 jumpDirection;
    [SerializeField] private float jumpFoarwardPower;
    [SerializeField]private bool isDash;

        [SerializeField] float apex;
    [SerializeField]float apexTime;
    public double jumpPower;
    public float virtualGra;

    public bool isGlide;
    float glideTime;

    // 1人称視点
    CameraFollow _cameraFollow;
    public float horizontalAngle;
    [SerializeField] float rotateSpeed = 0.4f;



    private void Start()
    {
        // 取得
        
        characterController = GetComponent<CharacterController>();
        initialMoveSpeed = moveSpeed;

        _cameraFollow = Camera.GetComponent<CameraFollow>();

       
        //jumpPower = apex/apexTime + 0.5*9.81*apexTime;
        //jumpPower = jumpPower*apexTime/(jumpPower/9.81);
        // virtualGra = (float)(jumpPower/apexTime)*-1;


        // 任意の高さとその高さに達するまでの任意の秒数をインスペクター上から入力し、それによって加速度（ここでは重力加速度9.81とは違い変わりうる変数）と初速度を求める
        // 重力の計算　h = 1/2at^2 を変形　（等加速運動の公式）
        virtualGra= 2*apex / Mathf.Pow(apexTime, 2)*-1;

        // 初速度の計算　vo = at
        jumpPower = virtualGra * apexTime*-1;
        

    }


    private void Update()
    {


           // moveDirection =  new Vector3(0f, virtualGra * time * jumpAdjust * Time.deltaTime,0f);
            // timeは地面から離れたらスタートする、なので実際は空中にいるときに重力がかかる でもこれもおかしい気がする

         //characterController.Move(moveDirection);

        //カメラ正面のベクトルのｘｚ成分を取得して、単位ベクトル化し、地面に平行なベクトルを取得
        cameraForward = Camera.transform.forward;
        cameraForward.y = 0;
        cameraForward = cameraForward.normalized;

        // 入力を受け付ける
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");



                    if(_cameraFollow.cameraMove && _cameraFollow.isCameraMoveEnd==false)
            {
                inputHorizontal = 0;
                inputVertical = 0;
            }


        //方向の入力に応じて動く方向を決める
        moveDirection = cameraForward * inputVertical + Camera.transform.right * inputHorizontal  ;

        // 斜め移動時はベクトルの長さで成分を割って、単位ベクトル化
        if(moveDirection.magnitude >=1)
        {
            moveDirection/=moveDirection.magnitude ;
        }


    






        if (characterController.isGrounded)
        {
            //_characterController.isGroundedの精度が悪いため（フレーム毎に接地判定されたりされなかったりする。）
            //時間によって接地判定。0.1秒以上接地がなかったとすると空中判定となる
            groundtime = 0.0f;
            isGrounded = true;
            //Debug.Log(999999);

        }
        else
        {
            groundtime += Time.deltaTime;
            if (groundtime >= 0.3f)
            { isGrounded = false; }
            else
            {isGrounded = true;}
        }


        if (isGrounded)
        {




            //ダッシュ処理
            if (Input.GetKey("4") || Input.GetButton("Dash"))
            {

                isDash=true;
                moveDirection.x *= 2;
                moveDirection.z *= 2;
                // animator.SetTrigger("Run");

                if ( Input.GetButtonDown("Jump") && jumpCount < 1)
                {                 

                    isJump = true;
                    isDashJump = true;
                    jumpCount++;

                    // ジャンプ時向きを取得


                    jumpDirection = transform.forward.normalized;
                    moveDirection = new Vector3(0,0,0);


                }
            }
            else //ダッシュしてないときの処理
            {
                    isDash=false;

                if ( Input.GetButtonDown("Jump") && jumpCount < 1)
                {
                    
                    isJump = true;
                    // startPosition = transform.position;    // 2段 防がなければ
                    jumpCount++;
                    
                    // ジャンプ時の方向入力の大きさ、向きを取得、大きさはスティックの段階判定に使用

                    beforeJumpInputHorizontal=inputHorizontal;
                    beforeJumpInputVertical=inputVertical;

                    jumpDirection = transform.forward.normalized;
                    jumpDirection.y=0;                  
                    

                }
            }

            



            if(!_cameraFollow.isFirstPerson) //1人称視点の際に、カニ歩きになるように、isFirstPersonを監視
            {
            //動く方向を向く transform.LookAtは引数に指定した位置をむく
            transform.LookAt(transform.position + new Vector3(moveDirection.x,0,moveDirection.z));
            }
            
            if(_cameraFollow.isCameraMoveEnd)// １人称視点の時のカメラは常にプレイヤーの前にあるので、カメラを動かすのではなく、プレイヤーの向きを変える。
            {
                horizontalAngle = Input.GetAxis("HorizontalCamera") * rotateSpeed;
                //transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + horizontalAngle, 0);
                transform.Rotate(transform.up,horizontalAngle);

            }
            

            characterController.Move(moveDirection * Time.deltaTime * moveSpeed );

                    
        }
        else
        {

            // transformを使った落下式。　貫通していしまう
            // Vector3 currentVelocity = new Vector3(0, Physics.gravity.y*Time.deltaTime, 0);
            // transform.position += currentVelocity;

            time += Time.deltaTime;

             moveDirection=new Vector3(0,virtualGra * time,0) ; // ジャンプではない自由落下　ジャンプの時は方向が毎フレーム加算される
            //moveDirection.y = virtualGra * time;
            characterController.Move(moveDirection * Time.deltaTime );



            // 自由落下 結構いい感じ。でも自由落下の計算おれ認識が間違っている気がする
            //moveDirection =  new Vector3(0f, 0.5f * Physics.gravity.y * time * time * jumpAdjust, 0f);

            //自由落下　速度なら こっちも結構いい感じ

        }

        //　自由落下時 
        // 多分接地判定ががばいから、端っこに行くと重力がめちゃかかる
        // if(isGrounded==false&& isJump==false)
        // {
            
        //     moveDirection.y += Physics.gravity.y;// 1フレーム当たりの重力加速度を加える 最終的にMOVE関数でデルタタイムを
        //     moveSpeed = 1;

        // }
        // else
        // {
        //     moveSpeed = initialMoveSpeed;

        // }


        // ジャンプ処理
        if (isJump)
        {

            moveDirection.x = 0;
            moveDirection.z = 0 ;



            // time += Time.deltaTime;
            // これはあとで直すかも



                //  後で下にレイをうってある程度の高さだったらトゥルーを返す用にしておく

            // if ( Input.GetButtonDown("Jump") && jumpCount == 1 && time >= 0.2) // timeは同じフレーム内で処理されてしまうので。
            // {
            //     isGlide = true;
            // }

            // if(Input.GetButtonDown("Jump") && jumpCount == 1 && glideTime >= 0.2)
            // {
            //     isGlide = false;
            //     isJump = false;
            //     glideTime = 0;
            //     time = 0; // 時間をそのまま経過させてしまうと、落下速度が大きすぎる
            // }







            // ダッシュ中にジャンプしたとき
            if(isDashJump)
            {
                moveDirection += jumpDirection*jumpFoarwardPower*2;
                moveDirection *= Time.deltaTime;
                                    
            }
            else
            {
                // スティックを少し倒している時
                if( Mathf.Abs(beforeJumpInputHorizontal) <= 0.5 && beforeJumpInputHorizontal !=0 
                || Mathf.Abs(beforeJumpInputVertical) <= 0.5 &&  beforeJumpInputVertical !=0)
                {
                    moveDirection += jumpDirection*jumpFoarwardPower*0.7f;
                     moveDirection *= Time.deltaTime;
                }
                // スティックを上記より倒している時
                if(Mathf.Abs(beforeJumpInputHorizontal) >= 0.5 || Mathf.Abs(beforeJumpInputVertical) >= 0.5 )
                {
                    moveDirection += jumpDirection*jumpFoarwardPower*1.4f;
                     moveDirection *= Time.deltaTime;

                    // 距離の調整
                    // if(moveDirection.magnitude >=1)
                    //     {
                    //         moveDirection/=moveDirection.magnitude ;
                    //     }
    
                } 

            }


            // moveDirection.y = jumpForce *Time.deltaTime;
            moveDirection.y = (float)jumpPower *Time.deltaTime;

            // 速さ＝初速＋加速×時間　の速さの公式を使っている。常に重力をかけてジャンプの時だけ初速加える
            
           // moveDirection.y = jumpForce + 0.5f * Physics.gravity.y * time;
           //  本来はmoveDirection.y = startPosition.y + jumpForce * time  + 0.5f * Physics.gravity.y  * time* time;になるが、
           //  最後にデルタタイムかけるので２，３項目のTIMEはなくしている。（）でくくっているイメージ
           // スタートポジションを足さない理由はムーブ関数に入れているから。普通にトランスフォームに代入するんだったら必要




            // ダッシュしながらジャンプすると正面方向の力を強くしたい

                // ジャンプ中は地上よりも動けなくするイメージ
                // moveDirection.x *=0.5f;
                // moveDirection.z *=0.5f;

           

        characterController.Move(moveDirection );
        }

        // if(isGlide) //スコープは落下より後ろ
        // {
        //     glideTime += Time.deltaTime;
        //     moveDirection.y = 0;

        // }
//         else
//         {
// moveDirection.y += Physics.gravity.y;
//         }








    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //ジャンプ処理
        if (hit.gameObject.tag == "ground")
        {
            isJump = false;
            isDashJump=false;
            time = 0;
            jumpCount = 0;
            isGrounded = true;
        }
    }

    // ジャンプした瞬間に方向転換したら逆のほうに向きながら飛んでしまう、これは向きの問題かも


}
