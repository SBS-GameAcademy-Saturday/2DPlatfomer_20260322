using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public AudioSource introSound;
    public AudioSource loopSound;

    void Start()
    {
        introSound.Play();
        loopSound.PlayScheduled(AudioSettings.dspTime + introSound.clip.length);
    }
}
