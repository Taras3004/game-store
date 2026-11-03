using GameStore.DAL.Mongo.Entities.ConfigurationEntities;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.Mongo.Entities
{
    internal class Employee : NorthwindEntity
    {
        [BsonElement("EmployeeID")]
        public int EmployeeId { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Title { get; set; }
        public string? TitleOfCourtesy { get; set; }
        public string? BirthDate { get; set; }
        public string? HireDate { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? HomePhone { get; set; }
        public string? Extension { get; set; }
        public string? Photo { get; set; }
        public string? Notes { get; set; }
        public string? ReportsTo { get; set; }
        public string? PhotoPath { get; set; }
    }
}
