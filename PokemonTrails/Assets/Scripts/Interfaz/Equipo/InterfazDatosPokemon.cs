using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InterfazDatosPokemon : MonoBehaviour
{
    [SerializeField] SpriteRenderer spritePokemon;
    [SerializeField] SpriteRenderer shinyStars;
    [SerializeField] TextMeshProUGUI numeroTxt;
    [SerializeField] TextMeshProUGUI nombreTxt;
    [SerializeField] TextMeshProUGUI nivelTxt;
    [SerializeField] TextMeshProUGUI potencialTxt;
    [SerializeField] SpriteRenderer tipo1Img;
    [SerializeField] SpriteRenderer tipo2Img;

    /// <summary>
    /// Muestra los datos del Pokémon recibido por parámetro en la interfaz.
    /// Actualiza el sprite, estrellas shiny, número, nombre, nivel, potencial y tipos.
    /// </summary>
    /// <param name="pokemon">Pokémon cuyos datos se van a mostrar.</param>
    public void MostrarDatos(Pokemon pokemon)
    {
        spritePokemon.sprite = pokemon.isShiny ? pokemon.Base.SpriteIdleShiny : pokemon.Base.SpriteIdle;
        shinyStars.gameObject.SetActive(pokemon.isShiny);

        numeroTxt.text = $"N. {pokemon.Base.Num.ToString("D3")}";
        nombreTxt.text = pokemon.Base.Nombre;
        nivelTxt.text = $"Nvl. {pokemon.Nivel}";
        potencialTxt.text = $"{pokemon.Potencial}/31";

        tipo1Img.sprite = pokemon.Base.SpriteTipo1;
        tipo2Img.sprite = pokemon.Base.SpriteTipo2;
        tipo2Img.gameObject.SetActive(pokemon.Base.Tipo2 != Type.None);
    }

}