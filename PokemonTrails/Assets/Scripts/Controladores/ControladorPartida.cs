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
    [SerializeField] PokedexManager pokedexManager;

    private List<Camera> todasLasCamaras;
    private List<Canvas> todasLasInterfaces;

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

    [SerializeField] Canvas interfazPokedex;
    [SerializeField] Camera camaraPokedex;

    [SerializeField] public Player jugador;
    [SerializeField] GameObject zonaActual;

    /// <summary>
    /// Se ejecuta al iniciar la escena. Muestra el menú de inicio.
    /// </summary>
    private void Start()
    {
        MostrarMenuInicio();
    }

    private void Awake()
    {
        TrainerManager.Init();
        PokemonBaseManager.Init();

        todasLasCamaras = new List<Camera> {
            mainCamera, CamaraBatalla, CamaraEquipo, CamaraDatos, CamaraPC, CamaraMenuInicio, camaraSeleccionPersonaje, camaraPokedex
        };
        todasLasInterfaces = new List<Canvas> {
            interfazExploracion, interfazBatalla, interfazEquipo, interfazDatos, interfazPC, interfazMenuInicio, interfazSeleccionPersonaje, interfazPokedex
        };
    }

    // Método auxiliar para activar solo la cámara deseada
    private void ActivarSoloEstaCamara(Camera camara)
    {
        foreach (var c in todasLasCamaras)
            c.enabled = (c == camara);
        CambiarAudioListener(camara);
    }

    // Método auxiliar para activar solo la interfaz deseada
    private void ActivarSoloEstaInterfaz(Canvas interfaz)
    {
        foreach (var i in todasLasInterfaces)
            i.gameObject.SetActive(i == interfaz);
    }

    /// <summary>
    /// Activa la interfaz y cámara del menú de inicio, desactivando el resto.
    /// También reproduce la música del menú.
    /// </summary>
    public void MostrarMenuInicio()
    {
        audioManager.PlayMusicaMenuInicio();
        ActivarSoloEstaCamara(CamaraMenuInicio);
        ActivarSoloEstaInterfaz(interfazMenuInicio);
    }

    public void MostrarPokedex()
    {
        pokedexManager.ActualizarIluminacionPokemons();
        audioManager.PlayMusicaPC();
        ActivarSoloEstaCamara(camaraPokedex);
        ActivarSoloEstaInterfaz(interfazPokedex);

        CambiarAudioListener(camaraPokedex);
    }

    public void SalirPokedex()
    {
        ActivarExploracion(true);
    }

    /// <summary>
    /// Inicia un combate contra un Pokémon salvaje, eligiendo la música según si es legendario.
    /// Prepara el sistema de combate y reinicia los estados de los Pokémon.
    /// </summary>
    public void IniciarCombate()
    {
        ActivarSoloEstaCamara(CamaraBatalla);

        ActivarSoloEstaInterfaz(interfazBatalla);

        CambiarAudioListener(CamaraBatalla);

        var equipo = jugador.equipo;

        var salvajes = zonaActual.GetComponent<Salvajes>();
        salvajes.Inicializar();

        bool shinyCharm = false;
        if (jugador.pokedex.pokemones.Count >= 100)
        {
            shinyCharm = true;
        }

        var salvaje = salvajes.GenerarPokemonSalvaje(equipo.GetMediaNivel(), shinyCharm);
        bool esLegendario = salvaje.Base.Num == 144 || salvaje.Base.Num == 145 ||
                    salvaje.Base.Num == 146 || salvaje.Base.Num == 150 ||
                    salvaje.Base.Num == 151 || salvaje.Base.Num == 243 ||
                    salvaje.Base.Num == 244 || salvaje.Base.Num == 245 ||
                    salvaje.Base.Num == 249 || salvaje.Base.Num == 250 ||
                    salvaje.Base.Num == 251;

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

    /// <summary>
    /// Cura a todos los Pokémon del equipo en un 10% de su vida máxima, sin superar el máximo.
    /// Guarda la partida antes de curar.
    /// </summary>
    public void CurarPokemon()
    {
        SavingSystem.i.Save("Save0.6");
        for (int i = 0; i < equipo.pokemones.Count; i++)
        {
            if (equipo.pokemones[i].HP < equipo.pokemones[i].MaxHP)
            {
                int cantidadCurar = Mathf.Max(1, (int)(equipo.pokemones[i].MaxHP * 0.1f));
                equipo.pokemones[i].HP += cantidadCurar;

                if (equipo.pokemones[i].HP > equipo.pokemones[i].MaxHP)
                {
                    equipo.pokemones[i].HP = equipo.pokemones[i].MaxHP;
                }
            }
        }
    }

    /// <summary>
    /// Termina el combate y vuelve al modo exploración, bloqueando temporalmente la interfaz si existe.
    /// </summary>
    public void TerminarCombate()
    {
        if (interfaz != null)
            interfaz.BloquearAvanzarTemporalmente();
        ActivarExploracion();
    }

    /// <summary>
    /// Termina el combate, añade el Pokémon capturado a la Pokédex y al equipo, y vuelve a exploración.
    /// </summary>
    /// <param name="pkmn">Pokémon capturado.</param>
    public void TerminarCombate(Pokemon pkmn)
    {
        if (jugador.pokedex.pokemones.Contains(pkmn.Base) == false)
        {
            jugador.pokedex.pokemones.Add(pkmn.Base);
        }
        jugador.equipo.AddPokemon(pkmn);
        ActivarExploracion();
    }

    /// <summary>
    /// Activa el modo exploración, cámaras e interfaces correspondientes.
    /// Opcionalmente cambia la música de fondo.
    /// </summary>
    /// <param name="cambiarMusica">Si es true, cambia la música de exploración.</param>
    void ActivarExploracion(bool cambiarMusica = true)
    {
        if (cambiarMusica)
            audioManager.PlayMusicaExploracion();
        SavingSystem.i.Save("Save0.6");
        var cambiadorSprites = FindFirstObjectByType<CambiadorSprites>();
        if (cambiadorSprites != null)
            cambiadorSprites.SendMessage("SetIdle");

        ActivarSoloEstaCamara(mainCamera);

        ActivarSoloEstaInterfaz(interfazExploracion);

        CambiarAudioListener(mainCamera);
    }

    /// <summary>
    /// Muestra la pantalla del equipo Pokémon, activando la cámara e interfaz correspondiente.
    /// </summary>
    public void PantallaEquipo()
    {
        ActivarSoloEstaCamara(CamaraEquipo);
        ActivarSoloEstaInterfaz(interfazEquipo);

        CambiarAudioListener(CamaraEquipo);
    }

    /// <summary>
    /// Muestra la pantalla de datos del Pokémon seleccionado.
    /// </summary>
    public void PantallaDatos()
    {
        sistemaDatos.MostrarDatos(sistemaEquipo.PokemonSeleccionado);
        ActivarSoloEstaCamara(CamaraDatos);
        ActivarSoloEstaInterfaz(interfazDatos);

        CambiarAudioListener(CamaraDatos);
    }

    /// <summary>
    /// Sale de la pantalla de equipo y vuelve a exploración sin cambiar la música.
    /// </summary>
    public void SalirPantallaEquipo()
    {
        sistemaEquipo.LimpiarSeleccion();
        ActivarExploracion(false);
    }

    /// <summary>
    /// Sale de la pantalla de datos y vuelve a la pantalla de equipo.
    /// </summary>
    public void SalirPantallaDatos()
    {
        sistemaEquipo.LimpiarSeleccion();
        PantallaEquipo();
    }

    /// <summary>
    /// Elimina el Pokémon seleccionado del equipo y limpia la selección.
    /// </summary>
    public void EliminarPokemonEquipo()
    {
        sistemaEquipo.Equipo.EliminarPokemon(sistemaEquipo.indiceSeleccionado);
        sistemaEquipo.LimpiarSeleccion();
    }

    /// <summary>
    /// Muestra la interfaz del PC, reproduce el sonido de abrir PC y la música correspondiente.
    /// </summary>
    public void PantallaPC()
    {
        audioManager.PlayEfecto("Menus", "abrirPC");
        ActivarSoloEstaCamara(CamaraPC);
        ActivarSoloEstaInterfaz(interfazPC);

        CambiarAudioListener(CamaraPC);

        sistemaPC.MostrarCaja(sistemaPC.cajaActual);
        sistemaPC.LimpiarSeleccion();
        audioManager.PlayMusicaPC();
    }

    /// <summary>
    /// Sale de la pantalla del PC, reproduce el sonido de cerrar PC y vuelve a exploración.
    /// </summary>
    public void SalirPantallaPC()
    {
        audioManager.PlayEfecto("Menus", "CerrarPC");
        sistemaPC.LimpiarSeleccion();
        sistemaPC.OcultarInterfazEquipoEnPC();

        StartCoroutine(ForzarPanelEquipoOculto());

        ActivarExploracion();
    }

    /// <summary>
    /// Corrutina para forzar la posición del panel del equipo en el PC tras salir de la interfaz.
    /// </summary>
    private IEnumerator ForzarPanelEquipoOculto()
    {
        yield return null;
        sistemaPC.panelEquipo.anchoredPosition = new Vector2(
            sistemaPC.panelEquipo.anchoredPosition.x, 440);
    }

    /// <summary>
    /// Cambia la caja activa del PC y limpia la selección.
    /// </summary>
    /// <param name="direccion">Dirección de cambio de caja (siguiente/anterior).</param>
    public void CambiarCaja(int direccion)
    {
        sistemaPC.CambiarCaja(direccion);
        sistemaPC.LimpiarSeleccion();
    }

    /// <summary>
    /// Elimina el Pokémon seleccionado de la caja actual del PC.
    /// </summary>
    public void EliminarPokemonPC()
    {
        sistemaPC.EliminarPokemonDeCajaActual(sistemaPC.indiceSeleccionado);
        sistemaPC.LimpiarSeleccion();
    }

    /// <summary>
    /// Inicia el intercambio de Pokémon dentro del equipo.
    /// </summary>
    public void CambiarPokemon()
    {
        sistemaEquipo.IniciarIntercambio();
        sistemaEquipo.LimpiarSeleccion();
    }

    /// <summary>
    /// Inicia el intercambio de Pokémon entre el PC y el equipo.
    /// </summary>
    public void CambiarPokemonPC()
    {
        sistemaPC.IniciarIntercambioConEquipo();
    }

    /// <summary>
    /// Saca el Pokémon seleccionado del PC y limpia la selección.
    /// </summary>
    public void SacarPokemonPC()
    {
        sistemaPC.SacarDelPC();
        sistemaPC.LimpiarSeleccion();
    }

    /// <summary>
    /// Muestra u oculta el panel del equipo en la interfaz del PC.
    /// </summary>
    public void MostrarPanelEquipo()
    {
        sistemaPC.TogglePanelEquipo();
    }

    /// <summary>
    /// Deposita el Pokémon seleccionado en el PC y limpia la selección.
    /// </summary>
    public void MeterPokemonPC()
    {
        sistemaPC.DepositarEnPC();
        sistemaPC.LimpiarSeleccion();
    }

    /// <summary>
    /// Activa el AudioListener solo en la cámara activa y lo desactiva en el resto.
    /// </summary>
    /// <param name="camaraActiva">Cámara que debe tener el AudioListener activo.</param>
    void CambiarAudioListener(Camera camaraActiva)
    {
        mainCamera.GetComponent<AudioListener>().enabled = false;
        CamaraBatalla.GetComponent<AudioListener>().enabled = false;
        CamaraEquipo.GetComponent<AudioListener>().enabled = false;
        CamaraDatos.GetComponent<AudioListener>().enabled = false;
        CamaraPC.GetComponent<AudioListener>().enabled = false;
        CamaraMenuInicio.GetComponent<AudioListener>().enabled = false;
        camaraSeleccionPersonaje.GetComponent<AudioListener>().enabled = false;
        camaraPokedex.GetComponent<AudioListener>().enabled = false;

        camaraActiva.GetComponent<AudioListener>().enabled = true;
    }

    /// <summary>
    /// Carga la partida guardada y vuelve al modo exploración.
    /// </summary>
    public void ContinuarPartida()
    {
        SavingSystem.i.Load("Save0.6");
        ActivarExploracion();
    }

    /// <summary>
    /// Inicia una nueva partida, asignando el entrenador, el Pokémon inicial y configurando el equipo y PC.
    /// </summary>
    /// <param name="trainerName">Nombre del entrenador.</param>
    /// <param name="pokemonIndex">Índice del Pokémon inicial.</param>
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
        jugador.pokedex.pokemones.Add(pokemonInicial);

        var cambiadorSprites = FindFirstObjectByType<CambiadorSprites>();
        if (cambiadorSprites != null)
            cambiadorSprites.SendMessage("SetIdle");

        ActivarExploracion(true);

        CambiarAudioListener(mainCamera);
    }

    /// <summary>
    /// Muestra la pantalla de selección de personaje, activando la cámara e interfaz correspondiente.
    /// </summary>
    public void MostrarSeleccionDePersonaje()
    {
        ActivarSoloEstaCamara(camaraSeleccionPersonaje);
        ActivarSoloEstaInterfaz(interfazSeleccionPersonaje);

        CambiarAudioListener(camaraSeleccionPersonaje);
    }

}
