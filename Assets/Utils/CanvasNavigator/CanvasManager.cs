using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils.CanvasNavigator
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] private bool _includeInactive;
        
        private List<CanvasController> canvasControllerList;
        private CanvasController lastActiveCanvas;

        private void Awake()
        {
            canvasControllerList = GetComponentsInChildren<CanvasController>(_includeInactive).ToList();
            canvasControllerList.ForEach(x =>
            {
                if (!x.gameObject.activeSelf)
                {
                    gameObject.SetActive(true);
                }
                x.gameObject.SetActive(false);
            });
        
            MenuType_SO startingMenu = canvasControllerList.Find(controller => controller.isStartMenu).canvasType;
            SwitchCanvas(startingMenu);
        }

        public void SwitchCanvas(MenuType_SO type)
        {
            if (lastActiveCanvas != null)
            {
                lastActiveCanvas.gameObject.SetActive(false);
            }
            CanvasController desiredCanvas = canvasControllerList.Find(x => x.canvasType == type);
            if (desiredCanvas != null)
            {
                desiredCanvas.gameObject.SetActive(true);
                lastActiveCanvas = desiredCanvas;
            }
            else
            {
                Debug.LogWarning("Desired canvas was not found");
            }
        
        
        }

        public void CloseCanvas()
        {
            if (lastActiveCanvas != null)
            {
                lastActiveCanvas.gameObject.SetActive(false);
                lastActiveCanvas = null;
            }
            else
            {
                Debug.LogWarning("No last active canvas");
            }
        
        }
    
    }
}
