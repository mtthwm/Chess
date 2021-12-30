using System;

namespace Chess
{
	public class Player
	{
		public string name { get; private set; }
		public Teams team;

		public Player(string name, Teams team)
		{
			this.name = name;
			this.team = team;
		}
	}
}
