<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmChartOption
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
        Me.BoxChrtDetails = New System.Windows.Forms.GroupBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.CrossingTxt = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Yfont = New System.Windows.Forms.TextBox()
        Me.Xfont = New System.Windows.Forms.TextBox()
        Me.Yinterval = New System.Windows.Forms.TextBox()
        Me.Xinterval = New System.Windows.Forms.TextBox()
        Me.Ymax = New System.Windows.Forms.TextBox()
        Me.Ymin = New System.Windows.Forms.TextBox()
        Me.Xmax = New System.Windows.Forms.TextBox()
        Me.Xmin = New System.Windows.Forms.TextBox()
        Me.YAxisScale = New System.Windows.Forms.Label()
        Me.XaisScale = New System.Windows.Forms.Label()
        Me.SeriesColor = New System.Windows.Forms.ColorDialog()
        Me.BoxSeries = New System.Windows.Forms.GroupBox()
        Me.BtnColor = New System.Windows.Forms.Button()
        Me.TxtLineWidth = New System.Windows.Forms.TextBox()
        Me.LblSeriesWidth = New System.Windows.Forms.Label()
        Me.LblSeriesStyle = New System.Windows.Forms.Label()
        Me.CmbLineStyle = New System.Windows.Forms.ComboBox()
        Me.LblSeries = New System.Windows.Forms.Label()
        Me.CmbBoxSeries = New System.Windows.Forms.ComboBox()
        Me.BtnApply = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.legendVisibleCmb = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.legendCmb = New System.Windows.Forms.ComboBox()
        Me.SecondaryBox = New System.Windows.Forms.GroupBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.YfontSecond = New System.Windows.Forms.TextBox()
        Me.YintervalSecond = New System.Windows.Forms.TextBox()
        Me.YmaxSecond = New System.Windows.Forms.TextBox()
        Me.YminSecond = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.CmbArea = New System.Windows.Forms.ComboBox()
        Me.BoxChrtDetails.SuspendLayout()
        Me.BoxSeries.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SecondaryBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'BoxChrtDetails
        '
        Me.BoxChrtDetails.Controls.Add(Me.Label12)
        Me.BoxChrtDetails.Controls.Add(Me.CrossingTxt)
        Me.BoxChrtDetails.Controls.Add(Me.Label4)
        Me.BoxChrtDetails.Controls.Add(Me.Label3)
        Me.BoxChrtDetails.Controls.Add(Me.Label2)
        Me.BoxChrtDetails.Controls.Add(Me.Label1)
        Me.BoxChrtDetails.Controls.Add(Me.Yfont)
        Me.BoxChrtDetails.Controls.Add(Me.Xfont)
        Me.BoxChrtDetails.Controls.Add(Me.Yinterval)
        Me.BoxChrtDetails.Controls.Add(Me.Xinterval)
        Me.BoxChrtDetails.Controls.Add(Me.Ymax)
        Me.BoxChrtDetails.Controls.Add(Me.Ymin)
        Me.BoxChrtDetails.Controls.Add(Me.Xmax)
        Me.BoxChrtDetails.Controls.Add(Me.Xmin)
        Me.BoxChrtDetails.Controls.Add(Me.YAxisScale)
        Me.BoxChrtDetails.Controls.Add(Me.XaisScale)
        Me.BoxChrtDetails.Font = New System.Drawing.Font("Calibri", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BoxChrtDetails.Location = New System.Drawing.Point(10, 41)
        Me.BoxChrtDetails.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.BoxChrtDetails.Name = "BoxChrtDetails"
        Me.BoxChrtDetails.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.BoxChrtDetails.Size = New System.Drawing.Size(423, 116)
        Me.BoxChrtDetails.TabIndex = 1
        Me.BoxChrtDetails.TabStop = False
        Me.BoxChrtDetails.Text = "※ Graph Details [Main Axis]"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(10, 87)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(68, 18)
        Me.Label12.TabIndex = 13
        Me.Label12.Text = "Cross Pt. :"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CrossingTxt
        '
        Me.CrossingTxt.Location = New System.Drawing.Point(80, 85)
        Me.CrossingTxt.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.CrossingTxt.Name = "CrossingTxt"
        Me.CrossingTxt.Size = New System.Drawing.Size(79, 25)
        Me.CrossingTxt.TabIndex = 8
        Me.CrossingTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(337, 15)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(64, 18)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Font Size"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(256, 15)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 18)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Interval"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(185, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 18)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Max"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(102, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 18)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Min"
        '
        'Yfont
        '
        Me.Yfont.Location = New System.Drawing.Point(332, 60)
        Me.Yfont.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Yfont.Name = "Yfont"
        Me.Yfont.Size = New System.Drawing.Size(79, 25)
        Me.Yfont.TabIndex = 7
        Me.Yfont.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Xfont
        '
        Me.Xfont.Location = New System.Drawing.Point(332, 35)
        Me.Xfont.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Xfont.Name = "Xfont"
        Me.Xfont.Size = New System.Drawing.Size(79, 25)
        Me.Xfont.TabIndex = 3
        Me.Xfont.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Yinterval
        '
        Me.Yinterval.Location = New System.Drawing.Point(248, 60)
        Me.Yinterval.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Yinterval.Name = "Yinterval"
        Me.Yinterval.Size = New System.Drawing.Size(79, 25)
        Me.Yinterval.TabIndex = 6
        Me.Yinterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Xinterval
        '
        Me.Xinterval.Location = New System.Drawing.Point(248, 35)
        Me.Xinterval.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Xinterval.Name = "Xinterval"
        Me.Xinterval.Size = New System.Drawing.Size(79, 25)
        Me.Xinterval.TabIndex = 2
        Me.Xinterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Ymax
        '
        Me.Ymax.Location = New System.Drawing.Point(164, 60)
        Me.Ymax.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Ymax.Name = "Ymax"
        Me.Ymax.Size = New System.Drawing.Size(79, 25)
        Me.Ymax.TabIndex = 5
        Me.Ymax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Ymin
        '
        Me.Ymin.Location = New System.Drawing.Point(80, 60)
        Me.Ymin.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Ymin.Name = "Ymin"
        Me.Ymin.Size = New System.Drawing.Size(79, 25)
        Me.Ymin.TabIndex = 4
        Me.Ymin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Xmax
        '
        Me.Xmax.Location = New System.Drawing.Point(164, 35)
        Me.Xmax.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Xmax.Name = "Xmax"
        Me.Xmax.Size = New System.Drawing.Size(79, 25)
        Me.Xmax.TabIndex = 1
        Me.Xmax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Xmin
        '
        Me.Xmin.Location = New System.Drawing.Point(80, 35)
        Me.Xmin.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Xmin.Name = "Xmin"
        Me.Xmin.Size = New System.Drawing.Size(79, 25)
        Me.Xmin.TabIndex = 0
        Me.Xmin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'YAxisScale
        '
        Me.YAxisScale.AutoSize = True
        Me.YAxisScale.Location = New System.Drawing.Point(18, 62)
        Me.YAxisScale.Name = "YAxisScale"
        Me.YAxisScale.Size = New System.Drawing.Size(50, 18)
        Me.YAxisScale.TabIndex = 1
        Me.YAxisScale.Text = "Y axis :"
        Me.YAxisScale.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'XaisScale
        '
        Me.XaisScale.AutoSize = True
        Me.XaisScale.Location = New System.Drawing.Point(18, 38)
        Me.XaisScale.Name = "XaisScale"
        Me.XaisScale.Size = New System.Drawing.Size(50, 18)
        Me.XaisScale.TabIndex = 0
        Me.XaisScale.Text = "X axis :"
        Me.XaisScale.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BoxSeries
        '
        Me.BoxSeries.Controls.Add(Me.BtnColor)
        Me.BoxSeries.Controls.Add(Me.TxtLineWidth)
        Me.BoxSeries.Controls.Add(Me.LblSeriesWidth)
        Me.BoxSeries.Controls.Add(Me.LblSeriesStyle)
        Me.BoxSeries.Controls.Add(Me.CmbLineStyle)
        Me.BoxSeries.Controls.Add(Me.LblSeries)
        Me.BoxSeries.Controls.Add(Me.CmbBoxSeries)
        Me.BoxSeries.Font = New System.Drawing.Font("Calibri", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BoxSeries.Location = New System.Drawing.Point(10, 231)
        Me.BoxSeries.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.BoxSeries.Name = "BoxSeries"
        Me.BoxSeries.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.BoxSeries.Size = New System.Drawing.Size(423, 103)
        Me.BoxSeries.TabIndex = 3
        Me.BoxSeries.TabStop = False
        Me.BoxSeries.Text = "※ Series Details"
        '
        'BtnColor
        '
        Me.BtnColor.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnColor.Location = New System.Drawing.Point(113, 73)
        Me.BtnColor.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.BtnColor.Name = "BtnColor"
        Me.BtnColor.Size = New System.Drawing.Size(119, 21)
        Me.BtnColor.TabIndex = 2
        Me.BtnColor.UseVisualStyleBackColor = True
        '
        'TxtLineWidth
        '
        Me.TxtLineWidth.Location = New System.Drawing.Point(328, 43)
        Me.TxtLineWidth.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TxtLineWidth.Name = "TxtLineWidth"
        Me.TxtLineWidth.Size = New System.Drawing.Size(81, 25)
        Me.TxtLineWidth.TabIndex = 3
        '
        'LblSeriesWidth
        '
        Me.LblSeriesWidth.AutoSize = True
        Me.LblSeriesWidth.Location = New System.Drawing.Point(238, 46)
        Me.LblSeriesWidth.Name = "LblSeriesWidth"
        Me.LblSeriesWidth.Size = New System.Drawing.Size(83, 18)
        Me.LblSeriesWidth.TabIndex = 5
        Me.LblSeriesWidth.Text = "Line Width :"
        '
        'LblSeriesStyle
        '
        Me.LblSeriesStyle.AutoSize = True
        Me.LblSeriesStyle.Location = New System.Drawing.Point(16, 47)
        Me.LblSeriesStyle.Name = "LblSeriesStyle"
        Me.LblSeriesStyle.Size = New System.Drawing.Size(75, 18)
        Me.LblSeriesStyle.TabIndex = 4
        Me.LblSeriesStyle.Text = "Line Style :"
        '
        'CmbLineStyle
        '
        Me.CmbLineStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbLineStyle.FormattingEnabled = True
        Me.CmbLineStyle.Location = New System.Drawing.Point(113, 45)
        Me.CmbLineStyle.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.CmbLineStyle.Name = "CmbLineStyle"
        Me.CmbLineStyle.Size = New System.Drawing.Size(120, 25)
        Me.CmbLineStyle.TabIndex = 1
        '
        'LblSeries
        '
        Me.LblSeries.AutoSize = True
        Me.LblSeries.Location = New System.Drawing.Point(16, 19)
        Me.LblSeries.Name = "LblSeries"
        Me.LblSeries.Size = New System.Drawing.Size(84, 18)
        Me.LblSeries.TabIndex = 1
        Me.LblSeries.Text = "Data Series :"
        '
        'CmbBoxSeries
        '
        Me.CmbBoxSeries.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbBoxSeries.DropDownWidth = 240
        Me.CmbBoxSeries.FormattingEnabled = True
        Me.CmbBoxSeries.Location = New System.Drawing.Point(113, 16)
        Me.CmbBoxSeries.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.CmbBoxSeries.Name = "CmbBoxSeries"
        Me.CmbBoxSeries.Size = New System.Drawing.Size(120, 25)
        Me.CmbBoxSeries.TabIndex = 13
        '
        'BtnApply
        '
        Me.BtnApply.Font = New System.Drawing.Font("Calibri", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnApply.Location = New System.Drawing.Point(329, 395)
        Me.BtnApply.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.BtnApply.Name = "BtnApply"
        Me.BtnApply.Size = New System.Drawing.Size(104, 36)
        Me.BtnApply.TabIndex = 5
        Me.BtnApply.Text = "Apply"
        Me.BtnApply.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.legendVisibleCmb)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.legendCmb)
        Me.GroupBox1.Font = New System.Drawing.Font("Calibri", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(10, 336)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.GroupBox1.Size = New System.Drawing.Size(423, 52)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "※ Legends"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(238, 25)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(58, 18)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "Visible :"
        '
        'legendVisibleCmb
        '
        Me.legendVisibleCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.legendVisibleCmb.FormattingEnabled = True
        Me.legendVisibleCmb.Items.AddRange(New Object() {"Show", "Hide"})
        Me.legendVisibleCmb.Location = New System.Drawing.Point(306, 22)
        Me.legendVisibleCmb.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.legendVisibleCmb.Name = "legendVisibleCmb"
        Me.legendVisibleCmb.Size = New System.Drawing.Size(103, 25)
        Me.legendVisibleCmb.TabIndex = 2
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(16, 25)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(114, 18)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Legend Position :"
        '
        'legendCmb
        '
        Me.legendCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.legendCmb.FormattingEnabled = True
        Me.legendCmb.Items.AddRange(New Object() {"Top", "Right", "Bottom", "Left", "Inside Chart", "Outside Chart"})
        Me.legendCmb.Location = New System.Drawing.Point(145, 22)
        Me.legendCmb.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.legendCmb.Name = "legendCmb"
        Me.legendCmb.Size = New System.Drawing.Size(88, 25)
        Me.legendCmb.TabIndex = 0
        '
        'SecondaryBox
        '
        Me.SecondaryBox.Controls.Add(Me.Label7)
        Me.SecondaryBox.Controls.Add(Me.Label8)
        Me.SecondaryBox.Controls.Add(Me.Label9)
        Me.SecondaryBox.Controls.Add(Me.Label10)
        Me.SecondaryBox.Controls.Add(Me.YfontSecond)
        Me.SecondaryBox.Controls.Add(Me.YintervalSecond)
        Me.SecondaryBox.Controls.Add(Me.YmaxSecond)
        Me.SecondaryBox.Controls.Add(Me.YminSecond)
        Me.SecondaryBox.Controls.Add(Me.Label11)
        Me.SecondaryBox.Font = New System.Drawing.Font("Calibri", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SecondaryBox.Location = New System.Drawing.Point(10, 161)
        Me.SecondaryBox.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.SecondaryBox.Name = "SecondaryBox"
        Me.SecondaryBox.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.SecondaryBox.Size = New System.Drawing.Size(423, 68)
        Me.SecondaryBox.TabIndex = 2
        Me.SecondaryBox.TabStop = False
        Me.SecondaryBox.Text = "※ Graph Details [Secondary Axis]"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(337, 15)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(64, 18)
        Me.Label7.TabIndex = 11
        Me.Label7.Text = "Font Size"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(256, 15)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(56, 18)
        Me.Label8.TabIndex = 10
        Me.Label8.Text = "Interval"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(185, 15)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(35, 18)
        Me.Label9.TabIndex = 9
        Me.Label9.Text = "Max"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(102, 15)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(33, 18)
        Me.Label10.TabIndex = 8
        Me.Label10.Text = "Min"
        '
        'YfontSecond
        '
        Me.YfontSecond.Location = New System.Drawing.Point(332, 35)
        Me.YfontSecond.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.YfontSecond.Name = "YfontSecond"
        Me.YfontSecond.Size = New System.Drawing.Size(79, 25)
        Me.YfontSecond.TabIndex = 12
        Me.YfontSecond.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'YintervalSecond
        '
        Me.YintervalSecond.Location = New System.Drawing.Point(248, 35)
        Me.YintervalSecond.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.YintervalSecond.Name = "YintervalSecond"
        Me.YintervalSecond.Size = New System.Drawing.Size(79, 25)
        Me.YintervalSecond.TabIndex = 11
        Me.YintervalSecond.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'YmaxSecond
        '
        Me.YmaxSecond.Location = New System.Drawing.Point(164, 35)
        Me.YmaxSecond.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.YmaxSecond.Name = "YmaxSecond"
        Me.YmaxSecond.Size = New System.Drawing.Size(79, 25)
        Me.YmaxSecond.TabIndex = 10
        Me.YmaxSecond.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'YminSecond
        '
        Me.YminSecond.Location = New System.Drawing.Point(80, 35)
        Me.YminSecond.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.YminSecond.Name = "YminSecond"
        Me.YminSecond.Size = New System.Drawing.Size(79, 25)
        Me.YminSecond.TabIndex = 9
        Me.YminSecond.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(18, 37)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(50, 18)
        Me.Label11.TabIndex = 1
        Me.Label11.Text = "Y axis :"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CmbArea
        '
        Me.CmbArea.Font = New System.Drawing.Font("맑은 고딕", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.CmbArea.FormattingEnabled = True
        Me.CmbArea.Location = New System.Drawing.Point(10, 12)
        Me.CmbArea.Name = "CmbArea"
        Me.CmbArea.Size = New System.Drawing.Size(423, 25)
        Me.CmbArea.TabIndex = 6
        Me.CmbArea.Text = "Chart"
        '
        'FrmChartOption
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(445, 444)
        Me.Controls.Add(Me.CmbArea)
        Me.Controls.Add(Me.SecondaryBox)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.BtnApply)
        Me.Controls.Add(Me.BoxSeries)
        Me.Controls.Add(Me.BoxChrtDetails)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmChartOption"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Chart Options"
        Me.TopMost = True
        Me.BoxChrtDetails.ResumeLayout(False)
        Me.BoxChrtDetails.PerformLayout()
        Me.BoxSeries.ResumeLayout(False)
        Me.BoxSeries.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.SecondaryBox.ResumeLayout(False)
        Me.SecondaryBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BoxChrtDetails As System.Windows.Forms.GroupBox
    Friend WithEvents SeriesColor As System.Windows.Forms.ColorDialog
    Friend WithEvents BoxSeries As System.Windows.Forms.GroupBox
    Friend WithEvents CmbBoxSeries As System.Windows.Forms.ComboBox
    Friend WithEvents BtnApply As System.Windows.Forms.Button
    Friend WithEvents Yfont As System.Windows.Forms.TextBox
    Friend WithEvents Xfont As System.Windows.Forms.TextBox
    Friend WithEvents Yinterval As System.Windows.Forms.TextBox
    Friend WithEvents Xinterval As System.Windows.Forms.TextBox
    Friend WithEvents Ymax As System.Windows.Forms.TextBox
    Friend WithEvents Ymin As System.Windows.Forms.TextBox
    Friend WithEvents Xmax As System.Windows.Forms.TextBox
    Friend WithEvents Xmin As System.Windows.Forms.TextBox
    Friend WithEvents YAxisScale As System.Windows.Forms.Label
    Friend WithEvents XaisScale As System.Windows.Forms.Label
    Friend WithEvents TxtLineWidth As System.Windows.Forms.TextBox
    Friend WithEvents LblSeriesWidth As System.Windows.Forms.Label
    Friend WithEvents LblSeriesStyle As System.Windows.Forms.Label
    Friend WithEvents CmbLineStyle As System.Windows.Forms.ComboBox
    Friend WithEvents LblSeries As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents legendCmb As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents legendVisibleCmb As System.Windows.Forms.ComboBox
    Friend WithEvents BtnColor As System.Windows.Forms.Button
    Friend WithEvents SecondaryBox As System.Windows.Forms.GroupBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents YfontSecond As System.Windows.Forms.TextBox
    Friend WithEvents YintervalSecond As System.Windows.Forms.TextBox
    Friend WithEvents YmaxSecond As System.Windows.Forms.TextBox
    Friend WithEvents YminSecond As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents CrossingTxt As System.Windows.Forms.TextBox
    Friend WithEvents CmbArea As System.Windows.Forms.ComboBox
End Class
