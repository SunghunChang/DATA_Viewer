Imports System.Windows.Forms.DataVisualization.Charting

Public Class GraphClass
    '상해값 그래프에서 쓰임 (Injury Valus)
    Public Overridable Sub GraphAxisLine(ByRef Charts As Windows.Forms.DataVisualization.Charting.Chart)

        Dim i As Integer

        '일반적 셋팅
        With Charts
            'For i = 1 To .Series.Count Step 1
            '    .Series(i - 1).ChartArea = "Default"
            'Next
            '.InjuryValChrt.Series("---").ChartType = DataVisualization.Charting.SeriesChartType.Line

            .TextAntiAliasingQuality = TextAntiAliasingQuality.SystemDefault
            '.BorderDashStyle = ChartDashStyle.Solid
            '.BorderWidth = 2
            .ChartAreas(0).AxisX.MajorGrid.LineColor = Color.LightGray
            .ChartAreas(0).AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash
            .ChartAreas(0).AxisX.MajorGrid.LineWidth = 1
            .ChartAreas(0).AxisY.MajorGrid.LineColor = Color.LightGray
            .ChartAreas(0).AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash
            .ChartAreas(0).AxisY.MajorGrid.LineWidth = 1
            .ChartAreas(0).AxisX.MajorTickMark.Interval = 0.5
            .ChartAreas(0).AxisX.MinorTickMark.Interval = 0.1
            '.ChartAreas(0).AxisY.MajorTickMark.Interval = 0.5
            .ChartAreas(0).AxisX.LabelStyle.Font = New Font("Arial", 11, FontStyle.Bold)
            .ChartAreas(0).AxisY.LabelStyle.Font = New Font("Arial", 11, FontStyle.Bold)

            'legend
            .Legends(0).LegendStyle = LegendStyle.Table
            .Legends(0).Docking = Docking.Bottom
            .Legends(0).BorderColor = Color.Gray
            .Legends(0).BorderWidth = 2
            .Legends(0).BorderDashStyle = ChartDashStyle.Solid
            .Legends(0).ShadowOffset = 2
            '.Legends(0).Position.X = 20
            '.Legends(0).Position.Y = 20
            .Legends(0).Font = New Font("Arial", 10, FontStyle.Bold)

            For i = 1 To .Series.Count Step 1
                .Series(i - 1).YValueType = ChartValueType.Single
                .Series(i - 1).SmartLabelStyle.Enabled = True

                '라벨표시 or 툴팁 : 둘 중 하나만 쓰자
                '.Series(i - 1).IsValueShownAsLabel = True
                .Series(i - 1).ToolTip = "#VALX : #VAL"
            Next

        End With
    End Sub

    Public Sub ScrollnZoom(ByRef Charts As Windows.Forms.DataVisualization.Charting.Chart)

        Dim k As Integer = 0

        With Charts
            For k = 0 To Charts.ChartAreas.Count - 1
                ' Scrollbars position
                .ChartAreas(k).AxisX.ScaleView.Zoomable = True
                .ChartAreas(0).AxisX.ScrollBar.IsPositionedInside = False
                .ChartAreas(0).AxisY.ScaleView.Zoomable = True
                .ChartAreas(0).AxisY.ScrollBar.IsPositionedInside = False

                ' Enable range selection and zooming end user interface
                .ChartAreas(k).CursorX.IsUserEnabled = True
                .ChartAreas(k).CursorX.IsUserSelectionEnabled = True
                .ChartAreas(k).CursorX.LineColor = Color.Transparent
                .ChartAreas(k).AxisX.ScaleView.MinSize = 0.01

                .ChartAreas(k).CursorY.IsUserEnabled = True
                .ChartAreas(k).CursorY.IsUserSelectionEnabled = True
                .ChartAreas(k).CursorY.LineColor = Color.Transparent
                .ChartAreas(k).AxisY.ScaleView.MinSize = 0.01

                ' Set scrollbar size
                .ChartAreas(k).AxisX.ScrollBar.Size = 15
                .ChartAreas(k).AxisY.ScrollBar.Size = 15

                ' Show small scroll buttons only
                .ChartAreas(k).AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All
                .ChartAreas(k).AxisY.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All

                ' Change scrollbar colors
                .ChartAreas(k).AxisX.ScrollBar.BackColor = Color.LightGray
                .ChartAreas(k).AxisX.ScrollBar.ButtonColor = Color.Gray
                .ChartAreas(k).AxisX.ScrollBar.LineColor = Color.Gray
                .ChartAreas(k).AxisY.ScrollBar.BackColor = Color.LightGray
                .ChartAreas(k).AxisY.ScrollBar.ButtonColor = Color.Gray
                .ChartAreas(k).AxisY.ScrollBar.LineColor = Color.Gray

                .ChartAreas(k).AxisX.ScaleView.SmallScrollSize = Double.NaN
                .ChartAreas(k).AxisX.ScaleView.SmallScrollMinSize = 0.01
                .ChartAreas(k).AxisY.ScaleView.SmallScrollSize = Double.NaN
                .ChartAreas(k).AxisY.ScaleView.SmallScrollMinSize = 0.01
            Next
            
        End With
    End Sub
