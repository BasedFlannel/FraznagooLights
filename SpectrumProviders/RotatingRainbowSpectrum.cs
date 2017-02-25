using CSCore.DSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCorePlaying {
	class RotatingRainbowSpectrum : RGBSpectrum{
		protected int baseWheelPos;
		public RotatingRainbowSpectrum(FftSize objFFTSize) : base(objFFTSize) {
			baseWheelPos = 0;
		}

		protected override int[] CreateRGBSpectrumInternal(float[] fftBuffer) {
			SpectrumPointData[] spectrumPoints = CalculateSpectrumPoints(512, fftBuffer);
			int[] arrRGB = new int[spectrumPoints.Length * 3];
			int[] rgbVals;

			this.baseWheelPos++;
			this.baseWheelPos %= 768;

			for (int i = 0; i < spectrumPoints.Length; i++) {
				SpectrumPointData p = spectrumPoints[i];
				rgbVals = wheel((int)(baseWheelPos + p.Value)%768);
				arrRGB[i * 3] = rgbVals[0];
				arrRGB[i * 3 + 1] = rgbVals[1];
				arrRGB[i * 3 + 2] = rgbVals[2];
			}
			return arrRGB;
		}
	}
}
