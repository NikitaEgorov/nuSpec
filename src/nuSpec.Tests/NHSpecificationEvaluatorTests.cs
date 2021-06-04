using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using NHibernate;
using NHibernate.Linq;

using nuSpec.NHibernate;
using nuSpec.Tests.Models;
using nuSpec.Tests.Specs;

using Xunit;

namespace nuSpec.Tests
{
    public class NHSpecificationEvaluatorTests : InMemoryDatabaseTest
    {
        private readonly IQueryable<Employee> sessionQuery;

        private readonly NhSpecificationEvaluator evaluator;

        public NHSpecificationEvaluatorTests()
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
            this.evaluator = new NhSpecificationEvaluator();
        }

        [Fact]
        public async Task GetQueryWithoutFetch()
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
        public async Task GetQueryWithFetch()
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
        public async Task GetQueryWithProjectionWithoutFetch()
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
        public async Task GetQueryWithProjectionWithFetch()
        {
            // Arrange
            var spec = new ProjectionQueryWithFetchSpec();

            // Act
            var result = await this.evaluator.GetQuery(this.sessionQuery, spec).ToListAsync();

            // Assert
            result.Should().ContainSingle(x => x.FullName == "F1 L1");
            NHibernateUtil.IsInitialized(result.Single().Emp.Department).Should().BeTrue();
        }

        [Fact]
        public async Task GetFuture()
        {
            // Arrange
            this.SessionFactory.Statistics.Clear();
            var spec = new ProjectionQuerySpec();
            var f1 = this.evaluator.GetFuture(this.sessionQuery, spec);
            var f2 = this.evaluator.GetFuture(this.sessionQuery, spec);
            var f3 = this.evaluator.GetFuture(this.sessionQuery, spec);

            // Act
            var result = (await f1.GetEnumerableAsync()).ToList();

            // Assert
            result.Should().ContainSingle(x => x.FullName == "F1 L1");
            (await f2.GetEnumerableAsync()).Should().ContainSingle(x => x.FullName == "F1 L1");
            (await f3.GetEnumerableAsync()).Should().ContainSingle(x => x.FullName == "F1 L1");
            this.SessionFactory.Statistics.QueryExecutionCount.Should().Be(1);
        }

        [Fact]
        public async Task GetFutureValue()
        {
            // Arrange
            this.SessionFactory.Statistics.Clear();
            var spec = new ProjectionQuerySpec();
            var f1 = this.evaluator.GetFutureValue(this.sessionQuery, spec);
            var f2 = this.evaluator.GetFutureValue(this.sessionQuery, spec);
            var f3 = this.evaluator.GetFutureValue(this.sessionQuery, spec);

            // Act
            var result = (await f1.GetValueAsync());

            // Assert
            result.FullName.Should().Be("F1 L1");
            (await f2.GetValueAsync()).FullName.Should().Be("F1 L1");
            (await f3.GetValueAsync()).FullName.Should().Be("F1 L1");
            this.SessionFactory.Statistics.QueryExecutionCount.Should().Be(1);
        }
    }
}
