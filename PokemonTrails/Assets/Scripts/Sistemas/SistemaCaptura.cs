using UnityEngine;
using DG.Tweening;
using System.Collections;

public class SistemaCaptura : MonoBehaviour
{

    [SerializeField] private GameObject pokeballObj;
    [SerializeField] private GameObject PkmnEnemigo;
    [SerializeField] private Equipo equipo;
    [SerializeField] Pokeball pokeball;
    [SerializeField] private AudioManager audioManager;
    private Sprite[] spritesPokeballLanzamiento;
    private Sprite[] spritesPokeballToque;
    private Sprite[] spritesPokeballCaptura;

    public bool IsCapturando { get; private set; }

    private int toques = 0;

    /// <summary>
    /// Inicializa la pokeball y carga los sprites necesarios al iniciar la escena.
    /// </summary>
    private void Start()
    {
        pokeballObj.SetActive(false);
        CargarSpritesPokeball();
        CargarPokeball();
    }

    /// <summary>
    /// Resetea la pokeball a su posición y estado inicial.
    /// </summary>
    public void resetPokeball()
    {
        pokeballObj.SetActive(false);
        pokeballObj.transform.localPosition = new Vector3(40.4f, -67.2f, -3680f);
    }

    /// <summary>
    /// Inicia el proceso de lanzamiento de la pokeball y la animación de captura.
    /// </summary>
    public void LanzarPokeball()
    {
        pokeballObj.transform.localPosition = new Vector3(40.4f, -67.2f, -3680f);
        pokeballObj.SetActive(true);
        StartCoroutine(ProcesoCaptura());
    }

    /// <summary>
    /// Corrutina principal que gestiona todo el proceso de captura: lanzamiento, animaciones, cálculo de probabilidad y resultado.
    /// </summary>
    private IEnumerator ProcesoCaptura()
    {
        IsCapturando = true;
        CargarPokeball();
        CargarSpritesPokeball();

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

    /// <summary>
    /// Corrutina que anima el lanzamiento de la pokeball hacia el Pokémon enemigo.
    /// </summary>
    private IEnumerator AnimacionLanzarPokeballCoroutine()
    {
        pokeballObj.SetActive(true);
        audioManager.PlayEfecto("Captura", "vueloPokeball");
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
        secuencia.Join(pokeballObj.transform.DOLocalMoveY(29.6f, 0.1f));

        yield return secuencia.WaitForCompletion();
    }

    /// <summary>
    /// Corrutina que anima los "toques" de la pokeball tras el lanzamiento, según la probabilidad de captura.
    /// </summary>
    private IEnumerator AnimacionDarToquesCoroutine()
    {
        var secuencia = DOTween.Sequence();
        if (toques > 0)
        {
            for (int i = 0; i < toques; i++)
            {
                secuencia.AppendCallback(() =>
                {
                    audioManager.PlayEfecto("Captura", "pokeToque");
                    pokeballObj.GetComponent<SpriteRenderer>().sprite = spritesPokeballToque[0];
                });
                secuencia.Append(pokeballObj.transform.DOLocalMoveY(29.6f, 0.2f));
                secuencia.Join(pokeballObj.transform.DOLocalMoveY(29.6f, 0.2f));
                secuencia.AppendCallback(() =>
                {
                    pokeballObj.GetComponent<SpriteRenderer>().sprite = spritesPokeballToque[1];
                });
                secuencia.Append(pokeballObj.transform.DOLocalMoveY(29.6f, 0.2f));
                secuencia.Join(pokeballObj.transform.DOLocalMoveY(29.6f, 0.2f));
                secuencia.AppendCallback(() =>
                {
                    pokeballObj.GetComponent<SpriteRenderer>().sprite = spritesPokeballToque[2];
                });
                secuencia.Append(pokeballObj.transform.DOLocalMoveY(29.6f, 0.2f));
                secuencia.Join(pokeballObj.transform.DOLocalMoveY(29.6f, 0.2f));
                secuencia.AppendCallback(() =>
                {
                    pokeballObj.GetComponent<SpriteRenderer>().sprite = spritesPokeballToque[3];
                });
                secuencia.Append(pokeballObj.transform.DOLocalMoveY(29.6f, 0.2f));
                secuencia.Join(pokeballObj.transform.DOLocalMoveY(29.6f, 0.2f));
                secuencia.AppendCallback(() =>
                {
                    pokeballObj.GetComponent<SpriteRenderer>().sprite = spritesPokeballToque[4];
                });

            }

            yield return secuencia.WaitForCompletion();
        }
    }

    /// <summary>
    /// Corrutina que anima la recuperación del Pokémon si la captura falla.
    /// </summary>
    private IEnumerator AnimacionRecuperarPokemonCoroutine()
    {
        audioManager.PlayEfecto("Captura", "escapePokeball");
        var secuencia = DOTween.Sequence();

        secuencia.AppendCallback(() =>
        {
            pokeballObj.GetComponent<SpriteRenderer>().sprite = spritesPokeballLanzamiento[5];
        });

        secuencia.Append(pokeballObj.GetComponent<SpriteRenderer>().DOFade(0f, 0.3f));

        secuencia.AppendCallback(() =>
        {
            pokeballObj.SetActive(false);
            pokeballObj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        });

        secuencia.Append(PkmnEnemigo.GetComponent<SpriteRenderer>().DOFade(1f, 0.5f));

        yield return secuencia.WaitForCompletion();
    }

    /// <summary>
    /// Corrutina que anima la captura exitosa del Pokémon.
    /// </summary>
    private IEnumerator AnimacionCapturaCoroutine()
    {
        audioManager.PlayEfecto("Captura", "captura");
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

    /// <summary>
    /// Carga los sprites de la pokeball para las diferentes fases de la animación.
    /// </summary>
    public void CargarSpritesPokeball()
    {
        spritesPokeballLanzamiento = pokeball.SpritesLanzamiento;
        spritesPokeballToque = pokeball.SpritesToque;
        spritesPokeballCaptura = pokeball.SpritesCaptura;
    }

    /// <summary>
    /// Carga la pokeball adecuada según el nivel medio del equipo (PokeBall, SuperBall o UltraBall).
    /// </summary>
    public void CargarPokeball()
    {
        if (equipo.GetMediaNivel() < 20)
        {
            pokeball = Resources.Load<Pokeball>("Pokeballs/PokeBall");
        }
        else if (equipo.GetMediaNivel() < 40)
        {
            pokeball = Resources.Load<Pokeball>("Pokeballs/SuperBall");
        }
        else
        {
            pokeball = Resources.Load<Pokeball>("Pokeballs/UltraBall");
        }
    }

    /// <summary>
    /// Calcula la probabilidad de captura del Pokémon enemigo y determina el número de "toques" de la pokeball.
    /// </summary>
    /// <param name="pkmnACapturar">Pokémon que se intenta capturar.</param>
    public void CalcularProbabilidadCaptura(Pokemon pkmnACapturar)
    {
        int maxHP = pkmnACapturar.MaxHP;
        int currentHP = pkmnACapturar.HP;
        int rate = pkmnACapturar.Base.RatioCaptura;

        int a = (int)(((3 * maxHP - 2 * currentHP) * rate * pokeball.RatioCaptura) / (3.0 * maxHP));

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

    /// <summary>
    /// Devuelve true si la captura ha sido exitosa (tres toques), false en caso contrario.
    /// </summary>
    /// <returns>True si la captura tiene éxito, false si falla.</returns>
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



