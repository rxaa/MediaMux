using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace df
{
    public class ListViewExt
    {
        ListView listV;

        public ListViewExt(ListView lv)
        {
            listV = lv;
        }

        

        public void setDrag()
        {
            listV.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewStream_ItemDrag);
            listV.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewStream_DragDrop);
            listV.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewStream_DragEnter);
            listV.DragOver += new System.Windows.Forms.DragEventHandler(this.listViewStream_DragOver);
            listV.DragLeave += (s, e) =>
            {
                listV.Controls.Remove(dragLine);
            };
        }

        private void listViewStream_ItemDrag(object sender, ItemDragEventArgs e)
        {
            listV.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void listViewStream_DragOver(object sender, DragEventArgs e)
        {
            Point ptScreen = new Point(e.X, e.Y);
            Point pt = listV.PointToClient(ptScreen);
            ListViewItem item = listV.GetItemAt(pt.X, pt.Y);
            var h = item.Bounds.Height;
            if (pt.Y - item.Position.Y > h / 2)
            {
                dragLine.Location = new Point(item.Position.X, item.Position.Y + h);
            }
            else
            {
                dragLine.Location = new Point(item.Position.X, item.Position.Y);
            }
        }
        PictureBox dragLine = new PictureBox
        {
            Name = "pictureBox",
            Size = new Size(100, 2),
            Location = new Point(100, 100),
            //Image = Image.FromFile("hello.jpg"),
            BackColor = Color.Red,

        };
        private void listViewStream_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;

            if (e.AllowedEffect == DragDropEffects.Move)
                listV.Controls.Add(dragLine);
        }


        public Action onSortStart = null;
        public Action<int, int, int> onSortEnd = null;

        private void listViewStream_DragDrop(object sender, DragEventArgs e)
        {
            listV.Controls.Remove(dragLine);
            ListViewItem draggedItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
            Point ptScreen = new Point(e.X, e.Y);
            Point pt = listV.PointToClient(ptScreen);
            ListViewItem TargetItem = listV.GetItemAt(pt.X, pt.Y);

            var h = TargetItem.Bounds.Height;

            var oldI = draggedItem.Index;
            var index = TargetItem.Index;
            if (pt.Y - TargetItem.Position.Y > h / 2)
            {
                index += 1;
            }
            if (onSortStart != null)
                onSortStart();

            listV.Items.Insert(index, (ListViewItem)draggedItem.Clone());
            listV.Items.Remove(draggedItem);

            var realI = index;
            if (realI > oldI)
            {
                realI -= 1;
            }

            if (onSortEnd != null)
                onSortEnd(oldI, index, realI);
        }
    }
}
