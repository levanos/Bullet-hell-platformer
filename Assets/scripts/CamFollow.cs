using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    private Transform playerMovement;

    public float offset;

    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        Vector3 temp = transform.position;
        temp.x = playerMovement.position.x;
        temp.x += offset;
        temp.y = playerMovement.position.y;
        transform.position = temp;
    }
}
