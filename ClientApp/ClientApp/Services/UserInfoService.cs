using ClientApp.Models;

namespace ClientApp.Services
{
    public class UserInfoService
    {
        private User? _user;
        private House? _house;

        public bool IsLoggedIn() => _user != null;

        public User? GetUserInfo()
        {
            if (_user is null)
            {
                Console.WriteLine("User has not logged in yet");
            }
            return _user;
        }

        public void SetUserInfo(User? user)
        {
            if (user != null)
            { 
                _user = user;
            }
        }

        public House? GetHouseInfo() => _house;

        public void SetActiveHouse(House? house)
        {
            if (house != null)
            {
                _house = house;
            }
        }
    }
}
