using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GravityGames.MizJam1.Utils
{
    /// <summary>
    /// The audio manager
    /// </summary>
    public static class AudioManager
    {
        static AudioSource audioSource;
        static Dictionary<AudioClipName, AudioClip> audioClips =
            new Dictionary<AudioClipName, AudioClip>();

        /// <summary>
        /// Initializes the audio manager
        /// </summary>
        /// <param name="source">audio source</param>
        public static void Initialize(AudioSource source)
        {
            audioSource = source;
            audioClips.Add(AudioClipName.Crash, 
                Resources.Load<AudioClip>(@"Audio\crash"));
            audioClips.Add(AudioClipName.BackgroundMusic,
                Resources.Load<AudioClip>(@"Audio\Richard_Wagner_-_The_Valkyrie_-_Ride_of_the_Valkyries"));
        }

        /// <summary>
        /// Plays the audio clip with the given name
        /// </summary>
        /// <param name="name">name of the audio clip to play</param>
        public static void Play(AudioClipName name)
        {
            audioSource.PlayOneShot(audioClips[name]);
        }
    }


    public static class AudioFadeOut {
        
        public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime) {
            float startVolume = audioSource.volume;

            while ( audioSource.volume > 0 ) {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }

    }
}