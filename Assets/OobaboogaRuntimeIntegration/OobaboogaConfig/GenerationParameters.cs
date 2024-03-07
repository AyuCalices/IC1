using UnityEngine;

namespace OobaboogaRuntimeIntegration.OobaboogaConfig
{
    [CreateAssetMenu(fileName = "OoobaboogaGenerationParameters", menuName = "Ooobabooga/GenerationParameters")]
    public class GenerationParameters : ScriptableObject, IGenerationParameters
    {
        /// <summary>
        /// The name of a file under text-generation-webui/presets (without the .yaml extension).
        /// The sampling parameters that get overwritten by this option are the keys in the default_preset() function in modules/presets.py.
        /// https://github.com/oobabooga/text-generation-webui/blob/main/modules/presets.py
        /// </summary>
        [SerializeField] private string _preset = string.Empty;
        public string Preset
        {
            get => _preset;
            set => _preset = value;
        }

        /// <summary>
        /// Tokens with probability smaller than (min_p) * (probability of the most likely token) are discarded. This is the same as top_a but without squaring the probability.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private float _minP = 0;
        public float Min_P 
        {
            get => _minP;
            set => _minP = value;
        }

        /// <summary>
        /// Activates Dynamic Temperature. This modifies temperature to range between "dynatemp_low" (minimum)
        /// and "dynatemp_high" (maximum), with an entropy-based scaling. The steepness of the curve is controlled by "dynatemp_exponent".
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [Space, SerializeField] private bool _dynamicTemperature = false;
        public bool Dynamic_Temperature 
        {
            get => _dynamicTemperature;
            set => _dynamicTemperature = value;
        }

        /// <summary>
        /// Activates Dynamic Temperature. This modifies temperature to range between "dynatemp_low" (minimum)
        /// and "dynatemp_high" (maximum), with an entropy-based scaling. The steepness of the curve is controlled by "dynatemp_exponent".
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private float _dynatempLow = 1;
        public float Dynatemp_Low 
        {
            get => _dynatempLow;
            set => _dynatempLow = value;
        }

        /// <summary>
        /// Activates Dynamic Temperature. This modifies temperature to range between "dynatemp_low" (minimum)
        /// and "dynatemp_high" (maximum), with an entropy-based scaling. The steepness of the curve is controlled by "dynatemp_exponent".
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private float _dynatempHigh = 1;
        public float Dynatemp_High 
        {
            get => _dynatempHigh;
            set => _dynatempHigh = value;
        }

        /// <summary>
        /// Activates Dynamic Temperature. This modifies temperature to range between "dynatemp_low" (minimum)
        /// and "dynatemp_high" (maximum), with an entropy-based scaling. The steepness of the curve is controlled by "dynatemp_exponent".
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private float _dynatempExponent = 1;
        public float DynaTemp_Exponent 
        {
            get => _dynatempExponent;
            set => _dynatempExponent = value;
        }

        /// <summary>
        /// Activates Quadratic Sampling. When `0 &lt; smoothing_factor &lt; 1`, the logits distribution becomes flatter. When `smoothing_factor &gt; 1`, it becomes more peaked.
        /// https://github.com/oobabooga/text-generation-webui/blob/992affefef7d71544352920720eec08cbf3f56b9/docs/03%20-%20Parameters%20Tab.md?plain=1#L58
        /// </summary>
        [Space, SerializeField] private float _smoothingFactor = 0;
        public float Smoothing_Factor 
        {
            get => _smoothingFactor;
            set => _smoothingFactor = value;
        }

        /// <summary>
        /// Similar to top_p, but select instead only the top_k most likely tokens. Higher value = higher range of possible random results.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private int _topK = 0;
        public int Top_K 
        {
            get => _topK;
            set => _topK = value;
        }

