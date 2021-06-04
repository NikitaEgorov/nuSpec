using System.Linq;

using nuSpec.Abstraction;
using nuSpec.Tests.Models;

namespace nuSpec.Tests.Specs
{
    public class QuerySpec : Specification<Employee>
    {
        public QuerySpec() => this.Query = q => q.Where(x => x.LastName == "L1");
    }
}
