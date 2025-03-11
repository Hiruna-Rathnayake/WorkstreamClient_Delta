using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WorkstreamClient.DTO;  // Ensure DTOs are referenced
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace WorkstreamClient.Services
{
    public class CustomerService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationService _authenticationService;  // For handling JWT token

        public CustomerService(HttpClient httpClient, AuthenticationService authenticationService)
        {
            _httpClient = httpClient;
            _authenticationService = authenticationService;  // Initialize AuthenticationService
        }

        // Create a new Customer
        public async Task<CustomerReadDTO> CreateCustomerAsync(CustomerWriteDTO customerDTO)
        {
            var token = await _authenticationService.GetTokenAsync();

            // Add token to request headers
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("api/customer", customerDTO);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CustomerReadDTO>();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error creating customer: {errorMessage}");
            }
        }

        // Get Customer by ID
        public async Task<CustomerReadDTO> GetCustomerByIdAsync(int customerId)
        {
            var token = await _authenticationService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"api/customer/{customerId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CustomerReadDTO>();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error retrieving customer: {errorMessage}");
            }
        }

        // Get All Customers for a Tenant
        public async Task<List<CustomerReadDTO>> GetAllCustomersAsync()
        {
            var token = await _authenticationService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("api/customer");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CustomerReadDTO>>();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error retrieving customers: {errorMessage}");
            }
        }

        // Update a Customer
        public async Task<CustomerReadDTO> UpdateCustomerAsync(int customerId, CustomerWriteDTO customerDTO)
        {
            var token = await _authenticationService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync($"api/customer/{customerId}", customerDTO);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CustomerReadDTO>();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error updating customer: {errorMessage}");
            }
        }

        // Soft delete a Customer
        public async Task DeleteCustomerAsync(int customerId)
        {
            var token = await _authenticationService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"api/customer/{customerId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error deleting customer: {errorMessage}");
            }
        }
    }
}
