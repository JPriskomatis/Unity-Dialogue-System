using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameEventResponse
{
    public GameEvent Event;
    public UnityEvent Response;
}

public class GameEventListener : MonoBehaviour
{
    public GameEventResponse[] EventResponses;

    private void OnEnable()
    {
        foreach (var response in EventResponses)
        {
            response.Event.RegisterListener(this);
        }
    }

    private void OnDisable()
    {
        foreach (var response in EventResponses)
        {
            response.Event.UnregisterListener(this);
        }
    }

    public void OnEventRaised(GameEvent raisedEvent)
    {
        foreach (var response in EventResponses)
        {
            if (response.Event == raisedEvent)
            {
                response.Response?.Invoke();
                break;
            }
        }
    }
}