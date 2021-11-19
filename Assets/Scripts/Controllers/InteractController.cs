using UnityEngine;
using UnityEngine.Events;

public class InteractController : MonoBehaviour
{
    public UnityEvent Action;
    public string HighlightText;

    public void activate() {
        if(this.Action != null) {
            this.Action.Invoke();
        }
    }
}
