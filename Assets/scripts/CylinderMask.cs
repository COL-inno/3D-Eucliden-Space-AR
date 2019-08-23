using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderMask : MonoBehaviour
{
    private bool isActivated = false;
    private bool isDirectionUp;
    private GameObject target;
    public GameObject particle;
    private ParticleSystem ps;
    public float movingSpeed = 0.3f;
    void Start() {
        ps = particle.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (isActivated) {
            if (ps.isStopped)
                ps.Play();
            float distFromTargetToMask = transform.position.y - target.transform.position.y;
            if (isDirectionUp) {
                transform.position += transform.up * movingSpeed * Time.deltaTime;
                if (distFromTargetToMask > transform.localScale.y * 2.6f)
                    deactivate();
            } else {
                transform.position -= transform.up * movingSpeed * Time.deltaTime;
                if (distFromTargetToMask < transform.localScale.y)
                    deactivate();
            }
        }    
    }
    public void activate(GameObject targetToCover, bool isGoingUp = true) {
        isDirectionUp = isGoingUp;
        target = targetToCover;
        Vector3 targetPos = targetToCover.transform.position;
        Vector3 targetSize = targetToCover.transform.localScale;
        transform.localScale = targetSize;
        transform.rotation = targetToCover.transform.rotation;
        if (isDirectionUp) {
            Vector3 maskOffset = transform.up * targetSize.y * 0.6f;
            transform.position = targetPos + maskOffset;
        } else {
            transform.position = new Vector3(targetPos.x, targetPos.y + transform.localScale.y * 2.9f, targetPos.z);
        }
        particle.GetComponent<myParticle>().revertSettings();
        transform.gameObject.SetActive(true);
        isActivated = true;
    }
    private void deactivate() {
        isActivated = false;
        particle.GetComponent<myParticle>().isMaskStopMoving = true;
        if (isDirectionUp)
            transform.gameObject.SetActive(false);
        else
            target.SetActive(false);
    }
}

