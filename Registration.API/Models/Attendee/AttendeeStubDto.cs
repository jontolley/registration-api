namespace Registration.API.Models
{
    public class AttendeeStubDto
    {
        public int Id { get; set; }
        public int SubgroupId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdult { get; set; }        
    }
}
