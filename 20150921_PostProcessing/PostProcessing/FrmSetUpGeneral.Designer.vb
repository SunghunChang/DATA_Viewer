<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmSetUpGeneral
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmSetUpGeneral))
        Me.NCAPsetting = New System.Windows.Forms.GroupBox()
        Me.CmbTHOR = New System.Windows.Forms.ComboBox()
        Me.CmbH3 = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PopupSetting = New System.Windows.Forms.GroupBox()
        Me.TxtHeight = New System.Windows.Forms.TextBox()
        Me.TxtWidth = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.BtnSave = New System.Windows.Forms.Button()
        Me.CorrelSetting = New System.Windows.Forms.GroupBox()
        Me.CorelTxt = New System.Windows.Forms.TextBox()
        Me.CorrelWeightGrid = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.NCAPsetting.SuspendLayout()
        Me.PopupSetting.SuspendLayout()
        Me.CorrelSetting.SuspendLayout()
        CType(Me.CorrelWeightGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'NCAPsetting
        '
        Me.NCAPsetting.Controls.Add(Me.CmbTHOR)
        Me.NCAPsetting.Controls.Add(Me.CmbH3)
        Me.NCAPsetting.Controls.Add(Me.Label3)
        Me.NCAPsetting.Controls.Add(Me.Label2)
        Me.NCAPsetting.Controls.Add(Me.Label1)
        Me.NCAPsetting.Location = New System.Drawing.Point(13, 12)
        Me.NCAPsetting.Name = "NCAPsetting"
        Me.NCAPsetting.Size = New System.Drawing.Size(244, 111)
        Me.NCAPsetting.TabIndex = 0
        Me.NCAPsetting.TabStop = False
        Me.NCAPsetting.Text = "NCAP Window"
        '
        'CmbTHOR
        '
        Me.CmbTHOR.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbTHOR.FormattingEnabled = True
        Me.CmbTHOR.Location = New System.Drawing.Point(90, 70)
        Me.CmbTHOR.Name = "CmbTHOR"
        Me.CmbTHOR.Size = New System.Drawing.Size(125, 23)
        Me.CmbTHOR.TabIndex = 4
        '
        'CmbH3
        '
        Me.CmbH3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbH3.FormattingEnabled = True
        Me.CmbH3.Location = New System.Drawing.Point(90, 41)
        Me.CmbH3.Name = "CmbH3"
        Me.CmbH3.Size = New System.Drawing.Size(125, 23)
        Me.CmbH3.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(21, 73)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 15)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "THOR"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(21, 44)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Hybrid-Ⅲ"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(5, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(106, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "※ Start Up Panel"
        '
        'PopupSetting
        '
        Me.PopupSetting.Controls.Add(Me.TxtHeight)
        Me.PopupSetting.Controls.Add(Me.TxtWidth)
        Me.PopupSetting.Controls.Add(Me.Label5)
        Me.PopupSetting.Controls.Add(Me.Label4)
        Me.PopupSetting.Location = New System.Drawing.Point(13, 129)
        Me.PopupSetting.Name = "PopupSetting"
        Me.PopupSetting.Size = New System.Drawing.Size(245, 84)
        Me.PopupSetting.TabIndex = 1
        Me.PopupSetting.TabStop = False
        Me.PopupSetting.Text = "Pop-Up Graph Window"
        '
        'TxtHeight
        '
        Me.TxtHeight.BackColor = System.Drawing.SystemColors.Info
        Me.TxtHeight.Location = New System.Drawing.Point(91, 49)
        Me.TxtHeight.Name = "TxtHeight"
        Me.TxtHeight.Size = New System.Drawing.Size(125, 23)
        Me.TxtHeight.TabIndex = 3
        '
        'TxtWidth
        '
        Me.TxtWidth.BackColor = System.Drawing.SystemColors.Info
        Me.TxtWidth.Location = New System.Drawing.Point(91, 20)
        Me.TxtWidth.Name = "TxtWidth"
        Me.TxtWidth.Size = New System.Drawing.Size(125, 23)
        Me.TxtWidth.TabIndex = 2
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(27, 52)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(46, 15)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Height"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(27, 23)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(42, 15)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Width"
        '
        'BtnSave
        '
        Me.BtnSave.Location = New System.Drawing.Point(235, 147)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(100, 25)
        Me.BtnSave.TabIndex = 2
        Me.BtnSave.Text = "Save"
        Me.BtnSave.UseVisualStyleBackColor = True
        '
        'CorrelSetting
        '
        Me.CorrelSetting.Controls.Add(Me.CorelTxt)
        Me.CorrelSetting.Controls.Add(Me.CorrelWeightGrid)
        Me.CorrelSetting.Location = New System.Drawing.Point(28, 219)
        Me.CorrelSetting.Name = "CorrelSetting"
        Me.CorrelSetting.Size = New System.Drawing.Size(256, 119)
        Me.CorrelSetting.TabIndex = 3
        Me.CorrelSetting.TabStop = False
        Me.CorrelSetting.Text = "Curve Correlation Criteria Weight"
        '
        'CorelTxt
        '
        Me.CorelTxt.BackColor = System.Drawing.SystemColors.Info
        Me.CorelTxt.Location = New System.Drawing.Point(104, 71)
        Me.CorelTxt.Name = "CorelTxt"
        Me.CorelTxt.Size = New System.Drawing.Size(125, 23)
        Me.CorelTxt.TabIndex = 4
        Me.CorelTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'CorrelWeightGrid
        '
        Me.CorrelWeightGrid.Location = New System.Drawing.Point(6, 22)
        Me.CorrelWeightGrid.Name = "CorrelWeightGrid"
        Me.CorrelWeightGrid.OcxState = CType(resources.GetObject("CorrelWeightGrid.OcxState"), System.Windows.Forms.AxHost.State)
        Me.CorrelWeightGrid.Size = New System.Drawing.Size(224, 43)
        Me.CorrelWeightGrid.TabIndex = 0
        '
        'FrmSetUpGeneral
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(379, 396)
        Me.Controls.Add(Me.CorrelSetting)
        Me.Controls.Add(Me.BtnSave)
        Me.Controls.Add(Me.PopupSetting)
        Me.Controls.Add(Me.NCAPsetting)
        Me.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmSetUpGeneral"
        Me.Text = "General Setting"
        Me.NCAPsetting.ResumeLayout(False)
        Me.NCAPsetting.PerformLayout()
        Me.PopupSetting.ResumeLayout(False)
        Me.PopupSetting.PerformLayout()
        Me.CorrelSetting.ResumeLayout(False)
        Me.CorrelSetting.PerformLayout()
        CType(Me.CorrelWeightGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents NCAPsetting As System.Windows.Forms.GroupBox
    Friend WithEvents PopupSetting As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CmbTHOR As System.Windows.Forms.ComboBox
    Friend WithEvents CmbH3 As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TxtHeight As System.Windows.Forms.TextBox
    Friend WithEvents TxtWidth As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents BtnSave As System.Windows.Forms.Button
    Friend WithEvents CorrelSetting As System.Windows.Forms.GroupBox
    Friend WithEvents CorrelWeightGrid As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents CorelTxt As System.Windows.Forms.TextBox
End Class
