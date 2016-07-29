using System;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Resources.Core;

namespace Imedit.Helpers
{
    public class ResourceHelper
    {
        public static string GetTranslation(string value)
        {
            const string keyFormat = "Resources/{0}";

            try
            {
                var v = ResourceManager.Current.MainResourceMap.FirstOrDefault(x => x.Key == string.Format(keyFormat, value));
                return v.Value.Candidates[0].ValueAsString;
            }
            catch (Exception ex)
            {
                Debug.Write("Error in GetTranslation. Reason: " + ex.Message);
            }

            return "?????" + (value ?? "");
        }
    }
}