using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro; // Asegúrate de tener esto arriba

public class SistemaCombate : MonoBehaviour
{
    [SerializeField] public PkmnCombate pkmnJugador;
    [SerializeField] CombateHUD pkmnJugadorHUD;
    [SerializeField] GameObject fondoCombate;
    [SerializeField] public PkmnCombate pkmnEnemigo;
    [SerializeField] CombateHUD pkmnEnemigoHUD;
    [SerializeField] Button Atk1;
    [SerializeField] Button Atk2;
    [SerializeField] Button huirBtn;
    [SerializeField] Button capturarBtn;
    [SerializeField] AudioManager audioManager;
    [SerializeField] GameObject velocidadJugador;
    [SerializeField] GameObject velocidadEnemigo;
    private ControladorPartida partida;
    private SistemaCaptura sistemaCaptura;

    private Type AtkType;
    bool isAttaking;
    bool isFinish;

    Pokemon salvaje;
    Equipo equipo;
    public void IniciarCombate(ControladorPartida controller, Equipo equipo, Pokemon salvaje, SistemaCaptura sistemaCaptura, ZonaBase zonaBase)
    {
        partida = controller;
        this.equipo = equipo;
        this.salvaje = salvaje;

        this.sistemaCaptura = sistemaCaptura;

        isFinish = false;

        Atk1.onClick.RemoveAllListeners();
        Atk2.onClick.RemoveAllListeners();
        capturarBtn.onClick.RemoveAllListeners();

        Atk1.onClick.AddListener(GetAtkType);
        Atk2.onClick.AddListener(GetAtkType);
        capturarBtn.onClick.AddListener(CapturarPokemon);

        SetupBattle(zonaBase.FondoCombate);
    }

    public void SetupBattle(ZonaBase.tipoCombate tipoCombate)
    {
        pkmnJugador.Setup(equipo.GetPokemonVivo());
        pkmnEnemigo.Setup(salvaje);
        pkmnJugadorHUD.SetData(pkmnJugador);
        pkmnEnemigoHUD.SetData(pkmnEnemigo);

        fondoCombate.GetComponent<Image>().sprite = Resources.Load<Sprite>("Combate/FondosCombate/" + tipoCombate.ToString());
    }


    public void CapturarPokemon()
    {
        Atk1.interactable = false;
        Atk2.interactable = false;
        capturarBtn.interactable = false;
        huirBtn.interactable = false;
        StartCoroutine(CapturaCoroutine());
    }

    private IEnumerator CapturaCoroutine()
    {
        sistemaCaptura.LanzarPokeball();
        while (sistemaCaptura.IsCapturando)
            yield return null;
        Atk1.interactable = true;
        Atk2.interactable = true;
        capturarBtn.interactable = true;
        huirBtn.interactable = true;

        if (sistemaCaptura.ExitoCaptura())
        {
            StartCoroutine(TerminarCombateCapturado());
        }
        else
        {
            StartCoroutine(TurnoEnemigo());
        }
    }

    public void GetAtkType()
    {
        if (!isAttaking && !isFinish)
        {
            isAttaking = true;

            GameObject botonPresionado = EventSystem.current.currentSelectedGameObject;
            if (botonPresionado == null)
            {
                Debug.LogError("No se ha detectado ningún botón presionado.");
                isAttaking = false;
                return;
            }
            Button btn = botonPresionado.GetComponent<Button>();
            if (btn == null || btn.image == null || btn.image.sprite == null)
            {
                Debug.LogError("El botón no tiene imagen o sprite asignado.");
                isAttaking = false;
                return;
            }
            string tipo = btn.image.sprite.name;

            Atk1.interactable = false;
            Atk2.interactable = false;
            capturarBtn.interactable = false;
            huirBtn.interactable = false;

            AtkType = StringToType(tipo.Split("_")[0]);
            int velEnemigo = Random.Range(1, 51);
            int velJugador = Random.Range(1, 51);

            StartCoroutine(MostrarVelocidadesYAtacar(velJugador, velEnemigo));
        }
    }

