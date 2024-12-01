using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Para cargar escenas

public class PersonajeClick : MonoBehaviour
{
    // Di�logo del personaje (editable desde el Inspector)
    public string dialogoInicial;

    // Di�logos alternativos seg�n las decisiones
    public string dialogoSinDinero;
    public string dialogoYaInvertido;
    public string dialogoOtraDecision;
    // Nombre del personaje (editable desde el Inspector)
    public string nombre;
    public TMP_Text nameText;        // Campo para el nombre del personaje

    // Referencias desde el Canvas
    public TMP_Text dialogoText;          // Campo para el texto del di�logo
    public Button botonSi;           // Bot�n de inversi�n "S�"
    public Button botonNo;           // Bot�n de inversi�n "No"
    public TMP_Text moneyText;       // Texto que muestra el dinero actual

    // Imagen del personaje
    public Image personajeImage;     // Imagen del personaje, asignada desde el Inspector

    // Dinero requerido para invertir (editable desde el Inspector)
    public int cantidadInversion;

    // Referencia al administrador de dinero
    public Money_mangement moneyManager;

    // Variables para el control de decisiones
    private bool yaInvertido = false;
    private bool introduccionMostrada = false; // Controla si ya se mostr� la introducci�n

    public static List<string> inversiones = new List<string>(); // Lista de inversiones realizadas
                                                                 // Lista est�tica para manejar im�genes activas globalmente
    private static List<Image> todasLasImagenes = new List<Image>();

    void Start()
    {
        // Agregar la imagen actual a la lista global
        if (personajeImage != null)
        {
            todasLasImagenes.Add(personajeImage);
            personajeImage.gameObject.SetActive(false); // Ocultar al inicio
        }


        // Vaciar el texto del di�logo
        if (dialogoText != null)
        {
            dialogoText.text = ""; // Texto vac�o al inicio
        }
        if (nameText != null)
        {
            nameText.text = ""; // Nombre vac�o al inicio
        }
        // Ocultar botones al inicio
        if (botonSi != null) botonSi.gameObject.SetActive(false);
        if (botonNo != null) botonNo.gameObject.SetActive(false);
    }

    void OnMouseDown()
    {
        
        // Verificar si ya se invirti� en este personaje
        if (yaInvertido)
        {
            dialogoText.text = dialogoYaInvertido;
            MostrarImagen();
            return;
        }

        // Verificar si ya se hicieron inversiones que afectan a este personaje
        if (InversionAfecta())
        {
            dialogoText.text = dialogoOtraDecision;
            MostrarImagen();
            return;
        }
        if (nameText != null)
        {
            nameText.text = nombre; // Update the name text with the character's name
        }


        // Mostrar el texto del di�logo inicial
        dialogoText.text = dialogoInicial + "\n�Quieres invertir " + cantidadInversion + " monedas?";

        // Mostrar la imagen del personaje
        MostrarImagen();

        // Activar botones de decisi�n
        if (botonSi != null) botonSi.gameObject.SetActive(true);
        if (botonNo != null) botonNo.gameObject.SetActive(true);

        // Asignar eventos a los botones
        botonSi.onClick.RemoveAllListeners(); // Limpiar listeners previos
        botonSi.onClick.AddListener(() => Invertir());

        botonNo.onClick.RemoveAllListeners();
        botonNo.onClick.AddListener(() => Cancelar());
    }
   

    void MostrarImagen()
    {
        // Ocultar todas las dem�s im�genes antes de mostrar la actual
        foreach (Image img in todasLasImagenes)
        {
            if (img != personajeImage)
            {
                img.gameObject.SetActive(false);
            }
        }

        // Mostrar la imagen actual
        if (personajeImage != null)
        {
            personajeImage.gameObject.SetActive(true);
        }
    }

    void OcultarImagen()
    {
        // Ocultar la imagen actual
        if (personajeImage != null)
        {
            personajeImage.gameObject.SetActive(false);
        }
    }


    void Invertir()
    {
        if (Money_mangement.money >= cantidadInversion)
        {
            // Reducir el dinero y actualizar el texto
            Money_mangement.money -= cantidadInversion;
            moneyText.text = " " + Money_mangement.money;

            // Confirmar la inversi�n
            dialogoText.text = "�Gracias por invertir en mi propuesta!";
            yaInvertido = true; // Marcar como ya invertido

            // Registrar la inversi�n en la lista
            inversiones.Add(gameObject.name);

            // Verificar si se cumplen las condiciones para un final
            VerificarFinal();
        }
        else
        {
            // Mostrar mensaje de error si no hay suficiente dinero
            dialogoText.text = dialogoSinDinero;
        }

        // Ocultar botones
        if (botonSi != null) botonSi.gameObject.SetActive(false);
        if (botonNo != null) botonNo.gameObject.SetActive(false);
    }

    void Cancelar()
    {
        // Cancelar inversi�n y cerrar botones
        dialogoText.text = "Tal vez en otra ocasi�n.";
        if (botonSi != null) botonSi.gameObject.SetActive(false);
        if (botonNo != null) botonNo.gameObject.SetActive(false);

        // Ocultar la imagen del personaje
        OcultarImagen();
    }

