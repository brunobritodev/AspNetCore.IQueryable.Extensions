using RESTFul.Api.Commands;
using RESTFul.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTFul.Api.Service.Interfaces
{
    public interface IDummyUserService
    {
        IQueryable<User> Query();
        Task<IEnumerable<User>> All();
        Task<User> Find(string id);
        Task Save(RegisterUserCommand command);
        Task Update(User actualUser);
        Task<int> Remove(string username);
    }
}