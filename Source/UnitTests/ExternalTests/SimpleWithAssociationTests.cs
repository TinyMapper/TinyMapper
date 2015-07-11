using System;
using System.Collections.Generic;
using System.Linq;
using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests.ExternalTests
{
    public class SimpleWithAssociationTests
    {
        [Fact]
        public void Test()
        {
            TinyMapper.Bind<User, UserViewModel>();

            List<User> source = GetUsers(1);

            var target = TinyMapper.Map<UserViewModel>(source.First());
        }

        private static List<User> GetUsers(int count)
        {
            var result = new List<User>();

            for (int i = 0; i < count; i++)
            {
                result.Add(
                    new User
                    {
                        Id = Guid.NewGuid(),
                        UserName = string.Format("Username - {0}", i),
                        Active = i % 2 == 0,
                        Address = string.Format("Address - {0}", i),
                        Age = i,
                        CreatedOn = DateTime.Now,
                        Deleted = i % 5 == 0,
                        Email = string.Format("Email - {0}", i),
                        Role = new Role
                        {
                            Id = Guid.NewGuid(),
                            Active = i % 3 == 0,
                            CreatedOn = DateTime.Now,
                            Name = string.Format("Role - {0}", i),
                            Deleted = i % 4 == 0,
                        },
                    });
            }
            return result;
        }

        public class Role
        {
            public bool Active { get; set; }
            public DateTime CreatedOn { get; set; }
            public bool Deleted { get; set; }
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class RoleViewModel
        {
            public bool Active { get; set; }
            public DateTime CreatedOn { get; set; }
            public bool Deleted { get; set; }
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class User
        {
            public bool Active { get; set; }
            public string Address { get; set; }
            public int Age { get; set; }
            public DateTime CreatedOn { get; set; }
            public bool Deleted { get; set; }
            public string Email { get; set; }
            public Guid Id { get; set; }

            public Role Role { get; set; }
            public string UserName { get; set; }
        }

        public class UserViewModel
        {
            public bool Active { get; set; }
            public string Address { get; set; }
            public int Age { get; set; }

            public RoleViewModel BelongTo { get; set; }
            public DateTime CreatedOn { get; set; }
            public bool Deleted { get; set; }
            public string Email { get; set; }
            public Guid Id { get; set; }
            public string UserName { get; set; }
        }
    }
}
