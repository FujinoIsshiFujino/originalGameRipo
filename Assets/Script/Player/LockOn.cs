using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{

    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Camera;
    [SerializeField] private GameObject LockOnCol;

    // public GameObject[] enemyList;  //エネミーの配列
    Vector3 playePposition;  //プレイヤーの座標
    bool isLockOn = false;
    Vector3 cameraForward;
    LockOnCol _lockOnCol;
    CameraFollow _CameraFollow;
    public bool isButtonLock;
    public List<GameObject> enemyListDistance = new List<GameObject>(); // エネミーのリスト
    Vector3 playerPosition;
    public List<GameObject> previousList = new List<GameObject>();
    //ロックオンを押したときの敵の距離準のlist　enemyListDistanceをそのまま使ってしまうと勝手にロックオン対象が距離に応じて切り替わるために仕様






    // Start is called before the first frame update
    void Start()
    {
        _lockOnCol = LockOnCol.GetComponent<LockOnCol>();
        _CameraFollow = Camera.GetComponent<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {

        enemyListDistance = new List<GameObject>(_lockOnCol.enemyListResult);



        playerPosition = Player.transform.position;

        // if (isButtonLock)
        // {
        //     _CameraFollow.enabled = false;


        // 距離と角度をかけ合わせたものに基づいてエネミーをソート
        enemyListDistance.Sort((a, b) =>
        {
            // float distanceA = getDistance(a.transform.position, Player.transform.position);
            // float distanceB = getDistance(b.transform.position, Player.transform.position);

            // // 昇順でソートする場合（近い順）
            // return distanceA.CompareTo(distanceB);

            // プレイヤーとエネミーの位置ベクトルを取得
            Vector3 positionA = a.transform.position;
            Vector3 positionB = b.transform.position;

            // プレイヤーからエネミーへのベクトルを計算
            Vector3 vectorA = positionA - playerPosition;
            Vector3 vectorB = positionB - playerPosition;

            // 角度を計算
            float angleA = Vector3.Angle(vectorA, Camera.transform.forward);
            float angleB = Vector3.Angle(vectorB, Camera.transform.forward);

            // 距離を計算
            float distanceA = getDistance(positionA, playerPosition);
            float distanceB = getDistance(positionB, playerPosition);

            // 角度と距離を掛け合わせて比較
            float scoreA = angleA * distanceA;
            float scoreB = angleB * distanceB;

            // 昇順でソートする場合（小さい順）
            return scoreA.CompareTo(scoreB);
        });

        if (Input.GetButtonDown("Lock") || Input.GetKeyDown("r"))
        {
            isButtonLock = !isButtonLock;
            previousList = new List<GameObject>(enemyListDistance);
        }

        if (isButtonLock)
        {
            // _CameraFollow.enabled = false;
            // Camera.transform.forward = (enemyListDistance[0].transform.position - Camera.transform.position).normalized;
        }
        else
        {
            _CameraFollow.enabled = true;
            previousList.Clear();
        }

        // listから敵がいなくなった場合（プレイヤーの離脱、敵の撃破）
        if (enemyListDistance.Count == 0)
        {
            isButtonLock = false;
            previousList.Clear();
        }
    }


    float getDistance(Vector3 a, Vector3 b)
    {
        Vector3 dv = a - b;
        return dv.magnitude;
    }
}
