﻿namespace DataAccess.DTO.Member
{
    public class MemberRequestDTO
    {
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string City { get; set; }
        public string? Country { get; set; }
        public string? Password { get; set; }
    }
}
