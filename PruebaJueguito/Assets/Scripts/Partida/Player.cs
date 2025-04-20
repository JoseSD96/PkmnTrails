using System;
using System.Collections;
using UnityEngine;
sealed

public class Player : MonoBehaviour, ISavable
{
    public string playerName;
    public Trainer trainer;
    public Equipo equipo;
    public PC pc;

    public object CaptureState()
    {
        var saveData = new PlayerSaveData(playerName, trainer.Nombre, equipo.GetSaveData(), pc.GetSaveData());
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

    public PlayerSaveData(string playerName, string trainerName, EquipoSaveData equipo, PCSaveData pc)
    {
        this.playerName = playerName;
        this.trainerName = trainerName;
        this.equipo = equipo;
        this.pc = pc;
    }
}