using UnityEngine;

namespace Ui
{
    public class LoadingScreen : MonoBehaviour
    {
        public bool Active
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }
    }
}