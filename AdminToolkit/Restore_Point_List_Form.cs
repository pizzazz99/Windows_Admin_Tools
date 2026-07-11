// ============================================================
//  Restore_Point_List_Form.cs   (C# 7.3 / .NET Framework)
//  Designer-based version. Pairs with
//  Restore_Point_List_Form.Designer.cs — all controls live
//  there; this file is logic only.
//
//  Usage from MainForm:
//      using (var f = new Restore_Point_List_Form())
//          f.ShowDialog(this);
// ============================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Management;
using System.Text;
using System.Windows.Forms;

namespace Admin_Tools
{
    public partial class Restore_Point_List_Form : Form
    {
        private List<Restore_Point_Info> _points = new List<Restore_Point_Info>();

        public Restore_Point_List_Form()
        {
            InitializeComponent();
            lvPoints.MultiSelect = true;
            Load_Points();
        }

        // --------------------------------------------------------
        //  Data load — sorted OLDEST first
        // --------------------------------------------------------
        private void Load_Points()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                _points = Restore_Point_Manager.Get_Restore_Points();

                // Manager returns newest-first; flip to oldest-first
                _points.Reverse();

                lvPoints.BeginUpdate();
                lvPoints.Items.Clear();

                foreach (var rp in _points)
                {
                    double ageDays = (DateTime.Now - rp.Creation_Time).TotalDays;

                    var item = new ListViewItem(rp.Sequence_Number.ToString());
                    item.SubItems.Add(rp.Creation_Time.ToString("yyyy-MM-dd HH:mm:ss"));
                    item.SubItems.Add(ageDays.ToString("0.0"));
                    item.SubItems.Add(rp.Type_Name);
                    item.SubItems.Add(rp.Event_Name);
                    item.SubItems.Add(rp.Description);
                    item.SubItems.Add(rp.Linked_Shadow_Id != null ? "Linked" : "—");
                    item.Tag = rp;

                    // Visual grouping hints
                    if (rp.Restore_Point_Type == 10)          // driver install
                        item.ForeColor = Color.DarkBlue;
                    else if (rp.Restore_Point_Type == 0 ||
                             rp.Restore_Point_Type == 1)      // app install/uninstall
                        item.ForeColor = Color.DarkGreen;
                    else if (rp.Restore_Point_Type == 13)     // cancelled
                        item.ForeColor = Color.Gray;

                    lvPoints.Items.Add(item);
                }

                lvPoints.EndUpdate();

                lblSummary.Text = _points.Count == 0
                    ? "No restore points found (System Protection may be off for the system drive)."
                    : _points.Count + " restore point(s)   |   oldest: "
                        + _points[0].Creation_Time.ToString("yyyy-MM-dd HH:mm")
                        + "   |   newest: "
                        + _points[_points.Count - 1].Creation_Time.ToString("yyyy-MM-dd HH:mm");

                Clear_Details();

