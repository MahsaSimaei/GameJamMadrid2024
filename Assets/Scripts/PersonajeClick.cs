using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
public class PersonajeClick : MonoBehaviour
{
    // Diálogo del personaje (editable desde el Inspector)
    public string dialogoInicial;
    public string dialogoSinDinero;
    public string dialogoYaInvertido;
    public string dialogoOtraDecision;

    // Nombre del personaje
    public string nombre;
    public TMP_Text nameText;

    // Referencias desde el Canvas
    public TMP_Text dialogoText;
    public Button botonSi;
    public Button botonNo;
    public TMP_Text moneyText;

    // Imagen del personaje
    public Image personajeImage;

    // Dinero requerido para invertir
    public int cantidadInversion;

    // Referencia al administrador de dinero
    public Money_mangement moneyManager;

    // Variables para el control de decisiones
    private bool yaInvertido = false;

    public static List<string> inversiones = new List<string>();
    private static List<Image> todasLasImagenes = new List<Image>();

    // Video management
    public VideoPlayer videoPlayer;           // VideoPlayer component
    public RawImage videoDisplay;             // Display for the video

    // Dictionary to map characters to their videos
    public Dictionary<string, VideoClip> characterVideos = new Dictionary<string, VideoClip>();

    void Start()
    {
        // Add the character image to the global list
        if (personajeImage != null)
        {
            todasLasImagenes.Add(personajeImage);
            personajeImage.gameObject.SetActive(false);
        }

        // Clear dialogue and name text
        if (dialogoText != null) dialogoText.text = "";
        if (nameText != null) nameText.text = "";

        // Hide buttons initially
        if (botonSi != null) botonSi.gameObject.SetActive(false);
        if (botonNo != null) botonNo.gameObject.SetActive(false);

        // Hide video display initially
        if (videoDisplay != null) videoDisplay.gameObject.SetActive(false);

        // Assign videos to characters (example setup)
        characterVideos.Add("Salud", Resources.Load<VideoClip>("Videos/Salud"));
        characterVideos.Add("Educacion", Resources.Load<VideoClip>("Videos/Educacion"));
        characterVideos.Add("Transporte", Resources.Load<VideoClip>("Videos/Transporte"));
        characterVideos.Add("I+D", Resources.Load<VideoClip>("Videos/ID"));
        characterVideos.Add("Cultura", Resources.Load<VideoClip>("Videos/Cultura"));
    }

    void OnMouseDown()
    {
        
        // Verificar si ya se invirtió en este personaje
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


        // Mostrar el texto del diálogo inicial
        dialogoText.text = dialogoInicial + "\n¿Quieres invertir " + cantidadInversion + " monedas?";

        // Mostrar la imagen del personaje
        MostrarImagen();

        // Activar botones de decisión
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
        // Ocultar todas las demás imágenes antes de mostrar la actual
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

            // Confirmar la inversión
            dialogoText.text = "¡Gracias por invertir en mi propuesta!";
            yaInvertido = true; // Marcar como ya invertido

            // Registrar la inversión en la lista
            inversiones.Add(gameObject.name);

            // Play the corresponding video
            PlayInvestmentVideo(gameObject.name);

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
        // Cancelar inversión y cerrar botones
        dialogoText.text = "Tal vez en otra ocasión.";
        if (botonSi != null) botonSi.gameObject.SetActive(false);
        if (botonNo != null) botonNo.gameObject.SetActive(false);

        // Ocultar la imagen del personaje
        OcultarImagen();
    }

    bool InversionAfecta()
    {
        // Caso 1: Si hablas primero con Cultura y aceptas su inversión
        if (inversiones.Contains("Cultura"))
        {
            if (gameObject.name == "Transporte")
            {
                dialogoOtraDecision = "¡Me ha tocado! Tengo que ir al concierto de Taylor Swift. Hablamos después.";
                if (botonSi != null) botonSi.gameObject.SetActive(false);
                if (botonNo != null) botonNo.gameObject.SetActive(false);
                return true;
            }
            if (gameObject.name == "I+D")
            {
                dialogoOtraDecision = "Me estoy preparando para un torneo de Madrid in Game… hablamos más tarde.";
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

        // Caso 2: Si hablas primero con I+D y aceptas su inversión
        if (inversiones.Contains("I+D"))
        {
            if (gameObject.name == "Cultura")
            {
                dialogoOtraDecision = "Me enteré que invertiste antes en algo como I+D... ese loco de la tecnología nos va a llevar a la ruina... hablamos en otro momento.";
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

        // Caso 3: Si hablas primero con Transporte y aceptas su inversión
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
                dialogoOtraDecision = "El presupuesto no da para más, amigo. Tendremos que buscar otra solución.";
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

        // Caso 4: Si hablas primero con Educación y aceptas su inversión
        if (inversiones.Contains("Educacion"))
        {
            if (gameObject.name == "I+D")
            {
                dialogoOtraDecision = "Me temo que tus finanzas están en rojo. Sin dinero, no podemos avanzar en este proyecto.";
                if (botonSi != null) botonSi.gameObject.SetActive(false);
                if (botonNo != null) botonNo.gameObject.SetActive(false);
                return true;
            }
            if (gameObject.name == "Transporte")
            {
                dialogoOtraDecision = "¡Uf! Parece que el presupuesto se agotó antes de llegar a nosotros. Tendremos que esperar para esto.";
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

        // Caso 5: Si hablas primero con Salud y aceptas su inversión
        if (inversiones.Contains("Salud"))
        {
            return true;
        }

        // Por defecto, no hay restricciones
        return false;
    }

    void PlayInvestmentVideo(string characterName)
    {
        if (characterVideos.ContainsKey(characterName))
        {
            VideoClip videoClip = characterVideos[characterName];

            if (videoClip != null)
            {
                videoPlayer.clip = videoClip;
                videoDisplay.gameObject.SetActive(true);

                videoPlayer.Play();

                StartCoroutine(HideVideoAfterPlay(videoPlayer.clip.length));
            }
            else
            {
                Debug.LogError($"Video for character '{characterName}' not found!");
            }
        }
        else
        {
            Debug.LogError($"No video mapped for character '{characterName}'!");
        }
    }

    IEnumerator HideVideoAfterPlay(double duration)
    {
        yield return new WaitForSeconds((float)duration);
        videoDisplay.gameObject.SetActive(false);
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
