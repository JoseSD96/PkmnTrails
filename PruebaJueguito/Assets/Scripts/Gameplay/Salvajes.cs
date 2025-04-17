using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Salvajes : MonoBehaviour
{
    private List<PokemonBase> todosLosPokemon;

    [Header("Pruebas desde Inspector")]
    public List<Type> tiposZona;

    private void Awake()
    {
        todosLosPokemon = Resources.LoadAll<PokemonBase>("Pokemon").ToList();
    }

    public Pokemon GenerarPokemonSalvaje(int nivelMedioEquipo)
    {
        var posibles = todosLosPokemon.Where(p =>
            (tiposZona.Contains(p.Tipo1) || (p.Tipo2 != Type.None && tiposZona.Contains(p.Tipo2)))
            && p.MinLvl <= nivelMedioEquipo
        ).ToList();

        if (posibles.Count == 0)
            return null;

        var baseElegida = posibles[Random.Range(0, posibles.Count)];
        int nivelMin = baseElegida.MinLvl;
        int nivelMax = nivelMedioEquipo+2;
        int nivel = Random.Range(nivelMin, nivelMax + 1);
        int potencial = Random.Range(0, 32);
        bool isShiny = Random.Range(0, 1501) == 0;

        Pokemon pokemonSalvaje = new Pokemon();
        pokemonSalvaje.Base = baseElegida;
        pokemonSalvaje.Nivel = nivel;
        pokemonSalvaje.Init(potencial, isShiny);

        return pokemonSalvaje;
    }
}