        /// <summary>
        /// Penalty factor for repeating prior tokens. 1 means no penalty, higher value = less repetition, lower value = more repetition.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private float _repetitionPenalty = 1;
        public float Repetition_Penalty 
        {
            get => _repetitionPenalty;
            set => _repetitionPenalty = value;
        }

        /// <summary>
        /// The number of most recent tokens to consider for repetition penalty. 0 makes all tokens be used.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private int _repetitionPenaltyRange = 1024;
        public int Repetition_Penalty_Range 
        {
            get => _repetitionPenaltyRange;
            set => _repetitionPenaltyRange = value;
        }

        /// <summary>
        /// If not set to 1, select only tokens that are at least this much more likely to appear than random tokens, given the prior text.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private float _typicalP = 1;
        public float Typical_P 
        {
            get => _typicalP;
            set => _typicalP = value;
        }

        /// <summary>
        /// Tries to detect a tail of low-probability tokens in the distribution and removes those tokens.
        /// See this blog post for details: https://www.trentonbricken.com/Tail-Free-Sampling/
        /// The closer to 0, the more discarded tokens.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private float _tfs = 1;
        public float Tfs 
        {
            get => _tfs;
            set => _tfs = value;
        }

        /// <summary>
        /// Tokens with probability smaller than (top_a) * (probability of the most likely token)^2 are discarded.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private int _topA = 0;
        public int Top_A 
        {
            get => _topA;
            set => _topA = value;
        }

        /// <summary>
        /// In units of 1e-4; a reasonable value is 3. This sets a probability floor below which tokens are excluded from being sampled.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private float _epsilonCutoff = 0 ;
        public float Epsilon_Cutoff 
        {
            get => _epsilonCutoff;
            set => _epsilonCutoff = value;
        }

        /// <summary>
        /// In units of 1e-4; a reasonable value is 3. The main parameter of the special Eta Sampling technique.
        /// See this paper for a description: https://arxiv.org/pdf/2210.15191.pdf
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private float _etaCutoff = 0;
        public float Eta_Cutoff 
        {
            get => _etaCutoff;
            set => _etaCutoff = value;
        }

        /// <summary>
        /// The main parameter for Classifier-Free Guidance (CFG).
        /// The paper suggests that 1.5 is a good value: https://arxiv.org/pdf/2306.17806.pdf
        /// It can be used in conjunction with a negative prompt or not.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private float _guidanceScale = 1;
        public float Guidance_Scale 
        {
            get => _guidanceScale;
            set => _guidanceScale = value;
        }

        /// <summary>
        /// Only used when guidance_scale != 1. It is most useful for instruct models and custom system messages.
        /// You place your full prompt in this field with the system message replaced with the default one
        /// for the model (like "You are Llama, a helpful assistant...") to make the model pay more attention to your custom system message.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private string _negativePrompt = string.Empty;
        public string Negative_Prompt 
        {
            get => _negativePrompt;
            set => _negativePrompt = value;
        }

        /// <summary>
        /// Contrastive Search is enabled by setting this to greater than zero and unchecking "do_sample".
        /// It should be used with a low value of top_k, for instance, top_k = 4.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private float _penaltyAlpha = 0;
        public float Penalty_Alpha 
        {
            get => _penaltyAlpha;
            set => _penaltyAlpha = value;
        }

        /// <summary>
        /// Activates the Mirostat sampling technique. It aims to control perplexity during sampling.
        /// See the paper: https://arxiv.org/abs/2007.14966
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private int _mirostatMode = 0;
        public int Mirostat_Mode 
        {
            get => _mirostatMode;
            set => _mirostatMode = value;
        }

        /// <summary>
        /// No idea, see the paper for details: https://arxiv.org/abs/2007.14966
        /// According to the Preset Arena, 8 is a good value.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private float _mirostatTau = 5;
        public float Mirostat_Tau 
        {
            get => _mirostatTau;
            set => _mirostatTau = value;
        }