End Class

Public Class DPInjuryVal
    '상해값 그래프에서 쓰임 (Injury Valus)
    Inherits GraphClass

    Public Sub AxisSetting(ByRef Charts As Windows.Forms.DataVisualization.Charting.Chart)

        Dim i As Integer

        '마커를 표시하고 선을 굵게 표시한다.
        With Charts
            For i = 1 To .Series.Count Step 1
                .Series(i - 1).MarkerStyle = MarkerStyle.Circle
                .Series(i - 1).MarkerSize = 8
                .Series(i - 1).YValueType = ChartValueType.Single
                .Series(i - 1).SmartLabelStyle.Enabled = True
                .Series(i - 1).BorderWidth = 3

                '라벨표시 or 툴팁 : 둘 중 하나만 쓰자
                '.Series(i - 1).IsValueShownAsLabel = True
                .Series(i - 1).ToolTip = "#VALX : #VAL"
            Next
        End With
    End Sub
End Class

Public Class DATA_Graphing
    '해석 데이터 그래프에서 쓰임 (32개 그래프)
    Inherits GraphClass
    Public Overrides Sub GraphAxisLine(ByRef Charts As System.Windows.Forms.DataVisualization.Charting.Chart)
        'MyBase.GraphAxisLine(Charts)
        Dim k As Integer = 0

        With Charts

            For k = 0 To .ChartAreas.Count - 1
                '.InjuryValChrt.Series("---").ChartType = DataVisualization.Charting.SeriesChartType.Line
                .TextAntiAliasingQuality = TextAntiAliasingQuality.SystemDefault
                .ChartAreas(k).AxisX.MajorGrid.LineColor = Color.LightGray
                .ChartAreas(k).AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash
                .ChartAreas(k).AxisX.MajorGrid.LineWidth = 1
                .ChartAreas(k).AxisY.MajorGrid.LineColor = Color.LightGray
                .ChartAreas(k).AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash
                .ChartAreas(k).AxisY.MajorGrid.LineWidth = 1

                .ChartAreas(k).AxisX.IsMarksNextToAxis = True

                .ChartAreas(k).AxisX.IsLabelAutoFit = True

                'Label 간격
                .ChartAreas(k).AxisX.Interval = 20
                '.ChartAreas(k).AxisX.IntervalOffset = 0.1
                .ChartAreas(k).AxisX.Minimum = 0.0

                'Label Format
                .ChartAreas(k).AxisX.LabelStyle.Font = New Font("Arial", 9, FontStyle.Bold)
                .ChartAreas(k).AxisY.LabelStyle.Font = New Font("Arial", 9, FontStyle.Bold)
                '.ChartAreas(k).AxisX.LabelStyle.Format = "#.0#"

                'Tick Marker 간격
                .ChartAreas(k).AxisX.MajorTickMark.Interval = 20
                .ChartAreas(k).AxisX.MinorTickMark.Interval = 10
                '.ChartAreas(k).AxisX.MajorTickMark.IntervalOffset = 1            
            Next

            For k = 0 To .Legends.Count - 1
                'Legend 설정
                .Legends(k).IsDockedInsideChartArea = False  '범례가 밖에 존재한다.
                .Legends(k).LegendStyle = LegendStyle.Table
                .Legends(k).Docking = Docking.Bottom
                .Legends(k).BorderColor = Color.Gray
                .Legends(k).BorderWidth = 1
                .Legends(k).BorderDashStyle = ChartDashStyle.Solid
                .Legends(k).ShadowOffset = 2
                .Legends(k).Font = New Font("Arial", 8)
            Next

            For i = 1 To .Series.Count Step 1
                .Series(i - 1).ChartType = DataVisualization.Charting.SeriesChartType.Line
            Next
        End With
    End Sub

    Public Sub SeriesValShow(ByRef Charts As System.Windows.Forms.DataVisualization.Charting.Chart)
        '차트에 마우스 커서를 놓으면 값을 보여준다.
        Dim i As Integer

        With Charts
            For i = 1 To .Series.Count Step 1
                .Series(i - 1).YValueType = ChartValueType.Single
                .Series(i - 1).ToolTip = "■ X Value : #VALX / ■ Y Value : #VAL"
            Next
        End With
    End Sub

    Public Sub GradientChart(ByRef Charts As System.Windows.Forms.DataVisualization.Charting.Chart)
        With Charts
            If .BackColor = Color.LightGray Then
                .BackColor = Color.White
                .BackGradientStyle = GradientStyle.None
                .ChartAreas(0).BackColor = Color.Transparent
            Else
                .BackColor = Color.LightGray
                .BackGradientStyle = GradientStyle.TopBottom
                .ChartAreas(0).BackColor = Color.Transparent
            End If
        End With
    End Sub
