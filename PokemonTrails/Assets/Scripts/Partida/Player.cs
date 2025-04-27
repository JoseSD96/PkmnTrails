using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ISavable
{
    public string playerName;
    public Trainer trainer;
    public Equipo equipo;
    public PC pc;

    public Pokedex pokedex;

    /// <summary>
    /// Actualiza la Pokédex del jugador cada frame, añadiendo los Pokémon del equipo y dle PC que aún no estén registrados.
    /// </summary>
    void Update()
    {
        foreach (Pokemon pokemon in equipo.pokemones)
        {
            if (!pokedex.pokemones.Contains(pokemon.Base))
            {
                pokedex.pokemones.Add(pokemon.Base);
            }
        }
        for (int i = 0; i < pc.cajas.Count; i++)
        {
            foreach (Pokemon pokemon in pc.cajas[i])
            {
                if (!pokedex.pokemones.Contains(pokemon.Base))
                {
                    pokedex.pokemones.Add(pokemon.Base);
                }
            }
        }
    }

    /// <summary>
    /// Captura el estado actual del jugador para guardado, incluyendo nombre, entrenador, equipo, PC y Pokédex.
    /// </summary>
    /// <returns>Objeto serializable con los datos del jugador.</returns>
    public object CaptureState()
    {
        var saveData = new PlayerSaveData(playerName, trainer.Nombre, equipo.GetSaveData(), pc.GetSaveData(), pokedex.GetSaveData());
        return saveData;
    }

    /// <summary>
    /// Restaura el estado del jugador a partir de los datos guardados.
    /// </summary>
    /// <param name="state">Objeto con los datos serializados del jugador.</param>
    public void RestoreState(object state)
    {
        PlayerSaveData saveData = (PlayerSaveData)state;
        playerName = saveData.playerName;
        trainer = TrainerManager.GetTrainer(saveData.trainerName);
        equipo.RestoreState(saveData.equipo);
        pc.RestoreState(saveData.pc);
    }
}

[Serializable]
public class PlayerSaveData
{
    public string playerName;
    public string trainerName;
    public EquipoSaveData equipo;
    public PCSaveData pc;
    public PokedexSaveData pokedex;

    /// <summary>
    /// Constructor para almacenar todos los datos necesarios del jugador al guardar la partida.
    /// </summary>
    public PlayerSaveData(string playerName, string trainerName, EquipoSaveData equipo, PCSaveData pc, PokedexSaveData pokedex)
    {
        this.playerName = playerName;
        this.trainerName = trainerName;
        this.equipo = equipo;
        this.pc = pc;
        this.pokedex = pokedex;
    }
}