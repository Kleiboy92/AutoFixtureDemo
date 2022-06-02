namespace api.Models
{
    public struct EmaildDto
    {
        public EmaildDto(string email, string phoneNumber)
        {
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public string Email { get; }
        public string PhoneNumber { get; }
    }
}
