using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xcon : MonoBehaviour
{
    void Update()
    {
        Vector3 camPos = Camera.main.transform.position;
        Vector3 rotVec = camPos - transform.position;
        transform.rotation = Quaternion.LookRotation(rotVec);        
    }
}
