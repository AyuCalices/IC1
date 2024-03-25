using DataStructures.Variables;
using TMPro;
using UnityEngine;

namespace Features.Connection.UI
{
    public class DirectoryButtonCondition : BaseButtonCondition
    {
        [SerializeField] private StringVariable _fileNameVariable;
        [SerializeField] private TMP_InputField _directoryPath;
        [SerializeField] private TMP_Text _errorText;

        private bool _errorTextActiveState;

        private void Awake()
        {
            _errorText.gameObject.SetActive(false);
            _directoryPath.onValueChanged.AddListener(_ => InternalOnConditionUpdate());
        }

        private void OnDestroy()
        {
            _directoryPath.onValueChanged.RemoveListener(_ => InternalOnConditionUpdate());
        }

        private void OnEnable()
        {
            _errorText.gameObject.SetActive(_errorTextActiveState);
            InternalOnConditionUpdate();
        }

        private void OnDisable()
        {
            _errorText.gameObject.SetActive(false);
            InternalOnConditionUpdate();
        }

        public override bool ButtonIsEnabled()
        {
            string requestedDirectory = UpdateSlashOnDirectory(_directoryPath.text.Trim()) + UpdateSlashOnFileName(_fileNameVariable.Get().Trim());
            bool isValid = System.IO.File.Exists(requestedDirectory);

            if (gameObject.activeSelf)
            {
                _errorTextActiveState = !isValid;
                _errorText.gameObject.SetActive(_errorTextActiveState);
            }
            
            _errorText.text = !isValid ? $"Invalid Directory: {requestedDirectory}" : "";
            return isValid;
        }
        
        //TODO: update slash
        private string UpdateSlashOnDirectory(string firstString)
        {
            if (!firstString.EndsWith("\\"))
            {
                return firstString + "\\";
            }
            
            return firstString;
        }
        
        //TODO: update slash
        private string UpdateSlashOnFileName(string secondString)
        {
            if (secondString.StartsWith("\\"))
            {
                return secondString.Remove(0);
            }

            return secondString;
        }
    }
}
