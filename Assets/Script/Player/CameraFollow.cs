using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


/// <summary>
///カメラの挙動
/// </summary>
public class CameraFollow : MonoBehaviour
{

    //基本的にはステートによって分けるのはメイクかそうじゃないかだけ。あとは共通化。
    //今後ステートが増えれば、ステートごとにパブリックなboolをつくってカメラはそれを監視する。


    // フレーム更新型

    // 移動や回転
    [SerializeField] GameObject Player;
    public float horizontalAngle;
    public float verticalAngle;
    private Vector3 playerForward;
    public bool cameraMove;
    public bool isFirstPerson = false;
    public bool isCameraMoveEnd = false;
    public float angleInDegrees;//検証用
    [SerializeField] float rotateSpeed;
    [SerializeField] float verticalUpAngleLimit = 70;
    [SerializeField] float verticalDownAngleLimit = -30;
    [SerializeField] float smoothnessFactor = 12.5f;
    [SerializeField] float upDistanceCorrection = 20f; //上方向のカメラの回転時の、距離の補正
    [SerializeField] float downDistanceCorrection = 4f; //下方向のカメラの回転時の、距離の補正
    [SerializeField] float verticalAngleUnderZeroGazePoint = 20f; // /下方向のカメラの回転時の注視点の高さの補正
    [SerializeField] Vector3 firstPersonDistanceY = new Vector3(0, 1, 0);

    Vector3 beforeTargetPosi;//カメラの追尾がいらなくなったので不要かもしれないがい一応保留
    PlayerControl _playerControl;
    Vector3 offset;// 回転時のプレイヤーからの離れ具合、
    BridgeMove _objMove;
    GameObject makeObj;



    //ロックオン関連
    private Vector3 lockOnGazePoint;
    int i;
    public List<GameObject> realTimeEnemyList = new List<GameObject>(); // 前回フレームのリスト
    public List<GameObject> GazeEnemyList = new List<GameObject>(); // 今回フレームのリスト
    [SerializeField] private float rotationSpeed = 12f; // 回転の速度

    Quaternion targetRotation; // 目標の回転
    bool isRotateLockOn;
    bool isSwitching = false;
    LockOn _lockOn;
    // public bool listHasChanged = false; // リストに変更があったかどうかを示すフラグ // 今後使う可能性がある


    void Start()
    {
        _playerControl = Player.GetComponent<PlayerControl>();

        // beforeTargetPosi = Player.transform.position;//プレイヤーの位置を記録
        cameraMove = false;
    }

    // Update is called once per frame
    void Update()
    {

        _lockOn = GetComponent<LockOn>();

        makeObj = GameObject.FindGameObjectWithTag("Make");
        if (makeObj != null)
        {
            _objMove = makeObj.GetComponent<BridgeMove>();
        }
        else
        {
            Debug.Log("MakeObjは見つかりません");
        }

        getInputAngle();

        /// <summary>
        ///一人称視点
        /// </summary>
        if (!_playerControl.isMake)
        {
            if (Input.GetButtonDown("First"))
            {
                firstPersonCameraSetUp();
            }
        }


        //１人称視点へカメラが移動中の処理
        if (cameraMove && isCameraMoveEnd == false)
        {

            Vector3 targetPosition = Player.transform.position + firstPersonDistanceY + Player.transform.forward.normalized;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothnessFactor);

            if (Vector3.Distance(transform.position, targetPosition) < 0.5f) // 厳密な座標の一致は難しいため
            {
                isCameraMoveEnd = true;
                cameraMove = false;
            }
        }
        else
        {
            _playerControl.getInputMove(true);
        }

        // カメラの追尾　カメラの移動に際して　
        //transform.position = Player.transform.position + offset;
        //  を使用しているので、この処理は不要
        // if (!isFirstPerson)
        // {
        //     transform.position += Player.transform.position - beforeTargetPosi; // カメラの位置にプレイヤーの位置の前フレームからの差分を代入して追跡させる
        //     beforeTargetPosi = Player.transform.position;//プレイヤー位置の更新
        // }

