using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class test : MonoBehaviour
{

    public CinemachineVirtualCamera one;
    public CinemachineVirtualCamera two;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (one.Priority == 1)
            {
                one.Priority = 2;
                two.Priority = 1;
            }
            else
            {
                one.Priority = 1;
                two.Priority = 2;
            }
        }
    }
}
