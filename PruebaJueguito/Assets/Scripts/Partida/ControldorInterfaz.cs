using UnityEngine;
using UnityEngine.UI;

public class ControldorInterfaz : MonoBehaviour
{
    private CarruselImagenes carruselImagenes;
    private CambiadorSpritesTrainer cambiadorSpritesTrainer;
    private CambiadorSpritesPkmn cambiadorSpritesPkmn;
    private ControladorPartida controladorPartida;

    private bool explorado = false;
    private int animacionesPendientes = 0;

    public Button avanzarButton;
    public Button explorarButton;
    public Button equipoButton;
    public Button salirEquipoButton;

    void Awake()
    {
        carruselImagenes = GetComponent<CarruselImagenes>();
        cambiadorSpritesTrainer = GetComponent<CambiadorSpritesTrainer>();
        cambiadorSpritesPkmn = GetComponent<CambiadorSpritesPkmn>();
        controladorPartida = GetComponent<ControladorPartida>();
    }

    void Start()
    {
        avanzarButton.onClick.RemoveAllListeners();
        explorarButton.onClick.RemoveAllListeners();
        equipoButton.onClick.RemoveAllListeners();
        salirEquipoButton.onClick.RemoveAllListeners();

        avanzarButton.onClick.AddListener(OnAvanzarButtonClick);
        explorarButton.onClick.AddListener(OnExplorarButtonClick);
        equipoButton.onClick.AddListener(OnEquipoButtonClick);
        salirEquipoButton.onClick.AddListener(OnSalirEquipoButtonClick);

        explorarButton.interactable = true;

    }

    public void OnAvanzarButtonClick()
    {
        explorado = false;
        explorarButton.interactable = false;

        animacionesPendientes = 3;

        carruselImagenes.AvanzarCarrusel(OnAnimacionTerminada);
        cambiadorSpritesTrainer.StartWalking(OnAnimacionTerminada);
        cambiadorSpritesPkmn.StartWalking(OnAnimacionTerminada);
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
        controladorPartida.IniciarCombate();
        cambiadorSpritesTrainer.Search();
        cambiadorSpritesPkmn.Search();
        explorarButton.interactable = false;
    }

    public void OnEquipoButtonClick()
    {
        controladorPartida.PantallaEquipo();
    }

    public void OnSalirEquipoButtonClick()
    {
        controladorPartida.SalirPantallaEquipo();
    }
}