using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerManager
{
    static Dictionary<string, Trainer> skins;

    public static void Init()
    {
        skins = new Dictionary<string, Trainer>();
        var bases = Resources.LoadAll<Trainer>("Trainers");
        foreach (var trainer in bases)
        {
            skins[trainer.Nombre] = trainer;
        }
    }

    public static Trainer GetTrainer(string nombre)
    {
        if (skins.ContainsKey(nombre))
        {
            return skins[nombre];
        }
        else
        {
            Debug.LogError("Trainer no encontrado: " + nombre);
            return null;
        }
    }
}