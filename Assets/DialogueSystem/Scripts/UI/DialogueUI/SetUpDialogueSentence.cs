using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class SetUpDialogueSentence : MonoBehaviour
{
    [Header("Dialogue UI Components")]
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private TextMeshProUGUI sentenceOwnerText;

    [SerializeField] private TypewriterEffect typeWriter;
    [SerializeField] private LocalizeStringEvent sentenceText;


    // I subcribed the events in the Start isntead of the OnEnable because there was a weird bug that sometimes
    // the SetUpDialogueSentence OnEnable was called BEFORE the DialogueManager Awake method, so the Instance was
    // not created and the event could not register, meaning that these event listeners were empty.
    private void Start()
    {
        DialogueManager.Instance.OnInitializeConversation += EnableUI;
        DialogueManager.Instance.OnSentenceChange += SetUpSentence;
        DialogueManager.Instance.OnFinishConversation += DisableUI;

        DialogueManager.Instance.OnRequestAutocompleteSentence += AutoCompleteSentence;
        DialogueManager.Instance.OnCheckSentenceFinished += FinishedSentence;
    }

    private void OnDestroy()
    {
        DialogueManager.Instance.OnInitializeConversation -= EnableUI;
        DialogueManager.Instance.OnSentenceChange -= SetUpSentence;
        DialogueManager.Instance.OnFinishConversation -= DisableUI;

        DialogueManager.Instance.OnRequestAutocompleteSentence -= AutoCompleteSentence;
        DialogueManager.Instance.OnCheckSentenceFinished -= FinishedSentence;
    }

    private void DisableUI()
    {
        dialogueUI.SetActive(false);
    }

    private void EnableUI()
    {
        dialogueUI.SetActive(true);
    }

    public void SetUpSentence(SOSentence sentence)
    {
        //First we grab the localized string and set it in the LocalizeStringEvent, and then we pass it to the typewriter to showcase the text;
        sentenceOwnerText.text = sentence.npcName + ": ";
        sentenceText.StringReference = sentence.sentence;

        typeWriter.StartTypewriter(sentenceText.StringReference.GetLocalizedString());

    }

    public bool FinishedSentence()
    {
        return typeWriter.IsTypewriterFinished();
    }
    public void AutoCompleteSentence()
    {
        typeWriter.AutoCompleteSentence();
    }
}