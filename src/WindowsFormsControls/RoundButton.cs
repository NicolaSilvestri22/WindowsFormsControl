﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WindowsFormsControls
{
    /// <summary>
    /// represents a windows button with rounded borders
    /// </summary>
    public partial class RoundButton : Button
    {
        private int _borderSize = 0;
        private int _borderRadius = 40;
        private Color _borderColor = Color.PaleVioletRed;

        /// <summary>
        /// creates a new instance of the <see cref="RoundButton"/>
        /// class
        /// </summary>
        public RoundButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Size = new Size(150, 40);
            BackColor = Color.RoyalBlue;
            ForeColor = Color.White;
        }

        /// <summary>
        /// gets or sets the size of the border
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Button appearance")]
        public int BorderSize
        {
            get => _borderSize;
            set
            {
                _borderSize = value;
                Invalidate();
            }
        }

        /// <summary>
        /// gets or sets the radius of the border
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Button appearance")]
        public int BorderRadius
        {
            get => _borderRadius;
            set
            {
                _borderRadius = value;
                Invalidate();
            }
        }

        /// <summary>
        /// gets or sets the color of the border
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Button appearance")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// redraws the control
        /// </summary>
        /// <param name="pe">the information of the event</param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            DrawControl(pe);
        }

        /// <summary>
        /// draws the control. this method is called by <see cref="OnPaint(PaintEventArgs)"/>
        /// method
        /// </summary>
        /// <param name="pe"></param>
        private void DrawControl(PaintEventArgs pe)
        {
            int width = Width;
            int height = Height;
            Graphics graphics = pe.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            DrawBorder(graphics, width, height);
            DrawSurface(graphics, width, height);
        }

        /// <summary>
        /// draws the border of the button
        /// </summary>
        /// <param name="graphics">the graphics of the control</param>
        private void DrawBorder(Graphics graphics, int width, int height)
        {
            RectangleF rectBorder = new(1, 1, width - 0.8F, height - 1);
            GraphicsPath pathBorder = GetFigurePath(rectBorder, _borderRadius - 1F);
            Pen penBorder = null;

            if (_borderRadius > 2)
            {
                penBorder = new Pen(_borderColor, 2);
                penBorder.Alignment = PenAlignment.Inset;

                if (_borderSize >= 1)
                {
                    graphics.DrawPath(penBorder, pathBorder);
                }
            }
            else
            {
                if (_borderSize >= 1)
                {
                    penBorder = new Pen(_borderColor, _borderSize);
                    penBorder.Alignment = PenAlignment.Inset;
                    graphics.DrawRectangle(penBorder, 0, 0, width - 1, height - 1);
                }
            }

            penBorder.Dispose();
            pathBorder.Dispose();
        }

        /// <summary>
        /// draws the surface of the button
        /// </summary>
        /// <param name="graphics">the graphics of the control</param>
        private void DrawSurface(Graphics graphics, int width, int height)
        {
            Control parent = Parent;
            RectangleF rectSurface = new(0, 0, width, height);
            GraphicsPath pathSurface = GetFigurePath(rectSurface, _borderRadius);
            Pen penSurface = new(parent.BackColor, 2);

            if (_borderRadius > 2)
            {
                Region = new Region(pathSurface);
                graphics.DrawPath(penSurface, pathSurface);
            }
            else
            {
                Region = new Region(rectSurface);
            }

            penSurface.Dispose();
            pathSurface.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Control parent = Parent;
            parent.BackColorChanged += new EventHandler(Container_BackColorChanged);
        }

        /// <summary>
        /// if the control is in design mode, this method redraws it
        /// </summary>
        /// <param name="sender">the object that called the event</param>
        /// <param name="e">the event informations</param>
        private void Container_BackColorChanged(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                Invalidate();
            }
        }

        /// <summary>
        /// gets the path of the control
        /// </summary>
        /// <param name="rect">the rectangle of the control</param>
        /// <param name="radius">the radius of the control</param>
        /// <returns>the path of the control</returns>
        private GraphicsPath GetFigurePath(RectangleF rect, float radius)
        {
            GraphicsPath path = new();
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}