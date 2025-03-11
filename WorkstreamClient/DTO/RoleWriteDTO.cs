using System.ComponentModel.DataAnnotations;

namespace WorkstreamClient.DTO
{
    public class RoleWriteDTO
    {
        public string Name { get; set; }
        public int TenantId { get; set; }
        public List<string> PermissionNames { get; set; } = new List<string>(); // List of selected permission names
    }


}
