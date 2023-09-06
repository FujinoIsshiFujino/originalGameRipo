using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmicCameraMove : MonoBehaviour
{
    [SerializeField] GameObject Camera;
    [SerializeField] GameObject Player;
    CameraFollow _cameraFollow;
    PlayerController _playerController;
    CharacterController _characterController;
    [SerializeField] float gimmicStopSecond; 
    [SerializeField] float returnStopSecond; 
    public float elapseTime;

    public bool cameraMove;
    public bool isCameraMoveEnd;
    public enum moveType
        {
            moment,
            slowly
        }

    [SerializeField]  float smoothnessFactor;
    Vector3 targetPosition;
    Vector3 beforeCameraPosi;
    Vector3 beforeCameraVec;
    [SerializeField ]moveType selectedType;


    // Start is called before the first frame update
    void Start()
    {
        // 本当は範囲にはいったら取得するぐらいでいい
        _cameraFollow= Camera.GetComponent<CameraFollow>();
        _characterController = Player.GetComponent<CharacterController>();
        _playerController = Player.GetComponent<PlayerController>();

        //isCameraMoveEnd = false; //　これは初期化しなくていいかも
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("9")) // ここはフラグになる
        {
            cameraMove = true;
            targetPosition = this.transform.position;

            beforeCameraPosi = Camera.transform.position;
            beforeCameraVec = Camera.transform.forward;
        }


        if(cameraMove)
        {

            _cameraFollow.enabled = false;
            _characterController.enabled = false;
            _playerController.enabled = false;

            
           

            if(selectedType == moveType.slowly)
            {
                 Camera.transform.position = Vector3.Lerp(Camera.transform.position, targetPosition, Time.deltaTime * smoothnessFactor);
                 Camera.transform.forward = this.transform.forward;

                if(Vector3.Distance(transform.position, targetPosition) < 0.01f) // 厳密な座標の一致は難しいため
                {

                    elapseTime += Time.deltaTime;

                    if(gimmicStopSecond <= elapseTime)
                    {

                        isCameraMoveEnd = true;
                        cameraMove =  false;
                        // _cameraFollow.enabled = true;
                        // _characterController.enabled = true;
                        // _playerController.enabled = true;
                        //transform.position = beforeCameraPosi;
                        elapseTime = 0; // 試作用にタイムは元に戻しているけど、フラグの一回きりだったらいらないかも
                    }

                }
            }
            else if(selectedType == moveType.moment)
            {
                Camera.transform.position = this.transform.position;
                Camera.transform.forward = this.transform.forward;

                if(Vector3.Distance(Camera.transform.position, targetPosition) < 0.01f) // 厳密な座標の一致は難しいため
                {
                    elapseTime += Time.deltaTime;

                    if(gimmicStopSecond <= elapseTime)
                    {

                        //isCameraMoveEnd = true;
                        cameraMove =  false;
                        _cameraFollow.enabled = true;
                        _characterController.enabled = true;
                        _playerController.enabled = true;
                         // transform.position = beforeCameraPosi;
                         elapseTime = 0; // 試作用にタイムは元に戻しているけど、フラグの一回きりだったらいらないかも

                    }

                }

            }


        }

            if(isCameraMoveEnd)
            {

                if(selectedType == moveType.slowly)
                {
                    Camera.transform.position = Vector3.Lerp(Camera.transform.position, beforeCameraPosi, Time.deltaTime * smoothnessFactor);
                    Camera.transform.forward =  beforeCameraVec;
                    if(Vector3.Distance(Camera.transform.position, beforeCameraPosi) < 0.01f) // 厳密な座標の一致は難しいため
                    {

                         elapseTime += Time.deltaTime;

                        if(returnStopSecond <= elapseTime)
                        {

                            isCameraMoveEnd = true;
                            _cameraFollow.enabled = true;
                            _characterController.enabled = true;
                            _playerController.enabled = true;
                            elapseTime = 0; // 試作用にタイムは元に戻しているけど、フラグの一回きりだったらいらないかも
                            isCameraMoveEnd = false;
                        }

                    }
                }

            }

    }
}
// 
