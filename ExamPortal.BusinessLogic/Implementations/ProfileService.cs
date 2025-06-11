using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.Profile;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;

namespace ExamPortal.BusinessLogic.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository _userRepository;
        public ProfileService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> ChangeUserPasswordAsync(string email, string oldPassword, string newPassword)
        {
            User user = await _userRepository.GetByEmailAsync(email);
            if (user == null || (!BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash))) return false;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            return await _userRepository.UpdateAsync(user);
        }

        public async Task<User?> GetNameAndImage(string email)
        {
            User user = await _userRepository.GetByEmailAsync(email);
            return user;
        }

        public async Task<MyProfileViewModel> GetUserProfileAsync(string email)
        {
            User user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return null;
            return new MyProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phonenumber = user.MobileNumber,
                Address = user.Address,
                Zipcode = user.Zipcode,
                Email = user.Email,
                ProfileImage = user.ProfileImg
            };
        }

        public async Task<bool> UpdateUserProfileAsync(MyProfileViewModel model)
        {
            User user = await _userRepository.GetByEmailAsync(model.Email);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.MobileNumber = model.Phonenumber;
            user.Address = model.Address;
            user.Zipcode = model.Zipcode;
            if (!string.IsNullOrEmpty(model.ProfileImage))
            {
                user.ProfileImg = model.ProfileImage;
            }
            return await _userRepository.UpdateAsync(user);
        }

    }
}