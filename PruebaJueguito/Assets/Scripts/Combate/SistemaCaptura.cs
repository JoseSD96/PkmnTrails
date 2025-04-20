using UnityEngine;
using DG.Tweening;
using System.Collections;

public class SistemaCaptura : MonoBehaviour
{

    [SerializeField] private GameObject pokeballObj;
    [SerializeField] private GameObject PkmnEnemigo;
    [SerializeField] private Equipo equipo;
    [SerializeField] Pokeball pokeball;
    private Sprite[] spritesPokeballLanzamiento;
    private Sprite[] spritesPokeballToque;
    private Sprite[] spritesPokeballCaptura;

    public bool IsCapturando { get; private set; }

    private int toques = 0;

    public void resetPokeball()
    {
        pokeballObj.SetActive(false);
        pokeballObj.transform.localPosition = new Vector3(40.4f, -67.2f, -3680f);
    }
    public void LanzarPokeball()
    {
        pokeballObj.transform.localPosition = new Vector3(40.4f, -67.2f, -3680f);
        pokeballObj.SetActive(true);
        StartCoroutine(ProcesoCaptura());
    }

    private IEnumerator ProcesoCaptura()
    {
        IsCapturando = true;
        CargarPokeball();

        yield return AnimacionLanzarPokeballCoroutine();

        CalcularProbabilidadCaptura(PkmnEnemigo.GetComponent<PkmnCombate>().Pkmn);

        if (toques > 0)
        {
            yield return AnimacionDarToquesCoroutine();
            if (toques == 3)
            {
                yield return AnimacionCapturaCoroutine();
            }
            else
            {
                yield return AnimacionRecuperarPokemonCoroutine();
                pokeballObj.SetActive(false);
            }
        }
        else
        {
            yield return AnimacionRecuperarPokemonCoroutine();
            pokeballObj.SetActive(false);
        }
        IsCapturando = false;
    }

    private IEnumerator AnimacionLanzarPokeballCoroutine()
    {
        pokeballObj.SetActive(true);

        var secuencia = DOTween.Sequence();

        secuencia.AppendCallback(() =>
        {
            pokeballObj.GetComponent<SpriteRenderer>().sprite = spritesPokeballLanzamiento[1];
        });

        secuencia.Append(pokeballObj.transform.DOLocalMoveX(32.4f, 0.1f));
        secuencia.Join(pokeballObj.transform.DOLocalMoveY(-37.1f, 0.1f));

        secuencia.AppendCallback(() =>
        {
            pokeballObj.GetComponent<SpriteRenderer>().sprite = spritesPokeballLanzamiento[2];
        });

        secuencia.Append(pokeballObj.transform.DOLocalMoveX(21.6f, 0.1f));
        secuencia.Join(pokeballObj.transform.DOLocalMoveY(-9.8f, 0.1f));

        secuencia.AppendCallback(() =>
        {
            pokeballObj.GetComponent<SpriteRenderer>().sprite = spritesPokeballLanzamiento[3];
        });

        secuencia.Append(pokeballObj.transform.DOLocalMoveX(9.1f, 0.1f));
        secuencia.Join(pokeballObj.transform.DOLocalMoveY(19.8f, 0.1f));

        secuencia.AppendCallback(() =>
        {
            pokeballObj.GetComponent<SpriteRenderer>().sprite = spritesPokeballLanzamiento[4];
        });

        secuencia.Append(pokeballObj.transform.DOLocalMoveX(0f, 0.1f));
        secuencia.Join(pokeballObj.transform.DOLocalMoveY(43.6f, 0.1f));

        secuencia.AppendCallback(() =>
        {
            pokeballObj.GetComponent<SpriteRenderer>().sprite = spritesPokeballLanzamiento[5];
        });

        secuencia.Append(pokeballObj.transform.DOLocalMoveX(0f, 0.1f));
        secuencia.Join(pokeballObj.transform.DOLocalMoveY(72.6f, 0.1f));
        secuencia.Join(PkmnEnemigo.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f));
        secuencia.AppendCallback(() =>
        {
            pokeballObj.GetComponent<SpriteRenderer>().sprite = spritesPokeballLanzamiento[6];
        });

        secuencia.Append(pokeballObj.transform.DOLocalMoveX(0f, 0.1f));
        secuencia.Join(pokeballObj.transform.DOLocalMoveY(20f, 0.1f));

