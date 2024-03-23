using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public abstract class BaseAPILoaderInstance : MonoBehaviour, IAPILoaderInstance
    {
        [SerializeField] private TMP_Text _loadingText;
        [SerializeField] private Button _connectButton;
        [SerializeField] private TMP_InputField _directoryInputField;
        [SerializeField] private TMP_Text _errorTest;
        [SerializeField, TextArea] private string _fileName;

        public string DirectoryPath => _directoryInputField.text.Trim();
        public string FileName => _fileName.Trim();

        private static HashSet<BaseAPILoaderInstance> _invalidLoaderInstances = new();

        private void Start()
        {
            _directoryInputField.text = PlayerPrefs.GetString(GetType().ToString());
            FileExistsValidation();
            _directoryInputField.onValueChanged.AddListener(_ => FileExistsValidation());
        }

        private void OnDestroy()
        {
            PlayerPrefs.SetString(GetType().ToString(), _directoryInputField.text);
            PlayerPrefs.Save();
            
            _directoryInputField.onValueChanged.RemoveListener(_ => FileExistsValidation());
        }

        private void FileExistsValidation()
        {
            bool isValid = CanBeStarted();
            _errorTest.text = !isValid ? "Invalid Directory!" : "";

            if (!isValid)
            {
                _invalidLoaderInstances.Add(this);
            }
            else
            {
                _invalidLoaderInstances.Remove(this);
            }

            _connectButton.interactable = _invalidLoaderInstances.Count == 0;
        }

        public bool CanBeStarted()
        {
            return System.IO.File.Exists(DirectoryPath + "\\" + FileName);
        }

        public virtual Task<bool> StartupFailed()
        {
            return Task.FromResult(true);
        }

        public virtual Task Initiate()
        {
            return Task.CompletedTask;
        }

        protected void UpdateProgressState(string text)
        {
            Debug.Log(text);
            _loadingText.text = text;
        }
    }
}
