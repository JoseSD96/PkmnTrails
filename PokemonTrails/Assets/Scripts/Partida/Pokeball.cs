using UnityEngine;

[CreateAssetMenu(fileName = "Pokeball", menuName = "Pokeball/Crear Nueva Pokeball")]
public class Pokeball : ScriptableObject
{
    [SerializeField] Sprite[] spritesLanzamiento;
    [SerializeField] Sprite[] spritesToque;
    [SerializeField] Sprite[] spritesCaptura;
    [SerializeField] float ratioCaptura;

    public Sprite[] SpritesLanzamiento => spritesLanzamiento;
    public Sprite[] SpritesToque => spritesToque;
    public Sprite[] SpritesCaptura => spritesCaptura;
    public float RatioCaptura => ratioCaptura;
}