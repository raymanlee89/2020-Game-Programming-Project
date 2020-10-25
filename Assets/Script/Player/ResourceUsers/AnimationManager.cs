using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    int frashlightMode = 0; // 0 : frashlight off, 1 : frashlight on
    public Sprite[] defaultImage; // 1 : with frashlight

    public Sprite takeOutFrashlightImage;

    public Sprite[] takingImage; // 1 : with frashlight

    public Sprite[] holdInHandImage; // row : 0 -> bandage, 1 -> water, 2 -> glow stick, 3 -> flare
    public Sprite[] usingImage;

    public Sprite[] holdInHandWithFrashlightImage; // row : 0 -> bandage, 1 -> water, 2 -> glow stick, 3 -> flare
    public Sprite[] usingWithFrashlightImage;

    bool flareIsInHand = false;
    SpriteRenderer GFX;

    #region Singleton
    public static AnimationManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one of instance of UImanager found!");
            return;
        }
        instance = this;
    }

    #endregion

    private void Start()
    {
        GFX = GetComponent<SpriteRenderer>();
    }

    public void SwitchFrashlight(bool toTurnOn)
    {
        if (toTurnOn)
            StartCoroutine(TurnOnFrashlight());
        else
            StartCoroutine(TurnOffFrashlight());
    }

    IEnumerator TurnOnFrashlight()
    {
        GFX.sprite = takeOutFrashlightImage;
        yield return new WaitForSeconds(0.2f);
        if(flareIsInHand)
            GFX.sprite = holdInHandWithFrashlightImage[3];
        else
            GFX.sprite = defaultImage[1];
        frashlightMode = 1;
    }

    IEnumerator TurnOffFrashlight()
    {
        GFX.sprite = takeOutFrashlightImage;
        yield return new WaitForSeconds(0.2f);
        if (flareIsInHand)
            GFX.sprite = holdInHandImage[3];
        else
            GFX.sprite = defaultImage[0];
        frashlightMode = 0;
    }

    public void UseItem(string userType)
    {
        Debug.Log("Call use item function");
        StartCoroutine(UseItemAnimation(userType));
    }

    IEnumerator UseItemAnimation(string userType)
    {
        float duration = 0.2f;
        Debug.Log("Animation start");
        int userTypeIndex = -1;
        if (userType == "Bandage")
        {
            duration = 0.5f;
            userTypeIndex = 0;
        }
        else if (userType == "Water")
        {
            duration = 0.7f;
            userTypeIndex = 1;
        }
        else if (userType == "GlowStick")
            userTypeIndex = 2;
        else if (userType == "Flare")
            userTypeIndex = 3;

        GFX.sprite = takingImage[frashlightMode];
        yield return new WaitForSeconds(0.2f);
        if(frashlightMode == 0)
        {
            GFX.sprite = holdInHandImage[userTypeIndex];
            yield return new WaitForSeconds(0.2f);
            GFX.sprite = usingImage[userTypeIndex];
            yield return new WaitForSeconds(duration);
        }
        else
        {
            GFX.sprite = holdInHandWithFrashlightImage[userTypeIndex];
            yield return new WaitForSeconds(0.2f);
            GFX.sprite = usingWithFrashlightImage[userTypeIndex];
            yield return new WaitForSeconds(duration);
        }

        if(userTypeIndex == 3) // deal with flare
        {
            flareIsInHand = true;
            if (frashlightMode == 0)
            {
                GFX.sprite = holdInHandImage[userTypeIndex];
            }
            else
            {
                GFX.sprite = holdInHandWithFrashlightImage[userTypeIndex];
            }
            yield return new WaitForSeconds(6f);
            flareIsInHand = false;
        }
        GFX.sprite = defaultImage[frashlightMode];
    }
}
