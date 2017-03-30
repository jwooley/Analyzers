using System.Data.Entity;
using System.Linq;

namespace TestProject
{
    public class LinqTests
    {
        public void TestContextBad()
        {
            var dc = new Context();
            // Do Something with the dc
            var people = dc.People.Take(5);
            // Asert
            var filtered = dc.People.ToList()
                .Where(p => p.LastName == "Wooley")
                .FirstOrDefault();
            
        }
    }

    public class Context
        : DbContext
    {
        public DbSet<Person> People { get; set; }
    }
}
