<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainMDI
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainMDI))
        Me.MainMenu = New System.Windows.Forms.MenuStrip()
        Me.시작ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.열기ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.USNCAPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.THORRMDBToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.종료EToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.시험ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.불러오기ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenTHORTToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.설정ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.해석데이터ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetDataProfileTHORToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GeneralSetUpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoteConnectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.기타기능EToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FEConverterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LSDYNAToMADYMOToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MADYMOToLSDYNAToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LSPREPOSTOuputToMADYMONotYetToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NodeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MADYMOPSMFileConverterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BatchRun설치ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CommandPromptWindowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FileCommentBrowserToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.정보IToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.Statuslbl = New System.Windows.Forms.ToolStripStatusLabel()
        Me.RemoteConnectLbl = New System.Windows.Forms.ToolStripStatusLabel()
        Me.UserDomainName = New System.Windows.Forms.ToolStripStatusLabel()
        Me.UserConfigure = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ProgressBarMain = New System.Windows.Forms.ToolStripProgressBar()
        Me.DeveloperLbl = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MainTool = New System.Windows.Forms.ToolStrip()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolBtnNCAP = New System.Windows.Forms.ToolStripButton()
        Me.ToolBtnTHOR = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolBtnTEST = New System.Windows.Forms.ToolStripButton()
        Me.ToolBtnTEST_THOR = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.RunXMADgic = New System.Windows.Forms.ToolStripButton()
        Me.RunHyperView = New System.Windows.Forms.ToolStripButton()
        Me.CorrelOpenFile = New System.Windows.Forms.OpenFileDialog()
        Me.MainToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.XMADgicOpn = New System.Windows.Forms.OpenFileDialog()
        Me.NodeExcludingDlg = New System.Windows.Forms.OpenFileDialog()
        Me.MainMenu.SuspendLayout()
        Me.StatusStrip.SuspendLayout()
        Me.MainTool.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu
        '
        Me.MainMenu.BackColor = System.Drawing.SystemColors.Control
        Me.MainMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.시작ToolStripMenuItem, Me.시험ToolStripMenuItem, Me.설정ToolStripMenuItem, Me.기타기능EToolStripMenuItem, Me.정보IToolStripMenuItem})
        Me.MainMenu.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu.Name = "MainMenu"
        Me.MainMenu.Padding = New System.Windows.Forms.Padding(5, 2, 0, 2)
        Me.MainMenu.Size = New System.Drawing.Size(777, 24)
        Me.MainMenu.TabIndex = 1
        Me.MainMenu.Text = "MainMenu"
        '
        '시작ToolStripMenuItem
        '
        Me.시작ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.열기ToolStripMenuItem, Me.종료EToolStripMenuItem})
        Me.시작ToolStripMenuItem.Name = "시작ToolStripMenuItem"
        Me.시작ToolStripMenuItem.Size = New System.Drawing.Size(107, 20)
        Me.시작ToolStripMenuItem.Text = "Analysis Data(&A)"
        '
        '열기ToolStripMenuItem
        '
        Me.열기ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.USNCAPToolStripMenuItem, Me.THORRMDBToolStripMenuItem})
        Me.열기ToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.CSH
        Me.열기ToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White
        Me.열기ToolStripMenuItem.Name = "열기ToolStripMenuItem"
        Me.열기ToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.열기ToolStripMenuItem.Text = "Open(&O)"
        '
        'USNCAPToolStripMenuItem
        '
        Me.USNCAPToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.FacetDummy
        Me.USNCAPToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White
        Me.USNCAPToolStripMenuItem.Name = "USNCAPToolStripMenuItem"
        Me.USNCAPToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
        Me.USNCAPToolStripMenuItem.Text = "Frontal && Offset NCAP"
        Me.USNCAPToolStripMenuItem.ToolTipText = "1. Click : Open MADYMO Result" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2. Ctrl + Click : Apply Correlation Factor" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    - " & _
            "Setting - Correl. Factor" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "3. Shift + Click : Graph Based Correl."
        '
        'THORRMDBToolStripMenuItem
        '
        Me.THORRMDBToolStripMenuItem.Enabled = False
        Me.THORRMDBToolStripMenuItem.Image = CType(resources.GetObject("THORRMDBToolStripMenuItem.Image"), System.Drawing.Image)
        Me.THORRMDBToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White
        Me.THORRMDBToolStripMenuItem.Name = "THORRMDBToolStripMenuItem"
        Me.THORRMDBToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
        Me.THORRMDBToolStripMenuItem.Text = "RMDB [THOR]"
        Me.THORRMDBToolStripMenuItem.ToolTipText = "1. Click : Open MADYMO Result" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2. Ctrl + Click : Apply Correlation Factor" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    - " & _
            "Setting - Correl. Factor"
        Me.THORRMDBToolStripMenuItem.Visible = False
        '
        '종료EToolStripMenuItem
        '
        Me.종료EToolStripMenuItem.Name = "종료EToolStripMenuItem"
        Me.종료EToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.종료EToolStripMenuItem.Text = "Exit(&E)"
        '
        '시험ToolStripMenuItem
        '
        Me.시험ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.불러오기ToolStripMenuItem, Me.OpenTHORTToolStripMenuItem})
        Me.시험ToolStripMenuItem.Name = "시험ToolStripMenuItem"
        Me.시험ToolStripMenuItem.Size = New System.Drawing.Size(87, 20)
        Me.시험ToolStripMenuItem.Text = "TEST Data(&T)"
        '
        '불러오기ToolStripMenuItem
        '
        Me.불러오기ToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.ANADlgChnHICCalc
        Me.불러오기ToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White
        Me.불러오기ToolStripMenuItem.Name = "불러오기ToolStripMenuItem"
        Me.불러오기ToolStripMenuItem.Size = New System.Drawing.Size(190, 22)
        Me.불러오기ToolStripMenuItem.Text = "Open [HYBRID-Ⅲ](&H)"
        Me.불러오기ToolStripMenuItem.ToolTipText = "TEST DATA Reading" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  - Header File : DAT ASCII format / TDM format" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  - DATA File" & _
            " : Little Endian Binary file [R64/tdx]" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "※ Use lower case extension for tdm files" & _
            "."
        '
        'OpenTHORTToolStripMenuItem
        '
        Me.OpenTHORTToolStripMenuItem.Enabled = False
        Me.OpenTHORTToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.THOR_TEST_img
        Me.OpenTHORTToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White
        Me.OpenTHORTToolStripMenuItem.Name = "OpenTHORTToolStripMenuItem"
        Me.OpenTHORTToolStripMenuItem.Size = New System.Drawing.Size(190, 22)
        Me.OpenTHORTToolStripMenuItem.Text = "Open [THOR](&T)"
        Me.OpenTHORTToolStripMenuItem.ToolTipText = "TEST DATA Reading" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  - Header File : DAT ASCII format / TDM format" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  - DATA File" & _
            " : Little Endian Binary file [R64/tdx]" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "※ Use lower case extension for tdm files" & _
            "."
        Me.OpenTHORTToolStripMenuItem.Visible = False
        '
        '설정ToolStripMenuItem
        '
        Me.설정ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.해석데이터ToolStripMenuItem, Me.SetDataProfileTHORToolStripMenuItem, Me.GeneralSetUpToolStripMenuItem, Me.RemoteConnectionToolStripMenuItem})
        Me.설정ToolStripMenuItem.Name = "설정ToolStripMenuItem"
        Me.설정ToolStripMenuItem.Size = New System.Drawing.Size(72, 20)
        Me.설정ToolStripMenuItem.Text = "Setting(&P)"
        '
        '해석데이터ToolStripMenuItem
        '
        Me.해석데이터ToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.setting_1
        Me.해석데이터ToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White
        Me.해석데이터ToolStripMenuItem.Name = "해석데이터ToolStripMenuItem"
        Me.해석데이터ToolStripMenuItem.Size = New System.Drawing.Size(229, 22)
        Me.해석데이터ToolStripMenuItem.Text = "Set Data Profile [Hybrid-3]"
        '
        'SetDataProfileTHORToolStripMenuItem
        '
        Me.SetDataProfileTHORToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.FacetDummy
        Me.SetDataProfileTHORToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White
        Me.SetDataProfileTHORToolStripMenuItem.Name = "SetDataProfileTHORToolStripMenuItem"
        Me.SetDataProfileTHORToolStripMenuItem.Size = New System.Drawing.Size(229, 22)
        Me.SetDataProfileTHORToolStripMenuItem.Text = "Set Data Profile [THOR]"
        Me.SetDataProfileTHORToolStripMenuItem.Visible = False
        '
        'GeneralSetUpToolStripMenuItem
        '
        Me.GeneralSetUpToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.SetUpGeneral
        Me.GeneralSetUpToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White
        Me.GeneralSetUpToolStripMenuItem.Name = "GeneralSetUpToolStripMenuItem"
        Me.GeneralSetUpToolStripMenuItem.Size = New System.Drawing.Size(229, 22)
        Me.GeneralSetUpToolStripMenuItem.Text = "General Set Up"
        '
        'RemoteConnectionToolStripMenuItem
        '
        Me.RemoteConnectionToolStripMenuItem.ForeColor = System.Drawing.Color.Red
        Me.RemoteConnectionToolStripMenuItem.Name = "RemoteConnectionToolStripMenuItem"
        Me.RemoteConnectionToolStripMenuItem.Size = New System.Drawing.Size(229, 22)
        Me.RemoteConnectionToolStripMenuItem.Text = "Remote Connection [Closed]"
        '
        '기타기능EToolStripMenuItem
        '
        Me.기타기능EToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FEConverterToolStripMenuItem, Me.MADYMOPSMFileConverterToolStripMenuItem, Me.BatchRun설치ToolStripMenuItem, Me.CommandPromptWindowToolStripMenuItem, Me.FileCommentBrowserToolStripMenuItem})
        Me.기타기능EToolStripMenuItem.Enabled = False
        Me.기타기능EToolStripMenuItem.Name = "기타기능EToolStripMenuItem"
        Me.기타기능EToolStripMenuItem.Size = New System.Drawing.Size(72, 20)
        Me.기타기능EToolStripMenuItem.Text = "Utilities(&E)"
        Me.기타기능EToolStripMenuItem.Visible = False
        '
        'FEConverterToolStripMenuItem
        '
        Me.FEConverterToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LSDYNAToMADYMOToolStripMenuItem, Me.MADYMOToLSDYNAToolStripMenuItem, Me.LSPREPOSTOuputToMADYMONotYetToolStripMenuItem, Me.NodeToolStripMenuItem})
        Me.FEConverterToolStripMenuItem.Enabled = False
        Me.FEConverterToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.Cabin
        Me.FEConverterToolStripMenuItem.Name = "FEConverterToolStripMenuItem"
        Me.FEConverterToolStripMenuItem.Size = New System.Drawing.Size(236, 22)
        Me.FEConverterToolStripMenuItem.Text = "FE Converter"
        Me.FEConverterToolStripMenuItem.ToolTipText = "Converting LS-DYNA Key File Input to XML"
        Me.FEConverterToolStripMenuItem.Visible = False
        '
        'LSDYNAToMADYMOToolStripMenuItem
        '
        Me.LSDYNAToMADYMOToolStripMenuItem.Name = "LSDYNAToMADYMOToolStripMenuItem"
        Me.LSDYNAToMADYMOToolStripMenuItem.Size = New System.Drawing.Size(323, 22)
        Me.LSDYNAToMADYMOToolStripMenuItem.Text = "LS-DYNA to MADYMO"
        Me.LSDYNAToMADYMOToolStripMenuItem.ToolTipText = "Converting LS-DYNA to MADYMO (Partial)"
        '
        'MADYMOToLSDYNAToolStripMenuItem
        '
        Me.MADYMOToLSDYNAToolStripMenuItem.Name = "MADYMOToLSDYNAToolStripMenuItem"
        Me.MADYMOToLSDYNAToolStripMenuItem.Size = New System.Drawing.Size(323, 22)
        Me.MADYMOToLSDYNAToolStripMenuItem.Text = "MADYMO to LS-DYNA (Mesh Only)"
        Me.MADYMOToLSDYNAToolStripMenuItem.ToolTipText = "Converting MADYMO FE Mesh in SYSTEM to LS-DYNA"
        '
        'LSPREPOSTOuputToMADYMONotYetToolStripMenuItem
        '
        Me.LSPREPOSTOuputToMADYMONotYetToolStripMenuItem.Name = "LSPREPOSTOuputToMADYMONotYetToolStripMenuItem"
        Me.LSPREPOSTOuputToMADYMONotYetToolStripMenuItem.Size = New System.Drawing.Size(323, 22)
        Me.LSPREPOSTOuputToMADYMONotYetToolStripMenuItem.Text = "LS-PREPOST Ouput to MADYMO (Mesh Only)"
        Me.LSPREPOSTOuputToMADYMONotYetToolStripMenuItem.ToolTipText = "Extract Mesh and Part definition From PSM key file"
        '
        'NodeToolStripMenuItem
        '
        Me.NodeToolStripMenuItem.Name = "NodeToolStripMenuItem"
        Me.NodeToolStripMenuItem.Size = New System.Drawing.Size(323, 22)
        Me.NodeToolStripMenuItem.Text = "Node Excluding [In PSM]"
        Me.NodeToolStripMenuItem.ToolTipText = "Excluding *SET_NODE_LIST from PSM File"
        '
        'MADYMOPSMFileConverterToolStripMenuItem
        '
        Me.MADYMOPSMFileConverterToolStripMenuItem.Enabled = False
        Me.MADYMOPSMFileConverterToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.Madymo
        Me.MADYMOPSMFileConverterToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White
        Me.MADYMOPSMFileConverterToolStripMenuItem.Name = "MADYMOPSMFileConverterToolStripMenuItem"
        Me.MADYMOPSMFileConverterToolStripMenuItem.Size = New System.Drawing.Size(236, 22)
        Me.MADYMOPSMFileConverterToolStripMenuItem.Text = "MADYMO PSM File Converter"
        Me.MADYMOPSMFileConverterToolStripMenuItem.ToolTipText = "Converting from LS-DYNA time history key file to MADYMO PSM file"
        Me.MADYMOPSMFileConverterToolStripMenuItem.Visible = False
        '
        'BatchRun설치ToolStripMenuItem
        '
        Me.BatchRun설치ToolStripMenuItem.Name = "BatchRun설치ToolStripMenuItem"
        Me.BatchRun설치ToolStripMenuItem.Size = New System.Drawing.Size(236, 22)
        Me.BatchRun설치ToolStripMenuItem.Text = "Batch Job Run"
        '
        'CommandPromptWindowToolStripMenuItem
        '
        Me.CommandPromptWindowToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.CommandPrompt
        Me.CommandPromptWindowToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White
        Me.CommandPromptWindowToolStripMenuItem.Name = "CommandPromptWindowToolStripMenuItem"
        Me.CommandPromptWindowToolStripMenuItem.Size = New System.Drawing.Size(236, 22)
        Me.CommandPromptWindowToolStripMenuItem.Text = "Command Prompt Window"
        '
        'FileCommentBrowserToolStripMenuItem
        '
        Me.FileCommentBrowserToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.FileCommentBrowser
        Me.FileCommentBrowserToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White
        Me.FileCommentBrowserToolStripMenuItem.Name = "FileCommentBrowserToolStripMenuItem"
        Me.FileCommentBrowserToolStripMenuItem.Size = New System.Drawing.Size(236, 22)
        Me.FileCommentBrowserToolStripMenuItem.Text = "File Comment Browser"
        '
        '정보IToolStripMenuItem
        '
        Me.정보IToolStripMenuItem.Name = "정보IToolStripMenuItem"
        Me.정보IToolStripMenuItem.Size = New System.Drawing.Size(54, 20)
        Me.정보IToolStripMenuItem.Text = "Info.(&I)"
        '
        'StatusStrip
        '
        Me.StatusStrip.AutoSize = False
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Statuslbl, Me.RemoteConnectLbl, Me.UserDomainName, Me.UserConfigure, Me.ProgressBarMain, Me.DeveloperLbl})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 374)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Padding = New System.Windows.Forms.Padding(1, 0, 12, 0)
        Me.StatusStrip.Size = New System.Drawing.Size(777, 30)
        Me.StatusStrip.SizingGrip = False
        Me.StatusStrip.TabIndex = 2
        Me.StatusStrip.Text = "StatusStrip1"
        '
        'Statuslbl
        '
        Me.Statuslbl.AutoSize = False
        Me.Statuslbl.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.Statuslbl.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Statuslbl.Name = "Statuslbl"
        Me.Statuslbl.Size = New System.Drawing.Size(162, 25)
        Me.Statuslbl.Spring = True
        Me.Statuslbl.Text = "Status"
        Me.Statuslbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'RemoteConnectLbl
        '
        Me.RemoteConnectLbl.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.RemoteConnectLbl.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.RemoteConnectLbl.ForeColor = System.Drawing.Color.Red
        Me.RemoteConnectLbl.Name = "RemoteConnectLbl"
        Me.RemoteConnectLbl.Size = New System.Drawing.Size(99, 25)
        Me.RemoteConnectLbl.Text = "Not Connected"
        '
        'UserDomainName
        '
        Me.UserDomainName.AutoSize = False
        Me.UserDomainName.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.UserDomainName.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.UserDomainName.ForeColor = System.Drawing.Color.Blue
        Me.UserDomainName.Name = "UserDomainName"
        Me.UserDomainName.Size = New System.Drawing.Size(100, 25)
        Me.UserDomainName.Text = "HKMC"
        '
        'UserConfigure
        '
        Me.UserConfigure.AutoSize = False
        Me.UserConfigure.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.UserConfigure.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.UserConfigure.Name = "UserConfigure"
        Me.UserConfigure.Size = New System.Drawing.Size(100, 25)
        Me.UserConfigure.Text = "6002317"
        '
        'ProgressBarMain
        '
        Me.ProgressBarMain.Maximum = 1140
        Me.ProgressBarMain.Name = "ProgressBarMain"
        Me.ProgressBarMain.Size = New System.Drawing.Size(200, 24)
        Me.ProgressBarMain.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        '
        'DeveloperLbl
        '
        Me.DeveloperLbl.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.DeveloperLbl.Name = "DeveloperLbl"
        Me.DeveloperLbl.Size = New System.Drawing.Size(101, 25)
        Me.DeveloperLbl.Text = "Sunghun, Chang"
        Me.DeveloperLbl.ToolTipText = "Developer : 장성훈 연구원"
        '
        'MainTool
        '
        Me.MainTool.Dock = System.Windows.Forms.DockStyle.Left
        Me.MainTool.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.MainTool.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripSeparator1, Me.ToolBtnNCAP, Me.ToolBtnTHOR, Me.ToolStripSeparator3, Me.ToolBtnTEST, Me.ToolBtnTEST_THOR, Me.ToolStripSeparator2, Me.RunXMADgic, Me.RunHyperView})
        Me.MainTool.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.MainTool.Location = New System.Drawing.Point(0, 24)
        Me.MainTool.Name = "MainTool"
        Me.MainTool.Size = New System.Drawing.Size(24, 350)
        Me.MainTool.TabIndex = 4
        Me.MainTool.Text = "ToolStrip1"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(21, 6)
        '
        'ToolBtnNCAP
        '
        Me.ToolBtnNCAP.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolBtnNCAP.Image = CType(resources.GetObject("ToolBtnNCAP.Image"), System.Drawing.Image)
        Me.ToolBtnNCAP.ImageTransparentColor = System.Drawing.SystemColors.Window
        Me.ToolBtnNCAP.Name = "ToolBtnNCAP"
        Me.ToolBtnNCAP.Size = New System.Drawing.Size(21, 20)
        Me.ToolBtnNCAP.Text = "정면/옵셋 상품성"
        Me.ToolBtnNCAP.ToolTipText = "1. Click : Open MADYMO Result" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2. Ctrl + Click : Apply Correlation Factor" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    - " & _
            "Setting - Correl. Factor" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "3. Shift + Click : Graph Based Correl." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "4. Alt + Click" & _
            " : Remote Folder Open"
        '
        'ToolBtnTHOR
        '
        Me.ToolBtnTHOR.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolBtnTHOR.Enabled = False
        Me.ToolBtnTHOR.Image = CType(resources.GetObject("ToolBtnTHOR.Image"), System.Drawing.Image)
        Me.ToolBtnTHOR.ImageTransparentColor = System.Drawing.Color.White
        Me.ToolBtnTHOR.Name = "ToolBtnTHOR"
        Me.ToolBtnTHOR.Size = New System.Drawing.Size(21, 20)
        Me.ToolBtnTHOR.Text = "ToolStripButton1"
        Me.ToolBtnTHOR.ToolTipText = "1. Click : Open MADYMO Result" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2. Ctrl + Click : Apply Correlation Factor" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    - " & _
            "Setting - Correl. Factor" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "3. Alt + Click : Remote Folder Open"
        Me.ToolBtnTHOR.Visible = False
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(21, 6)
        '
        'ToolBtnTEST
        '
        Me.ToolBtnTEST.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolBtnTEST.Image = Global.WindowsApplication1.My.Resources.Resources.ANADlgChnHICCalc
        Me.ToolBtnTEST.ImageTransparentColor = System.Drawing.Color.White
        Me.ToolBtnTEST.Name = "ToolBtnTEST"
        Me.ToolBtnTEST.Size = New System.Drawing.Size(21, 20)
        Me.ToolBtnTEST.Text = "시험데이터 불러오기"
        Me.ToolBtnTEST.ToolTipText = "TEST DATA Reading" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  - Header File : DAT ASCII format / TDM format" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  - DATA File" & _
            " : Little Endian Binary file [R64/tdx]"
        '
        'ToolBtnTEST_THOR
        '
        Me.ToolBtnTEST_THOR.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolBtnTEST_THOR.Enabled = False
        Me.ToolBtnTEST_THOR.Image = Global.WindowsApplication1.My.Resources.Resources.THOR_TEST_img
        Me.ToolBtnTEST_THOR.ImageTransparentColor = System.Drawing.Color.White
        Me.ToolBtnTEST_THOR.Name = "ToolBtnTEST_THOR"
        Me.ToolBtnTEST_THOR.Size = New System.Drawing.Size(21, 20)
        Me.ToolBtnTEST_THOR.Text = "ToolStripButton1"
        Me.ToolBtnTEST_THOR.ToolTipText = "TEST DATA Reading" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  - Header File : DAT ASCII format / TDM format" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  - DATA File" & _
            " : Little Endian Binary file [R64/tdx]"
        Me.ToolBtnTEST_THOR.Visible = False
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(21, 6)
        '
        'RunXMADgic
        '
        Me.RunXMADgic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.RunXMADgic.Image = Global.WindowsApplication1.My.Resources.Resources.XMADgic
        Me.RunXMADgic.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.RunXMADgic.Name = "RunXMADgic"
        Me.RunXMADgic.Size = New System.Drawing.Size(21, 20)
        Me.RunXMADgic.Text = "Run XMADgic"
        '
        'RunHyperView
        '
        Me.RunHyperView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.RunHyperView.Image = CType(resources.GetObject("RunHyperView.Image"), System.Drawing.Image)
        Me.RunHyperView.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.RunHyperView.Name = "RunHyperView"
        Me.RunHyperView.Size = New System.Drawing.Size(21, 20)
        Me.RunHyperView.Text = "Run HyperView"
        '
        'CorrelOpenFile
        '
        Me.CorrelOpenFile.FileName = "OpenFileDialog1"
        '
        'XMADgicOpn
        '
        Me.XMADgicOpn.FileName = "OpenFileDialog1"
        '
        'NodeExcludingDlg
        '
        Me.NodeExcludingDlg.FileName = "OpenFileDialog1"
        '
        'MainMDI
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(777, 404)
        Me.Controls.Add(Me.MainTool)
        Me.Controls.Add(Me.StatusStrip)
        Me.Controls.Add(Me.MainMenu)
        Me.Font = New System.Drawing.Font("굴림", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.MainMenu
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "MainMDI"
        Me.Text = "MADYMO Data Viewer {0}.{1:00}"
        Me.TransparencyKey = System.Drawing.SystemColors.Control
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MainMenu.ResumeLayout(False)
        Me.MainMenu.PerformLayout()
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.MainTool.ResumeLayout(False)
        Me.MainTool.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MainMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents 시작ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents Statuslbl As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents 시험ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 설정ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 해석데이터ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 불러오기ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 열기ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 종료EToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents USNCAPToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainTool As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolBtnNCAP As System.Windows.Forms.ToolStripButton
    Friend WithEvents ProgressBarMain As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents 기타기능EToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MADYMOPSMFileConverterToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolBtnTEST As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BatchRun설치ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UserConfigure As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents UserDomainName As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents 정보IToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CorrelOpenFile As System.Windows.Forms.OpenFileDialog
    Friend WithEvents FEConverterToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LSDYNAToMADYMOToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LSPREPOSTOuputToMADYMONotYetToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents RunXMADgic As System.Windows.Forms.ToolStripButton
    Friend WithEvents XMADgicOpn As System.Windows.Forms.OpenFileDialog
    Friend WithEvents RunHyperView As System.Windows.Forms.ToolStripButton
    Friend WithEvents NodeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NodeExcludingDlg As System.Windows.Forms.OpenFileDialog
    Friend WithEvents CommandPromptWindowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetDataProfileTHORToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents THORRMDBToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FileCommentBrowserToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolBtnTHOR As System.Windows.Forms.ToolStripButton
    Friend WithEvents OpenTHORTToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolBtnTEST_THOR As System.Windows.Forms.ToolStripButton
    Friend WithEvents DeveloperLbl As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents MADYMOToLSDYNAToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GeneralSetUpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoteConnectionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoteConnectLbl As System.Windows.Forms.ToolStripStatusLabel

End Class
