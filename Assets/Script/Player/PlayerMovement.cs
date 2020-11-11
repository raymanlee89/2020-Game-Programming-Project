using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerMovement : MonoBehaviour
{
    public float normalSpeed;
    public float runningSpeed;
    public float tiredSpeed;
    public float maxEnergy;
    public Volume postProcessingVolume;
    Vignette vignette;
    int staredCount = 0;
    float energy;
    float moveSpeed;
    RunningState runningState;
    Rigidbody2D ownRb;
    Camera cam;
    Vector2 movement;
    Vector2 mousePos;

    #region Singleton
    public static PlayerMovement instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one of instance of PlayerMovement found!");
            return;
        }
        instance = this;
    }

    #endregion

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
        if (postProcessingVolume.profile.TryGet<Vignette>(out vignette))
        {
            Debug.Log("Vignette exists");
        }
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = movement.normalized;

        //running system
        if(Input.GetButtonDown("Run") && runningState == RunningState.Normal && energy > 0)
        {
            //Debug.Log("Start running");
            runningState = RunningState.Running;
            moveSpeed = runningSpeed;
            SoundManager.instance?.Play("RunningBreath");
        }
        else if(runningState == RunningState.Running && Input.GetButtonUp("Run"))
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
        else if(runningState == RunningState.Running || staredCount > 0)
        {
            if(energy > 0)
                energy -= Time.deltaTime;
            UIManager.instance?.energyBar.SetFill(energy);
        }
        else if(runningState != RunningState.Running && staredCount == 0 && energy < maxEnergy)
        {
            energy += Time.deltaTime/2;
            UIManager.instance?.energyBar.SetFill(energy);
        }
        vignette.intensity.value = 1 - (energy / maxEnergy);
        //Debug.Log("Present Energy :" + presentEnergy);
    }

    void FixedUpdate()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
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
        if (energy >= maxEnergy)
            return true;
        return false;
    }

    public void StartBeingStared()
    {
        staredCount++;
        if (staredCount == 1)
            SoundManager.instance?.Play("Scaring");
    }

    public void StopBeingStared()
    {
        staredCount--;
        if(staredCount == 0 && runningState != RunningState.Running)
            SoundManager.instance?.StopPlay("Scaring", 1f);
    }
}
