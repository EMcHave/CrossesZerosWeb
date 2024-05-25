namespace CrossesZerosWeb
{
	public class GameResult
	{
		public int Id { get; set; }
		public string Winner { get; set; } = default!;
		public int Moves { get; set; } = default!;
		public string Bot { get; set; } = default!;
	}
}
