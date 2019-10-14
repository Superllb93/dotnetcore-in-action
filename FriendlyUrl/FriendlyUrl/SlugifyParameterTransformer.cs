using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;

namespace FriendlyUrl
{
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            return null == value
                ? null
                : Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}
