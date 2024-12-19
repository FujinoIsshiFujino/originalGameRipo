using System.Collections;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations;

public class ObjjRotate : MonoBehaviour
{
    //インスペクターから視認
    public Vector3 v3AxisForward;

    //任意回転軸用
    Vector3 v3AxisCrossRight;
    Vector3 v3AxisCrossUp;
    float dotWorldPlayer;
    public Vector3 v3Axisrgiht;
    public Vector3 v3AxisUp;
    public Vector3 v3Axis; //回転軸

    float fAngle = 0.0f;
    public float inputHorizontal;
    public float inputVertical;
    float resetTime;
    bool resetTimeIs;
    PlayerControl playerControl;
    public enum rotateType
    {
        vertical,
        horizon,
        arbitraryAxis
    }
    public rotateType selectedType;
    GameObject Player;
    bool isRotate;
    [SerializeField] float VerticalDepth = 0.2f;//斜め方向の感度
    Quaternion qRot;
    Vector3 axis;
    [SerializeField] float rotateSpeed = 2;
    void Start()
    {
        isRotate = false;

        PlayerControl playerControlComponent = GameObject.FindObjectOfType<PlayerControl>();
        if (playerControlComponent != null)
        {
            Player = playerControlComponent.gameObject;
            // ゲームオブジェクト名で絞るより、プログラム名で絞ったほうが変更が後々少なそうなのでプログラムで絞る。
        }
    }

    private void Update()
    {
        playerControl = Player.GetComponent<PlayerControl>();
        if (Input.GetButtonDown("L1"))
        {
            transform.forward = Player.transform.forward;//他のものも初期方向はｚがプレイヤーのｚと一致している予定だけど、ものによっては変わるかも
            resetTimeIs = true;
        }

        if (Input.GetButton("Rotate"))
        {
            if (resetTimeIs)
            {
                resetTimeStart();
            }
            else
            {
                Rotate();
            }
        }
    }

    protected virtual void Rotate()
    {
        //　入力方向に応じて、回転軸が任意に変わるようにした。
        //任意回転軸は２つのベクトルの合成によって作っている
        // 例えば右に１入力した時は、任意回転軸はy軸でそれ中心にまわればいい。なので右方向ベクトルと入力（１）の掛け算, 正面方向ベクトルをとの外積結果が回転軸となる。逆向きは入力方向がー１にしてくれる。
        //右斜めに入力したら、任意回転軸は左下がりの軸になってほしい。斜めに入力したら横も楯も入力は０．５くらいだとする。
        //そうすると各外積結果により任意回転軸の構成ベクトルが二つ生まれて、その合成が、ちょうどななめの任意回転軸になる。

        Quaternion qRot;
        v3AxisForward = Player.transform.forward.normalized;
        //特定のボタンを押すと、初期姿勢に戻る



        if (selectedType == rotateType.horizon)
        {
            inputHorizontal = Input.GetAxis("Horizontal");
            inputVertical = 0;
        }
        else if (selectedType == rotateType.vertical)
        {
            inputHorizontal = 0;
            inputVertical = Input.GetAxis("Vertical");
        }
        else if (selectedType == rotateType.arbitraryAxis)
        {
            inputHorizontal = Input.GetAxis("Horizontal");
            inputVertical = Input.GetAxis("Vertical");
        }

        /// <summary>
        /// コメントアウト部分は任意回転軸
        /// </summary>

        //回転方向の逆転を修正
        // if (inputHorizontal != 0)
        // {
        //     dotWorldPlayer = Vector3.Dot(v3AxisForward, Vector3.forward);
        // }
        // if (dotWorldPlayer < 0)
        // {
        //     inputHorizontal *= -1;
        // }

        // // 一旦仮の軸の作成
        // v3Axisrgiht = Vector3.right * inputHorizontal;
        // v3AxisUp = Vector3.up * inputVertical;


        // //任意回転軸の構成ベクトル（v3AxisCrossUp、v3AxisCrossRigh）の作成
        // //v3Axisrgihtを右に入力した場合は左手系の外積計算から下向きのｙ軸、左入力は上向きのｙ軸
        // //右を正の向きとしているので、右に入力した場合は実際は外積結果は下方向のものだが、便宜上名前はUpとする
        // v3AxisCrossUp = Vector3.Cross(v3Axisrgiht, v3AxisForward).normalized;

        // //同じようにx軸の作成　 
        // v3AxisCrossRight = Vector3.Cross(v3AxisUp, v3AxisForward).normalized;


        // // 任意回転軸の作成　例えば右に１入力したら、v3AxisUpは０で、v3AxisCrossUp０なので、任意回転軸はv3AxisCrossRight
        // v3Axis = (v3AxisCrossUp + v3AxisCrossRight).normalized; //ベクトル（横と縦回転軸）の合成による軸の作成

        // 入力がされている限り毎フレーム回転する角度　入力の度合いは関係ない
        // if (inputHorizontal > 0 || inputVertical > 0)
        // {
        //     fAngle = 90 * Time.deltaTime;
        // }
        // else if (inputHorizontal < 0 || inputVertical < 0)
        // {
        //     fAngle = 90 * Time.deltaTime;
        // }

        // //クォータ二オン　cosΘ/2 + nsinΘ/2  n=inx+jny+knz　unityはクォータ二オンのxyzwにそれぞれいれて、それを元の姿勢にかけてあげればいい
        // v3Axis.Normalize();                                         // 軸ベクトル単位化
        // qRot.w = Mathf.Cos(fAngle / 2.0f * Mathf.Deg2Rad);
        // qRot.x = Mathf.Sin(fAngle / 2.0f * Mathf.Deg2Rad) * v3Axis.x;
        // qRot.y = Mathf.Sin(fAngle / 2.0f * Mathf.Deg2Rad) * v3Axis.y;
        // qRot.z = Mathf.Sin(fAngle / 2.0f * Mathf.Deg2Rad) * v3Axis.z;

        // transform.rotation = qRot * transform.rotation;
        // //  姿勢の回転

        if (!isRotate)
        {
            // 斜め回転
            // if ((inputHorizontal <= 1 && VerticalDepth < inputHorizontal) && (inputVertical <= 1 && VerticalDepth < inputVertical))
            // {
            //     isRotate = true;
            //     StartCoroutine(RotateSpecifiedAngle(90, "leftup"));
            // }
            // else if ((inputHorizontal >= -1 && -VerticalDepth > inputHorizontal) && (inputVertical >= -1 && -VerticalDepth > inputVertical))
            // {
            //     isRotate = true;
            //     StartCoroutine(RotateSpecifiedAngle(-90, "leftup"));
            // }
            // else if ((inputHorizontal <= 1 && VerticalDepth < inputHorizontal) && (inputVertical >= -1 && -VerticalDepth > inputVertical))
            // {
            //     isRotate = true;
            //     StartCoroutine(RotateSpecifiedAngle(-90, "rightup"));
            // }
            // else if ((inputHorizontal >= -1 && -VerticalDepth > inputHorizontal) && (inputVertical <= 1 && VerticalDepth < inputVertical))
            // {
            //     isRotate = true;
            //     StartCoroutine(RotateSpecifiedAngle(90, "rightup"));
            // }
            // else 
            if ((inputHorizontal <= 1 && VerticalDepth < inputHorizontal) || Input.GetKeyDown("a"))
            {
                isRotate = true;
                StartCoroutine(RotateSpecifiedAngle(-90, "up"));
            }
            else if ((inputHorizontal >= -1 && -VerticalDepth > inputHorizontal) || Input.GetKeyDown("d"))
            {
                isRotate = true;
                StartCoroutine(RotateSpecifiedAngle(90, "up"));
            }
            // 縦回転の条件
            else if ((inputVertical <= 1 && VerticalDepth < inputVertical) || Input.GetKeyDown("w"))
            {
                isRotate = true;
                StartCoroutine(RotateSpecifiedAngle(90, "right"));
            }
            else if ((inputVertical >= -1 && -VerticalDepth > inputVertical) || Input.GetKeyDown("s"))
            {
                isRotate = true;
                StartCoroutine(RotateSpecifiedAngle(-90, "right"));
            }
        }
    }

