﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Drawing;

namespace Tools.Diagrams.Drawables
{
	public interface IDrawable
	{
		void DrawToGraphics (Graphics graphics);
	}
}
