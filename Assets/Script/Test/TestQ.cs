using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQ : MonoBehaviour
{

    private float fAngle = 0.0f;

    public Vector3 v3AxisForward = Vector3.forward; //ワールドの正面ベクトル


    public Vector3 v3AxisCrossRight;
    public Vector3 v3AxisCrossUp;
    public Vector3 v3Axis; //回転軸

    Vector3 IniPosi;//初期位置
    Quaternion IniQua;//初期回転（姿勢）

    float inputHorizontal;
    float inputVertical;

    float resetTime;
    bool resetTimeIs;





    public Vector3 v3Axisrgiht;
    public Vector3 v3AxisUp;


    // Start is called before the first frame update
    void Start()
    {
        IniPosi = transform.position;
        IniQua = transform.rotation;
    }

    private void Update()
    {
        //特定のボタンを押すと、初期姿勢に戻る
        if (Input.GetKeyDown("1"))
        {
            // transform.position= IniPosi;
            transform.rotation = IniQua;

            resetTimeIs = true;
        }

        // Debug.Log("１がおされました" + resetTimeIs);

        if (resetTimeIs)
        {

            resetTimeStart();
        }
        else
        {
            Quaternion qRot;
            Quaternion qPos;
            Vector3 v3Pos;
            // Vector3 v3Axis = new Vector3(1.0f, 1.0f, 1.0f);


            //　回転軸が任意なので、入力方向に応じて、回転軸が変わるようにした。
            //任意回転軸は２つのベクトルの足し算によって作っている
            // 右に１入力した時は、任意回転軸はy軸でそれ中心にまわればいい。なので右方向ベクトルと入力（１）の掛け算, 正面方向ベクトルをとの外積結果が回転軸となる。逆向きは入力方向がー１にしてくれる。
            //右斜めに入力したら、任意回転軸は左下がりの軸になってほしい。斜めに入力したら横も楯も入力は０．５くらいだとする。
            //そうすると各外積結果により任意回転軸の構成ベクトルが二つ生まれて、その足し算が、ちょうどななめの任意回転軸になる。

            inputHorizontal = Input.GetAxis("Horizontal");
            inputVertical = Input.GetAxis("Vertical");

            v3Axisrgiht = Vector3.right * inputHorizontal;
            v3AxisUp = Vector3.up * inputVertical;

            //v3Axisrgihtを右に入力した場合は左手系の外積計算から下向きのz軸、左に入力は上向きのz軸
            //なまえが紛らわしすぎるので変えたほうがいいかも
            v3AxisCrossRight = Vector3.Cross(v3Axisrgiht, v3AxisForward).normalized; //任意回転軸の構成ベクトルの作成

            //同じように右向きのx軸、左向きのx軸
            v3AxisCrossUp = Vector3.Cross(v3AxisUp, v3AxisForward).normalized; //任意回転軸の構成ベクトルの作成

            // 任意回転軸の作成　例えば右に１入力したら、v3AxisUpは０で、v3AxisCrossUp０なので、任意回転軸はv3AxisCrossRight
            v3Axis = (v3AxisCrossRight + v3AxisCrossUp).normalized; //ベクトル（横と縦回転軸）の合成による軸の作成

            //fAngle = 2.0f * Mathf.PI * Time.deltaTime / 10.0f;          // 角度


            // 入力がされている限り毎フレーム回転する角度　入力の度合いは関係ない
            if (inputHorizontal > 0 || inputVertical > 0)
            {
                fAngle = 2.0f * Mathf.PI * Time.deltaTime / 10.0f;
            }
            else if (inputHorizontal < 0 || inputVertical < 0)
            {
                fAngle = 2.0f * Mathf.PI * Time.deltaTime / 10.0f;
            }


            //クォータ二オン　cosΘ/2 + nsinΘ/2  n=inx+jny+knz　unityはクォータ二オンのxyzwにそれぞれいれて、それを元の姿勢にかけてあげればいい
            v3Axis.Normalize();                                         // 軸ベクトル単位化
            qRot.w = Mathf.Cos(fAngle / 2.0f);
            qRot.x = Mathf.Sin(fAngle / 2.0f) * v3Axis.x;
            qRot.y = Mathf.Sin(fAngle / 2.0f) * v3Axis.y;
            qRot.z = Mathf.Sin(fAngle / 2.0f) * v3Axis.z;

            transform.rotation = qRot * transform.rotation;             // 回転 姿勢の回転はもともとクォータニオンとして保持してるから
                                                                        // クォータニオンをかければいい。
                                                                        // 掛け算は回転をあらわす
                                                                        // rend.material.color = colorCube;

            // クォータニオンによる回転
            // v3Pos = transform.position;
            // qPos.x = v3Pos.x;
            // qPos.y = v3Pos.y;
            // qPos.z = v3Pos.z;
            // qPos.w = 1.0f;
            // qPos = qRot * qPos * new Quaternion(-qRot.x, -qRot.y, -qRot.z, qRot.w);// Quaternion.Inverse(qRot);
            // v3Pos.x = qPos.x;
            // v3Pos.y = qPos.y;
            // v3Pos.z = qPos.z;
            // transform.position = v3Pos;     // 変換　位置（ベクトル）の変換はインバースかけるやつ。
            //        transform.position = qRot * transform.position;     // 変換

        }
    }

    //回転リセットを押した直後はすこしうごきがとまる
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
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {



    }
}


//必要なのは軸の決定方法と。回転角度決定方法（こっちはいけるとおもう 軸を決めたらΘ分だけ左手回転で回転してくれるから、軸の決定が一番大事

//リセットしたときに、回転が少しだけ止まらない。フィックスのほうで回転させてるから、処理が遅れてるんだと思う。
//解決はフィックス内容をアップデートに移すか（でもまずはそもそもフィックスの理由を調べたほうがいい）、フィックス内に
//アップデートのボタンが押された信号を送るか
