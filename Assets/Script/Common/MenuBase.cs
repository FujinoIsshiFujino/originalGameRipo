using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuBase : MonoBehaviour
{
    public Button[] menuButtons;
    public int selectedButtonIndex = 0;
    public PlayerControl playerControl;
    public Color normalColor;
    public Color selectedColor;
    public Color disabledColor;
    public Color selectedDisabledColor;

    protected PlayerControl.StateMaking stateMaking;
    bool previousInputWasUp = false;
    bool previousInputWasDown = false;
    [SerializeField] float VerticalDepth = 0.2f;//斜め方向の感度

    protected virtual void Start()
    {
        // 最初のボタンを選択状態にする
        SelectButton(selectedButtonIndex);
        stateMaking = new PlayerControl.StateMaking();
    }

    //  オブジェクトがアクティブになるたびに実行されるメソッド
    protected virtual void OnEnable()
    {
        SelectButton(selectedButtonIndex);
    }

    protected virtual void Update()
    {
        float input = Input.GetAxis("Vertical");

        if (input <= 1 && VerticalDepth < input)
        {
            if (!previousInputWasUp)
            {
                selectedButtonIndex = selectedButtonIndex - 1;
                if (selectedButtonIndex < 0)
                {
                    selectedButtonIndex = menuButtons.Length - 1;
                }

                //ボタンの色を変える
                SelectButton(selectedButtonIndex);
                previousInputWasUp = true; //連続入力の禁止
            }
        }
        else
        {
            previousInputWasUp = false;
        }

        if (input >= -1 && -VerticalDepth > input)
        {
            if (!previousInputWasDown)
            {
                selectedButtonIndex = selectedButtonIndex + 1;
                if (menuButtons.Length - 1 < selectedButtonIndex)
                {
                    selectedButtonIndex = 0;
                }

                //ボタンの色を変える
                SelectButton(selectedButtonIndex);
                previousInputWasDown = true; //連続入力の禁止
            }
        }
        else
        {
            previousInputWasDown = false;
        }

        //選択ボタンを決定した時
        if (Input.GetButtonDown("Attack"))
        {
            DecisionAction();
        }
    }

    void SelectButton(int index)
    {
        // このメソッド実行時にすべてのボタンに対して実行される。条件に応じてボタンの色が変わる
        for (int i = 0; i < menuButtons.Length; i++)
        {
            var colors = menuButtons[i].colors;

            if (i == index)
            {
                if (colors.normalColor.Equals(disabledColor))
                {
                    colors.normalColor = selectedDisabledColor;
                }
                else
                {
                    colors.normalColor = selectedColor;
                }
            }
            else
            {
                colors.normalColor = ButtonsColor(i, colors).normalColor;
            }

            menuButtons[i].colors = colors;
        }
    }

    // Toggleは親メニューがあった場合に、子メニューを開くときに使う
    //CloseMenuやOpenMenuは親メニューを開くときに使う
    public void Toggle(GameObject gameObject)
    {
        //自分自身のアクティブをボタンが押されたときに切り替える
        gameObject.SetActive(!gameObject.activeSelf);
    }

    // 今選ばれていないボタンの処理
    protected virtual ColorBlock ButtonsColor(int index, ColorBlock colors)//オブジェによって処理を継承先で変える
    {
        return colors;
    }

    //決定ボタンが押されたときの処理
    protected virtual void DecisionAction()//オブジェによって処理を継承先で変える
    {

    }
}