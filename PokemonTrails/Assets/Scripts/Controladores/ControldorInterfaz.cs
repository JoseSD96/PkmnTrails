using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ControldorInterfaz : MonoBehaviour
{
    public CarruselImagenes carruselImagenes;
    public CambiadorSprites cambiadorSprites;
    public ControladorPartida controladorPartida;

    public SistemaMenu sistemaMenu;

    public ZonaBase zonaActual;

    [SerializeField] AudioManager audioManager;

    private bool explorado = false;
    private int animacionesPendientes = 0;

    bool inicio = true;

    public Button avanzarButton;
    public Button explorarButton;
    public Button equipoButton;
    public Button PCButton;
    public Button PokedexButton;

    public SpriteRenderer shinyCharm;

    public Button SalirPokedexButton;

    public Button btnDatos;
    public Button btnEliminarEquipo;

    public Button btnVolverDatos;
    public Button salirEquipoButton;
    public Button btnCambiarEquipo;

    public Button btnEliminarPC;
    public Button btnSalirPC;
    public Button btnCajaSiguiente;
    public Button btnCajaAnterior;
    public Button btnCambarConEquipoPC;
    public Button btnSacarPc;
    public Button btnMeterPC;
    public Button btnMostrarEquipo;

    public Button btnContinuar;
    public Button btnNuevaPartida;

    public Button huirButton;

    /// <summary>
    /// Configura los listeners de los botones y su comportamiento al iniciar la escena.
    /// También ajusta la interactuabilidad de los botones según el estado del juego.
    /// </summary>
    void Start()
    {
        avanzarButton.onClick.RemoveAllListeners();
        explorarButton.onClick.RemoveAllListeners();
        equipoButton.onClick.RemoveAllListeners();
        salirEquipoButton.onClick.RemoveAllListeners();
        huirButton.onClick.RemoveAllListeners();
        btnDatos.onClick.RemoveAllListeners();
        btnVolverDatos.onClick.RemoveAllListeners();
        btnSalirPC.onClick.RemoveAllListeners();
        btnCajaSiguiente.onClick.RemoveAllListeners();
        btnCajaAnterior.onClick.RemoveAllListeners();
        PCButton.onClick.RemoveAllListeners();
        btnEliminarEquipo.onClick.RemoveAllListeners();
        btnEliminarPC.onClick.RemoveAllListeners();
        btnCambiarEquipo.onClick.RemoveAllListeners();
        btnCambarConEquipoPC.onClick.RemoveAllListeners();
        btnSacarPc.onClick.RemoveAllListeners();
        btnMeterPC.onClick.RemoveAllListeners();
        btnMostrarEquipo.onClick.RemoveAllListeners();
        btnContinuar.onClick.RemoveAllListeners();
        btnNuevaPartida.onClick.RemoveAllListeners();
        PokedexButton.onClick.RemoveAllListeners();
        SalirPokedexButton.onClick.RemoveAllListeners();

        avanzarButton.onClick.AddListener(OnAvanzarButtonClick);
        explorarButton.onClick.AddListener(OnExplorarButtonClick);
        equipoButton.onClick.AddListener(OnEquipoButtonClick);
        salirEquipoButton.onClick.AddListener(OnSalirEquipoButtonClick);
        huirButton.onClick.AddListener(OnHuirButtonClick);
        btnDatos.onClick.AddListener(OnDatosButtonClick);
        btnVolverDatos.onClick.AddListener(OnVolverDatosButtonClick);
        btnSalirPC.onClick.AddListener(OnSalirPCButtonClick);
        btnCajaSiguiente.onClick.AddListener(OnCajaSiguienteButtonClick);
        btnCajaAnterior.onClick.AddListener(OnCajaAnteriorButtonClick);
        PCButton.onClick.AddListener(OnPCButtonClick);
        btnEliminarEquipo.onClick.AddListener(OnEliminarEquipoButtonClick);
        btnEliminarPC.onClick.AddListener(OnEliminarPCButtonClick);
        btnCambiarEquipo.onClick.AddListener(OnCambiarEquipoButtonClick);
        btnCambarConEquipoPC.onClick.AddListener(OnCambiarConEquipoPCButtonClick);
        btnSacarPc.onClick.AddListener(OnSacarPCButtonClick);
        btnMeterPC.onClick.AddListener(OnMeterPCButtonClick);
        btnMostrarEquipo.onClick.AddListener(OnMostrarEquipoButtonClick);
        btnContinuar.onClick.AddListener(OnContinuarButtonClick);
        btnNuevaPartida.onClick.AddListener(OnNuevaPartidaButtonClick);
        PokedexButton.onClick.AddListener(OnPokedexButtonClick);
        SalirPokedexButton.onClick.AddListener(OnSalirPokedexButtonClick);


        avanzarButton.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));
        explorarButton.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));
        equipoButton.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));
        salirEquipoButton.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));
        btnDatos.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));
        btnVolverDatos.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));
        btnSalirPC.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));
        btnCajaSiguiente.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));
        btnCajaAnterior.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));
        PCButton.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));
        btnEliminarEquipo.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));
        btnEliminarPC.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));
        btnCambiarEquipo.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));
        btnCambarConEquipoPC.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));
        btnSacarPc.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));
        btnMeterPC.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));
        btnMostrarEquipo.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));
        btnContinuar.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));
        btnNuevaPartida.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));
        huirButton.onClick.AddListener(() => audioManager.PlayEfecto("Combate", "huir"));
        PokedexButton.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));
        SalirPokedexButton.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));

        if (inicio)
        {
            explorarButton.interactable = false;
        }
        else
        {
            explorarButton.interactable = true;
        }

        if (SavingSystem.i.CheckIfSaveExists("Save0.6"))
        {
            btnContinuar.interactable = true;
        }
        else
        {
            btnContinuar.interactable = false;
        }

    }

    private void OnSalirPokedexButtonClick()
    {
        controladorPartida.SalirPokedex();
    }

    private void OnPokedexButtonClick()
    {
        controladorPartida.MostrarPokedex();
    }

    /// <summary>
    /// Llama a la función para salir de la pantalla del PC.
    /// </summary>
    private void OnSalirPCButtonClick()
    {
        controladorPartida.SalirPantallaPC();
    }

    /// <summary>
    /// Muestra la confirmación para iniciar una nueva partida y, si se acepta, muestra la selección de personaje.
    /// </summary>
    private void OnNuevaPartidaButtonClick()
    {
        sistemaMenu.ConfirmacionNuevaPartida(() =>
        {
            controladorPartida.MostrarSeleccionDePersonaje();
        });
    }

    /// <summary>
    /// Continúa la partida cargando el estado guardado.
    /// </summary>
    private void OnContinuarButtonClick()
    {
        controladorPartida.ContinuarPartida();
    }

    /// <summary>
    /// Muestra u oculta el panel del equipo en la interfaz del PC.
    /// </summary>
    private void OnMostrarEquipoButtonClick()
    {
        controladorPartida.MostrarPanelEquipo();
    }

    /// <summary>
    /// Deposita el Pokémon seleccionado en el PC.
    /// </summary>
    private void OnMeterPCButtonClick()
    {
        controladorPartida.MeterPokemonPC();
    }

    /// <summary>
    /// Saca el Pokémon seleccionado del PC.
    /// </summary>
    private void OnSacarPCButtonClick()
    {
        controladorPartida.SacarPokemonPC();
    }

    /// <summary>
    /// Intercambia Pokémon entre el equipo y el PC.
    /// </summary>
    private void OnCambiarConEquipoPCButtonClick()
    {
        controladorPartida.CambiarPokemonPC();
    }

    /// <summary>
    /// Intercambia Pokémon dentro del equipo.
    /// </summary>
    private void OnCambiarEquipoButtonClick()
    {
        controladorPartida.CambiarPokemon();
    }

    /// <summary>
    /// Elimina el Pokémon seleccionado del PC.
    /// </summary>
    private void OnEliminarPCButtonClick()
    {
        controladorPartida.EliminarPokemonPC();
    }

    /// <summary>
    /// Elimina el Pokémon seleccionado del equipo.
    /// </summary>
    private void OnEliminarEquipoButtonClick()
    {
        controladorPartida.EliminarPokemonEquipo();
    }

    /// <summary>
    /// Muestra la interfaz del PC.
    /// </summary>
    private void OnPCButtonClick()
    {
        controladorPartida.PantallaPC();
    }

    /// <summary>
    /// Cambia a la caja anterior del PC.
    /// </summary>
    private void OnCajaAnteriorButtonClick()
    {
        controladorPartida.CambiarCaja(-1);
    }

    /// <summary>
    /// Cambia a la siguiente caja del PC.
    /// </summary>
    private void OnCajaSiguienteButtonClick()
    {
        controladorPartida.CambiarCaja(1);
    }

    /// <summary>
    /// Permite huir del combate y volver a exploración.
    /// </summary>
    public void OnHuirButtonClick()
    {
        controladorPartida.TerminarCombate();
    }

    /// <summary>
    /// Avanza en la exploración, reproduce animaciones y cura al equipo.
    /// Desactiva los botones mientras se realizan las animaciones.
    /// </summary>
    public void OnAvanzarButtonClick()
    {
        if (animacionesPendientes > 0) return;

        explorado = false;

        animacionesPendientes = 2;

        carruselImagenes.AvanzarCarrusel(OnAnimacionTerminada);
        cambiadorSprites.StartWalking(OnAnimacionTerminada);
        controladorPartida.CurarPokemon();

        avanzarButton.interactable = false;
        explorarButton.interactable = false;
        equipoButton.interactable = false;
        PCButton.interactable = false;
        PokedexButton.interactable = false;
    }

    /// <summary>
    /// Se llama cuando termina una animación de avance o movimiento.
    /// Reactiva los botones si no quedan animaciones pendientes.
    /// </summary>
    public void OnAnimacionTerminada()
    {
        animacionesPendientes--;
        if (animacionesPendientes <= 0)
        {
            avanzarButton.interactable = true;
            explorarButton.interactable = true;
            equipoButton.interactable = true;
            PCButton.interactable = true;
            PokedexButton.interactable = true;
        }
    }

    /// <summary>
    /// Inicia la exploración de la zona y, tras una animación, comienza un combate.
    /// </summary>
    public void OnExplorarButtonClick()
    {
        if (explorado)
        {
            return;
        }
        explorado = true;
        explorarButton.interactable = false;

        cambiadorSprites.Search();
        StartCoroutine(IniciarCombateTrasAnimacion());
    }

    /// <summary>
    /// Corrutina que espera antes de iniciar el combate tras la animación de exploración.
    /// </summary>
    private IEnumerator IniciarCombateTrasAnimacion()
    {
        yield return new WaitForSeconds(0.5f);
        controladorPartida.IniciarCombate();
    }

    /// <summary>
    /// Muestra la pantalla del equipo Pokémon.
    /// </summary>
    public void OnEquipoButtonClick()
    {
        controladorPartida.PantallaEquipo();
    }

    /// <summary>
    /// Sale de la pantalla del equipo y vuelve a exploración.
    /// </summary>
    public void OnSalirEquipoButtonClick()
    {
        controladorPartida.SalirPantallaEquipo();
    }

    /// <summary>
    /// Muestra la pantalla de datos del Pokémon seleccionado.
    /// </summary>
    public void OnDatosButtonClick()
    {
        controladorPartida.PantallaDatos();
    }

    /// <summary>
    /// Sale de la pantalla de datos y vuelve a la pantalla de equipo.
    /// </summary>
    public void OnVolverDatosButtonClick()
    {
        controladorPartida.SalirPantallaDatos();
    }

    /// <summary>
    /// Bloquea temporalmente el botón de avanzar, normalmente tras terminar un combate.
    /// </summary>
    public void BloquearAvanzarTemporalmente()
    {
        avanzarButton.interactable = false;
        StartCoroutine(DesbloquearAvanzarTrasAnimacion());
    }

    /// <summary>
    /// Corrutina que desbloquea el botón de avanzar tras un breve periodo.
    /// </summary>
    private IEnumerator DesbloquearAvanzarTrasAnimacion()
    {
        yield return new WaitForSeconds(1f);
        avanzarButton.interactable = true;
    }
}