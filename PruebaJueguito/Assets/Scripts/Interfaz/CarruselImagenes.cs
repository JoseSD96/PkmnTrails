using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarruselImagenes : MonoBehaviour
{
    public RectTransform[] imagenes;
    public float duracionAnimacion;
    private List<Sprite> imagenesDisponibles;
    public Sprite zonaInicial;
    private int indiceActual;
    private float desplazamiento;

    private bool isCambiando = false;

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
    }

    void CargarImagenes()
    {
        imagenesDisponibles = new List<Sprite>(Resources.LoadAll<Sprite>("Zonas"));
    }

    void AsignarImagenesIniciales()
    {
        if (imagenesDisponibles.Count < 2) return;

        imagenes[0].GetComponent<Image>().sprite = zonaInicial;
        imagenes[0].anchoredPosition = Vector2.zero;

        int indiceAleatorio = Random.Range(0, imagenesDisponibles.Count);
        imagenes[1].GetComponent<Image>().sprite = imagenesDisponibles[indiceAleatorio];

        imagenes[1].anchoredPosition = new Vector2(desplazamiento, 0);

        indiceActual = 0;
    }

    public void AvanzarCarrusel(System.Action onFinish)
    {
        if (!isCambiando)
        {
            isCambiando = true;
            int siguienteIndice = (indiceActual + 1) % 2;

            imagenes[siguienteIndice].GetComponent<Image>().sprite = imagenesDisponibles[Random.Range(0, imagenesDisponibles.Count)];
            imagenes[siguienteIndice].anchoredPosition = new Vector2(desplazamiento, 0);

            StartCoroutine(AnimarCarrusel(imagenes[indiceActual], imagenes[siguienteIndice], onFinish));

            indiceActual = siguienteIndice;
        }
    }

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