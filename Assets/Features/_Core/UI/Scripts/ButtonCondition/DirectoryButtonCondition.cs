using Features._Core.DataStructures.Variables;
using TMPro;
using UnityEngine;

namespace Features._Core.UI.Scripts.ButtonCondition
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

        private void OnDestroy()
        {
            _directoryPath.onValueChanged.RemoveListener(_ => InternalOnConditionUpdate());
        }

        public override bool ButtonIsEnabled()
        {
            string requestedDirectory = _directoryPath.text.Trim().AddIfLastNotSlash() + _fileNameVariable.Get().Trim().RemoveIfFirstSlash();
            bool isValid = System.IO.File.Exists(requestedDirectory);

            if (gameObject.activeSelf)
            {
                _errorTextActiveState = !isValid;
                _errorText.gameObject.SetActive(_errorTextActiveState);
            }
            
            _errorText.text = !isValid ? $"Invalid Directory: {requestedDirectory}" : "";
            return isValid;
        }
    }
}
