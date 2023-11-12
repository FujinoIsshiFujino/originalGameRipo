using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjHeighAjust : MonoBehaviour
{

    public float verticalAngle;

    [SerializeField] float objUpDownMovingSpeed = 20;


    // // Start is called before the first frame update
    // void Start()
    // {

    // }

    // Update is called once per frame
    void Update()
    {



        verticalAngle += Input.GetAxis("VerticalCamera") / objUpDownMovingSpeed;// マイナス符号を付けることで上下反転
        float verticalDownAngleLimit = 0;
        float verticalUpAngleLimit = 10;
        verticalAngle = Mathf.Clamp(verticalAngle, verticalDownAngleLimit, verticalUpAngleLimit); // 垂直回転の角度を制限

        // 親オブジェクトのTransformコンポーネントを取得
        Transform parentTransform = this.gameObject.transform.parent;

        // this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, verticalAngle, this.gameObject.transform.position.z);
        parentTransform.transform.position = new Vector3(this.gameObject.transform.position.x, verticalAngle, this.gameObject.transform.position.z);

    }

    private void OnCollisionStay(Collision collision)
    {

        // if( collision.contacts[0].point.y>this.gameObject.)

        //衝突判定が橋自体にかかってる

        Debug.Log("hit");
        // this.gameObject.transform.position = this.gameObject.transform.position + Player.transform.forward.normalized * -0.01f;
        // moveDirection = new Vector3(0, 0, 0);
        // isHit = true;

        // 障害物の高さを取得
        float obstacleHeight = collision.contacts[0].point.y;
        Debug.Log("obstacleHeight" + obstacleHeight);


        Transform objectTransform = transform;

        // オブジェクトの高さ（Y座標）を取得
        float objectHeight = objectTransform.position.y;


        // オブジェクトの高さを障害物の高さに合わせる
        Vector3 newPosition = transform.position;
        newPosition.y = obstacleHeight;
        // transform.position = newPosition;

        verticalAngle = newPosition.y + objectHeight / 2 + 0.05f; // 0.05fは調整値

        //底面に当たっていることかふぃっくすアップデートの時間の感覚がおそいからか　　それかボックスコリジョンを使うか。底面だとしたらレイを底面以外にうつ？

    }
}
