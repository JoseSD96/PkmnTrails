using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Crear Nuevo Pokemon")]
public class PokemonBase : ScriptableObject
{
    [SerializeField] int num;

    [SerializeField] string nombre;

    [SerializeField] Type tipo1;
    [SerializeField] Type tipo2;

    [SerializeField] Sprite idle;
    [SerializeField] Sprite idleShiny;

    [SerializeField] Sprite explorar;
    [SerializeField] Sprite explorarShiny;

    [SerializeField] Sprite[] andar = new Sprite[4];
    [SerializeField] Sprite[] andarShiny = new Sprite[4];

    [SerializeField] int hp;
    [SerializeField] int atk;
    [SerializeField] int def;
    [SerializeField] int lvlEvo;
    [SerializeField] PokemonBase[] evolucion = new PokemonBase[1];
    [SerializeField] int minLvl;
    [SerializeField] int ratioCaptura;
    [SerializeField] TipoExp tipoExp;

    public Type Tipo1 => tipo1;
    public Type Tipo2 => tipo2;

    public TipoExp TipoExp => tipoExp;

    public PokemonBase[] Evolucion => evolucion;

    public string Nombre => nombre;

    public int Num => num;
    public int Def => def;
    public int Atk => atk;
    public int Hp => hp;
    public int LvlEvo => lvlEvo;
    public int MinLvl => minLvl;
    public int RatioCaptura => ratioCaptura;


    public Sprite SpriteTipo1;
    public Sprite SpriteTipo2;

    public Sprite SpriteIdle => idle;
    public Sprite SpriteIdleShiny => idleShiny;
    public Sprite SpriteExploracion => explorar;
    public Sprite SpriteExploracionShiny => explorarShiny;
    public Sprite[] SpritesAndar => andar;
    public Sprite[] SpritesAndarShiny => andarShiny;
}

public enum Type
{
    None, Acero, Agua, Bicho, Dragon, Electrico, Fantasma, Fuego, Hada,
    Hielo, Lucha, Normal, Planta, Psiquico, Roca, Siniestro, Tierra, Veneno, Volador
}

public enum TipoExp
{
    Parabolico, Lento, Medio, Rapido
}