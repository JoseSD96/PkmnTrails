using UnityEngine;
using DG.Tweening;

public class PkmnCombate : MonoBehaviour
{
    [SerializeField] public bool isEnemy;
    [SerializeField] AudioManager audioManager;
    public GameObject objetoPkmn;

    public Pokemon Pkmn { get; set; }
    Vector3 PosInicial;
    Color colorInicial;
    private void Awake()
    {
        PosInicial = objetoPkmn.transform.localPosition;
        colorInicial = objetoPkmn.GetComponent<SpriteRenderer>().color;
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
            secuencia.Append(objetoPkmn.GetComponent<SpriteRenderer>().transform.DOLocalMoveY(PosInicial.y + 5f, 0.25f));
        }
        else
        {
            secuencia.Append(objetoPkmn.GetComponent<SpriteRenderer>().transform.DOLocalMoveY(PosInicial.y - 5f, 0.25f));
        }

        secuencia.Append(objetoPkmn.GetComponent<SpriteRenderer>().transform.DOLocalMoveY(PosInicial.y, 0.25f));
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
    }

    public void ReiniciarEstado()
    {
        objetoPkmn.GetComponent<SpriteRenderer>().DOFade(1f, 0.5f);

        objetoPkmn.transform.localPosition = PosInicial;

        objetoPkmn.GetComponent<SpriteRenderer>().color = colorInicial;

    }
}
