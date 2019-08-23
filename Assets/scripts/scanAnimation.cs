using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scanAnimation : MonoBehaviour
{
    public float majorVert = 120;
    public float minorVert = 60;
    public float moveSpeed = -150;
    private Vector3 originTransform;
    private bool isMovingClockWise = true;
    private RectTransform rectTransform;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
        originTransform = new Vector3(0, 0, 0);
    } 
    
    void Update()
    {
        float xOffset = Screen.width/2;
        float yOffset = Screen.height/2;
        if (rectTransform.position.x <= xOffset - majorVert || rectTransform.position.x >= xOffset + majorVert) {
            moveSpeed = -moveSpeed;
        }
        float x = originTransform.x + moveSpeed * Time.deltaTime;
        if (x > majorVert)
            x = majorVert;
        if (x < -majorVert)
            x = -majorVert;
        float y = getYInEllipse(x);
        originTransform = new Vector3(x, y, 0);
        rectTransform.position = new Vector3(x + xOffset, y + yOffset, 0);
        if ((int) x == 0 && (int) y == (int) (-minorVert + 1)) {
            isMovingClockWise = !isMovingClockWise;
            moveSpeed = -moveSpeed;
        }
    }
    float getYInEllipse(float x) {
        float y = minorVert * Mathf.Sqrt(1 - (x * x / (majorVert * majorVert)));
        if ((isMovingClockWise && moveSpeed < 0) || (!isMovingClockWise && moveSpeed > 0)) 
            y = -y;
        return y;
    }
}
