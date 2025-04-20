using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Salvajes : MonoBehaviour
{
    private List<PokemonBase> todosLosPokemon;

    ZonaBase zonaBase;
    Type[] tiposZona;

    private void Awake()
    {
        todosLosPokemon = Resources.LoadAll<PokemonBase>("Pokemon").ToList();
    }

    public void Inicializar()
    {
        zonaBase = ZonaManager.Instance.ZonaActual;
        tiposZona = zonaBase.TiposZona;
    }

    public Pokemon GenerarPokemonSalvaje(int nivelMedioEquipo)
    {
        Pokemon pokemonSalvaje = new Pokemon();
        if (zonaBase.PokemonLegendario.Length > 0 && Random.Range(0, 100) < 10)
        {
            PokemonBase legendario = zonaBase.PokemonLegendario[0];

            pokemonSalvaje.Base = legendario;
            pokemonSalvaje.Nivel = legendario.MinLvl;
            pokemonSalvaje.Init(Random.Range(20, 32), Random.Range(0, 1501) < 5);
            return pokemonSalvaje;
        }
        else
        {
            var posibles = todosLosPokemon.Where(p =>
                (tiposZona.Contains(p.Tipo1) || (p.Tipo2 != Type.None && tiposZona.Contains(p.Tipo2)))
                && p.MinLvl <= nivelMedioEquipo
            ).ToList();

            var baseElegida = posibles[Random.Range(0, posibles.Count)];
            int nivelMin = baseElegida.MinLvl;
            int nivelMax = nivelMedioEquipo + 2;
            int nivel = Random.Range(nivelMin, nivelMax + 1);

            pokemonSalvaje.Base = baseElegida;
            pokemonSalvaje.Nivel = nivel;
            pokemonSalvaje.Init(Random.Range(0, 32), Random.Range(0, 100) < 5);

            return pokemonSalvaje;
        }
    }
}
