using UnityEngine;
using UnityEngine.UI;

public class UIWindow_GameMain : UIWindow
{
    [SerializeField] private Text _canIntrudeAmountText;

    [SerializeField] private Text _canIntrudeAmountMaxText;

    [SerializeField] private Text _playerGold;

    private void Start()
    {
        _canIntrudeAmountMaxText = transform.FindChildComponentByName<Text>("Text_ValueMax");
        _canIntrudeAmountText = transform.FindChildComponentByName<Text>("Text_Value");
        _playerGold = transform.FindChildComponentByName<Text>("Text_GoldValue");

        _playerGold.text = GameController.Instance.GetGold().ToString();
    }

    private void Update()
    {
        _canIntrudeAmountText.text =
            (GameController.Instance.intrudeTop - GameController.Instance.intrudeCount).ToString();
        _canIntrudeAmountMaxText.text = GameController.Instance.intrudeTop.ToString();
        _playerGold.text = GameController.Instance.GetGold().ToString();
    }
}