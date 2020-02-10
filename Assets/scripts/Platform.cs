using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private PlatformEffector2D effector;
    private float waitTime = 0.2f;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    private void Update()
    {
        if(Input.GetAxisRaw("Vertical") == -1)
        {
            effector.rotationalOffset = 180f;
            waitTime = 0.2f;
        }
        else if(waitTime >0)
        {
            waitTime -= Time.deltaTime;
        }
        else if(waitTime <= 0)
        {
            effector.rotationalOffset = 0f;
        }
    }

}
