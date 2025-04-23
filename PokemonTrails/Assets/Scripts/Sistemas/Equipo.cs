using System;
using System.Collections.Generic;
using UnityEngine;

public class Equipo : MonoBehaviour
{
    public List<Pokemon> pokemones;
    public PC pc;

    /// <summary>
    /// Devuelve los datos serializables del equipo para guardado, incluyendo todos los Pokémon.
    /// </summary>
    /// <returns>Instancia de EquipoSaveData con los datos actuales del equipo.</returns>
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

    /// <summary>
    /// Restaura el estado del equipo a partir de los datos guardados.
    /// </summary>
    /// <param name="saveData">Datos serializados del equipo.</param>
    public void RestoreState(EquipoSaveData saveData)
    {
        pokemones = new List<Pokemon>();
        foreach (var pokemonData in saveData.pokemones)
        {
            var pokemon = new Pokemon(pokemonData);
            pokemones.Add(pokemon);
        }
    }

    /// <summary>
    /// Devuelve el primer Pokémon del equipo que esté vivo (HP > 0).
    /// </summary>
    /// <returns>Pokémon vivo o null si ninguno está vivo.</returns>
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

    /// <summary>
    /// Calcula el nivel medio del equipo sumando los niveles de todos los Pokémon y dividiendo entre el total.
    /// </summary>
    /// <returns>Nivel medio del equipo.</returns>
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

    /// <summary>
    /// Devuelve el Pokémon en la posición indicada del equipo.
    /// </summary>
    /// <param name="index">Índice del Pokémon.</param>
    /// <returns>Pokémon correspondiente o null si el índice no es válido.</returns>
    public Pokemon GetPokemonByIndex(int index)
    {
        if (index >= 0 && index < pokemones.Count)
        {
            return pokemones[index];
        }
        return null;
    }

    /// <summary>
    /// Añade un Pokémon al equipo si hay espacio, o lo envía al PC si el equipo está lleno.
    /// </summary>
    /// <param name="pokemon">Pokémon a añadir.</param>
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

    /// <summary>
    /// Elimina el Pokémon en la posición indicada del equipo, siempre que quede al menos uno.
    /// </summary>
    /// <param name="index">Índice del Pokémon a eliminar.</param>
    public void EliminarPokemon(int index)
    {
        if (index >= 0 && index < pokemones.Count)
        {
            if (pokemones.Count > 1)
            {
                pokemones.RemoveAt(index);
            }
        }
    }

    /// <summary>
    /// Intercambia la posición de dos Pokémon dentro del equipo.
    /// </summary>
    /// <param name="indexA">Índice del primer Pokémon.</param>
    /// <param name="indexB">Índice del segundo Pokémon.</param>
    public void IntercambiarPokemones(int indexA, int indexB)
    {
        if (indexA >= 0 && indexA < pokemones.Count && indexB >= 0 && indexB < pokemones.Count && indexA != indexB)
        {
            var temp = pokemones[indexA];
            pokemones[indexA] = pokemones[indexB];
            pokemones[indexB] = temp;
        }
    }

    /// <summary>
    /// Intercambia un Pokémon del equipo con uno del PC, en la caja y posición indicadas.
    /// </summary>
    /// <param name="indexEquipo">Índice del Pokémon en el equipo.</param>
    /// <param name="pc">Referencia al PC.</param>
    /// <param name="indexPC">Índice del Pokémon en la caja del PC.</param>
    /// <param name="cajaPC">Índice de la caja del PC.</param>
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
    /// <summary>
    /// Lista de datos serializables de los Pokémon del equipo.
    /// </summary>
    public List<PokemonSaveData> pokemones;
}
