using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOConversation", menuName = "Dialogue/SOConversation")]
public class SOConversation : ScriptableObject
{
    public List<SOSentence> conversation = new List<SOSentence>();
    public GameEvent OnEndConversation;
}