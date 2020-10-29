using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data
{
	public class SessionData
	{
		public int ShootCount { get; set; }
		public int ReloadCount { get; set; }
		public float TraveledDistance { get; set; }
		public float Time { get; set; }
		public string Date { get; set; }
		public float Score { get; set; }
		public float ShootDamage { get; set; }
		public float HitDamage { get; set; }
	}
}
