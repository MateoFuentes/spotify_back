using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.core.Entities.Spotify
{
    public class ESpotify
    {
        public string URL { get; set; }
        public string Client_id { get; set; }
        public string Client_secret { get; set; }
    }

    public class AuthSpotify
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
    }
}
