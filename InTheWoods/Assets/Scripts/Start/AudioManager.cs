using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static private AudioManager instance;

    AudioSource myAudioSource;
    [Tooltip(" 0: Btn(G), 1: Btn(C), 2: Normal(L), 3: Normal(H), 4: Warning")]
    [SerializeField] List<AudioClip> clip_list;

    void Start()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            myAudioSource = GetComponent<AudioSource>();
            myAudioSource.loop = false;
            myAudioSource.playOnAwake = false;
            myAudioSource.clip = null;
        }
    }

    public void SetVolume(float value)
    {
        myAudioSource.volume = value;
    }

    public void PlayAudioClip(int n)
    {
        if (myAudioSource.isPlaying)
            myAudioSource.Stop();

        myAudioSource.PlayOneShot(clip_list[n]);
    }
}
