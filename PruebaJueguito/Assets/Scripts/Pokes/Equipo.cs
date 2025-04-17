using System.Collections.Generic;
using UnityEngine;

public class Equipo : MonoBehaviour
{
    [SerializeField] private List<Pokemon> pokemones;
    public List<Pokemon> Pokemones => pokemones;

    private void Start()
    {
        foreach (var pokemon in pokemones)
        {
            pokemon.Init(10, true);
        }
    }

    public Pokemon GetPokemonVivo()
    {
        foreach (var pokemon in pokemones)
        {
            if (pokemon.HP > 0)
            {
                return pokemon;
            }
        }
        return null;
    }

    public int GetMediaNivel()
    {
        int lvlMedio = 0;
        int lvlTotal = 0;
        foreach (var pokemon in pokemones)
        {
            lvlTotal += pokemon.Nivel;
        }

        lvlMedio = lvlTotal / pokemones.Count;
        return lvlMedio;
    }
}
