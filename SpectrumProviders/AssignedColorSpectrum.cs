using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore.DSP;
using System.Collections;

namespace CSCorePlaying.SpectrumProviders {
	class AssignedColorSpectrum:RGBSpectrum {
		private Color[] startColorList;
		private Color[] endColorList;
		private int[] baseRGBValues;
		private int[] fullRGBValues;
		public AssignedColorSpectrum(FftSize objFFTSize, ArrayList startColors, ArrayList endColors) : base(objFFTSize) {
			//Allows for editing of color list on the fly
			//this.startColorList = startColors.ToArray(typeof(Color));
			//this.endColorList = endColors;
		}

		protected override Color[] CreateRGBSpectrumInternal(float[] fftBuffer) {
			SpectrumPointData[] spectrumPoints = CalculateSpectrumPoints(1, fftBuffer);

			//First time setup
			if (baseRGBValues == null) {
				initializeColors(spectrumPoints.Length);
			}

			Color[] arrColors = Color.createEmptyArray(spectrumPoints.Length);


			for (int i = 0; i < spectrumPoints.Length; i++) {
				SpectrumPointData p = spectrumPoints[i];

				/*Setting RGB value
				rgbVals = null;
				arrColors[i].red = (int)(rgbVals[0] *p.Value);
				arrColors[i].green = (int)(rgbVals[1] * p.Value);
				arrColors[i].blue = (int)(rgbVals[2] * p.Value);
				*/
			}
			Console.WriteLine();
			return arrColors;
		}
		
		private void initializeColors(int numLights) {
			/*this.baseRGBValues = new int[numLights * 3];
			this.fullRGBValues = new int[numLights * 3];
			int numColors, numLightsBetweenColors, listPos;
			Color currentColor, nextColor;
			//Set up start colors
			numColors = this.startColorList.Count;
			//subtract 1 to place last color at the very end of the light strip
			numLightsBetweenColors = numLights/(numColors-1);
			for(int i = 0; i<numColors-1; i++) {
				currentColor = startColorList.
			}
			//determine how many colors there are, determine how many lights are between each color, and repeatedly interpolate between lights.
			//Set up end colors
			numColors = this.endColorList.Count*/
		}

	}
}