    private IEnumerator MostrarVelocidadesYAtacar(int velJugador, int velEnemigo)
    {
        yield return StartCoroutine(MostrarVelocidades(velJugador, velEnemigo));
        if (velJugador > velEnemigo)
        {
            yield return StartCoroutine(TurnoCombate(true));
        }
        else
        {
            yield return StartCoroutine(TurnoCombate(false));
        }
    }

    private IEnumerator MostrarVelocidades(int velJugador, int velEnemigo)
    {
        velocidadJugador.SetActive(true);
        velocidadEnemigo.SetActive(true);

        velocidadJugador.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = velJugador.ToString();
        velocidadEnemigo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = velEnemigo.ToString();

        yield return new WaitForSeconds(1f);

        velocidadJugador.SetActive(false);
        velocidadEnemigo.SetActive(false);
    }

    private IEnumerator TurnoCombate(bool jugadorPrimero)
    {
        Atk1.interactable = false;
        Atk2.interactable = false;
        huirBtn.interactable = false;
        capturarBtn.interactable = false;

        if (jugadorPrimero)
        {
            yield return StartCoroutine(TurnoJugador());
            if (!isFinish)
                yield return StartCoroutine(TurnoEnemigo());
        }
        else
        {
            yield return StartCoroutine(TurnoEnemigo());
            if (!isFinish)
                yield return StartCoroutine(TurnoJugador());
        }
        isAttaking = false;
        Atk1.interactable = true;
        Atk2.interactable = true;
        huirBtn.interactable = true;
        capturarBtn.interactable = true;
    }

    IEnumerator TerminarCombate()
    {
        yield return new WaitForSeconds(0.5f);
        Atk1.interactable = false;
        Atk2.interactable = false;
        capturarBtn.interactable = false;
        huirBtn.interactable = false;
        partida.TerminarCombate();
    }

    IEnumerator TerminarCombateCapturado()
    {
        foreach (var pkmn in equipo.pokemones)
        {
            pkmn.Experiencia += pkmnEnemigo.Pkmn.ExperienciaParaNivel(pkmnEnemigo.Pkmn.Nivel) / 5;
            pkmn.ComprobarSubirNivel();
        }
        yield return new WaitForSeconds(0.8f);
        sistemaCaptura.resetPokeball();
        Atk1.interactable = false;
        Atk2.interactable = false;
        capturarBtn.interactable = false;
        huirBtn.interactable = false;
        partida.TerminarCombate(pkmnEnemigo.Pkmn);
    }

    public static Type StringToType(string tipo)
    {
        return System.Enum.TryParse(tipo, true, out Type result) ? result : Type.None;
    }

    IEnumerator TurnoJugador()
    {
        pkmnJugador.AnimacionAtaque();
        yield return new WaitForSeconds(1f);

        pkmnEnemigo.AnimacionGolpe();
        bool isFainted = DoDmg(AtkType, pkmnJugador.Pkmn, pkmnEnemigo.Pkmn);

        yield return pkmnEnemigoHUD.UpdateHP();
        yield return new WaitForSeconds(1f);

        if (isFainted)
        {
            foreach (var pkmn in equipo.pokemones)
            {
                pkmn.Experiencia += pkmnEnemigo.Pkmn.ExperienciaParaNivel(pkmnEnemigo.Pkmn.Nivel) / 5;
                pkmn.ComprobarSubirNivel();
            }
            pkmnEnemigo.AnimacionDerrota();
            pkmnEnemigoHUD.AnimacionDerrota();
            pkmnEnemigoHUD.enabled = false;
            isAttaking = false;
            isFinish = true;
            StartCoroutine(TerminarCombate());
        }

    }

