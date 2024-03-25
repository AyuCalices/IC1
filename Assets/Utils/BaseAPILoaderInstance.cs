using System;
using System.Threading.Tasks;
using DataStructures.Variables;
using TMPro;
using UnityEngine;

namespace Utils
{
    public abstract class BaseAPILoaderInstance : MonoBehaviour, IAPILoaderInstance
    {
        [Header("Directory Path")]
        [SerializeField] private TMP_InputField _directoryInputField;
        [SerializeField] private StringVariable _fileName;

        public string DirectoryPath => _directoryInputField.text.Trim();
        public string FileName => _fileName.Get().Trim();
        public abstract bool CanStartupAPI { get; }

        public virtual Task<bool> TryStartup(Action<string> updateProgressMethod)
        {
            return Task.FromResult(true);
        }

        public virtual Task OnStart(Action<string> updateProgressMethod)
        {
            return Task.CompletedTask;
        }
    }
}
