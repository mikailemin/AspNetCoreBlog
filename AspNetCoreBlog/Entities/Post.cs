using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace AspNetCoreBlog.Entities
{
    public class Post
    {
        public int Id { get; set; }
        [Display(Name = "Başlık"), StringLength(100), Required(ErrorMessage = "Bu alan gereklidir!")]
        public string Title { get; set; }
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
        [Display(Name = "Resim"), StringLength(100)]
        public string? Image { get; set; }

        [Display(Name = "Durum")]
        public bool IsActive { get; set; }
        [Display(Name = "Anasayfada Göster")]
        public bool IsHome { get; set; }

        [Display(Name = "Ekleme Tarihi"), ScaffoldColumn(false)]
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        [Display(Name = "Kategori")]
        public int? CategoryId { get; set; }
        [Display(Name = "Kategori")]
        public virtual Category? Category  { get; set; }

    }
}
