using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCorePlaying {
	public class Color {
		private int _red;
		private int _green;
		private int _blue;

		public int red {
			get { return _red; }
			set {
				_red = value & 255;
			}
		}
		public int green {
			get { return _green; }
			set {
				_green = value & 255;
			}
		}
		public int blue {
			get { return _blue; }
			set {
				_blue = value & 255;
			}
		}

		/*Constructors*/
		public Color(int red, int green, int blue) {
			this.red = red;
			this.blue = blue;
			this.green = green;	
		}

		/*utility*/
		public Color setTo(Color c) {
			this.red = c.red;
			this.green = c.green;
			this.blue = c.blue;
			return this;
		}

		public static Color[] createEmptyArray(int size) {
			Color[] colorArray = new Color[size];
			for(int i = 0; i<colorArray.Length; i++) {
				colorArray[i] = new Color(0, 0, 0);
			}
			return colorArray;
		}

		public Color adjustBrightness(double brightness) {
			this.red = (int)(this.red * brightness);
			this.green = (int)(this.green * brightness);
			this.blue = (int)(this.blue * brightness);
			return this;
		}

		public static Color getAverageColor(Color c1, Color c2) {
			return new Color((c1.red+c2.red)/2, (c1.green + c2.green) / 2, (c1.blue + c2.blue) / 2);
		}
	}
}
