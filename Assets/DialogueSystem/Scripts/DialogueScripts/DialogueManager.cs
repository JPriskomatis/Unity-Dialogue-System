using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp;
using Cysharp.Threading.Tasks;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    //Actions to inform UI about the current conversation;
    public event Action OnInitializeConversation;
    public event Action<SOSentence> OnSentenceChange;
    public event Action OnFinishConversation;

    //Actions to inform if we need to autocomplete the sentence or go to the next one;
    public event Action OnRequestAutocompleteSentence;
    public event Func<bool> OnCheckSentenceFinished;

    [Header("Player Input Settings")]
    [SerializeField] private InputActionReference nextSentence;

    private SOConversation currentConversation;
    int sentenceIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void RegisterPlayerInputListeners()
    {
        nextSentence.action.Enable();
        nextSentence.action.performed += CallPlayerSkipSentence;
    }

    private void DeregisterPlayerInputListeners()
    {
        nextSentence.action.performed -= CallPlayerSkipSentence;
        nextSentence.action.Disable();

    }



    public void InitializeConversation(SOConversation conversation)
    {

        sentenceIndex = 0;

        //We set our current conversation;
        currentConversation = conversation;
        OnInitializeConversation?.Invoke();

        //We immobilize our player and maybe point the camera to the person they speak to;
        //TO BE IMPLEMENTED;

        //Start the conversation by invoking the next sentence, which here is the first one;
        NextSentence();
        RegisterPlayerInputListeners();

    }

    private void CallPlayerSkipSentence(InputAction.CallbackContext context)
    {
        PlayerSkipSentence().Forget();
    }
    async UniTask PlayerSkipSentence()
    {
        //This returnes true if the currentSentence has finished or false if it hasn't;
        bool isFinished = OnCheckSentenceFinished?.Invoke() ?? true;

        if (!isFinished)
        {
            OnRequestAutocompleteSentence?.Invoke();
            return;
        }

        //Before proceeding to the next sentence, we fire the end of sentence event;
        if (currentConversation.conversation[sentenceIndex - 1].OnSentenceEnd.OnEvent != null)
        {
            currentConversation.conversation[sentenceIndex - 1].OnSentenceEnd.OnEvent.Raise();

            //We call the delay before proceeding to the next sentence;
            await UniTask.WaitForSeconds(currentConversation.conversation[sentenceIndex - 1].OnSentenceEnd.delay);
        }
        NextSentence();

    }

    private void NextSentence()
    {
        if (sentenceIndex == currentConversation.conversation.Count)
        {
            FinishConversation();
            return;
        }

        //We raise the event in the start of the sentence if it has;
        if (currentConversation.conversation[sentenceIndex].OnSentenceStart.OnEvent != null)
        {
            currentConversation.conversation[sentenceIndex].OnSentenceStart.OnEvent.Raise();
        }

        //The UI grabs the current SOConversation;
        OnSentenceChange?.Invoke(currentConversation.conversation[sentenceIndex]);

        sentenceIndex++;
    }

    private void FinishConversation()
    {
        //Reset the currentIndex otherwise next conversation immediately ends;
        sentenceIndex = 0;

        //Disable the UI;
        OnFinishConversation?.Invoke();

        DeregisterPlayerInputListeners();

        //Raise the OnConversationEnd event if it has any, we do that AFTER the conversation has finished;
        if (currentConversation.OnEndConversation != null)
        {
            currentConversation.OnEndConversation.Raise();
        }
    }
}