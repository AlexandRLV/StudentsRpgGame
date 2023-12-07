using System.Collections;
using System.Collections.Generic;
using GameSettings;
using Services.DI;
using UnityEngine;

namespace Services.SoundsSystem
{
    public class SoundsSystem : MonoBehaviour
    {
        [Header("Music")]
        [SerializeField] private float _transitionTime;
        [SerializeField] private AudioSource _firstMusicSource;
        [SerializeField] private AudioSource _secondMusicSource;
        
        [Header("Effects")]
        [SerializeField] private AudioSource _effectsSource;
        
        [Header("Common")]
        [SerializeField] private SoundsData _soundsData;
        
        [Inject] private GameSettingsManager _gameSettingsManager;

        private Dictionary<SoundType, AudioClip> _effects;
        private Dictionary<MusicType, AudioClip> _music;
        
        private Coroutine _fadingCoroutine;

        private float _musicVolume;

        [Construct]
        private void Construct()
        {
            _effects = new Dictionary<SoundType, AudioClip>();
            _music = new Dictionary<MusicType, AudioClip>();

            foreach (var container in _soundsData.SoundClips)
            {
                _effects.Add(container.Type, container.Clip);
            }

            foreach (var container in _soundsData.MusicClips)
            {
                _music.Add(container.Type, container.Clip);
            }
            
            _gameSettingsManager.RegisterVolumeListener(SoundVolumeType.Music, UpdateMusicVolume);
            _musicVolume = _gameSettingsManager.GetVolume(SoundVolumeType.Music);
            _firstMusicSource.volume = _musicVolume;
            _secondMusicSource.volume = 0f;
        }

        private void OnDestroy()
        {
            _gameSettingsManager.UnregisterVolumeListener(SoundVolumeType.Music, UpdateMusicVolume);
        }

        public void PlaySound(SoundType type) => _effectsSource.PlayOneShot(_effects[type]);

        public void PlayMusic(MusicType musicType, bool fade = true)
        {
            var clip = _music[musicType];
            if (_firstMusicSource.isPlaying && fade)
            {
                FadeToMusic(clip);
            }
            else
            {
                _firstMusicSource.clip = clip;
                _firstMusicSource.Play();
            }
        }
        
        private void FadeToMusic(AudioClip clip)
        {
            if (_fadingCoroutine != null)
                StopCoroutine(_fadingCoroutine);

            _fadingCoroutine = StartCoroutine(FadeTracks(clip));
        }

        private void UpdateMusicVolume(float volume)
        {
            _musicVolume = volume;
            if (_fadingCoroutine != null) return;

            _firstMusicSource.volume = volume;
        }

        private IEnumerator FadeTracks(AudioClip nextTrack)
        {
            _secondMusicSource.clip = nextTrack;
            _secondMusicSource.volume = 0.0f;
            _secondMusicSource.Play();

            float time = 0.0f;
            while (time < _transitionTime)
            {
                float t = time / _transitionTime;

                _firstMusicSource.volume = Mathf.Lerp(_musicVolume, 0.0f, t);
                _secondMusicSource.volume = Mathf.Lerp(0.0f, _musicVolume, t);

                time += Time.deltaTime;

                yield return null;
            }

            _firstMusicSource.volume = 0.0f;
            _secondMusicSource.volume = _musicVolume;
            _firstMusicSource.Stop();
            (_firstMusicSource, _secondMusicSource) = (_secondMusicSource, _firstMusicSource);
            _fadingCoroutine = null;
        }
    }
}