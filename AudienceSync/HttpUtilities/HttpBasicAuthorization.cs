using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AudienceSync.HttpUtilities
{
    public class HttpBasicAuthorization
    {
        string userName = "{sss}";
        string passwd = "{sss}";

        public string GetAuthToken()
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes($"{userName}:{passwd}"));
        }
    }
}
