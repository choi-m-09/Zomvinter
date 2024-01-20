using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    const string EffectVolumeKey = "EffectVolume";
    const string BGMVolumeKey = "BGMVolume";

    static Sound instance = null;
    public static Sound Ins
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType(typeof(Sound)) as Sound;
                if(instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "SoundManager";
                    instance = obj.AddComponent<Sound>();

                    instance.EffectVolume = 1.0f - PlayerPrefs.GetFloat(EffectVolumeKey);
                    instance.BGMVolume = 1.0f - PlayerPrefs.GetFloat(BGMVolumeKey);

                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    public bool isPauseBGM = false;
    List<AudioSource> EffectSource = new List<AudioSource>();

    AudioSource _bgmSource = null;
    AudioSource BGMSource
    {
        get
        {
            if(_bgmSource == null)
            {
                _bgmSource = Camera.main.GetComponent<AudioSource>();
            }
            return _bgmSource;
        }
    }

    float _effectVolume = 1.0f;
    float _bgmVolume = 1.0f;

    public float EffectVolume
    {
        get
        {
            return _effectVolume;
        }
        set
        {
            //_effectVolume = value > 1.0f ? 1.0f : value;
            _effectVolume = Mathf.Clamp(value, 0.0f, 1.0f);
            PlayerPrefs.SetFloat(EffectVolumeKey, 1.0f - _effectVolume);
            SetEffectVolume(value);
        }
    }
    public float BGMVolume
    {
        get
        {
            return _bgmVolume;
        }
        set
        {
            //_bgmVolume = value > 1.0f ? 1.0f : value;
            _bgmVolume = Mathf.Clamp(value, 0.0f, 1.0f);
            PlayerPrefs.SetFloat(BGMVolumeKey, 1.0f - _bgmVolume);
            BGMSource.volume = value;
        }
    }

    //public void Initialize()
    //{
    //    //BGMSource = Camera.main.GetComponent<AudioSource>();
    //}

    public void AddEffectSource(AudioSource source)
    {
        EffectSource.Add(source);
    }

    public void SetEffectVolume(float volum)
    {
        foreach(AudioSource source in EffectSource)
        {
            source.volume = volum;
        }
    }

    public void PlayBGM(AudioClip bgm = null, bool loop = true)
    {
        if (isPauseBGM)
        {
            isPauseBGM = false;
            BGMSource.Play();
            return;
        }
        BGMSource.clip = bgm;
        BGMSource.loop = loop;
        BGMSource.Play();
    }
    public void PauseBGM()
    {
        isPauseBGM = true;
        BGMSource.Pause();
    }
}
