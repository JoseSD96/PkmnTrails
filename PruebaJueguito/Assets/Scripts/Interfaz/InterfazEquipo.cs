using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InterfazEquipo : MonoBehaviour
{
    [SerializeField] Equipo equipo;
    [SerializeField] GameObject[] botones;
    [SerializeField] GameObject botonesOpcion;

    public Pokemon PokemonSeleccionado { get; private set; }
    public Equipo Equipo => equipo;

    public int indiceSeleccionado = -1;

    private bool modoIntercambio = false;
    private int indiceIntercambio = -1;

    void Start()
    {
        botonesOpcion.SetActive(false);

        for (int i = 0; i < botones.Length; i++)
        {
            int index = i;
            var btn = botones[i].GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => SeleccionarPokemon(index));
            }
        }
        ActualizarInterfaz();
    }

    void Update()
    {
        ActualizarInterfaz();
    }

    public void IniciarIntercambio()
    {
        if (equipo.pokemones.Count > 1)
        {
            modoIntercambio = true;
            indiceIntercambio = indiceSeleccionado;
            LimpiarSeleccion();
        }
    }

    public void SeleccionarPokemon(int indice)
    {
        if (modoIntercambio)
        {
            if (indiceIntercambio != -1 && indice != indiceIntercambio && indice < equipo.pokemones.Count)
            {
                equipo.IntercambiarPokemones(indiceIntercambio, indice);
                modoIntercambio = false;
                indiceIntercambio = -1;
                
                ActualizarInterfaz();
            }
            return;
        }

        if (indice < equipo.pokemones.Count && equipo.pokemones[indice] != null)
        {
            PokemonSeleccionado = equipo.pokemones[indice];
            indiceSeleccionado = indice;
            botonesOpcion.SetActive(true);

            if (equipo.pc != null && equipo.pc.esperandoIntercambioConEquipo)
            {
                equipo.pc.IntercambiarConEquipo();
                equipo.pc.esperandoIntercambioConEquipo = false;
            }
        }
        else
        {
            botonesOpcion.SetActive(false);
        }
    }

    public void LimpiarSeleccion()
    {
        indiceSeleccionado = -1;
        botonesOpcion.SetActive(false);

        for (int i = 0; i < botones.Length; i++)
        {
            var btnImage = botones[i].GetComponent<Image>();
            if (btnImage != null)
                btnImage.color = Color.white;
        }
    }

    public void ActualizarInterfaz()
    {

        if (botones == null || equipo == null || equipo.pokemones == null)
            return;

        for (int i = 0; i < botones.Length; i++)
        {

            SpriteRenderer spritePokemon = null;
            TextMeshProUGUI nombreTxt = null;
            TextMeshProUGUI nivelTxt = null;

            var sprite = botones[i].GetComponentsInChildren<SpriteRenderer>(true);
            foreach (var img in sprite)
            {
                if (img.gameObject != botones[i])
                {
                    spritePokemon = img;
                    break;
                }
            }
            HPBar hpBar = botones[i].GetComponentInChildren<HPBar>(true);
            var textos = botones[i].GetComponentsInChildren<TextMeshProUGUI>(true);
            if (textos.Length >= 2)
            {
                nombreTxt = textos[0];
                nivelTxt = textos[1];
            }


            if (i < equipo.pokemones.Count && equipo.pokemones[i] != null)
            {
                var pokemon = equipo.pokemones[i];

                spritePokemon.sprite = pokemon.isShiny ? pokemon.Base.SpriteIdleShiny : pokemon.Base.SpriteIdle;
                spritePokemon.enabled = true;

                hpBar.gameObject.SetActive(true);
                hpBar.SetMaxHP(pokemon.MaxHP);
                hpBar.setHP(pokemon.HP);

                nombreTxt.text = pokemon.Base.Nombre;
                nombreTxt.gameObject.SetActive(true);
                nivelTxt.text = "Nv. " + pokemon.Nivel;
                nivelTxt.gameObject.SetActive(true);

                var shinyGO = botones[i].transform.Find("shiny");
                if (shinyGO != null)
                    shinyGO.gameObject.SetActive(pokemon.isShiny);
            }
            else
            {
                spritePokemon.enabled = false;
                hpBar.gameObject.SetActive(false);
                nombreTxt.gameObject.SetActive(false);
                nivelTxt.gameObject.SetActive(false);

                var shinyGO = botones[i].transform.Find("shiny");
                if (shinyGO != null)
                    shinyGO.gameObject.SetActive(false);
            }
            var btnImage = botones[i].GetComponent<Image>();
            if (btnImage != null)
            {
                if (i == indiceSeleccionado)
                    btnImage.color = Color.yellow;
                else
                    btnImage.color = Color.white;
            }
        }
    }
}
