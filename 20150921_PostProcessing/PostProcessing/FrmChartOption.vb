Imports System.Windows.Forms.DataVisualization.Charting

Public Class FrmChartOption

    Dim Targetchart As New Chart

    Public Sub New(ByRef RefChart As Chart)

        ' 이 호출은 디자이너에 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하십시오.

        Targetchart = RefChart

        Dim i As Integer = 0

        With Me
            .CmbLineStyle.Items.Add("Dash")
            .CmbLineStyle.Items.Add("Dash-Dot")
            .CmbLineStyle.Items.Add("Dot")
            .CmbLineStyle.Items.Add("Line")
            .CmbLineStyle.Items.Add("Area[Bar]")

            .CmbArea.Items.Add("Main Chart")

            If Targetchart.ChartAreas.Count > 1 Then
                For i = 1 To Targetchart.ChartAreas.Count - 1
                    .CmbArea.Items.Add("# " & i & " Added Chart")
                Next
            End If

            .CmbArea.SelectedIndex = 0
        End With


        With Targetchart

            'Related Chart Area================================================================================
            If Targetchart.Name <> "PreviewDATA" Then
                Me.Xmin.Text = Format(.ChartAreas(Me.CmbArea.SelectedIndex).AxisX.Minimum, "0.0")
            Else
                Me.Xmin.Text = Format(.ChartAreas(Me.CmbArea.SelectedIndex).AxisX.Minimum, "0.0")
            End If

            Me.Ymin.Text = Format(.ChartAreas(Me.CmbArea.SelectedIndex).AxisY.Minimum, "0.0")
            Me.Xmax.Text = Format(.ChartAreas(Me.CmbArea.SelectedIndex).AxisX.Maximum, "0.0")
            Me.Ymax.Text = Format(.ChartAreas(Me.CmbArea.SelectedIndex).AxisY.Maximum, "0.0")

            If .ChartAreas(Me.CmbArea.SelectedIndex).AxisX.Interval = 0.0 Then
                Me.Xinterval.Text = "AUTO"
            Else
                Me.Xinterval.Text = Format(.ChartAreas(Me.CmbArea.SelectedIndex).AxisX.Interval, "0.0###")
            End If

            If .ChartAreas(Me.CmbArea.SelectedIndex).AxisY.Interval = 0.0 Then
                Me.Yinterval.Text = "AUTO"
            Else
                Me.Yinterval.Text = Format(.ChartAreas(Me.CmbArea.SelectedIndex).AxisY.Interval, "0.0###")
            End If

            Me.Xfont.Text = .ChartAreas(Me.CmbArea.SelectedIndex).AxisX.LabelStyle.Font.Size
            Me.Yfont.Text = .ChartAreas(Me.CmbArea.SelectedIndex).AxisY.LabelStyle.Font.Size
            Me.CrossingTxt.Text = CStr(.ChartAreas(Me.CmbArea.SelectedIndex).AxisY.Crossing)
            If UCase(Me.CrossingTxt.Text) = "NAN" Then
                Me.CrossingTxt.Text = .ChartAreas(Me.CmbArea.SelectedIndex).AxisY.Minimum
            End If


            If .ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.Enabled = AxisEnabled.True Or _
                .ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.Enabled = AxisEnabled.Auto Then
                Me.YminSecond.Text = Format(.ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.Minimum, "0.0")
                Me.YmaxSecond.Text = Format(.ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.Maximum, "0.0")
                If .ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.Interval = 0.0 Then
                    Me.YintervalSecond.Text = "AUTO"
                Else
                    Me.YintervalSecond.Text = Format(.ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.Interval, "0.0###")
                End If
                Me.YfontSecond.Text = .ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.LabelStyle.Font.Size
            Else
                Me.SecondaryBox.Enabled = False
            End If
            '==========================================================================================================

            For i = 1 To .Series.Count
                Me.CmbBoxSeries.Items.Add(.Series(i - 1).Name)
            Next
            Me.CmbBoxSeries.SelectedIndex = 0

            Select Case .Series(Me.CmbBoxSeries.SelectedIndex).BorderDashStyle
                Case 1
                    Me.CmbLineStyle.SelectedIndex = 0
                Case 2
                    Me.CmbLineStyle.SelectedIndex = 1
                Case 4
                    Me.CmbLineStyle.SelectedIndex = 2
                Case 5
                    Me.CmbLineStyle.SelectedIndex = 3

            End Select

            Select Case .Legends(0).Enabled
                Case True
                    Me.legendVisibleCmb.SelectedIndex = 0
                Case False
                    Me.legendVisibleCmb.SelectedIndex = 1
            End Select

            If .Legends(0).Enabled = True Then
                Select Case .Legends(0).Docking
                    Case 0
                        Me.legendCmb.SelectedIndex = 0
                    Case 1
                        Me.legendCmb.SelectedIndex = 1
                    Case 2
                        Me.legendCmb.SelectedIndex = 2
                    Case 3
                        Me.legendCmb.SelectedIndex = 3
                    Case Else

                End Select
            End If

            Me.TxtLineWidth.Text = .Series(Me.CmbBoxSeries.SelectedIndex).BorderWidth

            If .Series(Me.CmbBoxSeries.SelectedIndex).Color.Name = "0" Then
                Me.BtnColor.Text = "Auto Color"
            Else
                Me.BtnColor.Text = ""
                Me.BtnColor.BackColor = .Series(Me.CmbBoxSeries.SelectedIndex).Color
            End If

        End With

    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click

        '유효성 검사=====================================================================================
        With Me
            If IsNumeric(.Xmin.Text) = False Then
                MsgBox("Insert Numeric Value", MsgBoxStyle.Critical, "Error")
                Me.Xmin.Focus()
                Exit Sub
            ElseIf IsNumeric(.Xmax.Text) = False Then
                MsgBox("Insert Numeric Value", MsgBoxStyle.Critical, "Error")
                Me.Xmax.Focus()
                Exit Sub
            ElseIf IsNumeric(.Ymin.Text) = False And (.Ymin.Text <> "NaN") Then
                MsgBox("Insert Numeric Value", MsgBoxStyle.Critical, "Error")
                Me.Ymin.Focus()
                Exit Sub
            ElseIf IsNumeric(.Ymax.Text) = False And (.Ymax.Text <> "NaN") Then
                MsgBox("Insert Numeric Value", MsgBoxStyle.Critical, "Error")
                Me.Ymax.Focus()
                Exit Sub
            ElseIf IsNumeric(Me.CrossingTxt.Text) = False And (.CrossingTxt.Text <> "NaN") Then
                MsgBox("Insert Numeric Value", MsgBoxStyle.Critical, "Error")
                Me.CrossingTxt.Focus()
                Exit Sub
            End If

            If (.Ymax.Text <> "NaN") And (.Ymin.Text <> "NaN") Then

                If CDec(.Xmin.Text) >= CDec(.Xmax.Text) Then
                    MsgBox("Invalid Minimum or Maximum Value", MsgBoxStyle.Critical, "Error")
                    Me.Xmin.Focus()
                    Exit Sub
                ElseIf CDec(.Ymin.Text) >= CDec(.Ymax.Text) Then
                    MsgBox("Invalid Minimum or Maximum Value", MsgBoxStyle.Critical, "Error")
                    Me.Ymin.Focus()
                    Exit Sub
                End If

            End If

            If Targetchart.ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.Enabled = AxisEnabled.True Then
                If IsNumeric(.YminSecond.Text) = False Then
                    MsgBox("Insert Numeric Value", MsgBoxStyle.Critical, "Error")
                    Me.YminSecond.Focus()
                    Exit Sub
                ElseIf IsNumeric(.YmaxSecond.Text) = False Then
                    MsgBox("Insert Numeric Value", MsgBoxStyle.Critical, "Error")
                    Me.YmaxSecond.Focus()
                    Exit Sub
                ElseIf IsNumeric(.YfontSecond.Text) = False Then
                    MsgBox("Insert Numeric Value", MsgBoxStyle.Critical, "Error")
                    Me.YfontSecond.Focus()
                    Exit Sub
                End If
            End If

        End With
        '================================================================================================

        With Targetchart
            '최대 최소값을 적용
            If Targetchart.Name <> "PreviewDATA" Then
                .ChartAreas(Me.CmbArea.SelectedIndex).AxisX.Minimum = CDec(Me.Xmin.Text)
            Else
                .ChartAreas(Me.CmbArea.SelectedIndex).AxisX.Minimum = CDec(Me.Xmin.Text)
            End If
            .ChartAreas(Me.CmbArea.SelectedIndex).AxisX.Maximum = CDec(Me.Xmax.Text)
            If Me.Ymin.Text <> "NaN" Then
                .ChartAreas(Me.CmbArea.SelectedIndex).AxisY.Minimum = CDec(Me.Ymin.Text)
                .ChartAreas(Me.CmbArea.SelectedIndex).AxisY.Maximum = CDec(Me.Ymax.Text)
            End If
            
            If IsNumeric(Me.Xinterval.Text) = True Then
                .ChartAreas(Me.CmbArea.SelectedIndex).AxisX.Interval = CDec(Me.Xinterval.Text)
            End If
            '자동간격(Y축)
            If Me.Yinterval.Text <> "AUTO" Then
                .ChartAreas(Me.CmbArea.SelectedIndex).AxisY.Interval = CDec(Me.Yinterval.Text)
            Else
                '아무것도 하지않음
                .ChartAreas(Me.CmbArea.SelectedIndex).AxisY.Interval = Double.NaN
            End If

            If UCase(Me.CrossingTxt.Text) <> "NAN" Then
                .ChartAreas(Me.CmbArea.SelectedIndex).AxisY.Crossing = CDbl(Me.CrossingTxt.Text)
            End If

            If CDec(Me.YminSecond.Text) <> CDec(Me.YmaxSecond.Text) Then
                If .ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.Enabled = AxisEnabled.True Or _
                .ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.Enabled = AxisEnabled.Auto Then
                    .ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.Minimum = CDec(Me.YminSecond.Text)
                    .ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.Maximum = CDec(Me.YmaxSecond.Text)
                    If Me.YintervalSecond.Text <> "AUTO" Then
                        .ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.Interval = CDec(Me.YintervalSecond.Text)
                    Else
                        '아무것도 하지않음
                        .ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.Interval = Double.NaN
                    End If
                    .ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.LabelStyle.Font = New Font(.ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.LabelStyle.Font.Name, CInt(Me.YfontSecond.Text), FontStyle.Bold)
                End If
            End If

            '각 축의 폰트 크기 적용
            .ChartAreas(Me.CmbArea.SelectedIndex).AxisX.LabelStyle.Font = New Font(.ChartAreas(Me.CmbArea.SelectedIndex).AxisX.LabelStyle.Font.Name, CInt(Me.Xfont.Text), FontStyle.Bold)
            .ChartAreas(Me.CmbArea.SelectedIndex).AxisY.LabelStyle.Font = New Font(.ChartAreas(Me.CmbArea.SelectedIndex).AxisY.LabelStyle.Font.Name, CInt(Me.Yfont.Text), FontStyle.Bold)

            '시리즈의 선 굵기 적용
            .Series(Me.CmbBoxSeries.SelectedIndex).BorderWidth = CInt(Me.TxtLineWidth.Text)

            '시리즈의 선 스타일 적용
            Select Case Me.CmbLineStyle.SelectedIndex
                Case 0
                    .Series(Me.CmbBoxSeries.SelectedIndex).ChartType = SeriesChartType.Line
                    .Series(Me.CmbBoxSeries.SelectedIndex).BorderDashStyle = ChartDashStyle.Dash
                Case 1
                    .Series(Me.CmbBoxSeries.SelectedIndex).ChartType = SeriesChartType.Line
                    .Series(Me.CmbBoxSeries.SelectedIndex).BorderDashStyle = ChartDashStyle.DashDot
                Case 2
                    .Series(Me.CmbBoxSeries.SelectedIndex).ChartType = SeriesChartType.Line
                    .Series(Me.CmbBoxSeries.SelectedIndex).BorderDashStyle = ChartDashStyle.Dot
                Case 3
                    .Series(Me.CmbBoxSeries.SelectedIndex).ChartType = SeriesChartType.Line
                    .Series(Me.CmbBoxSeries.SelectedIndex).BorderDashStyle = ChartDashStyle.Solid
                Case 4
                    .Series(Me.CmbBoxSeries.SelectedIndex).ChartType = SeriesChartType.Column
                End Select

            End With

    End Sub

    Private Sub Xmax_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Xmax.GotFocus
        Me.Xmax.SelectAll()
    End Sub
    Private Sub Xmin_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Xmin.GotFocus
        Me.Xmin.SelectAll()
    End Sub
    Private Sub Ymax_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Ymax.GotFocus
        Me.Ymax.SelectAll()
    End Sub
    Private Sub Ymin_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Xmin.GotFocus
        Me.Ymin.SelectAll()
    End Sub
    Private Sub Xinterval_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Xinterval.GotFocus
        Me.Xinterval.SelectAll()
    End Sub
    Private Sub Yinterval_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Yinterval.GotFocus
        Me.Yinterval.SelectAll()
    End Sub

    Private Sub CmbBoxSeries_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbBoxSeries.SelectedIndexChanged
        With Targetchart
            Select Case .Series(Me.CmbBoxSeries.SelectedIndex).BorderDashStyle
                Case 1
                    Me.CmbLineStyle.SelectedIndex = 0
                Case 2
                    Me.CmbLineStyle.SelectedIndex = 1
                Case 4
                    Me.CmbLineStyle.SelectedIndex = 2
                Case 5
                    Me.CmbLineStyle.SelectedIndex = 3
            End Select

            Me.TxtLineWidth.Text = .Series(Me.CmbBoxSeries.SelectedIndex).BorderWidth

            If .Series(Me.CmbBoxSeries.SelectedIndex).Color.Name = "0" Then
                Me.BtnColor.BackColor = Color.LightGray
                Me.BtnColor.Text = "Auto Color"
            Else
                Me.BtnColor.Text = ""
                Me.BtnColor.BackColor = .Series(Me.CmbBoxSeries.SelectedIndex).Color
            End If
        End With
    End Sub

    Private Sub Xmin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Xmin.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then Call BtnApply_Click(BtnApply, New KeyEventArgs(Keys.Return))
    End Sub
    Private Sub Xmax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Xmax.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then Call BtnApply_Click(BtnApply, New KeyEventArgs(Keys.Return))
    End Sub
    Private Sub Ymin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Ymin.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then Call BtnApply_Click(BtnApply, New KeyEventArgs(Keys.Return))
    End Sub
    Private Sub Ymax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Ymax.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then Call BtnApply_Click(BtnApply, New KeyEventArgs(Keys.Return))
    End Sub
    Private Sub Xinterval_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Xinterval.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then Call BtnApply_Click(BtnApply, New KeyEventArgs(Keys.Return))
    End Sub
    Private Sub Yinterval_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Yinterval.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then Call BtnApply_Click(BtnApply, New KeyEventArgs(Keys.Return))
    End Sub
    Private Sub TxtLineWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TxtLineWidth.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then Call BtnApply_Click(BtnApply, New KeyEventArgs(Keys.Return))
    End Sub
    Private Sub Xfont_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Xfont.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then Call BtnApply_Click(BtnApply, New KeyEventArgs(Keys.Return))
    End Sub
    Private Sub Yfont_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Yfont.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then Call BtnApply_Click(BtnApply, New KeyEventArgs(Keys.Return))
    End Sub

    Private Sub legendVisibleCmb_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles legendVisibleCmb.SelectedIndexChanged
        With Targetchart
            Select Case Me.legendVisibleCmb.SelectedIndex
                Case 0
                    .Legends(0).Enabled = True
                Case 1
                    .Legends(0).Enabled = False
            End Select
        End With
    End Sub

    Private Sub legendCmb_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles legendCmb.SelectedIndexChanged

        With Targetchart
            Select Case Me.legendCmb.SelectedIndex
                Case 0
                    .Legends(0).Enabled = True
                    .Legends(0).Docking = 0
                Case 1
                    .Legends(0).Enabled = True
                    .Legends(0).Docking = 1
                Case 2
                    .Legends(0).Enabled = True
                    .Legends(0).Docking = 2
                Case 3
                    .Legends(0).Enabled = True
                    .Legends(0).Docking = 3
                Case 4
                    .Legends(0).Enabled = True
                    .Legends(0).IsDockedInsideChartArea = True
                    .Legends(0).DockedToChartArea = .ChartAreas(0).Name
                Case 5
                    .Legends(0).IsDockedInsideChartArea = False
                    '.Legends(0).Enabled = False    '범례를 안보이게 한다
            End Select
        End With
    End Sub

    Private Sub BtnColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnColor.Click
        With Me
            If .SeriesColor.ShowDialog = Windows.Forms.DialogResult.OK Then
                Targetchart.Series(Me.CmbBoxSeries.SelectedIndex).Color = .SeriesColor.Color
                Me.BtnColor.BackColor = .SeriesColor.Color
                Me.BtnColor.Text = ""
            End If
        End With
    End Sub

    Private Sub YminSecond_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles YminSecond.GotFocus
        Me.YminSecond.SelectAll()
    End Sub

    Private Sub YmaxSecond_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles YmaxSecond.GotFocus
        Me.YmaxSecond.SelectAll()
    End Sub

    Private Sub YintervalSecond_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles YintervalSecond.GotFocus
        Me.YintervalSecond.SelectAll()
    End Sub

    Private Sub YfontSecond_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles YfontSecond.GotFocus
        Me.YfontSecond.SelectAll()
    End Sub

    Private Sub YminSecond_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles YminSecond.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then Call BtnApply_Click(BtnApply, New KeyEventArgs(Keys.Return))
    End Sub

    Private Sub YmaxSecond_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles YmaxSecond.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then Call BtnApply_Click(BtnApply, New KeyEventArgs(Keys.Return))
    End Sub

    Private Sub YintervalSecond_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles YintervalSecond.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then Call BtnApply_Click(BtnApply, New KeyEventArgs(Keys.Return))
    End Sub

    Private Sub YfontSecond_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles YfontSecond.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then Call BtnApply_Click(BtnApply, New KeyEventArgs(Keys.Return))
    End Sub

    Private Sub CrossingTxt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles CrossingTxt.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then Call BtnApply_Click(BtnApply, New KeyEventArgs(Keys.Return))
    End Sub

    Private Sub CmbLineStyle_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbLineStyle.SelectedIndexChanged
        With Targetchart
            '시리즈의 선 스타일 적용
            Select Case Me.CmbLineStyle.SelectedIndex
                Case 0
                    .Series(Me.CmbBoxSeries.SelectedIndex).ChartType = SeriesChartType.Line
                    .Series(Me.CmbBoxSeries.SelectedIndex).BorderDashStyle = ChartDashStyle.Dash
                Case 1
                    .Series(Me.CmbBoxSeries.SelectedIndex).ChartType = SeriesChartType.Line
                    .Series(Me.CmbBoxSeries.SelectedIndex).BorderDashStyle = ChartDashStyle.DashDot
                Case 2
                    .Series(Me.CmbBoxSeries.SelectedIndex).ChartType = SeriesChartType.Line
                    .Series(Me.CmbBoxSeries.SelectedIndex).BorderDashStyle = ChartDashStyle.Dot
                Case 3
                    .Series(Me.CmbBoxSeries.SelectedIndex).ChartType = SeriesChartType.Line
                    .Series(Me.CmbBoxSeries.SelectedIndex).BorderDashStyle = ChartDashStyle.Solid
                Case 4
                    .Series(Me.CmbBoxSeries.SelectedIndex).ChartType = SeriesChartType.Column
            End Select
        End With
    End Sub

    Private Sub CmbArea_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbArea.SelectedIndexChanged

        With Targetchart

            'Related Chart Area================================================================================
            If Targetchart.Name <> "PreviewDATA" Then
                Me.Xmin.Text = Format(.ChartAreas(Me.CmbArea.SelectedIndex).AxisX.Minimum, "0.0##")
            Else
                Me.Xmin.Text = Format(.ChartAreas(Me.CmbArea.SelectedIndex).AxisX.Minimum, "0.0##")
            End If

            Me.Ymin.Text = Format(.ChartAreas(Me.CmbArea.SelectedIndex).AxisY.Minimum, "0.0##")
            Me.Xmax.Text = Format(.ChartAreas(Me.CmbArea.SelectedIndex).AxisX.Maximum, "0.0##")
            Me.Ymax.Text = Format(.ChartAreas(Me.CmbArea.SelectedIndex).AxisY.Maximum, "0.0##")

            If .ChartAreas(Me.CmbArea.SelectedIndex).AxisX.Interval = 0.0 Then
                Me.Xinterval.Text = "AUTO"
            Else
                Me.Xinterval.Text = Format(.ChartAreas(Me.CmbArea.SelectedIndex).AxisX.Interval, "0.0###")
            End If

            If .ChartAreas(Me.CmbArea.SelectedIndex).AxisY.Interval = 0.0 Then
                Me.Yinterval.Text = "AUTO"
            Else
                Me.Yinterval.Text = Format(.ChartAreas(Me.CmbArea.SelectedIndex).AxisY.Interval, "0.0###")
            End If

            Me.Xfont.Text = .ChartAreas(Me.CmbArea.SelectedIndex).AxisX.LabelStyle.Font.Size
            Me.Yfont.Text = .ChartAreas(Me.CmbArea.SelectedIndex).AxisY.LabelStyle.Font.Size
            Me.CrossingTxt.Text = CStr(.ChartAreas(Me.CmbArea.SelectedIndex).AxisY.Crossing)
            If UCase(Me.CrossingTxt.Text) = "NAN" Then
                Me.CrossingTxt.Text = .ChartAreas(Me.CmbArea.SelectedIndex).AxisY.Minimum
            End If


            If .ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.Enabled = AxisEnabled.True Or _
                .ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.Enabled = AxisEnabled.Auto Then
                Me.YminSecond.Text = Format(.ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.Minimum, "0.0##")
                Me.YmaxSecond.Text = Format(.ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.Maximum, "0.0##")
                If .ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.Interval = 0.0 Then
                    Me.YintervalSecond.Text = "AUTO"
                Else
                    Me.YintervalSecond.Text = Format(.ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.Interval, "0.0###")
                End If
                Me.YfontSecond.Text = .ChartAreas(Me.CmbArea.SelectedIndex).AxisY2.LabelStyle.Font.Size
                Me.SecondaryBox.Enabled = True
            Else
                Me.SecondaryBox.Enabled = False
            End If
            '==========================================================================================================
        End With

    End Sub

    Private Sub FrmChartOption_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.AcceptButton = Me.BtnApply
    End Sub
End Class