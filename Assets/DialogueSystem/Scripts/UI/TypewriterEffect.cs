using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
public class TypewriterEffect : MonoBehaviour
{

    [Header("Text Display Settings")]
    [SerializeField] private float timePerChar = 0.05f;

    private TextMeshProUGUI tmpText;
    private int charIndex = 0;

    private string currentMsg = null;

    private CancellationTokenSource tokenSource = new();

    private void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
    }


    public void StartTypewriter(string sentence)
    {
        // We want to cancel the previous typewriter animation, without that, both typewriter operations would run simultaneously (if we skipped to the next sentence
        // while the previous one did not stop;
        tokenSource?.Cancel();
        tokenSource?.Dispose();

        tokenSource = new CancellationTokenSource();

        TypewriterText(sentence).Forget();
    }
    async UniTask TypewriterText(string sentence)
    {
        if (string.IsNullOrEmpty(sentence)) return;


        currentMsg = sentence;

        while (charIndex != currentMsg.Length)
        {

            charIndex++;

            tmpText.text = currentMsg.Substring(0, charIndex);

            await UniTask.WaitForSeconds(timePerChar, cancellationToken: tokenSource.Token);
        }

        charIndex = 0;
        currentMsg = null;

    }

    public bool IsTypewriterFinished()
    {
        if (charIndex == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void AutoCompleteSentence()
    {
        if (string.IsNullOrEmpty(currentMsg))
            return;

        tokenSource?.Cancel();

        tmpText.text = currentMsg;

        charIndex = 0;
        currentMsg = null;
    }

}