using FluentNHibernate.Mapping;

namespace nuSpec.Tests.Models
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            this.Id(x => x.Id).GeneratedBy.Identity();
            this.Map(x => x.FirstName);
            this.Map(x => x.LastName);
            this.References(x => x.Department).LazyLoad(Laziness.Proxy);
        }
    }
}
