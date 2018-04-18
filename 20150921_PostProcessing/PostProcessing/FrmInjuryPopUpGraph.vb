Imports System.Windows.Forms.DataVisualization.Charting
Imports System.IO
Imports Microsoft.Office.Interop
Imports Microsoft.Office.Core

Public Class InjuryPopUp

    'Public OriginChart As New Chart

    Dim TimeVals(,) As Decimal
    Dim DataVals(,) As Double
    Dim TimeVals_Tmp(,) As Decimal
    Dim DataVals_Tmp(,) As Double
    Dim max_cnt As Integer '시리즈 계열의 최고차수

    '해석 케이스 넘버
    Dim AnalCaseNum As Integer

    '이벤트를 발생시키지 않기위한 불리언 변수
    Dim EvenPass As Boolean = False

    Public tmp As DataPointCollection
    Public Tmp_Gindex As Integer
    Public IsTHOR As Integer
    Public Tmp_Chart As New Chart

    '창 크기 조절
    Dim FormerWidth As Integer
    Dim FormerHeight As Integer

    Dim DATAGraphDP As New DATA_Graphing

    Public Sub New(ByVal Graph_index As Integer, ByVal chrt As Chart, ByVal IsTHORgraph As Integer)
        'Public Sub New(ByVal Path As String, ByVal FileNames() As String, ByVal FileExt() As String, _
        '               ByVal Pos(,,) As Integer, ByVal PosTot(,,) As Integer, ByVal C_factor() As Decimal, _
        '               ByVal Graph_index As Integer, ByVal chrt As Chart) 'DataPointCollection) ', ByVal GraphTitle As String)
        'Public Sub New(ByVal Graph_index As Integer, ByVal chrt As Chart)

        ' IsTHORgraph
        ' 0 = Hybrid-3
        ' 1 = THOR

        ' 이 호출은 디자이너에 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하십시오.

        AnalCaseNum = chrt.Series.Count 'FileNames.Length
        Tmp_Gindex = Graph_index
        IsTHOR = IsTHORgraph
        Tmp_Chart = chrt '차트 데이터 복사 포함

    End Sub

    Private Sub InjuryPopUp_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        With Me
            .Width = 900
            .Height = 450
            .SplitContainer1.SplitterDistance = 650
        End With

        Me.StatusLbl.Text = "Select Series or Point"

        '===============================================================================================
        ' 데이터를 다시 읽어와서 쓰는 코드 (=변경했음)
        '===============================================================================================
        Dim i, j As Integer

        For i = Me.InjuryChart.Series.Count To 1 Step -1
            Me.InjuryChart.Series.RemoveAt(i - 1)
        Next

        With Me.InjuryChart

            '차트 이름 설정
            Select Case IsTHOR
                Case 0
                    .Titles.Add(GraphTitle(Tmp_Gindex))
                Case 1
                    .Titles.Add(GraphTitle_THOR(Tmp_Gindex))
            End Select

            .Titles(0).Font = New Font("맑은 고딕", 11, FontStyle.Bold)

            '데이터 박스 텍스트 설정
            Select Case IsTHOR
                Case 0
                    Me.DataBox.Text = GraphTitle(Tmp_Gindex) & " - Details"
                Case 1
                    Me.DataBox.Text = GraphTitle_THOR(Tmp_Gindex) & " - Details"
            End Select

            Me.DataBox.Font = New Font("맑은 고딕", 9, FontStyle.Bold)

            '폼의 제목 설정
            Select Case IsTHOR
                Case 0
                    Me.Text = "Injury Graph - " & GraphTitle(Tmp_Gindex)
                Case 1
                    Me.Text = "Injury Graph - " & GraphTitle_THOR(Tmp_Gindex)
            End Select

            For i = 0 To Tmp_Chart.Series.Count - 1 'UBound(FileNames)
                '데이터를 읽어와서 그래프를 그린다.=============================================================================
                'DataReading(Path, FileNames(i), FileExt(Graph_index), Pos(Graph_index, i, 0), Pos(Graph_index, i, 1), _
                '            PosTot(Graph_index, i, 0), PosTot(Graph_index, i, 1), TimeVals, DataVals, C_factor(Graph_index))
                .Series.Add(Tmp_Chart.Series(i).Name)
                '.Series("MD " & i + 1).Points.DataBindXY(TimeVals, DataVals)
                '=============================================================================================================

                Select Case Tmp_Chart.Series(i).YAxisType
                    Case AxisType.Primary
                        .Series(Tmp_Chart.Series(i).Name).YAxisType = AxisType.Primary
                    Case AxisType.Secondary
                        .ChartAreas(0).AxisY2.Enabled = AxisEnabled.Auto  '.True
                        .Series(Tmp_Chart.Series(i).Name).YAxisType = AxisType.Secondary
                        .ChartAreas(0).AxisY2.MinorGrid.Enabled = False
                        .ChartAreas(0).AxisY2.MajorGrid.Enabled = False
                        .ChartAreas(0).AxisY2.MajorTickMark.Enabled = True
                        .ChartAreas(0).AxisY2.LabelStyle.Font = New Font("Arial", 9, FontStyle.Bold)
                End Select

                '=============================================================================================================
                '데이터 포인트 컬랙션을 복사한다.===============================================================================
                tmp = Tmp_Chart.Series(i).Points
                For j = 0 To tmp.Count - 1
                    .Series(i).Points.AddXY(tmp(j).XValue, tmp(j).YValues(0))
                Next
                '=============================================================================================================
                .Series(i).Color = Tmp_Chart.Series(i).Color
                '=============================================================================================================

                '리스트 박스에 계열을 추가한다.
                Me.ChkLst.Items.Add(Tmp_Chart.Series(i).Name)
                Me.ChkLst.SetItemCheckState(i, CheckState.Checked)
            Next
            DATAGraphDP.GraphAxisLine(Me.InjuryChart)
            '팝업그래프만 선 굵기를 좀 굵게해준다.
            For i = 0 To .Series.Count - 1
                .Series(i).BorderWidth = 2
            Next
            DATAGraphDP.ScrollnZoom(Me.InjuryChart)
            DATAGraphDP.SeriesValShow(Me.InjuryChart)
            Me.InjuryChart.Legends(0).Docking = Docking.Bottom

            ''원래 차트와 같은 범위를 가지도록 설정 (X축만)
            Me.InjuryChart.ChartAreas(0).AxisX.Minimum = Tmp_Chart.ChartAreas(0).AxisX.Minimum
            Me.InjuryChart.ChartAreas(0).AxisX.Maximum = Tmp_Chart.ChartAreas(0).AxisX.Maximum
            Me.InjuryChart.ChartAreas(0).AxisX.MajorGrid.Interval = Tmp_Chart.ChartAreas(0).AxisX.MajorGrid.Interval

            'X축 Y축 글꼴설정
            .ChartAreas(0).AxisX.LabelStyle.Font = New Font("Arial", 10, FontStyle.Bold)
            .ChartAreas(0).AxisY.LabelStyle.Font = New Font("Arial", 10, FontStyle.Bold)
            .ChartAreas(0).AxisX.TitleFont = New Font("맑은 고딕", 11, FontStyle.Bold)
            .ChartAreas(0).AxisY.TitleFont = New Font("맑은 고딕", 11, FontStyle.Bold)
            .ChartAreas(0).AxisY2.TitleFont = New Font("맑은 고딕", 11, FontStyle.Bold)

            'Legend 글꼴
            .Legends(0).Font = New Font("맑은 고딕", 9, FontStyle.Bold)

            '타이틀 이름 설정 타겟을 위한 콤보박스 Item 추가
            Me.CmbChrtAreaTitle.Items.Add("Main Chart")
            Me.CmbChrtAreaTitle.SelectedIndex = 0

        End With
        '===============================================================================================

        ''================================================================= 차트 계열 복사..자꾸 문제생김
        'Dim i As Integer
        'Dim DATAGraphDP As New DATA_Graphing

        ''Dim chart1 = New Chart
        ''Dim chartArea1 As New ChartArea
        ''Me.Controls.Add(chart1)
        ''chart1.ChartAreas.Add(chartArea1)
        ''For i = 0 To Charts.Series.Count - 1 Step 1
        ''    chart1.Series.Add(i)
        ''    chart1.Series(i) = Charts.Series(i)
        ''Next

        ''Dim TmpDataSet As System.Data.DataSet
        'Dim Tmpseries As Series
        ''TmpDataSet = Charts.DataManipulator.ExportSeriesValues()
        'Tmpseries = OriginChart.Series(0)

        'With Me
        '    For i = .InjuryChart.Series.Count - 1 To 0 Step -1
        '        .InjuryChart.Series.RemoveAt(i)
        '    Next
        '    For i = 0 To 0 'Charts.Series.Count - 1 Step 1
        '        '.InjuryChart.Series.Add(Charts.Legends(0).ToString)
        '        .InjuryChart.Series.Add(i)
        '        .InjuryChart.Series(0) = Tmpseries   '<== 이게 문제임
        '    Next
        '    '.InjuryChart.DataBindTable(TmpDataSet.Tables(0))
        '    DATAGraphDP.GraphAxisLine(.InjuryChart)
        '    DATAGraphDP.ScrollnZoom(.InjuryChart)
        'End With
        '==================================================================

    End Sub

    Private Sub InjuryPopUp_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        Call SplitContainer1_SplitterMoved(sender, Nothing)

        If Me.Width > 600 And Me.Height > 300 Then
            With Me
                .InjuryChart.Location = New Point(5, 5)
                .InjuryChart.Size = New Size((.ClientRectangle.Width - 15) * 0.75, .ClientRectangle.Height - 30)
                '.DataBox.Location = New Point(5 + .InjuryChart.Width + 5, 5)
                '.DataBox.Size = New Size(.ClientRectangle.Width - (.ClientRectangle.Width - 15) * 0.75 - 15, .InjuryChart.Height)
                '.ChkLst.Location = New Point(5, 20)
                '.ChkLst.Size = New Size(.DataBox.ClientRectangle.Width - 10, AnalCaseNum * 21 + 2)
                '.LblXY.Location = New Point(5, .ChkLst.Location.Y + .ChkLst.Height + 10)
                '.LblXY.Size = New Size(.ChkLst.Width, 42)
                '.LblMin.Location = New Point(5, .LblXY.Location.Y + .LblXY.Height + 10)
                '.LblMin.Size = New Size(.LblXY.Width, 42)
                '.LblMax.Location = New Point(5, .LblMin.Location.Y + .LblMin.Height + 20)
                '.LblMax.Size = New Size(.LblMin.Width, 42)

                '.ChkMath.Location = New Point(.LblMax.Location.X, .LblMax.Location.Y + .LblMax.Height + 15)
                '.MathGroup.Location = New Point(.ChkMath.Location.X, .ChkMath.Location.Y + .ChkMath.Height + 5)
                '.MathGroup.Size = New Size(.ChkLst.Width, .DataBox.Height - MathGroup.Location.Y - 5)
                '.Xscale.Location = New Point(5, 15)
                '.Xscale.Size = New Size((.MathGroup.Width - 10) / 3, 22)
                '.Yscale.Location = New Point(.Xscale.Location.X + .Xscale.Width, .Xscale.Location.Y)
                '.Yscale.Size = .Xscale.Size
                '.LblScale.Size = .Xscale.Size
                '.LblScale.Location = New Point(.Yscale.Location.X + .Yscale.Width, .Yscale.Location.Y + 3)
                '.Xoffset.Location = New Point(.Xscale.Location.X, Xscale.Location.Y + Xscale.Height)
                '.Yoffset.Location = New Point(.Yscale.Location.X, Yscale.Location.Y + Yscale.Height)
                '.Xoffset.Size = .Xscale.Size
                '.Yoffset.Size = .Xscale.Size
                '.LblOffset.Location = New Point(.Yoffset.Location.X + .Yoffset.Width, .Yoffset.Location.Y + 3)

            End With
        End If
    End Sub

    Private Sub ChkLst_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles ChkLst.ItemCheck
        'Check 이벤트가 Selected 이벤트보다 먼저임

        If Me.ChkLst.SelectedIndex <> -1 And EvenPass <> True Then
            'MsgBox("Check")
            If Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Enabled = True Then
                Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Enabled = False
            Else
                Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Enabled = True
            End If

            EvenPass = True
        End If
    End Sub

    Private Sub ChkLst_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ChkLst.KeyDown
        If e.KeyCode = Keys.Y Then 'e.Modifiers = Keys.Control And 
            MoveItemAxisYToolStripMenuItem_Click(Nothing, e)
        End If
    End Sub

    Private Sub ChkLst_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ChkLst.KeyPress
        If e.KeyChar = ChrW(Keys.Escape) And Me.ChkLst.SelectedIndex <> -1 Then

            Dim i As Integer = 0

            With Me

                For i = 0 To .InjuryChart.ChartAreas.Count - 1
                    .InjuryChart.ChartAreas(i).AxisX.StripLines.Clear()
                Next

                For i = 1 To Me.InjuryChart.Series.Count Step 1
                    Me.InjuryChart.Series(i - 1).BorderWidth = 2
                    Me.InjuryChart.Series(i - 1).BorderDashStyle = ChartDashStyle.Solid
                    Dim MinValuePoint As DataPoint = .InjuryChart.Series(i - 1).Points.FindMinByValue()
                    Dim MaxValuePoint As DataPoint = .InjuryChart.Series(i - 1).Points.FindMaxByValue()
                    MinValuePoint.MarkerStyle = MarkerStyle.None
                    MaxValuePoint.MarkerStyle = MarkerStyle.None
                Next

            End With
        End If
    End Sub

    Private Sub ChkLst_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ChkLst.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then
            With Me
                .StatusLbl.Text = .ChkLst.Items(.ChkLst.SelectedIndex).ToString
            End With
        End If
    End Sub

    Private Sub ChkLst_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ChkLst.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Me.ListBoxMenu.Show(MousePosition)
        End If
    End Sub

    Private Sub ChkLst_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ChkLst.MouseUp
        If Me.ChkLst.SelectedIndex = -1 Then Exit Sub
        ToolTipCh.SetToolTip(Me.ChkLst, Me.ChkLst.Items(Me.ChkLst.SelectedIndex))
    End Sub

    Private Sub ChkLst_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkLst.SelectedIndexChanged

        'Item Check 할 때 이벤트 동작을 넘어가는 If문
        If EvenPass = True Then
            EvenPass = False
            Exit Sub
        End If

        With Me
            '선택된 계열을 강조한다.
            Dim i As Integer
            For i = 1 To .InjuryChart.Series.Count Step 1
                .InjuryChart.Series(i - 1).BorderWidth = 1
                .InjuryChart.Series(i - 1).BorderDashStyle = ChartDashStyle.Dot
                Dim Tmp_MinValuePoint As DataPoint = .InjuryChart.Series(i - 1).Points.FindMinByValue()
                Dim Tmp_MaxValuePoint As DataPoint = .InjuryChart.Series(i - 1).Points.FindMaxByValue()
                Tmp_MinValuePoint.MarkerStyle = MarkerStyle.None
                Tmp_MaxValuePoint.MarkerStyle = MarkerStyle.None
            Next
            '선택된 계열만 강조해준다.
            .InjuryChart.Series(.ChkLst.SelectedIndex).BorderWidth = 3
            .InjuryChart.Series(.ChkLst.SelectedIndex).BorderDashStyle = ChartDashStyle.Solid
            Dim MinValuePoint As DataPoint = .InjuryChart.Series(.ChkLst.SelectedIndex).Points.FindMinByValue()
            Dim MaxValuePoint As DataPoint = .InjuryChart.Series(.ChkLst.SelectedIndex).Points.FindMaxByValue()
            .LblMin.Text = "■ Minimum Value :" & vbCrLf & _
                "         " & Format(MinValuePoint.YValues(0), "0.0##") & " @ " & MinValuePoint.XValue
            .LblMax.Text = "■ Maximum Value :" & vbCrLf & _
                "         " & Format(MaxValuePoint.YValues(0), "0.0##") & " @ " & MaxValuePoint.XValue
            .StatusLbl.Text = .ChkLst.Items(.ChkLst.SelectedIndex).ToString

            '최대값의 위치표시
            MaxValuePoint.MarkerStyle = MarkerStyle.Cross
            MaxValuePoint.MarkerSize = 10
            MaxValuePoint.MarkerColor = Color.Red

            '최소값의 위치표시
            MinValuePoint.MarkerStyle = MarkerStyle.Cross
            MinValuePoint.MarkerSize = 10
            MinValuePoint.MarkerColor = Color.Red
        End With
        'MsgBox(Me.ChkLst.Items(Me.ChkLst.SelectedIndex))
        EvenPass = False
    End Sub

    Private Selectedpointindex As Integer = -1
    Private SelectedSeriesName As String
    Private PREresults As System.Windows.Forms.DataVisualization.Charting.HitTestResult

    Private Sub InjuryChart_Invalidated(ByVal sender As Object, ByVal e As System.Windows.Forms.InvalidateEventArgs) Handles InjuryChart.Invalidated
        Try
            With Me.InjuryChart
                For j = 0 To .Series.Count - 1
                    .Series(j).ToolTip = .Series(j).Name
                Next
            End With
        Catch ex As Exception
            Me.StatusLbl.Text = ex.Message.ToString
        End Try
    End Sub

    Private Sub InjuryChart_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles InjuryChart.KeyDown
        If e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            CToolStripMenuItem_Click(Nothing, e)
            'CopyChartImage(Me.InjuryChart)
        End If

        If e.Modifiers = Keys.Control And e.KeyCode = Keys.X Then
            'XYToolStripMenuItem_Click(Nothing, e) '계열 한개만 선택해서 데이터 복사하는 메뉴
            XYPairAllSeriesToolStripMenuItem_Click(Nothing, e) '전체 계열 다 복사
        End If

        If e.KeyCode = Keys.F1 Then
            ChartOptionToolStripMenuItem_Click(Nothing, e)
        End If

        If e.KeyCode = Keys.Delete Then
            ClearAdditionalChartToolStripMenuItem_Click(Nothing, e)
        End If

        If e.KeyCode = Keys.Insert Then
            AddAdditionalChartToolStripMenuItem_Click(Nothing, e)
        End If

        If e.KeyCode = Keys.F5 Then
            'chklst 에서 Escape Key를 눌렀을때와 동일함
            Dim i As Integer = 0

            With Me

                For i = 0 To .InjuryChart.ChartAreas.Count - 1
                    .InjuryChart.ChartAreas(i).AxisX.StripLines.Clear()
                Next

                For i = 1 To Me.InjuryChart.Series.Count Step 1
                    Me.InjuryChart.Series(i - 1).BorderWidth = 2
                    Me.InjuryChart.Series(i - 1).BorderDashStyle = ChartDashStyle.Solid
                    Dim MinValuePoint As DataPoint = .InjuryChart.Series(i - 1).Points.FindMinByValue()
                    Dim MaxValuePoint As DataPoint = .InjuryChart.Series(i - 1).Points.FindMaxByValue()
                    MinValuePoint.MarkerStyle = MarkerStyle.None
                    MaxValuePoint.MarkerStyle = MarkerStyle.None
                Next

            End With
        End If

        If e.Modifiers = Keys.Control And e.KeyCode = Keys.V Then
            '클립보드 데이터를 그려준다.

            Dim k As Integer = 0

            If Clipboard.GetText().Trim.EndsWith("paste data") = True Then
                '=============================================================================
                '                           Possible Format
                ' First Line  : Title
                ' Second Line  : Data Start
                ' Last Line   : paste data (MUST INCLUDED!!!)
                '=============================================================================
                Dim Text_Arr() As String
                Text_Arr = Clipboard.GetText().Split(vbCrLf)
                Dim NewSeriesName As String = ""

                Dim SeriesNew As String = ""
                SeriesNew = InputBox(" Insert New Series Name" & vbCrLf & _
                                     "   1. First Line  - Series Name" & vbCrLf & _
                                     "   2. Second Line - Skipped" & vbCrLf & _
                                     "   3. Below Third Line - Draw", "Insert Name", Text_Arr(0))
                NewSeriesName = SeriesNamingChrt(SeriesNew, Me.InjuryChart)

                Dim Xval(0) As Double
                Dim Result_ArrY(0) As Double
                Dim Tmp_str() As String
                Dim Str_Delimiter As String = " "

                If InStr(Text_Arr(1), " ") Then
                    Str_Delimiter = " "
                ElseIf InStr(Text_Arr(1), Chr(9)) Then
                    Str_Delimiter = Chr(9)
                ElseIf InStr(Text_Arr(1), ",") Then
                    Str_Delimiter = ","
                End If

                Tmp_str = Text_Arr(1).Trim.Split(Str_Delimiter)
                Xval(0) = Tmp_str(LBound(Tmp_str))
                Result_ArrY(0) = Tmp_str(UBound(Tmp_str))

                'For k = 2 To UBound(Text_Arr) - 1
                '    Tmp_str = Text_Arr(k).Trim.Split(Str_Delimiter)
                '    If IsNumeric(Tmp_str(LBound(Tmp_str))) = False Then Exit For

                '    ReDim Preserve Xval(UBound(Xval) + 1)
                '    ReDim Preserve Result_ArrY(UBound(Result_ArrY) + 1)
                '    Xval(k - 2) = Tmp_str(LBound(Tmp_str))
                '    Result_ArrY(k - 2) = Tmp_str(UBound(Tmp_str))
                'Next

                k = 2
                Tmp_str = Text_Arr(k).Trim.Split(Str_Delimiter)
                Do While IsNumeric(Tmp_str(LBound(Tmp_str))) = True

                    ReDim Preserve Xval(UBound(Xval) + 1)
                    ReDim Preserve Result_ArrY(UBound(Result_ArrY) + 1)
                    Xval(UBound(Xval)) = Tmp_str(LBound(Tmp_str))
                    Result_ArrY(UBound(Xval)) = Tmp_str(UBound(Tmp_str))

                    k = k + 1
                    Tmp_str = Text_Arr(k).Trim.Split(Str_Delimiter)
                Loop


                '계열추가
                Me.ChkLst.Items.Add(NewSeriesName)
                EvenPass = True
                Me.ChkLst.SetItemCheckState(Me.ChkLst.Items.Count - 1, CheckState.Checked)
                EvenPass = False

                Me.InjuryChart.Series.Add(NewSeriesName)
                Me.InjuryChart.Series(NewSeriesName).ChartType = SeriesChartType.FastLine
                Me.InjuryChart.Series(NewSeriesName).BorderWidth = 2
                Me.InjuryChart.Series(NewSeriesName).Points.DataBindXY(Xval, Result_ArrY)

                AnalCaseNum = AnalCaseNum + 1

            End If

            
        End If

    End Sub

    Private Sub InjuryChart_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles InjuryChart.KeyPress
        'If e.KeyChar = ChrW(Keys.Escape) And Selectedpointindex > -1 Then
        '    Me.InjuryChart.Series(SelectedSeriesName).Points(Selectedpointindex).MarkerStyle = MarkerStyle.None
        'End If
    End Sub

    Private Sub InjuryChart_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles InjuryChart.MouseClick
        Try
            If e.Button = Windows.Forms.MouseButtons.Right Then
                Me.ChartContextMenu.Show(MousePosition)
            ElseIf e.Button = Windows.Forms.MouseButtons.Left Then
                Dim results As System.Windows.Forms.DataVisualization.Charting.HitTestResult
                results = Me.InjuryChart.HitTest(e.X, e.Y)
                If results.Series IsNot Nothing Then
                    If Selectedpointindex > -1 Then
                        If PREresults.PointIndex = results.PointIndex Then
                            results.Series.Points(Selectedpointindex).MarkerStyle = DataVisualization.Charting.MarkerStyle.None
                            Me.StatusLbl.Text = "Select Series or Point"
                            Exit Sub
                        End If
                        results.Series.Points(Selectedpointindex).MarkerStyle = DataVisualization.Charting.MarkerStyle.None
                    End If
                    SelectedSeriesName = results.Series.Name
                    results.Series.Points(results.PointIndex).MarkerStyle = MarkerStyle.Cross
                    results.Series.Points(results.PointIndex).MarkerSize = 10
                    results.Series.Points(results.PointIndex).MarkerColor = Color.Red
                    Selectedpointindex = results.PointIndex
                    Me.InjuryChart.Invalidate()
                    Me.LblXY.Text = "X : " & Format(results.Series.Points(results.PointIndex).XValue, "0.000") & " / " & _
                        "Y : " & Format(results.Series.Points(results.PointIndex).YValues(0), "0.000")
                    Me.StatusLbl.Text = results.Series.Name & " -- " & "X : " & Format(results.Series.Points(results.PointIndex).XValue, "0.000") & " / " & _
                        "Y : " & Format(results.Series.Points(results.PointIndex).YValues(0), "0.000")
                    PREresults = results
                    'Me.TextBox1 = Me.InjuryChart.Series(results.Series.)
                    'MsgBox(results.Series.Name)
                    'Else
                    '    If selectedpointindex > -1 Then
                    '        results.Series.Points(selectedpointindex).MarkerStyle = DataVisualization.Charting.MarkerStyle.None
                    '    End If
                End If

            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub CToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CToolStripMenuItem.Click
        CopyChartImage(Me.InjuryChart)

        '********* 컨트롤의 영역을 복사하는 방법 *********
        'Dim controlImage As Bitmap = GetControlScreenshot(Me.InjuryChart)
        ''controlImage.Save("TestImage.bmp")
        'Clipboard.SetDataObject(controlImage)
    End Sub

    Private Function GetControlScreenshot(ByVal control As Control) As Bitmap
        Dim g As Graphics = control.CreateGraphics()
        Dim bitmap As Bitmap = New Bitmap(control.Width, control.Height)
        control.DrawToBitmap(bitmap, New Rectangle(control.Location, control.Size))
        GetControlScreenshot = bitmap
    End Function

    Private Sub ChartOptionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChartOptionToolStripMenuItem.Click
        Dim OptionFrm As New FrmChartOption(Me.InjuryChart)
        OptionFrm.Owner = Me
        OptionFrm.ShowDialog()
    End Sub

    Private Sub ExportXYDataToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportXYDataToolStripMenuItem.Click

        With Me.SaveXYDlg
            .Title = "저장 파일 선택"
            .FileName = Me.Text                    '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "dat 파일|*.dat|모든 파일|*.*"
            .ShowDialog()
        End With

        If Me.SaveXYDlg.FileName = "" Then Exit Sub

        '파일 생성
        Dim i, k As Integer
        Dim TmpSeriesPoints As DataPointCollection
        Dim NewTitleFile As StreamWriter
        NewTitleFile = New StreamWriter(Me.SaveXYDlg.FileName)

        With Me.InjuryChart
            For i = 0 To .Series.Count - 1
                NewTitleFile.WriteLine(.Series(i).Name)
                TmpSeriesPoints = .Series(i).Points
                For k = 0 To .Series(i).Points.Count - 1
                    NewTitleFile.WriteLine(Chr(9) & TmpSeriesPoints(k).XValue & Chr(9) & TmpSeriesPoints(k).YValues(0))
                Next
                NewTitleFile.WriteLine()
            Next
        End With

        NewTitleFile.Close()

    End Sub

    Private Sub ExportToExcelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportToExcelToolStripMenuItem.Click

        'With Me.SaveXYDlg
        '    .Title = "저장 파일 선택"
        '    .FileName = Me.Text                    '초기에 표시되는 파일 이름
        '    .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
        '    .Filter = "xlsx 파일|*.xlsx|모든 파일|*.*"
        '    .ShowDialog()
        'End With

        'If Me.SaveXYDlg.FileName = "" Then Exit Sub

        Dim TmpSeriesPoints As DataPointCollection

        Dim xls As New Excel.Application
        Dim wb As Excel.Workbook
        Dim ws As Excel.Worksheet
        'Dim Rng As Excel.Range

        xls.Visible = False

        'wb = xls.Workbooks.(Me.SaveXYDlg.FileName)
        wb = xls.Workbooks.Add
        ws = wb.Sheets("Sheet1")

        'Rng = ws.Range("a1")
        'Dim ss As String = Rng.Value  '//읽기
        'Rng = "aaaaaaa"  '//쓰기

        'ws.Cells(1, 1).value = 4

        MainMDI.ProgressBarMain.Value = 0

        With Me.InjuryChart
            For i = 0 To .Series.Count - 1
                TmpSeriesPoints = .Series(i).Points
                ws.Cells(1, 2 * (i + 1) - 1) = .Series(i).Name
                MainMDI.ProgressBarMain.Maximum = .Series(i).Points.Count - 1
                For k = 0 To .Series(i).Points.Count - 1
                    ws.Cells(k + 2, (i + 1) * 2 - 1).value = TmpSeriesPoints(k).XValue
                    ws.Cells(k + 2, (i + 1) * 2).value = TmpSeriesPoints(k).YValues(0)
                    MainMDI.ProgressBarMain.Value = k
                Next
            Next
        End With

        xls.Visible = True

        ws = Nothing
        wb = Nothing
        xls = Nothing
        'wb.Close()
        'xls.Quit()


    End Sub

    Private Sub ChkMath_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkMath.CheckedChanged
        Dim i, j As Integer

        If Me.ChkMath.Checked = False Then
            Me.MathGroup.Enabled = False
            'Me.MathToolStripMenuItem.Enabled = True
            '차트 데이터를 원래대로 복귀한다.
            Me.Xscale.Text = "1.0"
            Me.Yscale.Text = "1.0"
            Me.Xoffset.Text = "0.0"
            Me.Yoffset.Text = "0.0"

            For i = 0 To Tmp_Chart.Series.Count - 1
                '데이터 포인트 컬랙션을 복사한다.===============================================================================
                tmp = Tmp_Chart.Series(i).Points
                Me.InjuryChart.Series(i).Points.Clear()
                For j = 0 To tmp.Count - 1
                    Me.InjuryChart.Series(i).Points.AddXY(tmp(j).XValue, tmp(j).YValues(0))
                    Me.InjuryChart.Series(i).YAxisType = AxisType.Primary
                Next
                'For j = 0 To max_cnt - 1
                '    Me.InjuryChart.Series(i).Points.AddXY(TimeVals(i, j), DataVals(i, j))

                'Next
                '=============================================================================================================
            Next
            Exit Sub
        End If

        With Me
            .MathGroup.Enabled = True

            '.MathToolStripMenuItem.Enabled = False

            If .InjuryChart.Series.Count = 1 Then
                max_cnt = .InjuryChart.Series(0).Points.Count
            Else
                For i = 1 To .InjuryChart.Series.Count - 1
                    If .InjuryChart.Series(i).Points.Count > .InjuryChart.Series(i - 1).Points.Count Then
                        max_cnt = .InjuryChart.Series(i).Points.Count
                    Else
                        max_cnt = .InjuryChart.Series(i - 1).Points.Count
                    End If
                Next
            End If

            ReDim TimeVals(0 To .InjuryChart.Series.Count - 1, max_cnt - 1)
            ReDim DataVals(0 To .InjuryChart.Series.Count - 1, max_cnt - 1)

            For i = 0 To .InjuryChart.Series.Count - 1
                For j = 0 To .InjuryChart.Series(i).Points.Count - 1
                    TimeVals(i, j) = .InjuryChart.Series(i).Points(j).XValue
                    DataVals(i, j) = .InjuryChart.Series(i).Points(j).YValues(0)
                Next
            Next
            TimeVals_Tmp = TimeVals
            DataVals_Tmp = DataVals
        End With
    End Sub


    Private Sub Yscale_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Yscale.KeyPress

        If e.KeyChar = ChrW(Keys.Enter) And Me.ChkLst.SelectedIndex <> -1 Then

            With Me
                If (IsNumeric(.Xscale.Text) = False) Or (IsNumeric(Yscale.Text) = False) Or (IsNumeric(.Xoffset.Text) = False) Or (IsNumeric(.Yoffset.Text) = False) Then
                    MsgBox("Numeric Value Only!", , "Invalid Value")
                    Exit Sub
                Else
                    Graph_Handling(CDbl(.Xscale.Text), CDbl(.Yscale.Text), CDbl(.Xoffset.Text), CDbl(.Yoffset.Text))
                End If
            End With

            Me.Xscale.Text = "1.0"
            Me.Yscale.Text = "1.0"
            Me.Xoffset.Text = "0.0"
            Me.Yoffset.Text = "0.0"

        End If


    End Sub

    Private Sub Xscale_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Xscale.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) And Me.ChkLst.SelectedIndex <> -1 Then
            Yscale_KeyPress("-1", New KeyPressEventArgs(ChrW(Keys.Enter)))
        End If
    End Sub

    Private Sub Graph_Handling(ByVal Xscaleval As Double, ByVal yscaleval As Double, ByVal xoffsetval As Double, ByVal yoffsetval As Double)
        Dim i, k, PointCnt As Integer ', PointCnt
        Dim IsSecondary As Boolean = False

        Dim Temp_Time() As Double
        Dim Temp_Vals() As Double

        With Me
            'PointCnt = .InjuryChart.Series(.ChkLst.SelectedIndex).Points.Count - 1
            'PointCnt = UBound(TimeVals, 2) - 1

            If .InjuryChart.Series(.ChkLst.SelectedIndex).YAxisType = AxisType.Secondary Then
                IsSecondary = True
            End If

            PointCnt = .InjuryChart.Series(.ChkLst.SelectedIndex).Points.Count

            ReDim Temp_Time(0 To PointCnt - 1)
            ReDim Temp_Vals(0 To PointCnt - 1)
            For k = 0 To PointCnt - 1 Step 1
                Temp_Time(k) = .InjuryChart.Series(.ChkLst.SelectedIndex).Points(k).XValue
                Temp_Vals(k) = .InjuryChart.Series(.ChkLst.SelectedIndex).Points(k).YValues(0)
            Next

            .InjuryChart.Series(.ChkLst.SelectedIndex).Points.Clear()

            For i = 0 To PointCnt - 1 Step 1
                '.InjuryChart.Series(.ChkLst.SelectedIndex).Points.AddXY(TimeVals_Tmp(.ChkLst.SelectedIndex, i) * Xscaleval + xoffsetval, DataVals_Tmp(.ChkLst.SelectedIndex, i) * yscaleval + yoffsetval)
                .InjuryChart.Series(.ChkLst.SelectedIndex).Points.AddXY(Temp_Time(i) * Xscaleval + xoffsetval, Temp_Vals(i) * yscaleval + yoffsetval)

                '이부분 다시보자?????????????????????????????????????????????????????????????
                'TimeVals_Tmp(.ChkLst.SelectedIndex, i) = TimeVals_Tmp(.ChkLst.SelectedIndex, i) * Xscaleval + xoffsetval
                'DataVals_Tmp(.ChkLst.SelectedIndex, i) = DataVals_Tmp(.ChkLst.SelectedIndex, i) * yscaleval + yoffsetval
            Next

            If IsSecondary = True Then
                .InjuryChart.Series(.ChkLst.SelectedIndex).YAxisType = AxisType.Secondary
            End If


        End With

    End Sub

    Private Sub Yoffset_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Yoffset.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) And Me.ChkLst.SelectedIndex <> -1 Then
            Yscale_KeyPress("-1", New KeyPressEventArgs(ChrW(Keys.Enter)))
        End If
    End Sub

    Private Sub Xoffset_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Xoffset.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) And Me.ChkLst.SelectedIndex <> -1 Then
            Yscale_KeyPress("-1", New KeyPressEventArgs(ChrW(Keys.Enter)))
        End If
    End Sub

    Private Sub IntegrationSimpsonsRuleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IntegrationSimpsonsRuleToolStripMenuItem.Click

        'Simpson's rule is only slightly more complex than the trapezoidal rule. 
        'It requires that N be even and it is derived by approximating the integrand by a piecewise-quadratic function. 
        'This means that we expect it to give an exact result if the integrand is a quadratic function. 
        'But Simpson's rule does an even better job: It turns out that it gives an exact result for integrating even a cubic function.

        Dim TmpSeriesPoints As DataPointCollection
        Dim k As Integer
        Dim Summation As Integer = 0
        Dim Result_ArrY() As Double

        If Me.ChkLst.SelectedIndex < 0 Then
            MsgBox("Select a Series", , "Try Again")
            Exit Sub
        End If

        With Me.InjuryChart

            Dim NewSeriesName As String = "Integ. " & .Series(Me.ChkLst.SelectedIndex).Name
            Dim SameSeries As Boolean = False

            EvenPass = True
            For i = 0 To .Series.Count - 1
                If NewSeriesName = .Series(i).Name Then
                    SameSeries = True
                    NewSeriesName = "Re-" & NewSeriesName
                End If
            Next

            .Series.Add(NewSeriesName)
            Me.ChkLst.Items.Add(NewSeriesName)

            EvenPass = True
            Me.ChkLst.SetItemCheckState(Me.ChkLst.Items.Count - 1, CheckState.Checked)
            EvenPass = False
            TmpSeriesPoints = .Series(Me.ChkLst.SelectedIndex).Points

            ReDim Preserve Result_ArrY(0)
            Result_ArrY(0) = 0.0

            .Series(.Series.Count - 1).Points.AddXY(TmpSeriesPoints(0).XValue, Result_ArrY(0))

            'If IsNothing(TimeVals) = False Then
            'ReDim Preserve TimeVals(0 To UBound(TimeVals, 1) + 1, max_cnt - 1)
            'ReDim Preserve DataVals(0 To UBound(DataVals, 1) + 1, max_cnt - 1)

            'TimeVals(UBound(TimeVals, 1), 0) = TmpSeriesPoints(0).XValue
            'DataVals(UBound(DataVals, 1), 0) = Result_ArrY(0)
            'End If

            For k = 1 To .Series(Me.ChkLst.SelectedIndex).Points.Count - 1
                ReDim Preserve Result_ArrY(k)

                '실제 누적 적분값을 구하는 부분===============================================================================================================================
                Result_ArrY(k) = Result_ArrY(k - 1) + _
                     (TmpSeriesPoints(k).XValue - TmpSeriesPoints(k - 1).XValue) / 6 _
                     * (TmpSeriesPoints(k - 1).YValues(0) + ((TmpSeriesPoints(k - 1).YValues(0) + TmpSeriesPoints(k).YValues(0)) * 2) + TmpSeriesPoints(k).YValues(0))

                .Series(.Series.Count - 1).Points.AddXY(TmpSeriesPoints(k).XValue, Result_ArrY(k))
                '===========================================================================================================================================================
                'If IsNothing(TimeVals) = False Then
                'TimeVals(UBound(TimeVals, 1), k) = TmpSeriesPoints(k).XValue
                'DataVals(UBound(DataVals, 1), k) = Result_ArrY(k)
                'End If
            Next

            .Series(.Series.Count - 1).ChartArea = .Series(Me.ChkLst.SelectedIndex).ChartArea

            .ChartAreas(.Series(Me.ChkLst.SelectedIndex).ChartArea).AxisY2.Enabled = AxisEnabled.Auto
            .Series(.Series.Count - 1).ChartType = SeriesChartType.Line
            .Series(.Series.Count - 1).YAxisType = AxisType.Secondary
            'MsgBox(.ChartAreas(.Series(Me.ChkLst.SelectedIndex).ChartArea).AxisY2.Enabled)
            .ChartAreas(.Series(Me.ChkLst.SelectedIndex).ChartArea).AxisY2.MinorGrid.Enabled = False
            .ChartAreas(.Series(Me.ChkLst.SelectedIndex).ChartArea).AxisY2.MajorGrid.Enabled = False
            .ChartAreas(.Series(Me.ChkLst.SelectedIndex).ChartArea).AxisY2.MajorTickMark.Enabled = True
            .ChartAreas(.Series(Me.ChkLst.SelectedIndex).ChartArea).AxisY2.LabelStyle.Font = New Font("Arial", 10, FontStyle.Bold)


            AnalCaseNum = AnalCaseNum + 1
        End With

        InjuryPopUp_Resize(Me, EventArgs.Empty)

    End Sub

    Private Sub DifferentialForwardDifferenceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DifferentialForwardDifferenceToolStripMenuItem.Click
        Dim TmpSeriesPoints As DataPointCollection
        Dim k As Integer
        Dim Summation As Integer = 0
        Dim Result_ArrY() As Double

        If Me.ChkLst.SelectedIndex < 0 Then
            MsgBox("Select a Series", , "Try Again")
            Exit Sub
        End If

        With Me.InjuryChart

            Dim NewSeriesName As String = "Diff. " & .Series(Me.ChkLst.SelectedIndex).Name
            Dim SameSeries As Boolean = False

            EvenPass = True
            For i = 0 To .Series.Count - 1
                If NewSeriesName = .Series(i).Name Then
                    SameSeries = True
                    NewSeriesName = "Re-" & NewSeriesName
                End If
            Next

            .Series.Add(NewSeriesName)
            Me.ChkLst.Items.Add(NewSeriesName)

            EvenPass = True
            Me.ChkLst.SetItemCheckState(Me.ChkLst.Items.Count - 1, CheckState.Checked)
            EvenPass = False
            TmpSeriesPoints = .Series(Me.ChkLst.SelectedIndex).Points

            ReDim Preserve Result_ArrY(0)
            Result_ArrY(0) = 0.0

            .Series(.Series.Count - 1).Points.AddXY(TmpSeriesPoints(0).XValue, Result_ArrY(0))

            For k = 1 To .Series(Me.ChkLst.SelectedIndex).Points.Count - 1
                ReDim Preserve Result_ArrY(k)

                '실제 후위 미분값을 구하는 부분===============================================================================================================================
                Result_ArrY(k) = (TmpSeriesPoints(k).YValues(0) - TmpSeriesPoints(k - 1).YValues(0)) / (TmpSeriesPoints(k).XValue - TmpSeriesPoints(k - 1).XValue)

                .Series(.Series.Count - 1).Points.AddXY(TmpSeriesPoints(k).XValue, Result_ArrY(k))
                '===========================================================================================================================================================
            Next

            .Series(.Series.Count - 1).ChartArea = .Series(Me.ChkLst.SelectedIndex).ChartArea

            .ChartAreas(.Series(Me.ChkLst.SelectedIndex).ChartArea).AxisY2.Enabled = AxisEnabled.Auto
            .Series(.Series.Count - 1).ChartType = SeriesChartType.Line
            .Series(.Series.Count - 1).YAxisType = AxisType.Secondary
            .ChartAreas(.Series(Me.ChkLst.SelectedIndex).ChartArea).AxisY2.MinorGrid.Enabled = False
            .ChartAreas(.Series(Me.ChkLst.SelectedIndex).ChartArea).AxisY2.MajorGrid.Enabled = False
            .ChartAreas(.Series(Me.ChkLst.SelectedIndex).ChartArea).AxisY2.MajorTickMark.Enabled = True
            .ChartAreas(.Series(Me.ChkLst.SelectedIndex).ChartArea).AxisY2.LabelStyle.Font = New Font("Arial", 10, FontStyle.Bold)

            AnalCaseNum = AnalCaseNum + 1
        End With

        InjuryPopUp_Resize(Me, EventArgs.Empty)
    End Sub

    Private Sub SplitContainer1_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitContainer1.SplitterMoved
        With Me
            .DataBox.Location = New Point(5 + .InjuryChart.Width + 5, 5)
            .DataBox.Size = New Size(.ClientRectangle.Width - (.ClientRectangle.Width - 15) * 0.75 - 15, .InjuryChart.Height)
            .ChkLst.Location = New Point(5, 20)
            .ChkLst.Size = New Size(.DataBox.ClientRectangle.Width - 10, AnalCaseNum * 21 + 2)
            .LblXY.Location = New Point(5, .ChkLst.Location.Y + .ChkLst.Height + 10)
            .LblXY.Size = New Size(.ChkLst.Width, 42)
            .LblMin.Location = New Point(5, .LblXY.Location.Y + .LblXY.Height + 10)
            .LblMin.Size = New Size(.LblXY.Width, 42)
            .LblMax.Location = New Point(5, .LblMin.Location.Y + .LblMin.Height + 20)
            .LblMax.Size = New Size(.LblMin.Width, 42)

            .ChkMath.Location = New Point(.LblMax.Location.X, .LblMax.Location.Y + .LblMax.Height + 15)
            .MathGroup.Location = New Point(.ChkMath.Location.X, .ChkMath.Location.Y + .ChkMath.Height + 5)
            .MathGroup.Size = New Size(.ChkLst.Width, .DataBox.Height - MathGroup.Location.Y - 5 - 20)
            .ChkSize.Location = New Point(.MathGroup.Location.X, .MathGroup.Location.Y + .MathGroup.Height + 5)

            .Xscale.Location = New Point(5, 15)
            .Xscale.Size = New Size((.MathGroup.Width - 10) / 3, 22)
            .Yscale.Location = New Point(.Xscale.Location.X + .Xscale.Width, .Xscale.Location.Y)
            .Yscale.Size = .Xscale.Size
            .LblScale.Size = .Xscale.Size
            .LblScale.Location = New Point(.Yscale.Location.X + .Yscale.Width, .Yscale.Location.Y + 3)
            .Xoffset.Location = New Point(.Xscale.Location.X, Xscale.Location.Y + Xscale.Height)
            .Yoffset.Location = New Point(.Yscale.Location.X, Yscale.Location.Y + Yscale.Height)
            .Xoffset.Size = .Xscale.Size
            .Yoffset.Size = .Xscale.Size
            .LblOffset.Location = New Point(.Yoffset.Location.X + .Yoffset.Width, .Yoffset.Location.Y + 3)
        End With
    End Sub

    Private Sub RenameList_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles RenameList.KeyPress

        Dim S_Index As Integer = Me.ChkLst.SelectedIndex

        If e.KeyChar = ChrW(Keys.Enter) And Me.ChkLst.SelectedIndex <> -1 And Me.RenameList.Text <> "" Then
            'EvenPass = True
            'Me.ChkLst.Items.Insert(Me.ChkLst.SelectedIndex, Me.RenameList.Text)
            'Me.ChkLst.Items.RemoveAt(S_Index + 1)
            'Me.ChkLst.SelectedItem = Me.ChkLst.Items(S_Index)
            'EvenPass = True
            'Me.ChkLst.SetItemCheckState(S_Index, CheckState.Checked)
            'Me.InjuryChart.Series(S_Index).Name = Me.RenameList.Text
            'Me.RenameList.Text = ""
            'EvenPass = False

            EvenPass = True
            Me.ChkLst.Items.RemoveAt(S_Index)
            Me.ChkLst.Items.Insert(S_Index, Me.RenameList.Text)
            Me.ChkLst.SelectedItem = Me.ChkLst.Items(S_Index)
            EvenPass = True
            Me.ChkLst.SetItemCheckState(S_Index, CheckState.Checked)
            Me.InjuryChart.Series(S_Index).Name = Me.RenameList.Text
            Me.RenameList.Text = ""
            EvenPass = False
            Me.ListBoxMenu.Close()
        End If

    End Sub

    Private Sub ChartTitleTxt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ChartTitleTxt.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) And Me.ChartTitleTxt.Text <> "" Then
            ApplyTitles()
            Me.ChartContextMenu.Close()
        End If
    End Sub

    Private Sub XaxitTitleTxt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles XaxitTitleTxt.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) And Me.XaxitTitleTxt.Text <> "" Then
            ApplyTitles()
            Me.ChartContextMenu.Close()
        End If
    End Sub

    Private Sub YaxisTitleTxt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles YaxisTitleTxt.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) And Me.YaxisTitleTxt.Text <> "" Then
            ApplyTitles()
            Me.ChartContextMenu.Close()
        End If
    End Sub

    Private Sub Yaxis2TitleTxt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Yaxis2TitleTxt.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) And Me.Yaxis2TitleTxt.Text <> "" Then
            ApplyTitles()
            Me.ChartContextMenu.Close()
        End If
    End Sub

    Private Sub ApplyTitles()

        Dim k As Integer = Me.CmbChrtAreaTitle.SelectedIndex

        If Me.ChartTitleTxt.Text <> "" Then Me.InjuryChart.Titles(k).Text = Me.ChartTitleTxt.Text
        If Me.XaxitTitleTxt.Text <> "" Then Me.InjuryChart.ChartAreas(k).AxisX.Title = Me.XaxitTitleTxt.Text
        If Me.YaxisTitleTxt.Text <> "" Then Me.InjuryChart.ChartAreas(k).AxisY.Title = Me.YaxisTitleTxt.Text
        If Me.Yaxis2TitleTxt.Text <> "" And Me.InjuryChart.ChartAreas(k).AxisY2.Enabled = AxisEnabled.Auto Then Me.InjuryChart.ChartAreas(k).AxisY2.Title = Me.Yaxis2TitleTxt.Text
        Me.ChartTitleTxt.Text = ""
        Me.XaxitTitleTxt.Text = ""
        Me.YaxisTitleTxt.Text = ""
        Me.Yaxis2TitleTxt.Text = ""
    End Sub

    Private Sub XToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XToolStripMenuItem.Click
        ' Clipboard.SetData(Of Double, tmp_chart)()
        If Me.ChkLst.SelectedIndex < 0 Then
            MsgBox("Select a Series First.", MsgBoxStyle.Information, "Series Selection")
        End If

        Clipboard.Clear()
        Dim i As Integer
        Dim TempSeriesTxt As String = ""
        Dim TmpSeriesPoints As DataPointCollection = Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Points

        TempSeriesTxt = Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Name & vbCrLf

        For i = 0 To Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Points.Count - 1
            TempSeriesTxt = TempSeriesTxt & CStr(Format(TmpSeriesPoints(i).XValue, "0.00####")) & vbCrLf
        Next

        Clipboard.SetText(TempSeriesTxt)
    End Sub

    Private Sub YToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles YToolStripMenuItem.Click
        If Me.ChkLst.SelectedIndex < 0 Then
            MsgBox("Select a Series First.", MsgBoxStyle.Information, "Series Selection")
        End If

        Clipboard.Clear()
        Dim i As Integer
        Dim TempSeriesTxt As String = ""
        Dim TmpSeriesPoints As DataPointCollection = Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Points

        TempSeriesTxt = Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Name & vbCrLf

        For i = 0 To Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Points.Count - 1
            TempSeriesTxt = TempSeriesTxt & CStr(Format(TmpSeriesPoints(i).YValues(0), "0.00####")) & vbCrLf
        Next

        Clipboard.SetText(TempSeriesTxt)
    End Sub

    Private Sub XYToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XYToolStripMenuItem.Click
        If Me.ChkLst.SelectedIndex < 0 Then
            MsgBox("Select a Series First.", MsgBoxStyle.Information, "Series Selection")
            Exit Sub
        End If

        Clipboard.Clear()
        Dim i As Integer
        Dim TempSeriesTxt As String = ""
        Dim TmpSeriesPoints As DataPointCollection = Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Points

        TempSeriesTxt = Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Name & vbCrLf

        For i = 0 To Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Points.Count - 1
            TempSeriesTxt = TempSeriesTxt & CStr(Format(TmpSeriesPoints(i).XValue, "0.00####") & Chr(9) & Format(TmpSeriesPoints(i).YValues(0), "0.00####")) & vbCrLf
        Next

        Clipboard.SetText(TempSeriesTxt)
    End Sub

    Private Sub InjuryChart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InjuryChart.Click
        Me.InjuryChart.Focus()
    End Sub


    Private Sub XYPairAllSeriesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XYPairAllSeriesToolStripMenuItem.Click

        Clipboard.Clear()
        Dim i, k As Integer
        Dim TempSeriesTxt As String = ""
        Dim StartIndex(Me.InjuryChart.Series.Count - 1) As Integer
        Dim DataCnt(Me.InjuryChart.Series.Count - 1) As Integer
        Dim DataCntSort() As Integer

        TempSeriesTxt = Me.InjuryChart.Titles(0).Text

        '각 계열의 시작지점을 찾는다.
        For k = 0 To Me.InjuryChart.Series.Count - 1
            Dim TmpSeriesPoints As DataPointCollection = Me.InjuryChart.Series(k).Points
            For i = 0 To TmpSeriesPoints.Count - 1
                If Math.Abs(Me.InjuryChart.ChartAreas(Me.InjuryChart.Series(k).ChartArea).AxisX.Minimum - TmpSeriesPoints(i).XValue) < 0.0000001 Then
                    StartIndex(k) = i
                End If
            Next
        Next

        '제일 긴 시리즈를 찾는다
        For i = 0 To Me.InjuryChart.Series.Count - 1
            DataCnt(i) = Me.InjuryChart.Series(i).Points.Count - 1 - StartIndex(i)
        Next

        DataCntSort = DataCnt
        'Array.Sort(DataCntSort)

        Dim TmpRowTxt As String = ""
        Dim TmpDATAtxt As String = ""
        Dim m As Integer = 0

        For i = 0 To Me.InjuryChart.Series.Count - 1
            TmpDATAtxt = TmpDATAtxt & Me.InjuryChart.Series(i).Name & Chr(9) & Chr(9)
        Next

        TmpDATAtxt = TmpDATAtxt & vbCrLf

        For i = 0 To DataCntSort.Max 'DataCntSort(UBound(DataCntSort))
            For k = 0 To Me.InjuryChart.Series.Count - 1
                If DataCnt(k) >= i Then
                    TmpRowTxt = TmpRowTxt & CStr(Format(Me.InjuryChart.Series(k).Points(i + StartIndex(k)).XValue, "0.00####")) & _
                        Chr(9) & CStr(Format(Me.InjuryChart.Series(k).Points(i + StartIndex(k)).YValues(0), "0.00####")) & Chr(9)
                Else
                    TmpRowTxt = TmpRowTxt & "" & Chr(9) & "" & Chr(9)
                End If
            Next
            TmpDATAtxt = TmpDATAtxt & TmpRowTxt & vbCrLf
            TmpRowTxt = ""
        Next

        Clipboard.SetText(TmpDATAtxt)
    End Sub

    Private Sub IntervalTxt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles IntervalTxt.KeyDown

        If Me.ChkLst.SelectedIndex < 0 Then
            MsgBox("Select a Series First.", MsgBoxStyle.Information, "Series Selection")
            Exit Sub
        End If

        If e.KeyCode = Keys.Enter Then

            Dim i As Integer = 0
            Dim k As Integer = 0
            Dim inteval_Length As Double = CDbl(Me.IntervalTxt.Text)
            Dim TmpSeriesPoints As DataPointCollection = Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Points
            Dim DataPt As Integer = CInt(inteval_Length / (TmpSeriesPoints(1).XValue - TmpSeriesPoints(0).XValue)) ' + 1
            'Dim DeltaX As Double = Math.Abs(CDbl((TmpSeriesPoints(1).XValue - TmpSeriesPoints(0).XValue)))
            Dim TmpLocalSum As Double = 0.0
            Dim TmpLocalSumTmp As Double = 0.0
            Dim MaximaValue As Double = -1.0E+15
            Dim MaximaStartIndex As Integer = 0
            Dim MaximaEndIndex As Integer = 0
            Dim MinimaValue As Double = 1.0E+15
            Dim MinimaStartIndex As Integer = 0
            Dim MinimaEndIndex As Integer = 0

            If Me.ChkLst.SelectedIndex < 0 Or (IsNumeric(Me.IntervalTxt.Text) = False) Then
                MsgBox("Insert Numeric Value.", MsgBoxStyle.Information, "Try Again")
                Exit Sub
            Else

                Me.InjuryChart.ChartAreas(Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).ChartArea).AxisX.StripLines.Clear()

                For i = 0 To Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Points.Count - DataPt - 1

                    'For k = 1 To DataPt
                    '    TmpLocalSum = TmpLocalSum + _
                    '         (TmpSeriesPoints(i + k).XValue - TmpSeriesPoints(i + k - 1).XValue) / 6 _
                    '         * (TmpSeriesPoints(i + k - 1).YValues(0) + ((TmpSeriesPoints(i + k - 1).YValues(0) + TmpSeriesPoints(i + k).YValues(0)) * 2) + TmpSeriesPoints(i + k).YValues(0))
                    'Next

                    For k = 0 To DataPt - 1
                        'Summation
                        TmpLocalSum = TmpLocalSum + (TmpSeriesPoints(i + k).YValues(0) + TmpSeriesPoints(i + k + 1).YValues(0)) / 2
                    Next

                    If MaximaValue < TmpLocalSum Then
                        MaximaValue = TmpLocalSum
                        MaximaStartIndex = i
                        MaximaEndIndex = i + k
                    End If
                    If MinimaValue > TmpLocalSum Then
                        MinimaValue = TmpLocalSum
                        MinimaStartIndex = i
                        MinimaEndIndex = i + k
                    End If

                    TmpLocalSum = 0.0
                Next


            End If

            Dim el As New StripLine() ' Strip Line Add
            el.BorderColor = Color.DarkGray
            el.BackHatchStyle = ChartHatchStyle.DottedGrid
            el.BackColor = Color.LightGray
            'el.BackImageTransparentColor = Color.LightGray
            el.BackGradientStyle = GradientStyle.HorizontalCenter '배경
            el.BackHatchStyle = ChartHatchStyle.None
            el.StripWidth = inteval_Length '폭
            el.IntervalOffset = TmpSeriesPoints(MaximaStartIndex).XValue '시작위치
            el.Text = "Series : " & Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Name & vbCrLf & "■ Max : " & _
                Format(TmpSeriesPoints(MaximaStartIndex).XValue, "0.00###") & " ~ " & _
                Format(TmpSeriesPoints(MaximaEndIndex).XValue, "0.00###") & vbCrLf & _
                "■ Avg. : " & Format(MaximaValue / (DataPt), "0.00#") & vbCrLf & _
                "■ Integration : " & Format((MaximaValue / DataPt) * inteval_Length, "0.00#")
            el.TextOrientation = TextOrientation.Horizontal
            el.TextAlignment = StringAlignment.Near
            el.TextLineAlignment = StringAlignment.Far
            el.Font = New Font("맑은 고딕", 8, FontStyle.Bold)
            Me.InjuryChart.ChartAreas(Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).ChartArea).AxisX.StripLines.Add(el) '그래프에 추가
            'Me.InjuryChart.ChartAreas(Me.ChkLst.SelectedIndex + 1).AxisX.StripLines.Item(0).ToolTip = "■ Avg. : " & Format(MaximaValue / (DataPt), "0.00#") & vbCrLf & _
            '    "■ Integration : " & Format((MaximaValue / DataPt) * inteval_Length, "0.00#")

            Dim sl As New StripLine()
            sl.BorderColor = Color.DarkBlue
            sl.BackHatchStyle = ChartHatchStyle.DottedGrid
            sl.BackColor = Color.LightBlue
            sl.BackGradientStyle = GradientStyle.HorizontalCenter
            sl.BackHatchStyle = ChartHatchStyle.None
            sl.StripWidth = inteval_Length
            sl.IntervalOffset = TmpSeriesPoints(MinimaStartIndex).XValue
            sl.Text = "Series : " & Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Name & vbCrLf & "■ Min : " & _
                Format(TmpSeriesPoints(MinimaStartIndex).XValue, "0.00###") & " ~ " & _
                Format(TmpSeriesPoints(MinimaEndIndex).XValue, "0.00###") & vbCrLf & _
                "■ Avg. : " & Format(MinimaValue / DataPt, "0.00#") & vbCrLf & _
                "■ Integration : " & Format((MinimaValue / DataPt) * inteval_Length, "0.00#")
            sl.TextOrientation = TextOrientation.Horizontal
            sl.TextAlignment = StringAlignment.Near
            sl.Font = New Font("맑은 고딕", 8, FontStyle.Bold)
            Me.InjuryChart.ChartAreas(Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).ChartArea).AxisX.StripLines.Add(sl)
            'Me.InjuryChart.ChartAreas(Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).ChartArea).AxisX.StripLines.Item(1).ToolTip = "■ Avg. : " & Format(MinimaValue / DataPt, "0.00#") & vbCrLf & _
            '    "■ Integration : " & Format((MinimaValue / DataPt) * inteval_Length, "0.00#")

            'Dim MaxAnnotation As New VerticalLineAnnotation()
            'MaxAnnotation.AxisX = Me.InjuryChart.ChartAreas(0).AxisX
            'MaxAnnotation.AnchorX = TmpSeriesPoints(MaximaStartIndex).XValue
            'MaxAnnotation.IsInfinitive = True
            'MaxAnnotation.ClipToChartArea = Me.InjuryChart.ChartAreas(0).Name
            'MaxAnnotation.LineColor = Color.Red
            'MaxAnnotation.Visible = True
            'Me.InjuryChart.Annotations.Add(MaxAnnotation)

            Me.ChartContextMenu.Close()

        End If

    End Sub

    Private Sub TEMPToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TEMPToolStripMenuItem.Click

        Dim i As Integer = 0

        If Me.ChkLst.SelectedIndex < 0 Then
            MsgBox("Select a Reference Series", MsgBoxStyle.Information, "Try agin")
            Exit Sub
        End If

        If Me.InjuryChart.Series.Count < 2 Then
            MsgBox("At least two series are needed", MsgBoxStyle.Information, "Try agin")
            Exit Sub
        End If

        For i = 0 To Me.InjuryChart.Series.Count - 1
            If Me.InjuryChart.Series(i).Points.Count = 1 Then
                MsgBox("There are some EMPTY Series", MsgBoxStyle.Critical, "Try agin")
                Exit Sub
            End If
        Next

        '선택한 계열 (Reference가 된다.)
        Dim Ref_Selected As Integer = Me.ChkLst.SelectedIndex

        '가장 작은 끝점과 가장 큰 시작점을 찾는다.
        Dim FindStart As Double = -100.0
        Dim FindEnd As Double = 10000000.0

        For i = 0 To Me.InjuryChart.Series.Count - 1
            If FindStart <= Me.InjuryChart.Series(i).Points(0).XValue Then
                FindStart = Me.InjuryChart.Series(i).Points(0).XValue
            End If
            If FindEnd >= Me.InjuryChart.Series(i).Points(Me.InjuryChart.Series(i).Points.Count - 1).XValue Then
                FindEnd = Me.InjuryChart.Series(i).Points(Me.InjuryChart.Series(i).Points.Count - 1).XValue
            End If
        Next

        '데이터 간격을 설정 (Reference 기준)
        Dim Ref_Interval As Double = Math.Abs(Me.InjuryChart.Series(Ref_Selected).Points(0).XValue - Me.InjuryChart.Series(Ref_Selected).Points(1).XValue)
        If Math.Abs(Ref_Interval - 0.1) <= 0.01 Then
            Ref_Interval = 0.1
        ElseIf Math.Abs(Ref_Interval - 0.001) <= 0.0001 Then
            Ref_Interval = 0.001
        End If

        '그래프 범위가 조정되었을 경우 (더 작을 경우, 그 기준으로 한다)
        With Me.InjuryChart
            If FindStart < FindEnd Then
                If FindStart <= .ChartAreas(0).AxisX.Minimum Then FindStart = .ChartAreas(0).AxisX.Minimum
                If FindEnd >= .ChartAreas(0).AxisX.Maximum Then FindEnd = .ChartAreas(0).AxisX.Maximum
            End If
        End With

        '선택된 계열이 Reference 임
        '데이터를 일정 간격으로 샘플링 한다.
        Dim Data_Interval As Double = Ref_Interval
        Dim Sampled_Series_Data_Ref As Double()
        Sampled_Series_Data_Ref = LinearInterpolation(Me.InjuryChart, Me.ChkLst.SelectedIndex, FindStart, FindEnd, Data_Interval)
        '1: 보내는(차트)
        '2: 계열(Series) 번호
        '3: 시작값 (내부적으로 0.0인 점을 찾아서 시작값으로 쓴다.)
        '4: 끝값
        '5: 간격(Interval)
        '반환되는 것은 Y-array (Double 자료형)

        '나머지 계열들을 샘플링하면서 계산 한다.
        '샘플링 할 배열
        Dim Sampled_Series_Data As Double()
        '수치 저장 변수 선언
        Dim k As Integer = 0
        Dim DUC_Factor(Me.InjuryChart.Series.Count - 1) As Double
        Dim DUC_Relative(Me.InjuryChart.Series.Count - 1) As Double
        Dim WIFac(Me.InjuryChart.Series.Count - 1) As Double
        'Dim WIRel(Me.InjuryChart.Series.Count - 1) As Double
        Dim Tmp_DUC As Double()
        Dim SnG(Me.InjuryChart.Series.Count - 1) As Double
        Dim SnG_M(Me.InjuryChart.Series.Count - 1) As Double
        Dim SnG_P(Me.InjuryChart.Series.Count - 1) As Double
        Dim Tmp_SnG As Double()
        Dim GPV(Me.InjuryChart.Series.Count - 1) As Double
        Dim GPT(Me.InjuryChart.Series.Count - 1) As Double
        Dim Tmp_GPTV As Double()

        For i = 0 To Me.InjuryChart.Series.Count - 1
            If i <> Ref_Selected Then
                '샘플링된 데이터
                Sampled_Series_Data = LinearInterpolation(Me.InjuryChart, i, FindStart, FindEnd, Ref_Interval)

                '계산 시작
                'Difference area Under Curve (DUC)
                Tmp_DUC = FindDUC(Sampled_Series_Data_Ref, Sampled_Series_Data, Ref_Interval)
                'Index - 0 : Factor Method  
                'Index - 1 : Relative Method
                DUC_Factor(k) = Tmp_DUC(0)
                DUC_Relative(k) = Tmp_DUC(1)
                WIFac(k) = FindWIFac(Sampled_Series_Data_Ref, Sampled_Series_Data, Ref_Interval)
                'WIRel(k) = FindWIRel(Sampled_Series_Data_Ref, Sampled_Series_Data, Ref_Interval)
                Tmp_SnG = FindSnG(Sampled_Series_Data_Ref, Sampled_Series_Data, Ref_Interval)
                SnG(k) = Tmp_SnG(0)
                SnG_M(k) = Tmp_SnG(1)
                SnG_P(k) = Tmp_SnG(2)
                Tmp_GPTV = GPTV(Sampled_Series_Data_Ref, Sampled_Series_Data, Ref_Interval)
                GPV(k) = Tmp_GPTV(0)
                GPT(k) = Tmp_GPTV(1)

                k = k + 1
            Else
                'Reference는 넘어감
                k = k + 1
            End If
        Next

        Dim Tmp_SeriesNames(Me.ChkLst.Items.Count - 1) As String
        For i = 0 To Me.ChkLst.Items.Count - 1
            Tmp_SeriesNames(i) = Me.ChkLst.Items(i).ToString
        Next

        Dim ScoreBoardFrm As New FrmCorrel(DUC_Factor, DUC_Relative, WIFac, SnG, SnG_M, SnG_P, GPV, GPT, Ref_Selected, Tmp_SeriesNames)
        ScoreBoardFrm.Text = "Curve Correlation Score [Time Duration : " & FindStart & " ~ " & FindEnd & "]"
        ScoreBoardFrm.Show(Me)

    End Sub

    Private Sub CFC60ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CFC60ToolStripMenuItem.Click
        GraphCFCFilter(60)
    End Sub

    Private Sub GraphCFCFilter(ByVal CFC As Integer)

        If Me.ChkLst.SelectedIndex < 0 Then
            MsgBox("Select a Series First.", MsgBoxStyle.Information, "Series Selection")
            Exit Sub
        End If

        Dim i As Integer = 0
        Dim k As Integer = 0
        Dim TmpSeriesPoints As DataPointCollection = Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Points
        Dim a0 As Double = 0.0
        Dim a1 As Double = 0.0
        Dim a2 As Double = 0.0
        Dim b1 As Double = 0.0
        Dim b2 As Double = 0.0
        Dim Wd As Double = 0.0
        Dim Wa As Double = 0.0
        Dim THz As Double = 0.0001
        'Dim CFC As Double = 60

        Dim Xval(Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Points.Count - 1) As Double
        Dim FirstPhase(Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Points.Count - 1) As Double    'For Forward
        Dim SecondPhase(Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Points.Count - 1) As Double   'For Backward

        For i = 0 To Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Points.Count - 1
            Xval(i) = TmpSeriesPoints(i).XValue
        Next

        'CFC Constant=============================================================
        Wd = 2 * Math.PI * 2.0775 * CFC
        Wa = (Math.Sin((Wd * (THz / 2)))) / (Math.Cos(Wd * (THz / 2)))
        a0 = (Wa ^ 2) / (1 + Wa ^ 2 + Wa * Math.Sqrt(2))
        a1 = 2 * a0
        a2 = a0
        b1 = (-2 * (Wa ^ 2 - 1)) / (1 + Wa ^ 2 + Wa * Math.Sqrt(2))
        b2 = (-1 - Wa ^ 2 + Wa * Math.Sqrt(2)) / (1 + Wa ^ 2 + Wa * Math.Sqrt(2))
        'CFC Constant=============================================================

        'Forward Phase
        FirstPhase(0) = TmpSeriesPoints(0).YValues(0)
        FirstPhase(1) = TmpSeriesPoints(0).YValues(0)

        For i = 2 To Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Points.Count - 1
            FirstPhase(i) = a0 * TmpSeriesPoints(i).YValues(0) + _
                            a1 * TmpSeriesPoints(i - 1).YValues(0) + _
                            a2 * TmpSeriesPoints(i - 2).YValues(0) + _
                            b1 * FirstPhase(i - 1) + _
                            b2 * FirstPhase(i - 2)
        Next

        'BackwardPhase
        SecondPhase(UBound(SecondPhase)) = FirstPhase(UBound(FirstPhase))
        SecondPhase(UBound(SecondPhase) - 1) = FirstPhase(UBound(FirstPhase) - 1)

        For i = (UBound(FirstPhase) - 2) To 0 Step -1
            SecondPhase(i) = a0 * FirstPhase(i) + a1 * FirstPhase(i + 1) + a2 * FirstPhase(i + 2) + _
                            b1 * SecondPhase(i + 1) + b2 * SecondPhase(i + 2)
        Next

        Dim NewSeriesName As String = "CFC" & CFC & " Filtered" & " - " & Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Name

        Me.InjuryChart.Series.Add(NewSeriesName).Points.DataBindXY(Xval, SecondPhase)
        Me.InjuryChart.Series(NewSeriesName).ChartType = SeriesChartType.Line
        Me.InjuryChart.Series(NewSeriesName).ChartArea = Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).ChartArea

        'Add Check Box
        EvenPass = True
        Me.ChkLst.Items.Add(NewSeriesName)
        Me.ChkLst.SetItemCheckState(Me.ChkLst.Items.Count - 1, CheckState.Checked)
        EvenPass = False

        AnalCaseNum = AnalCaseNum + 1
        InjuryPopUp_Resize(Me, EventArgs.Empty)
        Me.ChkLst.SelectedIndex = Me.ChkLst.Items.Count - 1

    End Sub

    Private Sub CFC180ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CFC180ToolStripMenuItem.Click
        GraphCFCFilter(180)
    End Sub

    Private Sub CFC600ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CFC600ToolStripMenuItem.Click
        GraphCFCFilter(600)
    End Sub

    Private Sub CFC1000ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CFC1000ToolStripMenuItem.Click
        GraphCFCFilter(1000)
    End Sub

    Private Sub IntervalTxt2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles IntervalTxt2.KeyDown
        If Me.ChkLst.SelectedIndex < 0 Then
            MsgBox("Select a Series First.", MsgBoxStyle.Information, "Series Selection")
            Exit Sub
        End If

        If e.KeyCode = Keys.Enter Then

            Dim i As Integer = 0
            Dim k As Integer = 0
            Dim inteval_Length As Double = CDbl(Me.IntervalTxt2.Text)
            Dim TmpSeriesPoints As DataPointCollection = Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Points
            Dim DataPt As Integer = CInt(inteval_Length / (TmpSeriesPoints(1).XValue - TmpSeriesPoints(0).XValue))

            Dim Tmp_Val As Double

            Dim MaximaValue As Double = -1.0E+15
            Dim MaximaStartIndex As Integer = 0
            Dim MaximaEndIndex As Integer = 0
            Dim MinimaValue As Double = 1.0E+15
            Dim MinimaStartIndex As Integer = 0
            Dim MinimaEndIndex As Integer = 0

            If (IsNumeric(Me.IntervalTxt2.Text) = False) Then
                MsgBox("Insert Numeric Value.", MsgBoxStyle.Information, "Try Again")
                Exit Sub
            Else
                Me.InjuryChart.ChartAreas(Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).ChartArea).AxisX.StripLines.Clear()

                For i = 0 To Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Points.Count - 1 - DataPt

                    For k = 0 To DataPt

                        Tmp_Val = TmpSeriesPoints(i).YValues(0) - TmpSeriesPoints(i + k).YValues(0)

                        If MaximaValue < Tmp_Val Then
                            MaximaValue = Tmp_Val
                            MaximaStartIndex = i
                            MaximaEndIndex = i + DataPt
                        End If

                        If MinimaValue > Tmp_Val Then
                            MinimaValue = Tmp_Val
                            MinimaStartIndex = i
                            MinimaEndIndex = i + DataPt
                        End If

                    Next

                Next

                Tmp_Val = 0

            End If

            Dim el As New StripLine() ' Strip Line Add
            el.BorderColor = Color.DarkGray
            el.BackHatchStyle = ChartHatchStyle.DottedGrid
            el.BackColor = Color.LightGray
            'el.BackImageTransparentColor = Color.LightGray
            el.BackGradientStyle = GradientStyle.HorizontalCenter '배경
            el.BackHatchStyle = ChartHatchStyle.None
            el.StripWidth = inteval_Length '폭
            el.IntervalOffset = TmpSeriesPoints(MaximaStartIndex).XValue '시작위치
            Dim sl As New StripLine()
            sl.BorderColor = Color.DarkBlue
            sl.BackHatchStyle = ChartHatchStyle.DottedGrid
            sl.BackColor = Color.LightBlue
            sl.BackGradientStyle = GradientStyle.HorizontalCenter
            sl.BackHatchStyle = ChartHatchStyle.None
            sl.StripWidth = inteval_Length
            sl.IntervalOffset = TmpSeriesPoints(MinimaStartIndex).XValue

            sl.Text = "Series : " & Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Name & vbCrLf & _
                "■ Rise Interval : " & Format(TmpSeriesPoints(MinimaStartIndex).XValue, "0.00###") & " ~ " & _
                Format(TmpSeriesPoints(MinimaEndIndex).XValue, "0.00###") & vbCrLf & _
                "■ Rise amount : " & Format(Math.Abs(MinimaValue), "#0.00###")
            sl.TextOrientation = TextOrientation.Horizontal
            sl.TextAlignment = StringAlignment.Far
            sl.TextLineAlignment = StringAlignment.Far
            sl.Font = New Font("맑은 고딕", 8, FontStyle.Bold)
            sl.ForeColor = Color.Red
            Me.InjuryChart.ChartAreas(Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).ChartArea).AxisX.StripLines.Add(sl) '그래프에 추가

            el.Text = "Series : " & Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).Name & vbCrLf & _
                "■ Drop Interval : " & Format(TmpSeriesPoints(MaximaStartIndex).XValue, "0.00###") & " ~ " & _
                Format(TmpSeriesPoints(MaximaEndIndex).XValue, "0.00###") & vbCrLf & _
                "■ Drop amount : " & Format(Math.Abs(MaximaValue), "#0.00###")
            el.TextOrientation = TextOrientation.Horizontal
            el.TextAlignment = StringAlignment.Near
            el.TextLineAlignment = StringAlignment.Near
            el.Font = New Font("맑은 고딕", 8, FontStyle.Bold)
            el.ForeColor = Color.Blue
            Me.InjuryChart.ChartAreas(Me.InjuryChart.Series(Me.ChkLst.SelectedIndex).ChartArea).AxisX.StripLines.Add(el) '그래프에 추가

            Me.ChartContextMenu.Close()

        End If
    End Sub

    Private Sub ChkSize_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkSize.CheckedChanged
        If Me.ChkSize.Checked = True Then
            FormerWidth = Me.Width
            FormerHeight = Me.Height

            Me.Size = New Size(PopUpWidth, PopUpHeight)
        Else
            Me.Width = FormerWidth
            Me.Height = FormerHeight
        End If
    End Sub

    Private Sub AddAdditionalChartToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddAdditionalChartToolStripMenuItem.Click

        Dim i As Integer = 0

        With Me.InjuryChart

            .ChartAreas.Add(.ChartAreas.Count)

            .ChartAreas(.ChartAreas(.ChartAreas.Count - 1).Name).AxisY.Enabled = AxisEnabled.True
            .ChartAreas(.ChartAreas(.ChartAreas.Count - 1).Name).AxisY.MinorGrid.Enabled = False
            .ChartAreas(.ChartAreas(.ChartAreas.Count - 1).Name).AxisY.MajorGrid.Enabled = True
            .ChartAreas(.ChartAreas(.ChartAreas.Count - 1).Name).AxisY.MajorTickMark.Enabled = True
            .ChartAreas(.ChartAreas(.ChartAreas.Count - 1).Name).AxisY.LabelStyle.Font = New Font("Arial", 10, FontStyle.Bold)

            DATAGraphDP.GraphAxisLine(Me.InjuryChart)
            DATAGraphDP.ScrollnZoom(Me.InjuryChart)
            DATAGraphDP.SeriesValShow(Me.InjuryChart)

            For i = 0 To .Legends.Count - 1
                .Legends(0).Docking = Docking.Bottom
            Next

            For i = 1 To .ChartAreas.Count - 1
                '차트 정렬
                .ChartAreas(i).AlignWithChartArea = .ChartAreas(0).Name
                If .ChartAreas.Count < 4 Then
                    .ChartAreas(i).AlignmentStyle = AreaAlignmentStyles.PlotPosition
                Else
                    .ChartAreas(i).AlignmentOrientation = AreaAlignmentOrientations.None
                End If
                '.ChartAreas(i).AlignmentStyle = AreaAlignmentStyles.Cursor
                '.ChartAreas(i).AlignmentStyle = AreaAlignmentStyles.AxesView

                'X축 Y축 글꼴설정
                .ChartAreas(i).AxisX.LabelStyle.Font = New Font("Arial", 10, FontStyle.Bold)
                .ChartAreas(i).AxisY.LabelStyle.Font = New Font("Arial", 10, FontStyle.Bold)
                .ChartAreas(i).AxisX.TitleFont = New Font("맑은 고딕", 11, FontStyle.Bold)
                .ChartAreas(i).AxisY.TitleFont = New Font("맑은 고딕", 11, FontStyle.Bold)
                .ChartAreas(i).AxisY2.TitleFont = New Font("맑은 고딕", 11, FontStyle.Bold)

                ''원래 차트와 같은 범위를 가지도록 설정 (X축만)
                .ChartAreas(i).AxisX.Minimum = Tmp_Chart.ChartAreas(0).AxisX.Minimum
                .ChartAreas(i).AxisX.Maximum = Tmp_Chart.ChartAreas(0).AxisX.Maximum
                .ChartAreas(i).AxisX.MajorGrid.Interval = Tmp_Chart.ChartAreas(0).AxisX.MajorGrid.Interval
            Next

            'X축 Y축 글꼴설정
            .ChartAreas(0).AxisX.LabelStyle.Font = New Font("Arial", 10, FontStyle.Bold)
            .ChartAreas(0).AxisY.LabelStyle.Font = New Font("Arial", 10, FontStyle.Bold)
            .ChartAreas(0).AxisX.TitleFont = New Font("맑은 고딕", 11, FontStyle.Bold)
            .ChartAreas(0).AxisY.TitleFont = New Font("맑은 고딕", 11, FontStyle.Bold)
            .ChartAreas(0).AxisY2.TitleFont = New Font("맑은 고딕", 11, FontStyle.Bold)

            Me.CmbChartArea.Items.Add("#" & .ChartAreas(.ChartAreas.Count - 1).Name & " Add Chart")
            Me.CmbChrtAreaTitle.Items.Add("#" & .ChartAreas(.ChartAreas.Count - 1).Name & " Add Chart")
            Me.CmbChrtAreaTitle.SelectedIndex = 0

        End With

        If Me.InjuryChart.ChartAreas.Count = 2 Then
            Me.Height = Me.Height * 2
        End If

    End Sub

    Private Sub ClearAdditionalChartToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearAdditionalChartToolStripMenuItem.Click

        If Me.InjuryChart.ChartAreas.Count = 1 Then Exit Sub

        Dim i As Integer = 0

        With Me
            For i = .InjuryChart.ChartAreas.Count - 1 To 1 Step -1
                .InjuryChart.ChartAreas.RemoveAt(i)
            Next
            For i = .CmbChartArea.Items.Count - 1 To 0 Step -1
                .CmbChartArea.Items.RemoveAt(i)
            Next
            .CmbChartArea.Text = ""

            '모두 원복
            For i = 0 To .InjuryChart.Series.Count - 1
                .InjuryChart.Series(i).ChartArea = .InjuryChart.ChartAreas(0).Name
            Next
        End With

        Me.Height = Me.Height / 2

    End Sub

    Private Sub CmbChartArea_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles CmbChartArea.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            If Me.InjuryChart.ChartAreas.Count = 1 Then Exit Sub

            With Me
                If .InjuryChart.Series(.ChkLst.SelectedIndex).YAxisType = AxisType.Secondary Then
                    .InjuryChart.ChartAreas(.InjuryChart.ChartAreas(.CmbChartArea.SelectedIndex + 1).Name).AxisY2.Enabled = AxisEnabled.Auto
                    .InjuryChart.ChartAreas(.InjuryChart.ChartAreas(.CmbChartArea.SelectedIndex + 1).Name).AxisY2.MajorGrid.Interval = Double.NaN
                    .InjuryChart.ChartAreas(.InjuryChart.ChartAreas(.CmbChartArea.SelectedIndex + 1).Name).AxisY2.Maximum = Double.NaN
                    .InjuryChart.ChartAreas(.InjuryChart.ChartAreas(.CmbChartArea.SelectedIndex + 1).Name).AxisY2.Minimum = Double.NaN
                    .InjuryChart.ChartAreas(.InjuryChart.ChartAreas(.CmbChartArea.SelectedIndex + 1).Name).AxisY2.MinorGrid.Enabled = False
                    .InjuryChart.ChartAreas(.InjuryChart.ChartAreas(.CmbChartArea.SelectedIndex + 1).Name).AxisY2.MajorGrid.Enabled = False
                    .InjuryChart.ChartAreas(.InjuryChart.ChartAreas(.CmbChartArea.SelectedIndex + 1).Name).AxisY2.MajorTickMark.Enabled = True
                    .InjuryChart.ChartAreas(.InjuryChart.ChartAreas(.CmbChartArea.SelectedIndex + 1).Name).AxisY2.LabelStyle.Font = New Font("Arial", 10, FontStyle.Bold)
                End If
                .InjuryChart.Series(.ChkLst.SelectedIndex).ChartArea = .InjuryChart.ChartAreas(.CmbChartArea.SelectedIndex + 1).Name
                .InjuryChart.ChartAreas(.InjuryChart.ChartAreas(.CmbChartArea.SelectedIndex + 1).Name).RecalculateAxesScale()
            End With

            Me.ListBoxMenu.Close()

        End If

    End Sub

    Private Sub MoveItemAxisYToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveItemAxisYToolStripMenuItem.Click
        If Me.ChkLst.SelectedIndex >= 0 Then
            With Me
                If .InjuryChart.Series(.ChkLst.SelectedIndex).YAxisType = AxisType.Primary Then
                    .InjuryChart.Series(.ChkLst.SelectedIndex).YAxisType = AxisType.Secondary
                    .InjuryChart.ChartAreas(.InjuryChart.Series(.ChkLst.SelectedIndex).ChartArea).AxisY2.Enabled = AxisEnabled.Auto
                    .InjuryChart.ChartAreas(.InjuryChart.Series(.ChkLst.SelectedIndex).ChartArea).AxisY2.MajorGrid.Interval = Double.NaN
                    .InjuryChart.ChartAreas(.InjuryChart.Series(.ChkLst.SelectedIndex).ChartArea).AxisY2.Minimum = Double.NaN
                    .InjuryChart.ChartAreas(.InjuryChart.Series(.ChkLst.SelectedIndex).ChartArea).AxisY2.Minimum = Double.NaN
                    .InjuryChart.ChartAreas(.InjuryChart.Series(.ChkLst.SelectedIndex).ChartArea).AxisY2.MinorGrid.Enabled = False
                    .InjuryChart.ChartAreas(.InjuryChart.Series(.ChkLst.SelectedIndex).ChartArea).AxisY2.MajorGrid.Enabled = False
                    .InjuryChart.ChartAreas(.InjuryChart.Series(.ChkLst.SelectedIndex).ChartArea).AxisY2.MajorTickMark.Enabled = True
                    .InjuryChart.ChartAreas(.InjuryChart.Series(.ChkLst.SelectedIndex).ChartArea).AxisY2.LabelStyle.Font = New Font("Arial", 10, FontStyle.Bold)
                    .InjuryChart.ChartAreas(.InjuryChart.Series(.ChkLst.SelectedIndex).ChartArea).RecalculateAxesScale()
                Else
                    .InjuryChart.Series(.ChkLst.SelectedIndex).YAxisType = AxisType.Primary
                End If
            End With
        End If
    End Sub

    Private Sub To1stChartToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles To1stChartToolStripMenuItem.Click
        If Me.InjuryChart.ChartAreas.Count = 1 Then Exit Sub
        If Me.ChkLst.SelectedIndex >= 0 Then
            With Me
                .InjuryChart.Series(.ChkLst.SelectedIndex).ChartArea = .InjuryChart.ChartAreas(0).Name
            End With
        End If
    End Sub

    Private Sub CmbChartArea_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CmbChartArea.MouseDown

        If e.Button = Windows.Forms.MouseButtons.Right Then

            If Me.InjuryChart.ChartAreas.Count = 1 Then Exit Sub

            With Me
                If .InjuryChart.Series(.ChkLst.SelectedIndex).YAxisType = AxisType.Secondary Then
                    .InjuryChart.ChartAreas(.InjuryChart.ChartAreas(.CmbChartArea.SelectedIndex + 1).Name).AxisY2.Enabled = AxisEnabled.Auto
                    .InjuryChart.ChartAreas(.InjuryChart.ChartAreas(.CmbChartArea.SelectedIndex + 1).Name).AxisY2.MajorGrid.Interval = Double.NaN
                    .InjuryChart.ChartAreas(.InjuryChart.ChartAreas(.CmbChartArea.SelectedIndex + 1).Name).AxisY2.Maximum = Double.NaN
                    .InjuryChart.ChartAreas(.InjuryChart.ChartAreas(.CmbChartArea.SelectedIndex + 1).Name).AxisY2.Minimum = Double.NaN
                    .InjuryChart.ChartAreas(.InjuryChart.ChartAreas(.CmbChartArea.SelectedIndex + 1).Name).AxisY2.MinorGrid.Enabled = False
                    .InjuryChart.ChartAreas(.InjuryChart.ChartAreas(.CmbChartArea.SelectedIndex + 1).Name).AxisY2.MajorGrid.Enabled = False
                    .InjuryChart.ChartAreas(.InjuryChart.ChartAreas(.CmbChartArea.SelectedIndex + 1).Name).AxisY2.MajorTickMark.Enabled = True
                    .InjuryChart.ChartAreas(.InjuryChart.ChartAreas(.CmbChartArea.SelectedIndex + 1).Name).AxisY2.LabelStyle.Font = New Font("Arial", 10, FontStyle.Bold)
                End If
                .InjuryChart.Series(.ChkLst.SelectedIndex).ChartArea = .InjuryChart.ChartAreas(.CmbChartArea.SelectedIndex + 1).Name
                .InjuryChart.ChartAreas(.InjuryChart.ChartAreas(.CmbChartArea.SelectedIndex + 1).Name).RecalculateAxesScale()
            End With

            Me.ListBoxMenu.Close()

        End If
    End Sub

    Private Sub CmbChrtAreaTitle_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbChrtAreaTitle.SelectedIndexChanged

        With Me
            Dim k As Integer = Me.CmbChrtAreaTitle.SelectedIndex


            'If .InjuryChart.Titles(k).Text <> "" Then
            If k = 0 Then
                .ChartTitleTxt.Text = .InjuryChart.Titles(0).Text
                .ChartTitleTxt.Enabled = True
            Else
                .ChartTitleTxt.Text = ""
                .ChartTitleTxt.Enabled = False
            End If
            'End If
            If .InjuryChart.ChartAreas(k).AxisX.Title <> "" Then .XaxitTitleTxt.Text = .InjuryChart.ChartAreas(k).AxisX.Title
            If .InjuryChart.ChartAreas(k).AxisY.Title <> "" Then .YaxisTitleTxt.Text = .InjuryChart.ChartAreas(k).AxisY.Title
            If .InjuryChart.ChartAreas(k).AxisY2.Title <> "" Then .Yaxis2TitleTxt.Text = .InjuryChart.ChartAreas(k).AxisY2.Title
        End With

    End Sub

    Private Function SeriesNamingChrt(ByVal Name As String, ByRef Chrt As Chart) As String

        Dim Temp As String = ""
        'Dictionary에 같은 이름이 있을경우 새로운 이름을 만들어 준다
        Dim IsSame As Boolean = False
        If Chrt.Series.Count = 0 Then Return Name

        For i = 0 To Chrt.Series.Count - 1
            If Chrt.Series(i).Name = Name Then
                IsSame = True
            End If
        Next

        If IsSame = True Then
            Dim SeriesIden As String = ""
            SeriesIden = InputBox(" Same name is detected in chart." & vbCrLf & _
                                 " Insert Identifier" & vbCrLf & _
                                 "  - Identifier will be attached to the end of series name" & vbCrLf & _
                                 "     [Series Name]_[Idetifier]", "Insert Title", "Re")
            Name = Name & "_" & SeriesIden
            Temp = SeriesNamingChrt(Name, Chrt)
            Return Temp
        Else
            Return Name
        End If


    End Function

End Class