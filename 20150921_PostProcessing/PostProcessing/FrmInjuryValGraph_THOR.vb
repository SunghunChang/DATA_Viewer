Imports System.Windows.Forms.DataVisualization.Charting

Public Class FrmInjuryValGraph_THOR

    '라벨 표시를 나타태고 종료하는 것
    Public DATAlabelindex_THOR As Integer

    Public Initailize_Index_THOR As Integer

    Public SubHead3MSG_THOR() As Double     'H3MS_inj
    Public SubChest_G_CUMULATIVE_T3MS_inj_THOR() As Double      'T3MS_inj
    Public SubHIC15_THOR() As Double        'HIC15_inj
    Public SubHIC36_THOR() As Double        'HIC36_inj
    Public SubNTE_THOR() As Double      'NTE_inj
    Public SubNTF_THOR() As Double      'NTF_inj
    Public SubNCE_THOR() As Double      'NCE_inj
    Public SubNCF_THOR() As Double      'NCF_inj
    Public SubHead_Peak_G_THOR() As Double      'HaccRpeak_inj
    Public SubChest_D_THOR() As Double      'ThCC_inj--------------------->Average Value
    Public SubKneeS_L_THOR() As Double      'kneesliderL_inj
    Public SubKneeS_R_THOR() As Double      'kneesliderR_inj
    Public SubTibia_Comp_L_THOR() As Double     'TCFCLowL_inj
    Public SubTibia_Comp_R_THOR() As Double     'TCFCLowR_inj
    Public SubTI_upr_L_THOR() As Double     'TIUpL_inj
    Public SubTI_lwr_L_THOR() As Double     'TILowL_inj
    Public SubTI_upr_R_THOR() As Double     'TIUpR_inj
    Public SubTI_lwr_R_THOR() As Double     'TILowR_inj
    Public SubChest_VC_THOR() As Double     'VC_inj_CFC180
    Public SubFemurL_THOR() As Double       'FFCL_inj
    Public SubFemurR_THOR() As Double       'FFCR_inj
    Public SubNeck_Comp_THOR() As Double        'FNICtension_inj
    Public SubNeck_Tens_THOR() As Double        'FNICtension_inj
    Public SubNeck_Shear_THOR() As Double       'FNICshear_inj
    Public SubNeck_Exten_THOR() As Double       'FNICbending_inj
    Public SubStar_Rating_THOR() As Double

    'For THOR Added Injury
    Public SubAngularVelX() As Double
    Public SubAngularVelY() As Double
    Public SubAngularVelZ() As Double
    Public SubBrIC() As Double
    Public SubThxIrUpL() As Double
    Public SubThxIrUpR() As Double
    Public SubThxIrLowL() As Double
    Public SubThxIrLowR() As Double

    Public X_Label_THOR() As String

    Dim EventPassing As Boolean = False

    Private Sub FrmInjuryValGraph_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Initailize_Index_THOR = 0
        Me.InjuryLst.SelectionMode = SelectionMode.One
        Me.Width = 1050
        Me.Height = 550
        Me.SplitMain.SplitterDistance = Me.Width - 250

        DATAlabelindex_THOR = 0

    End Sub

    Public Sub New(ByVal a1() As Double, ByVal a2() As Double, ByVal a3() As Double, ByVal a4() As Double, ByVal a5() As Double _
                   , ByVal a6() As Double, ByVal a7() As Double, ByVal a8() As Double, ByVal a9() As Double _
                   , ByVal a10() As Double, ByVal a11() As Double, ByVal a12() As Double, ByVal a13() As Double _
                   , ByVal a14() As Double, ByVal a15() As Double, ByVal a16() As Double, ByVal a17() As Double _
                   , ByVal a18() As Double, ByVal a19() As Double, ByVal a20() As Double, ByVal a21() As Double _
                   , ByVal a22() As Double, ByVal a23() As Double, ByVal a24() As Double, ByVal a25() As Double _
                   , ByVal a26() As Double, ByVal a27() As Double, ByVal a28() As Double, ByVal a29() As Double _
                   , ByVal a30() As Double, ByVal a31() As Double, ByVal a32() As Double, ByVal a33() As Double _
                   , ByVal a34() As Double)

        ' 이 호출은 디자이너에 필요합니다.
        InitializeComponent()
        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하십시오.

        SubHead3MSG_THOR = a1
        SubChest_G_CUMULATIVE_T3MS_inj_THOR = a2
        SubHIC15_THOR = a3
        SubHIC36_THOR = a4
        SubNTE_THOR = a5
        SubNTF_THOR = a6
        SubNCE_THOR = a7
        SubNCF_THOR = a8
        SubHead_Peak_G_THOR = a9
        SubChest_D_THOR = a10
        SubKneeS_L_THOR = a11
        SubKneeS_R_THOR = a12
        SubTibia_Comp_L_THOR = a13
        SubTibia_Comp_R_THOR = a14
        SubTI_upr_L_THOR = a15
        SubTI_lwr_L_THOR = a16
        SubTI_upr_R_THOR = a17
        SubTI_lwr_R_THOR = a18
        SubChest_VC_THOR = a19
        SubFemurL_THOR = a20
        SubFemurR_THOR = a21
        SubNeck_Comp_THOR = a22
        SubNeck_Tens_THOR = a23
        SubNeck_Shear_THOR = a24
        SubNeck_Exten_THOR = a25
        SubStar_Rating_THOR = a26

        'For THOR Added Injury
        SubAngularVelX = a27
        SubAngularVelY = a28
        SubAngularVelZ = a29
        SubBrIC = a30
        SubThxIrUpL = a31
        SubThxIrUpR = a32
        SubThxIrLowL = a33
        SubThxIrLowR = a34

        Dim i As Integer

        For i = 0 To UBound(SubHIC15_THOR)
            Me.MDlst.Items.Add("MD " & i + 1 & ".")
        Next
    End Sub

    Private Sub InjuryLst_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InjuryLst.SelectedIndexChanged

        Dim i As Integer

        For i = Me.InjuryValChrt.Series.Count - 1 To 0 Step -1
            If Me.InjuryLst.Items(Me.InjuryLst.SelectedIndex).ToString = Me.InjuryValChrt.Series(i).Name.ToString Then Exit Sub
        Next

        ReDim X_Label_THOR(UBound(SubHead3MSG_THOR))

        For i = 0 To UBound(X_Label_THOR)
            X_Label_THOR(i) = Me.MDlst.Items(i)
        Next

        MainMDI.ProgressBarMain.Value = 0
        MainMDI.ProgressBarMain.Maximum = 1140

        With Me

            If Me.ChkSuPo.Checked = False Then
                Me.DetailTxt.Text = ""

                For i = .InjuryValChrt.Series.Count To 1 Step -1
                    .InjuryValChrt.Series.RemoveAt(i - 1)
                Next

                For i = .CmbSeries.Items.Count To 1 Step -1
                    .CmbSeries.Items.RemoveAt(i - 1)
                Next
            End If

            MainMDI.ProgressBarMain.Value = 300

            Select Case .InjuryLst.SelectedIndex
                Case 0
                    .InjuryValChrt.Series.Add("Head 3ms G")
                    .CmbSeries.Items.Add("Head 3ms G")
                    '.InjuryValChrt.Series(.InjuryValChrt.Series.Count + 1).Name = "Head 3ms G"
                    .InjuryValChrt.Series("Head 3ms G").Points.DataBindXY(X_Label_THOR, SubHead3MSG_THOR)
                    .InjuryValChrt.Series("Head 3ms G").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 1
                    .InjuryValChrt.Series.Add("Chest 3ms G")
                    .CmbSeries.Items.Add("Chest 3ms G")
                    .InjuryValChrt.Series("Chest 3ms G").Points.DataBindXY(X_Label_THOR, SubChest_G_CUMULATIVE_T3MS_inj_THOR)
                    .InjuryValChrt.Series("Chest 3ms G").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 2
                    .InjuryValChrt.Series.Add("BrIC")
                    .CmbSeries.Items.Add("BrIC")
                    .InjuryValChrt.Series("BrIC").Points.DataBindXY(X_Label_THOR, SubBrIC)
                    .InjuryValChrt.Series("BrIC").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 3
                    .InjuryValChrt.Series.Add("Max. Angular Velocity X")
                    .CmbSeries.Items.Add("Max. Angular Velocity X")
                    .InjuryValChrt.Series("Max. Angular Velocity X").Points.DataBindXY(X_Label_THOR, SubAngularVelX)
                    .InjuryValChrt.Series("Max. Angular Velocity X").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 4
                    .InjuryValChrt.Series.Add("Max. Angular Velocity Y")
                    .CmbSeries.Items.Add("Max. Angular Velocity Y")
                    .InjuryValChrt.Series("Max. Angular Velocity Y").Points.DataBindXY(X_Label_THOR, SubAngularVelY)
                    .InjuryValChrt.Series("Max. Angular Velocity Y").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 5
                    .InjuryValChrt.Series.Add("Max. Angular Velocity Z")
                    .CmbSeries.Items.Add("Max. Angular Velocity Z")
                    .InjuryValChrt.Series("Max. Angular Velocity Z").Points.DataBindXY(X_Label_THOR, SubAngularVelZ)
                    .InjuryValChrt.Series("Max. Angular Velocity Z").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 6
                    .InjuryValChrt.Series.Add("HIC15")
                    .CmbSeries.Items.Add("HIC15")
                    .InjuryValChrt.Series("HIC15").Points.DataBindXY(X_Label_THOR, SubHIC15_THOR)
                    .InjuryValChrt.Series("HIC15").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 7
                    .InjuryValChrt.Series.Add("HIC36")
                    .CmbSeries.Items.Add("HIC36")
                    .InjuryValChrt.Series("HIC36").Points.DataBindXY(X_Label_THOR, SubHIC36_THOR)
                    .InjuryValChrt.Series("HIC36").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 8
                    .InjuryValChrt.Series.Add("NTE")
                    .CmbSeries.Items.Add("NTE")
                    .InjuryValChrt.Series("NTE").Points.DataBindXY(X_Label_THOR, SubNTE_THOR)
                    .InjuryValChrt.Series("NTE").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 9
                    .InjuryValChrt.Series.Add("NTF")
                    .CmbSeries.Items.Add("NTF")
                    .InjuryValChrt.Series("NTF").Points.DataBindXY(X_Label_THOR, SubNTF_THOR)
                    .InjuryValChrt.Series("NTF").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 10
                    .InjuryValChrt.Series.Add("NCE")
                    .CmbSeries.Items.Add("NCE")
                    .InjuryValChrt.Series("NCE").Points.DataBindXY(X_Label_THOR, SubNCE_THOR)
                    .InjuryValChrt.Series("NCE").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 11
                    .InjuryValChrt.Series.Add("NCF")
                    .CmbSeries.Items.Add("NCF")
                    .InjuryValChrt.Series("NCF").Points.DataBindXY(X_Label_THOR, SubNCF_THOR)
                    .InjuryValChrt.Series("NCF").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 12
                    .InjuryValChrt.Series.Add("Head Peak G")
                    .CmbSeries.Items.Add("Head Peak G")
                    .InjuryValChrt.Series("Head Peak G").Points.DataBindXY(X_Label_THOR, SubHead_Peak_G_THOR)
                    .InjuryValChrt.Series("Head Peak G").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 13
                    .InjuryValChrt.Series.Add("Average CD")
                    .CmbSeries.Items.Add("Average CD")
                    .InjuryValChrt.Series("Average CD").Points.DataBindXY(X_Label_THOR, SubChest_D_THOR)
                    .InjuryValChrt.Series("Average CD").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 14
                    .InjuryValChrt.Series.Add("Chest Upper Left")
                    .CmbSeries.Items.Add("Chest Upper Left")
                    .InjuryValChrt.Series("Chest Upper Left").Points.DataBindXY(X_Label_THOR, SubThxIrUpL)
                    .InjuryValChrt.Series("Chest Upper Left").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 15
                    .InjuryValChrt.Series.Add("Chest Upper Right")
                    .CmbSeries.Items.Add("Chest Upper Right")
                    .InjuryValChrt.Series("Chest Upper Right").Points.DataBindXY(X_Label_THOR, SubThxIrUpR)
                    .InjuryValChrt.Series("Chest Upper Right").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 16
                    .InjuryValChrt.Series.Add("Chest Lower Left")
                    .CmbSeries.Items.Add("Chest Lower Left")
                    .InjuryValChrt.Series("Chest Lower Left").Points.DataBindXY(X_Label_THOR, SubThxIrLowL)
                    .InjuryValChrt.Series("Chest Lower Left").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 17
                    .InjuryValChrt.Series.Add("Chest Lower Right")
                    .CmbSeries.Items.Add("Chest Lower Right")
                    .InjuryValChrt.Series("Chest Lower Right").Points.DataBindXY(X_Label_THOR, SubThxIrLowR)
                    .InjuryValChrt.Series("Chest Lower Right").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 18
                    .InjuryValChrt.Series.Add("Knee Slide (L)")
                    .CmbSeries.Items.Add("Knee Slide (L)")
                    .InjuryValChrt.Series("Knee Slide (L)").Points.DataBindXY(X_Label_THOR, SubKneeS_L_THOR)
                    .InjuryValChrt.Series("Knee Slide (L)").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 19
                    .InjuryValChrt.Series.Add("Knee Slide (R)")
                    .CmbSeries.Items.Add("Knee Slide (R)")
                    .InjuryValChrt.Series("Knee Slide (R)").Points.DataBindXY(X_Label_THOR, SubKneeS_R_THOR)
                    .InjuryValChrt.Series("Knee Slide (R)").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 20
                    .InjuryValChrt.Series.Add("Tibia Comp (L)")
                    .CmbSeries.Items.Add("Tibia Comp (L)")
                    .InjuryValChrt.Series("Tibia Comp (L)").Points.DataBindXY(X_Label_THOR, SubTibia_Comp_L_THOR)
                    .InjuryValChrt.Series("Tibia Comp (L)").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 21
                    .InjuryValChrt.Series.Add("Tibia Comp (R)")
                    .CmbSeries.Items.Add("Tibia Comp (R)")
                    .InjuryValChrt.Series("Tibia Comp (R)").Points.DataBindXY(X_Label_THOR, SubTibia_Comp_R_THOR)
                    .InjuryValChrt.Series("Tibia Comp (R)").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 22
                    .InjuryValChrt.Series.Add("Tibia Index Upper (L)")
                    .CmbSeries.Items.Add("Tibia Index Upper (L)")
                    .InjuryValChrt.Series("Tibia Index Upper (L)").Points.DataBindXY(X_Label_THOR, SubTI_upr_L_THOR)
                    .InjuryValChrt.Series("Tibia Index Upper (L)").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 23
                    .InjuryValChrt.Series.Add("Tibia Index Lower (L)")
                    .CmbSeries.Items.Add("Tibia Index Lower (L)")
                    .InjuryValChrt.Series("Tibia Index Lower (L)").Points.DataBindXY(X_Label_THOR, SubTI_lwr_L_THOR)
                    .InjuryValChrt.Series("Tibia Index Lower (L)").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 24
                    .InjuryValChrt.Series.Add("Tibia Index Upper (R)")
                    .CmbSeries.Items.Add("Tibia Index Upper (R)")
                    .InjuryValChrt.Series("Tibia Index Upper (R)").Points.DataBindXY(X_Label_THOR, SubTI_upr_R_THOR)
                    .InjuryValChrt.Series("Tibia Index Upper (R)").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 25
                    .InjuryValChrt.Series.Add("Tibia Index Lower (R)")
                    .CmbSeries.Items.Add("Tibia Index Lower (R)")
                    .InjuryValChrt.Series("Tibia Index Lower (R)").Points.DataBindXY(X_Label_THOR, SubTI_lwr_R_THOR)
                    .InjuryValChrt.Series("Tibia Index Lower (R)").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 26
                    .InjuryValChrt.Series.Add("Chest VC")
                    .CmbSeries.Items.Add("Chest VC")
                    .InjuryValChrt.Series("Chest VC").Points.DataBindXY(X_Label_THOR, SubChest_VC_THOR)
                    .InjuryValChrt.Series("Chest VC").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 27
                    .InjuryValChrt.Series.Add("Femur (L)")
                    .CmbSeries.Items.Add("Femur (L)")
                    .InjuryValChrt.Series("Femur (L)").Points.DataBindXY(X_Label_THOR, SubFemurL_THOR)
                    .InjuryValChrt.Series("Femur (L)").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 28
                    .InjuryValChrt.Series.Add("Femur (R)")
                    .CmbSeries.Items.Add("Femur (R)")
                    .InjuryValChrt.Series("Femur (R)").Points.DataBindXY(X_Label_THOR, SubFemurR_THOR)
                    .InjuryValChrt.Series("Femur (R)").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 29
                    .InjuryValChrt.Series.Add("Neck Comp.")
                    .CmbSeries.Items.Add("Neck Comp.")
                    .InjuryValChrt.Series("Neck Comp.").Points.DataBindXY(X_Label_THOR, SubNeck_Comp_THOR)
                    .InjuryValChrt.Series("Neck Comp.").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 30
                    .InjuryValChrt.Series.Add("Neck Tens.")
                    .CmbSeries.Items.Add("Neck Tens.")
                    .InjuryValChrt.Series("Neck Tens.").Points.DataBindXY(X_Label_THOR, SubNeck_Tens_THOR)
                    .InjuryValChrt.Series("Neck Tens.").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 31
                    .InjuryValChrt.Series.Add("Neck Shear")
                    .CmbSeries.Items.Add("Neck Shear")
                    .InjuryValChrt.Series("Neck Shear").Points.DataBindXY(X_Label_THOR, SubNeck_Shear_THOR)
                    .InjuryValChrt.Series("Neck Shear").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 32
                    .InjuryValChrt.Series.Add("Neck Exten.")
                    .CmbSeries.Items.Add("Neck Exten.")
                    .InjuryValChrt.Series("Neck Exten.").Points.DataBindXY(X_Label_THOR, SubNeck_Exten_THOR)
                    .InjuryValChrt.Series("Neck Exten.").ChartType = DataVisualization.Charting.SeriesChartType.Line
                Case 33
                    .InjuryValChrt.Series.Add("Star Rating")
                    .CmbSeries.Items.Add("Star Rating")
                    .InjuryValChrt.Series("Star Rating").Points.DataBindXY(X_Label_THOR, SubStar_Rating_THOR)
                    .InjuryValChrt.Series("Star Rating").ChartType = DataVisualization.Charting.SeriesChartType.Line
            End Select

            MainMDI.ProgressBarMain.Value = 600

            .InjuryValChrt.ChartAreas(0).AxisY.Minimum = Double.NaN
            .InjuryValChrt.ChartAreas(0).AxisY.Maximum = Double.NaN
            .CmbSeries.SelectedIndex = .CmbSeries.Items.Count - 1

            Dim DescriptionStr As String
            Dim MinValuePoint As DataPoint = .InjuryValChrt.Series(.InjuryValChrt.Series.Count - 1).Points.FindMinByValue()
            Dim MaxValuePoint As DataPoint = .InjuryValChrt.Series(.InjuryValChrt.Series.Count - 1).Points.FindMaxByValue()
            Dim Mean As Double = .InjuryValChrt.DataManipulator.Statistics.Mean(.InjuryValChrt.Series(.InjuryValChrt.Series.Count - 1).Name)
            Dim Median As Double = .InjuryValChrt.DataManipulator.Statistics.Median(.InjuryValChrt.Series(.InjuryValChrt.Series.Count - 1).Name)
            Dim Variance As Double = .InjuryValChrt.DataManipulator.Statistics.Variance(.InjuryValChrt.Series(.InjuryValChrt.Series.Count - 1).Name, True)
            Dim standardDeviation As Double = Math.Sqrt(Variance)

            DescriptionStr = vbCrLf & "■ " & .CmbSeries.Items(.CmbSeries.SelectedIndex).ToString & vbCrLf & _
                              " Min Value : " & Format(MinValuePoint.YValues(0), "0.0##") & " [" & MinValuePoint.AxisLabel & "]" & vbCrLf & _
                              " Max Value : " & Format(MaxValuePoint.YValues(0), "0.0##") & " [" & MaxValuePoint.AxisLabel & "]" & vbCrLf & _
                              " Average : " & Format(Mean, "0.0##") & vbCrLf & _
                              " Median  : " & Format(Median, "0.0##") & vbCrLf & _
                              " Variance : " & Format(Variance, "0.0##") & vbCrLf & _
                              " S.Dev     : " & Format(standardDeviation, "0.0##") & vbCrLf
            .DetailTxt.Text = .DetailTxt.Text & DescriptionStr
        End With

        Dim ChrtDP As New DPInjuryVal
        ChrtDP.GraphAxisLine(Me.InjuryValChrt)
        MainMDI.ProgressBarMain.Value = 700
        ChrtDP.ScrollnZoom(Me.InjuryValChrt)
        MainMDI.ProgressBarMain.Value = 800
        ChrtDP.AxisSetting(Me.InjuryValChrt)
        MainMDI.ProgressBarMain.Value = 900

        Me.InjuryValChrt.ChartAreas(0).AxisX.MajorGrid.Interval = 1.0
        Me.InjuryValChrt.ChartAreas(0).AxisX.Minimum = 0.5
        Me.InjuryValChrt.ChartAreas(0).AxisX.LabelStyle.Interval = 0.5
        Me.InjuryValChrt.ChartAreas(0).AxisX.MajorTickMark.Interval = 1.0
        Me.InjuryValChrt.ChartAreas(0).AxisX.Maximum = UBound(SubHead3MSG_THOR) + 1.5

        Me.InjuryValChrt.Show()
        MainMDI.ProgressBarMain.Value = 1140
        'TEST ================================================================================================
        'Me.InjuryValChrt.DataManipulator.CopySeriesValues(Me.InjuryValChrt.Series(0).Name, Me.Chart1.Series(0).Name)
        '지워도 됨
        'Me.Chart1.Series(0) = Me.InjuryValChrt.Series(0)
        'TEST ================================================================================================
    End Sub

    Private Sub InjuryValChrt_AxisScrollBarClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.DataVisualization.Charting.ScrollBarEventArgs) Handles InjuryValChrt.AxisScrollBarClicked
        ' Handle zoom reset button
        If e.ButtonType = ScrollBarButtonType.ZoomReset Then

            ' Reset zoom on X and Y axis
            Me.InjuryValChrt.ChartAreas(0).AxisX.ScaleView.ZoomReset()
            Me.InjuryValChrt.ChartAreas(0).AxisY.ScaleView.ZoomReset()

        End If
    End Sub

    Private Sub InjuryValChrt_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles InjuryValChrt.DoubleClick
        Dim i As Integer

        If DATAlabelindex_THOR = 0 Then
            With Me.InjuryValChrt
                For i = 1 To Me.InjuryValChrt.Series.Count
                    .Series(i - 1).IsValueShownAsLabel = True
                Next
            End With
            DATAlabelindex_THOR = 1
        Else
            With Me.InjuryValChrt
                For i = 1 To Me.InjuryValChrt.Series.Count
                    .Series(i - 1).IsValueShownAsLabel = False
                Next
            End With
            DATAlabelindex_THOR = 0
        End If
    End Sub

    Private Sub FrmInjuryValGraph_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        If Me.Width > 250 And Me.Height > 200 And Me.SplitMain.Panel1.Width > 500 Then
            With Me
                .InjuryLst.Location = New Point(10, 10 + 60)
                .InjuryLst.Size = New Size(250, .Height - 60 - 20 - 60 - 60)
                .MDlst.Location = New Point(10, .InjuryLst.Top - 60)
                .MDlst.Size = New Size(.InjuryLst.Width, 55)
                .InjuryValChrt.Location = New Point(265, 10)
                .InjuryValChrt.Size = New Size(.SplitMain.SplitterDistance - 35 - 250, .InjuryLst.Height + 20 + 65)
                .MinTxt.Location = New Point(10, .InjuryLst.Top + .InjuryLst.Height + 5)
                .MinTxt.Size = New Size(.InjuryLst.Width / 3, 26)
                .MaxTxt.Location = New Point(.MinTxt.Width + .MinTxt.Left, .MinTxt.Top)
                .MaxTxt.Size = New Size(.InjuryLst.Width / 3, 26)

                .Intervaltxt.Location = New Point(.MaxTxt.Width + .MaxTxt.Left, .MaxTxt.Top)
                .Intervaltxt.Size = New Size(.InjuryLst.Width / 3, 26)
                .OptBox.Location = New Point(.MinTxt.Location.X, .MinTxt.Location.Y + .MinTxt.Height + 5)
                .OptBox.Size = New Size(.SplitMain.SplitterDistance - 25, .ClientRectangle.Height - .OptBox.Location.Y - 5)
                .ChkSuPo.Location = New Point(10, 20)
                .ChkMarker.Location = New Point(.ChkSuPo.Location.X, .ChkSuPo.Location.Y + .ChkSuPo.Height)
                .CmbSeries.Location = New Point(.ChkSuPo.Location.X + .ChkSuPo.Width + 10, 18)
                .CmbSeries.Size = New Size(.InjuryLst.Width, 23)
                .CmbLineBar.Location = New Point(.CmbSeries.Location.X + .CmbSeries.Width + 10, 18)
                .CmbLineBar.Size = New Size(90, 23)
            End With
        End If
    End Sub

    Private Sub InjuryValChrt_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles InjuryValChrt.Paint

        Me.MinTxt.Text = Me.InjuryValChrt.ChartAreas(0).AxisY.Minimum
        Me.MaxTxt.Text = Me.InjuryValChrt.ChartAreas(0).AxisY.Maximum

        If Me.InjuryValChrt.ChartAreas(0).AxisY.Interval = 0.0 Then
            Me.Intervaltxt.Text = "AUTO"
        Else
            Me.Intervaltxt.Text = Me.InjuryValChrt.ChartAreas(0).AxisY.Interval
        End If

    End Sub

    Private Sub MinTxt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MinTxt.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            If IsNumeric((Me.MinTxt.Text) = True) And IsNumeric((Me.MaxTxt.Text) = True) Then
                If CDbl(Me.MinTxt.Text) >= CDbl(Me.MaxTxt.Text) Then Exit Sub
                Me.InjuryValChrt.ChartAreas(0).AxisY.Minimum = CDbl(Me.MinTxt.Text)
                Me.InjuryValChrt.ChartAreas(0).AxisY.Maximum = CDbl(Me.MaxTxt.Text)

                If IsNumeric((Me.Intervaltxt.Text) = True) Then
                    Me.InjuryValChrt.ChartAreas(0).AxisY.Interval = CDbl(Me.Intervaltxt.Text)
                End If
            Else
                Exit Sub
            End If
            'ElseIf IsNumeric(Me.MinTxt.Text) = False And e.KeyChar <> Chr(8) Then
            '    MsgBox("Numeric Value Only!!", , "Error")
        End If
    End Sub

    Private Sub MaxTxt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MaxTxt.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) And IsNumeric(Me.MaxTxt.Text) = True Then
            If IsNumeric((Me.MinTxt.Text) = True) And IsNumeric((Me.MaxTxt.Text) = True) Then
                If CDbl(Me.MinTxt.Text) >= CDbl(Me.MaxTxt.Text) Then Exit Sub
                Me.InjuryValChrt.ChartAreas(0).AxisY.Minimum = CDbl(Me.MinTxt.Text)
                Me.InjuryValChrt.ChartAreas(0).AxisY.Maximum = CDbl(Me.MaxTxt.Text)

                If IsNumeric((Me.Intervaltxt.Text) = True) Then
                    Me.InjuryValChrt.ChartAreas(0).AxisY.Interval = CDbl(Me.Intervaltxt.Text)
                End If
            Else
                Exit Sub
            End If
            'ElseIf IsNumeric(Me.MaxTxt.Text) = False And e.KeyChar <> Chr(8) Then
            '    MsgBox("Numeric Value Only!!", , "Error")
        End If
    End Sub

    Private Sub Intervaltxt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Intervaltxt.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) And IsNumeric(Me.MaxTxt.Text) = True Then
            If IsNumeric((Me.MinTxt.Text) = True) And IsNumeric((Me.MaxTxt.Text) = True) Then
                If CDbl(Me.MinTxt.Text) >= CDbl(Me.MaxTxt.Text) Then Exit Sub
                Me.InjuryValChrt.ChartAreas(0).AxisY.Minimum = CDbl(Me.MinTxt.Text)
                Me.InjuryValChrt.ChartAreas(0).AxisY.Maximum = CDbl(Me.MaxTxt.Text)

                If IsNumeric((Me.Intervaltxt.Text) = True) Then
                    Me.InjuryValChrt.ChartAreas(0).AxisY.Interval = CDbl(Me.Intervaltxt.Text)
                End If
            Else
                Exit Sub
            End If
            'ElseIf IsNumeric(Me.MaxTxt.Text) = False And e.KeyChar <> Chr(8) Then
            '    MsgBox("Numeric Value Only!!", , "Error")
        End If
    End Sub

    Private Sub CopyToClipBoardBMPToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToClipBoardBMPToolStripMenuItem.Click
        CopyChartImage(Me.InjuryValChrt)
    End Sub

    Private Sub InjuryValChrt_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles InjuryValChrt.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Me.InjuryContextMenu.Show(MousePosition)
        End If
    End Sub

    Private Sub CmbSeries_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbSeries.SelectedIndexChanged
        EventPassing = True

        If Me.InjuryValChrt.Series(Me.CmbSeries.SelectedIndex).ChartType = SeriesChartType.Line Then
            Me.CmbLineBar.SelectedIndex = 0
        ElseIf Me.InjuryValChrt.Series(Me.CmbSeries.SelectedIndex).ChartType = SeriesChartType.Column Then
            Me.CmbLineBar.SelectedIndex = 1
        End If

        EventPassing = False
    End Sub

    Private Sub CmbLineBar_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbLineBar.SelectedIndexChanged

        If EventPassing = True Then Exit Sub

        If Me.CmbLineBar.SelectedIndex = 0 Then
            Me.InjuryValChrt.Series(Me.CmbSeries.SelectedIndex).ChartType = SeriesChartType.Line
        ElseIf Me.CmbLineBar.SelectedIndex = 1 Then
            Me.InjuryValChrt.Series(Me.CmbSeries.SelectedIndex).ChartType = SeriesChartType.Column
            Me.InjuryValChrt.Series(Me.CmbSeries.SelectedIndex)("PointWidth") = "0.3"
        ElseIf Me.CmbLineBar.SelectedIndex = 2 Then
            Me.InjuryValChrt.Series(Me.CmbSeries.SelectedIndex).ChartType = SeriesChartType.Column
            Me.InjuryValChrt.Series(Me.CmbSeries.SelectedIndex)("PointWidth") = "0.6"
        End If

    End Sub

    Private Sub SplitMain_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitMain.DoubleClick
        Me.SplitMain.SplitterDistance = Me.Width - 250
    End Sub

    Private Sub SplitMain_SplitterMoved(ByVal sender As System.Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitMain.SplitterMoved
        FrmInjuryValGraph_Resize(sender, Nothing)
    End Sub

    Private Sub ChkMarker_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkMarker.CheckedChanged

        'If EventPassing = True Then Exit Sub

        Dim i As Integer = 0

        With Me.InjuryValChrt
            If Me.ChkMarker.Checked = False Then
                For i = 0 To .Series.Count - 1
                    .Series(i).MarkerStyle = MarkerStyle.None
                Next
            Else
                For i = 0 To .Series.Count - 1
                    .Series(i).MarkerStyle = MarkerStyle.Circle
                Next
            End If
        End With

    End Sub

    Private Sub MDlst_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MDlst.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            With Me
                Me.MDLstMnu.Show(MousePosition)
            End With
        End If
    End Sub

    Private Sub RenameTxt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles RenameTxt.KeyPress

        Dim S_Index As Integer = Me.MDlst.SelectedIndex

        If e.KeyChar = ChrW(Keys.Enter) And Me.MDlst.SelectedIndex <> -1 And Me.RenameTxt.Text <> "" Then
            Me.MDlst.Items.RemoveAt(Me.MDlst.SelectedIndex)
            Me.MDlst.Items.Insert(S_Index, Me.RenameTxt.Text)
            Me.MDlst.SelectedIndex = S_Index

            Me.RenameTxt.Text = ""
            Me.MDLstMnu.Close()
        End If

    End Sub

End Class