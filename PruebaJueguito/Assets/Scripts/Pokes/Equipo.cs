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

    public void IntercambiarPokemones(int indexA, int indexB)
    {
        if (indexA >= 0 && indexA < pokemones.Count && indexB >= 0 && indexB < pokemones.Count && indexA != indexB)
        {
            var temp = pokemones[indexA];
            pokemones[indexA] = pokemones[indexB];
            pokemones[indexB] = temp;
        }
    }

    public void IntercambiarConPC(int indexEquipo, PC pc, int indexPC, int cajaPC)
    {
        if (indexEquipo >= 0 && indexEquipo < pokemones.Count && pc != null && pc.cajas.ContainsKey(cajaPC))
        {
            var caja = pc.cajas[cajaPC];
            if (indexPC >= 0 && indexPC < caja.Count)
            {
                var temp = pokemones[indexEquipo];
                pokemones[indexEquipo] = caja[indexPC];
                caja[indexPC] = temp;
            }
        }
    }
}

[Serializable]
public class EquipoSaveData
{
    public List<PokemonSaveData> pokemones;
}
