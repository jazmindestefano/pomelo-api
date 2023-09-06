using PomeloAPI.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace PomeloAPI.Services
{
	public class Service_PomeloAPI : IServicePomeloAPI
	{
		private static string _clientid;
		private static string _secretclient;
		private static string _audience;
		private static string _granttype;
		private static string _baseurl;
        private static string _token;


		public Service_PomeloAPI()
		{
			var builder  = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

			_clientid = builder.GetSection("ApiSettings:client_id").Value;
			_secretclient = builder.GetSection("ApiSettings:secret_client").Value;
			_audience = builder.GetSection("ApiSettings:audience").Value;
            _granttype = builder.GetSection("ApiSettings:grant_type").Value;
			_baseurl = builder.GetSection("ApiSettings:base_url").Value;
        }

        public async Task Auth()
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(_baseurl);

                var credentials = new Credential()
                {
                    client_id = "UFqB9x1Q1v7s9iBAO058Vc6m0mXMPGUJ",
                    audience = "https://auth-staging.pomelo.la",
                    client_secret = "vrp-8kfz8HHBrXmScJorx1gZBIBwatgBaq0IGbqad-FyzA4Wr4mbgNq57qX_nkLT",
                    grant_type = "client_credentials"
                };

                var content = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/oauth/token", content);

                if (response.IsSuccessStatusCode)
                {
                    var json_response = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<CredentialResult>(json_response);
                    _token = result.access_token;
                }
                else
                {
         
                    throw new Exception("La solicitud de autenticación no fue exitosa. Código de estado HTTP: " + response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error en la función Auth(): " + ex.Message, ex);
            }
        }

        public async Task <UserData> CreateUser(CreateUserDTO newUser)
		{
			await Auth();

			var client = new HttpClient();

			client.BaseAddress = new Uri(_baseurl);
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var content = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/users/v1", content);

			if (response.IsSuccessStatusCode)
			{
                var json_response = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<GetUserResponse>(json_response);

                return await GetUser(resultado.data.id);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"La solicitud a la API no fue exitosa. Código de estado HTTP: {response.StatusCode}. Mensaje: {errorMessage}");
            }

        

		}

        public async Task<UserData> GetUser(string id)
        {
            await Auth();
            var client = new HttpClient();

            client.BaseAddress = new Uri(_baseurl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await client.GetAsync($"/users/v1/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<GetUserResponse>(json_response);

                return resultado.data;
            }
            else
            {
                throw new Exception("Ocurrió un error en la función getUser" + id + response);

            }

          

        }

        public async Task<List<UserData>> GetUsers()
        {
            List<UserData> lista = new List<UserData>();
            await Auth();

            var client = new HttpClient();

            client.BaseAddress = new Uri(_baseurl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await client.GetAsync("/users/v1");

            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<GetUsersAPIResponse>(json_response);
                
                lista = resultado.data;
            } else
            {
                throw new Exception("La solicitud a la API no fue exitosa. Código de estado HTTP: " + response.StatusCode);

            }

            return lista;
        }
    }
}

