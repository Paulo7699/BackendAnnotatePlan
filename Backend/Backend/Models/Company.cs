using Backend.Models.Common;
using Google.Cloud.Firestore;

namespace Backend.Models;

[FirestoreData]
public class Company : IFirestoreDocument
{
    [FirestoreDocumentId]
    public string Id { get; set; }
}