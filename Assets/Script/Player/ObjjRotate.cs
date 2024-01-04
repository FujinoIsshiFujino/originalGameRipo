using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjjRotate : MonoBehaviour
{
    //インスペクターから視認
    public Vector3 v3AxisForward;
    public Vector3 v3Axisrgiht;
    public Vector3 v3AxisUp;
    public Vector3 v3Axis; //回転軸



    Vector3 v3AxisCrossRight;
    Vector3 v3AxisCrossUp;
    float fAngle = 0.0f;
    float inputHorizontal;
    float inputVertical;
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
    float dotWorldPlayer;

    void Start()
    {
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

        //回転方向の逆転を修正
        if (inputHorizontal != 0)
        {
            dotWorldPlayer = Vector3.Dot(v3AxisForward, Vector3.forward);
        }
        if (dotWorldPlayer < 0)
        {
            inputHorizontal *= -1;
        }

        //一旦仮の軸の作成
        v3Axisrgiht = Vector3.right * inputHorizontal;
        v3AxisUp = Vector3.up * inputVertical;


        //任意回転軸の構成ベクトル（v3AxisCrossUp、v3AxisCrossRigh）の作成
        //v3Axisrgihtを右に入力した場合は左手系の外積計算から下向きのｙ軸、左入力は上向きのｙ軸
        //右を正の向きとしているので、右に入力した場合は実際は外積結果は下方向のものだが、便宜上名前はUpとする
        v3AxisCrossUp = Vector3.Cross(v3Axisrgiht, v3AxisForward).normalized;

        //同じようにx軸の作成　 
        v3AxisCrossRight = Vector3.Cross(v3AxisUp, v3AxisForward).normalized;


        // 任意回転軸の作成　例えば右に１入力したら、v3AxisUpは０で、v3AxisCrossUp０なので、任意回転軸はv3AxisCrossRight
        v3Axis = (v3AxisCrossUp + v3AxisCrossRight).normalized; //ベクトル（横と縦回転軸）の合成による軸の作成

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

        transform.rotation = qRot * transform.rotation;
        //  姿勢の回転
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
        }
    }

}
