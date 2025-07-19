using UnityEngine;

public static class TransformExtensions
{
    public static int GetSortingOrder(this Transform transform, float ySortingOffset=0f)
    {
        return (-1) * Mathf.RoundToInt((transform.position.y + ySortingOffset) * 100f);
    }
}
