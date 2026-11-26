using System.Collections.Generic;

namespace BananaBridge.UI.Models
{
	public class ChatTurn
	{
		public string Role { get; set; }
		public List<ChatPart> Parts { get; set; } = new List<ChatPart>();
	}
}

