using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var Button = GetComponent<Button>();

        Button.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("SampleScene");
        });
    }


}
