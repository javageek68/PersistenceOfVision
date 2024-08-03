using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public GameObject waypointParent;
    public GameObject[] waypoints;
    public string waypointFile;

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
            GenerateWaypointFile2();
        }
    }

    private void GenerateWaypointFile2()
    {
        string content = string.Empty;
        foreach(Transform child in this.waypointParent.transform)
        {
            Vector3 v3Transformed = this.TransformToSpace(child.position);
            content += $"{v3Transformed.x}, {v3Transformed.y}, {v3Transformed.z} ,1,1\r\n";
        }
        System.IO.File.WriteAllText(this.waypointFile, content);
        Debug.Log("Path CSV file generated!");
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
