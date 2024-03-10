using UnityEngine;

namespace OobaboogaRuntimeIntegration.OobaboogaConfig
{
    [CreateAssetMenu(fileName = "OobaboogaGenerationParameters", menuName = "Oobabooga/GenerationParameters")]
    public class GenerationParameters : ScriptableObject, IGenerationParameters
    {
        [field: SerializeField, Header("General")] public PresetOption Preset_Option { get; set; }
        
        /// <summary>
        /// Set the Pytorch seed to this number. Note that some loaders do not use Pytorch (notably llama.cpp),
        /// and others are not deterministic (notably ExLlama v1 and v2). For these loaders, the seed has no effect.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField, Space] public int Seed { get; set; } = -1;
        
        /// <summary>
        /// Maximum number of tokens to generate. Don't set it higher than necessary: it is used in the truncation
        /// calculation through the formula (prompt_length) = min(truncation_length - max_new_tokens, prompt_length),
        /// so your prompt will get truncated if you set it too high.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public int Max_Tokens { get; set; } = 512;
        
        /// <summary>
        /// When checked, the max_new_tokens parameter is expanded in the backend to the available context length. The maximum
        /// length is given by the "truncation_length" parameter. This is useful for getting long replies in the Chat tab without
        /// having to click on "Continue" many times.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public bool Auto_Max_New_Tokens { get; set; } = false;

        /// <summary>
        /// Used to prevent the prompt from getting bigger than the model's context length. In the case of the transformers
        /// loader, which allocates memory dynamically, this parameter can also be used to set a VRAM ceiling and prevent
        /// out-of-memory errors. This parameter is automatically updated with the model's context length (from "n_ctx" or
        /// "max_seq_len" for loaders that use these parameters, and from the model metadata directly for loaders that do not)
        /// when you load a model.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public int Truncation_Length { get; set; } = 2048;
        
        /// <summary>
        /// Activates Prompt Lookup Decoding.
        /// https://github.com/oobabooga/text-generation-webui/blob/992affefef7d71544352920720eec08cbf3f56b9/modules/ui_parameters.py#L77
        /// </summary>
        [field: SerializeField] public int Prompt_Lookup_Num_Tokens { get; set; } = 0;

        /// <summary>
        /// One of the possible tokens that a model can generate is the EOS (End of Sequence) token. When it is generated,
        /// the generation stops prematurely. When this parameter is checked, that token is banned from being generated,
        /// and the generation will always generate "max_new_tokens" tokens.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField, Header("Sequence")] public bool Ban_Eos_Token { get; set; } = false;

        /// <summary>
        /// By default, the tokenizer will add a BOS (Beginning of Sequence) token to your prompt. During training, BOS
        /// tokens are used to separate different documents. If unchecked, no BOS token will be added, and the model will
        /// interpret your prompt as being in the middle of a document instead of at the start of one. This significantly
        /// changes the output and can make it more creative.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public bool Add_Bos_Token { get; set; } = true;

        /// <summary>
        /// When decoding the generated tokens, skip special tokens from being converted to their text representation.
        /// Otherwise, BOS appears as <s>, EOS as </s>, etc.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField] public bool Skip_Special_Tokens { get; set; } = true;
        
        /// <summary>
        /// to make text readable in real-time in case the model is generating too fast.
        /// Good if you want to flex and tell everyone how good your GPU is.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField, Header("Utility")] public int Max_Tokens_Second { get; set; } = 0;

        //TODO: might be to long string -> file import
        /// <summary>
        /// Allows you to constrain the model output to a particular format. For instance, you can make the model generate lists,
        /// JSON, specific words, etc. Grammar is extremely powerful and I highly recommend it. The syntax looks a bit daunting
        /// at first sight, but it gets very easy once you understand it.
        /// See the GBNF Guide for details: https://github.com/ggerganov/llama.cpp/blob/master/grammars/README.md
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField, Header("Formatting"), TextArea(3, 50)] public string Grammar_String { get; set; } = string.Empty;
        
        /// <summary>
        /// Only used when guidance_scale != 1. It is most useful for instruct models and custom system messages.
        /// You place your full prompt in this field with the system message replaced with the default one
        /// for the model (like "You are Llama, a helpful assistant...") to make the model pay more attention to your custom system message.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField, Header("Conditional"), TextArea(3, 50)] public string Negative_Prompt { get; set; } = string.Empty;
        
        //TODO: might be to long string -> file import
        /// <summary>
        /// Allows you to ban the model from generating certain tokens altogether. You need to find the token IDs under
        /// "Default" &gt; "Tokens" or "Notebook" &gt; "Tokens", or by looking at the tokenizer.json for the model directly.
        /// https://github.com/oobabooga/text-generation-webui/wiki/03-‐-Parameters-Tab
        /// </summary>
        [field: SerializeField, TextArea(3, 50)] public string Custom_Token_Bans { get; set; } = string.Empty;
    }
}