        if (isFirstPerson)
        {
            _lockOn.isLockOn = false;
            //カメラの移動が終わってからのカメラの回転と移動処理
            if (isCameraMoveEnd)
            {
                verticalAngle = Mathf.Clamp(verticalAngle, -50, 60); // 垂直回転の角度を制限

                offset = firstPersonDistanceY + new Vector3(0, 0, 1); // １人称にしたときの初期位置を適切な位置に配置

                offset = Quaternion.Euler(0, horizontalAngle, 0) * offset;//位置回転
                transform.rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0); //姿勢回転
                transform.position = Player.transform.position + offset;

                if (Math.Abs(horizontalAngle) >= 360f)
                {
                    horizontalAngle = 0f;
                }
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

            // 内積の値から角度を計算（ラジアンから度に変換）これは検証用
            angleInDegrees = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

            if (playerForward == null)
            {
                return;
            }

            if (Math.Abs(horizontalAngle) >= 360f)
            {
                horizontalAngle = 0f;
            }

            // 垂直回転の角度を制限
            if (!_playerControl.isMake)
            {
                verticalAngle = Mathf.Clamp(verticalAngle, verticalDownAngleLimit, verticalUpAngleLimit);
            }


            if (verticalAngle >= 30)
            {
                eachAngleCameraMove();
            }
            else if (verticalAngle < 0)
            {

                eachAngleCameraMove();
            }
            else
            {
                eachAngleCameraMove();

            }

            transform.position = Player.transform.position + offset;


            //make時のカメラ挙動
            //offsetとisMake時の注視点でガタガタしていたので、isMake時には全部が終わった最後に角度調整するようにこのファイル内のこの位置に記述
            if (_playerControl.isMake)
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

                if (makeObj != null)
                {
                    if (_objMove.distanceToPlayerHeigh < 16)
                    {
                        verticalAngle = 20;//角度によってカメラの距離が変化するので、Makeボタンを押したときの角度に関係なく定位置にカメラを移動させるため
                        transform.position = new Vector3(transform.position.x, Player.transform.position.y + 5, transform.position.z);

                        Vector3 objPlayerDistance = (makeObj.position - Player.transform.position) / 2;
                        //transform.position = new Vector3(transform.position.x, Player.transform.position.y + 5, transform.position.z);
                        transform.LookAt(Player.transform.position + objPlayerDistance);
                    }
                    else //規定値以上の高さにオブジェが移動すると、カメラは注視を辞める
                    {
                        //カメラの画核のふちにアイコンを発生させる処理を追加
                    }
                }
            }
        }
    }


    //角度ごとのカメラ挙動の制御
    private void eachAngleCameraMove()
    {
        if (verticalAngle >= 30)
        {
            // verticalAngleが上がるほど、カメラのとの距離を離す
            float distance = 9 + (verticalAngle - 30) / upDistanceCorrection;
            offset = new Vector3(0, 0, -distance); // カメラを適切な距離に配置


            // カメラをプレイヤーの周りに回転させる
            // ベクトルをクォータニオンの行列で１次変換して位置の回転とベクトルのスカラー倍をしている
            //これじたいは位置回転なので、姿勢回転はlockOnEnd()
            offset = Quaternion.Euler(verticalAngle, horizontalAngle, 0) * offset;
        }
        else if (verticalAngle < 0)
        {
            //  verticalAngleが下がるほど、カメラを近づける
            float distance = 9 + verticalAngle / downDistanceCorrection;
            distance = Mathf.Clamp(distance, 1.4f, 10); //カメラがプレイヤーに近づく距離を制限　make時に影響
            offset = Quaternion.Euler(0, horizontalAngle, 0) * new Vector3(0, 0, -distance) + new Vector3(0, 1f, 0); //new Vector3は床下が見えないように高さ調整
        }
        else
        {
            // カメラの位置をプレイヤーの周囲に回転させる
            offset = Quaternion.Euler(verticalAngle, horizontalAngle, 0) * new Vector3(0, 0, -9);
        }

        if (_lockOn.isLockOn)
        {
            lockOnStart();
        }
        else
        {
            if (verticalAngle >= 0)
            {
                lockOnEnd(verticalAngle);

            }
            else
            {
                // 0度以下の時は上の処理でカメラがプレイヤーに近づいていく、その時にカメラの注視点を上にする
                // //new Vector3(0, Mathf.Abs(verticalAngle) / verticalAngleUnderZeroGazePointでカメラの注視点をプレイヤーから少し上にずらしていく。verticalAngleUnderZeroGazePointは補正
                float gazaPointY = Mathf.Abs(verticalAngle) / verticalAngleUnderZeroGazePoint;
                gazaPointY = Mathf.Clamp(gazaPointY, 0, 1.6f);  //カメラがプレイヤーに近づく時に回転する角度を制限　make時に影響
                transform.LookAt(Player.transform.position + new Vector3(0, gazaPointY, 0));
                // カメラの注視点の違いから、lockOnEnd(verticalAngle)は使わない
            }

            isSwitching = false;
            GazeEnemyList.Clear();
            i = 0;

        }
    }


    //入力受付
    private void getInputAngle()
    {
        // カメラの水平回転入力
        horizontalAngle += Input.GetAxis("HorizontalCamera") * rotateSpeed;
        // カメラの垂直回転入力
        verticalAngle += -Input.GetAxis("VerticalCamera") * rotateSpeed; // マイナス符号を付けることで上下反転
    }

    //１人称視点へカメラの移動開始、フラグ起動、位置や角度のセットアップ・リセット
    void firstPersonCameraSetUp()
    {
        isFirstPerson = !isFirstPerson;
        if (isFirstPerson)
        {
            cameraMove = true;
            isCameraMoveEnd = false;

            //１人称視点の度に視点はリセット
            verticalAngle = 0;

            // カメラを１人称にした時の座標やrotationに関係なく正面方向に向かせる
            Vector3 direction = transform.forward;
            direction.y = 0f; // y軸の回転を無視する場合はこの行を追加
            transform.rotation = Quaternion.LookRotation(direction);
        }
        else //１人称辞視点やめる時
        {
            // isFirstPersonとcameraMoveをそろえてもいいかも
            cameraMove = false;
            verticalAngle = 10;// 少し角度をつけた状態で、１人称解除
        }

        // １人称にした瞬間にプレイヤーの向きを１人称の向き（カメラの向き）にそろえる
        Vector3 firstPlayerForward;
        firstPlayerForward = transform.forward;
        firstPlayerForward.y = 0;
        Player.transform.forward = firstPlayerForward;
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
            GazeEnemyList = _lockOn.nearEnemyList;
        }

        //プレイヤーとの距離が一番近い敵のlistを常に保持
        realTimeEnemyList = _lockOn.enemyListDistance;



        // ロックオン対象切り替え
        if (Input.GetButtonDown("L1"))
        {
            // リストがL1をおす前後と異なるかをチェック
            if (!ListsAreEqual(realTimeEnemyList, GazeEnemyList))
            {
                // listHasChanged = true;

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
                // listHasChanged = false;
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

