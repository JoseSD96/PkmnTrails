using System.Linq;
using UnityEngine;

/// <summary>
/// Controla la reproducción de música y efectos de sonido en el juego.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public AudioSource musicaSource;
    public AudioSource efectosSource;

    /// <summary>
    /// Reproduce una música aleatoria de exploración.
    /// Busca todos los clips en la carpeta "Sonido/Musica/Exploracion" y selecciona uno al azar.
    /// </summary>
    public void PlayMusicaExploracion()
    {
        AudioClip[] audios = Resources.LoadAll<AudioClip>("Sonido/Musica/Exploracion");
        if (audios.Length > 0)
        {
            musicaSource.clip = audios[Random.Range(0, audios.Length)];
            musicaSource.loop = true;
            musicaSource.Play();
        }
    }

    /// <summary>
    /// Reproduce la música de combate normal.
    /// Carga el clip "salvaje" de la carpeta "Sonido/Musica/CombateNormal".
    /// </summary>
    public void PlayMusicaCombateNormal()
    {
        AudioClip audio = Resources.Load<AudioClip>("Sonido/Musica/CombateNormal/salvaje");
        if (audio != null)
        {
            musicaSource.clip = audio;
            musicaSource.loop = true;
            musicaSource.Play();
        }
    }

    /// <summary>
    /// Reproduce la música del PC.
    /// Carga el clip "PC" de la carpeta "Sonido/Musica/PC".
    /// </summary>
    public void PlayMusicaPC()
    {
        AudioClip audio = Resources.Load<AudioClip>("Sonido/Musica/PC/PC");
        if (audio != null)
        {
            musicaSource.clip = audio;
            musicaSource.loop = true;
            musicaSource.Play();
        }
    }

    /// <summary>
    /// Reproduce la música de combate contra un Pokémon legendario.
    /// Carga el clip "legendario" de la carpeta "Sonido/Musica/CombateContraLegendario".
    /// </summary>
    public void PlayMusicaCombateLegendario()
    {
        AudioClip audio = Resources.Load<AudioClip>("Sonido/Musica/CombateContraLegendario/legendario");
        if (audio != null)
        {
            musicaSource.clip = audio;
            musicaSource.loop = true;
            musicaSource.Play();
        }
    }

    /// <summary>
    /// Reproduce la música del menú de inicio.
    /// Carga el clip "Menu" de la carpeta "Sonido/Musica/MenuInicio".
    /// </summary>
    public void PlayMusicaMenuInicio()
    {
        AudioClip audio = Resources.Load<AudioClip>("Sonido/Musica/MenuInicio/Menu");
        if (audio != null)
        {
            musicaSource.clip = audio;
            musicaSource.loop = true;
            musicaSource.Play();
        }
    }

    /// <summary>
    /// Reproduce un efecto de sonido específico.
    /// Busca el clip en la ruta "Sonido/Efectos/{carpeta}/{nombreEfecto}" y lo reproduce una vez.
    /// </summary>
    /// <param name="carpeta">Nombre de la carpeta donde está el efecto.</param>
    /// <param name="nombreEfecto">Nombre del archivo de audio del efecto.</param>
    public void PlayEfecto(string carpeta, string nombreEfecto)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Sonido/Efectos/{carpeta}/{nombreEfecto}");
        if (clip != null)
        {
            efectosSource.PlayOneShot(clip);
        }
    }
}