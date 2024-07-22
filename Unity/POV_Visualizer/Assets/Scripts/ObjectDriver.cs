using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDriver : MonoBehaviour
{
    float zMin = -15;
    float zMax = 15;
    float x = 0;
    float y = 0;
    float z = 0;
    float mag = 5;
    float speed = 1;
    float dir = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        z += dir * speed * Time.deltaTime;
        if (z > zMax || z < zMin) dir = dir * -1;
        x = mag * Mathf.Sin(z);

        this.transform.position = new Vector3(x, y, z);
        
    }
}
