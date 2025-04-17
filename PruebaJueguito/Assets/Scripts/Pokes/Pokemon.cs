using UnityEngine;

[System.Serializable]
public class Pokemon
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int nivel;

    public PokemonBase Base { get => _base; set => _base = value; }
    public int Nivel { get => nivel; set => nivel = value; }

    public int Potencial { get; set; }
    public int HP {get; set;}
    public bool isShiny {get;set;}

    public void Init(int potencial, bool isShiny)
    {
        Potencial = potencial;
        this.isShiny = isShiny;
        HP = MaxHP;
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

}
