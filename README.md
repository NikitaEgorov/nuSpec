# nuSpec
[![NuGet](https://img.shields.io/nuget/v/nuSpec.Abstraction.svg)](https://www.nuget.org/packages/nuSpec.Abstraction)[![NuGet](https://img.shields.io/nuget/dt/nuSpec.Abstraction.svg)](https://www.nuget.org/packages/nuSpec.Abstraction)
[![Build and Test .NET 5 CI](https://github.com/NikitaEgorov/nuSpec/actions/workflows/build+test.yml/badge.svg)](https://github.com/NikitaEgorov/nuSpec/actions/workflows/build+test.yml)

nuSpec is a framework that will help you to create specifications for queries that can be executed by a remote server. You can read more about the specification pattern in [Wikipedia](http://en.wikipedia.org/wiki/Specification_pattern).

# Features
- [Projection](https://nhibernate.info/doc/nhibernate-reference/queryqueryover.html#queryqueryover-projections) - get only part of data
- [Fetch](https://nhibernate.info/doc/nhibernate-reference/performance.html) - eager data loading 
- [Future](https://nhibernate.info/doc/nhibernate-reference/performance.html#performance-future) - batch execution


Almost all users of LINQ create specifications in their daily work, but most of them write those specifications scattered all over the code. The idea behind this project is to help the user to write, test and expose specifications as first-class objects. You will learn how to use nuSpec in this brief document.

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
                        .Select(e => new EmployeeProjection{ FullName = e.FullNam, Employee = e }
        this.AddFetch(q => q.Fetch(e => e.Department))
    }
}

public IEnumerable<T> Find()
{
    var does = this.evaluator.GetFuture([a queryable source], new EmployeeByLastNameSpecification("Doe"));
    var smiths = this.evaluator.GetFuture([a queryable source], new EmployeeByLastNameSpecification("Smith"));
    return does.GetEnumerable().Union(smiths.GetEnumerable())
}
```


# Supported platforms

- .NET Standard 2.0+
- .NET Framework 4.0+
- .NET Core 2.0+