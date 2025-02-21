using Backend.Models;
using Backend.Services.Interfaces;
using Backend.Utils;
using Backend.Utils.Converters;
using Google.Cloud.Firestore;

namespace Backend.Services;

public class UserService : AbstractDatabaseService<User>, IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string CollectionName = "users";
    private ILogger<IUserService> _logger;

    public UserService(FirestoreDb db,
        IHttpContextAccessor httpContextAccessor, ILogger<IUserService> logger) : base(db, CollectionName)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    protected override User ConvertDocumentField(DocumentSnapshot doc)
    {
        return UserConverters.ConvertDocumentField(doc);
    }

    protected override bool CanDelete(User model)
    {
        return false;
    }

    public async Task<User?> GetMe()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst("uid")?.Value;
        
        _logger.LogInformation($"User  id: {userId}");
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }

        User? user = await GetByIdAsync(userId);
        
        return user;
    }
}