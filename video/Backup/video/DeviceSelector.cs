using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using DShowNET;
using DShowNET.Device;

namespace video
{

    public partial class DeviceSelector : Form
    {
        public DsDevice SelectedDevice;
        //public DeviceSelector()
        //{
        //    InitializeComponent();
        //}
        public DeviceSelector(ArrayList devs)
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            ListViewItem item = null;
            foreach (DsDevice d in devs)
            {
                item = new ListViewItem(d.Name);
                item.Tag = d;
                deviceListVw.Items.Add(item);
            }
        }
        private void deviceListVw_DoubleClick(object sender, System.EventArgs e)
        {
            this.okButton_Click(sender, e);
        }
        private void okButton_Click(object sender, System.EventArgs e)
        {
            if (deviceListVw.SelectedItems.Count != 1)
                return;
            ListViewItem selitem = deviceListVw.SelectedItems[0];
            SelectedDevice = selitem.Tag as DsDevice;
            Close();
        }
        private void cancelButton_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}