using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class PlayerControl
{
    float waitTime;
    public class StateIdle : PlayerStateBase
    {

        public override void OnUpdate(PlayerControl owner)
        {

            if (owner.isGrounded)
            {
                if (Input.GetButtonDown("Jump") && owner.jumpCount < 1)
                {
                    owner.ChangeState(stateJumping);
                }


                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    owner.ChangeState(stateWalking);
                }

                if (!owner._cameraFollow.isFirstPerson)
                {
                    if (Input.GetButtonDown("Make"))
                    {
                        owner.ChangeState(stateMaking);
                        owner._lockOnCol.isLockOn = false;
                    }
                }

                if (owner.isRedyAttack)
                {
                    if (Input.GetButtonDown("Attack"))
                    {
                        owner.ChangeState(stateAttacking);
                        owner._animator.SetTrigger("Attack");
                    }
                }

            }





            owner._animator.SetTrigger("Idle");
            owner.WaitPose(owner.inputHorizontal, owner.inputVertical);

        }




    }
    private void WaitPose(float inputHorizontal, float inputVertical)
    {

        if (inputHorizontal == 0 && inputVertical == 0)
        {
            waitTime += Time.deltaTime;
        }
        else
        {
            waitTime = 0;
        }

        if (waitTime >= 3f)
        {
            _animator.SetBool("Rest", true);
        }
        else
        {
            _animator.SetBool("Rest", false);
        }
    }
}