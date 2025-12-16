namespace ApiEntregasMentoria.Data.Dto
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class DepositRequestDto
    {
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }

    public class WithdrawRequestDto
    {
        public decimal Amount { get; set; }
        public string PixKey { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class PixPaymentDto
    {
        public int TransactionId { get; set; }
        public string QrCode { get; set; } = string.Empty;
        public string QrCodeBase64 { get; set; } = string.Empty;
        public string PixKey { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
