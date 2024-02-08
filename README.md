
# ASP.NET Core IQueryable Extensions
![Nuget](https://img.shields.io/nuget/v/AspNetCore.IQueryable.Extensions)![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/brunohbrito/AspNetCore.IQueryable.Extensions/14)[![Build Status](https://dev.azure.com/brunohbrito/AspNetCore.IQueryable.Extensions/_apis/build/status/brunohbrito.AspNetCore.IQueryable.Extensions?branchName=master)](https://dev.azure.com/brunohbrito/AspNetCore.IQueryable.Extensions/_build/latest?definitionId=16&branchName=master)

<img align="right" width="100px" src="https://jpproject.blob.core.windows.net/images/restful-icon-github.png" />
Lightweight API that construct custom IQueryable LINQ Extensions to help you filter, sort and paginate your objects from a custom Class and expose it as GET parameter.


## Table of Contents ##

- [ASP.NET Core IQueryable Extensions](#aspnet-core-iqueryable-extensions)
  - [Table of Contents](#table-of-contents)
- [How](#how)
- [Sort](#sort)
- [Paging](#paging)
- [All in One](#all-in-one)
- [Criterias for filtering](#criterias-for-filtering)
- [Different database fields name](#different-database-fields-name)
- [Or Operator](#or-operator)
- [Why](#why)
- [License](#license)

------------------

# How #

You should install [AspNetCore.IQueryable.Extensions with NuGet](https://www.nuget.org/packages/AspNetCore.IQueryable.Extensions):
```
    Install-Package AspNetCore.IQueryable.Extensions
``` 

Or via the .NET Core command line interface:

```
    dotnet add package AspNetCore.IQueryable.Extensions
```

Create a class with filtering properties:

``` c#
public class UserSearch
{
    public string Username { get; set; }

    [QueryOperator(Operator = WhereOperator.GreaterThan)]
    public DateTime? Birthday { get; set; }

    [QueryOperator(Operator = WhereOperator.Contains, HasName = "Firstname")]
    public string Name { get; set; }
}
```

Expose this class as GET in your API and use it to Filter your collection:

``` c#
[HttpGet("")]
public async Task<ActionResult<IEnumerable<User>>> Get([FromQuery] UserSearch search)
{
    var result = await context.Users.AsQueryable().Filter(search).ToListAsync();

    return Ok(result);
}
```

Done! 
<img align="right" width="100px" src="https://jpproject.blob.core.windows.net/images/restful-icon.png" />
You can send a request to you API like this: `https://www.myapi.com/users?username=bhdebrito@gmail.com&name=bruno`


The component will construct a IQueryable. If you are using an ORM like EF Core it construct a SQL query based in IQueryable, improving performance.

# Sort

A comma separetd fields. E.g username,birthday,-firstname

**-**(minus) for **descending** **+**(plus) or nothing for **ascending**

``` c#
public class UserSearch
{
    public string Username { get; set; }

    public string SortBy { get; set; }
}
```


``` c#
[HttpGet("")]
public async Task<ActionResult<IEnumerable<User>>> Get([FromQuery] UserSearch search)
{
    var result = await context.Users.AsQueryable().Filter(search).Sort(search.SortBy).ToListAsync();

    return Ok(result);
}
```
Example GET: `https://www.myapi.com/users?username=bruno&sortby=username,-birtday`
<img align="right" width="100px" src="https://jpproject.blob.core.windows.net/images/restful-icon-2.png" />

# Paging

A exclusive extension for paging


``` c#
public class UserSearch
{
    public string Username { get; set; }

    [QueryOperator(Max = 100)]
    public int Limit { get; set; } = 10;

    public int Offset { get; set; } = 0;
}
```

**Limit** is the total results in response. **Offset** is how many rows to Skip. Optionally you can set the `Max` attribute to restrict the max items of pagination.

``` c#
[HttpGet("")]
public async Task<ActionResult<IEnumerable<User>>> Get([FromQuery] UserSearch search)
{
    var result = await context.Users.AsQueryable().Filter(search).Paging(search.Limit, search.Offset).ToListAsync();

    return Ok(result);
}
```

Example GET: `https://www.myapi.com/users?username=bruno&limit=10&offset=20`
<img align="right" width="100px" src="https://jpproject.blob.core.windows.net/images/all-in-one.png" />

# All in One


Create a search class like this

``` c#
public class UserSearch : IQuerySort, IQueryPaging
{
    public string Username { get; set; }

    [QueryOperator(Operator = WhereOperator.GreaterThan)]
    public DateTime? Birthday { get; set; }

    [QueryOperator(Operator = WhereOperator.Contains, HasName = "Firstname")]
    public string Name { get; set; }

    public int Offset { get; set; }
    public int Limit { get; set; } = 10;
    public string Sort { get; set; }
}
```
Call Apply method, instead calling each one with custom parameters.

``` c#
[HttpGet("")]
public async Task<ActionResult<IEnumerable<User>>> Get([FromQuery] UserSearch search)
{
    var result = await context.Users.AsQueryable().Apply(search).ToListAsync();

    return Ok(result);
}
```

`IQuerySort` and `IQueryPaging` give the ability for method `Apply` use **Sort** and **Pagination**. If don't wanna sort, just use pagination remove `IQuerySort` Interface from Class.

# Criterias for filtering

When creating a Search class, you can define criterias by decorating your properties:

``` c#
public class CustomUserSearch
{
    [QueryOperator(Operator = WhereOperator.Equals, UseNot = true)]
    public string Category { get; set; }

    [QueryOperator(Operator = WhereOperator.GreaterThanOrEqualTo)]
    public int OlderThan { get; set; }

    [QueryOperator(Operator = WhereOperator.StartsWith, CaseSensitive = true)]
    public string Username { get; set; }

    [QueryOperator(Operator = WhereOperator.GreaterThan)]
    public DateTime? Birthday { get; set; }

    [QueryOperator(Operator = WhereOperator.Contains)]
    public string Name { get; set; }
}
```

# Different database fields name

You can specify different property name to hide you properties original fields

``` c#
public class CustomUserSearch
{
    [QueryOperator(Operator = WhereOperator.Equals, UseNot = true, HasName = "Privilege")]
    public string Category { get; set; }

    [QueryOperator(Operator = WhereOperator.GreaterThanOrEqualTo)]
    public int OlderThan { get; set; }

    [QueryOperator(Operator = WhereOperator.StartsWith, CaseSensitive = true, HasName = "Username")]
    public string Email { get; set; }
}
```

# Or Operator

You can use Or operator for your queries.

``` c#
public class CustomUserSearch
{
    [QueryOperator(Operator = WhereOperator.Equals, UseOr = true]
    public string Category { get; set; }

    [QueryOperator(Operator = WhereOperator.GreaterThanOrEqualTo)]
    public int OlderThan { get; set; }

    [QueryOperator(Operator = WhereOperator.StartsWith, CaseSensitive = true, HasName = "Username")]
    public string Email { get; set; }
}
```
Take care, Or replace all "AND" at query.

# Why

RESTFul api's are hard to create. See the example get:

`https://www.myapi.com/users?name=bruno&age_lessthan=30&sortby=name,-age&limit=20&offset=20`

How many code you need to perform such search? A custom filter for each Field, maybe a for and a switch for each `sortby` and after all apply pagination.
How many resources your api have? 

This lightweight API create a custom IQueryable based in Querystring to help your ORM or LINQ to filter data.

---------------

# License

AspNet.Core.IQueryable.Extensions is Open Source software and is released under the MIT license. This license allow the use of AspNet.Core.IQueryable.Extensions in free and commercial applications and libraries without restrictions.
