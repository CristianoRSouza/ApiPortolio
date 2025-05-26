using ApiEntregasMentoria.Data.Entities;

public class Bet
{
    public int Id { get; set; }
    public DateTime BetDate { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public ICollection<BetItem> Items { get; set; }
}
