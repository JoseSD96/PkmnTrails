using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerManager
{
    static Dictionary<string, Trainer> skins;

    /// <summary>
    /// Inicializa el diccionario de entrenadores cargando todos los Trainer desde los recursos.
    /// Asocia cada entrenador a su nombre en el diccionario.
    /// </summary>
    public static void Init()
    {
        skins = new Dictionary<string, Trainer>();
        var bases = Resources.LoadAll<Trainer>("Trainers");
        foreach (var trainer in bases)
        {
            skins[trainer.Nombre] = trainer;
        }
    }

    /// <summary>
    /// Devuelve la instancia de Trainer correspondiente al nombre recibido.
    /// Si no existe, muestra un error en consola y retorna null.
    /// </summary>
    /// <param name="nombre">Nombre del entrenador a buscar.</param>
    /// <returns>Instancia de Trainer o null si no se encuentra.</returns>
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