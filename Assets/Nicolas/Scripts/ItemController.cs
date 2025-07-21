using UnityEngine;

public class ItemController : MonoBehaviour
{
    private void interacao()
    {
        Debug.Log("Interagindo com o item: " + gameObject.name + "  Isso ativará um diálogo ou ação específica.");
    }
}
