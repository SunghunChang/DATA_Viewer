<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InjuryDisplayFrm
    Inherits System.Windows.Forms.Form

    'Form은 Dispose를 재정의하여 구성 요소 목록을 정리합니다.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows Form 디자이너에 필요합니다.
    Private components As System.ComponentModel.IContainer

    '참고: 다음 프로시저는 Windows Form 디자이너에 필요합니다.
    '수정하려면 Windows Form 디자이너를 사용하십시오.  
    '코드 편집기를 사용하여 수정하지 마십시오.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InjuryDisplayFrm))
        Me.OpenDlg = New System.Windows.Forms.OpenFileDialog()
        Me.OpenWithXMADgic = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.OpenWithXMADgicToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenKn3WithHyperViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnValGraph = New System.Windows.Forms.Button()
        Me.BtnInjuryGraph = New System.Windows.Forms.Button()
        Me.PPTBtn = New System.Windows.Forms.Button()
        Me.ReloadBtn = New System.Windows.Forms.Button()
        Me.PPTmenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SplitConMain = New System.Windows.Forms.SplitContainer()
        Me.DescripTxt = New System.Windows.Forms.TextBox()
        Me.OpenLst = New System.Windows.Forms.ListBox()
        Me.PathLbl = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ScoreBoard = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.TabPage6 = New System.Windows.Forms.TabPage()
        Me.BtnFind = New System.Windows.Forms.Button()
        Me.CNCAP_Rear = New System.Windows.Forms.TabControl()
        Me.TabPage7 = New System.Windows.Forms.TabPage()
        Me.TabPage8 = New System.Windows.Forms.TabPage()
        Me.TabPage9 = New System.Windows.Forms.TabPage()
        Me.EuroFrontal = New System.Windows.Forms.TabControl()
        Me.TabPage11 = New System.Windows.Forms.TabPage()
        Me.TabPage12 = New System.Windows.Forms.TabPage()
        Me.TabPage10 = New System.Windows.Forms.TabPage()
        Me.ChkDropMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.FindDropToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddFileDlg = New System.Windows.Forms.OpenFileDialog()
        Me.CommentsToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.InjuryLbl1 = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.Peak_Injury1 = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.Peak_Injury_DOM_F = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.InjuryLbl2 = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.Peak_Injury_DOM_O = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.InjuryLbl3 = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.Peak_Injury_CHINA_F = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.InjuryLbl4 = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.Peak_Injury_CHINA_O = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.InjuryLbl5 = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.Peak_Injury_CHINA_F_Rear = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.Peak_Injury_CHINA_O_Rear = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.InjuryLbl6 = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.Peak_Injury_Euro_F = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.Peak_Injury_Euro_R = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.InjuryLbl7 = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.Peak_Injury_Euro_O = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.InjuryLbl8 = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.OpenWithXMADgic.SuspendLayout()
        CType(Me.SplitConMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitConMain.Panel1.SuspendLayout()
        Me.SplitConMain.Panel2.SuspendLayout()
        Me.SplitConMain.SuspendLayout()
        Me.ScoreBoard.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        Me.CNCAP_Rear.SuspendLayout()
        Me.TabPage7.SuspendLayout()
        Me.TabPage8.SuspendLayout()
        Me.TabPage9.SuspendLayout()
        Me.EuroFrontal.SuspendLayout()
        Me.TabPage11.SuspendLayout()
        Me.TabPage12.SuspendLayout()
        Me.TabPage10.SuspendLayout()
        Me.ChkDropMenu.SuspendLayout()
        CType(Me.InjuryLbl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Peak_Injury1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Peak_Injury_DOM_F, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InjuryLbl2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Peak_Injury_DOM_O, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InjuryLbl3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Peak_Injury_CHINA_F, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InjuryLbl4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Peak_Injury_CHINA_O, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InjuryLbl5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Peak_Injury_CHINA_F_Rear, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Peak_Injury_CHINA_O_Rear, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InjuryLbl6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Peak_Injury_Euro_F, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Peak_Injury_Euro_R, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InjuryLbl7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Peak_Injury_Euro_O, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InjuryLbl8, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'OpenDlg
        '
        Me.OpenDlg.FileName = "OpenFileDialog1"
        '
        'OpenWithXMADgic
        '
        Me.OpenWithXMADgic.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenWithXMADgicToolStripMenuItem, Me.OpenKn3WithHyperViewToolStripMenuItem})
        Me.OpenWithXMADgic.Name = "OpenWithXMADgic"
        Me.OpenWithXMADgic.Size = New System.Drawing.Size(217, 48)
        '
        'OpenWithXMADgicToolStripMenuItem
        '
        Me.OpenWithXMADgicToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.XMADgic
        Me.OpenWithXMADgicToolStripMenuItem.Name = "OpenWithXMADgicToolStripMenuItem"
        Me.OpenWithXMADgicToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.OpenWithXMADgicToolStripMenuItem.Text = "Open xml with XMADgic"
        '
        'OpenKn3WithHyperViewToolStripMenuItem
        '
        Me.OpenKn3WithHyperViewToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.HyperView
        Me.OpenKn3WithHyperViewToolStripMenuItem.Name = "OpenKn3WithHyperViewToolStripMenuItem"
        Me.OpenKn3WithHyperViewToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.OpenKn3WithHyperViewToolStripMenuItem.Text = "Open kn3 with HyperView"
        '
        'BtnValGraph
        '
        Me.BtnValGraph.Enabled = False
        Me.BtnValGraph.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.BtnValGraph.Location = New System.Drawing.Point(242, 187)
        Me.BtnValGraph.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.BtnValGraph.Name = "BtnValGraph"
        Me.BtnValGraph.Size = New System.Drawing.Size(104, 29)
        Me.BtnValGraph.TabIndex = 2
        Me.BtnValGraph.Text = "Injury Values"
        Me.BtnValGraph.UseVisualStyleBackColor = True
        '
        'BtnInjuryGraph
        '
        Me.BtnInjuryGraph.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.BtnInjuryGraph.Location = New System.Drawing.Point(352, 187)
        Me.BtnInjuryGraph.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.BtnInjuryGraph.Name = "BtnInjuryGraph"
        Me.BtnInjuryGraph.Size = New System.Drawing.Size(102, 29)
        Me.BtnInjuryGraph.TabIndex = 3
        Me.BtnInjuryGraph.Text = "Injury Graph"
        Me.BtnInjuryGraph.UseVisualStyleBackColor = True
        '
        'PPTBtn
        '
        Me.PPTBtn.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.PPTBtn.Location = New System.Drawing.Point(98, 187)
        Me.PPTBtn.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.PPTBtn.Name = "PPTBtn"
        Me.PPTBtn.Size = New System.Drawing.Size(117, 29)
        Me.PPTBtn.TabIndex = 4
        Me.PPTBtn.Text = "Export Table"
        Me.PPTBtn.UseVisualStyleBackColor = True
        '
        'ReloadBtn
        '
        Me.ReloadBtn.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.ReloadBtn.Location = New System.Drawing.Point(18, 187)
        Me.ReloadBtn.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ReloadBtn.Name = "ReloadBtn"
        Me.ReloadBtn.Size = New System.Drawing.Size(74, 29)
        Me.ReloadBtn.TabIndex = 5
        Me.ReloadBtn.Text = "Reload [F5]"
        Me.ReloadBtn.UseVisualStyleBackColor = True
        '
        'PPTmenu
        '
        Me.PPTmenu.Name = "PPTmenu"
        Me.PPTmenu.Size = New System.Drawing.Size(61, 4)
        '
        'SplitConMain
        '
        Me.SplitConMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitConMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitConMain.Location = New System.Drawing.Point(0, 0)
        Me.SplitConMain.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.SplitConMain.Name = "SplitConMain"
        Me.SplitConMain.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitConMain.Panel1
        '
        Me.SplitConMain.Panel1.Controls.Add(Me.DescripTxt)
        Me.SplitConMain.Panel1.Controls.Add(Me.OpenLst)
        Me.SplitConMain.Panel1.Controls.Add(Me.PathLbl)
        '
        'SplitConMain.Panel2
        '
        Me.SplitConMain.Panel2.Controls.Add(Me.Button1)
        Me.SplitConMain.Panel2.Controls.Add(Me.ScoreBoard)
        Me.SplitConMain.Panel2.Controls.Add(Me.BtnInjuryGraph)
        Me.SplitConMain.Panel2.Controls.Add(Me.BtnValGraph)
        Me.SplitConMain.Panel2.Controls.Add(Me.ReloadBtn)
        Me.SplitConMain.Panel2.Controls.Add(Me.PPTBtn)
        Me.SplitConMain.Size = New System.Drawing.Size(795, 343)
        Me.SplitConMain.SplitterDistance = 72
        Me.SplitConMain.SplitterWidth = 6
        Me.SplitConMain.TabIndex = 6
        '
        'DescripTxt
        '
        Me.DescripTxt.BackColor = System.Drawing.SystemColors.Info
        Me.DescripTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.DescripTxt.Location = New System.Drawing.Point(373, 13)
        Me.DescripTxt.Multiline = True
        Me.DescripTxt.Name = "DescripTxt"
        Me.DescripTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.DescripTxt.Size = New System.Drawing.Size(80, 44)
        Me.DescripTxt.TabIndex = 5
        '
        'OpenLst
        '
        Me.OpenLst.BackColor = System.Drawing.SystemColors.Window
        Me.OpenLst.ContextMenuStrip = Me.OpenWithXMADgic
        Me.OpenLst.Font = New System.Drawing.Font("Calibri", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OpenLst.FormattingEnabled = True
        Me.OpenLst.ItemHeight = 17
        Me.OpenLst.Location = New System.Drawing.Point(18, 37)
        Me.OpenLst.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.OpenLst.Name = "OpenLst"
        Me.OpenLst.Size = New System.Drawing.Size(213, 21)
        Me.OpenLst.TabIndex = 4
        '
        'PathLbl
        '
        Me.PathLbl.AutoSize = True
        Me.PathLbl.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.PathLbl.Location = New System.Drawing.Point(15, 13)
        Me.PathLbl.Name = "PathLbl"
        Me.PathLbl.Size = New System.Drawing.Size(45, 15)
        Me.PathLbl.TabIndex = 3
        Me.PathLbl.Text = "Label1"
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Button1.Location = New System.Drawing.Point(66, 229)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(79, 28)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = "Add Case"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ScoreBoard
        '
        Me.ScoreBoard.AllowDrop = True
        Me.ScoreBoard.Controls.Add(Me.TabPage1)
        Me.ScoreBoard.Controls.Add(Me.TabPage2)
        Me.ScoreBoard.Controls.Add(Me.TabPage3)
        Me.ScoreBoard.Controls.Add(Me.TabPage4)
        Me.ScoreBoard.Controls.Add(Me.TabPage5)
        Me.ScoreBoard.Controls.Add(Me.TabPage6)
        Me.ScoreBoard.Controls.Add(Me.TabPage9)
        Me.ScoreBoard.Controls.Add(Me.TabPage10)
        Me.ScoreBoard.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.ScoreBoard.Location = New System.Drawing.Point(3, 2)
        Me.ScoreBoard.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ScoreBoard.Name = "ScoreBoard"
        Me.ScoreBoard.SelectedIndex = 0
        Me.ScoreBoard.Size = New System.Drawing.Size(732, 159)
        Me.ScoreBoard.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.InjuryLbl1)
        Me.TabPage1.Controls.Add(Me.Peak_Injury1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 24)
        Me.TabPage1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage1.Size = New System.Drawing.Size(724, 131)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "TabPage1"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Peak_Injury_DOM_F)
        Me.TabPage2.Controls.Add(Me.InjuryLbl2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 24)
        Me.TabPage2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage2.Size = New System.Drawing.Size(724, 131)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "TabPage2"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.Peak_Injury_DOM_O)
        Me.TabPage3.Controls.Add(Me.InjuryLbl3)
        Me.TabPage3.Location = New System.Drawing.Point(4, 24)
        Me.TabPage3.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage3.Size = New System.Drawing.Size(724, 131)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "TabPage3"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.Peak_Injury_CHINA_F)
        Me.TabPage4.Controls.Add(Me.InjuryLbl4)
        Me.TabPage4.Location = New System.Drawing.Point(4, 24)
        Me.TabPage4.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage4.Size = New System.Drawing.Size(724, 131)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "TabPage4"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.Peak_Injury_CHINA_O)
        Me.TabPage5.Controls.Add(Me.InjuryLbl5)
        Me.TabPage5.Location = New System.Drawing.Point(4, 24)
        Me.TabPage5.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage5.Size = New System.Drawing.Size(724, 131)
        Me.TabPage5.TabIndex = 4
        Me.TabPage5.Text = "TabPage5"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.BtnFind)
        Me.TabPage6.Controls.Add(Me.CNCAP_Rear)
        Me.TabPage6.Controls.Add(Me.InjuryLbl6)
        Me.TabPage6.Location = New System.Drawing.Point(4, 24)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage6.Size = New System.Drawing.Size(724, 131)
        Me.TabPage6.TabIndex = 5
        Me.TabPage6.Text = "TabPage6"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'BtnFind
        '
        Me.BtnFind.Location = New System.Drawing.Point(79, 87)
        Me.BtnFind.Name = "BtnFind"
        Me.BtnFind.Size = New System.Drawing.Size(113, 29)
        Me.BtnFind.TabIndex = 3
        Me.BtnFind.Text = "Find Drop/Rise"
        Me.BtnFind.UseVisualStyleBackColor = True
        '
        'CNCAP_Rear
        '
        Me.CNCAP_Rear.Alignment = System.Windows.Forms.TabAlignment.Bottom
        Me.CNCAP_Rear.Controls.Add(Me.TabPage7)
        Me.CNCAP_Rear.Controls.Add(Me.TabPage8)
        Me.CNCAP_Rear.Location = New System.Drawing.Point(210, 13)
        Me.CNCAP_Rear.Name = "CNCAP_Rear"
        Me.CNCAP_Rear.SelectedIndex = 0
        Me.CNCAP_Rear.Size = New System.Drawing.Size(303, 89)
        Me.CNCAP_Rear.TabIndex = 2
        '
        'TabPage7
        '
        Me.TabPage7.Controls.Add(Me.Peak_Injury_CHINA_F_Rear)
        Me.TabPage7.Location = New System.Drawing.Point(4, 4)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage7.Size = New System.Drawing.Size(295, 61)
        Me.TabPage7.TabIndex = 0
        Me.TabPage7.Text = "Frontal"
        Me.TabPage7.UseVisualStyleBackColor = True
        '
        'TabPage8
        '
        Me.TabPage8.Controls.Add(Me.Peak_Injury_CHINA_O_Rear)
        Me.TabPage8.Location = New System.Drawing.Point(4, 4)
        Me.TabPage8.Name = "TabPage8"
        Me.TabPage8.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage8.Size = New System.Drawing.Size(295, 61)
        Me.TabPage8.TabIndex = 1
        Me.TabPage8.Text = "Offset"
        Me.TabPage8.UseVisualStyleBackColor = True
        '
        'TabPage9
        '
        Me.TabPage9.Controls.Add(Me.EuroFrontal)
        Me.TabPage9.Controls.Add(Me.InjuryLbl7)
        Me.TabPage9.Location = New System.Drawing.Point(4, 24)
        Me.TabPage9.Name = "TabPage9"
        Me.TabPage9.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage9.Size = New System.Drawing.Size(724, 131)
        Me.TabPage9.TabIndex = 6
        Me.TabPage9.Text = "TabPage9"
        Me.TabPage9.UseVisualStyleBackColor = True
        '
        'EuroFrontal
        '
        Me.EuroFrontal.Alignment = System.Windows.Forms.TabAlignment.Bottom
        Me.EuroFrontal.Controls.Add(Me.TabPage11)
        Me.EuroFrontal.Controls.Add(Me.TabPage12)
        Me.EuroFrontal.Location = New System.Drawing.Point(305, 28)
        Me.EuroFrontal.Name = "EuroFrontal"
        Me.EuroFrontal.SelectedIndex = 0
        Me.EuroFrontal.Size = New System.Drawing.Size(256, 99)
        Me.EuroFrontal.TabIndex = 2
        '
        'TabPage11
        '
        Me.TabPage11.Controls.Add(Me.Peak_Injury_Euro_F)
        Me.TabPage11.Location = New System.Drawing.Point(4, 4)
        Me.TabPage11.Name = "TabPage11"
        Me.TabPage11.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage11.Size = New System.Drawing.Size(248, 71)
        Me.TabPage11.TabIndex = 0
        Me.TabPage11.Text = "Front"
        Me.TabPage11.UseVisualStyleBackColor = True
        '
        'TabPage12
        '
        Me.TabPage12.Controls.Add(Me.Peak_Injury_Euro_R)
        Me.TabPage12.Location = New System.Drawing.Point(4, 4)
        Me.TabPage12.Name = "TabPage12"
        Me.TabPage12.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage12.Size = New System.Drawing.Size(248, 71)
        Me.TabPage12.TabIndex = 1
        Me.TabPage12.Text = "Rear"
        Me.TabPage12.UseVisualStyleBackColor = True
        '
        'TabPage10
        '
        Me.TabPage10.Controls.Add(Me.Peak_Injury_Euro_O)
        Me.TabPage10.Controls.Add(Me.InjuryLbl8)
        Me.TabPage10.Location = New System.Drawing.Point(4, 24)
        Me.TabPage10.Name = "TabPage10"
        Me.TabPage10.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage10.Size = New System.Drawing.Size(724, 131)
        Me.TabPage10.TabIndex = 7
        Me.TabPage10.Text = "TabPage10"
        Me.TabPage10.UseVisualStyleBackColor = True
        '
        'ChkDropMenu
        '
        Me.ChkDropMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FindDropToolStripMenuItem})
        Me.ChkDropMenu.Name = "ChkDropMenu"
        Me.ChkDropMenu.Size = New System.Drawing.Size(163, 26)
        '
        'FindDropToolStripMenuItem
        '
        Me.FindDropToolStripMenuItem.Name = "FindDropToolStripMenuItem"
        Me.FindDropToolStripMenuItem.Size = New System.Drawing.Size(162, 22)
        Me.FindDropToolStripMenuItem.Text = "Find Drop / Rise"
        '
        'AddFileDlg
        '
        Me.AddFileDlg.FileName = "OpenFileDialog1"
        '
        'InjuryLbl1
        '
        Me.InjuryLbl1.Location = New System.Drawing.Point(17, 12)
        Me.InjuryLbl1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.InjuryLbl1.Name = "InjuryLbl1"
        Me.InjuryLbl1.OcxState = CType(resources.GetObject("InjuryLbl1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.InjuryLbl1.Size = New System.Drawing.Size(379, 70)
        Me.InjuryLbl1.TabIndex = 1
        '
        'Peak_Injury1
        '
        Me.Peak_Injury1.Location = New System.Drawing.Point(10, 72)
        Me.Peak_Injury1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Peak_Injury1.Name = "Peak_Injury1"
        Me.Peak_Injury1.OcxState = CType(resources.GetObject("Peak_Injury1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.Peak_Injury1.Size = New System.Drawing.Size(430, 57)
        Me.Peak_Injury1.TabIndex = 0
        '
        'Peak_Injury_DOM_F
        '
        Me.Peak_Injury_DOM_F.Location = New System.Drawing.Point(20, 102)
        Me.Peak_Injury_DOM_F.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Peak_Injury_DOM_F.Name = "Peak_Injury_DOM_F"
        Me.Peak_Injury_DOM_F.OcxState = CType(resources.GetObject("Peak_Injury_DOM_F.OcxState"), System.Windows.Forms.AxHost.State)
        Me.Peak_Injury_DOM_F.Size = New System.Drawing.Size(700, 100)
        Me.Peak_Injury_DOM_F.TabIndex = 1
        '
        'InjuryLbl2
        '
        Me.InjuryLbl2.Location = New System.Drawing.Point(13, 13)
        Me.InjuryLbl2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.InjuryLbl2.Name = "InjuryLbl2"
        Me.InjuryLbl2.OcxState = CType(resources.GetObject("InjuryLbl2.OcxState"), System.Windows.Forms.AxHost.State)
        Me.InjuryLbl2.Size = New System.Drawing.Size(661, 86)
        Me.InjuryLbl2.TabIndex = 0
        '
        'Peak_Injury_DOM_O
        '
        Me.Peak_Injury_DOM_O.Location = New System.Drawing.Point(15, 110)
        Me.Peak_Injury_DOM_O.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Peak_Injury_DOM_O.Name = "Peak_Injury_DOM_O"
        Me.Peak_Injury_DOM_O.OcxState = CType(resources.GetObject("Peak_Injury_DOM_O.OcxState"), System.Windows.Forms.AxHost.State)
        Me.Peak_Injury_DOM_O.Size = New System.Drawing.Size(627, 81)
        Me.Peak_Injury_DOM_O.TabIndex = 1
        '
        'InjuryLbl3
        '
        Me.InjuryLbl3.Location = New System.Drawing.Point(18, 16)
        Me.InjuryLbl3.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.InjuryLbl3.Name = "InjuryLbl3"
        Me.InjuryLbl3.OcxState = CType(resources.GetObject("InjuryLbl3.OcxState"), System.Windows.Forms.AxHost.State)
        Me.InjuryLbl3.Size = New System.Drawing.Size(625, 93)
        Me.InjuryLbl3.TabIndex = 0
        '
        'Peak_Injury_CHINA_F
        '
        Me.Peak_Injury_CHINA_F.Location = New System.Drawing.Point(345, 37)
        Me.Peak_Injury_CHINA_F.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Peak_Injury_CHINA_F.Name = "Peak_Injury_CHINA_F"
        Me.Peak_Injury_CHINA_F.OcxState = CType(resources.GetObject("Peak_Injury_CHINA_F.OcxState"), System.Windows.Forms.AxHost.State)
        Me.Peak_Injury_CHINA_F.Size = New System.Drawing.Size(290, 57)
        Me.Peak_Injury_CHINA_F.TabIndex = 1
        '
        'InjuryLbl4
        '
        Me.InjuryLbl4.Location = New System.Drawing.Point(35, 21)
        Me.InjuryLbl4.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.InjuryLbl4.Name = "InjuryLbl4"
        Me.InjuryLbl4.OcxState = CType(resources.GetObject("InjuryLbl4.OcxState"), System.Windows.Forms.AxHost.State)
        Me.InjuryLbl4.Size = New System.Drawing.Size(261, 64)
        Me.InjuryLbl4.TabIndex = 0
        '
        'Peak_Injury_CHINA_O
        '
        Me.Peak_Injury_CHINA_O.Location = New System.Drawing.Point(11, 70)
        Me.Peak_Injury_CHINA_O.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Peak_Injury_CHINA_O.Name = "Peak_Injury_CHINA_O"
        Me.Peak_Injury_CHINA_O.OcxState = CType(resources.GetObject("Peak_Injury_CHINA_O.OcxState"), System.Windows.Forms.AxHost.State)
        Me.Peak_Injury_CHINA_O.Size = New System.Drawing.Size(420, 61)
        Me.Peak_Injury_CHINA_O.TabIndex = 1
        '
        'InjuryLbl5
        '
        Me.InjuryLbl5.Location = New System.Drawing.Point(11, 4)
        Me.InjuryLbl5.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.InjuryLbl5.Name = "InjuryLbl5"
        Me.InjuryLbl5.OcxState = CType(resources.GetObject("InjuryLbl5.OcxState"), System.Windows.Forms.AxHost.State)
        Me.InjuryLbl5.Size = New System.Drawing.Size(301, 80)
        Me.InjuryLbl5.TabIndex = 0
        '
        'Peak_Injury_CHINA_F_Rear
        '
        Me.Peak_Injury_CHINA_F_Rear.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Peak_Injury_CHINA_F_Rear.Location = New System.Drawing.Point(3, 3)
        Me.Peak_Injury_CHINA_F_Rear.Name = "Peak_Injury_CHINA_F_Rear"
        Me.Peak_Injury_CHINA_F_Rear.OcxState = CType(resources.GetObject("Peak_Injury_CHINA_F_Rear.OcxState"), System.Windows.Forms.AxHost.State)
        Me.Peak_Injury_CHINA_F_Rear.Size = New System.Drawing.Size(289, 55)
        Me.Peak_Injury_CHINA_F_Rear.TabIndex = 1
        '
        'Peak_Injury_CHINA_O_Rear
        '
        Me.Peak_Injury_CHINA_O_Rear.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Peak_Injury_CHINA_O_Rear.Location = New System.Drawing.Point(3, 3)
        Me.Peak_Injury_CHINA_O_Rear.Name = "Peak_Injury_CHINA_O_Rear"
        Me.Peak_Injury_CHINA_O_Rear.OcxState = CType(resources.GetObject("Peak_Injury_CHINA_O_Rear.OcxState"), System.Windows.Forms.AxHost.State)
        Me.Peak_Injury_CHINA_O_Rear.Size = New System.Drawing.Size(289, 57)
        Me.Peak_Injury_CHINA_O_Rear.TabIndex = 0
        '
        'InjuryLbl6
        '
        Me.InjuryLbl6.Location = New System.Drawing.Point(15, 13)
        Me.InjuryLbl6.Name = "InjuryLbl6"
        Me.InjuryLbl6.OcxState = CType(resources.GetObject("InjuryLbl6.OcxState"), System.Windows.Forms.AxHost.State)
        Me.InjuryLbl6.Size = New System.Drawing.Size(194, 54)
        Me.InjuryLbl6.TabIndex = 0
        '
        'Peak_Injury_Euro_F
        '
        Me.Peak_Injury_Euro_F.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Peak_Injury_Euro_F.Location = New System.Drawing.Point(3, 3)
        Me.Peak_Injury_Euro_F.Name = "Peak_Injury_Euro_F"
        Me.Peak_Injury_Euro_F.OcxState = CType(resources.GetObject("Peak_Injury_Euro_F.OcxState"), System.Windows.Forms.AxHost.State)
        Me.Peak_Injury_Euro_F.Size = New System.Drawing.Size(242, 65)
        Me.Peak_Injury_Euro_F.TabIndex = 0
        '
        'Peak_Injury_Euro_R
        '
        Me.Peak_Injury_Euro_R.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Peak_Injury_Euro_R.Location = New System.Drawing.Point(3, 3)
        Me.Peak_Injury_Euro_R.Name = "Peak_Injury_Euro_R"
        Me.Peak_Injury_Euro_R.OcxState = CType(resources.GetObject("Peak_Injury_Euro_R.OcxState"), System.Windows.Forms.AxHost.State)
        Me.Peak_Injury_Euro_R.Size = New System.Drawing.Size(242, 67)
        Me.Peak_Injury_Euro_R.TabIndex = 0
        '
        'InjuryLbl7
        '
        Me.InjuryLbl7.Location = New System.Drawing.Point(53, 19)
        Me.InjuryLbl7.Name = "InjuryLbl7"
        Me.InjuryLbl7.OcxState = CType(resources.GetObject("InjuryLbl7.OcxState"), System.Windows.Forms.AxHost.State)
        Me.InjuryLbl7.Size = New System.Drawing.Size(193, 32)
        Me.InjuryLbl7.TabIndex = 0
        '
        'Peak_Injury_Euro_O
        '
        Me.Peak_Injury_Euro_O.Location = New System.Drawing.Point(321, 32)
        Me.Peak_Injury_Euro_O.Name = "Peak_Injury_Euro_O"
        Me.Peak_Injury_Euro_O.OcxState = CType(resources.GetObject("Peak_Injury_Euro_O.OcxState"), System.Windows.Forms.AxHost.State)
        Me.Peak_Injury_Euro_O.Size = New System.Drawing.Size(192, 60)
        Me.Peak_Injury_Euro_O.TabIndex = 1
        '
        'InjuryLbl8
        '
        Me.InjuryLbl8.Location = New System.Drawing.Point(57, 28)
        Me.InjuryLbl8.Name = "InjuryLbl8"
        Me.InjuryLbl8.OcxState = CType(resources.GetObject("InjuryLbl8.OcxState"), System.Windows.Forms.AxHost.State)
        Me.InjuryLbl8.Size = New System.Drawing.Size(178, 43)
        Me.InjuryLbl8.TabIndex = 0
        '
        'InjuryDisplayFrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(795, 343)
        Me.Controls.Add(Me.SplitConMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.MaximizeBox = False
        Me.Name = "InjuryDisplayFrm"
        Me.Text = "Form1"
        Me.OpenWithXMADgic.ResumeLayout(False)
        Me.SplitConMain.Panel1.ResumeLayout(False)
        Me.SplitConMain.Panel1.PerformLayout()
        Me.SplitConMain.Panel2.ResumeLayout(False)
        CType(Me.SplitConMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitConMain.ResumeLayout(False)
        Me.ScoreBoard.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage6.ResumeLayout(False)
        Me.CNCAP_Rear.ResumeLayout(False)
        Me.TabPage7.ResumeLayout(False)
        Me.TabPage8.ResumeLayout(False)
        Me.TabPage9.ResumeLayout(False)
        Me.EuroFrontal.ResumeLayout(False)
        Me.TabPage11.ResumeLayout(False)
        Me.TabPage12.ResumeLayout(False)
        Me.TabPage10.ResumeLayout(False)
        Me.ChkDropMenu.ResumeLayout(False)
        CType(Me.InjuryLbl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Peak_Injury1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Peak_Injury_DOM_F, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InjuryLbl2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Peak_Injury_DOM_O, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InjuryLbl3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Peak_Injury_CHINA_F, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InjuryLbl4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Peak_Injury_CHINA_O, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InjuryLbl5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Peak_Injury_CHINA_F_Rear, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Peak_Injury_CHINA_O_Rear, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InjuryLbl6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Peak_Injury_Euro_F, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Peak_Injury_Euro_R, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InjuryLbl7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Peak_Injury_Euro_O, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InjuryLbl8, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents OpenDlg As System.Windows.Forms.OpenFileDialog
    Friend WithEvents BtnValGraph As System.Windows.Forms.Button
    Friend WithEvents BtnInjuryGraph As System.Windows.Forms.Button
    Friend WithEvents PPTBtn As System.Windows.Forms.Button
    Friend WithEvents ReloadBtn As System.Windows.Forms.Button
    Friend WithEvents PPTmenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents OpenWithXMADgic As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents OpenWithXMADgicToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenKn3WithHyperViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitConMain As System.Windows.Forms.SplitContainer
    Friend WithEvents PathLbl As System.Windows.Forms.Label
    Friend WithEvents OpenLst As System.Windows.Forms.ListBox
    Friend WithEvents ScoreBoard As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents InjuryLbl1 As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents Peak_Injury1 As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Peak_Injury_DOM_F As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents InjuryLbl2 As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents Peak_Injury_DOM_O As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents InjuryLbl3 As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents Peak_Injury_CHINA_F As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents InjuryLbl4 As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents Peak_Injury_CHINA_O As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents InjuryLbl5 As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents Peak_Injury_CHINA_F_Rear As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents InjuryLbl6 As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents CNCAP_Rear As System.Windows.Forms.TabControl
    Friend WithEvents TabPage7 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage8 As System.Windows.Forms.TabPage
    Friend WithEvents Peak_Injury_CHINA_O_Rear As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents ChkDropMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents FindDropToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BtnFind As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents AddFileDlg As System.Windows.Forms.OpenFileDialog
    Friend WithEvents DescripTxt As System.Windows.Forms.TextBox
    Friend WithEvents CommentsToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents TabPage9 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage10 As System.Windows.Forms.TabPage
    Friend WithEvents InjuryLbl7 As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents Peak_Injury_Euro_O As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents InjuryLbl8 As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents EuroFrontal As System.Windows.Forms.TabControl
    Friend WithEvents TabPage11 As System.Windows.Forms.TabPage
    Friend WithEvents Peak_Injury_Euro_F As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents TabPage12 As System.Windows.Forms.TabPage
    Friend WithEvents Peak_Injury_Euro_R As AxMSFlexGridLib.AxMSFlexGrid
End Class