        /// <summary>
        /// No idea, see the paper for details: https://arxiv.org/abs/2007.14966
        /// According to the Preset Arena, 0.1 is a good value.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private float _mirostatEta = 0.1f;
        public float Mirostat_Eta 
        {
            get => _mirostatEta;
            set => _mirostatEta = value;
        }

        /// <summary>
        /// Makes temperature the last sampler instead of the first. With this, you can remove low probability tokens
        /// with a sampler like min_p and then use a high temperature to make the model creative without losing coherency.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private bool _temperatureLast = false;
        public bool Temperature_Last 
        {
            get => _temperatureLast;
            set => _temperatureLast = value;
        }

        /// <summary>
        /// When unchecked, sampling is entirely disabled, and greedy decoding is used instead (the most likely token is always picked).
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private bool _doSample = true;
        public bool Do_Sample 
        {
            get => _doSample;
            set => _doSample = value;
        }

        /// <summary>
        /// Set the Pytorch seed to this number. Note that some loaders do not use Pytorch (notably llama.cpp),
        /// and others are not deterministic (notably ExLlama v1 and v2). For these loaders, the seed has no effect.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private int _seed = -1;
        public int Seed 
        {
            get => _seed;
            set => _seed = value;
        }

        /// <summary>
        /// Also known as the "Hallucinations filter". Used to penalize tokens that are not in the prior text.
        /// Higher value = more likely to stay in context, lower value = more likely to diverge.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private float _encoderRepetitionPenalty = 1;
        public float Encoder_Repetition_Penalty 
        {
            get => _encoderRepetitionPenalty;
            set => _encoderRepetitionPenalty = value;
        }

        /// <summary>
        /// If not set to 0, specifies the length of token sets that are completely blocked from repeating at all.
        /// Higher values = blocks larger phrases, lower values = blocks words or letters from repeating.
        /// Only 0 or high values are a good idea in most cases.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private int _noRepeatNgramSize = 0;
        public int No_Repeat_Ngram_Size 
        {
            get => _noRepeatNgramSize;
            set => _noRepeatNgramSize = value;
        }

        /// <summary>
        /// Minimum generation length in tokens. This is a built-in parameter in the transformers library that has never
        /// been very useful. Typically you want to check "Ban the eos_token" instead.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private int _minLength = 1;
        public int Min_Length 
        {
            get => _minLength;
            set => _minLength = value;
        }

        /// <summary>
        /// Number of beams for beam search. 1 means no beam search.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private int _numBeams = 1;
        public int Num_Beams 
        {
            get => _numBeams;
            set => _numBeams = value;
        }

        /// <summary>
        /// Used by beam search only. length_penalty &gt; 0.0 promotes longer sequences,
        /// while length_penalty &lt; 0.0 encourages shorter sequences.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private float _lengthPenalty = 1;
        public float Length_Penalty 
        {
            get => _lengthPenalty;
            set => _lengthPenalty = value;
        }

        /// <summary>
        /// Used by beam search only. When checked, the generation stops as soon as there are "num_beams"
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private bool _earlyStopping = false;
        public bool Early_Stopping 
        {
            get => _earlyStopping;
            set => _earlyStopping = value;
        }

        /// <summary>
        /// Used to prevent the prompt from getting bigger than the model's context length. In the case of the transformers
        /// loader, which allocates memory dynamically, this parameter can also be used to set a VRAM ceiling and prevent
        /// out-of-memory errors. This parameter is automatically updated with the model's context length (from "n_ctx" or
        /// "max_seq_len" for loaders that use these parameters, and from the model metadata directly for loaders that do not)
        /// when you load a model.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private int _truncationLength = 0;
        public int Truncation_Length 
        {
            get => _truncationLength;
            set => _truncationLength = value;
        }

        /// <summary>
        /// to make text readable in real-time in case the model is generating too fast.
        /// Good if you want to flex and tell everyone how good your GPU is.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private int _maxTokensSecond = 0;
        public int Max_Tokens_Second 
        {
            get => _maxTokensSecond;
            set => _maxTokensSecond = value;
        }

