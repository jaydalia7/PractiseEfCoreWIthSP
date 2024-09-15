using AutoMapper;
using FluentValidation.Results;
using Microsoft.IdentityModel.Tokens;
using PractiseEfCoreWIthSP.Models.Domains;
using PractiseEfCoreWIthSP.Models.ViewModels;
using PractiseEfCoreWIthSP.Repositories;
using PractiseEfCoreWIthSP.Repositories.IRepositories;
using PractiseEfCoreWIthSP.Services.IService;
using PractiseEfCoreWIthSP.Validations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace PractiseEfCoreWIthSP.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<WebApiReponseModel> CreateUser(UserAddModel userAddModel, CancellationToken cancellationToken)
        {
            WebApiReponseModel response = new WebApiReponseModel();
            try
            {
                //Validation
                UserValidation validationRules = new UserValidation();
                ValidationResult validationResult = validationRules.Validate(userAddModel);
                if (!validationResult.IsValid)
                {
                    string errorMessage = string.Empty;
                    foreach (FluentValidation.Results.ValidationFailure error in validationResult.Errors)
                    {
                        errorMessage += error.ErrorMessage;
                        break;

                    }
                    response.Status = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.ErrorMessage = errorMessage;
                    return response;
                }
                var IsEmailAddressExist = await _userRepository.GetUserByEmailAddress(userAddModel.EmailAddress, cancellationToken);
                if (IsEmailAddressExist != null)
                {
                    response.ErrorMessage = "User EmailAddress Already Exist";
                    response.Status = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }
                //AddUserData

                var userData = _mapper.Map<User>(userAddModel);
                var newUserData = await _userRepository.AddUser(userData, cancellationToken);
                if (newUserData.Id <= 0)
                {
                    response.Status = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.ErrorMessage = "Error while Adding User";
                    return response;
                }

                var newUser = _mapper.Map<UserDisplayModel>(newUserData);
                response.Status = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.Data = newUser;
                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = ex.Message.ToString();
                response.StatusCode = StatusCodes.Status400BadRequest;
                return response;
            }
        }

        public async Task<WebApiReponseModel> Login(Loginmodel loginmodel, CancellationToken cancellationToken)
        {
            WebApiReponseModel response = new WebApiReponseModel();
            try
            {
                //Validation
                LoginValidation validationRules = new LoginValidation();
                ValidationResult validationResult = validationRules.Validate(loginmodel);
                if (!validationResult.IsValid)
                {
                    var Errormessage = "";
                    foreach (FluentValidation.Results.ValidationFailure validationFailure in validationResult.Errors)
                    {
                        Errormessage += validationFailure.ErrorMessage;
                    }
                    response.Status = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.ErrorMessage = Errormessage;
                    return response;
                }
                //CheckExistByEmail
                var IsEmailAddressExist = await _userRepository.GetUserByEmailAddress(loginmodel.EmailAddress, cancellationToken);
                if (IsEmailAddressExist == null)
                {
                    response.ErrorMessage = "User EmailAddress Not Found";
                    response.Status = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }
                //Generate Token
                var token = Authenticate(IsEmailAddressExist);
                response.Status = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.Data = token;

                return response;

            }
            catch (Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = ex.Message.ToString();
                response.StatusCode = StatusCodes.Status400BadRequest;
                return response;
            }
        }
        private string Authenticate(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);

            var claims = new Dictionary<string, object>(){
                { ClaimTypes.Name, user.Name },
                { ClaimTypes.Email, user.EmailAddress }

            };

            //if (IsSuperAdmin)
            //    claims.Add(ClaimTypes.Role, "SuperAdmin");
            //else if (user.IsAdmin)
            //    claims.Add(ClaimTypes.Role, "Admin");
            //else
            //    claims.Add(ClaimTypes.Role, "User");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("sub", user.Id.ToString())
                }),

                Claims = claims,
                // Expires = DateTime.Now.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
