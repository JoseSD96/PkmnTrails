using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    public Slider slider;
    public Image hpBar;

    /// <summary>
    /// Establece el valor máximo de experiencia necesaria para subir de nivel.
    /// </summary>
    /// <param name="xp">Experiencia necesaria para el siguiente nivel.</param>
    public void SetXPNecesaria(int xp)
    {
        slider.maxValue = xp;
    }

    /// <summary>
    /// Actualiza el valor actual de experiencia en la barra.
    /// </summary>
    /// <param name="xp">Experiencia actual del Pokémon.</param>
    public void setXP(int xp)
    {
        slider.value = xp;
    }
}
