using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarruselImagenes : MonoBehaviour
{
    public RectTransform[] imagenes;
    public float duracionAnimacion;

    private ZonaBase[] zonasDisponibles;
    private List<Sprite> spritesZonasDisponibles = new List<Sprite>();

    private ZonaBase[] zonasLegendariasDisponibles;
    private List<Sprite> spriteZonasLegendariasDisponibles = new List<Sprite>();

    public ZonaBase zonaInicial;
    private int indiceActual;
    private float desplazamiento;

    private ZonaBase zonaActual;

    private bool isCambiando = false;
    private bool ultimaFueLegendaria = false;

    /// <summary>
    /// Inicializa el carrusel de imágenes al iniciar la escena.
    /// Ajusta los pivotes y anclas de las imágenes, carga las zonas y asigna la imagen inicial.
    /// </summary>
    void Start()
    {
        foreach (var img in imagenes)
        {
            img.anchorMin = new Vector2(0.5f, 0.5f);
            img.anchorMax = new Vector2(0.5f, 0.5f);
            img.pivot = new Vector2(0.5f, 0.5f);
        }

        CargarImagenes();
        Canvas.ForceUpdateCanvases();
        desplazamiento = imagenes[0].rect.width - 0.4f;
        AsignarImagenesIniciales();

        zonaActual = zonaInicial;
        ZonaManager.Instance.SetZonaActual(zonaActual);
    }

    /// <summary>
    /// Carga las zonas normales y legendarias desde los recursos y almacena sus sprites.
    /// </summary>
    void CargarImagenes()
    {
        zonasDisponibles = Resources.LoadAll<ZonaBase>("Zonas");
        foreach (var zona in zonasDisponibles)
        {
            spritesZonasDisponibles.Add(zona.SpriteZona);
        }

        zonasLegendariasDisponibles = Resources.LoadAll<ZonaBase>("ZonasLegendarias");
        foreach (var zona in zonasLegendariasDisponibles)
        {
            spriteZonasLegendariasDisponibles.Add(zona.SpriteZona);
        }
    }

    /// <summary>
    /// Asigna las imágenes iniciales al carrusel, colocando la zona inicial y una aleatoria.
    /// </summary>
    void AsignarImagenesIniciales()
    {
        imagenes[0].GetComponent<Image>().sprite = zonaInicial.SpriteZona;
        imagenes[0].anchoredPosition = Vector2.zero;

        int indiceAleatorio = Random.Range(0, spritesZonasDisponibles.Count);
        imagenes[1].GetComponent<Image>().sprite = spritesZonasDisponibles[indiceAleatorio];

        imagenes[1].anchoredPosition = new Vector2(desplazamiento, 0);

        indiceActual = 0;
    }

    /// <summary>
    /// Avanza el carrusel a la siguiente zona, eligiendo aleatoriamente si es legendaria o normal.
    /// Lanza la animación de transición y actualiza la zona actual.
    /// </summary>
    /// <param name="onFinish">Acción a ejecutar al terminar la animación.</param>
    public void AvanzarCarrusel(System.Action onFinish)
    {
        if (!isCambiando)
        {
            isCambiando = true;
            int siguienteIndice = (indiceActual + 1) % 2;

            ZonaBase zonaSiguiente;
            bool esLegendaria = false;

            if (!ultimaFueLegendaria && Random.Range(0, 101) < 2)
            {
                zonaSiguiente = zonasLegendariasDisponibles[Random.Range(0, zonasLegendariasDisponibles.Length)];
                esLegendaria = true;
            }
            else
            {
                zonaSiguiente = zonasDisponibles[Random.Range(0, zonasDisponibles.Length)];
            }

            ultimaFueLegendaria = esLegendaria;
            zonaActual = zonaSiguiente;
            ZonaManager.Instance.SetZonaActual(zonaActual);

            imagenes[siguienteIndice].GetComponent<Image>().sprite = zonaSiguiente.SpriteZona;
            imagenes[siguienteIndice].anchoredPosition = new Vector2(desplazamiento, 0);

            StartCoroutine(AnimarCarrusel(imagenes[indiceActual], imagenes[siguienteIndice], onFinish));

            indiceActual = siguienteIndice;
        }
    }

    /// <summary>
    /// Corrutina que anima la transición de las imágenes del carrusel.
    /// Mueve la imagen actual hacia la izquierda y la nueva al centro.
    /// </summary>
    /// <param name="imagenActual">RectTransform de la imagen actual.</param>
    /// <param name="imagenNueva">RectTransform de la nueva imagen.</param>
    /// <param name="onFinish">Acción a ejecutar al terminar la animación.</param>
    IEnumerator AnimarCarrusel(RectTransform imagenActual, RectTransform imagenNueva, System.Action onFinish)
    {
        float tiempo = 0f;
        Vector2 inicioActual = imagenActual.anchoredPosition;
        Vector2 finActual = new Vector2(-desplazamiento, 0);
        Vector2 inicioNueva = imagenNueva.anchoredPosition;
        Vector2 finNueva = Vector2.zero;

        while (tiempo < duracionAnimacion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracionAnimacion;
            imagenActual.anchoredPosition = Vector2.Lerp(inicioActual, finActual, t);
            imagenNueva.anchoredPosition = Vector2.Lerp(inicioNueva, finNueva, t);
            yield return null;
        }

        imagenActual.anchoredPosition = new Vector2(desplazamiento, 0);
        isCambiando = false;
        onFinish?.Invoke();
    }
}