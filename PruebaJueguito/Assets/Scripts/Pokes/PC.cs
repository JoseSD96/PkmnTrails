using System;
using System.Collections.Generic;
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
}

[Serializable]
public class PCSaveData
{
    public List<List<PokemonSaveData>> cajas = new List<List<PokemonSaveData>>();
}
