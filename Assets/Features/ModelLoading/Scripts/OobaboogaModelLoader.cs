using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataStructures.Variables;
using OobaboogaRuntimeIntegration;
using OobaboogaRuntimeIntegration.OobaboogaConfig;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace Features.ModelLoading.Scripts
{
    public class OobaboogaModelLoader : ModelLoaderInstance
    {
        [Header("Request")] 
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private OobaboogaAPIVariable _oobaboogaAPIVariable;
        [SerializeField] private OobaboogaModelsVariable _oobaboogaModelsVariable;

        protected override async Task<APIResponse> InternalLoadModel(Action<string> updateProgressMethod)
        {
            updateProgressMethod.Invoke("Get Current Text Generation Model ...");
            (APIResponse GetCurrentModelResponse, ModelInfoResponse Data) content = await _oobaboogaAPIVariable.Get().GetCurrentModelAsync();
            if (content.GetCurrentModelResponse.IsError) return content.GetCurrentModelResponse;
            
            string currentModel = content.Data.model_name;
            if (currentModel == _dropdown.options[_dropdown.value].text)
            {
                Debug.LogWarning($"Model already loaded: {currentModel}");
                return new APIResponse()
                {
                    ResponseCode = 200, 
                    Result = UnityWebRequest.Result.Success
                };
            }

            updateProgressMethod.Invoke("Load Text Generation Model ...");
            return await _oobaboogaModelsVariable.LoadModelAsync(_dropdown.options[_dropdown.value].text);
        }
    }
}
