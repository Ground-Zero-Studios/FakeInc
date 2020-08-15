using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GroundZero.Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class Sound : MonoBehaviour
    {
        [HideInInspector]
        public static Sound instance;
        private AudioSource _asrc;

        public static int maxTrackedSources = 64;

        public List<AudioSource> trackedSources = new List<AudioSource>();

        public List<AudioClip> soundTimeoff = new List<AudioClip>();

        void Start()
        {
            instance = this;
            _asrc = GetComponent<AudioSource>();
            InvokeRepeating("TrackSources", 1f, 0.5f);
        }

        public void TrackSources()
        {
            foreach (AudioSource src in trackedSources)
            {
                if (!src.isPlaying)
                {
                    trackedSources.Remove(src);
                    Destroy(src);
                }
            }
        }

        public void PlayLocal(AudioClip clip, Vector3 pos, float volume)
        {
            AudioSource.PlayClipAtPoint(clip, pos, volume);
        }
        public void PlayLocal(AudioClip clip, Vector3 pos)
        {
            PlayLocal(clip, pos, 0.8f);
        }

        public void PlayAttached(AudioClip clip, GameObject obj)
        {
            if (trackedSources.Count > maxTrackedSources)
            {
                Debug.LogWarning("Unable to play audio clip: Max tracked sources reached.");
                return;
            }
            AudioSource source = obj.AddComponent<AudioSource>();
            trackedSources.Add(source);
            source.clip = clip;
            source.Play();
        }

        public void PlayGlobal(AudioClip clip, float volume)
        {
            _asrc.PlayOneShot(clip, volume);
        }

        public void PlayGlobal(AudioClip clip)
        {
            PlayGlobal(clip, 0.8f);
        }
    }
}