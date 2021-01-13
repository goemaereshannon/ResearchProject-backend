using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using IdentityServices.DTOs;
using IdentityServices.Models;
using AutoMapper;
using IdentityServices.Repositories;

namespace IdentityServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<User> signInMgr;
        private readonly IConfiguration configuration;
        private readonly IGenericRepo<User> repo;
        private readonly IMapper mapper;
        private readonly IPasswordHasher<User> hasher;
        private readonly UserManager<User> userManager;
        private readonly ILogger<AuthController> logger;

        public AuthController(SignInManager<User> signInMgr, IPasswordHasher<User> hasher, UserManager<User> userManager, RoleManager<Role> roleManager, ILogger<AuthController> logger, IConfiguration configuration, IGenericRepo<User> repo, IMapper mapper)
        {
            this.signInMgr = signInMgr;
            this.configuration = configuration;
            this.repo = repo;
            this.mapper = mapper;
            this.hasher = hasher;
            this.userManager = userManager;
            RoleManager = roleManager;
            this.logger = logger;
        }

        public RoleManager<Role> RoleManager { get; }

        /// <summary>
        /// Register a new administrator
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     Post /register
        ///     {
        ///         "email": "John.Doe@hotmail.com",
        ///         "password": "password",
        ///         
        ///         "Firstname":"John",
        ///         "LastName":"Doe"
        ///     }
        /// </remarks>
        [HttpPost]
        [Route("/api/auth/register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = mapper.Map<User>(registerDTO);

                    var result = await userManager.CreateAsync(user, registerDTO.Password);
                    List<IdentityError> resultList = new List<IdentityError>();


                    if (result.Succeeded)
                    {
                        var role = await userManager.AddToRoleAsync(user, "Customer");
                        string userId = userManager.FindByEmailAsync(user.Email).Result.Id.ToString();

                        return Created("api/auth/register", user);
                    }
                    else
                    {
                        resultList = result.Errors.ToList();
                        for (int i = 0; i < resultList.Count(); i++)
                        {
                            if (resultList[i].Code == "DuplicateUserName")
                            {
                                resultList[i].Description = "This emailadres '" + user.Email + "' is already in use.";
                            }
                        }
                    }
                    return BadRequest(string.Join(",", resultList?.Select(error => error.Description)));
                }

                string errorMessage = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return BadRequest(errorMessage ?? "Bad Request");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return new StatusCodeResult(500);
            }

        }
        /// <summary>
        /// Allow an existing user to login with their emailaddress and a password
        /// </summary>
        /// <param name="loginDTO"></param>
        /// /// Sample Request:
        /// 
        ///     Post /login
        ///     {
        ///         "email": "John.Doe@hotmail.com",
        ///         "password": "password",
        ///        
        ///     }
        /// <returns></returns>
        [HttpPost]
        [Route("/api/auth/login")]
        [AllowAnonymous]

        // [ValidateAntiForgeryToken]
        //CSRF: enkel nodig indien (statefull) via een browser , form ingelogd wordt
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        // , [FromQuery(Name = "d")] string destination = "frontend")
        {
            var returnMessage = "";

            try

            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.FindByEmailAsync(loginDTO.Email);
                    if (user == null)
                        return StatusCode((int)HttpStatusCode.Unauthorized, "The combination of emailadres and password is wrong. Please try again.");

                    //geen persistence, geen lockout -> via false, false 
                    var result = await signInMgr.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, false, false);

                    if (result.Succeeded)
                    {
                        try
                        {
                            //password controle gebeurt ook in de JWTService
                            //extra checks zijn mogelijk . bvb op basis vd rol en een querystring item
                            var jwtsvc = new JWTServices<User>(configuration, logger, userManager, hasher);
                            var token = await jwtsvc.GenerateJwtToken(loginDTO);
                            return Ok(token);  // HET TOKEN returnen
                        }
                        catch (Exception exc)
                        {
                            logger.LogError($"Exception thrown when creating JWT: {exc}");
                        }
                    }
                    return StatusCode((int)HttpStatusCode.Unauthorized, "The combination of emailadres and password is wrong. Please try again.");
                    //zo algemeen mogelijke boodschap. Vertel niet dat het pwd niet juist is.
                }
                throw new Exception("Please fill in all information");

            }
            catch (Exception exc)
            {
                returnMessage = $"Foutief of ongeldig request: {exc.Message}";
                ModelState.AddModelError("", returnMessage);
                Debug.WriteLine(exc.Message);
                return new StatusCodeResult(500);
            }
            // return BadRequest(returnMessage); //zo weinig mogelijk (hacker) info
        }
        /// <summary>
        /// Validate a user based on their email and a JWT token
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/auth/validate")]
        public async Task<IActionResult> Validate([FromQuery(Name = "email")] string email, [FromQuery(Name = "token")] string token)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var jwtsvc = new JWTServices<User>(configuration, logger, userManager, hasher);
            var userId = jwtsvc.ValidateToken(token);

            if (userId != user.Id)
            {
                return BadRequest("Invalid token.");
            }

            return new OkObjectResult(userId);
        }

        [HttpGet]
        [Route("/api/auth/{userId}")]
        public async Task<ActionResult<UserDTO>> GetUserProfilebyUserId(Guid userId)
        {
            try
            {
                var user = await repo.GetAsyncByGuid(userId);
                //var user = await userRepo.GetUserWithAddressByUserId(userId);

                if (user == null)
                {
                    return NotFound();
                }
                UserDTO userDTO = mapper.Map<User, UserDTO>(user);
                return Ok(userDTO);


            }
            catch (Exception ex)
            {

                logger.LogError(ex.Message);
                return new StatusCodeResult(500);
            }
        }

        /// <summary>
        /// Update user profile based on userId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("/api/auth/{userId}")]
        public async Task<ActionResult<UserDTO>> EditUser(Guid userId, UserDTO userDTO)
        {
            try
            {

                //check object
                if (userDTO == null || userId == null) return BadRequest();
                var newUser = mapper.Map<User>(userDTO);
                newUser.Id = userId;
                if (!await repo.Exists(newUser, userId)) return NotFound("User not found");

                ////get old object

                //User olduser = await repo.GetAsyncByGuid(userId);
                //if (olduser == null) return NotFound("User not found");

                //update

                await repo.Update(newUser, userId);
                return NoContent();





            }
            catch (Exception ex)
            {

                logger.LogError(ex.Message);
                return BadRequest("User could not be updated.");
            }
        }
        /// <summary>
        /// Delete user profile based on userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("/api/auth/{userId}")]
        public async Task<ActionResult> DeleteUser(Guid userId)
        {
            try
            {

                //check object
                if (userId == null) return BadRequest();
                var user = await repo.GetAsyncByGuid(userId);
                if (user == null) return NotFound("User not Found");

                await repo.Delete(user);
                return NoContent();


            }
            catch (Exception ex)
            {

                logger.LogError(ex.Message);
                return BadRequest("User could not be updated.");
            }

        }




    }
}
