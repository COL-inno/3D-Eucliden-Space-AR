using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FlowStarter : MonoBehaviour
{
    // minimum dimension to start accepting tapping. Default width set to 25cm and length set to 70% of the width 
    public float minExtentOfPlaneForTap = 0.25f;
    private bool isSallyVisible = false;
    private ARPlane mainPlane = null;
    private ARRaycastManager arRaycaster;
    private ARPlaneManager planeManager;
    private ARReferencePointManager refPointManager;
    private GameObject startUI;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    private bool isDecentSizePlaneDetected = false;

    void Start()
    {
        arRaycaster = GetComponent<ARRaycastManager>();
        refPointManager = GetComponent<ARReferencePointManager>();
        planeManager = GetComponent<ARPlaneManager>();
        startUI = GameObject.Find("StartUI");
        
        // Adding inActiveAddedPlance to the planeChanged event, so it deactivates newly added planes once Sally's placed
        planeManager.planesChanged += inActivateAddedPlanes;
    }

    void Update()
    { 
        // Once Sally's placed, the plane she's stands on will be assigned to mainPlane. Then, Update is bypassed
        if (mainPlane != null) {
            return;
        }
        // When a plane with a decent size is detected, the cavas UI image and icod is changed for tap action
        if (!isDecentSizePlaneDetected && planeManager.trackables.count > 0) {
            foreach (var plane in planeManager.trackables) {
                // Default mininum width set to 25cm, lenght set to 70% of the width
                if (plane.extents.x > minExtentOfPlaneForTap && plane.extents.y > minExtentOfPlaneForTap * 0.7f) {
                    StartUI uiScript = startUI.GetComponent<StartUI>();
                    uiScript.isMainPlaneDetected = true;
                    isDecentSizePlaneDetected = true;
                }
            }
        } 
        
        // Quit update routine in case of no touch input
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            return;
        
        // Raycast on the plane and place Sally when Sally is not visible in the beginning
        if (!isSallyVisible && isDecentSizePlaneDetected && arRaycaster.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            
            // Get Camera bearing(rotation without vertical component) and set main plane
            Pose hitPose = s_Hits[0].pose;
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camBearing = new Vector3(camForward.x, 0, camForward.z);
            mainPlane = planeManager.GetPlane(s_Hits[0].trackableId);

            //Play tapplace sound effect//
            SoundManagerScript.Play("tapplace");

            // Make Sally visible on the plane and rotate her toward the Camera
            GameObject sally = GameObject.Find("skinnysally");
            sallyScript s = sally.GetComponent<sallyScript>();
            sally.transform.localScale = new Vector3(s.size, s.size, s.size); 
            s.isVisible = true;
            isSallyVisible = true;
            sally.transform.position = hitPose.position;
            sally.transform.rotation = Quaternion.LookRotation(camBearing, mainPlane.normal);
            sally.transform.Rotate(0, 180, 0, Space.Self);

            // Match CylinderMask at sally's position and rotation and activate with the moving up direction true
            GameObject mask = GameObject.Find("CylinderMask");
            mask.GetComponent<CylinderMask>().activate(sally, true);
            
            // Create AR Reference point(anchor) and parent it to Sally, Zero and Mask and deactivate UI and start the flow
            ARReferencePoint anchorRefPoint = refPointManager.AddReferencePoint(hitPose);
            GameObject anchor = anchorRefPoint.gameObject;
            sally.transform.parent = anchor.transform;
            GameObject zero = GameObject.Find("zero");
            zero.transform.parent = anchor.transform;
            mask.transform.parent = anchor.transform;
            startUI.SetActive(false);
            inActivatePlanes();
            Invoke("startFlow", 4.0f);
        }
    }
    // Callback for planeChanged event. Once Sally's places, inactivated all the added planes as the added 
    private void inActivateAddedPlanes (ARPlanesChangedEventArgs eventArgs) {
        if (mainPlane != null && eventArgs.added != null && eventArgs.added.Count > 0)
            inActivatePlanes(eventArgs.added);
    }
    // Inactivate planes in the List of planes. If called without an arguments, inactivate all trackable planes
    private void inActivatePlanes(List<ARPlane> planes = null) {
        if (planes == null) {
            foreach (var plane in planeManager.trackables)
                plane.gameObject.SetActive(false);
        } else {
            foreach (var plane in planes)
                plane.gameObject.SetActive(false);
        }
    }
    // Start the update routine in the FlowControl script and pass the mainPlane's trackableID and its normal to it
    private void startFlow() {
            FlowControl fcs = GetComponent<FlowControl>();
            fcs.flowStarted = true;
            fcs.mainPlaneId = mainPlane.trackableId;
            fcs.mainPlaneNormal = mainPlane.normal;
        }

}
