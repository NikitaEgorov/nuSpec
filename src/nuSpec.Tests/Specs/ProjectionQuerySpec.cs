using System.Linq;

using nuSpec.Abstraction;
using nuSpec.Tests.Models;

namespace nuSpec.Tests.Specs
{
    public class ProjectionQuerySpec : Specification<Employee, EmployeeProjection>
    {
        public ProjectionQuerySpec() =>
            this.Query = q => q.Where(x => x.LastName == "L1")
                               .Select(
                                       z => new EmployeeProjection
                                            {
                                                FullName = z.FirstName + ' ' + z.LastName,
                                                Emp = z
                                            });
    }
}
