using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class FlowControl : MonoBehaviour
{
    ARRaycastManager arRaycaster;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    GameObject sally; 
    sallyScript sallyScr;
    Animator sallyAnim;
    GameObject zero;
    GameObject xaxis;
    GameObject conex;
    GameObject yaxis;
    GameObject coney;
    GameObject zaxis;
    GameObject conez;
    GameObject xaxiscon;
    GameObject yaxiscon;
    GameObject zaxiscon;
    GameObject screencon;
    GameObject xcon;
    GameObject ycon;
    GameObject zcon;
    GameObject point;
    Vector3 newZeroPos;
    Vector3 dirToNewZeroPos;
    Vector3 coordToPlot = new Vector3(1, -2, -3);
    public GameObject pointFab;
    public GameObject abcFab;
    public GameObject aFab;
    public GameObject bFab;
    public GameObject cFab;
    public GameObject gridFab;
    public GameObject oneFab;
    public GameObject twoFab;
    public GameObject threeFab;
    public GameObject negOneFab;
    public GameObject negTwoFab;
    public GameObject negThreeFab;
    public GameObject ottFab;
    public GameObject CylinderMask;
    GameObject dashX;
    GameObject dashY;
    GameObject dashZ;
    GameObject dashW;
    GameObject espace;
    GameObject guideArrow;
    GameObject oulogo;
    GameObject coordText;
    GameObject UIcanvas;
    AudioSource audioSrc;
    bool isWaitHandled = false;
    public bool flowStarted = false;
    public TrackableId mainPlaneId;
    public Vector3 mainPlaneNormal;
    int totalSceneNumber = 25;
    public int currSceneNumber = 0;
    bool[] dialogPlayed;
    bool[] scenePlayed;
    bool isFirstTimePlotting = true;
    Vector3 tapPos;
    float gridGap;
    float pointSize;
    public float zeroSize = 0.015f;
    public float axisThickness = 0.005f;
    public Vector3 coneSize = new Vector3 (0.6f, 0.6f, 1);
    public Vector3 toNewZeroPos = new Vector3(0.16f, 0.16f, 0); 
    public float gridSize = 0.006f;
    public int numberOfPositiveGrid = 3; 
    public float axisTagSize = 0.5f;
    void Start()
    {
        arRaycaster = GetComponent<ARRaycastManager>();
        sally = GameObject.Find("skinnysally");
        sallyScr = sally.GetComponent<sallyScript>();
        sallyAnim = sally.GetComponent<Animator>();
        GameObject audio = GameObject.Find("Audio Source");
        audioSrc = audio.GetComponent<AudioSource>();
        dialogPlayed = new bool[totalSceneNumber];
        scenePlayed = new bool[totalSceneNumber];
        for (int i = 0; i < totalSceneNumber; i++) {
            dialogPlayed[i] = false;
            scenePlayed[i] = false; 
        }
        zero = GameObject.Find("zero");
        xaxis = GameObject.Find("newXAxis");
        yaxis = GameObject.Find("newYAxis");
        zaxis = GameObject.Find("newZAxis");
        xaxiscon = GameObject.Find("xaxiscon");
        yaxiscon = GameObject.Find("yaxiscon");
        zaxiscon = GameObject.Find("zaxiscon");
        conex = GameObject.Find("conex");
        coney = GameObject.Find("coney");
        conez = GameObject.Find("conez");
        screencon = GameObject.Find("screencon");
        xcon = GameObject.Find("xcon");
        ycon = GameObject.Find("ycon");
        zcon = GameObject.Find("zcon");
        point = GameObject.Find("point");
        dashX = GameObject.Find("dashX");
        dashY = GameObject.Find("dashY");
        dashZ = GameObject.Find("dashZ");
        dashW = GameObject.Find("dashW");
        espace = GameObject.Find("espace");
        guideArrow = GameObject.Find("guidearrow");
        oulogo = GameObject.Find("oulogo");
        coordText = GameObject.Find("coordText");
        UIcanvas = GameObject.Find("StartUI");
        gridGap = (toNewZeroPos.y - 0.02f) / (numberOfPositiveGrid + 1);
        pointSize = axisThickness * 2;
    }


    void Update() 
    {
        if (flowStarted) {
            if (currSceneNumber < totalSceneNumber) {
                playScene(currSceneNumber);
            } 
        }
    }
    
    void playScene(int sceneNumber) {
        switch(sceneNumber) {
            case 0 :
                if (!dialogPlayed[sceneNumber]) {
                    SoundManagerScript.Play("d0");
                    sallyAnim.SetTrigger("hi");
                    dialogPlayed[sceneNumber] = true;
                } else if (!audioSrc.isPlaying && !isWaitHandled) {
                        StartCoroutine(waitForNextScene(1)); 
                }
                break;
            case 1 :
                playDialogOnlyScene(sceneNumber);
                break;
            case 2 :
                playDialogOnlyScene(sceneNumber, "handwave0");
                break;
            case 3 :
                if (!dialogPlayed[sceneNumber]) {
                    string dialogFile = "d" + sceneNumber.ToString();
                    SoundManagerScript.Play(dialogFile);
                    sallyAnim.SetTrigger("handwave1");
                    sallyAnim.SetTrigger("pointing");
                    dialogPlayed[sceneNumber] = true;
                } else if (!audioSrc.isPlaying && !scenePlayed[sceneNumber]) {
                    showPanelWithText("Tap on the surface to set the origin");
                    Touch touch;
                    if (Input.touchCount > 0) {
                        touch = Input.GetTouch(0);
                        switch (touch.phase) { 
                            case TouchPhase.Began :
                                if (!getTapPos(touch))
                                    break;
                                
                                zero.transform.rotation = getTapRotation();
                                zero.transform.Rotate(0, 180, 0);
                                setZeroPosition(tapPos);
                                zero.transform.localScale = new Vector3(zeroSize, zeroSize, zeroSize);
                                break;
                            case TouchPhase.Moved :
                                if (!getTapPos(touch))
                                    break;

                                zero.transform.rotation = getTapRotation();
                                zero.transform.Rotate(0, 180, 0);
                                setZeroPosition(tapPos);
                                break;
                            case TouchPhase.Ended :
                                if (!getTapPos(touch)) {
                                    zero.transform.localScale = Vector3.zero;
                                    break;
                                }
                                hidePanel();
                                setPositionsToZero();
                                Vector3 ucPos = zero.transform.position + zero.transform.right * 0.15f + zero.transform.forward * 0.05f;
                                float distToUcPos = (sally.transform.position - ucPos).sqrMagnitude;
                                if (zero.transform.position.x < sally.transform.position.x || distToUcPos > 0.01f)
                                    sallyScr.setDes(ucPos, Camera.main.transform.position);
                                scenePlayed[sceneNumber] = true;
                                StartCoroutine(waitForNextScene(1));
                                break;
                        }
                    }
                }
                break;
            //debug scene1
            // case 3 :
            //     float axisLength = uc.transform.localScale.x * 1.5f;
            //     tapPos = zero.transform.position + zero.transform.forward * axisLength;
            //     showXaxis(tapPos);
            //     tapPos = zero.transform.position + zero.transform.right * axisLength * -1;
            //     showYaxis(tapPos);
            //     tapPos = zero.transform.position + zero.transform.up * axisLength;
            //     showZaxis(tapPos);
            //     Vector3 xOffset;
            //     Vector3 yOffset;
            //     Vector3 zOffset;
            //     Vector3 aPos;
            //     Vector3 bPos;
            //     currSceneNumber += 9;
            //     break;
        
            //real scenes
            case 4 : 
                if (!dialogPlayed[sceneNumber]) {
                    string dialogFile = "d" + sceneNumber.ToString();
                    SoundManagerScript.Play(dialogFile);
                    sallyAnim.SetTrigger("handwave2");
                    sallyAnim.SetTrigger("pointing");
                    dialogPlayed[sceneNumber] = true;
                } else if (!audioSrc.isPlaying && !scenePlayed[sceneNumber]) {
                    if (guideArrow.transform.localScale.x == 0) {
                        guideArrow.transform.rotation = zero.transform.rotation;
                        guideArrow.transform.Rotate(90, 0, 0);
                        guideArrow.transform.position = zero.transform.position + zero.transform.right * 0.03f
                                                        + zero.transform.forward * 0.15f;
                        guideArrow.transform.localScale = new Vector3(0.04f, 0.3f, 0.1f);
                        showPanelWithText("Draw X-axis along the guide arrow");
                    }
                    Touch touch;
                    if (Input.touchCount > 0) {
                        touch = Input.GetTouch(0);
                        switch (touch.phase) { 
                            case TouchPhase.Began :
                                if (!getTapPos(touch))
                                    break;

                                showXaxis(tapPos);
                                break;
                            case TouchPhase.Moved :
                                if (!getTapPos(touch))
                                    break;

                                showXaxis(tapPos);
                                break;
                            case TouchPhase.Ended :
                                if (xaxiscon.transform.localScale.y < sally.transform.localScale.x * 0.2f) {
                                    SoundManagerScript.Play("longer");
                                    sallyAnim.SetTrigger("longer");
                                } else {
                                    scenePlayed[sceneNumber] = true;
                                    guideArrow.transform.localScale = Vector3.zero;
                                    hidePanel();
                                    showAxesTag(xcon, conex);
                                    StartCoroutine(waitForNextScene(1));
                                }
                                break;
                        }
                    }
                }
                break;
            case 5 :
                if (!dialogPlayed[sceneNumber]) {
                    string dialogFile = "d" + sceneNumber.ToString();
                    SoundManagerScript.Play(dialogFile);
                    dialogPlayed[sceneNumber] = true;
                } else if (!audioSrc.isPlaying && !scenePlayed[sceneNumber]) {
                    if (guideArrow.transform.localScale.x == 0) {
                        guideArrow.transform.rotation = zero.transform.rotation;
                        guideArrow.transform.Rotate(90, 0, 0);
                        guideArrow.transform.Rotate(0, 0, 90);
                        guideArrow.transform.position = zero.transform.position - zero.transform.right * 0.12f
                                                        + zero.transform.forward * 0.03f;
                        guideArrow.transform.localScale = new Vector3(0.04f, 0.2f, 0.1f);
                        showPanelWithText("Draw Y-axis perpendicular to X-axis");
                    }
                    Touch touch;
                    if (Input.touchCount > 0) {
                        touch = Input.GetTouch(0);
                        switch (touch.phase) { 
                            case TouchPhase.Began :
                                if (!getTapPos(touch))
                                    break;

                                showYaxis(tapPos);
                                break;
                            case TouchPhase.Moved :
                                if (!getTapPos(touch))
                                    break;

                                showYaxis(tapPos);
                                break;
                            case TouchPhase.Ended :
                            if (yaxiscon.transform.localScale.y < sally.transform.localScale.x * 0.2f) {
                                    SoundManagerScript.Play("longer");
                                    sallyAnim.SetTrigger("longer");
                            } else {
                                scenePlayed[sceneNumber] = true;
                                guideArrow.transform.localScale = Vector3.zero;
                                hidePanel();
                                showAxesTag(ycon, coney);
                                StartCoroutine(waitForNextScene(1));
                            }
                                break;
                        }
                    }
                }
                break;
            case 6 :
                if (!dialogPlayed[sceneNumber]) {
                    string dialogFile = "d" + sceneNumber.ToString();
                    SoundManagerScript.Play(dialogFile);
                    sallyAnim.SetTrigger("handwave3");
                    dialogPlayed[sceneNumber] = true;
                } else if (!audioSrc.isPlaying && !scenePlayed[sceneNumber]) {
                    if (screencon.transform.localScale.x != 0.5f) {                  
                        screencon.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
                        screencon.transform.Rotate(0, 180, 0);
                        screencon.transform.position = zero.transform.position + zero.transform.up * screencon.transform.localScale.y * 0.5f;
                        showPanelWithText("Draw Z-axis perpendicular to both axes");
                    }
                    Touch touch;
                    if (Input.touchCount > 0) {
                        touch = Input.GetTouch(0);
                        switch (touch.phase) { 
                            case TouchPhase.Began :
                                if (!getTapPosZ(touch))
                                    break;
                                
                                showZaxis(tapPos);
                                break;
                            case TouchPhase.Moved :
                                if (!getTapPosZ(touch))
                                    break;
                                
                                showZaxis(tapPos);
                                break;
                            case TouchPhase.Ended :
                                if (!getTapPosZ(touch))
                                        break;
                                
                                if (zaxiscon.transform.localScale.y < sally.transform.localScale.x * 0.2f) {
                                        SoundManagerScript.Play("longer");
                                        sallyAnim.SetTrigger("longer");
                                } else {
                                    scenePlayed[sceneNumber] = true;
                                    screencon.transform.localScale = new Vector3 (0, 0, 0);
                                    hidePanel();
                                    showAxesTag(zcon, conez);
                                    StartCoroutine(waitForNextScene(1));
                                }
                                break;
                        }
                    }
                }
                break;
            case 7 :
                if (!dialogPlayed[sceneNumber]) {
                    SoundManagerScript.Play("euclidean");
                    sallyAnim.SetTrigger("longwave");
                    sallyAnim.SetTrigger("handwave0");
                    dialogPlayed[sceneNumber] = true;
                } else if (!audioSrc.isPlaying && !isWaitHandled) {
                        StartCoroutine(waitForNextScene(1)); 
                }
                break;
            case 8 : 
                //playDialogOnlyScene(sceneNumber);
                currSceneNumber++;
                break;
            case 9 : 
                //playDialogOnlyScene(sceneNumber);
                currSceneNumber++;
                break;
            case 10 :
                Vector3 xOffset = (coney.transform.position - zero.transform.position) * 0.5f;
                Vector3 yOffset = (conez.transform.position - zero.transform.position) * 0.5f;
                Vector3 zOffset = (conex.transform.position - zero.transform.position) * 0.5f;
                Vector3 pointABCPos = zero.transform.position + xOffset + yOffset + zOffset;
                Vector3 aPos = zero.transform.position + zOffset;
                GameObject pointABC;
                if (!dialogPlayed[sceneNumber]) {
                    Vector3 ucPos = sally.transform.position + zOffset - xOffset * 0.2f;
                    sallyScr.setDes(ucPos, pointABCPos);
                    string dialogFile = "d" + sceneNumber.ToString();
                    SoundManagerScript.Play(dialogFile);
                    pointABC = plotPoint(pointFab, pointABCPos, abcFab);
                    pointABC.name = "pointABC";
                    pointABC.transform.localScale = Vector3.one * pointSize;
                    point.transform.localScale = Vector3.one * pointSize;
                    sallyScr.headLookAt = false;
                    dialogPlayed[sceneNumber] = true;
                } else if (!audioSrc.isPlaying && !scenePlayed[sceneNumber]) {
                    point.transform.position = point.transform.position + xaxiscon.transform.up * 0.1875f * Time.deltaTime / 2;
                    Vector3 pointPosFromZero = point.transform.position - zero.transform.position;
                    bool aPosReached = Mathf.Abs(Vector3.Magnitude(pointPosFromZero) - xaxiscon.transform.localScale.y) <= 0.003f;
                    //bool aPosReached = Vector3.Distance(aPos, point.transform.position) <= 0.001f; 
                    if (aPosReached) {
                        point.transform.position = aPos;
                        Vector3 aLableOffset = new Vector3(0, aFab.transform.localScale.y * 0.5f, 0);
                        GameObject gridA = plotPoint(gridFab, aPos, true, aFab, aLableOffset, false);
                        gridA.transform.localScale = Vector3.one * (axisThickness + 0.002f);
                        scenePlayed[sceneNumber] = true;
                        currSceneNumber++;
                    }
                }
                break;
            case 11 : 
                xOffset = (coney.transform.position - zero.transform.position) * 0.5f;
                zOffset = (conex.transform.position - zero.transform.position) * 0.5f;
                aPos = zero.transform.position + zOffset;
                Vector3 bPos = zero.transform.position + xOffset + zOffset;
                if (!dialogPlayed[sceneNumber]) {
                    string dialogFile = "d" + sceneNumber.ToString();
                    SoundManagerScript.Play(dialogFile);
                    sallyAnim.SetTrigger("pointing");
                    dialogPlayed[sceneNumber] = true;
                } else if (!audioSrc.isPlaying && !scenePlayed[sceneNumber]) {
                    point.transform.position = point.transform.position + yaxiscon.transform.up * 0.1875f * Time.deltaTime / 2;
                    dashPoint(point.transform.position);
                    Vector3 pointPosFromAPos = point.transform.position - aPos;
                    bool bPosReached1 = Mathf.Abs(Vector3.Magnitude(pointPosFromAPos) - yaxiscon.transform.localScale.y) <= 0.003f;
                    //bool bPosReached1 = Vector3.Distance(bPos, point.transform.position) <= 0.001f; 
                    if (bPosReached1) {
                        zOffset = (conex.transform.position - zero.transform.position) * 0.5f;
                        point.transform.position = zero.transform.position + xOffset + zOffset;
                        dashPoint(point.transform.position); 
                        Vector3 bLableOffset = new Vector3(0, bFab.transform.localScale.y * 0.5f, 0);
                        GameObject gridB = plotPoint(gridFab, bPos - zOffset, true, bFab, bLableOffset, false);
                        gridB.transform.localScale = Vector3.one * (axisThickness + 0.002f);
                        scenePlayed[sceneNumber] = true;
                        currSceneNumber++;
                    }
                }
                break;
            case 12 :
                xOffset = (coney.transform.position - zero.transform.position) * 0.5f;
                yOffset = (conez.transform.position - zero.transform.position) * 0.5f;
                zOffset = (conex.transform.position - zero.transform.position) * 0.5f;
                pointABCPos = zero.transform.position + xOffset + yOffset + zOffset;
                if (!dialogPlayed[sceneNumber]) {
                    string dialogFile = "d" + sceneNumber.ToString();
                    SoundManagerScript.Play(dialogFile);
                    sallyAnim.SetTrigger("pointing");
                    dialogPlayed[sceneNumber] = true;
                } else if (!audioSrc.isPlaying && !scenePlayed[sceneNumber]) {
                    bool ABCReached = pointABCPos.y - point.transform.position.y <= 0.002f;
                    if (!ABCReached) {
                        point.transform.position = point.transform.position + zaxiscon.transform.up * 0.1875f * Time.deltaTime / 2;
                        dashPoint(point.transform.position);
                    } else {
                        point.transform.position = pointABCPos;
                        dashPoint(point.transform.position);
                        pointABC = GameObject.Find("pointABC");
                        Destroy(pointABC);
                        Vector3 cPos = zero.transform.position + yOffset; 
                        Vector3 cLableOffset = new Vector3(cFab.transform.localScale.y * 0.5f, 0, 0);
                        GameObject gridC = plotPoint(gridFab, cPos, true, cFab, cLableOffset, false);
                        gridC.transform.localScale = Vector3.one * (axisThickness + 0.002f);
                        Vector3 ucPos = zero.transform.position + zero.transform.right * 0.15f + zero.transform.forward * 0.05f;
                        sallyScr.headLookAt = true;
                        sallyScr.setDes(ucPos);
                        scenePlayed[sceneNumber] = true;
                        StartCoroutine(waitForNextScene(2));
                    }
                }
                break;
            case 13 : 
                playDialogOnlyScene(currSceneNumber);
                clearEspace();
                break;
            case 14 : 
                if (!dialogPlayed[sceneNumber]) {
                    setAxesAndConesSizeZero();
                    calculateNewZeroPos();
                    dialogPlayed[sceneNumber] = true;           
                }
                zero.transform.position += dirToNewZeroPos * 0.4f * Time.deltaTime;
                if (newZeroPos.y - zero.transform.position.y <= 0) {
                    zero.transform.position = newZeroPos;
                    setNewAxes();
                    currSceneNumber++;
                }
                break;
            case 15 :
                if (!dialogPlayed[sceneNumber]) {
                    string dialogFile = "d" + sceneNumber.ToString();
                    SoundManagerScript.Play(dialogFile);
                    sallyAnim.SetTrigger("longwave");
                    sallyAnim.SetTrigger("handwave0");
                    dialogPlayed[sceneNumber] = true;
                }
                if(!scenePlayed[sceneNumber]) {
                    extendNewAxis(xaxis, conex);
                    extendNewAxis(yaxis, coney);
                    extendNewAxis(zaxis, conez);
                    if (xaxis.transform.localScale.y >= toNewZeroPos.y - 0.02f) {
                        xaxis.transform.localScale = new Vector3(axisThickness, toNewZeroPos.y - 0.02f, axisThickness);
                        showAxesTag(xcon, conex);
                        showAxesTag(ycon, coney);
                        showAxesTag(zcon, conez);
                        setGrids();
                        scenePlayed[sceneNumber] = true;
                        currSceneNumber++;
                    }
                }
                break;
            case 16 :
                xOffset = zero.transform.right * gridGap * -2;
                yOffset = zero.transform.up * gridGap * 3;
                zOffset = zero.transform.forward * gridGap;
                Vector3 ottPos = zero.transform.position + xOffset + yOffset + zOffset;
                aPos = zero.transform.position + zOffset;
                if (!dialogPlayed[sceneNumber]) {
                    GameObject ott = plotPoint(pointFab, ottPos, ottFab);
                    ott.name = "ott";
                    ott.transform.localScale = Vector3.one * pointSize;
                    point.transform.localScale = Vector3.one * pointSize;
                    dialogPlayed[sceneNumber] = true;
                } else if (!audioSrc.isPlaying && !scenePlayed[sceneNumber]) {
                    //bool aPosReached = aPos.z - point.transform.position.z >= 0.0005f;
                    bool aPosReached = Vector3.Distance(aPos, point.transform.position) <= 0.001f;
                    if (!aPosReached) {
                        point.transform.position = point.transform.position + zero.transform.forward * 0.1875f * Time.deltaTime / 3;
                        //dashPoint(point.transform.position);
                    } else {
                        point.transform.position = aPos;
                        scenePlayed[sceneNumber] = true;
                        currSceneNumber++;
                    }
                }
                break;
            case 17 :
                xOffset = zero.transform.right * gridGap * -2;
                zOffset = zero.transform.forward * gridGap;
                bPos = zero.transform.position + zOffset + xOffset;
                //bool bPosReached = bPos.x - point.transform.position.x <= 0.0005f;
                bool bPosReached = Vector3.Distance(bPos, point.transform.position) <= 0.001f;
                if (!bPosReached) {
                    point.transform.position = point.transform.position + zero.transform.right * -0.1875f * Time.deltaTime / 2;
                    dashPoint(point.transform.position);
                } else {
                    point.transform.position = bPos;
                    dashPoint(point.transform.position);
                    scenePlayed[sceneNumber] = true;
                    currSceneNumber++;    
                }
                break;
            case 18 :
                xOffset = zero.transform.right * gridGap * -2;
                yOffset = zero.transform.up * gridGap * 3;
                zOffset = zero.transform.forward * gridGap;
                ottPos = zero.transform.position + xOffset + yOffset + zOffset;
                bool ottPosReached =  ottPos.y - point.transform.position.y <= 0.0005f;
                if (!ottPosReached) {
                    point.transform.position = point.transform.position + zero.transform.up * 0.1875f * Time.deltaTime / 2;
                    dashPoint(point.transform.position);
                } else if (!scenePlayed[sceneNumber]) {
                    point.transform.position = ottPos;
                    dashPoint(point.transform.position);
                    Destroy(GameObject.Find("ott"));
                    scenePlayed[sceneNumber] = true;
                } else if (!isWaitHandled) {
                    StartCoroutine(waitForNextScene(2));
                }
                break;
            case 19 :
                playDialogOnlyScene(sceneNumber);
                clearEspace();
                break;
            case 20 :
                if (!dialogPlayed[sceneNumber]) {
                    if (isFirstTimePlotting) {
                        string dialogFile = "d" + sceneNumber.ToString();
                        SoundManagerScript.Play(dialogFile);
                        sallyAnim.SetTrigger("handwave0");
                        sallyAnim.SetTrigger("pointing");
                    } else {
                        SoundManagerScript.Play("newpoint");
                        sallyAnim.SetTrigger("handwave0");
                    }
                    point.transform.position = zero.transform.position;
                    point.transform.localScale = Vector3.one * pointSize;
                    dialogPlayed[sceneNumber] = true;
                } else if (!audioSrc.isPlaying && !scenePlayed[sceneNumber]) {
                    if (screencon.transform.localScale == Vector3.zero) {
                        string xCoord = coordToPlot.x.ToString();
                        string yCoord = coordToPlot.y.ToString();
                        showPanelWithText($"Tap and Drag to the point({xCoord}, {yCoord})");
                        coordText.transform.position = sally.transform.position + sally.transform.up * 0.35f + sally.transform.right * -0.07f;
                        coordText.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        getScreeonOnXYPlane();
                    }
                    Touch touch;
                    if (Input.touchCount > 0) {
                        touch = Input.GetTouch(0);
                        switch (touch.phase) { 
                            case TouchPhase.Began :
                                if (!getTapPosZ(touch))
                                    break;
                                
                                point.transform.position = tapPos;
                                dashPoint(point.transform.position);
                                break;
                            case TouchPhase.Moved :
                                if (!getTapPosZ(touch))
                                    break;
                                
                                point.transform.position = tapPos;
                                dashPoint(point.transform.position);
                                break;
                            case TouchPhase.Ended :
                                Vector3 targetCoordinate = new Vector3 (coordToPlot.x, coordToPlot.y, 0);
                                Vector3 targetPos = convertPointToPos(targetCoordinate);
                                if (approxSamePos(point.transform.position, targetPos)) {
                                    point.transform.position = targetPos;
                                    dashPoint(point.transform.position);
                                    screencon.transform.localScale = Vector3.zero;
                                    hidePanel();
                                    scenePlayed[sceneNumber] = true;
                                    currSceneNumber++;
                                } else {
                                    SoundManagerScript.Play("tryagain");
                                }
                                break;
                        }
                    }
                }
                break;
            case 21 :
                if (!dialogPlayed[sceneNumber]) {
                    string dialogFile = "d" + sceneNumber.ToString();
                    SoundManagerScript.Play(dialogFile);
                    sallyAnim.SetTrigger("pointing");
                    dialogPlayed[sceneNumber] = true;
                } else if (!audioSrc.isPlaying && !scenePlayed[sceneNumber]) {
                    if (screencon.transform.localScale == Vector3.zero) {
                        string zCoord = coordToPlot.z.ToString();
                        showPanelWithText($"Drag the point to {zCoord} on Z-axis");
                        screencon.transform.Rotate(90, 0, 0);
                        screencon.transform.position += zero.transform.forward * gridGap;
                        screencon.transform.localScale = Vector3.one * gridGap * 2 * (numberOfPositiveGrid + 1);
                    }
                    Touch touch;
                    if (Input.touchCount > 0) {
                        touch = Input.GetTouch(0);
                        switch (touch.phase) { 
                            case TouchPhase.Began :
                                if (!getTapPosZ(touch))
                                    break;

                                point.transform.position = new Vector3(point.transform.position.x, tapPos.y, point.transform.position.z);
                                dashPoint(point.transform.position, true);
                                break;
                            case TouchPhase.Moved :
                                if (!getTapPosZ(touch))
                                    break;

                                point.transform.position = new Vector3(point.transform.position.x, tapPos.y, point.transform.position.z);;
                                dashPoint(point.transform.position, true);
                                break;
                            case TouchPhase.Ended :
                                Vector3 targetPos = convertPointToPos(coordToPlot);
                                if (approxSamePos(point.transform.position, targetPos)) {
                                    point.transform.position = targetPos;
                                    dashPoint(point.transform.position);
                                    screencon.transform.localScale = Vector3.zero;
                                    dashW.transform.localScale = Vector3.zero;
                                    coordText.transform.position = point.transform.position;
                                    hidePanel();
                                    scenePlayed[sceneNumber] = true;
                                    currSceneNumber++;
                                } else {
                                    SoundManagerScript.Play("tryagain");
                                }
                                break;
                            }
                        }
                }
                break;
            case 22 :
                if (!dialogPlayed[sceneNumber]) {   
                    if (isFirstTimePlotting) {
                        SoundManagerScript.Play("correct");
                        sallyAnim.SetTrigger("handwave3");
                    } else {
                        currSceneNumber++;
                    }
                    dialogPlayed[sceneNumber] = true;
                } else if (!audioSrc.isPlaying && !scenePlayed[sceneNumber] && !isWaitHandled) {
                    StartCoroutine(waitForNextScene(1));
                    scenePlayed[sceneNumber] = true;
                }
                break;
            case 23 :
                if (!dialogPlayed[sceneNumber]) {
                    if (isFirstTimePlotting) {
                        string dialogFile = "d" + sceneNumber.ToString();
                        SoundManagerScript.Play(dialogFile);
                        sallyAnim.SetTrigger("handwave1");
                        sallyAnim.SetTrigger("handwave3");
                        oulogo.transform.position = sally.transform.position + sally.transform.up * sally.transform.localScale.y * 1.9f;
                    } else {
                        SoundManagerScript.Play("taponme");
                        sallyAnim.SetTrigger("handwave3");
                    }
                    dialogPlayed[sceneNumber] = true;
                } else if (!audioSrc.isPlaying && !scenePlayed[sceneNumber]) {
                    if (oulogo.transform.localScale == Vector3.zero) {
                        oulogo.transform.localScale = new Vector3(0.25f, 0.1f ,0.1f);
                        SoundManagerScript.Play("logosound");
                        showPanelWithText("Tap logo to read or tap Sally to practice");
                    }
                    Touch touch;
                    if (Input.touchCount > 0 && (touch = Input.GetTouch(0)).phase == TouchPhase.Began) {
                        RaycastHit hit;
                        Ray ray = Camera.main.ScreenPointToRay(touch.position);
                        if (Physics.Raycast(ray, out hit, 1.5f)) {
                            if (hit.transform.gameObject.name == "oulogo") {
                                hidePanel();
                                currSceneNumber++;
                                scenePlayed[sceneNumber] = true;  
                            } else if (hit.transform.gameObject.name == "skinnysally") {
                                point.transform.position = zero.transform.position;
                                dashPoint(point.transform.position);
                                oulogo.transform.localScale = Vector3.zero;
                                hidePanel();
                                coordToPlot = generateRandomCoord(3);
                                coordText.transform.localScale = Vector3.zero;
                                coordText.GetComponent<TextMeshPro>().text = vec3ToCoordStr(coordToPlot);
                                for (int i = 20; i < totalSceneNumber; i++) {
                                    dialogPlayed[i] = false;
                                    scenePlayed[i] = false;
                                }
                                isFirstTimePlotting = false;
                                currSceneNumber = 20;
                            }
                        }
                    }
                }
                break;
            case 24 :
                if (!dialogPlayed[sceneNumber]) {
                    string dialogFile = "d" + sceneNumber.ToString();
                    SoundManagerScript.Play(dialogFile);
                    sallyAnim.SetTrigger("handwave3");
                    dialogPlayed[sceneNumber] = true;
                } else if (!audioSrc.isPlaying && !scenePlayed[sceneNumber]) {
                    SoundManagerScript.Play("tapplace");
                    sallyAnim.SetTrigger("hi");
                    CylinderMask.GetComponent<CylinderMask>().activate(sally, false);
                    Invoke("openSourceURL", 5.0f);
                    currSceneNumber++;
                    scenePlayed[sceneNumber] = true;
                }
                break;
        }
    }
    void openSourceURL() {
        Application.OpenURL("https://www.open.edu/openlearn/science-maths-technology/mathematics-statistics/vectors-and-conics/content-section-1.6");
    }
    string vec3ToCoordStr (Vector3 vec3) {
        string x = vec3.x.ToString();
        string y = vec3.y.ToString();
        string z = vec3.z.ToString();
        return $"({x},{y},{z})";
    }
    Vector3 generateRandomCoord(int maxInt) {
        int[] intForVec3 = new int[3];
        for (int i = 0; i < 3; i++) {
            int c = (int) Random.Range(-1 * maxInt + 1, maxInt + 1);
            // if (c == 0) 
            //     i--;
            // else
                intForVec3[i] = c; 
        }
        return new Vector3(intForVec3[0], intForVec3[1], intForVec3[2]);
    }
    void showPanelWithText(string textString) {
        if (!UIcanvas.activeSelf) {
            StartUI uiPanel = UIcanvas.GetComponent<StartUI>();
            uiPanel.setText(textString);
            UIcanvas.SetActive(true);
            uiPanel.aniOff();
        }
    }

    void hidePanel() {
        UIcanvas.SetActive(false);
    }

    bool approxSamePos (Vector3 pos1, Vector3 pos2) {
        return Vector3.Distance(pos1, pos2) <= 0.01f;
    }
    void getScreeonOnXYPlane () {
        screencon.transform.position = zero.transform.position;
        screencon.transform.rotation = zero.transform.rotation;
        screencon.transform.Rotate(-90, 0, 0);
        screencon.transform.localScale = Vector3.one * gridGap * 2 * (numberOfPositiveGrid + 1);
    }
    void showXaxis (Vector3 tapPos) {
        Vector3 tapPosOnXY = new Vector3(tapPos.x, zero.transform.position.y, tapPos.z);
        Vector3 dest = Vector3.Project(tapPosOnXY - zero.transform.position, zero.transform.forward);
        float destLength = dest.magnitude;
        conex.transform.localScale = coneSize;
        if (dest.normalized == zero.transform.forward * -1) {
            destLength = 0;
            conex.transform.localScale = coneSize * 0;
        } else if (destLength > sally.transform.localScale.x * 1.3f) {
            destLength = sally.transform.localScale.x * 1.3f;
            dest = zero.transform.forward * sally.transform.localScale.x * 1.3f;
        }
        xaxiscon.transform.localScale = new Vector3(axisThickness, destLength/2, axisThickness);
        conex.transform.position = zero.transform.position + dest + conex.transform.forward * axisThickness;
        // Debug.Log("zero : " + zero.transform.position.x + "," + zero.transform.position.y + "," + zero.transform.position.z + ",");
        // Debug.Log("tapPos : " + tapPos.x + "," + tapPos.y + "," + tapPos.z + ",");
        // Debug.Log("dest : " + dest.x + "," + dest.y + "," + dest.z + ",");
        }
    void showYaxis (Vector3 tapPos) {
        Vector3 tapPosOnXY = new Vector3(tapPos.x, zero.transform.position.y, tapPos.z);
        Vector3 dest = Vector3.Project(tapPosOnXY - zero.transform.position, zero.transform.right * -1);
        float destLength = dest.magnitude;
        coney.transform.localScale = coneSize;
        if (dest.normalized == zero.transform.right) {
            destLength = 0;
            coney.transform.localScale = coneSize * 0;
        } else if (destLength > sally.transform.localScale.x * 1.3f) {
            destLength = sally.transform.localScale.x * 1.3f;
            dest = zero.transform.right * sally.transform.localScale.x * -1.3f;
        }
        yaxiscon.transform.localScale = new Vector3(axisThickness, destLength/2, axisThickness);
        coney.transform.position = zero.transform.position + dest + coney.transform.forward * axisThickness;
    }

    void showZaxis (Vector3 tapPos) {
        Vector3 tapPosOnYZ = new Vector3(tapPos.x, tapPos.y, zero.transform.position.z);
        Vector3 dest = Vector3.Project(tapPosOnYZ - zero.transform.position, zero.transform.up);
        //Vector3 dest = new Vector3(0, tapPos.y - zero.transform.position.y, 0);
        conez.transform.localScale = coneSize;
        if (dest.y <= 0) {
            dest.y = 0;
            conez.transform.localScale = coneSize * 0;
        } else if (Mathf.Abs(dest.y) > sally.transform.localScale.x * 1.3f) {
            dest.y = sally.transform.localScale.x * 1.3f;
        }
        zaxiscon.transform.localScale = new Vector3(axisThickness, Mathf.Abs(dest.y)/2, axisThickness);
        conez.transform.position = zero.transform.position + dest + conez.transform.forward * axisThickness;
    }
    Vector3 convertPointToPos (Vector3 pointPos) {
        return zero.transform.position + (zero.transform.forward * gridGap * pointPos.x) 
                    + (zero.transform.right * gridGap * pointPos.y * -1) + (zero.transform.up * gridGap * pointPos.z);
    }
    void setGrids() {
        for (int i = 1; i < 4; i++) {
            Vector3 xGridOffset = zero.transform.forward * gridGap * i;
            Vector3 yGridOffset = zero.transform.right * gridGap * i * -1;
            Vector3 zGridOffset = zero.transform.up * gridGap * i;
            GameObject gridLabel = oneFab;
            GameObject negGridLabel = negOneFab;
            switch (i){
                case 1 :
                    gridLabel = oneFab;
                    negGridLabel = negOneFab;
                break;
                case 2 :
                    gridLabel = twoFab;
                    negGridLabel = negTwoFab;
                break;
                case 3 :
                    gridLabel = threeFab;
                    negGridLabel = negThreeFab;
                break;
            }
            plotGrid("x", zero.transform.position + xGridOffset, gridLabel);
            plotGrid("x", zero.transform.position - xGridOffset, negGridLabel);
            plotGrid("y", zero.transform.position + yGridOffset, gridLabel);
            plotGrid("y", zero.transform.position - yGridOffset, negGridLabel);
            plotGrid("z", zero.transform.position + zGridOffset, gridLabel);
            plotGrid("z", zero.transform.position - zGridOffset, negGridLabel);
        }
    }
    void setNewAxes() {
        xaxis.transform.position = zero.transform.position;
        yaxis.transform.position = zero.transform.position;
        zaxis.transform.position = zero.transform.position;
        xaxis.transform.up = zero.transform.forward;
        yaxis.transform.up = zero.transform.right * -1;
        zaxis.transform.up = zero.transform.up;
        conex.transform.forward = xaxis.transform.up;
        coney.transform.forward = yaxis.transform.up;
        conez.transform.forward = zaxis.transform.up;
        point.transform.position = zero.transform.position;
    }
    void extendNewAxis (GameObject newAxis, GameObject cone) {
        float targetLength = newAxis.transform.localScale.y + (toNewZeroPos.y - 0.02f) * Time.deltaTime;
        newAxis.transform.localScale = new Vector3(axisThickness, targetLength, axisThickness);
        cone.transform.position = newAxis.transform.position + newAxis.transform.up * newAxis.transform.localScale.y;
        if (cone.transform.localScale.x == 0) {
            cone.transform.localScale = coneSize;
        }
    }
    void showAxesTag(GameObject tagCon, GameObject cone) {
        tagCon.transform.localScale = new Vector3(axisTagSize, axisTagSize, 1);
        tagCon.transform.position = cone.transform.position;
        if (tagCon.transform.parent != zero.transform.parent) {
            tagCon.transform.parent = zero.transform.parent;
        }
    }
    void calculateNewZeroPos() {
        newZeroPos = zero.transform.position + zero.transform.right * -1 * toNewZeroPos.x + 
                     zero.transform.up * toNewZeroPos.y + zero.transform.forward * toNewZeroPos.z;
        // newZeroPos = zero.transform.position + yaxiscon.transform.up * toNewZeroPos.x + 
        //             zaxiscon.transform.up * toNewZeroPos.y + xaxiscon.transform.up * toNewZeroPos.z;
        dirToNewZeroPos = (newZeroPos - zero.transform.position).normalized;
    }
    void setAxesAndConesSizeZero() {
        xaxiscon.transform.localScale = Vector3.zero;
        yaxiscon.transform.localScale = Vector3.zero;
        zaxiscon.transform.localScale = Vector3.zero;
        conex.transform.localScale = Vector3.zero;
        coney.transform.localScale = Vector3.zero;
        conez.transform.localScale = Vector3.zero;
        xcon.transform.localScale = Vector3.zero;
        ycon.transform.localScale = Vector3.zero;
        zcon.transform.localScale = Vector3.zero;
    }
    void clearEspace() {
        point.transform.localScale = Vector3.zero;
        dashX.transform.localScale = Vector3.zero;
        dashY.transform.localScale = Vector3.zero;  
        dashZ.transform.localScale = Vector3.zero;
        dashW.transform.localScale = Vector3.zero;
        foreach (Transform child in espace.transform) {
            Destroy(child.gameObject);
        }
    }

    void plotGrid (string axis, Vector3 pos, GameObject label) {
        Vector3 labelOffset = Vector3.zero;
        switch (axis) {
            case "x" :
                labelOffset = new Vector3(label.transform.localScale.x * 0.8f, 0, 0);  
                break;
            case "y" :
                labelOffset = new Vector3(0, label.transform.localScale.x * -0.8f, 0);
                break;
            case "z" :
                labelOffset = new Vector3(label.transform.localScale.x * 0.8f, 0, 0);
                break;
        }
        GameObject p = plotPoint(gridFab, pos, true, label, labelOffset, true);
    }
    GameObject plotPoint(GameObject pointToPlot, Vector3 pos, bool withLabelOffset, GameObject label, Vector3 labelOffset, bool isGrid) {
        GameObject p;
        if (isGrid) {
            p = Instantiate(pointToPlot, pos, zero.transform.rotation, zero.transform.parent);
        } else {
            p = Instantiate(pointToPlot, pos, zero.transform.rotation, espace.transform);
        }
        p.transform.localScale = Vector3.one * gridSize;
        if (withLabelOffset) {
            Vector3 offset = zero.transform.right * labelOffset.x * -1.0f + zero.transform.up * labelOffset.y +
                            zero.transform.forward * labelOffset.z;
            if (isGrid) {
                Instantiate(label, pos + offset, Quaternion.identity, zero.transform.parent);
            } else {
                Instantiate(label, pos + offset, Quaternion.identity, espace.transform);
            }
        }
        return p;
    } 
    GameObject plotPoint(GameObject pointToPlot, Vector3 pos, GameObject label) {
        GameObject p = plotPoint(pointToPlot, pos, true, label, Vector3.zero, false);
        return p;
    }
    GameObject plotPoint(GameObject pointToPlot, Vector3 pos) {
        GameObject p = plotPoint(pointToPlot, pos, false, zero, Vector3.zero, false);
        return p;
    }
    void dashPoint (Vector3 pointPos, bool isDashW) {
        Vector3 proj = Vector3.ProjectOnPlane(pointPos, zero.transform.up * -1.0f);
        Vector3 offsetToXY = Vector3.Project(zero.transform.position, zero.transform.up);
        Vector3 projXY = proj + offsetToXY;
        Vector3 xPos = Vector3.Project(projXY - zero.transform.position, zero.transform.forward) + zero.transform.position;
        Vector3 yPos = Vector3.Project(projXY - zero.transform.position, zero.transform.right * -1) + zero.transform.position;
        float xlength = Vector3.Distance(projXY, xPos);
        float ylength = Vector3.Distance(projXY, yPos);
        float zlength = Vector3.Distance(projXY, pointPos);
        dashX.transform.position = xPos;
        dashX.transform.up = projXY - xPos;
        dashX.transform.localScale = new Vector3 (0.003f, xlength/2, 0.003f);
        dashY.transform.position = yPos;
        dashY.transform.up = projXY - yPos;
        dashY.transform.localScale = new Vector3 (0.003f, ylength/2, 0.003f);
        dashZ.transform.position = projXY;
        dashZ.transform.up = pointPos - projXY;
        dashZ.transform.localScale = new Vector3 (0.003f, zlength/2, 0.003f);
        if (isDashW) {
            Vector3 wPos = Vector3.Project(pointPos - zero.transform.position, zero.transform.up) + zero.transform.position;
            dashW.transform.position = wPos;
            dashW.transform.up = pointPos - wPos;
            dashW.transform.localScale = new Vector3 (0.003f, Vector3.Distance(pointPos, wPos)/2, 0.003f);
        }
    }
    void dashPoint (Vector3 pointPos) {
        dashPoint(pointPos, false);
    }

    void setPositionsToZero() {
        xaxiscon.transform.position = zero.transform.position;
        xaxiscon.transform.parent = zero.transform.parent;
        yaxiscon.transform.position = zero.transform.position;
        yaxiscon.transform.parent = zero.transform.parent;
        zaxiscon.transform.position = zero.transform.position;
        zaxiscon.transform.parent = zero.transform.parent;
        xaxiscon.transform.up = zero.transform.forward;
        yaxiscon.transform.up = zero.transform.right * -1;
        zaxiscon.transform.up = zero.transform.up;
        conex.transform.forward = zero.transform.forward;
        coney.transform.forward = zero.transform.right * -1;
        conez.transform.forward = zero.transform.up;
        conex.transform.parent = zero.transform.parent;
        coney.transform.parent = zero.transform.parent;
        conez.transform.parent = zero.transform.parent;
        screencon.transform.position = zero.transform.position;
        screencon.transform.rotation = zero.transform.rotation;
        screencon.transform.Rotate(0, 180, 0);
        screencon.transform.parent = zero.transform.parent;
        point.transform.position = zero.transform.position;
        point.transform.rotation = zero.transform.rotation;
        point.transform.parent = zero.transform.parent;
        dashX.transform.up = coney.transform.forward;
        dashX.transform.parent = zero.transform.parent;
        dashY.transform.up = conex.transform.forward;
        dashY.transform.parent = zero.transform.parent;
        dashZ.transform.up = conez.transform.forward;
        dashZ.transform.parent = zero.transform.parent;
        dashW.transform.parent = zero.transform.parent;
        espace.transform.position = zero.transform.position;
        espace.transform.parent = zero.transform.parent;
        xaxis.transform.parent = zero.transform.parent;
        yaxis.transform.parent = zero.transform.parent;
        zaxis.transform.parent = zero.transform.parent;
        guideArrow.transform.parent = zero.transform.parent;
        oulogo.transform.parent = zero.transform.parent;
    }
    bool getTapPos (Touch touch) {
        Vector2 adjustedTouchPos = new Vector2(touch.position.x, touch.position.y + 25);
        if (!arRaycaster.Raycast(adjustedTouchPos, s_Hits, TrackableType.PlaneWithinPolygon))
            return false;

        tapPos = s_Hits[0].pose.position;
        return true;
    }
    Quaternion getTapRotation() {
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camBearing = new Vector3(camForward.x, 0, camForward.z);
        return Quaternion.LookRotation(camBearing, mainPlaneNormal);
    }

    bool getTapPosZ (Touch touch) {
        int layerMask = 1 << 8;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(touch.position.x, touch.position.y + 35));
        if (!Physics.Raycast(ray, out hit, 1.5f, layerMask))
            return false;

        tapPos = hit.point;
        Debug.Log("tapPos : " + tapPos.x.ToString() + "," + tapPos.y.ToString() + tapPos.z.ToString());
        return true;
    }

    void playDialogOnlyScene (int sceneNumber, string aniTrigger = null) {
        if (!dialogPlayed[sceneNumber]) {
            string dialogFile = "d" + sceneNumber.ToString();
            SoundManagerScript.Play(dialogFile);
            if (aniTrigger != null)
                sallyAnim.SetTrigger(aniTrigger);
            dialogPlayed[sceneNumber] = true;
        } else if (!audioSrc.isPlaying && !isWaitHandled) {
                StartCoroutine(waitForNextScene(1)); 
        }  
    }

    void setZeroPosition (Vector3 tapPos) {
        Vector3 zeroPos = tapPos + zero.transform.up * zeroSize / 2;
        zero.transform.position = zeroPos; 
    }
    
    private IEnumerator waitForNextScene (float waitSecond) {
        isWaitHandled = true;
        yield return new WaitForSeconds (waitSecond);
        currSceneNumber++;
        isWaitHandled = false;
    }
}
