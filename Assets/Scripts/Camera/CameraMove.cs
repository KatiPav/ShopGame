using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public int Boundary = 50; // distance from edge scrolling starts
    public int speed = 5;
    public float zoomSpeed = 5f;
    public float minZoom = 3f;
    public float maxZoom = 20f;
    public float limit = 10f; // how far the camera can move from its starting position (each direction)

    private int theScreenWidth;
    private int theScreenHeight;

    private Camera cam;
    private Vector3 startPos;

    void Start()
    {
        theScreenWidth = Screen.width;
        theScreenHeight = Screen.height;

        startPos = transform.position;

        cam = GetComponent<Camera>();

        if (cam == null)
        {
            Debug.Log("Camera Move cannot find Camera. Zoom may not work properly.");
        }
    }

    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.mousePosition.x > theScreenWidth - Boundary)
        {
            pos.x += speed * Time.deltaTime; // move on +X axis
        }

        if (Input.mousePosition.x < 0 + 5) // it is less here as the menu is there
        {
            pos.x -= speed * Time.deltaTime; // move on -X axis
        }

        if (Input.mousePosition.y > theScreenHeight - Boundary)
        {
            pos.y += speed * Time.deltaTime; // move on +Y axis
        }

        if (Input.mousePosition.y < 0 + Boundary)
        {
            pos.y -= speed * Time.deltaTime; // move on -Y axis
        }

        // Keep the camera within 'limit' units of where it started
        pos.x = Mathf.Clamp(pos.x, startPos.x - limit, startPos.x + limit);
        pos.y = Mathf.Clamp(pos.y, startPos.y - limit, startPos.y + limit);

        transform.position = pos;
        UpdateZoom();
    }

    private void UpdateZoom()
    {
        if (cam == null) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }
}