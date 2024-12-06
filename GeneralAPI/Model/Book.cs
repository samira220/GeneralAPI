using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace GeneralAPI.Model
{
    public class Books
    {
        [Key]
        public int Id_Book { get; set; }


        public required string Name { get; set; }
        public string? Author { get; set; }

        [DataType(DataType.Date)]
        public DateTime Year_Izd { get; set; }
        public string? Description { get; set; }




        [ForeignKey("Zhanr")]
        public int Id_Zhanr { get; set; }
        public Zhanr Zhanr { get; set; }
    }
    public class ApiResponse
    {
        public List<Books> Books { get; set; }
        public bool Status { get; set; }
    }

    public class BookResponse
    {
        public Books Books { get; set; }
    }

}

