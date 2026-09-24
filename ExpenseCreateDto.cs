using System.ComponentModel.DataAnnotations;

public class ExpenseCreateDto
{
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount {get; set;}

        [Required]
        public string Category {get; set;}
}