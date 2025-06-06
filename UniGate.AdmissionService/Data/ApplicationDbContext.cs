using Microsoft.EntityFrameworkCore;
using UniGateAPI.Models;

namespace UniGateAPI.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Admission> Admissions { get; set; }
    public DbSet<ApplicantReference> Applicants { get; set; }
    public DbSet<ManagerReference> Managers { get; set; }
    public DbSet<DocumentReference> Documents { get; set; }
    public DbSet<ProgramPreference> ProgramPreferences { get; set; }
}