    IEnumerator TurnoEnemigo()
    {
        pkmnEnemigo.AnimacionAtaque();
        yield return new WaitForSeconds(1f);

        pkmnJugador.AnimacionGolpe();
        bool isFainted = DoDmg(EnemyAtk(), pkmnEnemigo.Pkmn, pkmnJugador.Pkmn);
        yield return pkmnJugadorHUD.UpdateHP();
        if (isFainted)
        {
            pkmnJugador.AnimacionDerrota();
            pkmnJugadorHUD.AnimacionDerrota();
            pkmnJugadorHUD.enabled = false;
            isFinish = true;
            StartCoroutine(TerminarCombate());
        }

        isAttaking = false;

        yield return new WaitForSeconds(1f);
    }

    public bool DoDmg(Type tipo, Pokemon atacante, Pokemon defensor)
    {
        float modificadores = Random.Range(0.85f, 1f);
        float efectividad = CalcularEfectividad(tipo, defensor.Base.Tipo1, defensor.Base.Tipo2);
        if (efectividad <= 0.5f)
        {
            audioManager.PlayEfecto("Combate", "pocoEfica");
        }
        else if (efectividad == 1f)
        {
            audioManager.PlayEfecto("Combate", "Neutro");
        }
        else if (efectividad >= 2f)
        {
            audioManager.PlayEfecto("Combate", "superEficazHit");
        }
        float dmg = ((2 * atacante.Nivel / 5 + 2) * 60 * atacante.Ataque / defensor.Defensa / 50 + 2) * modificadores * efectividad;
        int damage = Mathf.FloorToInt(dmg);
        if (damage <= 0)
        {
            damage = 1;
        }
        defensor.HP -= damage;
        if (defensor.HP <= 0)
        {
            defensor.HP = 0;
            return true;
        }

        return false;
    }

    public Type EnemyAtk()
    {
        int atk = Random.Range(1, 3);
        if (atk == 1)
        {
            return StringToType(pkmnEnemigo.Pkmn.Base.Tipo1.ToString());
        }
        else
        {
            string tipo = pkmnEnemigo.Pkmn.Base.Tipo2.ToString();
            if (tipo == "None")
            {
                tipo = "Normal";
            }
            return StringToType(tipo);
        }
    }

