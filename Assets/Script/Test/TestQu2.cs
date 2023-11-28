using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQu2 : MonoBehaviour
{
    public Vector3 v3Axis;
    public Quaternion qRot;
    public float fAngle;
    // Start is called before the first frame update
    void Start()
    {
        // fAngle = 90;

        // v3Axis = new Vector3(1, 1, 1);
        // v3Axis.Normalize();
        // qRot.w = Mathf.Cos(fAngle / 2.0f);
        // qRot.x = Mathf.Sin(fAngle / 2.0f) * v3Axis.x;
        // qRot.y = Mathf.Sin(fAngle / 2.0f) * v3Axis.y;
        // qRot.z = Mathf.Sin(fAngle / 2.0f) * v3Axis.z;

        // // transform.position = qRot * transform.position;
        // transform.rotation = qRot * transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        fAngle = 4 * Time.deltaTime;


        v3Axis = new Vector3(1, 1, 1);
        // v3Axis.Normalize();
        qRot.w = Mathf.Cos(fAngle / 2.0f * Mathf.Deg2Rad);
        qRot.x = Mathf.Sin(fAngle / 2.0f * Mathf.Deg2Rad) * v3Axis.x;
        qRot.y = Mathf.Sin(fAngle / 2.0f * Mathf.Deg2Rad) * v3Axis.y;
        qRot.z = Mathf.Sin(fAngle / 2.0f * Mathf.Deg2Rad) * v3Axis.z;
        // Debug.Log("qRot" + qRot + "seki" + qRot);
        //Mathf.SinおよびMathf.Cosはラジアンで角度を指定するため、度数をそのまま使うと正確な結果が得られないので Mathf.Deg2Radでラジアンに変換

        // transform.position = qRot * transform.position; //公転
        transform.rotation = qRot * transform.rotation; //姿勢回転
        // transform.rotation = transform.rotation * qRot;

        //  Quaternion inverse = Quaternion.Inverse(qRot);
        //   transform.position = qRot * transform.position*inverse;






        // Y 軸 (上方向) まわりに 30 度回転するのを表すクォータニオン
        // transform.rotation *= Quaternion.AngleAxis(4 * Time.deltaTime, new Vector3(1, 1, 1));

    }
}
