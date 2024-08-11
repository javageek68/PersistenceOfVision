using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelAnimation_Ver3a : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(-Vector3.back * Time.deltaTime * 100);
    }
}
