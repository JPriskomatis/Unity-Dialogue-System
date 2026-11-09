using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "SOSentence", menuName = "Dialogue/SOSentence")]
public class SOSentence : ScriptableObject
{
    //I've made a custom editor script (SOSentenceEditor.cs) that handles showing the npcName ONLY if the sentenceOwner is an NPC, otherwise it sets it to "Player";
    public string npcName;
    public LocalizedString sentence;
    public EventAction OnSentenceStart;
    public EventAction OnSentenceEnd;

    public SentenceOwner sentenceOwner;


    public enum SentenceOwner
    {
        NPC,
        Player
    }

    [Serializable]
    public struct EventAction
    {
        public GameEvent OnEvent;
        public float delay;
    }
    private void OnValidate()
    {
        if (sentenceOwner == SentenceOwner.Player)
        {
            npcName = "Player";
        }
    }
}