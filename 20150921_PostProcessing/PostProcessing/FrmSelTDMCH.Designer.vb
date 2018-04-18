<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmSelTDMCH
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
        Me.CmdOK = New System.Windows.Forms.Button()
        Me.TDMChLst = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'CmdOK
        '
        Me.CmdOK.Location = New System.Drawing.Point(284, 112)
        Me.CmdOK.Name = "CmdOK"
        Me.CmdOK.Size = New System.Drawing.Size(82, 24)
        Me.CmdOK.TabIndex = 1
        Me.CmdOK.Text = "Confirm"
        Me.CmdOK.UseVisualStyleBackColor = True
        '
        'TDMChLst
        '
        Me.TDMChLst.Font = New System.Drawing.Font("Calibri", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TDMChLst.FormattingEnabled = True
        Me.TDMChLst.ItemHeight = 22
        Me.TDMChLst.Location = New System.Drawing.Point(12, 12)
        Me.TDMChLst.Name = "TDMChLst"
        Me.TDMChLst.Size = New System.Drawing.Size(354, 92)
        Me.TDMChLst.TabIndex = 2
        '
        'FrmSelTDMCH
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(378, 143)
        Me.Controls.Add(Me.TDMChLst)
        Me.Controls.Add(Me.CmdOK)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmSelTDMCH"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Select TDM TEST DATA Channel Group"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents CmdOK As System.Windows.Forms.Button
    Friend WithEvents TDMChLst As System.Windows.Forms.ListBox
End Class
