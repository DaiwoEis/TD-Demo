using System;
using System.Collections;
using UnityEngine;

public class DissolveEffect : SceneChangeEffect<DissolveEffectConfigData>
{
    private bool _start;

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (_start)
            Graphics.Blit(src, dst, ConfigData.EffectMaterial);
        else
            Graphics.Blit(src, dst);
    }

    protected override IEnumerator _Run(bool reverse, Action onComplete = null)
    {
        _start = true;
        float timer = 0f;
        while (timer < configData.EffectTime)
        {
            ConfigData.EffectMaterial.SetFloat("_Magnitude", reverse ? 1f - timer / configData.EffectTime : timer / configData.EffectTime);
            yield return null;
            timer += Time.unscaledDeltaTime;
        }
        if (onComplete != null) onComplete();
        _start = false;
    }
}