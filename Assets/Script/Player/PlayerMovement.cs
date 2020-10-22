using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float normalSpeed;
    public float runningSpeed;
    public float tiredSpeed;
    public float maxEnergy;
    float energy;
    float moveSpeed;
    RunningState runningState;
    Rigidbody2D ownRb;
    Camera cam;
    Vector2 movement;
    Vector2 mousePos;

    private enum RunningState
    {
        Normal,
        Running,
        Tired
    }

    void Start()
    {
        ownRb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        moveSpeed = normalSpeed;
        energy = maxEnergy;
        UIManager.instance?.energyBar.SetMaxFill(maxEnergy);
        runningState = RunningState.Normal;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = movement.normalized;

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        //running system
        if(Input.GetButtonDown("Run") && runningState == RunningState.Normal && energy > 0)
        {
            //Debug.Log("Start running");
            runningState = RunningState.Running;
            moveSpeed = runningSpeed;
            SoundManager.instance?.Play("RunningBreath");
        }
        else if(Input.GetButtonUp("Run") && runningState == RunningState.Running)
        {
            //Debug.Log("Stop running");
            runningState = RunningState.Normal;
            moveSpeed = normalSpeed;
            SoundManager.instance?.StopPlay("RunningBreath", 1f);
        }
        else if(energy < 0 && runningState != RunningState.Tired) // get tired
        {
            runningState = RunningState.Tired;
            moveSpeed = tiredSpeed;
            SoundManager.instance?.StopPlay("RunningBreath", 0.5f);
            SoundManager.instance?.Play("TiredBreath");
        }
        else if(energy > maxEnergy && runningState != RunningState.Running) // full recovery
        {
            RecoverEnergy();
        }
        else if(runningState == RunningState.Running)
        {
            energy -= Time.deltaTime;
            UIManager.instance?.energyBar.SetFill(energy);
        }
        else if(runningState != RunningState.Running)
        {
            energy += Time.deltaTime/3;
            UIManager.instance?.energyBar.SetFill(energy);
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
        runningState = RunningState.Normal;
        moveSpeed = normalSpeed;
        energy = maxEnergy;
        UIManager.instance?.energyBar.SetFill(energy);
        SoundManager.instance?.StopPlay("TiredBreath", 1f);
    }

    public bool IsFullEnergy()
    {
        if (energy == maxEnergy)
            return true;
        return false;
    }
}
