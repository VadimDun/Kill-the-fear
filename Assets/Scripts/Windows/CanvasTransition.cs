using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasTransition : MonoBehaviour
{
    
    private Animator animator;

    private TriggerScript triggerScript;

    private string SceneName;

    private void Start()
    {
        animator = GameObject.Find("LevelChanger").GetComponent<Animator>();
    }

    public void StartTransitionFadein()
    {
        animator.Play("Fadein");
    }

    public void StartTransitionFade()
    {
        animator.Play("Fade");
    }

    public void StartTransitionFadeOut()
    {
        animator.Play("FadeOut");
    }

    public void OnFadeComplite()
    {
        triggerScript = GameObject.FindGameObjectWithTag("ChangeSceneTrigger").GetComponent<TriggerScript>();
        SceneName = triggerScript.GetSceneName;

        Debug.Log(SceneName);

        StartTransitionFade();

        SceneManager.LoadScene(SceneName);
    }
}
