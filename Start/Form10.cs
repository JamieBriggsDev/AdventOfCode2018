using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Start
{
    public partial class Form10 : Form
    {
        public Form10()
        {
            InitializeComponent();
        }


        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            // Following codes draw a line
            g.PageUnit = GraphicsUnit.Millimeter;
            Pen blackPen = new Pen(Color.Black, 1 / g.DpiX);
            //g.DrawLine(blackPen, 0, 0, 1, 1);

            const string drawString = "█";
            Font drawFont = new Font("Arial", 2);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            foreach(var pos in Day10.Positions)
            {

                g.DrawString(drawString, drawFont, drawBrush, pos.pX, pos.pY);
                //g.DrawRectangle(blackPen, 
                //    new Rectangle(
                //        new Point(pos.pX, pos.pY), 
                //        new Size(1, 1)));
            }

            
        }

    }
}
