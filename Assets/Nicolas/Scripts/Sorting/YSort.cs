using UnityEngine;

//Script para ordenar objetos pela posição Y
public class YSort : MonoBehaviour 
{
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.sortingOrder = transform.GetSortingOrder();
    }
}
