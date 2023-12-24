using GameSettings;
using Services.DI;
using Services.WindowsSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Windows
{
    public class SettingsWindow : WindowBase
    {
        [Header("Toggle")]
        [SerializeField] private Button _musicToggleButton;
        [SerializeField] private Button _effectsToggleButton;

        [Header("Toggle states")]
        [SerializeField] private GameObject _musicOnState;
        [SerializeField] private GameObject _musicOffState;
        [SerializeField] private GameObject _effectsOnState;
        [SerializeField] private GameObject _effectsOffState;
        
        [Header("Sliders")]
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _effectsVolumeSlider;
        
        [Header("Buttons")]
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _cancelButton;

        [Inject] private GameSettingsManager _gameSettingsManager;
        [Inject] private WindowsSystem _windowsSystem;
        
        private void Start()
        {
            var musicSettings = _gameSettingsManager.GetSettings(SoundVolumeType.Music);
            var effectsSettings = _gameSettingsManager.GetSettings(SoundVolumeType.Effects);
            
            _musicOnState.SetActive(musicSettings.enabled);
            _musicOffState.SetActive(!musicSettings.enabled);
            
            _effectsOnState.SetActive(effectsSettings.enabled);
            _effectsOffState.SetActive(!effectsSettings.enabled);

            _musicVolumeSlider.value = musicSettings.Volume;
            _effectsVolumeSlider.value = effectsSettings.Volume;
            
            _musicVolumeSlider.onValueChanged.AddListener(MusicVolumeChanged);
            _effectsVolumeSlider.onValueChanged.AddListener(EffectsVolumeChanged);
            
            _saveButton.onClick.AddListener(() =>
            {
                _gameSettingsManager.SaveSettings();
                _windowsSystem.DestroyWindow(this);
            });
            
            _cancelButton.onClick.AddListener(() =>
            {
                _windowsSystem.DestroyWindow(this);
            });
            
            _musicToggleButton.onClick.AddListener(ToggleMusic);
            _effectsToggleButton.onClick.AddListener(ToggleEffects);
        }

        private void MusicVolumeChanged(float volume)
        {
            _gameSettingsManager.SetVolume(SoundVolumeType.Music, volume);
            
            _musicOnState.SetActive(true);
            _musicOffState.SetActive(false);
        }

        private void EffectsVolumeChanged(float volume)
        {
            _gameSettingsManager.SetVolume(SoundVolumeType.Effects, volume);
            
            _effectsOnState.SetActive(true);
            _effectsOffState.SetActive(false);
        }

        private void ToggleMusic()
        {
            var settings = _gameSettingsManager.GetSettings(SoundVolumeType.Music);
            _gameSettingsManager.SetEnabled(SoundVolumeType.Music, !settings.enabled);
            
            _musicOnState.SetActive(settings.enabled);
            _musicOffState.SetActive(!settings.enabled);
            _musicVolumeSlider.SetValueWithoutNotify(0f);
        }

        private void ToggleEffects()
        {
            var settings = _gameSettingsManager.GetSettings(SoundVolumeType.Effects);
            _gameSettingsManager.SetEnabled(SoundVolumeType.Effects, !settings.enabled);
            
            _effectsOnState.SetActive(settings.enabled);
            _effectsOffState.SetActive(!settings.enabled);
            _effectsVolumeSlider.SetValueWithoutNotify(0f);
        }
    }
}