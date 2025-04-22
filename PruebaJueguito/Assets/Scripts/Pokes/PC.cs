using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PC : MonoBehaviour
{
    public Dictionary<int, List<Pokemon>> cajas;
    public TextMeshProUGUI textoCajaActual;
    public TextMeshProUGUI textoNumero;
    public TextMeshProUGUI textoNombre;
    public TextMeshProUGUI textoNivel;
    public TextMeshProUGUI textoPotencial;
    public int indiceSeleccionado = -1;
    public int cajaActual = 0;
    public GameObject caja;
    public GameObject btnOpciones;
    public GameObject panelDatos;
    private List<GameObject> botonesCajaActual = new List<GameObject>();
    public InterfazEquipo interfazEquipo;
    public RectTransform panelEquipo;
    public GameObject[] botonesPanelEquipo;
    public bool esperandoIntercambioConEquipo = false;

    void Awake()
    {
        panelEquipo.anchoredPosition = new Vector2(panelEquipo.anchoredPosition.x, 440);
    }

    void Start()
    {
        if (cajas == null)
        {
            cajas = new Dictionary<int, List<Pokemon>>();
            for (int i = 0; i < 18; i++)
            {
                if (!cajas.ContainsKey(i))
                {
                    cajas.Add(i, new List<Pokemon>());
                }

            }
        }

        botonesCajaActual.Clear();
        for (int i = 0; i < 30; i++)
        {
            botonesCajaActual.Add(caja.transform.GetChild(i).gameObject);
        }

        // Asumiendo que panelEquipo tiene exactamente 6 hijos, uno por cada slot del equipo
        botonesPanelEquipo = new GameObject[6];
        for (int i = 0; i < 6; i++)
        {
            botonesPanelEquipo[i] = panelEquipo.GetChild(i).gameObject;

            // Asigna el evento onClick para cada botón del equipo
            var btn = botonesPanelEquipo[i].GetComponent<Button>();
            int index = i; // Necesario para la clausura
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => interfazEquipo.SeleccionarPokemon(index));
            }
        }
    }

    public int GetCajaConHueco()
    {
        for (int i = 0; i < cajas.Count; i++)
        {
            if (cajas[i].Count < 30)
            {
                return i;
            }
        }
        return -1;
    }

    public void AddPokemon(Pokemon pokemon)
    {
        cajas[GetCajaConHueco()].Add(pokemon);
    }

    public void MostrarCaja(int numCaja)
    {
        textoCajaActual.text = "Caja " + (numCaja + 1).ToString();
        var pokemons = cajas[numCaja];
        for (int i = 0; i < botonesCajaActual.Count; i++)
        {
            var boton = botonesCajaActual[i];
            if (i < pokemons.Count)
            {
                Pokemon poke = pokemons[i];
                boton.SetActive(true);

                var sprites = boton.GetComponentsInChildren<SpriteRenderer>(true);
                if (sprites.Length > 0)
                    if (poke.isShiny)
                    {
                        sprites[0].sprite = poke.Base.SpriteIdleShiny;
                    }
                    else
                    {
                        sprites[0].sprite = poke.Base.SpriteIdle;
                    }

                var shinyObj = boton.transform.Find("shiny");
                shinyObj.gameObject.SetActive(poke.isShiny);

                var btn = boton.GetComponent<Button>();
                if (btn != null)
                {
                    int index = i;
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => SeleccionarPokemon(index));
                }
            }
            else
            {
                boton.SetActive(false);
            }
        }
    }

    public void CambiarCaja(int direccion)
    {
        cajaActual += direccion;
        if (cajaActual < 0) cajaActual = 17;
        if (cajaActual > 17) cajaActual = 0;
        MostrarCaja(cajaActual);
    }

    public void SeleccionarPokemon(int indice)
    {
        indiceSeleccionado = indice;
        var pokemons = cajas[cajaActual];
        for (int i = 0; i < botonesCajaActual.Count; i++)
        {
            var imagen = botonesCajaActual[i].GetComponent<Image>();
            if (imagen != null)
                imagen.color = (i == indice) ? Color.blue : Color.white;
        }
        bool haySeleccion = indice < pokemons.Count;
        panelDatos.SetActive(haySeleccion);
        btnOpciones.SetActive(haySeleccion);

        if (haySeleccion)
        {
            var poke = pokemons[indice];
            textoNumero.text = $"N.{poke.Base.Num:D3}";
            textoNombre.text = poke.Base.Nombre;
            textoNivel.text = $"Nivel {poke.Nivel}";
            textoPotencial.text = $"{poke.Potencial}/31";
        }
        else
        {
            textoNumero.text = "";
            textoNombre.text = "";
            textoNivel.text = "";
            textoPotencial.text = "";
        }
    }

    public void LimpiarSeleccion()
    {
        indiceSeleccionado = -1;

        for (int i = 0; i < botonesCajaActual.Count; i++)
        {
            var imagen = botonesCajaActual[i].GetComponent<Image>();
            if (imagen != null)
                imagen.color = Color.white;
        }

        panelDatos.SetActive(false);
        btnOpciones.SetActive(false);
    }

    public void EliminarPokemonDeCajaActual(int index)
    {
        var pokemons = cajas[cajaActual];
        if (index >= 0 && index < pokemons.Count)
        {
            pokemons.RemoveAt(index);
            MostrarCaja(cajaActual);
            LimpiarSeleccion();
        }
    }

    public void IntercambiarConEquipo()
    {
        if (interfazEquipo.indiceSeleccionado >= 0 && indiceSeleccionado >= 0)
        {
            interfazEquipo.Equipo.IntercambiarConPC(
                interfazEquipo.indiceSeleccionado,
                this,
                indiceSeleccionado,
                cajaActual
            );

            interfazEquipo.ActualizarInterfaz();
            MostrarCaja(cajaActual);
            interfazEquipo.LimpiarSeleccion();
            LimpiarSeleccion();
            ActualizarPanelEquipoSimple(botonesPanelEquipo, interfazEquipo.Equipo, interfazEquipo.indiceSeleccionado);

            OcultarInterfazEquipoEnPC();
        }
    }

    public void IniciarIntercambioConEquipo()
    {
        esperandoIntercambioConEquipo = true;
        MostrarInterfazEquipoEnPC();
    }

    public PCSaveData GetSaveData()
    {
        var saveData = new PCSaveData();
        foreach (var kvp in cajas)
        {
            var listaCaja = new List<PokemonSaveData>();
            foreach (var pokemon in kvp.Value)
            {
                listaCaja.Add(pokemon.GetSaveData());
            }
            saveData.cajas.Add(listaCaja);
        }
        return saveData;
    }

    public void RestoreState(PCSaveData saveData)
    {
        cajas = new Dictionary<int, List<Pokemon>>();
        for (int i = 0; i < 18; i++)
        {
            if (!cajas.ContainsKey(i))
            {
                cajas.Add(i, new List<Pokemon>());
            }

        }
        for (int i = 0; i < saveData.cajas.Count; i++)
        {
            var listaCaja = new List<Pokemon>();
            foreach (var pokemonData in saveData.cajas[i])
            {
                listaCaja.Add(new Pokemon(pokemonData));
            }
            cajas[i] = listaCaja;
        }
    }

    public void MostrarInterfazEquipoEnPC()
    {
        ActualizarPanelEquipoSimple(botonesPanelEquipo, interfazEquipo.Equipo, interfazEquipo.indiceSeleccionado);
        btnOpciones.SetActive(true);
        panelEquipo.DOLocalMoveY(0, 0.4f);
    }

    public void OcultarInterfazEquipoEnPC()
    {
        panelEquipo.DOLocalMoveY(440, 0.4f);
    }

    public void ActualizarPanelEquipoSimple(GameObject[] botones, Equipo equipo, int indiceSeleccionado)
    {
        for (int i = 0; i < botones.Length; i++)
        {
            // Solo sprite y shiny
            SpriteRenderer spritePokemon = botones[i].GetComponentInChildren<SpriteRenderer>(true);
            var shinyGO = botones[i].transform.Find("shiny");

            if (i < equipo.pokemones.Count && equipo.pokemones[i] != null)
            {
                var pokemon = equipo.pokemones[i];
                if (spritePokemon != null)
                {
                    spritePokemon.sprite = pokemon.isShiny ? pokemon.Base.SpriteIdleShiny : pokemon.Base.SpriteIdle;
                    spritePokemon.enabled = true;
                }
                if (shinyGO != null)
                    shinyGO.gameObject.SetActive(pokemon.isShiny);
            }
            else
            {
                if (spritePokemon != null)
                    spritePokemon.enabled = false;
                if (shinyGO != null)
                    shinyGO.gameObject.SetActive(false);
            }

            var btnImage = botones[i].GetComponent<UnityEngine.UI.Image>();
            if (btnImage != null)
                btnImage.color = (i == indiceSeleccionado) ? Color.yellow : Color.white;
        }
    }

    public void SacarDelPC()
    {
        var pokemons = cajas[cajaActual];
        if (indiceSeleccionado >= 0 && indiceSeleccionado < pokemons.Count)
        {
            var equipo = interfazEquipo.Equipo;
            if (equipo.pokemones.Count < 6)
            {
                equipo.pokemones.Add(pokemons[indiceSeleccionado]);
                pokemons.RemoveAt(indiceSeleccionado);

                MostrarCaja(cajaActual);
                interfazEquipo.ActualizarInterfaz();
                ActualizarPanelEquipoSimple(botonesPanelEquipo, equipo, interfazEquipo.indiceSeleccionado);
                LimpiarSeleccion();
                OcultarInterfazEquipoEnPC();
            }
            else
            {
                Debug.Log("¡El equipo está lleno!");
            }
        }
    }

    public void DepositarEnPC()
    {
        var equipo = interfazEquipo.Equipo;
        int seleccionado = interfazEquipo.indiceSeleccionado;

        // Comprobar que hay más de 1 Pokémon en el equipo
        if (equipo.pokemones.Count > 1 && seleccionado >= 0 && seleccionado < equipo.pokemones.Count)
        {
            // Añadir al PC
            AddPokemon(equipo.pokemones[seleccionado]);
            // Quitar del equipo
            equipo.pokemones.RemoveAt(seleccionado);

            // Actualizar interfaces
            interfazEquipo.ActualizarInterfaz();
            ActualizarPanelEquipoSimple(botonesPanelEquipo, equipo, interfazEquipo.indiceSeleccionado);
            MostrarCaja(cajaActual);
            interfazEquipo.LimpiarSeleccion();
            OcultarInterfazEquipoEnPC();
        }
        else
        {
            Debug.Log("No puedes dejar el equipo vacío.");
        }
    }
}

[Serializable]
public class PCSaveData
{
    public List<List<PokemonSaveData>> cajas = new List<List<PokemonSaveData>>();
}
