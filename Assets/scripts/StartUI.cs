using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StartUI : MonoBehaviour
{
    public GameObject UIPanel;
    public Text panelText;
    public GameObject scanAni;
    public GameObject tapImage;
    public bool isMainPlaneDetected = false;
    private bool isTapPanelShown = false;

    void Start()
    {
        UIPanel.SetActive(false);
        scanAni.SetActive(false);
        tapImage.SetActive(false);
        Invoke("showScanPanel", 1.0f);
    }

    void Update()
    {
        if (!isTapPanelShown && isMainPlaneDetected) {
            showTapPanel();
            isTapPanelShown = true;
        }
    }
    void showScanPanel() {
        UIPanel.SetActive(true);
        scanAni.SetActive(true);
    }
    void showTapPanel() {
        setText("Tap on the inner part of the plane");
        scanAni.SetActive(false);
        tapImage.SetActive(true);
    }
    public void setText(string textString) {
        panelText.text = textString;
    }
    public void aniOff() {
        tapImage.SetActive(false);
        scanAni.SetActive(false);
    }
}
