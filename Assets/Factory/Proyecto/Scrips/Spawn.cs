using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    private float movSpeed = 10;
    void Update()
    {
        float movX = Input.GetAxis("Horizontal") * movSpeed * Time.deltaTime;
        float movZ = Input.GetAxis("Vertical") * movSpeed * Time.deltaTime;

        transform.Translate(-movX, -movZ, 0);
    }
}
