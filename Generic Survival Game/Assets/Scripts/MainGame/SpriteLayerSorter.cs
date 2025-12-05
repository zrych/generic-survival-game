using UnityEngine;

public class SpriteLayerSorter : MonoBehaviour
{
    private SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void LateUpdate()
    {
        sr.sortingOrder = -(int)(transform.position.y * 100);
    }
}
