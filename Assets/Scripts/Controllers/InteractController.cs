using UnityEngine;
using UnityEngine.Events;

public class InteractController : MonoBehaviour
{
    public UnityEvent<RaycastHit> MouseDown = new UnityEvent<RaycastHit>();
    public UnityEvent<RaycastHit> MouseHeld = new UnityEvent<RaycastHit>();
    public UnityEvent<RaycastHit> MouseUp = new UnityEvent<RaycastHit>();
    public UnityEvent<RaycastHit> MouseHover = new UnityEvent<RaycastHit>();
    public string HighlightText;

    public void CallMouseDown(RaycastHit ray)
    {
        if (this.MouseDown != null)
        {
            this.MouseDown.Invoke(ray);
        }
    }

    public void CallMouseHeld(RaycastHit ray)
    {
        if (this.MouseHeld != null)
        {
            this.MouseHeld.Invoke(ray);
        }
    }

    public void CallMouseUp(RaycastHit ray)
    {
        if (this.MouseUp != null)
        {
            this.MouseUp.Invoke(ray);
        }
    }

    public void CallMouseHover(RaycastHit ray)
    {
        if (this.MouseHover != null)
        {
            this.MouseHover.Invoke(ray);
        }
    }
}
