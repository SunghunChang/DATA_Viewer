<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmFEconvertingInXML
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmFEconvertingInXML))
        Me.DyOpnBtn = New System.Windows.Forms.Button()
        Me.DyNameTxt = New System.Windows.Forms.TextBox()
        Me.LstFEsystem = New System.Windows.Forms.ListBox()
        Me.BtnConvert = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.StatusLbl = New System.Windows.Forms.ToolStripStatusLabel()
        Me.XMLFileDlg = New System.Windows.Forms.OpenFileDialog()
        Me.KeySave = New System.Windows.Forms.SaveFileDialog()
        Me.ChkRefCoord = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ChkRefELE = New System.Windows.Forms.CheckBox()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DyOpnBtn
        '
        Me.DyOpnBtn.Font = New System.Drawing.Font("맑은 고딕", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.DyOpnBtn.Location = New System.Drawing.Point(197, 43)
        Me.DyOpnBtn.Name = "DyOpnBtn"
        Me.DyOpnBtn.Size = New System.Drawing.Size(129, 25)
        Me.DyOpnBtn.TabIndex = 3
        Me.DyOpnBtn.Text = "Open File"
        Me.DyOpnBtn.UseVisualStyleBackColor = True
        '
        'DyNameTxt
        '
        Me.DyNameTxt.BackColor = System.Drawing.SystemColors.Info
        Me.DyNameTxt.Enabled = False
        Me.DyNameTxt.Font = New System.Drawing.Font("맑은 고딕", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.DyNameTxt.Location = New System.Drawing.Point(22, 12)
        Me.DyNameTxt.Name = "DyNameTxt"
        Me.DyNameTxt.Size = New System.Drawing.Size(304, 25)
        Me.DyNameTxt.TabIndex = 2
        Me.DyNameTxt.Text = "MADYMO XML File"
        '
        'LstFEsystem
        '
        Me.LstFEsystem.Enabled = False
        Me.LstFEsystem.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.LstFEsystem.FormattingEnabled = True
        Me.LstFEsystem.ItemHeight = 15
        Me.LstFEsystem.Location = New System.Drawing.Point(32, 89)
        Me.LstFEsystem.Name = "LstFEsystem"
        Me.LstFEsystem.Size = New System.Drawing.Size(304, 64)
        Me.LstFEsystem.TabIndex = 4
        '
        'BtnConvert
        '
        Me.BtnConvert.Enabled = False
        Me.BtnConvert.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.BtnConvert.Location = New System.Drawing.Point(246, 159)
        Me.BtnConvert.Name = "BtnConvert"
        Me.BtnConvert.Size = New System.Drawing.Size(90, 34)
        Me.BtnConvert.TabIndex = 5
        Me.BtnConvert.Text = "Convert"
        Me.BtnConvert.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLbl})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 339)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(375, 22)
        Me.StatusStrip1.TabIndex = 6
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'StatusLbl
        '
        Me.StatusLbl.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.StatusLbl.Name = "StatusLbl"
        Me.StatusLbl.Size = New System.Drawing.Size(360, 17)
        Me.StatusLbl.Spring = True
        Me.StatusLbl.Text = "Ready"
        Me.StatusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'XMLFileDlg
        '
        Me.XMLFileDlg.FileName = "OpenFileDialog1"
        '
        'ChkRefCoord
        '
        Me.ChkRefCoord.AutoSize = True
        Me.ChkRefCoord.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.ChkRefCoord.Location = New System.Drawing.Point(32, 160)
        Me.ChkRefCoord.Name = "ChkRefCoord"
        Me.ChkRefCoord.Size = New System.Drawing.Size(198, 34)
        Me.ChkRefCoord.TabIndex = 7
        Me.ChkRefCoord.Text = "COORDINATE_REF.CARTESIAN" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(For AirBag Mesh)"
        Me.ChkRefCoord.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(42, 234)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(345, 105)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = resources.GetString("Label1.Text")
        '
        'ChkRefELE
        '
        Me.ChkRefELE.AutoSize = True
        Me.ChkRefELE.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.ChkRefELE.Location = New System.Drawing.Point(32, 197)
        Me.ChkRefELE.Name = "ChkRefELE"
        Me.ChkRefELE.Size = New System.Drawing.Size(134, 34)
        Me.ChkRefELE.TabIndex = 9
        Me.ChkRefELE.Text = "ELEMENT_REF.OOO" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(For AirBag Mesh)"
        Me.ChkRefELE.UseVisualStyleBackColor = True
        '
        'FrmFEconvertingInXML
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(375, 361)
        Me.Controls.Add(Me.ChkRefELE)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ChkRefCoord)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.BtnConvert)
        Me.Controls.Add(Me.LstFEsystem)
        Me.Controls.Add(Me.DyOpnBtn)
        Me.Controls.Add(Me.DyNameTxt)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FrmFEconvertingInXML"
        Me.Text = "Mesh Converting In XML [Mesh Only]"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DyOpnBtn As System.Windows.Forms.Button
    Friend WithEvents DyNameTxt As System.Windows.Forms.TextBox
    Friend WithEvents LstFEsystem As System.Windows.Forms.ListBox
    Friend WithEvents BtnConvert As System.Windows.Forms.Button
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents StatusLbl As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents XMLFileDlg As System.Windows.Forms.OpenFileDialog
    Friend WithEvents KeySave As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ChkRefCoord As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ChkRefELE As System.Windows.Forms.CheckBox
End Class
