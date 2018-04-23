using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Repository;

namespace WindowsApplication123
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            mru = new MRUItemManager(barManager1, barLinkContainerItem1);
        }

        MRUItemManager mru;

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult res = openFileDialog1.ShowDialog();
            if (res != DialogResult.OK) return;
            mru.Add(openFileDialog1.FileName);
        }
    }

    public class MRUItemManager
    {
        private BarManager manager;
        private List<BarEditItem> list;
        private int maxItems;
        private RepositoryItemCheckEdit riCheck;
        private BarLinkContainerItem container;

        public MRUItemManager(BarManager manager, BarLinkContainerItem container)
        {
            this.manager = manager;
            this.list = new List<BarEditItem>();
            maxItems = 5;
            riCheck = new RepositoryItemCheckEdit();
            riCheck.Caption = "";
            riCheck.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            manager.RepositoryItems.Add(riCheck);
            this.container = container;
        }

        public void Add(object obj)
        {
            if (obj == null) return;
            BarEditItem item = null;
            for (int i = 0; i < list.Count; i++)
            {
                if (Equals(list[i].Tag, obj))
                {
                    item = list[i];
                    break;
                }
            }
            if (item == null)
            {
                item = new BarEditItem();
                manager.Items.Add(item);
                item.Edit = riCheck;
                item.AutoFillWidth = true;
                item.Caption = obj.ToString();
                item.Tag = obj;
                item.EditValue = false;
                item.ItemClick += new ItemClickEventHandler(item_ItemClick);
                list.Add(item);
            }
            Touch(item);
        }

        void item_ItemClick(object sender, ItemClickEventArgs e)
        {
            e.Item.Manager.CloseMenus();
            Touch(e.Item as BarEditItem);
        }

        protected void Touch(BarEditItem item)
        {
            list.Remove(item);
            list.Insert(0, item);
            int i = list.Count - 1;
            while (list.Count > maxItems && i >= 0)
            {
                bool pin = Equals(list[i].EditValue, true);
                if (!pin)
                {
                    BarEditItem xitem = list[i];
                    list.RemoveAt(i);
                    xitem.Dispose();
                }
                i--;
            }
            UpdateList();
        }

        public void UpdateList()
        {
            container.ItemLinks.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                container.AddItem(list[i]);
            }
        }
    }
}