End Class

Public Class TEST_Graphing
    Inherits GraphClass
    Public Overrides Sub GraphAxisLine(ByRef Charts As Chart)
        With Charts

            '모눈 스타일 설정
            .TextAntiAliasingQuality = TextAntiAliasingQuality.SystemDefault
            .ChartAreas(0).AxisX.MajorGrid.LineColor = Color.LightGray
            .ChartAreas(0).AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash
            .ChartAreas(0).AxisX.MajorGrid.LineWidth = 1
            .ChartAreas(0).AxisY.MajorGrid.LineColor = Color.LightGray
            .ChartAreas(0).AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash
            .ChartAreas(0).AxisY.MajorGrid.LineWidth = 1

            .ChartAreas(0).AxisX.IsMarksNextToAxis = True

            .ChartAreas(0).AxisX.IsLabelAutoFit = True

            'Label Format (X-Y 그래프 라벨 글꼴)
            .ChartAreas(0).AxisX.LabelStyle.Font = New Font("Arial", 9, FontStyle.Bold)
            .ChartAreas(0).AxisY.LabelStyle.Font = New Font("Arial", 9, FontStyle.Bold)

            '범례 아래로
            .Legends(0).Docking = Docking.Bottom

            'Legend 설정
            .Legends(0).LegendStyle = LegendStyle.Table
            .Legends(0).BorderColor = Color.Gray
            .Legends(0).BorderWidth = 1
            .Legends(0).BorderDashStyle = ChartDashStyle.Solid
            .Legends(0).ShadowOffset = 2
            .Legends(0).Font = New Font("Arial", 8, FontStyle.Bold)

            'Y축 최대/최소값 자동 설정
            .ChartAreas(0).AxisY.Minimum = Double.NaN
            .ChartAreas(0).AxisY.Maximum = Double.NaN
        End With
    End Sub

    Public Sub SeriesValShow(ByRef Charts As System.Windows.Forms.DataVisualization.Charting.Chart)
        '차트에 마우스 커서를 놓으면 값을 보여준다.
        Dim i As Integer

        With Charts
            For i = 1 To .Series.Count Step 1
                .Series(i - 1).YValueType = ChartValueType.Single
                .Series(i - 1).ToolTip = "■ X Value : #VALX / ■ Y Value : #VAL"
            Next
        End With
    End Sub

    Public Sub HideLegends(ByRef Charts As Chart)
        Charts.Legends(0).Enabled = False
    End Sub
    Public Sub ShowLegends(ByRef Charts As Chart)
        Charts.Legends(0).Enabled = True
    End Sub
    Public Sub TopLegends(ByRef Charts As Chart)
        Charts.Legends(0).Docking = Docking.Top
    End Sub
    Public Sub BottomLegends(ByRef Charts As Chart)
        Charts.Legends(0).Docking = Docking.Bottom
    End Sub
    Public Sub LeftLegends(ByRef Charts As Chart)
        Charts.Legends(0).Docking = Docking.Left
    End Sub
    Public Sub RightLegends(ByRef Charts As Chart)
        Charts.Legends(0).Docking = Docking.Right
    End Sub
    Public Sub InSideLegends(ByRef Charts As Chart)
        Charts.Legends(0).IsDockedInsideChartArea = False
    End Sub
    Public Sub OutSideLegends(ByRef Charts As Chart)
        Charts.Legends(0).IsDockedInsideChartArea = True
        Charts.Legends(0).DockedToChartArea = Charts.ChartAreas(0).Name
    End Sub
End Class

