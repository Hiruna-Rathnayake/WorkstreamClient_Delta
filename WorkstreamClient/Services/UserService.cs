using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WorkstreamClient.DTO;  // Make sure to reference the DTOs here
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace WorkstreamClient.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationService _authenticationService;  // Add AuthenticationService

        public UserService(HttpClient httpClient, AuthenticationService authenticationService)  // Inject AuthenticationService
        {
            _httpClient = httpClient;
            _authenticationService = authenticationService;  // Initialize AuthenticationService
        }

        // Create a new User
        public async Task<UserReadDTO> CreateUserAsync(UserWriteDTO userDTO)
        {
            var token = await _authenticationService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("api/user", userDTO);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserReadDTO>();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error creating user: {errorMessage}");
            }
        }

        public async Task<IEnumerable<RoleReadDTO>> GetAllRolesAsync()
        {
            try
            {
                var token = await _authenticationService.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Token is missing or expired.");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("api/Role");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<IEnumerable<RoleReadDTO>>();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error retrieving roles: {response.StatusCode}. {errorContent}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving roles.", ex);
            }
        }

        public async Task<IEnumerable<RoleWithPermissionsDTO>> GetRolesWithPermissionsAsync()
        {
            try
            {
                var token = await _authenticationService.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Token is missing or expired.");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("api/Role/with-permissions");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<IEnumerable<RoleWithPermissionsDTO>>();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error retrieving roles with permissions: {response.StatusCode}. {errorContent}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving roles with permissions.", ex);
            }
        }

        public async Task<RoleWithPermissionsDTO> GetRoleByIdAsync(int roleId)
        {
            try
            {
                var token = await _authenticationService.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Token is missing or expired.");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"api/Role/{roleId}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<RoleWithPermissionsDTO>();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error retrieving role by ID {roleId}: {response.StatusCode}. {errorContent}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving role with ID {roleId}.", ex);
            }
        }

        // Get a User by ID
        public async Task<UserReadDTO> GetUserByIdAsync(int userId)
        {
            var token = await _authenticationService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"api/user/{userId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserReadDTO>();
            }
            else
            {
                throw new Exception($"User with ID {userId} not found.");
            }
        }

        // Get all Users
        public async Task<IEnumerable<UserReadDTO>> GetAllUsersAsync()
        {
            try
            {
                var token = await _authenticationService.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Token is missing or expired.");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("api/user");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<IEnumerable<UserReadDTO>>();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error retrieving users: {response.StatusCode}. {errorContent}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving users.", ex);
            }
        }

        // Update an existing User
        public async Task<UserReadDTO> UpdateUserAsync(int userId, UserUpdateDTO updatedUserDTO)
        {
            var token = await _authenticationService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync($"api/user/{userId}", updatedUserDTO);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserReadDTO>();
            }
            else
            {
                throw new Exception("Error updating user.");
            }
        }

        // Delete a User
        public async Task DeleteUserAsync(int userId)
        {
            var token = await _authenticationService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"api/user/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error deleting user.");
            }
        }

        public async Task<IEnumerable<PermissionReadDTO>> GetAllPermissionsAsync()
        {
            var token = await _authenticationService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("api/Permission");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<PermissionReadDTO>>();
            }
            else
            {
                throw new Exception("Error retrieving permissions.");
            }
        }

        // Create a new Role
        public async Task<RoleReadDTO> CreateRoleAsync(RoleWriteDTO roleDTO)
        {
            var token = await _authenticationService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("api/Role", roleDTO);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RoleReadDTO>();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error creating role: {errorMessage}");
            }
        }

        // Update an existing Role
        public async Task<RoleReadDTO> UpdateRoleAsync(int roleId, RoleWriteDTO updatedRoleDTO)
        {
            var token = await _authenticationService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync($"api/Role/{roleId}", updatedRoleDTO);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RoleReadDTO>();
            }
            else
            {
                throw new Exception("Error updating role.");
            }
        }

        // Delete a Role
        public async Task DeleteRoleAsync(int roleId)
        {
            var token = await _authenticationService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"api/Role/{roleId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error deleting role.");
            }
        }
    }
}
