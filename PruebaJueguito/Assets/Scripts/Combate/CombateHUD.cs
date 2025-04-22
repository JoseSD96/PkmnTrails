using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombateHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameTxt;
    [SerializeField] TextMeshProUGUI lvlTxt;
    [SerializeField] HPBar hpBar;
    [SerializeField] XPBar xpBar;
    [SerializeField] Image ShinyStars;
    [SerializeField] Image tipo1;
    [SerializeField] Image tipo2;
    [SerializeField] SpriteRenderer iconoCapturado;
    [SerializeField] Button atk1;
    [SerializeField] Button atk2;
    [SerializeField] Button huirBtn;
    [SerializeField] Image extra;

    PkmnCombate pokemonCombate;
    [SerializeField] Player jugador;
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

        Color colorBtnHuir = huirBtn.image.color;
        colorBtnHuir.a = 0f;
        huirBtn.image.color = colorBtnHuir;
    }


    public void SetData(PkmnCombate pokemonCombate)
    {

        this.pokemonCombate = pokemonCombate;

        nameTxt.text = pokemonCombate.Pkmn.Base.Nombre;
        lvlTxt.text = "Lvl " + pokemonCombate.Pkmn.Nivel;
        hpBar.SetMaxHP(pokemonCombate.Pkmn.MaxHP);
        hpBar.setHP(pokemonCombate.Pkmn.HP);
        xpBar.SetXPNecesaria(pokemonCombate.Pkmn.ExperienciaParaSubirNivel());
        xpBar.setXP(pokemonCombate.Pkmn.Experiencia);

        ShinyStars.enabled = pokemonCombate.Pkmn.isShiny;
        tipo1.sprite = pokemonCombate.Pkmn.Base.SpriteTipo1;
        if (pokemonCombate.Pkmn.Base.SpriteTipo2 == null)
        {
            tipo2.enabled = false;
        }
        else
        {
            tipo2.enabled = true;
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
        
        if(pokemonCombate.isEnemy)
        {
            if (jugador.Pokedex.Contains(pokemonCombate.Pkmn.Base.Num))
            {
                iconoCapturado.enabled = true;
            }
            else
            {
                iconoCapturado.enabled = false;
            }
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
        secuencia.Join(huirBtn.image.DOFade(1f, 2f));
    }

    public void AnimacionDerrota()
    {
        var secuencia = DOTween.Sequence();

        if (!pokemonCombate.isEnemy)
        {
            secuencia.Append(transform.DOLocalMoveY(posInicial.y - 250f, 0.5f));
            secuencia.Join(atk1.image.DOFade(0f, 0.5f));
            secuencia.Join(atk2.image.DOFade(0f, 0.5f));
            secuencia.Join(huirBtn.image.DOFade(0f, 0.5f));
        }
        else
        {
            secuencia.Append(transform.DOLocalMoveY(posInicial.y + 250f, 0.5f));
        }
        secuencia.Join(extra.DOFade(0f, 0.5f));
    }

}
