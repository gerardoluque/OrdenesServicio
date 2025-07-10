using System.ComponentModel.DataAnnotations;

namespace ZOE.OS.Modelo
{
    public class OSStatus
    {
        [Key]
        public virtual short OSStatusId { get; set; }

        [Required]
        [MaxLength(100)]
        public virtual string Descr { get; set; }
    }
}
