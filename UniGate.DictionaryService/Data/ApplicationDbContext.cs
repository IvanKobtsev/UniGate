using Microsoft.EntityFrameworkCore;
using UniGate.DictionaryService.Models;

namespace UniGate.DictionaryService.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<EducationLevel> EducationLevels { get; set; }
    public DbSet<EducationDocumentType> EducationDocumentTypes { get; set; }
    public DbSet<EducationLevelAccess> EducationLevelAccesses { get; set; }
    public DbSet<Faculty> Faculties { get; set; }
    public DbSet<EducationProgram> EducationPrograms { get; set; }
    public DbSet<ImportState> ImportStates { get; set; }
}