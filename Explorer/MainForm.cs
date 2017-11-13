using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;

namespace Explorer
{
    public partial class MainForm : Form
    {
        private bool _isLoading = false;
        private int num = 1;
        private int itemnum = 0;
        private string copyFile;
        private string copyName;
        private string copySize;
        private bool isCopy;
        private NotifyIcon notify;
        TextBox tbx;
        Size size;

        public MainForm()
        {
            InitializeComponent();
        }

        // 폼 로드
        private void MainForm_Load(object sender, EventArgs e)
        {

            InitTreeDriveSetting();

            #region 원형아이콘
            var gp = new GraphicsPath();
            gp.AddEllipse(helpbt.DisplayRectangle);
            helpbt.Region = new Region(gp);
            #endregion

            toolTip1.SetToolTip(helpbt, "도움말");
            RenderTray();
        }

        #region 트레이 모드 관련
        private void RenderTray()
        {
            notify = new NotifyIcon();

            var menu = new ContextMenu();

            var menuItem1 = new MenuItem();
            menuItem1.Text = "정보";
            menuItem1.Click += (o, s) =>
            {
                MessageBox.Show("Windows Explorer making");
                //MessageBox.Show("© 2017 XS INC. ALL RIGHTS RESERVED.");
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

        // 트리뷰에 드라이브 정보 입력
        private void InitTreeDriveSetting()
        {
            int count;
            string[] strDrives = null;
            GetSystemImg img = new GetSystemImg();

            strDrives = Directory.GetLogicalDrives();
            imgTree.Images.Clear();

            foreach (string drive in strDrives)
            {
                imgTree.Images.Add(img.GetIcon(drive, false, false));

                count = treeView1.Nodes.Add(new TreeNode(drive.Substring(0, 2), imgTree.Images.Count - 1, imgTree.Images.Count - 1));

                if (HasSubDirectory(drive.Substring(0, 2)))
                {
                    // What?
                    treeView1.Nodes[count].Nodes.Add("XXX");
                }

                DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\");

                // What?
                foreach (DirectoryInfo subdirectoryInfo in directoryInfo.GetDirectories())
                {
                    imgTree.Images.Add(img.GetIcon(subdirectoryInfo.FullName, false, false));
                    imgTree.Images.Add(img.GetIcon(subdirectoryInfo.FullName, false, true));
                }

                // 초기화면 C드라이브 선택 효과
                treeView1.SelectedNode = treeView1.Nodes[0];
            }
        }

        // 하위 폴더가 있는지 검사한다.
        private bool HasSubDirectory(String strPath)
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

        // 초기화
        private void InitImage()
        {
            imgSmall.Images.Clear();
            imgLarge.Images.Clear();
            listView1.Items.Clear();
        }

        // 리스트뷰에 폴더및 파일정보 삽입
        private void ListViewSetting(String strPath)
        {
            if(_isLoading == true) return;

            itemnum = 0;

            InitImage();

            GetSystemImg img = new GetSystemImg();

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
                // 목록 로딩 중 다른 동작 제한
                _isLoading = true;
                treeView1.Visible = false;
                listView1.Visible = false; // 속도 향상의 핵심
                trackBar.Enabled = false;
                this.KeyPreview = false;

                int count = directoryInfo.GetDirectories().Length + directoryInfo.GetFiles().Length;

                SetLoadingBar(count);
                
                // 폴더 정보를 얻기
                foreach (DirectoryInfo subdirectoryInfo in directoryInfo.GetDirectories())
                {
                    if (subdirectoryInfo.Name == "Fonts")
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
                        imgLarge.Images.Add(img.GetIcon(subdirectoryInfo.FullName, true, false));
                        count = imgLarge.Images.Count;
                    }
                    else
                    {
                        imgSmall.Images.Add(img.GetIcon(subdirectoryInfo.FullName, false, false));
                        count = imgSmall.Images.Count;
                    }

                    listView1.Items.Add(subdirectoryInfo.Name, count - 1);
                    listView1.Items[count - 1].SubItems.Add("");
                    listView1.Items[count - 1].SubItems.Add("파일 폴더");
                    listView1.Items[count - 1].SubItems.Add(subdirectoryInfo.LastWriteTime.ToString());
                    listView1.Items[count - 1].SubItems.Add(((long)0).ToString());
                    itemnum++;

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
                        imgLarge.Images.Add(img.GetIcon(fileInfo.FullName, true, false));
                        count = imgLarge.Images.Count;
                    }
                    else
                    {
                        imgSmall.Images.Add(img.GetIcon(fileInfo.FullName, false, false));
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
                    itemnum++;

                    Application.DoEvents();
                    Loading();
                }
                status_txt.Text = itemnum.ToString() + "개 항목";
                
                _isLoading = false;
                treeView1.Visible = true;
                listView1.Visible = true;
                this.KeyPreview = true;
            }
            catch
            {
                // 디렉토리/파일 정보 읽을때 예외 발생하면 무시(파일 속성때문에 예외 발생함)
            }
        }

