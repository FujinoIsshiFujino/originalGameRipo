using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{

    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Camera;
    [SerializeField] private GameObject LockOnCol;

    public GameObject[] enemyList;  //エネミーの配列
    Vector3 playePposition;  //プレイヤーの座標
    bool isLockOn = false;
    Vector3 cameraForward;
    LockOnCol _lockOnCol;
    CameraFollow _CameraFollow;




    // Start is called before the first frame update
    void Start()
    {
        _lockOnCol = LockOnCol.GetComponent<LockOnCol>();
        _CameraFollow = Camera.GetComponent<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_lockOnCol.isLockOn)
        {
            // _CameraFollow.enabled = false;


            // Debug.Log("aaaaa");
        }
    }
}
