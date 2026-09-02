using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vehiculo.Api.Models
{
    [Table("CategoriaCache")]
    public class CategoriaCacheEntidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // No es IDENTITY aquí
        public int Idcategoria { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string Nombre { get; set; } = string.Empty;
    }
}
