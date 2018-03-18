using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCorePlaying.SpectrumModifiers.Bars {
	class BaseBarModifier:IRGBSpectrumModifier {
		/* I don't actually remember what this does at all.
		int r, g, b, minIndex, maxIndex;

		public BaseBarModifier(int startBar = 0, int endBar = -1) {
			this.minIndex = startBar*3;
			this.maxIndex = endBar*3;
		}
		*/
		public override void modifySpectrum(Color[] input) {
			//createBar(input);
		}
		/*
		public void createBar(int[] input, int index = 0) {
			if (this.maxIndex <= 0) {
				r = input[index];
				g = input[index + 1];
				b = input[index + 2];
			}
			else {
				for (int i = this.minIndex; i<= this.maxIndex; i+=3) {
					r += input[i];
					g += input[i+1];
					b += input[i+2];
				}
				r /= (this.maxIndex - this.minIndex); g /= (this.maxIndex - this.minIndex); b /= (this.maxIndex - this.minIndex);
			}
			for (int i = 0; i < input.Length; i += 3) {
				input[i] = r;
				input[i + 1] = g;
				input[i + 2] = b;
			}
		}*/
	}
}
