using System;
using System.ComponentModel.DataAnnotations;

namespace SAW.DTO.Event
{
    public class UpdateEventRequest
    {
        [Required(ErrorMessage = "Price is mandatory")]
        public double? Price { get; set; }

        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "2023-12-31", "2099-12-31", ErrorMessage = "Starting date should be future or present")]
        public DateTime? StartingDate { get; set; }

        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "2023-12-31", "2099-12-31", ErrorMessage = "Ending date should be future")]
        public DateTime? EndingDate { get; set; }

        [Required(ErrorMessage = "Seating capacity is mandatory")]
        public int? SeatingCapacity { get; set; }

        [Required(ErrorMessage = "Description is mandatory")]
        public string Description { get; set; }
    }
}