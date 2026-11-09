using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TestDialogueScript : MonoBehaviour
{
    public UnityEvent OnStartDialogue;
     public void ActivateDialogue()
    {
        OnStartDialogue?.Invoke();
    }

}
