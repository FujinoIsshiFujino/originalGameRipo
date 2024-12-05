using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class PlayerControl
{
    public bool isJump;
    float groundtime;
    int jumpCount;
    public bool isJumpRayGrounded = true;
    [SerializeField] float isJumpRayCheckDistance2;
    public class StateJumping : PlayerStateBase
    {

        float beforeJumpInputHorizontal;
        float beforeJumpInputVertical;
        Vector3 jumpDirection;
        float jumpFoarwardPower = 1.5f;
        Vector3 moveDirection;
        public override void OnEnter(PlayerControl owner, PlayerStateBase preState)
        {

            owner.isJumpRayGrounded = false;

            if (owner.jumpCount < 1)
            {
                owner.isJump = true;
                owner.jumpCount++;
                moveDirection = new Vector3(0, 0, 0);

                //入力方向にジャンプ方向を定める
                jumpDirection = (owner.cameraForward * owner.inputVertical + owner.Camera.transform.right * owner.inputHorizontal).normalized;


                beforeJumpInputHorizontal = owner.inputHorizontal;
                beforeJumpInputVertical = owner.inputVertical;


                jumpDirection.y = 0;
            }

        }
        public override void OnUpdate(PlayerControl owner)
        {

            owner.IsJumpRayGroundedDtermine();

            if (owner.isJump)
            {
                moveDirection.x = 0;
                moveDirection.z = 0;

                // ダッシュ中にジャンプしたとき
                if (owner.isDashJump)
                {
                    moveDirection += jumpDirection * jumpFoarwardPower * 2;
                    moveDirection *= Time.deltaTime;

                }
                else
                {
                    // スティックを少し倒している時
                    if (Mathf.Abs(beforeJumpInputHorizontal) <= 0.5 && beforeJumpInputHorizontal != 0
                    || Mathf.Abs(beforeJumpInputVertical) <= 0.5 && beforeJumpInputVertical != 0)
                    {
                        moveDirection += jumpDirection * jumpFoarwardPower * 0.7f;
                        moveDirection *= Time.deltaTime;
                    }
                    // スティックを上記より倒している時
                    if (Mathf.Abs(beforeJumpInputHorizontal) >= 0.5 || Mathf.Abs(beforeJumpInputVertical) >= 0.5)
                    {
                        moveDirection += jumpDirection * jumpFoarwardPower * 1.4f;
                        moveDirection *= Time.deltaTime;

                        // 距離の調整
                        // if(moveDirection.magnitude >=1)
                        //     {
                        //         moveDirection/=moveDirection.magnitude ;
                        //     }

                    }

                }







                // moveDirection.y = jumpForce *Time.deltaTime;


                moveDirection.y = (float)owner.jumpPower * Time.deltaTime;//クラス全体としては自由落下がかかり続けているので、このクラスには上昇のみ記述

                // 速さ＝初速＋加速×時間　の速さの公式を使っている。接地していあにときは、常に重力（加速×時間）をかけているので、ジャンプの時だけ初速加える

                owner.moveDirection = moveDirection + new Vector3(0, owner.moveDirection.y, 0);
                owner.characterController.Move(moveDirection);
            }

        }

        public override void OnExit(PlayerControl owner, PlayerStateBase nextState)
        {
            //カウントはここで０にするのか、こりじょんひっとで０にするのか
            //moveDirectionのｙがなぜ０にならないのかわはよくわからない



            owner.isDashJump = false;
            owner.isJump = false;

            owner._animator.SetBool("JumpBool", false);
        }
    }

    void IsJumpRayGroundedDtermine()
    {
        groundtime += Time.deltaTime;
        if (groundtime >= 0.3f)
        {
            // キャラクター直下にレイを発射すると、characterControllerは地面に接地しているのに、rayは接地していないという現象が起こるので、
            // characterControllerの円周上から下方向にrayを発射して、地面の接地を一致させている。
            // characterController.isGroundedは下方向以外にも接地判定があってしまうので、例えば空中で壁などにキャラクターが当たっても
            // 接地判定されてしまうので、下方向にrayを打つ必要があったが、上記の理由によりcharacterController.radiusの円周上に配置
            for (int i = 0; i < rayCount; i++)
            {
                // 円周上の角度を計算
                float angle = 360f / rayCount * i;
                // 円周上の座標を計算
                Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));
                Vector3 rayOrigin = transform.position + direction * radius;

                // RaycastHitを作成
                RaycastHit hit;

                // Rayを発射
                if (Physics.Raycast(rayOrigin, Vector3.down, out hit, isJumpRayCheckDistance2))
                {
                    isJumpRayGrounded = true;
                    Debug.Log($"Ray {i}: Hit Object Naame = {hit.collider.gameObject.name}");

                    // 1つでもヒットしたらループを終了
                    break;
                }
                else
                {
                    isJumpRayGrounded = false;
                }

                // Rayをデバッグ表示
                Debug.DrawRay(rayOrigin, Vector3.down * isJumpRayCheckDistance2, isJumpRayGrounded ? Color.black : Color.gray);
            }
        }
    }
}
