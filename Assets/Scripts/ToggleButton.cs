using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public Text text;

    private Color offColor = new Color(225 / 255f, 225 / 255f, 225 / 255f);
    private Color onColor = new Color(122 / 255f, 122 / 255f, 122 / 255f);
    private Color onTextColor = new Color(19 / 255f, 234 / 255f, 250 / 255f);
    private Toggle toggle;

    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(x => UpdateToggle(x));

        UpdateToggle(toggle.isOn);
    }

    public void UpdateToggle(bool isOn)
    {
        if (isOn)
        {
            toggle.image.color = onColor;
            text.color = onTextColor;
        }
        else
        {
            toggle.image.color = offColor;
            text.color = Color.black;
        }
    }
}
