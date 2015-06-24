TinyMapper - a quick object mapper for .Net
======================================================

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

Ignore mapping members

```csharp
TinyMapper.Bind<Person, PersonDto>(config =>
{
	config.Ignore(x => x.Id);
	config.Ignore(x => x.Email);
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