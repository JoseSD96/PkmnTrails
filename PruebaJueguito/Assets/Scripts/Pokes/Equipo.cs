using System;
using System.Collections.Generic;
using UnityEngine;

public class Equipo : MonoBehaviour
{
    public List<Pokemon> pokemones;
    public PC pc;

    public EquipoSaveData GetSaveData()
    {
        var saveData = new EquipoSaveData();
        saveData.pokemones = new List<PokemonSaveData>();
        foreach (var pokemon in pokemones)
        {
            saveData.pokemones.Add(pokemon.GetSaveData());
        }
        return saveData;
    }

    public void RestoreState(EquipoSaveData saveData)
    {
        pokemones = new List<Pokemon>();
        foreach (var pokemonData in saveData.pokemones)
        {
            var pokemon = new Pokemon(pokemonData);
            pokemones.Add(pokemon);
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
        int lvlTotal = 0;
        foreach (var pokemon in pokemones)
        {
            lvlTotal += pokemon.Nivel;
        }

        int lvlMedio = lvlTotal / pokemones.Count;
        return lvlMedio;
    }

    public Pokemon GetPokemonByIndex(int index)
    {
        if (index >= 0 && index < pokemones.Count)
        {
            return pokemones[index];
        }
        return null;
    }

    public void AddPokemon(Pokemon pokemon)
    {
        if (pokemones.Count < 6)
        {
            pokemones.Add(pokemon);
        }
        else
        {
            pc.AddPokemon(pokemon);
        }
    }

    public void EliminarPokemon(int index)
    {
        if (index >= 0 && index < pokemones.Count)
        {
            pokemones.RemoveAt(index);
        }
    }
}

[Serializable]
public class EquipoSaveData
{
    public List<PokemonSaveData> pokemones;
}
