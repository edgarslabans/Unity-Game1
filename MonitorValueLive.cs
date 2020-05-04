using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorValueLive : MonoBehaviour
{
    public static int rotZ = 0;
 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotZ = (int)transform.rotation.eulerAngles.z;
    }
}
