namespace Features.Connection.UI
{
    public static class TextFormattingHelper
    {
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
