using System;
using System.Threading.Tasks;
using UnityEngine;
using Utils;

namespace Features.ModelLoading.Scripts
{
    public abstract class ModelLoaderInstance : MonoBehaviour, IModelLoaderInstance
    {
        public async Task<(bool IsValid, string ErrorMessage)> LoadModel(Action<string> updateProgressMethod)
        {
            APIResponse response = await InternalLoadModel(updateProgressMethod);
            if (response.IsError)
            {
                string errorMessage = $"An error occured while fetching the available Models! Error: {response.ResponseCode} {response.Error}";
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
