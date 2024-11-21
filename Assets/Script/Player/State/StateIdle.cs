using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class PlayerControl
{
    [SerializeField] MenuBase _menuBase;
    float waitTime;
    public class StateIdle : PlayerStateBase
    {
        float inputTime;
        public override void OnUpdate(PlayerControl owner)
        {

            if (owner.isRayGrounded)
            {
                if (Input.GetButtonDown("Jump") && owner.jumpCount < 1)
                {
                    owner._animator.SetBool("JumpBool", true);
                    owner.ChangeState(stateJumping);
                }


                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    owner.ChangeState(stateWalking);
                }

                if (owner.currentState is not StateMaking)
                {
                    if (Input.GetButton("Rotate"))
                    {
                        inputTime += Time.deltaTime;
                        if (inputTime < 1)
                        {
                            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                            {
                                inputTime = 0;

                                owner._animator.SetTrigger("Roll");
                                owner.ChangeState(stateRolling);

                            }
                        }
                    }
                }

                if (!owner._cameraFollow.isFirstPerson)
                {
                    if (Input.GetButtonDown("Make"))
                    {
                        owner._menuBase.OpenMenu(owner.recipeDialog);
                        // recipeDialogをひらくと、同階層のunity上の他のメニューまで開いてしまうのでsetActiveでfalseにする
                        owner.mainMenuPanel.SetActive(false);
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

        if (inputHorizontal == 0 && inputVertical == 0 && !Input.anyKey)
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