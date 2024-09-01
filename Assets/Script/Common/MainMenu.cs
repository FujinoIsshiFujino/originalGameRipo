using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MenuBase
{
    [SerializeField] ItemDialog itemsDialog;
    [SerializeField] GameObject recipe;
    public enum faze
    {
        none,
        pause,
        item,
        recipe
    }
    public faze currentFaze;

    protected override void Start()
    {
        base.Start(); // 基底クラスのStartメソッドを呼び出す

        this.gameObject.SetActive(false);
        currentFaze = faze.pause;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        currentFaze = faze.pause;
    }

    protected override void Update()
    {
        base.Update();

        //どのメニューを開いているかで分岐
        if (Input.GetButtonDown("Dash"))
        {
            if (currentFaze == faze.pause)
            {
                CloseMenu(this.gameObject);
                currentFaze = faze.none;
            }
            else if (currentFaze == faze.item)
            {
                itemsDialog.Toggle();
                currentFaze = faze.pause;
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
        }
        // recipe
        else if (selectedButtonIndex == 1 && currentFaze == faze.pause)
        {
            Toggle(recipe);
            currentFaze = faze.recipe;
        }
        // 再開
        else if (selectedButtonIndex == 2 && currentFaze == faze.pause)
        {
            CloseMenu(this.gameObject);
            currentFaze = faze.none;
        }
    }
}