using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonBaseManager
{
    static Dictionary<int, PokemonBase> pokemones;

    public static void Init()
    {
        pokemones = new Dictionary<int, PokemonBase>();
        var pkmns = Resources.LoadAll<PokemonBase>("Pokemon");
        var pokemonesLegendarios = Resources.LoadAll<PokemonBase>("Pokemon/Legendarios");
        foreach (var pkmn in pkmns)
        {
            pokemones[pkmn.Num] = pkmn;
        }
        foreach (var pkmn in pokemonesLegendarios)
        {
            pokemones[pkmn.Num] = pkmn;
        }
    }

    public static PokemonBase GetPokemon(int num)
    {
        if (pokemones.ContainsKey(num))
        {
            Debug.Log("Pokemon encontrado: " + num);
            return pokemones[num];
        }
        else
        {
            Debug.LogError("Pokemon no encontrado: " + num);
            return null;
        }
    }
}