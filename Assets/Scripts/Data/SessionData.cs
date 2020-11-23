using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data
{
	[Serializable]
	public class SessionData
	{
		public int ShotCount { get; set; }
		public int ReloadCount { get; set; }
		public float TraveledDistance { get; set; }
		public float Duration { get; set; }
		public DateTime Date { get; set; }
		public int Score { get; set; }
		public float ShotDamage { get; set; }
		public float HitDamage { get; set; }
	}
}
