using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        public bool loop;
    }

    public List<Sound> sounds;
    private Dictionary<string, AudioSource> soundSources = new Dictionary<string, AudioSource>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: persist between scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = s.clip;
            source.volume = s.volume;
            source.loop = s.loop;
            soundSources[s.name] = source;
        }
    }

    public void Play(string name)
    {
        if (soundSources.ContainsKey(name))
        {
            soundSources[name].Play();
        }
        else
        {
            Debug.LogWarning("Sound not found: " + name);
        }
    }

    public void Stop(string name)
    {
        if (soundSources.ContainsKey(name))
        {
            soundSources[name].Stop();
        }
    }
}
