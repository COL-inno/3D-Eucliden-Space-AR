using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myParticle : MonoBehaviour
{
    public GameObject CylinderMask;
    public bool isMaskStopMoving = false; 
    private bool isParticleFinished = false;
    private ParticleSystem ps; 
    
    void Start() {
        ps = GetComponent<ParticleSystem>();
        ps.Stop();
    }
    void Update()
    {
        if (!isParticleFinished) {
            Vector3 cylPos = CylinderMask.transform.position;
            transform.position = 
                new Vector3(cylPos.x, cylPos.y - CylinderMask.transform.localScale.y, cylPos.z);      
            if (isMaskStopMoving)
                changeParticleSystem();
        }
    }
    void changeParticleSystem() {
        var main = ps.main;
        var shape = ps.shape;
        
        main.startSpeed = 3;
        main.simulationSpeed = 1;
        main.startLifetime = 1.5f;
        shape.shapeType = ParticleSystemShapeType.Donut;
        isParticleFinished = true;
    }
    public void revertSettings() {
        var main = ps.main;
        var shape = ps.shape;
        
        main.startSpeed = 0;
        main.simulationSpeed = 3;
        main.startLifetime = 1.0f;
        shape.shapeType = ParticleSystemShapeType.Circle;
        isParticleFinished = false;
        isMaskStopMoving = false;
    }
}
