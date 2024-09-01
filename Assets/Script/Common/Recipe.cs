using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : MenuBase
{
    [SerializeField] MenuBase _menuBase;
    MakeButtoon _makeButtoon;
    public MakeButtoon.makeItemType selectedMakeItemType;
    [SerializeField] MainMenu _mainMenu;

    void Start()
    {
        base.Start(); // 基底クラスのStartメソッドを呼び出す
        this.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Dash"))
        {
            //ポーズ画面からrecipeを開いたかどうかで分岐
            if (!_mainMenu.isActiveAndEnabled)
            {
                _menuBase.CloseMenu(this.gameObject);
            }
            else
            {
                _mainMenu.currentFaze = MainMenu.faze.pause;
                Toggle(this.gameObject);
            }
        }
    }

    //選択していないボタンのノーマルカラーを決定
    protected override ColorBlock ButtonsColor(int index, ColorBlock colors)
    {
        // 所持アイテムが消費アイテムに数に足りないとき
        if (menuButtons[index].TryGetComponent<MakeButtoon>(out var makeButton))
        {
            _makeButtoon = makeButton;
            Debug.Log(_makeButtoon + "_makeButtoon");
        }

        if (_makeButtoon)
        {
            foreach (var buttonItemData in _makeButtoon.itemDataArray)
            {
                // OwnedItemsData.Instance.OwnedItemsは所持しているアイテムの種類　それぞれのボタンに対応したアイテムを持っているか否かを見てボタンの色を変える
                foreach (var item in OwnedItemsData.Instance.OwnedItems)
                {
                    if (item.Type == buttonItemData.itemType)
                    {
                        if (item.Number < buttonItemData.consumeItemNumber)
                        {
                            colors.normalColor = disabledColor;
                        }
                    }
                    else
                    {
                        colors.normalColor = normalColor;
                    }
                }
            }
        }

        // resumeボタン用
        if (index == 3)
        {
            colors.normalColor = normalColor;
        }

        return colors;
    }

    protected override void DecisionAction()
    {
        // resume
        //ポーズ画面からrecipeを開いたかどうかで分岐
        if (selectedButtonIndex == 3)
        {
            if (!_mainMenu.isActiveAndEnabled)
            {
                _menuBase.CloseMenu(this.gameObject);
                return;
            }
            else
            {
                _mainMenu.currentFaze = MainMenu.faze.pause;
                Toggle(this.gameObject);
                return;
            }
        }

        //MakeButtoonを持つオブジェクトから取得、resumeボタンは取得しない
        if (menuButtons[selectedButtonIndex].TryGetComponent<MakeButtoon>(out var makeButton))
        {
            _makeButtoon = makeButton;
            selectedMakeItemType = _makeButtoon.type;
        }

        if (_makeButtoon)
        {
            bool allConditionsMet = true;
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
                            allConditionsMet = false;
                            break;
                        }
                    }

                if (!allConditionsMet)
                {
                    break;
                }
            }

            if (allConditionsMet)
            {
                //ポーズ画面からrecipeを開いたかどうかで分岐
                if (!_mainMenu.isActiveAndEnabled)
                {
                    _menuBase.CloseMenu(this.gameObject);
                }
                else
                {
                    _mainMenu.currentFaze = MainMenu.faze.pause;
                    Toggle(this.gameObject);
                    _menuBase.CloseMenu(_mainMenu.gameObject);
                }
                playerControl.ChangeState(stateMaking);
            }
        }
    }
}
