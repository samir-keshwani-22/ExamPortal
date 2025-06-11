using ExamPortal.BusinessLogic.ViewModel.Profile;

namespace ExamPortal.BusinessLogic.Interfaces
{
    public interface IProfileService
    {
        Task<MyProfileViewModel> GetUserProfileAsync(string email);
        Task<bool> ChangeUserPasswordAsync(string email, string oldPassword, string newPassword);
        Task<bool> UpdateUserProfileAsync(MyProfileViewModel model);

    }
}