using System;
using UnityEngine;

[System.Serializable]
public class Pokemon
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int nivel;

    public PokemonBase Base { get => _base; set => _base = value; }
    public int Nivel { get => nivel; set => nivel = value; }
    public int Experiencia { get; set; }
    public int Potencial { get; set; }
    public int HP { get; set; }
    public bool isShiny { get; set; }

    /// <summary>
    /// Inicializa el Pokémon con un potencial y si es shiny, y ajusta los HP al máximo.
    /// </summary>
    /// <param name="potencial">Valor de potencial individual.</param>
    /// <param name="isShiny">Indica si el Pokémon es shiny.</param>
    public void Init(int potencial, bool isShiny)
    {
        Potencial = potencial;
        this.isShiny = isShiny;
        HP = MaxHP;
    }

    /// <summary>
    /// Devuelve los datos serializables del Pokémon para guardado.
    /// </summary>
    /// <returns>Instancia de PokemonSaveData con los datos actuales.</returns>
    public PokemonSaveData GetSaveData()
    {
        return new PokemonSaveData
        {
            numero = Base.Num,
            nombrePokemon = Base.Nombre,
            hp = HP,
            nivel = Nivel,
            experiencia = Experiencia,
            potencial = Potencial,
            isShiny = isShiny
        };
    }

    /// <summary>
    /// Constructor vacío necesario para serialización.
    /// </summary>
    public Pokemon()
    {

    }

    /// <summary>
    /// Constructor para crear un Pokémon desde sus datos base, potencial, shiny y nivel.
    /// </summary>
    public Pokemon(PokemonBase pkmn, int potencial, bool isShiny, int nivel)
    {
        _base = pkmn;
        Potencial = potencial;
        this.isShiny = isShiny;
        this.nivel = nivel;
        HP = MaxHP;
    }

    public Pokemon(PokemonBase pkmn)
    {
        _base = pkmn;
    }

    /// <summary>
    /// Constructor para crear un Pokémon a partir de datos guardados.
    /// </summary>
    /// <param name="data">Datos serializados del Pokémon.</param>
    public Pokemon(PokemonSaveData data)
    {
        _base = PokemonBaseManager.GetPokemon(data.numero, data.nombrePokemon);
        HP = data.hp;
        nivel = data.nivel;
        Experiencia = data.experiencia;
        Potencial = data.potencial;
        isShiny = data.isShiny;
    }

    /// <summary>
    /// Calcula el HP máximo del Pokémon según su base, potencial y nivel.
    /// </summary>
    public int MaxHP
    {
        get { return ((2 * Base.Hp + Potencial) * Nivel / 100) + Nivel + 10; }
    }

    /// <summary>
    /// Calcula el ataque del Pokémon según su base, potencial y nivel.
    /// </summary>
    public int Ataque
    {
        get { return ((2 * Base.Atk + Potencial) * Nivel / 100) + 5; }
    }

    /// <summary>
    /// Calcula la defensa del Pokémon según su base, potencial y nivel.
    /// </summary>
    public int Defensa
    {
        get { return ((2 * Base.Def + Potencial) * Nivel / 100) + 5; }
    }

    /// <summary>
    /// Calcula la experiencia total necesaria para alcanzar un nivel objetivo.
    /// </summary>
    /// <param name="nivelObjetivo">Nivel objetivo.</param>
    /// <returns>Experiencia total necesaria.</returns>
    public int ExperienciaParaNivel(int nivelObjetivo)
    {
        switch (Base.TipoExp)
        {
            case TipoExp.Parabolico:
                return (int)(1.2f * Mathf.Pow(nivelObjetivo, 3) - 15 * Mathf.Pow(nivelObjetivo, 2) + 100 * nivelObjetivo - 140);
            case TipoExp.Lento:
                return (int)(1.25f * Mathf.Pow(nivelObjetivo, 3));
            case TipoExp.Medio:
                return (int)Mathf.Pow(nivelObjetivo, 3);
            case TipoExp.Rapido:
                return (int)(0.8f * Mathf.Pow(nivelObjetivo, 3));
            default:
                return (int)Mathf.Pow(nivelObjetivo, 3);
        }
    }

    /// <summary>
    /// Calcula la experiencia necesaria para subir al siguiente nivel.
    /// </summary>
    /// <returns>Experiencia necesaria para el siguiente nivel.</returns>
    public int ExperienciaParaSubirNivel()
    {
        return ExperienciaParaNivel(nivel + 1) - ExperienciaParaNivel(nivel);
    }

    /// <summary>
    /// Comprueba si el Pokémon puede subir de nivel y realiza el proceso si corresponde.
    /// También ajusta los HP y comprueba si debe evolucionar.
    /// </summary>
    public void ComprobarSubirNivel()
    {
        while (Experiencia >= ExperienciaParaSubirNivel())
        {
            if (nivel < 100)
            {
                int maxHPAntes = MaxHP;
                nivel++;
                Experiencia -= ExperienciaParaSubirNivel();
                HP += MaxHP - maxHPAntes;
                ComprobarEvolucion();
            }
        }
    }

    /// <summary>
    /// Comprueba si el Pokémon puede evolucionar y realiza la evolución si cumple los requisitos.
    /// </summary>
    public void ComprobarEvolucion()
    {
        if (Base.Evolucion.Length > 0)
        {
            if (nivel >= Base.LvlEvo)
            {
                if (Base.Evolucion.Length == 1)
                {
                    int maxHPAntes = MaxHP;
                    this._base = Base.Evolucion[0];
                    HP += MaxHP - maxHPAntes;
                }
                else
                {
                    int maxHPAntes = MaxHP;
                    this._base = Base.Evolucion[UnityEngine.Random.Range(0, Base.Evolucion.Length)];
                    HP += MaxHP - maxHPAntes;
                }
            }
        }
    }
}

[Serializable]
public class PokemonSaveData
{
    public int numero;
    public int hp;
    public int nivel;
    public int experiencia;
    public int potencial;
    public bool isShiny;
    public string nombrePokemon;
    // Esta clase es solo para almacenar los datos serializables del Pokémon.
}
