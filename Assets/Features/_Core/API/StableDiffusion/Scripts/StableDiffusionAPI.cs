using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Features._Core.API.StableDiffusion.Scripts
{
    [Serializable]
    public class StableDiffusionAPI
    {
        [SerializeField] private string _serverUrl;
        public string ServerUrl => _serverUrl;

        public StableDiffusionAPI(string url = "http://127.0.0.1:7860")
        {
            _serverUrl = url;
        }
        
        public async Task<(APIResponse Response, SDOutSampler[] Data)> GetSamplersAsync()
        {
            string url = $"{ServerUrl}/sdapi/v1/samplers";
            return await APICore.DispatchRequest<SDOutSampler[]>(url, UnityWebRequest.kHttpVerbGET);
        }
        
        public async Task<(APIResponse Response, SDOutModel[] Data)> GetAllModelsAsync()
        {
            string url = $"{ServerUrl}/sdapi/v1/sd-models";
            return await APICore.DispatchRequest<SDOutModel[]>(url, UnityWebRequest.kHttpVerbGET);
        }
        
        public async Task<APIResponse> PostOptionsModelCheckpointAsync(string modelName)
        {
            string url = $"{ServerUrl}/sdapi/v1/options";
            SDInOptionsModelCheckpoint loadModelRequest = new SDInOptionsModelCheckpoint()
            {
                sd_model_checkpoint = modelName
            };
            return (await APICore.DispatchRequest<SDOutModel[]>(url, UnityWebRequest.kHttpVerbPOST, APICore.CreateBody(loadModelRequest))).Response;
        }
        
        public async Task<(APIResponse Response, string Data)> InterruptGeneration()
        {
            string url = $"{ServerUrl}/sdapi/v1/interrupt";
            
            return await APICore.DispatchRequest<string>(url, UnityWebRequest.kHttpVerbPOST);
        }
        
        public async Task<(APIResponse Response, string Data)> GetSDCheckpointSha256Async()
        {
            string url = $"{ServerUrl}/sdapi/v1/options";
            
            return await APICore.DispatchRequest(url, UnityWebRequest.kHttpVerbGET, null, ParseResponse);
        }
        
        public async Task<(APIResponse Response, SDOutProgress Data)> GetProgressAsync()
        {
            string url = $"{ServerUrl}/sdapi/v1/progress";
            return await APICore.DispatchRequest<SDOutProgress>(url, UnityWebRequest.kHttpVerbGET);
        }

        public async Task<(APIResponse Response, SDOutTxt2Img Data)> PostTextToImage(SDInTxt2Img inTxt2Img)
        {
            string url = $"{ServerUrl}/sdapi/v1/txt2img";
            return await APICore.DispatchRequest<SDOutTxt2Img>(url, UnityWebRequest.kHttpVerbPOST, APICore.CreateBody(inTxt2Img));
        }
        
        private string ParseResponse(string responseText)
        {
            JObject obj = JObject.Parse(responseText);
            JToken currentModelSha256 = obj["sd_checkpoint_hash"];
            if (currentModelSha256 != null)
            {
                return currentModelSha256.Value<string>();
            }
            
            return "";
        }
    }
    
    [Serializable]
    public class SDOutProgress
    {
        public float progress;
        public float eta_relative;
        public Dictionary<string, object> state;
        public string current_image;
        public string textinfo;
    }
    
    [Serializable]
    public class SDOutSampler
    {
        public string name;
        public List<string> aliases;
        public Dictionary<string, string> options;
    }
    
    [Serializable]
    public class SDOutModel
    {
        public string title;
        public string model_name;
        public string hash;
        public string sha256;
        public string filename;
        public string config;
    }
    
    [Serializable]
    public class SDOutTxt2Img
    {
        public List<string> images;
        public Dictionary<string, object> parameters;
        public string info;
    }
    
    //TODO: add parameters
    [Serializable]
    public class SDInTxt2Img
    {
        [UsedImplicitly] public string prompt = string.Empty;
        [UsedImplicitly] public string negative_prompt = string.Empty;
        //public List<string> styles = new();
        [UsedImplicitly] public int seed = -1;
        //public int subseed = -1;
        //public int subseed_strength;
        //public int seed_resize_from_h = -1;
        //public int seed_resize_from_w = -1;
        [UsedImplicitly] public string sampler_name = string.Empty;
        //public int batch_size = 1;
        //public int n_iter = 1;
        [UsedImplicitly] public int steps = 50;
        [UsedImplicitly] public int cfg_scale = 7;
        [UsedImplicitly] public int width = 512;
        [UsedImplicitly] public int height = 512;
        //public bool restore_faces = true;
        //public bool tiling = true;
        //public bool do_not_save_samples;
        //public bool do_not_save_grid;
        //public double eta;
        //public double denoising_strength;
        //public double s_min_uncond;
        //public double s_churn;
        //public double s_tmax;
        //public double s_tmin;
        //public double s_noise;
        //public Dictionary<string, object> override_settings = new();
        //public bool override_settings_restore_afterwards = true;
        //public string refiner_checkpoint = string.Empty;
        //public int refiner_switch_at;
        //public bool disable_extra_networks;
        //public Dictionary<string, object> comments = new();
        //public bool enable_hr;
        //public int firstphase_width;
        //public int firstphase_height;
        //public int hr_scale = 2;
        //public string hr_upscaler = string.Empty;
        //public int hr_second_pass_steps;
        //public int hr_resize_x;
        //public int hr_resize_y;
        //public string hr_checkpoint_name = string.Empty;
        //public string hr_sampler_name = string.Empty;
        //public string hr_prompt = string.Empty;
        //public string hr_negative_prompt = string.Empty;
        //public string sampler_index = "Euler";
        //public string script_name = string.Empty;
        //public List<object> script_args = new();
        //public bool send_images = true;
        //public bool save_images;
        //public Dictionary<string, object> alwayson_scripts = new();
    }
    
    [Serializable]
    public class SDInOptionsModelCheckpoint
    {
        [UsedImplicitly] public string sd_model_checkpoint = "";
    }
}
