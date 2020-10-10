using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsController: MonoBehaviour
{
    public Transform rightStep;
    public Transform leftStep;
    public float stepDistance;
    public float footprintEffectiveDuration;
    float footprintEffectiveDurationLeft = 0f;
    bool nextRightStep = true;
    bool creatingFootstepSound = false;
    string footstepSoundType;
    Vector2 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        if (footprintEffectiveDurationLeft <= 0 && !creatingFootstepSound)
            return;

        footprintEffectiveDurationLeft -= Time.deltaTime;
        if (Vector2.Distance(lastPosition, transform.position) > stepDistance)
        {
            if (footprintEffectiveDurationLeft > 0)
            {
                CreatFootprint();
            }
            if (creatingFootstepSound)
            {
                CreatFootstepSound();
            }
            lastPosition = transform.position;
        }
    }

    void CreatFootprint()
    {
        Debug.Log("Creat footprint");
        if (nextRightStep)
        {
            Transform step = Instantiate(rightStep, transform.position, transform.rotation);
            Renderer rend = step.GetComponent<Renderer>();
            Color c = rend.material.color;
            rend.material.color = new Color(c.r, c.g, c.b, c.a * footprintEffectiveDurationLeft / footprintEffectiveDuration);
            nextRightStep = false;
        }
        else
        {
            Transform step = Instantiate(leftStep, transform.position, transform.rotation);
            Renderer rend = step.GetComponent<Renderer>();
            Color c = rend.material.color;
            rend.material.color = new Color(c.r, c.g, c.b, c.a * footprintEffectiveDurationLeft / footprintEffectiveDuration);
            nextRightStep = true;
        }
    }

    void CreatFootstepSound()
    {
        SoundManager.instance.Play(footstepSoundType);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Puddle")
        {
            Debug.Log("Step in puddle");
            creatingFootstepSound = true;
            footstepSoundType = collision.tag + "FootstepSound";
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Puddle")
        {
            Debug.Log("Step out puddle");
            footprintEffectiveDurationLeft = footprintEffectiveDuration;
            creatingFootstepSound = false;
        }
    }
}
