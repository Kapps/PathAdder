using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PathAdder {
    public partial class Main : Form {

        public Main() {
            InitializeComponent();
        }

		private void Main_Load(object sender, EventArgs e) {
			try {
				if(Clipboard.ContainsText()) {
					var ClipPath = Clipboard.GetText();
					EnvironmentUtils.EnsureValidEnvironmentPath(ClipPath);
					txtPath.Text = Clipboard.GetText();
				}
			} catch {
				// Ignore any issues setting clipboard path, it's just for convenience and should not prevent program being used.
			}
		}

		private void txtPath_KeyDown(object sender, KeyEventArgs e) {
			if(e.KeyCode == Keys.Enter) {
				AppendPath();
				e.Handled = true;
			}
		}

		private void cmdAppend_Click(object sender, EventArgs e) {
			AppendPath();
		}

		private void AppendPath() {
			try {
				EnvironmentUtils.AppendPath(txtPath.Text);
				txtPath.Text = "";
			} catch(Exception ex) {
				MessageBox.Show("Error appending path: " + ex.Message);
			}
		}

		private void txtPath_Click(object sender, EventArgs e) {
			using(var FBD = new FolderBrowserDialog()) {
				if(!String.IsNullOrWhiteSpace(txtPath.Text)) {
					try {
						FBD.SelectedPath = txtPath.Text;
					} catch {
						// Invalid path, reset settings.
						FBD.Reset();
					}
				}
				if(FBD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					txtPath.Text = FBD.SelectedPath;
			}
		}

		private void Main_KeyDown(object sender, KeyEventArgs e) {
			if(e.KeyCode == Keys.Escape)
				Close();
		}
    }
}
