using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using NHibernate;
using NHibernate.Linq;

using nuSpec.Abstraction;
using nuSpec.NHibernate;

using Xunit;

namespace nuSpec.Tests
{
    public class SpecificationTests : InMemoryDatabaseTest
    {
        private readonly IQueryable<Employee> sessionQuery;

        private readonly NHSpecificationEvalualor evaluator;

        public SpecificationTests()
        {
            var d1 = new Department { Name = "D1" };
            var d2 = new Department { Name = "D2" };
            this.Session.Save(d1);
            this.Session.Save(d2);

            var e1 = new Employee
                     {
                         FirstName = "F1",
                         LastName = "L1",
                         Department = d1
                     };
            var e2 = new Employee
                     {
                         FirstName = "F2",
                         LastName = "L2",
                         Department = d2
                     };
            this.Session.Save(e1);
            this.Session.Save(e2);

            this.Session.Evict(e1);
            this.Session.Evict(e2);
            this.Session.Evict(d1);
            this.Session.Evict(d2);

            this.sessionQuery = this.Session.Query<Employee>();
            this.evaluator = new NHSpecificationEvalualor();
        }

        [Fact]
        public async Task QueryWithoutFetch()
        {
            // Arrange
            var spec = new QuerySpec();

            // Act
            var result = await this.evaluator.GetQuery(this.sessionQuery, spec).ToListAsync();

            // Assert
            result.Should().ContainSingle(x => x.LastName == "L1");
            NHibernateUtil.IsInitialized(result.Single().Department).Should().BeFalse();
        }

        [Fact]
        public async Task QueryWithFetch()
        {
            // Arrange
            var spec = new QueryWithFetchSpec();

            // Act
            var result = await this.evaluator.GetQuery(this.sessionQuery, spec).ToListAsync();

            // Assert
            result.Should().ContainSingle(x => x.LastName == "L1");
            NHibernateUtil.IsInitialized(result.Single().Department).Should().BeTrue();
        }

        [Fact]
        public async Task ProjectionQueryWithoutFetch()
        {
            // Arrange
            var spec = new ProjectionQuerySpec();

            // Act
            var result = await this.evaluator.GetQuery(this.sessionQuery, spec).ToListAsync();

            // Assert
            result.Should().ContainSingle(x => x.FullName == "F1 L1");
            NHibernateUtil.IsInitialized(result.Single().Emp.Department).Should().BeFalse();
        }

        [Fact]
        public async Task ProjectionQueryWithFetch()
        {
            // Arrange
            var spec = new ProjectionQueryWithFetchSpec();

            // Act
            var result = await this.evaluator.GetQuery(this.sessionQuery, spec).ToListAsync();

            // Assert
            result.Should().ContainSingle(x => x.FullName == "F1 L1");
            NHibernateUtil.IsInitialized(result.Single().Emp.Department).Should().BeTrue();
        }

        private class QuerySpec : Specification<Employee>
        {
            public QuerySpec() => this.Query = q => q.Where(x => x.LastName == "L1");
        }

        private class QueryWithFetchSpec : QuerySpec
        {
            public QueryWithFetchSpec() => this.AddFetch(q => q.Fetch(f => f.Department));
        }

        private class ProjectionQuerySpec : Specification<Employee, EmployeeProjection>
        {
            public ProjectionQuerySpec()
            {
                this.Query = q => q.Where(x => x.LastName == "L1");
                this.Projection = q => q.Select(
                                                z => new EmployeeProjection
                                                     {
                                                         FullName = z.FirstName + ' ' + z.LastName,
                                                         Emp = z
                                                     });
            }
        }

        private class ProjectionQueryWithFetchSpec : ProjectionQuerySpec
        {
            public ProjectionQueryWithFetchSpec() => this.AddFetch(q => q.Fetch(f => f.Department));
        }
    }
}
