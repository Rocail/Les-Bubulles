using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //projectile = Resources.Load("Bubble") as GameObject;
    }

    // Update is called once per frame
    public GameObject shot;
    public Transform shotSpawn;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        }
    }
    /*void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            Vector3 sp = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 dir = (Input.mousePosition - sp).normalized;
       
            GetCompenant<rigidbody2D>().AddForce(dir * 500);

            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        } */
}