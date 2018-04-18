Imports System.Windows.Forms.DataVisualization.Charting.Chart
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.IO
Imports ReadingR64

Public Class FrmInjuryGraph_THOR

    Dim Path() As String
    Dim Names() As String
    Dim ConversionF() As Decimal
    Dim DATA_Pos(,,) As Integer
    Dim DATA_Tot_Len(,,) As Integer

    Dim TimeVals() As Decimal
    Dim DataVals() As Double
    Public GraphCollection As New Collection()
    Public AddGraphCollection As New Collection()

    '메뉴중에서 전체 차트에 옵션을 적용하는 것 (범례 On/Off나 위치 설정)
    Dim WholeChartOption As New TEST_Graphing

    'for TEST DATA Previw
    Public DATAPreviewDP As New TEST_Graphing

    Dim DATAGraphDP As New DATA_Graphing

    Dim TestFilePath() As String
    Dim SelDummyPara As Integer ' 50/5/-1
    Public DAT_Headers(,) As String ' 시험 데이터 헤더저장 변수
    '- (Index:(From 0~),0-데이터이름/1-시작위치/2-길이(갯수)/3-시작옵셋값/4-증분/5-Implicit여부/6-단위)
    Public ExtDetails(37) As String '해석파일 확장자 주석

    'TDM을 구분하는 변수
    Public IsTDMfile() As Boolean
    Dim TDMCHselEventPass As Boolean = False
    Dim Tmp_CH_Group_Name_Sel() As String 'TDM의 채널 그룹명 저장

    Dim EventPass As Boolean '시험/해석 파일리스트 클릭해서 항목 바뀔때 초기화시 이벤트 호출을 막는다.
    Dim ChkLstEventPass As Boolean 'SeriesLstBox 의 클릭 이벤트 조절용

    'Tab3에서 해석데이터 Channel을 클릭시 데이터를 파악하기 위한 변수
    Dim RowParaTab3 As String
    Dim ColparaTab3 As String

    '차트 데이터 복사를 위한 변수 (복사할 차트 / 대상 차트)
    Dim MovinChrt As Chart
    Dim TargetChrt As Chart

    Private Sub FrmInjuryGraph_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        MainMDI.Statuslbl.Text = "Status"
    End Sub

    Private Sub FrmInjuryGraph_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        MainMDI.Statuslbl.Text = "Injury Data Plotting"

        Me.ToolTip1.SetToolTip(Me.BtnAppMain, "Ctrl + Click : Apply All Graph (Title / Series Color)")

        With Me
            '.Text = "Injury Graph"
            .Width = 1200
            .Height = 800
            .GraphTab.Alignment = TabAlignment.Bottom
            .GraphTab.TabPages(0).Text = "Injury Graph Page 1."
            .GraphTab.TabPages(1).Text = "Injury Graph Page 2."
            .GraphTab.TabPages(2).Text = "Injury Graph Page 3."
            .GraphTab.TabPages(3).Text = "Additional DATA"

            .ChLstAnal.HorizontalScrollbar = True

            EventPass = True
            .ExtLst.Items.Add(".control")
            .ExtLst.Items.Add(".rds")
            .ExtLst.Items.Add(".lac")
            .ExtLst.Items.Add(".injury")
            .ExtLst.Items.Add(".aac")
            .ExtLst.Items.Add(".avl")
            .ExtLst.Items.Add(".ads")
            .ExtLst.Items.Add(".aps")
            .ExtLst.Items.Add(".cntfrc")
            .ExtLst.Items.Add(".dvl")
            .ExtLst.Items.Add(".fhs")
            .ExtLst.Items.Add(".frc")
            .ExtLst.Items.Add(".jps")
            .ExtLst.Items.Add(".jvl")
            .ExtLst.Items.Add(".jac")
            .ExtLst.Items.Add(".lds")
            .ExtLst.Items.Add(".lps")
            .ExtLst.Items.Add(".lvl")
            .ExtLst.Items.Add(".out")
            .ExtLst.Items.Add(".rlg")
            .ExtLst.Items.Add(".rtf")
            .ExtLst.Items.Add(".rtt")
            .ExtLst.Items.Add(".bds")
            .ExtLst.Items.Add(".can")
            .ExtLst.Items.Add(".cogout")
            .ExtLst.Items.Add(".energy")
            .ExtLst.Items.Add(".enggrp")
            .ExtLst.Items.Add(".engmat")
            .ExtLst.Items.Add(".fan")
            .ExtLst.Items.Add(".msl")
            .ExtLst.Items.Add(".pen")
            .ExtLst.Items.Add(".ptr")
            .ExtLst.Items.Add(".sector")
            .ExtLst.Items.Add(".tst")
            .ExtLst.Items.Add(".tq1")
            .ExtLst.Items.Add(".tq2")
            .ExtLst.Items.Add(".tq3")
            .ExtLst.Items.Add(".tyr")
            EventPass = False
        End With

        ExtDetails(0) = "CONTROL - Sensor and Control File"
        ExtDetails(1) = "RELDIS - Relative Displacement"
        ExtDetails(2) = "LINACC - Linear Accelerations"
        ExtDetails(3) = "INJURY - Injuries"
        ExtDetails(4) = "ANGACC - Angular Accelerations"
        ExtDetails(5) = "ANGVEL - Angular Velocities"
        ExtDetails(6) = "ANGDIS - Angular Displacement"
        ExtDetails(7) = "ANGPOS - Angular Positions"
        ExtDetails(8) = "CNTFRC - Contact Force"
        ExtDetails(9) = "DISVEL - Distance between 2 Point and Its time derivative"
        ExtDetails(10) = "AirBag related output"
        ExtDetails(11) = "FORCES - Belt and Restraint Force"
        ExtDetails(12) = "JNTPOS - Joint Position"
        ExtDetails(13) = "JNTVEL - Joint Velocities"
        ExtDetails(14) = "JNTACC - Joint Accelerations"
        ExtDetails(15) = "LINDIS - Linear Displacements"
        ExtDetails(16) = "LINPOS - Linear Positions"
        ExtDetails(17) = "LINVEL - Linear Velocities"
        ExtDetails(18) = "OUTLET - Retractor / Pretensioner / Load Limiter / Tying"
        ExtDetails(19) = "RELONG - Relative Elogations"
        ExtDetails(20) = "REACTF - Joint Constraint Force"
        ExtDetails(21) = "REACTT - Joint Constraint Torque"
        ExtDetails(22) = "BODSTS - Body State"
        ExtDetails(23) = "CARANG - Cardan Restraint Angle"
        ExtDetails(24) = "COGOUT - Centre of Gravity Output File"
        ExtDetails(25) = "ENERGY - Energy Output"
        ExtDetails(26) = "ENGGRP - Energy Output"
        ExtDetails(27) = "ENGMAT - FEM Material Energy Output"
        ExtDetails(28) = "FLEANG - Flexion-Torsion Restraint Angles"
        ExtDetails(29) = "MUSCLE - Muscle Data"
        ExtDetails(30) = "PENETR - Contact Penetration / Area"
        ExtDetails(31) = "PNTRST - Extra Point-Restraint Output"
        ExtDetails(32) = "SECFOR - Cross Section Force"
        ExtDetails(33) = "TISTEP - Integration Time Step"
        ExtDetails(34) = "TORQU1 - Cardan Restraint Torque"
        ExtDetails(35) = "TORQU2 - Flexion-Torsion Restraint Torque"
        ExtDetails(36) = "TORQU3 - Dynamic Joint Load"
        ExtDetails(37) = "TYRES - Tyre Ouput Data"

        EventPass = True
        For i = 0 To UBound(GraphTitle_THOR) - 1
            Me.ListBox1.Items.Add(GraphTitle_THOR(i))
        Next
        For i = 0 To 7
            Me.ListBox1.Items.Add(GraphTitle_THOR(UBound(GraphTitle_THOR)) & " #" & i + 1)
        Next
        EventPass = False

        Me.SplitContainer1.SplitterDistance = 180
        Me.SplitContainer2.SplitterDistance = 600
        Me.SplitContainer3.SplitterDistance = 400
    End Sub

    Public Sub New(ByVal TFilePath() As String, ByVal FileNames() As String, ByVal para_arr1(,,) As Integer, ByVal para_arr2(,,) As Integer, ByVal C_Factor() As Decimal)
        '         (FilePathGet(Me.OpenDlg.FileNames), OpenFile.names, ParaMeterArr1, ParaMeterArr2, ConversionFactor)


        ' 이 호출은 디자이너에 필요합니다.
        InitializeComponent()
        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하십시오.

        MainMDI.Statuslbl.Text = "Display Injury DATA...."

        Dim i, k As Integer

        MainMDI.ProgressBarMain.Value = 100

        With Me
            'Collection의 인덱스는 1부터 시작한다
            GraphCollection.Add(.Chart00)
            GraphCollection.Add(.Chart01)
            GraphCollection.Add(.Chart02)
            GraphCollection.Add(.Chart03)
            GraphCollection.Add(.Chart04)
            GraphCollection.Add(.Chart05)
            GraphCollection.Add(.Chart06)
            GraphCollection.Add(.Chart07)
            GraphCollection.Add(.Chart08)
            GraphCollection.Add(.Chart09)
            GraphCollection.Add(.Chart10)
            GraphCollection.Add(.Chart11)
            GraphCollection.Add(.Chart12)
            GraphCollection.Add(.Chart13)
            GraphCollection.Add(.Chart14)
            GraphCollection.Add(.Chart15)
            GraphCollection.Add(.Chart16)
            GraphCollection.Add(.Chart17)
            GraphCollection.Add(.Chart18)
            GraphCollection.Add(.Chart19)
            GraphCollection.Add(.Chart20)
            GraphCollection.Add(.Chart21)
            GraphCollection.Add(.Chart22)
            GraphCollection.Add(.Chart23)
            GraphCollection.Add(.Chart24)
            GraphCollection.Add(.Chart25)
            GraphCollection.Add(.Chart26)
            GraphCollection.Add(.Chart27)
            GraphCollection.Add(.Chart28)
            GraphCollection.Add(.Chart29)
            GraphCollection.Add(.Chart30)
            GraphCollection.Add(.Chart31)
            GraphCollection.Add(.Chart32)
            GraphCollection.Add(.Chart33)
            GraphCollection.Add(.Chart34)
            GraphCollection.Add(.Chart35)
            GraphCollection.Add(.Chart36)
            GraphCollection.Add(.Chart37)
            GraphCollection.Add(.Chart38)
            GraphCollection.Add(.Chart39)
            GraphCollection.Add(.Chart40)
            GraphCollection.Add(.Chart41)
            GraphCollection.Add(.Chart42)
            GraphCollection.Add(.Chart43)
            GraphCollection.Add(.Chart44)
            GraphCollection.Add(.Chart45)
            GraphCollection.Add(.Chart46)
            GraphCollection.Add(.Chart47)

            '추가 그래프
            AddGraphCollection.Add(.Chart48)
            AddGraphCollection.Add(.Chart49)
            AddGraphCollection.Add(.Chart50)
            AddGraphCollection.Add(.Chart51)
            AddGraphCollection.Add(.Chart52)
            AddGraphCollection.Add(.Chart53)
            AddGraphCollection.Add(.Chart54)
            AddGraphCollection.Add(.Chart55)
        End With

        '그래프에 있는 모든 시리즈를 삭제 (초기화)
        MainMDI.ProgressBarMain.Maximum = 580
        For i = 1 To 48
            MainMDI.ProgressBarMain.Value = 100 + i * 10
            DelAllSeries(GraphCollection.Item(i))
        Next

        '폴더 경로 (\포함)
        Path = TFilePath
        ReDim Names(UBound(FileNames))

        MainMDI.ProgressBarMain.Maximum = 970 + UBound(FileNames) * 300

        'Case 파일명 (확장자 없음)
        Names = FileNames
        '해석파일명 Tab3에 추가
        For i = 0 To UBound(Names)
            Me.AnalFileLst.Items.Add(Names(i))
        Next
        '데이터의 위치를 파악하는 변수 (그래프 번호 / Case 번호 / Row 번호 / Col 번호)
        DATA_Pos = para_arr1
        '데이터의 총 행/열 수를 파악하는 변수 (그래프 번호 / Case 번호 / Row 번호 / Col 번호)
        DATA_Tot_Len = para_arr2

        ConversionF = C_Factor

        MainMDI.ProgressBarMain.Value = 500

        'TimeVals, DataVals에 데이터를 읽어온다.=================================================================================================================

        For k = 0 To UBound(Names)
            For i = 0 To 47
                '프로그래스바 표시
                MainMDI.ProgressBarMain.Value = 500 + i * 10 + k * 300
                '데이터를 읽어오는 함수
                Analopen.DataReading(Path(k), Names(k), Graph_Ext_THOR(i), DATA_Pos(i, k, 0), DATA_Pos(i, k, 1), DATA_Tot_Len(i, k, 0), DATA_Tot_Len(i, k, 1), TimeVals, DataVals, C_Factor(i))
                '데이터를 그래프에 올리는 함수
                AddSeries(GraphCollection.Item(i + 1), TimeVals, DataVals, k + 1)
                'Display 정리
                DATAGraphDP.GraphAxisLine(GraphCollection(i + 1))
            Next
        Next

        i = 0
        For Each Chrt As Chart In GraphCollection
            Chrt.Titles.Add(GraphTitle_THOR(i))
            Chrt.Titles(0).Alignment = ContentAlignment.TopLeft
            Chrt.Titles(0).Font = New Font("맑은 고딕", 9, FontStyle.Bold)
            i = i + 1
        Next

        MainMDI.ProgressBarMain.Value = MainMDI.ProgressBarMain.Maximum
        MainMDI.Statuslbl.Text = "Complete."

        '========================================================================================================================================================
    End Sub

    Private Sub DelAllSeries(ByRef Charts As Windows.Forms.DataVisualization.Charting.Chart)
        Dim i As Integer

        For i = Charts.Series.Count To 1 Step -1
            Charts.Series.RemoveAt(i - 1)
        Next

    End Sub

    Private Sub AddSeries(ByRef Charts As Windows.Forms.DataVisualization.Charting.Chart, ByRef Xaxis() As Decimal, ByRef Yaxis() As Double, ByVal CaseNum As Integer)
        With Charts
            .Series.Add("MD " & CaseNum)
            .Series("MD " & CaseNum).Points.DataBindXY(Xaxis, Yaxis)
            .Series("MD " & CaseNum).ChartType = DataVisualization.Charting.SeriesChartType.Line
        End With
    End Sub

    Private Sub FrmInjuryGraph_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        Call SplitContainer2_SplitterMoved(sender, Nothing)

        If Me.Size.Width > 100 And Me.Size.Height > 100 Then
            With Me
                '.GraphTab.Location = New Point(5, 25)
                '.GraphTab.Size = New Size(.ClientSize.Width - 10, .ClientSize.Height - 10 - 20)

                '그래프 크기
                For Each Ctrl As Control In GraphCollection
                    Ctrl.Size = New Size((.GraphTab.ClientSize.Width - 10) / 4, (.GraphTab.ClientSize.Height - 20) / 4 - 2)
                Next

                .Chart00.Location = New Point(5, 5)
                .Chart01.Location = New Point(5 + .Chart00.Width, 5)
                .Chart02.Location = New Point(5 + .Chart00.Width * 2, 5)
                .Chart03.Location = New Point(5 + .Chart00.Width * 3, 5)
                .Chart04.Location = New Point(5, 5 + .Chart00.Height)
                .Chart05.Location = New Point(5 + .Chart00.Width, 5 + .Chart00.Height)
                .Chart06.Location = New Point(5 + .Chart00.Width * 2, 5 + .Chart00.Height)
                .Chart07.Location = New Point(5 + .Chart00.Width * 3, 5 + .Chart00.Height)
                .Chart08.Location = New Point(5, 5 + .Chart00.Height * 2)
                .Chart09.Location = New Point(5 + .Chart00.Width, 5 + .Chart00.Height * 2)
                .Chart10.Location = New Point(5 + .Chart00.Width * 2, 5 + .Chart00.Height * 2)
                .Chart11.Location = New Point(5 + .Chart00.Width * 3, 5 + .Chart00.Height * 2)
                .Chart12.Location = New Point(5, 5 + .Chart00.Height * 3)
                .Chart13.Location = New Point(5 + .Chart00.Width, 5 + .Chart00.Height * 3)
                .Chart14.Location = New Point(5 + .Chart00.Width * 2, 5 + .Chart00.Height * 3)
                .Chart15.Location = New Point(5 + .Chart00.Width * 3, 5 + .Chart00.Height * 3)

                .Chart16.Location = New Point(5, 5)
                .Chart17.Location = New Point(5 + .Chart16.Width, 5)
                .Chart18.Location = New Point(5 + .Chart16.Width * 2, 5)
                .Chart19.Location = New Point(5 + .Chart16.Width * 3, 5)
                .Chart20.Location = New Point(5, 5 + .Chart16.Height)
                .Chart21.Location = New Point(5 + .Chart16.Width, 5 + .Chart16.Height)
                .Chart22.Location = New Point(5 + .Chart16.Width * 2, 5 + .Chart16.Height)
                .Chart23.Location = New Point(5 + .Chart16.Width * 3, 5 + .Chart16.Height)
                .Chart24.Location = New Point(5, 5 + .Chart16.Height * 2)
                .Chart25.Location = New Point(5 + .Chart16.Width, 5 + .Chart16.Height * 2)
                .Chart26.Location = New Point(5 + .Chart16.Width * 2, 5 + .Chart16.Height * 2)
                .Chart27.Location = New Point(5 + .Chart16.Width * 3, 5 + .Chart16.Height * 2)
                .Chart28.Location = New Point(5, 5 + .Chart16.Height * 3)
                .Chart29.Location = New Point(5 + .Chart16.Width, 5 + .Chart16.Height * 3)
                .Chart30.Location = New Point(5 + .Chart16.Width * 2, 5 + .Chart16.Height * 3)
                .Chart31.Location = New Point(5 + .Chart16.Width * 3, 5 + .Chart16.Height * 3)

                .Chart32.Location = New Point(5, 5)
                .Chart33.Location = New Point(5 + .Chart32.Width, 5)
                .Chart34.Location = New Point(5 + .Chart32.Width * 2, 5)
                .Chart35.Location = New Point(5 + .Chart32.Width * 3, 5)
                .Chart36.Location = New Point(5, 5 + .Chart32.Height)
                .Chart37.Location = New Point(5 + .Chart32.Width, 5 + .Chart32.Height)
                .Chart38.Location = New Point(5 + .Chart32.Width * 2, 5 + .Chart32.Height)
                .Chart39.Location = New Point(5 + .Chart32.Width * 3, 5 + .Chart32.Height)
                .Chart40.Location = New Point(5, 5 + .Chart32.Height * 2)
                .Chart41.Location = New Point(5 + .Chart32.Width, 5 + .Chart32.Height * 2)
                .Chart42.Location = New Point(5 + .Chart32.Width * 2, 5 + .Chart32.Height * 2)
                .Chart43.Location = New Point(5 + .Chart32.Width * 3, 5 + .Chart32.Height * 2)
                .Chart44.Location = New Point(5, 5 + .Chart32.Height * 3)
                .Chart45.Location = New Point(5 + .Chart32.Width, 5 + .Chart32.Height * 3)
                .Chart46.Location = New Point(5 + .Chart32.Width * 2, 5 + .Chart32.Height * 3)
                .Chart47.Location = New Point(5 + .Chart32.Width * 3, 5 + .Chart32.Height * 3)

                .AnalBox.Location = New Point(2, 0)
                .AnalBox.Size = New Size((.GraphTab.ClientRectangle.Width - 4) / 4 - 4, .GraphTab.ClientRectangle.Height - 30)
                .Chart48.Location = New Point(.AnalBox.Location.X + .AnalBox.Width + 2, .AnalBox.Location.Y)
                .Chart48.Size = New Size(.AnalBox.Width, .AnalBox.Height / 4)
                .Chart49.Location = New Point(.Chart48.Location.X + .Chart48.Width, .AnalBox.Location.Y)
                .Chart49.Size = New Size(.Chart48.Width, .Chart48.Height)
                .TESTBox.Location = New Point(.Chart49.Location.X + .Chart49.Width + 2, .AnalBox.Location.Y)
                .TESTBox.Size = New Size(.AnalBox.Width + 1, .AnalBox.Height)
                .Chart50.Location = New Point(.Chart48.Location.X, .Chart49.Location.Y + .Chart49.Height)
                .Chart50.Size = New Size(.Chart48.Width, .Chart48.Height)
                .Chart51.Location = New Point(.Chart50.Location.X + .Chart50.Width, .Chart50.Location.Y)
                .Chart51.Size = New Size(.Chart48.Width, .Chart48.Height)
                .Chart52.Location = New Point(.Chart50.Location.X, .Chart50.Location.Y + .Chart50.Height)
                .Chart52.Size = New Size(.Chart48.Width, .Chart48.Height)
                .Chart53.Location = New Point(.Chart52.Location.X + .Chart52.Width, .Chart52.Location.Y)
                .Chart53.Size = New Size(.Chart48.Width, .Chart48.Height)
                .Chart54.Location = New Point(.Chart52.Location.X, .Chart52.Location.Y + .Chart52.Height)
                .Chart54.Size = New Size(.Chart48.Width, .Chart48.Height)
                .Chart55.Location = New Point(.Chart54.Location.X + .Chart54.Width, .Chart54.Location.Y)
                .Chart55.Size = New Size(.Chart48.Width, .Chart48.Height)

                .PreviewAnal.Location = New Point(5, 20)
                .PreviewAnal.Size = New Size(.AnalBox.ClientRectangle.Width - 10, .AnalBox.ClientRectangle.Height / 3)
                .XScaleLblAnal.Location = New Point(5, .PreviewAnal.Location.Y + .PreviewAnal.Height + 5 + 16)
                .AllSeriesChk.Location = New Point(5, .PreviewAnal.Location.Y + .PreviewAnal.Height)

                .XscaleAnal.Location = New Point(5, .XScaleLblAnal.Location.Y + .XScaleLblAnal.Height + 5)
                .XscaleAnal.Size = New Size(.PreviewAnal.Width / 4, 21)
                .YscaleAnal.Size = .XscaleAnal.Size
                .XoffsetAnal.Size = .XscaleAnal.Size
                .YoffsetAnal.Size = .XscaleAnal.Size
                .YscaleAnal.Location = New Point(.XscaleAnal.Location.X + .XscaleAnal.Width, .XscaleAnal.Location.Y)
                .YScaleLblAnal.Location = New Point(.YscaleAnal.Location.X, .XScaleLblAnal.Location.Y)
                .XoffsetAnal.Location = New Point(.YscaleAnal.Location.X + .YscaleAnal.Width, .YscaleAnal.Location.Y)
                .XOffsetLblAnal.Location = New Point(.XoffsetAnal.Location.X, .XScaleLblAnal.Location.Y)
                .YoffsetAnal.Location = New Point(.XoffsetAnal.Location.X + .XoffsetAnal.Width, .XoffsetAnal.Location.Y)
                .YOffsetLblAnal.Location = New Point(.YoffsetAnal.Location.X, .XScaleLblAnal.Location.Y)
                .XScaleLblAnal.Width = .XscaleAnal.Width
                .YScaleLblAnal.Width = .XscaleAnal.Width
                .XOffsetLblAnal.Width = .XscaleAnal.Width
                .YOffsetLblAnal.Width = .XscaleAnal.Width
                .AnalFileLst.Location = New Point(.PreviewAnal.Location.X, .XscaleAnal.Location.Y + .XscaleAnal.Height + 5)
                .AnalFileLst.Size = New Size(.PreviewAnal.Width, 83)
                .ExtLst.Location = New Point(.PreviewAnal.Location.X, .AnalFileLst.Location.Y + .AnalFileLst.Height + 8)
                .ExtLst.Size = New Size(.PreviewAnal.Width * 0.3 - 5, AnalBox.ClientRectangle.Height - .ExtLst.Location.Y - 10)
                .CompLstAnal.Location = New Point(.ExtLst.Location.X + .ExtLst.Width + 5, .AnalBox.ClientRectangle.Height - 84 + 5)
                .CompLstAnal.Size = New Size(.PreviewAnal.Width * 0.7, 84)
                .ChLstAnal.Location = New Point(.CompLstAnal.Location.X, .AnalFileLst.Location.Y + .AnalFileLst.Height + 8)
                .ChLstAnal.Size = New Size(.CompLstAnal.Width, .CompLstAnal.Location.Y - .ChLstAnal.Location.Y - 2)

                .PreviewTEST.Location = New Point(5, 20)
                .PreviewTEST.Size = New Size(.AnalBox.ClientRectangle.Width - 10, .TESTBox.ClientRectangle.Height / 3)
                .XScaleLbl.Location = New Point(5, .PreviewTEST.Location.Y + .PreviewTEST.Height + 10)
                .Xscale.Location = New Point(5, .XScaleLbl.Location.Y + .XScaleLbl.Height + 5)
                .Xscale.Size = New Size(.PreviewTEST.Width / 4, 21)
                .Yscale.Size = .Xscale.Size
                .Xoffset.Size = .Xscale.Size
                .Yoffset.Size = .Xscale.Size
                .Yscale.Location = New Point(.Xscale.Location.X + .Xscale.Width, .Xscale.Location.Y)
                .YScaleLbl.Location = New Point(.Yscale.Location.X, .XScaleLbl.Location.Y)
                .Xoffset.Location = New Point(.Yscale.Location.X + .Yscale.Width, .Yscale.Location.Y)
                .XOffsetLbl.Location = New Point(.Xoffset.Location.X, .XScaleLbl.Location.Y)
                .Yoffset.Location = New Point(.Xoffset.Location.X + .Xoffset.Width, .Xoffset.Location.Y)
                .YOffsetLbl.Location = New Point(.Yoffset.Location.X, .XScaleLbl.Location.Y)
                .XScaleLbl.Width = .Xscale.Width
                .YScaleLbl.Width = .Xscale.Width
                .XOffsetLbl.Width = .Xscale.Width
                .YOffsetLbl.Width = .Xscale.Width

                .TestFileLst.Location = New Point(5, .Xscale.Location.Y + .Xscale.Height + 10)
                .TestFileLst.Size = New Size(.PreviewTEST.Width, 21 * 3)
                .TDMchgroup.Location = New Point(5, .TestFileLst.Location.Y + .TestFileLst.Height + 10)
                .TDMchgroup.Size = New Size(.TestFileLst.Width, 25)
                .CmbXaxis.Location = New Point(5, .TDMchgroup.Location.Y + .TDMchgroup.Height + 10)
                .CmbXaxis.Width = .PreviewTEST.Width
                .ChList.Location = New Point(5, .CmbXaxis.Location.Y + .CmbXaxis.Height + 5)
                .ChList.Size = New Size(.PreviewTEST.Width, .TESTBox.ClientRectangle.Height - 5 - .ChList.Location.Y)
            End With
        End If
    End Sub

    Private Sub Chart00_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart00.DoubleClick

        Dim PopUpForm As New InjuryPopUp(0, Me.Chart00, 1) ', Me.Chart00.Titles(0).Text)

        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart01_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart01.DoubleClick

        Dim PopUpForm As New InjuryPopUp(1, Me.Chart01, 1) ', Me.Chart01.Titles(0).Text)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart02_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart02.DoubleClick

        Dim PopUpForm As New InjuryPopUp(2, Me.Chart02, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart03_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart03.DoubleClick

        Dim PopUpForm As New InjuryPopUp(3, Me.Chart03, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart04_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart04.DoubleClick

        Dim PopUpForm As New InjuryPopUp(4, Me.Chart04, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart05_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart05.DoubleClick

        Dim PopUpForm As New InjuryPopUp(5, Me.Chart05, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart06_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart06.DoubleClick

        Dim PopUpForm As New InjuryPopUp(6, Me.Chart06, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart07_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart07.DoubleClick

        Dim PopUpForm As New InjuryPopUp(7, Me.Chart07, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart08_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart08.DoubleClick

        Dim PopUpForm As New InjuryPopUp(8, Me.Chart08, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart09_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart09.DoubleClick

        Dim PopUpForm As New InjuryPopUp(9, Me.Chart09, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart10_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart10.DoubleClick

        Dim PopUpForm As New InjuryPopUp(10, Me.Chart10, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart11_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart11.DoubleClick

        Dim PopUpForm As New InjuryPopUp(11, Me.Chart11, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart12_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart12.DoubleClick

        Dim PopUpForm As New InjuryPopUp(12, Me.Chart12, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart13_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart13.DoubleClick

        Dim PopUpForm As New InjuryPopUp(13, Me.Chart13, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart14_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart14.DoubleClick

        Dim PopUpForm As New InjuryPopUp(14, Me.Chart14, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart15_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart15.DoubleClick

        Dim PopUpForm As New InjuryPopUp(15, Me.Chart15, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart16_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart16.DoubleClick

        Dim PopUpForm As New InjuryPopUp(16, Me.Chart16, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart17_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart17.DoubleClick

        Dim PopUpForm As New InjuryPopUp(17, Me.Chart17, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart18_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart18.DoubleClick

        Dim PopUpForm As New InjuryPopUp(18, Me.Chart18, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart19_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart19.DoubleClick

        Dim PopUpForm As New InjuryPopUp(19, Me.Chart19, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart20_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart20.DoubleClick

        Dim PopUpForm As New InjuryPopUp(20, Me.Chart20, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart21_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart21.DoubleClick

        Dim PopUpForm As New InjuryPopUp(21, Me.Chart21, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart22_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart22.DoubleClick

        Dim PopUpForm As New InjuryPopUp(22, Me.Chart22, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart23_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart23.DoubleClick

        Dim PopUpForm As New InjuryPopUp(23, Me.Chart23, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart24_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart24.DoubleClick

        Dim PopUpForm As New InjuryPopUp(24, Me.Chart24, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart25_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart25.DoubleClick

        Dim PopUpForm As New InjuryPopUp(25, Me.Chart25, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart26_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart26.DoubleClick

        Dim PopUpForm As New InjuryPopUp(26, Me.Chart26, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart27_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart27.DoubleClick

        Dim PopUpForm As New InjuryPopUp(27, Me.Chart27, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart28_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart28.DoubleClick

        Dim PopUpForm As New InjuryPopUp(28, Me.Chart28, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart29_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart29.DoubleClick

        Dim PopUpForm As New InjuryPopUp(29, Me.Chart29, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart30_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart30.DoubleClick

        Dim PopUpForm As New InjuryPopUp(30, Me.Chart30, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart31_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart31.DoubleClick

        Dim PopUpForm As New InjuryPopUp(31, Me.Chart31, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart32_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart32.DoubleClick

        Dim PopUpForm As New InjuryPopUp(32, Me.Chart32, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart33_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart33.DoubleClick

        Dim PopUpForm As New InjuryPopUp(33, Me.Chart33, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart34_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart34.DoubleClick

        Dim PopUpForm As New InjuryPopUp(34, Me.Chart34, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart35_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart35.DoubleClick

        Dim PopUpForm As New InjuryPopUp(35, Me.Chart35, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart36_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart36.DoubleClick

        Dim PopUpForm As New InjuryPopUp(36, Me.Chart36, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart37_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart37.DoubleClick

        Dim PopUpForm As New InjuryPopUp(37, Me.Chart37, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart38_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart38.DoubleClick

        Dim PopUpForm As New InjuryPopUp(38, Me.Chart38, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart39_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart39.DoubleClick

        Dim PopUpForm As New InjuryPopUp(39, Me.Chart39, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart40_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart40.DoubleClick

        Dim PopUpForm As New InjuryPopUp(40, Me.Chart40, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart41_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart41.DoubleClick

        Dim PopUpForm As New InjuryPopUp(41, Me.Chart41, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart42_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart42.DoubleClick

        Dim PopUpForm As New InjuryPopUp(42, Me.Chart42, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart43_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart43.DoubleClick

        Dim PopUpForm As New InjuryPopUp(43, Me.Chart43, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart44_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart44.DoubleClick

        Dim PopUpForm As New InjuryPopUp(44, Me.Chart44, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart45_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart45.DoubleClick

        Dim PopUpForm As New InjuryPopUp(45, Me.Chart45, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart46_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart46.DoubleClick

        Dim PopUpForm As New InjuryPopUp(46, Me.Chart46, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub
    Private Sub Chart47_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart47.DoubleClick

        Dim PopUpForm As New InjuryPopUp(47, Me.Chart47, 1)
        PopUpForm.Owner = Me
        PopUpForm.Show()

    End Sub

    Private Sub DRVToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DRVToolStripMenuItem.Click

        SelDummyPara = 50

        MainMDI.Statuslbl.Text = "Select TEST DATA File (DAT file)"

        With Me.OpenTestDlg
            .Title = "Header 파일 선택 [한개 파일 선택]"
            .Multiselect = False
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "Diadem DATA File|*.DAT;*.tdm|모든 파일|*.*"
            .ShowDialog()
        End With

        '파일 선택이 없는 경우
        If Me.OpenTestDlg.FileNames(0) = "" Then
            Exit Sub
        End If

        'TestFileLst
        '파일명 추가 및 파일 경로 저장
        ReDim Preserve TestFilePath(Me.TestFileLst.Items.Count + UBound(Me.OpenTestDlg.FileNames))

        'TDM 식별
        ReDim Preserve IsTDMfile(Me.TestFileLst.Items.Count + UBound(Me.OpenTestDlg.FileNames))
        If Mid(Me.OpenTestDlg.FileNames(0), Len(Me.OpenTestDlg.FileNames(0)) - 2, 4) = "tdm" Then
            IsTDMfile(Me.TestFileLst.Items.Count + UBound(Me.OpenTestDlg.FileNames)) = True
        Else
            IsTDMfile(Me.TestFileLst.Items.Count + UBound(Me.OpenTestDlg.FileNames)) = False
        End If

        For i = 0 To UBound(Me.OpenTestDlg.FileNames)
            TestFilePath(Me.TestFileLst.Items.Count + i) = FilePathGet(Me.OpenTestDlg.FileNames)
            Me.TestFileLst.Items.Add(Me.OpenTestDlg.SafeFileNames(i))
        Next
        Me.TestFileLst.SelectedIndex = 0

        TESToverlap(SelDummyPara, Me.TestFileLst.Items(Me.TestFileLst.Items.Count - 1), FilePathGet(Me.OpenTestDlg.FileNames)) 'TestFilePath(UBound(TestFilePath)), FilePathGet(Me.OpenTestDlg.FileNames)) ' 
    End Sub
    Private Sub PASToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PASToolStripMenuItem.Click

        SelDummyPara = 5

        With Me.OpenTestDlg
            .Title = "Header 파일 선택 [한개 파일 선택]"
            .Multiselect = False
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "Diadem DATA File|*.DAT;*.tdm|모든 파일|*.*"
            .ShowDialog()
        End With

        '파일 선택이 없는 경우
        If Me.OpenTestDlg.FileNames(0) = "" Then
            Exit Sub
        End If

        'TestFileLst
        '파일명 추가 및 파일 경로 저장
        ReDim Preserve TestFilePath(Me.TestFileLst.Items.Count + UBound(Me.OpenTestDlg.FileNames))

        'TDM 식별
        ReDim Preserve IsTDMfile(Me.TestFileLst.Items.Count + UBound(Me.OpenTestDlg.FileNames))
        If Mid(Me.OpenTestDlg.FileNames(0), Len(Me.OpenTestDlg.FileNames(0)) - 2, 4) = "tdm" Then
            IsTDMfile(Me.TestFileLst.Items.Count + UBound(Me.OpenTestDlg.FileNames)) = True
        Else
            IsTDMfile(Me.TestFileLst.Items.Count + UBound(Me.OpenTestDlg.FileNames)) = False
        End If

        For i = 0 To UBound(Me.OpenTestDlg.FileNames)
            TestFilePath(Me.TestFileLst.Items.Count + i) = FilePathGet(Me.OpenTestDlg.FileNames)
            Me.TestFileLst.Items.Add(Me.OpenTestDlg.SafeFileNames(i))
        Next
        Me.TestFileLst.SelectedIndex = 0

        TESToverlap(SelDummyPara, Me.TestFileLst.Items(Me.TestFileLst.Items.Count - 1), FilePathGet(Me.OpenTestDlg.FileNames)) 'TestFilePath(UBound(TestFilePath)), FilePathGet(Me.OpenTestDlg.FileNames)) ' 
    End Sub
    Private Sub EtcToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EtcToolStripMenuItem.Click

        SelDummyPara = -1

        With Me.OpenTestDlg
            .Title = "Header 파일 선택 [한개 파일 선택]"
            .Multiselect = False
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "Diadem DATA File|*.DAT;*.tdm|모든 파일|*.*"
            .ShowDialog()
        End With

        '파일 선택이 없는 경우
        If Me.OpenTestDlg.FileNames(0) = "" Then
            Exit Sub
        End If

        'TestFileLst
        '파일명 추가 및 파일 경로 저장
        ReDim Preserve TestFilePath(Me.TestFileLst.Items.Count + UBound(Me.OpenTestDlg.FileNames))

        'TDM 식별
        ReDim Preserve IsTDMfile(Me.TestFileLst.Items.Count + UBound(Me.OpenTestDlg.FileNames))
        If Mid(Me.OpenTestDlg.FileNames(0), Len(Me.OpenTestDlg.FileNames(0)) - 2, 4) = "tdm" Then
            IsTDMfile(Me.TestFileLst.Items.Count + UBound(Me.OpenTestDlg.FileNames)) = True
        Else
            IsTDMfile(Me.TestFileLst.Items.Count + UBound(Me.OpenTestDlg.FileNames)) = False
        End If

        For i = 0 To UBound(Me.OpenTestDlg.FileNames)
            TestFilePath(Me.TestFileLst.Items.Count + i) = FilePathGet(Me.OpenTestDlg.FileNames)
            Me.TestFileLst.Items.Add(Me.OpenTestDlg.SafeFileNames(i))
        Next
        Me.TestFileLst.SelectedIndex = 0

        TESToverlap(SelDummyPara, Me.TestFileLst.Items(Me.TestFileLst.Items.Count - 1), FilePathGet(Me.OpenTestDlg.FileNames)) 'TestFilePath(UBound(TestFilePath)), FilePathGet(Me.OpenTestDlg.FileNames)) ' 
    End Sub
    Private Sub TESToverlap(ByVal Dummy As Integer, ByVal DATAFileName As String, Optional ByVal DATAFilePath As String = "")

        '데이터 헤더를 읽어온다.=================================================================================
        If Mid(DATAFileName, Len(DATAFileName) - 2, 4) = "tdm" Then

            'TDM일 경우 그룹을 선택해야 한다.**************************************************************************************************
            '채널 그룹을 선택하게 해야한다.==================
            Dim TmpHeaderFile As StreamReader
            Dim Tmp_read As String
            Dim kk As Integer = 0
            Dim Paths As String = DATAFilePath & DATAFileName

            TmpHeaderFile = New StreamReader(Paths)

            ReDim Tmp_CH_Group_Name_Sel(0)

            Do While Not TmpHeaderFile.EndOfStream
                Tmp_read = TmpHeaderFile.ReadLine
                If InStr(Tmp_read, "<tdm_channelgroup id=""") > 0 Then
                    Tmp_read = TmpHeaderFile.ReadLine
                    kk = kk + 1
                    ReDim Preserve Tmp_CH_Group_Name_Sel(kk)
                    Tmp_CH_Group_Name_Sel(kk) = BetweenEle(Tmp_read)

                    Do While Not InStr(Tmp_read, "</tdm_channelgroup>") > 1
                        Tmp_read = TmpHeaderFile.ReadLine
                        '채널그룹의 이름식별=================
                        If InStr(Tmp_read, "<name>") > 0 Then
                            Tmp_CH_Group_Name_Sel(kk) = BetweenEle(Tmp_read)
                        End If
                    Loop
                End If
            Loop

            If UBound(Tmp_CH_Group_Name_Sel) = 0 Then
                TDM_Sel_CHGroup_Num = 1
            Else
                Dim SelTDMGroup As New FrmSelTDMCH(Tmp_CH_Group_Name_Sel)
                SelTDMGroup.ShowDialog(Me)
            End If
            '********************************************************************************************************************************
            DAT_Headers = OepnTDMfileHeader(DATAFilePath & DATAFileName, TDM_Sel_CHGroup_Num)
        Else
            DAT_Headers = OepnDATfileHeader(DATAFilePath & DATAFileName) '(TestFilePath(Me.TestFileLst.SelectedIndex) & Me.TestFileLst.SelectedItem)
            '- (Index:(From 0~),0-데이터이름/1-시작위치/2-길이(갯수)/3-시작옵셋값/4-증분/5-Implicit여부/6-단위)
        End If
        '========================================================================================================
        'Me.TestFileLst.

        '그래프 파라미터 위치를 파악한다.========================================================================
        Dim i, j, k, m, n, t, Marker, p As Integer
        Dim TEST_Para_Num(47, 1) As Integer   '그래프 파라미터 인덱스를 저장한다.
        Dim Tmp_StrX() As String
        Dim Tmp_StrY() As String
        Dim Tmp_Char() As String
        Dim FindDummy() As String = Nothing

        '범례에 데이터 파일명을 표시함
        '   ex> ==> 운전석을 읽고, 또 동승석을 읽는경우 파일 이름이 같아서 읽지 않는 경우가 생기므로
        '           범례를 쓸 때 아예 표시해준다. (Series 이름으로도 설정한다.)
        '           DrawingTESTgraphs Function에서 추가된다.
        Dim DummyDisplayLegends As String = ""

        Select Case Dummy
            Case 50
                ReDim FindDummy(UBound(TEST_DRV_Para))
                FindDummy = TEST_DRV_Para
                DummyDisplayLegends = " - DRV"
            Case 5
                ReDim FindDummy(UBound(TEST_PAS_Para))
                FindDummy = TEST_PAS_Para
                DummyDisplayLegends = " - PAS"
            Case -1
                ReDim FindDummy(UBound(TEST_Etc_Para))
                FindDummy = TEST_Etc_Para
                DummyDisplayLegends = " - Etc"
        End Select

        Dim IsX As Boolean = False
        Dim IsY As Boolean = False

        For k = 0 To 47
            'X 파라미터 파악
            Tmp_StrX = TEST_Para_THOR(k, 0).Split("/")
            For m = 0 To UBound(Tmp_StrX)
                Tmp_Char = Tmp_StrX(m).Split(",")
                Marker = -1
                For i = 0 To UBound(DAT_Headers, 1) 'Me.ChList.Items.Count - 1
                    If InStr(DAT_Headers(i, 0), Tmp_Char(LBound(Tmp_Char))) >= 1 Then
                        Marker = UBound(Tmp_Char)
                        For j = 0 To UBound(Tmp_Char)
                            If (UCase(Tmp_Char(j)) = "TIME") And (UCase(DAT_Headers(i, 0))) <> "TIME" Then
                                '채널명에 time 채널이 아닌데 time이 들어가는 것들이 생겨서 예외처리 하기위한 문====
                                Exit For
                            Else
                                If InStr(DAT_Headers(i, 0), Tmp_Char(j)) >= 1 Then
                                    Marker = Marker - 1 'j '이게 -1이어야 채널명이 맞는 것이다.
                                    If Marker = -1 Then
                                        TEST_Para_Num(k, 0) = i
                                        IsX = True
                                        Exit For
                                    End If
                                End If
                            End If
                        Next
                    End If
                    If IsX <> True Then
                        TEST_Para_Num(k, 0) = -1
                    Else
                        Exit For
                    End If
                Next
            Next

            IsX = False

            'Y 파라미터
            Tmp_StrY = TEST_Para_THOR(k, 1).Split("/")
            For m = 0 To UBound(Tmp_StrY)
                Tmp_Char = Tmp_StrY(m).Split(",")
                For i = 0 To UBound(DAT_Headers, 1)
                    If InStr(DAT_Headers(i, 0), Tmp_Char(LBound(Tmp_Char))) >= 1 Then
                        Marker = UBound(Tmp_Char)
                        For j = 0 To UBound(Tmp_Char)
                            If InStr(DAT_Headers(i, 0), Tmp_Char(j)) >= 1 Then
                                Marker = Marker - 1 ' - j '이게 -2이어야 Y채널명이 맞는 것이다.
                                If Marker = -1 Then
                                    '여기서 더미 파라미터를 확인한다.=============
                                    For n = 0 To UBound(FindDummy)
                                        If InStr(DAT_Headers(i, 0), FindDummy(n)) >= 1 Then
                                            Marker = Marker - 1
                                            If Marker = -2 Then
                                                TEST_Para_Num(k, 1) = i
                                                IsY = True
                                                Exit For
                                            End If
                                        End If
                                    Next
                                    '=============================================
                                End If
                            End If
                        Next
                    End If
                    If IsY <> True Then 'Marker = -2 Then
                        TEST_Para_Num(k, 1) = -1
                    Else
                        Exit For
                    End If
                Next
            Next

            IsY = False
        Next
        '시험 데이터 위치 파악 완료
        MainMDI.Statuslbl.Text = "Reading TEST DATA..."
        '======================================================================================================

        Dim Values As Byte()

        '시험 데이터를 해석 데이터에 오버랩시킨다.=============================================================
        If Mid(DATAFileName, Len(DATAFileName) - 2, 4) = "tdm" Then
            'TDM인 경우
            Dim R64File As New FileInfo(DATAFilePath & Replace(DATAFileName, ".tdm", ".tdx"))
            Dim NumByte As Long = R64File.Length
            Dim Fstream As New FileStream(DATAFilePath & Replace(DATAFileName, ".tdm", ".tdx"), FileMode.Open, FileAccess.Read)
            Dim FileRead As New BinaryReader(Fstream)
            'Byte Array를 읽어온다.
            Values = FileRead.ReadBytes(CInt(NumByte))

            FileRead.Close()
            Fstream.Close()
        Else
            'DAT 파일인 경우
            Dim R64File As New FileInfo(DATAFilePath & Replace(DATAFileName, ".DAT", ".R64"))
            Dim NumByte As Long = R64File.Length
            Dim Fstream As New FileStream(DATAFilePath & Replace(DATAFileName, ".DAT", ".R64"), FileMode.Open, FileAccess.Read)
            Dim FileRead As New BinaryReader(Fstream)
            'Byte Array를 읽어온다.
            Values = FileRead.ReadBytes(CInt(NumByte))

            FileRead.Close()
            Fstream.Close()
        End If



        Dim Tmp_X_Vals() As Double = Nothing
        Dim Tmp_Y_Vals() As Double = Nothing
        '******************************************************************************************************
        MainMDI.ProgressBarMain.Maximum = 4700

        For i = 0 To 47  '그래프 개수
            '그래프 추가 작업의 시작====================================================================
            If TEST_Para_Num(i, 0) <> -1 And TEST_Para_Num(i, 1) <> -1 Then
                If i > 0 Then
                    If DAT_Headers(TEST_Para_Num(i, 0), 1) = DAT_Headers(TEST_Para_Num(i - 1, 0), 1) _
                    And DAT_Headers(TEST_Para_Num(i, 0), 2) = DAT_Headers(TEST_Para_Num(i - 1, 0), 2) _
                    And DAT_Headers(TEST_Para_Num(i, 0), 5) <> 1 And IsNothing(Tmp_X_Vals) = False Then '앞쪽 데이터와 이름/길이가 같고, EXPLICIT 데이터인 조건
                        '이전 데이터와 X 데이터의 시작위치 및 길이(갯수)가 같으면 데이터를 한번 더 읽어올 필요가 없다.
                        '속도를 조금이라도 빠르게 함 (Y만 읽어온다)
                        If DAT_Headers(TEST_Para_Num(i, 1), 8) = 4 Then
                            Tmp_Y_Vals = TESTopen.OpenBinaryR32(Values, DAT_Headers(TEST_Para_Num(i, 1), 1), DAT_Headers(TEST_Para_Num(i, 1), 2))
                        Else
                            Tmp_Y_Vals = TESTopen.OpenBinaryR64(Values, DAT_Headers(TEST_Para_Num(i, 1), 1), DAT_Headers(TEST_Para_Num(i, 1), 2))
                        End If
                    Else
                        If DAT_Headers(TEST_Para_Num(i, 0), 5) = 1 Then
                            'Implicit DATA의 경우는 데이터를 만들어 줘야한다. (주로 X축 - 시간만 이런다.)
                            ReDim Tmp_X_Vals(CInt(DAT_Headers(TEST_Para_Num(i, 0), 2) - 1))
                            For p = 0 To UBound(Tmp_X_Vals)
                                Tmp_X_Vals(p) = DAT_Headers(TEST_Para_Num(i, 0), 3) + (p * DAT_Headers(TEST_Para_Num(i, 0), 4))
                            Next
                        Else
                            If DAT_Headers(TEST_Para_Num(i, 0), 8) = 4 Then
                                Tmp_X_Vals = TESTopen.OpenBinaryR32(Values, DAT_Headers(TEST_Para_Num(i, 0), 1), DAT_Headers(TEST_Para_Num(i, 0), 2))
                            Else
                                Tmp_X_Vals = TESTopen.OpenBinaryR64(Values, DAT_Headers(TEST_Para_Num(i, 0), 1), DAT_Headers(TEST_Para_Num(i, 0), 2))
                            End If
                        End If

                        'Time의 경우 sec를 msec로 바꾼다.
                        If Tmp_X_Vals(UBound(Tmp_X_Vals)) < 2.0 And InStr(UCase(DAT_Headers(TEST_Para_Num(i, 0), 0)), "TIME") >= 1 Then
                            '시험 time 단위가 sec이면 MADYMO와 같은 msec로 바꿔준다.
                            For t = 0 To UBound(Tmp_X_Vals)
                                Tmp_X_Vals(t) = Tmp_X_Vals(t) * 1000
                            Next
                        End If

                        'Y 데이터 읽기 (Explicit)
                        If DAT_Headers(TEST_Para_Num(i, 1), 8) = 4 Then
                            Tmp_Y_Vals = TESTopen.OpenBinaryR32(Values, DAT_Headers(TEST_Para_Num(i, 1), 1), DAT_Headers(TEST_Para_Num(i, 1), 2))
                        Else
                            Tmp_Y_Vals = TESTopen.OpenBinaryR64(Values, DAT_Headers(TEST_Para_Num(i, 1), 1), DAT_Headers(TEST_Para_Num(i, 1), 2))
                        End If
                    End If
                Else
                    '아니면 둘 다 읽어온다. (맨 처음 그래프)
                    If DAT_Headers(TEST_Para_Num(i, 0), 5) = 1 Then
                        'Implicit DATA의 경우는 데이터를 만들어 줘야한다. (주로 X축 - 시간만 이런다.)
                        ReDim Tmp_X_Vals(CInt(DAT_Headers(TEST_Para_Num(i, 0), 2) - 1))
                        For p = 0 To UBound(Tmp_X_Vals)
                            Tmp_X_Vals(p) = DAT_Headers(TEST_Para_Num(i, 0), 3) + (p * DAT_Headers(TEST_Para_Num(i, 0), 4))
                        Next
                    Else
                        If DAT_Headers(TEST_Para_Num(i, 0), 8) = 4 Then
                            Tmp_X_Vals = TESTopen.OpenBinaryR32(Values, DAT_Headers(TEST_Para_Num(i, 0), 1), DAT_Headers(TEST_Para_Num(i, 0), 2))
                        Else
                            Tmp_X_Vals = TESTopen.OpenBinaryR64(Values, DAT_Headers(TEST_Para_Num(i, 0), 1), DAT_Headers(TEST_Para_Num(i, 0), 2))
                        End If
                    End If
                    If DAT_Headers(TEST_Para_Num(i, 1), 8) = 4 Then
                        Tmp_Y_Vals = TESTopen.OpenBinaryR32(Values, DAT_Headers(TEST_Para_Num(i, 1), 1), DAT_Headers(TEST_Para_Num(i, 1), 2))
                    Else
                        Tmp_Y_Vals = TESTopen.OpenBinaryR64(Values, DAT_Headers(TEST_Para_Num(i, 1), 1), DAT_Headers(TEST_Para_Num(i, 1), 2))
                    End If

                    'Time의 경우 sec를 msec로 바꾼다.
                    If Tmp_X_Vals(UBound(Tmp_X_Vals)) < 2.0 And InStr(UCase(DAT_Headers(TEST_Para_Num(i, 0), 0)), "TIME") >= 1 Then
                        '시험 time 단위가 sec이면 MADYMO와 같은 msec로 바꿔준다.
                        For t = 0 To UBound(Tmp_X_Vals)
                            Tmp_X_Vals(t) = Tmp_X_Vals(t) * 1000
                        Next
                    End If
                End If
                '데이터를 그린다
                '데이터를 그리기위한 함수 호출
                'Tmp_CH_Group_Name_Sel : TDM의 그룹 명이 저장된 배열
                'TDM_Sel_CHGroup_Num : 선택한 그룹의 번호
                If Mid(DATAFileName, Len(DATAFileName) - 2, 4) = "tdm" Then
                    DrawingTESTgraphs(GraphCollection.Item(i + 1), Tmp_X_Vals, Tmp_Y_Vals, Tmp_CH_Group_Name_Sel(TDM_Sel_CHGroup_Num) & " " & DummyDisplayLegends, GraphTitle_THOR(i))
                Else
                    DrawingTESTgraphs(GraphCollection.Item(i + 1), Tmp_X_Vals, Tmp_Y_Vals, DATAFileName & DummyDisplayLegends, GraphTitle_THOR(i))
                End If
            Else
                '데이터가 없는 것들 그리는 함수
                Dim Zero_Arr(0) As Double
                If Mid(DATAFileName, Len(DATAFileName) - 2, 4) = "tdm" Then
                    DrawingTESTgraphs(GraphCollection.Item(i + 1), Zero_Arr, Zero_Arr, Tmp_CH_Group_Name_Sel(TDM_Sel_CHGroup_Num) & " " & DummyDisplayLegends, GraphTitle_THOR(i))
                Else
                    DrawingTESTgraphs(GraphCollection.Item(i + 1), Zero_Arr, Zero_Arr, DATAFileName & DummyDisplayLegends, GraphTitle_THOR(i))
                End If

            End If

            '==========================================================================================
            MainMDI.ProgressBarMain.Value = i * 100
            MainMDI.Statuslbl.Text = "TEST Graph Overlap Complete."
        Next
        '======================================================================================================
    End Sub

    Private Sub DrawingTESTgraphs(ByRef Charts As Chart, ByVal X_data As Double(), ByVal Y_data As Double(), ByVal SeriesTitle As String, _
                                  ByVal GraphName As String)
        With Charts

            Try
                Try
                    .Series.Add(SeriesTitle).Points.DataBindXY(X_data, Y_data)
                Catch ex As Exception
                    MsgBox(GraphName & "의 X-Y 데이터 크기가 일치하지 않습니다." & vbCrLf & "데이터를 확인하세요.", , "Warning")
                End Try
                .Series(SeriesTitle).ChartType = SeriesChartType.Line
            Catch ex As ArgumentException
                MainMDI.Statuslbl.Text = "Same DATA already Exist in Graph."
                Exit Sub
            End Try


            '맨 처음 그래프와 X범위를 똑같이 가져간다.
            .ChartAreas(0).AxisX.Interval = Chart00.ChartAreas(0).AxisX.Interval
            .ChartAreas(0).AxisX.Minimum = Chart00.ChartAreas(0).AxisX.Minimum
            .ChartAreas(0).AxisX.Maximum = Chart00.ChartAreas(0).AxisX.Maximum

            ''차트의 Min/Max/Inteverl 설정
            'If X_data(UBound(X_data)) < 10 Then
            '    .ChartAreas(0).AxisX.Interval = 0.02
            '    .ChartAreas(0).AxisX.Minimum = 0.0
            '    .ChartAreas(0).AxisX.Maximum = 0.2
            'Else
            '    .ChartAreas(0).AxisX.Interval = 20
            '    .ChartAreas(0).AxisX.Minimum = 0.0
            '    .ChartAreas(0).AxisX.Maximum = 200
            'End If
            .ChartAreas(0).AxisY.Minimum = Double.NaN
            .ChartAreas(0).AxisY.Maximum = Double.NaN

            'Label Format (X-Y 그래프 라벨 글꼴)
            .ChartAreas(0).AxisX.LabelStyle.Font = New Font("Arial", 7, FontStyle.Bold)
            .ChartAreas(0).AxisY.LabelStyle.Font = New Font("Arial", 7, FontStyle.Bold)

            '범례 표식
            .Legends(0).Font = New Font("Arial", 7, FontStyle.Bold)
            .Legends(0).Docking = Docking.Bottom

            .Update()
        End With

    End Sub

    Private Sub LegendOnOffToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LegendOnOffToolStripMenuItem.Click
        Dim i As Integer

        If Me.LegendOnOffToolStripMenuItem.Checked = True Then
            For i = 1 To 48
                WholeChartOption.ShowLegends(GraphCollection.Item(i))
            Next
            For i = 1 To 8
                WholeChartOption.ShowLegends(AddGraphCollection.Item(i))
            Next
        ElseIf Me.LegendOnOffToolStripMenuItem.Checked = False Then
            For i = 1 To 48
                WholeChartOption.HideLegends(GraphCollection.Item(i))
            Next
            For i = 1 To 8
                WholeChartOption.HideLegends(AddGraphCollection.Item(i))
            Next
        End If
    End Sub

    Private Sub TopToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TopToolStripMenuItem.Click
        Dim i As Integer
        For i = 1 To 48
            WholeChartOption.TopLegends(GraphCollection.Item(i))
        Next
        For i = 1 To 8
            WholeChartOption.BottomLegends(AddGraphCollection.Item(i))
        Next
    End Sub
    Private Sub BottomToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BottomToolStripMenuItem.Click
        Dim i As Integer
        For i = 1 To 48
            WholeChartOption.BottomLegends(GraphCollection.Item(i))
        Next
        For i = 1 To 8
            WholeChartOption.BottomLegends(AddGraphCollection.Item(i))
        Next
    End Sub
    Private Sub LeftToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LeftToolStripMenuItem.Click
        Dim i As Integer
        For i = 1 To 48
            WholeChartOption.LeftLegends(GraphCollection.Item(i))
        Next
        For i = 1 To 8
            WholeChartOption.LeftLegends(AddGraphCollection.Item(i))
        Next
    End Sub
    Private Sub RightToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RightToolStripMenuItem.Click
        Dim i As Integer
        For i = 1 To 48
            WholeChartOption.RightLegends(GraphCollection.Item(i))
        Next
        For i = 1 To 8
            WholeChartOption.RightLegends(AddGraphCollection.Item(i))
        Next
    End Sub
    Private Sub InsideOutsideToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InsideOutsideToolStripMenuItem.Click
        Dim i As Integer

        If Me.InsideOutsideToolStripMenuItem.Checked = True Then
            For i = 1 To 48
                WholeChartOption.InSideLegends(GraphCollection.Item(i))
            Next
            'For i = 1 To 16
            '    WholeChartOption.InOutLegends(AddGraphCollection.Item(i))
            'Next
        Else
            For i = 1 To 48
                WholeChartOption.OutSideLegends(GraphCollection.Item(i))
            Next
            'For i = 1 To 16
            '    WholeChartOption.InOutLegends(AddGraphCollection.Item(i))
            'Next
        End If
    End Sub

    Private Sub TestFileLst_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TestFileLst.MouseUp
        If Me.TestFileLst.SelectedIndex = -1 Then Exit Sub
        Me.ToolTip1.SetToolTip(Me.TestFileLst, "Selected Item : " & vbCrLf & Me.TestFileLst.Items(Me.TestFileLst.SelectedIndex))
    End Sub

    Private Sub TestFileLst_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TestFileLst.SelectedIndexChanged
        Dim i As Integer

        With Me

            .TDMchgroup.Enabled = False

            If .TestFileLst.SelectedIndex >= 0 Then
                '파일을 선택하면 채널명을 리스트에 쓴다.

                '기존의 리스트 항목 삭제
                EventPass = True
                .CmbXaxis.SelectedIndex = -1
                .ChList.SelectedIndex = -1
                EventPass = False
                For i = .ChList.Items.Count To 1 Step -1
                    .ChList.Items.RemoveAt(i - 1)
                    .CmbXaxis.Items.RemoveAt(i - 1)
                Next
                For i = .TDMchgroup.Items.Count To 1 Step -1
                    .TDMchgroup.Items.RemoveAt(i - 1)
                Next

                If IsTDMfile(.TestFileLst.SelectedIndex) = True Then

                    'TDM 파일인 경우=====================================================================================================
                    '채널 그룹을 선택하게 해야한다.==========================================================================
                    Dim HeaderFile As StreamReader
                    Dim Tmp_read As String
                    Dim k As Integer = 0
                    Dim Paths As String = TestFilePath(.TestFileLst.SelectedIndex) & .TestFileLst.SelectedItem
                    Dim Tmp_CH_Group_Name() As String = Nothing

                    HeaderFile = New StreamReader(Paths)

                    Do While Not HeaderFile.EndOfStream
                        Tmp_read = HeaderFile.ReadLine

                        If InStr(Tmp_read, "<tdm_channelgroup id=""") > 0 Then
                            Tmp_read = HeaderFile.ReadLine
                            k = k + 1
                            ReDim Preserve Tmp_CH_Group_Name(k)
                            Tmp_CH_Group_Name(k) = BetweenEle(Tmp_read)

                            Do While Not InStr(Tmp_read, "</tdm_channelgroup>") > 1
                                Tmp_read = HeaderFile.ReadLine
                                '채널그룹의 이름식별=========================================================================
                                If InStr(Tmp_read, "<name>") > 0 Then
                                    Tmp_CH_Group_Name(k) = BetweenEle(Tmp_read)
                                End If
                            Loop

                        End If
                    Loop

                    For k = 1 To UBound(Tmp_CH_Group_Name)
                        TDMCHselEventPass = True
                        .TDMchgroup.Items.Add(Tmp_CH_Group_Name(k))
                        TDMCHselEventPass = False
                        .TDMchgroup.SelectedIndex = 0
                        '.TDMCHSel.SelectedItem(.TDMCHSel.SelectedIndex) = 0
                        .TDMchgroup.Enabled = True
                    Next
                    '===================================================================================================================

                Else
                    'DAT 파일인 경우=====================================================================================================
                    '채널명 추가
                    '선택을 하면 헤더를 읽어온다.
                    '헤더를 읽어온다.
                    DAT_Headers = OepnDATfileHeader(TestFilePath(.TestFileLst.SelectedIndex) & _
                                                        .TestFileLst.SelectedItem) 'Me.OpenTESTDlg.FileNames(i)) 'OpenFile.names(i))
                    For i = 0 To UBound(DAT_Headers, 1)
                        .ChList.Items.Add(DAT_Headers(i, 0))
                        .CmbXaxis.Items.Add(DAT_Headers(i, 0))
                        'X라벨은 Time을 찾아서 한다. (없음 말고)
                        If UCase(.CmbXaxis.Items(i).ToString) = "TIME" Then
                            .CmbXaxis.SelectedIndex = i
                        End If
                    Next
                    '===================================================================================================================
                End If
            End If
        End With
    End Sub

    Private Sub AnalFileLst_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles AnalFileLst.MouseUp
        If Me.AnalFileLst.SelectedIndex = -1 Then Exit Sub
        Me.ToolTip1.SetToolTip(Me.AnalFileLst, "Selected Item : " & vbCrLf & Me.AnalFileLst.Items(Me.AnalFileLst.SelectedIndex))
    End Sub

    Private Sub ChList_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ChList.MouseUp
        If Me.ChList.SelectedIndex = -1 Then Exit Sub
        Me.ToolTip1.SetToolTip(Me.ChList, "Selected Item : " & vbCrLf & Me.ChList.Items(Me.ChList.SelectedIndex))
    End Sub

    Private Sub ChList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChList.SelectedIndexChanged
        If Me.TestFileLst.SelectedIndex = -1 Or EventPass = True Then Exit Sub

        Dim Values As Byte()
        Dim FileNameR64 As String

        If IsTDMfile(Me.TestFileLst.SelectedIndex) = False Then

            FileNameR64 = Replace(Me.TestFileLst.Items((Me.TestFileLst.SelectedIndex)), ".DAT", "")

            If Not System.IO.File.Exists(TestFilePath(Me.TestFileLst.SelectedIndex) & FileNameR64 & ".R64") Then
                MsgBox("데이터 파일을 찾을 수 없습니다", , "경고")
                Exit Sub
            End If

            Dim R64File As New FileInfo(TestFilePath(Me.TestFileLst.SelectedIndex) & FileNameR64 & ".R64")
            Dim NumByte As Long = R64File.Length
            Dim Fstream As New FileStream(TestFilePath(Me.TestFileLst.SelectedIndex) & FileNameR64 & ".R64", FileMode.Open, FileAccess.Read)
            Dim FileRead As New BinaryReader(Fstream)
            'Byte Array를 읽어온다.
            Values = FileRead.ReadBytes(CInt(NumByte))

            FileRead.Close()
            Fstream.Close()

        Else

            FileNameR64 = Replace(Me.TestFileLst.Items((Me.TestFileLst.SelectedIndex)), ".tdm", "")

            If Not System.IO.File.Exists(TestFilePath(Me.TestFileLst.SelectedIndex) & FileNameR64 & ".tdx") Then
                MsgBox("데이터 파일을 찾을 수 없습니다", , "경고")
                Exit Sub
            End If

            Dim R64File As New FileInfo(TestFilePath(Me.TestFileLst.SelectedIndex) & FileNameR64 & ".tdx")
            Dim NumByte As Long = R64File.Length
            Dim Fstream As New FileStream(TestFilePath(Me.TestFileLst.SelectedIndex) & FileNameR64 & ".tdx", FileMode.Open, FileAccess.Read)
            Dim FileRead As New BinaryReader(Fstream)
            'Byte Array를 읽어온다.
            Values = FileRead.ReadBytes(CInt(NumByte))

            FileRead.Close()
            Fstream.Close()

        End If

        '==================================================================================================================================
        Dim CH_DATA_X() As Double '채널 데이터
        Dim CH_DATA_Y() As Double '채널 데이터

        '데이터를 읽어온다 (X축)
        'CH_names(채널명,0-데이터이름/1-시작위치/2-길이(갯수)/3-시작옵셋값/4-증분/5-Implicit여부/6-단위)
        If Not IsNumeric(Me.Xscale.Text) Then Exit Sub
        If Not IsNumeric(Me.Yscale.Text) Then Exit Sub
        If Not IsNumeric(Me.Xoffset.Text) Then Exit Sub
        If Not IsNumeric(Me.Yoffset.Text) Then Exit Sub
        'X,Y 옵셋량과 변환 Factor를 적용한다.
        If DAT_Headers(Me.CmbXaxis.SelectedIndex, 5) = 1 Then
            'Implicit DATA의 경우는 데이터를 만들어 줘야한다. (주로 X축 - 시간만 이런다.)
            ReDim CH_DATA_X(CInt(DAT_Headers(Me.CmbXaxis.SelectedIndex, 2) - 1))
            For p = 0 To UBound(CH_DATA_X)
                CH_DATA_X(p) = DAT_Headers(Me.CmbXaxis.SelectedIndex, 3) + (p * DAT_Headers(Me.CmbXaxis.SelectedIndex, 4))
            Next
        Else
            If DAT_Headers(Me.CmbXaxis.SelectedIndex, 8) = 4 Then
                CH_DATA_X = TESTopen.OpenBinaryR32(Values, DAT_Headers(Me.CmbXaxis.SelectedIndex, 1), DAT_Headers(Me.CmbXaxis.SelectedIndex, 2))
            Else
                CH_DATA_X = TESTopen.OpenBinaryR64(Values, DAT_Headers(Me.CmbXaxis.SelectedIndex, 1), DAT_Headers(Me.CmbXaxis.SelectedIndex, 2))
            End If
        End If

        'CH_DATA_X = OpenBinaryR64(Values, DAT_Headers(Me.CmbXaxis.SelectedIndex, 1), DAT_Headers(Me.CmbXaxis.SelectedIndex, 2), CSng(Me.Xscale.Text), CSng(Me.Xoffset.Text))

        If CH_DATA_X(UBound(CH_DATA_X)) < 2.0 And InStr(UCase(DAT_Headers(Me.CmbXaxis.SelectedIndex, 0)), "TIME") >= 1 Then 'X축이 Time이면 단위변경
            '시험 time 단위가 sec이면 MADYMO와 같은 msec로 바꿔준다.
            For i = 0 To UBound(CH_DATA_X)
                CH_DATA_X(i) = CH_DATA_X(i) * 1000
            Next
        End If

        'Y 데이터도 Implicit 이면 만들어준다.
        If DAT_Headers(Me.ChList.SelectedIndex, 5) = 1 Then
            ReDim CH_DATA_Y(CInt(DAT_Headers(Me.ChList.SelectedIndex, 2) - 1))
            For i = 0 To UBound(CH_DATA_Y)
                CH_DATA_Y(i) = DAT_Headers(Me.ChList.SelectedIndex, 3) + (i * DAT_Headers(Me.ChList.SelectedIndex, 4))
            Next
        Else
            If DAT_Headers(Me.ChList.SelectedIndex, 8) = 4 Then
                CH_DATA_Y = TESTopen.OpenBinaryR32(Values, DAT_Headers(Me.ChList.SelectedIndex, 1), DAT_Headers(Me.ChList.SelectedIndex, 2), CSng(Me.Yscale.Text), CSng(Me.Yoffset.Text))
            Else
                CH_DATA_Y = TESTopen.OpenBinaryR64(Values, DAT_Headers(Me.ChList.SelectedIndex, 1), DAT_Headers(Me.ChList.SelectedIndex, 2), CSng(Me.Yscale.Text), CSng(Me.Yoffset.Text))
            End If

        End If

        'DATA Preview
        With Me.PreviewTEST
            For i = .Series.Count To 1 Step -1
                .Series.RemoveAt(i - 1)
            Next

            '.Series.Add(DAT_Headers(Me.ChList.SelectedIndex, 0).ToString)
            If IsTDMfile(Me.TestFileLst.SelectedIndex) = True Then
                .Series.Add(Me.TestFileLst.Items(Me.TestFileLst.SelectedIndex) & " " & Me.TDMchgroup.Items(Me.TDMchgroup.SelectedIndex))
            Else
                .Series.Add(Me.TestFileLst.Items(Me.TestFileLst.SelectedIndex))
            End If

            Try
                .Titles(0).Text = DAT_Headers(Me.ChList.SelectedIndex, 0).ToString
                .Titles(0).Font = New Font("Arial", 10, FontStyle.Bold)
            Catch ex As Exception
                .Titles.Add(DAT_Headers(Me.ChList.SelectedIndex, 0).ToString)
                .Titles(0).Font = New Font("Arial", 10, FontStyle.Bold)
            End Try

            Try
                '.Series(DAT_Headers(Me.ChList.SelectedIndex, 0).ToString).Points.DataBindXY(CH_DATA_X, CH_DATA_Y)
                '.Series(Me.TestFileLst.Items(Me.TestFileLst.SelectedIndex)).Points.DataBindXY(CH_DATA_X, CH_DATA_Y)
                .Series(.Series.Count - 1).Points.DataBindXY(CH_DATA_X, CH_DATA_Y)
            Catch ex As Exception
                If Math.Abs(UBound(CH_DATA_X) - UBound(CH_DATA_Y)) > 2 Then
                    MsgBox("데이터 크기 오류 (X-Y DATA Miss Matching)", , "경고")
                End If
            Finally
                'X 데이터와 Y 데이터의 크기가 한개정도 차이나면 그냥 무시하고 그래프 그림
                '.Series(DAT_Headers(Me.ChList.SelectedIndex, 0).ToString).ChartType = DataVisualization.Charting.SeriesChartType.Line
                '.Series(Me.TestFileLst.Items(Me.TestFileLst.SelectedIndex)).ChartType = DataVisualization.Charting.SeriesChartType.Line
                .Series(.Series.Count - 1).ChartType = DataVisualization.Charting.SeriesChartType.Line

                'DATA Preview 꾸미기
                DATAPreviewDP.GraphAxisLine(Me.PreviewTEST)
                DATAPreviewDP.ScrollnZoom(Me.PreviewTEST)
                DATAPreviewDP.SeriesValShow(Me.PreviewTEST)

                If CH_DATA_X(UBound(CH_DATA_X)) < 10 Then
                    '.ChartAreas(0).AxisX.IntervalOffset = 0.0001
                    .ChartAreas(0).AxisX.Interval = 0.02
                    .ChartAreas(0).AxisX.Minimum = 0.0
                    .ChartAreas(0).AxisX.Maximum = 0.18
                Else
                    '.ChartAreas(0).AxisX.IntervalOffset = 0.1
                    .ChartAreas(0).AxisX.Interval = 20
                    .ChartAreas(0).AxisX.Minimum = 0.0
                    .ChartAreas(0).AxisX.Maximum = 180
                End If

            End Try

        End With
    End Sub

    Private Sub PreviewTEST_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PreviewTEST.MouseClick

        If Me.PreviewTEST.Series.Count = 0 Then Exit Sub

        Me.PreviewTEST.Focus()

        If e.Button = Windows.Forms.MouseButtons.Right Then
            Me.PreviewContextMenu.Show(MousePosition)
        End If

    End Sub

    Private Sub ChartOptionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChartOptionToolStripMenuItem.Click

        If Me.PreviewTEST.Focused = True Then
            Dim OptionFrm As New FrmChartOption(Me.PreviewTEST)
            OptionFrm.Owner = Me
            OptionFrm.Show()
        ElseIf Me.PreviewAnal.Focused = True Then
            Dim OptionFrm As New FrmChartOption(Me.PreviewAnal)
            OptionFrm.Owner = Me
            OptionFrm.Show()
        End If


    End Sub

    Private Sub CopyToClipBoardBMPToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToClipBoardBMPToolStripMenuItem.Click
        If Me.PreviewTEST.Focused = True Then
            CopyChartImage(Me.PreviewTEST)
        ElseIf Me.PreviewAnal.Focused = True Then
            CopyChartImage(Me.PreviewAnal)
        End If
    End Sub

    Private Sub ExtLst_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ExtLst.MouseUp
        If Me.ExtLst.SelectedIndex = -1 Then Exit Sub
        ToolTip1.SetToolTip(Me.ExtLst, ExtDetails(Me.ExtLst.SelectedIndex))
    End Sub

    Private Sub ExtLst_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtLst.SelectedIndexChanged

        Me.CompLstAnal.SelectedIndex = -1

        If EventPass = True Then Exit Sub

        If Me.ExtLst.SelectedIndex = -1 Or Me.AnalFileLst.SelectedIndex = -1 Then Exit Sub

        Dim i As Integer
        Dim Read_Str As String
        Dim Tmp_Str() As String
        Dim FileReading As StreamReader
        Dim Tot_Row As Integer
        Dim Tot_Col As Integer

        With Me

            If Not System.IO.File.Exists(Path(.AnalFileLst.SelectedIndex) & .AnalFileLst.Items(.AnalFileLst.SelectedIndex) & .ExtLst.Items(.ExtLst.SelectedIndex)) Then
                MsgBox("데이터 파일을 찾을 수 없습니다", , "경고")

                EventPass = True
                Me.CompLstAnal.SelectedIndex = -1
                Me.ChLstAnal.SelectedIndex = -1

                For i = Me.ChLstAnal.Items.Count To 1 Step -1
                    Me.ChLstAnal.Items.RemoveAt(i - 1)
                Next

                For i = Me.CompLstAnal.Items.Count To 1 Step -1
                    Me.CompLstAnal.Items.RemoveAt(i - 1)
                Next
                EventPass = False

                Exit Sub
            End If

            Dim ReadFiles As New FileStream(Path(.AnalFileLst.SelectedIndex) & .AnalFileLst.Items(.AnalFileLst.SelectedIndex) & .ExtLst.Items(.ExtLst.SelectedIndex), FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            FileReading = New StreamReader(ReadFiles)
            Read_Str = FileReading.ReadLine
            Read_Str = FileReading.ReadLine
            Read_Str = FileReading.ReadLine
            Tmp_Str = RTrim(LTrim(Read_Str)).Split(" ")

            '해당 파일의 총 행/열 수를 구한다.
            Tot_Row = CInt(Tmp_Str(LBound(Tmp_Str)))
            Tot_Col = CInt(Tmp_Str(UBound(Tmp_Str)))

            EventPass = True
            '기존 리스트 제거
            For i = .ChLstAnal.Items.Count To 1 Step -1
                .ChLstAnal.Items.RemoveAt(i - 1)
            Next
            For i = .CompLstAnal.Items.Count To 1 Step -1
                .CompLstAnal.Items.RemoveAt(i - 1)
            Next

            '리스트 추가
            For i = 1 To Tot_Row
                Read_Str = FileReading.ReadLine
                .ChLstAnal.Items.Add(Read_Str)
            Next

            '리스트 추가
            For i = 1 To Tot_Col
                Read_Str = FileReading.ReadLine
                .CompLstAnal.Items.Add(Read_Str)
            Next
            EventPass = False

            FileReading.Close()
            ReadFiles.Close()
            'FileRowCol(Path,.AnalFileLst.Items(.AnalFileLst.SelectedIndex),.ExtLst.Items(.ExtLst.SelectedIndex),여기에 배열)
        End With
    End Sub

    Private Sub ChLstAnal_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ChLstAnal.MouseUp
        If Me.ChLstAnal.SelectedIndex = -1 Then Exit Sub
        ToolTip1.SetToolTip(Me.ChLstAnal, Me.ChLstAnal.Items(Me.ChLstAnal.SelectedIndex))
    End Sub

    Private Sub ChLstAnal_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChLstAnal.SelectedIndexChanged

        Me.CompLstAnal.SelectedIndex = -1

        If EventPass = True Then Exit Sub

        Dim TmpStr1() As String
        Dim TmpStr2() As String

        'Row 파라미터는 2개로 한다. (2개 이상 존재시)
        'Ex>/100/4001 ( /S-Hybrid_III_50th/ChestDeflection_dis ) CFC600
        '     /S-Hybrid_III_50th/ChestDeflection_dis 두개를 잘라서 본다.
        ' 1. ( ) 로 잘라서 쓴다.
        With Me
            TmpStr1 = .ChLstAnal.Items(.ChLstAnal.SelectedIndex).ToString.Split(" ")
            TmpStr2 = TmpStr1(2).Split("/")

            If UBound(TmpStr2) = 0 Then ' "/"가 없는 경우
                RowParaTab3 = TmpStr2(0)
            Else
                RowParaTab3 = TmpStr2(UBound(TmpStr2) - 1) & "/" & TmpStr2(UBound(TmpStr2))
            End If
        End With

        Me.CompLstAnal.SelectedIndex = 0

    End Sub

    Private Sub CompLstAnal_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompLstAnal.SelectedIndexChanged
        If Me.ChLstAnal.SelectedIndex = -1 Or Me.CompLstAnal.SelectedIndex = -1 Or EventPass = True Then Exit Sub

        If Not IsNumeric(Me.XscaleAnal.Text) Then Exit Sub
        If Not IsNumeric(Me.YscaleAnal.Text) Then Exit Sub
        If Not IsNumeric(Me.XoffsetAnal.Text) Then Exit Sub
        If Not IsNumeric(Me.YoffsetAnal.Text) Then Exit Sub

        With Me
            ColparaTab3 = .CompLstAnal.Items(.CompLstAnal.SelectedIndex)
        End With

        '최종적으로 여기를 클릭했을 때 데이터를 읽어온다.
        Dim i As Integer
        Dim AnalPara(3) As Integer
        ' {Row, Col, Tot_Row, Tot_Col}

        Dim TmpPara(1) As String  '파라미터 저장
        Dim TmpTimeVals() As Decimal = Nothing   '시간저장
        Dim TmpDataVals() As Double = Nothing '값 저장

        TmpPara(0) = RowParaTab3
        TmpPara(1) = ColparaTab3

        Try
            Me.PreviewAnal.Titles(0).Text = RowParaTab3 & vbCrLf & ColparaTab3
            Me.PreviewAnal.Titles(0).Font = New Font("Arial", 10, FontStyle.Bold)
        Catch ex As Exception
            Me.PreviewAnal.Titles.Add(RowParaTab3 & vbCrLf & ColparaTab3)
            Me.PreviewAnal.Titles(0).Font = New Font("Arial", 10, FontStyle.Bold)
        End Try

        '차트의 시리즈 삭제
        For i = Me.PreviewAnal.Series.Count - 1 To 0 Step -1
            Me.PreviewAnal.Series.RemoveAt(i)
        Next

        If Me.AllSeriesChk.Checked = False Then
            AnalPara = Analopen.FileRowCol(Path(Me.AnalFileLst.SelectedIndex), Me.AnalFileLst.Items(Me.AnalFileLst.SelectedIndex), Me.ExtLst.Items(Me.ExtLst.SelectedIndex), TmpPara)
            Analopen.DataReading(Path(Me.AnalFileLst.SelectedIndex), Me.AnalFileLst.Items(Me.AnalFileLst.SelectedIndex), Me.ExtLst.Items(Me.ExtLst.SelectedIndex), _
                        AnalPara(0), AnalPara(1), AnalPara(2), AnalPara(3), TmpTimeVals, TmpDataVals, _
                         CSng(Me.YscaleAnal.Text), CSng(Me.XscaleAnal.Text), CSng(Me.XoffsetAnal.Text), CSng(Me.Yoffset.Text))
            '데이터를 그래프에 올리는 함수
            AddSeries(Me.PreviewAnal, TmpTimeVals, TmpDataVals, Me.AnalFileLst.SelectedIndex + 1)
            'DATA Preview 꾸미기
            DATAPreviewDP.GraphAxisLine(Me.PreviewAnal)
            DATAPreviewDP.ScrollnZoom(Me.PreviewAnal)
            DATAPreviewDP.SeriesValShow(Me.PreviewAnal)

            If TmpTimeVals(UBound(TmpTimeVals)) < 10 Then
                '.ChartAreas(0).AxisX.IntervalOffset = 0.0001
                Me.PreviewAnal.ChartAreas(0).AxisX.Minimum = 0.0
                Me.PreviewAnal.ChartAreas(0).AxisX.Maximum = Double.NaN
                Me.PreviewAnal.ChartAreas(0).AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount
            Else
                '    '.ChartAreas(0).AxisX.IntervalOffset = 0.1
                'Me.PreviewAnal.ChartAreas(0).AxisX.Interval = 20
                Me.PreviewAnal.ChartAreas(0).AxisX.Minimum = 0.0
                Me.PreviewAnal.ChartAreas(0).AxisX.Maximum = Double.NaN
                Me.PreviewAnal.ChartAreas(0).AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount
            End If
        Else
            For i = 0 To Me.AnalFileLst.Items.Count - 1
                AnalPara = Analopen.FileRowCol(Path(i), Me.AnalFileLst.Items(i), Me.ExtLst.Items(Me.ExtLst.SelectedIndex), TmpPara)
                Analopen.DataReading(Path(i), Me.AnalFileLst.Items(i), Me.ExtLst.Items(Me.ExtLst.SelectedIndex), _
                            AnalPara(0), AnalPara(1), AnalPara(2), AnalPara(3), TmpTimeVals, TmpDataVals, _
                             CSng(Me.YscaleAnal.Text), CSng(Me.XscaleAnal.Text), CSng(Me.XoffsetAnal.Text), CSng(Me.Yoffset.Text))
                '데이터를 그래프에 올리는 함수
                AddSeries(Me.PreviewAnal, TmpTimeVals, TmpDataVals, i + 1)
                'DATA Preview 꾸미기
                DATAPreviewDP.GraphAxisLine(Me.PreviewAnal)
                DATAPreviewDP.ScrollnZoom(Me.PreviewAnal)
                DATAPreviewDP.SeriesValShow(Me.PreviewAnal)

                If TmpTimeVals(UBound(TmpTimeVals)) < 10 Then
                    '.ChartAreas(0).AxisX.IntervalOffset = 0.0001
                    'Me.PreviewAnal.ChartAreas(0).AxisX.Interval = 0.02
                    'Me.PreviewAnal.ChartAreas(0).AxisX.Minimum = 0.0
                    'Me.PreviewAnal.ChartAreas(0).AxisX.Maximum = 0.2
                    Me.PreviewAnal.ChartAreas(0).AxisX.Minimum = 0.0
                    Me.PreviewAnal.ChartAreas(0).AxisX.Maximum = Double.NaN
                    Me.PreviewAnal.ChartAreas(0).AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount
                Else
                    '    '.ChartAreas(0).AxisX.IntervalOffset = 0.1
                    'Me.PreviewAnal.ChartAreas(0).AxisX.Interval = 20
                    'Me.PreviewAnal.ChartAreas(0).AxisX.Minimum = 0.0
                    'Me.PreviewAnal.ChartAreas(0).AxisX.Maximum = Double.NaN
                    Me.PreviewAnal.ChartAreas(0).AxisX.Minimum = 0.0
                    Me.PreviewAnal.ChartAreas(0).AxisX.Maximum = Double.NaN
                    Me.PreviewAnal.ChartAreas(0).AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount
                End If
            Next
        End If

    End Sub

    Private Sub AnalFileLst_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AnalFileLst.SelectedIndexChanged
        EventPass = True
        Me.CompLstAnal.SelectedIndex = -1
        EventPass = False
    End Sub

    Private Sub PreviewAnal_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PreviewAnal.MouseClick

        If Me.PreviewAnal.Series.Count = 0 Then Exit Sub

        Me.PreviewAnal.Focus()

        If e.Button = Windows.Forms.MouseButtons.Right Then
            Me.PreviewContextMenu.Show(MousePosition)
        End If

    End Sub

    Private Sub PreviewAnal_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PreviewAnal.MouseDown
        '첫번째 : 이벤트 호출
        If Me.PreviewAnal.Series.Count = 0 Then Exit Sub
        Me.PreviewAnal.Focus()
        If e.Button = Windows.Forms.MouseButtons.Middle Then
            PreviewAnal.DoDragDrop(Me.PreviewAnal, DragDropEffects.Copy)
        End If
    End Sub

    Private Sub PreviewTEST_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PreviewTEST.MouseDown
        '첫번째 : 이벤트 호출
        If Me.PreviewTEST.Series.Count = 0 Then Exit Sub
        Me.PreviewTEST.Focus()
        If e.Button = Windows.Forms.MouseButtons.Middle Then
            Me.PreviewTEST.DoDragDrop(Me.PreviewTEST, DragDropEffects.Copy)
        End If
    End Sub

    Private Sub Chart48_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart48.DoubleClick
        Dim PopUpForm As New InjuryPopUp(48, Me.Chart48, 1) ', Me.Chart00.Titles(0).Text)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart49_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart49.DoubleClick
        Dim PopUpForm As New InjuryPopUp(48, Me.Chart49, 1) ', Me.Chart00.Titles(0).Text)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart50_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart50.DoubleClick
        Dim PopUpForm As New InjuryPopUp(48, Me.Chart50, 1) ', Me.Chart00.Titles(0).Text)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart51_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart51.DoubleClick
        Dim PopUpForm As New InjuryPopUp(48, Me.Chart51, 1) ', Me.Chart00.Titles(0).Text)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart52_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart52.DoubleClick
        Dim PopUpForm As New InjuryPopUp(48, Me.Chart52, 1) ', Me.Chart00.Titles(0).Text)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart53_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart53.DoubleClick
        Dim PopUpForm As New InjuryPopUp(48, Me.Chart53, 1) ', Me.Chart00.Titles(0).Text)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart54_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart54.DoubleClick
        Dim PopUpForm As New InjuryPopUp(48, Me.Chart54, 1) ', Me.Chart00.Titles(0).Text)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart55_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart55.DoubleClick
        Dim PopUpForm As New InjuryPopUp(48, Me.Chart55, 1) ', Me.Chart00.Titles(0).Text)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub

    Private Sub Chart48_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart48.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart48_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart48.DragDrop
        DragDropEventEnd(Chart48)
    End Sub
    Private Sub Chart49_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart49.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart49_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart49.DragDrop
        DragDropEventEnd(Chart49)
    End Sub
    Private Sub Chart50_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart50.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart50_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart50.DragDrop
        DragDropEventEnd(Chart50)
    End Sub
    Private Sub Chart51_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart51.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart51_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart51.DragDrop
        DragDropEventEnd(Chart51)
    End Sub
    Private Sub Chart52_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart52.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart52_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart52.DragDrop
        DragDropEventEnd(Chart52)
    End Sub
    Private Sub Chart53_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart53.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart53_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart53.DragDrop
        DragDropEventEnd(Chart53)
    End Sub
    Private Sub Chart54_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart54.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart54_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart54.DragDrop
        DragDropEventEnd(Chart54)
    End Sub
    Private Sub Chart55_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart55.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart55_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart55.DragDrop
        DragDropEventEnd(Chart55)
    End Sub

    Private Sub DragDropEventEnd(ByRef TChart As Chart)
        '세번째 : 작업의 시작
        Dim i, j As Integer
        Dim tmp As DataPointCollection
        Dim Titles As String

        With Me

            If Me.PreviewAnal.Focused = True Then
                'Graph Overlap
                '데이터 포인트 컬랙션을 복사한다.===============================================================================
                For i = 0 To .PreviewAnal.Series.Count - 1
                    Titles = .PreviewAnal.Series(i).Name & "-" & .ChLstAnal.SelectedItem.ToString & "-" & .CompLstAnal.SelectedItem.ToString
                    tmp = .PreviewAnal.Series(i).Points
                    Try
                        TChart.Series.Add(Titles)
                    Catch ex As Exception '같은 이름이 있는경우
                        TChart.Series.Add(Titles & "-Re")
                    End Try

                    For j = 0 To tmp.Count - 1
                        TChart.Series(Titles).Points.AddXY(tmp(j).XValue, tmp(j).YValues(0))
                    Next
                Next
                '=============================================================================================================
            ElseIf Me.PreviewTEST.Focused = True Then
                'Graph Overlap
                '데이터 포인트 컬랙션을 복사한다.===============================================================================
                For i = 0 To .PreviewTEST.Series.Count - 1
                    If IsTDMfile(.TestFileLst.SelectedIndex) = True Then
                        'Titles = .PreviewTEST.Series(i).Name & "-" & .ChList.Items(.ChList.SelectedIndex)
                        Titles = .TDMchgroup.Items(.TDMchgroup.SelectedIndex) & "-" & .ChList.Items(.ChList.SelectedIndex)
                    Else
                        Titles = .PreviewTEST.Series(i).Name & "-" & .ChList.Items(.ChList.SelectedIndex)
                    End If
                    tmp = .PreviewTEST.Series(i).Points
                    Try
                        TChart.Series.Add(Titles)
                    Catch ex As Exception '같은 이름이 있는경우
                        TChart.Series.Add(Titles & "-Re")
                    End Try

                    For j = 0 To tmp.Count - 1
                        TChart.Series(Titles).Points.AddXY(tmp(j).XValue, tmp(j).YValues(0))
                    Next
                Next
                '=============================================================================================================
            End If

            TChart.Update()
            DrawingADDgraphs(TChart)
        End With
    End Sub
    Private Sub DrawingADDgraphs(ByRef Charts As Chart)
        'Dim i As Integer
        With Charts
            For i = 0 To .Series.Count - 1
                .Series(i).ChartType = SeriesChartType.Line
            Next

            DATAPreviewDP.GraphAxisLine(Charts)

            '차트의 Min/Max/Inteverl 설정
            If .Series(0).Points(.Series(0).Points.Count - 1).XValue > 90 Then
                .ChartAreas(0).AxisX.Interval = 20
                .ChartAreas(0).AxisX.Minimum = 0.0
                .ChartAreas(0).AxisX.Maximum = .Series(0).Points(.Series(0).Points.Count - 1).XValue '160
            Else
                .ChartAreas(0).AxisX.Interval = 0.02
                .ChartAreas(0).AxisX.Minimum = 0.0
                .ChartAreas(0).AxisX.Maximum = .Series(0).Points(.Series(0).Points.Count - 1).XValue '0.2
            End If
            'If .ChartAreas(0).AxisX.Maximum < 10 Then
            '    .ChartAreas(0).AxisX.Interval = 0.02
            '    .ChartAreas(0).AxisX.Minimum = 0.0
            '    .ChartAreas(0).AxisX.Maximum = 0.2
            'Else
            '    .ChartAreas(0).AxisX.Interval = 20
            '    .ChartAreas(0).AxisX.Minimum = 0.0
            '    .ChartAreas(0).AxisX.Maximum = Me.PreviewAnal.ChartAreas(0).AxisX.Maximum
            'End If
            .ChartAreas(0).AxisY.Minimum = Double.NaN
            .ChartAreas(0).AxisY.Maximum = Double.NaN

            'Label Format (X-Y 그래프 라벨 글꼴)
            .ChartAreas(0).AxisX.LabelStyle.Font = New Font("Arial", 7, FontStyle.Bold)
            .ChartAreas(0).AxisY.LabelStyle.Font = New Font("Arial", 7, FontStyle.Bold)

            '범례 표식
            .Legends(0).Font = New Font("Arial", 7, FontStyle.Bold)

            .Update()
        End With

    End Sub

    Private Sub Chart48_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart48.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart48.Series.Count To 1 Step -1
                Me.Chart48.Series.RemoveAt(i - 1)
            Next
        End If
    End Sub
    Private Sub Chart49_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart49.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart49.Series.Count To 1 Step -1
                Me.Chart49.Series.RemoveAt(i - 1)
            Next
        End If
    End Sub
    Private Sub Chart50_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart50.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart50.Series.Count To 1 Step -1
                Me.Chart50.Series.RemoveAt(i - 1)
            Next
        End If
    End Sub
    Private Sub Chart51_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart51.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart51.Series.Count To 1 Step -1
                Me.Chart51.Series.RemoveAt(i - 1)
            Next
        End If
    End Sub
    Private Sub Chart52_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart52.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart52.Series.Count To 1 Step -1
                Me.Chart52.Series.RemoveAt(i - 1)
            Next
        End If
    End Sub
    Private Sub Chart53_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart53.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart53.Series.Count To 1 Step -1
                Me.Chart53.Series.RemoveAt(i - 1)
            Next
        End If
    End Sub
    Private Sub Chart54_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart54.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart54.Series.Count To 1 Step -1
                Me.Chart54.Series.RemoveAt(i - 1)
            Next
        End If
    End Sub
    Private Sub Chart55_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart55.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart55.Series.Count To 1 Step -1
                Me.Chart55.Series.RemoveAt(i - 1)
            Next
        End If
    End Sub

    Private Sub Chart48_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart48.Click
        Me.Chart48.Focus()
        Me.ListBox1.SelectedIndex = 48
    End Sub
    Private Sub Chart49_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart49.Click
        Me.Chart49.Focus()
        Me.ListBox1.SelectedIndex = 49
    End Sub
    Private Sub Chart50_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart50.Click
        Me.Chart50.Focus()
        Me.ListBox1.SelectedIndex = 50
    End Sub
    Private Sub Chart51_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart51.Click
        Me.Chart51.Focus()
        Me.ListBox1.SelectedIndex = 51
    End Sub
    Private Sub Chart52_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart52.Click
        Me.Chart52.Focus()
        Me.ListBox1.SelectedIndex = 52
    End Sub
    Private Sub Chart53_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart53.Click
        Me.Chart53.Focus()
        Me.ListBox1.SelectedIndex = 53
    End Sub
    Private Sub Chart54_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart54.Click
        Me.Chart54.Focus()
        Me.ListBox1.SelectedIndex = 54
    End Sub
    Private Sub Chart55_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart55.Click
        Me.Chart55.Focus()
        Me.ListBox1.SelectedIndex = 55
    End Sub

    Private Sub TDMchgroup_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TDMchgroup.SelectedIndexChanged
        If TDMCHselEventPass = True Then Exit Sub

        With Me
            If .TestFileLst.SelectedIndex >= 0 Then
                '파일을 선택하면 채널명을 리스트에 쓴다.

                '기존의 리스트 항목 삭제
                EventPass = True
                .CmbXaxis.SelectedIndex = -1
                .ChList.SelectedIndex = -1
                EventPass = False
                For i = .ChList.Items.Count To 1 Step -1
                    .ChList.Items.RemoveAt(i - 1)
                    .CmbXaxis.Items.RemoveAt(i - 1)
                Next

                '채널명 추가
                '선택을 하면 헤더를 읽어온다.
                For i = 0 To UBound(Me.OpenTestDlg.FileNames)
                    '헤더를 읽어온다.
                    If IsTDMfile(.TestFileLst.SelectedIndex) = False Then
                        DAT_Headers = OepnDATfileHeader(TestFilePath(.TestFileLst.SelectedIndex) & _
                                                    .TestFileLst.SelectedItem) 'Me.OpenTESTDlg.FileNames(i)) 'OpenFile.names(i))
                    ElseIf IsTDMfile(.TestFileLst.SelectedIndex) = True Then

                        DAT_Headers = OepnTDMfileHeader(TestFilePath(.TestFileLst.SelectedIndex) & .TestFileLst.SelectedItem, .TDMchgroup.SelectedIndex + 1)
                    End If
                Next
                For i = 0 To UBound(DAT_Headers, 1)
                    .ChList.Items.Add(DAT_Headers(i, 0))
                    .CmbXaxis.Items.Add(DAT_Headers(i, 0))
                    'X라벨은 Time을 찾아서 한다. (없음 말고)
                    If UCase(.CmbXaxis.Items(i).ToString) = "TIME" Then
                        .CmbXaxis.SelectedIndex = i
                    End If
                Next
            End If
        End With

    End Sub


    Private Sub SplitContainer1_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitContainer1.SplitterMoved
        Call FrmInjuryGraph_Resize(Me, EventArgs.Empty)
        Call SplitContainer2_SplitterMoved(Me, e)
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged

        If EventPass = True Then Exit Sub
        Dim i As Integer
        '시리즈 추가
        EventPass = True
        For i = Me.SeriesLstBox.Items.Count - 1 To 0 Step -1
            Me.SeriesLstBox.Items.RemoveAt(i)
        Next

        Me.XscaleTxt.Text = "1"
        Me.YscaleTxt.Text = "1"
        Me.XoffsetTxt.Text = "0"
        Me.YoffsetTxt.Text = "0"

        ChkLstEventPass = True
        If Me.ListBox1.SelectedIndex < 48 Then
            ChartNameToChkLstBox(GraphCollection(Me.ListBox1.SelectedIndex + 1))
        Else
            ChartNameToChkLstBox(AddGraphCollection(Me.ListBox1.SelectedIndex + 1 - 48))
        End If

        EventPass = False
        ChkLstEventPass = False
        If Me.SeriesLstBox.Items.Count >= 1 Then
            Me.SeriesLstBox.SelectedIndex = 0
        End If

        Me.ListBox1.Focus()

    End Sub

    Private Sub ChartNameToChkLstBox(ByVal Tchart As Chart)

        For i = 0 To Tchart.Series.Count - 1
            Me.SeriesLstBox.Items.Add(Tchart.Series(i).Name)
            If Tchart.Series(i).Enabled = True Then
                Me.SeriesLstBox.SetItemCheckState(i, CheckState.Checked)
            Else
                Me.SeriesLstBox.SetItemCheckState(i, CheckState.Unchecked)
            End If
        Next

    End Sub

    Private Sub SeriesLstBox_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles SeriesLstBox.ItemCheck
        'Check 이벤트가 Selected 이벤트보다 먼저임

        If ChkLstEventPass = True Then Exit Sub

        If ((Control.ModifierKeys And Keys.Control) = Keys.Control) Then '컨트롤 키를 누른채로 클릭
            If Me.ListBox1.SelectedIndex <= 47 Then
                Dim i As Integer
                For i = 1 To 48
                    SeriesHideAndShow(GraphCollection(i))
                Next
            End If
        Else
            If Me.ListBox1.SelectedIndex > 47 Then
                'Additional Graph
                SeriesHideAndShow(AddGraphCollection(Me.ListBox1.SelectedIndex + 1 - 48))
            Else
                'Default Graph
                SeriesHideAndShow(GraphCollection(Me.ListBox1.SelectedIndex + 1))
            End If
        End If

    End Sub

    Private Sub SeriesHideAndShow(ByVal Tchrt As Chart)

        If Me.SeriesLstBox.SelectedIndex <> -1 Then
            If Tchrt.Series(Me.SeriesLstBox.SelectedIndex).Enabled = True Then
                Tchrt.Series(Me.SeriesLstBox.SelectedIndex).Enabled = False
            Else
                Tchrt.Series(Me.SeriesLstBox.SelectedIndex).Enabled = True
            End If
        End If

    End Sub


    Private Sub SeriesLstBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SeriesLstBox.SelectedIndexChanged
        'Private Sub SeriesLstBox_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SeriesLstBox.SelectedIndexChanged
        If EventPass = True Then Exit Sub

        If Me.ListBox1.SelectedIndex <= 47 Then
            ChartHandlingMain_Start(GraphCollection(Me.ListBox1.SelectedIndex + 1))
        Else
            ChartHandlingMain_Start(AddGraphCollection(Me.ListBox1.SelectedIndex + 1 - 48))
        End If

    End Sub

    Private Sub ChartHandlingMain_Start(ByVal Tchart As Chart)
        If Tchart.Series.Count = 0 Then Exit Sub
        If Me.SeriesLstBox.SelectedIndex < 0 Then Exit Sub
        With Me
            .SeriesNameTxt.Text = Tchart.Series(Me.SeriesLstBox.SelectedIndex).Name.ToString
            .YminTxt.Text = Tchart.ChartAreas(0).AxisY.Minimum.ToString
            .YMaxTxt.Text = Tchart.ChartAreas(0).AxisY.Maximum.ToString
            .YIntervalTxt.Text = Format(Tchart.ChartAreas(0).AxisY.MajorGrid.Interval, "0.0##")
            If Tchart.Series(Me.SeriesLstBox.SelectedIndex).Color.Name = "0" Then
                .BtnClr.BackColor = Color.LightGray
                .BtnClr.Text = "Auto Color"
            Else
                .BtnClr.Text = ""
                .BtnClr.BackColor = Tchart.Series(Me.SeriesLstBox.SelectedIndex).Color
            End If
            .BtnClr.BackColor = Tchart.Series(Me.SeriesLstBox.SelectedIndex).Color
        End With
    End Sub

    Private Sub ChartHandlingMain_Series(ByVal Tchart As Chart)
        Dim Xscale, Yscale, Xoffset, Yoffset As Double
        Dim i As Integer = 0
        Dim Series_Cnt As Integer = 0

        If Me.SeriesLstBox.SelectedIndex < 0 Then Exit Sub

        With Me

            If IsNumeric(.XscaleTxt.Text) And IsNumeric(.YscaleTxt.Text) And IsNumeric(.XoffsetTxt.Text) And IsNumeric(.YoffsetTxt.Text) Then
            Else
                Exit Sub
            End If

            Xscale = CDbl(.XscaleTxt.Text)
            Yscale = CDbl(.YscaleTxt.Text)
            Xoffset = CDbl(.XoffsetTxt.Text)
            Yoffset = CDbl(.YoffsetTxt.Text)

            Dim TmpTime(0 To Tchart.Series(.SeriesLstBox.SelectedIndex).Points.Count - 1) As Double
            Dim TmpData(0 To Tchart.Series(.SeriesLstBox.SelectedIndex).Points.Count - 1) As Double

            For i = 0 To Tchart.Series(.SeriesLstBox.SelectedIndex).Points.Count - 1
                TmpTime(i) = Tchart.Series(.SeriesLstBox.SelectedIndex).Points(i).XValue
                TmpData(i) = Tchart.Series(.SeriesLstBox.SelectedIndex).Points(i).YValues(0)
            Next

            Series_Cnt = Tchart.Series(.SeriesLstBox.SelectedIndex).Points.Count

            Tchart.Series(.SeriesLstBox.SelectedIndex).Points.Clear()

            For i = 0 To Series_Cnt - 1 Step 1
                Tchart.Series(.SeriesLstBox.SelectedIndex).Points.AddXY(TmpTime(i) * Xscale + Xoffset, TmpData(i) * Yscale + Yoffset)
            Next

            Tchart.Series(.SeriesLstBox.SelectedIndex).Name = Me.SeriesNameTxt.Text

            Me.YminTxt.Text = Tchart.ChartAreas(0).AxisY.Minimum.ToString
            Me.YMaxTxt.Text = Tchart.ChartAreas(0).AxisY.Maximum.ToString
            Application.DoEvents()
        End With

    End Sub

    Private Sub ChartHandlingMain_Range(ByVal Tchart As Chart)
        Dim Ymin, Ymax As Double
        Dim Tmp_Max As Double
        Dim PtCnt As Integer

        If Me.SeriesLstBox.SelectedIndex < 0 Then Exit Sub

        If IsNumeric(Me.YminTxt.Text) And IsNumeric(Me.YMaxTxt.Text) Then
            With Me
                Ymin = CDbl(.YminTxt.Text)
                Ymax = CDbl(.YMaxTxt.Text)
                If Math.Abs(Ymax) > Math.Abs(Ymin) Then
                    Tmp_Max = Math.Abs(Ymax)
                Else
                    Tmp_Max = Math.Abs(Ymin)
                End If

                If Ymax <> Ymin And (Me.YMaxTxt.Text <> "NaN") And (Me.YminTxt.Text <> "NaN") Then
                    PtCnt = Math.Ceiling(Math.Log10(Tmp_Max))

                    If (Ymin < Ymax) Then
                        Select Case PtCnt
                            Case 0
                                Tchart.ChartAreas(0).AxisY.MajorTickMark.Interval = 0.1
                                Tchart.ChartAreas(0).AxisY.MajorGrid.Interval = 0.1
                                Tchart.ChartAreas(0).AxisY.LabelStyle.Interval = 0.1
                            Case 1
                                If Math.Log10(Tmp_Max) > 0.5 Then
                                    Tchart.ChartAreas(0).AxisY.MajorTickMark.Interval = 1
                                    Tchart.ChartAreas(0).AxisY.MajorGrid.Interval = 1
                                    Tchart.ChartAreas(0).AxisY.LabelStyle.Interval = 1
                                Else
                                    Tchart.ChartAreas(0).AxisY.MajorTickMark.Interval = 0.2
                                    Tchart.ChartAreas(0).AxisY.MajorGrid.Interval = 0.2
                                    Tchart.ChartAreas(0).AxisY.LabelStyle.Interval = 0.2
                                End If
                            Case 2
                                Tchart.ChartAreas(0).AxisY.MajorTickMark.Interval = 10
                                Tchart.ChartAreas(0).AxisY.MajorGrid.Interval = 10
                                Tchart.ChartAreas(0).AxisY.LabelStyle.Interval = 10
                            Case 3
                                Tchart.ChartAreas(0).AxisY.MajorTickMark.Interval = 100
                                Tchart.ChartAreas(0).AxisY.MajorGrid.Interval = 100
                                Tchart.ChartAreas(0).AxisY.LabelStyle.Interval = 100
                            Case 4
                                If Math.Log10(Tmp_Max) > 3.5 Then
                                    Tchart.ChartAreas(0).AxisY.MajorTickMark.Interval = 1000
                                    Tchart.ChartAreas(0).AxisY.MajorGrid.Interval = 1000
                                    Tchart.ChartAreas(0).AxisY.LabelStyle.Interval = 1000
                                Else
                                    Tchart.ChartAreas(0).AxisY.MajorTickMark.Interval = 500
                                    Tchart.ChartAreas(0).AxisY.MajorGrid.Interval = 500
                                    Tchart.ChartAreas(0).AxisY.LabelStyle.Interval = 500
                                End If
                            Case -1
                                Tchart.ChartAreas(0).AxisY.MajorTickMark.Interval = 0.01
                                Tchart.ChartAreas(0).AxisY.MajorGrid.Interval = 0.01
                                Tchart.ChartAreas(0).AxisY.LabelStyle.Interval = 0.01
                            Case -2
                                Tchart.ChartAreas(0).AxisY.MajorTickMark.Interval = 0.001
                                Tchart.ChartAreas(0).AxisY.MajorGrid.Interval = 0.001
                                Tchart.ChartAreas(0).AxisY.LabelStyle.Interval = 0.001
                            Case -3
                                Tchart.ChartAreas(0).AxisY.MajorTickMark.Interval = 0.0001
                                Tchart.ChartAreas(0).AxisY.MajorGrid.Interval = 0.0001
                                Tchart.ChartAreas(0).AxisY.LabelStyle.Interval = 0.0001
                            Case Else

                        End Select
                        'Tchart.ChartAreas(0).AxisY.MajorTickMark.Interval = 10
                        'Tchart.ChartAreas(0).AxisY.MajorGrid.Interval = 10
                        'Tchart.ChartAreas(0).AxisY.LabelStyle.Interval = 10
                    End If
                    Tchart.ChartAreas(0).AxisY.Minimum = Ymin
                    Tchart.ChartAreas(0).AxisY.Maximum = Ymax
                Else
                    Ymax = Ymin + 10
                    Tchart.ChartAreas(0).AxisY.Minimum = Ymin
                    Tchart.ChartAreas(0).AxisY.Maximum = Ymax
                End If
            End With
        Else
            Exit Sub
        End If

    End Sub

    Private Sub SplitContainer2_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitContainer2.SplitterMoved
        With Me
            .GroupBox3.Height = 230
            .SeriesNameTxt.Location = New Point(75, 25)
            .SeriesNameTxt.Width = .GroupBox3.ClientRectangle.Width - 75 - 10
            .XscaleTxt.Location = New Point(.SeriesNameTxt.Location.X, .SeriesNameTxt.Location.Y + .SeriesNameTxt.Height + 5)
            .XscaleTxt.Width = .SeriesNameTxt.Width / 2 - 3
            .YscaleTxt.Location = New Point(.XscaleTxt.Location.X + .XscaleTxt.Width + 6, .XscaleTxt.Location.Y)
            .YscaleTxt.Size = .XscaleTxt.Size
            .XoffsetTxt.Location = New Point(.XscaleTxt.Location.X, .XscaleTxt.Location.Y + .XscaleTxt.Height + 5)
            .XoffsetTxt.Size = .XscaleTxt.Size
            .YoffsetTxt.Location = New Point(.YscaleTxt.Location.X, .YscaleTxt.Location.Y + .YscaleTxt.Height + 5)
            .YoffsetTxt.Size = .XscaleTxt.Size
            .YminTxt.Location = New Point(.XoffsetTxt.Location.X, .XoffsetTxt.Location.Y + .XoffsetTxt.Height + 5)
            .YminTxt.Size = .XscaleTxt.Size
            .YMaxTxt.Location = New Point(.YoffsetTxt.Location.X, .YoffsetTxt.Location.Y + .YoffsetTxt.Height + 5)
            .YMaxTxt.Size = .XscaleTxt.Size

            .YIntervalTxt.Location = New Point(.YminTxt.Location.X, .YMaxTxt.Location.Y + .YMaxTxt.Height + 5)
            .YIntervalTxt.Size = New Size(.XscaleTxt.Width * 2 + 5, .XscaleTxt.Height)
            .Label5.Location = New Point(5, .YIntervalTxt.Location.Y + 3)

            .BtnClr.Location = New Point(.YIntervalTxt.Location.X, .YIntervalTxt.Location.Y + .YIntervalTxt.Height + 5)
            .BtnClr.Size = .SeriesNameTxt.Size
            .Label1.Location = New Point(5, .SeriesNameTxt.Location.Y + 3)
            .Label2.Location = New Point(5, .XscaleTxt.Location.Y + 3)
            .Label3.Location = New Point(5, .XoffsetTxt.Location.Y + 3)
            .Label4.Location = New Point(5, .YminTxt.Location.Y + 3)
            .BtnResetMain.Location = New Point(5, .BtnClr.Location.Y + .BtnClr.Height + 5)
            .BtnResetMain.Size = New Size((.GroupBox3.ClientRectangle.Width - 10) / 2 - 5, 25)
            .BtnAppMain.Location = New Point(5, .BtnResetMain.Location.Y)
            .BtnAppMain.Size = New Size(.BtnResetMain.Width * 2 + 10, .BtnResetMain.Height)
        End With
    End Sub

    Private Sub BtnResetMain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnResetMain.Click

        Dim i As Integer = Me.ListBox1.SelectedIndex

        If Me.ListBox1.SelectedIndex < 0 Then Exit Sub
        '그래프를 지운다.
        DelAllSeries(GraphCollection.Item(Me.ListBox1.SelectedIndex + 1))

        For k = 0 To UBound(Names)
            If Me.ListBox1.SelectedIndex > -1 And Me.ListBox1.SelectedIndex <= 47 Then
                '처음과 같이 그래프를 그린다.
                '데이터를 읽어오는 함수
                Analopen.DataReading(Path(k), Names(k), Graph_Ext_THOR(i), DATA_Pos(i, k, 0), DATA_Pos(i, k, 1), DATA_Tot_Len(i, k, 0), DATA_Tot_Len(i, k, 1), TimeVals, DataVals, ConversionF(i))
                '데이터를 그래프에 올리는 함수
                AddSeries(GraphCollection.Item(i + 1), TimeVals, DataVals, k + 1)
                'Display 정리
                DATAGraphDP.GraphAxisLine(GraphCollection(i + 1))
                Application.DoEvents()
            ElseIf Me.ListBox1.SelectedIndex >= 48 Then
                '그래프를 지운다.
                DelAllSeries(GraphCollection.Item(Me.ListBox1.SelectedIndex + 1))
            End If
        Next

    End Sub

    Private Sub BtnAppMain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAppMain.Click

        If ((Control.ModifierKeys And Keys.Control) = Keys.Control) Then '컨트롤 키를 누른채로 클릭
            With Me
                If IsNumeric(.XscaleTxt.Text) And IsNumeric(.YscaleTxt.Text) And IsNumeric(.XoffsetTxt.Text) And IsNumeric(.YoffsetTxt.Text) Then
                    If IsNumeric(.YminTxt.Text) And IsNumeric(.YMaxTxt.Text) Then
                        Dim i As Integer = 0
                        For i = 1 To 48
                            SeriesColorChange(GraphCollection(i))
                            ChartHandlingMain_Series(GraphCollection(i))
                            ChartHandlingMain_Range(GraphCollection(i))
                        Next
                    Else
                        MsgBox("Insert Numeric Value!", MsgBoxStyle.Critical, "NaN Error")
                    End If
                Else
                    MsgBox("Insert Numeric Value!", MsgBoxStyle.Critical, "NaN Error")
                End If
            End With

            Me.ListBox1.SelectedIndex = Me.ListBox1.SelectedIndex + 1
            Me.ListBox1.SelectedIndex = Me.ListBox1.SelectedIndex - 1

            Exit Sub

        End If

        '한 그래프에만 적용
        If Me.ListBox1.SelectedIndex < 0 Then Exit Sub
        If Me.SeriesLstBox.SelectedIndex < 0 Then Exit Sub

        With Me
            If IsNumeric(.XscaleTxt.Text) And IsNumeric(.YscaleTxt.Text) And IsNumeric(.XoffsetTxt.Text) And IsNumeric(.YoffsetTxt.Text) Then
                If IsNumeric(.YminTxt.Text) And IsNumeric(.YMaxTxt.Text) Then
                    If Me.ListBox1.SelectedIndex <= 47 Then
                        ChartHandlingMain_Series(GraphCollection(.ListBox1.SelectedIndex + 1))
                        ChartHandlingMain_Range(GraphCollection(.ListBox1.SelectedIndex + 1))
                    Else
                        ChartHandlingMain_Series(AddGraphCollection(.ListBox1.SelectedIndex + 1 - 48))
                        ChartHandlingMain_Range(AddGraphCollection(.ListBox1.SelectedIndex + 1 - 48))
                    End If
                Else
                    MsgBox("Insert Numeric Value!", MsgBoxStyle.Critical, "NaN Error")
                End If
            Else
                MsgBox("Insert Numeric Value!", MsgBoxStyle.Critical, "NaN Error")
            End If
        End With
    End Sub

    Private Sub XscaleTxt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles XscaleTxt.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) And Me.SeriesLstBox.SelectedIndex <> -1 Then
            'BtnAppMain_Click(Me, Nothing)
            If Me.ListBox1.SelectedIndex <= 47 Then
                ChartHandlingMain_Series(GraphCollection(Me.ListBox1.SelectedIndex + 1))
            Else
                ChartHandlingMain_Series(AddGraphCollection(Me.ListBox1.SelectedIndex + 1 - 48))
            End If
        End If
    End Sub

    Private Sub YscaleTxt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles YscaleTxt.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) And Me.SeriesLstBox.SelectedIndex <> -1 Then
            XscaleTxt_KeyPress("-1", New KeyPressEventArgs(ChrW(Keys.Enter)))
        End If
    End Sub

    Private Sub XoffsetTxt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles XoffsetTxt.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) And Me.SeriesLstBox.SelectedIndex <> -1 Then
            XscaleTxt_KeyPress("-1", New KeyPressEventArgs(ChrW(Keys.Enter)))
        End If
    End Sub

    Private Sub YoffsetTxt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles YoffsetTxt.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) And Me.SeriesLstBox.SelectedIndex <> -1 Then
            XscaleTxt_KeyPress("-1", New KeyPressEventArgs(ChrW(Keys.Enter)))
        End If
    End Sub

    Private Sub YminTxt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles YminTxt.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) And Me.SeriesLstBox.SelectedIndex <> -1 Then
            'XscaleTxt_KeyPress("-1", New KeyPressEventArgs(ChrW(Keys.Enter)))
            If Me.ListBox1.SelectedIndex <= 31 Then
                ChartHandlingMain_Range(GraphCollection(Me.ListBox1.SelectedIndex + 1))
            Else
                ChartHandlingMain_Range(AddGraphCollection(Me.ListBox1.SelectedIndex + 1 - 48))
            End If
        End If
    End Sub

    Private Sub YMaxTxt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles YMaxTxt.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) And Me.SeriesLstBox.SelectedIndex <> -1 Then
            YminTxt_KeyPress("-1", New KeyPressEventArgs(ChrW(Keys.Enter)))
        End If
    End Sub

    Private Sub BtnClr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClr.Click

        If Me.ListBox1.SelectedIndex < 0 Or Me.SeriesLstBox.SelectedIndex < 0 Then Exit Sub

        With Me
            If .SeriesColor.ShowDialog = Windows.Forms.DialogResult.OK Then
                If .ListBox1.SelectedIndex <= 47 Then
                    SeriesColorChange(GraphCollection(.ListBox1.SelectedIndex + 1))
                Else
                    SeriesColorChange(AddGraphCollection(.ListBox1.SelectedIndex + 1 - 48))
                End If

            End If
        End With
    End Sub

    Private Sub SeriesColorChange(ByVal Tchart As Chart)
        With Me
            Tchart.Series(.SeriesLstBox.SelectedIndex).Color = .SeriesColor.Color
            Me.BtnClr.BackColor = .SeriesColor.Color
            Me.BtnClr.Text = ""
        End With
    End Sub

    Private Sub Chart00_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart00.Click
        Me.ListBox1.SelectedIndex = 0
        Me.Chart00.Focus()
    End Sub
    Private Sub Chart01_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart01.Click
        Me.ListBox1.SelectedIndex = 1
        Me.Chart01.Focus()
    End Sub
    Private Sub Chart02_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart02.Click
        Me.ListBox1.SelectedIndex = 2
        Me.Chart02.Focus()
    End Sub
    Private Sub Chart03_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart03.Click
        Me.ListBox1.SelectedIndex = 3
        Me.Chart03.Focus()
    End Sub
    Private Sub Chart04_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart04.Click
        Me.ListBox1.SelectedIndex = 4
        Me.Chart04.Focus()
    End Sub
    Private Sub Chart05_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart05.Click
        Me.ListBox1.SelectedIndex = 5
        Me.Chart05.Focus()
    End Sub
    Private Sub Chart06_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart06.Click
        Me.ListBox1.SelectedIndex = 6
        Me.Chart06.Focus()
    End Sub
    Private Sub Chart07_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart07.Click
        Me.ListBox1.SelectedIndex = 7
        Me.Chart07.Focus()
    End Sub
    Private Sub Chart08_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart08.Click
        Me.ListBox1.SelectedIndex = 8
        Me.Chart08.Focus()
    End Sub
    Private Sub Chart09_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart09.Click
        Me.ListBox1.SelectedIndex = 9
        Me.Chart09.Focus()
    End Sub
    Private Sub Chart10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart10.Click
        Me.ListBox1.SelectedIndex = 10
        Me.Chart10.Focus()
    End Sub
    Private Sub Chart11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart11.Click
        Me.ListBox1.SelectedIndex = 11
        Me.Chart11.Focus()
    End Sub
    Private Sub Chart12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart12.Click
        Me.ListBox1.SelectedIndex = 12
        Me.Chart12.Focus()
    End Sub
    Private Sub Chart13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart13.Click
        Me.ListBox1.SelectedIndex = 13
        Me.Chart13.Focus()
    End Sub
    Private Sub Chart14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart14.Click
        Me.ListBox1.SelectedIndex = 14
        Me.Chart14.Focus()
    End Sub
    Private Sub Chart15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart15.Click
        Me.ListBox1.SelectedIndex = 15
        Me.Chart15.Focus()
    End Sub
    Private Sub Chart16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart16.Click
        Me.ListBox1.SelectedIndex = 16
        Me.Chart16.Focus()
    End Sub
    Private Sub Chart17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart17.Click
        Me.ListBox1.SelectedIndex = 17
        Me.Chart17.Focus()
    End Sub
    Private Sub Chart18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart18.Click
        Me.ListBox1.SelectedIndex = 18
        Me.Chart18.Focus()
    End Sub
    Private Sub Chart19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart19.Click
        Me.ListBox1.SelectedIndex = 19
        Me.Chart19.Focus()
    End Sub
    Private Sub Chart20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart20.Click
        Me.ListBox1.SelectedIndex = 20
        Me.Chart20.Focus()
    End Sub
    Private Sub Chart21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart21.Click
        Me.ListBox1.SelectedIndex = 21
        Me.Chart21.Focus()
    End Sub
    Private Sub Chart22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart22.Click
        Me.ListBox1.SelectedIndex = 22
        Me.Chart22.Focus()
    End Sub
    Private Sub Chart23_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart23.Click
        Me.ListBox1.SelectedIndex = 23
        Me.Chart23.Focus()
    End Sub
    Private Sub Chart24_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart24.Click
        Me.ListBox1.SelectedIndex = 24
        Me.Chart24.Focus()
    End Sub
    Private Sub Chart25_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart25.Click
        Me.ListBox1.SelectedIndex = 25
        Me.Chart25.Focus()
    End Sub
    Private Sub Chart26_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart26.Click
        Me.ListBox1.SelectedIndex = 26
        Me.Chart26.Focus()
    End Sub
    Private Sub Chart27_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart27.Click
        Me.ListBox1.SelectedIndex = 27
        Me.Chart27.Focus()
    End Sub
    Private Sub Chart28_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart28.Click
        Me.ListBox1.SelectedIndex = 28
        Me.Chart28.Focus()
    End Sub
    Private Sub Chart29_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart29.Click
        Me.ListBox1.SelectedIndex = 29
        Me.Chart29.Focus()
    End Sub
    Private Sub Chart30_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart30.Click
        Me.ListBox1.SelectedIndex = 30
        Me.Chart30.Focus()
    End Sub
    Private Sub Chart31_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart31.Click
        Me.ListBox1.SelectedIndex = 31
        Me.Chart31.Focus()
    End Sub
    Private Sub Chart32_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart32.Click
        Me.ListBox1.SelectedIndex = 32
        Me.Chart32.Focus()
    End Sub
    Private Sub Chart33_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart33.Click
        Me.ListBox1.SelectedIndex = 33
        Me.Chart33.Focus()
    End Sub
    Private Sub Chart34_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart34.Click
        Me.ListBox1.SelectedIndex = 34
        Me.Chart34.Focus()
    End Sub
    Private Sub Chart35_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart35.Click
        Me.ListBox1.SelectedIndex = 35
        Me.Chart35.Focus()
    End Sub
    Private Sub Chart36_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart36.Click
        Me.ListBox1.SelectedIndex = 36
        Me.Chart36.Focus()
    End Sub
    Private Sub Chart37_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart37.Click
        Me.ListBox1.SelectedIndex = 37
        Me.Chart37.Focus()
    End Sub
    Private Sub Chart38_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart38.Click
        Me.ListBox1.SelectedIndex = 38
        Me.Chart38.Focus()
    End Sub
    Private Sub Chart39_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart39.Click
        Me.ListBox1.SelectedIndex = 39
        Me.Chart39.Focus()
    End Sub
    Private Sub Chart40_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart40.Click
        Me.ListBox1.SelectedIndex = 40
        Me.Chart40.Focus()
    End Sub
    Private Sub Chart41_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart41.Click
        Me.ListBox1.SelectedIndex = 41
        Me.Chart41.Focus()
    End Sub
    Private Sub Chart42_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart42.Click
        Me.ListBox1.SelectedIndex = 42
        Me.Chart42.Focus()
    End Sub
    Private Sub Chart43_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart43.Click
        Me.ListBox1.SelectedIndex = 43
        Me.Chart43.Focus()
    End Sub
    Private Sub Chart44_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart44.Click
        Me.ListBox1.SelectedIndex = 44
        Me.Chart44.Focus()
    End Sub
    Private Sub Chart45_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart45.Click
        Me.ListBox1.SelectedIndex = 45
        Me.Chart45.Focus()
    End Sub
    Private Sub Chart46_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart46.Click
        Me.ListBox1.SelectedIndex = 46
        Me.Chart46.Focus()
    End Sub
    Private Sub Chart47_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart47.Click
        Me.ListBox1.SelectedIndex = 47
        Me.Chart47.Focus()
    End Sub

    Private Sub Chart00_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart00.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(0)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart01_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart01.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(1)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart02_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart02.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(2)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart03_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart03.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(3)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart04_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart04.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(4)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart05_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart05.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(5)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart06_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart06.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(6)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart07_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart07.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(7)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart08_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart08.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(8)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart09_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart09.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(9)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart10_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart10.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(10)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart11_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart11.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(11)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart12_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart12.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(12)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart13_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart13.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(13)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart14_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart14.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(14)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart15_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart15.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(15)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart16_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart16.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(16)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart17_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart17.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(17)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart18_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart18.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(18)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart19_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart19.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(19)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart20_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart20.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(20)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart21_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart21.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(21)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart22_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart22.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(22)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart23_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart23.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(23)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart24_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart24.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(24)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart25_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart25.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(25)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart26_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart26.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(26)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart27_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart27.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(27)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart28_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart28.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(28)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart29_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart29.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(29)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart30_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart30.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(30)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart31_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart31.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(31)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart32_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart32.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(32)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart33_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart33.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(33)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart34_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart34.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(34)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart35_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart35.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(35)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart36_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart36.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(36)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart37_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart37.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(37)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart38_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart38.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(38)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart39_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart39.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(39)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart40_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart40.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(40)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart41_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart41.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(41)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart42_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart42.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(42)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart43_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart43.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(43)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart44_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart44.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(44)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart45_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart45.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(45)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart46_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart46.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(46)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart47_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart47.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(47)
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub

    Private Sub PlotGradientToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlotGradientToolStripMenuItem.Click

        Dim GraphGradientDP As New DATA_Graphing

        For i = 1 To 48
            GraphGradientDP.GradientChart(GraphCollection.Item(i))
        Next
        For i = 1 To 8
            GraphGradientDP.GradientChart(AddGraphCollection.Item(i))
        Next
    End Sub

    '차트 복사 방법
    Private Sub ListBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListBox1.KeyDown

        Dim SelectedInList As Integer = Me.ListBox1.SelectedIndex + 1

        Try

            If e.Modifiers = Keys.Control And e.KeyCode = Keys.V Then

                Dim MsgResult As Integer = MessageBox.Show("Maintain Original Axis Setting?" & vbCrLf & "** Yes - Maintain Original Axis" _
                                                           & vbCrLf & "** No - Set All Series to Primary Axis" _
                                                           & vbCrLf & "** Cancel - Set All Series to Secondary Axis", "Select Axis Type", MessageBoxButtons.YesNoCancel)

                If MsgResult = DialogResult.Cancel Then
                    If SelectedInList <= 48 Then
                        InjuryChart_CopyEventEnd_Secondary(MovinChrt, GraphCollection(SelectedInList))
                    Else
                        InjuryChart_CopyEventEnd_Secondary(MovinChrt, AddGraphCollection(SelectedInList - 48))
                    End If
                ElseIf MsgResult = DialogResult.No Then
                    If SelectedInList <= 48 Then
                        InjuryChart_CopyEventEnd_Primary(MovinChrt, GraphCollection(SelectedInList))
                    Else
                        InjuryChart_CopyEventEnd_Primary(MovinChrt, AddGraphCollection(SelectedInList - 48))
                    End If
                ElseIf MsgResult = DialogResult.Yes Then
                    If SelectedInList <= 48 Then
                        InjuryChart_CopyEventEnd_Maintain(MovinChrt, GraphCollection(SelectedInList))
                    Else
                        InjuryChart_CopyEventEnd_Maintain(MovinChrt, AddGraphCollection(SelectedInList - 48))
                    End If
                End If

            ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then

                '복사할 그래프
                If SelectedInList <= 48 Then
                    MovinChrt = GraphCollection(SelectedInList)
                Else
                    MovinChrt = AddGraphCollection(SelectedInList - 48)
                End If

            End If

        Catch ex As NullReferenceException

            MsgBox("Select and Copy(Ctrl+C) Graph First", MsgBoxStyle.Critical, "Empty Chart")
            Exit Sub

        End Try

    End Sub

    Private Sub InjuryChart_CopyEventEnd_Primary(ByRef MoveChart As Chart, ByRef TChart As Chart)
        Dim i, j As Integer
        Dim tmp As DataPointCollection
        Dim Titles As String
        Dim Temp_title() As String

        With Me
            '데이터 포인트 컬랙션을 복사한다.===============================================================================
            For i = 0 To MoveChart.Series.Count - 1
                Try
                    Titles = MoveChart.Titles(0).Text & "::" & MoveChart.Series(i).Name
                Catch ex As Exception
                    Titles = GraphTitle_THOR(48) & "::" & MoveChart.Series(i).Name
                End Try
                Temp_title = Titles.Split("::")
                Titles = Temp_title(UBound(Temp_title) - 2) & "::" & Temp_title(UBound(Temp_title))
                tmp = MoveChart.Series(i).Points
                Try
                    TChart.Series.Add(Titles)
                Catch ex As Exception '같은 이름이 있는경우
                    'TChart.Series.Add(Titles & "-Re")  --> 뭔가 이상하게 그려진다....???
                    MsgBox("Same series already exists.", MsgBoxStyle.Critical, "Repetition")
                    DrawingADDgraphs(TChart)
                    Exit Sub
                End Try

                TChart.Series(Titles).YAxisType = AxisType.Primary

                For j = 0 To tmp.Count - 1
                    TChart.Series(Titles).Points.AddXY(tmp(j).XValue, tmp(j).YValues(0))
                Next
            Next
            TChart.Update()
            DrawingADDgraphs(TChart)
        End With
    End Sub

    Private Sub InjuryChart_CopyEventEnd_Secondary(ByRef MoveChart As Chart, ByRef TChart As Chart)
        Dim i, j As Integer
        Dim tmp As DataPointCollection
        Dim Titles As String
        Dim Temp_title() As String

        With Me
            '데이터 포인트 컬랙션을 복사한다.===============================================================================
            For i = 0 To MoveChart.Series.Count - 1
                Try
                    Titles = MoveChart.Titles(0).Text & "::" & MoveChart.Series(i).Name
                Catch ex As Exception
                    Titles = GraphTitle_THOR(48) & "::" & MoveChart.Series(i).Name
                End Try
                Temp_title = Titles.Split("::")
                Titles = Temp_title(UBound(Temp_title) - 2) & "::" & Temp_title(UBound(Temp_title))
                tmp = MoveChart.Series(i).Points
                Try
                    TChart.Series.Add(Titles)
                Catch ex As Exception '같은 이름이 있는경우
                    'TChart.Series.Add(Titles & "-Re")  --> 뭔가 이상하게 그려진다....???
                    MsgBox("Same series already exists.", MsgBoxStyle.Critical, "Repetition")
                    DrawingADDgraphs(TChart)
                    Exit Sub
                End Try

                With TChart
                    .Series(Titles).YAxisType = AxisType.Secondary
                    .Series(.Series.Count - 1).ChartType = SeriesChartType.Line
                    .ChartAreas(0).AxisY2.MinorGrid.Enabled = False
                    .ChartAreas(0).AxisY2.MajorGrid.Enabled = False
                    .ChartAreas(0).AxisY2.MajorTickMark.Enabled = True
                    .ChartAreas(0).AxisY2.LabelStyle.Font = New Font("Arial", .ChartAreas(0).AxisY.LabelStyle.Font.Size - 1, FontStyle.Bold)
                End With

                For j = 0 To tmp.Count - 1
                    TChart.Series(Titles).Points.AddXY(tmp(j).XValue, tmp(j).YValues(0))
                Next
            Next
            TChart.Update()
            DrawingADDgraphs(TChart)
        End With
    End Sub

    Private Sub InjuryChart_CopyEventEnd_Maintain(ByRef MoveChart As Chart, ByRef TChart As Chart)
        Dim i, j As Integer
        Dim tmp As DataPointCollection
        Dim Titles As String
        Dim Temp_title() As String

        With Me
            '데이터 포인트 컬랙션을 복사한다.===============================================================================
            For i = 0 To MoveChart.Series.Count - 1
                Try
                    Titles = MoveChart.Titles(0).Text & "::" & MoveChart.Series(i).Name
                Catch ex As Exception
                    Titles = GraphTitle_THOR(48) & "::" & MoveChart.Series(i).Name
                End Try
                Temp_title = Titles.Split("::")
                Titles = Temp_title(UBound(Temp_title) - 2) & "::" & Temp_title(UBound(Temp_title))
                tmp = MoveChart.Series(i).Points
                Try
                    TChart.Series.Add(Titles)
                Catch ex As Exception '같은 이름이 있는경우
                    'TChart.Series.Add(Titles & "-Re")  --> 뭔가 이상하게 그려진다....???
                    MsgBox("Same series already exists.", MsgBoxStyle.Critical, "Repetition")
                    DrawingADDgraphs(TChart)
                    Exit Sub
                End Try

                With TChart
                    .Series(.Series.Count - 1).ChartType = SeriesChartType.Line
                    Select Case MoveChart.Series(i).YAxisType
                        Case AxisType.Primary
                            .Series(TChart.Series.Count - 1).YAxisType = AxisType.Primary
                        Case AxisType.Secondary
                            .ChartAreas(0).AxisY2.Enabled = AxisEnabled.True
                            .Series(TChart.Series.Count - 1).YAxisType = AxisType.Secondary
                    End Select
                    .ChartAreas(0).AxisY2.MinorGrid.Enabled = False
                    .ChartAreas(0).AxisY2.MajorGrid.Enabled = False
                    .ChartAreas(0).AxisY2.MajorTickMark.Enabled = True
                    .ChartAreas(0).AxisY2.LabelStyle.Font = New Font("Arial", .ChartAreas(0).AxisY.LabelStyle.Font.Size - 1, FontStyle.Bold)
                End With

                '.Series(MoveChart.Series.Count + i).YAxisType = AxisType.Secondary

                For j = 0 To tmp.Count - 1
                    TChart.Series(Titles).Points.AddXY(tmp(j).XValue, tmp(j).YValues(0))
                Next
            Next
            TChart.Update()
            DrawingADDgraphs(TChart)
        End With
    End Sub

    Private Sub ResetGraph(ByVal i As Integer)
        Dim k As Integer
        DelAllSeries(GraphCollection.Item(i + 1))
        For k = 0 To UBound(Names)
            '데이터를 읽어오는 함수
            Analopen.DataReading(Path(k), Names(k), Graph_Ext_THOR(i), DATA_Pos(i, k, 0), DATA_Pos(i, k, 1), DATA_Tot_Len(i, k, 0), DATA_Tot_Len(i, k, 1), TimeVals, DataVals, ConversionF(i))
            '데이터를 그래프에 올리는 함수
            AddSeries(GraphCollection.Item(i + 1), TimeVals, DataVals, k + 1)
            'Display 정리
            DATAGraphDP.GraphAxisLine(GraphCollection(i + 1))
        Next
    End Sub

    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        Me.Opacity = Me.TrackBar1.Value / 100
        If Me.Opacity <> 1.0 Then
            Me.TrackBar1.BackColor = Color.Red
        Else
            Me.TrackBar1.BackColor = SystemColors.Control
        End If
    End Sub

    Private Sub YIntervalTxt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles YIntervalTxt.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) And Me.ListBox1.SelectedIndex <> -1 Then
            With Me
                If IsNumeric(.YIntervalTxt.Text) = True Then
                    Apply_Interval(GraphCollection(.ListBox1.SelectedIndex + 1), CDbl(.YIntervalTxt.Text))
                End If
            End With
        Else
            Exit Sub
        End If
    End Sub

    Private Sub Apply_Interval(ByVal Tchart As Chart, ByVal Interval As Double)
        With Tchart
            .ChartAreas(0).AxisY.MajorGrid.Interval = Interval
            .ChartAreas(0).AxisY.MajorTickMark.Interval = Interval
            .ChartAreas(0).AxisY.LabelStyle.Interval = Interval
        End With
    End Sub

    Private Sub CopyGraphAreaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyGraphAreaToolStripMenuItem.Click
        '********* 컨트롤의 영역을 복사하는 방법 *********
        Dim controlImage As Bitmap = GetControlScreenshot(Me.GraphTab.TabPages(Me.GraphTab.SelectedIndex))
        'controlImage.Save("TestImage.bmp")
        Clipboard.SetDataObject(controlImage)
    End Sub

    Private Function GetControlScreenshot(ByVal control As Control) As Bitmap
        Dim g As Graphics = control.CreateGraphics()
        Dim bitmap As Bitmap = New Bitmap(control.Width, control.Height)
        control.DrawToBitmap(bitmap, New Rectangle(control.Location, control.Size))
        GetControlScreenshot = bitmap
    End Function
End Class