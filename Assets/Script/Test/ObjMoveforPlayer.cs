using System.Collections;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using UnityEngine;


// Summary
//オブジェクト自身ではなくプレイヤーにアタッチしていた旧バージョン
// Summary
public class ObjMoveforPlayer : MonoBehaviour
{

    // [SerializeField] public GameObject prefabToInstantiate;
    // public bool isMake;
    // public bool makeEnd;
    // public float verticalAngle;

    // GameObject newObject;
    // [SerializeField] float objUpDownMovingSpeed = 20;
    // [SerializeField] float objForwardBackMovingSpeed = 20;
    // float inputObjHorizontal;
    // float inputObjVertical;

    // public float distanceToPlayer;
    // public Vector3 beforePos;

    // [SerializeField] PlayerController player;



    // private Rigidbody rb;

    // // Start is called before the first frame update
    // void Start()
    // {
    //     rb = GetComponent<Rigidbody>();

    // }

    // // Update is called once per frame
    // void Update()
    // {



    //     player = GetComponent<PlayerController>();
    //     if (player.isGrounded)
    //     {
    //         if (player._status.IsMovable)
    //         {

    //             if (isMake == false)
    //             {

    //                 if (Input.GetButtonDown("Make"))
    //                 {
    //                     isMake = true;
    //                     makeEnd = false;


    //                     Quaternion playerRotation = transform.rotation;

    //                     // プレハブからクローンを生成
    //                     newObject = Instantiate(prefabToInstantiate, transform.position + transform.forward * 8, playerRotation);

    //                     // // 生成したオブジェクトをプレイヤーの子オブジェクトに設定
    //                     // newObject.transform.parent = transform;

    //                 }
    //             }

    //             if (isMake)
    //             {

    //                 if (Input.GetButtonDown("Dash"))
    //                 {
    //                     isMake = false;
    //                     makeEnd = true;
    //                     Destroy(newObject);
    //                 }

    //                 verticalAngle += Input.GetAxis("VerticalCamera") / objUpDownMovingSpeed;// マイナス符号を付けることで上下反転
    //                 float verticalDownAngleLimit = 0;
    //                 float verticalUpAngleLimit = 10;
    //                 verticalAngle = Mathf.Clamp(verticalAngle, verticalDownAngleLimit, verticalUpAngleLimit); // 垂直回転の角度を制限

    //                 newObject.transform.position = new Vector3(newObject.transform.position.x, verticalAngle, newObject.transform.position.z);

    //                 if (Input.GetButton("First"))
    //                 {
    //                     // player.moveDirection = new Vector3(0, 0, 0);


    //                     inputObjVertical = Input.GetAxis("Vertical") / objForwardBackMovingSpeed;
    //                     float objForwardLimit = 20;
    //                     float objBackLimit = 5;

    //                     Vector3 calDistancePlayerVec = transform.position;
    //                     calDistancePlayerVec.y = 0;
    //                     Vector3 calDistanceObjVec = newObject.transform.position;
    //                     calDistanceObjVec.y = 0;
    //                     // distanceToPlayer = Vector3.Distance(transform.position, newObject.transform.position);
    //                     distanceToPlayer = Vector3.Distance(calDistancePlayerVec, calDistanceObjVec);

    //                     // 制限距離以上に移動しようとした場合は移動しない
    //                     if (distanceToPlayer < objBackLimit)
    //                     {
    //                         newObject.transform.position = newObject.transform.position + transform.forward.normalized * 0.01f;
    //                     }
    //                     else if (distanceToPlayer > objForwardLimit)
    //                     {
    //                         newObject.transform.position = newObject.transform.position + transform.forward.normalized * -0.01f;
    //                     }
    //                     else
    //                     {
    //                         newObject.transform.position = newObject.transform.position + transform.forward.normalized * inputObjVertical;
    //                     }

    //                 }

    //                 if (Input.GetButton("Lock"))
    //                 {

    //                 }
    //             }
    //         }
    //     }
    // }
}