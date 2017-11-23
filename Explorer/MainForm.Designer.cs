namespace Explorer
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("바탕 화면", 1, 1);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("다운로드", 2, 2);
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("문서", 3, 3);
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("즐겨찾기", 0, 0, new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this.panel1 = new System.Windows.Forms.Panel();
            this.helpbt = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.createDir = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.trayMode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLargeIcon = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDetail = new System.Windows.Forms.ToolStripMenuItem();
            this.menuList = new System.Windows.Forms.ToolStripMenuItem();
            this.function = new System.Windows.Forms.ToolStripMenuItem();
            this.refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.help = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.path = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imgLarge = new System.Windows.Forms.ImageList(this.components);
            this.imgSmall = new System.Windows.Forms.ImageList(this.components);
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imgTree = new System.Windows.Forms.ImageList(this.components);
            this.loadingBar = new System.Windows.Forms.ProgressBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.helpmenu = new System.Windows.Forms.ToolStripMenuItem();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.status_txt = new System.Windows.Forms.ToolStripStatusLabel();
            this.status = new System.Windows.Forms.StatusStrip();
            this.label2 = new System.Windows.Forms.Label();
            this.favoriteView = new System.Windows.Forms.TreeView();
            this.imgFavorite = new System.Windows.Forms.ImageList(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.imageListDrag = new System.Windows.Forms.ImageList(this.components);
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.status.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.helpbt);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1057, 52);
            this.panel1.TabIndex = 0;
            // 
            // helpbt
            // 
            this.helpbt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.helpbt.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.helpbt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.helpbt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.helpbt.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            this.helpbt.Image = global::Explorer.Properties.Resources.help_small;
            this.helpbt.Location = new System.Drawing.Point(1022, -1);
            this.helpbt.Name = "helpbt";
            this.helpbt.Size = new System.Drawing.Size(30, 24);
            this.helpbt.TabIndex = 6;
            this.helpbt.UseVisualStyleBackColor = false;
            this.helpbt.Click += new System.EventHandler(this.helpbt_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.menuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.menuStrip1.ImeMode = System.Windows.Forms.ImeMode.On;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuView,
            this.function,
            this.help});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.ShowItemToolTips = true;
            this.menuStrip1.Size = new System.Drawing.Size(1057, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createDir,
            this.toolStripMenuItem1,
            this.trayMode,
            this.menuClose});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(43, 20);
            this.menuFile.Text = "파일";
            // 
            // createDir
            // 
            this.createDir.Name = "createDir";
            this.createDir.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.createDir.Size = new System.Drawing.Size(199, 22);
            this.createDir.Text = "새 폴더 만들기";
            this.createDir.Click += new System.EventHandler(this.createDir_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(196, 6);
            // 
            // trayMode
            // 
            this.trayMode.Name = "trayMode";
            this.trayMode.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.trayMode.Size = new System.Drawing.Size(199, 22);
            this.trayMode.Text = "트레이 모드";
            this.trayMode.Click += new System.EventHandler(this.trayMode_Click);
            // 
            // menuClose
            // 
            this.menuClose.Name = "menuClose";
            this.menuClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.menuClose.Size = new System.Drawing.Size(199, 22);
            this.menuClose.Text = "종료";
            this.menuClose.Click += new System.EventHandler(this.menuClose_Click);
            // 
            // menuView
            // 
            this.menuView.Checked = true;
            this.menuView.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.menuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLargeIcon,
            this.menuDetail,
            this.menuList});
            this.menuView.Name = "menuView";
            this.menuView.Size = new System.Drawing.Size(43, 20);
            this.menuView.Text = "보기";
            // 
            // menuLargeIcon
            // 
            this.menuLargeIcon.Name = "menuLargeIcon";
            this.menuLargeIcon.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.menuLargeIcon.Size = new System.Drawing.Size(153, 22);
            this.menuLargeIcon.Text = "아이콘";
            this.menuLargeIcon.Click += new System.EventHandler(this.menuLargeIcon_Click);
            // 
            // menuDetail
            // 
            this.menuDetail.Checked = true;
            this.menuDetail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuDetail.Name = "menuDetail";
            this.menuDetail.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.menuDetail.Size = new System.Drawing.Size(153, 22);
            this.menuDetail.Text = "자세히";
            this.menuDetail.Click += new System.EventHandler(this.menuDetail_Click);
            // 
            // menuList
            // 
            this.menuList.Name = "menuList";
            this.menuList.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.menuList.Size = new System.Drawing.Size(153, 22);
            this.menuList.Text = "간단히";
            this.menuList.Click += new System.EventHandler(this.menuList_Click);
            // 
            // function
            // 
            this.function.Checked = true;
            this.function.CheckState = System.Windows.Forms.CheckState.Checked;
            this.function.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refresh});
            this.function.Name = "function";
            this.function.Size = new System.Drawing.Size(43, 20);
            this.function.Text = "기능";
            // 
            // refresh
            // 
            this.refresh.Name = "refresh";
            this.refresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.refresh.Size = new System.Drawing.Size(142, 22);
            this.refresh.Text = "새로고침";
            this.refresh.Click += new System.EventHandler(this.refresh_Click);
            // 
            // help
            // 
            this.help.Name = "help";
            this.help.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.help.Size = new System.Drawing.Size(55, 20);
            this.help.Text = "도움말";
            this.help.Click += new System.EventHandler(this.help_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.AutoScroll = true;
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.path);
            this.panel2.Location = new System.Drawing.Point(-7, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1074, 36);
            this.panel2.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Font = new System.Drawing.Font("굴림", 8F);
            this.button2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button2.Location = new System.Drawing.Point(12, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(70, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "◀";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button3.Font = new System.Drawing.Font("굴림", 8F);
            this.button3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button3.Location = new System.Drawing.Point(84, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(70, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "▶";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.SystemColors.HotTrack;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Font = new System.Drawing.Font("굴림", 8F);
            this.button1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button1.Location = new System.Drawing.Point(988, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(71, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "경로복사";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // path
            // 
            this.path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.path.BackColor = System.Drawing.SystemColors.Window;
            this.path.Location = new System.Drawing.Point(159, 3);
            this.path.Name = "path";
            this.path.ReadOnly = true;
            this.path.Size = new System.Drawing.Size(825, 21);
            this.path.TabIndex = 2;
            // 
            // listView1
            // 
            this.listView1.AllowDrop = true;
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.BackColor = System.Drawing.SystemColors.Window;
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.listView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.listView1.FullRowSelect = true;
            this.listView1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.listView1.LargeImageList = this.imgLarge;
            this.listView1.Location = new System.Drawing.Point(277, 52);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(780, 570);
            this.listView1.SmallImageList = this.imgSmall;
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            this.listView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listView1_ItemDrag);
            this.listView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView1_DragDrop);
            this.listView1.DragOver += new System.Windows.Forms.DragEventHandler(this.listView1_DragOver);
            this.listView1.DragLeave += new System.EventHandler(this.listView1_DragLeave);
            this.listView1.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.listView1_GiveFeedback);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
            this.listView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = " 이름";
            this.columnHeader1.Width = 266;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "크기";
            this.columnHeader2.Width = 127;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "유형";
            this.columnHeader3.Width = 124;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "수정한 날짜";
            this.columnHeader4.Width = 260;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "hidden";
            this.columnHeader5.Width = 0;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "속성";
            this.columnHeader6.Width = 0;
            // 
            // imgLarge
            // 
            this.imgLarge.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imgLarge.ImageSize = new System.Drawing.Size(45, 45);
            this.imgLarge.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imgSmall
            // 
            this.imgSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imgSmall.ImageSize = new System.Drawing.Size(16, 16);
            this.imgSmall.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.BackColor = System.Drawing.SystemColors.Window;
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.treeView1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imgTree;
            this.treeView1.Location = new System.Drawing.Point(-1, 68);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(278, 501);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterExpand);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // imgTree
            // 
            this.imgTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgTree.ImageStream")));
            this.imgTree.TransparentColor = System.Drawing.Color.Transparent;
            this.imgTree.Images.SetKeyName(0, "4.png");
            // 
            // loadingBar
            // 
            this.loadingBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.loadingBar.Location = new System.Drawing.Point(829, 622);
            this.loadingBar.Name = "loadingBar";
            this.loadingBar.Size = new System.Drawing.Size(225, 17);
            this.loadingBar.TabIndex = 6;
            // 
            // helpmenu
            // 
            this.helpmenu.Name = "helpmenu";
            this.helpmenu.Size = new System.Drawing.Size(55, 20);
            this.helpmenu.Text = "도움말";
            // 
            // trackBar
            // 
            this.trackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar.BackColor = System.Drawing.SystemColors.Control;
            this.trackBar.Cursor = System.Windows.Forms.Cursors.Default;
            this.trackBar.LargeChange = 6;
            this.trackBar.Location = new System.Drawing.Point(622, 620);
            this.trackBar.Maximum = 100;
            this.trackBar.Minimum = 20;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(202, 45);
            this.trackBar.SmallChange = 0;
            this.trackBar.TabIndex = 7;
            this.trackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar.Value = 60;
            this.trackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBar_MouseUp);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(540, 624);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "| 아이콘 크기 |";
            // 
            // status_txt
            // 
            this.status_txt.BackColor = System.Drawing.SystemColors.Control;
            this.status_txt.Name = "status_txt";
            this.status_txt.Size = new System.Drawing.Size(121, 17);
            this.status_txt.Text = "toolStripStatusLabel1";
            // 
            // status
            // 
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status_txt});
            this.status.Location = new System.Drawing.Point(0, 619);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(1057, 22);
            this.status.TabIndex = 5;
            this.status.Text = "statusStrip1";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(818, 624);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "| ";
            // 
            // favoriteView
            // 
            this.favoriteView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.favoriteView.ImageIndex = 0;
            this.favoriteView.ImageList = this.imgFavorite;
            this.favoriteView.Location = new System.Drawing.Point(-1, 2);
            this.favoriteView.Name = "favoriteView";
            treeNode1.ImageIndex = 1;
            treeNode1.Name = "노드1";
            treeNode1.SelectedImageIndex = 1;
            treeNode1.Text = "바탕 화면";
            treeNode2.ImageIndex = 2;
            treeNode2.Name = "노드0";
            treeNode2.SelectedImageIndex = 2;
            treeNode2.Text = "다운로드";
            treeNode3.ImageIndex = 3;
            treeNode3.Name = "노드1";
            treeNode3.SelectedImageIndex = 3;
            treeNode3.Text = "문서";
            treeNode4.ImageIndex = 0;
            treeNode4.Name = "노드0";
            treeNode4.SelectedImageIndex = 0;
            treeNode4.Text = "즐겨찾기";
            this.favoriteView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4});
            this.favoriteView.SelectedImageIndex = 0;
            this.favoriteView.Size = new System.Drawing.Size(278, 64);
            this.favoriteView.TabIndex = 10;
            this.favoriteView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.favoriteView_AfterCollapse);
            this.favoriteView.DoubleClick += new System.EventHandler(this.favoriteView_DoubleClick);
            // 
            // imgFavorite
            // 
            this.imgFavorite.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgFavorite.ImageStream")));
            this.imgFavorite.TransparentColor = System.Drawing.Color.Transparent;
            this.imgFavorite.Images.SetKeyName(0, "0.png");
            this.imgFavorite.Images.SetKeyName(1, "1.png");
            this.imgFavorite.Images.SetKeyName(2, "2.png");
            this.imgFavorite.Images.SetKeyName(3, "3.png");
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.treeView1);
            this.panel3.Controls.Add(this.favoriteView);
            this.panel3.Location = new System.Drawing.Point(0, 52);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(278, 570);
            this.panel3.TabIndex = 11;
            // 
            // imageListDrag
            // 
            this.imageListDrag.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListDrag.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListDrag.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1057, 641);
            this.Controls.Add(this.loadingBar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.status);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Windows Explorer";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ImageList imgLarge;
        private System.Windows.Forms.ImageList imgSmall;
        private System.Windows.Forms.ToolStripMenuItem helpmenu;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuView;
        private System.Windows.Forms.ToolStripMenuItem menuLargeIcon;
        private System.Windows.Forms.ToolStripMenuItem menuDetail;
        private System.Windows.Forms.ToolStripMenuItem menuList;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuClose;
        private System.Windows.Forms.ProgressBar loadingBar;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem createDir;
        private System.Windows.Forms.Button helpbt;
        private System.Windows.Forms.ImageList imgTree;
        private System.Windows.Forms.TextBox path;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem function;
        private System.Windows.Forms.ToolStripMenuItem refresh;
        private System.Windows.Forms.ToolStripStatusLabel status_txt;
        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.ToolStripMenuItem help;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ToolStripMenuItem trayMode;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.TreeView favoriteView;
        private System.Windows.Forms.ImageList imgFavorite;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ImageList imageListDrag;
    }
}

