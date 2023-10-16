using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsTest : MonoBehaviour
{

    private const string TestKey = "TEST";

    // Start is called before the first frame update
    void Start()
    {
        //保存するデータ
        var testData = "This is Test";

        //stringをセット
        PlayerPrefs.SetString(TestKey, testData);

        //保存
        PlayerPrefs.Save();

        //保存したstringの読み込み
        //一度保存した後は、保存処理をコメントアウトしてもThis is Testを読み込める
        var saveDate = PlayerPrefs.GetString(TestKey);
        Debug.Log(saveDate);
    }


}
