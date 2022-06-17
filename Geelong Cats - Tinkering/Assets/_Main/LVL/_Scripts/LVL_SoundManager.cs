using System;
using UnityEngine;

namespace com.DU.CE.LVL
{
    [System.Serializable]
    public class AudioFile
    {
        public string audioName;

        public AudioClip audioClip;

        [Range(0f, 1f)]
        public float volume;

        [HideInInspector]
        public AudioSource source;

        public bool isLooping;

        public bool playOnAwake;
    }

    public class LVL_SoundManager : MonoBehaviour
    {
        // Singleton class
        public static LVL_SoundManager instance;

        public AudioFile[] audioFiles;

        private void Awake()
        {
            // Make it a singleton
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            // Create a audio source for each file in audioFiles
            foreach (var s in audioFiles)
            {
                s.source = gameObject.AddComponent<AudioSource>();

                // Assign the parameters to the audio source
                s.source.clip = s.audioClip;
                s.source.volume = s.volume;
                s.source.loop = s.isLooping;

                if (s.playOnAwake)
                    s.source.Play();
            }
        }

        // Static method to play a audio clip given its name
        public static void PlayMusic(string name)
        {
            AudioFile s = Array.Find(instance.audioFiles, AudioFile => AudioFile.audioName == name);
            if (s == null)
            {
                Debug.LogError("Sound name" + name + "not found!");
                return;
            }
            else
                s.source.PlayOneShot(s.audioClip);
        }

        // Similar to the above but stop the music instead
        public static void StopMusic(string name)
        {
            AudioFile s = Array.Find(instance.audioFiles, AudioFile => AudioFile.audioName == name);
            if (s == null)
            {
                Debug.LogError("Sound name" + name + "not found!");
                return;
            }
            else
                s.source.Stop();
        }

        // Similar to the above but pauses the music instead
        public static void PauseMusic(string name)
        {
            AudioFile s = Array.Find(instance.audioFiles, AudioFile => AudioFile.audioName == name);
            if (s == null)
            {
                Debug.LogError("Sound name" + name + "not found!");
                return;
            }
            else
                s.source.Pause();
        }

        // Similar to above resumes music
        public static void UnPauseMusic(String name)
        {
            AudioFile s = Array.Find(instance.audioFiles, AudioFile => AudioFile.audioName == name);
            if (s == null)
            {
                Debug.LogError("Sound name" + name + "not found!");
                return;
            }
            else
            {
                s.source.UnPause();
            }
        }
    }
}