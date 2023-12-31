using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class PlayerControl
{
    //これやりながら走れてていまうので意味ない？メイキングから遷移できなくて歩きから遷移できるものがあるので、意味はありそう。
    //ならこのステートにも歩けるようにいしなきゃってなるけど、接地のときのやつどうしようってなる

    [SerializeField] public GameObject prefabToInstantiate;
    public bool isMake;
    public bool makeEnd;
    public class StateMaking : PlayerStateBase
    {
        public GameObject Camera;


        public Vector3 moveDirection;

        public override void OnEnter(PlayerControl owner, PlayerStateBase preState)
        {
            Camera = owner.Camera;

            owner.isMake = true;
            owner.makeEnd = false;
            Quaternion playerRotation = owner.transform.rotation;

            // プレハブからクローンを生成
            Instantiate(owner.prefabToInstantiate, owner.transform.position + owner.transform.forward * 8 + new Vector3(0, 1, 0), playerRotation);

        }
        public override void OnUpdate(PlayerControl owner)
        {
            owner.transform.forward = owner.cameraForward;

            //StateWalkingと共通

            // //方向の入力に応じて動く方向を決める
            //クラス全体としては常に自由落下がかかり続けて接地ができているので、そこを上書きしてはいけない　ここのmoveDirectionはこのステイトのみのパラメーターとおもったほうがいい
            moveDirection = owner.cameraForward * owner.inputVertical + Camera.transform.right * owner.inputHorizontal;


            //斜め移動時はベクトルの長さで成分を割って、単位ベクトル化
            if (moveDirection.magnitude >= 1)
            {
                moveDirection /= moveDirection.magnitude;
            }






            if (Input.GetButton("First"))
            {

                moveDirection = new Vector3(0, 0, 0);
            }

            if (Input.GetButton("Rotate"))
            {

                moveDirection = new Vector3(0, 0, 0);
            }

            //設置可能状態で
            if (owner.makeEnd)
            {
                if (Input.GetButton("Dash"))
                {
                    owner.ChangeState(stateIdle);
                    owner.isMake = false;
                }
            }













            owner.moveDirection = moveDirection + new Vector3(0, owner.moveDirection.y, 0);
            owner.characterController.Move(owner.moveDirection * Time.deltaTime * owner.moveSpeed);
            //owner._animator.SetFloat("Speed", moveDirection.magnitude);

            if (owner.inputHorizontal != 0 || owner.inputVertical != 0)
            {
                owner._animator.SetFloat("Speed", moveDirection.magnitude);
            }

        }
        public override void OnExit(PlayerControl owner, PlayerStateBase nextState)
        {
            owner.isMake = false;
            owner.makeEnd = true;

        }
    }
}
