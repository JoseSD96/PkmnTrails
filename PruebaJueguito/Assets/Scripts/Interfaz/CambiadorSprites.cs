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

    void Start()
    {
        SetIdle();
    }

    void SetIdle()
    {
        if (walkCoroutine != null)
        {
            StopCoroutine(walkCoroutine);
            walkCoroutine = null;
        }

        spriteRendererEntrenador.sprite = player.trainer.SpriteIdle;
        var pkmn = player.equipo.GetPokemonByIndex(0);
        if (pkmn.isShiny)
        {
            spriteRendererPkmn.sprite = pkmn.Base.SpriteIdleShiny;
        }
        else
        {
            spriteRendererPkmn.sprite = pkmn.Base.SpriteIdle;
        }
        isWalking = false;
    }

    public void StartWalking(System.Action onFinish = null)
    {
        walkCoroutine = StartCoroutine(WalkAnimation(onFinish));
    }

    IEnumerator WalkAnimation(System.Action onFinish)
    {
        isWalking = true;
        int currentFrame = 0;
        int walkCounter = 0;

        while (walkCounter < walkRepetitions)
        {
            spriteRendererEntrenador.sprite = player.trainer.SpritesAndar[currentFrame];
            var pkmn = player.equipo.GetPokemonByIndex(0);
            if (pkmn.isShiny)
            {
                spriteRendererPkmn.sprite = pkmn.Base.SpritesAndarShiny[currentFrame];
            }
            else
            {
                spriteRendererPkmn.sprite = pkmn.Base.SpritesAndar[currentFrame];
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

    IEnumerator SetSearch()
    {
        spriteRendererEntrenador.sprite = player.trainer.SpriteExploracion;
        var pkmn = player.equipo.GetPokemonByIndex(0);
        if (pkmn.isShiny)
        {
            spriteRendererPkmn.sprite = pkmn.Base.SpriteExploracionShiny;
        }
        else
        {
            spriteRendererPkmn.sprite = pkmn.Base.SpriteExploracion;
        }
        yield return new WaitForSeconds(2f);
        SetIdle();
    }

    public void Search()
    {
        if (!isWalking) StartCoroutine(SetSearch());
    }
}