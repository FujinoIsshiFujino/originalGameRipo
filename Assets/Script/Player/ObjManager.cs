using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjManager : MonoBehaviour
{
    public GameObject[] objArray = new GameObject[4];

    // Update is called once per frame
    void Update()
    {
        if (objArray[3] != null)
        {
            RemoveOldestObject();
        }
    }

    // シーン内に、オブジェ生成できる数を制限するメソッド
    private void RemoveOldestObject()
    {
        if (objArray[0] != null)
        {
            Destroy(objArray[0]); // シーンからオブジェクトを削除
        }

        for (int i = 0; i < objArray.Length - 1; i++)
        {
            objArray[i] = objArray[i + 1];
        }

        // 配列の最後の要素をnullにする
        objArray[objArray.Length - 1] = null;
    }
}
