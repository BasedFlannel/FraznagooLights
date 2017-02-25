using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CSCorePlaying.Raindrops {
	class RaindropLight {
		protected  int	frame, 
						targetFrame;
		protected int[] startRGB;
		protected double[] currRGB;

		public RaindropLight(int[] rgb, int targetFrame = 50, int startFrame = 0) {
			this.setColor(rgb[1], rgb[2], rgb[3]);

		}

		/*accessors*/
		public virtual void getColor() {

		}
		public virtual void setColor(int red, int green, int blue) {
			red &= 255; green &= 255; blue &= 255;
			this.startRGB = new[] { red, green, blue };
			this.currRGB = new[] {(double)red, (double)green, (double)blue};
		}

		/*core*/
		public virtual void update() {

		}

		/*utility*/
		protected double getFadeFactor() {
			return Math.Sin((Math.PI / 2.0) * (((float)this.targetFrame - this.frame)/(float)this.targetFrame));
		}
	}
}
