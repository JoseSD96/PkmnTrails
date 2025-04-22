using UnityEngine;

public class ZonaManager : MonoBehaviour
{
    public static ZonaManager Instance { get; private set; }
    public ZonaBase ZonaActual { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetZonaActual(ZonaBase zona)
    {
        ZonaActual = zona;
    }
}