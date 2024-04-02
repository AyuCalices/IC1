using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Features._Core.UI.Scripts
{
    public class PlayerPrefsInputField : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _directoryInputField;
        [SerializeField] private UnityEvent _onLoadComplete;

        private void Start()
        {
            _directoryInputField.text = PlayerPrefs.GetString(GetGameObjectPath(gameObject));
            _onLoadComplete.Invoke();
        }

        private void OnDestroy()
        {
            SaveAsPlayerPref();
        }

        [ContextMenu("Save as PlayerPref")]
        private void SaveAsPlayerPref()
        {
            PlayerPrefs.SetString(GetGameObjectPath(gameObject), _directoryInputField.text);
            PlayerPrefs.Save();
        }

        private string GetGameObjectPath(GameObject obj)
        {
            string path = obj.name;
            Transform parent = obj.transform.parent;

            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }

            return path;
        }
    }
}
