using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float normalSpeed;
    public float runningSpeed;
    public float tiredSpeed;
    public float maxEnergy;
    public Bar energyBar;
    float presentEnergy;
    float moveSpeed;
    bool runningOrNot = false;
    bool restingOrNot = false;
    Rigidbody2D ownRb;
    Camera cam;
    Vector2 movement;
    Vector2 mousePos;

    void Start()
    {
        ownRb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        moveSpeed = normalSpeed;
        presentEnergy = maxEnergy;
        energyBar.SetMaxFill(maxEnergy);
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = movement.normalized;

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        //running system
        if(Input.GetButtonDown("Run") && !runningOrNot && presentEnergy > 0 && moveSpeed != tiredSpeed)
        {
            //Debug.Log("Start running");
            runningOrNot = true;
            restingOrNot = false;
            moveSpeed = runningSpeed;
        }
        else if(Input.GetButtonUp("Run") && !restingOrNot)
        {
            //Debug.Log("Stop running");
            runningOrNot = false;
            restingOrNot = true;
            moveSpeed = normalSpeed;
        }
        else if(presentEnergy < 0 && !restingOrNot) // get tired
        {
            runningOrNot = false;
            restingOrNot = true;
            moveSpeed = tiredSpeed;
        }
        else if(presentEnergy > maxEnergy && restingOrNot) // full recovery
        {
            RecoverEnergy();
        }
        else if(runningOrNot)
        {
            presentEnergy -= Time.deltaTime;
            energyBar.SetFill(presentEnergy);
        }
        else if(restingOrNot)
        {
            presentEnergy += Time.deltaTime/3;
            energyBar.SetFill(presentEnergy);
        }
        
        //Debug.Log("Present Energy :" + presentEnergy);
    }

    void FixedUpdate()
    {
        ownRb.MovePosition(ownRb.position + movement * moveSpeed * Time.fixedDeltaTime);
        Vector2 lookDir = mousePos - ownRb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        ownRb.rotation = angle;
    }

    public void RecoverEnergy()
    {
        runningOrNot = false;
        restingOrNot = false;
        moveSpeed = normalSpeed;
        presentEnergy = maxEnergy;
        energyBar.SetFill(presentEnergy);
    }

    public bool IsFullEnergy()
    {
        if (presentEnergy == maxEnergy)
            return true;
        return false;
    }
}
