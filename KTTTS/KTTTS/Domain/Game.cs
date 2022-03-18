using System.ComponentModel.DataAnnotations;

namespace KTTTS.Domain
{
    public class Game: BaseEntity
    {
        [MaxLength(128)]
        public string P1Email { get; set; } = default!;
        public string P1Key { get; set; } = default!;
        public int P1Counter { get; set; }
        [MaxLength(128)]
        public string P2Email { get; set; } = default!;
        public string P2Key { get; set; } = default!;
        public int P2Counter { get; set; }
        public string GameState { get; set; } = default!;
    }
}