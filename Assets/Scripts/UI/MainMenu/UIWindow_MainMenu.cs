﻿using UnityEngine.UI;

public class UIWindow_MainMenu : UIWindow
{
    private void Awake()
    {
        transform.FindChildComponentByName<Button>("Btn_SelectLevel").onClick.AddListener(() =>
        {
            CSceneManager.LoadScene("MainScene");
        });

        transform.FindChildComponentByName<Button>("Btn_Setting").onClick
            .AddListener(() => UIManager.Instance.OpenWindow(UIWindowID.Setting));

        transform.FindChildComponentByName<Button>("Btn_About").onClick
            .AddListener(() => UIManager.Instance.OpenWindow(UIWindowID.About));

        transform.FindChildComponentByName<Button>("Btn_Exit").onClick
            .AddListener(() => GameManager.Instance.QuitGame());
    }
}
