using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCorePlaying {
	class Color {
		public int red {
			get { return red; }
			set {
				red = value & 255;
			}
		}
		public int green {
			get { return green; }
			set {
				green = value & 255;
			}
		}
		public int blue {
			get { return blue; }
			set {
				blue = value & 255;
			}
		}

		/*Constructors*/
		public Color(int red, int green, int blue) {
			this.red = red;
			this.blue = blue;
			this.green = green;	
		}

		/*utility*/

	}
}
