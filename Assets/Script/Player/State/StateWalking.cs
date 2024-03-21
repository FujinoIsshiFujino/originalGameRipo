using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControl
{
    [SerializeField] private float dashSpeed = 1.75f;
    bool isDash;


    public class StateWalking : PlayerStateBase
    {

        public GameObject Camera;
        public Vector3 moveDirection;//確認用にpublic


        public override void OnEnter(PlayerControl owner, PlayerStateBase preState)
        {
            Camera = owner.Camera;

        }

        public override void OnUpdate(PlayerControl owner)
        {



            // //方向の入力に応じて動く方向を決める
            //クラス全体としては常に自由落下がかかり続けて接地ができているので、そこを上書きしてはいけない　ここのmoveDirectionはこのステイトのみのパラメーターとおもったほうがいい
            moveDirection = owner.cameraForward * owner.inputVertical + Camera.transform.right * owner.inputHorizontal;

            // owner.moveDirection = moveDirection + new Vector3(0, owner.moveDirection.y, 0);
            //基本的にはinputなどを参照するので不要かもしれないが、一応moveDirectionのownerへの反映。
            //クラス全体としてはステイトチェンジした後に、moveDirectionをつかっていないので影響はない。

            //斜め移動時はベクトルの長さで成分を割って、単位ベクトル化
            if (moveDirection.magnitude >= 1)
            {
                moveDirection /= moveDirection.magnitude;
            }



            // //　ダッシュの速度upとジャンプのベクトル決め
            if (owner.isGrounded)
            {
                //     // if (_status.IsMovable)
                //     // {



                //ダッシュ処理
                if (Input.GetButton("Dash"))
                {

                    owner.isDash = true;
                    moveDirection.x *= owner.dashSpeed;
                    moveDirection.z *= owner.dashSpeed;

                }
                else //ダッシュしてないときの処理
                {
                    owner.isDash = false;

                }

                if (!owner._cameraFollow.isFirstPerson) //1人称視点の際に、カニ歩きになるように、isFirstPersonを監視
                {
                    //         //動く方向を向く transform.LookAtは引数に指定した位置をむく
                    owner.transform.LookAt(owner.transform.position + new Vector3(moveDirection.x, 0, moveDirection.z));
                }





                // if (Input.GetButtonDown("Attack"))
                // {
                //     _attack.AttackIfPossible();
                //     waitTime = 0;//攻撃が完了したことが後に実装されれば、 bool作って、あとのwai関数で時間んを０に戻したほうがきれい
                // }








                owner.moveDirection = moveDirection + new Vector3(0, owner.moveDirection.y, 0);
                // owner.moveDirection = moveDirection;
                owner.characterController.Move(owner.moveDirection * Time.deltaTime * owner.moveSpeed);
                //owner._animator.SetFloat("Speed", moveDirection.magnitude);

                if (owner.inputHorizontal != 0 || owner.inputVertical != 0)
                {
                    owner._animator.SetFloat("Speed", moveDirection.magnitude);
                }









                //Jumpは個々の位置におかないとうまくステイトチェンジしない

                if (owner.isGrounded)
                {
                    if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
                    {
                        owner.ChangeState(stateIdle);
                    }



                    if (Input.GetButton("Dash"))
                    {
                        if (Input.GetButtonDown("Jump") && owner.jumpCount < 1)
                        {
                            owner.isDashJump = true;
                            owner.ChangeState(stateJumping);
                        }
                    }
                    else
                    {
                        if (Input.GetButtonDown("Jump") && owner.jumpCount < 1)
                        {
                            owner.ChangeState(stateJumping);
                        }
                    }

                    if (!owner._cameraFollow.isFirstPerson)
                    {
                        if (Input.GetButtonDown("Make"))
                        {
                            owner._makeMchineUI.makeMchineUI();
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

                    if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                    {
                        if (owner.currentState is not StateMaking)
                        {
                            if (Input.GetButtonDown("Rotate"))
                            {
                                owner._animator.SetTrigger("Roll");
                                owner.ChangeState(stateRolling);
                            }
                        }
                    }
                }







            }


        }

    }
}
