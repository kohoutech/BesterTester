/* ----------------------------------------------------------------------------
Paste Up - a desktop publisher
Copyright (C) 2005-2021 George E Greaney

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
----------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PasteUp
{
    class PasteBox
    {
        public RectangleF rect;
        public Color color;
        public Color bordercolor;
        public bool isSelected;

        public PasteBox()
        {
            rect = new RectangleF(30, 30, 100, 100);
            color = Color.FromArgb(255, 128, 64);
            bordercolor = Color.Black;
            isSelected = false;
        }

        public virtual void paint(Graphics g)
        {
            if (isSelected)
            {
                Pen pen = new Pen(bordercolor);
                pen.Width = 5;
                g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
            }

            Brush brush = new SolidBrush(color);
            g.FillRectangle(brush, rect);


        }
    }
}
