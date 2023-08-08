using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip footstepSound1;
    [SerializeField] private AudioClip footstepSound2;
    [SerializeField] private AudioClip landSound1;
    [SerializeField] private AudioClip landSound2;
    [SerializeField] private AudioClip rollSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip slashSound1;
    [SerializeField] private AudioClip slashSound2;

    public void PlayDamageSound()
    {
        AudioManager.Instance.PlayOneShot(damageSound);
    }
    public void PlayFootstepSound1()
    {
        AudioManager.Instance.PlayOneShot(footstepSound1);
    }
    public void PlayFootstepSound2()
    {
        AudioManager.Instance.PlayOneShot(footstepSound2);
    }
    public void PlayLandSound()
    {
        AudioManager.Instance.PlayOneShot(Random.Range(0, 2) == 0 ? landSound1 : landSound2);
    }
    public void PlayRollSound()
    {
        AudioManager.Instance.PlayOneShot(rollSound);
    }
    public void PlayJumpSound()
    {
        AudioManager.Instance.PlayOneShot(jumpSound);
    }
    public void PlaySlashSound1()
    {
        AudioManager.Instance.PlayOneShot(slashSound1);
    }
    public void PlaySlashSound2()
    {
        AudioManager.Instance.PlayOneShot(slashSound2);
    }
}
