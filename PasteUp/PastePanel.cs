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
    class PastePanel : Panel
    {
        //compensate for the fact that if the scrollbar max = 50, the greatest value will be 50 - THUMBWIDTH (value determined experimentally)
        const int THUMBWIDTH = 9;

        public PasteCanvas canvas;

        HScrollBar hscroll;
        VScrollBar vscroll;

        int panelheight;
        int panelwidth;
        int canvasx;
        int canvasy;

        //for debugging purposes
        //TextBox text;

        public PastePanel() : base()
        {
            hscroll = new HScrollBar();
            hscroll.Scroll += Hscroll_Scroll;
            this.Controls.Add(hscroll);

            vscroll = new VScrollBar();
            vscroll.Scroll += Vscroll_Scroll;
            this.Controls.Add(vscroll);

            canvas = new PasteCanvas();
            canvas.Size = new Size(600, 400);
            this.Controls.Add(canvas);

            //text = new TextBox();
            //this.Controls.Add(text);

            calcPositions();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            calcPositions();
        }

        public void calcPositions()
        {
            //visible area excluding scrollbars
            panelheight = this.Height - hscroll.Height;
            panelwidth = this.Width - vscroll.Width;
            
            //set scrollbar pos
            hscroll.Location = new Point(0, panelheight);
            hscroll.Width = panelwidth;
            vscroll.Location = new Point(panelwidth, 0);
            vscroll.Height = panelheight;

            if (panelwidth >= canvas.Width)
            {
                canvasx = (panelwidth - canvas.Width) / 2;
                hscroll.Enabled = false;
                hscroll.Maximum = 0;
                hscroll.Value = 0;
            }
            else
            {
                canvasx = 0;
                hscroll.Enabled = true;
                hscroll.Maximum = canvas.Width - panelwidth + THUMBWIDTH;
                if (hscroll.Maximum - hscroll.Value < THUMBWIDTH)
                {
                    hscroll.Value = hscroll.Maximum - THUMBWIDTH;
                }
            }

            if (panelheight >= canvas.Height)
            {
                canvasy = (panelheight - canvas.Height) / 2;
                vscroll.Enabled = false;
                vscroll.Maximum = 0;
                vscroll.Value = 0;
            }
            else
            {
                canvasy = 0;
                vscroll.Enabled = true;
                vscroll.Maximum = canvas.Height - panelheight + THUMBWIDTH;
                if (vscroll.Maximum - vscroll.Value < THUMBWIDTH)
                {
                    vscroll.Value = vscroll.Maximum - THUMBWIDTH;
                }
            }

            canvas.Location = new Point(canvasx - hscroll.Value, canvasy - vscroll.Value);
            //text.Text = "max = " + hscroll.Maximum + " val = " + hscroll.Value;
        }

        private void Hscroll_Scroll(object sender, ScrollEventArgs e)
        {
            canvas.Location = new Point(canvasx - hscroll.Value, canvasy - vscroll.Value);
            //text.Text = "max = " + hscroll.Maximum + " val = " + hscroll.Value;
        }

        private void Vscroll_Scroll(object sender, ScrollEventArgs e)
        {
            canvas.Location = new Point(canvasx - hscroll.Value, canvasy - vscroll.Value);
        }
    }
}
