using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VocaDbToIcal.AspNet {

	public class JsonHttpClient {

		public static async Task<T> GetObject<T>(string url) {

			var client = new HttpClient();
			var result = await client.GetAsync(url);

			result.EnsureSuccessStatusCode();

			var str = await result.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<T>(str);

		}
	}

}
