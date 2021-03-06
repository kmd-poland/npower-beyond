using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using npower_beyond.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Humanizer;

namespace npower_beyond.Services
{
    public class RoutePlanService
    {
        private string _mapBoxKey;

        private HttpClient _httpClient = new HttpClient();
       

        private const int maxLat = 52289740;
        private const int minLon = 20844015;
        private const int minLat = 52117538;
        private const int maxLon = 21159038;

        public RoutePlanService(string mapboxkey)
        {
            _mapBoxKey = mapboxkey;

        }


        public async Task<RoutePlan> GetRoutePlan(int numOfEvents, int seed)
        {
             var  random = new Random(seed);

             var visitStartTimes = Enumerable
                 .Range(0, numOfEvents-1)
                 .Scan(((state, item) => state + random.Next(15, 45)), random.Next(15,60))
                 .Select(min => TimeSpan.FromHours(12).Add(TimeSpan.FromMinutes(min)));

            var users = await GetUsers(numOfEvents, seed.ToString());

            var visitTasks =
                visitStartTimes
                 .Select((time, idx) => GetVisit(users.Results[idx], time));

            var results = await Task.WhenAll(visitTasks);
            return new RoutePlan
            {
                Visits = results
            };

        }

        private async Task<Visit> GetVisit(UserData user, TimeSpan startTime)
        {
            var random = new Random((int)startTime.TotalMilliseconds);
            GeocodingResponse place;
            do {
                place = await GetPlace(random);
            } while (!place.Features?.Any() ?? false);

            var addrTruncated = Regex.Replace(place.Features[0].PlaceName, ", wojew.*", String.Empty);
            return new Visit
            {
                StartTime = DateTime.Today.Add(startTime),
                FirstName = user.Name.First.Humanize(LetterCasing.Sentence),
                LastName = user.Name.Last.Humanize(LetterCasing.Sentence),
                Avatar = user.Picture.Large,
                Address = addrTruncated,
                Coordinates = place.Features[0].Center
            };
        }

        private async Task<UserResponse> GetUsers(int num, string seed)
        {
            var url = $"https://randomuser.me/api/?nat=dk&results={num}&seed={seed}";
            var response = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<UserResponse>(response);
        }


        private async Task<GeocodingResponse> GetPlace(Random random)
        {
            var lat = random.Next(minLat, maxLat) / 1000000.0;
            var lon = random.Next(minLon, maxLon) / 1000000.0;

            var url = $"https://api.mapbox.com/geocoding/v5/mapbox.places/{lon},{lat}.json?limit=1&language=pl&access_token={_mapBoxKey}";
            var response = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<GeocodingResponse>(response);
        }
    }
}
