using UnityEngine;
using UnityEngine.UI;

public class SeleccionDePersonajes : MonoBehaviour
{
    [SerializeField] private Button[] TrainersButtons;
    [SerializeField] private Button[] PokemonButtons;
    [SerializeField] private ControladorPartida controladorPartida;
    [SerializeField] private Button botonAceptar;
    [SerializeField] AudioManager audioManager;

    private int trainerSeleccionado = -1;
    private int pokemonSeleccionado = -1;

    private readonly int[] numerosPokemon = { 1, 4, 7 };

    private readonly string[] nombresTrainers = {
        "Rojo", "Azul", "Hoja", "Oro", "Plata", "Cristal",
        "Bruno", "Aura", "Blasco", "Lucas", "Maya", "Israel"
    };

    /// <summary>
    /// Inicializa los listeners de los botones de entrenadores y Pokémon.
    /// Configura el botón de aceptar y actualiza los colores de selección al iniciar la escena.
    /// </summary>
    void Start()
    {
        for (int i = 0; i < TrainersButtons.Length; i++)
        {
            int index = i;
            TrainersButtons[i].onClick.AddListener(() => SeleccionarTrainer(index));
        }
        for (int i = 0; i < PokemonButtons.Length; i++)
        {
            int index = i;
            PokemonButtons[i].onClick.AddListener(() => SeleccionarPokemon(index));
        }
        botonAceptar.onClick.AddListener(ConfirmarSeleccion);
        botonAceptar.onClick.AddListener(() => audioManager.PlayEfecto("Menus", "menuBoton"));
        botonAceptar.interactable = false;
        ActualizarColoresTrainers();
        ActualizarColoresPokemons();
    }

    /// <summary>
    /// Selecciona un entrenador según el índice recibido y actualiza la interfaz.
    /// </summary>
    /// <param name="index">Índice del entrenador seleccionado.</param>
    void SeleccionarTrainer(int index)
    {
        trainerSeleccionado = index;
        ActualizarColoresTrainers();
        ActualizarBotonAceptar();
    }

    /// <summary>
    /// Selecciona un Pokémon inicial según el índice recibido y actualiza la interfaz.
    /// </summary>
    /// <param name="index">Índice del Pokémon seleccionado.</param>
    void SeleccionarPokemon(int index)
    {
        pokemonSeleccionado = index;
        ActualizarColoresPokemons();
        ActualizarBotonAceptar();
    }

    /// <summary>
    /// Habilita o deshabilita el botón de aceptar según si hay selección válida de entrenador y Pokémon.
    /// </summary>
    void ActualizarBotonAceptar()
    {
        botonAceptar.interactable = (trainerSeleccionado != -1 && pokemonSeleccionado != -1);
    }

    /// <summary>
    /// Confirma la selección de entrenador y Pokémon, e inicia la nueva partida con los valores elegidos.
    /// </summary>
    void ConfirmarSeleccion()
    {
        if (trainerSeleccionado != -1 && pokemonSeleccionado != -1)
        {
            string nombreTrainer = nombresTrainers[trainerSeleccionado];
            int numeroPokemon = numerosPokemon[pokemonSeleccionado];
            controladorPartida.IniciarNuevaPartida(nombreTrainer, numeroPokemon);
        }
    }

    /// <summary>
    /// Actualiza los colores de los botones de entrenadores para reflejar la selección actual.
    /// </summary>
    void ActualizarColoresTrainers()
    {
        for (int i = 0; i < TrainersButtons.Length; i++)
        {
            var img = TrainersButtons[i].GetComponent<Image>();
            if (img != null)
                img.color = (i == trainerSeleccionado) ? Color.blue : Color.white;
        }
    }

    /// <summary>
    /// Actualiza los colores de los botones de Pokémon para reflejar la selección actual.
    /// </summary>
    void ActualizarColoresPokemons()
    {
        for (int i = 0; i < PokemonButtons.Length; i++)
        {
            var img = PokemonButtons[i].GetComponent<Image>();
            if (img != null)
                img.color = (i == pokemonSeleccionado) ? Color.blue : Color.white;
        }
    }
}