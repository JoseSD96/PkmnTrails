using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ControldorInterfaz : MonoBehaviour
{
    private CarruselImagenes carruselImagenes;
    private CambiadorSprites cambiadorSprites;
    private ControladorPartida controladorPartida;

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

    void Awake()
    {
        carruselImagenes = GetComponent<CarruselImagenes>();
        cambiadorSprites = GetComponent<CambiadorSprites>();
        controladorPartida = GetComponent<ControladorPartida>();
    }

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

        if (inicio)
        {
            explorarButton.interactable = false;
        }
        else
        {
            explorarButton.interactable = true;
        }

        if (SavingSystem.i.CheckIfSaveExists("Save"))
        {
            btnContinuar.interactable = true;
        }
        else
        {
            btnContinuar.interactable = false;
        }

    }

    private void OnSalirPCButtonClick()
    {
        controladorPartida.SalirPantallaPC();
    }

    private void OnNuevaPartidaButtonClick()
    {
        sistemaMenu.ConfirmacionNuevaPartida(() =>
        {
            controladorPartida.MostrarSeleccionDePersonaje();
        });
    }

    private void OnContinuarButtonClick()
    {
        controladorPartida.ContinuarPartida();
    }

    private void OnMostrarEquipoButtonClick()
    {
        controladorPartida.MostrarPanelEquipo();
    }

    private void OnMeterPCButtonClick()
    {
        controladorPartida.MeterPokemonPC();
    }

    private void OnSacarPCButtonClick()
    {
        controladorPartida.SacarPokemonPC();
    }

    private void OnCambiarConEquipoPCButtonClick()
    {
        controladorPartida.CambiarPokemonPC();
    }

    private void OnCambiarEquipoButtonClick()
    {
        controladorPartida.CambiarPokemon();
    }

    private void OnEliminarPCButtonClick()
    {
        controladorPartida.EliminarPokemonPC();
    }

    private void OnEliminarEquipoButtonClick()
    {
        controladorPartida.EliminarPokemonEquipo();
    }

    private void OnPCButtonClick()
    {
        controladorPartida.PantallaPC();
    }

    private void OnCajaAnteriorButtonClick()
    {
        controladorPartida.CambiarCaja(-1);
    }

    private void OnCajaSiguienteButtonClick()
    {
        controladorPartida.CambiarCaja(1);
    }

    public void OnHuirButtonClick()
    {
        controladorPartida.TerminarCombate();
    }

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
    }

    public void OnAnimacionTerminada()
    {
        animacionesPendientes--;
        if (animacionesPendientes <= 0)
        {
            avanzarButton.interactable = true;
            explorarButton.interactable = true;
            equipoButton.interactable = true;
            PCButton.interactable = true;
        }
    }

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

    private IEnumerator IniciarCombateTrasAnimacion()
    {
        yield return new WaitForSeconds(0.5f);
        controladorPartida.IniciarCombate();
    }

    public void OnEquipoButtonClick()
    {
        controladorPartida.PantallaEquipo();
    }

    public void OnSalirEquipoButtonClick()
    {
        controladorPartida.SalirPantallaEquipo();
    }

    public void OnDatosButtonClick()
    {
        controladorPartida.PantallaDatos();
    }

    public void OnVolverDatosButtonClick()
    {
        controladorPartida.SalirPantallaDatos();
    }


    public void BloquearAvanzarTemporalmente()
    {
        avanzarButton.interactable = false;
        StartCoroutine(DesbloquearAvanzarTrasAnimacion());
    }

    private IEnumerator DesbloquearAvanzarTrasAnimacion()
    {
        yield return new WaitForSeconds(1f);
        avanzarButton.interactable = true;
    }
}