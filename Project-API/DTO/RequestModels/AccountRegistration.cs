namespace Project_API.DTO.RequestModels
{
    public class AccountRegistration
    {
        public string UserName { get; set; } = null!;
       
        public string UserPassword { get; set; } = null!;
    
        public string FullName { get; set; } = null!;
     
        public string UserAddress { get; set; } = null!;
    
        public bool? IsOver18 { get; set; }
    
        public string EmailAddress { get; set; } = null!;

        public string? TelNumber { get; set; }

        public int? IdUsers { get; set; }
 
        public int? RestaurantId { get; set; }
    
        public int Role { get; set; }
    }
}
