using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Salvajes : MonoBehaviour
{
    private List<PokemonBase> todosLosPokemon;

    ZonaBase zonaBase;
    Type[] tiposZona;

    /// <summary>
    /// Al despertar el objeto, carga todos los Pokémon base desde los recursos.
    /// </summary>
    private void Awake()
    {
        todosLosPokemon = Resources.LoadAll<PokemonBase>("Pokemon/NoLegendarios").ToList();
    }

    /// <summary>
    /// Inicializa la zona y los tipos de la zona actual usando el ZonaManager.
    /// </summary>
    public void Inicializar()
    {
        zonaBase = ZonaManager.Instance.ZonaActual;
        tiposZona = zonaBase.TiposZona;
    }

    /// <summary>
    /// Genera un Pokémon salvaje acorde a la zona y el nivel medio del equipo.
    /// Puede generar un legendario con cierta probabilidad o un Pokémon común de la zona.
    /// </summary>
    /// <param name="nivelMedioEquipo">Nivel medio del equipo del jugador.</param>
    /// <returns>Instancia de Pokémon salvaje generado.</returns>
    public Pokemon GenerarPokemonSalvaje(int nivelMedioEquipo)
    {
        Pokemon pokemonSalvaje = new Pokemon();
        if (zonaBase.PokemonLegendario.Length > 0 && Random.Range(0, 100) < 10)
        {
            PokemonBase legendario = zonaBase.PokemonLegendario[0];

            pokemonSalvaje.Base = legendario;
            pokemonSalvaje.Nivel = legendario.MinLvl;
            pokemonSalvaje.Init(Random.Range(20, 32), Random.Range(0, 100) < 5);
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
            pokemonSalvaje.Init(Random.Range(0, 32), Random.Range(0, 100) < 2);

            return pokemonSalvaje;
        }
    }
}