                // Auto-select newest (last row) so details aren't empty
                if (lvPoints.Items.Count > 0)
                {
                    var last = lvPoints.Items[lvPoints.Items.Count - 1];
                    last.Selected = true;
                    last.EnsureVisible();
                }
            }
            catch (Exception ex)
            {
                lblSummary.Text = "Query failed.";
                Clear_Details();
                txtNotes.Text = "WMI query failed: " + ex.Message +
                                "  Make sure the app is running as Administrator.";
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void Clear_Details()
        {
            txtSeq.Text = "";
            txtCreated.Text = "";
            txtAge.Text = "";
            txtDescription.Text = "";
            txtType.Text = "";
            txtEvent.Text = "";
            txtShadowId.Text = "";
            txtDevice.Text = "";
            txtNotes.Text = "";
        }

        // --------------------------------------------------------
        //  Selection -> details fields
        // --------------------------------------------------------
        private void Lv_Selection_Changed(object sender, EventArgs e)
        {
            if (lvPoints.SelectedItems.Count == 0) return;

            var rp = lvPoints.SelectedItems[0].Tag as Restore_Point_Info;
            if (rp == null) return;

            var age = DateTime.Now - rp.Creation_Time;

            txtSeq.Text = rp.Sequence_Number.ToString();
            txtCreated.Text = rp.Creation_Time.ToString("yyyy-MM-dd HH:mm:ss");
            txtAge.Text = age.TotalDays.ToString("0.0");
            txtDescription.Text = rp.Description;
            txtType.Text = rp.Type_Name + "  (" + rp.Restore_Point_Type + ")";
            txtEvent.Text = rp.Event_Name + "  (" + rp.Event_Type + ")";

            if (rp.Linked_Shadow_Id != null)
            {
                txtShadowId.Text = rp.Linked_Shadow_Id;
                txtDevice.Text = rp.Linked_Device;
            }
            else
            {
                txtShadowId.Text = "None found";
                txtDevice.Text = "Shadow copy may have been aged out (deleted) " +
                                   "while the restore point metadata remains.";
            }

            txtNotes.Text = Type_Hint(rp.Restore_Point_Type);
        }

        private static string Type_Hint(uint t)
        {
            switch (t)
            {
                case 0:
                    return "Created by an application installer before making changes. " +
                                "The description is whatever name the installer passed in.";
                case 1: return "Created by an uninstaller before removing an application.";
                case 6:
                    return "Created automatically before a System Restore operation ran, " +
                                "so the restore itself can be undone.";
                case 7:
                    return "A checkpoint - either the scheduled automatic one or one " +
                                "created manually via System Protection.";
                case 10: return "Created by Windows before installing a device driver.";
                case 12: return "Created before a system settings change.";
                case 13: return "The operation that created this point was cancelled before completing.";
                case 14: return "Created by a backup/recovery operation.";
                default: return "No additional notes for this type.";
            }
        }

        // --------------------------------------------------------
        //  Buttons
        // --------------------------------------------------------
        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            Load_Points();
        }

        private void Btn_Copy_Click(object sender, EventArgs e)
        {
            if (txtSeq.Text.Length == 0) return;

            var sb = new StringBuilder();
            sb.AppendLine("RESTORE POINT DETAILS");
            sb.AppendLine(new string('=', 55));
            sb.AppendLine("Sequence #    : " + txtSeq.Text);
            sb.AppendLine("Created       : " + txtCreated.Text);
            sb.AppendLine("Age           : " + txtAge.Text + " days");
            sb.AppendLine("Description   : " + txtDescription.Text);
            sb.AppendLine("Type          : " + txtType.Text);
            sb.AppendLine("Event         : " + txtEvent.Text);
            sb.AppendLine("Shadow ID     : " + txtShadowId.Text);
            sb.AppendLine("Device        : " + txtDevice.Text);
            sb.AppendLine("Notes         : " + txtNotes.Text);

            Clipboard.SetText(sb.ToString());
            lblSummary.Text = "Details copied to clipboard.";
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Delete_Button_Click(object sender, EventArgs e)
        {
            if (lvPoints.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select one or more restore points first "
                    + "(Ctrl+click / Shift+click for multiple).",
                    "Delete Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var seqs = new List<uint>();
            var names = new StringBuilder();

            foreach (ListViewItem item in lvPoints.SelectedItems)
            {
                var rp = item.Tag as Restore_Point_Info;
                if (rp == null) continue;
                seqs.Add(rp.Sequence_Number);
                names.AppendLine("  #" + rp.Sequence_Number + "  " + rp.Description);
            }

            var answer = MessageBox.Show(
                "Permanently delete " + seqs.Count + " restore point(s)?\n\n" + names +
                "\nThis also deletes each point's underlying shadow copy.\n" +
                "There is NO undo — you cannot restore the system to these\n" +
                "points afterward.",
                "Delete Restore Points", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (answer != DialogResult.Yes) return;

            Cursor = Cursors.WaitCursor;
            int deleted, failed;
            List<string> errors;
            Restore_Point_Delete.Delete_Many(seqs, out deleted, out failed, out errors);
            Cursor = Cursors.Default;

            string msg = deleted + " deleted, " + failed + " failed.";
            if (errors.Count > 0) msg += "\n\n" + string.Join("\n", errors.ToArray());

            MessageBox.Show(msg, "Delete Restore Points", MessageBoxButtons.OK,
                failed == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            Load_Points();
        }
    }
}
