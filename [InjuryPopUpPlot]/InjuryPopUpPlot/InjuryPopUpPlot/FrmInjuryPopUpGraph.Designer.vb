<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InjuryPopUp
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
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InjuryPopUp))
        Me.ChartContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RenameChartTitleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CmbChrtAreaTitle = New System.Windows.Forms.ToolStripComboBox()
        Me.ChartTitleTxt = New System.Windows.Forms.ToolStripTextBox()
        Me.XaxitTitleTxt = New System.Windows.Forms.ToolStripTextBox()
        Me.YaxisTitleTxt = New System.Windows.Forms.ToolStripTextBox()
        Me.Yaxis2TitleTxt = New System.Windows.Forms.ToolStripTextBox()
        Me.ChartOptionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.CToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyToClipBoardDataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.XYPairAllSeriesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.XYToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.XToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.YToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.AddAdditionalChartToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClearAdditionalChartToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExportXYDataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportToExcelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.MathToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.IntegrationSimpsonsRuleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DifferentialForwardDifferenceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.Find3msToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.IntervalTxt = New System.Windows.Forms.ToolStripTextBox()
        Me.FindDropRiseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.IntervalTxt2 = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.FilterCFCToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CFC60ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CFC180ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CFC600ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CFC1000ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TEMPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveXYDlg = New System.Windows.Forms.SaveFileDialog()
        Me.ToolTipCh = New System.Windows.Forms.ToolTip(Me.components)
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.StatusLbl = New System.Windows.Forms.ToolStripStatusLabel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.InjuryChart = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.DataBox = New System.Windows.Forms.GroupBox()
        Me.ChkSize = New System.Windows.Forms.CheckBox()
        Me.LblXY = New System.Windows.Forms.Label()
        Me.MathGroup = New System.Windows.Forms.GroupBox()
        Me.LblOffset = New System.Windows.Forms.Label()
        Me.Yoffset = New System.Windows.Forms.TextBox()
        Me.Xoffset = New System.Windows.Forms.TextBox()
        Me.LblScale = New System.Windows.Forms.Label()
        Me.Yscale = New System.Windows.Forms.TextBox()
        Me.Xscale = New System.Windows.Forms.TextBox()
        Me.ChkMath = New System.Windows.Forms.CheckBox()
        Me.LblMax = New System.Windows.Forms.Label()
        Me.LblMin = New System.Windows.Forms.Label()
        Me.ChkLst = New System.Windows.Forms.CheckedListBox()
        Me.ListBoxMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RenameItemToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RenameList = New System.Windows.Forms.ToolStripTextBox()
        Me.MoveItemToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CmbChartArea = New System.Windows.Forms.ToolStripComboBox()
        Me.To1stChartToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MoveItemAxisYToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChartContextMenu.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.InjuryChart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DataBox.SuspendLayout()
        Me.MathGroup.SuspendLayout()
        Me.ListBoxMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'ChartContextMenu
        '
        Me.ChartContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RenameChartTitleToolStripMenuItem, Me.ChartOptionToolStripMenuItem, Me.ToolStripSeparator6, Me.CToolStripMenuItem, Me.CopyToClipBoardDataToolStripMenuItem, Me.ToolStripSeparator1, Me.AddAdditionalChartToolStripMenuItem, Me.ClearAdditionalChartToolStripMenuItem, Me.ToolStripSeparator2, Me.ExportXYDataToolStripMenuItem, Me.ExportToExcelToolStripMenuItem, Me.ToolStripSeparator3, Me.MathToolStripMenuItem, Me.TEMPToolStripMenuItem})
        Me.ChartContextMenu.Name = "ChartContextMenu"
        Me.ChartContextMenu.Size = New System.Drawing.Size(258, 270)
        '
        'RenameChartTitleToolStripMenuItem
        '
        Me.RenameChartTitleToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CmbChrtAreaTitle, Me.ChartTitleTxt, Me.XaxitTitleTxt, Me.YaxisTitleTxt, Me.Yaxis2TitleTxt})
        Me.RenameChartTitleToolStripMenuItem.Name = "RenameChartTitleToolStripMenuItem"
        Me.RenameChartTitleToolStripMenuItem.Size = New System.Drawing.Size(257, 22)
        Me.RenameChartTitleToolStripMenuItem.Text = "Rename Chart [Title/Axis]"
        '
        'CmbChrtAreaTitle
        '
        Me.CmbChrtAreaTitle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbChrtAreaTitle.Name = "CmbChrtAreaTitle"
        Me.CmbChrtAreaTitle.Size = New System.Drawing.Size(121, 23)
        '
        'ChartTitleTxt
        '
        Me.ChartTitleTxt.BackColor = System.Drawing.SystemColors.Info
        Me.ChartTitleTxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ChartTitleTxt.Name = "ChartTitleTxt"
        Me.ChartTitleTxt.Size = New System.Drawing.Size(100, 23)
        Me.ChartTitleTxt.ToolTipText = "Graph Title"
        '
        'XaxitTitleTxt
        '
        Me.XaxitTitleTxt.BackColor = System.Drawing.SystemColors.Info
        Me.XaxitTitleTxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.XaxitTitleTxt.Name = "XaxitTitleTxt"
        Me.XaxitTitleTxt.Size = New System.Drawing.Size(100, 23)
        Me.XaxitTitleTxt.ToolTipText = "X Axis Title"
        '
        'YaxisTitleTxt
        '
        Me.YaxisTitleTxt.BackColor = System.Drawing.SystemColors.Info
        Me.YaxisTitleTxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.YaxisTitleTxt.Name = "YaxisTitleTxt"
        Me.YaxisTitleTxt.Size = New System.Drawing.Size(100, 23)
        Me.YaxisTitleTxt.ToolTipText = "Y Axis Title"
        '
        'Yaxis2TitleTxt
        '
        Me.Yaxis2TitleTxt.BackColor = System.Drawing.SystemColors.Info
        Me.Yaxis2TitleTxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Yaxis2TitleTxt.Name = "Yaxis2TitleTxt"
        Me.Yaxis2TitleTxt.Size = New System.Drawing.Size(100, 23)
        Me.Yaxis2TitleTxt.ToolTipText = "Y2 Axis Title"
        '
        'ChartOptionToolStripMenuItem
        '
        Me.ChartOptionToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.Menu_Option
        Me.ChartOptionToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White
        Me.ChartOptionToolStripMenuItem.Name = "ChartOptionToolStripMenuItem"
        Me.ChartOptionToolStripMenuItem.Size = New System.Drawing.Size(257, 22)
        Me.ChartOptionToolStripMenuItem.Text = "Chart Option {F1}"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(254, 6)
        '
        'CToolStripMenuItem
        '
        Me.CToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.Menu_CopyBmp
        Me.CToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White
        Me.CToolStripMenuItem.Name = "CToolStripMenuItem"
        Me.CToolStripMenuItem.Size = New System.Drawing.Size(257, 22)
        Me.CToolStripMenuItem.Text = "Copy to ClipBoard [BMP] {Ctrl+C}"
        '
        'CopyToClipBoardDataToolStripMenuItem
        '
        Me.CopyToClipBoardDataToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.XYPairAllSeriesToolStripMenuItem, Me.XYToolStripMenuItem, Me.XToolStripMenuItem, Me.YToolStripMenuItem})
        Me.CopyToClipBoardDataToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.Menu_Copy
        Me.CopyToClipBoardDataToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White
        Me.CopyToClipBoardDataToolStripMenuItem.Name = "CopyToClipBoardDataToolStripMenuItem"
        Me.CopyToClipBoardDataToolStripMenuItem.Size = New System.Drawing.Size(257, 22)
        Me.CopyToClipBoardDataToolStripMenuItem.Text = "Copy to ClipBoard [Data] {Ctrl+X}"
        '
        'XYPairAllSeriesToolStripMenuItem
        '
        Me.XYPairAllSeriesToolStripMenuItem.Name = "XYPairAllSeriesToolStripMenuItem"
        Me.XYPairAllSeriesToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.XYPairAllSeriesToolStripMenuItem.Text = "{X,Y} pair (All Series)"
        '
        'XYToolStripMenuItem
        '
        Me.XYToolStripMenuItem.Name = "XYToolStripMenuItem"
        Me.XYToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.XYToolStripMenuItem.Text = "{X,Y} pair"
        '
        'XToolStripMenuItem
        '
        Me.XToolStripMenuItem.Name = "XToolStripMenuItem"
        Me.XToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.XToolStripMenuItem.Text = "X points"
        '
        'YToolStripMenuItem
        '
        Me.YToolStripMenuItem.Name = "YToolStripMenuItem"
        Me.YToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.YToolStripMenuItem.Text = "Y points"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(254, 6)
        '
        'AddAdditionalChartToolStripMenuItem
        '
        Me.AddAdditionalChartToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.Graph
        Me.AddAdditionalChartToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White
        Me.AddAdditionalChartToolStripMenuItem.Name = "AddAdditionalChartToolStripMenuItem"
        Me.AddAdditionalChartToolStripMenuItem.Size = New System.Drawing.Size(257, 22)
        Me.AddAdditionalChartToolStripMenuItem.Text = "Add Additional Chart {Insert}"
        '
        'ClearAdditionalChartToolStripMenuItem
        '
        Me.ClearAdditionalChartToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.Trash
        Me.ClearAdditionalChartToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White
        Me.ClearAdditionalChartToolStripMenuItem.Name = "ClearAdditionalChartToolStripMenuItem"
        Me.ClearAdditionalChartToolStripMenuItem.Size = New System.Drawing.Size(257, 22)
        Me.ClearAdditionalChartToolStripMenuItem.Text = "Clear Additional Chart {Del}"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(254, 6)
        '
        'ExportXYDataToolStripMenuItem
        '
        Me.ExportXYDataToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.Menu_exportData
        Me.ExportXYDataToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White
        Me.ExportXYDataToolStripMenuItem.Name = "ExportXYDataToolStripMenuItem"
        Me.ExportXYDataToolStripMenuItem.Size = New System.Drawing.Size(257, 22)
        Me.ExportXYDataToolStripMenuItem.Text = "Export XY Data"
        '
        'ExportToExcelToolStripMenuItem
        '
        Me.ExportToExcelToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.Menu_Excel
        Me.ExportToExcelToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White
        Me.ExportToExcelToolStripMenuItem.Name = "ExportToExcelToolStripMenuItem"
        Me.ExportToExcelToolStripMenuItem.Size = New System.Drawing.Size(257, 22)
        Me.ExportToExcelToolStripMenuItem.Text = "Export to Excel"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(254, 6)
        '
        'MathToolStripMenuItem
        '
        Me.MathToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.IntegrationSimpsonsRuleToolStripMenuItem, Me.DifferentialForwardDifferenceToolStripMenuItem, Me.ToolStripSeparator4, Me.Find3msToolStripMenuItem, Me.FindDropRiseToolStripMenuItem, Me.ToolStripSeparator5, Me.FilterCFCToolStripMenuItem})
        Me.MathToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.Menu_Math
        Me.MathToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White
        Me.MathToolStripMenuItem.Name = "MathToolStripMenuItem"
        Me.MathToolStripMenuItem.Size = New System.Drawing.Size(257, 22)
        Me.MathToolStripMenuItem.Text = "Math"
        '
        'IntegrationSimpsonsRuleToolStripMenuItem
        '
        Me.IntegrationSimpsonsRuleToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.Integral
        Me.IntegrationSimpsonsRuleToolStripMenuItem.Name = "IntegrationSimpsonsRuleToolStripMenuItem"
        Me.IntegrationSimpsonsRuleToolStripMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.IntegrationSimpsonsRuleToolStripMenuItem.Text = "Integration [Simpson's Rule]"
        Me.IntegrationSimpsonsRuleToolStripMenuItem.ToolTipText = "Ctrl + Click : Add Additional Plot"
        '
        'DifferentialForwardDifferenceToolStripMenuItem
        '
        Me.DifferentialForwardDifferenceToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.Menu_Partial_Differential
        Me.DifferentialForwardDifferenceToolStripMenuItem.Name = "DifferentialForwardDifferenceToolStripMenuItem"
        Me.DifferentialForwardDifferenceToolStripMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.DifferentialForwardDifferenceToolStripMenuItem.Text = "Differential [Backward Difference]"
        Me.DifferentialForwardDifferenceToolStripMenuItem.ToolTipText = "Ctrl + Click : Add Additional Plot"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(252, 6)
        '
        'Find3msToolStripMenuItem
        '
        Me.Find3msToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.IntervalTxt})
        Me.Find3msToolStripMenuItem.Name = "Find3msToolStripMenuItem"
        Me.Find3msToolStripMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.Find3msToolStripMenuItem.Text = "Find Inteval Min/Max"
        '
        'IntervalTxt
        '
        Me.IntervalTxt.BackColor = System.Drawing.SystemColors.Info
        Me.IntervalTxt.Name = "IntervalTxt"
        Me.IntervalTxt.Size = New System.Drawing.Size(100, 23)
        Me.IntervalTxt.ToolTipText = "Insert Inteval"
        '
        'FindDropRiseToolStripMenuItem
        '
        Me.FindDropRiseToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.IntervalTxt2})
        Me.FindDropRiseToolStripMenuItem.Name = "FindDropRiseToolStripMenuItem"
        Me.FindDropRiseToolStripMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.FindDropRiseToolStripMenuItem.Text = "Find Drop/Rise"
        '
        'IntervalTxt2
        '
        Me.IntervalTxt2.BackColor = System.Drawing.SystemColors.Info
        Me.IntervalTxt2.Name = "IntervalTxt2"
        Me.IntervalTxt2.Size = New System.Drawing.Size(100, 23)
        Me.IntervalTxt2.ToolTipText = "Insert Time Window"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(252, 6)
        '
        'FilterCFCToolStripMenuItem
        '
        Me.FilterCFCToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CFC60ToolStripMenuItem, Me.CFC180ToolStripMenuItem, Me.CFC600ToolStripMenuItem, Me.CFC1000ToolStripMenuItem})
        Me.FilterCFCToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.Menu_Filtering
        Me.FilterCFCToolStripMenuItem.Name = "FilterCFCToolStripMenuItem"
        Me.FilterCFCToolStripMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.FilterCFCToolStripMenuItem.Text = "Filter [CFC-SAE/J111]"
        '
        'CFC60ToolStripMenuItem
        '
        Me.CFC60ToolStripMenuItem.Name = "CFC60ToolStripMenuItem"
        Me.CFC60ToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.CFC60ToolStripMenuItem.Text = "CFC60 [Fwd-Bwd]"
        '
        'CFC180ToolStripMenuItem
        '
        Me.CFC180ToolStripMenuItem.Name = "CFC180ToolStripMenuItem"
        Me.CFC180ToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.CFC180ToolStripMenuItem.Text = "CFC180 [Fwd-Bwd]"
        '
        'CFC600ToolStripMenuItem
        '
        Me.CFC600ToolStripMenuItem.Name = "CFC600ToolStripMenuItem"
        Me.CFC600ToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.CFC600ToolStripMenuItem.Text = "CFC600 [Fwd-Bwd]"
        '
        'CFC1000ToolStripMenuItem
        '
        Me.CFC1000ToolStripMenuItem.Name = "CFC1000ToolStripMenuItem"
        Me.CFC1000ToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.CFC1000ToolStripMenuItem.Text = "CFC1000 [Fwd-Bwd]"
        '
        'TEMPToolStripMenuItem
        '
        Me.TEMPToolStripMenuItem.Image = Global.WindowsApplication1.My.Resources.Resources.CurveCorrel
        Me.TEMPToolStripMenuItem.Name = "TEMPToolStripMenuItem"
        Me.TEMPToolStripMenuItem.Size = New System.Drawing.Size(257, 22)
        Me.TEMPToolStripMenuItem.Text = "Curve Correlation Score"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLbl})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 368)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(776, 22)
        Me.StatusStrip1.TabIndex = 3
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'StatusLbl
        '
        Me.StatusLbl.Font = New System.Drawing.Font("맑은 고딕", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusLbl.Name = "StatusLbl"
        Me.StatusLbl.Size = New System.Drawing.Size(761, 17)
        Me.StatusLbl.Spring = True
        Me.StatusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.InjuryChart)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.DataBox)
        Me.SplitContainer1.Size = New System.Drawing.Size(776, 368)
        Me.SplitContainer1.SplitterDistance = 539
        Me.SplitContainer1.TabIndex = 4
        '
        'InjuryChart
        '
        ChartArea1.Name = "ChartArea1"
        Me.InjuryChart.ChartAreas.Add(ChartArea1)
        Me.InjuryChart.Cursor = System.Windows.Forms.Cursors.Cross
        Me.InjuryChart.Dock = System.Windows.Forms.DockStyle.Fill
        Legend1.Name = "Legend1"
        Me.InjuryChart.Legends.Add(Legend1)
        Me.InjuryChart.Location = New System.Drawing.Point(0, 0)
        Me.InjuryChart.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.InjuryChart.Name = "InjuryChart"
        Series1.ChartArea = "ChartArea1"
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Me.InjuryChart.Series.Add(Series1)
        Me.InjuryChart.Size = New System.Drawing.Size(535, 364)
        Me.InjuryChart.TabIndex = 3
        Me.InjuryChart.Text = "Chart1"
        '
        'DataBox
        '
        Me.DataBox.Controls.Add(Me.ChkSize)
        Me.DataBox.Controls.Add(Me.LblXY)
        Me.DataBox.Controls.Add(Me.MathGroup)
        Me.DataBox.Controls.Add(Me.ChkMath)
        Me.DataBox.Controls.Add(Me.LblMax)
        Me.DataBox.Controls.Add(Me.LblMin)
        Me.DataBox.Controls.Add(Me.ChkLst)
        Me.DataBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataBox.Location = New System.Drawing.Point(0, 0)
        Me.DataBox.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.DataBox.Name = "DataBox"
        Me.DataBox.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.DataBox.Size = New System.Drawing.Size(229, 364)
        Me.DataBox.TabIndex = 2
        Me.DataBox.TabStop = False
        Me.DataBox.Text = "GroupBox1"
        '
        'ChkSize
        '
        Me.ChkSize.AutoSize = True
        Me.ChkSize.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.ChkSize.ForeColor = System.Drawing.Color.Silver
        Me.ChkSize.Location = New System.Drawing.Point(15, 340)
        Me.ChkSize.Name = "ChkSize"
        Me.ChkSize.Size = New System.Drawing.Size(165, 19)
        Me.ChkSize.TabIndex = 7
        Me.ChkSize.Text = "Resize Window for BMP"
        Me.ChkSize.UseVisualStyleBackColor = True
        '
        'LblXY
        '
        Me.LblXY.AutoSize = True
        Me.LblXY.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblXY.Location = New System.Drawing.Point(31, 91)
        Me.LblXY.Name = "LblXY"
        Me.LblXY.Size = New System.Drawing.Size(57, 15)
        Me.LblXY.TabIndex = 6
        Me.LblXY.Text = "X-Y Value"
        '
        'MathGroup
        '
        Me.MathGroup.Controls.Add(Me.LblOffset)
        Me.MathGroup.Controls.Add(Me.Yoffset)
        Me.MathGroup.Controls.Add(Me.Xoffset)
        Me.MathGroup.Controls.Add(Me.LblScale)
        Me.MathGroup.Controls.Add(Me.Yscale)
        Me.MathGroup.Controls.Add(Me.Xscale)
        Me.MathGroup.Enabled = False
        Me.MathGroup.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MathGroup.Location = New System.Drawing.Point(17, 228)
        Me.MathGroup.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MathGroup.Name = "MathGroup"
        Me.MathGroup.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MathGroup.Size = New System.Drawing.Size(230, 101)
        Me.MathGroup.TabIndex = 5
        Me.MathGroup.TabStop = False
        Me.MathGroup.Text = "Select"
        '
        'LblOffset
        '
        Me.LblOffset.AutoSize = True
        Me.LblOffset.Location = New System.Drawing.Point(142, 78)
        Me.LblOffset.Name = "LblOffset"
        Me.LblOffset.Size = New System.Drawing.Size(64, 15)
        Me.LblOffset.TabIndex = 10
        Me.LblOffset.Text = "X/Y Offset"
        '
        'Yoffset
        '
        Me.Yoffset.Location = New System.Drawing.Point(75, 72)
        Me.Yoffset.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Yoffset.Name = "Yoffset"
        Me.Yoffset.Size = New System.Drawing.Size(49, 23)
        Me.Yoffset.TabIndex = 9
        Me.Yoffset.Text = "0"
        Me.Yoffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Xoffset
        '
        Me.Xoffset.Location = New System.Drawing.Point(21, 71)
        Me.Xoffset.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Xoffset.Name = "Xoffset"
        Me.Xoffset.Size = New System.Drawing.Size(48, 23)
        Me.Xoffset.TabIndex = 8
        Me.Xoffset.Text = "0"
        Me.Xoffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LblScale
        '
        Me.LblScale.AutoSize = True
        Me.LblScale.Location = New System.Drawing.Point(134, 38)
        Me.LblScale.Name = "LblScale"
        Me.LblScale.Size = New System.Drawing.Size(57, 15)
        Me.LblScale.TabIndex = 7
        Me.LblScale.Text = "X/Y Scale"
        '
        'Yscale
        '
        Me.Yscale.Location = New System.Drawing.Point(69, 30)
        Me.Yscale.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Yscale.Name = "Yscale"
        Me.Yscale.Size = New System.Drawing.Size(46, 23)
        Me.Yscale.TabIndex = 6
        Me.Yscale.Text = "1"
        Me.Yscale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Xscale
        '
        Me.Xscale.Location = New System.Drawing.Point(14, 30)
        Me.Xscale.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Xscale.Name = "Xscale"
        Me.Xscale.Size = New System.Drawing.Size(40, 23)
        Me.Xscale.TabIndex = 5
        Me.Xscale.Text = "1"
        Me.Xscale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ChkMath
        '
        Me.ChkMath.AutoSize = True
        Me.ChkMath.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkMath.Location = New System.Drawing.Point(22, 196)
        Me.ChkMath.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.ChkMath.Name = "ChkMath"
        Me.ChkMath.Size = New System.Drawing.Size(169, 19)
        Me.ChkMath.TabIndex = 4
        Me.ChkMath.Text = "Modify DATA / Reset DATA"
        Me.ChkMath.UseVisualStyleBackColor = True
        '
        'LblMax
        '
        Me.LblMax.AutoSize = True
        Me.LblMax.Font = New System.Drawing.Font("굴림", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.LblMax.Location = New System.Drawing.Point(20, 165)
        Me.LblMax.Name = "LblMax"
        Me.LblMax.Size = New System.Drawing.Size(137, 12)
        Me.LblMax.TabIndex = 2
        Me.LblMax.Text = "■ Maximum Value :"
        '
        'LblMin
        '
        Me.LblMin.AutoSize = True
        Me.LblMin.Font = New System.Drawing.Font("굴림", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMin.Location = New System.Drawing.Point(20, 124)
        Me.LblMin.Name = "LblMin"
        Me.LblMin.Size = New System.Drawing.Size(133, 12)
        Me.LblMin.TabIndex = 1
        Me.LblMin.Text = "■ Minimum Value :"
        '
        'ChkLst
        '
        Me.ChkLst.FormattingEnabled = True
        Me.ChkLst.Location = New System.Drawing.Point(17, 38)
        Me.ChkLst.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.ChkLst.Name = "ChkLst"
        Me.ChkLst.Size = New System.Drawing.Size(166, 40)
        Me.ChkLst.TabIndex = 0
        '
        'ListBoxMenu
        '
        Me.ListBoxMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RenameItemToolStripMenuItem, Me.MoveItemToolStripMenuItem, Me.MoveItemAxisYToolStripMenuItem})
        Me.ListBoxMenu.Name = "ListBoxMenu"
        Me.ListBoxMenu.Size = New System.Drawing.Size(197, 70)
        '
        'RenameItemToolStripMenuItem
        '
        Me.RenameItemToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RenameList})
        Me.RenameItemToolStripMenuItem.Name = "RenameItemToolStripMenuItem"
        Me.RenameItemToolStripMenuItem.Size = New System.Drawing.Size(196, 22)
        Me.RenameItemToolStripMenuItem.Text = "Rename Item"
        '
        'RenameList
        '
        Me.RenameList.Name = "RenameList"
        Me.RenameList.Size = New System.Drawing.Size(100, 23)
        '
        'MoveItemToolStripMenuItem
        '
        Me.MoveItemToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CmbChartArea, Me.To1stChartToolStripMenuItem})
        Me.MoveItemToolStripMenuItem.Name = "MoveItemToolStripMenuItem"
        Me.MoveItemToolStripMenuItem.Size = New System.Drawing.Size(196, 22)
        Me.MoveItemToolStripMenuItem.Text = "Move Item [Chart]"
        '
        'CmbChartArea
        '
        Me.CmbChartArea.Name = "CmbChartArea"
        Me.CmbChartArea.Size = New System.Drawing.Size(121, 23)
        '
        'To1stChartToolStripMenuItem
        '
        Me.To1stChartToolStripMenuItem.Name = "To1stChartToolStripMenuItem"
        Me.To1stChartToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.To1stChartToolStripMenuItem.Text = "To 1st Chart"
        '
        'MoveItemAxisYToolStripMenuItem
        '
        Me.MoveItemAxisYToolStripMenuItem.Name = "MoveItemAxisYToolStripMenuItem"
        Me.MoveItemAxisYToolStripMenuItem.Size = New System.Drawing.Size(196, 22)
        Me.MoveItemAxisYToolStripMenuItem.Text = "Move Item [Axis Y] {Y}"
        '
        'InjuryPopUp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(776, 390)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "InjuryPopUp"
        Me.Text = "Injury"
        Me.ChartContextMenu.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.InjuryChart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DataBox.ResumeLayout(False)
        Me.DataBox.PerformLayout()
        Me.MathGroup.ResumeLayout(False)
        Me.MathGroup.PerformLayout()
        Me.ListBoxMenu.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ChartContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ChartOptionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportXYDataToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveXYDlg As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ExportToExcelToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolTipCh As System.Windows.Forms.ToolTip
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents StatusLbl As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents MathToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents IntegrationSimpsonsRuleToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DifferentialForwardDifferenceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents InjuryChart As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents DataBox As System.Windows.Forms.GroupBox
    Friend WithEvents LblXY As System.Windows.Forms.Label
    Friend WithEvents MathGroup As System.Windows.Forms.GroupBox
    Friend WithEvents LblOffset As System.Windows.Forms.Label
    Friend WithEvents Yoffset As System.Windows.Forms.TextBox
    Friend WithEvents Xoffset As System.Windows.Forms.TextBox
    Friend WithEvents LblScale As System.Windows.Forms.Label
    Friend WithEvents Yscale As System.Windows.Forms.TextBox
    Friend WithEvents Xscale As System.Windows.Forms.TextBox
    Friend WithEvents ChkMath As System.Windows.Forms.CheckBox
    Friend WithEvents LblMax As System.Windows.Forms.Label
    Friend WithEvents LblMin As System.Windows.Forms.Label
    Friend WithEvents ChkLst As System.Windows.Forms.CheckedListBox
    Friend WithEvents ListBoxMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents RenameItemToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RenameList As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents RenameChartTitleToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChartTitleTxt As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents XaxitTitleTxt As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents YaxisTitleTxt As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents Yaxis2TitleTxt As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents CopyToClipBoardDataToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents XToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents YToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents XYToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents XYPairAllSeriesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Find3msToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents IntervalTxt As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents TEMPToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FilterCFCToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CFC60ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CFC180ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CFC600ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CFC1000ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents FindDropRiseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents IntervalTxt2 As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ChkSize As System.Windows.Forms.CheckBox
    Friend WithEvents AddAdditionalChartToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ClearAdditionalChartToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MoveItemToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CmbChartArea As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents MoveItemAxisYToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents To1stChartToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CmbChrtAreaTitle As System.Windows.Forms.ToolStripComboBox
End Class
