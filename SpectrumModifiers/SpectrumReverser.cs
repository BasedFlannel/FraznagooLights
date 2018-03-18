using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCorePlaying.SpectrumModifiers {
	class SpectrumReverser:IRGBSpectrumModifier {
		public override void modifySpectrum(Color[] input) {
			Array.Reverse(input);
		}
	}
}
