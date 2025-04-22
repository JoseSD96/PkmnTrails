using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicaSource;
    public AudioSource efectosSource;

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

    public void PlayEfecto(string carpeta, string nombreEfecto)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Sonido/Efectos/{carpeta}/{nombreEfecto}");
        if (clip != null)
        {
            efectosSource.PlayOneShot(clip);
        }
    }
}