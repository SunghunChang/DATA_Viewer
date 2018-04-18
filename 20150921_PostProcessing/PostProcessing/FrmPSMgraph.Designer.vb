<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmPSMgraph
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
        Dim ChartArea2 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend2 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Me.LstRefAnal = New System.Windows.Forms.ListBox()
        Me.LstRefTEST = New System.Windows.Forms.ListBox()
        Me.ChrtDATA = New System.Windows.Forms.DataVisualization.Charting.Chart()
        CType(Me.ChrtDATA, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LstRefAnal
        '
        Me.LstRefAnal.FormattingEnabled = True
        Me.LstRefAnal.ItemHeight = 21
        Me.LstRefAnal.Items.AddRange(New Object() {"Reference X [Analysis]", "Reference Y [Analysis]", "Reference Z [Analysis]"})
        Me.LstRefAnal.Location = New System.Drawing.Point(12, 16)
        Me.LstRefAnal.Name = "LstRefAnal"
        Me.LstRefAnal.Size = New System.Drawing.Size(272, 67)
        Me.LstRefAnal.TabIndex = 0
        '
        'LstRefTEST
        '
        Me.LstRefTEST.FormattingEnabled = True
        Me.LstRefTEST.ItemHeight = 21
        Me.LstRefTEST.Items.AddRange(New Object() {"Reference X [TEST]", "Reference Y [TEST]", "Reference Z [TEST]"})
        Me.LstRefTEST.Location = New System.Drawing.Point(335, 16)
        Me.LstRefTEST.Name = "LstRefTEST"
        Me.LstRefTEST.Size = New System.Drawing.Size(278, 67)
        Me.LstRefTEST.TabIndex = 1
        '
        'ChrtDATA
        '
        ChartArea2.Name = "ChartArea1"
        Me.ChrtDATA.ChartAreas.Add(ChartArea2)
        Legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom
        Legend2.Name = "Legend1"
        Me.ChrtDATA.Legends.Add(Legend2)
        Me.ChrtDATA.Location = New System.Drawing.Point(17, 97)
        Me.ChrtDATA.Name = "ChrtDATA"
        Me.ChrtDATA.Size = New System.Drawing.Size(595, 347)
        Me.ChrtDATA.TabIndex = 2
        Me.ChrtDATA.Text = "Chart1"
        '
        'FrmPSMgraph
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(643, 465)
        Me.Controls.Add(Me.ChrtDATA)
        Me.Controls.Add(Me.LstRefTEST)
        Me.Controls.Add(Me.LstRefAnal)
        Me.Font = New System.Drawing.Font("Calibri", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "FrmPSMgraph"
        Me.Text = "Reference Point"
        CType(Me.ChrtDATA, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LstRefAnal As System.Windows.Forms.ListBox
    Friend WithEvents LstRefTEST As System.Windows.Forms.ListBox
    Friend WithEvents ChrtDATA As System.Windows.Forms.DataVisualization.Charting.Chart
End Class
