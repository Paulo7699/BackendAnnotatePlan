using Backend.Models.Common;
using Backend.Services.Interfaces;
using Backend.Utils.Converters;
using Google.Cloud.Firestore;

namespace Backend.Services;

public abstract class AbstractDatabaseService<T>: IAbstractService<T> where T : IFirestoreDocument
{
    protected readonly FirestoreDb Db;
    private readonly string _collectionName;

    protected AbstractDatabaseService(FirestoreDb db, string collectionName)
    {
        Db = db;
        _collectionName = collectionName;
    }
    
    public async Task<T> CreateAsync(T activity)
    {
        var docRef = await Db.Collection(_collectionName).AddAsync(activity);
        activity.Id = docRef.Id;
        return activity;
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        var docRef = Db.Collection(_collectionName).Document(id);
        var snapshot = await docRef.GetSnapshotAsync();
        
        if (!snapshot.Exists)
            return default;

        var model = ConvertDocumentField(snapshot);
        return model;
    }
    
    public async Task<T> UpdateAsync(string id, T activity)
    {
        var docRef = Db.Collection(_collectionName).Document(id);
        await docRef.SetAsync(activity);
        activity.Id = id;
        return activity;
    }

    public async Task DeleteAsync(string id)
    {
        var docRef = Db.Collection(_collectionName).Document(id);
        var snapshot = await docRef.GetSnapshotAsync();
        var model = ConvertDocumentField(snapshot);
        
        if (!CanDelete(model))
        {
            throw new UnauthorizedAccessException("Cannot delete this document");
        }
        
        await docRef.DeleteAsync();
    }
    
    public async Task<List<T>> GetAll()
    {
        var query = Db.Collection(_collectionName);

        var snapshot = await query.GetSnapshotAsync();
        var listAll = snapshot.Documents
            .Select(ConvertDocumentField)
            .ToList();

        return listAll;
    }

    protected abstract T ConvertDocumentField(DocumentSnapshot doc);
    protected abstract bool CanDelete(T model);
}