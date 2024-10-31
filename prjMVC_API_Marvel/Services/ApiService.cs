using prjMVC_API_Marvel.Models;
using System.Net.Http;
using System.Text.Json;

namespace prjMVC_API_Marvel.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TblAvenger>> GetAvengersAsync()
        {
            var response = await _httpClient.GetAsync("/users");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<TblAvenger>>();
        }

        public async Task<List<TblContact>> GetContactAsync()
        {
            var response = await _httpClient.GetAsync("/contacts");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<TblContact>>();
        }

        public async Task CreateAvengersAsync(TblAvenger newAvenger)
        {
            var response = await _httpClient.PostAsJsonAsync("/users", newAvenger);
            response.EnsureSuccessStatusCode();
        }

        public async Task CreateContactAsync(TblAvenger newContact)
        {
            var response = await _httpClient.PostAsJsonAsync("/users", newContact);
            response.EnsureSuccessStatusCode();
        }
 

//Delete method for TblAvenger
public async Task DeleteAvengersAsync(string username)
    {

        var response = await _httpClient.DeleteAsync($"/users/{username}");
        response.EnsureSuccessStatusCode();
    }

        public async Task <string> LoginAsync(LoginViewModel userLoginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("/login", userLoginDto);

            //Ensure the response is successful
            response.EnsureSuccessStatusCode();

            //Read the response as a string
            var jsonString = await response.Content.ReadAsStringAsync();

            //Deserialize the response to a dictionary
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);

            //Check if the token key exisits (note the lowercase 'token')
            if (result !=null && result.TryGetValue("token", out var token))
            {
                return token; //Return the token form the response
            }
            throw new Exception("Token not found in reponse"); //Throw an exception if the token is not found
        }
    }

}
//for our assignment we need to get a token that returns who the user is that signed in. Token stores the user role