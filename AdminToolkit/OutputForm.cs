using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Admin_Tools
{
    public partial class OutputForm : Form
    {
        // Print state
        private string[] _printLines;
        private int _printLineIndex;

        // Live log-tail state
        private System.Windows.Forms.Timer _tailTimer;
        private string _tailPath;
        private long _tailPos;

        public OutputForm()
        {
            InitializeComponent();
        }

        /// <summary>Replace the window content and title (static, one-shot).</summary>
        public void ShowOutput(string title, string content)
        {
            StopLive();                 // leaving live mode if we were in it
            Text = title;
            txtOutput.Text = content;
            txtOutput.SelectionStart = 0;
            txtOutput.SelectionLength = 0;
        }

        /// <summary>
        /// Live mode: show the given file and keep appending new lines as they
        /// are written. Safe to call again to re-point at a different file.
        /// </summary>
        public void ShowLiveLog(string title, string path)
        {
            Text = title;
            _tailPath = path;
            _tailPos = 0;
            txtOutput.Clear();

            ReadNewText();              // initial fill

            if (_tailTimer == null)
            {
                _tailTimer = new System.Windows.Forms.Timer();
                _tailTimer.Interval = 1000;                 // poll once a second
                _tailTimer.Tick += (s, e) => ReadNewText();
            }
            _tailTimer.Start();
        }

        /// <summary>Read whatever has been appended since the last read.</summary>
        private void ReadNewText()
        {
            if (string.IsNullOrEmpty(_tailPath)) return;
            try
            {
                if (!File.Exists(_tailPath)) return;

                // ReadWrite share so we never block the Logger from writing.
                using (var fs = new FileStream(_tailPath, FileMode.Open,
                           FileAccess.Read, FileShare.ReadWrite))
                {
                    if (fs.Length < _tailPos) _tailPos = 0;   // file rotated/shrank
                    fs.Seek(_tailPos, SeekOrigin.Begin);

                    using (var sr = new StreamReader(fs, Encoding.UTF8))
                    {
                        string chunk = sr.ReadToEnd();
                        _tailPos = fs.Length;
                        if (!string.IsNullOrEmpty(chunk))
                            txtOutput.AppendText(chunk);      // AppendText auto-scrolls
                    }
                }
            }
            catch
            {
                // File briefly locked mid-write - just try again next tick.
            }
        }

        private void StopLive()
        {
            if (_tailTimer != null) _tailTimer.Stop();
            _tailPath = null;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_tailTimer != null)
            {
                _tailTimer.Stop();
                _tailTimer.Dispose();
                _tailTimer = null;
            }
            base.OnFormClosed(e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            using (var printDoc = new PrintDocument())
            using (var dialog = new PrintDialog())
            {
                printDoc.DocumentName = Text;
                printDoc.PrintPage += PrintDoc_PrintPage;
                dialog.Document = printDoc;
                dialog.UseEXDialog = true;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _printLines = txtOutput.Text.Replace("\r\n", "\n").Split('\n');
                    _printLineIndex = 0;
                    try
                    {
                        printDoc.Print();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "Print failed:\n\n" + ex.Message,
                            "Print", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            using (var font = new Font("Consolas", 9f))
            {
                float lineHeight = font.GetHeight(e.Graphics);
                float y = e.MarginBounds.Top;

                while (_printLineIndex < _printLines.Length &&
                       y + lineHeight <= e.MarginBounds.Bottom)
                {
                    e.Graphics.DrawString(_printLines[_printLineIndex], font,
                        Brushes.Black, e.MarginBounds.Left, y);
                    y += lineHeight;
                    _printLineIndex++;
                }

                e.HasMorePages = _printLineIndex < _printLines.Length;
            }
        }

        private void Purge_Button_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(_tailPath) || !File.Exists(_tailPath))
                return;

            var answer = MessageBox.Show(this,
                "Clear the entire log file?\n\nThis cannot be undone.",
                "Purge Log",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);   // default = No

            if (answer != DialogResult.Yes) return;

            try
            {
                // Truncate to zero bytes. ReadWrite share matches how the
                // tail reads, so the Logger can keep its handle open.
                using (var fs = new FileStream(_tailPath, FileMode.Open,
                           FileAccess.Write, FileShare.ReadWrite))
                {
                    fs.SetLength(0);
                }

                txtOutput.Clear();
                _tailPos = 0;       // tail restarts from the top of the (empty) file
            }
            catch (IOException ex)
            {
                MessageBox.Show(this, "Could not purge the log file:\n\n" + ex.Message,
                    "Purge", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
  
    }
}
