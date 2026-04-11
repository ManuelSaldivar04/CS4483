using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatBars : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public Slider hSlider;
    public TextMeshProUGUI cText;
    public Slider cSlider;

    public void setHealth(int curr, int max)
    {
        float ratio = ((float)curr / (float)max) * 100;
        hSlider.value = ratio;
        hpText.text = curr + "/" + max;
    }

    public void setCombat(int curr, int max)
    {
        float ratio = ((float)curr / (float)max) * 100;
        cSlider.value = ratio;
        cText.text = curr + "/" + max;
    }
}
