using System.ComponentModel.DataAnnotations;

namespace GeneralAPI.Model
{
    public class Zhanr
    {
        [Key]
        public int Id_Zhanr { get; set; }

        [Required(ErrorMessage = "Название жанра обязательно")]
        public required string Name { get; set; }
    }

    public class APIResponce
    {
        public List<Zhanr> Zhanr { get; set; }
        public bool Status { get; set; }
    }
}
