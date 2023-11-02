using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDMovement : MonoBehaviour {

    [SerializeField] private float speed; 
    private Rigidbody playerRB;

    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    void FixedUpdate () 
    {
        float movX = Input.GetAxis("Horizontal") * speed;
        float movZ = Input.GetAxis("Vertical") * speed;

        Movement(movX, movZ);
    }

    public void Movement(float movX, float movZ) 
    {
        playerRB.AddForce(new Vector3(movX, 0, movZ));
    }
}
