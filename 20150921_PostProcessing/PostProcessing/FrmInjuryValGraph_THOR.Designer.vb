<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmInjuryValGraph_THOR
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
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmInjuryValGraph_THOR))
        Me.InjuryContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CopyToClipBoardBMPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SplitMain = New System.Windows.Forms.SplitContainer()
        Me.OptBox = New System.Windows.Forms.GroupBox()
        Me.ChkMarker = New System.Windows.Forms.CheckBox()
        Me.CmbSeries = New System.Windows.Forms.ComboBox()
        Me.CmbLineBar = New System.Windows.Forms.ComboBox()
        Me.ChkSuPo = New System.Windows.Forms.CheckBox()
        Me.Intervaltxt = New System.Windows.Forms.TextBox()
        Me.MaxTxt = New System.Windows.Forms.TextBox()
        Me.MinTxt = New System.Windows.Forms.TextBox()
        Me.InjuryLst = New System.Windows.Forms.ListBox()
        Me.InjuryValChrt = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.DetailBox = New System.Windows.Forms.GroupBox()
        Me.DetailTxt = New System.Windows.Forms.TextBox()
        Me.MDlst = New System.Windows.Forms.ListBox()
        Me.MDLstMnu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RenameToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RenameTxt = New System.Windows.Forms.ToolStripTextBox()
        Me.InjuryContextMenu.SuspendLayout()
        CType(Me.SplitMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitMain.Panel1.SuspendLayout()
        Me.SplitMain.Panel2.SuspendLayout()
        Me.SplitMain.SuspendLayout()
        Me.OptBox.SuspendLayout()
        CType(Me.InjuryValChrt, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DetailBox.SuspendLayout()
        Me.MDLstMnu.SuspendLayout()
        Me.SuspendLayout()
        '
        'InjuryContextMenu
        '
        Me.InjuryContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyToClipBoardBMPToolStripMenuItem})
        Me.InjuryContextMenu.Name = "InjuryContextMenu"
        Me.InjuryContextMenu.Size = New System.Drawing.Size(211, 26)
        '
        'CopyToClipBoardBMPToolStripMenuItem
        '
        Me.CopyToClipBoardBMPToolStripMenuItem.Name = "CopyToClipBoardBMPToolStripMenuItem"
        Me.CopyToClipBoardBMPToolStripMenuItem.Size = New System.Drawing.Size(210, 22)
        Me.CopyToClipBoardBMPToolStripMenuItem.Text = "Copy to ClipBoard [BMP]"
        '
        'SplitMain
        '
        Me.SplitMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitMain.Location = New System.Drawing.Point(0, 0)
        Me.SplitMain.Name = "SplitMain"
        '
        'SplitMain.Panel1
        '
        Me.SplitMain.Panel1.Controls.Add(Me.MDlst)
        Me.SplitMain.Panel1.Controls.Add(Me.OptBox)
        Me.SplitMain.Panel1.Controls.Add(Me.Intervaltxt)
        Me.SplitMain.Panel1.Controls.Add(Me.MaxTxt)
        Me.SplitMain.Panel1.Controls.Add(Me.MinTxt)
        Me.SplitMain.Panel1.Controls.Add(Me.InjuryLst)
        Me.SplitMain.Panel1.Controls.Add(Me.InjuryValChrt)
        '
        'SplitMain.Panel2
        '
        Me.SplitMain.Panel2.Controls.Add(Me.DetailBox)
        Me.SplitMain.Size = New System.Drawing.Size(818, 315)
        Me.SplitMain.SplitterDistance = 527
        Me.SplitMain.SplitterWidth = 6
        Me.SplitMain.TabIndex = 7
        '
        'OptBox
        '
        Me.OptBox.Controls.Add(Me.ChkMarker)
        Me.OptBox.Controls.Add(Me.CmbSeries)
        Me.OptBox.Controls.Add(Me.CmbLineBar)
        Me.OptBox.Controls.Add(Me.ChkSuPo)
        Me.OptBox.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.OptBox.Location = New System.Drawing.Point(42, 209)
        Me.OptBox.Name = "OptBox"
        Me.OptBox.Size = New System.Drawing.Size(402, 92)
        Me.OptBox.TabIndex = 12
        Me.OptBox.TabStop = False
        Me.OptBox.Text = "Etc"
        '
        'ChkMarker
        '
        Me.ChkMarker.AutoSize = True
        Me.ChkMarker.Checked = True
        Me.ChkMarker.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkMarker.Location = New System.Drawing.Point(27, 45)
        Me.ChkMarker.Name = "ChkMarker"
        Me.ChkMarker.Size = New System.Drawing.Size(69, 19)
        Me.ChkMarker.TabIndex = 5
        Me.ChkMarker.Text = "Marker"
        Me.ChkMarker.UseVisualStyleBackColor = True
        '
        'CmbSeries
        '
        Me.CmbSeries.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbSeries.FormattingEnabled = True
        Me.CmbSeries.Location = New System.Drawing.Point(119, 16)
        Me.CmbSeries.Name = "CmbSeries"
        Me.CmbSeries.Size = New System.Drawing.Size(142, 23)
        Me.CmbSeries.TabIndex = 4
        '
        'CmbLineBar
        '
        Me.CmbLineBar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbLineBar.FormattingEnabled = True
        Me.CmbLineBar.Items.AddRange(New Object() {"Line", "Bar [Thin]", "Bar [Thick]"})
        Me.CmbLineBar.Location = New System.Drawing.Point(267, 16)
        Me.CmbLineBar.Name = "CmbLineBar"
        Me.CmbLineBar.Size = New System.Drawing.Size(91, 23)
        Me.CmbLineBar.TabIndex = 3
        '
        'ChkSuPo
        '
        Me.ChkSuPo.AutoSize = True
        Me.ChkSuPo.Location = New System.Drawing.Point(6, 20)
        Me.ChkSuPo.Name = "ChkSuPo"
        Me.ChkSuPo.Size = New System.Drawing.Size(107, 19)
        Me.ChkSuPo.TabIndex = 0
        Me.ChkSuPo.Text = "Pile Up Graph"
        Me.ChkSuPo.UseVisualStyleBackColor = True
        '
        'Intervaltxt
        '
        Me.Intervaltxt.BackColor = System.Drawing.SystemColors.Info
        Me.Intervaltxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Intervaltxt.Location = New System.Drawing.Point(210, 153)
        Me.Intervaltxt.Name = "Intervaltxt"
        Me.Intervaltxt.Size = New System.Drawing.Size(56, 23)
        Me.Intervaltxt.TabIndex = 11
        Me.Intervaltxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'MaxTxt
        '
        Me.MaxTxt.BackColor = System.Drawing.SystemColors.Info
        Me.MaxTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.MaxTxt.Location = New System.Drawing.Point(144, 163)
        Me.MaxTxt.Name = "MaxTxt"
        Me.MaxTxt.Size = New System.Drawing.Size(56, 23)
        Me.MaxTxt.TabIndex = 10
        Me.MaxTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'MinTxt
        '
        Me.MinTxt.BackColor = System.Drawing.SystemColors.Info
        Me.MinTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.MinTxt.Location = New System.Drawing.Point(32, 157)
        Me.MinTxt.Name = "MinTxt"
        Me.MinTxt.Size = New System.Drawing.Size(61, 23)
        Me.MinTxt.TabIndex = 9
        Me.MinTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'InjuryLst
        '
        Me.InjuryLst.Font = New System.Drawing.Font("맑은 고딕", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.InjuryLst.FormattingEnabled = True
        Me.InjuryLst.ItemHeight = 17
        Me.InjuryLst.Location = New System.Drawing.Point(42, 29)
        Me.InjuryLst.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.InjuryLst.Name = "InjuryLst"
        Me.InjuryLst.Size = New System.Drawing.Size(139, 106)
        Me.InjuryLst.TabIndex = 8
        '
        'InjuryValChrt
        '
        Me.InjuryValChrt.BorderSkin.PageColor = System.Drawing.Color.Transparent
        ChartArea1.Name = "ChartArea1"
        Me.InjuryValChrt.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.InjuryValChrt.Legends.Add(Legend1)
        Me.InjuryValChrt.Location = New System.Drawing.Point(210, 15)
        Me.InjuryValChrt.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.InjuryValChrt.Name = "InjuryValChrt"
        Series1.ChartArea = "ChartArea1"
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Me.InjuryValChrt.Series.Add(Series1)
        Me.InjuryValChrt.Size = New System.Drawing.Size(173, 94)
        Me.InjuryValChrt.TabIndex = 7
        Me.InjuryValChrt.Text = "Chart1"
        '
        'DetailBox
        '
        Me.DetailBox.Controls.Add(Me.DetailTxt)
        Me.DetailBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DetailBox.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.DetailBox.Location = New System.Drawing.Point(0, 0)
        Me.DetailBox.Name = "DetailBox"
        Me.DetailBox.Size = New System.Drawing.Size(281, 311)
        Me.DetailBox.TabIndex = 1
        Me.DetailBox.TabStop = False
        Me.DetailBox.Text = "Selected Series Detail"
        '
        'DetailTxt
        '
        Me.DetailTxt.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DetailTxt.Location = New System.Drawing.Point(3, 19)
        Me.DetailTxt.Multiline = True
        Me.DetailTxt.Name = "DetailTxt"
        Me.DetailTxt.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.DetailTxt.Size = New System.Drawing.Size(275, 289)
        Me.DetailTxt.TabIndex = 0
        '
        'MDlst
        '
        Me.MDlst.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.MDlst.FormattingEnabled = True
        Me.MDlst.ItemHeight = 15
        Me.MDlst.Location = New System.Drawing.Point(309, 122)
        Me.MDlst.Name = "MDlst"
        Me.MDlst.Size = New System.Drawing.Size(56, 64)
        Me.MDlst.TabIndex = 13
        '
        'MDLstMnu
        '
        Me.MDLstMnu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RenameToolStripMenuItem})
        Me.MDLstMnu.Name = "MDLstMnu"
        Me.MDLstMnu.Size = New System.Drawing.Size(153, 48)
        '
        'RenameToolStripMenuItem
        '
        Me.RenameToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RenameTxt})
        Me.RenameToolStripMenuItem.Name = "RenameToolStripMenuItem"
        Me.RenameToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.RenameToolStripMenuItem.Text = "Rename"
        '
        'RenameTxt
        '
        Me.RenameTxt.BackColor = System.Drawing.SystemColors.Info
        Me.RenameTxt.Name = "RenameTxt"
        Me.RenameTxt.Size = New System.Drawing.Size(100, 23)
        '
        'FrmInjuryValGraph_THOR
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(818, 315)
        Me.Controls.Add(Me.SplitMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "FrmInjuryValGraph_THOR"
        Me.Text = "Injury Values"
        Me.InjuryContextMenu.ResumeLayout(False)
        Me.SplitMain.Panel1.ResumeLayout(False)
        Me.SplitMain.Panel1.PerformLayout()
        Me.SplitMain.Panel2.ResumeLayout(False)
        CType(Me.SplitMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitMain.ResumeLayout(False)
        Me.OptBox.ResumeLayout(False)
        Me.OptBox.PerformLayout()
        CType(Me.InjuryValChrt, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DetailBox.ResumeLayout(False)
        Me.DetailBox.PerformLayout()
        Me.MDLstMnu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents InjuryContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents CopyToClipBoardBMPToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitMain As System.Windows.Forms.SplitContainer
    Friend WithEvents OptBox As System.Windows.Forms.GroupBox
    Friend WithEvents CmbSeries As System.Windows.Forms.ComboBox
    Friend WithEvents CmbLineBar As System.Windows.Forms.ComboBox
    Friend WithEvents ChkSuPo As System.Windows.Forms.CheckBox
    Friend WithEvents Intervaltxt As System.Windows.Forms.TextBox
    Friend WithEvents MaxTxt As System.Windows.Forms.TextBox
    Friend WithEvents MinTxt As System.Windows.Forms.TextBox
    Friend WithEvents InjuryLst As System.Windows.Forms.ListBox
    Friend WithEvents InjuryValChrt As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents DetailBox As System.Windows.Forms.GroupBox
    Friend WithEvents DetailTxt As System.Windows.Forms.TextBox
    Friend WithEvents ChkMarker As System.Windows.Forms.CheckBox
    Friend WithEvents MDlst As System.Windows.Forms.ListBox
    Friend WithEvents MDLstMnu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents RenameToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RenameTxt As System.Windows.Forms.ToolStripTextBox
End Class
