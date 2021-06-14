using System;
using System.ComponentModel.DataAnnotations;

namespace Keepr.Server.Models
{
    public class Keep
    {
        public int Id { get; set; }
        public string CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }= "//placehold.it/200x300";
        public int Views { get; set; } =0;
        public int Shares { get; set; }=0;
        public int Keeps { get; set; }=0;
        public Profile Creator { get; set; }
    }
}