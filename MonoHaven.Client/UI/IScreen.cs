﻿using MonoHaven.Graphics;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public interface IScreen
	{
		void Show();
		void Close();
		void Resize(int newWidth, int newHeight);
		void Draw(DrawingContext dc);

		void MouseButtonDown(MouseButtonEventArgs e);
		void MouseButtonUp(MouseButtonEventArgs e);
		void MouseMove(MouseMoveEventArgs e);
		void MouseWheel(MouseWheelEventArgs e);
		void KeyDown(KeyEventArgs e);
		void KeyUp(KeyEventArgs e);
		void KeyPress(KeyPressEventArgs e);
	}
}
