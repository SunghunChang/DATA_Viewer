<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ProfileSetting
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ProfileSetting))
        Me.SetUpTab = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.CFactorLbl = New System.Windows.Forms.Label()
        Me.CFactorTxt = New System.Windows.Forms.TextBox()
        Me.SaveBtn = New System.Windows.Forms.Button()
        Me.AddBtn = New System.Windows.Forms.Button()
        Me.RowTxt = New System.Windows.Forms.TextBox()
        Me.ColTxt = New System.Windows.Forms.TextBox()
        Me.ExtTxt = New System.Windows.Forms.TextBox()
        Me.RowLst = New System.Windows.Forms.ListBox()
        Me.ColLst = New System.Windows.Forms.ListBox()
        Me.ExtLst = New System.Windows.Forms.ListBox()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.TESTIdenTxt3 = New System.Windows.Forms.TextBox()
        Me.TESTIdenTxt2 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TESTSaveBtn = New System.Windows.Forms.Button()
        Me.TESTaddBtn = New System.Windows.Forms.Button()
        Me.TESTIdenTxt1 = New System.Windows.Forms.TextBox()
        Me.TESTYTxt = New System.Windows.Forms.TextBox()
        Me.TESTXTxt = New System.Windows.Forms.TextBox()
        Me.TESTLblTxt = New System.Windows.Forms.TextBox()
        Me.TESTYLst = New System.Windows.Forms.ListBox()
        Me.TESTXLst = New System.Windows.Forms.ListBox()
        Me.TESTLblLst = New System.Windows.Forms.ListBox()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.BtnLblSave = New System.Windows.Forms.Button()
        Me.BtnLblReplace = New System.Windows.Forms.Button()
        Me.GraphLblTxt = New System.Windows.Forms.TextBox()
        Me.GraphLblLst = New System.Windows.Forms.ListBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.SaveCorrel = New System.Windows.Forms.Button()
        Me.OpenCorrel = New System.Windows.Forms.Button()
        Me.InjuryScaleVal = New System.Windows.Forms.TextBox()
        Me.SelectedInjuryTxt = New System.Windows.Forms.TextBox()
        Me.GraphScaleVal = New System.Windows.Forms.TextBox()
        Me.SelectedGraphTxt = New System.Windows.Forms.TextBox()
        Me.InjuryScaleLst = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.GraphScaleLst = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.TxtPathHW = New System.Windows.Forms.TextBox()
        Me.BtnFindFolderHW = New System.Windows.Forms.Button()
        Me.LblHW = New System.Windows.Forms.Label()
        Me.TxtPath = New System.Windows.Forms.TextBox()
        Me.BtnFindFolder = New System.Windows.Forms.Button()
        Me.LblXMADgic = New System.Windows.Forms.Label()
        Me.TabPage6 = New System.Windows.Forms.TabPage()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.CmbNCF = New System.Windows.Forms.ComboBox()
        Me.CmbNCE = New System.Windows.Forms.ComboBox()
        Me.CmbNTF = New System.Windows.Forms.ComboBox()
        Me.CmbNTE = New System.Windows.Forms.ComboBox()
        Me.BtnGbasedOpen = New System.Windows.Forms.Button()
        Me.BtnApplyNSave = New System.Windows.Forms.Button()
        Me.CmbMocyGraph = New System.Windows.Forms.ComboBox()
        Me.CmbMyGraph = New System.Windows.Forms.ComboBox()
        Me.CmbFzGraph = New System.Windows.Forms.ComboBox()
        Me.CmbFxGraph = New System.Windows.Forms.ComboBox()
        Me.CmbHICgraph = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.CorrelOpenFile = New System.Windows.Forms.OpenFileDialog()
        Me.CorrelSaveFile = New System.Windows.Forms.SaveFileDialog()
        Me.FindFolderPath = New System.Windows.Forms.FolderBrowserDialog()
        Me.GraphBasedDlg = New System.Windows.Forms.OpenFileDialog()
        Me.GraphBasedSave = New System.Windows.Forms.SaveFileDialog()
        Me.SetUpTab.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        CType(Me.InjuryScaleLst, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GraphScaleLst, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage5.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SetUpTab
        '
        Me.SetUpTab.Controls.Add(Me.TabPage1)
        Me.SetUpTab.Controls.Add(Me.TabPage3)
        Me.SetUpTab.Controls.Add(Me.TabPage4)
        Me.SetUpTab.Controls.Add(Me.TabPage2)
        Me.SetUpTab.Controls.Add(Me.TabPage5)
        Me.SetUpTab.Controls.Add(Me.TabPage6)
        Me.SetUpTab.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.SetUpTab.Location = New System.Drawing.Point(10, 10)
        Me.SetUpTab.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.SetUpTab.Name = "SetUpTab"
        Me.SetUpTab.SelectedIndex = 0
        Me.SetUpTab.Size = New System.Drawing.Size(706, 481)
        Me.SetUpTab.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.CFactorLbl)
        Me.TabPage1.Controls.Add(Me.CFactorTxt)
        Me.TabPage1.Controls.Add(Me.SaveBtn)
        Me.TabPage1.Controls.Add(Me.AddBtn)
        Me.TabPage1.Controls.Add(Me.RowTxt)
        Me.TabPage1.Controls.Add(Me.ColTxt)
        Me.TabPage1.Controls.Add(Me.ExtTxt)
        Me.TabPage1.Controls.Add(Me.RowLst)
        Me.TabPage1.Controls.Add(Me.ColLst)
        Me.TabPage1.Controls.Add(Me.ExtLst)
        Me.TabPage1.Location = New System.Drawing.Point(4, 24)
        Me.TabPage1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage1.Size = New System.Drawing.Size(698, 453)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "TabPage1"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'CFactorLbl
        '
        Me.CFactorLbl.AutoSize = True
        Me.CFactorLbl.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.CFactorLbl.Location = New System.Drawing.Point(90, 235)
        Me.CFactorLbl.Name = "CFactorLbl"
        Me.CFactorLbl.Size = New System.Drawing.Size(150, 15)
        Me.CFactorLbl.TabIndex = 9
        Me.CFactorLbl.Text = "←Unit Conversion Factor"
        '
        'CFactorTxt
        '
        Me.CFactorTxt.BackColor = System.Drawing.SystemColors.Info
        Me.CFactorTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.CFactorTxt.Location = New System.Drawing.Point(25, 233)
        Me.CFactorTxt.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.CFactorTxt.Name = "CFactorTxt"
        Me.CFactorTxt.Size = New System.Drawing.Size(60, 23)
        Me.CFactorTxt.TabIndex = 8
        '
        'SaveBtn
        '
        Me.SaveBtn.Location = New System.Drawing.Point(265, 230)
        Me.SaveBtn.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.SaveBtn.Name = "SaveBtn"
        Me.SaveBtn.Size = New System.Drawing.Size(52, 23)
        Me.SaveBtn.TabIndex = 7
        Me.SaveBtn.Text = "Save"
        Me.SaveBtn.UseVisualStyleBackColor = True
        '
        'AddBtn
        '
        Me.AddBtn.Location = New System.Drawing.Point(168, 221)
        Me.AddBtn.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.AddBtn.Name = "AddBtn"
        Me.AddBtn.Size = New System.Drawing.Size(82, 26)
        Me.AddBtn.TabIndex = 6
        Me.AddBtn.Text = "Add"
        Me.AddBtn.UseVisualStyleBackColor = True
        '
        'RowTxt
        '
        Me.RowTxt.BackColor = System.Drawing.SystemColors.Info
        Me.RowTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.RowTxt.Location = New System.Drawing.Point(230, 192)
        Me.RowTxt.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.RowTxt.Name = "RowTxt"
        Me.RowTxt.Size = New System.Drawing.Size(78, 23)
        Me.RowTxt.TabIndex = 5
        '
        'ColTxt
        '
        Me.ColTxt.BackColor = System.Drawing.SystemColors.Info
        Me.ColTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.ColTxt.Location = New System.Drawing.Point(129, 196)
        Me.ColTxt.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ColTxt.Name = "ColTxt"
        Me.ColTxt.Size = New System.Drawing.Size(64, 23)
        Me.ColTxt.TabIndex = 4
        '
        'ExtTxt
        '
        Me.ExtTxt.BackColor = System.Drawing.SystemColors.Info
        Me.ExtTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.ExtTxt.Location = New System.Drawing.Point(19, 195)
        Me.ExtTxt.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ExtTxt.Name = "ExtTxt"
        Me.ExtTxt.Size = New System.Drawing.Size(90, 23)
        Me.ExtTxt.TabIndex = 3
        '
        'RowLst
        '
        Me.RowLst.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RowLst.FormattingEnabled = True
        Me.RowLst.ItemHeight = 15
        Me.RowLst.Location = New System.Drawing.Point(226, 15)
        Me.RowLst.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.RowLst.Name = "RowLst"
        Me.RowLst.Size = New System.Drawing.Size(83, 124)
        Me.RowLst.TabIndex = 2
        '
        'ColLst
        '
        Me.ColLst.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ColLst.FormattingEnabled = True
        Me.ColLst.ItemHeight = 15
        Me.ColLst.Location = New System.Drawing.Point(127, 17)
        Me.ColLst.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ColLst.Name = "ColLst"
        Me.ColLst.Size = New System.Drawing.Size(78, 124)
        Me.ColLst.TabIndex = 1
        '
        'ExtLst
        '
        Me.ExtLst.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExtLst.FormattingEnabled = True
        Me.ExtLst.ItemHeight = 15
        Me.ExtLst.Location = New System.Drawing.Point(14, 17)
        Me.ExtLst.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ExtLst.Name = "ExtLst"
        Me.ExtLst.Size = New System.Drawing.Size(96, 124)
        Me.ExtLst.TabIndex = 0
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.TESTIdenTxt3)
        Me.TabPage3.Controls.Add(Me.TESTIdenTxt2)
        Me.TabPage3.Controls.Add(Me.Label3)
        Me.TabPage3.Controls.Add(Me.Label2)
        Me.TabPage3.Controls.Add(Me.Label1)
        Me.TabPage3.Controls.Add(Me.TESTSaveBtn)
        Me.TabPage3.Controls.Add(Me.TESTaddBtn)
        Me.TabPage3.Controls.Add(Me.TESTIdenTxt1)
        Me.TabPage3.Controls.Add(Me.TESTYTxt)
        Me.TabPage3.Controls.Add(Me.TESTXTxt)
        Me.TabPage3.Controls.Add(Me.TESTLblTxt)
        Me.TabPage3.Controls.Add(Me.TESTYLst)
        Me.TabPage3.Controls.Add(Me.TESTXLst)
        Me.TabPage3.Controls.Add(Me.TESTLblLst)
        Me.TabPage3.Location = New System.Drawing.Point(4, 24)
        Me.TabPage3.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage3.Size = New System.Drawing.Size(698, 453)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "시험데이터"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'TESTIdenTxt3
        '
        Me.TESTIdenTxt3.BackColor = System.Drawing.SystemColors.Info
        Me.TESTIdenTxt3.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.TESTIdenTxt3.Location = New System.Drawing.Point(353, 236)
        Me.TESTIdenTxt3.Name = "TESTIdenTxt3"
        Me.TESTIdenTxt3.Size = New System.Drawing.Size(75, 23)
        Me.TESTIdenTxt3.TabIndex = 13
        '
        'TESTIdenTxt2
        '
        Me.TESTIdenTxt2.BackColor = System.Drawing.SystemColors.Info
        Me.TESTIdenTxt2.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.TESTIdenTxt2.Location = New System.Drawing.Point(208, 240)
        Me.TESTIdenTxt2.Name = "TESTIdenTxt2"
        Me.TESTIdenTxt2.Size = New System.Drawing.Size(75, 23)
        Me.TESTIdenTxt2.TabIndex = 12
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Calibri", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(288, 247)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(30, 18)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Etc."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(178, 239)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(31, 18)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "PAS"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(24, 238)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 18)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "DRV"
        '
        'TESTSaveBtn
        '
        Me.TESTSaveBtn.Location = New System.Drawing.Point(409, 193)
        Me.TESTSaveBtn.Name = "TESTSaveBtn"
        Me.TESTSaveBtn.Size = New System.Drawing.Size(47, 20)
        Me.TESTSaveBtn.TabIndex = 8
        Me.TESTSaveBtn.Text = "Save"
        Me.TESTSaveBtn.UseVisualStyleBackColor = True
        '
        'TESTaddBtn
        '
        Me.TESTaddBtn.Location = New System.Drawing.Point(342, 193)
        Me.TESTaddBtn.Name = "TESTaddBtn"
        Me.TESTaddBtn.Size = New System.Drawing.Size(45, 20)
        Me.TESTaddBtn.TabIndex = 7
        Me.TESTaddBtn.Text = "Add"
        Me.TESTaddBtn.UseVisualStyleBackColor = True
        '
        'TESTIdenTxt1
        '
        Me.TESTIdenTxt1.BackColor = System.Drawing.SystemColors.Info
        Me.TESTIdenTxt1.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.TESTIdenTxt1.Location = New System.Drawing.Point(81, 238)
        Me.TESTIdenTxt1.Name = "TESTIdenTxt1"
        Me.TESTIdenTxt1.Size = New System.Drawing.Size(75, 23)
        Me.TESTIdenTxt1.TabIndex = 6
        '
        'TESTYTxt
        '
        Me.TESTYTxt.BackColor = System.Drawing.SystemColors.Info
        Me.TESTYTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.TESTYTxt.Location = New System.Drawing.Point(342, 166)
        Me.TESTYTxt.Name = "TESTYTxt"
        Me.TESTYTxt.Size = New System.Drawing.Size(114, 23)
        Me.TESTYTxt.TabIndex = 5
        '
        'TESTXTxt
        '
        Me.TESTXTxt.BackColor = System.Drawing.SystemColors.Info
        Me.TESTXTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.TESTXTxt.Location = New System.Drawing.Point(162, 192)
        Me.TESTXTxt.Name = "TESTXTxt"
        Me.TESTXTxt.Size = New System.Drawing.Size(114, 23)
        Me.TESTXTxt.TabIndex = 4
        '
        'TESTLblTxt
        '
        Me.TESTLblTxt.BackColor = System.Drawing.SystemColors.Info
        Me.TESTLblTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.TESTLblTxt.Location = New System.Drawing.Point(19, 204)
        Me.TESTLblTxt.Name = "TESTLblTxt"
        Me.TESTLblTxt.Size = New System.Drawing.Size(114, 23)
        Me.TESTLblTxt.TabIndex = 3
        '
        'TESTYLst
        '
        Me.TESTYLst.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.TESTYLst.FormattingEnabled = True
        Me.TESTYLst.ItemHeight = 15
        Me.TESTYLst.Location = New System.Drawing.Point(321, 25)
        Me.TESTYLst.Name = "TESTYLst"
        Me.TESTYLst.Size = New System.Drawing.Size(126, 94)
        Me.TESTYLst.TabIndex = 2
        '
        'TESTXLst
        '
        Me.TESTXLst.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.TESTXLst.FormattingEnabled = True
        Me.TESTXLst.ItemHeight = 15
        Me.TESTXLst.Location = New System.Drawing.Point(162, 21)
        Me.TESTXLst.Name = "TESTXLst"
        Me.TESTXLst.Size = New System.Drawing.Size(130, 109)
        Me.TESTXLst.TabIndex = 1
        '
        'TESTLblLst
        '
        Me.TESTLblLst.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.TESTLblLst.FormattingEnabled = True
        Me.TESTLblLst.ItemHeight = 15
        Me.TESTLblLst.Location = New System.Drawing.Point(14, 21)
        Me.TESTLblLst.Name = "TESTLblLst"
        Me.TESTLblLst.Size = New System.Drawing.Size(120, 124)
        Me.TESTLblLst.TabIndex = 0
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.BtnLblSave)
        Me.TabPage4.Controls.Add(Me.BtnLblReplace)
        Me.TabPage4.Controls.Add(Me.GraphLblTxt)
        Me.TabPage4.Controls.Add(Me.GraphLblLst)
        Me.TabPage4.Location = New System.Drawing.Point(4, 24)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(698, 453)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "TabPage4"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'BtnLblSave
        '
        Me.BtnLblSave.Location = New System.Drawing.Point(336, 95)
        Me.BtnLblSave.Name = "BtnLblSave"
        Me.BtnLblSave.Size = New System.Drawing.Size(58, 28)
        Me.BtnLblSave.TabIndex = 4
        Me.BtnLblSave.Text = "Save"
        Me.BtnLblSave.UseVisualStyleBackColor = True
        '
        'BtnLblReplace
        '
        Me.BtnLblReplace.Location = New System.Drawing.Point(235, 95)
        Me.BtnLblReplace.Name = "BtnLblReplace"
        Me.BtnLblReplace.Size = New System.Drawing.Size(60, 29)
        Me.BtnLblReplace.TabIndex = 3
        Me.BtnLblReplace.Text = "Replace"
        Me.BtnLblReplace.UseVisualStyleBackColor = True
        '
        'GraphLblTxt
        '
        Me.GraphLblTxt.BackColor = System.Drawing.SystemColors.Info
        Me.GraphLblTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.GraphLblTxt.Location = New System.Drawing.Point(176, 27)
        Me.GraphLblTxt.Name = "GraphLblTxt"
        Me.GraphLblTxt.Size = New System.Drawing.Size(164, 23)
        Me.GraphLblTxt.TabIndex = 2
        '
        'GraphLblLst
        '
        Me.GraphLblLst.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.GraphLblLst.FormattingEnabled = True
        Me.GraphLblLst.ItemHeight = 15
        Me.GraphLblLst.Location = New System.Drawing.Point(17, 27)
        Me.GraphLblLst.Name = "GraphLblLst"
        Me.GraphLblLst.Size = New System.Drawing.Size(132, 139)
        Me.GraphLblLst.TabIndex = 0
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.SaveCorrel)
        Me.TabPage2.Controls.Add(Me.OpenCorrel)
        Me.TabPage2.Controls.Add(Me.InjuryScaleVal)
        Me.TabPage2.Controls.Add(Me.SelectedInjuryTxt)
        Me.TabPage2.Controls.Add(Me.GraphScaleVal)
        Me.TabPage2.Controls.Add(Me.SelectedGraphTxt)
        Me.TabPage2.Controls.Add(Me.InjuryScaleLst)
        Me.TabPage2.Controls.Add(Me.GraphScaleLst)
        Me.TabPage2.Location = New System.Drawing.Point(4, 24)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(698, 453)
        Me.TabPage2.TabIndex = 4
        Me.TabPage2.Text = "TabPage2"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'SaveCorrel
        '
        Me.SaveCorrel.Location = New System.Drawing.Point(310, 243)
        Me.SaveCorrel.Name = "SaveCorrel"
        Me.SaveCorrel.Size = New System.Drawing.Size(83, 28)
        Me.SaveCorrel.TabIndex = 7
        Me.SaveCorrel.Text = "Save"
        Me.SaveCorrel.UseVisualStyleBackColor = True
        '
        'OpenCorrel
        '
        Me.OpenCorrel.Location = New System.Drawing.Point(216, 243)
        Me.OpenCorrel.Name = "OpenCorrel"
        Me.OpenCorrel.Size = New System.Drawing.Size(76, 29)
        Me.OpenCorrel.TabIndex = 6
        Me.OpenCorrel.Text = "Open"
        Me.OpenCorrel.UseVisualStyleBackColor = True
        '
        'InjuryScaleVal
        '
        Me.InjuryScaleVal.BackColor = System.Drawing.SystemColors.Info
        Me.InjuryScaleVal.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InjuryScaleVal.Location = New System.Drawing.Point(372, 204)
        Me.InjuryScaleVal.Name = "InjuryScaleVal"
        Me.InjuryScaleVal.Size = New System.Drawing.Size(65, 26)
        Me.InjuryScaleVal.TabIndex = 5
        '
        'SelectedInjuryTxt
        '
        Me.SelectedInjuryTxt.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SelectedInjuryTxt.Location = New System.Drawing.Point(216, 204)
        Me.SelectedInjuryTxt.Name = "SelectedInjuryTxt"
        Me.SelectedInjuryTxt.Size = New System.Drawing.Size(122, 26)
        Me.SelectedInjuryTxt.TabIndex = 4
        '
        'GraphScaleVal
        '
        Me.GraphScaleVal.BackColor = System.Drawing.SystemColors.Info
        Me.GraphScaleVal.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GraphScaleVal.Location = New System.Drawing.Point(134, 207)
        Me.GraphScaleVal.Name = "GraphScaleVal"
        Me.GraphScaleVal.Size = New System.Drawing.Size(67, 26)
        Me.GraphScaleVal.TabIndex = 2
        '
        'SelectedGraphTxt
        '
        Me.SelectedGraphTxt.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SelectedGraphTxt.Location = New System.Drawing.Point(10, 207)
        Me.SelectedGraphTxt.Name = "SelectedGraphTxt"
        Me.SelectedGraphTxt.Size = New System.Drawing.Size(118, 26)
        Me.SelectedGraphTxt.TabIndex = 1
        '
        'InjuryScaleLst
        '
        Me.InjuryScaleLst.Location = New System.Drawing.Point(226, 12)
        Me.InjuryScaleLst.Name = "InjuryScaleLst"
        Me.InjuryScaleLst.OcxState = CType(resources.GetObject("InjuryScaleLst.OcxState"), System.Windows.Forms.AxHost.State)
        Me.InjuryScaleLst.Size = New System.Drawing.Size(226, 291)
        Me.InjuryScaleLst.TabIndex = 3
        '
        'GraphScaleLst
        '
        Me.GraphScaleLst.Location = New System.Drawing.Point(6, 6)
        Me.GraphScaleLst.Name = "GraphScaleLst"
        Me.GraphScaleLst.OcxState = CType(resources.GetObject("GraphScaleLst.OcxState"), System.Windows.Forms.AxHost.State)
        Me.GraphScaleLst.Size = New System.Drawing.Size(244, 299)
        Me.GraphScaleLst.TabIndex = 0
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.TxtPathHW)
        Me.TabPage5.Controls.Add(Me.BtnFindFolderHW)
        Me.TabPage5.Controls.Add(Me.LblHW)
        Me.TabPage5.Controls.Add(Me.TxtPath)
        Me.TabPage5.Controls.Add(Me.BtnFindFolder)
        Me.TabPage5.Controls.Add(Me.LblXMADgic)
        Me.TabPage5.Location = New System.Drawing.Point(4, 24)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage5.Size = New System.Drawing.Size(698, 453)
        Me.TabPage5.TabIndex = 5
        Me.TabPage5.Text = "TabPage5"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'TxtPathHW
        '
        Me.TxtPathHW.Location = New System.Drawing.Point(127, 74)
        Me.TxtPathHW.Name = "TxtPathHW"
        Me.TxtPathHW.Size = New System.Drawing.Size(178, 23)
        Me.TxtPathHW.TabIndex = 5
        '
        'BtnFindFolderHW
        '
        Me.BtnFindFolderHW.Location = New System.Drawing.Point(331, 77)
        Me.BtnFindFolderHW.Name = "BtnFindFolderHW"
        Me.BtnFindFolderHW.Size = New System.Drawing.Size(88, 23)
        Me.BtnFindFolderHW.TabIndex = 4
        Me.BtnFindFolderHW.Text = "Browse"
        Me.BtnFindFolderHW.UseVisualStyleBackColor = True
        '
        'LblHW
        '
        Me.LblHW.AutoSize = True
        Me.LblHW.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblHW.Location = New System.Drawing.Point(17, 77)
        Me.LblHW.Name = "LblHW"
        Me.LblHW.Size = New System.Drawing.Size(103, 15)
        Me.LblHW.TabIndex = 3
        Me.LblHW.Text = "HyperView Path :"
        '
        'TxtPath
        '
        Me.TxtPath.Location = New System.Drawing.Point(127, 17)
        Me.TxtPath.Name = "TxtPath"
        Me.TxtPath.Size = New System.Drawing.Size(178, 23)
        Me.TxtPath.TabIndex = 2
        '
        'BtnFindFolder
        '
        Me.BtnFindFolder.Location = New System.Drawing.Point(331, 20)
        Me.BtnFindFolder.Name = "BtnFindFolder"
        Me.BtnFindFolder.Size = New System.Drawing.Size(88, 23)
        Me.BtnFindFolder.TabIndex = 1
        Me.BtnFindFolder.Text = "Browse"
        Me.BtnFindFolder.UseVisualStyleBackColor = True
        '
        'LblXMADgic
        '
        Me.LblXMADgic.AutoSize = True
        Me.LblXMADgic.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblXMADgic.Location = New System.Drawing.Point(17, 20)
        Me.LblXMADgic.Name = "LblXMADgic"
        Me.LblXMADgic.Size = New System.Drawing.Size(90, 15)
        Me.LblXMADgic.TabIndex = 0
        Me.LblXMADgic.Text = "XMADgic Path :"
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.GroupBox1)
        Me.TabPage6.Location = New System.Drawing.Point(4, 24)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage6.Size = New System.Drawing.Size(698, 453)
        Me.TabPage6.TabIndex = 6
        Me.TabPage6.Text = "TabPage6"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.CmbNCF)
        Me.GroupBox1.Controls.Add(Me.CmbNCE)
        Me.GroupBox1.Controls.Add(Me.CmbNTF)
        Me.GroupBox1.Controls.Add(Me.CmbNTE)
        Me.GroupBox1.Controls.Add(Me.BtnGbasedOpen)
        Me.GroupBox1.Controls.Add(Me.BtnApplyNSave)
        Me.GroupBox1.Controls.Add(Me.CmbMocyGraph)
        Me.GroupBox1.Controls.Add(Me.CmbMyGraph)
        Me.GroupBox1.Controls.Add(Me.CmbFzGraph)
        Me.GroupBox1.Controls.Add(Me.CmbFxGraph)
        Me.GroupBox1.Controls.Add(Me.CmbHICgraph)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(692, 447)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Target Graph"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(332, 88)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(406, 195)
        Me.Label13.TabIndex = 24
        Me.Label13.Text = resources.GetString("Label13.Text")
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(40, 282)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(104, 15)
        Me.Label12.TabIndex = 23
        Me.Label12.Text = "NCF [Calculated]"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(35, 238)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(104, 15)
        Me.Label11.TabIndex = 22
        Me.Label11.Text = "NCE [Calculated]"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(30, 222)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(103, 15)
        Me.Label10.TabIndex = 21
        Me.Label10.Text = "NTF [Calculated]"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(28, 194)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(103, 15)
        Me.Label9.TabIndex = 20
        Me.Label9.Text = "NTE [Calculated]"
        '
        'CmbNCF
        '
        Me.CmbNCF.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.CmbNCF.FormattingEnabled = True
        Me.CmbNCF.Location = New System.Drawing.Point(127, 280)
        Me.CmbNCF.Name = "CmbNCF"
        Me.CmbNCF.Size = New System.Drawing.Size(118, 23)
        Me.CmbNCF.TabIndex = 19
        '
        'CmbNCE
        '
        Me.CmbNCE.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.CmbNCE.FormattingEnabled = True
        Me.CmbNCE.Location = New System.Drawing.Point(127, 251)
        Me.CmbNCE.Name = "CmbNCE"
        Me.CmbNCE.Size = New System.Drawing.Size(118, 23)
        Me.CmbNCE.TabIndex = 18
        '
        'CmbNTF
        '
        Me.CmbNTF.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.CmbNTF.FormattingEnabled = True
        Me.CmbNTF.Location = New System.Drawing.Point(127, 222)
        Me.CmbNTF.Name = "CmbNTF"
        Me.CmbNTF.Size = New System.Drawing.Size(118, 23)
        Me.CmbNTF.TabIndex = 17
        '
        'CmbNTE
        '
        Me.CmbNTE.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.CmbNTE.FormattingEnabled = True
        Me.CmbNTE.Location = New System.Drawing.Point(127, 194)
        Me.CmbNTE.Name = "CmbNTE"
        Me.CmbNTE.Size = New System.Drawing.Size(118, 23)
        Me.CmbNTE.TabIndex = 16
        '
        'BtnGbasedOpen
        '
        Me.BtnGbasedOpen.Location = New System.Drawing.Point(127, 314)
        Me.BtnGbasedOpen.Name = "BtnGbasedOpen"
        Me.BtnGbasedOpen.Size = New System.Drawing.Size(81, 25)
        Me.BtnGbasedOpen.TabIndex = 15
        Me.BtnGbasedOpen.Text = "Oepn"
        Me.BtnGbasedOpen.UseVisualStyleBackColor = True
        '
        'BtnApplyNSave
        '
        Me.BtnApplyNSave.Location = New System.Drawing.Point(220, 314)
        Me.BtnApplyNSave.Name = "BtnApplyNSave"
        Me.BtnApplyNSave.Size = New System.Drawing.Size(70, 25)
        Me.BtnApplyNSave.TabIndex = 14
        Me.BtnApplyNSave.Text = "Save"
        Me.BtnApplyNSave.UseVisualStyleBackColor = True
        '
        'CmbMocyGraph
        '
        Me.CmbMocyGraph.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.CmbMocyGraph.FormattingEnabled = True
        Me.CmbMocyGraph.Location = New System.Drawing.Point(110, 150)
        Me.CmbMocyGraph.Name = "CmbMocyGraph"
        Me.CmbMocyGraph.Size = New System.Drawing.Size(180, 23)
        Me.CmbMocyGraph.TabIndex = 13
        '
        'CmbMyGraph
        '
        Me.CmbMyGraph.BackColor = System.Drawing.SystemColors.Info
        Me.CmbMyGraph.FormattingEnabled = True
        Me.CmbMyGraph.Location = New System.Drawing.Point(109, 120)
        Me.CmbMyGraph.Name = "CmbMyGraph"
        Me.CmbMyGraph.Size = New System.Drawing.Size(180, 23)
        Me.CmbMyGraph.TabIndex = 12
        '
        'CmbFzGraph
        '
        Me.CmbFzGraph.BackColor = System.Drawing.SystemColors.Info
        Me.CmbFzGraph.FormattingEnabled = True
        Me.CmbFzGraph.Location = New System.Drawing.Point(110, 88)
        Me.CmbFzGraph.Name = "CmbFzGraph"
        Me.CmbFzGraph.Size = New System.Drawing.Size(180, 23)
        Me.CmbFzGraph.TabIndex = 11
        '
        'CmbFxGraph
        '
        Me.CmbFxGraph.BackColor = System.Drawing.SystemColors.Info
        Me.CmbFxGraph.FormattingEnabled = True
        Me.CmbFxGraph.Location = New System.Drawing.Point(109, 56)
        Me.CmbFxGraph.Name = "CmbFxGraph"
        Me.CmbFxGraph.Size = New System.Drawing.Size(180, 23)
        Me.CmbFxGraph.TabIndex = 10
        '
        'CmbHICgraph
        '
        Me.CmbHICgraph.BackColor = System.Drawing.SystemColors.Info
        Me.CmbHICgraph.FormattingEnabled = True
        Me.CmbHICgraph.Location = New System.Drawing.Point(110, 22)
        Me.CmbHICgraph.Name = "CmbHICgraph"
        Me.CmbHICgraph.Size = New System.Drawing.Size(215, 23)
        Me.CmbHICgraph.TabIndex = 9
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(12, 158)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(145, 15)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Neck Mocy [Calculated]"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 123)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(59, 15)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "Neck My"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 90)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(53, 15)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Neck Fz"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 55)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 15)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Neck Fx"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(15, 25)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(105, 15)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Head Acc. Graph"
        '
        'CorrelOpenFile
        '
        Me.CorrelOpenFile.FileName = "OpenFileDialog1"
        '
        'FindFolderPath
        '
        Me.FindFolderPath.ShowNewFolderButton = False
        '
        'GraphBasedDlg
        '
        Me.GraphBasedDlg.FileName = "OpenFileDialog1"
        '
        'ProfileSetting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(728, 577)
        Me.Controls.Add(Me.SetUpTab)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "ProfileSetting"
        Me.ShowInTaskbar = False
        Me.Text = "Profile Setting [Hybrid-3]"
        Me.SetUpTab.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage4.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        CType(Me.InjuryScaleLst, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GraphScaleLst, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage5.PerformLayout()
        Me.TabPage6.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SetUpTab As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents ExtLst As System.Windows.Forms.ListBox
    Friend WithEvents ColLst As System.Windows.Forms.ListBox
    Friend WithEvents RowLst As System.Windows.Forms.ListBox
    Friend WithEvents ExtTxt As System.Windows.Forms.TextBox
    Friend WithEvents ColTxt As System.Windows.Forms.TextBox
    Friend WithEvents RowTxt As System.Windows.Forms.TextBox
    Friend WithEvents AddBtn As System.Windows.Forms.Button
    Friend WithEvents SaveBtn As System.Windows.Forms.Button
    Friend WithEvents CFactorTxt As System.Windows.Forms.TextBox
    Friend WithEvents CFactorLbl As System.Windows.Forms.Label
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents GraphLblLst As System.Windows.Forms.ListBox
    Friend WithEvents GraphLblTxt As System.Windows.Forms.TextBox
    Friend WithEvents BtnLblSave As System.Windows.Forms.Button
    Friend WithEvents BtnLblReplace As System.Windows.Forms.Button
    Friend WithEvents TESTLblLst As System.Windows.Forms.ListBox
    Friend WithEvents TESTSaveBtn As System.Windows.Forms.Button
    Friend WithEvents TESTaddBtn As System.Windows.Forms.Button
    Friend WithEvents TESTIdenTxt1 As System.Windows.Forms.TextBox
    Friend WithEvents TESTYTxt As System.Windows.Forms.TextBox
    Friend WithEvents TESTXTxt As System.Windows.Forms.TextBox
    Friend WithEvents TESTLblTxt As System.Windows.Forms.TextBox
    Friend WithEvents TESTYLst As System.Windows.Forms.ListBox
    Friend WithEvents TESTXLst As System.Windows.Forms.ListBox
    Friend WithEvents TESTIdenTxt3 As System.Windows.Forms.TextBox
    Friend WithEvents TESTIdenTxt2 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents GraphScaleLst As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents GraphScaleVal As System.Windows.Forms.TextBox
    Friend WithEvents SelectedGraphTxt As System.Windows.Forms.TextBox
    Friend WithEvents InjuryScaleLst As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents InjuryScaleVal As System.Windows.Forms.TextBox
    Friend WithEvents SelectedInjuryTxt As System.Windows.Forms.TextBox
    Friend WithEvents SaveCorrel As System.Windows.Forms.Button
    Friend WithEvents OpenCorrel As System.Windows.Forms.Button
    Friend WithEvents CorrelOpenFile As System.Windows.Forms.OpenFileDialog
    Friend WithEvents CorrelSaveFile As System.Windows.Forms.SaveFileDialog
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents BtnFindFolder As System.Windows.Forms.Button
    Friend WithEvents LblXMADgic As System.Windows.Forms.Label
    Friend WithEvents FindFolderPath As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents TxtPath As System.Windows.Forms.TextBox
    Friend WithEvents TxtPathHW As System.Windows.Forms.TextBox
    Friend WithEvents BtnFindFolderHW As System.Windows.Forms.Button
    Friend WithEvents LblHW As System.Windows.Forms.Label
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents CmbHICgraph As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents CmbMocyGraph As System.Windows.Forms.ComboBox
    Friend WithEvents CmbMyGraph As System.Windows.Forms.ComboBox
    Friend WithEvents CmbFzGraph As System.Windows.Forms.ComboBox
    Friend WithEvents CmbFxGraph As System.Windows.Forms.ComboBox
    Friend WithEvents BtnApplyNSave As System.Windows.Forms.Button
    Friend WithEvents BtnGbasedOpen As System.Windows.Forms.Button
    Friend WithEvents GraphBasedDlg As System.Windows.Forms.OpenFileDialog
    Friend WithEvents CmbNCF As System.Windows.Forms.ComboBox
    Friend WithEvents CmbNCE As System.Windows.Forms.ComboBox
    Friend WithEvents CmbNTF As System.Windows.Forms.ComboBox
    Friend WithEvents CmbNTE As System.Windows.Forms.ComboBox
    Friend WithEvents GraphBasedSave As System.Windows.Forms.SaveFileDialog
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
End Class
