using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DynamicSortingOrder : MonoBehaviour
{
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        // Flip Y so lower = higher order
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.z * 100);
        // Optional: offset for held items
    }
}
