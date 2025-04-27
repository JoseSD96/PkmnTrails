using System;
using System.Collections.Generic;
using UnityEngine;

public class Pokedex : MonoBehaviour
{
    public List<PokemonBase> pokemones;

    /// <summary>
    /// Devuelve los datos serializables de la pokedex para guardado.
    /// </summary>
    /// <returns>Instancia de PokedexSaveData con los datos actuales de la pokedex.</returns>
    public PokedexSaveData GetSaveData()
    {
        var saveData = new PokedexSaveData();
        saveData.pokemones = new List<PokemonSaveData>();
        foreach (var pkmn in pokemones)
        {
            saveData.pokemones.Add(new Pokemon(pkmn).GetSaveData());
        }
        return saveData;
    }

    /// <summary>
    /// Restaura el estado de la pokedex a partir de los datos guardados.
    /// </summary>
    /// <param name="saveData">Datos serializados de la pokedex.</param>
    public void RestoreState(PokedexSaveData saveData)
    {
        pokemones = new List<PokemonBase>();
        foreach (var pkmn in saveData.pokemones)
        {
            var pkmnBase = PokemonBaseManager.GetPokemon(pkmn.numero);
            pokemones.Add(pkmnBase);
        }
    }
}


[Serializable]
public class PokedexSaveData
{
    /// <summary>
    /// Lista de datos serializables de los numeros de los Pokémon capturados.
    /// </summary>
    public List<PokemonSaveData> pokemones;
}