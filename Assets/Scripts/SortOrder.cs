using UnityEngine;

public class SortOrder : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Vector3 lastPosition;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;
    }

    void LateUpdate()
    {
        if (transform.position != lastPosition)
        {

            int order = -(Mathf.RoundToInt(transform.position.y * 1000) - Mathf.RoundToInt(transform.position.x * 10));
            spriteRenderer.sortingOrder = order;
            lastPosition = transform.position;
        }
    }

}