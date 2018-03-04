using UnityEngine;
using UnityEngine.UI;

public class UIProgressShow : MonoBehaviour
{
    private Slider _slider;

    private void Awake()
    {

        _slider = transform.FindChildComponentByName<Slider>("Slider");
        Camera.main.GetComponent<IntermediateSceneController>().OnSceneLoadProgressChanged += v => { _slider.value = v; };
    }
}
