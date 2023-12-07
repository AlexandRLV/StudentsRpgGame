using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace GameSettings
{
    public class GameSettingsManager
    {
        private const string SoundSettingsKey = "SoundSettings";
        private static SoundVolumeType[] _volumeTypes = new[] { SoundVolumeType.Effects, SoundVolumeType.Music };
        
        private Dictionary<SoundVolumeType, Action<float>> _soundVolumeListeners = new();
        private List<SoundSettings> _soundSettings;
        
        public GameSettingsManager()
        {
            foreach (var volumeType in _volumeTypes)
            {
                _soundVolumeListeners.Add(volumeType, null);
            }
            
            if (!PlayerPrefs.HasKey(SoundSettingsKey))
            {
                _soundSettings = new List<SoundSettings>();
                foreach (var volumeType in _volumeTypes)
                {
                    var settings = new SoundSettings
                    {
                        soundType = volumeType,
                        enabled = true,
                        volume = 1f
                    };
                    _soundSettings.Add(settings);
                }
                return;
            }

            string json = PlayerPrefs.GetString(SoundSettingsKey);
            _soundSettings = JsonConvert.DeserializeObject<List<SoundSettings>>(json);
        }

        public void SaveSettings()
        {
            if (_soundSettings == null) return;

            string json = JsonConvert.SerializeObject(_soundSettings);
            PlayerPrefs.SetString(SoundSettingsKey, json);
        }

        public float GetVolume(SoundVolumeType type) => GetSettings(type).Volume;

        public SoundSettings GetSettings(SoundVolumeType type)
        {
            foreach (var soundSetting in _soundSettings)
            {
                if (soundSetting.soundType == type) return soundSetting;
            }

            return null;
        }

        public void SetVolume(SoundVolumeType type, float volume)
        {
            var settings = GetSettings(type);
            settings.volume = volume;
            settings.enabled = true;
            
            _soundVolumeListeners[type]?.Invoke(settings.Volume);
        }

        public void SetEnabled(SoundVolumeType type, bool enabled)
        {
            var settings = GetSettings(type);
            settings.enabled = enabled;
            
            _soundVolumeListeners[type]?.Invoke(settings.Volume);
        }

        public void RegisterVolumeListener(SoundVolumeType type, Action<float> callback) =>
            _soundVolumeListeners[type] += callback;
        public void UnregisterVolumeListener(SoundVolumeType type, Action<float> callback) =>
            _soundVolumeListeners[type] -= callback;
    }
}