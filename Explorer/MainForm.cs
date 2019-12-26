using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.VisualBasic.FileIO;

namespace Explorer
{
    public partial class MainForm : Form
    {
        #region Field
        private bool _isLoading;
        private bool isCopy;
        private int itemNum;
        
        private string copyFile;
        private string copyName;
        private string copySize;
        private string copyAttribute;
        private string userName;
        private string desktopDirectoryPath;
        private string[] splitDesktopDirectoryPath;

        private Stack<string> forwardStack = new Stack<string>();
        private ListViewItem dragMove;
        private NotifyIcon notify;
        private TextBox tbx;
        private Size size;
        #endregion

        #region Constructor
        public MainForm()
        {
            
            InitializeComponent();
            
            this._isLoading = false;
            this.itemNum = 0;
            this.desktopDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            this.splitDesktopDirectoryPath = desktopDirectoryPath.Split('\\');
            this.userName = splitDesktopDirectoryPath[2];

            InitTreeDriveSetting();

            label1.Visible = false;
            trackBar.Visible = false;
            listView1.DoubleBuffered(true);
            toolTip1.SetToolTip(helpbt, "도움말");
            RenderTray();
        }
        #endregion

        #region TreeView Event
        // 트리 노드 선택 이벤트
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_isLoading == false)
            {
                path.Text = e.Node.FullPath;
                ListViewSetting(e.Node.FullPath);
            }
        }

        // 트리 노드 확장 이벤트
        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            // 하위노드 모두 제거
            for (; ; )
            {
                if (e.Node.FirstNode == null)
                    break;
                else
                    e.Node.Nodes[0].Remove();
            }

            TreeNodeSetting(e.Node);
            e.Node.Expand();

        }
        #endregion

        #region TreeView Method
        // 드라이브 렌더링
        private void InitTreeDriveSetting()
        {
            int count;
            string[] strDrives = null;

            strDrives = Directory.GetLogicalDrives();
            
            InitImage();
            
            foreach (string drive in strDrives)
            {
                imgTree.Images.Add(GetSystemImg.GetIcon(drive, false));

                count = treeView1.Nodes.Add(new TreeNode(drive.Substring(0, 2), imgTree.Images.Count - 1, imgTree.Images.Count - 1));

                if (HasSubDirectory(drive.Substring(0, 2)))
                {
                    treeView1.Nodes[count].Nodes.Add("XXX");
                }
            }

            // 초기화면 C드라이브 선택 효과
            favoriteView.SelectedNode = favoriteView.Nodes[0];
            favoriteView.SelectedNode.Expand();
            treeView1.SelectedNode = treeView1.Nodes[0];
            treeView1.SelectedNode.Expand();
        }

        // 하위 폴더 렌더링
        private void TreeNodeSetting(TreeNode node)
        {
            string strPath;
            int count;

            strPath = node.FullPath + "\\";

            DirectoryInfo directoryInfo = new DirectoryInfo(strPath);

            // 폴더 정보
            try
            {
                treeView1.BeginUpdate(); // 업데이트가 끝날 때까지 UI 갱신 중지
                listView1.BeginUpdate();
                trackBar.Enabled = false;
                this.KeyPreview = false;

                // 폴더 정보를 얻기
                foreach (DirectoryInfo subdirectoryInfo in directoryInfo.GetDirectories())
                {
                    if (subdirectoryInfo.Name == "Fonts" || subdirectoryInfo.Name == "assembly")
                    {
                        continue; // Fonts폴더를 열 수 없어 우선 배제처리
                    }

                    if ((subdirectoryInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    {
                        continue; // 숨긴폴더 사전제거
                    }

                    count = node.Nodes.Add(new TreeNode(subdirectoryInfo.Name, 0, 0));

                    if (HasSubDirectory(node.Nodes[count].FullPath))
                    {
                        node.Nodes[count].Nodes.Add("XXX");
                    }
                }

                treeView1.EndUpdate(); // 업데이트 종료
                listView1.EndUpdate();
                trackBar.Enabled = true;
                this.KeyPreview = true;
            }
            catch
            {
                // 디렉토리/파일 정보 읽을때 예외 발생하면 무시(파일 속성때문에 예외 발생함)
            }
        }
        
        // 하위 폴더 유무 검사
        private bool HasSubDirectory(string strPath)
        {
            strPath = strPath + "\\";

            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(strPath);

                if (directoryInfo.GetDirectories().Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        // 트리 노드 찾기 : 현재 선택된 노드의 자식들 중에서 찾는다.
        private TreeNode FindNode(string strDirectory)
        {
            TreeNode temp_node;

            // 현재 선택된 노드의 처음 자식 노드를 찾음
            temp_node = treeView1.SelectedNode.FirstNode;

            for (; ; )
            {
                try
                {
                    if (temp_node.Text.Equals(strDirectory))
                        return temp_node;
                }
                catch
                {
                }

                temp_node = temp_node.NextNode;
            }
        }
        #endregion

        #region ListView Event
        // 리스트뷰 더블클릭
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (_isLoading == true) return;
            try
            {
                if (listView1.SelectedItems[0].Text.Equals("빈 폴더")) return;
            }
            catch
            {
            }

            string strSel;
            string strSize;

            try
            {
                strSel = listView1.SelectedItems[0].SubItems[0].Text;
                strSize = listView1.SelectedItems[0].SubItems[1].Text;
            }
            catch // 렌더링 중 이동불가 처리
            {
                return;
            }

            if (strSize.Equals("")) // 폴더일 경우
            {
                TreeNode treeNodeTemp = null;
                try
                {
                    treeView1.SelectedNode.Expand();
                    treeNodeTemp = FindNode(strSel);
                }
                catch
                {
                    foreach (TreeNode node in treeView1.Nodes)
                    {
                        if (node.Text.Equals(path.Text))
                        {
                            treeView1.SelectedNode = node;
                            return;
                        }
                    }
                    treeView1.SelectedNode.Expand();
                    treeNodeTemp = FindNode(strSel);
                }
                if (treeNodeTemp != null)
                {
                    try
                    {
                        treeView1.SelectedNode = treeNodeTemp;
                    }
                    catch
                    {
                    }
                    treeNodeTemp.Expand();
                }
            }
            else // 파일일 경우
            {
                try
                {
                    //FileExecute.ShellExecute(0, "open", path.Text + "\\" + strSel, null, null, (int)FileExecute.SW.SHOWNORMAL);
                    Process.Start(path.Text + "\\" + listView1.SelectedItems[0].SubItems[0].Text);
                }
                catch
                {
                    MessageBox.Show("ERROR: 연결된 프로그램이 없습니다.\n연결 프로그램 지정 후 실행하십시오.");
                }
            }
        }

        // 칼럼값으로 정렬
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (_isLoading == true) return;

            //정렬을 위하여 사용 됨.
            if (listView1.Sorting == SortOrder.Ascending)
                listView1.Sorting = SortOrder.Descending;
            else
                listView1.Sorting = SortOrder.Ascending;


            listView1.ListViewItemSorter = new ListViewItemComparer(e.Column, listView1.Sorting);
        }

        // 리스트뷰 아이템 우클릭 메뉴
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (listView1.SelectedItems[0].Text.Equals("빈 폴더")) return;
            }
            catch
            {
            }
            if (e.Button == MouseButtons.Right)
            {
                var selected = listView1.SelectedItems;

                if (selected.Count > 0 && listView1.GetItemAt(e.X, e.Y) != null)
                {
                    var item = selected[0];
                    var ctx = new ContextMenuStrip();
                    var menu0 = new ToolStripMenuItem("열기");
                    menu0.Click += (o, s) =>
                    {
                        #region 실행
                        string strSel;
                        string strSize;

                        strSel = listView1.SelectedItems[0].SubItems[0].Text;
                        strSize = listView1.SelectedItems[0].SubItems[1].Text;

                        if (strSize.Equals("")) // 폴더일 경우
                        {
                            TreeNode treeNodeTemp = null;
                            try
                            {
                                treeView1.SelectedNode.Expand();
                                treeNodeTemp = FindNode(strSel);
                            }
                            catch
                            {
                                foreach (TreeNode node in treeView1.Nodes)
                                {
                                    if (node.Text.Equals(path.Text))
                                    {
                                        treeView1.SelectedNode = node;
                                        return;
                                    }
                                }
                                treeView1.SelectedNode.Expand();
                                treeNodeTemp = FindNode(strSel);
                            }
                            if (treeNodeTemp != null)
                            {
                                treeView1.SelectedNode = treeNodeTemp;
                                treeNodeTemp.Expand();
                            }
                        }
                        else // 파일일 경우
                        {
                            //FileExecute.ShellExecute(0, "open", path.Text + "\\" + strSel, null, null, (int)FileExecute.SW.SHOWNORMAL);
                            Process.Start(path.Text + "\\" + listView1.SelectedItems[0].SubItems[0].Text);
                        }
                        #endregion
                    };
                    ctx.Items.Add(menu0);

                    ctx.Items.Add("-");

                    var menu3 = new ToolStripMenuItem("잘라내기");
                    menu3.Click += (o, s) =>
                    {
                        isCopy = false;
                        copyFile = path.Text + "\\" + listView1.SelectedItems[0].SubItems[0].Text;
                        copyName = listView1.SelectedItems[0].SubItems[0].Text;
                        copySize = listView1.SelectedItems[0].SubItems[1].Text;
                        copyAttribute = listView1.SelectedItems[0].SubItems[5].Text;
                    };
                    ctx.Items.Add(menu3);

                    var menu4 = new ToolStripMenuItem("복사");
                    menu4.Click += (o, s) =>
                    {
                        isCopy = true;
                        copyFile = path.Text + "\\" + listView1.SelectedItems[0].SubItems[0].Text;
                        copyName = listView1.SelectedItems[0].SubItems[0].Text;
                        copySize = listView1.SelectedItems[0].SubItems[1].Text;
                        copyAttribute = listView1.SelectedItems[0].SubItems[5].Text;
                    };
                    ctx.Items.Add(menu4);

                    #region 불필요
                    /*var menu5 = new ToolStripMenuItem("붙여넣기");
                       menu5.Click += (o, s) =>
                       {
                           if (copyFile == null)
                           {
                               MessageBox.Show("복사된 파일/폴더가 없습니다.");
                               return;
                           }
                           if (listView1.SelectedItems[0].SubItems[1].Text.Equals("")) // 폴더일 경우
                           {
                               string path1 = copyFile;
                               string path2 = path.Text + "\\" + listView1.SelectedItems[0].SubItems[0].Text + "\\" + copyName;

                               if (copySize.Equals("")) // 폴더일 경우
                               {
                                   if (Directory.Exists(path2))
                                   {
                                       MessageBox.Show("중복된 폴더명 입니다.");
                                       return;
                                   }
                                   try
                                   {
                                       if (isCopy)
                                       {
                                           FileSystem.CopyDirectory(path1, path2);
                                       }
                                       else
                                       {
                                           FileSystem.MoveDirectory(path1, path2);
                                       }
                                   }
                                   catch
                                   {
                                       MessageBox.Show("대상 폴더가 원본 폴더의 하위 폴더입니다.");
                                   }
                               }
                               else
                               {
                                   if (File.Exists(path2))
                                   {
                                       MessageBox.Show("중복된 파일명 입니다.");
                                       return;
                                   }
                                   if (isCopy)
                                   {
                                       File.Copy(path1, path2);
                                   }
                                   else
                                   {
                                       File.Move(path1, path2);
                                   }
                               }

                               if (HasSubDirectory(path.Text + "\\" + listView1.SelectedItems[0].SubItems[0].Text))
                               {
                                   TreeNode node = FindNode(listView1.SelectedItems[0].SubItems[0].Text);
                                   node.Nodes.Add("XXX");
                               }
                           }
                           else
                           {
                               MessageBox.Show("폴더에만 붙여넣을 수 있습니다.");
                               return;
                           }
                           if (!isCopy) ListViewSetting(path.Text);
                       };
                       ctx.Items.Add(menu5);
                       */
                    #endregion

                    ctx.Items.Add("-");

                    // TODO: 삭제 컨펌 창
                    var menu6 = new ToolStripMenuItem("삭제");
                    menu6.Click += (o, s) =>
                    {
                        if (MessageBox.Show("삭제 하시겠습니까?", "경고", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            string strSel;
                            string strSize;
                            string strAttribute;

                            strSel = listView1.SelectedItems[0].SubItems[0].Text;
                            strSize = listView1.SelectedItems[0].SubItems[1].Text;
                            strAttribute = listView1.SelectedItems[0].SubItems[5].Text;
                            
                            // TODO : ReadOnly 제거
                            if (strSize.Equals("")) // 폴더일 경우
                            {
                                try
                                {
                                    FileSystem.DeleteDirectory(path.Text + "\\" + item.SubItems[0].Text, UIOption.AllDialogs, RecycleOption.SendToRecycleBin, UICancelOption.DoNothing);
                                }
                                catch
                                {
                                    MessageBox.Show("Error: 권한이 없습니다.");
                                }
                            }
                            else // 파일일 경우
                            {
                                try
                                {
                                    FileSystem.DeleteFile(path.Text + "\\" + item.SubItems[0].Text, UIOption.AllDialogs, RecycleOption.SendToRecycleBin, UICancelOption.DoNothing);
                                }
                                catch
                                {
                                    MessageBox.Show("Error: 권한이 없습니다.");
                                }
                            }
                            ListViewSetting(path.Text);
                            treeView1.SelectedNode.Collapse();
                            treeView1.SelectedNode.Expand();
                        }
                    };
                    ctx.Items.Add(menu6);

                    var menu7 = new ToolStripMenuItem("이름 바꾸기");
                    menu7.Click += (o, s) =>
                    {
                        tbx = new TextBox();
                        tbx.Text = item.SubItems[0].Text;
                        tbx.Font = new Font("굴림", 9f);
                        tbx.Size = new Size(200, 21);
                        tbx.Location = new Point(item.Position.X + 17, item.Position.Y - 2);
                        listView1.Controls.Add(tbx);
                        tbx.Focus();
                        tbx.LostFocus += tbx_LostFocus;
                        tbx.KeyUp += tbx_KeyUp;
                    };
                    ctx.Items.Add(menu7);

                    ctx.Items.Add("-");

                    var menu8 = new ToolStripMenuItem("속성");
                    menu8.Click += (o, s) =>
                    {
                        ShowPropertiesDialog.ShowFileProperties(path.Text + "\\" + item.SubItems[0].Text);
                    };
                    ctx.Items.Add(menu8);

                    ctx.Items.Add("-");

                    #region 레지스트리를 이용한 확장자에 따른 추가 메뉴
                    bool isDirectory = item.SubItems[1].Text.Equals("");
                    var extstr = "Directory";

                    RegistryKey rKey = Registry.ClassesRoot.OpenSubKey(isDirectory ? "Directory" : "." + item.SubItems[2].Text);
                    if (rKey != null)
                    {
                        if (!isDirectory)
                        {
                            extstr = rKey.GetValue("") as string;
                        }

                        rKey.Close();
                        if (extstr != null)
                        {
                            var subKey = Registry.ClassesRoot.OpenSubKey(extstr);
                            if (subKey != null)
                            {
                                var shellKey = subKey.OpenSubKey("shell");
                                subKey.Close();
                                if (shellKey != null)
                                {
                                    var subKeys = shellKey.GetSubKeyNames();
                                    if (subKeys != null)
                                    {
                                        foreach (string key in subKeys)
                                        {
                                            var menuRegistry = new ToolStripMenuItem();
                                            menuRegistry.Text = key;

                                            if (key == "find" || key == "AnyCode" || key == "printto" || key == "Hwp.Print" || key == "printTo") continue;


                                            #region 아이콘 추출
                                            RegistryKey icon = shellKey.OpenSubKey(key);
                                            if (icon.GetValue("icon") != null)
                                            {
                                                FileInfo iconFile = new FileInfo(icon.GetValue("icon").ToString());

                                                if (iconFile.Exists)
                                                {
                                                    Icon subIcon = GetSystemImg.GetIcon(iconFile.FullName, false);
                                                    menuRegistry.Image = subIcon.ToBitmap();
                                                }
                                            }

                                            RegistryKey cmd = icon.OpenSubKey("command");
                                            string registryPath = null;
                                            if (cmd != null)
                                            {
                                                try
                                                {
                                                    registryPath = cmd.GetValue("").ToString();
                                                }
                                                catch
                                                {
                                                }
                                                cmd.Close();
                                            }
                                            #endregion

                                            menuRegistry.Click += (o, s) =>
                                            {
                                                if (registryPath != null)
                                                {
                                                    string[] parsedPath;
                                                    parsedPath = RegistryParsing(registryPath, (path.Text + "\\" + item.SubItems[0].Text));
                                                    try
                                                    {
                                                        Process.Start('\"' + parsedPath[0] + '\"', parsedPath[1]);
                                                    }
                                                    catch
                                                    {
                                                        try
                                                        {
                                                            Process.Start(parsedPath[0]);
                                                        }
                                                        catch
                                                        {
                                                        }
                                                    }
                                                }
                                            };

                                            if (registryPath != null)
                                            {
                                                ctx.Items.Add(menuRegistry);
                                            }
                                            icon.Close();
                                        }
                                        shellKey.Close();
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    ctx.Show(this, new Point(e.X + ((Control)sender).Left, e.Y + ((Control)sender).Top));
                }
            }
                
        }

        // 리스트뷰 여백 우클릭 메뉴
        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (_isLoading == true) return;
            if (e.Button == MouseButtons.Right)
            {
                if (listView1.GetItemAt(e.X, e.Y) == null)
                {
                    var ctx = new ContextMenu();

                    var menu1 = new MenuItem("새 폴더 만들기");
                    menu1.Click += (o, s) =>
                    {
                        createDir.PerformClick();
                    };
                    ctx.MenuItems.Add(menu1);

                    ctx.MenuItems.Add("-");

                    var menu2 = new MenuItem("보기");
                    var subMenu1 = new MenuItem("아이콘");
                    subMenu1.Click += (o, s) =>
                    {
                        menuLargeIcon.PerformClick();
                    };
                    var subMenu2 = new MenuItem("자세히");
                    subMenu2.Click += (o, s) =>
                    {
                        menuDetail.PerformClick();
                    };
                    var subMenu3 = new MenuItem("간단히");
                    subMenu3.Click += (o, s) =>
                    {
                        menuList.PerformClick();
                    };
                    menu2.MenuItems.Add(subMenu1);
                    menu2.MenuItems.Add(subMenu2);
                    menu2.MenuItems.Add(subMenu3);
                    ctx.MenuItems.Add(menu2);

                    ctx.MenuItems.Add("-");

                    var menu3 = new MenuItem("정렬");
                    var subMenu4 = new MenuItem("이름");
                    subMenu4.Click += (o, s) =>
                    {
                        if (_isLoading == true) return;

                        //정렬을 위하여 사용 됨.
                        if (listView1.Sorting == SortOrder.Ascending)
                            listView1.Sorting = SortOrder.Descending;
                        else
                            listView1.Sorting = SortOrder.Ascending;


                        listView1.ListViewItemSorter = new ListViewItemComparer(0, listView1.Sorting);
                    };
                    var subMenu5 = new MenuItem("크기");
                    subMenu5.Click += (o, s) =>
                    {
                        if (_isLoading == true) return;

                        //정렬을 위하여 사용 됨.
                        if (listView1.Sorting == SortOrder.Ascending)
                            listView1.Sorting = SortOrder.Descending;
                        else
                            listView1.Sorting = SortOrder.Ascending;


                        listView1.ListViewItemSorter = new ListViewItemComparer(4, listView1.Sorting);
                    };
                    var subMenu6 = new MenuItem("유형");
                    subMenu6.Click += (o, s) =>
                    {
                        if (_isLoading == true) return;

                        //정렬을 위하여 사용 됨.
                        if (listView1.Sorting == SortOrder.Ascending)
                            listView1.Sorting = SortOrder.Descending;
                        else
                            listView1.Sorting = SortOrder.Ascending;


                        listView1.ListViewItemSorter = new ListViewItemComparer(2, listView1.Sorting);
                    };
                    var subMenu7 = new MenuItem("수정된 날짜");
                    subMenu7.Click += (o, s) =>
                    {
                        if (_isLoading == true) return;

                        //정렬을 위하여 사용 됨.
                        if (listView1.Sorting == SortOrder.Ascending)
                            listView1.Sorting = SortOrder.Descending;
                        else
                            listView1.Sorting = SortOrder.Ascending;


                        listView1.ListViewItemSorter = new ListViewItemComparer(3, listView1.Sorting);
                    };
                    menu3.MenuItems.Add(subMenu4);
                    menu3.MenuItems.Add(subMenu5);
                    menu3.MenuItems.Add(subMenu6);
                    menu3.MenuItems.Add(subMenu7);
                    ctx.MenuItems.Add(menu3);

                    ctx.MenuItems.Add("-");

                    var menu5 = new MenuItem("붙여넣기");
                    menu5.Click += (o, s) =>
                    {
                        if (copyFile == null)
                        {
                            MessageBox.Show("ERROR: 복사된 파일/폴더가 없습니다.");
                            return;
                        }

                        string path1 = copyFile;
                        string path2 = path.Text + "\\" + copyName;

                        if (copySize.Equals("")) // 폴더일 경우
                        {
                            if (copyAttribute.IndexOf("ReadOnly") != -1 || copyAttribute.IndexOf("ReadOnly") != -1)
                            {
                                MessageBox.Show("Error: ReadOnly 폴더입니다.");
                                return;
                            }

                            if (Directory.Exists(path2))
                            {
                                int num = 1;
                                string name;
                                for (; ; )
                                {
                                    path2 = path.Text + "\\overlap" + num + "_" + copyName;
                                    if (Directory.Exists(path2))
                                    {
                                        num++;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (isCopy)
                                            {
                                                FileSystem.CopyDirectory(path1, path2, UIOption.AllDialogs, UICancelOption.DoNothing);
                                            }
                                            else
                                            {
                                                FileSystem.MoveDirectory(path1, path2, UIOption.AllDialogs, UICancelOption.DoNothing);
                                            }
                                        }
                                        catch
                                        {
                                            return;
                                        }

                                        try
                                        {
                                            name = treeView1.SelectedNode.Text;
                                            treeView1.SelectedNode.Parent.Collapse();
                                            treeView1.SelectedNode.Expand();
                                            treeView1.SelectedNode = FindNode(name);
                                            treeView1.SelectedNode.Expand();
                                        }
                                        catch
                                        {
                                            ListViewSetting(path.Text);
                                            treeView1.SelectedNode.Collapse();
                                            treeView1.SelectedNode.Expand();
                                        }

                                        return;
                                    }
                                }
                            }
                            try
                            {
                                string name;

                                if (isCopy)
                                {
                                    FileSystem.CopyDirectory(path1, path2, UIOption.AllDialogs, UICancelOption.DoNothing);
                                    //if (HasSubDirectory(path.Text))
                                    //{
                                    //    TreeNode node = treeView1.SelectedNode;
                                    //    node.Nodes.Add("XXX");
                                    //}
                                }
                                else
                                {
                                    FileSystem.MoveDirectory(path1, path2, UIOption.AllDialogs, UICancelOption.DoNothing);
                                }

                                try
                                {
                                    name = treeView1.SelectedNode.Text;
                                    treeView1.SelectedNode.Parent.Collapse();
                                    treeView1.SelectedNode.Expand();
                                    treeView1.SelectedNode = FindNode(name);
                                    treeView1.SelectedNode.Expand();
                                }
                                catch
                                {
                                    ListViewSetting(path.Text);
                                    treeView1.SelectedNode.Collapse();
                                    treeView1.SelectedNode.Expand();
                                }
                            }
                            catch
                            {
                                MessageBox.Show("ERROR: 대상 폴더가 원본 폴더의 하위 폴더입니다.");
                            }
                        }
                        else // 파일일 경우
                        {
                            if (copyAttribute.IndexOf("ReadOnly") != -1 || copyAttribute.IndexOf("ReadOnly") != -1)
                            {
                                MessageBox.Show("Error: ReadOnly 파일입니다.");
                                return;
                            }

                            if (File.Exists(path2))
                            {
                                int num = 1;

                                for (; ; )
                                {
                                    path2 = path.Text + "\\overlap" + num + "_" + copyName;
                                    if (File.Exists(path2))
                                    {
                                        num++;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (isCopy)
                                            {
                                                FileSystem.CopyFile(path1, path2, UIOption.AllDialogs, UICancelOption.DoNothing);
                                            }
                                            else
                                            {
                                                FileSystem.MoveFile(path1, path2, UIOption.AllDialogs, UICancelOption.DoNothing);
                                            }
                                        }
                                        catch
                                        {
                                            return;
                                        }

                                        ListViewSetting(path.Text);
                                        treeView1.SelectedNode.Collapse();
                                        treeView1.SelectedNode.Expand();

                                        return;
                                    }
                                }
                            }
                            if (isCopy)
                            {
                                FileSystem.CopyFile(path1, path2, UIOption.AllDialogs, UICancelOption.DoNothing);
                            }
                            else
                            {
                                FileSystem.MoveFile(path1, path2, UIOption.AllDialogs, UICancelOption.DoNothing);
                            }
                        }
                        ListViewSetting(path.Text);
                    };
                    ctx.MenuItems.Add(menu5);

                    ctx.Show(this, new Point(e.X + ((Control)sender).Left, e.Y + ((Control)sender).Top));
                }
            }
        }

        // 이름 바꾸기 enter키 입력 이벤트
        private void tbx_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbx.Visible = false;
            }
        }

        // 이름 바꾸기 포커스 상실 이벤트
        private void tbx_LostFocus(object sender, EventArgs e)
        {
            string strSize = listView1.SelectedItems[0].SubItems[1].Text;
            string path1 = path.Text + "\\" + listView1.SelectedItems[0].SubItems[0].Text;
            string path2 = path.Text + "\\" + tbx.Text;

            if (strSize.Equals("")) // 폴더일 경우
            {
                if (Directory.Exists(path2))
                {
                    //MessageBox.Show("중복된 폴더명 입니다.");
                    tbx.Dispose();
                    return;
                }
                Directory.Move(path1, path2);
            }
            else // 파일일 경우
            {
                if (File.Exists(path2))
                {
                    //MessageBox.Show("중복된 파일명 입니다.");
                    tbx.Dispose();
                    return;
                }
                File.Move(path1, path2);
            }

            ListViewSetting(path.Text);
            treeView1.SelectedNode.Collapse();
            treeView1.SelectedNode.Expand();

            tbx.Dispose();
        }
        #endregion

        #region ListView Method
        // 초기화
        private void InitImage()
        {
            imgSmall.Images.Clear();
            imgLarge.Images.Clear();
            listView1.Items.Clear();
        }

        // 폴더, 파일 렌더링
        private void ListViewSetting(string strPath)
        {
            if (_isLoading == true) return;

            itemNum = 0;
            
            InitImage();

            strPath = strPath + "\\";

            DirectoryInfo directoryInfo = new DirectoryInfo(strPath);


            // 빈 폴더일 경우
            try
            {
                if (directoryInfo.GetDirectories().Length == 0 && directoryInfo.GetFiles().Length == 0)
                {
                    listView1.Items.Add("빈 폴더");
                    return;
                }
            }
            catch
            {
                listView1.Items.Add("빈 폴더");
                return;
            }

            try
            {
                _isLoading = true; // 목록 로딩 중 다른 동작 제한
                treeView1.BeginUpdate(); // 업데이트가 끝날 때까지 UI 갱신 중지
                listView1.BeginUpdate();
                trackBar.Enabled = false;
                this.KeyPreview = false;

                int count = directoryInfo.GetDirectories().Length + directoryInfo.GetFiles().Length;

                SetLoadingBar(count);

                // 폴더 정보를 얻기
                foreach (DirectoryInfo subdirectoryInfo in directoryInfo.GetDirectories())
                {
                    if (subdirectoryInfo.Name == "Fonts" || subdirectoryInfo.Name == "assembly")
                    {
                        Loading();
                        continue; // Fonts폴더를 열 수 없어 우선 배제처리
                    }

                    if ((subdirectoryInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    {
                        Loading();
                        continue; // 숨긴폴더 사전제거
                    }

                    // 리스트뷰에 입력
                    if (listView1.View == View.LargeIcon)
                    {
                        imgLarge.Images.Add(GetSystemImg.GetIcon(subdirectoryInfo.FullName, true));
                        count = imgLarge.Images.Count;
                    }
                    else
                    {
                        imgSmall.Images.Add(GetSystemImg.GetIcon(subdirectoryInfo.FullName, false));
                        count = imgSmall.Images.Count;
                    }

                    listView1.Items.Add(subdirectoryInfo.Name, count - 1);
                    listView1.Items[count - 1].SubItems.Add("");
                    listView1.Items[count - 1].SubItems.Add("파일 폴더");
                    listView1.Items[count - 1].SubItems.Add(subdirectoryInfo.LastWriteTime.ToString());
                    listView1.Items[count - 1].SubItems.Add(((long)0).ToString());
                    listView1.Items[count - 1].SubItems.Add(subdirectoryInfo.Attributes.ToString());
                    itemNum++;

                    Application.DoEvents();
                    Loading();

                }

                // 파일 정보(리스트뷰 입력)
                foreach (FileInfo fileInfo in directoryInfo.GetFiles())
                {
                    if ((fileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    {
                        Loading();
                        continue; // 숨긴폴더 사전제거
                    }

                    // 리스트뷰에 입력
                    if (listView1.View == View.LargeIcon)
                    {
                        imgLarge.Images.Add(GetSystemImg.GetIcon(fileInfo.FullName, true));
                        count = imgLarge.Images.Count;
                    }
                    else
                    {
                        imgSmall.Images.Add(GetSystemImg.GetIcon(fileInfo.FullName, false));
                        count = imgSmall.Images.Count;
                    }

                    listView1.Items.Add(fileInfo.Name, count - 1);
                    if (fileInfo.Length > 1024 * 1024 * 1024)
                        listView1.Items[count - 1].SubItems.Add(string.Format("{0}GB", fileInfo.Length / 1024 / 1024 / 1024));
                    else if (fileInfo.Length > 1024 * 1024)
                        listView1.Items[count - 1].SubItems.Add(string.Format("{0}MB", fileInfo.Length / 1024 / 1024));
                    else if (fileInfo.Length > 1024)
                        listView1.Items[count - 1].SubItems.Add(string.Format("{0}KB", fileInfo.Length / 1024));
                    else
                        listView1.Items[count - 1].SubItems.Add(string.Format("{0}BYTE", fileInfo.Length));
                    try
                    {
                        listView1.Items[count - 1].SubItems.Add(fileInfo.Extension.Substring(1));
                    }
                    catch
                    {
                        listView1.Items[count - 1].SubItems.Add("file");
                    }
                    listView1.Items[count - 1].SubItems.Add(fileInfo.LastWriteTime.ToString());
                    listView1.Items[count - 1].SubItems.Add(fileInfo.Length.ToString());
                    listView1.Items[count - 1].SubItems.Add(fileInfo.Attributes.ToString());
                    itemNum++;

                    Application.DoEvents();
                    Loading();
                }
                status_txt.Text = itemNum.ToString() + "개 항목";

                _isLoading = false;
                listView1.EndUpdate(); // Refresh하여 보여줌
                treeView1.EndUpdate();
                this.KeyPreview = true;
            }
            catch
            {
                // 디렉토리/파일 정보 읽을때 예외 발생하면 무시(파일 속성때문에 예외 발생함)
            }
        }

        // 레지스트리 경로 파싱
        private string[] RegistryParsing(string deliveryPath, string itemPath)
        {
            char[] st = " ".ToCharArray();
            string[] parsedPath;

            deliveryPath = deliveryPath.Replace("\"", "");
            deliveryPath = deliveryPath.Replace("\'", "");
            deliveryPath = deliveryPath.Replace("%1", '\"' + itemPath + '\"');
            deliveryPath = deliveryPath.Replace("%V", '\"' + itemPath + '\"');
            deliveryPath = deliveryPath.Replace("Program Files", "ProgramFiles");
            deliveryPath = deliveryPath.Replace("ProgramFiles (x86)", "ProgramFiles(x86)");
            deliveryPath = deliveryPath.Replace("Common Files", "CommonFiles");
            deliveryPath = deliveryPath.Replace("Microsoft Shared", "MicrosoftShared");
            parsedPath = deliveryPath.Split(st, 2);
            for (int i = 0; i < parsedPath.Length; i++)
            {
                parsedPath[i] = parsedPath[i].Replace("ProgramFiles", "Program Files");
                parsedPath[i] = parsedPath[i].Replace("Program Files(x86)", "Program Files (x86)");
                parsedPath[i] = parsedPath[i].Replace("CommonFiles", "Common Files");
                parsedPath[i] = parsedPath[i].Replace("MicrosoftShared", "Microsoft Shared");
            }
            return parsedPath;
        }

        //아이콘 크기조절
        private void trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (listView1.View != View.LargeIcon)
            {
                MessageBox.Show("보기-아이콘에서 이용가능합니다.");
                trackBar.Value = 60;
                return;
            }

            if (trackBar.Value >= 45)
            {
                size.Width = 45 + (trackBar.Value - 45);
                size.Height = 45 + (trackBar.Value - 45);
            }
            else if (trackBar.Value < 45)
            {
                size.Width = 45 - (45 - trackBar.Value);
                size.Height = 45 - (45 - trackBar.Value);
            }

            if (imgLarge.ImageSize == size) return;

            else if (imgLarge.ImageSize != size)
            {
                imgLarge.ImageSize = size;

                listView1.LargeImageList = imgLarge;
                ListViewSetting(path.Text);
            }
        }
        #endregion

        #region TopBar Event
        private void menuLargeIcon_Click(object sender, EventArgs e)
        {
            InitMenuCheck();
            listView1.View = View.LargeIcon;
            menuLargeIcon.Checked = true;
            label1.Visible = true;
            trackBar.Visible = true;
            ListViewSetting(path.Text);
        }

        private void menuDetail_Click(object sender, EventArgs e)
        {
            InitMenuCheck();
            listView1.View = View.Details;
            menuDetail.Checked = true;
            label1.Visible = false;
            trackBar.Visible = false;
            ListViewSetting(path.Text);
        }

        private void menuList_Click(object sender, EventArgs e)
        {
            InitMenuCheck();
            listView1.View = View.List;
            menuList.Checked = true;
            label1.Visible = false;
            trackBar.Visible = false;
            ListViewSetting(path.Text);
        }

        // 종료
        private void menuClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        // 뒤로 가기
        private void button2_Click(object sender, EventArgs e)
        {
            if (_isLoading == true) return;
            if(path.Text.IndexOf("\u005C")==-1)
            {
                //MessageBox.Show("ERROR: 최상위 폴더입니다.");
                return;
            }
            else
            {
                try
                {
                    forwardStack.Push(treeView1.SelectedNode.Text);
                    TreeNode parentNode = treeView1.SelectedNode.Parent;
                    treeView1.SelectedNode = parentNode;
                    treeView1.SelectedNode.Collapse();
                }
                catch
                {
                }
            }
        }

        // 앞으로 가기
        private void button3_Click(object sender, EventArgs e)
        {
            if (_isLoading == true) return;
            try
            {
                treeView1.SelectedNode.Collapse();
                treeView1.SelectedNode.Expand();
                treeView1.SelectedNode = FindNode(forwardStack.Pop());
            }
            catch
            {
                treeView1.SelectedNode.Collapse();
            }
        }

        // 경로복사
        private void button1_Click(object sender, EventArgs e)
        {
            if (_isLoading == true) return;
            Clipboard.SetText(path.Text);
        }

        // 새로고침
        private void refresh_Click(object sender, EventArgs e)
        {
            if (_isLoading == false)
            {
                ListViewSetting(path.Text);
            }
        }

        // 새 폴더 생성
        private void createDir_Click(object sender, EventArgs e)
        {
            if (_isLoading == true) return;
            int num = 1;
            string name;

            for (; ; )
            {
                if (Directory.Exists(path.Text + "\\새 폴더" + num))
                {
                    num++;
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(path.Text + "\\새 폴더" + num);
                    }
                    catch
                    {
                        return;
                    }

                    try
                    {
                        name = treeView1.SelectedNode.Text;
                        treeView1.SelectedNode.Parent.Collapse();
                        treeView1.SelectedNode.Expand();
                        treeView1.SelectedNode = FindNode(name);
                        treeView1.SelectedNode.Expand();
                    }
                    catch
                    {
                        ListViewSetting(path.Text);
                        treeView1.SelectedNode.Collapse();
                        treeView1.SelectedNode.Expand();
                    }

                    break;
                }
            }
        }

        // 트레이모드 변경
        private void trayMode_Click(object sender, EventArgs e)
        {
            this.Hide();
            notify.Visible = true;
            this.notify.BalloonTipTitle = "트레이 모드";
            this.notify.BalloonTipText = "창모드로 이동하시려면 더블 클릭하세요.";
            this.notify.ShowBalloonTip(2000);
        }

        // 도움말 클릭 시
        private void helpbt_Click(object sender, EventArgs e)
        {
            MessageBox.Show("               -단축키-\n\n  도움말             :  F1\n  새로고침          :  F5\n  트레이 모드      :  Ctrl+T\n  새 폴더 만들기  :  Ctrl+W\n  아이콘 보기      :  Ctrl+I\n  자세히 보기      :  Ctrl+D\n  간단히 보기      :  Ctrl+L\n  종료                :  Ctrl+x");
        }

        private void help_Click(object sender, EventArgs e)
        {
            helpbt.PerformClick();
        }
        #endregion

        #region TopBar Method
        private void InitMenuCheck()
        {
            menuLargeIcon.Checked = false;
            menuList.Checked = false;
            menuDetail.Checked = false;
        }
        #endregion

        #region LoadingBar Method
        // 로딩 최대치 설정
        private void SetLoadingBar(int count)
        {
            loadingBar.Maximum = count;
            loadingBar.Value = 0;
        }

        // 로딩 게이지 증가
        private void Loading()
        {
            if (loadingBar.Value < loadingBar.Maximum)
            {
                loadingBar.Value = loadingBar.Value + 1;
                if (loadingBar.Maximum == loadingBar.Value)
                    trackBar.Enabled = true;
            }
        }
        #endregion

        #region  Drag and Drop Event
        // 드래그 시작
        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            //listView1.SelectedItems[0].BackColor = Color.FromArgb(10, 51, 153, 255);
            //listView1.SelectedItems[0].ForeColor = Color.White;

            listView1.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        // 드래그 도중
        private void listView1_DragOver(object sender, DragEventArgs e)
        {
            if(dragMove!=null)
            {
                dragMove.BackColor = Color.White;
                dragMove.ForeColor = Color.Black;
            }
            
            e.Effect = DragDropEffects.Move;
            Point p = listView1.PointToClient(MousePosition);
            ListViewItem item = listView1.GetItemAt(p.X, p.Y);

            if (item != null && item.Selected == false)
            {
                dragMove = item;
                dragMove.BackColor = Color.FromArgb(10, 51, 153, 255);
                dragMove.ForeColor = Color.White;
            }
        }

        // 항목을 끄는 도중
        private void listView1_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (e.Effect == DragDropEffects.Move)
            {
                e.UseDefaultCursors = false;

                Icon icon = GetSystemImg.GetIcon(path.Text + "\\" + listView1.SelectedItems[0].Text, true);
                listView1.Cursor = new Cursor(icon.Handle);
                GetSystemImg.DestroyIcon(icon.Handle);
            }
            else e.UseDefaultCursors = true;
        }

        //드래그 도중 이탈
        private void listView1_DragLeave(object sender, EventArgs e)
        {
            if (dragMove != null)
            {
                dragMove.BackColor = Color.White;
                dragMove.ForeColor = Color.Black;
            }
        }

        // 드래그 종료
        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }
            if (dragMove != null)
            {
                dragMove.BackColor = Color.White;
                dragMove.ForeColor = Color.Black;
            }

            Point p = listView1.PointToClient(MousePosition);
            ListViewItem item = listView1.GetItemAt(p.X, p.Y);

            // 복사
            if (Control.ModifierKeys == Keys.Control)
            {
                string path1;
                string path2;
                if (item == null)
                {
                    path1 = path.Text + "\\" + listView1.SelectedItems[0].SubItems[0].Text;
                    path2 = path.Text + "\\" + "overlap_" + listView1.SelectedItems[0].SubItems[0].Text;
                }
                else
                {
                    path1 = path.Text + "\\" + listView1.SelectedItems[0].SubItems[0].Text;
                    path2 = path.Text + "\\" + item.SubItems[0].Text + "\\" + listView1.SelectedItems[0].SubItems[0].Text;
                }

                if (listView1.SelectedItems[0].SubItems[1].Text.Equals("")) // 폴더일 경우
                {
                    if (Directory.Exists(path2))
                    {
                        MessageBox.Show("Error: 같은 이름의 폴더가 존재합니다.");
                        return;
                    }
                    try
                    {
                        FileSystem.CopyDirectory(path1, path2, UIOption.AllDialogs, UICancelOption.DoNothing);
                        ListViewSetting(path.Text);
                        treeView1.SelectedNode.Collapse();
                        treeView1.SelectedNode.Expand();
                    }
                    catch
                    {
                        MessageBox.Show("Error: 폴더를 복사할 수 없습니다.");
                    }
                }
                else // 파일일 경우
                {
                    if (File.Exists(path2))
                    {
                        MessageBox.Show("Error: 같은 이름의 파일이 존재합니다.");
                        return;
                    }
                    try
                    {
                        FileSystem.CopyFile(path1, path2, UIOption.AllDialogs, UICancelOption.DoNothing);
                        ListViewSetting(path.Text);
                        treeView1.SelectedNode.Collapse();
                        treeView1.SelectedNode.Expand();
                    }
                    catch
                    {
                        MessageBox.Show("Error: 파일을 복사할 수 없습니다.");
                    }
                }
            }

            ////  바로가기 생성
            //else if (Control.ModifierKeys == Keys.Alt)
            //{
            //    IWshRuntimeLibrary.WshShell wsh = new IWshRuntimeLibrary.WshShell();
            //    IWshRuntimeLibrary.IWshShortcut myShotCut = (IWshRuntimeLibrary.IWshShortcut)wsh.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/test.lnk");
            //    myShotCut.TargetPath = @"C:\Users\Administrator\Desktop\\Explorer";
            //    myShotCut.IconLocation = @"C:\Users\Administrator\Desktop\\foldericon.ico";
            //    myShotCut.Save();

            //    ListViewSetting(path.Text);
            //    treeView1.SelectedNode.Collapse();
            //    treeView1.SelectedNode.Expand();

            //    //string path1;
            //    //string path2;
            //    //if (item == null)
            //    //{
            //    //    path1 = path.Text + "\\" + listView1.SelectedItems[0].SubItems[0].Text;
            //    //    path2 = path.Text + "\\" + "overlap_" + listView1.SelectedItems[0].SubItems[0].Text;
            //    //}
            //    //else
            //    //{
            //    //    path1 = path.Text + "\\" + listView1.SelectedItems[0].SubItems[0].Text;
            //    //    path2 = path.Text + "\\" + item.SubItems[0].Text + "\\" + listView1.SelectedItems[0].SubItems[0].Text;
            //    //}

            //    //if (listView1.SelectedItems[0].SubItems[1].Text.Equals("")) // 폴더일 경우
            //    //{
            //    //    if (Directory.Exists(path2))
            //    //    {
            //    //        MessageBox.Show("Error: 같은 이름의 폴더가 존재합니다.");
            //    //        return;
            //    //    }
            //    //    try
            //    //    {
            //    //        FileSystem.CopyDirectory(path1, path2, UIOption.AllDialogs, UICancelOption.DoNothing);
            //    //        ListViewSetting(path.Text);
            //    //        treeView1.SelectedNode.Collapse();
            //    //        treeView1.SelectedNode.Expand();
            //    //    }
            //    //    catch
            //    //    {
            //    //        MessageBox.Show("Error: 폴더를 복사할 수 없습니다.");
            //    //    }
            //    //}
            //    //else // 파일일 경우
            //    //{
            //    //    if (File.Exists(path2))
            //    //    {
            //    //        MessageBox.Show("Error: 같은 이름의 파일이 존재합니다.");
            //    //        return;
            //    //    }
            //    //    try
            //    //    {
            //    //        FileSystem.CopyFile(path1, path2, UIOption.AllDialogs, UICancelOption.DoNothing);
            //    //        ListViewSetting(path.Text);
            //    //        treeView1.SelectedNode.Collapse();
            //    //        treeView1.SelectedNode.Expand();
            //    //    }
            //    //    catch
            //    //    {
            //    //        MessageBox.Show("Error: 파일을 복사할 수 없습니다.");
            //    //    }
            //    //}
            //}

            // 이동
            else
            {
                if (item == null || !(item.SubItems[1].Text.Equals("")) || listView1.SelectedItems[0].SubItems[0].Text.Equals(item.SubItems[0].Text))
                {
                    return;
                }
                else
                {
                    string path1 = path.Text + "\\" + listView1.SelectedItems[0].SubItems[0].Text;
                    string path2 = path.Text + "\\" + item.SubItems[0].Text + "\\" + listView1.SelectedItems[0].SubItems[0].Text;

                    if (listView1.SelectedItems[0].SubItems[1].Text.Equals("")) // 폴더일 경우
                    {
                        if (Directory.Exists(path2))
                        {
                            MessageBox.Show("Error: 같은 이름의 폴더가 존재합니다.");
                            return;
                        }
                        try
                        {
                            FileSystem.MoveDirectory(path1, path2, UIOption.AllDialogs, UICancelOption.DoNothing);
                            ListViewSetting(path.Text);
                            treeView1.SelectedNode.Collapse();
                            treeView1.SelectedNode.Expand();
                        }
                        catch
                        {
                            MessageBox.Show("Error: 폴더를 이동할 수 없습니다.");
                        }
                    }
                    else // 파일일 경우
                    {
                        if (File.Exists(path2))
                        {
                            MessageBox.Show("Error: 같은 이름의 파일이 존재합니다.");
                            return;
                        }
                        try
                        {
                            FileSystem.MoveFile(path1, path2, UIOption.AllDialogs, UICancelOption.DoNothing);
                            ListViewSetting(path.Text);
                            treeView1.SelectedNode.Collapse();
                            treeView1.SelectedNode.Expand();
                        }
                        catch
                        {
                            MessageBox.Show("Error: 파일을 이동할 수 없습니다.");
                        }
                    }
                }
            }
            
        }
        #endregion

        #region TrayMode Method
        private void RenderTray()
        {
            notify = new NotifyIcon();

            var menu = new ContextMenu();

            var menuItem1 = new MenuItem();
            menuItem1.Text = "정보";
            menuItem1.Click += (o, s) =>
            {
                MessageBox.Show("Explorer");
            };

            var menuItem2 = new MenuItem();
            menuItem2.Text = "창모드";
            menuItem2.Click += (o, s) =>
            {
                notify.Visible = false;
                this.Show();
            };

            var menuItem3 = new MenuItem();
            menuItem3.Text = "종료";
            menuItem3.Click += (o, s) =>
            {
                this.Close();
            };

            menu.MenuItems.Add(menuItem1);
            menu.MenuItems.Add(menuItem2);
            menu.MenuItems.Add(menuItem3);

            notify.Icon = Properties.Resources.foldericon;
            notify.ContextMenu = menu;
            notify.DoubleClick += (o, s) =>
            {
                this.Show();
                notify.Visible = false;
            };
        }
        #endregion

        #region favoriteView Event
        private void favoriteView_DoubleClick(object sender, EventArgs e)
        {
            Point p = favoriteView.PointToClient(MousePosition);
            TreeNode selectedNode = favoriteView.GetNodeAt(p.X, p.Y);

            if (selectedNode.Text.Equals("바탕 화면"))
            {
                treeView1.Nodes[0].Collapse();
                treeView1.SelectedNode = treeView1.Nodes[0];
                treeView1.Nodes[0].Expand();
                treeView1.SelectedNode = FindNode("Users");
                treeView1.SelectedNode.Expand();
                treeView1.SelectedNode = FindNode(userName);
                treeView1.SelectedNode.Expand();
                treeView1.SelectedNode = FindNode("Desktop");
            }
            else if (selectedNode.Text.Equals("다운로드"))
            {
                treeView1.Nodes[0].Collapse();
                treeView1.SelectedNode = treeView1.Nodes[0];
                treeView1.Nodes[0].Expand();

                treeView1.SelectedNode = FindNode("Users");
                treeView1.SelectedNode.Expand();
                treeView1.SelectedNode = FindNode(userName);
                treeView1.SelectedNode.Expand();
                treeView1.SelectedNode = FindNode("Downloads");
            }
            else if (selectedNode.Text.Equals("문서"))
            {
                treeView1.Nodes[0].Collapse();
                treeView1.SelectedNode = treeView1.Nodes[0];
                treeView1.Nodes[0].Expand();

                treeView1.SelectedNode = FindNode("Users");
                treeView1.SelectedNode.Expand();
                treeView1.SelectedNode = FindNode(userName);
                treeView1.SelectedNode.Expand();
                treeView1.SelectedNode = FindNode("Documents");
            }
        }
        
        private void favoriteView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            favoriteView.SelectedNode.Expand();
            MessageBox.Show("이동할 즐겨찾기 항목을 선택하세요.");
        }
        #endregion
    }
}