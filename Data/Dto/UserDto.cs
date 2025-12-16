namespace ApiEntregasMentoria.Data.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Cpf { get; set; }
        public decimal Balance { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
    }

    public class UpdateUserDto
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
    }

    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public class UserStatisticsDto
    {
        public int TotalBets { get; set; }
        public int WonBets { get; set; }
        public int LostBets { get; set; }
        public int PendingBets { get; set; }
        public decimal TotalBetAmount { get; set; }
        public decimal TotalWinnings { get; set; }
        public decimal WinRate { get; set; }
        public decimal Roi { get; set; }
    }
}
