using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    public float speed = 3f;

    void Start()
    {
        
    }

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        transform.position += new Vector3(0, verticalInput * speed * Time.deltaTime, 0);
    }
}
