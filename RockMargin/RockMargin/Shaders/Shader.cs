using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace RockMargin
{
	class Shader : ShaderEffect
	{
		private static PixelShader _shader = new PixelShader();

		public Shader()
		{
			this.PixelShader = _shader;
			this.PixelShader.UriSource = new Uri(Utils.GetEmbedeedFilePath("Shaders/shader.ps"));
			UpdateShaderValue(InputProperty);
			UpdateShaderValue(OpacityProperty);
		}

		public Brush Input
		{
			get { return (Brush)GetValue(InputProperty); }
			set { SetValue(InputProperty, value); }
		}

		public static readonly DependencyProperty InputProperty =
			ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(Shader), 0);

	public double Opacity
	{
		get { return (double)GetValue(OpacityProperty); }
		set { SetValue(OpacityProperty, value); }
	}

	public static readonly DependencyProperty OpacityProperty =
			DependencyProperty.Register("Opacity", typeof(double), typeof(Shader),
				new UIPropertyMetadata(1.0d, PixelShaderConstantCallback(0)));
	}
}
