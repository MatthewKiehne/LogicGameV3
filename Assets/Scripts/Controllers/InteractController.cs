using UnityEngine;
using UnityEngine.Events;

public class InteractController : MonoBehaviour
{
    public UnityEvent Action = new UnityEvent();
    public string HighlightText;

    public void activate() {
        if(this.Action != null) {
            Debug.Log(this.Action.GetPersistentEventCount());
            this.Action.Invoke();
        }
    }
}
