using System.Linq;

using nuSpec.Abstraction;

namespace nuSpec.Tests
{
    public class QueryFutureSpec : FutureSpecification<Employee>
    {
        public QueryFutureSpec()
        {
            this.AddQuery(q => q.Where(x=>x.LastName == "L1"));
            this.AddQuery(q => q.Where(x=>x.LastName == "L2"));
        }
    }
}
