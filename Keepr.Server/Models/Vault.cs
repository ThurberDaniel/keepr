using System;
using System.ComponentModel.DataAnnotations;

namespace Keepr.Server.Models
{
    public class Vault
    {
        public int Id { get; set; }

        public string CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Name { get; set; }="Name...";
        public string Description { get; set; }="No description yet";
        public string ImgUrl { get; set; } = "//placehold.it/200x200";
        public bool IsPrivate { get; set; } = false;

        public Profile Creator { get; set; }
    }
}