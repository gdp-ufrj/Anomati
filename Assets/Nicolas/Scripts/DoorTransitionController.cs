using UnityEngine;

public class DoorTransitionController : MonoBehaviour    // É uma classe singleton, ou seja, só pode existir uma instância dela nas cenas
{
    private static DoorTransitionController instance;
    [HideInInspector] public bool isTransitioning = false; // Flag para controlar se a transição de porta está em andamento

    public static DoorTransitionController GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}