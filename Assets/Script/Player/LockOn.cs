using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{

    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Camera;
    [SerializeField] private GameObject LockOnCol;

    LockOnCol _lockOnCol;
    public bool isLockOn;
    public List<GameObject> enemyListDistance = new List<GameObject>(); // エネミーのリスト
    public List<GameObject> nearEnemyList = new List<GameObject>();
    //ロックオンを押したときの敵の距離順のlist　enemyListDistanceをそのまま使ってしまうと勝手にロックオン対象が距離に応じて切り替わるために使用
    Vector3 playerPosition;



    void Start()
    {
        _lockOnCol = LockOnCol.GetComponent<LockOnCol>();
    }

    // Update is called once per frame
    void Update()
    {

        enemyListDistance = new List<GameObject>(_lockOnCol.enemyListResult);

        playerPosition = Player.transform.position;

        // 距離と角度をかけ合わせたものに基づいてエネミーをソート　同じ距離ならプレイヤー（厳密にはカメラ）との角度を小さい方を近い方とする
        enemyListDistance.Sort((a, b) =>
        {
            // 単純に距離だけでソートする場合
            // float distanceA = getDistance(a.transform.position, Player.transform.position);
            // float distanceB = getDistance(b.transform.position, Player.transform.position);

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
            isLockOn = !isLockOn;
            nearEnemyList = new List<GameObject>(enemyListDistance);
        }

        if (!isLockOn)
        {
            nearEnemyList.Clear();
        }

        // listから敵がいなくなった場合（プレイヤーの離脱、敵の撃破）
        if (enemyListDistance.Count == 0)
        {
            isLockOn = false;
            nearEnemyList.Clear();
        }
    }


    float getDistance(Vector3 a, Vector3 b)
    {
        Vector3 dv = a - b;
        return dv.magnitude;
    }
}
