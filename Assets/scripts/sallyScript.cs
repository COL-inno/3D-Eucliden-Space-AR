using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sallyScript : MonoBehaviour
{
    Animator anim;
    //CharacterController controller;
    public bool isVisible = false;
    public float size = 0.3f; 
    public bool headLookAt = true;
    public GameObject headLookTarget;
    public float horizontalMaxLookAngle = 40.0f;
    public float verticalMaxLookAngle = 25.0f; 
    private GameObject neck;
    private GameObject head;
    private float prevNeckHorAngle = 0.0f;
    private float prevNeckVerAngle = 0.0f; 
    //Fields for both input method
    public bool isMoving = false;
    bool isFacingCam = true;
    Quaternion rotToCam;
    public bool isRunning = false;
    const float walkingSpeed = 0.6f;
    const float runningSpeed = 1.8f;
    //Fields for Input 0 (Click || Tap)
    bool isArrivedDes = true;
    float prevDistToDes = 1000.0f;
    Vector3 desByTap;
    Quaternion rotToTap;
    const float rotSpeed = 10.0f;
    void Start()
    {
        anim = GetComponent<Animator>();
        neck = GameObject.Find("Neck");
        head = GameObject.Find("Head");
        transform.localScale = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (!isVisible) {
             return;
        }
        if (!isArrivedDes) {
            if (!isMoving) {
                isMoving = true;
                anim.SetBool("isMoving", true);
            }
            moveToDes();
        } else if (!isFacingCam) {
            faceCamera();
        }
    }
    void LateUpdate() {
        if (!isMoving && isFacingCam && headLookAt) {
            lookAtCamera(headLookTarget);
        }
    }
    private void lookAtCamera(GameObject headLookTarget) {
        Vector3 targetVec = headLookTarget.transform.position - head.transform.position;
        Vector3 horTargetAdjusted = new Vector3(targetVec.x, 0.0f , targetVec.z);
        Vector3 projToTransformFoward = Vector3.Project(horTargetAdjusted, transform.forward);
        Vector3 xToAdjust = projToTransformFoward - horTargetAdjusted;
        Vector3 verTargetAdjusted = targetVec + xToAdjust;
        float horAngle = Vector3.Angle(horTargetAdjusted, transform.forward);
        float verAngle = Vector3.Angle(verTargetAdjusted, transform.forward);
        float horDot = Vector3.Dot(transform.right, horTargetAdjusted);
        float verDot = Vector3.Dot(transform.up, verTargetAdjusted);
        if (horAngle < horizontalMaxLookAngle) {
            if (horDot < 0.0f)
                horAngle = -1.0f * horAngle;
            neck.transform.Rotate(0, horAngle, 0);
            prevNeckHorAngle = horAngle;
        } else {
            neck.transform.Rotate(0, prevNeckHorAngle, 0);
        }
        if (verAngle < verticalMaxLookAngle) {
            if (verDot > 0.0f)
                verAngle = -1.0f * verAngle;
            neck.transform.Rotate(verAngle, 0, 0);
            prevNeckVerAngle = verAngle;
        } else {
            neck.transform.Rotate(prevNeckVerAngle, 0, 0);
        }
    }
    private void moveToDes() {
        float speedPercent = (isRunning)? 1.0f : 0.5f;
        anim.SetFloat("speedPercent", speedPercent);
        float moveSpeed = (isRunning)? runningSpeed * transform.localScale.x / 0.5f : walkingSpeed * transform.localScale.x / 0.5f;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotToTap, rotSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, desByTap, moveSpeed * Time.deltaTime);
        float sqrDistToDes = (transform.position - desByTap).sqrMagnitude;
        if (sqrDistToDes < 0.01f) {
            transform.position = desByTap;
            isArrivedDes = true;
            isMoving = false;
            anim.SetBool("isMoving", false);
            prevDistToDes = 1000.0f;
        } else {
            prevDistToDes = sqrDistToDes;
        }
    }

    private void faceCamera() {
        transform.rotation = Quaternion.Slerp(transform.rotation, rotToCam, rotSpeed * Time.deltaTime);
        if (Quaternion.Angle(transform.rotation, rotToCam) < 3) {
            transform.rotation = rotToCam;
            isFacingCam = true;
        }
    }
    public void setDes (Vector3 pos, Vector3 lookAtPos, bool lookForward = false) {
        desByTap = pos;
        Vector3 turnDir = desByTap - transform.position;
        Vector3 turnDirYAdjusted = new Vector3(turnDir.x, 0, turnDir.z);
        rotToTap = Quaternion.LookRotation(turnDirYAdjusted);
        if (lookForward) {
             GameObject zero = GameObject.Find("zero");
             rotToCam = zero.transform.rotation;
        } else {
            Vector3 lookDir = lookAtPos - desByTap;
            Vector3 lookDirYAdjusted = new Vector3(lookDir.x, 0, lookDir.z);
            rotToCam = Quaternion.LookRotation(lookDirYAdjusted);
        }
        isArrivedDes = false;
        isFacingCam = false;
    }
    public void setDes (Vector3 pos) {
        setDes(pos, Vector3.zero, true);
    }
}
