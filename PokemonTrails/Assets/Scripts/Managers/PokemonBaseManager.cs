using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonBaseManager
{
    static Dictionary<int, PokemonBase> pokemones;

    /// <summary>
    /// Inicializa el diccionario de Pokémon base cargando todos los Pokémon normales y legendarios desde los recursos.
    /// Asocia cada Pokémon a su número en el diccionario.
    /// </summary>
    public static void Init()
    {
        pokemones = new Dictionary<int, PokemonBase>();
        var pkmns = Resources.LoadAll<PokemonBase>("Pokemon/NoLegendarios");
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

    /// <summary>
    /// Devuelve la instancia de PokemonBase correspondiente al número recibido.
    /// Si no existe, muestra un error en consola y retorna null.
    /// </summary>
    /// <param name="num">Número del Pokémon a buscar.</param>
    /// <returns>Instancia de PokemonBase o null si no se encuentra.</returns>
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