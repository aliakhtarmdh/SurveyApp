using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SurveyApp.Models
{
    public class Survey
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Range(1, 120)]
    public int Age { get; set; }

    [Required]
     public string FeedbackType { get; set; } // Ensure this property matches what you're using in your views

    public List<string> ServicesUsed { get; set; } = new List<string>(); // Ensure 

    public string? DocumentPath { get; set; }
    public string? AdditionalComments { get; set; }
}
}
