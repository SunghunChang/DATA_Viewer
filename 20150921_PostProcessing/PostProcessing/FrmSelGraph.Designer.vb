<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmSelGraph
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
        Me.CmbDR = New System.Windows.Forms.ComboBox()
        Me.LstGraph = New System.Windows.Forms.ListBox()
        Me.TxtTime = New System.Windows.Forms.TextBox()
        Me.TxtDiffVal = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'CmbDR
        '
        Me.CmbDR.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbDR.FormattingEnabled = True
        Me.CmbDR.Location = New System.Drawing.Point(104, 13)
        Me.CmbDR.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.CmbDR.Name = "CmbDR"
        Me.CmbDR.Size = New System.Drawing.Size(189, 23)
        Me.CmbDR.TabIndex = 0
        '
        'LstGraph
        '
        Me.LstGraph.FormattingEnabled = True
        Me.LstGraph.ItemHeight = 15
        Me.LstGraph.Location = New System.Drawing.Point(104, 43)
        Me.LstGraph.Name = "LstGraph"
        Me.LstGraph.Size = New System.Drawing.Size(189, 109)
        Me.LstGraph.TabIndex = 1
        '
        'TxtTime
        '
        Me.TxtTime.Location = New System.Drawing.Point(104, 158)
        Me.TxtTime.Name = "TxtTime"
        Me.TxtTime.Size = New System.Drawing.Size(189, 23)
        Me.TxtTime.TabIndex = 2
        Me.TxtTime.Text = "10"
        Me.TxtTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TxtDiffVal
        '
        Me.TxtDiffVal.Location = New System.Drawing.Point(104, 187)
        Me.TxtDiffVal.Name = "TxtDiffVal"
        Me.TxtDiffVal.Size = New System.Drawing.Size(189, 23)
        Me.TxtDiffVal.TabIndex = 4
        Me.TxtDiffVal.Text = "500"
        Me.TxtDiffVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(206, 216)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(87, 26)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(69, 15)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Select Type"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 88)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(71, 15)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Select DATA"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 161)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(83, 15)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Time Window"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(9, 190)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(89, 15)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Variation Value"
        '
        'FrmSelGraph
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(305, 252)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TxtDiffVal)
        Me.Controls.Add(Me.TxtTime)
        Me.Controls.Add(Me.LstGraph)
        Me.Controls.Add(Me.CmbDR)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmSelGraph"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Select Graph DATA"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents CmbDR As System.Windows.Forms.ComboBox
    Friend WithEvents LstGraph As System.Windows.Forms.ListBox
    Friend WithEvents TxtTime As System.Windows.Forms.TextBox
    Friend WithEvents TxtDiffVal As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
