TinyMapper.Bind<Person, PersonDto>();

var person = new Person
{
    Id = Guid.NewGuid(),
    FirstName = "John",
    LastName = "Doe"
};

var personDto = TinyMapper.Map<PersonDto>(person);

// with mapping members ignored

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
