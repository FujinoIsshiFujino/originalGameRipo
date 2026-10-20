using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Recipe : MenuBase
{
    [SerializeField] MenuBase _menuBase;
    MakeButtoon _makeButtoon;
    public MakeButtoon.makeItemType selectedMakeItemType;
    [SerializeField] MainMenu _mainMenu;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform viewport;
    [SerializeField] private Transform content;

    void Start()
    {
        base.Start(); // 基底クラスのStartメソッドを呼び出す
        this.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        // 更新前の選択位置を記録
        int previousIndex = selectedButtonIndex;

        // 共通のボタン選択処理
        base.Update();

        // 選択位置が変わったら、必要に応じてスクロール
        if (selectedButtonIndex != previousIndex &&
            selectedButtonIndex >= 0 &&
            selectedButtonIndex < menuButtons.Length)
        {
            ScrollToSelectedButton(menuButtons[selectedButtonIndex]);
        }

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

    private void ScrollToSelectedButton(Button button)
    {
        if (scrollRect == null ||
            viewport == null ||
            scrollRect.content == null ||
            button == null)
        {
            Debug.LogWarning("ScrollRect、Viewport、Content、またはButtonが未設定です。");
            return;
        }

        // レシピボタン以外はスクロール対象外
        if (!button.TryGetComponent<MakeButtoon>(out _))
        {
            return;
        }

        RectTransform target = button.GetComponent<RectTransform>();
        RectTransform contentRect = scrollRect.content;

        // レイアウトを最新の状態にする
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
        Canvas.ForceUpdateCanvases();

        // 選択ボタンの範囲をViewportの座標系で取得
        Bounds bounds =
            RectTransformUtility.CalculateRelativeRectTransformBounds(
                viewport,
                target
            );

        float bottom = viewport.rect.min.y;
        float top = viewport.rect.max.y;

        float moveY = 0f;

        // ボタンがViewportの下にはみ出している
        if (bounds.min.y < bottom)
        {
            moveY = bottom - bounds.min.y;
        }
        // ボタンがViewportの上にはみ出している
        else if (bounds.max.y > top)
        {
            moveY = top - bounds.max.y;
        }

        // 画面内に収まっていれば何もしない
        if (Mathf.Approximately(moveY, 0f))
        {
            return;
        }

        // Contentを移動する。ScrollRectがスクロールバーも更新する
        Vector2 position = contentRect.anchoredPosition;
        position.y += moveY;
        contentRect.anchoredPosition = position;
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
                    makeButton.CanMake = true;
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
            if (makeButton.CanMake)
            {
                _makeButtoon = makeButton;
                selectedMakeItemType = makeButton.type;
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
        // Content内のレシピボタンだけを取得
        Button[] recipeButtons =
            content.GetComponentsInChildren<Button>(true)
            .Where(button =>
                button.TryGetComponent<MakeButtoon>(out _))
            .ToArray();

        // menuButtonsの最後はResumeボタンとする
        Button resumeButton =
            menuButtons[menuButtons.Length - 1];

        // レシピボタン + Resumeボタンで配列を作り直す
        Button[] newMenuButtons =
            new Button[recipeButtons.Length + 1];

        for (int i = 0; i < recipeButtons.Length; i++)
        {
            newMenuButtons[i] = recipeButtons[i];
        }

        newMenuButtons[newMenuButtons.Length - 1] =
            resumeButton;

        menuButtons = newMenuButtons;

        // 選択位置が配列の範囲外なら調整
        if (selectedButtonIndex >= menuButtons.Length)
        {
            selectedButtonIndex = menuButtons.Length - 1;
        }
    }
}
