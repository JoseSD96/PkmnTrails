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

    private void OnNoButtonClicked()
    {
        audioManager.PlayEfecto("Menus", "menuBoton");
        panelConfirmacion.SetActive(false);
        btnSi.interactable = false;
        btnNo.interactable = false;
    }

    private void OnSiButtonClicked()
    {
        audioManager.PlayEfecto("Menus", "menuBoton");
        SavingSystem.i.Delete("Save");
        panelConfirmacion.SetActive(false);
        onConfirmarNuevaPartida?.Invoke();
        onConfirmarNuevaPartida = null;
    }

    public void ConfirmacionNuevaPartida(Action onConfirmar)
    {
        panelConfirmacion.SetActive(true);
        btnSi.interactable = true;
        btnNo.interactable = true;
        onConfirmarNuevaPartida = onConfirmar;

        if (SavingSystem.i.CheckIfSaveExists("Save"))
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