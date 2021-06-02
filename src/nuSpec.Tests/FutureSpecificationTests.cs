using System;
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
    public class FutureSpecificationTests : InMemoryDatabaseTest
    {
        private readonly IQueryable<Employee> sessionQuery;

        private readonly NHSpecificationEvalualor evaluator;

        public FutureSpecificationTests()
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
            this.SessionFactory.Statistics.Clear();

            // Act
            var list = this
                       .evaluator
                       .GetQuery(this.sessionQuery, spec)
                       .Select(q => q.ToFuture())
                       .ToList();
            await list.First().GetEnumerableAsync();
            var result = list.SelectMany(z => z.ToList()).ToList();

            // Assert

            result.Should().HaveCount(2);
            NHibernateUtil.IsInitialized(result.First().Department).Should().BeFalse();
            this.SessionFactory.Statistics.QueryExecutionCount.Should().Be(1);
        }

        [Fact]
        public async Task QueryWithFetch()
        {
            // Arrange
            var spec = new QueryWithFetchSpec();
            this.SessionFactory.Statistics.Clear();

            // Act
            var list = this
                       .evaluator
                       .GetQuery(this.sessionQuery, spec)
                       .Select(q => q.ToFuture())
                       .ToList();
            await list.First().GetEnumerableAsync();
            var result = list.SelectMany(z => z.ToList()).ToList();

            // Assert
            result.Should().HaveCount(2);
            NHibernateUtil.IsInitialized(result.First().Department).Should().BeTrue();
            this.SessionFactory.Statistics.QueryExecutionCount.Should().Be(1);
        }

        [Fact]
        public async Task ProjectionQueryWithoutFetch()
        {
            // Arrange
            var spec = new ProjectionQuerySpec();
            this.SessionFactory.Statistics.Clear();

            // Act
            var list = this
                       .evaluator
                       .GetQuery(this.sessionQuery, spec)
                       .Select(q => q.ToFuture())
                       .ToList();
            await list.First().GetEnumerableAsync();
            var result = list.SelectMany(z => z.ToList()).ToList();

            // Assert
            result.Should().HaveCount(2);
            NHibernateUtil.IsInitialized(result.First().Emp.Department).Should().BeFalse();
            this.SessionFactory.Statistics.QueryExecutionCount.Should().Be(1);
        }

        [Fact]
        public async Task ProjectionQueryWithFetch()
        {
            // Arrange
            var spec = new ProjectionQueryWithFetchSpec();
            this.SessionFactory.Statistics.Clear();

            // Act
            var list = this
                       .evaluator
                       .GetQuery(this.sessionQuery, spec)
                       .Select(q => q.ToFuture())
                       .ToList();
            await list.First().GetEnumerableAsync();
            var result = list.SelectMany(z => z.ToList()).ToList();

            // Assert
            result.Should().HaveCount(2);
            NHibernateUtil.IsInitialized(result.First().Emp.Department).Should().BeTrue();
            this.SessionFactory.Statistics.QueryExecutionCount.Should().Be(1);
        }

        private class QuerySpec : FutureSpecification<Employee>
        {
            public QuerySpec()
            {
                this.AddQuery(q => q.Where(x => x.LastName == "L1"));
                this.AddQuery(q => q.Where(x => x.LastName == "L2"));
            }
        }

        private class QueryWithFetchSpec : QuerySpec
        {
            public QueryWithFetchSpec() => this.AddFetch(q => q.Fetch(f => f.Department));
        }

        private class ProjectionQuerySpec : FutureSpecification<Employee, EmployeeProjection>
        {
            public ProjectionQuerySpec()
            {
                this.AddQuery(q => q.Where(x => x.LastName == "L1"));
                this.AddQuery(q => q.Where(x => x.LastName == "L2"));

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
            public ProjectionQueryWithFetchSpec()
            {
                this.AddFetch(x => x.Fetch(z => z.Department));
            }
        }
    }
}
