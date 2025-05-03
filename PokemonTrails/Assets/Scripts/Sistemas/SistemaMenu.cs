using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SistemaMenu : MonoBehaviour
{
    [SerializeField] private Button btnSi;
    [SerializeField] private Button btnNo;
    [SerializeField] private GameObject panelConfirmacion;
    [SerializeField] private TextMeshProUGUI textoConfirmacion;
    [SerializeField] AudioManager audioManager;
    private Action onConfirmarNuevaPartida;

    /// <summary>
    /// Inicializa el panel de confirmación y los listeners de los botones al iniciar la escena.
    /// Desactiva el panel y los botones hasta que se requiera una confirmación.
    /// </summary>
    void Start()
    {
        panelConfirmacion.SetActive(false);
        btnSi.onClick.RemoveAllListeners();
        btnNo.onClick.RemoveAllListeners();

        btnSi.onClick.AddListener(OnSiButtonClicked);
        btnNo.onClick.AddListener(OnNoButtonClicked);

        btnSi.interactable = false;
        btnNo.interactable = false;
    }

    /// <summary>
    /// Acción ejecutada al pulsar el botón "No" en la confirmación.
    /// Cierra el panel y desactiva los botones.
    /// </summary>
    private void OnNoButtonClicked()
    {
        audioManager.PlayEfecto("Menus", "menuBoton");
        panelConfirmacion.SetActive(false);
        btnSi.interactable = false;
        btnNo.interactable = false;
    }

    /// <summary>
    /// Acción ejecutada al pulsar el botón "Sí" en la confirmación.
    /// Borra la partida guardada, cierra el panel y ejecuta la acción de confirmación.
    /// </summary>
    private void OnSiButtonClicked()
    {
        audioManager.PlayEfecto("Menus", "menuBoton");
        SavingSystem.i.Delete("Save");
        panelConfirmacion.SetActive(false);
        onConfirmarNuevaPartida?.Invoke();
        onConfirmarNuevaPartida = null;
    }

    /// <summary>
    /// Muestra el panel de confirmación para iniciar una nueva partida.
    /// Si ya existe una partida guardada, muestra el mensaje de advertencia.
    /// Si no existe, ejecuta directamente la acción de confirmación.
    /// </summary>
    /// <param name="onConfirmar">Acción a ejecutar si se confirma la nueva partida.</param>
    public void ConfirmacionNuevaPartida(Action onConfirmar)
    {
        panelConfirmacion.SetActive(true);
        btnSi.interactable = true;
        btnNo.interactable = true;
        onConfirmarNuevaPartida = onConfirmar;

        if (SavingSystem.i.CheckIfSaveExists("Save0.6"))
        {
            textoConfirmacion.text = "Esto borrara tu partida actual, ¿Seguro que quieres continuar?";
        }
        else
        {
            textoConfirmacion.text = "";
            onConfirmarNuevaPartida?.Invoke();
            panelConfirmacion.SetActive(false);
        }
    }
}