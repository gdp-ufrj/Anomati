using UnityEngine;

public class EnableAnimatorDadCutscene : MonoBehaviour
{
    public Animator animatorDadCutscene;

    void Start()
    {
        animatorDadCutscene.enabled = true;
    }
}
