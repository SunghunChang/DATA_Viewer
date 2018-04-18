<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmPSMexport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmPSMexport))
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.SelMainFile = New System.Windows.Forms.Button()
        Me.DescriptionLbl = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.Start = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.UnitLbl = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TxtZscale = New System.Windows.Forms.TextBox()
        Me.TxtYscale = New System.Windows.Forms.TextBox()
        Me.TxtXscale = New System.Windows.Forms.TextBox()
        Me.UnitConverter = New System.Windows.Forms.TextBox()
        Me.ChkDefault = New System.Windows.Forms.CheckBox()
        Me.SaveFileDlg = New System.Windows.Forms.SaveFileDialog()
        Me.ChkTESTuse = New System.Windows.Forms.CheckBox()
        Me.GraphX = New System.Windows.Forms.Button()
        Me.ModelAngleTxt = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.TxtToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.FileToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.AnalysisTab = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.RefBox = New System.Windows.Forms.GroupBox()
        Me.CmdZ = New System.Windows.Forms.Button()
        Me.TxtZcurve = New System.Windows.Forms.TextBox()
        Me.CmdY = New System.Windows.Forms.Button()
        Me.TxtYcurve = New System.Windows.Forms.TextBox()
        Me.CmdX = New System.Windows.Forms.Button()
        Me.TxtXcurve = New System.Windows.Forms.TextBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.RefBox_2 = New System.Windows.Forms.GroupBox()
        Me.CmdPitch = New System.Windows.Forms.Button()
        Me.CmdYawing = New System.Windows.Forms.Button()
        Me.TxtPitching = New System.Windows.Forms.TextBox()
        Me.TxtYawing = New System.Windows.Forms.TextBox()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.ExNODE_BOX = New System.Windows.Forms.GroupBox()
        Me.ChkExNODE = New System.Windows.Forms.CheckBox()
        Me.LblExNode = New System.Windows.Forms.Label()
        Me.CmdSelNODESET = New System.Windows.Forms.Button()
        Me.TxtNodeSET = New System.Windows.Forms.TextBox()
        Me.TESTTab = New System.Windows.Forms.TabControl()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.RefBoxTEST = New System.Windows.Forms.GroupBox()
        Me.CmdZTest = New System.Windows.Forms.Button()
        Me.TxtZcurveTEST = New System.Windows.Forms.TextBox()
        Me.CmdYTest = New System.Windows.Forms.Button()
        Me.TxtYcurveTEST = New System.Windows.Forms.TextBox()
        Me.CmdXTest = New System.Windows.Forms.Button()
        Me.TxtXcurveTEST = New System.Windows.Forms.TextBox()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.RefBoxTEST_2 = New System.Windows.Forms.GroupBox()
        Me.CmdPitchTEST = New System.Windows.Forms.Button()
        Me.CmdYawingTEST = New System.Windows.Forms.Button()
        Me.TxtPitchingTEST = New System.Windows.Forms.TextBox()
        Me.TxtYawingTEST = New System.Windows.Forms.TextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.ReferencePtZ = New System.Windows.Forms.TextBox()
        Me.ReferencePtY = New System.Windows.Forms.TextBox()
        Me.ReferencePtX = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.DispScaleZ = New System.Windows.Forms.TextBox()
        Me.DispScaleY = New System.Windows.Forms.TextBox()
        Me.DispScaleX = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.GraphAngle = New System.Windows.Forms.Button()
        Me.NodeExcludingDlg = New System.Windows.Forms.OpenFileDialog()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.StatusLbl = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.GroupBox1.SuspendLayout()
        Me.AnalysisTab.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.RefBox.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.RefBox_2.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.ExNODE_BOX.SuspendLayout()
        Me.TESTTab.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.RefBoxTEST.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.RefBoxTEST_2.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(7, 184)
        Me.TextBox1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(322, 23)
        Me.TextBox1.TabIndex = 1
        '
        'SelMainFile
        '
        Me.SelMainFile.Font = New System.Drawing.Font("Calibri", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SelMainFile.Location = New System.Drawing.Point(454, 184)
        Me.SelMainFile.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.SelMainFile.Name = "SelMainFile"
        Me.SelMainFile.Size = New System.Drawing.Size(170, 33)
        Me.SelMainFile.TabIndex = 2
        Me.SelMainFile.Text = "Select File"
        Me.SelMainFile.UseVisualStyleBackColor = True
        '
        'DescriptionLbl
        '
        Me.DescriptionLbl.AutoSize = True
        Me.DescriptionLbl.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.DescriptionLbl.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.DescriptionLbl.Location = New System.Drawing.Point(338, 192)
        Me.DescriptionLbl.Name = "DescriptionLbl"
        Me.DescriptionLbl.Size = New System.Drawing.Size(110, 15)
        Me.DescriptionLbl.TabIndex = 3
        Me.DescriptionLbl.Text = "※ Key File Format"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Start
        '
        Me.Start.Font = New System.Drawing.Font("Calibri", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Start.Location = New System.Drawing.Point(632, 494)
        Me.Start.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Start.Name = "Start"
        Me.Start.Size = New System.Drawing.Size(171, 32)
        Me.Start.TabIndex = 5
        Me.Start.Text = "Converting"
        Me.Start.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.UnitLbl)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.TxtZscale)
        Me.GroupBox1.Controls.Add(Me.TxtYscale)
        Me.GroupBox1.Controls.Add(Me.TxtXscale)
        Me.GroupBox1.Controls.Add(Me.UnitConverter)
        Me.GroupBox1.Font = New System.Drawing.Font("Calibri", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(14, 437)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox1.Size = New System.Drawing.Size(582, 92)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Translational Motion Modification Option - ※ 0.0% : Not Use"
        '
        'UnitLbl
        '
        Me.UnitLbl.AutoSize = True
        Me.UnitLbl.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.UnitLbl.Location = New System.Drawing.Point(41, 27)
        Me.UnitLbl.Name = "UnitLbl"
        Me.UnitLbl.Size = New System.Drawing.Size(98, 15)
        Me.UnitLbl.TabIndex = 9
        Me.UnitLbl.Text = "Unit Conversion"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.Label5.Location = New System.Drawing.Point(356, 71)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(216, 18)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "※ Not Applied to TEST Correction"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Label4.Location = New System.Drawing.Point(455, 27)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(101, 15)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Z Correction [%]"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Label3.Location = New System.Drawing.Point(321, 27)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(101, 15)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Y Correction [%]"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Label2.Location = New System.Drawing.Point(181, 27)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(102, 15)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "X Correction [%]"
        '
        'TxtZscale
        '
        Me.TxtZscale.Location = New System.Drawing.Point(440, 46)
        Me.TxtZscale.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TxtZscale.Name = "TxtZscale"
        Me.TxtZscale.Size = New System.Drawing.Size(130, 25)
        Me.TxtZscale.TabIndex = 3
        Me.TxtZscale.Text = "0.0"
        Me.TxtZscale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TxtYscale
        '
        Me.TxtYscale.Location = New System.Drawing.Point(304, 46)
        Me.TxtYscale.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TxtYscale.Name = "TxtYscale"
        Me.TxtYscale.Size = New System.Drawing.Size(130, 25)
        Me.TxtYscale.TabIndex = 2
        Me.TxtYscale.Text = "0.0"
        Me.TxtYscale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TxtXscale
        '
        Me.TxtXscale.Location = New System.Drawing.Point(168, 46)
        Me.TxtXscale.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TxtXscale.Name = "TxtXscale"
        Me.TxtXscale.Size = New System.Drawing.Size(130, 25)
        Me.TxtXscale.TabIndex = 1
        Me.TxtXscale.Text = "100.0"
        Me.TxtXscale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'UnitConverter
        '
        Me.UnitConverter.Location = New System.Drawing.Point(30, 46)
        Me.UnitConverter.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.UnitConverter.Name = "UnitConverter"
        Me.UnitConverter.Size = New System.Drawing.Size(120, 25)
        Me.UnitConverter.TabIndex = 0
        Me.UnitConverter.Text = "0.001"
        Me.UnitConverter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ChkDefault
        '
        Me.ChkDefault.AutoSize = True
        Me.ChkDefault.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkDefault.Location = New System.Drawing.Point(15, 548)
        Me.ChkDefault.Name = "ChkDefault"
        Me.ChkDefault.Size = New System.Drawing.Size(68, 19)
        Me.ChkDefault.TabIndex = 7
        Me.ChkDefault.Text = "Default"
        Me.ChkDefault.UseVisualStyleBackColor = True
        '
        'ChkTESTuse
        '
        Me.ChkTESTuse.AutoSize = True
        Me.ChkTESTuse.Location = New System.Drawing.Point(341, 386)
        Me.ChkTESTuse.Name = "ChkTESTuse"
        Me.ChkTESTuse.Size = New System.Drawing.Size(163, 19)
        Me.ChkTESTuse.TabIndex = 9
        Me.ChkTESTuse.Text = "Use TEST Trans. Motion"
        Me.ChkTESTuse.UseVisualStyleBackColor = True
        '
        'GraphX
        '
        Me.GraphX.Location = New System.Drawing.Point(510, 382)
        Me.GraphX.Name = "GraphX"
        Me.GraphX.Size = New System.Drawing.Size(170, 33)
        Me.GraphX.TabIndex = 10
        Me.GraphX.Text = "View Ref/TEST Trans. Data"
        Me.GraphX.UseVisualStyleBackColor = True
        '
        'ModelAngleTxt
        '
        Me.ModelAngleTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.ModelAngleTxt.Location = New System.Drawing.Point(11, 212)
        Me.ModelAngleTxt.Name = "ModelAngleTxt"
        Me.ModelAngleTxt.Size = New System.Drawing.Size(87, 23)
        Me.ModelAngleTxt.TabIndex = 11
        Me.ModelAngleTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(104, 215)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(344, 15)
        Me.Label11.TabIndex = 12
        Me.Label11.Text = "Model Initial Angle [in degree - Respect to Global Coord.]"
        '
        'TxtToolTip
        '
        Me.TxtToolTip.AutomaticDelay = 100
        Me.TxtToolTip.AutoPopDelay = 10000
        Me.TxtToolTip.BackColor = System.Drawing.Color.White
        Me.TxtToolTip.InitialDelay = 100
        Me.TxtToolTip.ReshowDelay = 20
        Me.TxtToolTip.ShowAlways = True
        Me.TxtToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning
        Me.TxtToolTip.ToolTipTitle = "Information"
        '
        'FileToolTip
        '
        Me.FileToolTip.AutomaticDelay = 100
        Me.FileToolTip.AutoPopDelay = 10000
        Me.FileToolTip.InitialDelay = 100
        Me.FileToolTip.ReshowDelay = 20
        Me.FileToolTip.ShowAlways = True
        Me.FileToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
        Me.FileToolTip.ToolTipTitle = "File Name"
        '
        'AnalysisTab
        '
        Me.AnalysisTab.Controls.Add(Me.TabPage1)
        Me.AnalysisTab.Controls.Add(Me.TabPage2)
        Me.AnalysisTab.Controls.Add(Me.TabPage5)
        Me.AnalysisTab.Location = New System.Drawing.Point(7, 12)
        Me.AnalysisTab.Name = "AnalysisTab"
        Me.AnalysisTab.SelectedIndex = 0
        Me.AnalysisTab.Size = New System.Drawing.Size(875, 165)
        Me.AnalysisTab.TabIndex = 22
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.Color.White
        Me.TabPage1.Controls.Add(Me.RefBox)
        Me.TabPage1.Location = New System.Drawing.Point(4, 24)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(867, 137)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "TabPage1"
        '
        'RefBox
        '
        Me.RefBox.Controls.Add(Me.CmdZ)
        Me.RefBox.Controls.Add(Me.TxtZcurve)
        Me.RefBox.Controls.Add(Me.CmdY)
        Me.RefBox.Controls.Add(Me.TxtYcurve)
        Me.RefBox.Controls.Add(Me.CmdX)
        Me.RefBox.Controls.Add(Me.TxtXcurve)
        Me.RefBox.Font = New System.Drawing.Font("Calibri", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RefBox.Location = New System.Drawing.Point(6, 6)
        Me.RefBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.RefBox.Name = "RefBox"
        Me.RefBox.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.RefBox.Size = New System.Drawing.Size(929, 124)
        Me.RefBox.TabIndex = 1
        Me.RefBox.TabStop = False
        Me.RefBox.Text = "Reference Position Curve [meter scale] : Rigid Body Motion"
        '
        'CmdZ
        '
        Me.CmdZ.Location = New System.Drawing.Point(752, 88)
        Me.CmdZ.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.CmdZ.Name = "CmdZ"
        Me.CmdZ.Size = New System.Drawing.Size(171, 26)
        Me.CmdZ.TabIndex = 5
        Me.CmdZ.Text = "Z Curve"
        Me.CmdZ.UseVisualStyleBackColor = True
        '
        'TxtZcurve
        '
        Me.TxtZcurve.BackColor = System.Drawing.SystemColors.Info
        Me.TxtZcurve.Font = New System.Drawing.Font("맑은 고딕", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.TxtZcurve.Location = New System.Drawing.Point(6, 88)
        Me.TxtZcurve.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TxtZcurve.Name = "TxtZcurve"
        Me.TxtZcurve.Size = New System.Drawing.Size(740, 26)
        Me.TxtZcurve.TabIndex = 4
        Me.TxtZcurve.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'CmdY
        '
        Me.CmdY.Location = New System.Drawing.Point(752, 54)
        Me.CmdY.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.CmdY.Name = "CmdY"
        Me.CmdY.Size = New System.Drawing.Size(171, 26)
        Me.CmdY.TabIndex = 3
        Me.CmdY.Text = "Y Curve"
        Me.CmdY.UseVisualStyleBackColor = True
        '
        'TxtYcurve
        '
        Me.TxtYcurve.BackColor = System.Drawing.SystemColors.Info
        Me.TxtYcurve.Font = New System.Drawing.Font("맑은 고딕", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtYcurve.Location = New System.Drawing.Point(6, 54)
        Me.TxtYcurve.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TxtYcurve.Name = "TxtYcurve"
        Me.TxtYcurve.Size = New System.Drawing.Size(740, 26)
        Me.TxtYcurve.TabIndex = 2
        Me.TxtYcurve.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'CmdX
        '
        Me.CmdX.Location = New System.Drawing.Point(752, 20)
        Me.CmdX.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.CmdX.Name = "CmdX"
        Me.CmdX.Size = New System.Drawing.Size(171, 26)
        Me.CmdX.TabIndex = 1
        Me.CmdX.Text = "X Curve"
        Me.CmdX.UseVisualStyleBackColor = True
        '
        'TxtXcurve
        '
        Me.TxtXcurve.BackColor = System.Drawing.SystemColors.Info
        Me.TxtXcurve.Font = New System.Drawing.Font("맑은 고딕", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtXcurve.Location = New System.Drawing.Point(6, 20)
        Me.TxtXcurve.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TxtXcurve.Name = "TxtXcurve"
        Me.TxtXcurve.Size = New System.Drawing.Size(740, 26)
        Me.TxtXcurve.TabIndex = 0
        Me.TxtXcurve.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.Color.White
        Me.TabPage2.Controls.Add(Me.RefBox_2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 24)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(867, 137)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "TabPage2"
        '
        'RefBox_2
        '
        Me.RefBox_2.Controls.Add(Me.CmdPitch)
        Me.RefBox_2.Controls.Add(Me.CmdYawing)
        Me.RefBox_2.Controls.Add(Me.TxtPitching)
        Me.RefBox_2.Controls.Add(Me.TxtYawing)
        Me.RefBox_2.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RefBox_2.Location = New System.Drawing.Point(14, 6)
        Me.RefBox_2.Name = "RefBox_2"
        Me.RefBox_2.Size = New System.Drawing.Size(480, 110)
        Me.RefBox_2.TabIndex = 0
        Me.RefBox_2.TabStop = False
        Me.RefBox_2.Text = "Reference Position Curve [Degree scale] : Rigid Body Motion"
        '
        'CmdPitch
        '
        Me.CmdPitch.Location = New System.Drawing.Point(342, 60)
        Me.CmdPitch.Name = "CmdPitch"
        Me.CmdPitch.Size = New System.Drawing.Size(79, 25)
        Me.CmdPitch.TabIndex = 19
        Me.CmdPitch.Text = "Pitch"
        Me.CmdPitch.UseVisualStyleBackColor = True
        '
        'CmdYawing
        '
        Me.CmdYawing.Location = New System.Drawing.Point(342, 26)
        Me.CmdYawing.Name = "CmdYawing"
        Me.CmdYawing.Size = New System.Drawing.Size(79, 25)
        Me.CmdYawing.TabIndex = 18
        Me.CmdYawing.Text = "Yaw"
        Me.CmdYawing.UseVisualStyleBackColor = True
        '
        'TxtPitching
        '
        Me.TxtPitching.BackColor = System.Drawing.SystemColors.Info
        Me.TxtPitching.Font = New System.Drawing.Font("맑은 고딕", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtPitching.Location = New System.Drawing.Point(36, 59)
        Me.TxtPitching.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TxtPitching.Name = "TxtPitching"
        Me.TxtPitching.Size = New System.Drawing.Size(268, 26)
        Me.TxtPitching.TabIndex = 6
        Me.TxtPitching.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TxtYawing
        '
        Me.TxtYawing.BackColor = System.Drawing.SystemColors.Info
        Me.TxtYawing.Font = New System.Drawing.Font("맑은 고딕", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtYawing.Location = New System.Drawing.Point(36, 25)
        Me.TxtYawing.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TxtYawing.Name = "TxtYawing"
        Me.TxtYawing.Size = New System.Drawing.Size(268, 26)
        Me.TxtYawing.TabIndex = 5
        Me.TxtYawing.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.ExNODE_BOX)
        Me.TabPage5.Location = New System.Drawing.Point(4, 24)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage5.Size = New System.Drawing.Size(867, 137)
        Me.TabPage5.TabIndex = 2
        Me.TabPage5.Text = "TabPage5"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'ExNODE_BOX
        '
        Me.ExNODE_BOX.Controls.Add(Me.ChkExNODE)
        Me.ExNODE_BOX.Controls.Add(Me.LblExNode)
        Me.ExNODE_BOX.Controls.Add(Me.CmdSelNODESET)
        Me.ExNODE_BOX.Controls.Add(Me.TxtNodeSET)
        Me.ExNODE_BOX.Location = New System.Drawing.Point(29, 22)
        Me.ExNODE_BOX.Name = "ExNODE_BOX"
        Me.ExNODE_BOX.Size = New System.Drawing.Size(396, 95)
        Me.ExNODE_BOX.TabIndex = 0
        Me.ExNODE_BOX.TabStop = False
        Me.ExNODE_BOX.Text = "Excluding *SET NODE LIST"
        '
        'ChkExNODE
        '
        Me.ChkExNODE.AutoSize = True
        Me.ChkExNODE.Location = New System.Drawing.Point(18, 64)
        Me.ChkExNODE.Name = "ChkExNODE"
        Me.ChkExNODE.Size = New System.Drawing.Size(56, 16)
        Me.ChkExNODE.TabIndex = 22
        Me.ChkExNODE.Text = "Apply"
        Me.ChkExNODE.UseVisualStyleBackColor = True
        '
        'LblExNode
        '
        Me.LblExNode.AutoSize = True
        Me.LblExNode.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LblExNode.Location = New System.Drawing.Point(68, 53)
        Me.LblExNode.Name = "LblExNode"
        Me.LblExNode.Size = New System.Drawing.Size(658, 30)
        Me.LblExNode.TabIndex = 21
        Me.LblExNode.Text = "※ If you want to exclude *SET_NODE_LIST in MADYMO PSM File, Select a KEY file tha" & _
            "t contains some NODE LIST." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "   Such models [PSM+FE] may cause a fatal instabilit" & _
            "y error. NOTE when you use."
        '
        'CmdSelNODESET
        '
        Me.CmdSelNODESET.Location = New System.Drawing.Point(292, 24)
        Me.CmdSelNODESET.Name = "CmdSelNODESET"
        Me.CmdSelNODESET.Size = New System.Drawing.Size(79, 25)
        Me.CmdSelNODESET.TabIndex = 20
        Me.CmdSelNODESET.Text = "Select File"
        Me.CmdSelNODESET.UseVisualStyleBackColor = True
        '
        'TxtNodeSET
        '
        Me.TxtNodeSET.BackColor = System.Drawing.SystemColors.Info
        Me.TxtNodeSET.Font = New System.Drawing.Font("맑은 고딕", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtNodeSET.Location = New System.Drawing.Point(18, 23)
        Me.TxtNodeSET.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TxtNodeSET.Name = "TxtNodeSET"
        Me.TxtNodeSET.Size = New System.Drawing.Size(268, 26)
        Me.TxtNodeSET.TabIndex = 19
        Me.TxtNodeSET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TESTTab
        '
        Me.TESTTab.Controls.Add(Me.TabPage3)
        Me.TESTTab.Controls.Add(Me.TabPage4)
        Me.TESTTab.Location = New System.Drawing.Point(7, 241)
        Me.TESTTab.Name = "TESTTab"
        Me.TESTTab.SelectedIndex = 0
        Me.TESTTab.Size = New System.Drawing.Size(617, 139)
        Me.TESTTab.TabIndex = 23
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.RefBoxTEST)
        Me.TabPage3.Location = New System.Drawing.Point(4, 24)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(609, 111)
        Me.TabPage3.TabIndex = 0
        Me.TabPage3.Text = "TabPage3"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'RefBoxTEST
        '
        Me.RefBoxTEST.Controls.Add(Me.CmdZTest)
        Me.RefBoxTEST.Controls.Add(Me.TxtZcurveTEST)
        Me.RefBoxTEST.Controls.Add(Me.CmdYTest)
        Me.RefBoxTEST.Controls.Add(Me.TxtYcurveTEST)
        Me.RefBoxTEST.Controls.Add(Me.CmdXTest)
        Me.RefBoxTEST.Controls.Add(Me.TxtXcurveTEST)
        Me.RefBoxTEST.Location = New System.Drawing.Point(6, 6)
        Me.RefBoxTEST.Name = "RefBoxTEST"
        Me.RefBoxTEST.Size = New System.Drawing.Size(495, 132)
        Me.RefBoxTEST.TabIndex = 9
        Me.RefBoxTEST.TabStop = False
        Me.RefBoxTEST.Text = "Use TEST Curve (Optional - Summation) [meter scale] : Rigid Body Motion"
        '
        'CmdZTest
        '
        Me.CmdZTest.Location = New System.Drawing.Point(217, 95)
        Me.CmdZTest.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.CmdZTest.Name = "CmdZTest"
        Me.CmdZTest.Size = New System.Drawing.Size(171, 26)
        Me.CmdZTest.TabIndex = 11
        Me.CmdZTest.Text = "Z Curve"
        Me.CmdZTest.UseVisualStyleBackColor = True
        '
        'TxtZcurveTEST
        '
        Me.TxtZcurveTEST.BackColor = System.Drawing.SystemColors.Info
        Me.TxtZcurveTEST.Font = New System.Drawing.Font("맑은 고딕", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.TxtZcurveTEST.Location = New System.Drawing.Point(8, 95)
        Me.TxtZcurveTEST.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TxtZcurveTEST.Name = "TxtZcurveTEST"
        Me.TxtZcurveTEST.Size = New System.Drawing.Size(203, 26)
        Me.TxtZcurveTEST.TabIndex = 10
        Me.TxtZcurveTEST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'CmdYTest
        '
        Me.CmdYTest.Location = New System.Drawing.Point(217, 61)
        Me.CmdYTest.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.CmdYTest.Name = "CmdYTest"
        Me.CmdYTest.Size = New System.Drawing.Size(171, 26)
        Me.CmdYTest.TabIndex = 9
        Me.CmdYTest.Text = "Y Curve"
        Me.CmdYTest.UseVisualStyleBackColor = True
        '
        'TxtYcurveTEST
        '
        Me.TxtYcurveTEST.BackColor = System.Drawing.SystemColors.Info
        Me.TxtYcurveTEST.Font = New System.Drawing.Font("맑은 고딕", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtYcurveTEST.Location = New System.Drawing.Point(8, 61)
        Me.TxtYcurveTEST.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TxtYcurveTEST.Name = "TxtYcurveTEST"
        Me.TxtYcurveTEST.Size = New System.Drawing.Size(203, 26)
        Me.TxtYcurveTEST.TabIndex = 8
        Me.TxtYcurveTEST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'CmdXTest
        '
        Me.CmdXTest.Location = New System.Drawing.Point(217, 27)
        Me.CmdXTest.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.CmdXTest.Name = "CmdXTest"
        Me.CmdXTest.Size = New System.Drawing.Size(171, 26)
        Me.CmdXTest.TabIndex = 7
        Me.CmdXTest.Text = "X Curve (Not neccesary)"
        Me.CmdXTest.UseVisualStyleBackColor = True
        '
        'TxtXcurveTEST
        '
        Me.TxtXcurveTEST.BackColor = System.Drawing.SystemColors.Info
        Me.TxtXcurveTEST.Font = New System.Drawing.Font("맑은 고딕", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtXcurveTEST.Location = New System.Drawing.Point(8, 27)
        Me.TxtXcurveTEST.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TxtXcurveTEST.Name = "TxtXcurveTEST"
        Me.TxtXcurveTEST.Size = New System.Drawing.Size(203, 26)
        Me.TxtXcurveTEST.TabIndex = 6
        Me.TxtXcurveTEST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.RefBoxTEST_2)
        Me.TabPage4.Location = New System.Drawing.Point(4, 24)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(609, 111)
        Me.TabPage4.TabIndex = 1
        Me.TabPage4.Text = "TabPage4"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'RefBoxTEST_2
        '
        Me.RefBoxTEST_2.Controls.Add(Me.CmdPitchTEST)
        Me.RefBoxTEST_2.Controls.Add(Me.CmdYawingTEST)
        Me.RefBoxTEST_2.Controls.Add(Me.TxtPitchingTEST)
        Me.RefBoxTEST_2.Controls.Add(Me.TxtYawingTEST)
        Me.RefBoxTEST_2.Location = New System.Drawing.Point(12, 12)
        Me.RefBoxTEST_2.Name = "RefBoxTEST_2"
        Me.RefBoxTEST_2.Size = New System.Drawing.Size(482, 93)
        Me.RefBoxTEST_2.TabIndex = 10
        Me.RefBoxTEST_2.TabStop = False
        Me.RefBoxTEST_2.Text = "Use TEST Curve (Optional - Summation) [Degree scale] : Rigid Body Motion"
        '
        'CmdPitchTEST
        '
        Me.CmdPitchTEST.Location = New System.Drawing.Point(318, 58)
        Me.CmdPitchTEST.Name = "CmdPitchTEST"
        Me.CmdPitchTEST.Size = New System.Drawing.Size(79, 25)
        Me.CmdPitchTEST.TabIndex = 20
        Me.CmdPitchTEST.Text = "Pitch"
        Me.CmdPitchTEST.UseVisualStyleBackColor = True
        '
        'CmdYawingTEST
        '
        Me.CmdYawingTEST.Location = New System.Drawing.Point(315, 23)
        Me.CmdYawingTEST.Name = "CmdYawingTEST"
        Me.CmdYawingTEST.Size = New System.Drawing.Size(79, 25)
        Me.CmdYawingTEST.TabIndex = 19
        Me.CmdYawingTEST.Text = "Yaw"
        Me.CmdYawingTEST.UseVisualStyleBackColor = True
        '
        'TxtPitchingTEST
        '
        Me.TxtPitchingTEST.BackColor = System.Drawing.SystemColors.Info
        Me.TxtPitchingTEST.Font = New System.Drawing.Font("맑은 고딕", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtPitchingTEST.Location = New System.Drawing.Point(38, 57)
        Me.TxtPitchingTEST.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TxtPitchingTEST.Name = "TxtPitchingTEST"
        Me.TxtPitchingTEST.Size = New System.Drawing.Size(268, 26)
        Me.TxtPitchingTEST.TabIndex = 7
        Me.TxtPitchingTEST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TxtYawingTEST
        '
        Me.TxtYawingTEST.BackColor = System.Drawing.SystemColors.Info
        Me.TxtYawingTEST.Font = New System.Drawing.Font("맑은 고딕", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtYawingTEST.Location = New System.Drawing.Point(6, 23)
        Me.TxtYawingTEST.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TxtYawingTEST.Name = "TxtYawingTEST"
        Me.TxtYawingTEST.Size = New System.Drawing.Size(268, 26)
        Me.TxtYawingTEST.TabIndex = 6
        Me.TxtYawingTEST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.ReferencePtZ)
        Me.GroupBox2.Controls.Add(Me.ReferencePtY)
        Me.GroupBox2.Controls.Add(Me.ReferencePtX)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.DispScaleZ)
        Me.GroupBox2.Controls.Add(Me.DispScaleY)
        Me.GroupBox2.Controls.Add(Me.DispScaleX)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Location = New System.Drawing.Point(556, 232)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(356, 144)
        Me.GroupBox2.TabIndex = 24
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Ref. Point Coord. / Deformation Scale"
        '
        'ReferencePtZ
        '
        Me.ReferencePtZ.BackColor = System.Drawing.SystemColors.Info
        Me.ReferencePtZ.Location = New System.Drawing.Point(236, 76)
        Me.ReferencePtZ.Name = "ReferencePtZ"
        Me.ReferencePtZ.Size = New System.Drawing.Size(100, 23)
        Me.ReferencePtZ.TabIndex = 31
        Me.ReferencePtZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ReferencePtY
        '
        Me.ReferencePtY.BackColor = System.Drawing.SystemColors.Info
        Me.ReferencePtY.Location = New System.Drawing.Point(130, 76)
        Me.ReferencePtY.Name = "ReferencePtY"
        Me.ReferencePtY.Size = New System.Drawing.Size(100, 23)
        Me.ReferencePtY.TabIndex = 30
        Me.ReferencePtY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ReferencePtX
        '
        Me.ReferencePtX.BackColor = System.Drawing.SystemColors.Info
        Me.ReferencePtX.Location = New System.Drawing.Point(24, 76)
        Me.ReferencePtX.Name = "ReferencePtX"
        Me.ReferencePtX.Size = New System.Drawing.Size(100, 23)
        Me.ReferencePtX.TabIndex = 29
        Me.ReferencePtX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.Label9.Location = New System.Drawing.Point(21, 104)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(289, 15)
        Me.Label9.TabIndex = 28
        Me.Label9.Text = "※ Assumption : No Rolling : No Angle Correction"
        '
        'DispScaleZ
        '
        Me.DispScaleZ.BackColor = System.Drawing.SystemColors.Info
        Me.DispScaleZ.Location = New System.Drawing.Point(236, 45)
        Me.DispScaleZ.Name = "DispScaleZ"
        Me.DispScaleZ.Size = New System.Drawing.Size(100, 23)
        Me.DispScaleZ.TabIndex = 27
        Me.DispScaleZ.Text = "1.0"
        Me.DispScaleZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'DispScaleY
        '
        Me.DispScaleY.BackColor = System.Drawing.SystemColors.Info
        Me.DispScaleY.Location = New System.Drawing.Point(130, 45)
        Me.DispScaleY.Name = "DispScaleY"
        Me.DispScaleY.Size = New System.Drawing.Size(100, 23)
        Me.DispScaleY.TabIndex = 26
        Me.DispScaleY.Text = "1.0"
        Me.DispScaleY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'DispScaleX
        '
        Me.DispScaleX.BackColor = System.Drawing.SystemColors.Info
        Me.DispScaleX.Location = New System.Drawing.Point(24, 45)
        Me.DispScaleX.Name = "DispScaleX"
        Me.DispScaleX.Size = New System.Drawing.Size(100, 23)
        Me.DispScaleX.TabIndex = 25
        Me.DispScaleX.Text = "1.0"
        Me.DispScaleX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Label8.Location = New System.Drawing.Point(245, 26)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(81, 15)
        Me.Label8.TabIndex = 24
        Me.Label8.Text = "Z Disp. Scale"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Label7.Location = New System.Drawing.Point(141, 26)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(81, 15)
        Me.Label7.TabIndex = 23
        Me.Label7.Text = "Y Disp. Scale"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Label6.Location = New System.Drawing.Point(37, 26)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(82, 15)
        Me.Label6.TabIndex = 22
        Me.Label6.Text = "X Disp. Scale"
        '
        'GraphAngle
        '
        Me.GraphAngle.Location = New System.Drawing.Point(696, 386)
        Me.GraphAngle.Name = "GraphAngle"
        Me.GraphAngle.Size = New System.Drawing.Size(170, 33)
        Me.GraphAngle.TabIndex = 25
        Me.GraphAngle.Text = "View Ref/TEST Rot. Data"
        Me.GraphAngle.UseVisualStyleBackColor = True
        '
        'NodeExcludingDlg
        '
        Me.NodeExcludingDlg.FileName = "OpenFileDialog2"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLbl, Me.ProgressBar1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 596)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(918, 22)
        Me.StatusStrip1.SizingGrip = False
        Me.StatusStrip1.TabIndex = 26
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'StatusLbl
        '
        Me.StatusLbl.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.StatusLbl.Name = "StatusLbl"
        Me.StatusLbl.Size = New System.Drawing.Size(620, 17)
        Me.StatusLbl.Spring = True
        Me.StatusLbl.Text = "Status"
        Me.StatusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(250, 16)
        '
        'FrmPSMexport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(918, 618)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.GraphAngle)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.TESTTab)
        Me.Controls.Add(Me.AnalysisTab)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.ModelAngleTxt)
        Me.Controls.Add(Me.GraphX)
        Me.Controls.Add(Me.ChkTESTuse)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Start)
        Me.Controls.Add(Me.DescriptionLbl)
        Me.Controls.Add(Me.ChkDefault)
        Me.Controls.Add(Me.SelMainFile)
        Me.Controls.Add(Me.TextBox1)
        Me.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmPSMexport"
        Me.Text = "PSM File Converter"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.AnalysisTab.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.RefBox.ResumeLayout(False)
        Me.RefBox.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.RefBox_2.ResumeLayout(False)
        Me.RefBox_2.PerformLayout()
        Me.TabPage5.ResumeLayout(False)
        Me.ExNODE_BOX.ResumeLayout(False)
        Me.ExNODE_BOX.PerformLayout()
        Me.TESTTab.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.RefBoxTEST.ResumeLayout(False)
        Me.RefBoxTEST.PerformLayout()
        Me.TabPage4.ResumeLayout(False)
        Me.RefBoxTEST_2.ResumeLayout(False)
        Me.RefBoxTEST_2.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents SelMainFile As System.Windows.Forms.Button
    Friend WithEvents DescriptionLbl As System.Windows.Forms.Label
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Start As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents UnitConverter As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TxtZscale As System.Windows.Forms.TextBox
    Friend WithEvents TxtYscale As System.Windows.Forms.TextBox
    Friend WithEvents TxtXscale As System.Windows.Forms.TextBox
    Friend WithEvents ChkDefault As System.Windows.Forms.CheckBox
    Friend WithEvents SaveFileDlg As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ChkTESTuse As System.Windows.Forms.CheckBox
    Friend WithEvents GraphX As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ModelAngleTxt As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents TxtToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents FileToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents AnalysisTab As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents RefBox As System.Windows.Forms.GroupBox
    Friend WithEvents CmdZ As System.Windows.Forms.Button
    Friend WithEvents TxtZcurve As System.Windows.Forms.TextBox
    Friend WithEvents CmdY As System.Windows.Forms.Button
    Friend WithEvents TxtYcurve As System.Windows.Forms.TextBox
    Friend WithEvents CmdX As System.Windows.Forms.Button
    Friend WithEvents TxtXcurve As System.Windows.Forms.TextBox
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents RefBox_2 As System.Windows.Forms.GroupBox
    Friend WithEvents CmdPitch As System.Windows.Forms.Button
    Friend WithEvents CmdYawing As System.Windows.Forms.Button
    Friend WithEvents TxtPitching As System.Windows.Forms.TextBox
    Friend WithEvents TxtYawing As System.Windows.Forms.TextBox
    Friend WithEvents TESTTab As System.Windows.Forms.TabControl
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents RefBoxTEST As System.Windows.Forms.GroupBox
    Friend WithEvents CmdZTest As System.Windows.Forms.Button
    Friend WithEvents TxtZcurveTEST As System.Windows.Forms.TextBox
    Friend WithEvents CmdYTest As System.Windows.Forms.Button
    Friend WithEvents TxtYcurveTEST As System.Windows.Forms.TextBox
    Friend WithEvents CmdXTest As System.Windows.Forms.Button
    Friend WithEvents TxtXcurveTEST As System.Windows.Forms.TextBox
    Friend WithEvents RefBoxTEST_2 As System.Windows.Forms.GroupBox
    Friend WithEvents UnitLbl As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents ReferencePtZ As System.Windows.Forms.TextBox
    Friend WithEvents ReferencePtY As System.Windows.Forms.TextBox
    Friend WithEvents ReferencePtX As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents DispScaleZ As System.Windows.Forms.TextBox
    Friend WithEvents DispScaleY As System.Windows.Forms.TextBox
    Friend WithEvents DispScaleX As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TxtPitchingTEST As System.Windows.Forms.TextBox
    Friend WithEvents TxtYawingTEST As System.Windows.Forms.TextBox
    Friend WithEvents CmdPitchTEST As System.Windows.Forms.Button
    Friend WithEvents CmdYawingTEST As System.Windows.Forms.Button
    Friend WithEvents GraphAngle As System.Windows.Forms.Button
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents ExNODE_BOX As System.Windows.Forms.GroupBox
    Friend WithEvents CmdSelNODESET As System.Windows.Forms.Button
    Friend WithEvents TxtNodeSET As System.Windows.Forms.TextBox
    Friend WithEvents LblExNode As System.Windows.Forms.Label
    Friend WithEvents NodeExcludingDlg As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ChkExNODE As System.Windows.Forms.CheckBox
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents StatusLbl As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ToolStripProgressBar
End Class
