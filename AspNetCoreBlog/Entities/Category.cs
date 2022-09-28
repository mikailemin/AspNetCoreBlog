using System.ComponentModel.DataAnnotations;

namespace AspNetCoreBlog.Entities
{
    public class Category
    {
        public int Id { get; set; }
        [Display(Name = "Adı"),StringLength(50),Required(ErrorMessage ="Bu alan gereklidir!")]
        public string Name { get; set; }
        [Display(Name = "Açıklama")]
        public string? Description { get; set; } // description kolonunun nullable yani boş geçilebilir olması ? işareti koyuyoruz
        [Display(Name = "Durum")]
        public bool IsActive { get; set; }
        [Display(Name ="Ekleme Tarihi"),ScaffoldColumn(false)] // ScaffoldColumn attiribute ü View larda CreateDate alanı için crud sayfalarında veri giriş alanı oluşmasını sağlar.
        public DateTime? CreateDate { get; set; } =DateTime.Now;
        public List<Post> Posts { get; set; }
        public Category()
        {
            Posts = new List<Post>();
        }

    }
}
