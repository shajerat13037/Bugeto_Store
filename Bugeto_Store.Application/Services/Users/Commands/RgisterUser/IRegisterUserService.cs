using Bugeto_Store.Application.Interfaces.Contexts;
using Bugeto_Store.Common.Dto;
using Bugeto_Store.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugeto_Store.Application.Services.Users.Commands.RgisterUser
{
    public interface IRegisterUserService
    {
        ResultDto<ResultRegisterUserDto> Execute(RequestRegisterUserDto request);
    }

    public class RegisterUserService : IRegisterUserService
    {
        private readonly IDataBaseContext _context;
        public RegisterUserService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ResultRegisterUserDto> Execute(RequestRegisterUserDto request)
        {
            User user = new User()
            {
                Email = request.Email,
                FullName = request.FullName,
            };

            List<UserInRole> userInRoles = new List<UserInRole>();
            foreach (var item in request.roles)
            {
                var roles = _context.Roles.Find(item.Id);
                userInRoles.Add(new UserInRole
                {
                  Role= roles,
                  RoleId = roles.Id,
                  User = user,
                  UserId=user.Id,
                });
            }

            user.UserInRoles = userInRoles;

            _context.Users.Add(user);
            _context.SaveChanges();

            return new ResultDto<ResultRegisterUserDto>()
            {
                Data = new ResultRegisterUserDto()
                {
                    UserId = user.Id,

                },
                IsSuccess = true,
                Message = "ثبت نام کاربر انجام شد",
            };
        }
    }

    public class RequestRegisterUserDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<RolesDtoRegisterUserDto> roles { get; set; }

    }

    public class RolesDtoRegisterUserDto
    {
        public long Id { get; set; }

    }

    public class ResultRegisterUserDto
    {
        public long UserId { get; set; }


    }
}
