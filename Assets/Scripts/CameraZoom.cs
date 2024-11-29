using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 1f;
    public float moveSpeed = 5f;
    public float minOrthoSize = 5f;
    public float maxOrthoSize = 20f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Handle zoom functionality
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            float newSize = cam.orthographicSize - scroll * zoomSpeed * 20f;
            cam.orthographicSize = Mathf.Clamp(newSize, minOrthoSize, maxOrthoSize);
        }

        // Handle movement functionality
        float moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        cam.transform.Translate(moveHorizontal, moveVertical, 0);
    }
}