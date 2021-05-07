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
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PasteUp
{
    class PasteCanvas : Control
    {

        public LinkedList<PasteBox> boxes;
        public PasteBox selectedBox;

        public PasteCanvas()
        {
            this.BackColor = Color.Wheat;
            this.DoubleBuffered = true;

            boxes = new LinkedList<PasteBox>();
            selectedBox = null;
        }

        internal void InsertPasteText()
        {
            PasteText box = new PasteText();
            box.bounds = new RectangleF(2, 2, 50, 50);
            boxes.AddLast(box);
            Invalidate();
        }

        internal void InsertPasteImage()
        {
            PasteImage box = new PasteImage();
            box.bounds = new RectangleF(548, 348, 50, 50);
            boxes.AddLast(box);
            Invalidate();
        }

        //- mouse handling ------------------------------------------------------------

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            bool handled = false;
            foreach (PasteBox box in boxes)
            {
                if (box.bounds.Contains(e.Location))
                {
                    if (selectedBox != null)
                    {
                        selectedBox.isSelected = false;
                    }
                    selectedBox = box;
                    selectedBox.isSelected = true;
                    handled = true;
                    break;
                }
            }

            //we clicked on a blank area of the canvas - deselect current selection if there is one
            if (!handled)
            {
                if (selectedBox != null)
                    selectedBox.isSelected = false;
                selectedBox = null;
            }
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        //protected override void OnMouseClick(MouseEventArgs e)
        //{
        //}

        //protected override void OnMouseDoubleClick(MouseEventArgs e)
        //{
        //}

        //- keyboard handling ---------------------------------------------------------

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (selectedBox != null)
                {
                    boxes.Remove(selectedBox);
                    selectedBox = null;
                    Invalidate();
                }
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
        }

        //- painting ------------------------------------------------------------------


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;


            foreach (PasteBox box in boxes)
            {
                box.paint(g);
            }
        }


    }
}