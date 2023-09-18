using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotation : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float rotationSpeed = 5.0f; // 回転の速度
    bool rotationStart;

    private Quaternion targetRotation; // 目標の回転

    void Update()
    {
        if (Input.GetButtonDown("Dash"))
        {
            rotationStart = !rotationStart;
        }
        if (rotationStart)
        {
            // ターゲットの方向を向く回転を計算
            Vector3 directionToTarget = target.transform.position - transform.position;
            directionToTarget.y = 0;
            Quaternion rotationWithoutZ = Quaternion.LookRotation(directionToTarget);

            // Z軸の回転を0度に固定
            targetRotation = Quaternion.Euler(rotationWithoutZ.eulerAngles.x, rotationWithoutZ.eulerAngles.y, 0);
            transform.rotation = targetRotation;

            // // 回転を補間して滑らかに行う
            // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
