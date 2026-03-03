using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] towerAttackClip;
    public AudioClip monsterDieClip;
    public AudioClip towerUpgradeClip;
    public AudioClip towerCreateClip;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayClip(AudioClip clip, float volume = 0.2f)
    {
        audioSource.PlayOneShot(clip, volume);
    }
}
