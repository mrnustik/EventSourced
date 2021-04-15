using System;
using System.Text;

namespace EventSourced.Diagnostics.Web.Helpers
{
    public static class Base64Encoder
    {
        public static string Encode(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }
        
        public static string Decode(string encodedValue)
        {
            var bytes = Convert.FromBase64String(encodedValue);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}