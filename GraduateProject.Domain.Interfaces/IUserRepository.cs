using GraduateProject.Domain.Core;

namespace GraduateProject.Domain.Interfaces
{
    public interface IUserRepository
    {
        User GetUser(Guid id);
        void CreateUser(User user);
        void UpdateUser(User user);

        void DeleteUser(Guid id);

        User? GetUserByLoginAndPassword(string login, string password);
    }
}
