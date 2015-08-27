using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarApp.Model
{
	public class Audit
	{

		public string Who { get; set; }

		public string What { get; set; }

		public DateTime When { get; set; }

		public string Where { get; set; }

		public bool IsException { get; set; }

		public Audit(string who, string what, string where, bool isException = false)
		{
			Who = who;
			What = what;
			Where = where;
			IsException = isException;
			When = DateTime.UtcNow;
		}

	}
}
