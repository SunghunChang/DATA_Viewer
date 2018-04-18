<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmCMDprompt
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmCMDprompt))
        Me.OutputTextBox = New System.Windows.Forms.TextBox()
        Me.InputTextBox = New System.Windows.Forms.ComboBox()
        Me.ExecuteButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'OutputTextBox
        '
        Me.OutputTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OutputTextBox.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.OutputTextBox.Location = New System.Drawing.Point(10, 10)
        Me.OutputTextBox.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.OutputTextBox.Multiline = True
        Me.OutputTextBox.Name = "OutputTextBox"
        Me.OutputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.OutputTextBox.Size = New System.Drawing.Size(710, 206)
        Me.OutputTextBox.TabIndex = 3
        '
        'InputTextBox
        '
        Me.InputTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.InputTextBox.BackColor = System.Drawing.SystemColors.Info
        Me.InputTextBox.Font = New System.Drawing.Font("Consolas", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InputTextBox.FormattingEnabled = True
        Me.InputTextBox.Location = New System.Drawing.Point(10, 219)
        Me.InputTextBox.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.InputTextBox.Name = "InputTextBox"
        Me.InputTextBox.Size = New System.Drawing.Size(336, 25)
        Me.InputTextBox.TabIndex = 0
        '
        'ExecuteButton
        '
        Me.ExecuteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExecuteButton.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExecuteButton.Location = New System.Drawing.Point(609, 219)
        Me.ExecuteButton.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ExecuteButton.Name = "ExecuteButton"
        Me.ExecuteButton.Size = New System.Drawing.Size(111, 22)
        Me.ExecuteButton.TabIndex = 1
        Me.ExecuteButton.Text = "Run Command"
        Me.ExecuteButton.UseVisualStyleBackColor = True
        '
        'FrmCMDprompt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(731, 243)
        Me.Controls.Add(Me.ExecuteButton)
        Me.Controls.Add(Me.InputTextBox)
        Me.Controls.Add(Me.OutputTextBox)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "FrmCMDprompt"
        Me.Text = "Command Prompt by C.S.H"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OutputTextBox As System.Windows.Forms.TextBox
    Friend WithEvents InputTextBox As System.Windows.Forms.ComboBox
    Friend WithEvents ExecuteButton As System.Windows.Forms.Button
End Class
