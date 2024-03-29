using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StableDiffusionRuntimeIntegration;
using StableDiffusionRuntimeIntegration.SDConfig;
using UnityEngine;
using Utils;

namespace Features.ModelLoading.Scripts
{
    public class StableDiffusionModelFetcher : ModelFetcherInstance
    {
        [Header("Request")]
        [SerializeField] private SDModelsVariable _sdModelsVariable;

        protected override async Task<(APIResponse Response, string CurrentModel)> TryGetCurrentAPIModel()
        {
            (APIResponse Response, SDOutModel Data) content = await _sdModelsVariable.GetCurrentModelAsync();

            return content.Response.IsError ? (content.Response, null) : (content.Response, content.Data.model_name);
        }

        protected override async Task<(APIResponse Response, List<string> ModelList)> TryGetModelList()
        {
            (APIResponse Response, SDOutModel[] Data) content = await _sdModelsVariable.GetAllModelsAsync();

            List<string> mappedList = content.Data.Select(sdOutModel => sdOutModel.model_name).ToList();
            return content.Response.IsError ? (content.Response, null) : (content.Response, mappedList);
        }
    }
}
