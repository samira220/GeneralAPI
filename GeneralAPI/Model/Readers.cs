using System.ComponentModel.DataAnnotations;

namespace GeneralAPI.Model
{
    public class Readers
    {
        [Key]
        public int Id_Reader { get; set; }
        [Required(ErrorMessage = "Введите ваше имя")]
        public string Name { get; set; }
        public string FName { get; set; }

        [Required(ErrorMessage = "Введите ваше день рождения")]
        public DateTime Birth_Day { get; set; }

        [Required(ErrorMessage = "")]
        public string Contact { get; set; }

        public DateTime DateRegist { get; set; }
    }
    public class ApiResponseReader
    {
        public List<Readers> Readers { get; set; }
        public bool Status { get; set; }
    }

    public class ReaderResponce
    {
        public Readers Reader { get; set; }
    }

}
