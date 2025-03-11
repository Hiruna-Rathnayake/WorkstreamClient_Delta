using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WorkstreamClient.DTO;  // Ensure DTOs are referenced
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace WorkstreamClient.Services
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationService _authenticationService;  // For handling JWT token

        public OrderService(HttpClient httpClient, AuthenticationService authenticationService)
        {
            _httpClient = httpClient;
            _authenticationService = authenticationService;  // Initialize AuthenticationService
        }

        // Create a new Order
        public async Task<OrderReadDTO> CreateOrderAsync(OrderWriteDTO orderWriteDto)
        {
            var token = await _authenticationService.GetTokenAsync();

            // Add token to request headers
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("api/order", orderWriteDto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<OrderReadDTO>();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error creating order: {errorMessage}");
            }
        }

        // Get Order by ID
        public async Task<OrderReadDTO> GetOrderByIdAsync(int orderId)
        {
            var token = await _authenticationService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"api/order/{orderId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<OrderReadDTO>();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error retrieving order: {errorMessage}");
            }
        }

        // Update an Order
        public async Task<OrderReadDTO> UpdateOrderAsync(int orderId, OrderWriteDTO orderWriteDto)
        {
            var token = await _authenticationService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync($"api/order/{orderId}", orderWriteDto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<OrderReadDTO>();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error updating order: {errorMessage}");
            }
        }

        // Add Inventory Item to an Order
        public async Task<OrderItemReadDTO> AddInventoryItemToOrderAsync(int orderId, OrderItemWriteDTO orderItemWriteDto)
        {
            var token = await _authenticationService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync($"api/order/{orderId}/items", orderItemWriteDto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<OrderItemReadDTO>();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error adding item to order: {errorMessage}");
            }
        }

        // Get all Order Items for an Order
        public async Task<List<OrderItemReadDTO>> GetOrderItemsAsync(int orderId)
        {
            var token = await _authenticationService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"api/order/{orderId}/items");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<OrderItemReadDTO>>();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error retrieving order items: {errorMessage}");
            }
        }

        public async Task<List<OrderReadDTO>> GetAllOrdersAsync()
        {
            // Get the token
            var token = await _authenticationService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Make the GET request without tenantId in the URL
            var response = await _httpClient.GetAsync("api/order/orders");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<OrderReadDTO>>();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error retrieving orders: {errorMessage}");
            }
        }

    }
}
