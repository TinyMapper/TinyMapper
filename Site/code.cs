TinyMapper.Bind<Person, PersonDto>();

var person = new Person
{
    Id = Guid.NewGuid(),
    FirstName = "John",
    LastName = "Doe"
};

var personDto = TinyMapper.Map<PersonDto>(person);

// with mapping members ignored and bind members with different names/types

TinyMapper.Bind<Person, PersonDto>(config =>
{
	config.Ignore(x => x.Id);
	config.Ignore(x => x.Email);
	config.Bind(source => source.LastName, target => target.Surname);
	config.Bind(target => source.Emails, typeof(List<string>));
});

var person = new Person
{
	Id = Guid.NewGuid(),
	FirstName = "John",
	LastName = "Doe",
	Emails = new List<string>{"support@tinymapper.net", "MyEmail@tinymapper.net"}
};

var personDto = TinyMapper.Map<PersonDto>(person);
