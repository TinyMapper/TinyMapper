TinyMapper - a quick object mapper for .Net
======================================================

Getting Started
===============

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