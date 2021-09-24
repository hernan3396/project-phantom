using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    private int currentStep = 0;
    public GameObject[] disableOnTutorial;
    public UnityEvent[] tutorialEvents;
    private KeyCode[] keyCode = new KeyCode[2]; // 0 = change char , 1 = jump button

    private void Start()
    {
        tutorialEvents[0]?.Invoke();

        keyCode[0] = KeybindingsManager.GetInstance.GetChangeCharactersButton;
        keyCode[1] = KeybindingsManager.GetInstance.GetJumpButton;
    }

    public void PlayEvent(int step)
    {
        currentStep = step;
        tutorialEvents[step]?.Invoke();
    }

    public void DisableOnTutorial()
    {
        foreach (GameObject disable in disableOnTutorial)
        {
            disable.SetActive(false);
        }
    }

    public void WaitForKeyPress(int value)
    {
        KeyCode key = keyCode[value];
        StartCoroutine(WaitingForKeyPress(key));
    }

    IEnumerator WaitingForKeyPress(KeyCode key)
    {
        while (!Input.GetKeyDown(key))
        {
            yield return null;
        }

        PlayEvent(currentStep + 1);
    }
}
