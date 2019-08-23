using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acon : MonoBehaviour
{
    void Update()
    {
        Vector3 camPos = Camera.main.transform.position;
        Vector3 rotVec = transform.position - camPos;
        transform.rotation = Quaternion.LookRotation(rotVec);   
        
    }
}
