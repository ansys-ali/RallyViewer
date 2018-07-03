namespace RallyViewer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.labelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grid = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.showOnlyOwnerListBox = new System.Windows.Forms.CheckedListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.highlightOwnerListBox = new System.Windows.Forms.CheckedListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.showOnlyProjectListBox = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.highlightProjectListBox = new System.Windows.Forms.CheckedListBox();
            this._chart = new Braincase.GanttChart.Chart();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fILEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandTasksMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseTasksMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.showHideCompletedStoriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleLargeFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openInBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.resizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.week1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.week2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.week3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.week1ToWeek2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.week2ToWeek3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.entireIterationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setPredecessorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 639);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1008, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // labelStatus
            // 
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(993, 17);
            this.labelStatus.Spring = true;
            this.labelStatus.Text = "Status:";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1008, 615);
            this.splitContainer1.SplitterDistance = 226;
            this.splitContainer1.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.grid);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(220, 824);
            this.panel1.TabIndex = 0;
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.MultiSelect = false;
            this.grid.Name = "grid";
            this.grid.ReadOnly = true;
            this.grid.RowHeadersVisible = false;
            this.grid.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid.RowTemplate.Height = 3;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new System.Drawing.Size(220, 824);
            this.grid.TabIndex = 1;
            this.grid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellDoubleClick);
            this.grid.SelectionChanged += new System.EventHandler(this.grid_SelectionChanged);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Show";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 50;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.HeaderText = "Saga Feature";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Enabled = false;
            this.splitContainer2.Panel2Collapsed = true;
            this.splitContainer2.Size = new System.Drawing.Size(778, 615);
            this.splitContainer2.SplitterDistance = 523;
            this.splitContainer2.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this._chart);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(778, 615);
            this.panel2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(600, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(175, 609);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.showOnlyOwnerListBox);
            this.groupBox5.Location = new System.Drawing.Point(6, 457);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(163, 140);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Show Only Owner";
            // 
            // showOnlyOwnerListBox
            // 
            this.showOnlyOwnerListBox.FormattingEnabled = true;
            this.showOnlyOwnerListBox.Items.AddRange(new object[] {
            "Addin",
            "FWI",
            "UI",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.showOnlyOwnerListBox.Location = new System.Drawing.Point(6, 19);
            this.showOnlyOwnerListBox.Name = "showOnlyOwnerListBox";
            this.showOnlyOwnerListBox.Size = new System.Drawing.Size(151, 109);
            this.showOnlyOwnerListBox.TabIndex = 0;
            this.showOnlyOwnerListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.showOnlyOwnerListBox_ItemCheck);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.highlightOwnerListBox);
            this.groupBox4.Location = new System.Drawing.Point(6, 311);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(163, 140);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Highlight Owner";
            // 
            // highlightOwnerListBox
            // 
            this.highlightOwnerListBox.FormattingEnabled = true;
            this.highlightOwnerListBox.Items.AddRange(new object[] {
            "Addin",
            "FWI",
            "UI",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.highlightOwnerListBox.Location = new System.Drawing.Point(6, 19);
            this.highlightOwnerListBox.Name = "highlightOwnerListBox";
            this.highlightOwnerListBox.Size = new System.Drawing.Size(151, 109);
            this.highlightOwnerListBox.TabIndex = 0;
            this.highlightOwnerListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.highlightOwnerListBox_ItemCheck);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.showOnlyProjectListBox);
            this.groupBox3.Location = new System.Drawing.Point(6, 165);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(163, 140);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Show Only Project";
            // 
            // showOnlyProjectListBox
            // 
            this.showOnlyProjectListBox.FormattingEnabled = true;
            this.showOnlyProjectListBox.Items.AddRange(new object[] {
            "Addin",
            "FWI",
            "UI",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.showOnlyProjectListBox.Location = new System.Drawing.Point(6, 19);
            this.showOnlyProjectListBox.Name = "showOnlyProjectListBox";
            this.showOnlyProjectListBox.Size = new System.Drawing.Size(151, 109);
            this.showOnlyProjectListBox.TabIndex = 0;
            this.showOnlyProjectListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.showOnlyProjectListBox_ItemCheck);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.highlightProjectListBox);
            this.groupBox2.Location = new System.Drawing.Point(6, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(163, 140);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Highlight Project";
            // 
            // highlightProjectListBox
            // 
            this.highlightProjectListBox.FormattingEnabled = true;
            this.highlightProjectListBox.Items.AddRange(new object[] {
            "Addin",
            "FWI",
            "UI",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.highlightProjectListBox.Location = new System.Drawing.Point(6, 19);
            this.highlightProjectListBox.Name = "highlightProjectListBox";
            this.highlightProjectListBox.Size = new System.Drawing.Size(151, 109);
            this.highlightProjectListBox.TabIndex = 0;
            this.highlightProjectListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.highlightProjectListBox_ItemCheck);
            // 
            // _chart
            // 
            this._chart.AllowTaskDragDrop = false;
            this._chart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._chart.AutoScroll = true;
            this._chart.BarWidth = 15;
            this._chart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._chart.FullDateStringFormat = null;
            this._chart.Location = new System.Drawing.Point(0, 3);
            this._chart.Margin = new System.Windows.Forms.Padding(0);
            this._chart.Name = "_chart";
            this._chart.Size = new System.Drawing.Size(597, 609);
            this._chart.TabIndex = 1;
            this._chart.TaskMouseClick += new System.EventHandler<Braincase.GanttChart.TaskMouseEventArgs>(this.chart_TaskMouseClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fILEToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fILEToolStripMenuItem
            // 
            this.fILEToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printMenuItem,
            this.toolStripSeparator3,
            this.exitToolStripMenuItem});
            this.fILEToolStripMenuItem.Name = "fILEToolStripMenuItem";
            this.fILEToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fILEToolStripMenuItem.Text = "File";
            // 
            // printMenuItem
            // 
            this.printMenuItem.Name = "printMenuItem";
            this.printMenuItem.Size = new System.Drawing.Size(158, 22);
            this.printMenuItem.Text = "Export as bmp...";
            this.printMenuItem.Click += new System.EventHandler(this.printMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(155, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.expandAllToolStripMenuItem,
            this.collapseAllToolStripMenuItem,
            this.expandTasksMenuItem,
            this.collapseTasksMenuItem,
            this.toolStripSeparator1,
            this.showHideCompletedStoriesToolStripMenuItem,
            this.toggleLargeFontToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // expandAllToolStripMenuItem
            // 
            this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
            this.expandAllToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.expandAllToolStripMenuItem.Text = "Expand All";
            this.expandAllToolStripMenuItem.Click += new System.EventHandler(this.expandAllToolStripMenuItem_Click);
            // 
            // collapseAllToolStripMenuItem
            // 
            this.collapseAllToolStripMenuItem.Name = "collapseAllToolStripMenuItem";
            this.collapseAllToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.collapseAllToolStripMenuItem.Text = "Collapse All";
            this.collapseAllToolStripMenuItem.Click += new System.EventHandler(this.collapseAllToolStripMenuItem_Click);
            // 
            // expandTasksMenuItem
            // 
            this.expandTasksMenuItem.Name = "expandTasksMenuItem";
            this.expandTasksMenuItem.Size = new System.Drawing.Size(233, 22);
            this.expandTasksMenuItem.Text = "Expand Tasks";
            this.expandTasksMenuItem.Click += new System.EventHandler(this.expandTasksMenuItem_Click);
            // 
            // collapseTasksMenuItem
            // 
            this.collapseTasksMenuItem.Name = "collapseTasksMenuItem";
            this.collapseTasksMenuItem.Size = new System.Drawing.Size(233, 22);
            this.collapseTasksMenuItem.Text = "Collapse Tasks";
            this.collapseTasksMenuItem.Click += new System.EventHandler(this.collapseTasksMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(230, 6);
            // 
            // showHideCompletedStoriesToolStripMenuItem
            // 
            this.showHideCompletedStoriesToolStripMenuItem.Name = "showHideCompletedStoriesToolStripMenuItem";
            this.showHideCompletedStoriesToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.showHideCompletedStoriesToolStripMenuItem.Text = "Show/Hide Completed Stories";
            this.showHideCompletedStoriesToolStripMenuItem.Click += new System.EventHandler(this.showHideCompletedStoriesToolStripMenuItem_Click);
            // 
            // toggleLargeFontToolStripMenuItem
            // 
            this.toggleLargeFontToolStripMenuItem.Checked = true;
            this.toggleLargeFontToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toggleLargeFontToolStripMenuItem.Name = "toggleLargeFontToolStripMenuItem";
            this.toggleLargeFontToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.toggleLargeFontToolStripMenuItem.Text = "Large Font";
            this.toggleLargeFontToolStripMenuItem.Click += new System.EventHandler(this.toggleLargeFontToolStripMenuItem_Click);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openInBrowserToolStripMenuItem,
            this.toolStripSeparator4,
            this.resizeToolStripMenuItem,
            this.moveToToolStripMenuItem,
            this.setPredecessorToolStripMenuItem,
            this.toolStripSeparator5,
            this.refreshToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(171, 126);
            // 
            // openInBrowserToolStripMenuItem
            // 
            this.openInBrowserToolStripMenuItem.Name = "openInBrowserToolStripMenuItem";
            this.openInBrowserToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.openInBrowserToolStripMenuItem.Text = "Open in Browser...";
            this.openInBrowserToolStripMenuItem.Click += new System.EventHandler(this.openInBrowserToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(167, 6);
            // 
            // resizeToolStripMenuItem
            // 
            this.resizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.week1ToolStripMenuItem,
            this.week2ToolStripMenuItem,
            this.week3ToolStripMenuItem,
            this.week1ToWeek2ToolStripMenuItem,
            this.week2ToWeek3ToolStripMenuItem,
            this.entireIterationToolStripMenuItem});
            this.resizeToolStripMenuItem.Name = "resizeToolStripMenuItem";
            this.resizeToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.resizeToolStripMenuItem.Text = "Resize";
            // 
            // week1ToolStripMenuItem
            // 
            this.week1ToolStripMenuItem.Name = "week1ToolStripMenuItem";
            this.week1ToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.week1ToolStripMenuItem.Text = "Week 1";
            this.week1ToolStripMenuItem.Click += new System.EventHandler(this.resizeWeekToolStripMenuItem_Click);
            // 
            // week2ToolStripMenuItem
            // 
            this.week2ToolStripMenuItem.Name = "week2ToolStripMenuItem";
            this.week2ToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.week2ToolStripMenuItem.Text = "Week 2";
            this.week2ToolStripMenuItem.Click += new System.EventHandler(this.resizeWeekToolStripMenuItem_Click);
            // 
            // week3ToolStripMenuItem
            // 
            this.week3ToolStripMenuItem.Name = "week3ToolStripMenuItem";
            this.week3ToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.week3ToolStripMenuItem.Text = "Week 3";
            this.week3ToolStripMenuItem.Click += new System.EventHandler(this.resizeWeekToolStripMenuItem_Click);
            // 
            // week1ToWeek2ToolStripMenuItem
            // 
            this.week1ToWeek2ToolStripMenuItem.Name = "week1ToWeek2ToolStripMenuItem";
            this.week1ToWeek2ToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.week1ToWeek2ToolStripMenuItem.Text = "Week 1 to Week 2";
            this.week1ToWeek2ToolStripMenuItem.Click += new System.EventHandler(this.resizeWeekToolStripMenuItem_Click);
            // 
            // week2ToWeek3ToolStripMenuItem
            // 
            this.week2ToWeek3ToolStripMenuItem.Name = "week2ToWeek3ToolStripMenuItem";
            this.week2ToWeek3ToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.week2ToWeek3ToolStripMenuItem.Text = "Week 2 to Week 3";
            this.week2ToWeek3ToolStripMenuItem.Click += new System.EventHandler(this.resizeWeekToolStripMenuItem_Click);
            // 
            // entireIterationToolStripMenuItem
            // 
            this.entireIterationToolStripMenuItem.Name = "entireIterationToolStripMenuItem";
            this.entireIterationToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.entireIterationToolStripMenuItem.Text = "Entire Iteration";
            this.entireIterationToolStripMenuItem.Click += new System.EventHandler(this.resizeWeekToolStripMenuItem_Click);
            // 
            // moveToToolStripMenuItem
            // 
            this.moveToToolStripMenuItem.Name = "moveToToolStripMenuItem";
            this.moveToToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.moveToToolStripMenuItem.Text = "Move to";
            // 
            // setPredecessorToolStripMenuItem
            // 
            this.setPredecessorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noneToolStripMenuItem2});
            this.setPredecessorToolStripMenuItem.Name = "setPredecessorToolStripMenuItem";
            this.setPredecessorToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.setPredecessorToolStripMenuItem.Text = "Set Predecessor(s)";
            // 
            // noneToolStripMenuItem2
            // 
            this.noneToolStripMenuItem2.Name = "noneToolStripMenuItem2";
            this.noneToolStripMenuItem2.Size = new System.Drawing.Size(103, 22);
            this.noneToolStripMenuItem2.Text = "None";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(167, 6);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 661);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Rally View";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Braincase.GanttChart.Chart _chart;
        private System.Windows.Forms.ToolStripStatusLabel labelStatus;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fILEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem printMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem openInBrowserToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem resizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem week1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem week2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem week3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem week1ToWeek2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem week2ToWeek3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem entireIterationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setPredecessorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem moveToToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.ToolStripMenuItem showHideCompletedStoriesToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckedListBox highlightProjectListBox;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckedListBox showOnlyOwnerListBox;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckedListBox highlightOwnerListBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckedListBox showOnlyProjectListBox;
        private System.Windows.Forms.ToolStripMenuItem toggleLargeFontToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStripMenuItem expandTasksMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseTasksMenuItem;
    }
}

