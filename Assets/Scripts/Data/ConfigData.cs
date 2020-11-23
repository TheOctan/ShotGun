using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data
{
	[Serializable]
	public class ConfigData
	{
		public string Nickname { get; set; }
		public float MasterVolume { get; set; }
		public float MusicVolume { get; set; }
		public float SoundVolume { get; set; }
		public int ResolutionIndex { get; set; }
		public bool IsFullScreen { get; set; }
	}
}
