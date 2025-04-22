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

    private void Awake()
    {
        PosInicial = objetoPkmn.transform.localPosition;
        colorInicial = objetoPkmn.GetComponent<SpriteRenderer>().color;

        // Asegura que la sombra sea hija del Pokémon y esté en la posición correcta
        shadow.transform.SetParent(objetoPkmn.transform);
        shadow.transform.localPosition = sombraOffset;
    }

    public void Setup(Pokemon pkmn)
    {
        Pkmn = pkmn;
        objetoPkmn.GetComponent<SpriteRenderer>().sprite = GetSprite();
        AnimacionEntrada();
    }

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

        // La sombra siempre sigue al Pokémon, no hace falta animarla por separado

        if (Pkmn.isShiny)
        {
            audioManager.PlayEfecto("Combate", "shiny");
        }
    }

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
        // La sombra sigue al Pokémon automáticamente
    }

    public void AnimacionGolpe()
    {
        var secuencia = DOTween.Sequence();
        secuencia.Append(objetoPkmn.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.1f));
        secuencia.Append(objetoPkmn.GetComponent<SpriteRenderer>().DOColor(colorInicial, 0.1f));
    }

    public void AnimacionDerrota()
    {
        audioManager.PlayEfecto("Combate", "pkmnDerrotado");
        var secuencia = DOTween.Sequence();
        secuencia.Append(objetoPkmn.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f));
        secuencia.Join(shadow.DOFade(0f, 0.5f));
    }

    public void ReiniciarEstado()
    {
        objetoPkmn.GetComponent<SpriteRenderer>().DOFade(1f, 0.5f);
        shadow.DOFade(1f, 0.5f);

        objetoPkmn.transform.localPosition = PosInicial;
        // La sombra se mantiene en sombraOffset automáticamente

        objetoPkmn.GetComponent<SpriteRenderer>().color = colorInicial;

    }
}
