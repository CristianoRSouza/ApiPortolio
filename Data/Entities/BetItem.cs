using ApiEntregasMentoria.Data.Entities;

public class BetItem
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string BetType { get; set; }
    public string? Result { get; set; }
    public DateTime BetDate { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int MatchId { get; set; }
    public Match Match { get; set; }

    public int BetId { get; set; }              // Adicionado
    public Bet Bet { get; set; }                // Adicionado
}
