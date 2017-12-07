using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIWindow_GameMain : UIWindow
{
    [SerializeField] private Text _canIntrudeAmountText;

    [SerializeField] private Text _canIntrudeAmountMaxText;

    [SerializeField] private Text _playerGold;

    private UIBuyDefender _buyDefender;

    private UISettingDefender _defenderSetting;

    public LayerMask towerLayer;

    private void Start()
    {
        _canIntrudeAmountMaxText = transform.FindChildComponentByName<Text>("Text_ValueMax");
        _canIntrudeAmountText = transform.FindChildComponentByName<Text>("Text_Value");
        _playerGold = transform.FindChildComponentByName<Text>("Text_GoldValue");

        _playerGold.text = GameController.Instance.GetGold().ToString();

        _buyDefender = transform.FindChildComponentByName<UIBuyDefender>("BuyDefender");
        _defenderSetting = transform.FindChildComponentByName<UISettingDefender>("DefenderSetting");
    }

    private void Update()
    {
        _canIntrudeAmountText.text =
            (GameController.Instance.intrudeTop - GameController.Instance.intrudeCount).ToString();
        _canIntrudeAmountMaxText.text = GameController.Instance.intrudeTop.ToString();
        _playerGold.text = GameController.Instance.GetGold().ToString();

        DisplayUIs();
    }

    private void DisplayUIs()
    {
        if (Input.GetMouseButtonUp(0) && IsPointerOverUI() == false)
        {
            _buyDefender.Hide();
            _defenderSetting.Hide();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, towerLayer))
            {
                Tower hs = hit.transform.GetComponent<Tower>();

                if (hs.currDefender == null)
                    _buyDefender.Show(hs);
                else
                    _defenderSetting.Show(hs);
            }
        }
    }

    private bool IsPointerOverUI()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            return EventSystem.current.IsPointerOverGameObject();
        return false;
    }
}