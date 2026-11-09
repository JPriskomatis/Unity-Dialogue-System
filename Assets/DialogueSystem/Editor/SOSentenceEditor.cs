using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SOSentence))]
public class SOSentenceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var sentenceOwner = serializedObject.FindProperty("sentenceOwner");
        var npcName = serializedObject.FindProperty("npcName");

        EditorGUILayout.PropertyField(sentenceOwner);

        if ((SOSentence.SentenceOwner)sentenceOwner.enumValueIndex ==
            SOSentence.SentenceOwner.NPC)
        {
            EditorGUILayout.PropertyField(npcName);
        }
        else
        {
            npcName.stringValue = "Player";
        }

        DrawPropertiesExcluding(serializedObject,
            "m_Script", "sentenceOwner", "npcName");

        serializedObject.ApplyModifiedProperties();
    }
}