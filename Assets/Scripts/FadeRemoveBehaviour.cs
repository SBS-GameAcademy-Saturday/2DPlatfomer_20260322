using UnityEngine;

public class FadeRemoveBehaviour : StateMachineBehaviour
{
    public float fadeTime = 1.0f;
    public float fadeDelay = 0.0f;
    
    private float timerElapsed = 0.0f;
    private float fadeDalayElapsed = 0;

    SpriteRenderer spriteRenderer;
    GameObject objToRemove;
    Color startColor;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timerElapsed = 0f;
        fadeDalayElapsed = 0f;
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
        objToRemove = animator.gameObject;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Delay을 체크하는 로직
        // fadeDelay(0.3) > fadeDalayElapsed(0.0) => fadeDalayElapsed증가 
        // fadeDelay(0.3) > fadeDalayElapsed(0.1)
        // fadeDelay(0.3) > fadeDalayElapsed(0.2)
        // fadeDelay(0.3) > fadeDalayElapsed(0.3) => Delay가 끝
        // fadeDelay(0.0) => Delay없이 바로 Fade 효과 시작
        if (fadeDelay > fadeDalayElapsed)
        {
            fadeDalayElapsed += Time.deltaTime;
        }
        // Fade 효과 시작
        else 
        {
            timerElapsed += Time.deltaTime;

            // fadeTime(1)
            // 1 - (timerElapsed(0) / fadeTime(1)) => 1
            // 1 - (timerElapsed(0.1) / fadeTime(1)) => 0.9
            // 1 - (timerElapsed(0.2) / fadeTime(1)) => 0.8
            // 1 - (timerElapsed(0.5) / fadeTime(1)) => 0.5
            // ...
            // 1 - (timerElapsed(1) / fadeTime(1)) => 0
            float newAlpha = startColor.a * (1 - (timerElapsed / fadeTime));

            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            if(timerElapsed > fadeTime)
            {
                Destroy(objToRemove);
            }
        }

    }
}
