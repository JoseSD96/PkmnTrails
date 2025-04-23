using System.Collections;
using UnityEngine;

public class CambiadorSprites : MonoBehaviour
{
    public string trainerName;
    public int walkRepetitions = 5;
    public Player player;

    private Coroutine walkCoroutine;
    [SerializeField] SpriteRenderer spriteRendererEntrenador;
    [SerializeField] SpriteRenderer spriteRendererPkmn;

    private bool isWalking = false;

    /// <summary>
    /// Al iniciar la escena, pone al entrenador y al Pokémon en estado idle.
    /// </summary>
    void Start()
    {
        SetIdle();
    }

    /// <summary>
    /// Cambia los sprites del entrenador y del Pokémon al estado idle.
    /// Si hay una animación de caminar en curso, la detiene.
    /// </summary>
    void SetIdle()
    {
        if (walkCoroutine != null)
        {
            StopCoroutine(walkCoroutine);
            walkCoroutine = null;
        }

        spriteRendererEntrenador.sprite = player.trainer.SpriteIdle;
        var pkmn = player.equipo != null ? player.equipo.GetPokemonByIndex(0) : null;
        if (pkmn != null)
        {
            if (pkmn.isShiny)
            {
                spriteRendererPkmn.sprite = pkmn.Base.SpriteIdleShiny;
            }
            else
            {
                spriteRendererPkmn.sprite = pkmn.Base.SpriteIdle;
            }
        }
        else
        {
            spriteRendererPkmn.sprite = null;
        }
        isWalking = false;
    }

    /// <summary>
    /// Inicia la animación de caminar del entrenador y el Pokémon.
    /// Al finalizar, ejecuta una acción opcional.
    /// </summary>
    /// <param name="onFinish">Acción a ejecutar al terminar la animación.</param>
    public void StartWalking(System.Action onFinish = null)
    {
        walkCoroutine = StartCoroutine(WalkAnimation(onFinish));
    }

    /// <summary>
    /// Corrutina que alterna los sprites de caminar del entrenador y el Pokémon durante varias repeticiones.
    /// </summary>
    /// <param name="onFinish">Acción a ejecutar al terminar la animación.</param>
    IEnumerator WalkAnimation(System.Action onFinish)
    {
        isWalking = true;
        int currentFrame = 0;
        int walkCounter = 0;

        while (walkCounter < walkRepetitions)
        {
            spriteRendererEntrenador.sprite = player.trainer.SpritesAndar[currentFrame];
            var pkmn = player.equipo != null ? player.equipo.GetPokemonByIndex(0) : null;
            if (pkmn != null)
            {
                if (pkmn.isShiny)
                {
                    spriteRendererPkmn.sprite = pkmn.Base.SpritesAndarShiny[currentFrame];
                }
                else
                {
                    spriteRendererPkmn.sprite = pkmn.Base.SpritesAndar[currentFrame];
                }
            }
            else
            {
                spriteRendererPkmn.sprite = null;
            }
            currentFrame = (currentFrame + 1) % 4;
            if (currentFrame == 0)
            {
                walkCounter++;
            }

            yield return new WaitForSeconds(0.1f);
        }
        isWalking = false;
        SetIdle();
        onFinish?.Invoke();
    }

    /// <summary>
    /// Corrutina que cambia los sprites al estado de búsqueda/exploración durante 2 segundos y luego vuelve a idle.
    /// </summary>
    IEnumerator SetSearch()
    {
        spriteRendererEntrenador.sprite = player.trainer.SpriteExploracion;
        var pkmn = player.equipo != null ? player.equipo.GetPokemonByIndex(0) : null;
        if (pkmn != null)
        {
            if (pkmn.isShiny)
            {
                spriteRendererPkmn.sprite = pkmn.Base.SpriteExploracionShiny;
            }
            else
            {
                spriteRendererPkmn.sprite = pkmn.Base.SpriteExploracion;
            }
        }
        else
        {
            spriteRendererPkmn.sprite = null;
        }
        yield return new WaitForSeconds(2f);
        SetIdle();
    }

    /// <summary>
    /// Inicia la animación de búsqueda/exploración si no se está caminando.
    /// </summary>
    public void Search()
    {
        if (!isWalking) StartCoroutine(SetSearch());
    }
}