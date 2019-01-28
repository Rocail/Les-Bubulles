
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterControler : MonoBehaviour
{
    private float rotationSpeed = 80.0f;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.back * this.rotationSpeed * Time.deltaTime);
            if (transform.eulerAngles.z < 275.0 && transform.eulerAngles.z > 180.0)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 275.0f);
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * this.rotationSpeed * Time.deltaTime);
            if (transform.eulerAngles.z > 85.0 && transform.eulerAngles.z < 180.0)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 85.0f);
            }
        }
    }
}
