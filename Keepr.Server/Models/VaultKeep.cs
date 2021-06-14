using System;
using System.ComponentModel.DataAnnotations;

namespace Keepr.Server.Models
{
    public class VaultKeep
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int VaultId { get; set; }

        [Required]
        public int KeepId { get; set; }

        public string CreatorId { get; set; }
    }

    public class VaultKeepViewModel : Keep
    {
        public int VaultKeepId { get; set; }

        [Required]
        public int VaultId { get; set; }

        [Required]
        public int KeepId { get; set; }
        
    }

}