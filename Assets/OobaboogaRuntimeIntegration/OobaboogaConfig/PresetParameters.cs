using System;
using UnityEngine;

namespace OobaboogaRuntimeIntegration.OobaboogaConfig
{
    [CreateAssetMenu(fileName = "OobaboogaPresetParameters", menuName = "Oobabooga/Preset/Parameters")]
    public class PresetParameters : ScriptableObject, IPresetParameters
    {
        /// <summary>
        /// Primary factor to control the randomness of outputs. 0 = deterministic (only the most likely token is used). Higher value = more randomness.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public float Temperature { get; set; } = 1;

        /// <summary>
        /// If not set to 1, select tokens with probabilities adding up to less than this number. Higher value = higher range of possible random results.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public float Top_P { get; set; } = 1;
        
        /// <summary>
        /// Tokens with probability smaller than (min_p) * (probability of the most likely token) are discarded. This is the same as top_a but without squaring the probability.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public float Min_P { get; set; } = 0;
        
        /// <summary>
        /// Similar to top_p, but select instead only the top_k most likely tokens. Higher value = higher range of possible random results.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public int Top_K { get; set; } = 0;
        
        /// <summary>
        /// Penalty factor for repeating prior tokens. 1 means no penalty, higher value = less repetition, lower value = more repetition.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public float Repetition_Penalty { get; set; } = 1;
        
        /// <summary>
        /// repetition_penalty: Penalty factor for repeating prior tokens. 1 means no penalty, higher value = less repetition, lower value = more repetition.
        /// Similar to repetition_penalty, but with an additive offset on the raw token scores instead of a multiplicative factor.
        /// It may generate better results. 0 means no penalty, higher value = less repetition, lower value = more repetition.
        /// Previously called "additive_repetition_penalty".
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public int Presence_Penalty { get; set; } = 0;
        
        /// <summary>
        /// Repetition penalty that scales based on how many times the token has appeared in the context.
        /// Be careful with this; there's no limit to how much a token can be penalized.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public int Frequency_Penalty { get; set; } = 0;
        
        /// <summary>
        /// The number of most recent tokens to consider for repetition penalty. 0 makes all tokens be used.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public int Repetition_Penalty_Range { get; set; } = 1024;
        
        /// <summary>
        /// If not set to 1, select only tokens that are at least this much more likely to appear than random tokens, given the prior text.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public float Typical_P { get; set; } = 1;
        
        /// <summary>
        /// Tries to detect a tail of low-probability tokens in the distribution and removes those tokens.
        /// See this blog post for details: https://www.trentonbricken.com/Tail-Free-Sampling/
        /// The closer to 0, the more discarded tokens.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public float Tfs { get; set; } = 1;
        
        /// <summary>
        /// Tokens with probability smaller than (top_a) * (probability of the most likely token)^2 are discarded.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public int Top_A { get; set; } = 0;
        
        /// <summary>
        /// In units of 1e-4; a reasonable value is 3. This sets a probability floor below which tokens are excluded from being sampled.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public float Epsilon_Cutoff { get; set; } = 0;
        
        /// <summary>
        /// In units of 1e-4; a reasonable value is 3. The main parameter of the special Eta Sampling technique.
        /// See this paper for a description: https://arxiv.org/pdf/2210.15191.pdf
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public float Eta_Cutoff { get; set; } = 0;
        
        /// <summary>
        /// The main parameter for Classifier-Free Guidance (CFG).
        /// The paper suggests that 1.5 is a good value: https://arxiv.org/pdf/2306.17806.pdf
        /// It can be used in conjunction with a negative prompt or not.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public float Guidance_Scale { get; set; } = 1;
    
        /// <summary>
        /// Contrastive Search is enabled by setting this to greater than zero and unchecking "do_sample".
        /// It should be used with a low value of top_k, for instance, top_k = 4.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public float Penalty_Alpha { get; set; } = 0;
        
        /// <summary>
        /// Activates the Mirostat sampling technique. It aims to control perplexity during sampling.
        /// See the paper: https://arxiv.org/abs/2007.14966
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public int Mirostat_Mode { get; set; } = 0;
    
        /// <summary>
        /// No idea, see the paper for details: https://arxiv.org/abs/2007.14966
        /// According to the Preset Arena, 8 is a good value.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public float Mirostat_Tau { get; set; } = 5;
    
