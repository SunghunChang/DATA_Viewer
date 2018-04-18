Imports System.IO

Public Class FrmCorrel

    Dim DUC_Factor() As Double
    Dim DUC_Relative() As Double
    Dim WIFac() As Double
    'Dim WIRel(Me.InjuryChart.Series.Count - 2) As Double
    Dim SnG() As Double
    Dim SnG_M() As Double
    Dim SnG_P() As Double
    Dim GPV() As Double
    Dim GPT() As Double
    Dim Ref_Series_Num As Integer
    Dim SeriesName() As String

    'Correlation Weight Factor
    Dim DUC_Factor_Weight As Double
    Dim DUC_Relative_Weight As Double
    Dim WIFac_Weight As Double
    Dim SnG_Weight As Double
    Dim SnG_M_Weight As Double
    Dim SnG_P_Weight As Double
    Dim GPV_Weight As Double
    Dim GPT_Weight As Double

    Public Sub New(ByVal Tmp_DUC_F() As Double, ByVal Tmp_DUC_R() As Double, ByVal Tmp_WIFac() As Double, ByVal Tmp_SnG() As Double, _
                   ByVal Tmp_SnG_M() As Double, ByVal Tmp_SnG_P() As Double, ByVal Tmp_GPV() As Double, ByVal Tmp_GPT() As Double, _
                   ByVal Ref_Num As Integer, ByVal Tmp_Names As String())

        ' 이 호출은 디자이너에 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하십시오.

        DUC_Factor = Tmp_DUC_F
        DUC_Relative = Tmp_DUC_R
        WIFac = Tmp_WIFac
        SnG = Tmp_SnG
        SnG_M = Tmp_SnG_M
        SnG_P = Tmp_SnG_P
        GPV = Tmp_GPV
        GPT = Tmp_GPT
        Ref_Series_Num = Ref_Num
        SeriesName = Tmp_Names

        '파일 읽기
        Dim SetupFile As StreamReader
        'InjuryFile = New StreamReader(Me.GraphBasedDlg.FileName)
        SetupFile = New StreamReader(Application.StartupPath & "\DATA\General.setup")
        Dim Tmp_Str() As String

        Tmp_Str = SetupFile.ReadLine.Split("\")
        Tmp_Str = SetupFile.ReadLine.Split("\")
        Tmp_Str = SetupFile.ReadLine.Split("\")
        Tmp_Str = SetupFile.ReadLine.Split("\")

        Tmp_Str = SetupFile.ReadLine.Split("\")
        DUC_Factor_Weight = CDbl(Tmp_Str(1))
        Tmp_Str = SetupFile.ReadLine.Split("\")
        DUC_Relative_Weight = CDbl(Tmp_Str(1))
        Tmp_Str = SetupFile.ReadLine.Split("\")
        WIFac_Weight = CDbl(Tmp_Str(1))
        Tmp_Str = SetupFile.ReadLine.Split("\")
        SnG_Weight = CDbl(Tmp_Str(1))
        Tmp_Str = SetupFile.ReadLine.Split("\")
        SnG_M_Weight = CDbl(Tmp_Str(1))
        Tmp_Str = SetupFile.ReadLine.Split("\")
        SnG_P_Weight = CDbl(Tmp_Str(1))
        Tmp_Str = SetupFile.ReadLine.Split("\")
        GPV_Weight = CDbl(Tmp_Str(1))
        Tmp_Str = SetupFile.ReadLine.Split("\")
        GPT_Weight = CDbl(Tmp_Str(1))
        SetupFile.Close()

    End Sub

    Private Sub FrmCorrel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Width = 913
        Me.Height = 300

        Dim i As Integer = 0
        Dim k As Integer = 0

        Me.Range1.Text = "0%~25%"
        Me.Range2.Text = "25%~50%"
        Me.Range3.Text = "50%~75%"
        Me.Range4.Text = "75%~100%"

        With Me.ScoreGrid

            .Rows = DUC_Factor.Count + 2
            .Cols = 10
            .FixedRows = 2
            .FixedCols = 1
            .MergeCells = 1

            .set_ColWidth(0, 800)
            .set_RowHeight(0, 600)
            .set_MergeCol(0, True)
            '.set_ColAlignment(0, 4)
            .set_MergeRow(0, True)
            .set_MergeRow(1, True)
            .Font = New Font("맑은 고딕", 9, FontStyle.Bold)
            .WordWrap = True
            .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillRepeat

            For i = 1 To 9
                .set_MergeCol(i, True)
                .set_ColAlignment(i, 4) '가운데 정렬
                .set_ColWidth(i, 1400)
            Next

            'Header
            .set_TextMatrix(0, 0, "Criteria")
            .set_TextMatrix(1, 0, "Criteria")
            .set_TextMatrix(0, 1, "Difference Area" & vbCrLf & "Under Curve")
            .set_TextMatrix(1, 1, "Factor" & "[" & Format(DUC_Factor_Weight, "#0.0") & "]")
            .set_TextMatrix(0, 2, "Difference Area" & vbCrLf & "Under Curve")
            .set_TextMatrix(1, 2, "Relative" & "[" & Format(DUC_Relative_Weight, "#0.0") & "]")
            .set_TextMatrix(0, 3, "Weighted" & vbCrLf & "Integration" & vbCrLf & "[" & Format(WIFac_Weight, "#0.0") & "]")
            .set_TextMatrix(1, 3, .get_TextMatrix(0, 3))
            .set_TextMatrix(0, 4, "S&G" & vbCrLf & "[" & Format(SnG_Weight, "#0.0") & "]")
            .set_TextMatrix(1, 4, .get_TextMatrix(0, 4))
            .set_TextMatrix(0, 5, "S&G" & vbCrLf & "Magnitude" & vbCrLf & "[" & Format(SnG_M_Weight, "#0.0") & "]")
            .set_TextMatrix(1, 5, .get_TextMatrix(0, 5))
            .set_TextMatrix(0, 6, "S&G" & vbCrLf & "Phase" & vbCrLf & "[" & Format(SnG_P_Weight, "#0.0") & "]")
            .set_TextMatrix(1, 6, .get_TextMatrix(0, 6))
            .set_TextMatrix(0, 7, "Global Peak")
            .set_TextMatrix(0, 8, "Global Peak")
            .set_TextMatrix(1, 7, "Value" & "[" & Format(GPV_Weight, "#0.0") & "]")
            .set_TextMatrix(1, 8, "Timing" & "[" & Format(GPT_Weight, "#0.0") & "]")
            .set_TextMatrix(0, 9, "Weighted" & vbCrLf & "Average")
            .set_TextMatrix(1, 9, "Weighted" & vbCrLf & "Average")

            Dim AverageScore As Double

            For i = 0 To UBound(DUC_Factor)
                If i <> Ref_Series_Num Then
                    .set_TextMatrix(i + 2, 0, SeriesName(i))
                    .set_TextMatrix(i + 2, 1, Format(DUC_Factor(i), "##0.0%"))
                    .set_TextMatrix(i + 2, 2, Format(DUC_Relative(i), "##0.0%"))
                    .set_TextMatrix(i + 2, 3, Format(WIFac(i), "##0.0%"))
                    .set_TextMatrix(i + 2, 4, Format(SnG(i), "##0.0%"))
                    .set_TextMatrix(i + 2, 5, Format(SnG_M(i), "##0.0%"))
                    .set_TextMatrix(i + 2, 6, Format(SnG_P(i), "##0.0%"))
                    .set_TextMatrix(i + 2, 7, Format(GPV(i), "##0.0%"))
                    .set_TextMatrix(i + 2, 8, Format(GPT(i), "##0.0%"))
                    AverageScore = (DUC_Factor(i) * DUC_Factor_Weight + DUC_Relative(i) * DUC_Relative_Weight + _
                                    WIFac(i) * WIFac_Weight + _
                                    SnG(i) * SnG_Weight + SnG_M(i) * SnG_M_Weight + SnG_P(i) * SnG_P_Weight + _
                                    GPV(i) * GPV_Weight + GPT(i) * GPT_Weight) / _
                                (DUC_Factor_Weight + DUC_Relative_Weight + WIFac_Weight + SnG_Weight + SnG_M_Weight + SnG_P_Weight + GPV_Weight + GPT_Weight)
                    .set_TextMatrix(i + 2, 9, Format(AverageScore, "##0.0%"))
                    For k = 1 To 9
                        .Row = i + 2
                        .Col = k
                        .RowSel = i + 2
                        .ColSel = k
                        If CDbl(Mid(.get_TextMatrix(i + 2, k), 1, Len(.get_TextMatrix(i + 2, k)) - 1)) >= 75.0 Then
                            .CellBackColor = Color.LightGreen
                            '.CellForeColor = Color.Green
                        ElseIf CDbl(Mid(.get_TextMatrix(i + 2, k), 1, Len(.get_TextMatrix(i + 2, k)) - 1)) >= 50.0 Then
                            .CellBackColor = Color.Yellow
                            '.CellForeColor = Color.Yellow
                        ElseIf CDbl(Mid(.get_TextMatrix(i + 2, k), 1, Len(.get_TextMatrix(i + 2, k)) - 1)) >= 25.0 Then
                            .CellBackColor = Color.Orange
                            '.CellForeColor = Color.Orange
                        Else
                            .CellBackColor = Color.Red
                            '.CellForeColor = Color.Red
                        End If
                        'If k = 9 Then .CellFontSize = 10
                    Next
                Else
                    .set_MergeRow(i + 2, True)
                    .set_TextMatrix(i + 2, 0, SeriesName(i))
                    .set_TextMatrix(i + 2, 1, "Reference Curve")
                    .set_TextMatrix(i + 2, 2, .get_TextMatrix(i + 2, 1))
                    .set_TextMatrix(i + 2, 3, .get_TextMatrix(i + 2, 1))
                    .set_TextMatrix(i + 2, 4, .get_TextMatrix(i + 2, 1))
                    .set_TextMatrix(i + 2, 5, .get_TextMatrix(i + 2, 1))
                    .set_TextMatrix(i + 2, 6, .get_TextMatrix(i + 2, 1))
                    .set_TextMatrix(i + 2, 7, .get_TextMatrix(i + 2, 1))
                    .set_TextMatrix(i + 2, 8, .get_TextMatrix(i + 2, 1))
                    .set_TextMatrix(i + 2, 9, .get_TextMatrix(i + 2, 1))
                End If
            Next

        End With
    End Sub

    Private Sub FrmCorrel_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        With Me
            .ScoreGrid.Location = New Point(0, 0)
            .ScoreGrid.Size = New Size(.ClientRectangle.Width, .ClientRectangle.Height - 70)
            .DescripLbl.Location = New Point(.ScoreGrid.Location.X, .ScoreGrid.Height)
            .Range1.Size = New Size(85, 20)
            .Range2.Size = .Range1.Size
            .Range3.Size = .Range1.Size
            .Range4.Size = .Range1.Size

            .Range1.Location = New Point(.ClientRectangle.Width - 350, .DescripLbl.Location.Y)
            .Range2.Location = New Point(.Range1.Location.X + .Range1.Width, .Range1.Location.Y)
            .Range3.Location = New Point(.Range2.Location.X + .Range2.Width, .Range2.Location.Y)
            .Range4.Location = New Point(.Range3.Location.X + .Range3.Width, .Range3.Location.Y)
        End With
    End Sub

    Private Sub ScoreGrid_ClickEvent(ByVal sender As Object, ByVal e As System.EventArgs) Handles ScoreGrid.ClickEvent
        With Me
            .StatusLbl1.ForeColor = Color.Black
            If (.ScoreGrid.RowSel - 2) = Ref_Series_Num Then
                .StatusLbl1.ForeColor = Color.Red
                .StatusLbl1.Text = "Reference Curve : " & .ScoreGrid.get_TextMatrix(.ScoreGrid.RowSel, 0)
            Else
                .StatusLbl1.Text = .ScoreGrid.get_TextMatrix(.ScoreGrid.RowSel, 0)
            End If
        End With
    End Sub

    Private Sub ScoreGrid_SelChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles ScoreGrid.SelChange
        With Me
            .StatusLbl1.ForeColor = Color.Black
            If (.ScoreGrid.RowSel - 2) = Ref_Series_Num Then
                .StatusLbl1.ForeColor = Color.Red
                .StatusLbl1.Text = "Reference Curve : " & .ScoreGrid.get_TextMatrix(.ScoreGrid.RowSel, 0)
            Else
                .StatusLbl1.Text = .ScoreGrid.get_TextMatrix(.ScoreGrid.RowSel, 0)
            End If
        End With
    End Sub
End Class