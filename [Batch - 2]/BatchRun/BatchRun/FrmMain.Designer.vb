<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMain
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMain))
        Me.CmdOpn = New System.Windows.Forms.Button()
        Me.TxtStatus = New System.Windows.Forms.TextBox()
        Me.OpenDlg = New System.Windows.Forms.OpenFileDialog()
        Me.MainFileGrid = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.BtnRun = New System.Windows.Forms.Button()
        Me.BtnIni = New System.Windows.Forms.Button()
        Me.ProgressBarTot = New System.Windows.Forms.ProgressBar()
        Me.LblTot = New System.Windows.Forms.Label()
        Me.VerTooltip = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.MainFileGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CmdOpn
        '
        Me.CmdOpn.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdOpn.Location = New System.Drawing.Point(621, 221)
        Me.CmdOpn.Name = "CmdOpn"
        Me.CmdOpn.Size = New System.Drawing.Size(113, 35)
        Me.CmdOpn.TabIndex = 0
        Me.CmdOpn.Text = "Select File(s)"
        Me.CmdOpn.UseVisualStyleBackColor = True
        '
        'TxtStatus
        '
        Me.TxtStatus.BackColor = System.Drawing.SystemColors.Info
        Me.TxtStatus.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtStatus.Location = New System.Drawing.Point(12, 262)
        Me.TxtStatus.Multiline = True
        Me.TxtStatus.Name = "TxtStatus"
        Me.TxtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TxtStatus.Size = New System.Drawing.Size(841, 351)
        Me.TxtStatus.TabIndex = 1
        '
        'OpenDlg
        '
        Me.OpenDlg.FileName = "OpenFileDialog1"
        '
        'MainFileGrid
        '
        Me.MainFileGrid.AllowDrop = True
        Me.MainFileGrid.Location = New System.Drawing.Point(12, 12)
        Me.MainFileGrid.Name = "MainFileGrid"
        Me.MainFileGrid.OcxState = CType(resources.GetObject("MainFileGrid.OcxState"), System.Windows.Forms.AxHost.State)
        Me.MainFileGrid.Size = New System.Drawing.Size(841, 203)
        Me.MainFileGrid.TabIndex = 2
        '
        'BtnRun
        '
        Me.BtnRun.Enabled = False
        Me.BtnRun.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnRun.Location = New System.Drawing.Point(740, 221)
        Me.BtnRun.Name = "BtnRun"
        Me.BtnRun.Size = New System.Drawing.Size(113, 35)
        Me.BtnRun.TabIndex = 3
        Me.BtnRun.Text = "Run Batch Job"
        Me.BtnRun.UseVisualStyleBackColor = True
        '
        'BtnIni
        '
        Me.BtnIni.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnIni.Location = New System.Drawing.Point(12, 221)
        Me.BtnIni.Name = "BtnIni"
        Me.BtnIni.Size = New System.Drawing.Size(113, 35)
        Me.BtnIni.TabIndex = 4
        Me.BtnIni.Text = "Initialize"
        Me.BtnIni.UseVisualStyleBackColor = True
        '
        'ProgressBarTot
        '
        Me.ProgressBarTot.Cursor = System.Windows.Forms.Cursors.Default
        Me.ProgressBarTot.Location = New System.Drawing.Point(12, 634)
        Me.ProgressBarTot.Name = "ProgressBarTot"
        Me.ProgressBarTot.Size = New System.Drawing.Size(840, 19)
        Me.ProgressBarTot.Step = 1
        Me.ProgressBarTot.TabIndex = 5
        '
        'LblTot
        '
        Me.LblTot.AutoSize = True
        Me.LblTot.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTot.Location = New System.Drawing.Point(12, 616)
        Me.LblTot.Name = "LblTot"
        Me.LblTot.Size = New System.Drawing.Size(109, 15)
        Me.LblTot.TabIndex = 7
        Me.LblTot.Text = "※ Total Job Status"
        '
        'VerTooltip
        '
        Me.VerTooltip.AutomaticDelay = 100
        Me.VerTooltip.AutoPopDelay = 5000
        Me.VerTooltip.InitialDelay = 100
        Me.VerTooltip.ReshowDelay = 20
        '
        'FrmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(865, 661)
        Me.Controls.Add(Me.LblTot)
        Me.Controls.Add(Me.ProgressBarTot)
        Me.Controls.Add(Me.BtnIni)
        Me.Controls.Add(Me.BtnRun)
        Me.Controls.Add(Me.MainFileGrid)
        Me.Controls.Add(Me.TxtStatus)
        Me.Controls.Add(Me.CmdOpn)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FrmMain"
        Me.Text = "MADYMO Batch Job Run [By Sunghun, Chang]"
        CType(Me.MainFileGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents CmdOpn As System.Windows.Forms.Button
    Friend WithEvents TxtStatus As System.Windows.Forms.TextBox
    Friend WithEvents OpenDlg As System.Windows.Forms.OpenFileDialog
    Friend WithEvents MainFileGrid As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents BtnRun As System.Windows.Forms.Button
    Friend WithEvents BtnIni As System.Windows.Forms.Button
    Friend WithEvents ProgressBarTot As System.Windows.Forms.ProgressBar
    Friend WithEvents LblTot As System.Windows.Forms.Label
    Friend WithEvents VerTooltip As System.Windows.Forms.ToolTip

End Class
