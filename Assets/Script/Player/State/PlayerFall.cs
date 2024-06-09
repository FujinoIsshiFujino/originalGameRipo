using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;



// 落下にまつわる処理をまとめたpartialクラス
public partial class PlayerControl
{

    public float slopeAngleThreshold = 45f; // 斜面と判定する角度の閾値
    public float rayLength = 1.5f; // レイの長さ
    public float rayLengthDetemine = 0.5f; // レイの長さ
    private float pushForce = 5f; // プレイヤーを押し出す力
    public bool isLastGroundPosiForDown = true;

    // 指定角度以上の斜面だと押し出される処理　崖の斜面に立たないために使用
    public void SlopePush()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; // プレイヤーの位置から少し上にレイを飛ばす

        // プレイヤーを中心として円状にRayを発射する
        for (int i = 0; i < 4; i++)
        {
            float angle = i * 90f;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward * 0.3f; //2はcircleの大きさの調整
            RaycastHit hitGrounCircle;
            if (Physics.Raycast(rayOrigin + direction, Vector3.down, out hitGrounCircle, rayLength))
            {
                Vector3 groundCircleNormal = hitGrounCircle.normal;
                float slopeAngle = Vector3.Angle(groundCircleNormal, Vector3.up);
                Debug.DrawRay(rayOrigin + direction, Vector3.down * rayLength, Color.green); // レイをデバッグ表示

                if (slopeAngle >= slopeAngleThreshold)
                {
                    // 指定角度以上の斜面に接している場合の処理
                    if (slopeAngle >= 45f)
                    {
                        // 斜面の法線ベクトル方向にプレイヤーを押し出す処理

                        characterController.enabled = false;
                        // 斜面の法線ベクトル方向にプレイヤーを押し出す
                        Vector3 pushDirection = Vector3.ProjectOnPlane(-groundCircleNormal, Vector3.up).normalized;
                        pushDirection.y = 0;
                        GetComponent<Rigidbody>().AddForce(-pushDirection * pushForce, ForceMode.Force);
                        break; // 1つでも斜面に接している場合はループを終了する
                    }
                }
                else
                {
                    // 指定角度以下の場合
                    characterController.enabled = true;
                }
            }
            else
            {
                // Rayが何にも当たらなかった場合
                Debug.DrawRay(rayOrigin + direction, Vector3.down * rayLength, Color.red); // レイをデバッグ表示
                characterController.enabled = true;
            }
        }
    }

    //崖際ぎりぎりに復帰しないための処理
    public void DetermaineLastPosi()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; // プレイヤーの位置から少し上にレイを飛ばす
        bool allSlopesBelowThreshold = true; // 全ての斜面の角度が閾値以下であるかどうか

        // プレイヤーを中心として円状にRayを発射する
        for (int i = 0; i < 12; i++)
        {
            if (!allSlopesBelowThreshold) break; // 既にfalseならチェックをスキップ

            float angle = i * 30f;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward * 2; //2はcircleの大きさの調整
            RaycastHit hitGroundCircle;
            if (Physics.Raycast(rayOrigin + direction, Vector3.down, out hitGroundCircle, rayLengthDetemine))
            {
                // 指定角度以下の斜面に接している場合は復帰地点としない
                Vector3 groundCircleNormal = hitGroundCircle.normal;
                float slopeAngle = Vector3.Angle(groundCircleNormal, Vector3.up);
                Debug.DrawRay(rayOrigin + direction, Vector3.down * rayLengthDetemine, Color.blue); // レイをデバッグ表示

                if (slopeAngle >= slopeAngleThreshold)
                {
                    // 指定角度以上の斜面に接している場合は復帰地点としない
                    allSlopesBelowThreshold = false; // 条件を満たした場合にfalseに設定
                }
            }
            else
            {
                // Rayが何にも当たらなかった場合
                Debug.DrawRay(rayOrigin + direction, Vector3.down * rayLengthDetemine, Color.red); // レイをデバッグ表示
                allSlopesBelowThreshold = false; // Rayが何にも当たらなかった場合もfalseに設定
            }
        }
        isLastGroundPosiForDown = allSlopesBelowThreshold;

        if (isLastGroundPosiForDown)
        {
            PlayerSideCircleRay();
        }
    }

    public int raysNum = 10; // Rayの本数
    public float circleRadius = 1f; // 円の半径
    public bool isLastGroundPosiForSide = true;

    // 45度以上の斜面ににたっているはずなのに、なぜか法線ベクトルが0,1,0で接地角度も0度の時、プレイヤーの周りの壁を調査
    //DetermaineLastPosiと併用して復帰地点を決める
    public void PlayerSideCircleRay()
    {
        // プレイヤーの位置を中心として円状にRayを発射する
        for (int i = 0; i < raysNum; i++)
        {
            // 角度を計算
            float angle = i * 360f / raysNum;
            // 極座標からワールド座標へ変換
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            // Rayを発射
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0, transform.localScale.y, 0), direction, out hit, circleRadius))
            {
                // Rayが何かに当たった場合の処理
                Debug.DrawLine(transform.position + new Vector3(0, transform.localScale.y, 0), hit.point, Color.red);
                isLastGroundPosiForSide = false;
            }
            else
            {
                isLastGroundPosiForSide = true;
                // Rayが何にも当たらなかった場合の処理
                Debug.DrawRay(transform.position + new Vector3(0, transform.localScale.y, 0), direction * circleRadius, Color.green);
            }
        }
    }
}
