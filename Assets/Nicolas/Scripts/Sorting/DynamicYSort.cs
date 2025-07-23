using System;
using UnityEngine;

public class DynamicYSort : MonoBehaviour
{
    private int _baseSortingOrder = 0;
    private float _ySortingOffset;
    [SerializeField] private SortableSprite[] _sortableSprites;
    [SerializeField] private Transform _sortingOffset;

    [Serializable]
    public struct SortableSprite
    {
        public SpriteRenderer spriteRenderer;
        public int relativeOrder;
    }

    void Start()
    {
        _ySortingOffset = _sortingOffset != null ? _sortingOffset.localPosition.y : 0f;
    }
    void LateUpdate()
    {
        _baseSortingOrder = transform.GetSortingOrder(_ySortingOffset);
        foreach (var sortableSprite in _sortableSprites)
        {
            if (sortableSprite.spriteRenderer != null)
            {
                int newOrder = _baseSortingOrder + sortableSprite.relativeOrder;
                sortableSprite.spriteRenderer.sortingOrder = newOrder;
            }
        }
    }
}
