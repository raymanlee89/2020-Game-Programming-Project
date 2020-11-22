using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private enum RightHandMode
    {
        Nothing,
        Frashlight,
        Flare
    }

    RightHandMode rightHandMode;
    public Sprite[] defaultImage; // 1 : with frashlight, 2 : with flare (right hand)

    public Sprite takeOutFrashlightImage;
    public Sprite[] flareImage; // 0 : take , 1 : trigger, 2 : hold

    public Sprite[] takingImage; // 1 : with frashlight

    public Sprite[] holdInHandImage; // 0 -> bandage, 1 -> water, 2 -> glow stick (left hand)
    public Sprite[] usingImage;

    public Sprite[] holdInHandWithFrashlightImage; // 0 -> bandage, 1 -> water, 2 -> glow stick (left hand)
    public Sprite[] usingWithFrashlightImage;

    public Sprite[] holdInHandWithFlareImage; // 0 -> bandage, 1 -> water, 2 -> glow stick (left hand)
    public Sprite[] usingWithFlareImage;

    [HideInInspector]
    public bool flareIsInHand = false;
    SpriteRenderer GFX;

    #region Singleton
    public static AnimationManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one of instance of AnimationManager found!");
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
        GFX.sprite = defaultImage[1];
        rightHandMode = RightHandMode.Frashlight;
    }

    IEnumerator TurnOffFrashlight()
    {
        GFX.sprite = takeOutFrashlightImage;
        yield return new WaitForSeconds(0.2f);
        GFX.sprite = defaultImage[0];
        rightHandMode = RightHandMode.Nothing;
    }

    public void UseItem(string userType)
    {
        Debug.Log("Call use item function");
        StartCoroutine(UseItemAnimation(userType));
    }

    IEnumerator UseItemAnimation(string userType)
    {
        int mode = 0;
        if (rightHandMode == RightHandMode.Frashlight)
            mode = 1;
        else if (rightHandMode == RightHandMode.Flare)
            mode = 2;

        GFX.sprite = takingImage[mode];

        if (userType == "Flare")
        {
            flareIsInHand = true;
            for(int i = 0 ; i < flareImage.Length; i++)
            {
                yield return new WaitForSeconds(0.2f);
                GFX.sprite = flareImage[i];
            }
            rightHandMode = RightHandMode.Flare;
            yield return new WaitForSeconds(11f);
            flareIsInHand = false;
            rightHandMode = RightHandMode.Nothing;
        }
        else
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

            yield return new WaitForSeconds(0.2f);
            if (rightHandMode == RightHandMode.Nothing)
            {
                GFX.sprite = holdInHandImage[userTypeIndex];
                yield return new WaitForSeconds(0.2f);
                GFX.sprite = usingImage[userTypeIndex];
                yield return new WaitForSeconds(duration);
            }
            else if (rightHandMode == RightHandMode.Frashlight)
            {
                GFX.sprite = holdInHandWithFrashlightImage[userTypeIndex];
                yield return new WaitForSeconds(0.2f);
                GFX.sprite = usingWithFrashlightImage[userTypeIndex];
                yield return new WaitForSeconds(duration);
            }
            else if (rightHandMode == RightHandMode.Flare)
            {
                GFX.sprite = holdInHandWithFlareImage[userTypeIndex];
                yield return new WaitForSeconds(0.2f);
                GFX.sprite = usingWithFlareImage[userTypeIndex];
                yield return new WaitForSeconds(duration);
            }
        }

        mode = 0;
        if (rightHandMode == RightHandMode.Frashlight)
            mode = 1;
        else if (rightHandMode == RightHandMode.Flare)
            mode = 2;
        GFX.sprite = defaultImage[mode];
    }
}
