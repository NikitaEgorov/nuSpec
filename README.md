# nuSpec
[![NuGet](https://img.shields.io/nuget/v/nuSpec.Abstraction.svg)](https://www.nuget.org/packages/nuSpec.Abstraction)[![NuGet](https://img.shields.io/nuget/dt/nuSpec.Abstraction.svg)](https://www.nuget.org/packages/nuSpec.Abstraction)
[![Build and Test .NET 5 CI](https://github.com/NikitaEgorov/nuSpec/actions/workflows/build+test.yml/badge.svg)](https://github.com/NikitaEgorov/nuSpec/actions/workflows/build+test.yml)

nuSpec is a framework that will help you to create specifications for queries that can be executed by a remote server. You can read more about the specification pattern in [Wikipedia](http://en.wikipedia.org/wiki/Specification_pattern).

# Features
- [Projection](https://nhibernate.info/doc/nhibernate-reference/queryqueryover.html#queryqueryover-projections) - get only part of data
- [Fetch](https://nhibernate.info/doc/nhibernate-reference/performance.html) - eager data loading 
- [Future](https://nhibernate.info/doc/nhibernate-reference/performance.html#performance-future) - batch execution


Almost all LINQ users in their daily work create specifications, but most of them write those specifications scattered all over the code. The idea behind this project is to help the user to write, test and expose specifications as first-class objects. You will learn how to use nuSpec in this brief document.

### Defining simple specifications

```csharp
using nuSpec.Abstraction;

public class EmployeeByLastNameSpecification : Specification<Employee>
{
    public EmployeeByLastNameSpecification(string lastName)
    {
        this.Query = q => q.Where(x => x.LastName == lastName);
    }
}
```


Simple as is, to use this class, your repository should implement these kind of methods:

```csharp
using nuSpec.NHibernate;

public class Repository<T>
{
    private ISpecificationEvaluator evaluator;
    public Repository(ISpecificationEvaluator evaluator)
    {
        this.evaluator = evaluator;
    }

    public IEnumerable<T> Find(Specification<T> specification)
    {
        return this.evaluator.GetQuery([a queryable source], specification).ToList();
    }
}
```

The usage is very simple:

```csharp
var spec = new EmployeeByLastNameSpecification("Doe");
var result = employeeRepository.Find(spec);
```

### Future

```csharp
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

[Fact]
public async Task GetFutureValueWithSelector()
{
    // Arrange
    var spec = new ProjectionQuerySpec();
    var f1 = this.evaluator.GetFutureValue(this.sessionQuery, spec);
    var countFuture = this.evaluator.GetFutureValue(this.sessionQuery, spec, x => x.Count());

    // Act
    var result = await f1.GetValueAsync();

    // Assert
    result.FullName.Should().Be("F1 L1");
    countFuture.Value.Should().Be(1);
}
```

### MS SQL Server example (Split max params)

```csharp
var ids = Enumerable.Range(1, 5000);
var chunks = ids.Split(1000);
var employees = new List<EmployeeProjection>();

foreach(var batch in chunks)
{
    var spec = new EmployeeByIDsSpecification(batch);
    employees.AddRange(employeeRepository.Find(spec)); // Do not run in parallel an ISession is a non-threadsafe
}
```


### Hardcore example

```csharp
public class EmployeeProjection
{
    public string FullName {get; set;}
    public Employee Employee {get; set;}
}

using nuSpec.Abstraction;

public class EmployeeByLastNameSpecification : Specification<Employee, EmployeeProjection>
{
    public QuerySpec(string lastName)
    {
        this.Query = q => q.Where(x => x.LastName == lastName)
                        .Select(e => new EmployeeProjection{ FullName = e.FullNam, Employee = e });
        this.AddFetch(q => q.Fetch(e => e.Department));
    }
}

public class Repository
{
    public IEnumerable<T> Find()
    {
        var does = this.evaluator.GetFuture([a queryable source], new EmployeeByLastNameSpecification("Doe"));
        var smiths = this.evaluator.GetFuture([a queryable source], new EmployeeByLastNameSpecification("Smith"));
        return does.GetEnumerable().Union(smiths.GetEnumerable())
    }
}
```

## Using with ASP.NET Core DI

```csharp

// Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<ISpecificationEvaluator, NhSpecificationEvaluator>();
}


// Repository.cs
public class Repository
{
    private readonly ISpecificationEvaluator evaluator;

    private readonly ISession session;

    public Repository(ISession session, ISpecificationEvaluator evaluator)
    {
        this.evaluator = evaluator;
        this.session = session;
    }

    public async Task<IList<TProjection>> ListAsync<TDomainObject, TProjection>(
        Specification<TDomainObject, TProjection> specification,
        CancellationToken cancellationToken = default) =>
        await this
                .evaluator
                .GetQuery(this.session.Query<TDomainObject>(), specification)
                .ToListAsync(cancellationToken);
    
    public async Task<TProjection> SingleOrDefaultAsync<TDomainObject, TProjection>(
        Specification<TDomainObject, TProjection> specification,
        CancellationToken cancellationToken = default) =>
        await this
                .evaluator
                .GetQuery(this.session.Query<TDomainObject>(), specification)
                .SingleOrDefaultAsync(cancellationToken);
}

```


# Supported platforms

- .NET Standard 2.0+
- .NET Framework 4.0+
- .NET Core 2.0+