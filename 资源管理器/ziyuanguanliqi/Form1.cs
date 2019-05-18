using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Microsoft.VisualBasic.Devices;
using System.Threading;
using System.Security.AccessControl;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace ziyuanguanliqi
{
    public partial class Form1 : Form
    {
        private string curFilePath = "";
        private string fileName;
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e){}

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;

            //TreeNode root = new TreeNode("Computer", 0, 0);
            //reeView1.Nodes.Add(root);
            DriveInfo[] drvs=DriveInfo.GetDrives();
            foreach (DriveInfo d in drvs)
            {

                //TreeNode tn =new TreeNode(d.VolumeLabel+"("+d.Name.Substring(0,d.Name.Length-1)+")");
                TreeNode tn = treeView1.Nodes.Add(d.VolumeLabel + "(" + d.Name.Substring(0, d.Name.Length - 1) + ")");
                tn.Tag = d.Name;
                tn.ImageIndex = IconsIndexes.FixedDrive;
                //root.Nodes.Add(tn);
            }
            //root.Expand();
            foreach (TreeNode node in treeView1.Nodes)
            {
                LoadChildNodes(node);
            }
            //MessageBox.Show("s");
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            /*if (e.Node.Nodes != null)
            {
                foreach (TreeNode nd in e.Node.Nodes)
                {
                    string path = (string)nd.Tag;
                    string[] dirs = null;
                    try
                    {
                        dirs = Directory.GetDirectories(path);
                        //e.Node.Tag = path;
                    }
                    catch (Exception){}
                    //DirectoryInfo di = new DirectoryInfo(path);
                    if (dirs != null)
                    {
                        foreach (string d in dirs)
                        {
                            string dirname = d.Substring(d.LastIndexOf('\\') + 1);
                            TreeNode td = new TreeNode(dirname, 2, 3);
                            nd.Tag = d;
                            nd.Nodes.Add(td);
                        }
                    }
                }
            }*/
           e.Node.Expand();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //e.Node.Expand();
            //listView1.Clear();
            listView1.Columns.Add("Name", 100);
            listView1.Columns.Add("ModificationTime", 150);
            listView1.Columns.Add("Type", 100);
            listView1.Columns.Add("Size", 100);
            /*if (e.Node != null)
            {
                DirectoryInfo t = new DirectoryInfo(e.Node.Tag.ToString());
                DirectoryInfo[] dirs = t.GetDirectories();
                if(dirs!=null)
                    foreach (DirectoryInfo d in dirs)
                    {
                        //e.Node.Tag = t.FullName;
                        ListViewItem item = new ListViewItem(d.Name);
                        item.SubItems.Add(d.LastWriteTime.ToString());
                        item.ImageIndex = IconsIndexes.Folder;
                        listView1.Items.Add(item);
                    }
                FileInfo[] files = t.GetFiles();
            }*/
            ShowFilesList(e.Node.Tag.ToString(), false);
        }

        private void toolStripLabel1_Click(object sender, EventArgs e){}

        private void viewToolStripMenuItem_Click(object sender, EventArgs e){}

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            LoadChildNodes(e.Node);
        }

        private class IconsIndexes
        {
            public const int FixedDrive = 0; 
            public const int Image = 1; 
            public const int RemovableDisk = 2; 
            public const int Folder = 3; 
            public const int RecentFiles = 4; 
        }
        private void LoadChildNodes(TreeNode node)
        {
            try
            {             
                node.Nodes.Clear();
                    DirectoryInfo directoryInfo = new DirectoryInfo(node.Tag.ToString());
                    DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();

                    foreach (DirectoryInfo info in directoryInfos)
                    {

                        TreeNode childNode = node.Nodes.Add(info.Name);
                        childNode.Tag = info.FullName;
                        childNode.ImageIndex = IconsIndexes.Folder;
                        childNode.SelectedImageIndex = IconsIndexes.Folder;
                        childNode.Nodes.Add("");
                    
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void SearchWithMultiThread(string path, string fileName)
        {
            listView1.Items.Clear();
            toolStripStatusLabel1.Text = 0 + " 个项目";
            this.fileName = fileName;
            ThreadPool.SetMaxThreads(1000, 1000);
            ThreadPool.QueueUserWorkItem(new WaitCallback(Search), path);
        }


        private void toolStripTextBox1_Enter(object sender, EventArgs e)
        {
            toolStripTextBox1.Text = "";
        }

        private void toolStripTextBox1_Leave(object sender, EventArgs e)
        {
            toolStripTextBox1.Text = "Search";
        }


        public void Search(Object obj)
        {
            string path = obj.ToString();

            DirectorySecurity directorySecurity = new DirectorySecurity(path, AccessControlSections.Access);

            //目录可以访问
            if (!directorySecurity.AreAccessRulesProtected)
            {

                //待搜索路径
                DirectoryInfo directoryInfo = new DirectoryInfo(path);

                //待搜索路径下的文件
                FileInfo[] fileInfos = directoryInfo.GetFiles();

                //搜索文件
                if (fileInfos.Length > 0)
                {
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        try
                        {
                            if (fileInfo.Name.Split('.')[0].Contains(fileName))
                            {
                                //AddSearchResultItemIntoList(fileInfo.FullName, true);
                                ShowFilesList(fileInfo.FullName, true);
                                toolStripStatusLabel1.Text = listView1.Items.Count + " 个项目";
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }

                }


                //待搜索路径下的子文件夹
                DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();

                //搜索文件夹
                if (directoryInfos.Length > 0)
                {
                    foreach (DirectoryInfo dirInfo in directoryInfos)
                    {
                        try
                        {
                            if (dirInfo.Name.Contains(fileName))
                            {
                                //AddSearchResultItemIntoList(dirInfo.FullName, false);
                                ShowFilesList(dirInfo.FullName, true);
                                //更新状态栏
                                toolStripStatusLabel1.Text = listView1.Items.Count + " 个项目";
                            }
                            else
                            {
                                //多线程策略一：从待搜索文件夹开始，递归过程中每遇到一个文件夹便为其开一个线程进行递归搜索，线程总数多，但是
                                //使用的是线程池，它会进行自动管理，使得线程可以被反复利用，一个线程的搜索任务执行完成之后，又可以继续被利用去
                                //执行另一个在任务队列中的搜索任务。
                                //优点：可以适应普遍情况，搜索速度一般情况下很快！
                                ThreadPool.QueueUserWorkItem(new WaitCallback(Search), dirInfo.FullName);

                                //多线程策略二：为待搜索文件夹下每个文件夹开一个线程进行递归搜索，此后不再开线程，线程总数等于待搜索文件夹的子文件夹数。
                                //缺点:当待搜索文件夹的子文件夹数越少时，效果越差，速度越慢。
                                //ThreadPool.QueueUserWorkItem(new WaitCallback(SearchWithOneThread), dirInfo.FullName);
                            }
                        }
                        catch (Exception){}

                    }
                }
            }
        }
        public void ShowFilesList(string path, bool isRecord)
        {

            //tsbtnBack.Enabled = true;

            /*if (isRecord)
            {

                DoublyLinkedListNode newNode = new DoublyLinkedListNode();
                newNode.Path = path;
                curPathNode.NextNode = newNode;
                newNode.PreNode = curPathNode;

                curPathNode = newNode;
            }*/
            
            //MessageBox.Show("s");
            listView1.BeginUpdate();

            listView1.Items.Clear();
                try
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(path);
                    DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
                    FileInfo[] fileInfos = directoryInfo.GetFiles();

                    foreach (ListViewItem item in listView1.Items)
                    {
                        if (item.Text.EndsWith(".exe"))
                        {
                            imageList1.Images.RemoveByKey(item.Text);
                        }
                    }


                    foreach (DirectoryInfo dirInfo in directoryInfos)
                    {
                        ListViewItem item = listView1.Items.Add(dirInfo.Name, IconsIndexes.Folder);
                        item.Tag = dirInfo.FullName;
                        item.SubItems.Add(dirInfo.LastWriteTime.ToString());
                        item.SubItems.Add("文件夹");
                        item.SubItems.Add("");
                    }


                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        ListViewItem item = listView1.Items.Add(fileInfo.Name);


                        if (fileInfo.Extension == ".exe" || fileInfo.Extension == "")
                        {
                           
                            Icon fileIcon = GetSystemIcon.GetIconByFileName(fileInfo.FullName);

                            imageList1.Images.Add(fileInfo.Name, fileIcon);

                            item.ImageKey = fileInfo.Name;
                        }

                        else
                        {
                            if (!imageList1.Images.ContainsKey(fileInfo.Extension))
                            {
                                Icon fileIcon = GetSystemIcon.GetIconByFileName(fileInfo.FullName);
                                
                                imageList1.Images.Add(fileInfo.Extension, fileIcon);
                            }

                            item.ImageKey = fileInfo.Extension;
                        }

                        item.Tag = fileInfo.FullName;
                        item.SubItems.Add(fileInfo.LastWriteTime.ToString());
                        item.SubItems.Add(fileInfo.Extension + "文件");
                        item.SubItems.Add(ShowFileSize(fileInfo.Length).Split('(')[0]);
                    }

                }
               catch (Exception e)
                {
                    MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            curFilePath = path;

            //tscboAddress.Text = curFilePath;

            toolStripStatusLabel1.Text = listView1.Items.Count + " 个项目";

            listView1.EndUpdate();

        }

         public static string ShowFileSize(long fileSize)
        {
            string fileSizeStr = "";

            if (fileSize < 1024)
            {
                fileSizeStr = fileSize + " Byte(s)";
            }
            else if (fileSize >= 1024 && fileSize < 1024 * 1024)
            {
                fileSizeStr = Math.Round(fileSize * 1.0 / 1024, 2, MidpointRounding.AwayFromZero) + " KB(" + fileSize + "Byte(s))";
            }
            else if (fileSize >= 1024 * 1024 && fileSize < 1024 * 1024 * 1024)
            {
                fileSizeStr = Math.Round(fileSize * 1.0 / (1024 * 1024), 2, MidpointRounding.AwayFromZero) + " MB(" + fileSize + "Byte(s))";
            }
            else if (fileSize >= 1024 * 1024 * 1024)
            {
                fileSizeStr = Math.Round(fileSize * 1.0 / (1024 * 1024 * 1024), 2, MidpointRounding.AwayFromZero) + " GB(" + fileSize + "字节)";
            }

            return fileSizeStr;
        }

         private void AddSearchResultItemIntoList(string fullPath, bool isFile)
         {
             //MessageBox.Show("s");
             //是文件
             listView1.BeginUpdate();

             listView1.Items.Clear();
             if (isFile)
             {
                 //MessageBox.Show("s");
                 FileInfo fileInfo = new FileInfo(fullPath);
                 ListViewItem item = listView1.Items.Add(fileInfo.Name);
                 //MessageBox.Show("s");

                 if (fileInfo.Extension == ".exe" || fileInfo.Extension == "")
                 {
                     //通过当前系统获得文件相应图标
                     Icon fileIcon = GetSystemIcon.GetIconByFileName(fileInfo.FullName);

                     //因为不同的exe文件一般图标都不相同，所以不能按拓展名存取图标，应按文件名存取图标
                     imageList1.Images.Add(fileInfo.Name, fileIcon);

                     item.ImageKey = fileInfo.Name;
                 }

                 else
                 {
                     if (!imageList1.Images.ContainsKey(fileInfo.Extension))
                     {
                         Icon fileIcon = GetSystemIcon.GetIconByFileName(fileInfo.FullName);

                         imageList1.Images.Add(fileInfo.Extension, fileIcon);
                     }

                     item.ImageKey = fileInfo.Extension;
                 }

                 item.Tag = fileInfo.FullName;

                 item.SubItems.Add(fileInfo.LastWriteTimeUtc.ToString());
                 item.SubItems.Add(fileInfo.Extension + "文件");
                 item.SubItems.Add(ShowFileSize(fileInfo.Length).Split('(')[0]);
             }
             //是文件夹
             else
             {
                 MessageBox.Show("s");
                 DirectoryInfo dirInfo = new DirectoryInfo(fullPath);
                 ListViewItem item = listView1.Items.Add(dirInfo.Name, IconsIndexes.Folder);
                 item.Tag = dirInfo.FullName;
                 item.SubItems.Add(dirInfo.LastWriteTimeUtc.ToString());
                 item.SubItems.Add("文件夹");
                 item.SubItems.Add("");
             }
             listView1.EndUpdate();
         }

         private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
         {
             //回车输入文件名
             if (e.KeyCode == Keys.Enter)
             {
                 string fileName = toolStripTextBox1.Text;

                 if (string.IsNullOrEmpty(fileName))
                 {
                     return;
                 }

                 //使用多线程搜索文件/文件夹
                 SearchWithMultiThread(curFilePath, fileName);
             }
         }


         private void Open()
         {
             if (listView1.SelectedItems.Count > 0)
             {
                 string path = listView1.SelectedItems[0].Tag.ToString();

                 try
                 {
                     if (Directory.Exists(path))
                     {
                         ShowFilesList(path, true);
                     }

                     else
                     {
                         Process.Start(path);
                     }
                 }
                 catch (Exception e)
                 {
                     MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 }
             }
         }

         private void openOToolStripMenuItem_Click(object sender, EventArgs e)
         {
             Open();
         }

         private void listView1_ItemActivate(object sender, EventArgs e)
         {
             Open();
         }

         private void toolStripMenuItem1_Click(object sender, EventArgs e){}

         private void statusStripToolStripMenuItem_Click(object sender, EventArgs e)
         {
             if (statusStrip1.Visible == true) statusStrip1.Visible = false;
             else statusStrip1.Visible = true;
         }

         private void toolStripToolStripMenuItem_Click(object sender, EventArgs e)
         {
             if (toolStrip1.Visible == true) toolStrip1.Visible = false;
             else toolStrip1.Visible = true;
         }

    }
    class GetSystemIcon
    {
        //依据文件名读取图标，若指定文件不存在，则返回空值。
        public static Icon GetIconByFileName(string fileName)
        {
            if (fileName == null || fileName.Equals(string.Empty)) return null;
            if (!File.Exists(fileName)) return null;

            SHFILEINFO shinfo = new SHFILEINFO();
            //Use this to get the small Icon
            Win32.SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_ICON | Win32.SHGFI_LARGEICON);
            //The icon is returned in the hIcon member of the shinfo struct
            System.Drawing.Icon myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon);
            return myIcon;
        }


        //给出文件扩展名（.*），返回相应图标。若不以"."开头则返回文件夹的图标。
        public static Icon GetIconByFileType(string fileType, bool isLarge)
        {
            if (fileType == null || fileType.Equals(string.Empty)) return null;

            RegistryKey regVersion = null;
            string regFileType = null;
            string regIconString = null;
            string systemDirectory = Environment.SystemDirectory + "\\";

            if (fileType[0] == '.')
            {
                //读系统注册表中文件类型信息
                regVersion = Registry.ClassesRoot.OpenSubKey(fileType, true);
                if (regVersion != null)
                {
                    regFileType = regVersion.GetValue("") as string;
                    regVersion.Close();
                    regVersion = Registry.ClassesRoot.OpenSubKey(regFileType + @"\DefaultIcon", true);
                    if (regVersion != null)
                    {
                        regIconString = regVersion.GetValue("") as string;
                        regVersion.Close();
                    }
                }
                if (regIconString == null)
                {
                    //没有读取到文件类型注册信息，指定为未知文件类型的图标
                    regIconString = systemDirectory + "shell32.dll,0";
                }
            }
            else
            {
                //直接指定为文件夹图标
                regIconString = systemDirectory + "shell32.dll,3";
            }
            string[] fileIcon = regIconString.Split(new char[] { ',' });
            if (fileIcon.Length != 2)
            {
                //系统注册表中注册的标图不能直接提取，则返回可执行文件的通用图标
                fileIcon = new string[] { systemDirectory + "shell32.dll", "2" };
            }
            Icon resultIcon = null;
            try
            {
                //调用API方法读取图标
                int[] phiconLarge = new int[1];
                int[] phiconSmall = new int[1];
                uint count = Win32.ExtractIconEx(fileIcon[0], Int32.Parse(fileIcon[1]), phiconLarge, phiconSmall, 1);
                IntPtr IconHnd = new IntPtr(isLarge ? phiconLarge[0] : phiconSmall[0]);
                resultIcon = Icon.FromHandle(IconHnd);
            }
            catch { }
            return resultIcon;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public IntPtr iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    };


    //定义调用的API方法
    class Win32
    {
        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; // Large icon
        public const uint SHGFI_SMALLICON = 0x1; // Small icon

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
        [DllImport("shell32.dll")]
        public static extern uint ExtractIconEx(string lpszFile, int nIconIndex, int[] phiconLarge, int[] phiconSmall, uint nIcons);

    }
}
