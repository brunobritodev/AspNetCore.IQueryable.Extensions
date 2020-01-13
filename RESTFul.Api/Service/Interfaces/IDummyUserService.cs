using RESTFul.Api.Commands;
using RESTFul.Api.Models;
using System.Collections.Generic;

namespace RESTFul.Api.Service.Interfaces
{
    public interface IDummyUserService
    {
        IEnumerable<User> All();
        User Find(string id);
        void Save(RegisterUserCommand command);
        void Update(User actualUser);
        void Remove(string username);
    }
}