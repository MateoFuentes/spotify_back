using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using test.core.Entities.Rta;
using test.core.Entities.Spotify;
using test.core.Interface.Spotify;

namespace test.infraestructure.Spotify
{
    public class SpotifyConsums : ISpotify
    {
        private readonly ESpotify _spotify;

        public SpotifyConsums(IOptions<ESpotify> spotify)
        {
            _spotify = spotify.Value;
        }

        #region Search
        public async Task<ERta> Search(string q, string type, string token)
        {
            ERta rta = new ERta();
            try
            {
                using var httpClient = new HttpClient();

                // Construir la URL de la solicitud
                var url = $"{_spotify.URL}?q={q}&type={type}&market=CO&limit=50&offset=0";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Enviar la solicitud HTTP y procesar la respuesta
                using (var response = await httpClient.SendAsync(request))
                {
                    // Verificar si la solicitud fue exitosa
                    response.EnsureSuccessStatusCode();
                    rta.Status = (int)response.StatusCode;

                    if (response.IsSuccessStatusCode)
                    {
                        // Leer el contenido de la respuesta como cadena JSON
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var responseObj = JObject.Parse(jsonResponse);
                        List<JObject> nuevoObj = new List<JObject>();

                        JArray items = (JArray)responseObj[$"{type}s"]["items"];

                        // Definir las propiedades a extraer dependiendo del tipo
                        List<string> properties = type switch
                        {
                            "track" => new List<string> { "duration_ms", "name", "album", "type" },
                            "artist" => new List<string> { "followers", "popularity", "images", "genres", "name", "type" },
                            "album" => new List<string> { "album_type", "total_tracks", "images", "available_markets", "name", "type", "release_date", "release_date_precision", "artists" },
                            _ => throw new ArgumentException($"Invalid type: {type}")
                        };

                        // Iterar sobre cada elemento y extraer las propiedades requeridas
                        foreach (JObject item in items)
                        {
                            JObject obj = new JObject();

                            foreach (JProperty property in item.Properties())
                            {
                                if (properties.Contains(property.Name))
                                {
                                    // Tratar casos especiales como imágenes y artistas
                                    if (property.Name == "album" && item[property.Name] is JObject album)
                                    {
                                        if (album["images"] is JArray images && images.Count > 0)
                                        {
                                            var img = images[0];
                                            obj.Add("image", img);
                                        }
                                    }
                                    else if (property.Name == "images" && item["images"] is JArray images && images.Count > 0)
                                    {
                                        var img = images[0];
                                        obj.Add("image", img);
                                    }
                                    else if (property.Name == "artists" && item["artists"] is JArray artists && artists.Count > 0)
                                    {
                                        var artistNames = artists.Select(artist => artist.Value<string>("name")).ToArray();
                                        obj.Add("artist", String.Join(",", artistNames));
                                    }
                                    else
                                    {
                                        // Agregar la propiedad al objeto resultante
                                        obj.Add(property.Name, property.Value);
                                    }
                                }
                            }
                            // Agregar el objeto resultante a la lista
                            nuevoObj.Add(obj);
                        }
                        // Serializar la lista de objetos JSON resultantes
                        rta.Data = JsonConvert.SerializeObject(nuevoObj);
                    }

                    return rta;
                }
            }
            catch (Exception ex)
            {
                rta.Message = ex.Message;
                return rta;
            }
        }
        #endregion
    }
}
