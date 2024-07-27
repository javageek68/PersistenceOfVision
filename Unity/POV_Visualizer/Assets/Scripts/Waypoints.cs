using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public GameObject[] waypoints;
    public string waypointFile;
    public string lineEndings;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private Vector3 TransformToSpace(Vector3 vec)
    {
        Vector3 v3RetVal = vec;

        return v3RetVal;
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 100), "Generate Waypoint File"))
        {
            GenerateWaypointFile();
        }
    }

    private void GenerateWaypointFile()
    {
        string content = string.Empty;
        foreach (GameObject go in waypoints)
        {
            Vector3 v3Transformed = this.TransformToSpace(go.transform.position);
            content += $"{v3Transformed.x}, {v3Transformed.y}, {v3Transformed.z} ,1,1\r\n";
        }
        System.IO.File.WriteAllText(this.waypointFile, content);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
