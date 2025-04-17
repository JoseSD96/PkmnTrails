using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InterfazEquipo : MonoBehaviour
{
    [SerializeField] Equipo equipo;
    [SerializeField] GameObject[] botones;

    void Start()
    {
        ActualizarInterfaz();
    }

    void Update()
    {
        ActualizarInterfaz();
    }

    public void ActualizarInterfaz()
    {
        for (int i = 0; i < botones.Length; i++)
        {

            SpriteRenderer spritePokemon = null;
            TextMeshProUGUI nombreTxt = null;
            TextMeshProUGUI nivelTxt = null;

            var sprite = botones[i].GetComponentsInChildren<SpriteRenderer>(true);
            foreach (var img in sprite)
            {
                if (img.gameObject != botones[i])
                {
                    spritePokemon = img;
                    break;
                }
            }
            HPBar hpBar = botones[i].GetComponentInChildren<HPBar>(true);
            var textos = botones[i].GetComponentsInChildren<TextMeshProUGUI>(true);
            if (textos.Length >= 2)
            {
                nombreTxt = textos[0];
                nivelTxt = textos[1];
            }


            if (i < equipo.Pokemones.Count && equipo.Pokemones[i] != null)
            {
                var pokemon = equipo.Pokemones[i];

                spritePokemon.sprite = pokemon.isShiny ? pokemon.Base.SpriteIdleShiny : pokemon.Base.SpriteIdle;
                spritePokemon.enabled = true;

                hpBar.gameObject.SetActive(true);
                hpBar.SetMaxHP(pokemon.MaxHP);
                hpBar.setHP(pokemon.HP);

                nombreTxt.text = pokemon.Base.Nombre;
                nombreTxt.gameObject.SetActive(true);
                nivelTxt.text = "Nv. " + pokemon.Nivel;
                nivelTxt.gameObject.SetActive(true);
            }
            else
            {
                spritePokemon.enabled = false;
                hpBar.gameObject.SetActive(false);
                nombreTxt.gameObject.SetActive(false);
                nivelTxt.gameObject.SetActive(false);
            }
        }
    }
}
