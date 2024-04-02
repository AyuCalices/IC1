using System;
using System.Threading.Tasks;
using Features._Core.API;
using UnityEngine;

namespace Features.ModelSelect.Scripts.ModelLoading
{
    public abstract class ModelLoaderInstance : MonoBehaviour, IModelLoaderInstance
    {
        public async Task<(bool IsValid, string ErrorMessage)> LoadModel(Action<string> updateProgressMethod)
        {
            APIResponse response = await InternalLoadModel(updateProgressMethod);
            Debug.LogWarning(response.ResponseCode + " " + response.Error + " " + response.Result + " " + response.IsError);
            if (response.IsError)
            {
                string errorMessage = $"An error occured while loading the Model! Error: {response.ResponseCode} {response.Error}";
                return (false, errorMessage);
            }

            return (true, "");
        }

        protected abstract Task<APIResponse> InternalLoadModel(Action<string> updateProgressMethod);
    }

    public interface IModelLoaderInstance
    {
        public Task<(bool IsValid, string ErrorMessage)> LoadModel(Action<string> updateProgressMethod);
    }
}
