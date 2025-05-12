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
    [SerializeField] TextMeshProUGUI txtEfectividadAtk1;
    [SerializeField] TextMeshProUGUI txtEfectividadAtk2;
    PkmnCombate pokemonCombate;
    [SerializeField] Player jugador;
    Vector3 posInicial;

    /// <summary>
    /// Inicializa la posición inicial del HUD y deja los botones y elementos visuales transparentes.
    /// </summary>
    private void Awake()
    {
        posInicial = transform.localPosition;
        var colorExtra = extra.color;
        colorExtra.a = 0f;
        extra.color = colorExtra;

        var colorBtn1 = atk1.image.color;
        colorBtn1.a = 0f;
        atk1.image.color = colorBtn1;

        var colorBtn2 = atk2.image.color;
        colorBtn2.a = 0f;
        atk2.image.color = colorBtn2;

        var colorBtnHuir = huirBtn.image.color;
        colorBtnHuir.a = 0f;
        huirBtn.image.color = colorBtnHuir;

        var color1 = txtEfectividadAtk1.color;
        color1.a = 0f;
        txtEfectividadAtk1.color = color1;

        var color2 = txtEfectividadAtk2.color;
        color2.a = 0f;
        txtEfectividadAtk2.color = color2;
    }

    /// <summary>
    /// Configura el HUD con los datos del Pokémon en combate, actualizando nombre, nivel, barras y sprites.
    /// También gestiona la visualización de tipos, shiny, capturado y lanza la animación de entrada.
    /// </summary>
    /// <param name="pokemonCombate">Instancia de PkmnCombate a mostrar.</param>
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

        if (pokemonCombate.isEnemy)
        {
            bool capturado = jugador.pokedex.pokemones.Exists(
                p => p.Num == pokemonCombate.Pkmn.Base.Num && p.Nombre == pokemonCombate.Pkmn.Base.Nombre
            );
            iconoCapturado.gameObject.SetActive(capturado);
        }

        ResetAnimacionEntrada();

        AnimacionEntrada();
    }

    /// <summary>
    /// Corrutina que actualiza la barra de vida del Pokémon con animación.
    /// </summary>
    public IEnumerator UpdateHP()
    {
        yield return hpBar.animacionHP(pokemonCombate.Pkmn.HP);
    }

    /// <summary>
    /// Realiza la animación de entrada del HUD, desplazando y mostrando los botones y elementos.
    /// </summary>
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
        secuencia.Join(txtEfectividadAtk1.DOFade(1f, 2f));
        secuencia.Join(txtEfectividadAtk2.DOFade(1f, 2f));
        secuencia.Join(huirBtn.image.DOFade(1f, 2f));
    }

    /// <summary>
    /// Realiza la animación de derrota, ocultando el HUD y los botones.
    /// </summary>
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

    /// <summary>
    /// Resetea la animación de entrada, colocando el HUD fuera de pantalla y ocultando los botones.
    /// </summary>
    public void ResetAnimacionEntrada()
    {
        if (!pokemonCombate.isEnemy)
            transform.localPosition = new Vector3(posInicial.x, 250f);
        else
            transform.localPosition = new Vector3(posInicial.x, -250f);

        var colorExtra = extra.color;
        colorExtra.a = 0f;
        extra.color = colorExtra;

        var colorBtn1 = atk1.image.color;
        colorBtn1.a = 0f;
        atk1.image.color = colorBtn1;

        var colorBtn2 = atk2.image.color;
        colorBtn2.a = 0f;
        atk2.image.color = colorBtn2;

        var colorBtnHuir = huirBtn.image.color;
        colorBtnHuir.a = 0f;
        huirBtn.image.color = colorBtnHuir;

        var color1 = txtEfectividadAtk1.color;
        color1.a = 0f;
        txtEfectividadAtk1.color = color1;

        var color2 = txtEfectividadAtk2.color;
        color2.a = 0f;
        txtEfectividadAtk2.color = color2;
    }
}
