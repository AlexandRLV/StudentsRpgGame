using UnityEngine;

namespace Ui
{
    public class UiRoot : MonoBehaviour
    {
        public Transform HudParent => _hudParent;
        public Transform WindowsParent => _windowsParent;
        public Transform NotificationsParent => _notificationsParent;
        public Transform OverlayParent => _overlayParent;
        
        [SerializeField] private Transform _hudParent;
        [SerializeField] private Transform _windowsParent;
        [SerializeField] private Transform _notificationsParent;
        [SerializeField] private Transform _overlayParent;
    }
}