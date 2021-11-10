using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class PlayerInteract : MonoBehaviour
{

    public CinemachineVirtualCamera cam;

    public void interact()
    {
        cam.gameObject.SetActive(true);
    }

    public void doneInteracting()
    {
        cam.gameObject.SetActive(false);
    }
}