        // 트리 노드 선택 이벤트
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_isLoading == false)
            {
                path.Text = e.Node.FullPath;
                ListViewSetting(e.Node.FullPath);
            }
        }
        
        // node에 하위 폴더 삽입
        private void TreeNodeSetting(TreeNode node)
        {
            String strPath;
            int count;
            strPath = node.FullPath + "\\";

            DirectoryInfo directoryInfo = new DirectoryInfo(strPath);

            // 폴더 정보
            try
            {
                // 폴더 정보를 얻기
                foreach (DirectoryInfo subdirectoryInfo in directoryInfo.GetDirectories())
                {
                    if ((subdirectoryInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    {
                        continue; // 숨긴폴더 사전제거
                    }

                    count = node.Nodes.Add(new TreeNode(subdirectoryInfo.Name, imgTree.Images.Count - 2, imgTree.Images.Count - 1));
                    if (HasSubDirectory(node.Nodes[count].FullPath))
                    {
                        node.Nodes[count].Nodes.Add("XXX");
                    }
                }
            }
            catch
            {
                // 디렉토리/파일 정보 읽을때 예외 발생하면 무시(파일 속성때문에 예외 발생함)
            }

        }

        // 트리노드 확장 후 발생 이벤트
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
     
        // 트리 노드 찾기, 현재 선택된 노드의 자식들중에서 찾는다.
        private TreeNode FindNode(String strDirectory)
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

            String strSel;
            String strSize;

            try
            {
                strSel = listView1.SelectedItems[0].SubItems[0].Text;
                strSize = listView1.SelectedItems[0].SubItems[1].Text;
            }
            catch // 렌더링 중 이동불가 처리
            {
                return;
            }

            if (strSize.Equals("")) // 폴더일 경우 트리뷰에 경로 표시
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
            else
            {
                //FileExecute.ShellExecute(0, "open", path.Text + "\\" + strSel, null, null, (int)FileExecute.SW.SHOWNORMAL);
                try
                {
                    Process.Start(path.Text + "\\" + listView1.SelectedItems[0].SubItems[0].Text);
                }
                catch
                {
                    MessageBox.Show("연결된 프로그램이 없습니다.\n연결 프로그램 지정 후 실행하십시오.");
                }
            }
        }

        // 리스트뷰 우클릭 메뉴
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
                    var ctx = new ContextMenu();

                    var menu0 = new MenuItem("열기");
                    menu0.Click += (o, s) =>
                    {
                        #region 실행
                        String strSel;
                        String strSize;

                        strSel = listView1.SelectedItems[0].SubItems[0].Text;
                        strSize = listView1.SelectedItems[0].SubItems[1].Text;

                        if (strSize.Equals("")) // 폴더일 경우 트리뷰에 경로 표시
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
                        else
                        {
                            //FileExecute.ShellExecute(0, "open", path.Text + "\\" + strSel, null, null, (int)FileExecute.SW.SHOWNORMAL);
                            Process.Start(path.Text + "\\" + listView1.SelectedItems[0].SubItems[0].Text);
                        }
                        #endregion
                    };
                    ctx.MenuItems.Add(menu0);

                    ctx.MenuItems.Add("-");
                    
                    var menu3 = new MenuItem("잘라내기");
                    menu3.Click += (o, s) =>
                    {
                        isCopy = false;
                        copyFile = path.Text + "\\" + listView1.SelectedItems[0].SubItems[0].Text;
                        copyName = listView1.SelectedItems[0].SubItems[0].Text;
                        copySize = listView1.SelectedItems[0].SubItems[1].Text;
                    };
                    ctx.MenuItems.Add(menu3);

                    var menu4 = new MenuItem("복사");
                    menu4.Click += (o, s) =>
                    {
                        isCopy = true;
                        copyFile = path.Text + "\\" + listView1.SelectedItems[0].SubItems[0].Text;
                        copyName = listView1.SelectedItems[0].SubItems[0].Text;
                        copySize = listView1.SelectedItems[0].SubItems[1].Text;
                    };
                    ctx.MenuItems.Add(menu4);

                    var menu5 = new MenuItem("붙여넣기");
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
                    ctx.MenuItems.Add(menu5);

                    ctx.MenuItems.Add("-");

                    // TODO: 삭제 컨펌 창
                    var menu6 = new MenuItem("삭제");
                    menu6.Click += (o, s) =>
                    {
                        if (MessageBox.Show("삭제 하시겠습니까?", "경고", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            String strSel;
                            String strSize;

                            strSel = listView1.SelectedItems[0].SubItems[0].Text;
                            strSize = listView1.SelectedItems[0].SubItems[1].Text;

                            if (strSize.Equals("")) // 폴더일 경우
                            {
                                Directory.Delete(path.Text + "\\" + item.SubItems[0].Text, true);
                            }
                            else
                            {
                                File.Delete(path.Text + "\\" + item.SubItems[0].Text);
                            }
                            ListViewSetting(path.Text);
                        }
                    };
                    ctx.MenuItems.Add(menu6);

                    var menu7 = new MenuItem("이름 바꾸기");
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
                    ctx.MenuItems.Add(menu7);

                    ctx.MenuItems.Add("-");

                    var menu8 = new MenuItem("속성");
                    menu8.Click += (o, s) =>
                    {
                        ShowPropertiesDialog.ShowFileProperties(path.Text + "\\" + item.SubItems[0].Text);
                    };
                    ctx.MenuItems.Add(menu8);

                    ctx.MenuItems.Add("-");

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
                                            var menuRegistry = new MenuItem();
                                            menuRegistry.Text = key;
                                            ctx.MenuItems.Add(menuRegistry);
                                            menuRegistry.Click += (o, s) =>
                                            {
                                                MessageBox.Show("레지스트리 값 : " + key);
                                            };
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
                            MessageBox.Show("복사된 파일/폴더가 없습니다.");
                            return;
                        }

                        string path1 = copyFile;
                        string path2 = path.Text + "\\" + copyName;

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
                        if (HasSubDirectory(path.Text))
                        {
                            TreeNode node = treeView1.SelectedNode;
                            node.Nodes.Add("XXX");
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
                    MessageBox.Show("중복된 폴더명 입니다.");
                    tbx.Dispose();
                    return;
                }
                Directory.Move(path1, path2);
            }
            else
            {
                if (File.Exists(path2))
                {
                    MessageBox.Show("중복된 파일명 입니다.");
                    tbx.Dispose();
                    return;
                }
                File.Move(path1, path2);
            }
            
            ListViewSetting(path.Text);

            tbx.Dispose();
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

        #region 상단바
        private void InitMenuCheck()
        {
            menuLargeIcon.Checked = false;
            menuList.Checked = false;
            menuDetail.Checked = false;
        }

        private void menuLargeIcon_Click(object sender, EventArgs e)
        {
            InitMenuCheck();
            listView1.View = View.LargeIcon;
            menuLargeIcon.Checked = true;
            ListViewSetting(path.Text);
        }

        private void menuDetail_Click(object sender, EventArgs e)
        {
            InitMenuCheck();
            listView1.View = View.Details;
            menuDetail.Checked = true;
            ListViewSetting(path.Text);
        }

        private void menuList_Click(object sender, EventArgs e)
        {
            InitMenuCheck();
            listView1.View = View.List;
            menuList.Checked = true;
            ListViewSetting(path.Text);
        }

        private void menuClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // 위로가기
        private void button2_Click(object sender, EventArgs e)
        {
            if (_isLoading == true) return;
            if(path.Text.IndexOf("\u005C")==-1)
            { 
                MessageBox.Show("최상위 폴더입니다.");
                return;
            }
            else
            {
                TreeNode parentNode = treeView1.SelectedNode.Parent;
                treeView1.SelectedNode = parentNode;
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
                    ListViewSetting(path.Text);
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
            MessageBox.Show("               단축키\n\n  도움말 : F1\n  새로고침 : F5\n  트레이 모드 : Ctrl+T\n  새 폴더 만들기 : Ctrl+W\n  아이콘 보기 : Ctrl+I\n  자세히 보기 : Ctrl+D\n  간단히 보기 : Ctrl+L\n  종료 : Ctrl+x");
        }

        private void help_Click(object sender, EventArgs e)
        {
            helpbt.PerformClick();
        }
        #endregion

        #region 하단바
        // 로딩 최대치 설정
        private void SetLoadingBar(int count)
        {
            loadingBar.Maximum = count;
            loadingBar.Value = 0;
        }

        // 게이지 증가
        private void Loading()
        {
            if (loadingBar.Value < loadingBar.Maximum)
            {
                loadingBar.Value = loadingBar.Value + 1;
                if (loadingBar.Maximum == loadingBar.Value)
                    trackBar.Enabled = true;
            }
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
        
    }
}