using UnityEngine;
using UnityEngine.UI;

namespace Features.Connection.UI
{
    public class UpdateSetScrollRectVertical : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private Vector2 _normalizedTargetPosition;
        
        private bool _isActivated;

        public void EnableUpdate()
        {
            _isActivated = true;
        }

        public void DisableUpdate()
        {
            _isActivated = false;
        }

        private void OnEnable()
        {
            _scrollRect.normalizedPosition = _normalizedTargetPosition;
        }

        private void Update()
        {
            if (_isActivated)
            {
                _scrollRect.normalizedPosition = _normalizedTargetPosition;
            }
        }
    }
}
