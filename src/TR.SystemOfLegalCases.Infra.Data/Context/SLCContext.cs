using Microsoft.EntityFrameworkCore;
using TR.SystemOfLegalCases.Domain.LegalCaseRoot;

namespace TR.SystemOfLegalCases.Infra.Data.Context
{
    public class SLCContext : DbContext
    {
        public SLCContext(DbContextOptions<SLCContext> options) : base(options) 
        { 
        
        }

        public DbSet<LegalCase> LegalCases { get; set; }
    }
}
