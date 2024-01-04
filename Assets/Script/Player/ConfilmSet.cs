// 子オブジェの中心判定　これを親オブジェで感知する
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ConfilmSet : MonoBehaviour
// {
//     public bool isSet;
//     Vector3 contactNormal;

//     private void OnCollisionStay(Collision collision)
//     {
//         contactNormal = collision.contacts[0].normal.normalized;
//         if (contactNormal == Vector3.up)
//         {
//             isSet = true;
//         }
//     }

//     private void OnCollisionExit(Collision collision)
//     {
//         contactNormal = Vector3.zero;
//         isSet = false;
//     }

// }







//四隅が接触している。＆＆接触物が下にある　かつ衝突レイヤー
using UnityEngine;

public class ConfilmSet : MonoBehaviour
{
    public bool isSet = false;
    public string targetTag = "ground";

    void Update()
    {
        // オブジェクトの四隅のポイントを計算 親オブジェのスケールに試供されないように実際のスケール（lossyScale）を適用
        Vector3 topLeft = new Vector3(-transform.lossyScale.x / 2, transform.lossyScale.y / 2, transform.lossyScale.z / 2);
        Vector3 topRight = new Vector3(transform.lossyScale.x / 2, transform.lossyScale.y / 2, transform.lossyScale.z / 2);
        Vector3 bottomLeft = new Vector3(-transform.lossyScale.x / 2, transform.lossyScale.y / 2, -transform.lossyScale.z / 2);
        Vector3 bottomRight = new Vector3(transform.lossyScale.x / 2, transform.lossyScale.y / 2, -transform.lossyScale.z / 2);

        // 各ポイントで衝突判定
        bool topLeftCollision = CheckCollision(topLeft);
        bool topRightCollision = CheckCollision(topRight);
        bool bottomLeftCollision = CheckCollision(bottomLeft);
        bool bottomRightCollision = CheckCollision(bottomRight);

        // 四隅がすべて指定のタグの物体に接触している場合に isSet を true 
        isSet = topLeftCollision && topRightCollision && bottomLeftCollision && bottomRightCollision;
    }

    bool CheckCollision(Vector3 point)
    {
        // 衝突判定
        Collider[] colliders = Physics.OverlapSphere(transform.position + point, 0.1f);

        // タグが指定のものであるかどうかを確認
        foreach (var collider in colliders)
        {
            if (collider.CompareTag(targetTag))
            {
                return true; // 指定のタグの物体に接触している場合は true を返す
            }
        }

        return false; // 指定のタグの物体に接触していない場合は false を返す
    }
}


