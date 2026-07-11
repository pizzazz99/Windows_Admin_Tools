using System;
using System.Drawing;
using System.Windows.Forms;

namespace Admin_Tools
{
    /// <summary>
    /// Minimal modal password prompt, built entirely in code (no Designer file).
    /// Input is masked; the value is only ever returned to the caller, never
    /// displayed or logged.
    /// </summary>
    internal static class PasswordBox
    {
        /// <summary>
        /// Show the prompt. Returns the entered text, or null if the user
        /// cancelled or left it blank.
        /// </summary>
        public static string Show(IWin32Window owner, string title, string prompt)
        {
            using (var form = new Form())
            {
                form.Text = title;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.MinimizeBox = false;
                form.MaximizeBox = false;
                form.ShowInTaskbar = false;
                form.ClientSize = new Size(400, 140);

                var lbl = new Label
                {
                    Text = prompt,
                    AutoSize = false,
                    Bounds = new Rectangle(12, 12, 376, 50)
                };

                var txt = new TextBox
                {
                    Bounds = new Rectangle(12, 68, 376, 25),
                    UseSystemPasswordChar = true
                };

                var ok = new Button
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK,
                    Bounds = new Rectangle(224, 100, 80, 28)
                };

                var cancel = new Button
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                    Bounds = new Rectangle(308, 100, 80, 28)
                };

                form.Controls.Add(lbl);
                form.Controls.Add(txt);
                form.Controls.Add(ok);
                form.Controls.Add(cancel);
                form.AcceptButton = ok;      // Enter submits
                form.CancelButton = cancel;  // Esc cancels

                if (form.ShowDialog(owner) == DialogResult.OK)
                {
                    string value = txt.Text;
                    return string.IsNullOrEmpty(value) ? null : value;
                }
                return null;
            }
        }
    }
}
