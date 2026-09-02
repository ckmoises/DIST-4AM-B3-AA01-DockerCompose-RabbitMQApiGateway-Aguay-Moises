using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vehiculo.Api.Models
{
    [Table("Vehiculo")]
    public class VehiculoEntidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdVehiculo { get; set; }

        [Required]
        public int idcategoria { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string Marca { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "DECIMAL(18,2)")]
        public decimal precio { get; set; }

        [Required]
        public int stock { get; set; }

        public bool estado { get; set; }
    }
}