        /// <summary>
        /// Activates Prompt Lookup Decoding.
        /// https://github.com/oobabooga/text-generation-webui/blob/992affefef7d71544352920720eec08cbf3f56b9/modules/ui_parameters.py#L77
        /// </summary>
        [SerializeField] private int _promptLookupNumTokens = 0;
        public int Prompt_Lookup_Num_Tokens 
        {
            get => _promptLookupNumTokens;
            set => _promptLookupNumTokens = value;
        }

        //TODO: might be to long string -> file import
        /// <summary>
        /// Allows you to ban the model from generating certain tokens altogether. You need to find the token IDs under
        /// "Default" &gt; "Tokens" or "Notebook" &gt; "Tokens", or by looking at the tokenizer.json for the model directly.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private string _customTokenBans = string.Empty;
        public string Custom_Token_Bans 
        {
            get => _customTokenBans;
            set => _customTokenBans = value;
        }

        /// <summary>
        /// List of samplers where the first items will appear first in the stack. Example: [\"top_k\", \"temperature\", \"top_p\"].
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [SerializeField] private string[] _samplerPriority = {
            "temperature", 
            "dynamic_temperature",
            "quadratic_sampling",
            "top_k",
            "top_p",
            "typical_p",
            "epsilon_cutoff",
            "eta_cutoff",
            "tfs",
            "top_a",
            "min_p",
            "mirostat"
        };
        public string[] Sampler_Priority 
        {
            get => _samplerPriority;
            set => _samplerPriority = value;
        }

        /// <summary>
        /// When checked, the max_new_tokens parameter is expanded in the backend to the available context length. The maximum
        /// length is given by the "truncation_length" parameter. This is useful for getting long replies in the Chat tab without
        /// having to click on "Continue" many times.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private bool _autoMaxNewTokens = false;
        public bool Auto_Max_New_Tokens 
        {
            get => _autoMaxNewTokens;
            set => _autoMaxNewTokens = value;
        }

        /// <summary>
        /// One of the possible tokens that a model can generate is the EOS (End of Sequence) token. When it is generated,
        /// the generation stops prematurely. When this parameter is checked, that token is banned from being generated,
        /// and the generation will always generate "max_new_tokens" tokens.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private bool _banEosToken = false;
        public bool Ban_Eos_Token 
        {
            get => _banEosToken;
            set => _banEosToken = value;
        }

        /// <summary>
        /// By default, the tokenizer will add a BOS (Beginning of Sequence) token to your prompt. During training, BOS
        /// tokens are used to separate different documents. If unchecked, no BOS token will be added, and the model will
        /// interpret your prompt as being in the middle of a document instead of at the start of one. This significantly
        /// changes the output and can make it more creative.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private bool _addBosToken = true;
        public bool Add_Bos_Token 
        {
            get => _addBosToken;
            set => _addBosToken = value;
        }

        /// <summary>
        /// When decoding the generated tokens, skip special tokens from being converted to their text representation.
        /// Otherwise, BOS appears as <s>, EOS as </s>, etc.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private bool _skipSpecialTokens = true;
        public bool Skip_Special_Tokens 
        {
            get => _skipSpecialTokens;
            set => _skipSpecialTokens = value;
        }

        //TODO: might be to long string -> file import
        /// <summary>
        /// Allows you to constrain the model output to a particular format. For instance, you can make the model generate lists,
        /// JSON, specific words, etc. Grammar is extremely powerful and I highly recommend it. The syntax looks a bit daunting
        /// at first sight, but it gets very easy once you understand it.
        /// See the GBNF Guide for details: https://github.com/ggerganov/llama.cpp/blob/master/grammars/README.md
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [SerializeField] private string _grammarString = string.Empty;
        public string Grammar_String 
        {
            get => _grammarString;
            set => _grammarString = value;
        }
    }
}
