using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntermediateSceneController : MonoBehaviour
{
    public event Action<float> OnSceneLoadProgressChanged;

    private void Start()
    {
        StartCoroutine(_LoadScene(CSceneManager.NextScene));
    }

    private IEnumerator _LoadScene(string scene)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(scene);
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
        {
            if (OnSceneLoadProgressChanged != null) OnSceneLoadProgressChanged(op.progress);
            yield return null;      
        }

        TriggerEvent(1f);
        yield return null;

        CSceneManager.NextScene = "";

        op.allowSceneActivation = true;
    }

    private void TriggerEvent(float progress)
    {
        if (OnSceneLoadProgressChanged != null)
            OnSceneLoadProgressChanged(progress);
    }
}
