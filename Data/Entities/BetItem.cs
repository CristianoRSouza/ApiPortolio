using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Shared.Enums;

public class BetItem
{
    public int Id { get; set; }
    public decimal Amount { get; set; }         
    public MarketType BetType { get; set; }          
    public BetResult? Result { get; set; }         
    public DateTime BetDate { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int BetId { get; set; }
    public Bet Bet { get; set; }

    public int MarketId { get; set; }          
    public Market Market { get; set; }
}
