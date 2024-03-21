// using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : MonoBehaviour
{
    public Button[] menuButtons;
    public int selectedButtonIndex = 0;
    [SerializeField] MakeMchineUI _makeMchineUI;
    MakeButtoon _makeButtoon;
    public MakeButtoon.makeItemType selectedMakeItemType;

    [SerializeField] PlayerControl playerControl;
    [SerializeField] Color selectedColor;
    [SerializeField] Color disabledColor;
    [SerializeField] Color selectedDisavledColor;

    PlayerControl.StateMaking stateMaking;


    private bool isSel;

    void Start()
    {
        // 最初のボタンを選択状態にする
        SelectButton(selectedButtonIndex);

        stateMaking = new PlayerControl.StateMaking();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // ボタンのインデックスを更新する
            selectedButtonIndex = (selectedButtonIndex + 1) % menuButtons.Length;

            // 新しいボタンを選択する
            SelectButton(selectedButtonIndex);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // ボタンのインデックスを更新する
            selectedButtonIndex = (selectedButtonIndex - 1 + menuButtons.Length) % menuButtons.Length;

            // 新しいボタンを選択する
            SelectButton(selectedButtonIndex);
        }


        // float input = Input.GetAxis("Vertical");
        // a = Input.GetAxis("Vertical");

        // if (input != 1 && input != -1)
        // {
        //     // isSel = true;
        // }
        // else { isSel = false; }

        // if (input == -1f && !isSel)//下入力
        // {
        //     // ボタンのインデックスを更新する
        //     // selectedButtonIndex = (selectedButtonIndex + 1) % menuButtons.Length;
        //     selectedButtonIndex++;

        //     // 新しいボタンを選択する
        //     SelectButton(selectedButtonIndex);
        //     isSel = true;



        // }

        // if (input == 1f && !isSel)//上入力
        // {

        //     // ボタンのインデックスを更新する
        //     // selectedButtonIndex = (selectedButtonIndex - 1 + menuButtons.Length) % menuButtons.Length;
        //     selectedButtonIndex--;

        //     // 新しいボタンを選択する
        //     SelectButton(selectedButtonIndex);
        //     isSel = true;
        // }




        Debug.Log(isSel + "isSel");
        Debug.Log("selectedButtonIndex" + selectedButtonIndex);

        _makeButtoon = menuButtons[selectedButtonIndex].GetComponent<MakeButtoon>();


        //基底化するときはほかのメニューに使い切るという概念がないので、ここはわける
        // 作れないものはdisavleさせる
        //今所持しているアイテムの種類分だけループさせて、全てのボタンの消費アイテムを一致するまでループする。
        foreach (var item in OwnedItemsData.Instance.OwnedItems)
        {
            for (int i = 0; i < menuButtons.Length; i++)
            {
                var eachButton = menuButtons[i].GetComponent<MakeButtoon>();
                foreach (var buttonItemData in eachButton.itemDataArray)
                    if (item.Type == buttonItemData.itemType)
                    {
                        if (item.Number < buttonItemData.consumeItemNumber)
                        {
                            DisabledButton(i);
                        }
                    }
            }
        }

        // 特定のキーが押されたときにデバッグする
        if (Input.GetButtonDown("Attack"))
        {
            _makeButtoon = menuButtons[selectedButtonIndex].GetComponent<MakeButtoon>();
            selectedMakeItemType = _makeButtoon.type;

            bool allConditionsMet = true;
            //選択した物の材料の所持数が消費数を超えていた場合に状態遷移
            foreach (var item in OwnedItemsData.Instance.OwnedItems)
            {
                foreach (var buttonItemData in _makeButtoon.itemDataArray)
                    if (item.Type == buttonItemData.itemType)
                    {
                        if (item.Number >= buttonItemData.consumeItemNumber)
                        {
                            OwnedItemsData.Instance.Use(buttonItemData.itemType, buttonItemData.consumeItemNumber);
                        }
                        else
                        {
                            // 一つでも条件を満たさない場合はフラグを false に設定してループを終了
                            allConditionsMet = false;
                            break;
                        }
                    }

                if (!allConditionsMet)
                {
                    break;
                }
            }

            // 消費アイテムの全てを消費数より多く持っていた場合にのみ処理を実行
            if (allConditionsMet)
            {
                playerControl.ChangeState(stateMaking);
                _makeMchineUI.CloseRecipeDialog();

            }
        }
    }

    void SelectButton(int index)
    {
        Debug.Log("indexx" + index);
        var colors = menuButtons[index].colors;
        colors.selectedColor = Color.red;
        colors.selectedColor = selectedColor;

        menuButtons[index].colors = colors;
        menuButtons[index].Select();

    }

    void DisabledButton(int index)
    {
        var colors = menuButtons[index].colors;
        colors.normalColor = disabledColor;
        if (menuButtons[index].colors.normalColor == disabledColor)
        {
            colors.selectedColor = selectedDisavledColor;
        }
        menuButtons[index].colors = colors;
    }
}
