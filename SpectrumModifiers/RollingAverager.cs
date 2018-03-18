using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCorePlaying.SpectrumModifiers {
	class RollingAverager:IRGBSpectrumModifier {
		private int[,] averageHistory;
		private int currentRow;
		
		public RollingAverager(int lightCount, int historySize) {
			averageHistory = new int[historySize, lightCount*3];
			currentRow = 0;
		}
		public override void modifySpectrum(Color[] inputColors) {
			int[] input = new int[inputColors.Length * 3];

			//QuickN'Dirty Boy
			for (int i = 0; i<inputColors.Length; i++) {
				input[i*3] = inputColors[i].red;
				input[i * 3 + 1] = inputColors[i].green;
				input[i * 3 + 2] = inputColors[i].blue;
			}
			for (int i = 0; i < averageHistory.GetLength(1); i++) {
				averageHistory[currentRow, i] = input[i];
				input[i] = 0;
			}
			currentRow++;
			currentRow %= averageHistory.GetLength(0);

			for (int i = 0; i<averageHistory.GetLength(0); i++) {
				for(int j = 0;j<averageHistory.GetLength(1); j++) {
					input[j] += averageHistory[i, j];
				}
			}

			for (int i = 0; i < averageHistory.GetLength(1); i++) {
				input[i] = rgbClamp((int)Math.Round((double)((double)input[i]/(double)averageHistory.GetLength(0))));
			}

			//reverse taht Quickn'Dirty Boyo
			for (int i = 0; i < inputColors.Length; i++) {
				inputColors[i].red = input[i * 3];
				inputColors[i].green = input[i * 3 + 1];
				inputColors[i].blue = input[i * 3 + 2];
			}
			return;
		}
	}
}
