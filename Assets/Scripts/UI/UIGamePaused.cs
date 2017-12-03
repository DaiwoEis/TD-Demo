using UnityEngine;
using UnityEngine.UI;

public class UIGamePaused : MonoBehaviour 
{

    private void Awake()
    {
        transform.FindChildComponentByName<Button>("Btn_Restart").onClick.AddListener(() =>
        {
            GameController.Instance.GameRestart();
        });

        transform.FindChildComponentByName<Button>("Btn_Exit").onClick.AddListener(() =>
        {
            GameController.Instance.ShutDown();
        });

        gameObject.SetActive(false);

        GameController.Instance.OnGamePaused += p =>
        {
            gameObject.SetActive(p);
        };
    }
}
