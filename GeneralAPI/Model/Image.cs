using System.ComponentModel.DataAnnotations;

namespace GeneralAPI.Model
{
    public class Image
    {
        [Key]
        public int Id_image { get; set; }
        public int Entity_Id { get; set; }
        public string Entity_Name { get; set; }
        public string fileUrl { get; set; }
        public DateTime Create_date { get; set; }
    }
}
