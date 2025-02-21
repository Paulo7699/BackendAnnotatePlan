using Backend.Models.Common;
using Google.Cloud.Firestore;

namespace Backend.Models;

[FirestoreData]
public class User : IFirestoreDocument
{
    [FirestoreDocumentId]
    public string Id { get; set; }

    [FirestoreProperty]
    public string Email { get; set; }

    [FirestoreProperty]
    public string DisplayName { get; set; }
}