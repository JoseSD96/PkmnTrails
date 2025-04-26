using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PokedexManager : MonoBehaviour
{
    public GameObject filaPrefab;
    public Transform contenedorPokedex;
    public GameObject celdaPrefab;

    private const int POKEMON_POR_FILA = 10;

    public Pokedex pokedexCapturados;

    private List<Image> imagenesPokemons = new List<Image>();

    void Start()
    {
        PokemonBase[] pokemons = Resources.LoadAll<PokemonBase>("Pokemon/NoLegendarios");
        PokemonBase[] pokemonLegendarios = Resources.LoadAll<PokemonBase>("Pokemon/Legendarios");

        List<PokemonBase> todosLosPokemons = new List<PokemonBase>();
        todosLosPokemons.AddRange(pokemons);
        todosLosPokemons.AddRange(pokemonLegendarios);
        todosLosPokemons.Sort((a, b) => a.Num.CompareTo(b.Num));

        int totalPokemons = todosLosPokemons.Count;
        int totalFilas = Mathf.CeilToInt((float)totalPokemons / POKEMON_POR_FILA);

        int indexPokemon = 0;

        for (int i = 0; i < totalFilas; i++)
        {
            GameObject nuevaFila = Instantiate(filaPrefab, contenedorPokedex);

            for (int j = 0; j < POKEMON_POR_FILA; j++)
            {
                if (indexPokemon >= totalPokemons)
                    break;

                GameObject nuevaCelda = Instantiate(celdaPrefab, nuevaFila.transform);

                Image imagen = nuevaCelda.GetComponentInChildren<Image>();
                imagen.sprite = todosLosPokemons[indexPokemon].SpriteIdle;

                imagenesPokemons.Add(imagen); // Guardamos la referencia

                if (!pokedexCapturados.pokemones.Contains(todosLosPokemons[indexPokemon].Num))
                {
                    Color color = imagen.color;
                    color.a = 0.3f;
                    imagen.color = color;
                }
                else
                {
                    Color color = imagen.color;
                    color.a = 1f;
                    imagen.color = color;
                }

                indexPokemon++;
            }
        }
    }

    public void ActualizarIluminacionPokemons()
    {
        PokemonBase[] pokemons = Resources.LoadAll<PokemonBase>("Pokemon/NoLegendarios");
        PokemonBase[] pokemonLegendarios = Resources.LoadAll<PokemonBase>("Pokemon/Legendarios");

        List<PokemonBase> todosLosPokemons = new List<PokemonBase>();
        todosLosPokemons.AddRange(pokemons);
        todosLosPokemons.AddRange(pokemonLegendarios);
        todosLosPokemons.Sort((a, b) => a.Num.CompareTo(b.Num));

        for (int i = 0; i < imagenesPokemons.Count; i++)
        {
            Image imagen = imagenesPokemons[i];
            if (!pokedexCapturados.pokemones.Contains(todosLosPokemons[i].Num))
            {
                Color color = imagen.color;
                color.a = 0.3f;
                imagen.color = color;
            }
            else
            {
                Color color = imagen.color;
                color.a = 1f;
                imagen.color = color;
            }
        }
    }
}
