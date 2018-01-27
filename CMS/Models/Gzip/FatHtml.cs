using System.Web.Mvc;
using System.Text;

namespace CMS.Models.Gzip
{
    public static class FatHtml
    {
        const string keyForScript = "__key_For_Js_StringBuilder";
        const string keyForStyle = "__key_For_Css_StringBuilder";

        
        
        /// <summary> 
        /// Creates a StringBuilder in the ViewDataDictionary. 
        /// Allows access to the StringBuilder class. 
        /// <</summary> 
        /// <param name="key">The key for retrieving the StringBuilder from the ViewDataDictionary.</param> 
        /// <param name="delimBetweenStrings">Char to delimit string values. 
        static void AddToStringBuilder(this ViewDataDictionary dictionary, string key, string addString, char delimBetweenStrings)
        {
            StringBuilder str;
            object viewDataObject;

            if (!dictionary.TryGetValue(key, out viewDataObject))
            {
                str = new StringBuilder();
                str.Append(addString);
                dictionary[key] = str;
            }
            else
            {
                str = viewDataObject as StringBuilder;
                str.Append(delimBetweenStrings);
                str.Append(addString);
            }
        }

        /// <summary> 
        /// Add a script to the list of scripts. 
        /// </summary> 
        public static void Script(this HtmlHelper html, string path)
        {
            html.ViewData.AddToStringBuilder(keyForScript, path, '|');
        }

        /// <summary> 
        /// Add a style to the list of styles. 
        /// <</summary> 
        public static void Style(this HtmlHelper html, string path)
        {           
            html.ViewData.AddToStringBuilder(keyForStyle, path, '|');
        }

        /// <summary> 
        /// Grabs the list of scripts stored in ViewData. 
        /// Converts script list into a "Zipped" script tag. 
        /// </summary> 
        public static string CompressJs(this HtmlHelper html, UrlHelper url)
        {
           
        
          
            //get the builder from ViewData 
            var builder = html.ViewData[keyForScript] as StringBuilder;

            //create the url 
            string urlPath = url.Action("Script", "Zip", new { Path = builder.ToString() });

            //return the script tag 
            return string.Format(@"<script type=""text/javascript"" src=""{0}""></script>", urlPath);
        }
        
       

        /// <summary> 
        /// Grabs the style StringBuilder from the ViewData dictionary. 
        /// Converts style list into a "Zipped" style tag. 
        /// </summary> 
        public static string CompressCss(this HtmlHelper html, UrlHelper url)
        {          

            //get the builder from ViewData 
            var builder = html.ViewData[keyForStyle] as StringBuilder;

            //create the url 
            string urlPath = url.Action("Style", "Zip", new { Path = builder.ToString() });

            //return the style tag 
            return string.Format(@"<link href=""{0}"" type=""text/css"" rel=""stylesheet"" />", urlPath);
        }
    }

    
}