using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data
{
	[Serializable]
	public class ScoreData
	{
		public string Name { get; set; }
		public int Score { get; set; }
		public DateTime Date { get; set; }
	}
}
