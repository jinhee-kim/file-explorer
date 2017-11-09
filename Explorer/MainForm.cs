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

namespace Explorer
{
    public partial class MainForm : Form
    {
        private bool _isLoading = false;
        private int num = 0;
        TextBox tbx;

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

            toolTip1.SetToolTip(helpbt, "컬럼 클릭 시 정렬가능");
        }

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
            
            InitImage();

            GetSystemImg img = new GetSystemImg();

            strPath = strPath + "\\";

            DirectoryInfo directoryInfo = new DirectoryInfo(strPath);

            try
            {
                status_txt.Text = (directoryInfo.GetDirectories().Length + directoryInfo.GetFiles().Length).ToString() + "개 항목";
            }
            catch
            {
                status_txt.Text = strPath;
            }

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
                button1.UseWaitCursor = true;
                button2.UseWaitCursor = true;
                listView1.UseWaitCursor = true;
                treeView1.UseWaitCursor = true;
                treeView1.Enabled = false;
                //listView1.Enabled = false;

                int count = directoryInfo.GetDirectories("*").Length + directoryInfo.GetFiles("*.*").Length;

                SetLoadingBar(count);

                // 폴더 정보를 얻기
                foreach (DirectoryInfo subdirectoryInfo in directoryInfo.GetDirectories("*"))
                {
                    if(subdirectoryInfo.Name.Equals("assembly")) continue; // 에러발생 폴더 사전제거
                    
                    // 리스트뷰에 입력
                    imgSmall.Images.Add(img.GetIcon(subdirectoryInfo.FullName, false, false));
                    imgLarge.Images.Add(img.GetIcon(subdirectoryInfo.FullName, true, false));

                    count = imgSmall.Images.Count;

                    listView1.Items.Add(subdirectoryInfo.Name, count - 1);
                    listView1.Items[count - 1].SubItems.Add("");
                    listView1.Items[count - 1].SubItems.Add("파일 폴더");
                    listView1.Items[count - 1].SubItems.Add(subdirectoryInfo.LastWriteTime.ToString());
                    listView1.Items[count - 1].SubItems.Add(subdirectoryInfo.Attributes.ToString());

                    Application.DoEvents();
                    Loading();

                }

                // 파일 정보(리스트뷰 입력)

                foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.*"))
                {

                    imgSmall.Images.Add(img.GetIcon(fileInfo.FullName, false, false));
                    imgLarge.Images.Add(img.GetIcon(fileInfo.FullName, true, false));

                    count = imgSmall.Images.Count;

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

                    Application.DoEvents();
                    Loading();
                }

                _isLoading = false;
                button1.UseWaitCursor = false;
                button2.UseWaitCursor = false;
                listView1.UseWaitCursor = false;
                treeView1.UseWaitCursor = false;
                treeView1.Enabled = true;
                //listView1.Enabled = true;
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
            GetSystemImg sysimglst = new GetSystemImg();

            strPath = node.FullPath + "\\";

            DirectoryInfo directoryInfo = new DirectoryInfo(strPath);

