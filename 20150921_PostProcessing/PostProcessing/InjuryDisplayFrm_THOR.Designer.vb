<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InjuryDisplayFrm_THOR
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InjuryDisplayFrm_THOR))
        Me.SplitConMain = New System.Windows.Forms.SplitContainer()
        Me.DescripTxt = New System.Windows.Forms.TextBox()
        Me.OpenLst = New System.Windows.Forms.ListBox()
        Me.PathLbl = New System.Windows.Forms.Label()
        Me.ScoreBoard = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Peak_Injury1 = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.InjuryLbl1 = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.BtnInjuryGraph = New System.Windows.Forms.Button()
        Me.BtnValGraph = New System.Windows.Forms.Button()
        Me.ReloadBtn = New System.Windows.Forms.Button()
        Me.PPTBtn = New System.Windows.Forms.Button()
        Me.CommentsToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.OpenDlg = New System.Windows.Forms.OpenFileDialog()
        Me.AddFileDlg = New System.Windows.Forms.OpenFileDialog()
        Me.OpenWithXMADgic = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.OpenWithXMADgicToolStripMenuItemToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenKn3WithHyperViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.SplitConMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitConMain.Panel1.SuspendLayout()
        Me.SplitConMain.Panel2.SuspendLayout()
        Me.SplitConMain.SuspendLayout()
        Me.ScoreBoard.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.Peak_Injury1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InjuryLbl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.OpenWithXMADgic.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitConMain
        '
        Me.SplitConMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitConMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitConMain.Location = New System.Drawing.Point(0, 0)
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
        Me.SplitConMain.Panel2.Controls.Add(Me.ScoreBoard)
        Me.SplitConMain.Panel2.Controls.Add(Me.Button1)
        Me.SplitConMain.Panel2.Controls.Add(Me.BtnInjuryGraph)
        Me.SplitConMain.Panel2.Controls.Add(Me.BtnValGraph)
        Me.SplitConMain.Panel2.Controls.Add(Me.ReloadBtn)
        Me.SplitConMain.Panel2.Controls.Add(Me.PPTBtn)
        Me.SplitConMain.Size = New System.Drawing.Size(640, 312)
        Me.SplitConMain.SplitterDistance = 90
        Me.SplitConMain.TabIndex = 0
        '
        'DescripTxt
        '
        Me.DescripTxt.BackColor = System.Drawing.SystemColors.Info
        Me.DescripTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.DescripTxt.Location = New System.Drawing.Point(386, 24)
        Me.DescripTxt.Multiline = True
        Me.DescripTxt.Name = "DescripTxt"
        Me.DescripTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.DescripTxt.Size = New System.Drawing.Size(80, 44)
        Me.DescripTxt.TabIndex = 8
        '
        'OpenLst
        '
        Me.OpenLst.BackColor = System.Drawing.SystemColors.Window
        Me.OpenLst.Font = New System.Drawing.Font("Calibri", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OpenLst.FormattingEnabled = True
        Me.OpenLst.ItemHeight = 17
        Me.OpenLst.Location = New System.Drawing.Point(31, 48)
        Me.OpenLst.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.OpenLst.Name = "OpenLst"
        Me.OpenLst.Size = New System.Drawing.Size(213, 21)
        Me.OpenLst.TabIndex = 7
        '
        'PathLbl
        '
        Me.PathLbl.AutoSize = True
        Me.PathLbl.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.PathLbl.Location = New System.Drawing.Point(28, 24)
        Me.PathLbl.Name = "PathLbl"
        Me.PathLbl.Size = New System.Drawing.Size(45, 15)
        Me.PathLbl.TabIndex = 6
        Me.PathLbl.Text = "Label1"
        '
        'ScoreBoard
        '
        Me.ScoreBoard.Controls.Add(Me.TabPage1)
        Me.ScoreBoard.Font = New System.Drawing.Font("굴림", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.ScoreBoard.Location = New System.Drawing.Point(21, 16)
        Me.ScoreBoard.Name = "ScoreBoard"
        Me.ScoreBoard.SelectedIndex = 0
        Me.ScoreBoard.Size = New System.Drawing.Size(577, 112)
        Me.ScoreBoard.TabIndex = 12
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Peak_Injury1)
        Me.TabPage1.Controls.Add(Me.InjuryLbl1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(569, 86)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "TabPage1"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Peak_Injury1
        '
        Me.Peak_Injury1.Location = New System.Drawing.Point(162, 30)
        Me.Peak_Injury1.Name = "Peak_Injury1"
        Me.Peak_Injury1.OcxState = CType(resources.GetObject("Peak_Injury1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.Peak_Injury1.Size = New System.Drawing.Size(168, 38)
        Me.Peak_Injury1.TabIndex = 1
        '
        'InjuryLbl1
        '
        Me.InjuryLbl1.Location = New System.Drawing.Point(3, 6)
        Me.InjuryLbl1.Name = "InjuryLbl1"
        Me.InjuryLbl1.OcxState = CType(resources.GetObject("InjuryLbl1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.InjuryLbl1.Size = New System.Drawing.Size(126, 43)
        Me.InjuryLbl1.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Button1.Location = New System.Drawing.Point(88, 175)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(79, 28)
        Me.Button1.TabIndex = 11
        Me.Button1.Text = "Add Case"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'BtnInjuryGraph
        '
        Me.BtnInjuryGraph.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.BtnInjuryGraph.Location = New System.Drawing.Point(374, 133)
        Me.BtnInjuryGraph.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.BtnInjuryGraph.Name = "BtnInjuryGraph"
        Me.BtnInjuryGraph.Size = New System.Drawing.Size(102, 29)
        Me.BtnInjuryGraph.TabIndex = 8
        Me.BtnInjuryGraph.Text = "Injury Graph"
        Me.BtnInjuryGraph.UseVisualStyleBackColor = True
        '
        'BtnValGraph
        '
        Me.BtnValGraph.Enabled = False
        Me.BtnValGraph.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.BtnValGraph.Location = New System.Drawing.Point(264, 133)
        Me.BtnValGraph.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.BtnValGraph.Name = "BtnValGraph"
        Me.BtnValGraph.Size = New System.Drawing.Size(104, 29)
        Me.BtnValGraph.TabIndex = 7
        Me.BtnValGraph.Text = "Injury Values"
        Me.BtnValGraph.UseVisualStyleBackColor = True
        '
        'ReloadBtn
        '
        Me.ReloadBtn.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.ReloadBtn.Location = New System.Drawing.Point(40, 133)
        Me.ReloadBtn.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ReloadBtn.Name = "ReloadBtn"
        Me.ReloadBtn.Size = New System.Drawing.Size(74, 29)
        Me.ReloadBtn.TabIndex = 10
        Me.ReloadBtn.Text = "Reload"
        Me.ReloadBtn.UseVisualStyleBackColor = True
        '
        'PPTBtn
        '
        Me.PPTBtn.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.PPTBtn.Location = New System.Drawing.Point(120, 133)
        Me.PPTBtn.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.PPTBtn.Name = "PPTBtn"
        Me.PPTBtn.Size = New System.Drawing.Size(117, 29)
        Me.PPTBtn.TabIndex = 9
        Me.PPTBtn.Text = "Export Table"
        Me.PPTBtn.UseVisualStyleBackColor = True
        '
        'OpenDlg
        '
        Me.OpenDlg.FileName = "OpenFileDialog1"
        '
        'AddFileDlg
        '
        Me.AddFileDlg.FileName = "OpenFileDialog1"
        '
        'OpenWithXMADgic
        '
        Me.OpenWithXMADgic.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenWithXMADgicToolStripMenuItemToolStripMenuItem, Me.OpenKn3WithHyperViewToolStripMenuItem})
        Me.OpenWithXMADgic.Name = "OpenWithXMADgic"
        Me.OpenWithXMADgic.Size = New System.Drawing.Size(217, 48)
        '
        'OpenWithXMADgicToolStripMenuItemToolStripMenuItem
        '
        Me.OpenWithXMADgicToolStripMenuItemToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.XMADgic
        Me.OpenWithXMADgicToolStripMenuItemToolStripMenuItem.Name = "OpenWithXMADgicToolStripMenuItemToolStripMenuItem"
        Me.OpenWithXMADgicToolStripMenuItemToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.OpenWithXMADgicToolStripMenuItemToolStripMenuItem.Text = "Open xml with XMADgic"
        '
        'OpenKn3WithHyperViewToolStripMenuItem
        '
        Me.OpenKn3WithHyperViewToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.HyperView
        Me.OpenKn3WithHyperViewToolStripMenuItem.Name = "OpenKn3WithHyperViewToolStripMenuItem"
        Me.OpenKn3WithHyperViewToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.OpenKn3WithHyperViewToolStripMenuItem.Text = "Open kn3 with HyperView"
        '
        'InjuryDisplayFrm_THOR
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(640, 312)
        Me.Controls.Add(Me.SplitConMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "InjuryDisplayFrm_THOR"
        Me.Text = "InjuryDisplayFrm_THOR"
        Me.SplitConMain.Panel1.ResumeLayout(False)
        Me.SplitConMain.Panel1.PerformLayout()
        Me.SplitConMain.Panel2.ResumeLayout(False)
        CType(Me.SplitConMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitConMain.ResumeLayout(False)
        Me.ScoreBoard.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.Peak_Injury1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InjuryLbl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.OpenWithXMADgic.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitConMain As System.Windows.Forms.SplitContainer
    Friend WithEvents DescripTxt As System.Windows.Forms.TextBox
    Friend WithEvents OpenLst As System.Windows.Forms.ListBox
    Friend WithEvents PathLbl As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents BtnInjuryGraph As System.Windows.Forms.Button
    Friend WithEvents BtnValGraph As System.Windows.Forms.Button
    Friend WithEvents ReloadBtn As System.Windows.Forms.Button
    Friend WithEvents PPTBtn As System.Windows.Forms.Button
    Friend WithEvents ScoreBoard As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents Peak_Injury1 As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents InjuryLbl1 As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents CommentsToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents OpenDlg As System.Windows.Forms.OpenFileDialog
    Friend WithEvents AddFileDlg As System.Windows.Forms.OpenFileDialog
    Friend WithEvents OpenWithXMADgic As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents OpenWithXMADgicToolStripMenuItemToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenKn3WithHyperViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