        yield return secuencia.WaitForCompletion();
    }

    private IEnumerator AnimacionDarToquesCoroutine()
    {
        var secuencia = DOTween.Sequence();
        if (toques > 0)
        {
            for (int i = 0; i < toques; i++)
            {

                secuencia.AppendCallback(() =>
                {
                    pokeballObj.GetComponent<SpriteRenderer>().sprite = spritesPokeballToque[0];
                });
                secuencia.Append(pokeballObj.transform.DOLocalMoveY(0f, 0.1f));
                secuencia.Join(pokeballObj.transform.DOLocalMoveY(0f, 0.1f));
                secuencia.AppendCallback(() =>
                {
                    pokeballObj.GetComponent<SpriteRenderer>().sprite = spritesPokeballToque[1];
                });
                secuencia.Append(pokeballObj.transform.DOLocalMoveY(0f, 0.1f));
                secuencia.Join(pokeballObj.transform.DOLocalMoveY(0f, 0.1f));
                secuencia.AppendCallback(() =>
                {
                    pokeballObj.GetComponent<SpriteRenderer>().sprite = spritesPokeballToque[2];
                });
                secuencia.Append(pokeballObj.transform.DOLocalMoveY(0f, 0.1f));
                secuencia.Join(pokeballObj.transform.DOLocalMoveY(0f, 0.1f));
                secuencia.AppendCallback(() =>
                {
                    pokeballObj.GetComponent<SpriteRenderer>().sprite = spritesPokeballToque[3];
                });
                secuencia.Append(pokeballObj.transform.DOLocalMoveY(0f, 0.1f));
                secuencia.Join(pokeballObj.transform.DOLocalMoveY(0f, 0.1f));
                secuencia.AppendCallback(() =>
                {
                    pokeballObj.GetComponent<SpriteRenderer>().sprite = spritesPokeballToque[4];
                });
            }

            yield return secuencia.WaitForCompletion();
        }
    }

    private IEnumerator AnimacionRecuperarPokemonCoroutine()
    {
        var secuencia = DOTween.Sequence();

        // Cambia el sprite de la pokeball antes de desaparecer
        secuencia.AppendCallback(() =>
        {
            pokeballObj.GetComponent<SpriteRenderer>().sprite = spritesPokeballLanzamiento[5];
        });

        // Opcional: puedes hacer un pequeño fade de la pokeball antes de desactivarla
        secuencia.Append(pokeballObj.GetComponent<SpriteRenderer>().DOFade(0f, 0.3f));

        // Desactiva la pokeball después del fade
        secuencia.AppendCallback(() =>
        {
            pokeballObj.SetActive(false);
            // Restaura la opacidad para el siguiente uso
            pokeballObj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        });

        // Ahora hace el fade del Pokémon enemigo
        secuencia.Append(PkmnEnemigo.GetComponent<SpriteRenderer>().DOFade(1f, 0.5f));

        yield return secuencia.WaitForCompletion();
    }

    private IEnumerator AnimacionCapturaCoroutine()
    {
        var secuencia = DOTween.Sequence();

        for (int i = 0; i < spritesPokeballCaptura.Length; i++)
        {
            int index = i;
            secuencia.AppendCallback(() =>
            {
                pokeballObj.GetComponent<SpriteRenderer>().sprite = spritesPokeballCaptura[index];
            });
            secuencia.AppendInterval(0.08f);
        }

        yield return secuencia.WaitForCompletion();
    }

    public void CargarPokeball()
    {
        spritesPokeballLanzamiento = pokeball.SpritesLanzamiento;
        spritesPokeballToque = pokeball.SpritesToque;
        spritesPokeballCaptura = pokeball.SpritesCaptura;
    }

    public float CargarProbabilidadPokeball()
    {
        if (equipo.GetMediaNivel() < 20)
        {
            return 1f;
        }
        else if (equipo.GetMediaNivel() < 40)
        {
            return 1.5f;
        }
        else
        {
            return 2f;
        }
    }

    public void CalcularProbabilidadCaptura(Pokemon pkmnACapturar)
    {
        int maxHP = pkmnACapturar.MaxHP;
        int currentHP = pkmnACapturar.HP;
        int rate = pkmnACapturar.Base.RatioCaptura;

        int a = (int)(((3 * maxHP - 2 * currentHP) * rate * CargarProbabilidadPokeball()) / (3.0 * maxHP));

        double b = 0;
        if (a > 0)
        {
            b = 1048560 / Mathf.Sqrt(Mathf.Sqrt((float)(16711680.0 / a)));
            b = Mathf.Clamp((float)b, 0, 65535);
        }

        int temblores = 0;

        for (int i = 0; i < 3; i++)
        {
            int r = Random.Range(0, 65536);
            if (r <= b)
            {
                temblores++;
            }
            else
            {
                break;
            }
        }
        Debug.Log("Temblores: " + temblores);
        toques = temblores;
    }

    public bool ExitoCaptura()
    {
        if (toques == 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}



