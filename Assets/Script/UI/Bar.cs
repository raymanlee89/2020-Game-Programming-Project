using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxFill(float maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = maxValue;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetFill(float value)
    {
        slider.value = value;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
