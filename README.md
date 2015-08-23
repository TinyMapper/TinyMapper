TinyMapper - a quick object mapper for .Net
======================================================
[![Nuget downloads](http://img.shields.io/nuget/dt/tinymapper.svg)](https://www.nuget.org/packages/TinyMapper/)
[![GitHub license](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/TinyMapper/TinyMapper/blob/master/LICENSE)

## Performance Comparison

![Performance Comparison](https://raw.githubusercontent.com/TinyMapper/TinyMapper/master/Source/Benchmark/DataSource/PrimitiveTypeMapping.jpg)

## Installation

Available on [nuget](https://www.nuget.org/packages/TinyMapper/)

	PM> Install-Package TinyMapper

## Getting Started

```csharp
TinyMapper.Bind<Person, PersonDto>();

var person = new Person
{
	Id = Guid.NewGuid(),
	FirstName = "John",
	LastName = "Doe",
	Email = "support@tinymapper.net"
};

var personDto = TinyMapper.Map<PersonDto>(person);
```

Ignore mapping source members and bind members with different names

```csharp
TinyMapper.Bind<Person, PersonDto>(config =>
{
	config.Ignore(x => x.Id);
	config.Ignore(x => x.Email);
	config.Bind(source => source.LastName, target => target.Surname);
});

var person = new Person
{
	Id = Guid.NewGuid(),
	FirstName = "John",
	LastName = "Doe",
	Email = "support@tinymapper.net"
};

var personDto = TinyMapper.Map<PersonDto>(person);
```

`TinyMapper` supports the following platforms:
* .Net 3.0+
* Mono

## What to read

 * [TinyMapper: yet another object to object mapper for .net](http://www.codeproject.com/Articles/886420/TinyMapper-yet-another-object-to-object-mapper-for)
 * [Complex mapping](https://github.com/TinyMapper/TinyMapper/wiki/Complex-mapping)
 * [How to create custom mapping](https://github.com/TinyMapper/TinyMapper/wiki/Custom-mapping)
 
## Contributors
A big thanks to all of TinyMapper's contributors:
 
 * [iEmiya](https://github.com/iEmiya)
 * [lijaso](https://github.com/lijaso)
 * [nomailme](https://github.com/nomailme)
 * [Skaiol](https://github.com/Skaiol)
 * [qihangnet](https://github.com/qihangnet)