using Backend.Models;

namespace Backend.Services.Interfaces;

public interface IAbstractService<T>
{
    Task<T> CreateAsync(T model);
    Task<T?> GetByIdAsync(string id);
    Task<T> UpdateAsync(string id, T model);
    Task DeleteAsync(string id);

    Task<List<T>> GetAll();
}