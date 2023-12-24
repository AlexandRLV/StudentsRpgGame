using UnityEngine;
using TMPro;

namespace Ui
{
    public class LoadingScreen : MonoBehaviour
    {
        public bool Active
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        [SerializeField] private float _minChangeTimer;
        [SerializeField] private float _maxChangeTimer;
        [SerializeField] private TextMeshProUGUI _underText;
        [SerializeField] private string[] _underPhrases;

        private float _timer;

        private void OnEnable()
        {
            SetNext();
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer > 0f) return;

            SetNext();
        }

        private void SetNext()
        {
            _underText.text = _underPhrases[Random.Range(0, _underPhrases.Length)];
            _timer = Random.Range(_minChangeTimer, _maxChangeTimer);
        }
    }
}