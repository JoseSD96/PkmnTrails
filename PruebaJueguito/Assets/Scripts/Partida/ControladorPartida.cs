using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControladorPartida : MonoBehaviour
{
    public ControldorInterfaz interfaz;
    [SerializeField] SistemaCombate sistemaCombate;
    [SerializeField] SistemaCaptura sistemaCaptura;
    [SerializeField] InterfazDatosPokemon sistemaDatos;
    [SerializeField] InterfazEquipo sistemaEquipo;

    [SerializeField] SistemaMenu sistemaMenu;

    [SerializeField] PC sistemaPC;
    [SerializeField] Equipo equipo;
    [SerializeField] AudioManager audioManager;

    [SerializeField] Canvas interfazExploracion;
    [SerializeField] Camera mainCamera;

    [SerializeField] Canvas interfazBatalla;
    [SerializeField] Camera CamaraBatalla;

    [SerializeField] Canvas interfazEquipo;
    [SerializeField] Camera CamaraEquipo;

    [SerializeField] Canvas interfazDatos;
    [SerializeField] Camera CamaraDatos;

    [SerializeField] Camera CamaraPC;
    [SerializeField] Canvas interfazPC;

    [SerializeField] Camera CamaraMenuInicio;
    [SerializeField] Canvas interfazMenuInicio;

    [SerializeField] Canvas interfazSeleccionPersonaje;
    [SerializeField] Camera camaraSeleccionPersonaje;

    [SerializeField] Player jugador;
    [SerializeField] GameObject zonaActual;

    private void Start()
    {
        MostrarMenuInicio();
    }

    private void Awake()
    {
        TrainerManager.Init();
        PokemonBaseManager.Init();
    }

    public void MostrarMenuInicio()
    {
        mainCamera.enabled = false;
        CamaraBatalla.enabled = false;
        CamaraEquipo.enabled = false;
        CamaraDatos.enabled = false;
        CamaraPC.enabled = false;
        CamaraMenuInicio.enabled = true;

        interfazMenuInicio.gameObject.SetActive(true);
        interfazExploracion.gameObject.SetActive(false);
        interfazBatalla.gameObject.SetActive(false);
        interfazEquipo.gameObject.SetActive(false);
        interfazDatos.gameObject.SetActive(false);
        interfazPC.gameObject.SetActive(false);

        CambiarAudioListener(CamaraMenuInicio);

    }

    public void IniciarCombate()
    {
        mainCamera.enabled = false;
        CamaraBatalla.enabled = true;
        CamaraEquipo.enabled = false;
        CamaraDatos.enabled = false;
        CamaraPC.enabled = false;

        interfazExploracion.gameObject.SetActive(false);
        interfazBatalla.gameObject.SetActive(true);
        interfazEquipo.gameObject.SetActive(false);
        interfazDatos.gameObject.SetActive(false);
        interfazPC.gameObject.SetActive(false);

        CambiarAudioListener(CamaraBatalla);

        var equipo = jugador.equipo;

        var salvajes = zonaActual.GetComponent<Salvajes>();
        salvajes.Inicializar();

        var salvaje = salvajes.GenerarPokemonSalvaje(equipo.GetMediaNivel());
        bool esLegendario = salvaje.Base.Num == 144 || salvaje.Base.Num == 145 ||
                    salvaje.Base.Num == 146 || salvaje.Base.Num == 150 ||
                    salvaje.Base.Num == 151;

        if (esLegendario)
        {
            audioManager.PlayMusicaCombateLegendario();
        }
        else
        {
            audioManager.PlayMusicaCombateNormal();
        }
        sistemaCombate.IniciarCombate(this, equipo, salvaje, sistemaCaptura, ZonaManager.Instance.ZonaActual);

        sistemaCombate.pkmnJugador.ReiniciarEstado();
        sistemaCombate.pkmnEnemigo.ReiniciarEstado();
    }

    public void CurarPokemon()
    {
        SavingSystem.i.Save("Save");
        for (int i = 0; i < equipo.pokemones.Count; i++)
        {
            if (equipo.pokemones[i].HP < equipo.pokemones[i].MaxHP)
            {
                int cantidadCurar = Mathf.Max(1, (int)(equipo.pokemones[i].MaxHP * 0.05f));
                equipo.pokemones[i].HP += cantidadCurar;

                if (equipo.pokemones[i].HP > equipo.pokemones[i].MaxHP)
                {
                    equipo.pokemones[i].HP = equipo.pokemones[i].MaxHP;
                }
            }
        }
    }

    public void TerminarCombate()
    {
        audioManager.PlayEfecto("Combate", "huir");
        if (interfaz != null)
            interfaz.BloquearAvanzarTemporalmente();
        ActivarExploracion();
    }

    public void TerminarCombate(Pokemon pkmn)
    {
        if (jugador.Pokedex.Contains(pkmn.Base.Num) == false)
        {
            jugador.Pokedex.Add(pkmn.Base.Num);
        }
        jugador.equipo.AddPokemon(pkmn);
        ActivarExploracion();
    }

    void ActivarExploracion()
    {
        audioManager.PlayMusicaExploracion();
        SavingSystem.i.Save("Save");
        var cambiadorSprites = FindFirstObjectByType<CambiadorSprites>();
        if (cambiadorSprites != null)
            cambiadorSprites.SendMessage("SetIdle");

        mainCamera.enabled = true;
        CamaraBatalla.enabled = false;
        CamaraEquipo.enabled = false;
        CamaraDatos.enabled = false;
        CamaraPC.enabled = false;
        CamaraMenuInicio.enabled = false;
        camaraSeleccionPersonaje.enabled = false;

        interfazPC.gameObject.SetActive(false);
        interfazExploracion.gameObject.SetActive(true);
        interfazBatalla.gameObject.SetActive(false);
        interfazEquipo.gameObject.SetActive(false);
        interfazDatos.gameObject.SetActive(false);
        interfazMenuInicio.gameObject.SetActive(false);
        interfazSeleccionPersonaje.gameObject.SetActive(false);

        CambiarAudioListener(mainCamera);
    }

    public void PantallaEquipo()
    {
        mainCamera.enabled = false;
        CamaraEquipo.enabled = true;
        CamaraBatalla.enabled = false;
        CamaraDatos.enabled = false;
        CamaraPC.enabled = false;

        interfazPC.gameObject.SetActive(false);
        interfazExploracion.gameObject.SetActive(false);
        interfazBatalla.gameObject.SetActive(false);
        interfazDatos.gameObject.SetActive(false);
        interfazEquipo.gameObject.SetActive(true);

        CambiarAudioListener(CamaraEquipo);
    }

    public void PantallaDatos()
    {
        sistemaDatos.MostrarDatos(sistemaEquipo.PokemonSeleccionado);
        mainCamera.enabled = false;
        CamaraEquipo.enabled = false;
        CamaraDatos.enabled = true;
        CamaraBatalla.enabled = false;
        CamaraPC.enabled = false;

        interfazPC.gameObject.SetActive(false);
        interfazExploracion.gameObject.SetActive(false);
        interfazBatalla.gameObject.SetActive(false);
        interfazEquipo.gameObject.SetActive(false);
        interfazDatos.gameObject.SetActive(true);

        CambiarAudioListener(CamaraDatos);
    }

    public void SalirPantallaEquipo()
    {
        sistemaEquipo.LimpiarSeleccion();
        ActivarExploracion();
    }

    public void SalirPantallaDatos()
    {
        sistemaEquipo.LimpiarSeleccion();
        PantallaEquipo();
    }

    public void EliminarPokemonEquipo()
    {
        sistemaEquipo.Equipo.EliminarPokemon(sistemaEquipo.indiceSeleccionado);
        sistemaEquipo.LimpiarSeleccion();
    }

    public void PantallaPC()
    {
        mainCamera.enabled = false;
        CamaraEquipo.enabled = false;
        CamaraDatos.enabled = false;
        CamaraBatalla.enabled = false;
        CamaraPC.enabled = true;

        interfazPC.gameObject.SetActive(true);
        interfazExploracion.gameObject.SetActive(false);
        interfazBatalla.gameObject.SetActive(false);
        interfazEquipo.gameObject.SetActive(false);
        interfazDatos.gameObject.SetActive(false);

        CambiarAudioListener(CamaraPC);

        sistemaPC.MostrarCaja(sistemaPC.cajaActual);
        sistemaPC.LimpiarSeleccion();
    }

    public void SalirPantallaPC()
    {
        sistemaPC.LimpiarSeleccion();
        sistemaPC.OcultarInterfazEquipoEnPC();

        StartCoroutine(ForzarPanelEquipoOculto());

        ActivarExploracion();
    }

    private IEnumerator ForzarPanelEquipoOculto()
    {
        yield return null;
        sistemaPC.panelEquipo.anchoredPosition = new Vector2(
            sistemaPC.panelEquipo.anchoredPosition.x, 440);
    }

    public void CambiarCaja(int direccion)
    {
        sistemaPC.CambiarCaja(direccion);
        sistemaPC.LimpiarSeleccion();
    }

    public void EliminarPokemonPC()
    {
        sistemaPC.EliminarPokemonDeCajaActual(sistemaPC.indiceSeleccionado);
        sistemaPC.LimpiarSeleccion();
    }

    public void CambiarPokemon()
    {
        sistemaEquipo.IniciarIntercambio();
        sistemaEquipo.LimpiarSeleccion();
    }

    public void CambiarPokemonPC()
    {
        sistemaPC.IniciarIntercambioConEquipo();
    }

    public void SacarPokemonPC()
    {
        sistemaPC.SacarDelPC();
        sistemaPC.LimpiarSeleccion();
    }

    public void MostrarPanelEquipo()
    {
        sistemaPC.MostrarInterfazEquipoEnPC();
    }

    public void MeterPokemonPC()
    {
        sistemaPC.DepositarEnPC();
        sistemaPC.LimpiarSeleccion();
    }

    void CambiarAudioListener(Camera camaraActiva)
    {
        mainCamera.GetComponent<AudioListener>().enabled = false;
        CamaraBatalla.GetComponent<AudioListener>().enabled = false;
        CamaraEquipo.GetComponent<AudioListener>().enabled = false;
        CamaraDatos.GetComponent<AudioListener>().enabled = false;
        CamaraPC.GetComponent<AudioListener>().enabled = false;
        CamaraMenuInicio.GetComponent<AudioListener>().enabled = false;
        camaraSeleccionPersonaje.GetComponent<AudioListener>().enabled = false;

        camaraActiva.GetComponent<AudioListener>().enabled = true;
    }

    public void ContinuarPartida()
    {
        SavingSystem.i.Load("Save");
        ActivarExploracion();
    }

    public void IniciarNuevaPartida(string trainerName, int pokemonIndex)
    {
        jugador.trainer = TrainerManager.GetTrainer(trainerName);

        if (jugador.equipo == null)
            jugador.equipo = equipo;
        jugador.equipo.pokemones = new List<Pokemon>();

        if (jugador.pc == null)
            jugador.pc = sistemaPC;
        jugador.pc.cajas = new Dictionary<int, List<Pokemon>>();
        for (int i = 0; i < 18; i++)
            jugador.pc.cajas.Add(i, new List<Pokemon>());

        jugador.equipo.pc = jugador.pc;

        var pokemonInicial = PokemonBaseManager.GetPokemon(pokemonIndex);
        int potencialInicial = Random.Range(20, 32);
        bool isShiny = Random.Range(0, 100) < 10;
        jugador.equipo.AddPokemon(new Pokemon(pokemonInicial, potencialInicial, isShiny, 5));
        jugador.Pokedex.Add(pokemonInicial.Num);

        var cambiadorSprites = FindFirstObjectByType<CambiadorSprites>();
        if (cambiadorSprites != null)
            cambiadorSprites.SendMessage("SetIdle");

        interfazSeleccionPersonaje.gameObject.SetActive(false);
        camaraSeleccionPersonaje.enabled = false;
        mainCamera.enabled = true;
        interfazExploracion.gameObject.SetActive(true);

        CambiarAudioListener(mainCamera);
    }

    public void MostrarSeleccionDePersonaje()
    {
        mainCamera.enabled = false;
        CamaraBatalla.enabled = false;
        CamaraEquipo.enabled = false;
        CamaraDatos.enabled = false;
        CamaraPC.enabled = false;
        CamaraMenuInicio.enabled = false;
        camaraSeleccionPersonaje.enabled = true;

        interfazMenuInicio.gameObject.SetActive(false);
        interfazExploracion.gameObject.SetActive(false);
        interfazBatalla.gameObject.SetActive(false);
        interfazEquipo.gameObject.SetActive(false);
        interfazDatos.gameObject.SetActive(false);
        interfazPC.gameObject.SetActive(false);
        interfazSeleccionPersonaje.gameObject.SetActive(true);

        CambiarAudioListener(camaraSeleccionPersonaje);
    }

}