        /// <summary>
        /// No idea, see the paper for details: https://arxiv.org/abs/2007.14966
        /// According to the Preset Arena, 0.1 is a good value.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public float Mirostat_Eta { get; set; } = 0.1f;
        
        /// <summary>
        /// Activates Quadratic Sampling. When `0 &lt; smoothing_factor &lt; 1`, the logits distribution becomes flatter. When `smoothing_factor &gt; 1`, it becomes more peaked.
        /// https://github.com/oobabooga/text-generation-webui/blob/992affefef7d71544352920720eec08cbf3f56b9/docs/03%20-%20Parameters%20Tab.md?plain=1#L58
        /// </summary>
        [field: SerializeField] public float Smoothing_Factor { get; set; } = 0;
        
        /// <summary>
        /// Adjusts the dropoff curve of Quadratic Sampling.
        /// </summary>
        [field: SerializeField] public float Smoothing_Curve { get; set; } = 1f;
        
        /// <summary>
        /// Activates Dynamic Temperature. This modifies temperature to range between "dynatemp_low" (minimum)
        /// and "dynatemp_high" (maximum), with an entropy-based scaling. The steepness of the curve is controlled by "dynatemp_exponent".
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public bool Dynamic_Temperature { get; set; } = false;
        
        /// <summary>
        /// Activates Dynamic Temperature. This modifies temperature to range between "dynatemp_low" (minimum)
        /// and "dynatemp_high" (maximum), with an entropy-based scaling. The steepness of the curve is controlled by "dynatemp_exponent".
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public float Dynatemp_Low { get; set; } = 1;
    
        /// <summary>
        /// Activates Dynamic Temperature. This modifies temperature to range between "dynatemp_low" (minimum)
        /// and "dynatemp_high" (maximum), with an entropy-based scaling. The steepness of the curve is controlled by "dynatemp_exponent".
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public float Dynatemp_High { get; set; } = 1;
    
        /// <summary>
        /// Activates Dynamic Temperature. This modifies temperature to range between "dynatemp_low" (minimum)
        /// and "dynatemp_high" (maximum), with an entropy-based scaling. The steepness of the curve is controlled by "dynatemp_exponent".
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public float DynaTemp_Exponent { get; set; } = 1;
        
        /// <summary>
        /// Makes temperature the last sampler instead of the first. With this, you can remove low probability tokens
        /// with a sampler like min_p and then use a high temperature to make the model creative without losing coherency.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public bool Temperature_Last { get; set; } = false;
        
        /// <summary>
        /// When unchecked, sampling is entirely disabled, and greedy decoding is used instead (the most likely token is always picked).
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public bool Do_Sample { get; set; } = true;
    
        /// <summary>
        /// Also known as the "Hallucinations filter". Used to penalize tokens that are not in the prior text.
        /// Higher value = more likely to stay in context, lower value = more likely to diverge.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public float Encoder_Repetition_Penalty { get; set; } = 1;
    
        /// <summary>
        /// If not set to 0, specifies the length of token sets that are completely blocked from repeating at all.
        /// Higher values = blocks larger phrases, lower values = blocks words or letters from repeating.
        /// Only 0 or high values are a good idea in most cases.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public int No_Repeat_Ngram_Size { get; set; } = 0;
    
        /// <summary>
        /// Minimum generation length in tokens. This is a built-in parameter in the transformers library that has never
        /// been very useful. Typically you want to check "Ban the eos_token" instead.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public int Min_Length { get; set; } = 1;
    
        /// <summary>
        /// Number of beams for beam search. 1 means no beam search.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public int Num_Beams { get; set; } = 1;
    
        /// <summary>
        /// Used by beam search only. length_penalty &gt; 0.0 promotes longer sequences,
        /// while length_penalty &lt; 0.0 encourages shorter sequences.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public float Length_Penalty { get; set; } = 1;
    
        /// <summary>
        /// Used by beam search only. When checked, the generation stops as soon as there are "num_beams"
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public bool Early_Stopping { get; set; } = false;
    
        /// <summary>
        /// List of samplers where the first items will appear first in the stack. Example: [\"top_k\", \"temperature\", \"top_p\"].
        /// https://github.com/oobabooga/text-generation-webui/blob/main/extensions/openai/typing.py
        /// </summary>
        [field: SerializeField] public string[] Sampler_Priority { get; set; } = {
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
    }
}
