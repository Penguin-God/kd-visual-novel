using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] Sound[] bgmSounds;
    [SerializeField] AudioSource bgmPlayer;
    void BgmPlay(string p_Name)
    {
        for(int i = 0; i < bgmSounds.Length; i++)
        {
            if(bgmSounds[i].name == p_Name)
            {
                bgmPlayer.clip = bgmSounds[i].clip;
                bgmPlayer.Play();
                return;
            }
        }
        Debug.LogWarning("찾을 수 없는 브금 이름 : " + p_Name);
    }

    void StopBgm()
    {
        bgmPlayer.Stop();
    }

    void PauseBgm()
    {
        bgmPlayer.Pause();
    }

    void UnpauseBgm()
    {
        bgmPlayer.UnPause();
    }


    [SerializeField] Sound[] effectSounds;
    [SerializeField] AudioSource effectPlayer;
    void PlayEffectSound(string p_Name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (p_Name == effectSounds[i].name)
            { 
                effectPlayer.PlayOneShot(effectSounds[i].clip);
                return;
            }
        }
        Debug.LogWarning("찾을 수 없는 효과음 이름 : " + p_Name);
    }

    void StopEffectSound()
    {
        effectPlayer.Stop();
    }


    [SerializeField] AudioSource voicePlayer;
    void PlayVoiceSound(string p_Name)
    {
        AudioClip _clip = Resources.Load<AudioClip>("Sounds/Voice/" + p_Name);
        if (_clip != null)
        {
            voicePlayer.clip = _clip;
            voicePlayer.Play();
        }
        else Debug.LogWarning("찾을 수 없는 보이스 : " + p_Name);
    }


    /// <summary>
    /// soundType 0 : bgm
    /// soundType 1 : effectSound
    /// soundType 2 : voice
    /// </summary>
    /// <param name="p_Name"></param>
    /// <param name="soundType"></param>
    public void PlaySound(string p_Name, int soundType)
    {
        switch (soundType)
        {
            case 0: BgmPlay(p_Name); break;
            case 1: PlayEffectSound(p_Name); break;
            case 2: PlayVoiceSound(p_Name); break;
            default: Debug.LogWarning("찾을 수 없는 타입 넘버 : " + soundType); break;
        }
    }
}
