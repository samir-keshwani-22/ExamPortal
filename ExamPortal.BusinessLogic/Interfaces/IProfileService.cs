using ExamPortal.BusinessLogic.ViewModel.Profile;
using ExamPortal.DataAccess.Models;

namespace ExamPortal.BusinessLogic.Interfaces
{
    public interface IProfileService
    {
        Task<MyProfileViewModel> GetUserProfileAsync(string email);
        Task<bool> ChangeUserPasswordAsync(string email, string oldPassword, string newPassword);
        Task<bool> UpdateUserProfileAsync(MyProfileViewModel model);
        Task<User?> GetNameAndImage(string email);
    }
}