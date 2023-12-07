using Services.DI;
using Services.SoundsSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Extra
{
    [RequireComponent(typeof(Button))]
    [DisallowMultipleComponent]
    public class UiButtonSoundProvider : MonoBehaviour
    {
        [Inject] private SoundsSystem _soundsSystem;

        private Button _button;
        
        private void Start()
        {
            GameContainer.InjectToInstance(this);
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => _soundsSystem.PlaySound(SoundType.Click));
        }
    }
}