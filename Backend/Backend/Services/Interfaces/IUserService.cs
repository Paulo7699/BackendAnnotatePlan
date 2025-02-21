using Backend.Models;

namespace Backend.Services.Interfaces;

public interface IUserService : IAbstractService<User>
{
    Task<User?> GetMe();
}