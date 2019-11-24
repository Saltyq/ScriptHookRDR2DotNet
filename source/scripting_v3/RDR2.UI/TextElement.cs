//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace RDR2.UI
{
	public class TextElement : IElement
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TextElement"/> class used for drawing text on the screen.
		/// </summary>
		/// <param name="caption">The <see cref="TextElement"/> to draw.</param>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="TextElement"/>.</param>
		/// <param name="scale">Sets a <see cref="Scale"/> used to increase of decrease the size of the <see cref="TextElement"/>, for no scaling use 1.0f.</param>
		public TextElement(string caption, PointF position, float scale) :
			this(caption, position, scale, Color.WhiteSmoke, Alignment.Left, false, false, 0.0f)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="TextElement"/> class used for drawing text on the screen.
		/// </summary>
		/// <param name="caption">The <see cref="TextElement"/> to draw.</param>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="TextElement"/>.</param>
		/// <param name="scale">Sets a <see cref="Scale"/> used to increase of decrease the size of the <see cref="TextElement"/>, for no scaling use 1.0f.</param>
		/// <param name="color">Set the <see cref="Color"/> used to draw the <see cref="TextElement"/>.</param>
		public TextElement(string caption, PointF position, float scale, Color color) :
			this(caption, position, scale, color, Alignment.Left, false, false, 0.0f)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextElement"/> class used for drawing text on the screen.
		/// </summary>
		/// <param name="caption">The <see cref="TextElement"/> to draw.</param>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="TextElement"/>.</param>
		/// <param name="scale">Sets a <see cref="Scale"/> used to increase of decrease the size of the <see cref="TextElement"/>, for no scaling use 1.0f.</param>
		/// <param name="color">Set the <see cref="Color"/> used to draw the <see cref="TextElement"/>.</param>
		/// <param name="alignment">Sets the <see cref="Alignment"/> used when drawing the text, <see cref="RDR2.UI.Alignment.Left"/>,<see cref="RDR2.UI.Alignment.Center"/> or <see cref="RDR2.UI.Alignment.Right"/>.</param>
		public TextElement(string caption, PointF position, float scale, Color color, Alignment alignment) :
			this(caption, position, scale, color, alignment, false, false, 0.0f)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="TextElement"/> class used for drawing text on the screen.
		/// </summary>
		/// <param name="caption">The <see cref="TextElement"/> to draw.</param>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="TextElement"/>.</param>
		/// <param name="scale">Sets a <see cref="Scale"/> used to increase of decrease the size of the <see cref="TextElement"/>, for no scaling use 1.0f.</param>
		/// <param name="color">Set the <see cref="Color"/> used to draw the <see cref="TextElement"/>.</param>
		/// <param name="alignment">Sets the <see cref="Alignment"/> used when drawing the text, <see cref="RDR2.UI.Alignment.Left"/>,<see cref="RDR2.UI.Alignment.Center"/> or <see cref="RDR2.UI.Alignment.Right"/>.</param>
		/// <param name="shadow">Sets whether or not to draw the <see cref="TextElement"/> with a <see cref="Shadow"/> effect.</param>
		/// <param name="outline">Sets whether or not to draw the <see cref="TextElement"/> with an <see cref="Outline"/> around the letters.</param>
		public TextElement(string caption, PointF position, float scale, Color color, Alignment alignment, bool shadow, bool outline) :
			this(caption, position, scale, color, alignment, shadow, outline, 0.0f)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="TextElement"/> class used for drawing text on the screen.
		/// </summary>
		/// <param name="caption">The <see cref="TextElement"/> to draw.</param>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="TextElement"/>.</param>
		/// <param name="scale">Sets a <see cref="Scale"/> used to increase of decrease the size of the <see cref="TextElement"/>, for no scaling use 1.0f.</param>
		/// <param name="color">Set the <see cref="Color"/> used to draw the <see cref="TextElement"/>.</param>
		/// <param name="alignment">Sets the <see cref="Alignment"/> used when drawing the text, <see cref="RDR2.UI.Alignment.Left"/>,<see cref="RDR2.UI.Alignment.Center"/> or <see cref="RDR2.UI.Alignment.Right"/>.</param>
		/// <param name="shadow">Sets whether or not to draw the <see cref="TextElement"/> with a <see cref="Shadow"/> effect.</param>
		/// <param name="outline">Sets whether or not to draw the <see cref="TextElement"/> with an <see cref="Outline"/> around the letters.</param>
		/// <param name="wrapWidth">Sets how many horizontal pixel to draw before wrapping the <see cref="TextElement"/> on the next line down.</param>
		public TextElement(string caption, PointF position, float scale, Color color, Alignment alignment, bool shadow, bool outline, float wrapWidth)
		{
			_pinnedText = new List<IntPtr>();
			Enabled = true;
			Caption = caption;
			Position = position;
			Scale = scale;
			Color = color;
			Alignment = alignment;
			Shadow = shadow;
			Outline = outline;
			WrapWidth = wrapWidth;
		}

		~TextElement()
		{
			foreach (var ptr in _pinnedText)
			{
				Marshal.FreeCoTaskMem(ptr); //free any existing allocated text
			}
			_pinnedText.Clear();
		}

		private string _caption;
		private readonly List<IntPtr> _pinnedText;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="TextElement" /> will be drawn.
		/// </summary>
		/// <value>
		///   <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		public bool Enabled
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets the color of this <see cref="TextElement" />.
		/// </summary>
		/// <value>
		/// The color.
		/// </value>
		public Color Color
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets the position of this <see cref="TextElement" />.
		/// </summary>
		/// <value>
		/// The position scaled on a 1280*720 pixel base.
		/// </value>
		/// <remarks>
		/// If ScaledDraw is called, the position will be scaled by the width returned in <see cref="Screen.ScaledWidth" />.
		/// </remarks>
		public PointF Position
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets the scale of this <see cref="TextElement"/>.
		/// </summary>
		/// <value>
		/// The scale usually a value between ~0.5 and 3.0, Default = 1.0
		/// </value>
		public float Scale
		{
			get; set;
		}

		/// <summary>
		/// Gets or sets the text to draw in this <see cref="TextElement"/>.
		/// </summary>
		/// <value>
		/// The caption.
		/// </value>
		public string Caption
		{
			get
			{
				return _caption;
			}
			set
			{
				_caption = value;
				foreach (var ptr in _pinnedText)
				{
					Marshal.FreeCoTaskMem(ptr); //free any existing allocated text
				}
				_pinnedText.Clear();

			}
		}

		/// <summary>
		/// Gets or sets the alignment of this <see cref="TextElement"/>.
		/// </summary>
		/// <value>
		/// The alignment:<c>Left</c>, <c>Center</c>, <c>Right</c> Justify
		/// </value>
		public Alignment Alignment
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="TextElement"/> is drawn with a shadow effect.
		/// </summary>
		/// <value>
		///   <c>true</c> if shadow; otherwise, <c>false</c>.
		/// </value>
		public bool Shadow
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="TextElement"/> is drawn with an outline.
		/// </summary>
		/// <value>
		///   <c>true</c> if outline; otherwise, <c>false</c>.
		/// </value>
		public bool Outline
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets the maximum size of the <see cref="TextElement"/> before it wraps to a new line.
		/// </summary>
		/// <value>
		/// The width of the <see cref="TextElement"/>.
		/// </value>
		public float WrapWidth
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets a value indicating whether the alignment of this <see cref="TextElement" /> is centered.
		/// See <see cref="Alignment"/>
		/// </summary>
		/// <value>
		///   <c>true</c> if centered; otherwise, <c>false</c>.
		/// </value>
		public bool Centered
		{
			get
			{
				return Alignment == Alignment.Center;
			}
			set
			{
				if (value)
				{
					Alignment = Alignment.Center;
				}
			}
		}

		/// <summary>
		/// Draws the <see cref="TextElement" /> this frame.
		/// </summary>
		public virtual void Draw()
		{
			Draw(SizeF.Empty);
		}
		/// <summary>
		/// Draws the <see cref="TextElement" /> this frame at the specified offset.
		/// </summary>
		/// <param name="offset">The offset to shift the draw position of this <see cref="TextElement" /> using a 1280*720 pixel base.</param>
		public virtual void Draw(SizeF offset)
		{
			InternalDraw(offset, Screen.Width, Screen.Height);
		}

		/// <summary>
		/// Draws the <see cref="TextElement" /> this frame using the width returned in <see cref="Screen.ScaledWidth" />.
		/// </summary>
		public virtual void ScaledDraw()
		{
			ScaledDraw(SizeF.Empty);
		}
		/// <summary>
		/// Draws the <see cref="TextElement" /> this frame at the specified offset using the width returned in <see cref="Screen.ScaledWidth" />.
		/// </summary>
		/// <param name="offset">The offset to shift the draw position of this <see cref="TextElement" /> using a <see cref="Screen.ScaledWidth" />*720 pixel base.</param>
		public virtual void ScaledDraw(SizeF offset)
		{
			InternalDraw(offset, Screen.ScaledWidth, Screen.Height);
		}

		void InternalDraw(SizeF offset, float screenWidth, float screenHeight)
		{
			if (!Enabled)
			{
				return;
			}

			float x = (Position.X + offset.Width) / screenWidth;
			float y = (Position.Y + offset.Height) / screenHeight;
			float w = WrapWidth / screenWidth;

			if (Shadow)
			{
				Function.Call(Hash.SET_TEXT_DROPSHADOW,2,0,0,0,255);
			}

			Function.Call(Hash.SET_TEXT_SCALE, Scale, Scale);
			Function.Call(Hash._SET_TEXT_COLOR, Color.R, Color.G, Color.B, Color.A);
			string varString = Function.Call<string>(Hash.CREATE_STRING, 10, "LITERAL_STRING", _caption);
			Function.Call(Hash._DRAW_TEXT, varString, x, y);

		}
	}
}
