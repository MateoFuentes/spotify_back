using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.core.Entities.Rta;
using test.core.Entities.Spotify;

namespace test.core.Interface.Spotify
{
    public interface ISpotify
    {
        Task<ERta> Search(string q, string type, string token);
    }
}
