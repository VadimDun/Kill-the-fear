using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Transition : MonoBehaviour
{
    private Camera mainCamera;

    private float transitionDuration = 1.0f;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private bool isTransitioning = false;

    public async Task StartTransition()
    {
        if (!isTransitioning)
        {
            isTransitioning = true;
            await TransitionCoroutine();
            isTransitioning = false;
        }
    }

    private async Task TransitionCoroutine()
    {
        float elapsedTime = 0.0f;
        Color originalColor = mainCamera.backgroundColor;
        Color targetColor = new Color(0, 0, 0, 1);

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);
            mainCamera.backgroundColor = Color.Lerp(originalColor, targetColor, t);
            await Task.Yield();
        }

        mainCamera.backgroundColor = targetColor;
    }

}
