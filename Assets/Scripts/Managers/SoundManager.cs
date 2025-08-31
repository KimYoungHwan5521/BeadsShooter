using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager
{
    AudioMixer currentMixer;
    AudioMixerGroup AMGmaster;
    AudioMixerGroup AMGbgm;
    AudioMixerGroup AMGsfx;

    AudioSource[] bgmArray = new AudioSource[2];
    const int sfxMaxNumber = 30;
    AudioSource[] sfxArray = new AudioSource[sfxMaxNumber];
    Queue<AudioSource> sfxQueue = new();

    Action AudioEffectUpdate;
    public IEnumerator Initiate()
    {
        currentMixer = ResourceManager.Mixer;
        if (currentMixer == null) Debug.LogWarning("AudioMixer not found!");
        AMGmaster = currentMixer.FindMatchingGroups("Master")[0];
        AMGbgm = currentMixer.FindMatchingGroups("BGM")[0];
        AMGsfx = currentMixer.FindMatchingGroups("SFX")[0];
        if (AMGmaster == null) Debug.LogWarning("AudioMixerGroup Master not found!");
        if (AMGbgm == null) Debug.LogWarning("AudioMixerGroup BGM not found!");
        if (AMGsfx == null) Debug.LogWarning("AudioMixerGroup SFX not found!");

        Transform soundContainer = new GameObject("Sound Container").transform;
        soundContainer.SetParent(GameManager.Instance.transform);
        // BGM ��ü�� �� ���̵���/�ƿ� �ϱ����� AudioSource�� 2�� �غ�
        GameObject bgmCarrier = new("BGM Carrier", typeof(AudioSource), typeof(AudioSource));
        bgmCarrier.transform.SetParent(soundContainer);
        bgmArray = bgmCarrier.GetComponents<AudioSource>();

        for (int i = 0; i < bgmArray.Length; i++)
        {
            bgmArray[i].outputAudioMixerGroup = AMGbgm;
            bgmArray[i].loop = true;
            bgmArray[i].playOnAwake = false;
            // BGM�� �Ÿ� ��� �����Ƿ�
            bgmArray[i].maxDistance = float.MaxValue;
            bgmArray[i].minDistance = float.MaxValue;
        }

        for (int i = 0; i < sfxMaxNumber; i++)
        {
            GameObject sfxCarrier = new("SFX Carrier", typeof(AudioSource));
            sfxCarrier.transform.SetParent(soundContainer);
            AudioSource currentSource = sfxCarrier.GetComponent<AudioSource>();
            currentSource.outputAudioMixerGroup = AMGsfx;
            currentSource.playOnAwake = false;
            currentSource.spatialBlend = 1;
            sfxArray[i] = currentSource;
            sfxQueue.Enqueue(currentSource);
        }

        GameManager.Instance.ManagerUpdate += SoundManagerUpdate;
        yield return null;
    }

    void SoundManagerUpdate()
    {
        AudioEffectUpdate?.Invoke();
    }

    public void UpdateBGMMixer()
    {
        bgmArray[0].volume = Mathf.SmoothStep(bgmArray[0].volume, 1, Time.unscaledTime * 5);
        bgmArray[1].volume = Mathf.SmoothStep(bgmArray[1].volume, 0, Time.unscaledTime * 5);
        if (bgmArray[0].volume > 0.99)
        {
            AudioEffectUpdate -= UpdateBGMMixer;
        }
    }

    public static void Play(ResourceEnum.BGM wantBGM)
    {
        // 0 : �÷��� �� ���
        // 1 : ���� �÷��� ���� ���
        // ���� 0���� 1������ ������ �÷��� �� ����� 0�� �ֱ�
        SoundManager soundManager = GameManager.Instance.SoundManager;
        soundManager.bgmArray[1].clip = soundManager.bgmArray[0].clip;
        soundManager.bgmArray[1].time = soundManager.bgmArray[0].time;
        soundManager.bgmArray[1].volume = soundManager.bgmArray[0].volume;

        soundManager.bgmArray[0].clip = ResourceManager.Get(wantBGM);
        soundManager.bgmArray[0].time = 0;
        soundManager.bgmArray[0].volume = 0;
        soundManager.bgmArray[0].Play();

        soundManager.AudioEffectUpdate -= soundManager.UpdateBGMMixer;
        soundManager.AudioEffectUpdate += soundManager.UpdateBGMMixer;
    }

    public static void Play(ResourceEnum.SFX wantSFX, Vector3 soundOrigin, bool loop = false)
    {
        SoundManager soundManager = GameManager.Instance.SoundManager;
        AudioClip clip = ResourceManager.Get(wantSFX);
        if (soundManager.sfxQueue.TryDequeue(out AudioSource currentSource))
        {
            currentSource.clip = clip;
            currentSource.loop = loop;
            currentSource.transform.position = soundOrigin;
            currentSource.Play();
            soundManager.sfxQueue.Enqueue(currentSource);
        }
    }

    public static void Play(ResourceEnum.SFX wantSFX, Vector3 soundOrigin, bool loop, out AudioSource source)
    {
        SoundManager soundManager = GameManager.Instance.SoundManager;
        AudioClip clip = ResourceManager.Get(wantSFX);
        if (soundManager.sfxQueue.TryDequeue(out AudioSource currentSource))
        {
            currentSource.clip = clip;
            currentSource.loop = loop;
            currentSource.transform.position = soundOrigin;
            currentSource.Play();
            source = currentSource;
            if (!loop) soundManager.sfxQueue.Enqueue(currentSource);
        }
        else source = null;
    }

    public void Enqueue(AudioSource audioSource)
    {
        GameManager.Instance.SoundManager.sfxQueue.Enqueue(audioSource);
    }

    public static void StopBGM()
    {
        foreach (var bgm in GameManager.Instance.SoundManager.bgmArray)
        {
            bgm.Stop();
        }
    }

    public static void StopSFX(AudioSource source)
    {
        source.Stop();
        GameManager.Instance.SoundManager.sfxQueue.Enqueue(source);
    }

    public enum AudioMixerGroupType
    {
        Master, BGM, SFX
    }
    public void ToggleAudioMixerGroup(AudioMixerGroupType type, bool toggle, float value)
    {
        // �����̴� 0~1 �� dB
        float dB = Mathf.Log10(Mathf.Max(value, 0.001f)) * 20f;
        switch (type)
        {
            case AudioMixerGroupType.Master:
                AMGmaster.audioMixer.SetFloat("Master", toggle ? dB : -80);
                break;
            case AudioMixerGroupType.BGM:
                AMGmaster.audioMixer.SetFloat("BGM", toggle ? dB : -80);
                break;
            case AudioMixerGroupType.SFX:
                AMGmaster.audioMixer.SetFloat("SFX", toggle ? dB : -80);
                break;
        }
    }

    public void PitchShift(float wantPitch)
    {
        //foreach (var audioSource in GameManager.Instance.SoundManager.bgmArray) audioSource.pitch = wantPitch;
        foreach (var audioSource in GameManager.Instance.SoundManager.sfxArray)
        {
            audioSource.pitch = wantPitch > 0 ? wantPitch : 1;
            if (wantPitch > 0) currentMixer.SetFloat("SFX_Pitch", 1 / wantPitch);
            else currentMixer.SetFloat("SFX_Pitch", 1);
        }
    }
}