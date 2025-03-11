using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WorkstreamClient.DTO;  // Ensure DTOs are referenced
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace WorkstreamClient.Services
{
    public class InventoryService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationService _authenticationService;  // For handling JWT token

        public InventoryService(HttpClient httpClient, AuthenticationService authenticationService)
        {
            _httpClient = httpClient;
            _authenticationService = authenticationService;  // Initialize AuthenticationService
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            // Check if the response is successful (status code 2xx)
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }
            // Handle specific error case for Forbidden (403)
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new PermissionDeniedException("You do not have permission to perform this action.");
            }
            // Catch any other unexpected errors and throw a general exception
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error: {errorMessage}");
            }
        }

        public async Task<InventoryItemReadDTO> CreateInventoryItemAsync(InventoryItemWriteDTO inventoryItemDTO)
        {
            try
            {
                var token = await _authenticationService.GetTokenAsync();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsJsonAsync("api/inventory", inventoryItemDTO);

                return await HandleResponse<InventoryItemReadDTO>(response);
            }
            catch (PermissionDeniedException ex)
            {
                // Handle PermissionDeniedException
                throw ex;
            }
            catch (Exception ex)
            {
                // Handle all other types of exceptions
                throw new Exception($"An error occurred while creating the inventory item: {ex.Message}");
            }
        }

        public async Task<InventoryItemReadDTO> GetInventoryItemByIdAsync(int itemId)
        {
            try
            {
                var token = await _authenticationService.GetTokenAsync();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"api/inventory/{itemId}");

                return await HandleResponse<InventoryItemReadDTO>(response);
            }
            catch (PermissionDeniedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the inventory item: {ex.Message}");
            }
        }

        public async Task<List<InventoryItemReadDTO>> GetAllInventoryItemsAsync()
        {
            try
            {
                var token = await _authenticationService.GetTokenAsync();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("api/inventory");

                return await HandleResponse<List<InventoryItemReadDTO>>(response);
            }
            catch (PermissionDeniedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving all inventory items: {ex.Message}");
            }
        }

        public async Task<InventoryItemReadDTO> UpdateInventoryItemAsync(int itemId, InventoryItemWriteDTO inventoryItemDTO)
        {
            try
            {
                var token = await _authenticationService.GetTokenAsync();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PutAsJsonAsync($"api/inventory/{itemId}", inventoryItemDTO);

                return await HandleResponse<InventoryItemReadDTO>(response);
            }
            catch (PermissionDeniedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the inventory item: {ex.Message}");
            }
        }

        public async Task DeleteInventoryItemAsync(int itemId)
        {
            try
            {
                var token = await _authenticationService.GetTokenAsync();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"api/inventory/{itemId}");

                await HandleResponse<object>(response);  // No content to return, just ensure it's a valid response
            }
            catch (PermissionDeniedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the inventory item: {ex.Message}");
            }
        }

        public async Task<List<InventoryItemReadDTO>> SearchInventoryItemsAsync(string searchTerm)
        {
            try
            {
                var token = await _authenticationService.GetTokenAsync();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"api/inventory/search?searchTerm={searchTerm}");

                return await HandleResponse<List<InventoryItemReadDTO>>(response);
            }
            catch (PermissionDeniedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while searching inventory items: {ex.Message}");
            }
        }
    }

    // Custom exception for handling permission errors
    public class PermissionDeniedException : Exception
    {
        public PermissionDeniedException(string message) : base(message) { }
    }
}
