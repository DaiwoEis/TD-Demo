using UnityEngine;
using UnityEngine.EventSystems;

public class InputDetector : MonoBehaviour
{
    private UIBuyDefender buyDefender;
    private UISettingDefender defenderSetting;

    public LayerMask towerLayer;

    private void Awake()
    {
        buyDefender = FindObjectOfType<UIBuyDefender>();
        defenderSetting = FindObjectOfType<UISettingDefender>();
    }

    private void Update()
    {
        DisplayUIs();
        OnEscape();
    }

    private void DisplayUIs()
    { 
        if (Input.GetMouseButtonUp(0) && IsPointerOverUI() == false)
        { 
            buyDefender.Hide();
            defenderSetting.Hide();
 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, towerLayer))
            {
                Tower hs = hit.transform.GetComponent<Tower>();

                if (hs.currDefender == null)
                    buyDefender.Show(hs);
                else
                    defenderSetting.Show(hs);
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

    private void OnEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            GameController.Instance.GamePause();
    }
}
