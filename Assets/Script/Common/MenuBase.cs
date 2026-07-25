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
    [SerializeField] private GameObject backPanel;
    [SerializeField] GameObject Player;
    PlayerControl _playerControl;
    CharacterController _characterController;

    protected virtual void Start()
    {
        // 最初のボタンを選択状態にする
        SelectButton(selectedButtonIndex);
        stateMaking = new PlayerControl.StateMaking();

        _playerControl = Player.GetComponent<PlayerControl>();
        _characterController = Player.GetComponent<CharacterController>();
    }

    //  オブジェクトがアクティブになるたびに実行されるメソッド
    // メニュー系を開いたときになにかしらのボタンが選択されている状態
    protected virtual void OnEnable()
    {
        SelectButton(selectedButtonIndex);
    }

    //ボタンの選択や決定
    protected virtual void Update()
    {
        float input = Input.GetAxis("Vertical");

        if ((input <= 1 && VerticalDepth < input) || Input.GetKeyDown("w"))
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
                previousInputWasUp = true; //連続入力の制御
            }
        }
        else
        {
            previousInputWasUp = false;
        }

        if ((input >= -1 && -VerticalDepth > input) || Input.GetKeyDown("s"))
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
                previousInputWasDown = true; //連続入力の制御
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
        // EventSystem 経由で選択状態を切り替える
        for (int i = 0; i < menuButtons.Length; i++)
        {
            if (i == index)
            {
                if (menuButtons[i].interactable == true)
                {
                    menuButtons[i].Select(); // ← これだけで selectedColor になる
                }
            }
        }
    }

    // Toggleは親メニューがあった場合に、親メニューに手を加えずに子メニューを開くときに使う
    //CloseMenuやOpenMenuは親メニュー自体に。時間も止める。
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
    public void CloseMenu(GameObject menu)
    {
        Time.timeScale = 1;

        menu.SetActive(false);

        _characterController.enabled = true;
        _playerControl.enabled = true;

        backPanel.SetActive(true);
    }

    public void OpenMenu(GameObject menu)
    {
        Time.timeScale = 0;

        menu.SetActive(true);

        _characterController.enabled = false;
        _playerControl.enabled = false;

        backPanel.SetActive(false);
    }
}