using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace BulkyBook.Models3
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30)]
        public string? Name { get; set; }
        [Required]
        [DisplayName("Display Order")]
        [Range(1,100,ErrorMessage ="Display order must be within 1-100")]
        public int DisplayOrder { get; set; }
    }
}
