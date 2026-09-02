using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Categoria.Api.Models
{
    [Table("Categoria")]
    public class CategoriaEntidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Idcategoria { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string Nombre { get; set; } = string.Empty;

        [Column(TypeName = "VARCHAR(255)")]
        public string Descripcion { get; set; } = string.Empty;

        public bool estado { get; set; }
    }
}
