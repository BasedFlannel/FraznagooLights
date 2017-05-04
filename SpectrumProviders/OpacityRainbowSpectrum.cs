using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore.DSP;

namespace CSCorePlaying.SpectrumProviders {
	class OpacityRainbowSpectrum:RGBSpectrum {
		int basePos,speed;
		double minimumBrightness;
		double maximumBrightness;
		double brightnessModifier;
		double addVal;

		public OpacityRainbowSpectrum(FftSize objFFTSize, int speed = 1, double minimumBrightness=0.0, double maximumBrightness=1.0, double brightnessModifier = 1.0, double addVal = 0.0) : base(objFFTSize) {
			this.basePos = 0;
			this.speed = speed;
			this.minimumBrightness = minimumBrightness;
			this.maximumBrightness = maximumBrightness;
			this.brightnessModifier = brightnessModifier;
			this.addVal = addVal;
		}

		protected override int[] CreateRGBSpectrumInternal(float[] fftBuffer) {
			SpectrumPointData[] spectrumPoints = CalculateSpectrumPoints(1, fftBuffer);
			int[] arrRGB = new int[spectrumPoints.Length * 3];
			int[] rgbVals;
			int pos;
			for (int i = 0; i < spectrumPoints.Length; i++) {
				SpectrumPointData p = spectrumPoints[i];
				p.Value = (p.Value * brightnessModifier)+addVal;
				p.Value = this.clampDouble(p.Value, this.maximumBrightness, this.minimumBrightness);
				pos = ((int)((i/((float)spectrumPoints.Length))* 768.0) + basePos) % 768;
				rgbVals = wheel(pos);
				arrRGB[i * 3] = (int)(rgbVals[0] *p.Value);
				arrRGB[i * 3 + 1] = (int)(rgbVals[1] * p.Value);
				arrRGB[i * 3 + 2] = (int)(rgbVals[2] * p.Value);
			}
			Console.WriteLine();
			this.basePos+=this.speed;
			this.basePos %= 768;
			return arrRGB;
		}
	}
}
