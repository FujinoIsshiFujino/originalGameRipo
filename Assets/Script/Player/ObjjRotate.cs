using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjjRotate : MonoBehaviour
{

    private float fAngle = 0.0f;

    // public Vector3 v3AxisForward = Vector3.forward; //ワールドの正面ベクトル
    public Vector3 v3AxisForward;


    public Vector3 v3AxisCrossRight;
    public Vector3 v3AxisCrossUp;
    public Vector3 v3Axis; //回転軸

    Vector3 IniPosi;//初期位置
    Quaternion IniQua;//初期回転（姿勢）

    float inputHorizontal;
    protected float inputVertical;

    float resetTime;
    bool resetTimeIs;


    public Vector3 v3Axisrgiht;
    public Vector3 v3AxisUp;

    PlayerController _playerController;

    public enum rotateType
    {
        vertical,
        horizon,
        arbitraryAxis
    }

    [SerializeField] rotateType selectedType;


    // Start is called before the first frame update
    void Start()
    {
        IniPosi = transform.position;
        IniQua = transform.rotation;

    }

    private void Update()
    {

        // Rotate();



        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _playerController = player.GetComponent<PlayerController>();
        if (Input.GetKeyDown("2"))
        {
            // transform.rotation = IniQua;
            transform.forward = player.transform.forward;//他のものも初期方向はｚがプレイヤーのｚと一致している予定だけど、ものによっては変わるかも
            resetTimeIs = true;
        }


        if (Input.GetButton("Rotate"))
        {

            v3AxisForward = player.transform.forward;
            //特定のボタンを押すと、初期姿勢に戻る

            if (resetTimeIs)
            {

                resetTimeStart();
            }
            else
            {
                Quaternion qRot;

                //　回転軸が任意なので、入力方向に応じて、回転軸が変わるようにした。
                //任意回転軸は２つのベクトルの足し算によって作っている
                // 右に１入力した時は、任意回転軸はy軸でそれ中心にまわればいい。なので右方向ベクトルと入力（１）の掛け算, 正面方向ベクトルをとの外積結果が回転軸となる。逆向きは入力方向がー１にしてくれる。
                //右斜めに入力したら、任意回転軸は左下がりの軸になってほしい。斜めに入力したら横も楯も入力は０．５くらいだとする。
                //そうすると各外積結果により任意回転軸の構成ベクトルが二つ生まれて、その足し算が、ちょうどななめの任意回転軸になる。



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

                v3Axisrgiht = Vector3.right * inputHorizontal;
                v3AxisUp = Vector3.up * inputVertical;


                //v3Axisrgihtを右に入力した場合は左手系の外積計算から下向きのz軸、左に入力は上向きのz軸
                //なまえが紛らわしすぎるので変えたほうがいいかも
                // v3AxisCrossRight = Vector3.Cross(v3Axisrgiht, v3AxisForward).normalized; //任意回転軸の構成ベクトルの作成
                v3AxisCrossRight = Vector3.Cross(v3Axisrgiht, v3AxisForward).normalized;

                //同じように右向きのx軸、左向きのx軸
                v3AxisCrossUp = Vector3.Cross(v3AxisUp, v3AxisForward).normalized; //任意回転軸の構成ベクトルの作成

                // 任意回転軸の作成　例えば右に１入力したら、v3AxisUpは０で、v3AxisCrossUp０なので、任意回転軸はv3AxisCrossRight
                v3Axis = (v3AxisCrossRight + v3AxisCrossUp).normalized; //ベクトル（横と縦回転軸）の合成による軸の作成

                // 入力がされている限り毎フレーム回転する角度　入力の度合いは関係ない
                if (inputHorizontal > 0 || inputVertical > 0)
                {
                    fAngle = 90 * Time.deltaTime;
                }
                else if (inputHorizontal < 0 || inputVertical < 0)
                {
                    fAngle = 90 * Time.deltaTime;
                }

                //クォータ二オン　cosΘ/2 + nsinΘ/2  n=inx+jny+knz　unityはクォータ二オンのxyzwにそれぞれいれて、それを元の姿勢にかけてあげればいい
                v3Axis.Normalize();                                         // 軸ベクトル単位化
                qRot.w = Mathf.Cos(fAngle / 2.0f * Mathf.Deg2Rad);
                qRot.x = Mathf.Sin(fAngle / 2.0f * Mathf.Deg2Rad) * v3Axis.x;
                qRot.y = Mathf.Sin(fAngle / 2.0f * Mathf.Deg2Rad) * v3Axis.y;
                qRot.z = Mathf.Sin(fAngle / 2.0f * Mathf.Deg2Rad) * v3Axis.z;

                transform.rotation = qRot * transform.rotation;             // 回転 姿勢の回転はもともとクォータニオンとして保持してるから
                                                                            // クォータニオンをかければいい。
                                                                            // 掛け算は回転をあらわす
                                                                            // rend.material.color = colorCube;
            }
        }

    }

    protected virtual void Rotate()
    {

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

}
