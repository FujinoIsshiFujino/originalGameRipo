using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : ObjMove
{

    [SerializeField] Vector3 boxSize = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private LayerMask raycastLayerMask;

    protected override void ObjInternal()
    {
        base.ObjInternal();

        /// <summary>
        /// terrainなどの中身がないものにめり込んだ場合
        /// </summary>

        //橋の場合は端同士からrayを打つ
        Vector3[] corners = new Vector3[2];
        corners[0] = transform.position + new Vector3(0, 0, -transform.localScale.z / 2);
        corners[1] = transform.position + new Vector3(0, 0, transform.localScale.z / 2);

        // 各四隅からRayを飛ばしてめり込み時は上昇するように
        foreach (Vector3 corner in corners)
        {
            if (Physics.Raycast(corner, direction, out raycasthit, distance, raycastLayerMask))
            {

            }
            else
            {
                Vector3 newPosition = transform.position + Vector3.up * 100 * Time.deltaTime;
                transform.position = newPosition;
            }
        }

        // /// <summary>
        // /// /中身のあるものにめり込んだ場合
        // /// </summary>

        // オーバーラップしているColliderの配列を取得
        Collider[] overlappingColliders = Physics.OverlapBox(transform.position, boxSize / 2, Quaternion.identity);

        // オーバーラップしているオブジェクトが1つ以上ある場合
        if (overlappingColliders.Length > 0)
        {
            foreach (Collider collider in overlappingColliders)
            {
                // 自分以外のオーバーラップしているオブジェクトに対する処理
                if (collider.gameObject != gameObject)
                {
                    // ここでオーバーラップしているオブジェクトに対する処理を行う
                    Vector3 newPosition = transform.position + Vector3.up * 100 * Time.deltaTime;
                    transform.position = newPosition;
                }
            }
        }
    }

    //OnCollisionでやると、基底クラスのOnCollisionも上書きされてしまうので、メソッド単位でoverride
    protected override bool isObjVecDiscrimination()
    {
        base.isObjVecDiscrimination();

        // 6面のどの面が上に向いていてもいいので
        isObjVec = true;

        return isObjVec;
    }


    // 接触を判定する距離
    public float rayLength;


    protected override bool isSetableDiscrimination()
    {
        base.isSetableDiscrimination();

        rayLength = transform.localScale.y / 2 + 0.2f;

        // レイを飛ばす起点の座標
        Vector3 origin = transform.position;
        // レイを飛ばす方向（ローカル座標系の-Y方向）
        Vector3 direction = -Vector3.up;

        // レイキャスト結果を格納する変数
        RaycastHit hit;

        // デバッグ用の線を描画（レイキャストの可視化）
        Debug.DrawRay(origin, direction * rayLength, Color.red, 0.1f);

        // レイキャストを実行
        if (Physics.Raycast(origin, direction, out hit, rayLength))
        {
            // 他のオブジェクトに接触している場合
            isSetable = true;
            Debug.Log("下の面が接触しています: " + hit.collider.gameObject.name);

            // 接触した位置までのレイを緑色で描画
            Debug.DrawRay(origin, direction * hit.distance, Color.green, 0.1f);
        }
        else
        {
            // 他のオブジェクトに接触していない場合
            isSetable = false;
            Debug.Log("下の面は接触していません");

            // 接触していない場合は赤い線を描画（既に描画済み）
        }

        // 結果を返す
        return isSetable;
    }


    void OnCollisionExit(Collision collisionInfo)
    {
        isObjVec = false;
        isSetable = false;
    }
}
