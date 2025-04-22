using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
sealed

public class Player : MonoBehaviour, ISavable
{
    public string playerName;
    public Trainer trainer;
    public Equipo equipo;
    public PC pc;

    private List<int> pokedex = new List<int>();
    public List<int> Pokedex => pokedex;

    void Update()
    {
        foreach (Pokemon pokemon in equipo.pokemones)
        {
            if (!pokedex.Contains(pokemon.Base.Num))
            {
                pokedex.Add(pokemon.Base.Num);
            }
        }
    }

    public object CaptureState()
    {
        var saveData = new PlayerSaveData(playerName, trainer.Nombre, equipo.GetSaveData(), pc.GetSaveData(), pokedex);
        return saveData;
    }

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
    public List<int> pokedex = new List<int>();

    public PlayerSaveData(string playerName, string trainerName, EquipoSaveData equipo, PCSaveData pc, List<int> pokedex)
    {
        this.playerName = playerName;
        this.trainerName = trainerName;
        this.equipo = equipo;
        this.pc = pc;
        this.pokedex = pokedex;
    }
}