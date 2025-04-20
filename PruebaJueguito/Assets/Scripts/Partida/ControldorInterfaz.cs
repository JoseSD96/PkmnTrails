using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ControldorInterfaz : MonoBehaviour
{
    private CarruselImagenes carruselImagenes;
    private CambiadorSprites cambiadorSprites;
    private ControladorPartida controladorPartida;

    public ZonaBase zonaActual;

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

    public Button btnEliminarPC;
    public Button btnSalirPC;
    public Button btnCajaSiguiente;
    public Button btnCajaAnterior;

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

        avanzarButton.onClick.AddListener(OnAvanzarButtonClick);
        explorarButton.onClick.AddListener(OnExplorarButtonClick);
        equipoButton.onClick.AddListener(OnEquipoButtonClick);
        salirEquipoButton.onClick.AddListener(OnSalirEquipoButtonClick);
        huirButton.onClick.AddListener(OnHuirButtonClick);
        btnDatos.onClick.AddListener(OnDatosButtonClick);
        btnVolverDatos.onClick.AddListener(OnVolverDatosButtonClick);
        btnSalirPC.onClick.AddListener(OnSalirEquipoButtonClick);
        btnCajaSiguiente.onClick.AddListener(OnCajaSiguienteButtonClick);
        btnCajaAnterior.onClick.AddListener(OnCajaAnteriorButtonClick);
        PCButton.onClick.AddListener(OnPCButtonClick);
        btnEliminarEquipo.onClick.AddListener(OnEliminarEquipoButtonClick);
        btnEliminarPC.onClick.AddListener(OnEliminarPCButtonClick);

        if (inicio)
        {
            explorarButton.interactable = false;
        }
        else
        {
            explorarButton.interactable = true;
        }

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
        explorarButton.interactable = false;

        animacionesPendientes = 2;

        carruselImagenes.AvanzarCarrusel(OnAnimacionTerminada);
        cambiadorSprites.StartWalking(OnAnimacionTerminada);
        controladorPartida.CurarPokemon();

        avanzarButton.interactable = false;
        equipoButton.interactable = false;
    }

    public void OnAnimacionTerminada()
    {
        animacionesPendientes--;
        if (animacionesPendientes <= 0)
        {
            avanzarButton.interactable = true;
            explorarButton.interactable = true;
            equipoButton.interactable = true;
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