using UnityEngine;

public class PlayOneShotBehaviour : StateMachineBehaviour
{
    public AudioClip clip;
    public float volum = 1.0f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioSource.PlayClipAtPoint(clip, animator.gameObject.transform.position, volum);
    }
}