            // 폴더 정보
            try
            {
                // 폴더 정보를 얻기
                foreach (DirectoryInfo subdirectoryInfo in directoryInfo.GetDirectories("*"))
                {
                    count = node.Nodes.Add(new TreeNode(subdirectoryInfo.Name, imgTree.Images.Count - 2, imgTree.Images.Count - 1));
                    if (HasSubDirectory(node.Nodes[count].FullPath))
                    {
                        node.Nodes[count].Nodes.Add("XXX");
                    }
                }
            }
            catch
            {
                //CloseLoadingWnd();
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
                if (temp_node.Text.Equals(strDirectory))
                    return temp_node;

                temp_node = temp_node.NextNode;
            }
        }

        // 리스트뷰 더블클릭
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (_isLoading == true) return;
            if (listView1.SelectedItems[0].Text.Equals("빈 폴더")) return;

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
                Process.Start(path.Text+"\\"+ listView1.SelectedItems[0].SubItems[0].Text);
            }
        }
        
        // 리스트뷰 우클릭 메뉴
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems[0].Text.Equals("빈 폴더")) return;
            if (e.Button == MouseButtons.Right)
            {
                var selected = listView1.SelectedItems;
                if (selected.Count > 0)
                {
                    var item = selected[0];
                    var ctx = new ContextMenu();
                    var menu1 = new MenuItem();

                    menu1.Text = "열기(O)";
                    menu1.Click += (o, s) =>
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
                    ctx.MenuItems.Add(menu1);

                    #region 레지스트리를 이용한 확장자에 따른 추가 메뉴
                    RegistryKey rKey = Registry.ClassesRoot.OpenSubKey("." + item.SubItems[2].Text);
                    if (rKey != null)
                    {
                        var extstr = rKey.GetValue("") as string;
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
                                        foreach (var key in subKeys)
                                        {
                                            var openKey = shellKey.OpenSubKey(key);
                                            if (openKey != null)
                                            {
                                                var text = openKey.GetValue("") as string;
                                                if (text != null)
                                                {
                                                    var menuRegistry = new MenuItem();
                                                    menuRegistry.Text = text;
                                                    menuRegistry.Click += (o, s) =>
                                                    {
                                                        Process.Start(path.Text + "\\" + listView1.SelectedItems[0].SubItems[0].Text);
                                                    };
                                                    ctx.MenuItems.Add(menuRegistry);
                                                }
                                            }
                                            openKey.Close();
                                        }
                                        shellKey.Close();
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    // TODO: 삭제 컨펌 창
                    var menu2 = new MenuItem();
                    menu2.Text = "삭제(D)";
                    menu2.Click += (o, s) =>
                    {
                        if(MessageBox.Show("삭제 하시겠습니까?", "경고", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
                    ctx.MenuItems.Add(menu2);

                    var menu3 = new MenuItem();
                    menu3.Text = "이름 바꾸기(M)";
                    menu3.Click += (o, s) =>
                    {
                        tbx = new TextBox();
                        tbx.Text = item.SubItems[0].Text;
                        tbx.Font = new Font("굴림", 9f);
                        tbx.Size = new Size(200, 21);
                        tbx.Location = new Point(item.Position.X+17,item.Position.Y-2);
                        listView1.Controls.Add(tbx);
                        tbx.Focus();
                        tbx.LostFocus += tbx_LostFocus;
                        tbx.KeyUp += tbx_KeyUp;
                    };
                    ctx.MenuItems.Add(menu3);
                    
                    ctx.Show(this, new Point(e.X + ((Control)sender).Left + 20, e.Y + ((Control)sender).Top + 20));
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
            string strSize = strSize = listView1.SelectedItems[0].SubItems[1].Text;
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
        }

        private void menuDetail_Click(object sender, EventArgs e)
        {
            InitMenuCheck();
            listView1.View = View.Details;
            menuDetail.Checked = true;
        }

        private void menuList_Click(object sender, EventArgs e)
        {
            InitMenuCheck();
            listView1.View = View.List;
            menuList.Checked = true;
        }

        private void menuClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // 위로가기
        private void button2_Click(object sender, EventArgs e)
        {
            if (_isLoading == true) return;
            try
            {
                TreeNode parentNode = treeView1.SelectedNode.Parent;
                treeView1.SelectedNode = parentNode;
                treeView1.SelectedNode.Collapse();
            }
            catch
            {
                // 최상의 디렉토리에서는 위로가기 불가
            }
        }

        // 새로고침
        private void button1_Click(object sender, EventArgs e)
        {
            if (_isLoading == false)
            {
                InitImage();
                treeView1.Nodes.Clear();
                InitTreeDriveSetting();
            }
        }

        // 새 폴더 생성
        private void 폴더ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(path.Text + "\\새 폴더" + ++num);
            }
            catch
            {
            }
            ListViewSetting(path.Text);
        }

        // 도움말 클릭 시
        private void helpbt_Click(object sender, EventArgs e)
        {
            MessageBox.Show("컬럼 클릭 시 정렬가능");
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
            }
        }

        //아이콘 크기조절
        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            if(listView1.View != View.LargeIcon)
            {
                MessageBox.Show("보기-아이콘에서 이용가능합니다.");
                return;
            }

            switch (trackBar.Value)
            {
                case 0:
                    imgLarge.ImageSize = new Size(30, 30);
                    break;
                case 1:
                    imgLarge.ImageSize = new Size(35, 35);
                    break;
                case 2:
                    imgLarge.ImageSize = new Size(40, 40);
                    break;
                case 3:
                    imgLarge.ImageSize = new Size(45, 45);
                    break;
                case 4:
                    imgLarge.ImageSize = new Size(50, 50);
                    break;
                case 5:
                    imgLarge.ImageSize = new Size(55, 55);
                    break;
                case 6:
                    imgLarge.ImageSize = new Size(60, 60);
                    break;
            }
            
            ListViewSetting(path.Text);
        }
        #endregion
    }
}
