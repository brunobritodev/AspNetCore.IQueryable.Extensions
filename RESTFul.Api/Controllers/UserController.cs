using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RESTFul.Api.Commands;
using RESTFul.Api.Models;
using RESTFul.Api.Notification;
using RESTFul.Api.Service.Interfaces;
using System.Collections.Generic;

namespace RESTFul.Api.Controllers
{
    [Route("users")]
    public class UserController : ApiBaseController
    {
        private readonly IDummyUserService _dummyUserService;
        public UserController(
            INotificationHandler<DomainNotification> notifications,
            IDomainNotificationMediatorService mediator,
            IDummyUserService dummyUserService) : base(notifications, mediator)
        {
            _dummyUserService = dummyUserService;
        }

        [HttpGet("")]
        public ActionResult<IEnumerable<User>> Get()
        {
            return ResponseGet(_dummyUserService.All());
        }

        [HttpGet("{username}")]
        public ActionResult<User> Get(string username)
        {
            return ResponseGet(_dummyUserService.Find(username));
        }

        [HttpPost("")]
        public ActionResult<User> Post([FromBody] RegisterUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return ModelStateErrorResponseError();
            }

            _dummyUserService.Save(command);
            var newUser = _dummyUserService.Find(command.Username);
            return ResponsePost(nameof(Get), new { id = command.Username }, newUser);
        }

        [HttpPatch("{username}")]
        public ActionResult Patch(string username, [FromBody] JsonPatchDocument<User> model)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return ModelStateErrorResponseError();
            }

            var actualUser = _dummyUserService.Find(username);
            model.ApplyTo(actualUser);
            _dummyUserService.Update(actualUser);
            return ResponsePutPatch();
        }


        [HttpPut("{username}")]
        public ActionResult Put(string username, [FromBody] UpdateUserCommand model)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return ModelStateErrorResponseError();
            }

            var actual = _dummyUserService.Find(username);
            model.Update(actual);
            _dummyUserService.Update(actual);
            return ResponsePutPatch();
        }

    }
}
