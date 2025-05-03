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

    /// <summary>
    /// Inicializa la posición del panel del equipo al despertar el objeto.
    /// </summary>
    void Awake()
    {
        panelEquipo.anchoredPosition = new Vector2(0, 259);
    }

    /// <summary>
    /// Inicializa las cajas del PC, los botones de la caja actual y los botones del panel de equipo.
    /// Asigna los listeners para la selección de Pokémon en el equipo.
    /// </summary>
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
        botonesPanelEquipo = new GameObject[6];
        for (int i = 0; i < 6; i++)
        {
            botonesPanelEquipo[i] = panelEquipo.GetChild(i).gameObject;

            var btn = botonesPanelEquipo[i].GetComponent<Button>();
            int index = i;
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    interfazEquipo.SeleccionarPokemon(index);
                    ActualizarPanelEquipoSimple(botonesPanelEquipo, interfazEquipo.Equipo, interfazEquipo.indiceSeleccionado);
                });
            }
        }
    }

    /// <summary>
    /// Devuelve el índice de la primera caja con espacio disponible (menos de 30 Pokémon).
    /// </summary>
    /// <returns>Índice de la caja con hueco o -1 si todas están llenas.</returns>
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

    /// <summary>
    /// Añade un Pokémon a la primera caja con espacio disponible.
    /// </summary>
    /// <param name="pokemon">Pokémon a añadir.</param>
    public void AddPokemon(Pokemon pokemon)
    {
        cajas[GetCajaConHueco()].Add(pokemon);
    }

    /// <summary>
    /// Muestra los Pokémon de la caja seleccionada en la interfaz, actualizando sprites y datos.
    /// </summary>
    /// <param name="numCaja">Índice de la caja a mostrar.</param>
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

    /// <summary>
    /// Cambia la caja actual en la dirección indicada y actualiza la interfaz.
    /// </summary>
    /// <param name="direccion">-1 para anterior, 1 para siguiente.</param>
    public void CambiarCaja(int direccion)
    {
        cajaActual += direccion;
        if (cajaActual < 0) cajaActual = 17;
        if (cajaActual > 17) cajaActual = 0;
        MostrarCaja(cajaActual);
    }

    /// <summary>
    /// Selecciona un Pokémon de la caja actual y muestra sus datos en la interfaz.
    /// </summary>
    /// <param name="indice">Índice del Pokémon seleccionado.</param>
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

    /// <summary>
    /// Limpia la selección de Pokémon en la caja y oculta los paneles de datos y opciones.
    /// </summary>
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

    /// <summary>
    /// Elimina el Pokémon seleccionado de la caja actual y actualiza la interfaz.
    /// </summary>
    /// <param name="index">Índice del Pokémon a eliminar.</param>
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

    /// <summary>
    /// Intercambia el Pokémon seleccionado del equipo con el seleccionado del PC.
    /// Actualiza ambas interfaces y oculta el panel del equipo.
    /// </summary>
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

    /// <summary>
    /// Inicia el proceso para intercambiar un Pokémon del equipo con uno del PC, mostrando el panel de equipo.
    /// </summary>
    public void IniciarIntercambioConEquipo()
    {
        float yAbajo = 0f;
        if (Mathf.Abs(panelEquipo.anchoredPosition.y - yAbajo) > 1f)
        {
            esperandoIntercambioConEquipo = true;
            TogglePanelEquipo();
        }
        else
        {
            esperandoIntercambioConEquipo = true;
        }
    }

    /// <summary>
    /// Devuelve los datos serializables del PC para guardado, incluyendo todas las cajas y sus Pokémon.
    /// </summary>
    /// <returns>Instancia de PCSaveData con los datos actuales del PC.</returns>
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

    /// <summary>
    /// Restaura el estado del PC a partir de los datos guardados.
    /// </summary>
    /// <param name="saveData">Datos serializados del PC.</param>
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

    /// <summary>
    /// Oculta el panel del equipo en la interfaz del PC mediante animación.
    /// </summary>
    public void OcultarInterfazEquipoEnPC()
    {
        panelEquipo.DOAnchorPos(new Vector2(0, 259), 0.4f);
    }

    /// <summary>
    /// Actualiza el panel del equipo en el PC mostrando solo sprites y shiny.
    /// Resalta el Pokémon seleccionado.
    /// </summary>
    /// <param name="botones">Array de botones del panel de equipo.</param>
    /// <param name="equipo">Referencia al equipo del jugador.</param>
    /// <param name="indiceSeleccionado">Índice del Pokémon seleccionado.</param>
    public void ActualizarPanelEquipoSimple(GameObject[] botones, Equipo equipo, int indiceSeleccionado)
    {
        for (int i = 0; i < botones.Length; i++)
        {
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

    /// <summary>
    /// Saca el Pokémon seleccionado del PC y lo añade al equipo si hay espacio.
    /// </summary>
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

    /// <summary>
    /// Deposita el Pokémon seleccionado del equipo en el PC, siempre que el equipo no quede vacío.
    /// </summary>
    public void DepositarEnPC()
    {
        var equipo = interfazEquipo.Equipo;
        int seleccionado = interfazEquipo.indiceSeleccionado;

        if (equipo.pokemones.Count > 1 && seleccionado >= 0 && seleccionado < equipo.pokemones.Count)
        {
            AddPokemon(equipo.pokemones[seleccionado]);
            equipo.pokemones.RemoveAt(seleccionado);

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

    /// <summary>
    /// Alterna la visibilidad del panel del equipo en el PC mediante animación.
    /// </summary>
    public void TogglePanelEquipo()
    {
        float yActual = panelEquipo.anchoredPosition.y;
        float yArriba = 259f;
        float yAbajo = -259f;
        float duracion = 0.4f;

        ActualizarPanelEquipoSimple(botonesPanelEquipo, interfazEquipo.Equipo, interfazEquipo.indiceSeleccionado);

        if (Mathf.Abs(yActual - yArriba) < 1f)
        {
            panelEquipo.DOAnchorPos(new Vector2(0, yAbajo), duracion);
            btnOpciones.SetActive(true);
        }
        else
        {
            panelEquipo.DOAnchorPos(new Vector2(0, yArriba), duracion);
            btnOpciones.SetActive(false);
            LimpiarSeleccion();
            esperandoIntercambioConEquipo = false;
        }
    }
}

[Serializable]
public class PCSaveData
{
    /// <summary>
    /// Lista de cajas, cada una con su lista de datos serializables de Pokémon.
    /// </summary>
    public List<List<PokemonSaveData>> cajas = new List<List<PokemonSaveData>>();
}
