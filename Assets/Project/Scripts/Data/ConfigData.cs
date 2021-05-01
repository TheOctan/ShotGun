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
		public float MasterVolume;
		public float MusicVolume;
		public float SoundVolume;
		public int ResolutionIndex;
		public bool IsFullScreen;
	}
}
