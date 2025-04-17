using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CambiadorSpritesPkmn : MonoBehaviour
{
    public bool isShiny = false;
    private int walkRepetitions = 5;
    public GameObject pokemonBackObject;
    private SpriteRenderer spriteRenderer;
    public PokemonBase pokemon;
    private int currentFrame = 0;
    private bool isWalking = false;
    private int walkCounter = 0;
    void Start()
    {
        spriteRenderer = pokemonBackObject.GetComponent<SpriteRenderer>();
        SetIdle();
    }

    void SetIdle()
    {
        if (isShiny)
        {
            spriteRenderer.sprite = pokemon.SpriteIdleShiny;
        }
        else
        {
            spriteRenderer.sprite = pokemon.SpriteIdle;
        }

        currentFrame = 0;
        isWalking = false;
        walkCounter = 0;
    }

    public void StartWalking(System.Action onFinish = null)
    {
        if (!isWalking)
        {
            StartCoroutine(WalkCoroutine(onFinish));
        }
    }

    private IEnumerator WalkCoroutine(System.Action onFinish)
    {
        isWalking = true;
        currentFrame = 0;
        walkCounter = 0;

        while (walkCounter < walkRepetitions)
        {
            if (isShiny)
            {
                spriteRenderer.sprite = pokemon.SpritesAndarShiny[currentFrame];
            }
            else
            {
                spriteRenderer.sprite = pokemon.SpritesAndar[currentFrame];
            }

            yield return new WaitForSeconds(0.1f);

            currentFrame = (currentFrame + 1) % 4;

            if (currentFrame == 0)
            {
                walkCounter++;
            }
        }

        SetIdle();
        onFinish?.Invoke();
    }

    public void Search()
    {
        if (!isWalking)
        {
            StartCoroutine(SearchCoroutine());
        }
    }

    IEnumerator SearchCoroutine()
    {
        if (isShiny)
        {
            spriteRenderer.sprite = pokemon.SpriteExploracionShiny;
        }
        else
        {
            spriteRenderer.sprite = pokemon.SpriteExploracion;
        }

        yield return new WaitForSeconds(2);
        SetIdle();
    }

}