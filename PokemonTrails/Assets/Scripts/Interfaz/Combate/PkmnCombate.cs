using UnityEngine;
using DG.Tweening;

public class PkmnCombate : MonoBehaviour
{
    [SerializeField] public bool isEnemy;
    [SerializeField] AudioManager audioManager;
    public GameObject objetoPkmn;
    public SpriteRenderer shadow;

    public Pokemon Pkmn { get; set; }
    Vector3 PosInicial;
    Color colorInicial;
    private readonly Vector3 sombraOffset = new Vector3(0f, -0.25f, 0.1f);

    /// <summary>
    /// Inicializa la posición y color inicial del Pokémon, y coloca la sombra en la posición correcta como hija del objeto.
    /// </summary>
    private void Awake()
    {
        PosInicial = objetoPkmn.transform.localPosition;
        colorInicial = objetoPkmn.GetComponent<SpriteRenderer>().color;

        shadow.transform.SetParent(objetoPkmn.transform);
        shadow.transform.localPosition = sombraOffset;
    }

    /// <summary>
    /// Configura el Pokémon a mostrar en combate, asigna el sprite correspondiente y lanza la animación de entrada.
    /// </summary>
    /// <param name="pkmn">Instancia de Pokémon a mostrar.</param>
    public void Setup(Pokemon pkmn)
    {
        Pkmn = pkmn;
        objetoPkmn.GetComponent<SpriteRenderer>().sprite = GetSprite();
        AnimacionEntrada();
    }

    /// <summary>
    /// Devuelve el sprite adecuado según si el Pokémon es enemigo y si es shiny.
    /// </summary>
    /// <returns>Sprite correspondiente al estado del Pokémon.</returns>
    public Sprite GetSprite()
    {
        if (isEnemy)
        {
            if (Pkmn.isShiny)
            {
                return Pkmn.Base.SpriteIdleShiny;
            }
            else
            {
                return Pkmn.Base.SpriteIdle;
            }
        }
        else
        {
            if (Pkmn.isShiny)
            {
                return Pkmn.Base.SpriteExploracionShiny;
            }
            else
            {
                return Pkmn.Base.SpriteExploracion;
            }
        }
    }

    /// <summary>
    /// Realiza la animación de entrada del Pokémon en combate, desplazándolo desde fuera de la pantalla.
    /// Si es shiny, reproduce el efecto de sonido correspondiente.
    /// </summary>
    public void AnimacionEntrada()
    {
        if (!isEnemy)
        {
            objetoPkmn.transform.localPosition = new Vector3(PosInicial.x, -256f);
        }
        else
        {
            objetoPkmn.transform.localPosition = new Vector3(PosInicial.x, 256f);
        }

        objetoPkmn.transform.DOLocalMoveY(PosInicial.y, 2f);


        if (Pkmn.isShiny)
        {
            audioManager.PlayEfecto("Combate", "shiny");
        }
    }

    /// <summary>
    /// Realiza la animación de ataque, moviendo el Pokémon hacia adelante y luego regresando a su posición inicial.
    /// </summary>
    public void AnimacionAtaque()
    {
        var secuencia = DOTween.Sequence();
        if (!isEnemy)
        {
            secuencia.Append(objetoPkmn.transform.DOLocalMoveY(PosInicial.y + 5f, 0.25f));
        }
        else
        {
            secuencia.Append(objetoPkmn.transform.DOLocalMoveY(PosInicial.y - 5f, 0.25f));
        }
        secuencia.Append(objetoPkmn.transform.DOLocalMoveY(PosInicial.y, 0.25f));
    }

    /// <summary>
    /// Realiza la animación de recibir un golpe, cambiando el color del sprite a rojo y luego restaurándolo.
    /// </summary>
    public void AnimacionGolpe()
    {
        var secuencia = DOTween.Sequence();
        secuencia.Append(objetoPkmn.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.1f));
        secuencia.Append(objetoPkmn.GetComponent<SpriteRenderer>().DOColor(colorInicial, 0.1f));
    }

    /// <summary>
    /// Realiza la animación de derrota, desvaneciendo el sprite y la sombra, y reproduce el sonido de derrota.
    /// </summary>
    public void AnimacionDerrota()
    {
        audioManager.PlayEfecto("Combate", "pkmnDerrotado");
        var secuencia = DOTween.Sequence();
        secuencia.Append(objetoPkmn.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f));
        secuencia.Join(shadow.DOFade(0f, 0.5f));
    }

    /// <summary>
    /// Restaura el estado visual del Pokémon tras una derrota o cambio, devolviéndolo a su posición y color inicial.
    /// </summary>
    public void ReiniciarEstado()
    {
        objetoPkmn.GetComponent<SpriteRenderer>().DOFade(1f, 0.5f);
        shadow.DOFade(1f, 0.5f);

        objetoPkmn.transform.localPosition = PosInicial;

        objetoPkmn.GetComponent<SpriteRenderer>().color = colorInicial;

    }
}
