using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float maxSpeed = 5f;
    public GameObject arrowRotation;
    //float bubbleRadius = 0.5f;
    //float rotationZ;
    private Vector3 position;
    private Vector3 mooving;
    public Rigidbody2D rb;

    private bool wallTouch;

    // Start is called before the first frame update
    void Start()
    {
        wallTouch = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (wallTouch == false)
        {
            position = transform.position;
            mooving = new Vector3(0, maxSpeed * Time.deltaTime, 0);

            position += transform.rotation * mooving;
            transform.position = position;
        }
        else
        {
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Wall_Left")
        {
            wallTouch = true;
            print("Mur gauche touché");
        }
    }
}
