using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMove : MonoBehaviour
{



    //Makeの基底クラス

    [SerializeField] float objUpDownMovingSpeed = 20;
    [SerializeField] float objForwardBackMovingSpeed = 20;

    public float inputObjVertical;

    public float distanceToPlayer;


    public PlayerController _PlayerController;
    PlayerControl _playerControl;

    GameObject Player;

    public Vector3 verocity;

    public Rigidbody rb;
    [SerializeField] float rbAjustSpeed = 30f;



    [SerializeField] float objForwardLimit = 20;
    [SerializeField] float objBackLimit = 5;

    public RaycastHit raycasthit;

    public float distance = 50;
    public Vector3 direction = -Vector3.up;
    CameraFollow _cameraFollow;
    GameObject Camera;
    float horizontalAngle;
    public ObjManager _objManager;
    public GameObject ObjManager;

    public ConfilmSet _confilmSetFront;
    public ConfilmSet _confilmSetBack;
    public GameObject frontColl;
    public GameObject backColl;
    public bool isObjVec;
    public bool isSetable;
    ObjjRotate _objjRotate;

    // Start is called before the first frame update
    void Start()
    {
        PlayerControl yourComponent = GameObject.FindObjectOfType<PlayerControl>();


        if (yourComponent != null)
        {
            Player = yourComponent.gameObject;
            Transform objTransform = yourComponent.transform;

            Debug.Log("objobj" + Player);
            transform.parent = Player.transform;

            // ゲームオブジェクト名で絞るより、プログラム名で絞ったほうが変更が後々少なそうなのでプログラムで絞る。
        }

        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        _cameraFollow = Camera.GetComponent<CameraFollow>();

        ObjManager = GameObject.FindGameObjectWithTag("ObjManager");
        //  ObjectContainer objectContainer = FindObjectOfType<ObjectContainer>();
        _objManager = ObjManager.GetComponent<ObjManager>();
    }

    // Update is called once per frame
    void Update()
    {

        rb = GetComponent<Rigidbody>();
        Vector3 playerForward = Player.transform.forward.normalized;

        //現段階ではstartで取得してしまってもいいかもしれないが（そもそもこのオブジェ生成が成功している時点で、isMakeがtrueなので。
        //後々のことを考えてプレイヤーの状態は参照することが多いので、一旦毎フレーム取得
        _playerControl = Player.GetComponent<PlayerControl>();


        if (_playerControl.isGrounded)
        {

            Vector3 objPosi = this.gameObject.transform.position;
            objPosi.y = Player.transform.position.y;

            distanceToPlayer = Vector3.Distance(objPosi, Player.transform.position);



            if (_playerControl.isMake)
            {


                objSet();

                inputObjVertical = Input.GetAxis("VerticalCamera");
                // マイナス符号を付けることで上下反転

                Vector3 playerUpVec = Player.transform.up;
                playerUpVec = playerUpVec.normalized;

                // float verticalStic = Input.GetAxis("Vertical");
                // このあと調整に使うかも

                Vector3 distanceToPlayerHeighTransPosi =
                new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z);

                float distanceToPlayerHeigh = Vector3.Distance(distanceToPlayerHeighTransPosi, Player.transform.position);

                if (distanceToPlayerHeigh < 15)
                {
                    verocity = playerUpVec * rbAjustSpeed * inputObjVertical;
                    rb.velocity = verocity;
                }
                // else if (distanceToPlayerHeigh > 15 && distanceToPlayerHeigh < 16)
                else
                {
                    // このあとカメラ調整に使うかも
                    //horizontalAngle = Input.GetAxis("HorizontalCamera");

                    rb.velocity = new Vector3(0, 0, 0);

                    //範囲何に戻すための処理　2
                    // inputObjVertical = 0;
                    // transform.position = transform.position + -playerUpVec * 0.01f;
                    // transform.position = new Vector3(transform.position.x, transform.position.y - 0.001f, transform.position.z);


                    if (inputObjVertical < 0)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
                        verocity = playerUpVec * rbAjustSpeed * inputObjVertical;
                        rb.velocity = verocity;

                    }
                    else if (inputObjVertical > 0)
                    {
                        inputObjVertical = 0;
                    }

                }
                // else if (distanceToPlayerHeigh > 16)
                // {

                //     //角度がうまくいかない、マックの調節後の話してから？
                //     // _cameraFollow.enabled = false;


                //     // if (horizontalAngle < 0)
                //     // {
                //     //     if (horizontalAngle > 0)
                //     //     {
                //     //         _cameraFollow.enabled = true;
                //     //     }
                //     // }
                //     // else
                //     // {
                //     //     if (horizontalAngle < 0)
                //     //     {
                //     //         _cameraFollow.enabled = true;
                //     //     }
                //     // }






                //     //正面方向にすすんで行ったときに、制限高度より高くならないように
                //     // _playerControl.enabled = false;
                //     // if (verticalStic < 0)
                //     // {
                //     //     _playerControl.enabled = true;
                //     //     Debug.Log("tomatoma2");
                //     // }

                //     transform.position = new Vector3(transform.position.x, transform.position.y - 50 * Time.deltaTime, transform.position.z);
                // }

                if (_playerControl.isMake)
                {
                    if (Input.GetButton("First"))
                    {

                        inputObjVertical = Input.GetAxis("Vertical");

                        // 制限距離以上に移動しようとした場合は移動しない処理
                        if (distanceToPlayer < objForwardLimit && distanceToPlayer > objBackLimit)
                        {
                            verocity = playerForward * rbAjustSpeed * inputObjVertical;
                            rb.velocity = verocity;
                        }
                        else
                        {
                            if (distanceToPlayer > objForwardLimit)
                            {
                                //範囲何に戻すための処理　2
                                // rb.velocity = new Vector3(0, 0, 0);
                                // transform.position = transform.position + -playerForward * 0.01f;

                                rb.velocity = new Vector3(0, 0, 0);

                                if (inputObjVertical < 0)
                                {
                                    transform.position = transform.position + -playerForward * 0.1f;

                                    verocity = playerUpVec * rbAjustSpeed * inputObjVertical;
                                    rb.velocity = verocity;
                                }
                            }

                            if (distanceToPlayer < objBackLimit)
                            {
                                //範囲何に戻すための処理　2
                                // rb.velocity = new Vector3(0, 0, 0);
                                // transform.position = transform.position + playerForward * 0.01f;

                                if (inputObjVertical > 0)
                                {
                                    transform.position = transform.position + playerForward * 0.1f;

                                    verocity = playerUpVec * rbAjustSpeed * inputObjVertical;
                                    rb.velocity = verocity;
                                }
                            }
                        }
                    }
                }

                //前後異動ではなく、縦移動で斜面に沿って動かして下がってきたときに、設定距離よりも動いてしまう時の処理（プレイヤーの上にかぶさるのを防ぐ
                if (distanceToPlayer < objForwardLimit && distanceToPlayer > objBackLimit)
                {

                }
                else
                {

                    if (distanceToPlayer > objForwardLimit)
                    {
                        rb.velocity = new Vector3(0, 0, 0);
                        transform.position = transform.position + -playerForward * 0.1f;

                    }

                    if (distanceToPlayer < objBackLimit)
                    {
                        rb.velocity = new Vector3(0, 0, 0);
                        transform.position = transform.position + playerForward * 0.1f;

                    }

                }

                ObjInternal();


                //今の座標がプレイヤー正面の延長線上にあるかどうかを判断する。延長線上になければ移動させる　これがないと障害物に引っかかって、オブジェがどんどんずれていく
                Vector3 heightZeroAssumedPosi = transform.position;
                heightZeroAssumedPosi.y = Player.transform.position.y;

                Vector3 targetPosition = Player.transform.position + playerForward * Vector3.Distance(Player.transform.position, heightZeroAssumedPosi);//+ new Vector3(0, 1, 0)

                //作ったものがプレイヤーの正面に常にあるように。
                if (Vector3.Distance(heightZeroAssumedPosi, targetPosition) > 0.05f)
                {
                    transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
                }

                //デバッグ用のray
                // Debug.DrawRay(transform.position, new Vector3(-0.29f, 0.00f, -0.96f), Color.red);
            }

        }


    }

    private void objSet()
    {
        _objjRotate = GetComponent<ObjjRotate>();

        // 回転軸のタイプを見て、オブジェのベクトルがあるべき方向にあるかを調べて、isSetableを更新
        if (_objjRotate.selectedType == ObjjRotate.rotateType.horizon)
        {
            isSetable = isObjVecDiscrimination();
            Debug.Log("isSetableisSetable" + isSetable);
        }
        else if (_objjRotate.selectedType == ObjjRotate.rotateType.vertical)
        {

        }
        else if (_objjRotate.selectedType == ObjjRotate.rotateType.arbitraryAxis)
        {

        }

        //そのうえでちゃんと接地できているかを確認してisSetableを更新　ここの順序は逆にしてはいけない
        isSetable = isSetableDiscrimination();

        if (isSetable)
        {
            if (Input.GetButtonDown("Dash"))
            {
                transform.parent = null;

                if (_objManager != null)
                {
                    // 配列に格納する処理
                    for (int i = 0; i < _objManager.objArray.Length; i++)
                    {
                        if (_objManager.objArray[i] == null)
                        {
                            _objManager.objArray[i] = gameObject;
                            break;
                        }
                    }
                }
                else
                {
                    Debug.LogError("ObjectStorage component not found!");
                }

                ObjjRotate _objjRotate = GetComponent<ObjjRotate>();
                _objjRotate.enabled = false;

                // _playerController.isMake = false;
                _playerControl.isMake = false;

                _playerControl.makeEnd = true;
                gameObject.tag = "ground";

                //固定化
                Destroy(rb);
                Destroy(this);//多分これいみない
                this.enabled = false;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        isObjVecDiscrimination();

        float obstacleHeight = collision.contacts[0].point.y;

        // Vector3 distanceToPlayerHeighTransPosi2 = new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z);
        // float distanceToPlayerHeigh2 = Vector3.Distance(distanceToPlayerHeighTransPosi2, Player.transform.position);


        // if (distanceToPlayerHeigh2 < 15)
        // {



        // 衝突している他のオブジェクトの法線ベクトルを取得
        Vector3 contactNormal = collision.contacts[0].normal.normalized;



        // 衝突した面をチェック
        //オブジェクト自身の上下面か、衝突した障害物の面はオブジェクト自身に対して斜面か、垂直か　の場合分け
        if (contactNormal == Vector3.up || contactNormal == -Vector3.up)
        {
            // 衝突した側面が上側面の場合、ここで必要な処理を実行
            Debug.Log("normar" + contactNormal);
        }
        else if (Mathf.Abs(Vector3.Dot(contactNormal, Vector3.up)) < 1 && Mathf.Abs(Vector3.Dot(contactNormal, Vector3.up)) > 0) //斜面判断
        {
            // Debug.Log("sssangle" + Mathf.Abs(Vector3.Dot(contactNormal.normalized, playerForward)) +
            //  "aaaangle2 " + Mathf.Abs(Vector3.Dot(contactNormal.normalized, Vector3.up)) +
            //   "contactNormal" + contactNormal + "colPoint" + collision.contacts[0].point.y);

            this.gameObject.transform.position = new Vector3(transform.position.x, obstacleHeight + transform.localScale.y / 2 + 0.03f, transform.position.z);


        }
        else
        {

            //法線ベクトルが手前側で直観と反する場合。プレイヤーベクトルと法線ベクトルのときで場合分けする。ノーマライズして角度計算しやすくする。
            //多分頂点？は微量の法線ベクトルがでるので、条件追加
            // Debug.Log("aaaangle" + Mathf.Abs(Vector3.Dot(contactNormal.normalized, playerForward)) +
            //  "aaaangle2 " + Mathf.Abs(Vector3.Dot(contactNormal.normalized, Vector3.up)) +
            //   "contactNormal" + contactNormal + "colPoint" + collision.contacts[0].point.y);

            // if (Vector3.Dot(contactNormal, Vector3.up) != 0)
            // {



            // if (Mathf.Abs(Vector3.Dot(contactNormal.normalized, playerForward)) > 0.4f
            //  || Mathf.Abs(Vector3.Dot(contactNormal.normalized, playerForward)) < 0.05)
            // if (Mathf.Abs(Vector3.Dot(contactNormal.normalized, playerForward)) < 0.05)
            // {


            //     float obstacleHeight2 = collision.contacts[0].point.y;

            //     // Debug.Log("saka" + Vector3.Dot(contactNormal, Vector3.up) + "aaa" + obstacleHeight2);


            //     // this.gameObject.transform.position = transform.position + new Vector3(0, 25 * Time.deltaTime, 0);
            //     this.gameObject.transform.position = new Vector3(transform.position.x, obstacleHeight2 + transform.localScale.y / 2 + 50 * Time.deltaTime, transform.position.z);
            //     return;


            //     //this.gameObject.transform.position = new Vector3(transform.position.x, obstacleHeight, transform.position.z); // 0.05fは調整値
            //     // this.gameObject.transform.position = new Vector3(transform.position.x, obstacleHeight + transform.localScale.y / 2, transform.position.z); // 0.05fは調整値
            // }
            // }




            // this.gameObject.transform.position = this.gameObject.transform.position + playerForward * -0.01f;




            this.gameObject.transform.position = new Vector3(transform.position.x, obstacleHeight + transform.localScale.y / 2 + 0.03f, transform.position.z);




            // // 障害物の上面の位置を取得して移動 これもわるくはない
            // Vector3 obstacleTopPosition = collision.collider.bounds.max;
            // newPosition.y = obstacleTopPosition.y + (transform.localScale.y * 0.5f); // オブジェクトの中心が上面に位置するように調整
            // transform.position = newPosition;

            //transform.position = new Vector3(transform.position.x, collision.transform.position.y + collision.gameObject.transform.localScale.y / 2 + 0.03f, transform.position.z);
        }
        // }
    }
    protected virtual bool isObjVecDiscrimination()//オブジェによって処理を継承先で変える
    {
        return isObjVec;
    }
    protected virtual bool isSetableDiscrimination()//オブジェによって処理を継承先で変える
    {
        return isSetable;
    }

    protected virtual void ObjInternal()//オブジェによって処理を継承先で変える
    {

    }

}
