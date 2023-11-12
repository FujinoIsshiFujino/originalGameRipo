using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


/// <summary>
///カメラの挙動
/// </summary>
public class CameraFollow : MonoBehaviour
{
    // フレーム更新型

    // 追跡
    public GameObject Player;
    Vector3 beforeTargetPosi;

    // 回転
    public float horizontalAngle;
    public float verticalAngle;
    private Vector3 playerForward;
    [SerializeField] float rotateSpeed;


    public float angleInDegrees;
    [SerializeField] float verticalUpAngleLimit = 70;
    [SerializeField] float verticalDownAngleLimit = -30;

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
    public float cameraDistance;
    Vector3 offset;// 回転時のプレイヤーからの離れ具合、
    [SerializeField] float upDistanceCorrection = 20f; //上方向のカメラの回転時の、距離の補正
    [SerializeField] float downDistanceCorrection = 4f; //下方向のカメラの回転時の、距離の補正
    [SerializeField] float verticalAngleUnderZeroGazePoint = 20f; // /下方向のカメラの回転時の注視点の高さの補正



    LockOn _lockOn;
    private Vector3 lockOnGazePoint;
    int i;
    public List<GameObject> realTimeEnemyList = new List<GameObject>(); // 前回フレームのリスト
    public List<GameObject> GazeEnemyList = new List<GameObject>(); // 今回フレームのリスト
    public bool listHasChanged = false; // リストに変更があったかどうかを示すフラグ




    [SerializeField] private float rotationSpeed = 5.0f; // 回転の速度


    private Quaternion targetRotation; // 目標の回転
    public bool isRotateLockOn;
    public bool isSwitching = false;

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


        //確認用でcameraDistanceは無くてもいい。
        cameraDistance = (Player.transform.position - this.transform.position).magnitude;

        _lockOn = GetComponent<LockOn>();



        /// <summary>
        ///一人称視点
        /// </summary>


