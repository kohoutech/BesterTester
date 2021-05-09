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
    enum ResizeDirection { NONE, HORZ, VERT, BOTH};

    class PasteBox
    {
        Color BORDERCOLOR = Color.Black;
        DashStyle BORDERSTYLE = DashStyle.Dash;
        int BORDERWIDTH = 3;

        public RectangleF bounds;
        public RectangleF borderBox;
        public Color color;
        public bool isSelected;

        public PasteBox(float x, float y, float width, float height)
        {
            bounds = new RectangleF(x, y, width, height);
            borderBox = new RectangleF(x - BORDERWIDTH, y - BORDERWIDTH, width + (BORDERWIDTH * 2), height + (BORDERWIDTH * 2));
            color = Color.FromArgb(255, 255, 255);

            isSelected = false;
        }

        public bool hitTest(Point p)
        {
            return bounds.Contains(p);
        }

        public ResizeDirection borderHitTest(Point p)
        {
            ResizeDirection result = ResizeDirection.NONE;

            float rdelta = p.X - bounds.Right;
            float bdelta = p.Y - bounds.Bottom;
            if (0 <= rdelta && rdelta < BORDERWIDTH)
            {
                result = ResizeDirection.HORZ;
            }

            if (0 <= bdelta && bdelta < BORDERWIDTH)
            {
                result = (result == ResizeDirection.HORZ) ? ResizeDirection.BOTH : ResizeDirection.VERT;
            }

            return result;
        }

        //- positioning ---------------------------------------------------

        public void select(bool _isSelected)
        {
            isSelected = _isSelected;
        }

        public Point getPos()
        {
            PointF loc = bounds.Location;
            return Point.Round(loc);
        }

        public void setPos(Point pos)
        {
            bounds.X = pos.X;
            bounds.Y = pos.Y;
        }

        public int getHeight()
        {
            return (int)bounds.Height;
        }

        public void setHeight(int height)
        {
            height = (height > 0) ? height : 1;
            bounds.Height = height;
        }

        public int getWidth()
        {
            return (int)bounds.Width;
        }

        public void setWidth(int width)
        {
            width = (width > 0) ? width : 1;
            bounds.Width = width;
        }

        //- painting ----------------------------------------------------------

        public virtual void paint(Graphics g)
        {
            //draw border if selected
            if (isSelected)
            {
                Pen pen = new Pen(BORDERCOLOR);
                pen.DashStyle = BORDERSTYLE;
                pen.Width = BORDERWIDTH;
                g.DrawRectangle(pen, bounds.X, bounds.Y, bounds.Width, bounds.Height);
            }

            Brush brush = new SolidBrush(color);
            g.FillRectangle(brush, bounds);
        }

    }
}
