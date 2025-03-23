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

        // Helper method to handle HTTP responses
        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    throw new PermissionDeniedException($"Permission denied: {errorMessage}");
                }
                throw new Exception($"Error: {errorMessage}");
            }
        }

        // Create a new Customer
        public async Task<CustomerReadDTO> CreateCustomerAsync(CustomerWriteDTO customerDTO)
        {
            try
            {
                var token = await _authenticationService.GetTokenAsync();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsJsonAsync("api/customer", customerDTO);

                return await HandleResponse<CustomerReadDTO>(response);
            }
            catch (PermissionDeniedException ex)
            {
                // Handle PermissionDeniedException
                throw new PermissionDeniedException($"Permission denied while creating customer: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle all other types of exceptions
                throw new Exception($"An error occurred while creating the customer: {ex.Message}");
            }
        }

        // Get Customer by ID
        public async Task<CustomerReadDTO> GetCustomerByIdAsync(int customerId)
        {
            try
            {
                var token = await _authenticationService.GetTokenAsync();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"api/customer/{customerId}");

                return await HandleResponse<CustomerReadDTO>(response);
            }
            catch (PermissionDeniedException ex)
            {
                // Handle PermissionDeniedException
                throw new PermissionDeniedException($"Permission denied while retrieving customer: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle all other types of exceptions
                throw new Exception($"An error occurred while retrieving the customer: {ex.Message}");
            }
        }

        // Get All Customers for a Tenant
        public async Task<List<CustomerReadDTO>> GetAllCustomersAsync()
        {
            try
            {
                var token = await _authenticationService.GetTokenAsync();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("api/customer");

                return await HandleResponse<List<CustomerReadDTO>>(response);
            }
            catch (PermissionDeniedException ex)
            {
                // Handle PermissionDeniedException
                throw new PermissionDeniedException($"Permission denied while retrieving customers: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle all other types of exceptions
                throw new Exception($"An error occurred while retrieving customers: {ex.Message}");
            }
        }

        // Update a Customer
        public async Task<CustomerReadDTO> UpdateCustomerAsync(int customerId, CustomerWriteDTO customerDTO)
        {
            try
            {
                var token = await _authenticationService.GetTokenAsync();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PutAsJsonAsync($"api/customer/{customerId}", customerDTO);

                return await HandleResponse<CustomerReadDTO>(response);
            }
            catch (PermissionDeniedException ex)
            {
                // Handle PermissionDeniedException
                throw new PermissionDeniedException($"Permission denied while updating customer: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle all other types of exceptions
                throw new Exception($"An error occurred while updating the customer: {ex.Message}");
            }
        }

        // Soft delete a Customer
        public async Task DeleteCustomerAsync(int customerId)
        {
            try
            {
                var token = await _authenticationService.GetTokenAsync();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"api/customer/{customerId}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        throw new PermissionDeniedException($"Permission denied: {errorMessage}");
                    }
                    throw new Exception($"Error deleting customer: {errorMessage}");
                }
            }
            catch (PermissionDeniedException ex)
            {
                // Handle PermissionDeniedException
                throw new PermissionDeniedException($"Permission denied while deleting customer: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle all other types of exceptions
                throw new Exception($"An error occurred while deleting the customer: {ex.Message}");
            }
        }
    }
}
