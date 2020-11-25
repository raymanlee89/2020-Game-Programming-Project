using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class turning : MonoBehaviour
{
    public System.Action<string> dialInput;
    private int blockAngles = 36; //only 10 blocks
    private float totalRotate;
    RectTransform ownTransform;

    private void Start()
    {
        ownTransform = GetComponent<RectTransform>();
    }

    private void OnMouseDown()
    {
        StartCoroutine(RotatingDial());
    }

    void OnMouseUp()
    {
        StopAllCoroutines(); //end rotating and calculate angle

        int num = Mathf.RoundToInt(ownTransform.localEulerAngles.z / blockAngles);
        Vector3 rotation = ownTransform.localEulerAngles;
        rotation.z = num * blockAngles;
        float roundAngle = ownTransform.localEulerAngles.z - rotation.z;
        ownTransform.localEulerAngles = rotation;
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
        float preAngle = ownTransform.localEulerAngles.z;
        Vector3 preMousePos = Input.mousePosition;
        totalRotate = 0;

        while(true) //check mouse position
        {
            Vector2 dialWorldPos = new Vector2(ownTransform.position.x, ownTransform.position.y);
            Vector2 mouseInitWorldPos = Camera.main.ScreenToWorldPoint(preMousePos);
            Vector2 mouseCurrWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float angle = Vector2.Angle(mouseInitWorldPos - dialWorldPos, mouseCurrWorldPos - dialWorldPos);
            Vector3 cross = Vector3.Cross(mouseInitWorldPos - dialWorldPos, mouseCurrWorldPos - dialWorldPos);
            if (cross.z > 0)
            {
                angle = -angle;
            }

            if(Vector2.Angle(mouseInitWorldPos, mouseCurrWorldPos) > 1)
            {
                //GetComponent<AudioSource>().Play();
            }

            ownTransform.localEulerAngles = new Vector3(0, 0, preAngle - angle);
            totalRotate += angle;
            preMousePos = Input.mousePosition;
            preAngle = ownTransform.localEulerAngles.z;

            yield return null;

        }
    }
}

