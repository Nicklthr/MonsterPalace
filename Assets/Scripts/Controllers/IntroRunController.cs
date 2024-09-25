using Michsky.UI.Dark;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IntroRunController : MonoBehaviour
{
    [SerializeField] private UnityEvent onIntroStart;
    [SerializeField] private UnityEvent onIntroEnd;
    [SerializeField] private float introDuration = 4f;
    [SerializeField] private UIDissolveEffect transitionHelper;
    [SerializeField] private MainPanelManager mainPanelManager;

    public void StartIntro()
    {
        StartCoroutine(IntroCoroutine());
    }

    private IEnumerator IntroCoroutine()
    {
        onIntroStart.Invoke();
        mainPanelManager.FadeOutAllPanels();
        yield return new WaitForSeconds(introDuration);
        transitionHelper.DissolveIn();
        yield return new WaitForSeconds(2f);
        onIntroEnd.Invoke();
        Debug.Log("Intro terminée");
    }
}