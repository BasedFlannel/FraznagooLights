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
		public override void modifySpectrum(Color[] input) {
			Color[] averages = new Color[input.Length / 2];
			if (this.blnUseHalf) {
				int startPos = blnUseTopHalf ? input.Length / 2 : 0;
				Array.Copy(input, startPos, averages, 0, averages.Length);
			}
			else {
				for (int i = 0; i < input.Length; i +=2) {
					averages[i / 2] = Color.getAverageColor(input[i], input[i + 1]);
				}
			}

			//averages set up, now duplicate them
			Array.Copy(averages, input, averages.Length);
			Array.Reverse(averages);
			Array.Copy(averages,0, input,averages.Length, averages.Length);
		}
	}
}
