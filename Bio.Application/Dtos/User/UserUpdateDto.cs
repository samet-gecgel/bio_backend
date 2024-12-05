namespace Bio.Application.Dtos.User
{
    public class UserUpdateDto 
    {
        public string FullName { get; set; }
        public string TcKimlik { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public string District { get; set; }
    }
}
