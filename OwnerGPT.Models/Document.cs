using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwnerGPT.Models
{
    [Table("documents")]
    public class Document
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("upload_date")]
        public DateTime UploadDate { get; set; }

        [Column("extension")]
        public string Extension { get; set; }

        [Column("mime_type")]
        public string MimeType { get; set; }
    }
}
