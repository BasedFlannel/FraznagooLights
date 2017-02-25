using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCorePlaying.SpectrumModifiers {
	class ReflectedSpectrum:IRGBSpectrumModifier {
		private bool blnUseHalf;
		private bool blnUseTopHalf;
		public ReflectedSpectrum(bool useHalf = false, bool useTopHalf = false) {
			this.blnUseHalf = useHalf;
			this.blnUseTopHalf = useTopHalf;
		}
		public override void modifySpectrum(int[] input) {
			int[] averages = new int[input.Length / 2];
			if (this.blnUseHalf) {
				int iOffset = this.blnUseTopHalf ? input.Length/2 : 0;
				for (int i = 0; i<averages.Length; i++) {
					averages[i] = input[i + iOffset];
				}
			}
			else {
				for (int i = 0; i < input.Length; i += 6) {
					averages[i / 2] = rgbClamp(((int)((input[i] + input[i + 3]) / 2.0)));
					averages[i / 2 + 1] = rgbClamp(((int)((input[i + 1] + input[i + 4]) / 2.0)));
					averages[i / 2 + 2] = rgbClamp(((int)((input[i + 2] + input[i + 5]) / 2.0)));
				}
			}
			for (int i = 0; i < input.Length / 2; i++) {
				input[i] = averages[i];
			}
			for (int i = input.Length - 3; i > input.Length / 2 - 1; i -= 3) {
				input[i] = input[input.Length - 3 - i];
				input[i + 1] = input[input.Length - 2 - i];
				input[i + 2] = input[input.Length - 1 - i];
			}
		}
	}
}
