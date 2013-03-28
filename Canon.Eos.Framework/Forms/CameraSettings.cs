using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Canon.Eos.Framework.Helper;

namespace Canon.Eos.Framework.Forms
{
    public partial class CameraSettings : Form
    {
        EosCamera _camera;

        public CameraSettings(EosCamera cam)
        {
            InitializeComponent();
            _camera = cam;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            lbl_model.Text = _camera.ProductName;
            lbl_mode.Text = _camera.AEMode.ToString();
          
            foreach (var item in _camera.ISOList)
            {
                combo_ISO.Items.Add(item);
                
                if ( EnumHelper.GetEnumValue((string)item, typeof(EosISO)).ToString() == Properties.Settings.Default.ISO)
                    combo_ISO.SelectedIndex = combo_ISO.Items.Count -1;
            }
            foreach (var item in _camera.AvList)
            {
                combo_AV.Items.Add(item);
                if (EnumHelper.GetEnumValue((string)item, typeof(EosAv)).ToString() == Properties.Settings.Default.AV)
                    combo_AV.SelectedIndex = combo_AV.Items.Count - 1;
            }
            foreach (var item in _camera.TvList)
            {
                combo_TV.Items.Add(item);
                if (EnumHelper.GetEnumValue((string)item, typeof(EosTv)).ToString() == Properties.Settings.Default.TV)
                    combo_TV.SelectedIndex = combo_TV.Items.Count - 1;
            }
           
        }

        private void Settings_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_camera.AEMode != EosAEMode.Manual)
            {
                MessageBox.Show("Settings can only be saved in Manual Mode", "Error Saving Settings", MessageBoxButtons.OK);
                return;
            }
           Properties.Settings.Default.ISO = EnumHelper.GetEnumValue((string)combo_ISO.SelectedItem, typeof(EosISO)).ToString();
           Properties.Settings.Default.AV = EnumHelper.GetEnumValue((string)combo_AV.SelectedItem, typeof(EosAv)).ToString();
           Properties.Settings.Default.TV = EnumHelper.GetEnumValue((string)combo_TV.SelectedItem, typeof(EosTv)).ToString();

            Properties.Settings.Default.Save();

             _camera.Tv =   (EosTv) Enum.Parse(typeof(EosTv), Properties.Settings.Default.TV);
             _camera.Av =    (EosAv)Enum.Parse(typeof(EosAv), Properties.Settings.Default.AV);
             _camera.ISO =    (EosISO)Enum.Parse(typeof(EosISO), Properties.Settings.Default.ISO);


        }
    }
}
