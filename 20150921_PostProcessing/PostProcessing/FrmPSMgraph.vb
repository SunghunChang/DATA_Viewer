Imports System.Windows.Forms.DataVisualization.Charting

Public Class FrmPSMgraph

    Dim T_data() As Single
    Dim GraphConfigure As Boolean
    Dim X_Anal() As Double
    Dim Y_Anal() As Double
    Dim Z_Anal() As Double
    Dim X_Test() As Double
    Dim Y_Test() As Double
    Dim Z_Test() As Double

    Public Sub New(ByVal IsTrans As Boolean, ByVal Times() As Single, ByVal X_Ref_Disp() As Double, ByVal Y_Ref_Disp() As Double, ByVal Z_Ref_Disp() As Double, ByVal X_TEST_Disp() As Double, ByVal Y_TEST_Disp() As Double, ByVal Z_TEST_Disp() As Double)

        ' 이 호출은 디자이너에 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하십시오.

        GraphConfigure = IsTrans

        T_data = Times
        X_Anal = X_Ref_Disp
        Y_Anal = Y_Ref_Disp
        Z_Anal = Z_Ref_Disp
        X_Test = X_TEST_Disp
        Y_Test = Y_TEST_Disp
        Z_Test = Z_TEST_Disp
    End Sub

    Private Sub FrmPSMgraph_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If GraphConfigure = False Then
            With Me
                .LstRefAnal.Items.RemoveAt(2)
                .LstRefTEST.Items.RemoveAt(2)
                .LstRefAnal.Items.RemoveAt(1)
                .LstRefTEST.Items.RemoveAt(1)
                .LstRefAnal.Items.RemoveAt(0)
                .LstRefTEST.Items.RemoveAt(0)
                .LstRefAnal.Items.Add("Yawing")
                .LstRefAnal.Items.Add("Pitching")
                .LstRefTEST.Items.Add("TEST Yawing")
                .LstRefTEST.Items.Add("TEST Pitching")
            End With
        End If

        With Me.ChrtDATA
            .Series.Add("Analysis")
            .Series.Add("TEST")
            .Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Spline
            .Series(1).ChartType = DataVisualization.Charting.SeriesChartType.Spline
            .Series(0).BorderWidth = 2
            .Series(0).BorderWidth = 2
            .ChartAreas(0).AxisX.MajorGrid.LineColor = Color.LightGray
            .ChartAreas(0).AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash
            .ChartAreas(0).AxisX.MajorGrid.LineWidth = 1
            .ChartAreas(0).AxisY.MajorGrid.LineColor = Color.LightGray
            .ChartAreas(0).AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash
            .ChartAreas(0).AxisY.MajorGrid.LineWidth = 1
            .ChartAreas(0).AxisX.MajorTickMark.Interval = 0.01
            .ChartAreas(0).AxisX.MinorTickMark.Interval = 0.005
            .ChartAreas(0).AxisX.LabelStyle.Font = New Font("Arial", 10, FontStyle.Bold)
            .ChartAreas(0).AxisY.LabelStyle.Font = New Font("Arial", 10, FontStyle.Bold)
            .ChartAreas(0).AxisX.LabelStyle.Format = "0.000"
            .ChartAreas(0).AxisX.Minimum = 0.0
            .Series(0).ToolTip = "#VALX : #VAL"
            .Series(1).ToolTip = "#VALX : #VAL"
            .Series(0).MarkerStyle = MarkerStyle.Circle
            .Series(1).MarkerSize = 6
        End With
    End Sub

    Private Sub LstRefAnal_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstRefAnal.SelectedIndexChanged
        Select Case Me.LstRefAnal.SelectedIndex
            Case 0
                Try
                    Me.ChrtDATA.Series("Analysis").Points.DataBindXY(T_data, X_Anal)
                Catch ex As Exception

                End Try
            Case 1
                Try
                    Me.ChrtDATA.Series("Analysis").Points.DataBindXY(T_data, Y_Anal)
                Catch ex As Exception

                End Try
            Case 2
                Try
                    Me.ChrtDATA.Series("Analysis").Points.DataBindXY(T_data, Z_Anal)
                Catch ex As Exception

                End Try
        End Select
    End Sub

    Private Sub LstRefTEST_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstRefTEST.SelectedIndexChanged
        Select Case Me.LstRefTEST.SelectedIndex
            Case 0
                Try
                    Me.ChrtDATA.Series("TEST").Points.DataBindXY(T_data, X_Test)
                Catch ex As Exception

                End Try
            Case 1
                Try
                    Me.ChrtDATA.Series("TEST").Points.DataBindXY(T_data, Y_Test)
                Catch ex As Exception

                End Try
            Case 2
                Try
                    Me.ChrtDATA.Series("TEST").Points.DataBindXY(T_data, Z_Test)
                Catch ex As Exception

                End Try
        End Select
    End Sub
End Class