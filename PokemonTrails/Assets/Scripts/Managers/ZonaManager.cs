using UnityEngine;

public class ZonaManager : MonoBehaviour
{
    public static ZonaManager Instance { get; private set; }
    public ZonaBase ZonaActual { get; private set; }

    /// <summary>
    /// Se ejecuta al despertar el objeto. Implementa el patrón Singleton para asegurar que solo haya una instancia de ZonaManager.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Establece la zona actual activa en el juego.
    /// </summary>
    /// <param name="zona">ZonaBase que se va a establecer como actual.</param>
    public void SetZonaActual(ZonaBase zona)
    {
        ZonaActual = zona;
    }
}