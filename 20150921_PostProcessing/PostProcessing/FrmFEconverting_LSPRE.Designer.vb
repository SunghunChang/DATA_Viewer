<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmFEconverting_LSPRE
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmFEconverting_LSPRE))
        Me.KeyFileDlg = New System.Windows.Forms.OpenFileDialog()
        Me.XMLsave = New System.Windows.Forms.SaveFileDialog()
        Me.DyOpnBtn = New System.Windows.Forms.Button()
        Me.DyNameTxt = New System.Windows.Forms.TextBox()
        Me.StatusLbl = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SETchk = New System.Windows.Forms.CheckBox()
        Me.TxtID = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'KeyFileDlg
        '
        Me.KeyFileDlg.FileName = "OpenFileDialog1"
        '
        'DyOpnBtn
        '
        Me.DyOpnBtn.Font = New System.Drawing.Font("맑은 고딕", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.DyOpnBtn.Location = New System.Drawing.Point(383, 11)
        Me.DyOpnBtn.Name = "DyOpnBtn"
        Me.DyOpnBtn.Size = New System.Drawing.Size(109, 25)
        Me.DyOpnBtn.TabIndex = 3
        Me.DyOpnBtn.Text = "Open File"
        Me.DyOpnBtn.UseVisualStyleBackColor = True
        '
        'DyNameTxt
        '
        Me.DyNameTxt.BackColor = System.Drawing.SystemColors.Info
        Me.DyNameTxt.Enabled = False
        Me.DyNameTxt.Font = New System.Drawing.Font("맑은 고딕", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.DyNameTxt.Location = New System.Drawing.Point(12, 12)
        Me.DyNameTxt.Name = "DyNameTxt"
        Me.DyNameTxt.Size = New System.Drawing.Size(365, 25)
        Me.DyNameTxt.TabIndex = 2
        Me.DyNameTxt.Text = "LS-DYNA Key File"
        '
        'StatusLbl
        '
        Me.StatusLbl.AutoSize = True
        Me.StatusLbl.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusLbl.ForeColor = System.Drawing.Color.Red
        Me.StatusLbl.Location = New System.Drawing.Point(9, 76)
        Me.StatusLbl.Name = "StatusLbl"
        Me.StatusLbl.Size = New System.Drawing.Size(48, 18)
        Me.StatusLbl.TabIndex = 4
        Me.StatusLbl.Text = "Label1"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.Desktop
        Me.Label1.Location = New System.Drawing.Point(9, 107)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(483, 36)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "※ All Materials and Properties are converted to NULL." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "   (Originally, D3PLOT doe" & _
            "sn't include a information of material and property.)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'ToolTip1
        '
        Me.ToolTip1.AutomaticDelay = 100
        Me.ToolTip1.AutoPopDelay = 10000
        Me.ToolTip1.InitialDelay = 100
        Me.ToolTip1.ReshowDelay = 20
        Me.ToolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
        Me.ToolTip1.ToolTipTitle = "Information"
        '
        'SETchk
        '
        Me.SETchk.AutoSize = True
        Me.SETchk.Checked = True
        Me.SETchk.CheckState = System.Windows.Forms.CheckState.Checked
        Me.SETchk.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SETchk.Location = New System.Drawing.Point(14, 47)
        Me.SETchk.Name = "SETchk"
        Me.SETchk.Size = New System.Drawing.Size(149, 19)
        Me.SETchk.TabIndex = 6
        Me.SETchk.Text = "Export *SET_PART_LIST"
        Me.SETchk.UseVisualStyleBackColor = True
        '
        'TxtID
        '
        Me.TxtID.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtID.Location = New System.Drawing.Point(169, 43)
        Me.TxtID.Name = "TxtID"
        Me.TxtID.Size = New System.Drawing.Size(122, 23)
        Me.TxtID.TabIndex = 7
        Me.TxtID.Text = "9999999"
        Me.TxtID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'FrmFEconverting_LSPRE
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(512, 152)
        Me.Controls.Add(Me.TxtID)
        Me.Controls.Add(Me.SETchk)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.StatusLbl)
        Me.Controls.Add(Me.DyOpnBtn)
        Me.Controls.Add(Me.DyNameTxt)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FrmFEconverting_LSPRE"
        Me.Text = "LS-DYNA to MADYMO (LS-PREPOST)"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents KeyFileDlg As System.Windows.Forms.OpenFileDialog
    Friend WithEvents XMLsave As System.Windows.Forms.SaveFileDialog
    Friend WithEvents DyOpnBtn As System.Windows.Forms.Button
    Friend WithEvents DyNameTxt As System.Windows.Forms.TextBox
    Friend WithEvents StatusLbl As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents SETchk As System.Windows.Forms.CheckBox
    Friend WithEvents TxtID As System.Windows.Forms.TextBox
End Class
