namespace personal_blog.core.Common.Helpers;

using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

public static class SlugGenHelper
{
    public static string GenerateSlug(string phrase)
    {
        string str = phrase.ToLower(CultureInfo.InvariantCulture);
        
        str = RemoveDiacritics(str);
        
        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
        
        str = Regex.Replace(str, @"\s+", " ").Trim();
        
        str = Regex.Replace(str, @"\s", "-");
        
        str = Regex.Replace(str, @"-+", "-");

        return str;
    }

    private static string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in from c in normalizedString 
                 let unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c) 
                 where unicodeCategory != UnicodeCategory.NonSpacingMark
                 select c)
        {
            stringBuilder.Append(c);
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
}