    bool InversionAfecta()
    {
        // Caso 1: Si hablas primero con Cultura y aceptas su inversi�n
        if (inversiones.Contains("Cultura"))
        {
            if (gameObject.name == "Transporte")
            {
                dialogoOtraDecision = "�Me ha tocado! Tengo que ir al concierto de Taylor Swift. Hablamos despu�s.";
                if (botonSi != null) botonSi.gameObject.SetActive(false);
                if (botonNo != null) botonNo.gameObject.SetActive(false);
                return true;
            }
            if (gameObject.name == "I+D")
            {
                dialogoOtraDecision = "Me estoy preparando para un torneo de Madrid in Game� hablamos m�s tarde.";
                if (botonSi != null) botonSi.gameObject.SetActive(false);
                if (botonNo != null) botonNo.gameObject.SetActive(false);
                return true;
            }
            if (gameObject.name == "Salud")
            {
                dialogoOtraDecision = "Ding me llama mi madre me ha dicho que me ha preparado hamburguesa para comer.";
                if (botonSi != null) botonSi.gameObject.SetActive(false);
                if (botonNo != null) botonNo.gameObject.SetActive(false);
                return true;
            }
        }

        // Caso 2: Si hablas primero con I+D y aceptas su inversi�n
        if (inversiones.Contains("I+D"))
        {
            if (gameObject.name == "Cultura")
            {
                dialogoOtraDecision = "Me enter� que invertiste antes en algo como I+D... ese loco de la tecnolog�a nos va a llevar a la ruina... hablamos en otro momento.";
                if (botonSi != null) botonSi.gameObject.SetActive(false);
                if (botonNo != null) botonNo.gameObject.SetActive(false);
                return true;
            }
            if (gameObject.name == "Educacion")
            {
                dialogoOtraDecision = "No tienes suficiente dinero para invertir.";
                if (botonSi != null) botonSi.gameObject.SetActive(false);
                if (botonNo != null) botonNo.gameObject.SetActive(false);
                return true;
            }
            if (gameObject.name == "Salud")
            {
                dialogoOtraDecision = "Ding me llama mi madre me ha dicho que me ha preparado hamburguesa para comer.";
                if (botonSi != null) botonSi.gameObject.SetActive(false);
                if (botonNo != null) botonNo.gameObject.SetActive(false);
                return true;
            }
        }

        // Caso 3: Si hablas primero con Transporte y aceptas su inversi�n
        if (inversiones.Contains("Transporte"))
        {
            if (gameObject.name == "Cultura")
            {
                dialogoOtraDecision = "Casi me atropellan esos locos con esa nueva especie de trenes... no quiero hablar de dinero, la verdad.";
                if (botonSi != null) botonSi.gameObject.SetActive(false);
                if (botonNo != null) botonNo.gameObject.SetActive(false);
                return true;
            }
            if (gameObject.name == "Educacion")
            {
                dialogoOtraDecision = "El presupuesto no da para m�s, amigo. Tendremos que buscar otra soluci�n.";
                if (botonSi != null) botonSi.gameObject.SetActive(false);
                if (botonNo != null) botonNo.gameObject.SetActive(false);
                return true;
            }
            if (gameObject.name == "Salud")
            {
                dialogoOtraDecision = "Ding me llama mi madre me ha dicho que me ha preparado hamburguesa para comer.";
                if (botonSi != null) botonSi.gameObject.SetActive(false);
                if (botonNo != null) botonNo.gameObject.SetActive(false);
                return true;
            }
        }

        // Caso 4: Si hablas primero con Educaci�n y aceptas su inversi�n
        if (inversiones.Contains("Educacion"))
        {
            if (gameObject.name == "I+D")
            {
                dialogoOtraDecision = "Me temo que tus finanzas est�n en rojo. Sin dinero, no podemos avanzar en este proyecto.";
                if (botonSi != null) botonSi.gameObject.SetActive(false);
                if (botonNo != null) botonNo.gameObject.SetActive(false);
                return true;
            }
            if (gameObject.name == "Transporte")
            {
                dialogoOtraDecision = "�Uf! Parece que el presupuesto se agot� antes de llegar a nosotros. Tendremos que esperar para esto.";
                if (botonSi != null) botonSi.gameObject.SetActive(false);
                if (botonNo != null) botonNo.gameObject.SetActive(false);
                return true;
            }
            if (gameObject.name == "Salud")
            {
                dialogoOtraDecision = "Ding... Mi madre me llama. Parece que hay hamburguesas otra vez.";
                if (botonSi != null) botonSi.gameObject.SetActive(false);
                if (botonNo != null) botonNo.gameObject.SetActive(false);
                return true;
            }
        }

        // Caso 5: Si hablas primero con Salud y aceptas su inversi�n
        if (inversiones.Contains("Salud"))
        {
            return true;
        }

        // Por defecto, no hay restricciones
        return false;
    }

    void VerificarFinal()
    {
        if (inversiones.Contains("Salud"))
        {
            SceneManager.LoadScene("MadridSaludable"); // Final 1
        }
        else if (inversiones.Contains("I+D") && inversiones.Contains("Transporte"))
        {
            SceneManager.LoadScene("MadridCyberpunk"); // Final 2
        }
        else if (inversiones.Contains("Educacion") && inversiones.Contains("Cultura"))
        {
            SceneManager.LoadScene("MadridDarkAcademy"); // Final 3
        }
    }
}
