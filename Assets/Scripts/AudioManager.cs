using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class AudioPlayerNode
{
    public AudioSource audioSource;
    public Audio currentlyPlaying;
    public bool IsPlaying => audioSource.isPlaying;

    public string GetNamePlaying()
    {
        if (currentlyPlaying != null && audioSource.isPlaying)
        {
            return currentlyPlaying.name;
        }
        else
        {
            return null;
        }
    }
    public void Play(Audio audio)
    {
        currentlyPlaying = audio;
        audioSource.Stop();
        audioSource.clip = audio.clip;
        audioSource.volume = audio.volume;
        audioSource.pitch = audio.pitch;
        audioSource.loop = audio.isLoop;
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void Resume()
    {
        audioSource.UnPause();
    }

    public void PlayOneShot(Audio audio)
    {
        audioSource.PlayOneShot(audio.clip);
    }

    public AudioPlayerNode(AudioSource source)
    {
        audioSource = source;
    }
    
    
}
public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioPack> soundPacks;
    [SerializeField] private List<AudioPack> musicPacks;
    public List<AudioPlayerNode> audioSources;
    public List<AudioPlayerNode> musicSources;
    private List<Audio> audios = new List<Audio>();
    private List<Audio> musics = new List<Audio>();

    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        audios = soundPacks.SelectMany((x) => x.audios).ToList();
        musics = musicPacks.SelectMany((x) => x.audios).ToList();
        audioSources.Add(new AudioPlayerNode(gameObject.AddComponent<AudioSource>()));
        musicSources.Add(new AudioPlayerNode(gameObject.AddComponent<AudioSource>()));
    }

    public void PlaySoundOneShot(string soundName)
    {
        Audio audio = audios.FirstOrDefault((x) => x.name == soundName);
        if (audio == default(Audio))
        {
            Debug.LogError("Sound Name " + soundName + " Not Found");
            return;
        }

        AudioPlayerNode node = audioSources[0];
        node.PlayOneShot(audio);
    }
    
    public void PlaySound(string soundName)
    {
        Audio audio = audios.FirstOrDefault((x) => x.name == soundName);
        if (audio == default(Audio))
        {
            Debug.LogError("Sound Name " + soundName + " Not Found");
            return;
        }

        AudioPlayerNode node = audioSources.FirstOrDefault((x) => !x.IsPlaying);
        if (node == default(AudioPlayerNode))
        {
            node = new AudioPlayerNode(gameObject.AddComponent<AudioSource>());
            audioSources.Add(node);
            node.Play(audio);
        }
        else
        {
            node.Play(audio);
        }
    }
    
    public void StopSound(string soundName)
    {
        foreach (var node in audioSources)
        {
            if ((node.currentlyPlaying != null ? node.currentlyPlaying.name : "")  == soundName)
            {
                node.Stop();
            }
        }
    }
    
    
    
    public void PlayMusic(string soundName, bool isParallel = false)
    {
        Audio audio = musics.FirstOrDefault((x) => x.name == soundName);
        if (audio == default(Audio))
        {
            Debug.LogError("Music Name " + soundName + " Not Found");
            return;
        }
        AudioPlayerNode node = musicSources[0];
        if (isParallel)
        {
            
        }
        if (node == default(AudioPlayerNode))
        {
            node = new AudioPlayerNode(gameObject.AddComponent<AudioSource>());
            audioSources.Add(node);
            node.Play(audio);
        }
        else
        {
            node.Play(audio);
        }
    }
    public void StopMusic(string soundName)
    {
        

        foreach (var node in musicSources)
        {
            if ((node.currentlyPlaying != null ? node.currentlyPlaying.name : "")  == soundName)
            {
                node.Stop();
            }
        }
    }

    public void StopALlSound()
    {
        foreach (AudioPlayerNode node in audioSources)
        {
            node.Stop();
        }
    }
    
    public void StopALlMusic()
    {
        foreach (AudioPlayerNode node in musicSources)
        {
            node.Stop();
        }
    }


    public bool IsMusicPlaying(string soundName)
    {
        foreach (var node in musicSources)
        {
            if ((node.currentlyPlaying != null ? node.currentlyPlaying.name : "")  == soundName && node.IsPlaying)
            {
                return true;
            }
        }
        return false;
    }
    
    public bool IsSoundPlaying(string soundName)
    {
        foreach (var node in audioSources)
        {
            if ((node.currentlyPlaying != null ? node.currentlyPlaying.name : "")  == soundName && node.IsPlaying)
            {
                return true;
            }
        }
        return false;
    }
    
}
