using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sallyblink : MonoBehaviour
{
    private SkinnedMeshRenderer sallyMesh;
    private bool isBlink = false;				
    private bool isEyeOpen = true;
    private float timePassed = 0.0f;
    public float blinkTime = 0.4f;				
    public float noBlinkChance = 0.4f;				
    public float blinkInterval = 3.0f;
    public float closingSpeed = 2.0f;
    public float timeWithClosed = 0.3f;

    public int eyeclosedMeshIndex = 11;
    void Start ()
    {
        sallyMesh = GameObject.Find("Body").GetComponent<SkinnedMeshRenderer>();
        StartCoroutine ("RandomChange");
    }

     void Update ()
    {
        if (isBlink) {
            if (isEyeOpen) {
                SmoothClose();
            } else {
                SmoothOpen();  
            }
        }
    }

    void SmoothClose() {
        timePassed += Time.deltaTime * closingSpeed;
        float eyeClosure = 100 * (timePassed / blinkTime);
        if (eyeClosure >= 100) {
            eyeClosure = 100;
        } 
        sallyMesh.SetBlendShapeWeight (eyeclosedMeshIndex, eyeClosure);
        if (timePassed >= blinkTime + timeWithClosed) {
            isEyeOpen = false;
            timePassed = 0.0f;
        }
    }

    void SmoothOpen () {
        timePassed += Time.deltaTime;
        float eyeClosure = 100 * (blinkTime - timePassed / blinkTime); 
        if (eyeClosure <= 0) {
            eyeClosure = 0;
        }
        sallyMesh.SetBlendShapeWeight (eyeclosedMeshIndex, eyeClosure);
        if (timePassed >= blinkTime) {
            isEyeOpen = true;
            isBlink = false;
            timePassed = 0.0f;
        }
    }
    IEnumerator RandomChange ()
    {
        while (true) {
            float rndf = Random.Range (0.0f, 1.0f);
            if (!isBlink && rndf > noBlinkChance) {
                isBlink = true;
            }
            yield return new WaitForSeconds (blinkInterval);
        }
    }
}

