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

    private void Start()
    {
        //FindObjectOfType<DialogueManager>().AfterTalkEvent += PlayVoice_byTalk;
    }

    void PlayVoice_byTalk(Dialogue dialogue, int contextCount)
    {
        string _voiceName = dialogue.voiceNames[contextCount].Trim(); // Trim()�� ������ �ȵǴ� " " �� �����ϴ¿��� ������ ���
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
        Debug.LogWarning("ã�� �� ���� ��� �̸� : " + p_Name);
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
    public event Action EffectSoundEvent;
    public void PlayEffectSound(string p_Name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (p_Name == effectSounds[i].name)
            { 
                effectPlayer.PlayOneShot(effectSounds[i].clip);
                if(EffectSoundEvent != null) EffectSoundEvent();
                return;
            }
        }
        Debug.LogWarning("ã�� �� ���� ȿ���� �̸� : " + p_Name);
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
        else Debug.LogWarning("ã�� �� ���� ���̽� : " + p_Name);
    }
}
