using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TestUnityEvent : MonoBehaviour
{

    [SerializeField] private UnityEvent myEvent = new UnityEvent();

    void Start()
    {

        //スタート時にテスト用の関数を実行
        TestFunc();

    }

    public void TestFunc()
    {

        //myEventに登録されている関数を実行
        myEvent.Invoke();

    }
}