using UnityEngine;

public class ControladorPartida : MonoBehaviour
{
    [SerializeField] SistemaCombate sistemaCombate;
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera CamaraBatalla;
    [SerializeField] Camera CamaraEquipo;
    [SerializeField] Canvas interfazExploracion;
    [SerializeField] Canvas interfazBatalla;
    [SerializeField] Canvas interfazEquipo;
    [SerializeField] GameObject jugador;
    [SerializeField] GameObject zonaActual;
    private void Start()
    {
        ActivarExploracion();
    }

    public void IniciarCombate()
    {
        mainCamera.enabled = false;
        CamaraBatalla.enabled = true;
        CamaraEquipo.enabled = false;

        interfazExploracion.gameObject.SetActive(false);
        interfazBatalla.gameObject.SetActive(true);
        interfazEquipo.gameObject.SetActive(false);

        var equipo = jugador.GetComponent<Equipo>();
        var salvaje = zonaActual.GetComponent<Salvajes>().GenerarPokemonSalvaje(equipo.GetMediaNivel());

        sistemaCombate.IniciarCombate(this, equipo, salvaje);

        sistemaCombate.pkmnJugador.ReiniciarEstado();
        sistemaCombate.pkmnEnemigo.ReiniciarEstado();
    }

    public void TerminarCombate()
    {
        ActivarExploracion();
    }

    void ActivarExploracion()
    {
        mainCamera.enabled = true;
        CamaraBatalla.enabled = false;
        CamaraEquipo.enabled = false;

        interfazExploracion.gameObject.SetActive(true);
        interfazBatalla.gameObject.SetActive(false);
        interfazEquipo.gameObject.SetActive(false);
    }

    public void PantallaEquipo()
    {
        mainCamera.enabled = false;
        CamaraEquipo.enabled = true;
        CamaraBatalla.enabled = false;

        interfazExploracion.gameObject.SetActive(false);
        interfazBatalla.gameObject.SetActive(false);
        interfazEquipo.gameObject.SetActive(true);
    }

    public void SalirPantallaEquipo()
    {
        ActivarExploracion();
    }

}
