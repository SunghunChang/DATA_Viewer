<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmFEconverting
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmFEconverting))
        Me.DyNameTxt = New System.Windows.Forms.TextBox()
        Me.DyOpnBtn = New System.Windows.Forms.Button()
        Me.KeyFileDlg = New System.Windows.Forms.OpenFileDialog()
        Me.ResultTxt = New System.Windows.Forms.TextBox()
        Me.DescriptionLbl = New System.Windows.Forms.Label()
        Me.XMLsave = New System.Windows.Forms.SaveFileDialog()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ProStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ChkMaterials = New System.Windows.Forms.CheckBox()
        Me.ChkNull = New System.Windows.Forms.CheckBox()
        Me.CardDescription = New System.Windows.Forms.Label()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DyNameTxt
        '
        Me.DyNameTxt.BackColor = System.Drawing.SystemColors.Info
        Me.DyNameTxt.Enabled = False
        Me.DyNameTxt.Font = New System.Drawing.Font("맑은 고딕", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.DyNameTxt.Location = New System.Drawing.Point(12, 12)
        Me.DyNameTxt.Name = "DyNameTxt"
        Me.DyNameTxt.Size = New System.Drawing.Size(173, 25)
        Me.DyNameTxt.TabIndex = 0
        Me.DyNameTxt.Text = "LS-DYNA Key File"
        '
        'DyOpnBtn
        '
        Me.DyOpnBtn.Font = New System.Drawing.Font("맑은 고딕", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.DyOpnBtn.Location = New System.Drawing.Point(202, 11)
        Me.DyOpnBtn.Name = "DyOpnBtn"
        Me.DyOpnBtn.Size = New System.Drawing.Size(129, 25)
        Me.DyOpnBtn.TabIndex = 1
        Me.DyOpnBtn.Text = "Open File"
        Me.DyOpnBtn.UseVisualStyleBackColor = True
        '
        'KeyFileDlg
        '
        Me.KeyFileDlg.FileName = "OpenFileDialog1"
        '
        'ResultTxt
        '
        Me.ResultTxt.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ResultTxt.Location = New System.Drawing.Point(12, 42)
        Me.ResultTxt.Multiline = True
        Me.ResultTxt.Name = "ResultTxt"
        Me.ResultTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.ResultTxt.Size = New System.Drawing.Size(319, 43)
        Me.ResultTxt.TabIndex = 2
        '
        'DescriptionLbl
        '
        Me.DescriptionLbl.AutoSize = True
        Me.DescriptionLbl.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DescriptionLbl.Location = New System.Drawing.Point(9, 102)
        Me.DescriptionLbl.Name = "DescriptionLbl"
        Me.DescriptionLbl.Size = New System.Drawing.Size(43, 15)
        Me.DescriptionLbl.TabIndex = 4
        Me.DescriptionLbl.Text = "Label1"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ProStatus})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 357)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(362, 24)
        Me.StatusStrip1.TabIndex = 5
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ProStatus
        '
        Me.ProStatus.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.ProStatus.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner
        Me.ProStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ProStatus.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.ProStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ProStatus.Name = "ProStatus"
        Me.ProStatus.Size = New System.Drawing.Size(316, 19)
        Me.ProStatus.Spring = True
        Me.ProStatus.Text = "Ready"
        Me.ProStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ChkMaterials
        '
        Me.ChkMaterials.AutoSize = True
        Me.ChkMaterials.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkMaterials.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ChkMaterials.Location = New System.Drawing.Point(96, 98)
        Me.ChkMaterials.Name = "ChkMaterials"
        Me.ChkMaterials.Size = New System.Drawing.Size(266, 34)
        Me.ChkMaterials.TabIndex = 6
        Me.ChkMaterials.Text = "Write MATERIAL INCLUDE Element into XML" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "[Converted by Y.H.M]"
        Me.ChkMaterials.UseVisualStyleBackColor = True
        '
        'ChkNull
        '
        Me.ChkNull.AutoSize = True
        Me.ChkNull.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.ChkNull.Location = New System.Drawing.Point(36, 138)
        Me.ChkNull.Name = "ChkNull"
        Me.ChkNull.Size = New System.Drawing.Size(238, 19)
        Me.ChkNull.TabIndex = 7
        Me.ChkNull.Text = "Convert All Material to Null Material"
        Me.ChkNull.UseVisualStyleBackColor = True
        '
        'CardDescription
        '
        Me.CardDescription.AutoSize = True
        Me.CardDescription.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.CardDescription.ForeColor = System.Drawing.Color.Red
        Me.CardDescription.Location = New System.Drawing.Point(24, 182)
        Me.CardDescription.Name = "CardDescription"
        Me.CardDescription.Size = New System.Drawing.Size(236, 165)
        Me.CardDescription.TabIndex = 8
        Me.CardDescription.Text = resources.GetString("CardDescription.Text")
        '
        'FrmFEconverting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(362, 381)
        Me.Controls.Add(Me.CardDescription)
        Me.Controls.Add(Me.ChkNull)
        Me.Controls.Add(Me.ChkMaterials)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.DescriptionLbl)
        Me.Controls.Add(Me.ResultTxt)
        Me.Controls.Add(Me.DyOpnBtn)
        Me.Controls.Add(Me.DyNameTxt)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FrmFEconverting"
        Me.Text = "LS-DYNA to MADYMO"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DyNameTxt As System.Windows.Forms.TextBox
    Friend WithEvents DyOpnBtn As System.Windows.Forms.Button
    Friend WithEvents KeyFileDlg As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ResultTxt As System.Windows.Forms.TextBox
    Friend WithEvents DescriptionLbl As System.Windows.Forms.Label
    Friend WithEvents XMLsave As System.Windows.Forms.SaveFileDialog
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ProStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ChkMaterials As System.Windows.Forms.CheckBox
    Friend WithEvents ChkNull As System.Windows.Forms.CheckBox
    Friend WithEvents CardDescription As System.Windows.Forms.Label
End Class
