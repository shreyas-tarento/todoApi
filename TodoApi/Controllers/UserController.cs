﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController:ControllerBase
    {
        private readonly IUser _iUser;
        public UserController(IUser iUser)
        {
            _iUser = iUser;
        }
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(_iUser.GetAllUsers());
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetUser([FromRoute] int id)
        {
            var existingUserInfo = _iUser.GetUserInfo(id);
            if (existingUserInfo == null)
                return NotFound($"User Details for id:#{id} not found");

            return Ok(_iUser.GetUser(id));
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDetails userDetails)
        {
            if (userDetails == null)
                return BadRequest();

            var doesUseExistOnUserName = _iUser.UserExistOnUserName(userDetails.UserName);
            if (doesUseExistOnUserName)
                return BadRequest($"{userDetails.UserName} user name already exists");

            return new JsonResult(_iUser.CreateUser(userDetails))
            {
                StatusCode = 201
            };
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateUser([FromRoute]int id, [FromBody]UserDetails userDetails)
        {
            if (id != userDetails.UserId)
                return BadRequest("UserId mismatch");

            var existingUserDetails = _iUser.GetUserInfo(id);
            if (existingUserDetails == null)
                return NotFound($"User Details for id:#{id} not found");

            return Ok(_iUser.UpdateUser(id, userDetails));
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteUser([FromRoute]int id)
        {
            var existingUserInfo = _iUser.GetUserInfo(id);
            if (existingUserInfo == null)
                return NotFound($"User Details for id:#{id} not found");

            var existingUserAddress = _iUser.GetUserAddress(id);
            var existingUserContact = _iUser.GetUserContact(id);
            return Ok(_iUser.DeleteUser(id));
        }

    }
}
