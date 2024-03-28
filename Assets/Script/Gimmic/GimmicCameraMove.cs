using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmicCameraMove : MonoBehaviour
{
    [SerializeField] GameObject Camera;
    [SerializeField] GameObject Player;
    CameraFollow _cameraFollow;
    PlayerControl _playerControl;
    CharacterController _characterController;
    [SerializeField] float gimmicStopSecond;
    [SerializeField] float returnStopSecond;//カメラが元の位置に戻ってからの静止時間
    public float elapseTime;
    public bool cameraMove;
    public bool isCameraMoveEnd;
    public enum moveType
    {
        moment,
        slowly
    }

    [SerializeField] float smoothnessFactor = 7;
    Vector3 targetPosition;
    Vector3 beforeCameraPosi;
    Vector3 beforeCameraVec;
    [SerializeField] moveType selectedType;
    [SerializeField] float slowlyPositionMatchWidth = 0.5f;
    [SerializeField] float momentPositionMatchWidth = 0.01f;
    FlagCaller flagCaller;
    public bool firstTime;


    // Start is called before the first frame update
    void Start()
    {
        // 本当は範囲にはいったら取得するぐらいでいい
        _cameraFollow = Camera.GetComponent<CameraFollow>();
        _characterController = Player.GetComponent<CharacterController>();
        _playerControl = Player.GetComponent<PlayerControl>();

        //isCameraMoveEnd = false; //　これは初期化しなくていいかも

        firstTime = false;
    }

    // Update is called once per frame
    void Update()
    {
        flagCaller = GetComponent<FlagCaller>();

        if (firstTime == false)
        {
            if (flagCaller.isOn)
            {

                cameraMove = true;
                targetPosition = this.transform.position;

                beforeCameraPosi = Camera.transform.position;
                beforeCameraVec = Camera.transform.forward;

                firstTime = true;
            }
        }

        //カメラが動いている途中
        if (cameraMove)
        {
            _cameraFollow.enabled = false;
            _characterController.enabled = false;
            _playerControl.enabled = false;

            if (selectedType == moveType.slowly)
            {
                Camera.transform.position = Vector3.Lerp(Camera.transform.position, targetPosition, Time.deltaTime * smoothnessFactor);
                Camera.transform.forward = this.transform.forward;

                if (Vector3.Distance(Camera.transform.position, targetPosition) < slowlyPositionMatchWidth) // 厳密な座標の一致は難しいため
                {
                    cameraStopToReturn(false);
                }
            }
            else if (selectedType == moveType.moment)
            {
                Camera.transform.position = this.transform.position;
                Camera.transform.forward = this.transform.forward;

                if (Vector3.Distance(Camera.transform.position, targetPosition) < momentPositionMatchWidth) // 厳密な座標の一致は難しいため
                {
                    cameraStopToReturn(false);
                }
            }
        }

        //場合によって戻った後に硬直させたくないとき、もしくは場合によって硬直秒数を変えたいときはここに、なにか条件をつけ足す
        //カメラがプレイヤーに戻った後も止めたい場合
        if (isCameraMoveEnd)
        {
            _cameraFollow.enabled = false;
            _characterController.enabled = false;
            _playerControl.enabled = false;

            if (selectedType == moveType.slowly)
            {
                Camera.transform.position = Vector3.Lerp(Camera.transform.position, beforeCameraPosi, Time.deltaTime * smoothnessFactor);
                Camera.transform.forward = beforeCameraVec;
            }
            else if (selectedType == moveType.moment)
            {
                Camera.transform.position = beforeCameraPosi;
                Camera.transform.forward = beforeCameraVec;
            }

            if (Vector3.Distance(Camera.transform.position, beforeCameraPosi) < 0.01f) // 厳密な座標の一致は難しいため
            {
                cameraStopToReturn(true);
            }
        }
    }

    //カメラが定位置に止まってからまた動き出すまでの処理
    void cameraStopToReturn(bool isReturnOrGo)
    {
        elapseTime += Time.deltaTime;
        if (isReturnOrGo)
        {
            //戻ってから止まる処理
            if (returnStopSecond <= elapseTime)
            {
                _cameraFollow.enabled = true;
                _characterController.enabled = true;
                _playerControl.enabled = true;
                elapseTime = 0; // 試作用にタイムは元に戻しているけど、フラグの一回きりだったらいらないかも
                isCameraMoveEnd = false;
            }
        }
        else
        {
            //ギミック前で止まる処理
            if (gimmicStopSecond <= elapseTime)
            {
                isCameraMoveEnd = true;
                cameraMove = false;
                _cameraFollow.enabled = true;
                _characterController.enabled = true;
                _playerControl.enabled = true;

                elapseTime = 0; // 試作用にタイムは元に戻しているけど、フラグの一回きりだったらいらないかも
            }
        }
    }
}
