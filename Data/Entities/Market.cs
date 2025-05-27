using ApiEntregasMentoria.Shared.Enums;

namespace ApiEntregasMentoria.Data.Entities
{
    public class Market
    {
        public int Id { get; set; }

        public MarketType NameEnum { get; set; }

        public int MatchId { get; set; }
        public Match Match { get; set; }

        public ICollection<BetItem> BetItems { get; set; } = new List<BetItem>();
    }
}
