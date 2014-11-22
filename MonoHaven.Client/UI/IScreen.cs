﻿#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System.Drawing;
using MonoHaven.Graphics;
using OpenTK;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public interface IScreen
	{
		void Show();
		void Close();
		void Resize(int newWidth, int newHeight);
		void Draw(DrawingContext dc);
		void Update(int dt);

		void MouseButtonDown(MouseButtonEventArgs e);
		void MouseButtonUp(MouseButtonEventArgs e);
		void MouseMove(MouseMoveEventArgs e);
		void MouseWheel(MouseWheelEventArgs e);
		void KeyDown(KeyboardKeyEventArgs e);
		void KeyUp(KeyboardKeyEventArgs e);
		void KeyPress(KeyPressEventArgs e);
	}

	public static class ScreenExt
	{
		public static void Resize(this IScreen screen, Size newSize)
		{
			screen.Resize(newSize.Width, newSize.Height);
		}
	}
}
