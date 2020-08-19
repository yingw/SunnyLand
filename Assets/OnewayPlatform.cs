using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnewayPlatform : MonoBehaviour
{
    PlatformEffector2D effector;
    private float waittime = 0.5f;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && !Input.GetKey(KeyCode.DownArrow))
        {
            effector.rotationalOffset = 0f;
        }
        // if (Input.GetKey(KeyCode.DownArrow))
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetButtonDown("Jump"))
        {
            effector.rotationalOffset = 180f;
            // if (waittime <= 0f)
            // {
            //     effector.rotationalOffset = 180f;
            //     waittime = 0.5f;
            // }
            // else
            // {
            //     waittime -= Time.deltaTime;
            // }
        }
    }
}
