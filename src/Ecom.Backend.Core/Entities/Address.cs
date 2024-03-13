using System.ComponentModel.DataAnnotations.Schema;

namespace Ecom.Backend.Core.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        //Naviagational Properties
        public virtual string UserId { get; set; }
        public virtual User User { get; set; }
    }
}