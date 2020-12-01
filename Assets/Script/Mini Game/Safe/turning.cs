using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//RequireComponent(typeof(CircleCollider2D))]
public class turning : MonoBehaviour
{
    public System.Action<string> dialInput;
    private int blockAngles = 36; //only 10 blocks
    private float totalRotate;
    Vector3 dialCenterPos;
    Vector3 cameraCenterPos;

    private void Start()
    {
        dialCenterPos = this.GetComponent<RectTransform>().anchoredPosition;
        cameraCenterPos = GameManager.instance.player.transform.position;
    }

    public void OnMouseDown()
    {
        Debug.Log("mouse down");
        StartCoroutine(RotatingDial());
    }

    public void OnMouseUp()
    {
        //Debug.Log("mouse up");
        StopAllCoroutines(); //end rotating and calculate angle

        int num = Mathf.RoundToInt(this.GetComponent<RectTransform>().eulerAngles.z / blockAngles);
        Vector3 rotation = this.GetComponent<RectTransform>().eulerAngles;
        rotation.z = num * blockAngles;
        float roundAngle = this.GetComponent<RectTransform>().eulerAngles.z - rotation.z;
        this.GetComponent<RectTransform>().eulerAngles = rotation;
        totalRotate = Mathf.RoundToInt(totalRotate + roundAngle);
        
        if (totalRotate == 0)
        {
            return;
        }
        //output rotation result as string
        dialInput.Invoke(((totalRotate > 0) ? "R" : "L") + Mathf.Abs((num % 10) * 10).ToString());
    }

    private IEnumerator RotatingDial()
    {
        float preAngle = this.GetComponent<RectTransform>().eulerAngles.z;
        //Debug.Log(preAngle);

        Vector3 preMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(preMousePos);
        totalRotate = 0;

        while(true) //check mouse position
        {
            //cameraPos = GameManager.instance.player.transform.position;
            Vector3 mouseInitWorldPos = preMousePos - cameraCenterPos;
            Vector3 mouseCurrWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - cameraCenterPos;
            //Debug.Log(mouseCurrWorldPos);

            float angle = Vector2.Angle(mouseInitWorldPos - dialCenterPos, mouseCurrWorldPos - dialCenterPos);
            //Debug.Log(angle);
            Vector3 cross = Vector3.Cross(mouseInitWorldPos - dialCenterPos, mouseCurrWorldPos - dialCenterPos);
            
            if (cross.z > 0)
            {
                angle = -angle;
            }

            Debug.Log(preAngle - angle);
            
            this.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, preAngle - angle);
            totalRotate += angle;
            preMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            preAngle = this.GetComponent<RectTransform>().eulerAngles.z;

            yield return null;
        }
    }
}

