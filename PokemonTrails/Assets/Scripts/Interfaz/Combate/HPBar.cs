using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image hpBar;

    /// <summary>
    /// Establece el valor máximo de la barra de vida y la rellena completamente.
    /// También ajusta el color de la barra al máximo (verde).
    /// </summary>
    /// <param name="hp">Valor máximo de HP.</param>
    public void SetMaxHP(int hp)
    {
        slider.maxValue = hp;
        slider.value = hp;
        hpBar.color = gradient.Evaluate(1f);
    }

    /// <summary>
    /// Actualiza el valor actual de la barra de vida y ajusta su color según el porcentaje restante.
    /// </summary>
    /// <param name="hp">Valor actual de HP.</param>
    public void setHP(int hp)
    {
        slider.value = hp;
        hpBar.color = gradient.Evaluate(slider.normalizedValue);
    }

    /// <summary>
    /// Corrutina que anima la transición de la barra de vida desde el valor actual hasta el nuevo valor.
    /// Cambia el color y el valor de la barra de forma suave durante medio segundo.
    /// </summary>
    /// <param name="newHP">Nuevo valor de HP al que animar la barra.</param>
    public IEnumerator animacionHP(int newHP)
    {
        float HPActual = slider.value;
        float duracion = 0.5f;
        float tiempo = 0f;
        float inicio = HPActual;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.Clamp01(tiempo / duracion);
            float valor = Mathf.Lerp(inicio, newHP, t);
            setHP((int)valor);
            yield return null;
        }

        setHP(newHP);
    }
}
