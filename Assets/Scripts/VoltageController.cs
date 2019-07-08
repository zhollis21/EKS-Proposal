﻿using UnityEngine;
using UnityEngine.UI;

public class VoltageController : MonoBehaviour
{
    public Slider slider;
    public Image sliderFill;
    public InputField inputField;
    public Image circleFill;
    public int minValue;
    public int maxValue;
    public float defaultValue;

    private Color _green = new Color(0, 1, 0, 0.75f);
    private Color _yellow = new Color(1, 1, 0, 0.75f);
    private Color _red = new Color(1, 0, 0, 0.7f);

    // Start is called before the first frame update
    void Start()
    {
        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.value = defaultValue;
        SetImageColor(sliderFill);
        slider.onValueChanged.AddListener(x => OnSliderChanged(x));

        inputField.text = slider.value.ToString();
        inputField.onValueChanged.AddListener(x => OnInputFieldChanged(x));

        UpdateCircleFill();
    }

    private void SetImageColor(Image img)
    {
        if (slider.normalizedValue < .5)
        {
            img.color = Color.Lerp(_green, _yellow, slider.normalizedValue * 2);
        }
        else
        {
            img.color = Color.Lerp(_yellow, _red, (slider.normalizedValue - 0.5f) * 2);
        }
    }

    private void UpdateCircleFill()
    {
        if (circleFill != null)
        {
            circleFill.fillAmount = slider.normalizedValue;
            SetImageColor(circleFill);
        }
    }

    private void OnSliderChanged(float value)
    {
        // Slider has build in min and max so we don't need to do any checking
        inputField.text = value.ToString();
        SetImageColor(sliderFill);
        UpdateCircleFill();
    }

    private void OnInputFieldChanged(string input)
    {
        if (!float.TryParse(input, out float value))
        {
            return;
        }

        if (value > maxValue)
        {
            inputField.text = maxValue.ToString();
        }
        else if (value < minValue)
        {
            inputField.text = minValue.ToString();
        }
        else
        {
            slider.value = value;
            SetImageColor(sliderFill);
            UpdateCircleFill();
        }
    }
}