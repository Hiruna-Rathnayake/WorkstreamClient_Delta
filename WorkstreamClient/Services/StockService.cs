using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WorkstreamClient.DTO; // Make sure to reference the correct DTOs here
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace WorkstreamClient.Services
{
    public class StockService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationService _authenticationService; // Add AuthenticationService

        public StockService(HttpClient httpClient, AuthenticationService authenticationService)  // Inject AuthenticationService
        {
            _httpClient = httpClient;
            _authenticationService = authenticationService;  // Initialize AuthenticationService
        }

        // Create Stock
        public async Task<StockReadDTO> CreateStockAsync(StockWriteDTO stockDTO)
        {
            var token = await _authenticationService.GetTokenAsync();

            // Add token to request headers
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("api/stock", stockDTO); // Adjusted API endpoint to "api/stock"

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<StockReadDTO>();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error creating stock: {errorMessage}");
            }
        }

        // Get Stock by ID
        public async Task<StockReadDTO> GetStockByIdAsync(int id)
        {
            var token = await _authenticationService.GetTokenAsync();

            // Add token to request headers
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"api/stock/{id}"); // Adjusted API endpoint to "api/stock/{id}"

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<StockReadDTO>();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error fetching stock with ID {id}: {errorMessage}");
            }
        }

        // Get all Stocks for a Tenant
        public async Task<List<StockReadDTO>> GetAllStocksByTenantIdAsync(int tenantId)
        {
            var token = await _authenticationService.GetTokenAsync();

            // Add token to request headers
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"api/stock/tenant/{tenantId}"); // Adjusted API endpoint to "api/stock/tenant/{tenantId}"

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<StockReadDTO>>();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error fetching stocks for tenant ID {tenantId}: {errorMessage}");
            }
        }

        // Update Stock
        public async Task<StockReadDTO> UpdateStockAsync(int id, StockWriteDTO stockDTO)
        {
            var token = await _authenticationService.GetTokenAsync();

            // Add token to request headers
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync($"api/stock/{id}", stockDTO); // Adjusted API endpoint to "api/stock/{id}"

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<StockReadDTO>();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error updating stock with ID {id}: {errorMessage}");
            }
        }

        // Delete Stock
        public async Task DeleteStockAsync(int id)
        {
            var token = await _authenticationService.GetTokenAsync();

            // Add token to request headers
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"api/stock/{id}"); // Adjusted API endpoint to "api/stock/{id}"

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error deleting stock with ID {id}: {errorMessage}");
            }
        }
    }
}
