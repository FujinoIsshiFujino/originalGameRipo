using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;



// 落下にまつわる処理をまとめたpartialクラス
public partial class PlayerControl
{
    public float rayLength = 1.5f; // レイの長さ
    public float rayLengthDetemine = 0.5f; // レイの長さ
    private float pushForce = 5f; // プレイヤーを押し出す力
    public bool isLastGroundPosiForDown = true;

    // 指定角度以上の斜面だと押し出される処理　崖の斜面に立たないために使用
    [System.NonSerialized]
    public Vector3 groundNormal = Vector3.zero;
    private Vector3 lastGroundNormal = Vector3.zero;
    [System.NonSerialized]
    public Vector3 lastHitPoint = new Vector3(Mathf.Infinity, 0, 0);
    protected float groundAngle = 0;
    public bool isOverLimitSlope;

    // 現在接地している地面の法線ベクトルを取得
    void MotorOnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal.y > 0 && hit.moveDirection.y < 0)
        {
            if ((hit.point - lastHitPoint).sqrMagnitude > 0.001f || lastGroundNormal == Vector3.zero)
            {
                groundNormal = hit.normal;
            }
            else
            {
                groundNormal = lastGroundNormal;
            }

            lastHitPoint = hit.point;
        }

        // 現在の接地面の角度を取得
        groundAngle = Vector3.Angle(hit.normal, Vector3.up);

        // 指定角度以下の斜面に接している場合は復帰地点としない
        if (groundAngle <= characterController.slopeLimit)
        {
            isOverLimitSlope = false;
        }
        else
        {
            isOverLimitSlope = true;
        }
    }

    // 斜面に立っている際に閾値以上の場合は押し出す
    private void HandleSlopeSliding()
    {
        if (IsGrounded() && characterController.slopeLimit <= groundAngle)
        {
            float slidingSpeed = 50f;
            moveDirection.x += groundNormal.x * slidingSpeed;
            moveDirection.y = -groundNormal.y * slidingSpeed;
            moveDirection.z += groundNormal.z * slidingSpeed;

            characterController.Move(moveDirection * Time.deltaTime);
        }
    }

    //崖際ぎりぎりに復帰しないための処理
    public void DetermaineLastPosi()
    {
        // 閾値以上のときには復帰地点を更新しない
        if (!isOverLimitSlope)
        {
            Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; // プレイヤーの位置から少し上にレイを飛ばす
            bool allSlopesBelowThreshold = true; // 全ての斜面の角度が閾値以下であるかどうか

            // プレイヤーを中心として円状にRayを発射する
            for (int i = 0; i < 12; i++)
            {
                if (!allSlopesBelowThreshold) break; // 既にfalseならチェックをスキップ

                float angle = i * 30f;
                Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward * 2; //2はcircleの大きさの調整
                if (Physics.Raycast(rayOrigin + direction, Vector3.down * rayLengthDetemine, rayLengthDetemine))
                {
                    // Debug.DrawRay(rayOrigin + direction, Vector3.down * rayLengthDetemine, Color.blue); // レイをデバッグ表示
                    allSlopesBelowThreshold = true;
                }
                else
                {
                    // Rayが何にも当たらなかった場合
                    // Debug.DrawRay(rayOrigin + direction, Vector3.down * rayLengthDetemine, Color.red); // レイをデバッグ表示
                    allSlopesBelowThreshold = false; // Rayが何にも当たらなかった場合もfalseに設定
                }
            }
            isLastGroundPosiForDown = allSlopesBelowThreshold;
        }
    }
}
