using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Features._Core.API;
using Features._Core.API.StableDiffusion.Scripts;
using UnityEngine;

namespace Features.ModelSelect.Scripts.ModelFetching
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
