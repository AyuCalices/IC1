using System.Collections.Generic;
using System.Linq;
using Features.Core.UI.Scripts.TransitionView;
using UnityEngine;
using UnityEngine.Events;

namespace Utils.CanvasNavigator
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] private bool _includeInactive;
        [SerializeField] private UnityEvent _onPrepareCanvases;
        
        private List<CanvasController> _canvasControllerList;
        private CanvasController _lastActiveCanvas;

        private void Awake()
        {
            _canvasControllerList = GetComponentsInChildren<CanvasController>(_includeInactive).ToList();
            _canvasControllerList.ForEach(x =>
            {
                if (!x.gameObject.activeSelf)
                {
                    gameObject.SetActive(true);
                }
                x.gameObject.SetActive(false);
            });
            _onPrepareCanvases.Invoke();
        
            MenuType_SO startingMenu = _canvasControllerList.Find(controller => controller.isStartMenu).canvasType;
            SwitchCanvas(startingMenu);
        }

        public void SwitchCanvas(MenuType_SO type)
        {
            if (_lastActiveCanvas != null)
            {
                _lastActiveCanvas.gameObject.Disable(DeactivationType.Disable);
            }
            CanvasController desiredCanvas = _canvasControllerList.Find(x => x.canvasType == type);
            if (desiredCanvas != null)
            {
                desiredCanvas.gameObject.Enable();
                _lastActiveCanvas = desiredCanvas;
            }
            else
            {
                Debug.LogWarning("Desired canvas was not found");
            }
        }

        public void CloseCanvas()
        {
            if (_lastActiveCanvas != null)
            {
                _lastActiveCanvas.gameObject.Disable(DeactivationType.Disable);
                _lastActiveCanvas = null;
            }
            else
            {
                Debug.LogWarning("No last active canvas");
            }
        
        }
    
    }
}
