<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmCorrel
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmCorrel))
        Me.ScoreGrid = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.DescripLbl = New System.Windows.Forms.Label()
        Me.Range1 = New System.Windows.Forms.Label()
        Me.Range2 = New System.Windows.Forms.Label()
        Me.Range3 = New System.Windows.Forms.Label()
        Me.Range4 = New System.Windows.Forms.Label()
        Me.ScoreStatusBar = New System.Windows.Forms.StatusStrip()
        Me.StatusLbl1 = New System.Windows.Forms.ToolStripStatusLabel()
        CType(Me.ScoreGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ScoreStatusBar.SuspendLayout()
        Me.SuspendLayout()
        '
        'ScoreGrid
        '
        Me.ScoreGrid.Location = New System.Drawing.Point(0, 0)
        Me.ScoreGrid.Name = "ScoreGrid"
        Me.ScoreGrid.OcxState = CType(resources.GetObject("ScoreGrid.OcxState"), System.Windows.Forms.AxHost.State)
        Me.ScoreGrid.Size = New System.Drawing.Size(332, 182)
        Me.ScoreGrid.TabIndex = 0
        '
        'DescripLbl
        '
        Me.DescripLbl.AutoSize = True
        Me.DescripLbl.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.DescripLbl.Location = New System.Drawing.Point(12, 187)
        Me.DescripLbl.Name = "DescripLbl"
        Me.DescripLbl.Size = New System.Drawing.Size(422, 45)
        Me.DescripLbl.TabIndex = 1
        Me.DescripLbl.Text = "※ Weighted Integration uses Factor Method." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "※ Interpolation Method - Linear / Int" & _
            "egration Method - Rectangle Rule" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "※ There is a possibility that the NUMERICAL er" & _
            "ror occurs." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Range1
        '
        Me.Range1.BackColor = System.Drawing.Color.Red
        Me.Range1.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Range1.Location = New System.Drawing.Point(279, 87)
        Me.Range1.Name = "Range1"
        Me.Range1.Size = New System.Drawing.Size(55, 29)
        Me.Range1.TabIndex = 2
        Me.Range1.Text = "Label1"
        Me.Range1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Range2
        '
        Me.Range2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Range2.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Range2.Location = New System.Drawing.Point(315, 103)
        Me.Range2.Name = "Range2"
        Me.Range2.Size = New System.Drawing.Size(55, 29)
        Me.Range2.TabIndex = 3
        Me.Range2.Text = "Label1"
        Me.Range2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Range3
        '
        Me.Range3.BackColor = System.Drawing.Color.Yellow
        Me.Range3.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Range3.Location = New System.Drawing.Point(340, 116)
        Me.Range3.Name = "Range3"
        Me.Range3.Size = New System.Drawing.Size(55, 29)
        Me.Range3.TabIndex = 4
        Me.Range3.Text = "Label2"
        Me.Range3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Range4
        '
        Me.Range4.BackColor = System.Drawing.Color.Lime
        Me.Range4.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Range4.Location = New System.Drawing.Point(368, 132)
        Me.Range4.Name = "Range4"
        Me.Range4.Size = New System.Drawing.Size(55, 29)
        Me.Range4.TabIndex = 5
        Me.Range4.Text = "Label3"
        Me.Range4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ScoreStatusBar
        '
        Me.ScoreStatusBar.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.ScoreStatusBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLbl1})
        Me.ScoreStatusBar.Location = New System.Drawing.Point(0, 244)
        Me.ScoreStatusBar.Name = "ScoreStatusBar"
        Me.ScoreStatusBar.Size = New System.Drawing.Size(445, 22)
        Me.ScoreStatusBar.TabIndex = 6
        '
        'StatusLbl1
        '
        Me.StatusLbl1.Name = "StatusLbl1"
        Me.StatusLbl1.Size = New System.Drawing.Size(430, 17)
        Me.StatusLbl1.Spring = True
        Me.StatusLbl1.Text = "Correlation Score"
        Me.StatusLbl1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FrmCorrel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(445, 266)
        Me.Controls.Add(Me.ScoreStatusBar)
        Me.Controls.Add(Me.DescripLbl)
        Me.Controls.Add(Me.Range4)
        Me.Controls.Add(Me.Range3)
        Me.Controls.Add(Me.Range2)
        Me.Controls.Add(Me.Range1)
        Me.Controls.Add(Me.ScoreGrid)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FrmCorrel"
        Me.Text = "Curve Correlation Score"
        CType(Me.ScoreGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ScoreStatusBar.ResumeLayout(False)
        Me.ScoreStatusBar.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ScoreGrid As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents DescripLbl As System.Windows.Forms.Label
    Friend WithEvents Range1 As System.Windows.Forms.Label
    Friend WithEvents Range2 As System.Windows.Forms.Label
    Friend WithEvents Range3 As System.Windows.Forms.Label
    Friend WithEvents Range4 As System.Windows.Forms.Label
    Friend WithEvents ScoreStatusBar As System.Windows.Forms.StatusStrip
    Friend WithEvents StatusLbl1 As System.Windows.Forms.ToolStripStatusLabel
End Class
