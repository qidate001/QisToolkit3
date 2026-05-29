using Microsoft.Win32;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Principal;
using System.Xml.Linq;
using QisToolkit3.Forms;
using static Qi;

namespace QisToolkit3
{
    public partial class FilesOperation : Form
    {
        private string selectedFiles;

        public FilesOperation()
        {
            InitializeComponent();
        }

        private void FilesOperation_Load(object sender, EventArgs e)
        {
            if (Qi.QisToolkit3_Datas.FilesOperation_AutomaticallyPopUpTheOpenFileWindow)
                OpenFileMain();
            else
                ReSetData();
        }

        private void OpenFileMain()
        {
            ReSetData();

            Directory.CreateDirectory(@"C:\WINDOWS\system32\config\systemprofile\Desktop");

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // 获取用户选择的文件路径
                selectedFiles = openFileDialog.FileNames[0];

                labelFileName.Text = string.Empty;

                if (selectedFiles != null)
                    GetDatas();
            }
        }

        private void GetDatas()
        {
            labelFileName.Text = "文件：" + selectedFiles + '\n';

            buttonDeleteFile.Enabled = true;
            buttonClearFile.Enabled = true;
            buttonReadFile.Enabled = true;
            buttonStartFile.Enabled = true;
            checkBoxSystem.Enabled = true;
            checkBoxHidden.Enabled = true;
            checkBoxArchive.Enabled = true;
            checkBoxReadOnly.Enabled = true;
            checkBoxNotContentIndexed.Enabled = true;
            checkBoxOffline.Enabled = true;

            buttonCreationTimeSet.Enabled = true;
            buttonLastWriteTimeSet.Enabled = true;
            buttonLastAccessTimeSet.Enabled = true;
            buttonCreationTimeSetNow.Enabled = true;
            buttonLastWriteTimeSetNow.Enabled = true;
            buttonLastAccessTimeSetNow.Enabled = true;
            textBoxCreationTime.Enabled = true;
            textBoxLastWriteTime.Enabled = true;
            textBoxLastAccessTime.Enabled = true;

            richTextBox.Enabled = true;
            buttonRead.Enabled = true;
            buttonCover.Enabled = true;
            buttonAppend.Enabled = true;
            checkBoxBinaryFile.Enabled = true;

            richTextBox.Text = Qi.TryReadAllText(selectedFiles);
            textBoxCreationTime.Text = File.GetCreationTime(selectedFiles).ToString();
            textBoxLastWriteTime.Text = File.GetLastWriteTime(selectedFiles).ToString();
            textBoxLastAccessTime.Text = File.GetLastAccessTime(selectedFiles).ToString();

            checkBoxSystem.Checked = (File.GetAttributes(selectedFiles) & FileAttributes.System) == FileAttributes.System;
            checkBoxHidden.Checked = (File.GetAttributes(selectedFiles) & FileAttributes.Hidden) == FileAttributes.Hidden;
            checkBoxOffline.Checked = (File.GetAttributes(selectedFiles) & FileAttributes.Offline) == FileAttributes.Offline;
            checkBoxArchive.Checked = (File.GetAttributes(selectedFiles) & FileAttributes.Archive) == FileAttributes.Archive;
            checkBoxReadOnly.Checked = (File.GetAttributes(selectedFiles) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
            checkBoxNotContentIndexed.Checked = (File.GetAttributes(selectedFiles) & FileAttributes.NotContentIndexed) == FileAttributes.NotContentIndexed;
        }

        private void ReSetData()
        {
            button_ManualPath.Visible = false;
            textBox_ManualPath.Visible = false;
            textBox_ManualPath.Text = "";

            ComponentResourceManager resources = new ComponentResourceManager(typeof(FilesOperation));
            labelFileName.Text = resources.GetString("labelFileName.Text");
            buttonDeleteFile.Enabled = false;
            buttonClearFile.Enabled = false;
            buttonReadFile.Enabled = false;
            buttonStartFile.Enabled = false;
            checkBoxSystem.Enabled = false;
            checkBoxHidden.Enabled = false;
            checkBoxOffline.Enabled = false;
            checkBoxArchive.Enabled = false;
            checkBoxReadOnly.Enabled = false;
            checkBoxNotContentIndexed.Enabled = false;

            richTextBox.Enabled = false;
            buttonRead.Enabled = false;
            buttonCover.Enabled = false;
            buttonAppend.Enabled = false;
            checkBoxBinaryFile.Enabled = false;

            buttonCreationTimeSet.Enabled = false;
            buttonLastWriteTimeSet.Enabled = false;
            buttonLastAccessTimeSet.Enabled = false;
            buttonCreationTimeSetNow.Enabled = false;
            buttonLastWriteTimeSetNow.Enabled = false;
            buttonLastAccessTimeSetNow.Enabled = false;
            textBoxCreationTime.Enabled = false;
            textBoxLastWriteTime.Enabled = false;
            textBoxLastAccessTime.Enabled = false;
            textBoxCreationTime.Text = "";
            textBoxLastWriteTime.Text = "";
            textBoxLastAccessTime.Text = "";
            richTextBox.Text = "";
        }

        private void buttonDeleteFile_Click(object sender, EventArgs e) => DeleteFileMain();

        private void buttonClearFile_Click(object sender, EventArgs e) => ClearFileMain();

        private void buttonReadFile_Click(object sender, EventArgs e) => Qi.MessageBoxReadFile(selectedFiles);

        private void buttonCreationTimeSet_Click(object sender, EventArgs e) => SetCreationTimeMain();

        private void buttonLastWriteTimeSet_Click(object sender, EventArgs e) => SetLastWriteTimeMain();

        private void buttonLastAccessTimeSet_Click(object sender, EventArgs e) => SetLastAccessTimeMain();

        private void buttonStartFile_Click(object sender, EventArgs e) => StartFileMain();


        private void DeleteFileMain()
        {
            try
            {
                File.Delete(selectedFiles);
                MessageBox.Show("文件已删除", "提示");
                OpenFileMain();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("文件删除失败，原因：" + ex.Message, "错误", MessageBoxButtons.CancelTryContinue) == DialogResult.TryAgain)
                    DeleteFileMain();
            }
        }

