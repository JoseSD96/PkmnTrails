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

    public void Init(int potencial, bool isShiny)
    {
        Potencial = potencial;
        this.isShiny = isShiny;
        HP = MaxHP;
    }

    public PokemonSaveData GetSaveData()
    {
        return new PokemonSaveData
        {
            numero = Base.Num,
            hp = HP,
            nivel = Nivel,
            experiencia = Experiencia,
            potencial = Potencial,
            isShiny = isShiny
        };
    }

    public Pokemon()
    {

    }

    public Pokemon(PokemonBase pkmn, int potencial, bool isShiny, int nivel)
    {
        _base = pkmn;
        Potencial = potencial;
        this.isShiny = isShiny;
        this.nivel = nivel;
        HP = MaxHP;
    }

    public Pokemon(PokemonSaveData data)
    {
        _base = PokemonBaseManager.GetPokemon(data.numero);
        HP = data.hp;
        nivel = data.nivel;
        Experiencia = data.experiencia;
        Potencial = data.potencial;
        isShiny = data.isShiny;
    }

    public int MaxHP
    {
        get { return ((2 * Base.Hp + Potencial) * Nivel / 100) + Nivel + 10; }
    }

    public int Ataque
    {
        get { return ((2 * Base.Atk + Potencial) * Nivel / 100) + 5; }
    }

    public int Defensa
    {
        get { return ((2 * Base.Def + Potencial) * Nivel / 100) + 5; }
    }

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

    public int ExperienciaParaSubirNivel()
    {
        return ExperienciaParaNivel(nivel + 1) - ExperienciaParaNivel(nivel);
    }

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

    public void ComprobarEvolucion()
    {
        if (Base.LvlEvo > 0)
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
}
