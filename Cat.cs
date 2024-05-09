using MongoDB.Bson;
using Realms;
namespace RealmWinUI;
public partial class Cat : IRealmObject
{
    [PrimaryKey]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
   
    public string Name { get; set; } = string.Empty;
   
    public int Age { get; set; } = 0;
   
    public string Breed { get; set; } = string.Empty;
    
}


