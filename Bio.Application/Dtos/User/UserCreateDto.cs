namespace Bio.Application.Dtos.User
{
    public class UserCreateDto
    {
        public string FullName { get; set; }
        public string TcKimlik { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public string District { get; set; }
        public string Password { get; set; }
    }
}
