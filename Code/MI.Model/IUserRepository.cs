namespace MI.Model
{
    public interface IUserRepository
    {
        IAppUser GetAuthenticatedUserOrNull(string usernameOrEmail, string password);
        IAppUser GetUserByName(string name);
        IAppUser GetUser(int id);
        void InsertUser(IAppUser user);
        void UpdateUser(IAppUser user);
        void DeleteUser(IAppUser user);
    }
}