    IEnumerator RotateSpecifiedAngle(float targetAngle, string axisName)
    {
        float rotatedAngle = 0f;
        fAngle = 0;
        float tolerance = 0.01f;

        while (Mathf.Abs(rotatedAngle - targetAngle) > tolerance && !resetTimeIs)
        {
            if (axisName == "up")
            {
                axis = Vector3.up;
            }
            else if (axisName == "right")
            {
                axis = Player.transform.right;
                //外積で縦回転軸の算出
                // axis = Vector3.Cross(Vector3.up, Player.transform.forward).normalized;
            }
            // else if (axisName == "leftup")
            // {
            //     axis = (Player.transform.right + Vector3.up * -1).normalized;
            // }
            // else if (axisName == "rightup")
            // {
            //     axis = (Player.transform.right + Vector3.up).normalized;
            // Debug.DrawLine(new Vector3(transform.position.x + transform.localScale.x / 2, transform.position.y + transform.localScale.y / 2, transform.position.z),
            //                 new Vector3(transform.position.x + -transform.localScale.x / 2, transform.position.y + -transform.localScale.y / 2, transform.position.z), Color.green, 0.1f, false);
            // }

            fAngle = targetAngle * Time.deltaTime * rotateSpeed;//１フレームに回る角度
            rotatedAngle += fAngle;//回った合計の角度

            if (Mathf.Abs(rotatedAngle) > Mathf.Abs(targetAngle))
            {
                float diff = rotatedAngle - targetAngle;
                fAngle -= diff; // 誤差分を補正
                rotatedAngle = targetAngle;
            }
            qRot.w = Mathf.Cos(fAngle / 2.0f * Mathf.Deg2Rad);
            qRot.x = Mathf.Sin(fAngle / 2.0f * Mathf.Deg2Rad) * axis.x;
            qRot.y = Mathf.Sin(fAngle / 2.0f * Mathf.Deg2Rad) * axis.y;
            qRot.z = Mathf.Sin(fAngle / 2.0f * Mathf.Deg2Rad) * axis.z;

            transform.rotation = qRot * transform.rotation;

            // Debug.Log("rotatedAngle : " + rotatedAngle + "targetAngle : " + targetAngle + "resetTimeIs" + resetTimeIs);

            yield return null;
        }

        isRotate = false;
    }


    //回転リセット
    void resetTimeStart()
    {
        resetTime += Time.deltaTime;

        if (resetTime < 0.3)
        {
            fAngle = 0;
        }
        else
        {
            resetTimeIs = false;
            resetTime = 0; //リセットタイム更新

            //回転姿勢・位置を初期姿勢に戻す
            this.transform.forward = Player.transform.forward;
        }
    }

}
