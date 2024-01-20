using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundProperties : MonoBehaviour
{
    private AudioSource _source;
    protected AudioSource mySpeaker
    {
        get
        {
            if(_source == null)
            {
                _source = GetComponent<AudioSource>();
                Sound.Ins.AddEffectSource(_source);
            }
            return _source;
        }
    }
}
