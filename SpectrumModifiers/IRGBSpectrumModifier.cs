using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCorePlaying.SpectrumModifiers {
	abstract class IRGBSpectrumModifier {
		public abstract void modifySpectrum(int[] input);

		public int rgbClamp(int val) {
			return(val < 0 ? 0 : val > 255 ? 255 : val);
		}
	}
}
