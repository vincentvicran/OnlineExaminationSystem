using System;
using System.Collections.Generic;
using System.Text;
using ViewModels;

namespace BLL.Services
{
    public interface IAccountService
    {
        LoginViewModel Login(LoginViewModel vm);
        bool AddTeacher(UserViewModel vm);
        PagedResult<UserViewModel> GetAllTeachers(int pageNumber, int pageSize);

        bool GetTeacher(UserViewModel vm);
        bool GetTeacherById(int id);
        bool DeleteTeacher(UserViewModel vm);
    }
}
