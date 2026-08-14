using TC1.RepairShop.Domain.Entities.Common;

namespace TC1.RepairShop.Domain.Entities.Costumers
{
    public class Costumer : BaseEntity
    {
        public string Name { get; private set; }
        public string NationalId { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }

        private Costumer(string name, string nationalId, string phone, string email) : base()
        {
            Name = name;
            NationalId = nationalId;
            Phone = phone;
            Email = email;
        }

        public static Costumer Create(string name, string nationalId, string phone, string email)
        {

            return new Costumer(name, nationalId, phone, email);
        }

        public void UpdateContactInfo(string phone, string email)
        {
            Phone = phone;
            Email = email;
        }

        public bool VerifyPassword(string password)
        {
            return true;
        }

        public void ChangePassword(string password) { 
        }
    }
}
