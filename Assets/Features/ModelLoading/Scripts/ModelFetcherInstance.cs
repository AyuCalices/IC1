using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using Utils;

namespace Features.ModelLoading.Scripts
{
    public abstract class ModelFetcherInstance : MonoBehaviour, IModelFetcherInstance
    {
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private TMP_Text _currentModelText;
        [SerializeField] private TMP_Text _notifyText;
        
        protected OptionsContainer optionsContainer;
        private bool _initialized;

        private void Awake()
        {
            _dropdown.onValueChanged.AddListener(UpdateOptionIndex);
        }

        private void OnDestroy()
        {
            _dropdown.onValueChanged.RemoveListener(UpdateOptionIndex);
        }

        private void OnDisable()
        {
            if (!_initialized) return;
            
            _initialized = false;
            FinalizeModelList();
        }

        private void UpdateOptionIndex(int selectedIndex)
        {
            optionsContainer.selectedOptionIndex = selectedIndex;
        }
        
        public void FinalizeModelList()
        {
            string serializedOptions = JsonConvert.SerializeObject(optionsContainer);
            PlayerPrefs.SetString(GetType().ToString(), serializedOptions);
            PlayerPrefs.Save();
        }

        public async Task<(bool IsValid, string ErrorMessage)> TryInitializeModelList()
        {
            _initialized = true;
            _notifyText.gameObject.SetActive(false);
            
            //get current api model
            (APIResponse Response, string CurrentModel) currentAPIModelContent = await TryGetCurrentAPIModel();
            if (currentAPIModelContent.Response.IsError)
            {
                string errorMessage = $"An error occured while fetching the available Models! Error: {currentAPIModelContent.Response.ResponseCode} {currentAPIModelContent.Response.Result}";
                return (false, errorMessage);
            }
            _currentModelText.text = currentAPIModelContent.CurrentModel;
            
            //get entire api model list
            (APIResponse Response, List<string> ModelList) modelListContent = await TryGetModelList();
            if (modelListContent.Response.IsError)
            {
                string errorMessage = $"An error occured while fetching the available Models! Error: {modelListContent.Response.ResponseCode} {modelListContent.Response.Result}";
                return (false, errorMessage);
            }

            //return, if api has no models
            if (modelListContent.ModelList.Count == 0)
            {
                return (false, "An API is missing valid models!");
            }

            SetupVisuals(modelListContent);
            
            return (true, "");
        }

        protected abstract Task<(APIResponse Response, string CurrentModel)> TryGetCurrentAPIModel();
        
        protected abstract Task<(APIResponse Response, List<string> ModelList)> TryGetModelList();
        
        private void SetupVisuals((APIResponse Response, List<string> ModelList) modelListContent)
        {
            //load saved previous selected model
            string serializedOptions = PlayerPrefs.GetString(GetType().ToString());
            Debug.LogWarning(serializedOptions);
            if (serializedOptions != "null")
            {
                OptionsContainer previousOptions = JsonConvert.DeserializeObject<OptionsContainer>(serializedOptions);
            
                //notify, if server model list isn't the same a local model list
                if (!ModelListMatchesWithPrevious(previousOptions.options, modelListContent.ModelList))
                {
                    _notifyText.gameObject.SetActive(true);
                    _notifyText.text = "The model list changed from last session! Selected the first Model!";
                    optionsContainer = new OptionsContainer(0, modelListContent.ModelList);
                }
                else
                {
                    optionsContainer = new OptionsContainer(previousOptions.selectedOptionIndex, modelListContent.ModelList);
                }
            }
            else
            {
                optionsContainer = new OptionsContainer(0, modelListContent.ModelList);
            }
            
            _dropdown.ClearOptions();
            _dropdown.AddOptions(modelListContent.ModelList);
            _dropdown.value = optionsContainer.selectedOptionIndex;
        }
        
        private bool ModelListMatchesWithPrevious(List<string> previousOptions, List<string> newOptions)
        {
            if (previousOptions.Count == newOptions.Count)
            {
                foreach (var previousOption in previousOptions)
                {
                    if (!newOptions.Contains(previousOption))
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }
        
        protected class OptionsContainer
        {
            public OptionsContainer(int selectedOptionIndex, List<string> options)
            {
                this.selectedOptionIndex = selectedOptionIndex;
                this.options = options;
            }
            
            public int selectedOptionIndex;
            public readonly List<string> options;
        }
    }
    
    public interface IModelFetcherInstance
    {
        public Task<(bool IsValid, string ErrorMessage)> TryInitializeModelList();
        public void FinalizeModelList();
    }
}
