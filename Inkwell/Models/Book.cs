using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;

namespace Inkwell.Models
{
    public class BookDto
    { 
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [DefaultValue(null)]
        public List<string>? Genres { get; set; } = null;

        [DefaultValue(false)]
        public bool IsFavourite { get; set; } = false;

        [DefaultValue(false)]
        public bool IsRead { get; set; } = false;

    }

    public class Book : BookDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? UserId { get; set; }
    }
}
