using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    // フレーム更新型

    // 追跡
    public GameObject Player;
    Vector3 beforeTargetPosi;

    // 回転
    private float horizontalAngle;
    public float verticalAngle;
    private Vector3 playerForward;
    [SerializeField] float rotateSpeed;


    public float angleInDegrees;
    [SerializeField] float verticalAngleLimit = 70;

    // １人称
    public bool isFirstPerson = false;
    public Vector3 firstPlayerForward;
    [SerializeField] float smoothnessFactor = 0.5f;
    public bool cameraMove;
    public bool isCameraMoveEnd = false;
    CharacterController _characterController;
    Vector3 cameraForward;
    Vector3 beforeTargetPosiFirst;
    PlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {

        _characterController = Player.GetComponent<CharacterController>();
        _playerController = Player.GetComponent<PlayerController>();

        beforeTargetPosi = Player.transform.position;//プレイヤーの位置を記録
        cameraMove = false;
    }

    // Update is called once per frame
    void Update()
    {




        if (Input.GetKeyDown("1") || Input.GetButtonDown("First"))
        {
            isFirstPerson = !isFirstPerson;
            if (isFirstPerson) //カメラの移動開始フラグ
            {
                cameraMove = true;
                isCameraMoveEnd = false;
            }
            else
            {
                // isFirstPersonとcameraMoveをそろえる
                cameraMove = false;
            }

            // １人称にした瞬間にプレイヤーの向きを１人称の向き（カメラの向き）にそろえる
            firstPlayerForward = transform.forward;
            firstPlayerForward.y = 0;
            Player.transform.forward = firstPlayerForward;

        }


        if (cameraMove && isCameraMoveEnd == false)
        {

            Vector3 targetPosition = Player.transform.position + Player.transform.forward + new Vector3(0, 0.5f, 0);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothnessFactor);

            //_characterController.enabled = false; //これも悪くはないが、これだと、落下が止まったりいろいろ不都合が起きる。
            //なのでプレイヤーコントローラーの方でスティック入力を受け付けないようにする



            if (Vector3.Distance(transform.position, targetPosition) < 0.01f) // 厳密な座標の一致は難しいため
            {
                isCameraMoveEnd = true;
                cameraMove = false;
                beforeTargetPosiFirst = Player.transform.position;

            }
        }
        else
        {
            _characterController.enabled = true;
        }


        if (!isFirstPerson)
        {
            // カメラの追尾　実際はリープを使ってもうすこし滑らかにできるかも
            transform.position += Player.transform.position - beforeTargetPosi; // カメラの位置にプレイヤーの位置の前フレームからの差分を代入して追跡させる
            beforeTargetPosi = Player.transform.position;//プレイヤー位置の更新
        }






        if (isFirstPerson)
        {

            //kokodesu
            //kokodesu
            //kokodesu

            if (isCameraMoveEnd) //カメラの移動が終わったら
            {
                //常にプレイヤーの前に、プレイヤーの正面方向をむいたカメラが存在　その上で角度を書き換える。
                // カメラの向きをプレイヤーの正面にそろえる
                firstPlayerForward = Player.transform.forward;
                //firstPlayerForward.y = 0;//カメラが上向きの時にプレイヤーもそれにつられて回転しないように
                transform.forward = firstPlayerForward;
                //カメラをプレイヤーの前に移動させる
                transform.position = Player.transform.position + Player.transform.forward + new Vector3(0, 0.5f, 0);

                // カメラの垂直回転
                verticalAngle += -Input.GetAxis("VerticalCamera") * rotateSpeed; // マイナス符号を付けることで上下反転
                verticalAngle = Mathf.Clamp(verticalAngle, -80, 80); // 垂直回転の角度を制限
                transform.Rotate(Vector3.right, verticalAngle);
            }

        }
        else
        {
            // 2つのベクトルの内積を計算
            playerForward = Player.transform.forward.normalized;
            float dotProduct = Vector3.Dot(playerForward, transform.forward.normalized);

            // 内積の値から角度を計算（ラジアンから度に変換）
            angleInDegrees = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;



            if (playerForward == null)
            {
                return;
            }




            // カメラの水平回転
            horizontalAngle += Input.GetAxis("HorizontalCamera") * rotateSpeed;

            // カメラの垂直回転
            verticalAngle += -Input.GetAxis("VerticalCamera") * rotateSpeed; // マイナス符号を付けることで上下反転
            verticalAngle = Mathf.Clamp(verticalAngle, -5, verticalAngleLimit); // 垂直回転の角度を制限

            // カメラの位置をプレイヤーの周囲に回転させる
            Vector3 offset = Quaternion.Euler(verticalAngle, horizontalAngle, 0) * new Vector3(0, 0, -9);
            transform.position = Player.transform.position + offset;

            // カメラがプレイヤーを常に向くようにする
            transform.LookAt(Player.transform.position);
        }

    }





    // 頭いいベクトル逆方向型
    //public Transform target; // 追尾対象のプレイヤーのTransform
    //public float followDistance = 5f; // カメラの追尾距離

    //void LateUpdate()
    //{
    //    if (target == null)
    //    {
    //        Debug.LogWarning("カメラの追尾対象が設定されていません。");
    //        return;
    //    }

    //    // 追尾対象の後ろに追従する位置を計算
    //    // target.forwardの逆をベクトル計算することで、常にカメラを後ろにすることに成功
    //    Vector3 behindPosition = target.position - target.forward * followDistance;
    //    behindPosition.y = 1;

    //    // カメラの位置を更新
    //    transform.position = behindPosition;
    //   // transform.LookAt(target); // カメラがプレイヤーを常に見つめるようにする
    //}




    // 初期位置から距離を計算して、カメラの現在のポジションにその距離を足して一定の距離を守るタイプ

    //public Transform target; // 追尾対象のプレイヤーのTransform
    //public float distanceFromPlayer = 5f; // カメラの初期位置からの距離
    //public float followSpeed = 5f; // カメラの追尾速度

    //private Vector3 initialOffset; // カメラの初期位置とプレイヤーの位置のオフセット

    //void Start()
    //{


    //    // カメラの初期位置とプレイヤーの位置のオフセットを計算
    //    initialOffset = transform.position - target.position;
    //}

    //void LateUpdate()
    //{


    //    // カメラの追尾対象の位置を計算
    //    Vector3 targetPosition = target.position + initialOffset.normalized * distanceFromPlayer;

    //    // 追尾速度を考慮してカメラの位置を更新
    //    Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
    //    transform.position = newPosition;
    //}
}

