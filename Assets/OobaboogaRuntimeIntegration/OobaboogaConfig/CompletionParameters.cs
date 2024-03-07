using System;
using System.Collections.Generic;
using UnityEngine;

namespace OobaboogaRuntimeIntegration.OobaboogaConfig
{
    [CreateAssetMenu(fileName = "OoobaboogaCompletionParameters", menuName = "Ooobabooga/CompletionParameters")]
    public class CompletionParameters : ScriptableObject, ICompletionParams
    {
        /// <summary>
        /// Unused parameter. To change the model, use the /v1/internal/model/load endpoint.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [SerializeField, HideInInspector] private string _model = string.Empty;
        public string Model
        {
            get => _model;
            set => _model = value;
        }

        /// <summary>
        /// Given your prompt, the model calculates the probabilities for every possible next token.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private string _prompt = string.Empty;
        public string Prompt 
        {
            get => _prompt;
            set => _prompt = value;
        }

        /// <summary>
        /// Unused parameter.
        /// https://github.com/oobabooga/text-generation-webui/blob/992affefef7d71544352920720eec08cbf3f56b9/extensions/openai/typing.py#L55
        /// </summary>
        [SerializeField, HideInInspector] private int _bestOf = 1;
        public int Best_Of 
        {
            get => _bestOf;
            set => _bestOf = value;
        }

        //TODO: probably debugging for oobabooga internal?
        [SerializeField, HideInInspector] private bool _echo = false;
        public bool Echo 
        {
            get => _echo;
            set => _echo = value;
        }

        /// <summary>
        /// Repetition penalty that scales based on how many times the token has appeared in the context.
        /// Be careful with this; there's no limit to how much a token can be penalized.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private int _frequencyPenalty = 0;
        public int Frequency_Penalty 
        {
            get => _frequencyPenalty;
            set => _frequencyPenalty = value;
        }

        /// <summary>
        /// logit_bias and logprobs not yet supported
        /// </summary>
        [SerializeField, HideInInspector] private Dictionary<string, object> _logitBias = new();
        public Dictionary<string, object> Logit_Bias 
        {
            get => _logitBias;
            set => _logitBias = value;
        }

        /// <summary>
        /// logit_bias and logprobs not yet supported
        /// </summary>
        [SerializeField, HideInInspector] private int _logprobs = 0;
        public int Logprobs 
        {
            get => _logprobs;
            set => _logprobs = value;
        }

        /// <summary>
        /// Maximum number of tokens to generate. Don't set it higher than necessary: it is used in the truncation
        /// calculation through the formula (prompt_length) = min(truncation_length - max_new_tokens, prompt_length),
        /// so your prompt will get truncated if you set it too high.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private int _maxTokens = 0;
        public int Max_Tokens 
        {
            get => _maxTokens;
            set => _maxTokens = value;
        }

        /// <summary>
        /// Unused parameter.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [SerializeField, HideInInspector] private int _n = 1;
        public int N 
        {
            get => _n;
            set => _n = value;
        }

        /// <summary>
        /// repetition_penalty: Penalty factor for repeating prior tokens. 1 means no penalty, higher value = less repetition, lower value = more repetition.
        /// Similar to repetition_penalty, but with an additive offset on the raw token scores instead of a multiplicative factor.
        /// It may generate better results. 0 means no penalty, higher value = less repetition, lower value = more repetition.
        /// Previously called "additive_repetition_penalty".
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private int _presencePenalty = 0;
        public int Presence_Penalty 
        {
            get => _presencePenalty;
            set => _presencePenalty = value;
        }

        //TODO: might be -> Custom stopping strings: The model stops generating as soon as any of the strings set in this field is generated. Note that when generating text in the Chat tab, some default stopping strings are set regardless of this parameter, like "\nYour Name:" and "\nBot name:" for chat mode. That's why this parameter has a "Custom" in its name.
        [SerializeField, HideInInspector] private string[] _stop = Array.Empty<string>();
        public string[] Stop 
        {
            get => _stop;
            set => _stop = value;
        }

        //TODO: no clue
        [SerializeField, HideInInspector] private string _suffix = string.Empty;
        public string Suffix 
        {
            get => _suffix;
            set => _suffix = value;
        }

        /// <summary>
        /// Primary factor to control the randomness of outputs. 0 = deterministic (only the most likely token is used). Higher value = more randomness.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private float _temperature = 1;
        public float Temperature 
        {
            get => _temperature;
            set => _temperature = value;
        }

        /// <summary>
        /// If not set to 1, select tokens with probabilities adding up to less than this number. Higher value = higher range of possible random results.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private float _topP = 1;
        public float Top_P 
        {
            get => _topP;
            set => _topP = value;
        }

        /// <summary>
        /// Unused parameter.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [SerializeField, HideInInspector] private string _user = string.Empty;
        public string User 
        {
            get => _user;
            set => _user = value;
        }
    }
}
