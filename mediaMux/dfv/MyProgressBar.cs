using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace df
{
    public class MyProgressBar : ProgressBar
    {
        const int xOffset = 2;
        bool press = false;
        public MyProgressBar()
        {
            this.Maximum = 1000;
            base.SetStyle(ControlStyles.UserPaint, true);
            this.MouseDown += (o, e) =>
            {
                press = true;
                setX(e.X + xOffset);

                onMove?.Invoke();
            };

            this.MouseUp += (o, e) =>
            {
                press = false;
                onUp?.Invoke();
            };
            this.MouseMove += (o, e) =>
            {
                if (press)
                {
                    setX(e.X + xOffset);
                    onMove?.Invoke();
                }
            };
            this.DoubleBuffered = true;
        }

        public Action onMove = null;
        public Action onUp = null;

        public void addControlMove(Control con)
        {
            con.MouseDown += (o, e) =>
            {

                int x = e.Location.X + xOffset + con.Location.X - this.Location.X;
                press = true;
                setX(x);
                onMove?.Invoke();
            };

            con.MouseUp += (o, e) =>
            {
                press = false;
                onUp?.Invoke();
            };
            con.MouseMove += (o, e) =>
            {
                int x = e.Location.X + xOffset + con.Location.X - this.Location.X;
                if (press)
                {
                    setX(x + 1);
                    onMove?.Invoke();
                }

            };
        }


        void setX(int x)
        {
            int prev = x * 1000 / this.Size.Width;
            if (prev > 1000)
                prev = 1000;
            if (prev < 0)
                prev = 0;
            this.Value = prev;
        }

        public void setValue(int prev)
        {
            if (prev > 1000)
                prev = 1000;
            if (prev < 0)
                prev = 0;

            this.Value = prev;
        }

        //重写OnPaint方法
        protected override void OnPaint(PaintEventArgs e)
        {
            SolidBrush brush = new SolidBrush(this.ForeColor);
            Rectangle bounds = new Rectangle(0, 0, base.Width, base.Height);
            //...
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), 0, 0, bounds.Width, bounds.Height);
            bounds.Height -= 4;
            bounds.Width = ((int)(bounds.Width * (((double)base.Value) / ((double)base.Maximum)))) - 4;
            e.Graphics.FillRectangle(brush, 2, 2, bounds.Width, bounds.Height);

        }
    }
}
