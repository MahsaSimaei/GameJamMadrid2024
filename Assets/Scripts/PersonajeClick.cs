using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PersonajeClick : MonoBehaviour
{
    // Diálogo del personaje (editable desde el Inspector)
    public string dialogo;

    // Referencias desde el Canvas
    public Text dialogoText;          // Campo para el texto del diálogo
    public Button botonSi;           // Botón de inversión "Sí"
    public Button botonNo;           // Botón de inversión "No"
    public TMP_Text moneyText;       // Texto que muestra el dinero actual

    // Dinero requerido para invertir (editable desde el Inspector)
    public int cantidadInversion;

    // Referencia al administrador de dinero
    public Money_mangement moneyManager;

    void OnMouseDown()
    {
        // Mostrar el texto del diálogo
        if (dialogoText != null)
        {
            dialogoText.text = dialogo + "\n¿Quieres invertir " + cantidadInversion + " monedas?";
        }

        // Activar botones de decisión
        botonSi.gameObject.SetActive(true);
        botonNo.gameObject.SetActive(true);

        // Asignar eventos a los botones
        botonSi.onClick.RemoveAllListeners(); // Limpiar listeners previos
        botonSi.onClick.AddListener(() => Invertir());

        botonNo.onClick.RemoveAllListeners();
        botonNo.onClick.AddListener(() => Cancelar());
    }

    void Invertir()
    {
        if (Money_mangement.money >= cantidadInversion)
        {
            // Reducir el dinero y actualizar el texto
            Money_mangement.money -= cantidadInversion;
            moneyText.text = "Money: " + Money_mangement.money;

            // Confirmar la inversión
            dialogoText.text = "¡Gracias por invertir en mi propuesta!";
        }
        else
        {
            // Mostrar mensaje de error si no hay suficiente dinero
            dialogoText.text = "No tienes suficiente dinero para invertir.";
        }

        // Ocultar botones
        botonSi.gameObject.SetActive(false);
        botonNo.gameObject.SetActive(false);
    }

    void Cancelar()
    {
        // Cancelar inversión y cerrar botones
        dialogoText.text = "Tal vez en otra ocasión.";
        botonSi.gameObject.SetActive(false);
        botonNo.gameObject.SetActive(false);
    }
}
