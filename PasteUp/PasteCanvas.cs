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

        bool dragging;
        Point dragOrg;
        Point dragOfs;

        bool resizing;
        ResizeDirection resizeDir;
        Point resizeOfs;
        int resizeHeight;
        int resizeWidth;
        Cursor prevCursor;

        public PasteCanvas()
        {
            this.BackColor = Color.Wheat;
            this.DoubleBuffered = true;

            boxes = new LinkedList<PasteBox>();
            selectedBox = null;

            dragging = false;
            resizing = false;
        }

        //- management ---------------------------------------------------------

        public void InsertPasteText()
        {
            PasteText box = new PasteText(2, 2, 50, 50);
            boxes.AddLast(box);
            Invalidate();
        }

        public void InsertPasteImage()
        {
            PasteImage box = new PasteImage(548, 348, 50, 50);
            boxes.AddLast(box);
            Invalidate();
        }

        public void selectPasteBox(PasteBox box)
        {
            if (selectedBox != null)
            {
                selectedBox.select(false);
            }
            selectedBox = box;
            selectedBox.select(true);
        }

        public void deletePasteBox()
        {
            if (selectedBox != null)
            {
                boxes.Remove(selectedBox);
                selectedBox = null;
                Invalidate();
            }
        }

        //- mouse handling ------------------------------------------------------------

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            bool handled = false;
            foreach (PasteBox box in boxes)
            {
                if (box.hitTest(e.Location))
                {
                    selectPasteBox(box);
                    startDrag(e.Location);
                    handled = true;
                    break;
                }

                ResizeDirection resizeDir = box.borderHitTest(e.Location);
                if (resizeDir != ResizeDirection.NONE)
                {
                    selectPasteBox(box);
                    startResize(e.Location, resizeDir);
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
                handled = true;
            }

            if (handled)
            {
                Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (dragging)
            {
                drag(e.Location);
            }

            if (resizing)
            {
                resize(e.Location);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (dragging)
            {
                endDrag(e.Location);
            }

            if (resizing)
            {
                endResize(e.Location);
            }
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
                deletePasteBox();
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
        }

        //- dragging ------------------------------------------------------------------

        //track diff between pos when mouse button was pressed and where it is now, and move box by the same offset
        private void startDrag(Point p)
        {
            dragging = true;
            prevCursor = this.Cursor;
            this.Cursor = Cursors.SizeAll;
            dragOrg = selectedBox.getPos();
            dragOfs = p;
        }

        private void drag(Point p)
        {
            int newX = p.X - dragOfs.X;
            int newY = p.Y - dragOfs.Y;
            selectedBox.setPos(new Point(dragOrg.X + newX, dragOrg.Y + newY));
            Invalidate();
        }

        //we've finished a drag, let the model know what has moved and where it is now
        private void endDrag(Point p)
        {
            dragging = false;
            this.Cursor = prevCursor;
        }

        //- resizing ------------------------------------------------------------------

        public void startResize(Point p, ResizeDirection _resizedir)
        {
            resizing = true;
            prevCursor = this.Cursor;
            resizeDir = _resizedir;
            switch (resizeDir)
            {
                case ResizeDirection.HORZ: this.Cursor = Cursors.SizeWE; break;
                case ResizeDirection.VERT: this.Cursor = Cursors.SizeNS; break;
                case ResizeDirection.BOTH: this.Cursor = Cursors.SizeNWSE; break;
            }
            resizeHeight = selectedBox.getHeight();
            resizeWidth = selectedBox.getWidth();
            resizeOfs = p;
        }

        public void resize(Point p)
        {
            int newX = p.X - resizeOfs.X;
            int newY = p.Y - resizeOfs.Y;
            if ((resizeDir == ResizeDirection.HORZ) || (resizeDir == ResizeDirection.BOTH))
            {
                selectedBox.setWidth(newX + resizeWidth);
            }
            if ((resizeDir == ResizeDirection.VERT) || (resizeDir == ResizeDirection.BOTH))
            {
                selectedBox.setHeight(newY + resizeHeight);
            }
            Invalidate();
        }

        public void endResize(Point p)
        {
            resizing = false;
            this.Cursor = prevCursor;
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