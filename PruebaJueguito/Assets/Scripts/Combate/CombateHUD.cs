using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombateHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameTxt;
    [SerializeField] TextMeshProUGUI lvlTxt;
    [SerializeField] HPBar hpBar;
    [SerializeField] Image ShinyStars;
    [SerializeField] Image tipo1;
    [SerializeField] Image tipo2;
    [SerializeField] Button atk1;
    [SerializeField] Button atk2;

    [SerializeField] Image extra;

    PkmnCombate pokemonCombate;

    Vector3 posInicial;
    private void Awake()
    {
        posInicial = transform.localPosition;
        Color colorExtra = extra.color;
        colorExtra.a = 0f;
        extra.color = colorExtra;

        Color colorBtn1 = atk1.image.color;
        colorBtn1.a = 0f;
        atk1.image.color = colorBtn1;

        Color colorBtn2 = atk2.image.color;
        colorBtn2.a = 0f;
        atk2.image.color = colorBtn2;
    }


    public void SetData(PkmnCombate pokemonCombate)
    {

        this.pokemonCombate = pokemonCombate;

        nameTxt.text = pokemonCombate.Pkmn.Base.Nombre;
        lvlTxt.text = "Lvl " + pokemonCombate.Pkmn.Nivel;
        hpBar.SetMaxHP(pokemonCombate.Pkmn.MaxHP);
        hpBar.setHP(pokemonCombate.Pkmn.HP);
        ShinyStars.enabled = pokemonCombate.Pkmn.isShiny;
        tipo1.sprite = pokemonCombate.Pkmn.Base.SpriteTipo1;
        if (pokemonCombate.Pkmn.Base.SpriteTipo2 == null)
        {
            tipo2.enabled = false;
        }
        else
        {
            tipo2.sprite = pokemonCombate.Pkmn.Base.SpriteTipo2;
        }

        if (!pokemonCombate.isEnemy)
        {
            string tipo1Txt = pokemonCombate.Pkmn.Base.Tipo1.ToString();
            string tipo2Txt = pokemonCombate.Pkmn.Base.Tipo2.ToString();
            if (tipo2Txt == "None")
            {
                tipo2Txt = "Normal";
            }
            atk1.image.sprite = Resources.Load<Sprite>("Combate/tipos/" + tipo1Txt);
            atk2.image.sprite = Resources.Load<Sprite>("Combate/tipos/" + tipo2Txt);
        }
        AnimacionEntrada();
    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.animacionHP(pokemonCombate.Pkmn.HP);
    }

    public void AnimacionEntrada()
    {
        if (!pokemonCombate.isEnemy)
        {
            transform.localPosition = new Vector3(posInicial.x, 250f);
        }
        else
        {
            transform.localPosition = new Vector3(posInicial.x, -250f);
        }

        var secuencia = DOTween.Sequence();
        secuencia.Append(transform.DOLocalMoveY(posInicial.y, 2f));

        secuencia.Append(atk1.image.DOFade(1f, 2f));
        secuencia.Join(atk2.image.DOFade(1f, 2f));
        secuencia.Join(extra.DOFade(1f, 2f));
    }

    public void AnimacionDerrota()
    {
        var secuencia = DOTween.Sequence();

        if (!pokemonCombate.isEnemy)
        {
            secuencia.Append(transform.DOLocalMoveY(posInicial.y - 250f, 0.5f));
            secuencia.Join(atk1.image.DOFade(0f, 2f));
            secuencia.Join(atk2.image.DOFade(0f, 2f));
        }
        else
        {
            secuencia.Append(transform.DOLocalMoveY(posInicial.y + 250f, 0.5f));
        }
        secuencia.Join(extra.DOFade(0f, 2f));
    }

}
