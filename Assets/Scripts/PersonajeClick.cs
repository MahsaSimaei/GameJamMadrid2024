using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonajeClick : MonoBehaviour
{
    // Di�logo del personaje
    public string dialogo;

    // Referencia al texto del Canvas
    public Text dialogoText;

    void OnMouseDown()
    {
        // Cambiar el texto del di�logo al hacer clic
        if (dialogoText != null)
        {
            dialogoText.text = dialogo;
        }
    }
}