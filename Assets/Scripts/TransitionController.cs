using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{
    public static TransitionController Instance;
    public Image CanvasImage;

    public void Awake()
    {
        Instance = this;
    }

    public void StartTimeTransition()
    {
        StartCoroutine(BounceFade(0f, 1f, 2f));
    }

    public void FadeToTransition(float fromValue, float toValue, float time, float delay = 0f, Action onComplete = null)
    {
        StartCoroutine(FadeTo(fromValue, toValue, time, delay, onComplete));
    }

    IEnumerator BounceFade(float fromValue, float toValue, float time)
    {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
        {
            Color newColor;
            
            if (t <= 0.5f)
            {
                newColor = new Color(0, 0, 0, Mathf.Lerp(fromValue,toValue,t * 2));
            }
            else
            {
                newColor = new Color(0, 0, 0, Mathf.Lerp(toValue,fromValue,(t  -0.5f) * 2));
            }
            
            CanvasImage.color = newColor;
            yield return null;
        }
    }
    
    IEnumerator FadeTo(float fromValue, float toValue, float time, float delay = 0f, Action onComplete = null)
    {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / delay) {}

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
        {
            Color newColor;
 
            newColor = new Color(0, 0, 0, Mathf.Lerp(fromValue,toValue,t));
       
            
            CanvasImage.color = newColor;
            yield return null;
        }

        onComplete?.Invoke();
    }
}
