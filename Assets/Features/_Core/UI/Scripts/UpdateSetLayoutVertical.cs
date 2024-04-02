using UnityEngine;
using UnityEngine.UI;

namespace Features._Core.UI.Scripts
{
    public class UpdateSetLayoutVertical : MonoBehaviour
    {
        [SerializeField] private ContentSizeFitter _contentSizeFitter;

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
            _contentSizeFitter.SetLayoutVertical();
        }

        private void Update()
        {
            if (_isActivated)
            {
                _contentSizeFitter.SetLayoutVertical();
            }
        }
    }
}
