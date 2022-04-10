using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    //public static SoundManager instance;
    //private void Awake()
    //{
    //    if(instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    [SerializeField] DialogueChannel dialogueChannel = null;
    private void Start()
    {
        dialogueChannel.ChangeContextEvent += PlayVoice_byTalk;
    }

    void PlayVoice_byTalk(DialogueData dialogue, int contextCount)
    {
        string _voiceName = dialogue.voiceNames[contextCount].Trim(); // Trim()은 감지가 안되는 " " 가 존재하는오류 때문에 사용
        if (_voiceName != "") PlayVoiceSound(_voiceName);
    }

    [SerializeField] Sound[] bgmSounds;
    [SerializeField] AudioSource bgmPlayer;
    public void PlayBgm(string p_Name)
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
    //public event Action EffectSoundEvent;
    public void PlayEffectSound(string p_Name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (p_Name == effectSounds[i].name)
            { 
                effectPlayer.PlayOneShot(effectSounds[i].clip);
                //if(EffectSoundEvent != null) EffectSoundEvent();
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
    public void PlayVoiceSound(string p_Name)
    {
        AudioClip _clip = Resources.Load<AudioClip>("Sounds/Voice/" + p_Name);
        if (_clip != null)
        {
            voicePlayer.clip = _clip;
            voicePlayer.Play();
        }
        else Debug.LogWarning("찾을 수 없는 보이스 : " + p_Name);
    }
}