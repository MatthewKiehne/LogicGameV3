using UnityEngine;
using TMPro;

[RequireComponent(typeof(Camera))]

public class CameraInteract : MonoBehaviour
{
    private Camera cam;
    //private Camera cam;
    [SerializeField]
    private TMP_Text interactText;


    void Start()
    {
        this.cam = this.gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = this.cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Interact")
        {
            InteractController interactController = hit.transform.gameObject.GetComponent<InteractController>();
            if (interactController != null)
            {
                this.interactText.text = interactController.HighlightText;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    interactController.CallMouseDown(hit);
                }
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    interactController.CallMouseHeld(hit);
                }
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    interactController.CallMouseUp(hit);
                }
                if (!Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKeyUp(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Mouse0))
                {
                    interactController.CallMouseHover(hit);
                }
            }
        }
    }
}
