<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmProSettingTHOR
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmProSettingTHOR))
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
        Me.TabPage2 = New System.Windows.Forms.TabPage()
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
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.BtnLblSave = New System.Windows.Forms.Button()
        Me.BtnLblReplace = New System.Windows.Forms.Button()
        Me.GraphLblTxt = New System.Windows.Forms.TextBox()
        Me.GraphLblLst = New System.Windows.Forms.ListBox()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.InjuryScaleLst = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.GraphScaleLst = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.SaveCorrel = New System.Windows.Forms.Button()
        Me.OpenCorrel = New System.Windows.Forms.Button()
        Me.InjuryScaleVal = New System.Windows.Forms.TextBox()
        Me.SelectedInjuryTxt = New System.Windows.Forms.TextBox()
        Me.GraphScaleVal = New System.Windows.Forms.TextBox()
        Me.SelectedGraphTxt = New System.Windows.Forms.TextBox()
        Me.CorrelSaveFile = New System.Windows.Forms.SaveFileDialog()
        Me.CorrelOpenFile = New System.Windows.Forms.OpenFileDialog()
        Me.SetUpTab.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        CType(Me.InjuryScaleLst, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GraphScaleLst, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SetUpTab
        '
        Me.SetUpTab.Controls.Add(Me.TabPage1)
        Me.SetUpTab.Controls.Add(Me.TabPage2)
        Me.SetUpTab.Controls.Add(Me.TabPage3)
        Me.SetUpTab.Controls.Add(Me.TabPage4)
        Me.SetUpTab.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SetUpTab.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.SetUpTab.Location = New System.Drawing.Point(0, 0)
        Me.SetUpTab.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.SetUpTab.Name = "SetUpTab"
        Me.SetUpTab.SelectedIndex = 0
        Me.SetUpTab.Size = New System.Drawing.Size(511, 301)
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
        Me.TabPage1.Size = New System.Drawing.Size(503, 273)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Analysis [THOR]"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'CFactorLbl
        '
        Me.CFactorLbl.AutoSize = True
        Me.CFactorLbl.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.CFactorLbl.Location = New System.Drawing.Point(107, 236)
        Me.CFactorLbl.Name = "CFactorLbl"
        Me.CFactorLbl.Size = New System.Drawing.Size(150, 15)
        Me.CFactorLbl.TabIndex = 19
        Me.CFactorLbl.Text = "←Unit Conversion Factor"
        '
        'CFactorTxt
        '
        Me.CFactorTxt.BackColor = System.Drawing.SystemColors.Info
        Me.CFactorTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.CFactorTxt.Location = New System.Drawing.Point(42, 234)
        Me.CFactorTxt.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.CFactorTxt.Name = "CFactorTxt"
        Me.CFactorTxt.Size = New System.Drawing.Size(60, 23)
        Me.CFactorTxt.TabIndex = 18
        '
        'SaveBtn
        '
        Me.SaveBtn.Location = New System.Drawing.Point(282, 231)
        Me.SaveBtn.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.SaveBtn.Name = "SaveBtn"
        Me.SaveBtn.Size = New System.Drawing.Size(52, 23)
        Me.SaveBtn.TabIndex = 17
        Me.SaveBtn.Text = "Save"
        Me.SaveBtn.UseVisualStyleBackColor = True
        '
        'AddBtn
        '
        Me.AddBtn.Location = New System.Drawing.Point(185, 222)
        Me.AddBtn.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.AddBtn.Name = "AddBtn"
        Me.AddBtn.Size = New System.Drawing.Size(82, 26)
        Me.AddBtn.TabIndex = 16
        Me.AddBtn.Text = "Add"
        Me.AddBtn.UseVisualStyleBackColor = True
        '
        'RowTxt
        '
        Me.RowTxt.BackColor = System.Drawing.SystemColors.Info
        Me.RowTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.RowTxt.Location = New System.Drawing.Point(247, 193)
        Me.RowTxt.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.RowTxt.Name = "RowTxt"
        Me.RowTxt.Size = New System.Drawing.Size(78, 23)
        Me.RowTxt.TabIndex = 15
        '
        'ColTxt
        '
        Me.ColTxt.BackColor = System.Drawing.SystemColors.Info
        Me.ColTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.ColTxt.Location = New System.Drawing.Point(145, 197)
        Me.ColTxt.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ColTxt.Name = "ColTxt"
        Me.ColTxt.Size = New System.Drawing.Size(64, 23)
        Me.ColTxt.TabIndex = 14
        '
        'ExtTxt
        '
        Me.ExtTxt.BackColor = System.Drawing.SystemColors.Info
        Me.ExtTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.ExtTxt.Location = New System.Drawing.Point(36, 196)
        Me.ExtTxt.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ExtTxt.Name = "ExtTxt"
        Me.ExtTxt.Size = New System.Drawing.Size(90, 23)
        Me.ExtTxt.TabIndex = 13
        '
        'RowLst
        '
        Me.RowLst.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RowLst.FormattingEnabled = True
        Me.RowLst.ItemHeight = 15
        Me.RowLst.Location = New System.Drawing.Point(242, 16)
        Me.RowLst.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.RowLst.Name = "RowLst"
        Me.RowLst.Size = New System.Drawing.Size(83, 139)
        Me.RowLst.TabIndex = 12
        '
        'ColLst
        '
        Me.ColLst.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ColLst.FormattingEnabled = True
        Me.ColLst.ItemHeight = 15
        Me.ColLst.Location = New System.Drawing.Point(144, 18)
        Me.ColLst.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ColLst.Name = "ColLst"
        Me.ColLst.Size = New System.Drawing.Size(78, 139)
        Me.ColLst.TabIndex = 11
        '
        'ExtLst
        '
        Me.ExtLst.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExtLst.FormattingEnabled = True
        Me.ExtLst.ItemHeight = 15
        Me.ExtLst.Location = New System.Drawing.Point(31, 18)
        Me.ExtLst.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ExtLst.Name = "ExtLst"
        Me.ExtLst.Size = New System.Drawing.Size(96, 139)
        Me.ExtLst.TabIndex = 10
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.TESTIdenTxt3)
        Me.TabPage2.Controls.Add(Me.TESTIdenTxt2)
        Me.TabPage2.Controls.Add(Me.Label3)
        Me.TabPage2.Controls.Add(Me.Label2)
        Me.TabPage2.Controls.Add(Me.Label1)
        Me.TabPage2.Controls.Add(Me.TESTSaveBtn)
        Me.TabPage2.Controls.Add(Me.TESTaddBtn)
        Me.TabPage2.Controls.Add(Me.TESTIdenTxt1)
        Me.TabPage2.Controls.Add(Me.TESTYTxt)
        Me.TabPage2.Controls.Add(Me.TESTXTxt)
        Me.TabPage2.Controls.Add(Me.TESTLblTxt)
        Me.TabPage2.Controls.Add(Me.TESTYLst)
        Me.TabPage2.Controls.Add(Me.TESTXLst)
        Me.TabPage2.Controls.Add(Me.TESTLblLst)
        Me.TabPage2.Location = New System.Drawing.Point(4, 24)
        Me.TabPage2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage2.Size = New System.Drawing.Size(503, 273)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "TEST [DAT-R64/tdm-tdx]"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TESTIdenTxt3
        '
        Me.TESTIdenTxt3.BackColor = System.Drawing.SystemColors.Info
        Me.TESTIdenTxt3.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.TESTIdenTxt3.Location = New System.Drawing.Point(370, 230)
        Me.TESTIdenTxt3.Name = "TESTIdenTxt3"
        Me.TESTIdenTxt3.Size = New System.Drawing.Size(75, 23)
        Me.TESTIdenTxt3.TabIndex = 27
        '
        'TESTIdenTxt2
        '
        Me.TESTIdenTxt2.BackColor = System.Drawing.SystemColors.Info
        Me.TESTIdenTxt2.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.TESTIdenTxt2.Location = New System.Drawing.Point(226, 234)
        Me.TESTIdenTxt2.Name = "TESTIdenTxt2"
        Me.TESTIdenTxt2.Size = New System.Drawing.Size(75, 23)
        Me.TESTIdenTxt2.TabIndex = 26
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Calibri", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(305, 241)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(30, 18)
        Me.Label3.TabIndex = 25
        Me.Label3.Text = "Etc."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(195, 233)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(31, 18)
        Me.Label2.TabIndex = 24
        Me.Label2.Text = "PAS"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(41, 232)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 18)
        Me.Label1.TabIndex = 23
        Me.Label1.Text = "DRV"
        '
        'TESTSaveBtn
        '
        Me.TESTSaveBtn.Location = New System.Drawing.Point(426, 186)
        Me.TESTSaveBtn.Name = "TESTSaveBtn"
        Me.TESTSaveBtn.Size = New System.Drawing.Size(47, 20)
        Me.TESTSaveBtn.TabIndex = 22
        Me.TESTSaveBtn.Text = "Save"
        Me.TESTSaveBtn.UseVisualStyleBackColor = True
        '
        'TESTaddBtn
        '
        Me.TESTaddBtn.Location = New System.Drawing.Point(360, 186)
        Me.TESTaddBtn.Name = "TESTaddBtn"
        Me.TESTaddBtn.Size = New System.Drawing.Size(45, 20)
        Me.TESTaddBtn.TabIndex = 21
        Me.TESTaddBtn.Text = "Add"
        Me.TESTaddBtn.UseVisualStyleBackColor = True
        '
        'TESTIdenTxt1
        '
        Me.TESTIdenTxt1.BackColor = System.Drawing.SystemColors.Info
        Me.TESTIdenTxt1.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.TESTIdenTxt1.Location = New System.Drawing.Point(99, 232)
        Me.TESTIdenTxt1.Name = "TESTIdenTxt1"
        Me.TESTIdenTxt1.Size = New System.Drawing.Size(75, 23)
        Me.TESTIdenTxt1.TabIndex = 20
        '
        'TESTYTxt
        '
        Me.TESTYTxt.BackColor = System.Drawing.SystemColors.Info
        Me.TESTYTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.TESTYTxt.Location = New System.Drawing.Point(360, 160)
        Me.TESTYTxt.Name = "TESTYTxt"
        Me.TESTYTxt.Size = New System.Drawing.Size(114, 23)
        Me.TESTYTxt.TabIndex = 19
        '
        'TESTXTxt
        '
        Me.TESTXTxt.BackColor = System.Drawing.SystemColors.Info
        Me.TESTXTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.TESTXTxt.Location = New System.Drawing.Point(179, 186)
        Me.TESTXTxt.Name = "TESTXTxt"
        Me.TESTXTxt.Size = New System.Drawing.Size(114, 23)
        Me.TESTXTxt.TabIndex = 18
        '
        'TESTLblTxt
        '
        Me.TESTLblTxt.BackColor = System.Drawing.SystemColors.Info
        Me.TESTLblTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.TESTLblTxt.Location = New System.Drawing.Point(37, 198)
        Me.TESTLblTxt.Name = "TESTLblTxt"
        Me.TESTLblTxt.Size = New System.Drawing.Size(114, 23)
        Me.TESTLblTxt.TabIndex = 17
        '
        'TESTYLst
        '
        Me.TESTYLst.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.TESTYLst.FormattingEnabled = True
        Me.TESTYLst.ItemHeight = 15
        Me.TESTYLst.Location = New System.Drawing.Point(339, 18)
        Me.TESTYLst.Name = "TESTYLst"
        Me.TESTYLst.Size = New System.Drawing.Size(126, 109)
        Me.TESTYLst.TabIndex = 16
        '
        'TESTXLst
        '
        Me.TESTXLst.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.TESTXLst.FormattingEnabled = True
        Me.TESTXLst.ItemHeight = 15
        Me.TESTXLst.Location = New System.Drawing.Point(179, 14)
        Me.TESTXLst.Name = "TESTXLst"
        Me.TESTXLst.Size = New System.Drawing.Size(130, 124)
        Me.TESTXLst.TabIndex = 15
        '
        'TESTLblLst
        '
        Me.TESTLblLst.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.TESTLblLst.FormattingEnabled = True
        Me.TESTLblLst.ItemHeight = 15
        Me.TESTLblLst.Location = New System.Drawing.Point(32, 14)
        Me.TESTLblLst.Name = "TESTLblLst"
        Me.TESTLblLst.Size = New System.Drawing.Size(120, 139)
        Me.TESTLblLst.TabIndex = 14
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.BtnLblSave)
        Me.TabPage3.Controls.Add(Me.BtnLblReplace)
        Me.TabPage3.Controls.Add(Me.GraphLblTxt)
        Me.TabPage3.Controls.Add(Me.GraphLblLst)
        Me.TabPage3.Location = New System.Drawing.Point(4, 24)
        Me.TabPage3.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage3.Size = New System.Drawing.Size(503, 273)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Graph Titles"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'BtnLblSave
        '
        Me.BtnLblSave.Location = New System.Drawing.Point(383, 123)
        Me.BtnLblSave.Name = "BtnLblSave"
        Me.BtnLblSave.Size = New System.Drawing.Size(58, 28)
        Me.BtnLblSave.TabIndex = 8
        Me.BtnLblSave.Text = "Save"
        Me.BtnLblSave.UseVisualStyleBackColor = True
        '
        'BtnLblReplace
        '
        Me.BtnLblReplace.Location = New System.Drawing.Point(283, 123)
        Me.BtnLblReplace.Name = "BtnLblReplace"
        Me.BtnLblReplace.Size = New System.Drawing.Size(60, 29)
        Me.BtnLblReplace.TabIndex = 7
        Me.BtnLblReplace.Text = "Replace"
        Me.BtnLblReplace.UseVisualStyleBackColor = True
        '
        'GraphLblTxt
        '
        Me.GraphLblTxt.BackColor = System.Drawing.SystemColors.Info
        Me.GraphLblTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.GraphLblTxt.Location = New System.Drawing.Point(223, 55)
        Me.GraphLblTxt.Name = "GraphLblTxt"
        Me.GraphLblTxt.Size = New System.Drawing.Size(164, 23)
        Me.GraphLblTxt.TabIndex = 6
        '
        'GraphLblLst
        '
        Me.GraphLblLst.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.GraphLblLst.FormattingEnabled = True
        Me.GraphLblLst.ItemHeight = 15
        Me.GraphLblLst.Location = New System.Drawing.Point(64, 55)
        Me.GraphLblLst.Name = "GraphLblLst"
        Me.GraphLblLst.Size = New System.Drawing.Size(132, 154)
        Me.GraphLblLst.TabIndex = 5
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.InjuryScaleLst)
        Me.TabPage4.Controls.Add(Me.GraphScaleLst)
        Me.TabPage4.Controls.Add(Me.SaveCorrel)
        Me.TabPage4.Controls.Add(Me.OpenCorrel)
        Me.TabPage4.Controls.Add(Me.InjuryScaleVal)
        Me.TabPage4.Controls.Add(Me.SelectedInjuryTxt)
        Me.TabPage4.Controls.Add(Me.GraphScaleVal)
        Me.TabPage4.Controls.Add(Me.SelectedGraphTxt)
        Me.TabPage4.Location = New System.Drawing.Point(4, 24)
        Me.TabPage4.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage4.Size = New System.Drawing.Size(503, 273)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Correl. Factor"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'InjuryScaleLst
        '
        Me.InjuryScaleLst.Location = New System.Drawing.Point(267, 45)
        Me.InjuryScaleLst.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.InjuryScaleLst.Name = "InjuryScaleLst"
        Me.InjuryScaleLst.OcxState = CType(resources.GetObject("InjuryScaleLst.OcxState"), System.Windows.Forms.AxHost.State)
        Me.InjuryScaleLst.Size = New System.Drawing.Size(281, 123)
        Me.InjuryScaleLst.TabIndex = 15
        '
        'GraphScaleLst
        '
        Me.GraphScaleLst.Location = New System.Drawing.Point(52, 42)
        Me.GraphScaleLst.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.GraphScaleLst.Name = "GraphScaleLst"
        Me.GraphScaleLst.OcxState = CType(resources.GetObject("GraphScaleLst.OcxState"), System.Windows.Forms.AxHost.State)
        Me.GraphScaleLst.Size = New System.Drawing.Size(210, 127)
        Me.GraphScaleLst.TabIndex = 14
        '
        'SaveCorrel
        '
        Me.SaveCorrel.Location = New System.Drawing.Point(339, 206)
        Me.SaveCorrel.Name = "SaveCorrel"
        Me.SaveCorrel.Size = New System.Drawing.Size(83, 28)
        Me.SaveCorrel.TabIndex = 13
        Me.SaveCorrel.Text = "Save"
        Me.SaveCorrel.UseVisualStyleBackColor = True
        '
        'OpenCorrel
        '
        Me.OpenCorrel.Location = New System.Drawing.Point(245, 206)
        Me.OpenCorrel.Name = "OpenCorrel"
        Me.OpenCorrel.Size = New System.Drawing.Size(76, 29)
        Me.OpenCorrel.TabIndex = 12
        Me.OpenCorrel.Text = "Open"
        Me.OpenCorrel.UseVisualStyleBackColor = True
        '
        'InjuryScaleVal
        '
        Me.InjuryScaleVal.BackColor = System.Drawing.SystemColors.Info
        Me.InjuryScaleVal.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InjuryScaleVal.Location = New System.Drawing.Point(401, 166)
        Me.InjuryScaleVal.Name = "InjuryScaleVal"
        Me.InjuryScaleVal.Size = New System.Drawing.Size(65, 26)
        Me.InjuryScaleVal.TabIndex = 11
        '
        'SelectedInjuryTxt
        '
        Me.SelectedInjuryTxt.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SelectedInjuryTxt.Location = New System.Drawing.Point(245, 166)
        Me.SelectedInjuryTxt.Name = "SelectedInjuryTxt"
        Me.SelectedInjuryTxt.Size = New System.Drawing.Size(122, 26)
        Me.SelectedInjuryTxt.TabIndex = 10
        '
        'GraphScaleVal
        '
        Me.GraphScaleVal.BackColor = System.Drawing.SystemColors.Info
        Me.GraphScaleVal.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GraphScaleVal.Location = New System.Drawing.Point(163, 170)
        Me.GraphScaleVal.Name = "GraphScaleVal"
        Me.GraphScaleVal.Size = New System.Drawing.Size(67, 26)
        Me.GraphScaleVal.TabIndex = 9
        '
        'SelectedGraphTxt
        '
        Me.SelectedGraphTxt.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SelectedGraphTxt.Location = New System.Drawing.Point(38, 170)
        Me.SelectedGraphTxt.Name = "SelectedGraphTxt"
        Me.SelectedGraphTxt.Size = New System.Drawing.Size(118, 26)
        Me.SelectedGraphTxt.TabIndex = 8
        '
        'CorrelOpenFile
        '
        Me.CorrelOpenFile.FileName = "OpenFileDialog1"
        '
        'FrmProSettingTHOR
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(511, 301)
        Me.Controls.Add(Me.SetUpTab)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "FrmProSettingTHOR"
        Me.Text = "Profile Setting [THOR]"
        Me.SetUpTab.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage4.PerformLayout()
        CType(Me.InjuryScaleLst, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GraphScaleLst, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SetUpTab As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents CFactorLbl As System.Windows.Forms.Label
    Friend WithEvents CFactorTxt As System.Windows.Forms.TextBox
    Friend WithEvents SaveBtn As System.Windows.Forms.Button
    Friend WithEvents AddBtn As System.Windows.Forms.Button
    Friend WithEvents RowTxt As System.Windows.Forms.TextBox
    Friend WithEvents ColTxt As System.Windows.Forms.TextBox
    Friend WithEvents ExtTxt As System.Windows.Forms.TextBox
    Friend WithEvents RowLst As System.Windows.Forms.ListBox
    Friend WithEvents ColLst As System.Windows.Forms.ListBox
    Friend WithEvents ExtLst As System.Windows.Forms.ListBox
    Friend WithEvents TESTIdenTxt3 As System.Windows.Forms.TextBox
    Friend WithEvents TESTIdenTxt2 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TESTSaveBtn As System.Windows.Forms.Button
    Friend WithEvents TESTaddBtn As System.Windows.Forms.Button
    Friend WithEvents TESTIdenTxt1 As System.Windows.Forms.TextBox
    Friend WithEvents TESTYTxt As System.Windows.Forms.TextBox
    Friend WithEvents TESTXTxt As System.Windows.Forms.TextBox
    Friend WithEvents TESTLblTxt As System.Windows.Forms.TextBox
    Friend WithEvents TESTYLst As System.Windows.Forms.ListBox
    Friend WithEvents TESTXLst As System.Windows.Forms.ListBox
    Friend WithEvents TESTLblLst As System.Windows.Forms.ListBox
    Friend WithEvents BtnLblSave As System.Windows.Forms.Button
    Friend WithEvents BtnLblReplace As System.Windows.Forms.Button
    Friend WithEvents GraphLblTxt As System.Windows.Forms.TextBox
    Friend WithEvents GraphLblLst As System.Windows.Forms.ListBox
    Friend WithEvents SaveCorrel As System.Windows.Forms.Button
    Friend WithEvents OpenCorrel As System.Windows.Forms.Button
    Friend WithEvents InjuryScaleVal As System.Windows.Forms.TextBox
    Friend WithEvents SelectedInjuryTxt As System.Windows.Forms.TextBox
    Friend WithEvents GraphScaleVal As System.Windows.Forms.TextBox
    Friend WithEvents SelectedGraphTxt As System.Windows.Forms.TextBox
    Friend WithEvents InjuryScaleLst As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents GraphScaleLst As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents CorrelSaveFile As System.Windows.Forms.SaveFileDialog
    Friend WithEvents CorrelOpenFile As System.Windows.Forms.OpenFileDialog
End Class
