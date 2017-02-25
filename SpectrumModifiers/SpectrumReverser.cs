using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCorePlaying.SpectrumModifiers {
	class SpectrumReverser:IRGBSpectrumModifier {
		public override void modifySpectrum(int[] input) {
			int r, g, b = 0;
			for (int i=0; i < input.Length/2; i+=3) {
				swap(input, i, input.Length - 3 - i);
				swap(input, i + 1, input.Length - 2 - i);
				swap(input, i +2, input.Length - 1 - i);
			}
		}

		private void swap(int[] input, int first, int second) {
			int temp = input[first];
			input[first] = input[second];
			input[second] = temp;
		}
	}
}
