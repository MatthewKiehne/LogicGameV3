using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
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
            interactText.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                PlayerInteract interactable = hit.transform.gameObject.GetComponent<PlayerInteract>();
                if(interactable != null) {
                    interactable.interact();
                }
            }
        }
        else
        {
            interactText.gameObject.SetActive(false);
        }
    }
}
