using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MenuBase
{
    [SerializeField] ItemDialog itemsDialog;
    [SerializeField] GameObject recipe;

    // フェーズは現在開かれているウィンドウによって遷移などが違うために定義
    public enum faze
    {
        none,
        pause,
        item,
        recipe
    }
    public faze currentFaze;
    public faze beforeFaze;//メニューを経由して開いたかどうかを監視

    protected override void Start()
    {
        base.Start(); // 基底クラスのStartメソッドを呼び出す

        this.gameObject.SetActive(false);
        currentFaze = faze.pause;
        beforeFaze = faze.none;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        currentFaze = faze.pause;
        beforeFaze = faze.none;
    }

    protected override void Update()
    {
        base.Update();

        //どのメニューを開いているかで分岐
        if (Input.GetButtonDown("Dash"))
        {
            if (currentFaze == faze.pause)
            {
                currentFaze = faze.none;
                beforeFaze = faze.none;
                CloseMenu(this.gameObject);
            }
            else if (currentFaze == faze.item)
            {
                itemsDialog.Toggle();
                currentFaze = faze.pause;
                beforeFaze = faze.item;
            }
            else if (currentFaze == faze.recipe && beforeFaze == faze.pause)
            {
                currentFaze = faze.pause;
                beforeFaze = faze.recipe;
                Toggle(recipe);
            }
        }
    }

    //選択していないボタンのノーマルカラーを決定
    protected override ColorBlock ButtonsColor(int index, ColorBlock colors)
    {
        colors.normalColor = normalColor;
        return colors;
    }

    protected override void DecisionAction()
    {
        // アイテム
        if (selectedButtonIndex == 0 && currentFaze == faze.pause)
        {
            itemsDialog.Toggle();
            currentFaze = faze.item;
            beforeFaze = faze.pause;
        }
        // recipe
        else if (selectedButtonIndex == 1 && currentFaze == faze.pause)
        {
            currentFaze = faze.recipe;
            beforeFaze = faze.pause;

            Toggle(recipe);
        }
        // 再開
        else if (selectedButtonIndex == 2 && currentFaze == faze.pause)
        {
            currentFaze = faze.none;
            beforeFaze = faze.none;

            CloseMenu(this.gameObject);
        }
    }
}