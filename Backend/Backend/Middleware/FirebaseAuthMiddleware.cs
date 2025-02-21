using System.Security.Claims;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Logging;

namespace Backend.Middleware;

public class FirebaseAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<FirebaseAuthMiddleware> _logger;

    public FirebaseAuthMiddleware(RequestDelegate next, ILogger<FirebaseAuthMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string? authHeader = context.Request.Headers.Authorization.FirstOrDefault();
        
        if (string.IsNullOrEmpty(authHeader))
        {
            _logger.LogWarning("Authentication failed: No Authorization header present");
            context.Response.StatusCode = 401;
            return;
        }
        
        if (!authHeader.StartsWith("Bearer "))
        {
            _logger.LogWarning("Authentication failed: Authorization header does not start with 'Bearer'");
            context.Response.StatusCode = 401;
            return;
        }
        
        string idToken = authHeader.Substring("Bearer ".Length);
        _logger.LogInformation("Attempting to verify Firebase token");
        
        try
        {
            FirebaseToken firebaseToken = await FirebaseAuth.DefaultInstance
                .VerifyIdTokenAsync(idToken);
            
            _logger.LogInformation("Firebase token verified successfully for user {UserId}", firebaseToken.Uid);
            
            var claims = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new[] { new Claim("uid", firebaseToken.Uid) },
                    "Firebase"
                )
            );
            
            context.User = claims;
            _logger.LogDebug("Claims principal created and set for user {UserId}", firebaseToken.Uid);
        }
        catch (FirebaseAuthException ex)
        {
            _logger.LogError(ex, "Firebase authentication failed: {Message}", ex.Message);
            context.Response.StatusCode = 401;
            return;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during authentication");
            context.Response.StatusCode = 401;
            return;
        }
        
        await _next(context);
    }
}