using System.Collections.Generic;
using System.Threading.Tasks;
using OobaboogaRuntimeIntegration;
using OobaboogaRuntimeIntegration.OobaboogaConfig;
using UnityEngine;
using Utils;

namespace Features.ModelLoading.Scripts
{
    public class OobaboogaModelFetcher : ModelFetcherInstance
    {
        [Header("Request")]
        [SerializeField] private OobaboogaModelsVariable _oobaboogaModelsVariable;

        protected override async Task<(APIResponse Response, string CurrentModel)> TryGetCurrentAPIModel()
        {
            (APIResponse Response, ModelInfoResponse Data) content = await _oobaboogaModelsVariable.GetCurrentModelAsync();
            
            return content.Response.IsError ? (content.Response, null) : (content.Response, content.Data.model_name);
        }

        protected override async Task<(APIResponse Response, List<string> ModelList)> TryGetModelList()
        {
            (APIResponse Response, ModelListResponse Data) content = await _oobaboogaModelsVariable.GetAllModelsAsync();
            
            return content.Response.IsError ? (content.Response, null) : (content.Response, content.Data.model_names);
        }
    }
}
