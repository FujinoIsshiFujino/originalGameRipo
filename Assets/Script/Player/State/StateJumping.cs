using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class PlayerControl
{
    public bool isJump;
    int jumpCount;

    public class StateJumping : PlayerStateBase
    {

        float beforeJumpInputHorizontal;
        float beforeJumpInputVertical;
        Vector3 jumpDirection;
        float jumpFoarwardPower = 1.5f;
        Vector3 moveDirection;
        public override void OnEnter(PlayerControl owner, PlayerStateBase preState)
        {



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

                // _animator.SetBool("Jump", true);


                owner._animator.SetTrigger("Jump");
            }





        }
        public override void OnUpdate(PlayerControl owner)
        {


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
                owner._animator.SetFloat("Speed", 0);
            }

        }

        public override void OnExit(PlayerControl owner, PlayerStateBase nextState)
        {
            //カウントはここで０にするのか、こりじょんひっとで０にするのか
            //moveDirectionのｙがなぜ０にならないのかわはよくわからない


            owner._animator.SetTrigger("Idle");

            owner.isDashJump = false;
            owner.isJump = false;

            // owner.ChangeState(stateIdle);
        }
    }


}
