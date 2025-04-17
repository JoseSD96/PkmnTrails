using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image hpBar;

    public void SetMaxHP(int hp)
    {
        slider.maxValue = hp;
        slider.value = hp;

        hpBar.color = gradient.Evaluate(1f);
    }

    public void setHP(int hp)
    {
        slider.value = hp;

        hpBar.color = gradient.Evaluate(slider.normalizedValue);
    }

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
