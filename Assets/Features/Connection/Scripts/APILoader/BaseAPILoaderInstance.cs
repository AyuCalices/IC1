using System;
using System.Threading.Tasks;
using Features._Core.DataStructures.Variables;
using TMPro;
using UnityEngine;

namespace Features.Connection.Scripts.APILoader
{
    public abstract class BaseAPILoaderInstance : MonoBehaviour, IAPILoaderInstance
    {
        [Header("Directory Path")]
        [SerializeField] private TMP_InputField _directoryInputField;
        [SerializeField] private StringVariable _fileName;
        
        public string DirectoryPath => _directoryInputField.text;
        public string FileName => _fileName.Get();
        public abstract bool CanStartupAPI { get; }
        public abstract string URL { get; }

        public virtual Task<bool> TryStartup(Action<string> updateProgressMethod)
        {
            return Task.FromResult(true);
        }
    }
}
