using Microsoft.EntityFrameworkCore;
using UniGateAPI.Models;

namespace UniGateAPI.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Admission> Admissions { get; set; }
    public DbSet<Applicant> Applicants { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<ProgramPreference> ProgramPreferences { get; set; }
}