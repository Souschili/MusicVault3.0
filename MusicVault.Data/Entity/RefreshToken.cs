using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicVault.Data.Entity
{
    public class RefreshToken
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Content { get; set; } 
        [Required]
        public bool isRevoke { get; set; }
        [Required]
        public DateTime Created { get; set; }
    }
}
