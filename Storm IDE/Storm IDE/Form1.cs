using FastColoredTextBoxNS;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Storm_IDE
{
    public partial class Form1 : Form
    {
        public Form1(string[] args)
        {
            InitializeComponent();

            string str = Application.StartupPath.ToString() + "\\Storm Editor.exe";
            RegistryKey softKey = Registry.ClassesRoot.CreateSubKey(@"Storm Editor");
            RegistryKey subKey1 = Registry.ClassesRoot.CreateSubKey("DesktopBackground\\Shell\\Открыть с помощью Storm Editor");
            RegistryKey subKey2 = Registry.ClassesRoot.CreateSubKey("DesktopBackground\\Shell\\Открыть с помощью Storm Editor\\command");
            subKey1.SetValue("icon", str);
            subKey1.SetValue("MUIVerb", "Открыть с помощью Storm Editor");
            subKey2.SetValue("", str);
            RegistryKey subKey3 = Registry.ClassesRoot.CreateSubKey("*\\shell\\Открыть с помощью Storm Editor");
            RegistryKey subKey4 = Registry.ClassesRoot.CreateSubKey("*\\shell\\Открыть с помощью Storm Editor\\command");
            subKey3.SetValue("icon", str);
            subKey3.SetValue("MUIVerb", "Открыть с помощью Storm Editor");
            subKey4.SetValue("", (str + " %1"));

            try
            {
                OpenFile(args[0]);
            }
            catch { }
        }
        private string FileName = "";
        private int NumberTab = 2;
        /// <summary>
        /// FastColoredTextBox list
        /// </summary>
        private List<FastColoredTextBox> fastColoredTextBoxes = new List<FastColoredTextBox>();
        /// <summary>
        /// List of file paths
        /// </summary>
        private List<string> PathsFiles = new List<string>();
        /// <summary>
        /// List of file names
        /// </summary>
        private List<string> NamesFiles = new List<string>();
        private void Form1_Load(object sender, EventArgs e)
        {
            ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();
            ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem("Вставить");
            ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem("Копировать");
            ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem("Вырезать");
            ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem("Выделить все");
            contextMenuStrip1.Items.AddRange(new ToolStripMenuItem[4]
            {
                toolStripMenuItem1,
                toolStripMenuItem2,
                toolStripMenuItem3,
                toolStripMenuItem4
            });
            toolStripMenuItem1.Click += new EventHandler(вставитьToolStripMenuItem_Click);
            toolStripMenuItem2.Click += new EventHandler(копироватьToolStripMenuItem_Click);
            toolStripMenuItem3.Click += new EventHandler(вырезатьToolStripMenuItem_Click);
            toolStripMenuItem4.Click += new EventHandler(выделитьВсToolStripMenuItem_Click);
            fastColoredTextBox1.ContextMenuStrip = contextMenuStrip1;
            contextMenuStrip1.Items.AddRange((ToolStripItem[])new ToolStripMenuItem[4]
            {
                  toolStripMenuItem1,
                  toolStripMenuItem2,
                  toolStripMenuItem3,
                  toolStripMenuItem4
            });
            fastColoredTextBox1.KeyDown += Ft_KeyDown;
            fastColoredTextBox1.DragLeave += Ft_DragLeave;
            fastColoredTextBox1.DragEnter += Ft_DragEnter;
            fastColoredTextBox1.DragDrop += Ft_DragDrop;
        }

        private void файлToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog()
            {
                DefaultExt = ".cs", //(*.txt)|*.txt|Все файлы (*.*)|*.*"
                Filter = "C# Files|*.cs|HTML Files|*.html|JS Files|*.js|PHP Files|*.php|XML Files|*.xml|JSON Files|*.json|All Files|*.*"
            };

            if(op.ShowDialog() == DialogResult.OK)
            {
                FileName = op.FileName;
                OpenFile(op.FileName);
            }
        }
        public void OpenFile(string fileName)
        {
            autocompleteMenu1.SetAutocompleteMenu(fastColoredTextBox1, autocompleteMenu1);
            TabPage page = new TabPage();
            page.Text = Path.GetFileName(fileName);
            FastColoredTextBox ft = new FastColoredTextBox();
            switch (Path.GetExtension(fileName))
            {
                case ".cs":
                    ft.Language = Language.CSharp;
                    autocompleteMenu1.SetAutocompleteMenu(ft, autocompleteMenu1);
                    break;
                case ".html":
                    ft.Language = Language.HTML;
                    break;
                case ".php":
                    ft.Language = Language.PHP;
                    break;
                case ".js":
                    ft.Language = Language.JS;
                    break;
                case ".xml":
                    ft.Language = Language.XML;
                    break;
                case ".json":
                    ft.Language = Language.JSON;
                    break;
            }
            ft.Dock = DockStyle.Fill;
            page.Controls.Add(ft);
            ft.AutoCompleteBrackets = true;
            tabControl1.TabPages.Add(page);
            ft.Text = File.ReadAllText(fileName);
            fastColoredTextBoxes.Add(ft);
            PathsFiles.Add(fileName);
            documentMap1.Target = ft;
            NamesFiles.Add(Path.GetFileName(fileName));
            tabControl1.SelectedTab = page;
            #region
            ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();
            ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem("Вставить");
            ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem("Копировать");
            ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem("Вырезать");
            ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem("Выделить все");
            contextMenuStrip1.Items.AddRange(new ToolStripMenuItem[4]
            {
                toolStripMenuItem1,
                toolStripMenuItem2,
                toolStripMenuItem3,
                toolStripMenuItem4
            });
            toolStripMenuItem1.Click += new EventHandler(вставитьToolStripMenuItem_Click);
            toolStripMenuItem2.Click += new EventHandler(копироватьToolStripMenuItem_Click);
            toolStripMenuItem3.Click += new EventHandler(вырезатьToolStripMenuItem_Click);
            toolStripMenuItem4.Click += new EventHandler(выделитьВсToolStripMenuItem_Click);
            ft.ContextMenuStrip = contextMenuStrip1;
            contextMenuStrip1.Items.AddRange((ToolStripItem[])new ToolStripMenuItem[4]
            {
                  toolStripMenuItem1,
                  toolStripMenuItem2,
                  toolStripMenuItem3,
                  toolStripMenuItem4
            });
            ft.KeyDown += Ft_KeyDown;
            #endregion
            ft.DragEnter += Ft_DragEnter;
            ft.DragDrop += Ft_DragDrop;
            ft.DragLeave += Ft_DragLeave;
            ft.Focus();
        }

        private void Ft_DragLeave(object sender, EventArgs e) { }

        private void Ft_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            for(int i = 0;i < files.Length; i++)
            {
                OpenFile(files[i]);
            }
        }

        private void Ft_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            autocompleteMenu1.SetAutocompleteMenu(fastColoredTextBox1, autocompleteMenu1);
            TabPage page = new TabPage();
            page.Text = Path.GetFileName($"Новый {NumberTab}");
            FastColoredTextBox ft = new FastColoredTextBox();
            ft.Dock = DockStyle.Fill;
            ft.Language = Language.CSharp;
            page.Controls.Add(ft);
            ft.AutoCompleteBrackets = true;
            tabControl1.TabPages.Add(page);
            NumberTab += 1;
            tabControl1.SelectedTab = page;
            fastColoredTextBoxes.Add(ft);
            documentMap1.Target = ft;
            #region
            ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();
            ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem("Вставить");
            ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem("Копировать");
            ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem("Вырезать");
            ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem("Выделить все");
            contextMenuStrip1.Items.AddRange(new ToolStripMenuItem[4]
            {
                toolStripMenuItem1,
                toolStripMenuItem2,
                toolStripMenuItem3,
                toolStripMenuItem4
            });
            toolStripMenuItem1.Click += new EventHandler(вставитьToolStripMenuItem_Click);
            toolStripMenuItem2.Click += new EventHandler(копироватьToolStripMenuItem_Click);
            toolStripMenuItem3.Click += new EventHandler(вырезатьToolStripMenuItem_Click);
            toolStripMenuItem4.Click += new EventHandler(выделитьВсToolStripMenuItem_Click);
            ft.ContextMenuStrip = contextMenuStrip1;
            contextMenuStrip1.Items.AddRange((ToolStripItem[])new ToolStripMenuItem[4]
            {
                  toolStripMenuItem1,
                  toolStripMenuItem2,
                  toolStripMenuItem3,
                  toolStripMenuItem4
            });
            #endregion
            ft.KeyDown += Ft_KeyDown;
            ft.DragLeave += Ft_DragLeave;
            ft.DragEnter += Ft_DragEnter;
            ft.DragDrop += Ft_DragDrop;
            ft.Focus();
        }

        private void Ft_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                сохранитьToolStripMenuItem.PerformClick();
            }
            if (e.Control && e.KeyCode == Keys.O)
            {
                открытьToolStripMenuItem.PerformClick();
            }
            if (e.Control && e.KeyCode == Keys.W)
            {
                закрытьToolStripMenuItem.PerformClick();
            }
            if (e.Control && e.KeyCode == Keys.N)
            {
                новыйToolStripMenuItem.PerformClick();
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileName != "")
                {
                    File.WriteAllText(FileName, fastColoredTextBoxes[tabControl1.SelectedIndex - 1].Text);
                }
            }
            catch { }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.SelectedTab.Text.Contains("Новый"))
                {
                    Text = $"{tabControl1.SelectedTab.Text} - Storm Editor";
                    documentMap1.Target = fastColoredTextBox1;
                    if (fastColoredTextBox1.ReadOnly == false)
                        толькоЧтениеToolStripMenuItem.Checked = false;
                    else
                        толькоЧтениеToolStripMenuItem.Checked = true;

                    if (fastColoredTextBox1.WordWrap == true)
                        переносПоСловамToolStripMenuItem.Checked = true;
                    else
                        переносПоСловамToolStripMenuItem.Checked = false;
                }
                else
                {
                    documentMap1.Target = fastColoredTextBoxes[tabControl1.SelectedIndex - 1];
                    this.Text = $"{PathsFiles[tabControl1.SelectedIndex - 1]} - Storm Editor ";
                    if (fastColoredTextBoxes[tabControl1.SelectedIndex - 1].ReadOnly == false)
                        толькоЧтениеToolStripMenuItem.Checked = false;
                    else
                        толькоЧтениеToolStripMenuItem.Checked = true;

                    if (fastColoredTextBoxes[tabControl1.SelectedIndex - 1].WordWrap == false)
                        переносПоСловамToolStripMenuItem.Checked = false;
                    else
                        переносПоСловамToolStripMenuItem.Checked = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sv = new SaveFileDialog();
            if (sv.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(sv.FileName, fastColoredTextBoxes[tabControl1.SelectedIndex - 1].Text);
                    OpenFile(sv.FileName);
                }
                catch
                {
                    File.WriteAllText(sv.FileName, fastColoredTextBoxes[tabControl1.SelectedIndex].Text);
                    OpenFile(sv.FileName);
                }
            }
        }

        private void выделитьВсToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.SelectedTab.Text.Contains("Новый 1"))
                {
                    fastColoredTextBox1.SelectAll();
                }
                fastColoredTextBoxes[tabControl1.SelectedIndex - 1].SelectAll();
            }
            catch
            {
                fastColoredTextBoxes[tabControl1.SelectedIndex].SelectAll();
            }
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.SelectedTab.Text.Contains("Новый 1"))
                {
                    fastColoredTextBox1.Copy();
                }
                fastColoredTextBoxes[tabControl1.SelectedIndex - 1].Copy();
            }
            catch
            {
                fastColoredTextBoxes[tabControl1.SelectedIndex].Copy();
            }
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.SelectedTab.Text.Contains("Новый 1"))
                {
                    fastColoredTextBox1.Cut();
                }
                fastColoredTextBoxes[tabControl1.SelectedIndex - 1].Cut();
            }
            catch
            {
                fastColoredTextBoxes[tabControl1.SelectedIndex].Cut();
            }
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.SelectedTab.Text.Contains("Новый"))
                {
                    fastColoredTextBox1.Paste();
                }
                fastColoredTextBoxes[tabControl1.SelectedIndex - 1].Paste();
            }
            catch
            {
                fastColoredTextBoxes[tabControl1.SelectedIndex].Paste();
            }
        }

        private void отменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.SelectedTab.Text.Contains("Новый 1"))
                {
                    fastColoredTextBox1.Undo();
                }
                fastColoredTextBoxes[tabControl1.SelectedIndex - 1].Undo();
            }
            catch
            {
                fastColoredTextBoxes[tabControl1.SelectedIndex].Undo();
            }
        }

        private void вернутьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.SelectedTab.Text.Contains("Новый 1"))
                {
                    fastColoredTextBox1.Redo();
                }
                fastColoredTextBoxes[tabControl1.SelectedIndex - 1].Redo();
            }
            catch
            {
                fastColoredTextBoxes[tabControl1.SelectedIndex].Redo();
            }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.SelectedTab.Text.Contains("Новый 1"))
                {
                    fastColoredTextBox1.SelectedText = "";
                }
                fastColoredTextBoxes[tabControl1.SelectedIndex - 1].SelectedText = "";
            }
            catch
            {
                fastColoredTextBoxes[tabControl1.SelectedIndex].SelectedText = "";
            }
        }

        private void печатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog1 = new PrintDialog();
            PrintDocument printDocument1 = new PrintDocument();

            printDocument1.PrintPage += PrintPageHandler;

            printDialog1.Document = printDocument1;

            if (printDialog1.ShowDialog() == DialogResult.OK)
                printDialog1.Document.Print();
        }
        private void PrintPageHandler(object s, PrintPageEventArgs e)
        {
            try
            {
                if(tabControl1.SelectedTab.Text.Contains("Новый 1"))
                    e.Graphics.DrawString(fastColoredTextBox1.Text, new Font("Arial", 14), Brushes.Black, 0, 0);
                else
                    e.Graphics.DrawString(fastColoredTextBoxes[tabControl1.SelectedIndex - 1].Text,
                        new Font("Arial", 14), Brushes.Black, 0, 0);
            }
            catch { }
        }

        private void topMostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.TopMost == false)
            {
                this.TopMost = true;
                оToolStripMenuItem.Checked = true;
            }
            else
            {
                this.TopMost = false;
                оToolStripMenuItem.Checked = false;
            }
        }

        private void переносПоСловамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.SelectedTab.Text.Contains("Новый 1"))
                {
                    if (fastColoredTextBox1.WordWrap == false)
                    {
                        fastColoredTextBox1.WordWrap = true;
                        переносПоСловамToolStripMenuItem.Checked = true;
                    }
                    else
                    {
                        fastColoredTextBox1.WordWrap = false;
                        переносПоСловамToolStripMenuItem.Checked = false;
                    }
                }
                else
                {
                    if (fastColoredTextBoxes[tabControl1.SelectedIndex - 1].WordWrap == false)
                    {
                        fastColoredTextBoxes[tabControl1.SelectedIndex - 1].WordWrap = true;
                        переносПоСловамToolStripMenuItem.Checked = true;
                    }
                    else
                    {
                        fastColoredTextBoxes[tabControl1.SelectedIndex - 1].WordWrap = false;
                        переносПоСловамToolStripMenuItem.Checked = false;
                    }
                }
            }
            catch { }
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e) { 
            tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
            string path = NamesFiles[tabControl1.SelectedIndex - 1];
            NamesFiles.Remove(path);
        }

        private void закрытьВсеToolStripMenuItem_Click(object sender, EventArgs e) { 
            tabControl1.TabPages.Clear();
            string path = PathsFiles[tabControl1.SelectedIndex - 1];
            PathsFiles.Remove(path);
        }

        private void имяФайлаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(NamesFiles[tabControl1.SelectedIndex - 1]);
            }
            catch { }
        }

        private void путьКФайлуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(PathsFiles[tabControl1.SelectedIndex - 1]);
            }
            catch { }
        }

        private void папкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            if(dlg.ShowDialog() == DialogResult.OK)
            {
                foreach(string path in Directory.GetFiles(dlg.SelectedPath))
                {
                    OpenFile(path);
                }
            }
        }

        private void толькоЧтениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Text.Contains("Новый 1"))
            {
                if (fastColoredTextBox1.ReadOnly == false)
                {
                    fastColoredTextBox1.ReadOnly = true;
                    толькоЧтениеToolStripMenuItem.Checked = true;
                }
                else
                {
                    fastColoredTextBox1.ReadOnly = false;
                    толькоЧтениеToolStripMenuItem.Checked = false;
                }
            }
            else
            {
                if (fastColoredTextBoxes[tabControl1.SelectedIndex - 1].ReadOnly == false)
                {
                    fastColoredTextBoxes[tabControl1.SelectedIndex - 1].ReadOnly = true;
                    толькоЧтениеToolStripMenuItem.Checked = true;
                }
                else
                {
                    fastColoredTextBoxes[tabControl1.SelectedIndex - 1].ReadOnly = false;
                    толькоЧтениеToolStripMenuItem.Checked = false;
                }
            }
        }
    }
}
