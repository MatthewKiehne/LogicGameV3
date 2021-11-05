using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]

public class CameraInteract : MonoBehaviour
{    private Camera cam;

    [SerializeField]
    private Text interactText;

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
            interactText.gameObject.SetActive(true);
        }
        else
        {
            interactText.gameObject.SetActive(false);
        }
    }
}
