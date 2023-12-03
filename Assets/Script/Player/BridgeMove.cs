using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeMove : ObjMove
{

    public Vector3 boxSize = new Vector3(0.1f, 0.1f, 0.1f);

    [SerializeField] private LayerMask raycastLayerMask;

    protected override void ObjInternal()
    {

        base.ObjInternal();

        //terrainなどの中身がないものにめり込んだ場合
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


        //中身のあるものにめり込んだ場合
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
                    // Debug.Log("オブジェクトがめり込んでいます。対象: " + collider.gameObject.name);

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

        if (transform.up == Vector3.up)
        {
            isObjVec = true;
        }
        else
        {
            isObjVec = false;
        }
        return isObjVec;

    }

    protected override bool isSetableDiscrimination()
    {
        base.isSetableDiscrimination();

        if (isObjVec)
        {
            _confilmSetFront = frontColl.GetComponent<ConfilmSet>();
            _confilmSetBack = backColl.GetComponent<ConfilmSet>();

            if (_confilmSetFront.isSet == true && _confilmSetBack.isSet == true)
            {
                isSetable = true;
            }
            else
            {
                isSetable = false;
            }
        }

        return isSetable;
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        isObjVec = false;
        isSetable = false;
    }
}
