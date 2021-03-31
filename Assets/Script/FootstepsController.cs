using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsController: MonoBehaviour
{
    public Transform rightStep;
    public Transform leftStep;
    public float stepDistance;
    public float footprintEffectiveDuration;
    public GameObject legs = null;
    float footprintEffectiveDurationLeft = 0f;
    bool nextRightStep = true;
    bool creatingFootstepSound = false;
    string footstepSoundType = "NormalFootstepSound";
    public GameObject splash;
    Vector2 lastPosition;
    Transform footprintCreatorTransform;
    public bool turnNinetyDegree = true;

    void Start()
    {
        if (legs == null)
            footprintCreatorTransform = transform;
        else
            footprintCreatorTransform = legs.transform;

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
        if (nextRightStep)
        {   
            Transform step = Instantiate(rightStep, footprintCreatorTransform.position, footprintCreatorTransform.rotation);
            if (legs == null)
            {
                if(turnNinetyDegree)
                    step.Rotate(0, 0, -90);
                else
                    step.Rotate(0, 0, 180);
            }
            else
            {
                if(Mathf.Abs(footprintCreatorTransform.rotation.z - transform.rotation.z + 90) > 90)
                    step.Rotate(0, 0, 180);
            }
            Renderer rend = step.GetComponent<Renderer>();
            Color c = rend.material.color;
            rend.material.color = new Color(c.r, c.g, c.b, c.a * footprintEffectiveDurationLeft / footprintEffectiveDuration);
            nextRightStep = false;
        }
        else
        {
            Transform step = Instantiate(leftStep, footprintCreatorTransform.position, footprintCreatorTransform.rotation);
            if (legs == null)
            {
                if (turnNinetyDegree)
                    step.Rotate(0, 0, -90);
                else
                    step.Rotate(0, 0, 180);

            }
            else
            {
                if (Mathf.Abs(footprintCreatorTransform.rotation.z - transform.rotation.z + 90) > 90)
                    step.Rotate(0, 0, 180);
            }
            Renderer rend = step.GetComponent<Renderer>();
            Color c = rend.material.color;
            rend.material.color = new Color(c.r, c.g, c.b, c.a * footprintEffectiveDurationLeft / footprintEffectiveDuration);
            nextRightStep = true;
        }
    }

    void CreatFootstepSound()
    {
        SoundManager.instance.Play(footstepSoundType, 0, transform);
        if (footstepSoundType == "PuddleFootstepSound")
            Instantiate(splash, transform.position, transform.rotation);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Puddle")
        {
            //Debug.Log("Step in puddle");
            creatingFootstepSound = true;
            footprintEffectiveDurationLeft = 0;
            footstepSoundType = collision.tag + "FootstepSound";
        }

        if (collision.gameObject.tag == "Podium")
        {
            //Debug.Log("Step in podium");
            creatingFootstepSound = true;
            footstepSoundType = collision.tag + "FootstepSound";
        }

        if (collision.gameObject.tag == "Soil")
        {
            //Debug.Log("Step in soil");
            creatingFootstepSound = true;
            footstepSoundType = collision.tag + "FootstepSound";
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Puddle")
        {
            //Debug.Log("Step out puddle");
            footprintEffectiveDurationLeft = footprintEffectiveDuration;
            creatingFootstepSound = false;
        }
        
        if (collision.gameObject.tag == "Podium")
        {
            //Debug.Log("Step out podium");
            creatingFootstepSound = false;
        }

        if (collision.gameObject.tag == "Soil")
        {
            //Debug.Log("Step in soil");
            creatingFootstepSound = false;
        }
    }
}
