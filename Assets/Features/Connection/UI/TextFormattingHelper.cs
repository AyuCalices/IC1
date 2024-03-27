namespace Features.Connection.UI
{
    public static class TextFormattingHelper
    {
        public static string FormatTextQuotation(string originalText, char separator, string prefix, string suffix)
        {
            string[] parts = originalText.Split(separator);
            string formattedText = "";

            for (int i = 0; i < parts.Length; i++)
            {
                if (string.IsNullOrEmpty(parts[i])) continue;
                
                if (i % 2 == 0)
                {
                    formattedText += "" + parts[i].Trim() + "\n\n";
                }
                else
                {
                    formattedText += prefix + parts[i].Trim() + suffix + "\n\n";
                }
            }

            return formattedText;
        }
        
        public static string AddIfLastNotSlash(this string text)
        {
            if (!text.EndsWith("\\") || !text.EndsWith("/"))
            {
                return text + "\\";
            }
            
            return text;
        }
        
        public static string RemoveIfLastSlash(this string text)
        {
            if (text.EndsWith("\\") || text.EndsWith("/"))
            {
                return text.Remove(text.Length - 1);
            }
            
            return  text;
        }
        
        public static string AddIfFirstNotSlash(this string text)
        {
            if (!text.StartsWith("\\") || !text.StartsWith("/"))
            {
                return text + "\\";
            }
            
            return  text;
        }
        
        public static string RemoveIfFirstSlash(this string text)
        {
            if (text.StartsWith("\\") || text.StartsWith("/"))
            {
                return text.Remove(0);
            }

            return text;
        }
    }
}
