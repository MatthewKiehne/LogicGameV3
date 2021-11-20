using UnityEngine;
using UnityEngine.Events;

public class InteractController : MonoBehaviour
{
    public UnityEvent MouseDown = new UnityEvent();
    public UnityEvent MouseHeld = new UnityEvent();
    public UnityEvent MouseUp = new UnityEvent();
    public string HighlightText;

    public void CallMouseDown() {
        if(this.MouseDown != null) {
            this.MouseDown.Invoke();
        }
    }

    public void CallMouseHeld() {
        if(this.MouseHeld != null) {
            this.MouseHeld.Invoke();
        }
    }

    public void CallMouseUp() {
        if(this.MouseUp != null) {
            this.MouseUp.Invoke();
        }
    }
}
