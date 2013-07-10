using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        public static Bitmap sheep = new Bitmap("../../sheep.png");
        public Point[] locs, vels;
        Random r;
        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            last = DateTime.Now;
        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        DateTime last;
        public void Idle(object sender, EventArgs evt)
        {
            TimeSpan len = DateTime.Now - last;
            var dt = len.TotalMilliseconds;
            while (AppStillIdle)
            {
                if (locs != null)
                {
                    for (int i = 0; i < 25; ++i)
                    {
                        locs[i].X += (int)(vels[i].X * dt / 1000);
                        locs[i].Y += (int)(vels[i].Y * dt / 1000);
                        if (locs[i].Y >= this.Height - sheep.Height && vels[i].Y > 0)
                        {
                            vels[i].Y *= -1;
                            locs[i].Y = this.Height - sheep.Height - 1;
                        }
                        vels[i].Y++;
                        if (locs[i].X <= 0 || locs[i].X > this.Width - sheep.Width)
                        {
                            vels[i].X *= -1;
                        }
                    }
                    this.Invalidate();
                }
            }
            last = DateTime.Now;
        }

        public bool AppStillIdle
        {
            get
            {
                PeekMsg msg;
                return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct PeekMsg
        {
            public IntPtr hWnd;
            public Message msg;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public System.Drawing.Point p;
        }
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern bool PeekMessage(out PeekMsg msg, IntPtr hWnd,
                uint messageFilterMin, uint messageFilterMax, uint flags);
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            r = new Random();
            locs = new Point[25];
            vels = new Point[25];
            for (int i = 0; i < 25; ++i)
            {
                locs[i] = new Point(r.Next(0, this.Width - 10), r.Next(0, this.Height / 2));
                vels[i] = new Point(r.Next(-100, 100), 0);
            }
        }
        public new int Width
        {
            get
            {
                return base.Width - 1;
            }
            set
            {
                base.Width = value + 1;
            }
        }
        public new int Height
        {
            get
            {
                return base.Height - 1;
            }
            set
            {
                base.Height = value + 1;
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (locs != null)
                foreach (Point loc in locs)
                    e.Graphics.DrawImage(sheep, loc);
        }
    }
}