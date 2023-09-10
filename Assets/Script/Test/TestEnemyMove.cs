using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyMove : MonoBehaviour
{
    public Transform startPoint;  // 開始位置
    public Transform endPoint;    // 終了位置
    public float speed = 2.0f;    // 移動速度

    private Vector3 nextPosition;
    private bool movingToEnd = true;

    void Start()
    {
        if (startPoint == null || endPoint == null)
        {
            Debug.LogError("Start Point and End Point must be assigned!");
            enabled = false;  // スクリプトを無効にする
            return;
        }

        transform.position = startPoint.position;
        nextPosition = endPoint.position;
    }

    void Update()
    {
        float step = speed * Time.deltaTime;

        // 移動方向を設定
        if (movingToEnd)
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, step);
        else
            transform.position = Vector3.MoveTowards(transform.position, startPoint.position, step);

        // 目的地に到達した場合、方向を切り替える
        if (Vector3.Distance(transform.position, nextPosition) < 0.001f)
        {
            movingToEnd = !movingToEnd;
            if (movingToEnd)
                nextPosition = endPoint.position;
            else
                nextPosition = startPoint.position;
        }
    }
}