        private void ClearFileMain()
        {
            try
            {
                File.WriteAllText(selectedFiles, string.Empty);
                MessageBox.Show("文件清空完成", "提示");
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("文件清空失败，原因：" + ex.Message, "错误", MessageBoxButtons.CancelTryContinue) == DialogResult.TryAgain)
                    ClearFileMain();
            }
        }

        private void ReadFileMain()
        {
            try
            {
                MessageBox.Show(File.ReadAllText(selectedFiles), "文件阅读");
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("文件阅读失败，原因：" + ex.Message, "错误", MessageBoxButtons.CancelTryContinue) == DialogResult.TryAgain)
                    ReadFileOnRichTextBoxMain();
            }
        }

        private void buttonCreationTimeSetNow_Click(object sender, EventArgs e)
        {
            textBoxCreationTime.Text = DateTime.Now.ToString();
            SetCreationTimeMain();
        }

        private void buttonLastWriteTimeSetNow_Click(object sender, EventArgs e)
        {
            textBoxLastWriteTime.Text = DateTime.Now.ToString();
            SetLastWriteTimeMain();
        }

        private void buttonLastAccessTimeSetNow_Click(object sender, EventArgs e)
        {
            textBoxLastAccessTime.Text = DateTime.Now.ToString();
            SetLastAccessTimeMain();
        }

        private void SetCreationTimeMain()
        {
            try
            {
                File.SetCreationTime(selectedFiles, Convert.ToDateTime(textBoxCreationTime.Text));
                textBoxCreationTime.Text = Convert.ToDateTime(textBoxCreationTime.Text).ToString();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("文件修改属性失败，原因：" + ex.Message, "错误", MessageBoxButtons.CancelTryContinue) == DialogResult.TryAgain)
                    SetCreationTimeMain();
            }
        }

        private void SetLastWriteTimeMain()
        {
            try
            {
                File.SetLastWriteTime(selectedFiles, Convert.ToDateTime(textBoxLastWriteTime.Text));
                textBoxLastWriteTime.Text = Convert.ToDateTime(textBoxLastWriteTime.Text).ToString();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("文件修改属性失败，原因：" + ex.Message, "错误", MessageBoxButtons.CancelTryContinue) == DialogResult.TryAgain)
                    SetLastWriteTimeMain();
            }
        }

        private void SetLastAccessTimeMain()
        {
            try
            {
                File.SetLastAccessTime(selectedFiles, Convert.ToDateTime(textBoxLastAccessTime.Text));
                textBoxLastAccessTime.Text = Convert.ToDateTime(textBoxLastAccessTime.Text).ToString();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("文件修改属性失败，原因：" + ex.Message, "错误", MessageBoxButtons.CancelTryContinue) == DialogResult.TryAgain)
                    SetLastAccessTimeMain();
            }
        }

        private void StartFileMain()
        {
            try
            {
                Qi.OpenFile(selectedFiles);
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("文件打开失败，原因：" + ex.Message, "错误", MessageBoxButtons.CancelTryContinue) == DialogResult.TryAgain)
                    StartFileMain();
            }
        }


        private void ReadFileOnRichTextBoxMain()
        {
            if (checkBoxBinaryFile.Checked)
                richTextBox.Text = Qi.StringToByte(File.ReadAllText(selectedFiles));
            else
                richTextBox.Text = File.ReadAllText(selectedFiles);
        }

        private void WriteFileMain()
        {
            //if (checkBoxBinaryFile.Checked)
            //    Qi.ExportBinaryFile(selectedFiles, Encoding.Unicode.GetBytes(richTextBox.Text));
            //else
            File.WriteAllText(selectedFiles, richTextBox.Text);
        }

        private void labelFileName_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                contextMenuStrip.Show(MousePosition.X, MousePosition.Y);
            else
                OpenFileMain();
        }

        private void toolStripMenuItem_Automatic_Click(object sender, EventArgs e) => OpenFileMain();

        private void ToolStripMenuItem_ReSet_Click(object sender, EventArgs e) => ReSetData();

        private void ToolStripMenuItem_Manual_Click(object sender, EventArgs e)
        {
            button_ManualPath.Visible = true;
            textBox_ManualPath.Visible = true;
        }

        private void button_ManualPath_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBox_ManualPath.Text))
            {
                button_ManualPath.Visible = false;
                textBox_ManualPath.Visible = false;

                selectedFiles = textBox_ManualPath.Text;

                GetDatas();
            }
            else
                MessageBox.Show("文件不存在", "错误", MessageBoxButtons.OK);
        }

        private void checkBoxHidden_CheckedChanged(object sender, EventArgs e) => SetHiddenFileMain(selectedFiles, checkBoxHidden.Checked);

        private void checkBoxArchive_CheckedChanged(object sender, EventArgs e) => SetArchiveFileMain(selectedFiles, checkBoxArchive.Checked);

        private void checkBoxReadOnly_CheckedChanged(object sender, EventArgs e) => SetReadOnlyFileMain(selectedFiles, checkBoxReadOnly.Checked);

        private void checkBoxSystem_CheckedChanged(object sender, EventArgs e) => SetSystemFileMain(selectedFiles, checkBoxSystem.Checked);

        private void checkBoxOffline_CheckedChanged(object sender, EventArgs e) => SetOfflineFileMain(selectedFiles, checkBoxOffline.Checked);

        private void checkBoxNotContentIndexed_CheckedChanged(object sender, EventArgs e) => SetNotContentIndexedFileMain(selectedFiles, checkBoxNotContentIndexed.Checked);

        private void buttonRead_Click(object sender, EventArgs e) => ReadFileOnRichTextBoxMain();

        private void buttonCover_Click(object sender, EventArgs e) => WriteFileMain();

        private void checkBoxBinaryFile_CheckedChanged(object sender, EventArgs e)
        {
            ReadFileOnRichTextBoxMain();
            richTextBox.ReadOnly = checkBoxBinaryFile.Checked;
        }

        private void button_FilesOperationPlus_Click(object sender, EventArgs e) =>
            new FilesOperationPlus().Show();

        private void labelFileName_Click(object sender, EventArgs e)
        {

        }
    }
}