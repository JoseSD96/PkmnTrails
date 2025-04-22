using UnityEngine;
using UnityEngine.UI;

public class SeleccionDePersonajes : MonoBehaviour
{
    [SerializeField] private Button[] TrainersButtons;
    [SerializeField] private Button[] PokemonButtons;
    [SerializeField] private ControladorPartida controladorPartida;
    [SerializeField] private Button botonAceptar;

    private int trainerSeleccionado = -1;
    private int pokemonSeleccionado = -1;

    private readonly int[] numerosPokemon = { 1, 4, 7 };

    private readonly string[] nombresTrainers = {
        "Rojo", "Azul", "Hoja", "Oro", "Plata", "Cristal",
        "Bruno", "Aura", "Blasco", "Lucas", "Maya", "Israel"
    };

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
        botonAceptar.interactable = false;
        ActualizarColoresTrainers();
        ActualizarColoresPokemons();
    }

    void SeleccionarTrainer(int index)
    {
        trainerSeleccionado = index;
        ActualizarColoresTrainers();
        ActualizarBotonAceptar();
    }

    void SeleccionarPokemon(int index)
    {
        pokemonSeleccionado = index;
        ActualizarColoresPokemons();
        ActualizarBotonAceptar();
    }

    void ActualizarBotonAceptar()
    {
        botonAceptar.interactable = (trainerSeleccionado != -1 && pokemonSeleccionado != -1);
    }

    void ConfirmarSeleccion()
    {
        if (trainerSeleccionado != -1 && pokemonSeleccionado != -1)
        {
            string nombreTrainer = nombresTrainers[trainerSeleccionado];
            int numeroPokemon = numerosPokemon[pokemonSeleccionado];
            controladorPartida.IniciarNuevaPartida(nombreTrainer, numeroPokemon);
        }
    }

    void ActualizarColoresTrainers()
    {
        for (int i = 0; i < TrainersButtons.Length; i++)
        {
            var img = TrainersButtons[i].GetComponent<Image>();
            if (img != null)
                img.color = (i == trainerSeleccionado) ? Color.blue : Color.white;
        }
    }

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