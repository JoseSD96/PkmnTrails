using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{

    public Slider slider;
    public Image hpBar;

    public void SetXPNecesaria(int xp)
    {
        slider.maxValue = xp;
    }

    public void setXP(int xp)
    {
        slider.value = xp;

    }

}
