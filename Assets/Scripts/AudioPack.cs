using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Audio
{
    public string name;
    public AudioClip clip;
    public bool isLoop = false;
    public float volume = 1f;
    public float pitch = 1f;
}
[CreateAssetMenu(fileName = "Audio Pack", menuName = "Audio Pack")]

public class AudioPack : ScriptableObject
{
    public List<Audio> audios = new List<Audio>();
}
