namespace ApiEntregasMentoria.Data.Dto
{
    public class MatchUpdateDto
    {
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
