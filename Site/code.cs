TinyMapper.Bind<Person, PersonDto>();

var person = new Person
{
    Id = Guid.NewGuid(),
    FirstName = "John",
    LastName = "Doe"
};

var personDto = TinyMapper.Map<PersonDto>(person);
