using Microsoft.EntityFrameworkCore;
using SurveyApp.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Survey> Surveys { get; set; }
}
