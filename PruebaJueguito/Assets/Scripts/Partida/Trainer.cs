using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Trainer", menuName = "Trainer/Crear Nuevo Trainer")]
public class Trainer : ScriptableObject
{
    [SerializeField] string nombre;
    [SerializeField] Sprite spriteIdle;
    [SerializeField] Sprite spriteExploracion;
    [SerializeField] Sprite[] spritesAndar = new Sprite[4];

    public string Nombre => nombre;
    public Sprite SpriteIdle => spriteIdle;
    public Sprite SpriteExploracion => spriteExploracion;
    public Sprite[] SpritesAndar => spritesAndar;
}