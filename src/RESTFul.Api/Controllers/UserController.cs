using AspNetCore.IQueryable.Extensions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTFul.Api.Commands;
using RESTFul.Api.Models;
using RESTFul.Api.Notification;
using RESTFul.Api.Service.Interfaces;
using RESTFul.Api.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RESTFul.Api.Controllers
{
    [Route("users")]
    public class UserController : ApiBaseController
    {
        private readonly IDummyUserService _dummyUserService;
        private readonly IMapper _mapper;

        public UserController(
            INotificationHandler<DomainNotification> notifications,
            IDomainNotificationMediatorService mediator,
            IDummyUserService dummyUserService,
            IMapper mapper) : base(notifications, mediator)
        {
            _dummyUserService = dummyUserService;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<ActionResult<List<UserViewModel>>> Get([FromQuery] UserSearch search)
        {
            var result = _dummyUserService.Query().Apply(search);

            return ResponseGet(await _mapper.ProjectTo<UserViewModel>(result).ToListAsync());
        }

        [HttpGet("{username}"),
         HttpHead("{username}"),
         ResponseCache(Location = ResponseCacheLocation.Any, Duration = 600)]
        public async Task<ActionResult<User>> Get(string username)
        {
            return ResponseGet(await _dummyUserService.Find(username).ConfigureAwait(false));
        }

        [HttpPost("")]
        public async Task<ActionResult<User>> Post([FromBody] RegisterUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return ModelStateErrorResponseError();
            }

            await _dummyUserService.Save(command).ConfigureAwait(false);
            var newUser = await _dummyUserService.Find(command.Username).ConfigureAwait(false);
            return ResponsePost(nameof(Get), new { id = command.Username }, newUser);
        }

        [HttpPatch("{username}")]
        public async Task<ActionResult> Patch(string username, [FromBody] JsonPatchDocument<User> model)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return ModelStateErrorResponseError();
            }

            var actualUser = await _dummyUserService.Find(username).ConfigureAwait(false);
            model.ApplyTo(actualUser);
            await _dummyUserService.Update(actualUser).ConfigureAwait(false);
            return ResponsePutPatch();
        }


        [HttpPut("{username}")]
        public async Task<ActionResult> Put(string username, [FromBody] UpdateUserCommand model)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return ModelStateErrorResponseError();
            }

            var actual = await _dummyUserService.Find(username).ConfigureAwait(false);
            model.Update(actual);
            await _dummyUserService.Update(actual).ConfigureAwait(false);
            return ResponsePutPatch();
        }


        [HttpDelete("{username}")]
        public ActionResult<User> Delete(string username)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return ModelStateErrorResponseError();
            }

            _dummyUserService.Remove(username);
            return ResponseDelete();
        }

    }
}
