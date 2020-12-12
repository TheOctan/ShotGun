using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data
{
	[Serializable]
	public class LoginData
	{
		public bool IsLogined { get; set; }
		public string Nickname { get; set; }
	}
}
