using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Zona", menuName = "Zona/Crear Nuevo Zona")]
public class ZonaBase : ScriptableObject
{
    [SerializeField] Sprite spriteZona;
    [SerializeField] Type[] tiposZona;
    [SerializeField] PokemonBase[] pokemonLegendario;
    [SerializeField] tipoCombate fondoCombate;

    public Sprite SpriteZona => spriteZona;
    public Type[] TiposZona => tiposZona;
    public PokemonBase[] PokemonLegendario => pokemonLegendario;
    public tipoCombate FondoCombate => fondoCombate;

    public enum tipoCombate
    {
        bosque, cueva, hielo
    }
}