    private static readonly Dictionary<Type, Dictionary<Type, float>> tablaTipos = new()
    {
        { Type.Normal, new() { { Type.Roca, 0.5f }, { Type.Fantasma, 0f }, { Type.Acero, 0.5f } } },
        { Type.Fuego, new() {
            { Type.Planta, 2f }, { Type.Hielo, 2f }, { Type.Bicho, 2f }, { Type.Acero, 2f },
            { Type.Fuego, 0.5f }, { Type.Agua, 0.5f }, { Type.Roca, 0.5f }, { Type.Dragon, 0.5f }
        }},
        { Type.Agua, new() {
            { Type.Fuego, 2f }, { Type.Roca, 2f }, { Type.Tierra, 2f },
            { Type.Agua, 0.5f }, { Type.Planta, 0.5f }, { Type.Dragon, 0.5f }
        }},
        { Type.Planta, new() {
            { Type.Agua, 2f }, { Type.Roca, 2f }, { Type.Tierra, 2f },
            { Type.Fuego, 0.5f }, { Type.Planta, 0.5f }, { Type.Veneno, 0.5f }, { Type.Volador, 0.5f },
            { Type.Bicho, 0.5f }, { Type.Dragon, 0.5f }, { Type.Acero, 0.5f }
        }},
        { Type.Electrico, new() {
            { Type.Agua, 2f }, { Type.Volador, 2f },
            { Type.Electrico, 0.5f }, { Type.Planta, 0.5f }, { Type.Dragon, 0.5f },
            { Type.Tierra, 0f }
        }},
        { Type.Hielo, new() {
            { Type.Planta, 2f }, { Type.Tierra, 2f }, { Type.Volador, 2f }, { Type.Dragon, 2f },
            { Type.Fuego, 0.5f }, { Type.Agua, 0.5f }, { Type.Hielo, 0.5f }, { Type.Acero, 0.5f }
        }},
        { Type.Lucha, new() {
            { Type.Normal, 2f }, { Type.Hielo, 2f }, { Type.Roca, 2f }, { Type.Siniestro, 2f }, { Type.Acero, 2f },
            { Type.Veneno, 0.5f }, { Type.Volador, 0.5f }, { Type.Psiquico, 0.5f }, { Type.Bicho, 0.5f }, { Type.Hada, 0.5f },
            { Type.Fantasma, 0f }
        }},
        { Type.Veneno, new() {
            { Type.Planta, 2f }, { Type.Hada, 2f },
            { Type.Veneno, 0.5f }, { Type.Tierra, 0.5f }, { Type.Roca, 0.5f }, { Type.Fantasma, 0.5f },
            { Type.Acero, 0f }
        }},
        { Type.Tierra, new() {
            { Type.Fuego, 2f }, { Type.Electrico, 2f }, { Type.Veneno, 2f }, { Type.Roca, 2f }, { Type.Acero, 2f },
            { Type.Planta, 0.5f }, { Type.Bicho, 0.5f },
            { Type.Volador, 0f }
        }},
        { Type.Volador, new() {
            { Type.Planta, 2f }, { Type.Lucha, 2f }, { Type.Bicho, 2f },
            { Type.Roca, 0.5f }, { Type.Electrico, 0.5f }, { Type.Acero, 0.5f }
        }},
        { Type.Psiquico, new() {
            { Type.Lucha, 2f }, { Type.Veneno, 2f },
            { Type.Psiquico, 0.5f }, { Type.Acero, 0.5f },
            { Type.Siniestro, 0f }
        }},
        { Type.Bicho, new() {
            { Type.Planta, 2f }, { Type.Psiquico, 2f }, { Type.Siniestro, 2f },
            { Type.Fuego, 0.5f }, { Type.Lucha, 0.5f }, { Type.Veneno, 0.5f }, { Type.Volador, 0.5f },
            { Type.Fantasma, 0.5f }, { Type.Acero, 0.5f }, { Type.Hada, 0.5f }
        }},
        { Type.Roca, new() {
            { Type.Fuego, 2f }, { Type.Hielo, 2f }, { Type.Volador, 2f }, { Type.Bicho, 2f },
            { Type.Lucha, 0.5f }, { Type.Tierra, 0.5f }, { Type.Acero, 0.5f }
        }},
        { Type.Fantasma, new() {
            { Type.Psiquico, 2f }, { Type.Fantasma, 2f },
            { Type.Siniestro, 0.5f }, { Type.Normal, 0f }
        }},
        { Type.Dragon, new() {
            { Type.Dragon, 2f },
            { Type.Acero, 0.5f }, { Type.Hada, 0f }
        }},
        { Type.Siniestro, new() {
            { Type.Psiquico, 2f }, { Type.Fantasma, 2f },
            { Type.Lucha, 0.5f }, { Type.Siniestro, 0.5f }, { Type.Hada, 0.5f }
        }},
        { Type.Acero, new() {
            { Type.Hielo, 2f }, { Type.Roca, 2f }, { Type.Hada, 2f },
            { Type.Fuego, 0.5f }, { Type.Agua, 0.5f }, { Type.Electrico, 0.5f }, { Type.Acero, 0.5f }
        }},
        { Type.Hada, new() {
            { Type.Lucha, 2f }, { Type.Dragon, 2f }, { Type.Siniestro, 2f },
            { Type.Fuego, 0.5f }, { Type.Veneno, 0.5f }, { Type.Acero, 0.5f }
        }},
    };

    public static float CalcularEfectividad(Type ataque, Type defensa1, Type defensa2 = Type.None)
    {
        float mult1 = GetMultiplier(ataque, defensa1);
        float mult2 = defensa2 != Type.None ? GetMultiplier(ataque, defensa2) : 1f;
        return mult1 * mult2;
    }

    private static float GetMultiplier(Type ataque, Type defensa)
    {
        if (tablaTipos.TryGetValue(ataque, out var contra))
        {
            if (contra.TryGetValue(defensa, out float valor))
            {
                return valor;
            }
        }
        return 1f;
    }


}