        if (!_playerController.isMake)
        {
            //どのボタンかは決定していないけど、多分物体を回転させるときに、一度使われているボタンを押すはずなので、状況に合わせて個々の処理を変える
            if (Input.GetButtonDown("First"))
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
            /// <summary>
            ///３人称視点
            /// </summary>


            // 2つのベクトルの内積を計算
            playerForward = Player.transform.forward.normalized;
            float dotProduct = Vector3.Dot(playerForward, transform.forward.normalized);

            // 内積の値から角度を計算（ラジアンから度に変換）
            angleInDegrees = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;



            if (playerForward == null)
            {
                return;
            }

            // カメラの水平回転入力
            horizontalAngle += Input.GetAxis("HorizontalCamera") * rotateSpeed;

            if (!_playerController.isMake)
            {
                // カメラの垂直回転入力
                verticalAngle += -Input.GetAxis("VerticalCamera") * rotateSpeed; // マイナス符号を付けることで上下反転
                verticalAngle = Mathf.Clamp(verticalAngle, verticalDownAngleLimit, verticalUpAngleLimit); // 垂直回転の角度を制限

            }


            if (verticalAngle >= 30)
            {

                // verticalAngleが上がるほど、カメラのとの距離を離す
                float distance = 9 + (verticalAngle - 30) / upDistanceCorrection;
                offset = new Vector3(0, 0, -distance); // カメラを適切な距離に配置

                // カメラをプレイヤーの周りに回転させる
                // ベクトルをクォータニオンの行列で１次変換している
                offset = Quaternion.Euler(verticalAngle, horizontalAngle, 0) * offset;



                if (_lockOn.isButtonLock)
                {

                    lockOnStart();
                }
                else
                {
                    lockOnEnd(verticalAngle);
                    isSwitching = false;
                    GazeEnemyList.Clear();
                    i = 0;
                }


            }
            else if (verticalAngle < 0)
            {

                //  verticalAngleが下がるほど、カメラを近づける
                float distance = 9 + (verticalAngle) / downDistanceCorrection;
                offset = Quaternion.Euler(0, horizontalAngle, 0) * new Vector3(0, 0, -distance) + new Vector3(0, 1f, 0); //new Vector3は床下が見えないように高さ調整


                if (_lockOn.isButtonLock)
                {

                    lockOnStart();

                }
                else
                {
                    if (!_playerController.isMake)
                    {
                        // // カメラをプレイヤーに向ける
                        // //new Vector3(0, Mathf.Abs(verticalAngle) / verticalAngleUnderZeroGazePointでカメラの注視点をプレイヤーから少し上にずらしていく。verticalAngleUnderZeroGazePointは補正
                        transform.LookAt(Player.transform.position + new Vector3(0, Mathf.Abs(verticalAngle) / verticalAngleUnderZeroGazePoint, 0));
                        // カメラの注視点の違いから、lockOnEnd(verticalAngle)は使わない

                        isSwitching = false;
                        GazeEnemyList.Clear();
                        i = 0;
                    }
                }




            }
            else
            {
                // カメラの位置をプレイヤーの周囲に回転させる
                offset = Quaternion.Euler(verticalAngle, horizontalAngle, 0) * new Vector3(0, 0, -9);

                if (_lockOn.isButtonLock)
                {

                    lockOnStart();
                }
                else
                {
                    // カメラがプレイヤーを常に向くようにする
                    // transform.LookAt(Player.transform.position);
                    lockOnEnd(verticalAngle);
                    isSwitching = false;
                    i = 0;
                }

            }

            transform.position = Player.transform.position + offset;


            if (_playerController.isMake)
            {
                // 作成後のオブジェを検知してしまわないようにプレイヤーの子オブジェクトの中から特定のタグを持つオブジェクトを探す 
                Transform makeObj = null;

                foreach (Transform child in Player.GetComponentInChildren<Transform>())
                {
                    if (child.tag == "Make")
                    {
                        makeObj = child;
                    }
                }


                //暫定的なカメラのずれの対応。offsetとisMake時の注視点でガタガタしていたので、isMake時には全部が終わった最後に角度調整するようにこのファイル内のこの位置に記述
                if (makeObj != null)
                {
                    verticalAngle = 20;//角度によってカメラの距離が変化するので、Makeボタンを押したときの角度に関係なく定位置にカメラを移動させるため
                    transform.position = new Vector3(transform.position.x, Player.transform.position.y + 5, transform.position.z);

                    Vector3 objPlayerDistance = (makeObj.position - Player.transform.position) / 2;
                    //transform.position = new Vector3(transform.position.x, Player.transform.position.y + 5, transform.position.z);
                    transform.LookAt(Player.transform.position + objPlayerDistance);
                }
            }
        }
    }



    void lockOnStart()
    {
        // _lockOn.enemyListDistanceを注視すると、カメラはプレイヤーとの距離に応じて勝手にロックオン対象を切り替えてしまう。なのでカメラは別のGazeEnemyListを注視しづける。
        // GazeEnemyListは初回ロックオフ→オン時に代入され、ロックオン対象の切り替えをして、ロックオン範囲内の敵情報が変わったフレームorロックオンが終わるor敵の撃破（これはまだ未処理）によって更新される
        //更新されたかどうかはListsAreEqualdメソッドで判断
        //ロックオン範囲内の敵情報が変わっていない状態で、ロックオン対象を切り替えてもlistは更新されず、切り替えるごとにそのままlistのインデックスをあげていって遠い敵をロックオンする
        //更新された状態においても、更新前にロックオンしていた敵が更新後に一番近い（index０）の場合と、そうでない場合にわけた。


        //ロックオフ→オン時にの処理
        if (isSwitching == false)
        {
            GazeEnemyList = _lockOn.previousList;
        }

        //プレイヤーとの距離が一番近い敵のlistを常に保持
        realTimeEnemyList = _lockOn.enemyListDistance;



        // ロックオン対象切り替え
        if (Input.GetButtonDown("L1") || Input.GetKeyDown("e"))
        {
            // リストがL1前後と異なるかをチェック
            if (!ListsAreEqual(realTimeEnemyList, GazeEnemyList))
            {
                listHasChanged = true;

                if (realTimeEnemyList[0] == GazeEnemyList[i])
                {
                    //一番近い敵がL1前後で同じ場合。
                    i = 1;
                }
                else
                {
                    i = 0;
                }

                GazeEnemyList = new List<GameObject>(realTimeEnemyList);

            }
            else
            {
                listHasChanged = false;
                i++;
            }

            if (GazeEnemyList.Count - i <= 0)
            {
                i = 0;
            }

            isSwitching = true;

        }


        // 旧
        // if (Input.GetButtonDown("L1"))
        // {

        //     i++;
        //     if (_lockOn.enemyListDistance.Count - i <= 0)
        //     {
        //         i = 0;
        //     }

        // }

        //  ロックオンオン時はプレイヤーと敵の中心を回転の軸にする
        // lockOnGazePoint = (_lockOn.enemyListDistance[i].transform.position - Player.transform.position) / 2;

        lockOnGazePoint = (GazeEnemyList[i].transform.position - Player.transform.position) / 2;

        Vector3 directionToTarget = (Player.transform.position + lockOnGazePoint) - transform.position;
        // directionToTarget.y = 0;
        // ベクトルから、回転先のクォータニオンを取得
        Quaternion rotationWithoutZ = Quaternion.LookRotation(directionToTarget);

        // Z軸の回転を0度に固定
        targetRotation = Quaternion.Euler(rotationWithoutZ.eulerAngles.x, rotationWithoutZ.eulerAngles.y, 0);
        // transform.rotation = targetRotation;


        // 回転を補間して滑らかに行う
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 一定の角度以下になったら補完をやめ、注視する
        float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);
        if (angleDifference < 5.0f)
        {
            transform.LookAt(Player.transform.position + lockOnGazePoint);
        }
    }
    void lockOnEnd(float verticalAngle)
    {
        Vector3 directionToTarget = Player.transform.position - transform.position;
        Quaternion rotationWithoutZ = Quaternion.LookRotation(directionToTarget);

        // Z軸の回転を0度に固定
        targetRotation = Quaternion.Euler(rotationWithoutZ.eulerAngles.x, rotationWithoutZ.eulerAngles.y, 0);

        if (isRotateLockOn == false)
        {
            // 回転を補間して滑らかに行う
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }


        float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);

        // 一定の角度以下になったら補完をやめ、注視する
        if (angleDifference < 5.0f)
        {
            if (verticalAngle >= 0)
            {
                transform.LookAt(Player.transform.position);
            }


            isRotateLockOn = true;
        }
        else
        {
            isRotateLockOn = false;
        }
    }

    // 2つのリストが等しいかどうかをチェックするメソッド
    private bool ListsAreEqual(List<GameObject> list1, List<GameObject> list2)
    {
        if (list1.Count != list2.Count)
        {
            return false; // リストの要素数が異なる場合、異なるリストとみなす
        }

        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i] != list2[i])
            {
                return false; // 要素が異なる場合、異なるリストとみなす
            }
        }

        return true; // リストが等しい場合、同じリストとみなす
    }

}

