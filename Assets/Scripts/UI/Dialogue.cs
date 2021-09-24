using UnityEngine;
using System.Collections.Generic;

public class Dialogue : MonoBehaviour
{
    public delegate void ShowDialogue(TextAsset text, TextAsset key);
    public ShowDialogue showDialogue;
    public ShowDialogue showKey;
    public TextAsset key;
    public TextAsset text;

    public void ActivateDialogue()
    {
        showDialogue(text, key);
    }
}
