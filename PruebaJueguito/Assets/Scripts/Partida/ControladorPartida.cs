using UnityEngine;

public class ControladorPartida : MonoBehaviour
{
    public ControldorInterfaz interfaz;
    [SerializeField] SistemaCombate sistemaCombate;
    [SerializeField] SistemaCaptura sistemaCaptura;
    [SerializeField] InterfazDatosPokemon sistemaDatos;
    [SerializeField] InterfazEquipo sistemaEquipo;
    [SerializeField] PC sistemaPC;

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

    [SerializeField] Player jugador;
    [SerializeField] GameObject zonaActual;

    private void Start()
    {
        SavingSystem.i.Load("PartidaGuardada");
        ActivarExploracion();
    }

    private void Awake()
    {
        TrainerManager.Init();
        PokemonBaseManager.Init();
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

        var equipo = jugador.equipo;

        var salvajes = zonaActual.GetComponent<Salvajes>();
        salvajes.Inicializar();

        var salvaje = salvajes.GenerarPokemonSalvaje(equipo.GetMediaNivel());

        sistemaCombate.IniciarCombate(this, equipo, salvaje, sistemaCaptura, ZonaManager.Instance.ZonaActual);

        sistemaCombate.pkmnJugador.ReiniciarEstado();
        sistemaCombate.pkmnEnemigo.ReiniciarEstado();
    }

    public void CurarPokemon()
    {
        SavingSystem.i.Save("PartidaGuardada1");
        var equipo = jugador.equipo;
        for (int i = 0; i < equipo.pokemones.Count; i++)
        {
            if (equipo.pokemones[i].HP < equipo.pokemones[i].MaxHP)
            {
                equipo.pokemones[i].HP += (int)(equipo.pokemones[i].MaxHP * 0.05f);
                if (equipo.pokemones[i].HP > equipo.pokemones[i].MaxHP)
                {
                    equipo.pokemones[i].HP = equipo.pokemones[i].MaxHP;
                }
            }
        }
    }

    public void TerminarCombate()
    {
        if (interfaz != null)
            interfaz.BloquearAvanzarTemporalmente();
        ActivarExploracion();
    }

    public void TerminarCombate(Pokemon pkmn)
    {
        jugador.equipo.AddPokemon(pkmn);
        ActivarExploracion();
    }

    void ActivarExploracion()
    {
        SavingSystem.i.Save("PartidaGuardada");
        var cambiadorSprites = FindFirstObjectByType<CambiadorSprites>();
        if (cambiadorSprites != null)
            cambiadorSprites.SendMessage("SetIdle");

        mainCamera.enabled = true;
        CamaraBatalla.enabled = false;
        CamaraEquipo.enabled = false;
        CamaraDatos.enabled = false;
        CamaraPC.enabled = false;

        interfazPC.gameObject.SetActive(false);
        interfazExploracion.gameObject.SetActive(true);
        interfazBatalla.gameObject.SetActive(false);
        interfazEquipo.gameObject.SetActive(false);
        interfazDatos.gameObject.SetActive(false);
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

        sistemaPC.MostrarCaja(sistemaPC.cajaActual);
        sistemaPC.LimpiarSeleccion();
    }

    public void SalirPantallaPC()
    {
        sistemaPC.LimpiarSeleccion();
        ActivarExploracion();
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
}
