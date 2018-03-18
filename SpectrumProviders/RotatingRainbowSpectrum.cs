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

		protected override Color[] CreateRGBSpectrumInternal(float[] fftBuffer) {
			SpectrumPointData[] spectrumPoints = CalculateSpectrumPoints(512, fftBuffer);
			Color[] arrColors = Color.createEmptyArray(spectrumPoints.Length);

			this.baseWheelPos++;
			this.baseWheelPos %= 768;

			for (int i = 0; i < spectrumPoints.Length; i++) {
				arrColors[i].setTo(wheel((int)(baseWheelPos + spectrumPoints[i].Value)%768));
			}
			return arrColors;
		}
	}
}
