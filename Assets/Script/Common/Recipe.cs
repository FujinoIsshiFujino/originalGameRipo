using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Linq;

public class Recipe : MenuBase
{
    [SerializeField] MenuBase _menuBase;
    MakeButtoon _makeButtoon;
    public MakeButtoon.makeItemType selectedMakeItemType;
    [SerializeField] MainMenu _mainMenu;
    [SerializeField] GameObject mainMenu;
    [SerializeField] Scrollbar scrollbar;
    public float scrollbarPosition;

    [SerializeField] private Transform content;
    private UnityEngine.UI.Button[] contentArry;

    void Start()
    {
        base.Start(); // 基底クラスのStartメソッドを呼び出す
        this.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

        // スクロールバーの位置を決定（バーの位置だけなので、ボタンの色などは関係ない）
        if (selectedButtonIndex == 0)
        {
            scrollbarPosition = 1.0f;
        }
        else if (selectedButtonIndex == (menuButtons.Length - 2))
        {
            scrollbarPosition = 0;
        }
        else if (selectedButtonIndex == (menuButtons.Length - 1))
        {
            // 何も書かないが、書かないことによってスクロールバーの位置をとどまらせる
        }
        else
        {
            scrollbarPosition = 1.0f / (((menuButtons.Length - 1) - 2) + 1);
            scrollbarPosition = 1 - (scrollbarPosition * selectedButtonIndex);
        }
        scrollbar.value = scrollbarPosition;

        //recipeを閉じるときの処理
        if (Input.GetButtonDown("Dash"))
        {
            // メニューを経由した場合
            if (_mainMenu.beforeFaze == MainMenu.faze.pause && _mainMenu.currentFaze == MainMenu.faze.recipe)
            {
                // MainMenuに記述
            }
            // していない場合
            else
            {
                _mainMenu.currentFaze = MainMenu.faze.none;
                _mainMenu.beforeFaze = MainMenu.faze.none;
                _menuBase.CloseMenu(this.gameObject);
            }
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        InitButtonsColor();
    }

    void InitButtonsColor()
    {
        foreach (var button in menuButtons)
        {
            // MakeButtoon を持つボタンのみ対象
            if (!button.TryGetComponent<MakeButtoon>(out var makeButton))
            {
                continue;
            }

            makeButton.CanMake = true;

            // 必要なアイテムごとにチェック
            foreach (var req in makeButton.itemDataArray)
            {
                // 所持しているか？
                var owned = OwnedItemsData.Instance.OwnedItems
                    .FirstOrDefault(x => x.Type == req.itemType);

                // そのアイテムをもっていないもしくはそのアイテムを持ってはいるが量が足りない
                if (owned == null || owned.Number < req.consumeItemNumber)
                {
                    makeButton.CanMake = false;

                    break;
                }
                // 消費アイテムに対してそれ以上に持っている時
                else if (owned.Number >= req.consumeItemNumber)
                {
                    button.interactable = true;
                }
            }

            ColorBlock colors = button.colors;

            if (makeButton.CanMake)
            {
                colors.normalColor = normalColor;
                colors.selectedColor = selectedColor;
            }
            else
            {
                colors.normalColor = disabledColor;
                colors.selectedColor = selectedDisabledColor;
            }

            // 色を反映
            button.colors = colors;
        }
    }

    // recipeから何かを選択した時
    protected override void DecisionAction()
    {
        // resume
        //ポーズ画面からrecipeを開いたかどうかで分岐
        if (selectedButtonIndex == menuButtons.Length - 1)
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
            // アイテムを十分に持っている場合に、selectedMakeItemType（選択したボタンのアイテムタイプ）が更新され、それがmakeのstateで参照される
            if (menuButtons[selectedButtonIndex].interactable == true)
            {
                _makeButtoon = makeButton;
                selectedMakeItemType = _makeButtoon.type;
            }
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
                            // ここではアイテムの消費を行っているだけで実際の作成はStateMakingで行う
                            // Debug.Log("Recipe DecisionAction UseItem:" + buttonItemData.itemType);
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

        _mainMenu.beforeFaze = MainMenu.faze.none;
        _mainMenu.currentFaze = MainMenu.faze.none;
    }

    public void UpdateMenuButtons()
    {
        // すでにボタンが追加されていたら処理しない
        if (content.childCount == (menuButtons.Length - 1)) return;

        // content 内の全ボタンを取得
        contentArry = content.GetComponentsInChildren<UnityEngine.UI.Button>();

        UnityEngine.UI.Button recipeLastButton = menuButtons[menuButtons.Length - 1];

        // 新しい配列を作成し、サイズを1つ増やす
        UnityEngine.UI.Button[] newMenuButtons = new UnityEngine.UI.Button[menuButtons.Length + 1];

        // 既存の要素を新しい配列にコピー
        for (int i = 0; i < menuButtons.Length - 1; i++)
        {
            newMenuButtons[i] = menuButtons[i];
        }
        // 最後から１つ前（さいごはresumeボタン）に取得したレシピを追加
        newMenuButtons[newMenuButtons.Length - 2] = contentArry[contentArry.Length - 1];

        // 最後の要素として recipeLastButton を追加
        newMenuButtons[newMenuButtons.Length - 1] = recipeLastButton;

        // menuButtons を新しい配列に置き換え
        menuButtons = newMenuButtons;
    }
}
