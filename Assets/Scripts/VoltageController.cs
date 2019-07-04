using UnityEngine;
using UnityEngine.UI;

public class VoltageController : MonoBehaviour
{
    public Slider slider;
    public Image sliderFill;
    public InputField inputField;
    public int minValue;
    public int maxValue;
    public float defaultValue;

    private Color _green = new Color(0, 1, 0);
    private Color _yellow = new Color(1, 1, 0);
    private Color _red = new Color(1, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.value = defaultValue;
        SetSliderColor();
        slider.onValueChanged.AddListener(x => OnSliderChanged(x));

        inputField.text = slider.value.ToString();
        inputField.onValueChanged.AddListener(x => OnInputFieldChanged(x));
    }

    private void SetSliderColor()
    {
        if (slider.normalizedValue < .5)
        {
            sliderFill.color = Color.Lerp(_green, _yellow, slider.normalizedValue * 2);
        }
        else
        {
            sliderFill.color = Color.Lerp(_yellow, _red, (slider.normalizedValue - 0.5f) * 2);
        }
    }

    private void OnSliderChanged(float value)
    {
        // Slider has build in min and max so we don't need to do any checking
        inputField.text = value.ToString();
        SetSliderColor();
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
            SetSliderColor();
        }
    }
}