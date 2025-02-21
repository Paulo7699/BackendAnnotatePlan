using Backend.Models;
using Backend.Models.Common;
using Google.Cloud.Firestore;
using Newtonsoft.Json;

namespace Backend.Utils.Converters;

public static class UserConverters
{
    public static User ConvertDocumentField(DocumentSnapshot doc)
    {
        var data = doc.ToDictionary();
        return new User
        {
            Id = doc.Id,
            Email = data.GetValueOrDefault("email")?.ToString() ?? "",
            DisplayName = data.GetValueOrDefault("displayName")?.ToString() ?? ""
        };
    }
}