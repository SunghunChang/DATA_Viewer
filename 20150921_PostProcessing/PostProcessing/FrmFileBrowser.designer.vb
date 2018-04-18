<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmFileBrowser
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmFileBrowser))
        Me.InputCmb = New System.Windows.Forms.ComboBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.FileLstView = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.FileConMnu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RemoteRunToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LocalRunMADYMOToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CommentTxt = New System.Windows.Forms.TextBox()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.FileConMnu.SuspendLayout()
        Me.SuspendLayout()
        '
        'InputCmb
        '
        Me.InputCmb.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.InputCmb.FormattingEnabled = True
        Me.InputCmb.Location = New System.Drawing.Point(35, 12)
        Me.InputCmb.Name = "InputCmb"
        Me.InputCmb.Size = New System.Drawing.Size(481, 23)
        Me.InputCmb.TabIndex = 0
        Me.InputCmb.Text = "C:\"
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Button1.Location = New System.Drawing.Point(522, 12)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(119, 23)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Browse Folder"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'FileLstView
        '
        Me.FileLstView.AllowDrop = True
        Me.FileLstView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6})
        Me.FileLstView.ContextMenuStrip = Me.FileConMnu
        Me.FileLstView.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.FileLstView.FullRowSelect = True
        Me.FileLstView.Location = New System.Drawing.Point(35, 53)
        Me.FileLstView.Name = "FileLstView"
        Me.FileLstView.Size = New System.Drawing.Size(953, 250)
        Me.FileLstView.TabIndex = 3
        Me.FileLstView.UseCompatibleStateImageBehavior = False
        Me.FileLstView.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "File"
        Me.ColumnHeader1.Width = 92
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Ext."
        Me.ColumnHeader2.Width = 25
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Comments"
        Me.ColumnHeader3.Width = 146
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Size"
        Me.ColumnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Last Modified"
        Me.ColumnHeader5.Width = 25
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "FullPath"
        '
        'FileConMnu
        '
        Me.FileConMnu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RemoteRunToolStripMenuItem, Me.LocalRunMADYMOToolStripMenuItem})
        Me.FileConMnu.Name = "FileConMnu"
        Me.FileConMnu.Size = New System.Drawing.Size(215, 48)
        '
        'RemoteRunToolStripMenuItem
        '
        Me.RemoteRunToolStripMenuItem.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.RemoteRunToolStripMenuItem.Name = "RemoteRunToolStripMenuItem"
        Me.RemoteRunToolStripMenuItem.Size = New System.Drawing.Size(214, 22)
        Me.RemoteRunToolStripMenuItem.Text = "Remote Run [MADYMO]"
        '
        'LocalRunMADYMOToolStripMenuItem
        '
        Me.LocalRunMADYMOToolStripMenuItem.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.LocalRunMADYMOToolStripMenuItem.Name = "LocalRunMADYMOToolStripMenuItem"
        Me.LocalRunMADYMOToolStripMenuItem.Size = New System.Drawing.Size(214, 22)
        Me.LocalRunMADYMOToolStripMenuItem.Text = "Local Run [MADYMO]"
        '
        'CommentTxt
        '
        Me.CommentTxt.BackColor = System.Drawing.SystemColors.Info
        Me.CommentTxt.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.CommentTxt.Location = New System.Drawing.Point(12, 313)
        Me.CommentTxt.Multiline = True
        Me.CommentTxt.Name = "CommentTxt"
        Me.CommentTxt.Size = New System.Drawing.Size(1029, 56)
        Me.CommentTxt.TabIndex = 4
        '
        'FrmFileBrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1087, 381)
        Me.Controls.Add(Me.CommentTxt)
        Me.Controls.Add(Me.FileLstView)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.InputCmb)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FrmFileBrowser"
        Me.Text = "File Comment Browser"
        Me.FileConMnu.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents InputCmb As System.Windows.Forms.ComboBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents FileLstView As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents CommentTxt As System.Windows.Forms.TextBox
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents FileConMnu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents RemoteRunToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LocalRunMADYMOToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
