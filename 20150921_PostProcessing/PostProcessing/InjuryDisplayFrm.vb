Option Explicit On

Imports System.IO
Imports System.Diagnostics
Imports AxMSFlexGridLib
Imports Microsoft.Office.Interop
Imports Microsoft.Office.Core
Imports DSOFile

Public Class InjuryDisplayFrm

#Region "Global Variables"

    Dim Tot_File As Integer '전체 파일 수
    Dim OpenFile As New GlobalClass '읽어온 파일명 저장 (Path와 확장자는 없음)
    Dim Tmp_Path() As String

    Dim IsRemotePath() As String

    Dim EventPass As Boolean

    '파워포인트를 만들기 위한 변수===========================
    Dim objPPT As PowerPoint.Application
    Dim objPres As PowerPoint.Presentation
    '========================================================

    '더미구분 변수
    Public DummyType() As Integer '0=50% H-3 / 1=5% H-3 / 2=50%THOR

    '데이터의 위치 및 총 행/열수 저장변수
    Dim ParaMeterArr1(,,) As Integer
    '{그래프 번호 , Case 번호 , 데이터위치(Row/Col) )
    Dim ParaMeterArr2(,,) As Integer
    '{그래프 번호 , Case 번호 , 총채널수(Row/Col) )

    'Scale Factor 저장변수
    Dim ConversionFactor() As Decimal
    '{Conversion Factor}

    '상관성 계수-Correlation Factor 저장 변수
    Dim InjuryCorrel(28) As Single
    Dim GraphCorrel(31) As Single

    Public GraphBased As Boolean = False

    '상해저장 변수
    Public Head3MSG() As Double     'H3MS_inj
    Public Chest_G_CUMULATIVE_T3MS_inj() As Double      'T3MS_inj
    Public HIC15() As Double        'HIC15_inj
    Public HIC36() As Double        'HIC36_inj
    Public NTE() As Double      'NTE_inj
    Public NTF() As Double      'NTF_inj
    Public NCE() As Double      'NCE_inj
    Public NCF() As Double      'NCF_inj
    Public Head_Peak_G() As Double      'HaccRpeak_inj
    'Public Chest_G() As Double      'TaccRpeak_inj 5%없음
    Public Chest_D() As Double      'ThCC_inj
    Public KneeS_L() As Double      'kneesliderL_inj
    Public KneeS_R() As Double      'kneesliderR_inj
    Public Tibia_Comp_L() As Double     'TCFCLowL_inj
    Public Tibia_Comp_R() As Double     'TCFCLowR_inj
    Public TI_upr_L() As Double     'TIUpL_inj
    Public TI_lwr_L() As Double     'TILowL_inj
    Public TI_upr_R() As Double     'TIUpR_inj
    Public TI_lwr_R() As Double     'TILowR_inj
    Public Chest_VC() As Double     'VC_inj_CFC180
    Public FemurL() As Double       'FFCL_inj
    Public FemurR() As Double       'FFCR_inj
    Public Neck_Comp() As Double        'FNICtension_inj
    Public Neck_Tens() As Double        'FNICtension_inj
    Public Neck_Shear() As Double       'FNICshear_inj
    Public Neck_Exten() As Double       'FNICbending_inj
    '===============For THOR ATD =============================
    Public ThxIrUpL() As Double          'ThxIrTraccRibL_CFC600_dis
    Public ThxIrUpR() As Double         'ThxIrTraccRibR_CFC600_dis
    Public ThxIrLowL() As Double         'ThxLowIrTraccRibL_CFC600_dis
    Public ThxIrLowR() As Double         'ThxLowIrTraccRibR_CFC600_dis
    '=========================================================
    Public Neck_Flex() As Double       'For Q-Dummy

    'Probability for US-NCAP
    Public P_Head() As Double
    Public P_Neck_Tens() As Double
    Public P_Neck_Comp() As Double
    Public P_Neck_NTE() As Double
    Public P_Neck_NTF() As Double
    Public P_Neck_NCE() As Double
    Public P_Neck_NCF() As Double
    Public P_Neck_Max() As Double
    Public P_FemurL() As Double
    Public P_FemurR() As Double
    Public P_Femur_Max() As Double
    Public P_CD() As Double
    Public Star_Rating() As Double

#End Region

    '모든 자식폼을 같이 닫는 코드 : MDI의 Child Form에는 적용되지 않는듯
    'Private Sub InjuryDisplayFrm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    '    Me.Form_Closeing(0, Me)
    'End Sub

    'Private Sub Form_Closeing(ByRef Count As Integer, ByVal FindForm As System.Windows.Forms.Form)

    '    For Each Children As Form In FindForm.OwnedForms
    '        If Children.OwnedForms.GetLength(0) > 0 Then
    '            Me.Form_Closeing(Count, Children)
    '        End If
    '        Count += 1
    '        Children.Dispose()
    '    Next Children

    'End Sub

    Public Sub New(ByVal Temp_Injury_Correl() As Single, ByVal Temp_Graph_Correl() As Single, ByVal GraphBasedInjury As Boolean)

        ' 이 호출은 디자이너에 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하십시오.

        '상관성 계수-Correlation Factor 저장 변수
        InjuryCorrel = Temp_Injury_Correl
        GraphCorrel = Temp_Graph_Correl
        GraphBased = GraphBasedInjury
    End Sub

    Private Sub InjuryDisplayFrm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F5 Then
            Call ReloadBtn_Click(e, Nothing)
        End If
    End Sub

    Private Sub InjuryDisplayFrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        MainMDI.ProgressBarMain.Value = 0
        MainMDI.ProgressBarMain.Maximum = 1140

        Me.Hide()
        Application.DoEvents()
        Me.MdiParent = MainMDI

        Me.CommentsToolTip.SetToolTip(Me.DescripTxt, "To Save" & vbCrLf & "Ctrl + S")

        'If TempLicExpire = False Then
        '    '원격로그
        '    Try
        '        Using NewfileNum As StreamWriter = File.AppendText(RemoteValFolder & System.Environment.UserName)
        '            NewfileNum.WriteLine("## Injury Display Form Open : " & Date.Now.ToString & " ##")
        '            NewfileNum.Close()
        '        End Using
        '    Catch ex As Exception
        '        'End
        '    End Try
        'End If

        Dim i As Integer
        'Dim Tmp_Path() As String

        If FileDrop = False Then

            If IsRemote = False Then

                With Me.OpenDlg
                    .Title = "Peak 파일 선택"
                    .Multiselect = True
                    .FileName = ""                   '초기에 표시되는 파일 이름
                    .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
                    .Filter = "Peak 파일|*.peak|모든 파일|*.*"
                    .ShowDialog()
                End With

                If Me.OpenDlg.FileNames(0) = "" Then
                    MainMDI.ProgressBarMain.Value = 0
                    MainMDI.Statuslbl.Text = "Cancel"
                    Me.Close()
                    Exit Sub
                End If

                'Array.Sort(Me.OpenDlg.FileNames)
                'Array.Sort(Me.OpenDlg.SafeFileNames)

                'MsgBox(Me.OpenAnalDlg.FileNames(0))         'Path를 포함한 파일명 배열
                'MsgBox(Me.OpenAnalDlg.SafeFileNames(0))     'Path가 제외(확장자 포함된 배열)
                OpenFile.names = FileNameGet(Me.OpenDlg.SafeFileNames)  '파일명만 빼온다.(확장자 없음
                ReDim Tmp_Path(UBound(OpenFile.names))
                ReDim IsRemotePath(UBound(OpenFile.names))
                Me.PathLbl.Text = "Path : " & FilePathGet(Me.OpenDlg.FileNames)
                For i = 0 To UBound(OpenFile.names)
                    Tmp_Path(i) = FilePathGet2(Me.OpenDlg.FileNames(i))
                    IsRemotePath(i) = ""
                Next

            Else

                If Connected = False Then
                    MsgBox("Connect to Server!!", MsgBoxStyle.Critical, "Error")
                    Me.Close()
                    Exit Sub
                End If

                Dim RemoteFolder As New FrmRemoteFileLst("Reading")

                Me.Hide()
                RemoteFolder.ShowDialog(Me)

                If IsNothing(RemoteFilesNames) = False Then
                    OpenFile.names = RemoteFilesNames
                    ReDim Tmp_Path(UBound(OpenFile.names))
                    ReDim IsRemotePath(UBound(OpenFile.names))
                    Me.PathLbl.Text = "Path : " & "[Remote]" & Application.StartupPath & "\TempResults\"
                    For i = 0 To UBound(OpenFile.names)
                        Tmp_Path(i) = Application.StartupPath & "\TempResults\"
                        IsRemotePath(i) = RemoteFolderName ' \포함
                    Next
                Else
                    IsRemote = False
                    Me.Close()
                    Exit Sub
                End If

                IsRemote = False

            End If

        Else

            ReDim Tmp_Path(UBound(DragFiles))
            ReDim IsRemotePath(UBound(DragFiles))
            Tmp_Path(0) = StrReverse(DragFiles(0))
            Tmp_Path(0) = Mid(Tmp_Path(0), InStr(Tmp_Path(0), "\")).ToString
            Tmp_Path(0) = StrReverse(Tmp_Path(0))
            IsRemotePath(0) = ""

            Dim TmpArrPath() As String
            For i = 0 To UBound(DragFiles)
                Tmp_Path(i) = FilePathGet2(DragFiles(i))
                IsRemotePath(i) = ""
                TmpArrPath = DragFiles(i).Split("\")
                DragFiles(i) = TmpArrPath(UBound(TmpArrPath))
            Next


            OpenFile.names = FileNameGet(DragFiles)

            Me.PathLbl.Text = "Path : " & Tmp_Path(0)
            'Tmp_Path = FilePathGet(DragFiles)
        End If

        'MsgBox(FilePathGet(Me.OpenDlg.FileNames))   'Path 표시
        'For i = 0 To UBound(OpenFile.names)
        '    MsgBox(OpenFile.names(i), , "파일명")  '파일명 표시
        'Next

        'MsgBox("총 파일수 : " & OpenFile.names.Length)

        Me.ScoreBoard.TabPages(0).Text = "US-NCAP"
        Me.ScoreBoard.TabPages(1).Text = "DOM-Frontal"
        Me.ScoreBoard.TabPages(2).Text = "DOM-Offset"
        Me.ScoreBoard.TabPages(3).Text = "China-Frontal"
        Me.ScoreBoard.TabPages(4).Text = "China-Offset"
        Me.ScoreBoard.TabPages(5).Text = "China-Rear"
        Me.ScoreBoard.TabPages(6).Text = "Euro-Frontal"
        Me.ScoreBoard.TabPages(7).Text = "Euro-Offset"

        With Me.OpenLst
            For i = 0 To UBound(OpenFile.names)
                .Items.Insert(i, i + 1 & ". " & OpenFile.names(i))
            Next
            .Height = 105
        End With

        If OpenFile.names.Length > 1 Then Me.BtnValGraph.Enabled = True

        ReDim DummyType(UBound(OpenFile.names))

        MainMDI.ProgressBarMain.Value = 20
        '더미구분
        MainMDI.Statuslbl.Text = "Configure Dummies..."
        Dummy(UBound(OpenFile.names), Tmp_Path)
        ' 0 = Hybrid-Ⅲ 50%
        ' 1 = Hybrid-Ⅲ 5%
        ' 2 = THOR-NT 50%
        ' 3 = Q-6
        ' 4 = Q-10

        ''Temporary Code : Q-Dummy Skip
        'For i = 0 To UBound(DummyType)
        '    If DummyType(i) = 3 Then
        '        MsgBox("Q-6 dummy model cannot be imported yet...", MsgBoxStyle.Information, "Try again")
        '        Me.Close()
        '    ElseIf DummyType(i) = 4 Then
        '        MsgBox("Q-10 dummy model cannot be imported yet...", MsgBoxStyle.Information, "Try again")
        '        Me.Close()
        '    End If
        'Next

        MainMDI.ProgressBarMain.Value = 50
        ''상해를 읽는다
        'Reading_Peak()

        If OpenFile.names.Length > 5 Then
            Me.Height = 780
        Else
            Me.Height = 630
        End If

        ' ''시트를 그린다.
        ''MainMDI.Statuslbl.Text = "Drawing Injury Sheets"
        ''MainMDI.ProgressBarMain.Value = 100
        ''Sheet_Lbl()
        ''Sheet_US()

        ''MainMDI.ProgressBarMain.Value = 200
        ''Sheet_Lbl2()
        ''Sheet_DOM_F()

        ''MainMDI.ProgressBarMain.Value = 300
        ''Sheet_Lbl3()
        ''Sheet_DOM_O()

        ''MainMDI.ProgressBarMain.Value = 400
        ''Sheet_Lbl4()
        ''Sheet_China_F()

        ''Sheet_Lbl5()
        ''Sheet_China_0()

        ''MainMDI.ProgressBarMain.Value = 500

        ''Sheet_Lbl6()
        ''Sheet_China_F_Rear()
        ''Sheet_China_O_Rear()

        '그래프 파라미터를 미리 읽어온다.
        Dim RowCol(1) As Integer

        ReDim ParaMeterArr1(31, UBound(OpenFile.names), 1)
        '{그래프 번호 , Case 번호 , 데이터위치(Row/Col) )
        ReDim ParaMeterArr2(31, UBound(OpenFile.names), 1)
        '{그래프 번호 , Case 번호 , 총채널수(Row/Col) )
        ReDim ConversionFactor(31)
        '{그래프 Conversion Factor - 단위변환용}

        MainMDI.Statuslbl.Text = "Loading DATA Location Parameters"
        For i = 0 To UBound(OpenFile.names)
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(0), Graph_01)
            ParaMeterArr1(0, i, 0) = RowCol(0)
            ParaMeterArr1(0, i, 1) = RowCol(1)
            ParaMeterArr2(0, i, 0) = RowCol(2)
            ParaMeterArr2(0, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 100
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(1), Graph_02)
            ParaMeterArr1(1, i, 0) = RowCol(0)
            ParaMeterArr1(1, i, 1) = RowCol(1)
            ParaMeterArr2(1, i, 0) = RowCol(2)
            ParaMeterArr2(1, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 120
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(2), Graph_03)
            ParaMeterArr1(2, i, 0) = RowCol(0)
            ParaMeterArr1(2, i, 1) = RowCol(1)
            ParaMeterArr2(2, i, 0) = RowCol(2)
            ParaMeterArr2(2, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 140
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(3), Graph_04)
            ParaMeterArr1(3, i, 0) = RowCol(0)
            ParaMeterArr1(3, i, 1) = RowCol(1)
            ParaMeterArr2(3, i, 0) = RowCol(2)
            ParaMeterArr2(3, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 160
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(4), Graph_05)
            ParaMeterArr1(4, i, 0) = RowCol(0)
            ParaMeterArr1(4, i, 1) = RowCol(1)
            ParaMeterArr2(4, i, 0) = RowCol(2)
            ParaMeterArr2(4, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 180
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(5), Graph_06)
            ParaMeterArr1(5, i, 0) = RowCol(0)
            ParaMeterArr1(5, i, 1) = RowCol(1)
            ParaMeterArr2(5, i, 0) = RowCol(2)
            ParaMeterArr2(5, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 200
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(6), Graph_07)
            ParaMeterArr1(6, i, 0) = RowCol(0)
            ParaMeterArr1(6, i, 1) = RowCol(1)
            ParaMeterArr2(6, i, 0) = RowCol(2)
            ParaMeterArr2(6, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 220
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(7), Graph_08)
            ParaMeterArr1(7, i, 0) = RowCol(0)
            ParaMeterArr1(7, i, 1) = RowCol(1)
            ParaMeterArr2(7, i, 0) = RowCol(2)
            ParaMeterArr2(7, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 240
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(8), Graph_09)
            ParaMeterArr1(8, i, 0) = RowCol(0)
            ParaMeterArr1(8, i, 1) = RowCol(1)
            ParaMeterArr2(8, i, 0) = RowCol(2)
            ParaMeterArr2(8, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 260
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(9), Graph_10)
            ParaMeterArr1(9, i, 0) = RowCol(0)
            ParaMeterArr1(9, i, 1) = RowCol(1)
            ParaMeterArr2(9, i, 0) = RowCol(2)
            ParaMeterArr2(9, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 280
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(10), Graph_11)
            ParaMeterArr1(10, i, 0) = RowCol(0)
            ParaMeterArr1(10, i, 1) = RowCol(1)
            ParaMeterArr2(10, i, 0) = RowCol(2)
            ParaMeterArr2(10, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 300
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(11), Graph_12)
            ParaMeterArr1(11, i, 0) = RowCol(0)
            ParaMeterArr1(11, i, 1) = RowCol(1)
            ParaMeterArr2(11, i, 0) = RowCol(2)
            ParaMeterArr2(11, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 320
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(12), Graph_13)
            ParaMeterArr1(12, i, 0) = RowCol(0)
            ParaMeterArr1(12, i, 1) = RowCol(1)
            ParaMeterArr2(12, i, 0) = RowCol(2)
            ParaMeterArr2(12, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 340
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(13), Graph_14)
            ParaMeterArr1(13, i, 0) = RowCol(0)
            ParaMeterArr1(13, i, 1) = RowCol(1)
            ParaMeterArr2(13, i, 0) = RowCol(2)
            ParaMeterArr2(13, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 360
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(14), Graph_15)
            ParaMeterArr1(14, i, 0) = RowCol(0)
            ParaMeterArr1(14, i, 1) = RowCol(1)
            ParaMeterArr2(14, i, 0) = RowCol(2)
            ParaMeterArr2(14, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 380
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(15), Graph_16)
            ParaMeterArr1(15, i, 0) = RowCol(0)
            ParaMeterArr1(15, i, 1) = RowCol(1)
            ParaMeterArr2(15, i, 0) = RowCol(2)
            ParaMeterArr2(15, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 400
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(16), Graph_17)
            ParaMeterArr1(16, i, 0) = RowCol(0)
            ParaMeterArr1(16, i, 1) = RowCol(1)
            ParaMeterArr2(16, i, 0) = RowCol(2)
            ParaMeterArr2(16, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 420
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(17), Graph_18)
            ParaMeterArr1(17, i, 0) = RowCol(0)
            ParaMeterArr1(17, i, 1) = RowCol(1)
            ParaMeterArr2(17, i, 0) = RowCol(2)
            ParaMeterArr2(17, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 440
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(18), Graph_19)
            ParaMeterArr1(18, i, 0) = RowCol(0)
            ParaMeterArr1(18, i, 1) = RowCol(1)
            ParaMeterArr2(18, i, 0) = RowCol(2)
            ParaMeterArr2(18, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 460
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(19), Graph_20)
            ParaMeterArr1(19, i, 0) = RowCol(0)
            ParaMeterArr1(19, i, 1) = RowCol(1)
            ParaMeterArr2(19, i, 0) = RowCol(2)
            ParaMeterArr2(19, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 480
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(20), Graph_21)
            ParaMeterArr1(20, i, 0) = RowCol(0)
            ParaMeterArr1(20, i, 1) = RowCol(1)
            ParaMeterArr2(20, i, 0) = RowCol(2)
            ParaMeterArr2(20, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 500
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(21), Graph_22)
            ParaMeterArr1(21, i, 0) = RowCol(0)
            ParaMeterArr1(21, i, 1) = RowCol(1)
            ParaMeterArr2(21, i, 0) = RowCol(2)
            ParaMeterArr2(21, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 520
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(22), Graph_23)
            ParaMeterArr1(22, i, 0) = RowCol(0)
            ParaMeterArr1(22, i, 1) = RowCol(1)
            ParaMeterArr2(22, i, 0) = RowCol(2)
            ParaMeterArr2(22, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 540
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(23), Graph_24)
            ParaMeterArr1(23, i, 0) = RowCol(0)
            ParaMeterArr1(23, i, 1) = RowCol(1)
            ParaMeterArr2(23, i, 0) = RowCol(2)
            ParaMeterArr2(23, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 560
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(24), Graph_25)
            ParaMeterArr1(24, i, 0) = RowCol(0)
            ParaMeterArr1(24, i, 1) = RowCol(1)
            ParaMeterArr2(24, i, 0) = RowCol(2)
            ParaMeterArr2(24, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 580
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(25), Graph_26)
            ParaMeterArr1(25, i, 0) = RowCol(0)
            ParaMeterArr1(25, i, 1) = RowCol(1)
            ParaMeterArr2(25, i, 0) = RowCol(2)
            ParaMeterArr2(25, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 600
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(26), Graph_27)
            ParaMeterArr1(26, i, 0) = RowCol(0)
            ParaMeterArr1(26, i, 1) = RowCol(1)
            ParaMeterArr2(26, i, 0) = RowCol(2)
            ParaMeterArr2(26, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 620
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(27), Graph_28)
            ParaMeterArr1(27, i, 0) = RowCol(0)
            ParaMeterArr1(27, i, 1) = RowCol(1)
            ParaMeterArr2(27, i, 0) = RowCol(2)
            ParaMeterArr2(27, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 640
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(28), Graph_29)
            ParaMeterArr1(28, i, 0) = RowCol(0)
            ParaMeterArr1(28, i, 1) = RowCol(1)
            ParaMeterArr2(28, i, 0) = RowCol(2)
            ParaMeterArr2(28, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 660
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(29), Graph_30)
            ParaMeterArr1(29, i, 0) = RowCol(0)
            ParaMeterArr1(29, i, 1) = RowCol(1)
            ParaMeterArr2(29, i, 0) = RowCol(2)
            ParaMeterArr2(29, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 680
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(30), Graph_31)
            ParaMeterArr1(30, i, 0) = RowCol(0)
            ParaMeterArr1(30, i, 1) = RowCol(1)
            ParaMeterArr2(30, i, 0) = RowCol(2)
            ParaMeterArr2(30, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 700
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(31), Graph_32)
            ParaMeterArr1(31, i, 0) = RowCol(0)
            ParaMeterArr1(31, i, 1) = RowCol(1)
            ParaMeterArr2(31, i, 0) = RowCol(2)
            ParaMeterArr2(31, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 720
        Next

        'Conversion Factor Load and Correlation Factor Load
        '단위 환산 계수와 상관성 수치를 동시에 로드한다.
        ConversionFactor(0) = CDec(Graph_01(UBound(Graph_01))) * GraphCorrel(0)
        ConversionFactor(1) = CDec(Graph_02(UBound(Graph_02))) * GraphCorrel(1)
        ConversionFactor(2) = CDec(Graph_03(UBound(Graph_03))) * GraphCorrel(2)
        ConversionFactor(3) = CDec(Graph_04(UBound(Graph_04))) * GraphCorrel(3)
        ConversionFactor(4) = CDec(Graph_05(UBound(Graph_05))) * GraphCorrel(4)
        ConversionFactor(5) = CDec(Graph_06(UBound(Graph_06))) * GraphCorrel(5)
        ConversionFactor(6) = CDec(Graph_07(UBound(Graph_07))) * GraphCorrel(6)
        ConversionFactor(7) = CDec(Graph_08(UBound(Graph_08))) * GraphCorrel(7)
        ConversionFactor(8) = CDec(Graph_09(UBound(Graph_09))) * GraphCorrel(8)
        ConversionFactor(9) = CDec(Graph_10(UBound(Graph_10))) * GraphCorrel(9)
        ConversionFactor(10) = CDec(Graph_11(UBound(Graph_11))) * GraphCorrel(10)
        ConversionFactor(11) = CDec(Graph_12(UBound(Graph_12))) * GraphCorrel(11)
        ConversionFactor(12) = CDec(Graph_13(UBound(Graph_13))) * GraphCorrel(12)
        ConversionFactor(13) = CDec(Graph_14(UBound(Graph_14))) * GraphCorrel(13)
        ConversionFactor(14) = CDec(Graph_15(UBound(Graph_15))) * GraphCorrel(14)
        ConversionFactor(15) = CDec(Graph_16(UBound(Graph_16))) * GraphCorrel(15)
        ConversionFactor(16) = CDec(Graph_17(UBound(Graph_17))) * GraphCorrel(16)
        ConversionFactor(17) = CDec(Graph_18(UBound(Graph_18))) * GraphCorrel(17)
        ConversionFactor(18) = CDec(Graph_19(UBound(Graph_19))) * GraphCorrel(18)
        ConversionFactor(19) = CDec(Graph_20(UBound(Graph_20))) * GraphCorrel(19)
        ConversionFactor(20) = CDec(Graph_21(UBound(Graph_21))) * GraphCorrel(20)
        ConversionFactor(21) = CDec(Graph_22(UBound(Graph_22))) * GraphCorrel(21)
        ConversionFactor(22) = CDec(Graph_23(UBound(Graph_23))) * GraphCorrel(22)
        ConversionFactor(23) = CDec(Graph_24(UBound(Graph_24))) * GraphCorrel(23)
        ConversionFactor(24) = CDec(Graph_25(UBound(Graph_25))) * GraphCorrel(24)
        ConversionFactor(25) = CDec(Graph_26(UBound(Graph_26))) * GraphCorrel(25)
        ConversionFactor(26) = CDec(Graph_27(UBound(Graph_27))) * GraphCorrel(26)
        ConversionFactor(27) = CDec(Graph_28(UBound(Graph_28))) * GraphCorrel(27)
        ConversionFactor(28) = CDec(Graph_29(UBound(Graph_29))) * GraphCorrel(28)
        ConversionFactor(29) = CDec(Graph_30(UBound(Graph_30))) * GraphCorrel(29)
        ConversionFactor(30) = CDec(Graph_31(UBound(Graph_31))) * GraphCorrel(30)
        ConversionFactor(31) = CDec(Graph_32(UBound(Graph_32))) * GraphCorrel(31)

        MainMDI.ProgressBarMain.Value = 740

        '상해를 읽는다
        Reading_Peak()

        MainMDI.ProgressBarMain.Value = 800

        '시트를 그린다.
        MainMDI.Statuslbl.Text = "Drawing Injury Sheets"
        Sheet_Lbl()
        Sheet_US()

        MainMDI.ProgressBarMain.Value = 840

        Sheet_Lbl2()
        Sheet_DOM_F()

        MainMDI.ProgressBarMain.Value = 860

        Sheet_Lbl3()
        Sheet_DOM_O()

        MainMDI.ProgressBarMain.Value = 880

        Sheet_Lbl4()
        Sheet_China_F()

        MainMDI.ProgressBarMain.Value = 900

        Sheet_Lbl5()
        Sheet_China_0()

        MainMDI.ProgressBarMain.Value = 920

        Sheet_Lbl6()
        Sheet_China_F_Rear()
        Sheet_China_O_Rear()

        MainMDI.ProgressBarMain.Value = 940

        '유럽 시트들
        Sheet_Lbl7()
        Sheet_Euro_F()
        Sheet_Euro_Rear()

        Sheet_Lbl8()
        Sheet_Euro_O()


        Me.Show()

        FileDrop = False
        DragFiles = Nothing

        MainMDI.ProgressBarMain.Value = MainMDI.ProgressBarMain.Maximum

        Me.ScoreBoard.SelectedIndex = StartUpTab_NCAP

        '================================================================================================
        Try
            Using NewfileNum As StreamWriter = File.AppendText(RemoteValFolder & System.Environment.UserName)
                NewfileNum.WriteLine("##")
                For i = 0 To UBound(OpenFile.names)
                    NewfileNum.WriteLine("## File : " & Tmp_Path(i) & "\" & OpenFile.names(i))
                Next
                NewfileNum.Close()
            End Using
        Catch ex As Exception

        End Try
        '================================================================================================

        MainMDI.Statuslbl.Text = "Complete"

        Me.Width = 1100
    End Sub

    Private Sub Dummy(ByVal File_tot As Integer, ByVal Path() As String)

        '0=50% H-3 / 1=5% H-3 / 2=50%THOR

        Dim i As Integer
        Dim tmp_txt As String

        'fileNum = New FileStream(Path & OpenFile.names(i) & ".log", FileMode.Open, FileAccess.Read, FileShare.Read)

        For i = 0 To File_tot Step 1

            Dim ReadFiles As New FileStream(Path(i) & OpenFile.names(i) & ".log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            '맨 마지막 상수를 FileShare.ReadWrite 로 지정해야 해석 도중 접근이 가능함

            Using fileNum As New StreamReader(ReadFiles)
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If (InStr(1, tmp_txt, "d_thor50el", vbTextCompare) > 0 Or InStr(1, tmp_txt, "d_thorM50el", vbTextCompare) > 0) AndAlso (InStr(1, tmp_txt, "Opening", vbTextCompare) > 0 OrElse InStr(1, tmp_txt, "Assessing", vbTextCompare) > 0) Then
                        DummyType(i) = 2
                        Exit Do
                    ElseIf InStr(1, tmp_txt, "d_hyb350", vbTextCompare) > 0 AndAlso (InStr(1, tmp_txt, "Opening", vbTextCompare) > 0 OrElse InStr(1, tmp_txt, "Assessing", vbTextCompare) > 0) Then
                        DummyType(i) = 0
                        Exit Do
                    ElseIf InStr(1, tmp_txt, "d_hyb305", vbTextCompare) > 0 AndAlso (InStr(1, tmp_txt, "Opening", vbTextCompare) > 0 OrElse InStr(1, tmp_txt, "Assessing", vbTextCompare) > 0) Then
                        DummyType(i) = 1
                        Exit Do
                    ElseIf InStr(1, tmp_txt, "d_q06y", vbTextCompare) > 0 AndAlso (InStr(1, tmp_txt, "Opening", vbTextCompare) > 0 OrElse InStr(1, tmp_txt, "Assessing", vbTextCompare) > 0) Then
                        DummyType(i) = 3
                        Exit Do
                    ElseIf InStr(1, tmp_txt, "d_q10y", vbTextCompare) > 0 AndAlso (InStr(1, tmp_txt, "Opening", vbTextCompare) > 0 OrElse InStr(1, tmp_txt, "Assessing", vbTextCompare) > 0) Then
                        DummyType(i) = 4
                        Exit Do
                    End If
                Loop
            End Using

            ReadFiles.Close()
        Next
    End Sub

    Private Sub Reading_Peak()

        Dim i As Integer
        Dim tmp_txt As String
        Dim fileNum As StreamReader

        '배열 크기 재설정
        ReDim Head3MSG(UBound(OpenFile.names))      'H3MS_inj
        ReDim Chest_G_CUMULATIVE_T3MS_inj(UBound(OpenFile.names))       'T3MS_inj
        ReDim HIC15(UBound(OpenFile.names))         'HIC15_inj
        ReDim HIC36(UBound(OpenFile.names))         'HIC36_inj
        ReDim NTE(UBound(OpenFile.names))       'NTE_inj
        ReDim NTF(UBound(OpenFile.names))       'NTF_inj
        ReDim NCE(UBound(OpenFile.names))       'NCE_inj
        ReDim NCF(UBound(OpenFile.names))       'NCF_inj
        ReDim Head_Peak_G(UBound(OpenFile.names))       'HaccRpeak_inj
        'ReDim Chest_G(UBound(OpenFile.names))       'TaccRpeak_inj 50%없음
        ReDim Chest_D(UBound(OpenFile.names))       'ThCC_inj
        ReDim KneeS_L(UBound(OpenFile.names))       'kneesliderL_inj
        ReDim KneeS_R(UBound(OpenFile.names))       'kneesliderR_inj
        ReDim Tibia_Comp_L(UBound(OpenFile.names))      'TCFCLowL_inj
        ReDim Tibia_Comp_R(UBound(OpenFile.names))      'TCFCLowR_inj
        ReDim TI_upr_L(UBound(OpenFile.names))      'TIUpL_inj
        ReDim TI_lwr_L(UBound(OpenFile.names))      'TILowL_inj
        ReDim TI_upr_R(UBound(OpenFile.names))      'TIUpR_inj
        ReDim TI_lwr_R(UBound(OpenFile.names))      'TILowR_inj
        ReDim Chest_VC(UBound(OpenFile.names))      'VC_inj_CFC180
        ReDim FemurL(UBound(OpenFile.names))        'FFCL_inj
        ReDim FemurR(UBound(OpenFile.names))        'FFCR_inj
        ReDim Neck_Comp(UBound(OpenFile.names))         'FNICtension_inj
        ReDim Neck_Tens(UBound(OpenFile.names))         'FNICtension_inj
        ReDim Neck_Shear(UBound(OpenFile.names))        'FNICshear_inj
        ReDim Neck_Exten(UBound(OpenFile.names))        'FNICbending_inj

        ReDim P_Head(UBound(OpenFile.names))
        ReDim P_Neck_Tens(UBound(OpenFile.names))
        ReDim P_Neck_Comp(UBound(OpenFile.names))
        ReDim P_Neck_NTE(UBound(OpenFile.names))
        ReDim P_Neck_NTF(UBound(OpenFile.names))
        ReDim P_Neck_NCE(UBound(OpenFile.names))
        ReDim P_Neck_NCF(UBound(OpenFile.names))
        ReDim P_Neck_Max(UBound(OpenFile.names))
        ReDim P_FemurL(UBound(OpenFile.names))
        ReDim P_FemurR(UBound(OpenFile.names))
        ReDim P_Femur_Max(UBound(OpenFile.names))
        ReDim P_CD(UBound(OpenFile.names))

        '================================== For THOR 50% ATD Injury Values ==========================================
        ReDim ThxIrUpL(UBound(OpenFile.names))           'ThxIrTraccRibL_CFC600_dis
        ReDim ThxIrUpR(UBound(OpenFile.names))           'ThxIrTraccRibR_CFC600_dis
        ReDim ThxIrLowL(UBound(OpenFile.names))          'ThxLowIrTraccRibL_CFC600_dis
        ReDim ThxIrLowR(UBound(OpenFile.names))          'ThxLowIrTraccRibR_CFC600_dis
        '============================================================================================================

        ReDim Neck_Flex(UBound(OpenFile.names)) 'For Q-Dummy

        '위의 상해를 다 읽어온다.

        'Dim FilePathTemp As String = Mid(Me.PathLbl.Text, 8) 

        For i = 0 To UBound(OpenFile.names) Step 1

            'Dim ReadFiles As New FileStream(FilePathGet(Me.OpenDlg.FileNames) & OpenFile.names(i) & ".peak" _
            '                                , FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            Dim ReadFiles As New FileStream(Tmp_Path(i) & OpenFile.names(i) & ".peak" _
                                            , FileMode.Open, FileAccess.Read, FileShare.ReadWrite)

            fileNum = New StreamReader(ReadFiles)

            If DummyType(i) = 0 Or DummyType(i) = 1 Then  '================================================================ HYBRID-Ⅲ 50% / 5%

                If GraphBased = True Then
                    '그래프베이스 상해 관련*****************************************************************
                    '**************************************************************************************
                    '그래프 스케일도 바꾼다.
                    '**************************************************************************************
                    Dim TimeVals() As Decimal = Nothing
                    Dim DataVals() As Double = Nothing
                    Dim TmpHIC1536() As Double
                    Dim time_interval As Double
                    Dim Neck_Fx() As Double = Nothing
                    Dim Neck_Fz() As Double = Nothing
                    Dim Neck_My() As Double = Nothing
                    Dim Neck_Mocy() As Double
                    Dim Tmp_Array_NTE() As Double
                    Dim Tmp_Array_NTF() As Double
                    Dim Tmp_Array_NCE() As Double
                    Dim Tmp_Array_NCF() As Double

                    '파일을 열어온다
                    Dim InjuryFile As StreamReader
                    InjuryFile = New StreamReader(Application.StartupPath & "\Profile\GraphSetting.Gcorrel")
                    Dim Tmp_Str() As String

                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim Head_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NeckFx_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NeckFz_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NeckMy_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NeckMocy_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NTE_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NTF_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NCE_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NCF_Graph_Index As Integer = CInt(Tmp_Str(1))

                    InjuryFile.Close()

                    For k = 0 To UBound(OpenFile.names)
                        'InjuryValGraph is specified graph number for injury calculation

                        'HIC
                        Analopen.DataReading(Tmp_Path(k), OpenFile.names(k), Graph_Ext(Head_Graph_Index), ParaMeterArr1(Head_Graph_Index, k, 0), ParaMeterArr1(Head_Graph_Index, k, 1), ParaMeterArr2(Head_Graph_Index, k, 0), ParaMeterArr2(Head_Graph_Index, k, 1), TimeVals, DataVals, ConversionFactor(Head_Graph_Index))
                        time_interval = TimeVals(1) - TimeVals(0)
                        'Data Acquire 및 상해치 계산 (HIC15/36 및 Nij / Neck Tens. , Comps....)
                        TmpHIC1536 = HICfromDATA(TimeVals, DataVals)
                        HIC15(k) = TmpHIC1536(0)
                        HIC36(k) = TmpHIC1536(1)
                        Head_Peak_G(k) = DataVals.Max
                        'Head3MSG(k) = Acc3MSfromDATA(TimeVals, DataVals)

                        'Neck Tens,Comp
                        DataVals = Nothing
                        Analopen.DataReading(Tmp_Path(k), OpenFile.names(k), Graph_Ext(NeckFz_Graph_Index), ParaMeterArr1(NeckFz_Graph_Index, k, 0), ParaMeterArr1(NeckFz_Graph_Index, k, 1), ParaMeterArr2(NeckFz_Graph_Index, k, 0), ParaMeterArr2(NeckFz_Graph_Index, k, 1), TimeVals, DataVals, ConversionFactor(NeckFz_Graph_Index))
                        time_interval = TimeVals(1) - TimeVals(0)
                        Neck_Fz = DataVals
                        'Data Acquire 및 상해치 계산 (HIC15/36 및 Nij / Neck Tens. , Comps....)
                        Neck_Tens(k) = Math.Abs(DataVals.Max) / 1000
                        Neck_Comp(k) = Math.Abs(DataVals.Min) / 1000

                        'Neck Shear
                        DataVals = Nothing
                        Analopen.DataReading(Tmp_Path(k), OpenFile.names(k), Graph_Ext(NeckFx_Graph_Index), ParaMeterArr1(NeckFx_Graph_Index, k, 0), ParaMeterArr1(NeckFx_Graph_Index, k, 1), ParaMeterArr2(NeckFx_Graph_Index, k, 0), ParaMeterArr2(NeckFx_Graph_Index, k, 1), TimeVals, DataVals, ConversionFactor(NeckFx_Graph_Index))
                        time_interval = TimeVals(1) - TimeVals(0)
                        Neck_Fx = DataVals
                        'Data Acquire 및 상해치 계산 (HIC15/36 및 Nij / Neck Tens. , Comps....)
                        Neck_Shear(k) = Math.Abs(DataVals.Min) / 1000

                        'Neck Ext.
                        DataVals = Nothing
                        Analopen.DataReading(Tmp_Path(k), OpenFile.names(k), Graph_Ext(NeckMy_Graph_Index), ParaMeterArr1(NeckMy_Graph_Index, k, 0), ParaMeterArr1(NeckMy_Graph_Index, k, 1), ParaMeterArr2(NeckMy_Graph_Index, k, 0), ParaMeterArr2(NeckMy_Graph_Index, k, 1), TimeVals, DataVals, ConversionFactor(NeckMy_Graph_Index))
                        time_interval = TimeVals(1) - TimeVals(0)
                        Neck_My = DataVals
                        'Data Acquire 및 상해치 계산 (HIC15/36 및 Nij / Neck Tens. , Comps....)
                        Neck_Exten(k) = Math.Abs(DataVals.Min)

                        '여기서 목상해를 계산해준다.
                        Neck_Mocy = CalMocyDATA(Neck_Fx, Neck_My, DummyType(k))
                        Tmp_Array_NTE = CalNTEDATA(Neck_Fz, Neck_Mocy, DummyType(k))
                        Tmp_Array_NTF = CalNTFDATA(Neck_Fz, Neck_Mocy, DummyType(k))
                        Tmp_Array_NCE = CalNCEDATA(Neck_Fz, Neck_Mocy, DummyType(k))
                        Tmp_Array_NCF = CalNCFDATA(Neck_Fz, Neck_Mocy, DummyType(k))

                        NTE(k) = Tmp_Array_NTE.Max
                        NTF(k) = Tmp_Array_NTF.Max
                        NCE(k) = Tmp_Array_NCE.Max
                        NCF(k) = Tmp_Array_NCF.Max
                    Next
                    '**************************************************************************************
                End If

                'If GraphBased = False Then
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "H3MS_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        Head3MSG(i) = (CDbl(Mid(tmp_txt, 30, 15)) / 9.81) * InjuryCorrel(0)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                'End If

                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "T3MS_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        Chest_G_CUMULATIVE_T3MS_inj(i) = (CDbl(Mid(tmp_txt, 30, 15)) / 9.81) * InjuryCorrel(1)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                If GraphBased = False Then
                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "HIC15_inj", vbTextCompare) > 0 Then
                            tmp_txt = fileNum.ReadLine
                            tmp_txt = fileNum.ReadLine
                            HIC15(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(2)
                            Exit Do
                        End If
                    Loop
                    '=================================================================================
                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "HIC36_inj", vbTextCompare) > 0 Then
                            tmp_txt = fileNum.ReadLine
                            tmp_txt = fileNum.ReadLine
                            HIC36(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(3)
                            Exit Do
                        End If
                    Loop
                    '=================================================================================
                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "NTE_inj", vbTextCompare) > 0 Then
                            tmp_txt = fileNum.ReadLine
                            tmp_txt = fileNum.ReadLine
                            NTE(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(4)
                            Exit Do
                        End If
                    Loop
                    '=================================================================================
                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "NTF_inj", vbTextCompare) > 0 Then
                            tmp_txt = fileNum.ReadLine
                            tmp_txt = fileNum.ReadLine
                            NTF(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(5)
                            Exit Do
                        End If
                    Loop
                    '=================================================================================
                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "NCE_inj", vbTextCompare) > 0 Then
                            tmp_txt = fileNum.ReadLine
                            tmp_txt = fileNum.ReadLine
                            NCE(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(6)
                            Exit Do
                        End If
                    Loop
                    '=================================================================================
                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "NCF_inj", vbTextCompare) > 0 Then
                            tmp_txt = fileNum.ReadLine
                            tmp_txt = fileNum.ReadLine
                            NCF(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(7)
                            Exit Do
                        End If
                    Loop
                    '=================================================================================
                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "HaccRpeak_inj", vbTextCompare) > 0 Then
                            tmp_txt = fileNum.ReadLine
                            tmp_txt = fileNum.ReadLine
                            Head_Peak_G(i) = (CDbl(Mid(tmp_txt, 30, 15)) / 9.81) * InjuryCorrel(8)
                            Exit Do
                        End If
                    Loop
                End If
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "TaccRpeak_inj", vbTextCompare) > 0 Then
                '        tmp_txt = fileNum.ReadLine
                '        tmp_txt = fileNum.ReadLine
                '        Chest_G(i) = (CDbl(Mid(tmp_txt, 30, 15)) / 9.81)
                '        Exit do
                '    End If
                'Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "ThCC_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        Chest_D(i) = Math.Abs(CDbl(Mid(tmp_txt, 30, 15))) * 1000 * InjuryCorrel(9)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "kneesliderL_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        KneeS_L(i) = CDbl(Mid(tmp_txt, 30, 15)) * 1000 * InjuryCorrel(10)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "kneesliderR_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        KneeS_R(i) = CDbl(Mid(tmp_txt, 30, 15)) * 1000 * InjuryCorrel(11)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "TCFCLowL_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        Tibia_Comp_L(i) = (CDbl(Mid(tmp_txt, 30, 15)) / 1000) * InjuryCorrel(12)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "TCFCLowR_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        Tibia_Comp_R(i) = (CDbl(Mid(tmp_txt, 30, 15)) / 1000) * InjuryCorrel(13)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "TIUpL_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        TI_upr_L(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(14)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "TILowL_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        TI_lwr_L(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(15)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "TIUpR_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        TI_upr_R(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(16)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "TILowR_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        TI_lwr_R(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(17)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "VC_inj_CFC180", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        Chest_VC(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(18)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "FFCL_inj", vbTextCompare) > 0 Then
                        If InStr(1, tmp_txt, "negative", vbTextCompare) > 0 Then
                            tmp_txt = fileNum.ReadLine
                            tmp_txt = fileNum.ReadLine
                            FemurL(i) = Math.Abs((CDbl(Mid(tmp_txt, 60, 12)) / 1000)) * InjuryCorrel(19)
                            Exit Do
                        End If
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "FFCR_inj", vbTextCompare) > 0 Then
                        If InStr(1, tmp_txt, "negative", vbTextCompare) > 0 Then
                            tmp_txt = fileNum.ReadLine
                            tmp_txt = fileNum.ReadLine
                            FemurR(i) = Math.Abs((CDbl(Mid(tmp_txt, 60, 12)) / 1000)) * InjuryCorrel(20)
                            Exit Do
                        End If
                    End If
                Loop
                '=================================================================================
                If GraphBased = False Then
                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "FNICtension_inj", vbTextCompare) > 0 Then
                            If InStr(1, tmp_txt, "negative", vbTextCompare) > 0 Then
                                tmp_txt = fileNum.ReadLine
                                tmp_txt = fileNum.ReadLine
                                Neck_Comp(i) = (CDbl(Mid(tmp_txt, 60, 12)) / 1000) * InjuryCorrel(21)
                                Exit Do
                            End If
                        End If
                    Loop
                    '=================================================================================
                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "FNICtension_inj", vbTextCompare) > 0 Then
                            If InStr(1, tmp_txt, "positive", vbTextCompare) > 0 Then
                                tmp_txt = fileNum.ReadLine
                                tmp_txt = fileNum.ReadLine
                                Neck_Tens(i) = Math.Abs(CDbl(Mid$(tmp_txt, 60, 12)) / 1000) * InjuryCorrel(22)
                                Exit Do
                            End If
                        End If
                    Loop
                    '=================================================================================
                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "FNICshear_inj", vbTextCompare) > 0 Then
                            If InStr(1, tmp_txt, "negative", vbTextCompare) > 0 Then
                                tmp_txt = fileNum.ReadLine
                                tmp_txt = fileNum.ReadLine
                                Neck_Shear(i) = Math.Abs(CDbl(Mid$(tmp_txt, 60, 12)) / 1000) * InjuryCorrel(23)
                                Exit Do
                            End If
                        End If
                    Loop
                    '=================================================================================
                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "FNICbending_inj", vbTextCompare) > 0 Then
                            If InStr(1, tmp_txt, "negative", vbTextCompare) > 0 Then
                                tmp_txt = fileNum.ReadLine
                                tmp_txt = fileNum.ReadLine
                                Neck_Exten(i) = Math.Abs(CDbl(Mid$(tmp_txt, 60, 12))) * InjuryCorrel(24)
                                Exit Do
                            End If
                        End If
                    Loop
                End If

                fileNum.Close()
                ReadFiles.Close()

                'Probability Calculation
                'Head P
                P_Head(i) = CND(((Math.Log(Format(HIC15(i), "#.0")) - 7.45231) / 0.73998))

                If DummyType(i) = 0 Then
                    P_Neck_Tens(i) = 1 / (1 + Math.Exp(10.9745 - 2.375 * Format(Neck_Tens(i), "#.000")))
                    P_Neck_Comp(i) = 1 / (1 + Math.Exp(10.9745 - 2.375 * Format(Neck_Comp(i), "#.000")))
                    P_FemurL(i) = 1 / (1 + Math.Exp(5.795 - 0.5196 * Format(FemurL(i), "#.000")))
                    P_FemurR(i) = 1 / (1 + Math.Exp(5.795 - 0.5196 * Format(FemurR(i), "#.000")))
                    P_CD(i) = 1 / (1 + Math.Exp(10.5456 - 1.568 * (Format(Chest_D(i), "#.0") ^ 0.4612)))
                ElseIf DummyType(i) = 1 Then
                    P_Neck_Tens(i) = 1 / (1 + Math.Exp(10.958 - 3.77 * Format(Neck_Tens(i), "#.000")))
                    P_Neck_Comp(i) = 1 / (1 + Math.Exp(10.958 - 3.77 * Format(Neck_Comp(i), "#.000")))
                    P_FemurL(i) = 1 / (1 + Math.Exp(5.7949 - 0.7619 * Format(FemurL(i), "#.000")))
                    P_FemurR(i) = 1 / (1 + Math.Exp(5.7949 - 0.7619 * Format(FemurR(i), "#.000")))
                    P_CD(i) = 1 / (1 + Math.Exp(10.5456 - 1.7212 * (Format(Chest_D(i), "#.0") ^ 0.4612)))
                End If
                P_Neck_NTE(i) = 1 / (1 + Math.Exp(3.2269 - 1.9688 * Format(NTE(i), "#.00")))
                P_Neck_NTF(i) = 1 / (1 + Math.Exp(3.2269 - 1.9688 * Format(NTF(i), "#.00")))
                P_Neck_NCE(i) = 1 / (1 + Math.Exp(3.2269 - 1.9688 * Format(NCE(i), "#.00")))
                P_Neck_NCF(i) = 1 / (1 + Math.Exp(3.2269 - 1.9688 * Format(NCF(i), "#.00")))

                P_Neck_Max(i) = P_Neck_Max_Judg(P_Neck_Tens(i), P_Neck_Comp(i), P_Neck_NTE(i), P_Neck_NTF(i), P_Neck_NCE(i), P_Neck_NCF(i))
                If P_FemurL(i) >= P_FemurR(i) Then
                    P_Femur_Max(i) = P_FemurL(i)
                Else
                    P_Femur_Max(i) = P_FemurR(i)
                End If

            ElseIf DummyType(i) = 2 Then '============================================================================> For THOR 50% ATD

                If GraphBased = True Then
                    '그래프베이스 상해 관련*****************************************************************
                    '**************************************************************************************
                    '그래프 스케일도 바꾼다.
                    '**************************************************************************************
                    Dim TimeVals() As Decimal = Nothing
                    Dim DataVals() As Double = Nothing
                    Dim TmpHIC1536() As Double
                    Dim time_interval As Double
                    'Dim Neck_Fx() As Double
                    'Dim Neck_Fz() As Double
                    'Dim Neck_My() As Double
                    'Dim Neck_Mocy() As Double
                    'Dim Tmp_NTE() As Double
                    'Dim Tmp_NTF() As Double
                    'Dim Tmp_NCE() As Double
                    'Dim Tmp_NCF() As Double

                    '파일을 열어온다
                    Dim InjuryFile As StreamReader
                    InjuryFile = New StreamReader(Application.StartupPath & "\Profile\GraphSetting.Gcorrel")
                    Dim Tmp_Str() As String

                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim Head_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NeckFx_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NeckFz_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NeckMy_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NeckMocy_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NTE_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NTF_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NCE_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NCF_Graph_Index As Integer = CInt(Tmp_Str(1))

                    InjuryFile.Close()

                    For k = 0 To UBound(OpenFile.names)
                        'InjuryValGraph is specified graph number for injury calculation

                        'HIC
                        Analopen.DataReading(Tmp_Path(k), OpenFile.names(k), Graph_Ext(Head_Graph_Index), ParaMeterArr1(Head_Graph_Index, k, 0), ParaMeterArr1(Head_Graph_Index, k, 1), ParaMeterArr2(Head_Graph_Index, k, 0), ParaMeterArr2(Head_Graph_Index, k, 1), TimeVals, DataVals, ConversionFactor(Head_Graph_Index))
                        time_interval = TimeVals(1) - TimeVals(0)
                        'Data Acquire 및 상해치 계산 (HIC15/36 및 Nij / Neck Tens. , Comps....)
                        TmpHIC1536 = HICfromDATA(TimeVals, DataVals)
                        HIC15(k) = TmpHIC1536(0)
                        HIC36(k) = TmpHIC1536(1)
                        Array.Sort(DataVals)
                        Head_Peak_G(k) = DataVals(UBound(DataVals))

                        'Neck Tens,Comp
                        Analopen.DataReading(Tmp_Path(k), OpenFile.names(k), Graph_Ext(NeckFz_Graph_Index), ParaMeterArr1(NeckFz_Graph_Index, k, 0), ParaMeterArr1(NeckFz_Graph_Index, k, 1), ParaMeterArr2(NeckFz_Graph_Index, k, 0), ParaMeterArr2(NeckFz_Graph_Index, k, 1), TimeVals, DataVals, ConversionFactor(NeckFz_Graph_Index))
                        time_interval = TimeVals(1) - TimeVals(0)
                        'Data Acquire 및 상해치 계산 (HIC15/36 및 Nij / Neck Tens. , Comps....)
                        Array.Sort(DataVals)
                        Neck_Tens(k) = Math.Abs(DataVals(UBound(DataVals))) / 1000
                        Neck_Comp(k) = Math.Abs(DataVals(0)) / 1000

                        'Neck Shear
                        Analopen.DataReading(Tmp_Path(k), OpenFile.names(k), Graph_Ext(NeckFx_Graph_Index), ParaMeterArr1(NeckFx_Graph_Index, k, 0), ParaMeterArr1(NeckFx_Graph_Index, k, 1), ParaMeterArr2(NeckFx_Graph_Index, k, 0), ParaMeterArr2(NeckFx_Graph_Index, k, 1), TimeVals, DataVals, ConversionFactor(NeckFx_Graph_Index))
                        time_interval = TimeVals(1) - TimeVals(0)
                        'Data Acquire 및 상해치 계산 (HIC15/36 및 Nij / Neck Tens. , Comps....)
                        Array.Sort(DataVals)
                        Neck_Shear(k) = Math.Abs(DataVals(0)) / 1000

                        'Neck Ext.
                        Analopen.DataReading(Tmp_Path(k), OpenFile.names(k), Graph_Ext(NeckMy_Graph_Index), ParaMeterArr1(NeckMy_Graph_Index, k, 0), ParaMeterArr1(NeckMy_Graph_Index, k, 1), ParaMeterArr2(NeckMy_Graph_Index, k, 0), ParaMeterArr2(NeckMy_Graph_Index, k, 1), TimeVals, DataVals, ConversionFactor(NeckMy_Graph_Index))
                        time_interval = TimeVals(1) - TimeVals(0)
                        'Data Acquire 및 상해치 계산 (HIC15/36 및 Nij / Neck Tens. , Comps....)
                        Array.Sort(DataVals)
                        Neck_Exten(k) = Math.Abs(DataVals(0))


                        '여기서 목상해를 계산해준다.
                        'THOR 의 Nij 계산법은 다르므로 적용하지 않는다.
                        'Neck_Mocy = CalMocyDATA(Neck_Fx, Neck_My, DummyType(k))
                        'Tmp_NTE = CalNTEDATA(Neck_Fz, Neck_Mocy, DummyType(k))
                        'Tmp_NTF = CalNTFDATA(Neck_Fz, Neck_Mocy, DummyType(k))
                        'Tmp_NCE = CalNCEDATA(Neck_Fz, Neck_Mocy, DummyType(k))
                        'Tmp_NCF = CalNCFDATA(Neck_Fz, Neck_Mocy, DummyType(k))

                        'Array.Sort(Tmp_NTE)
                        'Array.Sort(Tmp_NTF)
                        'Array.Sort(Tmp_NCE)
                        'Array.Sort(Tmp_NCF)

                        'NTE(k) = Tmp_NTE(UBound(Tmp_NTE))
                        'NTF(k) = Tmp_NTF(UBound(Tmp_NTF))
                        'NCE(k) = Tmp_NCE(UBound(Tmp_NCE))
                        'NCF(k) = Tmp_NCF(UBound(Tmp_NCF))
                    Next
                    '**************************************************************************************
                End If

                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "T3MS_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        Chest_G_CUMULATIVE_T3MS_inj(i) = (CDbl(Mid(tmp_txt, 30, 15)) / 9.81) * InjuryCorrel(1)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                If GraphBased = False Then
                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "HIC15_inj", vbTextCompare) > 0 Then
                            tmp_txt = fileNum.ReadLine
                            tmp_txt = fileNum.ReadLine
                            HIC15(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(2)
                            Exit Do
                        End If
                    Loop
                    '=================================================================================
                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "HIC36_inj", vbTextCompare) > 0 Then
                            tmp_txt = fileNum.ReadLine
                            tmp_txt = fileNum.ReadLine
                            HIC36(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(3)
                            Exit Do
                        End If
                    Loop
                End If
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "NTE_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        NTE(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(4)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "NTF_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        NTF(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(5)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "NCE_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        NCE(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(6)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "NCF_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        NCF(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(7)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "KneeSliderL_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        KneeS_L(i) = CDbl(Mid(tmp_txt, 30, 15)) * 1000 * InjuryCorrel(10)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "KneeSliderR_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        KneeS_R(i) = CDbl(Mid(tmp_txt, 30, 15)) * 1000 * InjuryCorrel(11)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "FemurLcompZpeak_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        FemurL(i) = Math.Abs((CDbl(Mid(tmp_txt, 30, 15)) / 1000)) * InjuryCorrel(19)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "FemurRcompZpeak_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        FemurR(i) = Math.Abs((CDbl(Mid(tmp_txt, 30, 15)) / 1000)) * InjuryCorrel(20)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "TCFCLowL_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        Tibia_Comp_L(i) = (CDbl(Mid(tmp_txt, 30, 15)) / 1000) * InjuryCorrel(12)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "TCFCLowR_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        Tibia_Comp_R(i) = (CDbl(Mid(tmp_txt, 30, 15)) / 1000) * InjuryCorrel(13)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "TIUpL_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        TI_upr_L(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(14)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "TILowL_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        TI_lwr_L(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(15)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "TIUpR_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        TI_upr_R(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(16)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "TILowR_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        TI_lwr_R(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(17)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "ThxIrTraccRibL_CFC600_dis", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        ThxIrUpL(i) = (CDbl(Mid(tmp_txt, 40, 12)) * -1000) * InjuryCorrel(25)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "ThxIrTraccRibR_CFC600_dis", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        ThxIrUpR(i) = (CDbl(Mid(tmp_txt, 40, 12)) * -1000) * InjuryCorrel(26)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "ThxLowIrTraccRibL_CFC600_dis", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        ThxIrLowL(i) = (CDbl(Mid(tmp_txt, 40, 12)) * -1000) * InjuryCorrel(27)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "ThxLowIrTraccRibR_CFC600_dis", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        ThxIrLowR(i) = (CDbl(Mid(tmp_txt, 40, 12)) * -1000) * InjuryCorrel(28)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                If GraphBased = False Then

                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "FNICtension_inj", vbTextCompare) > 0 Then
                            If InStr(1, tmp_txt, "negative", vbTextCompare) > 0 Then
                                tmp_txt = fileNum.ReadLine
                                tmp_txt = fileNum.ReadLine
                                Neck_Comp(i) = (CDbl(Mid(tmp_txt, 60, 12)) / 1000) * InjuryCorrel(21)
                                Exit Do
                            End If
                        End If
                    Loop
                    '=================================================================================
                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "FNICtension_inj", vbTextCompare) > 0 Then
                            If InStr(1, tmp_txt, "positive", vbTextCompare) > 0 Then
                                tmp_txt = fileNum.ReadLine
                                tmp_txt = fileNum.ReadLine
                                Neck_Tens(i) = Math.Abs(CDbl(Mid$(tmp_txt, 60, 12)) / 1000) * InjuryCorrel(22)
                                Exit Do
                            End If
                        End If
                    Loop
                    '=================================================================================
                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "FNICshear_inj", vbTextCompare) > 0 Then
                            If InStr(1, tmp_txt, "negative", vbTextCompare) > 0 Then
                                tmp_txt = fileNum.ReadLine
                                tmp_txt = fileNum.ReadLine
                                Neck_Shear(i) = Math.Abs(CDbl(Mid$(tmp_txt, 60, 12)) / 1000) * InjuryCorrel(23)
                                Exit Do
                            End If
                        End If
                    Loop
                    '=================================================================================
                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "FNICbending_inj", vbTextCompare) > 0 Then
                            If InStr(1, tmp_txt, "negative", vbTextCompare) > 0 Then
                                tmp_txt = fileNum.ReadLine
                                tmp_txt = fileNum.ReadLine
                                Neck_Exten(i) = Math.Abs(CDbl(Mid$(tmp_txt, 60, 12))) * InjuryCorrel(24)
                                Exit Do
                            End If
                        End If
                    Loop
                End If

                fileNum.Close()
                ReadFiles.Close()

                P_Head(i) = CND(((Math.Log(Format(HIC15(i), "#.0")) - 7.45231) / 0.73998))
                Chest_D(i) = (ThxIrUpL(i) + ThxIrUpR(i) + ThxIrLowL(i) + ThxIrLowR(i)) / 4

                P_Neck_Tens(i) = 1 / (1 + Math.Exp(10.9745 - 2.375 * Format(Neck_Tens(i), "#.000")))
                P_Neck_Comp(i) = 1 / (1 + Math.Exp(10.9745 - 2.375 * Format(Neck_Comp(i), "#.000")))
                P_FemurL(i) = 1 / (1 + Math.Exp(5.795 - 0.5196 * Format(FemurL(i), "#.000")))
                P_FemurR(i) = 1 / (1 + Math.Exp(5.795 - 0.5196 * Format(FemurR(i), "#.000")))
                P_CD(i) = 1 / (1 + Math.Exp(10.5456 - 1.568 * (Format(Chest_D(i), "#.0") ^ 0.4612)))
                P_Neck_NTE(i) = 1 / (1 + Math.Exp(3.2269 - 1.9688 * Format(NTE(i), "#.00")))
                P_Neck_NTF(i) = 1 / (1 + Math.Exp(3.2269 - 1.9688 * Format(NTF(i), "#.00")))
                P_Neck_NCE(i) = 1 / (1 + Math.Exp(3.2269 - 1.9688 * Format(NCE(i), "#.00")))
                P_Neck_NCF(i) = 1 / (1 + Math.Exp(3.2269 - 1.9688 * Format(NCF(i), "#.00")))

                P_Neck_Max(i) = P_Neck_Max_Judg(P_Neck_Tens(i), P_Neck_Comp(i), P_Neck_NTE(i), P_Neck_NTF(i), P_Neck_NCE(i), P_Neck_NCF(i))
                If P_FemurL(i) >= P_FemurR(i) Then
                    P_Femur_Max(i) = P_FemurL(i)
                Else
                    P_Femur_Max(i) = P_FemurR(i)
                End If

            ElseIf DummyType(i) = 3 Or DummyType(i) = 4 Then

                'Q 더미 상해 만드는 중 ***********************************************************************************************Q 더미 상해

                If GraphBased = True Then
                    '그래프베이스 상해 관련*****************************************************************
                    '**************************************************************************************
                    '그래프 스케일도 바꾼다.
                    '**************************************************************************************
                    Dim TimeVals() As Decimal = Nothing
                    Dim DataVals() As Double = Nothing
                    Dim TmpHIC1536() As Double
                    Dim time_interval As Double
                    Dim Neck_Fx() As Double = Nothing
                    Dim Neck_Fz() As Double = Nothing
                    Dim Neck_My() As Double = Nothing
                    'Dim Neck_Mocy() As Double
                    'Dim Tmp_Array_NTE() As Double
                    'Dim Tmp_Array_NTF() As Double
                    'Dim Tmp_Array_NCE() As Double
                    'Dim Tmp_Array_NCF() As Double

                    '파일을 열어온다
                    Dim InjuryFile As StreamReader
                    InjuryFile = New StreamReader(Application.StartupPath & "\Profile\GraphSetting.Gcorrel")
                    Dim Tmp_Str() As String

                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim Head_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NeckFx_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NeckFz_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NeckMy_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NeckMocy_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NTE_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NTF_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NCE_Graph_Index As Integer = CInt(Tmp_Str(1))
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    Dim NCF_Graph_Index As Integer = CInt(Tmp_Str(1))

                    InjuryFile.Close()

                    For k = 0 To UBound(OpenFile.names)
                        'InjuryValGraph is specified graph number for injury calculation

                        'HIC
                        Analopen.DataReading(Tmp_Path(k), OpenFile.names(k), Graph_Ext(Head_Graph_Index), ParaMeterArr1(Head_Graph_Index, k, 0), ParaMeterArr1(Head_Graph_Index, k, 1), ParaMeterArr2(Head_Graph_Index, k, 0), ParaMeterArr2(Head_Graph_Index, k, 1), TimeVals, DataVals, ConversionFactor(Head_Graph_Index))
                        time_interval = TimeVals(1) - TimeVals(0)
                        'Data Acquire 및 상해치 계산 (HIC15/36 및 Nij / Neck Tens. , Comps....)
                        TmpHIC1536 = HICfromDATA(TimeVals, DataVals)
                        HIC15(k) = TmpHIC1536(0)
                        HIC36(k) = TmpHIC1536(1)
                        Head_Peak_G(k) = DataVals.Max
                        'Head3MSG(k) = Acc3MSfromDATA(TimeVals, DataVals)

                        'Neck Tens,Comp
                        DataVals = Nothing
                        Analopen.DataReading(Tmp_Path(k), OpenFile.names(k), Graph_Ext(NeckFz_Graph_Index), ParaMeterArr1(NeckFz_Graph_Index, k, 0), ParaMeterArr1(NeckFz_Graph_Index, k, 1), ParaMeterArr2(NeckFz_Graph_Index, k, 0), ParaMeterArr2(NeckFz_Graph_Index, k, 1), TimeVals, DataVals, ConversionFactor(NeckFz_Graph_Index))
                        time_interval = TimeVals(1) - TimeVals(0)
                        Neck_Fz = DataVals
                        'Data Acquire 및 상해치 계산 (HIC15/36 및 Nij / Neck Tens. , Comps....)
                        Neck_Tens(k) = Math.Abs(DataVals.Max) / 1000
                        Neck_Comp(k) = Math.Abs(DataVals.Min) / 1000

                        'Neck Shear
                        DataVals = Nothing
                        Analopen.DataReading(Tmp_Path(k), OpenFile.names(k), Graph_Ext(NeckFx_Graph_Index), ParaMeterArr1(NeckFx_Graph_Index, k, 0), ParaMeterArr1(NeckFx_Graph_Index, k, 1), ParaMeterArr2(NeckFx_Graph_Index, k, 0), ParaMeterArr2(NeckFx_Graph_Index, k, 1), TimeVals, DataVals, ConversionFactor(NeckFx_Graph_Index))
                        time_interval = TimeVals(1) - TimeVals(0)
                        Neck_Fx = DataVals
                        'Data Acquire 및 상해치 계산 (HIC15/36 및 Nij / Neck Tens. , Comps....)
                        Neck_Shear(k) = Math.Abs(DataVals.Min) / 1000

                        'Neck Ext.
                        DataVals = Nothing
                        Analopen.DataReading(Tmp_Path(k), OpenFile.names(k), Graph_Ext(NeckMy_Graph_Index), ParaMeterArr1(NeckMy_Graph_Index, k, 0), ParaMeterArr1(NeckMy_Graph_Index, k, 1), ParaMeterArr2(NeckMy_Graph_Index, k, 0), ParaMeterArr2(NeckMy_Graph_Index, k, 1), TimeVals, DataVals, ConversionFactor(NeckMy_Graph_Index))
                        time_interval = TimeVals(1) - TimeVals(0)
                        Neck_My = DataVals
                        'Data Acquire 및 상해치 계산 (HIC15/36 및 Nij / Neck Tens. , Comps....)
                        Neck_Exten(k) = Math.Abs(DataVals.Min)
                        Neck_Flex(k) = Math.Abs(DataVals.Max)

                        '여기서 목상해(Nij)를 계산해준다.
                        'Q 더미는 목상해가 없음
                    Next
                    '**************************************************************************************
                End If

                'If GraphBased = False Then
                'Head 3MS G================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "Head_CFC1000_AccR_3MS_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        Head3MSG(i) = (CDbl(Mid(tmp_txt, 30, 15)) / 9.81) * InjuryCorrel(0)
                        Exit Do
                    End If
                Loop
                '=========================================================================================
                'End If

                'Thorax 3MS G==============================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "Thorax_CFC180_AccR_3MS_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        Chest_G_CUMULATIVE_T3MS_inj(i) = (CDbl(Mid(tmp_txt, 30, 15)) / 9.81) * InjuryCorrel(1)
                        Exit Do
                    End If
                Loop
                '=========================================================================================

                If GraphBased = False Then
                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "HIC15_inj", vbTextCompare) > 0 Then
                            tmp_txt = fileNum.ReadLine
                            tmp_txt = fileNum.ReadLine
                            HIC15(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(2)
                            Exit Do
                        End If
                    Loop
                    '=================================================================================
                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "HIC36_inj", vbTextCompare) > 0 Then
                            tmp_txt = fileNum.ReadLine
                            tmp_txt = fileNum.ReadLine
                            HIC36(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(3)
                            Exit Do
                        End If
                    Loop
                    '=================================================================================
                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "Head_CFC1000_AccR_peak_inj", vbTextCompare) > 0 Then
                            tmp_txt = fileNum.ReadLine
                            tmp_txt = fileNum.ReadLine
                            Head_Peak_G(i) = (CDbl(Mid(tmp_txt, 30, 15)) / 9.81) * InjuryCorrel(8)
                            Exit Do
                        End If
                    Loop
                    '=================================================================================
                End If

                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "ChestDefFront_CFC180_peak_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        Chest_D(i) = (CDbl(Mid(tmp_txt, 30, 15))) * 1000 * InjuryCorrel(9)
                        Exit Do
                    End If
                Loop

                If GraphBased = False Then
                    '=================================================================================
                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "NeckUpLC_CFC600_lce_F", vbTextCompare) > 0 Then
                            If InStr(1, tmp_txt, "On child", vbTextCompare) > 0 Then
                                tmp_txt = fileNum.ReadLine
                                tmp_txt = fileNum.ReadLine
                                tmp_txt = fileNum.ReadLine
                                tmp_txt = fileNum.ReadLine
                                tmp_txt = fileNum.ReadLine
                                tmp_txt = fileNum.ReadLine
                                tmp_txt = fileNum.ReadLine
                                tmp_txt = fileNum.ReadLine
                                Neck_Comp(i) = Math.Abs((CDbl(Mid(tmp_txt, 40, 12)) / 1000)) * InjuryCorrel(21)
                                Neck_Tens(i) = (CDbl(Mid(tmp_txt, 60, 12)) / 1000) * InjuryCorrel(22)
                                Exit Do
                            End If
                        End If
                    Loop
                    '=================================================================================
                    Do While Not fileNum.EndOfStream
                        tmp_txt = fileNum.ReadLine
                        If InStr(1, tmp_txt, "NeckUpLC_CFC600_lce_T", vbTextCompare) > 0 Then
                            If InStr(1, tmp_txt, "On child", vbTextCompare) > 0 Then
                                tmp_txt = fileNum.ReadLine
                                tmp_txt = fileNum.ReadLine
                                tmp_txt = fileNum.ReadLine
                                tmp_txt = fileNum.ReadLine
                                tmp_txt = fileNum.ReadLine
                                tmp_txt = fileNum.ReadLine
                                tmp_txt = fileNum.ReadLine
                                Neck_Exten(i) = Math.Abs(CDbl(Mid(tmp_txt, 40, 12))) * InjuryCorrel(24)
                                Neck_Flex(i) = Math.Abs(CDbl(Mid(tmp_txt, 60, 12))) '--------------------------------- 없다없어;;;
                                Exit Do
                            End If
                        End If
                    Loop
                    '=================================================================================
                End If


                'Q 더미 상해 만드는 중 **************************************************************************************************************
            End If

        Next

    End Sub

#Region "Injury Values Display"

    'US-NCAP Header
    Private Sub Sheet_Lbl()
        Dim i As Integer

        With Me.InjuryLbl1
            .Rows = 5 '+ file_tot * 3
            .Cols = 15
            .FixedRows = 0
            .FixedCols = 0

            '.MergeCells = flexMergeRestrictAll
            '.SelectionMode = MSFlexGridLib.SelectionModeSettings.flexSelectionFree
            .MergeCells = 1       '셀병합 허용

            For i = 0 To 14
                .set_MergeCol(i, True)
                .set_ColAlignment(i, 4) '가운데 정렬
                .set_ColWidth(i, 1008 + 10)
            Next

            .set_RowHeight(0, 250)
            .set_RowHeight(1, 250)
            .set_RowHeight(2, 250)
            .set_RowHeight(3, 250)
            .set_RowHeight(4, 250)

            .AllowBigSelection = True

            .set_MergeRow(0, True)
            .set_MergeRow(1, True)
            .set_MergeRow(2, True)
            .set_MergeRow(3, True)
            .set_MergeRow(4, True)

            .set_TextMatrix(0, 0, "US-NCAP")
            .set_TextMatrix(1, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(2, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(3, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(4, 0, .get_TextMatrix(0, 0))

            .set_TextMatrix(0, 1, "구분")
            .set_TextMatrix(1, 1, "법규")
            .set_TextMatrix(2, 1, "개발목표")
            .set_TextMatrix(3, 1, "상해")
            .set_TextMatrix(4, 1, .get_TextMatrix(3, 1))

            .set_TextMatrix(0, 2, "Head")
            .set_TextMatrix(1, 2, "700")
            .set_TextMatrix(2, 2, "560")
            .set_TextMatrix(3, 2, "HIC15")
            .set_TextMatrix(4, 2, .get_TextMatrix(3, 2))

            .set_TextMatrix(0, 3, "Neck")
            .set_TextMatrix(1, 3, "4.17(2.62)")
            .set_TextMatrix(2, 3, "3.34(2.10)")
            .set_TextMatrix(3, 3, "Tens." & vbCrLf & "[kN]")
            .set_TextMatrix(4, 3, .get_TextMatrix(3, 3))

            .set_TextMatrix(0, 4, "Neck")
            .set_TextMatrix(1, 4, "4.0(2.52)")
            .set_TextMatrix(2, 4, "3.2(2.02)")
            .set_TextMatrix(3, 4, "Comp." & vbCrLf & "[kN]")
            .set_TextMatrix(4, 4, .get_TextMatrix(3, 4))

            .set_TextMatrix(0, 5, "Neck")
            .set_TextMatrix(1, 5, "1.0")
            .set_TextMatrix(2, 5, "0.8")
            .set_TextMatrix(3, 5, "NTE")
            .set_TextMatrix(4, 5, .get_TextMatrix(3, 5))
            .set_TextMatrix(0, 6, .get_TextMatrix(0, 5))
            .set_TextMatrix(1, 6, "1.0")
            .set_TextMatrix(2, 6, "0.8")
            .set_TextMatrix(3, 6, "NTF")
            .set_TextMatrix(4, 6, .get_TextMatrix(3, 6))
            .set_TextMatrix(0, 7, .get_TextMatrix(0, 5))
            .set_TextMatrix(1, 7, "1.0")
            .set_TextMatrix(2, 7, "0.8")
            .set_TextMatrix(3, 7, "NCE")
            .set_TextMatrix(4, 7, .get_TextMatrix(3, 7))
            .set_TextMatrix(0, 8, .get_TextMatrix(0, 5))
            .set_TextMatrix(1, 8, "1.0")
            .set_TextMatrix(2, 8, "0.8")
            .set_TextMatrix(3, 8, "NCF")
            .set_TextMatrix(4, 8, .get_TextMatrix(3, 8))

            .set_TextMatrix(0, 9, "Chest")
            .set_TextMatrix(1, 9, "63(52)")
            .set_TextMatrix(2, 9, "50.4(41.6)")
            .set_TextMatrix(3, 9, "Disp." & vbCrLf & "[mm]")
            .set_TextMatrix(4, 9, .get_TextMatrix(3, 9))

            .set_TextMatrix(0, 10, .get_TextMatrix(0, 9))
            .set_TextMatrix(1, 10, "60.0")
            .set_TextMatrix(2, 10, "48.0")
            .set_TextMatrix(3, 10, "3ms[G]") '& vbCrLf & "[mm]")
            .set_TextMatrix(4, 10, .get_TextMatrix(3, 10))

            .set_TextMatrix(0, 11, "Femur")
            .set_TextMatrix(1, 11, "10.0(6.81)")
            .set_TextMatrix(2, 11, "8.0(5.44)")
            .set_TextMatrix(3, 11, "Peak [kN]")
            .set_TextMatrix(4, 11, "LH")
            .set_TextMatrix(0, 12, "Femur")
            .set_TextMatrix(1, 12, "10.0(6.81)")
            .set_TextMatrix(2, 12, "8.0(5.44)")
            .set_TextMatrix(3, 12, "Peak [kN]")
            .set_TextMatrix(4, 12, "RH")

            .set_TextMatrix(0, 13, "P-Aver." & vbCrLf & "RRS")
            .set_TextMatrix(1, 13, .get_TextMatrix(0, 13))
            .set_TextMatrix(2, 13, .get_TextMatrix(0, 13))
            .set_TextMatrix(3, 13, .get_TextMatrix(0, 13))
            .set_TextMatrix(4, 13, .get_TextMatrix(0, 13))

            .set_TextMatrix(0, 14, "Rating" & vbCrLf & "[★]")
            .set_TextMatrix(1, 14, .get_TextMatrix(0, 14))
            .set_TextMatrix(2, 14, .get_TextMatrix(0, 14))
            .set_TextMatrix(3, 14, .get_TextMatrix(0, 14))
            .set_TextMatrix(4, 14, .get_TextMatrix(0, 14))
        End With
    End Sub

    'K-NCAP Frontal Header
    Private Sub Sheet_Lbl2()
        '내수
        Dim i As Integer

        With Me.InjuryLbl2
            .WordWrap = True
            .ScrollBars = MSFlexGridLib.ScrollBarsSettings.flexScrollBarNone

            .Rows = 5 '+ file_tot * 3
            .Cols = 15 '21
            .FixedRows = 0
            .FixedCols = 0

            .FocusRect = MSFlexGridLib.FocusRectSettings.flexFocusNone
            .MergeCells = 1       '셀병합 허용

            .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillRepeat
            .Row = 1
            .Col = 2
            .RowSel = 2
            .ColSel = 13 '19
            .CellFontBold = False
            .CellFontSize = 8
            .CellForeColor = Color.Gray

            For i = 1 To 13
                .set_MergeCol(i, True)
                .set_ColAlignment(i, 4) '가운데 정렬
                .set_ColWidth(i, 1010)
            Next
            .set_ColWidth(1, 1050)
            .set_ColWidth(14, 1100)
            .set_MergeCol(0, True)
            .set_ColAlignment(0, 4) '가운데 정렬
            .set_MergeCol(14, True)
            .set_ColAlignment(14, 4) '가운데 정렬

            .set_RowHeight(0, 250)
            .set_RowHeight(1, 250)
            .set_RowHeight(2, 250)
            .set_RowHeight(3, 250)
            .set_RowHeight(4, 250)

            .AllowBigSelection = True

            .set_MergeRow(0, True)
            .set_MergeRow(1, True)
            .set_MergeRow(2, True)
            .set_MergeRow(3, True)
            .set_MergeRow(4, True)

            .set_TextMatrix(0, 0, "내수" & vbCrLf & "정면")
            .set_TextMatrix(1, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(2, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(3, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(4, 0, .get_TextMatrix(0, 0))

            .set_TextMatrix(0, 1, "구분")
            .set_TextMatrix(1, 1, "상한")
            .set_TextMatrix(2, 1, "하한")
            .set_TextMatrix(3, 1, "상해")
            .set_TextMatrix(4, 1, .get_TextMatrix(3, 1))

            .set_TextMatrix(0, 2, "Head & Neck")
            .set_TextMatrix(1, 2, "80")
            .set_TextMatrix(2, 2, "80")
            .set_TextMatrix(3, 2, "Peak" & vbCrLf & "G")
            .set_TextMatrix(4, 2, .get_TextMatrix(3, 2))

            .set_TextMatrix(0, 3, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 3, "88")
            .set_TextMatrix(2, 3, "72")
            .set_TextMatrix(3, 3, "3ms" & vbCrLf & "G")
            .set_TextMatrix(4, 3, .get_TextMatrix(3, 3))

            .set_TextMatrix(0, 4, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 4, "1000(700)")
            .set_TextMatrix(2, 4, "650(500)")
            .set_TextMatrix(3, 4, "HIC36" & vbCrLf & "[HIC15]")
            .set_TextMatrix(4, 4, .get_TextMatrix(3, 4))

            .set_TextMatrix(0, 5, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 5, "3.1 (1.95)")
            .set_TextMatrix(2, 5, "1.9 (1.2)")
            .set_TextMatrix(3, 5, "Shear" & vbCrLf & "[kN]")
            .set_TextMatrix(4, 5, .get_TextMatrix(3, 5))
            .set_TextMatrix(0, 6, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 6, "3.3 (2.62)")
            .set_TextMatrix(2, 6, "2.7 (1.7)")
            .set_TextMatrix(3, 6, "Tens." & vbCrLf & "[kN]")
            .set_TextMatrix(4, 6, .get_TextMatrix(3, 6))
            .set_TextMatrix(0, 7, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 7, "57 (49)")
            .set_TextMatrix(2, 7, "42 (36)")
            .set_TextMatrix(3, 7, "Exten." & vbCrLf & "[Nm]")
            .set_TextMatrix(4, 7, .get_TextMatrix(3, 7))

            .set_TextMatrix(0, 8, "Chest")
            .set_TextMatrix(1, 8, "50 (48)")
            .set_TextMatrix(2, 8, "22")
            .set_TextMatrix(3, 8, "Disp" & vbCrLf & "[mm]")
            .set_TextMatrix(4, 8, .get_TextMatrix(3, 8))
            .set_TextMatrix(0, 9, .get_TextMatrix(0, 8))
            .set_TextMatrix(1, 9, "1.0")
            .set_TextMatrix(2, 9, "0.5")
            .set_TextMatrix(3, 9, "VC" & vbCrLf & "[m/s]")
            .set_TextMatrix(4, 9, .get_TextMatrix(3, 9))

            .set_TextMatrix(0, 10, "Femur/Knee")
            .set_TextMatrix(1, 10, "15")
            .set_TextMatrix(2, 10, "6")
            .set_TextMatrix(3, 10, "K.Slide [mm]")
            .set_TextMatrix(4, 10, "LH")
            .set_TextMatrix(0, 11, .get_TextMatrix(0, 10))
            .set_TextMatrix(1, 11, .get_TextMatrix(1, 10))
            .set_TextMatrix(2, 11, .get_TextMatrix(2, 10))
            .set_TextMatrix(3, 11, .get_TextMatrix(3, 10))
            .set_TextMatrix(4, 11, "RH")

            .set_TextMatrix(0, 12, .get_TextMatrix(0, 10))
            .set_TextMatrix(1, 12, "9.07 (6.8)")
            .set_TextMatrix(2, 12, "3.8")
            .set_TextMatrix(3, 12, "K.Peak [kN]")
            .set_TextMatrix(4, 12, "LH")
            .set_TextMatrix(0, 13, .get_TextMatrix(0, 10))
            .set_TextMatrix(1, 13, .get_TextMatrix(1, 12))
            .set_TextMatrix(2, 13, .get_TextMatrix(2, 12))
            .set_TextMatrix(3, 13, .get_TextMatrix(3, 12))
            .set_TextMatrix(4, 13, "RH")

            '.set_TextMatrix(0, 14, "Tibia")
            '.set_TextMatrix(1, 14, "1.3")
            '.set_TextMatrix(2, 14, "0.4")
            '.set_TextMatrix(3, 14, "TI - Upr")
            '.set_TextMatrix(4, 14, "LH")
            '.set_TextMatrix(0, 15, .get_TextMatrix(0, 14))
            '.set_TextMatrix(1, 15, .get_TextMatrix(1, 14))
            '.set_TextMatrix(2, 15, .get_TextMatrix(2, 14))
            '.set_TextMatrix(3, 15, .get_TextMatrix(3, 14))
            '.set_TextMatrix(4, 15, "RH")
            '.set_TextMatrix(0, 16, .get_TextMatrix(0, 14))
            '.set_TextMatrix(1, 16, .get_TextMatrix(1, 14))
            '.set_TextMatrix(2, 16, .get_TextMatrix(2, 14))
            '.set_TextMatrix(3, 16, "TI - Lwr")
            '.set_TextMatrix(4, 16, "LH")
            '.set_TextMatrix(0, 17, .get_TextMatrix(0, 14))
            '.set_TextMatrix(1, 17, .get_TextMatrix(1, 14))
            '.set_TextMatrix(2, 17, .get_TextMatrix(2, 14))
            '.set_TextMatrix(3, 17, .get_TextMatrix(3, 16))
            '.set_TextMatrix(4, 17, "RH")
            '.set_TextMatrix(0, 18, .get_TextMatrix(0, 14))
            '.set_TextMatrix(1, 18, "8")
            '.set_TextMatrix(2, 18, "2")
            '.set_TextMatrix(3, 18, "Comp.[kN]")
            '.set_TextMatrix(4, 18, "LH")
            '.set_TextMatrix(0, 19, .get_TextMatrix(0, 14))
            '.set_TextMatrix(1, 19, .get_TextMatrix(1, 18))
            '.set_TextMatrix(2, 19, .get_TextMatrix(2, 18))
            '.set_TextMatrix(3, 19, .get_TextMatrix(3, 18))
            '.set_TextMatrix(4, 19, "RH")

            .set_TextMatrix(0, 14, "점수")
            .set_TextMatrix(1, 14, .get_TextMatrix(0, 14))
            .set_TextMatrix(2, 14, .get_TextMatrix(0, 14))
            .set_TextMatrix(3, 14, .get_TextMatrix(0, 14))
            .set_TextMatrix(4, 14, .get_TextMatrix(0, 14))

        End With
    End Sub

    'K-NCAP Offset Header
    Private Sub Sheet_Lbl3()
        '내수
        Dim i As Integer

        With Me.InjuryLbl3
            .WordWrap = True
            .ScrollBars = MSFlexGridLib.ScrollBarsSettings.flexScrollBarNone

            .Rows = 5 '+ file_tot * 3
            .Cols = 21
            .FixedRows = 0
            .FixedCols = 0

            .FocusRect = MSFlexGridLib.FocusRectSettings.flexFocusNone
            .MergeCells = 1       '셀병합 허용

            .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillRepeat
            .Row = 1
            .Col = 2
            .RowSel = 2
            .ColSel = 19
            .CellFontBold = False
            .CellFontSize = 8
            .CellForeColor = Color.Gray

            For i = 0 To 20
                .set_MergeCol(i, True)
                .set_ColAlignment(i, 4) '가운데 정렬
                .set_ColWidth(i, 730)
            Next
            For i = 4 To 7
                .set_ColWidth(i, 810)
            Next
            For i = 14 To 19
                .set_ColWidth(i, 570)
            Next

            .set_ColWidth(0, 730 + 200)
            .set_ColWidth(20, 1100)

            .set_RowHeight(0, 250)
            .set_RowHeight(1, 250)
            .set_RowHeight(2, 250)
            .set_RowHeight(3, 250)
            .set_RowHeight(4, 250)

            .AllowBigSelection = True

            .set_MergeRow(0, True)
            .set_MergeRow(1, True)
            .set_MergeRow(2, True)
            .set_MergeRow(3, True)
            .set_MergeRow(4, True)

            .set_TextMatrix(0, 0, "내수" & vbCrLf & "옵셋")
            .set_TextMatrix(1, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(2, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(3, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(4, 0, .get_TextMatrix(0, 0))

            .set_TextMatrix(0, 1, "구분")
            .set_TextMatrix(1, 1, "상한")
            .set_TextMatrix(2, 1, "하한")
            .set_TextMatrix(3, 1, "상해")
            .set_TextMatrix(4, 1, .get_TextMatrix(3, 1))

            .set_TextMatrix(0, 2, "Head & Neck")
            .set_TextMatrix(1, 2, "80")
            .set_TextMatrix(2, 2, "80")
            .set_TextMatrix(3, 2, "Peak" & vbCrLf & "G")
            .set_TextMatrix(4, 2, .get_TextMatrix(3, 2))

            .set_TextMatrix(0, 3, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 3, "88")
            .set_TextMatrix(2, 3, "72")
            .set_TextMatrix(3, 3, "3ms" & vbCrLf & "G")
            .set_TextMatrix(4, 3, .get_TextMatrix(3, 3))

            .set_TextMatrix(0, 4, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 4, "1000")
            .set_TextMatrix(2, 4, "650")
            .set_TextMatrix(3, 4, "HIC36")
            .set_TextMatrix(4, 4, .get_TextMatrix(3, 4))

            .set_TextMatrix(0, 5, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 5, "3.1")
            .set_TextMatrix(2, 5, "1.9")
            .set_TextMatrix(3, 5, "Shear" & vbCrLf & "[kN]")
            .set_TextMatrix(4, 5, .get_TextMatrix(3, 5))
            .set_TextMatrix(0, 6, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 6, "3.3")
            .set_TextMatrix(2, 6, "2.7")
            .set_TextMatrix(3, 6, "Tens." & vbCrLf & "[kN]")
            .set_TextMatrix(4, 6, .get_TextMatrix(3, 6))
            .set_TextMatrix(0, 7, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 7, "57")
            .set_TextMatrix(2, 7, "42")
            .set_TextMatrix(3, 7, "Exten." & vbCrLf & "[Nm]")
            .set_TextMatrix(4, 7, .get_TextMatrix(3, 7))

            .set_TextMatrix(0, 8, "Chest")
            .set_TextMatrix(1, 8, "50")
            .set_TextMatrix(2, 8, "22")
            .set_TextMatrix(3, 8, "Disp" & vbCrLf & "[mm]")
            .set_TextMatrix(4, 8, .get_TextMatrix(3, 8))
            .set_TextMatrix(0, 9, .get_TextMatrix(0, 8))
            .set_TextMatrix(1, 9, "1.0")
            .set_TextMatrix(2, 9, "0.5")
            .set_TextMatrix(3, 9, "VC" & vbCrLf & "[m/s]")
            .set_TextMatrix(4, 9, .get_TextMatrix(3, 9))

            .set_TextMatrix(0, 10, "Femur/Knee")
            .set_TextMatrix(1, 10, "15")
            .set_TextMatrix(2, 10, "6")
            .set_TextMatrix(3, 10, "K.Slide [mm]")
            .set_TextMatrix(4, 10, "LH")
            .set_TextMatrix(0, 11, .get_TextMatrix(0, 10))
            .set_TextMatrix(1, 11, .get_TextMatrix(1, 10))
            .set_TextMatrix(2, 11, .get_TextMatrix(2, 10))
            .set_TextMatrix(3, 11, .get_TextMatrix(3, 10))
            .set_TextMatrix(4, 11, "RH")

            .set_TextMatrix(0, 12, .get_TextMatrix(0, 10))
            .set_TextMatrix(1, 12, "9.07")
            .set_TextMatrix(2, 12, "3.8")
            .set_TextMatrix(3, 12, "K.Peak [kN]")
            .set_TextMatrix(4, 12, "LH")
            .set_TextMatrix(0, 13, .get_TextMatrix(0, 10))
            .set_TextMatrix(1, 13, .get_TextMatrix(1, 12))
            .set_TextMatrix(2, 13, .get_TextMatrix(2, 12))
            .set_TextMatrix(3, 13, .get_TextMatrix(3, 12))
            .set_TextMatrix(4, 13, "RH")

            .set_TextMatrix(0, 14, "Tibia")
            .set_TextMatrix(1, 14, "1.3")
            .set_TextMatrix(2, 14, "0.4")
            .set_TextMatrix(3, 14, "TI - Upr")
            .set_TextMatrix(4, 14, "LH")
            .set_TextMatrix(0, 15, .get_TextMatrix(0, 14))
            .set_TextMatrix(1, 15, .get_TextMatrix(1, 14))
            .set_TextMatrix(2, 15, .get_TextMatrix(2, 14))
            .set_TextMatrix(3, 15, .get_TextMatrix(3, 14))
            .set_TextMatrix(4, 15, "RH")
            .set_TextMatrix(0, 16, .get_TextMatrix(0, 14))
            .set_TextMatrix(1, 16, .get_TextMatrix(1, 14))
            .set_TextMatrix(2, 16, .get_TextMatrix(2, 14))
            .set_TextMatrix(3, 16, "TI - Lwr")
            .set_TextMatrix(4, 16, "LH")
            .set_TextMatrix(0, 17, .get_TextMatrix(0, 14))
            .set_TextMatrix(1, 17, .get_TextMatrix(1, 14))
            .set_TextMatrix(2, 17, .get_TextMatrix(2, 14))
            .set_TextMatrix(3, 17, .get_TextMatrix(3, 16))
            .set_TextMatrix(4, 17, "RH")
            .set_TextMatrix(0, 18, .get_TextMatrix(0, 14))
            .set_TextMatrix(1, 18, "8")
            .set_TextMatrix(2, 18, "2")
            .set_TextMatrix(3, 18, "Comp.[kN]")
            .set_TextMatrix(4, 18, "LH")
            .set_TextMatrix(0, 19, .get_TextMatrix(0, 14))
            .set_TextMatrix(1, 19, .get_TextMatrix(1, 18))
            .set_TextMatrix(2, 19, .get_TextMatrix(2, 18))
            .set_TextMatrix(3, 19, .get_TextMatrix(3, 18))
            .set_TextMatrix(4, 19, "RH")
            .set_TextMatrix(0, 20, "점수")
            .set_TextMatrix(1, 20, .get_TextMatrix(0, 20))
            .set_TextMatrix(2, 20, .get_TextMatrix(0, 20))
            .set_TextMatrix(3, 20, .get_TextMatrix(0, 20))
            .set_TextMatrix(4, 20, .get_TextMatrix(0, 20))

        End With
    End Sub

    'C-NCAP Frontal Header
    Private Sub Sheet_Lbl4()
        '내수
        Dim i As Integer

        With Me.InjuryLbl4
            .WordWrap = True
            .ScrollBars = MSFlexGridLib.ScrollBarsSettings.flexScrollBarNone

            .Rows = 5 '+ file_tot * 3
            .Cols = 21
            .FixedRows = 0
            .FixedCols = 0

            .FocusRect = MSFlexGridLib.FocusRectSettings.flexFocusNone
            .MergeCells = 1       '셀병합 허용

            .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillRepeat
            .Row = 1
            .Col = 2
            .RowSel = 2
            .ColSel = 19
            .CellFontBold = False
            .CellFontSize = 8
            .CellForeColor = Color.Gray

            For i = 0 To 20
                .set_MergeCol(i, True)
                .set_ColAlignment(i, 4) '가운데 정렬
                .set_ColWidth(i, 730)
            Next
            For i = 4 To 7
                .set_ColWidth(i, 810)
            Next
            For i = 14 To 19
                .set_ColWidth(i, 570)
            Next

            .set_ColWidth(0, 730 + 200)
            .set_ColWidth(20, 1100)

            .set_RowHeight(0, 250)
            .set_RowHeight(1, 250)
            .set_RowHeight(2, 250)
            .set_RowHeight(3, 250)
            .set_RowHeight(4, 250)

            .AllowBigSelection = True

            .set_MergeRow(0, True)
            .set_MergeRow(1, True)
            .set_MergeRow(2, True)
            .set_MergeRow(3, True)
            .set_MergeRow(4, True)

            .set_TextMatrix(0, 0, "중국" & vbCrLf & "정면")
            .set_TextMatrix(1, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(2, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(3, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(4, 0, .get_TextMatrix(0, 0))

            .set_TextMatrix(0, 1, "구분")
            .set_TextMatrix(1, 1, "상한")
            .set_TextMatrix(2, 1, "하한")
            .set_TextMatrix(3, 1, "상해")
            .set_TextMatrix(4, 1, .get_TextMatrix(3, 1))

            .set_TextMatrix(0, 2, "Head")
            .set_TextMatrix(1, 2, "80")
            .set_TextMatrix(2, 2, "80")
            .set_TextMatrix(3, 2, "Peak" & vbCrLf & "G")
            .set_TextMatrix(4, 2, .get_TextMatrix(3, 2))

            .set_TextMatrix(0, 3, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 3, "88")
            .set_TextMatrix(2, 3, "72")
            .set_TextMatrix(3, 3, "3ms" & vbCrLf & "G")
            .set_TextMatrix(4, 3, .get_TextMatrix(3, 3))

            .set_TextMatrix(0, 4, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 4, "1000")
            .set_TextMatrix(2, 4, "650")
            .set_TextMatrix(3, 4, "HIC36")
            .set_TextMatrix(4, 4, .get_TextMatrix(3, 4))

            .set_TextMatrix(0, 5, "Neck")
            .set_TextMatrix(1, 5, "3.1")
            .set_TextMatrix(2, 5, "1.9")
            .set_TextMatrix(3, 5, "Shear" & vbCrLf & "[kN]")
            .set_TextMatrix(4, 5, .get_TextMatrix(3, 5))
            .set_TextMatrix(0, 6, .get_TextMatrix(0, 5))
            .set_TextMatrix(1, 6, "3.3")
            .set_TextMatrix(2, 6, "2.7")
            .set_TextMatrix(3, 6, "Tens." & vbCrLf & "[kN]")
            .set_TextMatrix(4, 6, .get_TextMatrix(3, 6))
            .set_TextMatrix(0, 7, .get_TextMatrix(0, 5))
            .set_TextMatrix(1, 7, "57")
            .set_TextMatrix(2, 7, "42")
            .set_TextMatrix(3, 7, "Exten." & vbCrLf & "[Nm]")
            .set_TextMatrix(4, 7, .get_TextMatrix(3, 7))

            .set_TextMatrix(0, 8, "Chest")
            .set_TextMatrix(1, 8, "50")
            .set_TextMatrix(2, 8, "22")
            .set_TextMatrix(3, 8, "Disp" & vbCrLf & "[mm]")
            .set_TextMatrix(4, 8, .get_TextMatrix(3, 8))
            .set_TextMatrix(0, 9, .get_TextMatrix(0, 8))
            .set_TextMatrix(1, 9, "1.0")
            .set_TextMatrix(2, 9, "0.5")
            .set_TextMatrix(3, 9, "VC" & vbCrLf & "[m/s]")
            .set_TextMatrix(4, 9, .get_TextMatrix(3, 9))

            .set_TextMatrix(0, 10, "Femur/Knee")
            .set_TextMatrix(1, 10, "15")
            .set_TextMatrix(2, 10, "6")
            .set_TextMatrix(3, 10, "K.Slide [mm]")
            .set_TextMatrix(4, 10, "LH")
            .set_TextMatrix(0, 11, .get_TextMatrix(0, 10))
            .set_TextMatrix(1, 11, .get_TextMatrix(1, 10))
            .set_TextMatrix(2, 11, .get_TextMatrix(2, 10))
            .set_TextMatrix(3, 11, .get_TextMatrix(3, 10))
            .set_TextMatrix(4, 11, "RH")

            .set_TextMatrix(0, 12, .get_TextMatrix(0, 10))
            .set_TextMatrix(1, 12, "9.07")
            .set_TextMatrix(2, 12, "3.8")
            .set_TextMatrix(3, 12, "K.Peak [kN]")
            .set_TextMatrix(4, 12, "LH")
            .set_TextMatrix(0, 13, .get_TextMatrix(0, 10))
            .set_TextMatrix(1, 13, .get_TextMatrix(1, 12))
            .set_TextMatrix(2, 13, .get_TextMatrix(2, 12))
            .set_TextMatrix(3, 13, .get_TextMatrix(3, 12))
            .set_TextMatrix(4, 13, "RH")

            .set_TextMatrix(0, 14, "Tibia")
            .set_TextMatrix(1, 14, "1.3")
            .set_TextMatrix(2, 14, "0.4")
            .set_TextMatrix(3, 14, "TI - Upr")
            .set_TextMatrix(4, 14, "LH")
            .set_TextMatrix(0, 15, .get_TextMatrix(0, 14))
            .set_TextMatrix(1, 15, .get_TextMatrix(1, 14))
            .set_TextMatrix(2, 15, .get_TextMatrix(2, 14))
            .set_TextMatrix(3, 15, .get_TextMatrix(3, 14))
            .set_TextMatrix(4, 15, "RH")
            .set_TextMatrix(0, 16, .get_TextMatrix(0, 14))
            .set_TextMatrix(1, 16, .get_TextMatrix(1, 14))
            .set_TextMatrix(2, 16, .get_TextMatrix(2, 14))
            .set_TextMatrix(3, 16, "TI - Lwr")
            .set_TextMatrix(4, 16, "LH")
            .set_TextMatrix(0, 17, .get_TextMatrix(0, 14))
            .set_TextMatrix(1, 17, .get_TextMatrix(1, 14))
            .set_TextMatrix(2, 17, .get_TextMatrix(2, 14))
            .set_TextMatrix(3, 17, .get_TextMatrix(3, 16))
            .set_TextMatrix(4, 17, "RH")
            .set_TextMatrix(0, 18, .get_TextMatrix(0, 14))
            .set_TextMatrix(1, 18, "8")
            .set_TextMatrix(2, 18, "2")
            .set_TextMatrix(3, 18, "Comp.[kN]")
            .set_TextMatrix(4, 18, "LH")
            .set_TextMatrix(0, 19, .get_TextMatrix(0, 14))
            .set_TextMatrix(1, 19, .get_TextMatrix(1, 18))
            .set_TextMatrix(2, 19, .get_TextMatrix(2, 18))
            .set_TextMatrix(3, 19, .get_TextMatrix(3, 18))
            .set_TextMatrix(4, 19, "RH")
            .set_TextMatrix(0, 20, "점수")
            .set_TextMatrix(1, 20, .get_TextMatrix(0, 20))
            .set_TextMatrix(2, 20, .get_TextMatrix(0, 20))
            .set_TextMatrix(3, 20, .get_TextMatrix(0, 20))
            .set_TextMatrix(4, 20, .get_TextMatrix(0, 20))

        End With
    End Sub

    'C-NCAP Offset Header
    Private Sub Sheet_Lbl5()
        '중국 옵셋
        Dim i As Integer

        With Me.InjuryLbl5
            .WordWrap = True
            .ScrollBars = MSFlexGridLib.ScrollBarsSettings.flexScrollBarNone

            .Rows = 5 '+ file_tot * 3
            .Cols = 21
            .FixedRows = 0
            .FixedCols = 0

            .FocusRect = MSFlexGridLib.FocusRectSettings.flexFocusNone
            .MergeCells = 1       '셀병합 허용

            .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillRepeat
            .Row = 1
            .Col = 2
            .RowSel = 2
            .ColSel = 19
            .CellFontBold = False
            .CellFontSize = 8
            .CellForeColor = Color.Gray

            For i = 0 To 20
                .set_MergeCol(i, True)
                .set_ColAlignment(i, 4) '가운데 정렬
                .set_ColWidth(i, 730)
            Next
            For i = 4 To 7
                .set_ColWidth(i, 810)
            Next
            For i = 14 To 19
                .set_ColWidth(i, 570)
            Next

            .set_ColWidth(0, 730 + 200)
            .set_ColWidth(20, 1100)

            .set_RowHeight(0, 250)
            .set_RowHeight(1, 250)
            .set_RowHeight(2, 250)
            .set_RowHeight(3, 250)
            .set_RowHeight(4, 250)

            .AllowBigSelection = True

            .set_MergeRow(0, True)
            .set_MergeRow(1, True)
            .set_MergeRow(2, True)
            .set_MergeRow(3, True)
            .set_MergeRow(4, True)

            .set_TextMatrix(0, 0, "중국" & vbCrLf & "옵셋")
            .set_TextMatrix(1, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(2, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(3, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(4, 0, .get_TextMatrix(0, 0))

            .set_TextMatrix(0, 1, "구분")
            .set_TextMatrix(1, 1, "상한")
            .set_TextMatrix(2, 1, "하한")
            .set_TextMatrix(3, 1, "상해")
            .set_TextMatrix(4, 1, .get_TextMatrix(3, 1))

            .set_TextMatrix(0, 2, "Head & Neck")
            .set_TextMatrix(1, 2, "80")
            .set_TextMatrix(2, 2, "80")
            .set_TextMatrix(3, 2, "Peak" & vbCrLf & "G")
            .set_TextMatrix(4, 2, .get_TextMatrix(3, 2))

            .set_TextMatrix(0, 3, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 3, "88")
            .set_TextMatrix(2, 3, "72")
            .set_TextMatrix(3, 3, "3ms" & vbCrLf & "G")
            .set_TextMatrix(4, 3, .get_TextMatrix(3, 3))

            .set_TextMatrix(0, 4, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 4, "1000")
            .set_TextMatrix(2, 4, "650")
            .set_TextMatrix(3, 4, "HIC36")
            .set_TextMatrix(4, 4, .get_TextMatrix(3, 4))

            .set_TextMatrix(0, 5, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 5, "3.1")
            .set_TextMatrix(2, 5, "1.9")
            .set_TextMatrix(3, 5, "Shear" & vbCrLf & "[kN]")
            .set_TextMatrix(4, 5, .get_TextMatrix(3, 5))
            .set_TextMatrix(0, 6, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 6, "3.3")
            .set_TextMatrix(2, 6, "2.7")
            .set_TextMatrix(3, 6, "Tens." & vbCrLf & "[kN]")
            .set_TextMatrix(4, 6, .get_TextMatrix(3, 6))
            .set_TextMatrix(0, 7, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 7, "57")
            .set_TextMatrix(2, 7, "42")
            .set_TextMatrix(3, 7, "Exten." & vbCrLf & "[Nm]")
            .set_TextMatrix(4, 7, .get_TextMatrix(3, 7))

            .set_TextMatrix(0, 8, "Chest")
            .set_TextMatrix(1, 8, "50")
            .set_TextMatrix(2, 8, "22")
            .set_TextMatrix(3, 8, "Disp" & vbCrLf & "[mm]")
            .set_TextMatrix(4, 8, .get_TextMatrix(3, 8))
            .set_TextMatrix(0, 9, .get_TextMatrix(0, 8))
            .set_TextMatrix(1, 9, "1.0")
            .set_TextMatrix(2, 9, "0.5")
            .set_TextMatrix(3, 9, "VC" & vbCrLf & "[m/s]")
            .set_TextMatrix(4, 9, .get_TextMatrix(3, 9))

            .set_TextMatrix(0, 10, "Femur/Knee")
            .set_TextMatrix(1, 10, "15")
            .set_TextMatrix(2, 10, "6")
            .set_TextMatrix(3, 10, "K.Slide [mm]")
            .set_TextMatrix(4, 10, "LH")
            .set_TextMatrix(0, 11, .get_TextMatrix(0, 10))
            .set_TextMatrix(1, 11, .get_TextMatrix(1, 10))
            .set_TextMatrix(2, 11, .get_TextMatrix(2, 10))
            .set_TextMatrix(3, 11, .get_TextMatrix(3, 10))
            .set_TextMatrix(4, 11, "RH")

            .set_TextMatrix(0, 12, .get_TextMatrix(0, 10))
            .set_TextMatrix(1, 12, "9.07")
            .set_TextMatrix(2, 12, "3.8")
            .set_TextMatrix(3, 12, "K.Peak [kN]")
            .set_TextMatrix(4, 12, "LH")
            .set_TextMatrix(0, 13, .get_TextMatrix(0, 10))
            .set_TextMatrix(1, 13, .get_TextMatrix(1, 12))
            .set_TextMatrix(2, 13, .get_TextMatrix(2, 12))
            .set_TextMatrix(3, 13, .get_TextMatrix(3, 12))
            .set_TextMatrix(4, 13, "RH")

            .set_TextMatrix(0, 14, "Tibia")
            .set_TextMatrix(1, 14, "1.3")
            .set_TextMatrix(2, 14, "0.4")
            .set_TextMatrix(3, 14, "TI - Upr")
            .set_TextMatrix(4, 14, "LH")
            .set_TextMatrix(0, 15, .get_TextMatrix(0, 14))
            .set_TextMatrix(1, 15, .get_TextMatrix(1, 14))
            .set_TextMatrix(2, 15, .get_TextMatrix(2, 14))
            .set_TextMatrix(3, 15, .get_TextMatrix(3, 14))
            .set_TextMatrix(4, 15, "RH")
            .set_TextMatrix(0, 16, .get_TextMatrix(0, 14))
            .set_TextMatrix(1, 16, .get_TextMatrix(1, 14))
            .set_TextMatrix(2, 16, .get_TextMatrix(2, 14))
            .set_TextMatrix(3, 16, "TI - Lwr")
            .set_TextMatrix(4, 16, "LH")
            .set_TextMatrix(0, 17, .get_TextMatrix(0, 14))
            .set_TextMatrix(1, 17, .get_TextMatrix(1, 14))
            .set_TextMatrix(2, 17, .get_TextMatrix(2, 14))
            .set_TextMatrix(3, 17, .get_TextMatrix(3, 16))
            .set_TextMatrix(4, 17, "RH")
            .set_TextMatrix(0, 18, .get_TextMatrix(0, 14))
            .set_TextMatrix(1, 18, "8")
            .set_TextMatrix(2, 18, "2")
            .set_TextMatrix(3, 18, "Comp.[kN]")
            .set_TextMatrix(4, 18, "LH")
            .set_TextMatrix(0, 19, .get_TextMatrix(0, 14))
            .set_TextMatrix(1, 19, .get_TextMatrix(1, 18))
            .set_TextMatrix(2, 19, .get_TextMatrix(2, 18))
            .set_TextMatrix(3, 19, .get_TextMatrix(3, 18))
            .set_TextMatrix(4, 19, "RH")
            .set_TextMatrix(0, 20, "점수")
            .set_TextMatrix(1, 20, .get_TextMatrix(0, 20))
            .set_TextMatrix(2, 20, .get_TextMatrix(0, 20))
            .set_TextMatrix(3, 20, .get_TextMatrix(0, 20))
            .set_TextMatrix(4, 20, .get_TextMatrix(0, 20))

        End With
    End Sub

    'C-NCAP Rear Header
    Private Sub Sheet_Lbl6()
        Dim i As Integer

        With Me.InjuryLbl6
            .WordWrap = True
            .ScrollBars = MSFlexGridLib.ScrollBarsSettings.flexScrollBarNone

            .Rows = 4
            .Cols = 9
            .FixedRows = 0
            .FixedCols = 0

            .FocusRect = MSFlexGridLib.FocusRectSettings.flexFocusNone
            .MergeCells = 1       '셀병합 허용

            .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillRepeat
            .Row = 3 ' 번호다. 갯수 아님
            .Col = 4
            .RowSel = 3
            .ColSel = 5
            .CellFontBold = False
            .CellFontSize = 8
            .CellForeColor = Color.Gray

            .Row = 1
            .Col = 2
            .RowSel = 2
            .ColSel = 6
            .CellFontBold = False
            .CellFontSize = 8
            .CellForeColor = Color.Gray

            .Row = 0
            .Col = 8
            .RowSel = 3
            .ColSel = 8
            .CellFontBold = False
            .CellFontSize = 8
            .CellForeColor = Color.Gray

            For i = 0 To 8
                .set_MergeCol(i, True)
                .set_ColAlignment(i, 4) '가운데 정렬
                .set_ColWidth(i, 1700)
            Next
            .set_ColWidth(0, 1750)

            .set_RowHeight(0, 250)
            .set_RowHeight(1, 250)
            .set_RowHeight(2, 250)
            .set_RowHeight(3, 500)

            .AllowBigSelection = True

            .set_MergeRow(0, True)
            .set_MergeRow(1, True)
            .set_MergeRow(2, True)
            .set_MergeRow(3, True)

            .set_TextMatrix(0, 0, "중국" & vbCrLf & "후석")
            .set_TextMatrix(1, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(2, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(3, 0, .get_TextMatrix(0, 0))

            .set_TextMatrix(0, 1, "Articles")
            .set_TextMatrix(1, 1, "Upr.")
            .set_TextMatrix(2, 1, "Lwr")
            .set_TextMatrix(3, 1, "Injury")

            .set_TextMatrix(0, 2, "Head")
            .set_TextMatrix(1, 2, "700")
            .set_TextMatrix(2, 2, "500")
            .set_TextMatrix(3, 2, "HIC15" & vbCrLf & "[G]")

            .set_TextMatrix(0, 3, "Neck")
            .set_TextMatrix(1, 3, "2.62")
            .set_TextMatrix(2, 3, "1.7")
            .set_TextMatrix(3, 3, "Tens" & vbCrLf & "[kN]")

            .set_TextMatrix(0, 4, .get_TextMatrix(0, 3))
            .set_TextMatrix(1, 4, "1.95")
            .set_TextMatrix(2, 4, "1.2")
            .set_TextMatrix(3, 4, "Shear" & vbCrLf & "[kN]")

            .set_TextMatrix(0, 5, .get_TextMatrix(0, 3))
            .set_TextMatrix(1, 5, "49")
            .set_TextMatrix(2, 5, "36")
            .set_TextMatrix(3, 5, "Exten." & vbCrLf & "[Nm]")

            .set_TextMatrix(0, 6, "Chest")
            .set_TextMatrix(1, 6, "48")
            .set_TextMatrix(2, 6, "23")
            .set_TextMatrix(3, 6, "Disp." & vbCrLf & "[mm]")

            .set_TextMatrix(0, 7, "No Secondary" & vbCrLf & "Impact")
            .set_TextMatrix(1, 7, .get_TextMatrix(0, 7))
            .set_TextMatrix(2, 7, .get_TextMatrix(0, 7))
            .set_TextMatrix(3, 7, .get_TextMatrix(0, 7))

            .set_TextMatrix(0, 8, "Secondary" & vbCrLf & "Impact")
            .set_TextMatrix(1, 8, .get_TextMatrix(0, 8))
            .set_TextMatrix(2, 8, .get_TextMatrix(0, 8))
            .set_TextMatrix(3, 8, .get_TextMatrix(0, 8))

        End With
    End Sub

    'US-NCAP Injury Sheet
    Private Sub Sheet_US()
        '상해 점수 시트 (북미)
        Dim i As Integer
        Dim file_tot As Integer
        file_tot = OpenFile.names.Length
        Tot_File = file_tot

        Peak_Injury1.Rows = file_tot * 3
        Peak_Injury1.Cols = 15

        With Peak_Injury1
            .MergeCells = 1
            .SelectionMode = MSFlexGridLib.SelectionModeSettings.flexSelectionFree
            For i = 0 To 14
                .set_MergeCol(i, True)
                .set_ColAlignment(i, 4) '가운데 정렬
                .set_ColWidth(i, 1008 + 10)
            Next

            ReDim Star_Rating(OpenFile.names.Length - 1)

            For i = 1 To OpenFile.names.Length

                .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillRepeat
                .Row = (i - 1) * 3 + 1
                .Col = 1
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 13
                .CellFontBold = False
                .CellForeColor = Color.Gray

                .set_RowHeight((i - 1) * 3, 250)
                .set_RowHeight((i - 1) * 3 + 1, 250)
                .set_RowHeight((i - 1) * 3 + 2, 250)

                If DummyType(i - 1) = 0 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[50%]")
                ElseIf DummyType(i - 1) = 1 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[5%]")
                ElseIf DummyType(i - 1) = 2 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[THOR]")
                ElseIf DummyType(i - 1) = 3 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[Q-6]")
                ElseIf DummyType(i - 1) = 4 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[Q-10]")
                End If
                .set_TextMatrix((i - 1) * 3 + 1, 0, .get_TextMatrix((i - 1) * 3, 0))
                .set_TextMatrix((i - 1) * 3 + 2, 0, .get_TextMatrix((i - 1) * 3, 0))

                .set_TextMatrix((i - 1) * 3, 1, "Value")
                .set_TextMatrix((i - 1) * 3 + 1, 1, "Portion")
                .set_TextMatrix((i - 1) * 3 + 2, 1, "P")

                .set_TextMatrix((i - 1) * 3, 2, Format(HIC15(i - 1), "####.0"))
                .set_TextMatrix((i - 1) * 3 + 1, 2, Format(HIC15(i - 1) / 700, "0.0%"))
                .set_TextMatrix((i - 1) * 3 + 2, 2, Format(P_Head(i - 1), "0.00%"))

                .set_TextMatrix((i - 1) * 3, 3, Format(Neck_Tens(i - 1), "0.000"))
                If DummyType(i - 1) = 0 Or DummyType(i - 1) = 2 Then  'THOR and Hybrid 50%
                    .set_TextMatrix((i - 1) * 3 + 1, 3, Format(Neck_Tens(i - 1) / 4.17, "0.0%"))
                    .set_TextMatrix((i - 1) * 3 + 2, 3, Format(P_Neck_Tens(i - 1), "0.00%"))
                ElseIf DummyType(i - 1) = 1 _
                    Or DummyType(i - 1) = 3 Or DummyType(i - 1) = 4 Then
                    .set_TextMatrix((i - 1) * 3 + 1, 3, Format(Neck_Tens(i - 1) / 2.62, "0.0%"))
                    .set_TextMatrix((i - 1) * 3 + 2, 3, Format(P_Neck_Tens(i - 1), "0.00%"))
                End If

                .set_TextMatrix((i - 1) * 3, 4, Format(Neck_Comp(i - 1), "0.000"))
                If DummyType(i - 1) = 0 Or DummyType(i - 1) = 2 Then  'THOR and Hybrid 50%
                    .set_TextMatrix((i - 1) * 3 + 1, 4, Format(Neck_Comp(i - 1) / 4, "0.0%"))
                    .set_TextMatrix((i - 1) * 3 + 2, 4, Format(P_Neck_Comp(i - 1), "0.00%"))
                ElseIf DummyType(i - 1) = 1 _
                    Or DummyType(i - 1) = 3 Or DummyType(i - 1) = 4 Then
                    .set_TextMatrix((i - 1) * 3 + 1, 4, Format(Neck_Comp(i - 1) / 2.52, "0.0%"))
                    .set_TextMatrix((i - 1) * 3 + 2, 4, Format(P_Neck_Comp(i - 1), "0.00%"))
                End If

                .set_TextMatrix((i - 1) * 3, 5, Format(NTE(i - 1), "0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 5, Format(NTE(i - 1) / 1.0, "0.0%"))
                .set_TextMatrix((i - 1) * 3 + 2, 5, Format(P_Neck_NTE(i - 1), "0.00%"))
                .set_TextMatrix((i - 1) * 3, 6, Format(NTF(i - 1), "0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 6, Format(NTF(i - 1) / 1.0, "0.0%"))
                .set_TextMatrix((i - 1) * 3 + 2, 6, Format(P_Neck_NTF(i - 1), "0.00%"))
                .set_TextMatrix((i - 1) * 3, 7, Format(NCE(i - 1), "0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 7, Format(NCE(i - 1) / 1.0, "0.0%"))
                .set_TextMatrix((i - 1) * 3 + 2, 7, Format(P_Neck_NCE(i - 1), "0.00%"))
                .set_TextMatrix((i - 1) * 3, 8, Format(NCF(i - 1), "0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 8, Format(NCF(i - 1) / 1.0, "0.0%"))
                .set_TextMatrix((i - 1) * 3 + 2, 8, Format(P_Neck_NCF(i - 1), "0.00%"))

                .set_TextMatrix((i - 1) * 3, 9, Format(Chest_D(i - 1), "##.0"))
                If DummyType(i - 1) = 0 Then  'Hybrid 50%
                    ' 2015.12.23 변경 - 자릿수와 점수 동일하게
                    '.set_TextMatrix((i - 1) * 3 + 1, 9, Format(Chest_D(i - 1) / 63, "0.0%"))
                    .set_TextMatrix((i - 1) * 3 + 1, 9, Format(Format(Chest_D(i - 1), "##.0") / 63, "0.0%"))
                    .set_TextMatrix((i - 1) * 3 + 2, 9, Format(P_CD(i - 1), "0.00%"))
                ElseIf DummyType(i - 1) = 1 Or DummyType(i - 1) = 3 Or DummyType(i - 1) = 4 Then
                    ' 2015.12.23 변경 - 자릿수와 점수 동일하게
                    '.set_TextMatrix((i - 1) * 3 + 1, 9, Format(Chest_D(i - 1) / 52, "0.0%"))
                    .set_TextMatrix((i - 1) * 3 + 1, 9, Format(Format(Chest_D(i - 1), "##.0") / 52, "0.0%"))
                    .set_TextMatrix((i - 1) * 3 + 2, 9, Format(P_CD(i - 1), "0.00%"))
                ElseIf DummyType(i - 1) = 2 Then 'THOR
                    .set_TextMatrix((i - 1) * 3, 9, Format(ThxIrUpL(i - 1), "0.00") & "/" & Format(ThxIrUpR(i - 1), "0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 9, Format(ThxIrLowL(i - 1), "0.00") & "/" & Format(ThxIrLowR(i - 1), "0.00"))
                    .set_TextMatrix((i - 1) * 3 + 2, 9, "L / R " & Format(Chest_D(i - 1), "0.0"))
                    'Temp CD for THOR
                    .Row = (i - 1) * 3
                    .Col = 9
                    .RowSel = (i - 1) * 3 + 1
                    .ColSel = 9
                    .CellFontBold = False
                    .CellFontSize = 8
                    .CellForeColor = Color.Black
                End If

                'CG
                .set_TextMatrix((i - 1) * 3, 10, Format(Chest_G_CUMULATIVE_T3MS_inj(i - 1), "0.0"))
                .set_TextMatrix((i - 1) * 3 + 1, 10, Format(Chest_G_CUMULATIVE_T3MS_inj(i - 1) / 60.0, "0.0%"))
                .set_TextMatrix((i - 1) * 3 + 2, 10, "-")

                .set_TextMatrix((i - 1) * 3, 11, Format(FemurL(i - 1), "0.000"))
                If DummyType(i - 1) = 0 Or DummyType(i - 1) = 2 Then  'THOR and Hybrid 50%
                    .set_TextMatrix((i - 1) * 3 + 1, 11, Format(FemurL(i - 1) / 10, "0.0%"))
                    .set_TextMatrix((i - 1) * 3 + 2, 11, Format(P_FemurL(i - 1), "0.00%"))
                ElseIf DummyType(i - 1) = 1 _
                    Or DummyType(i - 1) = 3 Or DummyType(i - 1) = 4 Then
                    .set_TextMatrix((i - 1) * 3 + 1, 11, Format(FemurL(i - 1) / 6.81, "0.0%"))
                    .set_TextMatrix((i - 1) * 3 + 2, 11, Format(P_FemurL(i - 1), "0.00%"))
                End If
                .set_TextMatrix((i - 1) * 3, 12, Format(FemurR(i - 1), "0.000"))
                If DummyType(i - 1) = 0 Or DummyType(i - 1) = 2 Then  'THOR and Hybrid 50%
                    .set_TextMatrix((i - 1) * 3 + 1, 12, Format(FemurR(i - 1) / 10, "0.0%"))
                    .set_TextMatrix((i - 1) * 3 + 2, 12, Format(P_FemurR(i - 1), "0.00%"))
                ElseIf DummyType(i - 1) = 1 _
                    Or DummyType(i - 1) = 3 Or DummyType(i - 1) = 4 Then
                    .set_TextMatrix((i - 1) * 3 + 1, 12, Format(FemurR(i - 1) / 6.81, "0.0%"))
                    .set_TextMatrix((i - 1) * 3 + 2, 12, Format(P_FemurR(i - 1), "0.00%"))
                End If

                .set_TextMatrix((i - 1) * 3, 13, "--")
                'Average Probability
                .set_TextMatrix((i - 1) * 3 + 1, 13, Format(Math.Round(1 - ((1 - P_Head(i - 1)) * (1 - P_Neck_Max(i - 1)) * (1 - P_CD(i - 1)) * (1 - P_Femur_Max(i - 1))), 3), "0.00%"))
                'RRS
                Dim tmp_P_aver As Double
                tmp_P_aver = Math.Round(1 - ((1 - P_Head(i - 1)) * (1 - P_Neck_Max(i - 1)) * (1 - P_CD(i - 1)) * (1 - P_Femur_Max(i - 1))), 3)
                .set_TextMatrix((i - 1) * 3 + 2, 13, Math.Round(tmp_P_aver / 0.15, 2))

                Dim Tmp_Star As Double

                Tmp_Star = Fix(StarRating(Math.Round(tmp_P_aver / 0.15, 2)) * 100) / 100
                Star_Rating(i - 1) = Tmp_Star
                .set_TextMatrix((i - 1) * 3, 14, Tmp_Star & "★")
                .set_TextMatrix((i - 1) * 3 + 1, 14, Tmp_Star & "★")
                .set_TextMatrix((i - 1) * 3 + 2, 14, Tmp_Star & "★")

            Next

        End With

    End Sub

    'k-NCAP Frontal Injury Sheet
    Private Sub Sheet_DOM_F()
        '상해 점수 시트 (내수정면)
        Dim i As Integer
        Dim file_tot As Integer
        file_tot = OpenFile.names.Length

        Me.Peak_Injury_DOM_F.Rows = file_tot * 3
        Me.Peak_Injury_DOM_F.Cols = 15 '21

        With Me.Peak_Injury_DOM_F
            .WordWrap = True
            .ScrollBars = MSFlexGridLib.ScrollBarsSettings.flexScrollBarVertical
            .FixedCols = 0
            .FixedRows = 0
            .MergeCells = 1
            .SelectionMode = MSFlexGridLib.SelectionModeSettings.flexSelectionFree
            .AllowBigSelection = True
            .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillRepeat

            For i = 1 To 13
                '.set_MergeCol(i, True)
                .set_ColAlignment(i, 4) '가운데 정렬
                .set_ColWidth(i, 1010)
            Next
            .set_ColWidth(1, 1050)
            .set_ColWidth(14, 1100)
            .set_MergeCol(0, True)
            .set_MergeCol(1, True)
            .set_MergeCol(14, True)
            .set_ColAlignment(0, 4) '가운데 정렬
            .set_ColAlignment(1, 4) '가운데 정렬
            .set_ColAlignment(14, 4) '가운데 정렬

            Dim Score_Arr(20) As Single
            Score_Arr(0) = -10

            For i = 1 To OpenFile.names.Length
                .set_RowHeight((i - 1) * 3, 250)
                .set_RowHeight((i - 1) * 3 + 1, 250)
                .set_RowHeight((i - 1) * 3 + 2, 250)

                '.set_MergeRow((i - 1) * 3 + 2, True)

                If DummyType(i - 1) = 0 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[50%]")
                ElseIf DummyType(i - 1) = 1 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[5%]")
                ElseIf DummyType(i - 1) = 2 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[THOR]")
                ElseIf DummyType(i - 1) = 3 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[Q-6]")
                ElseIf DummyType(i - 1) = 4 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[Q-10]")
                End If
                .set_TextMatrix((i - 1) * 3 + 1, 0, .get_TextMatrix((i - 1) * 3, 0))
                .set_TextMatrix((i - 1) * 3 + 2, 0, .get_TextMatrix((i - 1) * 3, 0))

                .set_TextMatrix((i - 1) * 3, 1, "Value")
                .set_TextMatrix((i - 1) * 3 + 1, 1, "Score")
                .set_TextMatrix((i - 1) * 3 + 2, 1, .get_TextMatrix((i - 1) * 3 + 1, 1))

                If DummyType(i - 1) = 0 Or DummyType(i - 1) = 2 Then '*****************************THOR 포함************
                    '50% 더미============================================================================================
                    .set_TextMatrix((i - 1) * 3, 2, Format(Head_Peak_G(i - 1), "###0.0#"))
                    .set_TextMatrix((i - 1) * 3 + 1, 2, "-")

                    .set_TextMatrix((i - 1) * 3, 3, Format(Head3MSG(i - 1), "###0.0#"))
                    .set_TextMatrix((i - 1) * 3 + 1, 3, "-")

                    .set_TextMatrix((i - 1) * 3, 4, Format(HIC36(i - 1), "###0"))
                    .set_TextMatrix((i - 1) * 3 + 1, 4, Format(DOM_Score(1000, 650, HIC36(i - 1), 6.0), "0.00"))
                    Score_Arr(1) = Format(DOM_Score(1000, 650, HIC36(i - 1), 6.0), "0.00")

                    .set_TextMatrix((i - 1) * 3, 5, Format(Neck_Shear(i - 1), "###0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 5, Format(DOM_Score(3.1, 1.9, Neck_Shear(i - 1), 6.0), "0.00"))
                    Score_Arr(2) = Format(DOM_Score(3.1, 1.9, Neck_Shear(i - 1), 6.0), "0.00")

                    .set_TextMatrix((i - 1) * 3, 6, Format(Neck_Tens(i - 1), "###0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 6, Format(DOM_Score(3.3, 2.7, Neck_Tens(i - 1), 6.0), "0.00"))
                    Score_Arr(3) = Format(DOM_Score(3.3, 2.7, Neck_Tens(i - 1), 6.0), "0.00")

                    .set_TextMatrix((i - 1) * 3, 7, Format(Neck_Exten(i - 1), "###0.0"))
                    .set_TextMatrix((i - 1) * 3 + 1, 7, Format(DOM_Score(57, 42, Neck_Exten(i - 1), 6.0), "0.00"))
                    Score_Arr(4) = Format(DOM_Score(57, 42, Neck_Exten(i - 1), 6.0), "0.00")

                    .set_TextMatrix((i - 1) * 3 + 2, 7, Format(DOM_min(Score_Arr, 4), "0.00"))
                    '.set_TextMatrix((i - 1) * 3 + 2, 6, .get_TextMatrix((i - 1) * 3 + 2, 7))
                    '.set_TextMatrix((i - 1) * 3 + 2, 5, .get_TextMatrix((i - 1) * 3 + 2, 7))
                    '.set_TextMatrix((i - 1) * 3 + 2, 4, .get_TextMatrix((i - 1) * 3 + 2, 7))
                    '.set_TextMatrix((i - 1) * 3 + 2, 3, .get_TextMatrix((i - 1) * 3 + 2, 7))
                    '.set_TextMatrix((i - 1) * 3 + 2, 2, .get_TextMatrix((i - 1) * 3 + 2, 7))

                    .Row = (i - 1) * 3 + 2
                    .Col = 2
                    .RowSel = (i - 1) * 3 + 2
                    .ColSel = 7

                    If DOM_min(Score_Arr, 4) >= 6.0 Then
                        .CellBackColor = Color.LightGreen
                    ElseIf DOM_min(Score_Arr, 4) >= 4.0 Then
                        .CellBackColor = Color.Yellow
                    ElseIf DOM_min(Score_Arr, 4) >= 2.0 Then
                        .CellBackColor = Color.FromArgb(255, 192, 0)
                    ElseIf DOM_min(Score_Arr, 4) > 0.0 Then
                        .CellBackColor = Color.FromArgb(128, 96, 0)
                    Else
                        .CellBackColor = Color.Red
                    End If

                    .set_TextMatrix((i - 1) * 3, 8, Format(Chest_D(i - 1), "###0.0"))
                    ' 2015.12.23 변경 - 자릿수와 점수 동일하게
                    '.set_TextMatrix((i - 1) * 3 + 1, 8, Format(DOM_Score(50, 22, Chest_D(i - 1), 6.0), "0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 8, Format(DOM_Score(50, 22, Format(Chest_D(i - 1), "###0.0"), 6.0), "0.00"))
                    'Score_Arr(1) = Format(DOM_Score(50, 22, Chest_D(i - 1), 6.0), "0.00")
                    Score_Arr(1) = Format(DOM_Score(50, 22, Format(Chest_D(i - 1), "###0.0"), 6.0), "0.00")

                    .set_TextMatrix((i - 1) * 3, 9, Format(Chest_VC(i - 1), "###0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 9, Format(DOM_Score(1.0, 0.5, Chest_VC(i - 1), 6.0), "0.00"))
                    Score_Arr(2) = Format(DOM_Score(1.0, 0.5, Chest_VC(i - 1), 6.0), "0.00")

                    .set_TextMatrix((i - 1) * 3 + 2, 9, Format(DOM_min(Score_Arr, 2), "0.00"))
                    '.set_TextMatrix((i - 1) * 3 + 2, 8, .get_TextMatrix((i - 1) * 3 + 2, 9))

                    .Row = (i - 1) * 3 + 2
                    .Col = 8
                    .RowSel = (i - 1) * 3 + 2
                    .ColSel = 9
                    If DOM_min(Score_Arr, 2) >= 6.0 Then
                        .CellBackColor = Color.LightGreen
                    ElseIf DOM_min(Score_Arr, 2) >= 4.0 Then
                        .CellBackColor = Color.Yellow
                    ElseIf DOM_min(Score_Arr, 2) >= 2.0 Then
                        .CellBackColor = Color.FromArgb(255, 192, 0)
                    ElseIf DOM_min(Score_Arr, 2) > 0.0 Then
                        .CellBackColor = Color.FromArgb(128, 96, 0)
                    Else
                        .CellBackColor = Color.Red
                    End If

                    '내수 정면 k.slide 점수 안들어감
                    .set_TextMatrix((i - 1) * 3, 10, Format(KneeS_L(i - 1), "###0.0"))
                    .set_TextMatrix((i - 1) * 3 + 1, 10, "-") ' Format(DOM_Score(15.0, 6.0, KneeS_L(i - 1), 4.0), "0.00"))
                    Score_Arr(3) = Format(DOM_Score(15.0, 6.0, KneeS_L(i - 1), 4.0), "0.00")

                    .set_TextMatrix((i - 1) * 3, 11, Format(KneeS_R(i - 1), "###0.0"))
                    .set_TextMatrix((i - 1) * 3 + 1, 11, "-") ' Format(DOM_Score(15.0, 6.0, KneeS_R(i - 1), 4.0), "0.00"))
                    Score_Arr(4) = Format(DOM_Score(15.0, 6.0, KneeS_R(i - 1), 4.0), "0.00")

                    .set_TextMatrix((i - 1) * 3, 12, Format(FemurL(i - 1), "###0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 12, Format(DOM_Score(9.07, 3.8, FemurL(i - 1), 4.0), "0.00"))
                    Score_Arr(1) = Format(DOM_Score(9.07, 3.8, FemurL(i - 1), 4.0), "0.00")


                    .set_TextMatrix((i - 1) * 3, 13, Format(FemurR(i - 1), "###0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 13, Format(DOM_Score(9.07, 3.8, FemurR(i - 1), 4.0), "0.00"))
                    Score_Arr(2) = Format(DOM_Score(9.07, 3.8, FemurR(i - 1), 4.0), "0.00")


                    .set_TextMatrix((i - 1) * 3 + 2, 13, Format(DOM_min(Score_Arr, 2), "0.00"))
                    '.set_TextMatrix((i - 1) * 3 + 2, 12, .get_TextMatrix((i - 1) * 3 + 2, 13))
                    '.set_TextMatrix((i - 1) * 3 + 2, 11, .get_TextMatrix((i - 1) * 3 + 2, 13))
                    '.set_TextMatrix((i - 1) * 3 + 2, 10, .get_TextMatrix((i - 1) * 3 + 2, 13))

                    .Row = (i - 1) * 3 + 2
                    .Col = 10
                    .RowSel = (i - 1) * 3 + 2
                    .ColSel = 13
                    If DOM_min(Score_Arr, 2) >= 4.0 Then
                        .CellBackColor = Color.LightGreen
                    ElseIf DOM_min(Score_Arr, 2) >= 2.67 Then
                        .CellBackColor = Color.Yellow
                    ElseIf DOM_min(Score_Arr, 2) >= 1.33 Then
                        .CellBackColor = Color.FromArgb(255, 192, 0)
                    ElseIf DOM_min(Score_Arr, 2) > 0.0 Then
                        .CellBackColor = Color.FromArgb(128, 96, 0)
                    Else
                        .CellBackColor = Color.Red
                    End If

                    '.set_TextMatrix((i - 1) * 3, 14, Format(TI_upr_L(i - 1), "###0.00"))
                    '.set_TextMatrix((i - 1) * 3 + 1, 14, "-")
                    '.set_TextMatrix((i - 1) * 3 + 2, 14, "-")

                    '.set_TextMatrix((i - 1) * 3, 15, Format(TI_upr_R(i - 1), "###0.00"))
                    '.set_TextMatrix((i - 1) * 3 + 1, 15, "-")
                    '.set_TextMatrix((i - 1) * 3 + 2, 15, "-")

                    '.set_TextMatrix((i - 1) * 3, 16, Format(TI_lwr_L(i - 1), "###0.00"))
                    '.set_TextMatrix((i - 1) * 3 + 1, 16, "-")
                    '.set_TextMatrix((i - 1) * 3 + 2, 16, "-")

                    '.set_TextMatrix((i - 1) * 3, 17, Format(TI_lwr_R(i - 1), "###0.00"))
                    '.set_TextMatrix((i - 1) * 3 + 1, 17, "-")
                    '.set_TextMatrix((i - 1) * 3 + 2, 17, "-")

                    '.set_TextMatrix((i - 1) * 3, 18, Format(Tibia_Comp_L(i - 1), "###0.00"))
                    '.set_TextMatrix((i - 1) * 3 + 1, 18, "-")
                    '.set_TextMatrix((i - 1) * 3 + 2, 18, "-")

                    '.set_TextMatrix((i - 1) * 3, 19, Format(Tibia_Comp_R(i - 1), "###0.00"))
                    '.set_TextMatrix((i - 1) * 3 + 1, 19, "-")
                    '.set_TextMatrix((i - 1) * 3 + 2, 19, "-")

                    '.Row = (i - 1) * 3 + 1
                    '.Col = 14
                    '.RowSel = (i - 1) * 3 + 2
                    '.ColSel = 19
                    '.CellBackColor = Color.SlateGray

                    .set_TextMatrix((i - 1) * 3, 14, Format(CSng(.get_TextMatrix((i - 1) * 3 + 2, 7)) _
                                     + CSng(.get_TextMatrix((i - 1) * 3 + 2, 9)) _
                                     + CSng(.get_TextMatrix((i - 1) * 3 + 2, 13)), "0.0"))
                    .set_TextMatrix((i - 1) * 3 + 1, 14, .get_TextMatrix((i - 1) * 3, 14))
                    .set_TextMatrix((i - 1) * 3 + 2, 14, .get_TextMatrix((i - 1) * 3, 14))

                    '이상 50% ======================================================================================================
                ElseIf DummyType(i - 1) = 1 _
                    Or DummyType(i - 1) = 3 Or DummyType(i - 1) = 4 Then
                    '5% 더미 + 유아 Q더미============================================================================================
                    .set_TextMatrix((i - 1) * 3, 2, Format(Head_Peak_G(i - 1), "###0.0#"))
                    .set_TextMatrix((i - 1) * 3 + 1, 2, "-")

                    .set_TextMatrix((i - 1) * 3, 3, Format(Head3MSG(i - 1), "###0.0#"))
                    .set_TextMatrix((i - 1) * 3 + 1, 3, "-")

                    .set_TextMatrix((i - 1) * 3, 4, Format(HIC15(i - 1), "###0"))
                    .set_TextMatrix((i - 1) * 3 + 1, 4, Format(DOM_Score(700, 500, HIC15(i - 1), 6.0), "0.00"))
                    Score_Arr(1) = Format(DOM_Score(700, 500, HIC15(i - 1), 6.0), "0.00")

                    .set_TextMatrix((i - 1) * 3, 5, Format(Neck_Shear(i - 1), "###0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 5, Format(DOM_Score(1.95, 1.2, Neck_Shear(i - 1), 6.0), "0.00"))
                    Score_Arr(2) = Format(DOM_Score(1.95, 1.2, Neck_Shear(i - 1), 6.0), "0.00")

                    .set_TextMatrix((i - 1) * 3, 6, Format(Neck_Tens(i - 1), "###0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 6, Format(DOM_Score(2.62, 1.7, Neck_Tens(i - 1), 6.0), "0.00"))
                    Score_Arr(3) = Format(DOM_Score(2.62, 1.7, Neck_Tens(i - 1), 6.0), "0.00")

                    .set_TextMatrix((i - 1) * 3, 7, Format(Neck_Exten(i - 1), "###0.0"))
                    .set_TextMatrix((i - 1) * 3 + 1, 7, Format(DOM_Score(49, 36, Neck_Exten(i - 1), 6.0), "0.00"))
                    Score_Arr(4) = Format(DOM_Score(49, 36, Neck_Exten(i - 1), 6.0), "0.00")

                    .set_TextMatrix((i - 1) * 3 + 2, 7, Format(DOM_min(Score_Arr, 4), "0.00"))
                    '.set_TextMatrix((i - 1) * 3 + 2, 6, .get_TextMatrix((i - 1) * 3 + 2, 7))
                    '.set_TextMatrix((i - 1) * 3 + 2, 5, .get_TextMatrix((i - 1) * 3 + 2, 7))
                    '.set_TextMatrix((i - 1) * 3 + 2, 4, .get_TextMatrix((i - 1) * 3 + 2, 7))
                    '.set_TextMatrix((i - 1) * 3 + 2, 3, .get_TextMatrix((i - 1) * 3 + 2, 7))
                    '.set_TextMatrix((i - 1) * 3 + 2, 2, .get_TextMatrix((i - 1) * 3 + 2, 7))

                    .Row = (i - 1) * 3 + 2
                    .Col = 2
                    .RowSel = (i - 1) * 3 + 2
                    .ColSel = 7

                    If DOM_min(Score_Arr, 4) >= 6.0 Then
                        .CellBackColor = Color.LightGreen
                    ElseIf DOM_min(Score_Arr, 4) >= 4.0 Then
                        .CellBackColor = Color.Yellow
                    ElseIf DOM_min(Score_Arr, 4) >= 2.0 Then
                        .CellBackColor = Color.FromArgb(255, 192, 0)
                    ElseIf DOM_min(Score_Arr, 4) > 0.0 Then
                        .CellBackColor = Color.FromArgb(128, 96, 0)
                    Else
                        .CellBackColor = Color.Red
                    End If

                    .set_TextMatrix((i - 1) * 3, 8, Format(Chest_D(i - 1), "###0.0"))
                    ' 2015.12.23 변경 - 자릿수와 점수 동일하게
                    '.set_TextMatrix((i - 1) * 3 + 1, 8, Format(DOM_Score(48, 22, Chest_D(i - 1), 6.0), "0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 8, Format(DOM_Score(48, 22, Format(Chest_D(i - 1), "###0.0"), 6.0), "0.00"))
                    'Score_Arr(1) = Format(DOM_Score(48, 22, Chest_D(i - 1), 6.0), "0.00")
                    Score_Arr(1) = Format(DOM_Score(48, 22, Format(Chest_D(i - 1), "###0.0"), 6.0), "0.00")

                    .set_TextMatrix((i - 1) * 3, 9, Format(Chest_VC(i - 1), "###0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 9, Format(DOM_Score(1.0, 0.5, Chest_VC(i - 1), 6.0), "0.00"))
                    .set_TextMatrix((i - 1) * 3 + 2, 9, Format(DOM_min(Score_Arr, 2), "0.00"))
                    Score_Arr(2) = Format(DOM_Score(1.0, 0.5, Chest_VC(i - 1), 6.0), "0.00")
                    '.set_TextMatrix((i - 1) * 3 + 2, 8, .get_TextMatrix((i - 1) * 3 + 2, 9))

                    .Row = (i - 1) * 3 + 2
                    .Col = 8
                    .RowSel = (i - 1) * 3 + 2
                    .ColSel = 9
                    If DOM_min(Score_Arr, 2) >= 6.0 Then
                        .CellBackColor = Color.LightGreen
                    ElseIf DOM_min(Score_Arr, 2) >= 4.0 Then
                        .CellBackColor = Color.Yellow
                    ElseIf DOM_min(Score_Arr, 2) >= 2.0 Then
                        .CellBackColor = Color.FromArgb(255, 192, 0)
                    ElseIf DOM_min(Score_Arr, 2) > 0.0 Then
                        .CellBackColor = Color.FromArgb(128, 96, 0)
                    Else
                        .CellBackColor = Color.Red
                    End If

                    '내수 정면 k.slide 점수 안들어감
                    .set_TextMatrix((i - 1) * 3, 10, Format(KneeS_L(i - 1), "###0.0"))
                    .set_TextMatrix((i - 1) * 3 + 1, 10, "-") 'Format(DOM_Score(15.0, 6.0, KneeS_L(i - 1), 4.0), "0.00"))
                    Score_Arr(3) = Format(DOM_Score(15.0, 6.0, KneeS_L(i - 1), 4.0), "0.00")

                    .set_TextMatrix((i - 1) * 3, 11, Format(KneeS_R(i - 1), "###0.0"))
                    .set_TextMatrix((i - 1) * 3 + 1, 11, "-") 'Format(DOM_Score(15.0, 6.0, KneeS_R(i - 1), 4.0), "0.00"))
                    Score_Arr(4) = Format(DOM_Score(15.0, 6.0, KneeS_R(i - 1), 4.0), "0.00")

                    .set_TextMatrix((i - 1) * 3, 12, Format(FemurL(i - 1), "###0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 12, Format(DOM_Score(6.8, 3.8, FemurL(i - 1), 4.0), "0.00"))
                    Score_Arr(1) = Format(DOM_Score(6.8, 3.8, FemurL(i - 1), 4.0), "0.00")

                    .set_TextMatrix((i - 1) * 3, 13, Format(FemurR(i - 1), "###0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 13, Format(DOM_Score(6.8, 3.8, FemurR(i - 1), 4.0), "0.00"))
                    Score_Arr(2) = Format(DOM_Score(6.8, 3.8, FemurR(i - 1), 4.0), "0.00")
                    .set_TextMatrix((i - 1) * 3 + 2, 13, Format(DOM_min(Score_Arr, 2), "0.00"))
                    '.set_TextMatrix((i - 1) * 3 + 2, 12, .get_TextMatrix((i - 1) * 3 + 2, 13))
                    '.set_TextMatrix((i - 1) * 3 + 2, 11, .get_TextMatrix((i - 1) * 3 + 2, 13))
                    '.set_TextMatrix((i - 1) * 3 + 2, 10, .get_TextMatrix((i - 1) * 3 + 2, 13))

                    .Row = (i - 1) * 3 + 2
                    .Col = 10
                    .RowSel = (i - 1) * 3 + 2
                    .ColSel = 13
                    If DOM_min(Score_Arr, 2) >= 4.0 Then
                        .CellBackColor = Color.LightGreen
                    ElseIf DOM_min(Score_Arr, 2) >= 2.67 Then
                        .CellBackColor = Color.Yellow
                    ElseIf DOM_min(Score_Arr, 2) >= 1.33 Then
                        .CellBackColor = Color.FromArgb(255, 192, 0)
                    ElseIf DOM_min(Score_Arr, 2) > 0.0 Then
                        .CellBackColor = Color.FromArgb(128, 96, 0)
                    Else
                        .CellBackColor = Color.Red
                    End If

                    '.set_TextMatrix((i - 1) * 3, 14, Format(TI_upr_L(i - 1), "###0.00"))
                    '.set_TextMatrix((i - 1) * 3 + 1, 14, "-")
                    '.set_TextMatrix((i - 1) * 3 + 2, 14, "-")

                    '.set_TextMatrix((i - 1) * 3, 15, Format(TI_upr_R(i - 1), "###0.00"))
                    '.set_TextMatrix((i - 1) * 3 + 1, 15, "-")
                    '.set_TextMatrix((i - 1) * 3 + 2, 15, "-")

                    '.set_TextMatrix((i - 1) * 3, 16, Format(TI_lwr_L(i - 1), "###0.00"))
                    '.set_TextMatrix((i - 1) * 3 + 1, 16, "-")
                    '.set_TextMatrix((i - 1) * 3 + 2, 16, "-")

                    '.set_TextMatrix((i - 1) * 3, 17, Format(TI_lwr_R(i - 1), "###0.00"))
                    '.set_TextMatrix((i - 1) * 3 + 1, 17, "-")
                    '.set_TextMatrix((i - 1) * 3 + 2, 17, "-")

                    '.set_TextMatrix((i - 1) * 3, 18, Format(Tibia_Comp_L(i - 1), "###0.00"))
                    '.set_TextMatrix((i - 1) * 3 + 1, 18, "-")
                    '.set_TextMatrix((i - 1) * 3 + 2, 18, "-")

                    '.set_TextMatrix((i - 1) * 3, 19, Format(Tibia_Comp_R(i - 1), "###0.00"))
                    '.set_TextMatrix((i - 1) * 3 + 1, 19, "-")
                    '.set_TextMatrix((i - 1) * 3 + 2, 19, "-")

                    '.Row = (i - 1) * 3 + 1
                    '.Col = 14
                    '.RowSel = (i - 1) * 3 + 2
                    '.ColSel = 19
                    '.CellBackColor = Color.SlateGray

                    .set_TextMatrix((i - 1) * 3, 14, Format(CSng(.get_TextMatrix((i - 1) * 3 + 2, 7)) _
                                     + CSng(.get_TextMatrix((i - 1) * 3 + 2, 9)) _
                                     + CSng(.get_TextMatrix((i - 1) * 3 + 2, 13)), "0.0"))
                    .set_TextMatrix((i - 1) * 3 + 1, 14, .get_TextMatrix((i - 1) * 3, 14))
                    .set_TextMatrix((i - 1) * 3 + 2, 14, .get_TextMatrix((i - 1) * 3, 14))

                    '5% 더미============================================================================================
                End If

                .Row = (i - 1) * 3 + 1
                .Col = 2
                .RowSel = (i - 1) * 3 + 1
                .ColSel = 13
                .CellFontBold = False
                .CellForeColor = Color.Gray


            Next

        End With

    End Sub

    'k-NCAP Offset Injury Sheet
    Private Sub Sheet_DOM_O()
        '상해 점수 시트 (내수옵셋)
        Dim i As Integer
        Dim file_tot As Integer
        file_tot = OpenFile.names.Length

        Me.Peak_Injury_DOM_O.Rows = file_tot * 3
        Me.Peak_Injury_DOM_O.Cols = 21

        With Me.Peak_Injury_DOM_O
            .WordWrap = True
            .ScrollBars = MSFlexGridLib.ScrollBarsSettings.flexScrollBarVertical
            .FixedCols = 0
            .FixedRows = 0
            .MergeCells = 1
            .SelectionMode = MSFlexGridLib.SelectionModeSettings.flexSelectionFree
            .AllowBigSelection = True
            .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillRepeat

            For i = 0 To 20
                .set_ColAlignment(i, 4) '가운데 정렬
                .set_ColWidth(i, 730)
            Next
            For i = 4 To 7
                .set_ColWidth(i, 810)
            Next
            For i = 14 To 19
                '.set_MergeCol(i, True)
                .set_ColWidth(i, 570)
            Next

            .set_ColWidth(0, 730 + 200)
            .set_ColWidth(20, 1100)

            .set_MergeCol(0, True)
            .set_MergeCol(1, True)
            .set_MergeCol(20, True)
            .set_ColAlignment(20, 4) '가운데 정렬

            Dim Score_Arr(20) As Single
            Score_Arr(0) = -10

            For i = 1 To OpenFile.names.Length
                .set_RowHeight((i - 1) * 3, 250)
                .set_RowHeight((i - 1) * 3 + 1, 250)
                .set_RowHeight((i - 1) * 3 + 2, 250)

                '.set_MergeRow((i - 1) * 3 + 2, True)

                If DummyType(i - 1) = 0 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[50%]")
                ElseIf DummyType(i - 1) = 1 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[5%]")
                ElseIf DummyType(i - 1) = 2 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[THOR]")
                ElseIf DummyType(i - 1) = 3 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[Q-6]")
                ElseIf DummyType(i - 1) = 4 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[Q-10]")
                End If
                .set_TextMatrix((i - 1) * 3 + 1, 0, .get_TextMatrix((i - 1) * 3, 0))
                .set_TextMatrix((i - 1) * 3 + 2, 0, .get_TextMatrix((i - 1) * 3, 0))

                .set_TextMatrix((i - 1) * 3, 1, "Value")
                .set_TextMatrix((i - 1) * 3 + 1, 1, "Score")
                .set_TextMatrix((i - 1) * 3 + 2, 1, .get_TextMatrix((i - 1) * 3 + 1, 1))

                '50% 더미============================================================================================
                .set_TextMatrix((i - 1) * 3, 2, Format(Head_Peak_G(i - 1), "###0.0#"))
                .set_TextMatrix((i - 1) * 3 + 1, 2, "-")

                .set_TextMatrix((i - 1) * 3, 3, Format(Head3MSG(i - 1), "###0.0#"))
                .set_TextMatrix((i - 1) * 3 + 1, 3, "-")

                .set_TextMatrix((i - 1) * 3, 4, Format(HIC36(i - 1), "###0"))
                .set_TextMatrix((i - 1) * 3 + 1, 4, Format(DOM_Score(1000, 650, HIC36(i - 1), 4.0), "0.00"))
                Score_Arr(1) = Format(DOM_Score(1000, 650, HIC36(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 5, Format(Neck_Shear(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 5, Format(DOM_Score(3.1, 1.9, Neck_Shear(i - 1), 4.0), "0.00"))
                Score_Arr(2) = Format(DOM_Score(3.1, 1.9, Neck_Shear(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 6, Format(Neck_Tens(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 6, Format(DOM_Score(3.3, 2.7, Neck_Tens(i - 1), 4.0), "0.00"))
                Score_Arr(3) = Format(DOM_Score(3.3, 2.7, Neck_Tens(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 7, Format(Neck_Exten(i - 1), "###0.0"))
                .set_TextMatrix((i - 1) * 3 + 1, 7, Format(DOM_Score(57, 42, Neck_Exten(i - 1), 4.0), "0.00"))
                Score_Arr(4) = Format(DOM_Score(57, 42, Neck_Exten(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3 + 2, 7, Format(DOM_min(Score_Arr, 4), "0.00"))

                .Row = (i - 1) * 3 + 2
                .Col = 2
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 7

                If DOM_min(Score_Arr, 4) >= 4.0 Then
                    .CellBackColor = Color.LightGreen
                ElseIf DOM_min(Score_Arr, 4) >= 2.67 Then
                    .CellBackColor = Color.Yellow
                ElseIf DOM_min(Score_Arr, 4) >= 1.33 Then
                    .CellBackColor = Color.FromArgb(255, 192, 0)
                ElseIf DOM_min(Score_Arr, 4) > 0.0 Then
                    .CellBackColor = Color.FromArgb(128, 96, 0)
                Else
                    .CellBackColor = Color.Red
                End If

                .set_TextMatrix((i - 1) * 3, 8, Format(Chest_D(i - 1), "###0.0"))
                ' 2015.12.23 변경 - 자릿수와 점수 동일하게
                '.set_TextMatrix((i - 1) * 3 + 1, 8, Format(DOM_Score(50, 22, Chest_D(i - 1), 4.0), "0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 8, Format(DOM_Score(50, 22, Format(Chest_D(i - 1), "###0.0"), 4.0), "0.00"))
                'Score_Arr(1) = Format(DOM_Score(50, 22, Chest_D(i - 1), 4.0), "0.00")
                Score_Arr(1) = Format(DOM_Score(50, 22, Format(Chest_D(i - 1), "###0.0"), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 9, Format(Chest_VC(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 9, Format(DOM_Score(1.0, 0.5, Chest_VC(i - 1), 4.0), "0.00"))
                Score_Arr(2) = Format(DOM_Score(1.0, 0.5, Chest_VC(i - 1), 4.0), "0.00")
                .set_TextMatrix((i - 1) * 3 + 2, 9, Format(DOM_min(Score_Arr, 2), "0.00"))

                .Row = (i - 1) * 3 + 2
                .Col = 8
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 9
                If DOM_min(Score_Arr, 2) >= 4.0 Then
                    .CellBackColor = Color.LightGreen
                ElseIf DOM_min(Score_Arr, 2) >= 2.67 Then
                    .CellBackColor = Color.Yellow
                ElseIf DOM_min(Score_Arr, 2) >= 1.33 Then
                    .CellBackColor = Color.FromArgb(255, 192, 0)
                ElseIf DOM_min(Score_Arr, 2) > 0.0 Then
                    .CellBackColor = Color.FromArgb(128, 96, 0)
                Else
                    .CellBackColor = Color.Red
                End If

                .set_TextMatrix((i - 1) * 3, 10, Format(KneeS_L(i - 1), "###0.0"))
                .set_TextMatrix((i - 1) * 3 + 1, 10, Format(DOM_Score(15.0, 6.0, KneeS_L(i - 1), 4.0), "0.00"))
                Score_Arr(1) = Format(DOM_Score(15.0, 6.0, KneeS_L(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 11, Format(KneeS_R(i - 1), "###0.0"))
                .set_TextMatrix((i - 1) * 3 + 1, 11, Format(DOM_Score(15.0, 6.0, KneeS_R(i - 1), 4.0), "0.00"))
                Score_Arr(2) = Format(DOM_Score(15.0, 6.0, KneeS_R(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 12, Format(FemurL(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 12, Format(DOM_Score(9.07, 3.8, FemurL(i - 1), 4.0), "0.00"))
                Score_Arr(3) = Format(DOM_Score(9.07, 3.8, FemurL(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 13, Format(FemurR(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 13, Format(DOM_Score(9.07, 3.8, FemurR(i - 1), 4.0), "0.00"))
                Score_Arr(4) = Format(DOM_Score(9.07, 3.8, FemurR(i - 1), 4.0), "0.00")
                .set_TextMatrix((i - 1) * 3 + 2, 13, Format(DOM_min(Score_Arr, 4), "0.00"))

                .Row = (i - 1) * 3 + 2
                .Col = 10
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 13
                If DOM_min(Score_Arr, 4) >= 4.0 Then
                    .CellBackColor = Color.LightGreen
                ElseIf DOM_min(Score_Arr, 4) >= 2.67 Then
                    .CellBackColor = Color.Yellow
                ElseIf DOM_min(Score_Arr, 4) >= 1.33 Then
                    .CellBackColor = Color.FromArgb(255, 192, 0)
                ElseIf DOM_min(Score_Arr, 4) > 0.0 Then
                    .CellBackColor = Color.FromArgb(128, 96, 0)
                Else
                    .CellBackColor = Color.Red
                End If

                .set_TextMatrix((i - 1) * 3, 14, Format(TI_upr_L(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 14, Format(DOM_Score(1.3, 0.4, TI_upr_L(i - 1), 4.0), "0.00"))
                '.set_TextMatrix((i - 1) * 3 + 2, 14, "-")
                Score_Arr(1) = Format(DOM_Score(1.3, 0.4, TI_upr_L(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 15, Format(TI_upr_R(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 15, Format(DOM_Score(1.3, 0.4, TI_upr_R(i - 1), 4.0), "0.00"))
                '.set_TextMatrix((i - 1) * 3 + 2, 15, "-")
                Score_Arr(2) = Format(DOM_Score(1.3, 0.4, TI_upr_R(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 16, Format(TI_lwr_L(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 16, Format(DOM_Score(1.3, 0.4, TI_lwr_L(i - 1), 4.0), "0.00"))
                '.set_TextMatrix((i - 1) * 3 + 2, 16, "-")
                Score_Arr(3) = Format(DOM_Score(1.3, 0.4, TI_lwr_L(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 17, Format(TI_lwr_R(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 17, Format(DOM_Score(1.3, 0.4, TI_lwr_R(i - 1), 4.0), "0.00"))
                '.set_TextMatrix((i - 1) * 3 + 2, 17, "-")
                Score_Arr(4) = Format(DOM_Score(1.3, 0.4, TI_lwr_R(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 18, Format(Tibia_Comp_L(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 18, Format(DOM_Score(8, 2, Tibia_Comp_L(i - 1), 4.0), "0.00"))
                '.set_TextMatrix((i - 1) * 3 + 2, 18, "-")
                Score_Arr(5) = Format(DOM_Score(8, 2, Tibia_Comp_L(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 19, Format(Tibia_Comp_R(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 19, Format(DOM_Score(8, 2, Tibia_Comp_R(i - 1), 4.0), "0.00"))
                Score_Arr(6) = Format(DOM_Score(8, 2, Tibia_Comp_R(i - 1), 4.0), "0.00")
                .set_TextMatrix((i - 1) * 3 + 2, 19, Format(DOM_min(Score_Arr, 6), "0.00"))

                .Row = (i - 1) * 3 + 2
                .Col = 14
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 19
                If DOM_min(Score_Arr, 6) >= 4.0 Then
                    .CellBackColor = Color.LightGreen
                ElseIf DOM_min(Score_Arr, 6) >= 2.67 Then
                    .CellBackColor = Color.Yellow
                ElseIf DOM_min(Score_Arr, 6) >= 1.33 Then
                    .CellBackColor = Color.FromArgb(255, 192, 0)
                ElseIf DOM_min(Score_Arr, 6) > 0.0 Then
                    .CellBackColor = Color.FromArgb(128, 96, 0)
                Else
                    .CellBackColor = Color.Red
                End If

                .Row = (i - 1) * 3 + 1
                .Col = 2
                .RowSel = (i - 1) * 3 + 1
                .ColSel = 19
                .CellFontBold = False
                .CellForeColor = Color.Gray

                .set_TextMatrix((i - 1) * 3, 20, Format(CSng(.get_TextMatrix((i - 1) * 3 + 2, 7)) _
                                 + CSng(.get_TextMatrix((i - 1) * 3 + 2, 9)) _
                                 + CSng(.get_TextMatrix((i - 1) * 3 + 2, 13)) _
                                 + CSng(.get_TextMatrix((i - 1) * 3 + 2, 19)), "0.0"))
                .set_TextMatrix((i - 1) * 3 + 1, 20, .get_TextMatrix((i - 1) * 3, 20))
                .set_TextMatrix((i - 1) * 3 + 2, 20, .get_TextMatrix((i - 1) * 3, 20))

                '이상 50% ======================================================================================================
            Next

        End With

    End Sub

    'C-NCAP Frontal Injury Sheet
    Private Sub Sheet_China_F()
        '상해 점수 시트 (중국 정면)
        Dim i As Integer
        Dim file_tot As Integer
        file_tot = OpenFile.names.Length

        Me.Peak_Injury_CHINA_F.Rows = file_tot * 3
        Me.Peak_Injury_CHINA_F.Cols = 21

        With Me.Peak_Injury_CHINA_F
            .WordWrap = True
            .ScrollBars = MSFlexGridLib.ScrollBarsSettings.flexScrollBarVertical
            .FixedCols = 0
            .FixedRows = 0
            .MergeCells = 1
            .SelectionMode = MSFlexGridLib.SelectionModeSettings.flexSelectionFree
            .AllowBigSelection = True
            .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillRepeat

            For i = 0 To 20
                .set_ColAlignment(i, 4) '가운데 정렬
                .set_ColWidth(i, 730)
            Next
            For i = 4 To 7
                .set_ColWidth(i, 810)
            Next
            For i = 14 To 19
                '.set_MergeCol(i, True)
                .set_ColWidth(i, 570)
            Next

            .set_ColWidth(0, 730 + 200)
            .set_ColWidth(20, 1100)

            .set_MergeCol(0, True)
            .set_MergeCol(1, True)
            .set_MergeCol(20, True)
            .set_ColAlignment(20, 4) '가운데 정렬

            Dim Score_Arr(20) As Single
            Score_Arr(0) = -10

            For i = 1 To OpenFile.names.Length
                .set_RowHeight((i - 1) * 3, 250)
                .set_RowHeight((i - 1) * 3 + 1, 250)
                .set_RowHeight((i - 1) * 3 + 2, 250)

                '.set_MergeRow((i - 1) * 3 + 2, True)

                '헤더
                If DummyType(i - 1) = 0 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[50%]")
                ElseIf DummyType(i - 1) = 1 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[5%]")
                ElseIf DummyType(i - 1) = 2 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[THOR]")
                ElseIf DummyType(i - 1) = 3 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[Q-6]")
                ElseIf DummyType(i - 1) = 4 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[Q-10]")
                End If
                .set_TextMatrix((i - 1) * 3 + 1, 0, .get_TextMatrix((i - 1) * 3, 0))
                .set_TextMatrix((i - 1) * 3 + 2, 0, .get_TextMatrix((i - 1) * 3, 0))

                .set_TextMatrix((i - 1) * 3, 1, "Value")
                .set_TextMatrix((i - 1) * 3 + 1, 1, "Score")
                .set_TextMatrix((i - 1) * 3 + 2, 1, .get_TextMatrix((i - 1) * 3 + 1, 1))

                '50% 더미============================================================================================
                .set_TextMatrix((i - 1) * 3, 2, Format(Head_Peak_G(i - 1), "###0.0#"))
                .set_TextMatrix((i - 1) * 3 + 1, 2, "-")

                .set_TextMatrix((i - 1) * 3, 3, Format(Head3MSG(i - 1), "###0.0#"))
                .set_TextMatrix((i - 1) * 3 + 1, 3, Format(DOM_Score(88, 72, Head3MSG(i - 1), 5.0), "0.00"))
                Score_Arr(1) = Format(DOM_Score(88, 72, Head3MSG(i - 1), 5.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 4, Format(HIC36(i - 1), "###0"))
                .set_TextMatrix((i - 1) * 3 + 1, 4, Format(DOM_Score(1000, 650, HIC36(i - 1), 5.0), "0.00"))
                Score_Arr(2) = Format(DOM_Score(1000, 650, HIC36(i - 1), 5.0), "0.00")

                .set_TextMatrix((i - 1) * 3 + 2, 4, Format(DOM_min(Score_Arr, 2), "0.00"))

                '머리점수
                .Row = (i - 1) * 3 + 2
                .Col = 2
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 4

                If DOM_min(Score_Arr, 2) >= 5.0 Then
                    .CellBackColor = Color.LightGreen
                ElseIf DOM_min(Score_Arr, 2) >= (5 * (2 / 3)) Then
                    .CellBackColor = Color.Yellow
                ElseIf DOM_min(Score_Arr, 2) >= (5 * (1 / 3)) Then
                    .CellBackColor = Color.FromArgb(255, 192, 0)
                ElseIf DOM_min(Score_Arr, 2) > 0.0 Then
                    .CellBackColor = Color.FromArgb(128, 96, 0)
                Else
                    .CellBackColor = Color.Red
                End If

                .set_TextMatrix((i - 1) * 3, 5, Format(Neck_Shear(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 5, Format(DOM_Score(3.1, 1.9, Neck_Shear(i - 1), 2.0), "0.00"))
                Score_Arr(1) = Format(DOM_Score(3.1, 1.9, Neck_Shear(i - 1), 2.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 6, Format(Neck_Tens(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 6, Format(DOM_Score(3.3, 2.7, Neck_Tens(i - 1), 2.0), "0.00"))
                Score_Arr(2) = Format(DOM_Score(3.3, 2.7, Neck_Tens(i - 1), 2.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 7, Format(Neck_Exten(i - 1), "###0.0"))
                .set_TextMatrix((i - 1) * 3 + 1, 7, Format(DOM_Score(57, 42, Neck_Exten(i - 1), 2.0), "0.00"))
                Score_Arr(3) = Format(DOM_Score(57, 42, Neck_Exten(i - 1), 2.0), "0.00")

                .set_TextMatrix((i - 1) * 3 + 2, 7, Format(DOM_min(Score_Arr, 3), "0.00"))

                .Row = (i - 1) * 3 + 2
                .Col = 5
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 7

                If DOM_min(Score_Arr, 3) >= 2.0 Then
                    .CellBackColor = Color.LightGreen
                ElseIf DOM_min(Score_Arr, 3) >= (2 * (2 / 3)) Then
                    .CellBackColor = Color.Yellow
                ElseIf DOM_min(Score_Arr, 3) >= (2 * (2 / 3)) Then
                    .CellBackColor = Color.FromArgb(255, 192, 0)
                ElseIf DOM_min(Score_Arr, 3) > 0.0 Then
                    .CellBackColor = Color.FromArgb(128, 96, 0)
                Else
                    .CellBackColor = Color.Red
                End If

                .set_TextMatrix((i - 1) * 3, 8, Format(Chest_D(i - 1), "###0.0"))
                ' 2015.12.23 변경 - 자릿수와 점수 동일하게
                '.set_TextMatrix((i - 1) * 3 + 1, 8, Format(DOM_Score(50, 22, Chest_D(i - 1), 5.0), "0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 8, Format(DOM_Score(50, 22, Format(Chest_D(i - 1), "###0.0"), 5.0), "0.00"))
                'Score_Arr(1) = Format(DOM_Score(50, 22, Chest_D(i - 1), 5.0), "0.00")
                Score_Arr(1) = Format(DOM_Score(50, 22, Format(Chest_D(i - 1), "###0.0"), 5.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 9, Format(Chest_VC(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 9, Format(DOM_Score(1.0, 0.5, Chest_VC(i - 1), 5.0), "0.00"))
                Score_Arr(2) = Format(DOM_Score(1.0, 0.5, Chest_VC(i - 1), 5.0), "0.00")

                .set_TextMatrix((i - 1) * 3 + 2, 9, Format(DOM_min(Score_Arr, 2), "0.00"))

                .Row = (i - 1) * 3 + 2
                .Col = 8
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 9
                If DOM_min(Score_Arr, 2) >= 5.0 Then
                    .CellBackColor = Color.LightGreen
                ElseIf DOM_min(Score_Arr, 2) >= (5 * (2 / 3)) Then
                    .CellBackColor = Color.Yellow
                ElseIf DOM_min(Score_Arr, 2) >= (5 * (1 / 3)) Then
                    .CellBackColor = Color.FromArgb(255, 192, 0)
                ElseIf DOM_min(Score_Arr, 2) > 0.0 Then
                    .CellBackColor = Color.FromArgb(128, 96, 0)
                Else
                    .CellBackColor = Color.Red
                End If

                .set_TextMatrix((i - 1) * 3, 10, Format(KneeS_L(i - 1), "###0.0"))
                .set_TextMatrix((i - 1) * 3 + 1, 10, Format(DOM_Score(15.0, 6.0, KneeS_L(i - 1), 2.0), "0.00"))
                Score_Arr(1) = Format(DOM_Score(15.0, 6.0, KneeS_L(i - 1), 2.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 11, Format(KneeS_R(i - 1), "###0.0"))
                .set_TextMatrix((i - 1) * 3 + 1, 11, Format(DOM_Score(15.0, 6.0, KneeS_R(i - 1), 2.0), "0.00"))
                Score_Arr(2) = Format(DOM_Score(15.0, 6.0, KneeS_R(i - 1), 2.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 12, Format(FemurL(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 12, Format(DOM_Score(9.07, 3.8, FemurL(i - 1), 2.0), "0.00"))
                Score_Arr(3) = Format(DOM_Score(9.07, 3.8, FemurL(i - 1), 2.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 13, Format(FemurR(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 13, Format(DOM_Score(9.07, 3.8, FemurR(i - 1), 2.0), "0.00"))
                Score_Arr(4) = Format(DOM_Score(9.07, 3.8, FemurR(i - 1), 2.0), "0.00")
                .set_TextMatrix((i - 1) * 3 + 2, 13, Format(DOM_min(Score_Arr, 4), "0.00"))

                .Row = (i - 1) * 3 + 2
                .Col = 10
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 13
                If DOM_min(Score_Arr, 4) >= 2.0 Then
                    .CellBackColor = Color.LightGreen
                ElseIf DOM_min(Score_Arr, 4) >= (2 * (2 / 3)) Then
                    .CellBackColor = Color.Yellow
                ElseIf DOM_min(Score_Arr, 4) >= (2 * (1 / 3)) Then
                    .CellBackColor = Color.FromArgb(255, 192, 0)
                ElseIf DOM_min(Score_Arr, 4) > 0.0 Then
                    .CellBackColor = Color.FromArgb(128, 96, 0)
                Else
                    .CellBackColor = Color.Red
                End If

                .set_TextMatrix((i - 1) * 3, 14, Format(TI_upr_L(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 14, Format(DOM_Score(1.3, 0.4, TI_upr_L(i - 1), 2.0), "0.00"))
                '.set_TextMatrix((i - 1) * 3 + 2, 14, "-")
                Score_Arr(1) = Format(DOM_Score(1.3, 0.4, TI_upr_L(i - 1), 2.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 15, Format(TI_upr_R(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 15, Format(DOM_Score(1.3, 0.4, TI_upr_R(i - 1), 2.0), "0.00"))
                '.set_TextMatrix((i - 1) * 3 + 2, 15, "-")
                Score_Arr(2) = Format(DOM_Score(1.3, 0.4, TI_upr_R(i - 1), 2.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 16, Format(TI_lwr_L(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 16, Format(DOM_Score(1.3, 0.4, TI_lwr_L(i - 1), 2.0), "0.00"))
                '.set_TextMatrix((i - 1) * 3 + 2, 16, "-")
                Score_Arr(3) = Format(DOM_Score(1.3, 0.4, TI_lwr_L(i - 1), 2.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 17, Format(TI_lwr_R(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 17, Format(DOM_Score(1.3, 0.4, TI_lwr_R(i - 1), 2.0), "0.00"))
                '.set_TextMatrix((i - 1) * 3 + 2, 17, "-")
                Score_Arr(4) = Format(DOM_Score(1.3, 0.4, TI_lwr_R(i - 1), 2.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 18, Format(Tibia_Comp_L(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 18, Format(DOM_Score(8, 2, Tibia_Comp_L(i - 1), 2.0), "0.00"))
                '.set_TextMatrix((i - 1) * 3 + 2, 18, "-")
                Score_Arr(5) = Format(DOM_Score(8, 2, Tibia_Comp_L(i - 1), 2.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 19, Format(Tibia_Comp_R(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 19, Format(DOM_Score(8, 2, Tibia_Comp_R(i - 1), 2.0), "0.00"))
                Score_Arr(6) = Format(DOM_Score(8, 2, Tibia_Comp_R(i - 1), 2.0), "0.00")
                .set_TextMatrix((i - 1) * 3 + 2, 19, Format(DOM_min(Score_Arr, 6), "0.00"))

                .Row = (i - 1) * 3 + 2
                .Col = 14
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 19
                If DOM_min(Score_Arr, 6) >= 2.0 Then
                    .CellBackColor = Color.LightGreen
                ElseIf DOM_min(Score_Arr, 6) >= (2 * (2 / 3)) Then
                    .CellBackColor = Color.Yellow
                ElseIf DOM_min(Score_Arr, 6) >= (2 * (1 / 3)) Then
                    .CellBackColor = Color.FromArgb(255, 192, 0)
                ElseIf DOM_min(Score_Arr, 6) > 0.0 Then
                    .CellBackColor = Color.FromArgb(128, 96, 0)
                Else
                    .CellBackColor = Color.Red
                End If

                .Row = (i - 1) * 3 + 1
                .Col = 2
                .RowSel = (i - 1) * 3 + 1
                .ColSel = 19
                .CellFontBold = False
                .CellForeColor = Color.Gray

                .set_TextMatrix((i - 1) * 3, 20, Format(CSng(.get_TextMatrix((i - 1) * 3 + 2, 4)) _
                                 + CSng(.get_TextMatrix((i - 1) * 3 + 2, 7)) _
                                 + CSng(.get_TextMatrix((i - 1) * 3 + 2, 9)) _
                                 + CSng(.get_TextMatrix((i - 1) * 3 + 2, 13)) _
                                 + CSng(.get_TextMatrix((i - 1) * 3 + 2, 19)), "0.0"))
                .set_TextMatrix((i - 1) * 3 + 1, 20, .get_TextMatrix((i - 1) * 3, 20))
                .set_TextMatrix((i - 1) * 3 + 2, 20, .get_TextMatrix((i - 1) * 3, 20))

                '이상 50% ======================================================================================================
            Next

        End With
    End Sub

    'C-NCAP Offset Injury Sheet
    Private Sub Sheet_China_0()
        '상해 점수 시트 (중국 옵셋)
        Dim i As Integer
        Dim file_tot As Integer
        file_tot = OpenFile.names.Length

        Me.Peak_Injury_CHINA_O.Rows = file_tot * 3
        Me.Peak_Injury_CHINA_O.Cols = 21

        With Me.Peak_Injury_CHINA_O
            .WordWrap = True
            .ScrollBars = MSFlexGridLib.ScrollBarsSettings.flexScrollBarVertical
            .FixedCols = 0
            .FixedRows = 0
            .MergeCells = 1
            .SelectionMode = MSFlexGridLib.SelectionModeSettings.flexSelectionFree
            .AllowBigSelection = True
            .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillRepeat

            For i = 0 To 20
                .set_ColAlignment(i, 4) '가운데 정렬
                .set_ColWidth(i, 730)
            Next
            For i = 4 To 7
                .set_ColWidth(i, 810)
            Next
            For i = 14 To 19
                '.set_MergeCol(i, True)
                .set_ColWidth(i, 570)
            Next


            .set_ColWidth(0, 730 + 200)
            .set_ColWidth(20, 1100)

            .set_MergeCol(0, True)
            .set_MergeCol(1, True)
            .set_MergeCol(20, True)

            .set_ColAlignment(20, 4) '가운데 정렬

            Dim Score_Arr(20) As Single
            Score_Arr(0) = -10

            For i = 1 To OpenFile.names.Length
                .set_RowHeight((i - 1) * 3, 250)
                .set_RowHeight((i - 1) * 3 + 1, 250)
                .set_RowHeight((i - 1) * 3 + 2, 250)

                '.set_MergeRow((i - 1) * 3 + 2, True)

                '헤더
                If DummyType(i - 1) = 0 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[50%]")
                ElseIf DummyType(i - 1) = 1 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[5%]")
                ElseIf DummyType(i - 1) = 2 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[THOR]")
                ElseIf DummyType(i - 1) = 3 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[Q-6]")
                ElseIf DummyType(i - 1) = 4 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[Q-10]")
                End If
                .set_TextMatrix((i - 1) * 3 + 1, 0, .get_TextMatrix((i - 1) * 3, 0))
                .set_TextMatrix((i - 1) * 3 + 2, 0, .get_TextMatrix((i - 1) * 3, 0))

                .set_TextMatrix((i - 1) * 3, 1, "Value")
                .set_TextMatrix((i - 1) * 3 + 1, 1, "Score")
                .set_TextMatrix((i - 1) * 3 + 2, 1, .get_TextMatrix((i - 1) * 3 + 1, 1))

                '50% 더미============================================================================================
                .set_TextMatrix((i - 1) * 3, 2, Format(Head_Peak_G(i - 1), "###0.0#"))
                .set_TextMatrix((i - 1) * 3 + 1, 2, "-")

                .set_TextMatrix((i - 1) * 3, 3, Format(Head3MSG(i - 1), "###0.0#"))
                .set_TextMatrix((i - 1) * 3 + 1, 3, Format(DOM_Score(88, 72, Head3MSG(i - 1), 4.0), "0.00"))
                Score_Arr(1) = Format(DOM_Score(88, 72, Head3MSG(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 4, Format(HIC36(i - 1), "###0"))
                .set_TextMatrix((i - 1) * 3 + 1, 4, Format(DOM_Score(1000, 650, HIC36(i - 1), 4.0), "0.00"))
                Score_Arr(2) = Format(DOM_Score(1000, 650, HIC36(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 5, Format(Neck_Shear(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 5, Format(DOM_Score(3.1, 1.9, Neck_Shear(i - 1), 4.0), "0.00"))
                Score_Arr(3) = Format(DOM_Score(3.1, 1.9, Neck_Shear(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 6, Format(Neck_Tens(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 6, Format(DOM_Score(3.3, 2.7, Neck_Tens(i - 1), 4.0), "0.00"))
                Score_Arr(4) = Format(DOM_Score(3.3, 2.7, Neck_Tens(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 7, Format(Neck_Exten(i - 1), "###0.0"))
                .set_TextMatrix((i - 1) * 3 + 1, 7, Format(DOM_Score(57, 42, Neck_Exten(i - 1), 4.0), "0.00"))
                Score_Arr(5) = Format(DOM_Score(57, 42, Neck_Exten(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3 + 2, 7, Format(DOM_min(Score_Arr, 5), "0.00"))

                .Row = (i - 1) * 3 + 2
                .Col = 2
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 7

                '머리/목 점수
                If DOM_min(Score_Arr, 3) >= 4.0 Then
                    .CellBackColor = Color.LightGreen
                ElseIf DOM_min(Score_Arr, 3) >= (4 * (2 / 3)) Then
                    .CellBackColor = Color.Yellow
                ElseIf DOM_min(Score_Arr, 3) >= (4 * (2 / 3)) Then
                    .CellBackColor = Color.FromArgb(255, 192, 0)
                ElseIf DOM_min(Score_Arr, 3) > 0.0 Then
                    .CellBackColor = Color.FromArgb(128, 96, 0)
                Else
                    .CellBackColor = Color.Red
                End If

                .set_TextMatrix((i - 1) * 3, 8, Format(Chest_D(i - 1), "###0.0"))
                ' 2015.12.23 변경 - 자릿수와 점수 동일하게
                '.set_TextMatrix((i - 1) * 3 + 1, 8, Format(DOM_Score(50, 22, Chest_D(i - 1), 4.0), "0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 8, Format(DOM_Score(50, 22, Format(Chest_D(i - 1), "###0.0"), 4.0), "0.00"))
                'Score_Arr(1) = Format(DOM_Score(50, 22, Chest_D(i - 1), 4.0), "0.00")
                Score_Arr(1) = Format(DOM_Score(50, 22, Format(Chest_D(i - 1), "###0.0"), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 9, Format(Chest_VC(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 9, Format(DOM_Score(1.0, 0.5, Chest_VC(i - 1), 4.0), "0.00"))
                Score_Arr(2) = Format(DOM_Score(1.0, 0.5, Chest_VC(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3 + 2, 9, Format(DOM_min(Score_Arr, 2), "0.00"))

                .Row = (i - 1) * 3 + 2
                .Col = 8
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 9
                If DOM_min(Score_Arr, 2) >= 4.0 Then
                    .CellBackColor = Color.LightGreen
                ElseIf DOM_min(Score_Arr, 2) >= (4 * (2 / 3)) Then
                    .CellBackColor = Color.Yellow
                ElseIf DOM_min(Score_Arr, 2) >= (4 * (1 / 3)) Then
                    .CellBackColor = Color.FromArgb(255, 192, 0)
                ElseIf DOM_min(Score_Arr, 2) > 0.0 Then
                    .CellBackColor = Color.FromArgb(128, 96, 0)
                Else
                    .CellBackColor = Color.Red
                End If

                .set_TextMatrix((i - 1) * 3, 10, Format(KneeS_L(i - 1), "###0.0"))
                .set_TextMatrix((i - 1) * 3 + 1, 10, Format(DOM_Score(15.0, 6.0, KneeS_L(i - 1), 4.0), "0.00"))
                Score_Arr(1) = Format(DOM_Score(15.0, 6.0, KneeS_L(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 11, Format(KneeS_R(i - 1), "###0.0"))
                .set_TextMatrix((i - 1) * 3 + 1, 11, Format(DOM_Score(15.0, 6.0, KneeS_R(i - 1), 4.0), "0.00"))
                Score_Arr(2) = Format(DOM_Score(15.0, 6.0, KneeS_R(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 12, Format(FemurL(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 12, Format(DOM_Score(9.07, 3.8, FemurL(i - 1), 4.0), "0.00"))
                Score_Arr(3) = Format(DOM_Score(9.07, 3.8, FemurL(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 13, Format(FemurR(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 13, Format(DOM_Score(9.07, 3.8, FemurR(i - 1), 4.0), "0.00"))
                Score_Arr(4) = Format(DOM_Score(9.07, 3.8, FemurR(i - 1), 4.0), "0.00")
                .set_TextMatrix((i - 1) * 3 + 2, 13, Format(DOM_min(Score_Arr, 4), "0.00"))

                .Row = (i - 1) * 3 + 2
                .Col = 10
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 13
                If DOM_min(Score_Arr, 4) >= 4.0 Then
                    .CellBackColor = Color.LightGreen
                ElseIf DOM_min(Score_Arr, 4) >= (4 * (2 / 3)) Then
                    .CellBackColor = Color.Yellow
                ElseIf DOM_min(Score_Arr, 4) >= (4 * (1 / 3)) Then
                    .CellBackColor = Color.FromArgb(255, 192, 0)
                ElseIf DOM_min(Score_Arr, 4) > 0.0 Then
                    .CellBackColor = Color.FromArgb(128, 96, 0)
                Else
                    .CellBackColor = Color.Red
                End If

                .set_TextMatrix((i - 1) * 3, 14, Format(TI_upr_L(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 14, Format(DOM_Score(1.3, 0.4, TI_upr_L(i - 1), 4.0), "0.00"))
                '.set_TextMatrix((i - 1) * 3 + 2, 14, "-")
                Score_Arr(1) = Format(DOM_Score(1.3, 0.4, TI_upr_L(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 15, Format(TI_upr_R(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 15, Format(DOM_Score(1.3, 0.4, TI_upr_R(i - 1), 4.0), "0.00"))
                '.set_TextMatrix((i - 1) * 3 + 2, 15, "-")
                Score_Arr(2) = Format(DOM_Score(1.3, 0.4, TI_upr_R(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 16, Format(TI_lwr_L(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 16, Format(DOM_Score(1.3, 0.4, TI_lwr_L(i - 1), 4.0), "0.00"))
                '.set_TextMatrix((i - 1) * 3 + 2, 16, "-")
                Score_Arr(3) = Format(DOM_Score(1.3, 0.4, TI_lwr_L(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 17, Format(TI_lwr_R(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 17, Format(DOM_Score(1.3, 0.4, TI_lwr_R(i - 1), 4.0), "0.00"))
                '.set_TextMatrix((i - 1) * 3 + 2, 17, "-")
                Score_Arr(4) = Format(DOM_Score(1.3, 0.4, TI_lwr_R(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 18, Format(Tibia_Comp_L(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 18, Format(DOM_Score(8, 2, Tibia_Comp_L(i - 1), 4.0), "0.00"))
                '.set_TextMatrix((i - 1) * 3 + 2, 18, "-")
                Score_Arr(5) = Format(DOM_Score(8, 2, Tibia_Comp_L(i - 1), 4.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 19, Format(Tibia_Comp_R(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 19, Format(DOM_Score(8, 2, Tibia_Comp_R(i - 1), 4.0), "0.00"))
                Score_Arr(6) = Format(DOM_Score(8, 2, Tibia_Comp_R(i - 1), 4.0), "0.00")
                .set_TextMatrix((i - 1) * 3 + 2, 19, Format(DOM_min(Score_Arr, 6), "0.00"))

                .Row = (i - 1) * 3 + 2
                .Col = 14
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 19
                If DOM_min(Score_Arr, 6) >= 4.0 Then
                    .CellBackColor = Color.LightGreen
                ElseIf DOM_min(Score_Arr, 6) >= (4 * (2 / 3)) Then
                    .CellBackColor = Color.Yellow
                ElseIf DOM_min(Score_Arr, 6) >= (4 * (1 / 3)) Then
                    .CellBackColor = Color.FromArgb(255, 192, 0)
                ElseIf DOM_min(Score_Arr, 6) > 0.0 Then
                    .CellBackColor = Color.FromArgb(128, 96, 0)
                Else
                    .CellBackColor = Color.Red
                End If

                .Row = (i - 1) * 3 + 1
                .Col = 2
                .RowSel = (i - 1) * 3 + 1
                .ColSel = 19
                .CellFontBold = False
                .CellForeColor = Color.Gray

                .set_TextMatrix((i - 1) * 3, 20, Format(CSng(.get_TextMatrix((i - 1) * 3 + 2, 7)) _
                                 + CSng(.get_TextMatrix((i - 1) * 3 + 2, 9)) _
                                 + CSng(.get_TextMatrix((i - 1) * 3 + 2, 13)) _
                                 + CSng(.get_TextMatrix((i - 1) * 3 + 2, 19)), "0.0"))
                .set_TextMatrix((i - 1) * 3 + 1, 20, .get_TextMatrix((i - 1) * 3, 20))
                .set_TextMatrix((i - 1) * 3 + 2, 20, .get_TextMatrix((i - 1) * 3, 20))

                '이상 50% ======================================================================================================
            Next

        End With
    End Sub

    'C-NCAP Rear Frontal
    Private Sub Sheet_China_F_Rear()
        '상해 점수 시트 (중국 후석 정면)
        Dim i As Integer
        Dim file_tot As Integer
        file_tot = OpenFile.names.Length

        Me.Peak_Injury_CHINA_F_Rear.Rows = file_tot * 3
        Me.Peak_Injury_CHINA_F_Rear.Cols = 9

        With Me.Peak_Injury_CHINA_F_Rear

            .WordWrap = True
            .ScrollBars = MSFlexGridLib.ScrollBarsSettings.flexScrollBarVertical
            .FixedCols = 0
            .FixedRows = 0
            .MergeCells = 1
            .SelectionMode = MSFlexGridLib.SelectionModeSettings.flexSelectionFree
            .AllowBigSelection = True
            .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillRepeat

            For i = 0 To 8
                .set_ColAlignment(i, 4) '가운데 정렬
                .set_ColWidth(i, 1700)
            Next

            .set_ColWidth(0, 1650)

            .set_MergeCol(0, True)
            .set_MergeCol(7, True)
            .set_MergeCol(8, True)

            Dim Score_Arr(20) As Single
            Score_Arr(0) = -10

            For i = 1 To OpenFile.names.Length
                .set_RowHeight((i - 1) * 3, 250)
                .set_RowHeight((i - 1) * 3 + 1, 250)
                .set_RowHeight((i - 1) * 3 + 2, 250)

                '.set_MergeRow((i - 1) * 3 + 2, True)

                '헤더
                If DummyType(i - 1) = 0 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[50%]")
                ElseIf DummyType(i - 1) = 1 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[5%]")
                ElseIf DummyType(i - 1) = 2 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[THOR]")
                ElseIf DummyType(i - 1) = 3 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[Q-6]")
                ElseIf DummyType(i - 1) = 4 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[Q-10]")
                End If
                .set_TextMatrix((i - 1) * 3 + 1, 0, .get_TextMatrix((i - 1) * 3, 0))
                .set_TextMatrix((i - 1) * 3 + 2, 0, .get_TextMatrix((i - 1) * 3, 0))

                .set_TextMatrix((i - 1) * 3, 1, "Value")
                .set_TextMatrix((i - 1) * 3 + 1, 1, "Score")
                .set_TextMatrix((i - 1) * 3 + 2, 1, "Total")

                '50% 더미============================================================================================
                .set_TextMatrix((i - 1) * 3, 2, Format(HIC15(i - 1), "###0.0#"))
                .set_TextMatrix((i - 1) * 3 + 1, 2, Format(China_Rear_Score(700, 500, HIC15(i - 1), 0.8), "0.00"))
                .set_TextMatrix((i - 1) * 3 + 2, 2, "0.80" & "[" & Format(China_Rear_Score(700, 500, HIC15(i - 1), 0.8), "0.00") & "]")

                .set_TextMatrix((i - 1) * 3, 3, Format(Neck_Tens(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 3, Format(China_Rear_Score(2.62, 1.7, Neck_Tens(i - 1), 0.2), "0.00"))
                Score_Arr(1) = Format(China_Rear_Score(2.62, 1.7, Neck_Tens(i - 1), 0.2), "0.00")

                .set_TextMatrix((i - 1) * 3, 4, Format(Neck_Shear(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 4, Format(China_Rear_Score(1.95, 1.2, Neck_Shear(i - 1), 0.2), "0.00"))
                Score_Arr(2) = Format(China_Rear_Score(1.95, 1.2, Neck_Shear(i - 1), 0.2), "0.00")

                .set_TextMatrix((i - 1) * 3, 5, Format(Neck_Exten(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 5, Format(China_Rear_Score(49, 36, Neck_Exten(i - 1), 0.2), "0.00"))
                Score_Arr(3) = Format(China_Rear_Score(49, 36, Neck_Exten(i - 1), 0.2), "0.00")

                .set_TextMatrix((i - 1) * 3 + 2, 3, Format(China_Rear_Score(2.62, 1.7, Neck_Tens(i - 1), 0.2), "0.00") & _
                "[" & Format(DOM_min(Score_Arr, 3), "0.00") & "]")
                '.set_TextMatrix((i - 1) * 3 + 2, 4, .get_TextMatrix((i - 1) * 3 + 2, 3))
                '.set_TextMatrix((i - 1) * 3 + 2, 5, .get_TextMatrix((i - 1) * 3 + 2, 3))

                '가슴
                .set_TextMatrix((i - 1) * 3, 6, Format(Chest_D(i - 1), "###0.00"))
                ' 2015.12.23 변경 - 자릿수와 점수 동일하게
                '.set_TextMatrix((i - 1) * 3 + 1, 6, Format(China_Rear_Score(48, 23, Chest_D(i - 1), 1.0), "0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 6, Format(China_Rear_Score(48, 23, Format(Chest_D(i - 1), "###0.00"), 1.0), "0.00"))
                .set_TextMatrix((i - 1) * 3 + 2, 6, Format(China_Rear_Score(48, 23, Format(Chest_D(i - 1), "###0.00"), 1.0), "0.00"))

                '총점
                '2차 충격 없음
                .set_TextMatrix((i - 1) * 3, 7, Format(0.8 + .get_TextMatrix((i - 1) * 3 + 1, 3) + .get_TextMatrix((i - 1) * 3 + 1, 6), "0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 7, .get_TextMatrix((i - 1) * 3, 7))
                .set_TextMatrix((i - 1) * 3 + 2, 7, .get_TextMatrix((i - 1) * 3, 7))
                '2차 충격 있음
                .set_TextMatrix((i - 1) * 3, 8, Format(China_Rear_Score(700, 500, HIC15(i - 1), 0.8) + DOM_min(Score_Arr, 3) + .get_TextMatrix((i - 1) * 3 + 1, 6), "0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 8, .get_TextMatrix((i - 1) * 3, 8))
                .set_TextMatrix((i - 1) * 3 + 2, 8, .get_TextMatrix((i - 1) * 3, 8))

                .Row = (i - 1) * 3
                .Col = 7
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 8
                .CellBackColor = Color.White

                .Row = (i - 1) * 3
                .Col = 4
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 5
                .CellFontBold = False
                .CellFontSize = 8
                .CellForeColor = Color.Gray

                .Row = (i - 1) * 3
                .Col = 8
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 8
                .CellFontBold = False
                .CellFontSize = 8
                .CellForeColor = Color.Gray

                .Row = (i - 1) * 3 + 1
                .Col = 1
                .RowSel = (i - 1) * 3 + 1
                .ColSel = 6
                .CellFontBold = False
                .CellFontSize = 8
                .CellForeColor = Color.Gray
            Next

        End With
    End Sub

    'C-NCAP Rear Offset
    Private Sub Sheet_China_O_Rear()
        '상해 점수 시트 (중국 후석 옵셋)
        Dim i As Integer
        Dim file_tot As Integer
        file_tot = OpenFile.names.Length

        Me.Peak_Injury_CHINA_O_Rear.Rows = file_tot * 3
        Me.Peak_Injury_CHINA_O_Rear.Cols = 9

        With Me.Peak_Injury_CHINA_O_Rear

            .WordWrap = True
            .ScrollBars = MSFlexGridLib.ScrollBarsSettings.flexScrollBarVertical
            .FixedCols = 0
            .FixedRows = 0
            .MergeCells = 1
            .SelectionMode = MSFlexGridLib.SelectionModeSettings.flexSelectionFree
            .AllowBigSelection = True
            .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillRepeat

            For i = 0 To 8
                .set_ColAlignment(i, 4) '가운데 정렬
                .set_ColWidth(i, 1700)
            Next

            .set_ColWidth(0, 1650)

            .set_MergeCol(0, True)
            .set_MergeCol(7, True)
            .set_MergeCol(8, True)

            Dim Score_Arr(20) As Single
            Score_Arr(0) = -10

            For i = 1 To OpenFile.names.Length
                .set_RowHeight((i - 1) * 3, 250)
                .set_RowHeight((i - 1) * 3 + 1, 250)
                .set_RowHeight((i - 1) * 3 + 2, 250)

                '.set_MergeRow((i - 1) * 3 + 2, True)

                '헤더
                If DummyType(i - 1) = 0 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[50%]")
                ElseIf DummyType(i - 1) = 1 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[5%]")
                ElseIf DummyType(i - 1) = 2 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[THOR]")
                ElseIf DummyType(i - 1) = 3 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[Q-6]")
                ElseIf DummyType(i - 1) = 4 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[Q-10]")
                End If
                .set_TextMatrix((i - 1) * 3 + 1, 0, .get_TextMatrix((i - 1) * 3, 0))
                .set_TextMatrix((i - 1) * 3 + 2, 0, .get_TextMatrix((i - 1) * 3, 0))

                .set_TextMatrix((i - 1) * 3, 1, "Value")
                .set_TextMatrix((i - 1) * 3 + 1, 1, "Score")
                .set_TextMatrix((i - 1) * 3 + 2, 1, "Total")

                '50% 더미============================================================================================
                .set_TextMatrix((i - 1) * 3, 2, Format(HIC15(i - 1), "###0.0#"))
                .set_TextMatrix((i - 1) * 3 + 1, 2, Format(China_Rear_Score(700, 500, HIC15(i - 1), 1.0), "0.00"))
                .set_TextMatrix((i - 1) * 3 + 2, 2, "1.00" & "[" & Format(China_Rear_Score(700, 500, HIC15(i - 1), 1.0), "0.00") & "]")

                .set_TextMatrix((i - 1) * 3, 3, Format(Neck_Tens(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 3, Format(China_Rear_Score(2.62, 1.7, Neck_Tens(i - 1), 1.0), "0.00"))
                Score_Arr(1) = Format(China_Rear_Score(2.62, 1.7, Neck_Tens(i - 1), 1.0), "0.00")


                .set_TextMatrix((i - 1) * 3, 4, Format(Neck_Shear(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 4, Format(China_Rear_Score(1.95, 1.2, Neck_Shear(i - 1), 1.0), "0.00"))
                Score_Arr(2) = Format(China_Rear_Score(1.95, 1.2, Neck_Shear(i - 1), 1.0), "0.00")

                .set_TextMatrix((i - 1) * 3, 5, Format(Neck_Exten(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 5, Format(China_Rear_Score(49, 36, Neck_Exten(i - 1), 1.0), "0.00"))
                Score_Arr(3) = Format(China_Rear_Score(49, 36, Neck_Exten(i - 1), 1.0), "0.00")

                .set_TextMatrix((i - 1) * 3 + 2, 3, Format(China_Rear_Score(2.62, 1.7, Neck_Tens(i - 1), 1.0), "0.00") & _
                "[" & Format(DOM_min(Score_Arr, 3), "0.00") & "]")

                '가슴
                .set_TextMatrix((i - 1) * 3, 6, Format(Chest_D(i - 1), "###0.00"))
                ' 2015.12.23 변경 - 자릿수와 점수 동일하게
                '.set_TextMatrix((i - 1) * 3 + 1, 6, Format(China_Rear_Score(48, 23, Chest_D(i - 1), 1.0), "0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 6, Format(China_Rear_Score(48, 23, Format(Chest_D(i - 1), "###0.00"), 1.0), "0.00"))
                .set_TextMatrix((i - 1) * 3 + 2, 6, Format(China_Rear_Score(48, 23, Format(Chest_D(i - 1), "###0.00"), 1.0), "0.00"))

                '총점
                '2차 충격 없음
                If 1.0 > DOM_min(Score_Arr, 3) Then
                    .set_TextMatrix((i - 1) * 3, 7, Format(DOM_min(Score_Arr, 3) + .get_TextMatrix((i - 1) * 3 + 1, 6), "0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 7, .get_TextMatrix((i - 1) * 3, 7))
                    .set_TextMatrix((i - 1) * 3 + 2, 7, .get_TextMatrix((i - 1) * 3, 7))
                Else
                    .set_TextMatrix((i - 1) * 3, 7, Format(1.0 + .get_TextMatrix((i - 1) * 3 + 1, 6), "0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 7, .get_TextMatrix((i - 1) * 3, 7))
                    .set_TextMatrix((i - 1) * 3 + 2, 7, .get_TextMatrix((i - 1) * 3, 7))
                End If
                '2차 충격 있음
                Score_Arr(4) = Format(China_Rear_Score(700, 500, HIC15(i - 1), 1.0))
                .set_TextMatrix((i - 1) * 3, 8, Format(DOM_min(Score_Arr, 4) + .get_TextMatrix((i - 1) * 3 + 1, 6), "0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 8, .get_TextMatrix((i - 1) * 3, 8))
                .set_TextMatrix((i - 1) * 3 + 2, 8, .get_TextMatrix((i - 1) * 3, 8))

                .Row = (i - 1) * 3
                .Col = 7
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 8
                .CellBackColor = Color.White

                .Row = (i - 1) * 3
                .Col = 4
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 5
                .CellFontBold = False
                .CellFontSize = 8
                .CellForeColor = Color.Gray

                .Row = (i - 1) * 3
                .Col = 8
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 8
                .CellFontBold = False
                .CellFontSize = 8
                .CellForeColor = Color.Gray

                .Row = (i - 1) * 3 + 1
                .Col = 1
                .RowSel = (i - 1) * 3 + 1
                .ColSel = 6
                .CellFontBold = False
                .CellFontSize = 8
                .CellForeColor = Color.Gray
            Next

        End With
    End Sub

    'EuroNCAP Frontal Header
    Private Sub Sheet_Lbl7()
        'EuroNCAP Frontal
        Dim i As Integer

        With Me.InjuryLbl7
            .WordWrap = True
            .ScrollBars = MSFlexGridLib.ScrollBarsSettings.flexScrollBarNone

            .Rows = 5 '+ file_tot * 3
            .Cols = 13
            .FixedRows = 0
            .FixedCols = 0

            .FocusRect = MSFlexGridLib.FocusRectSettings.flexFocusNone
            .MergeCells = 1       '셀병합 허용

            .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillRepeat
            .Row = 1
            .Col = 2
            .RowSel = 2
            .ColSel = 11
            .CellFontBold = False
            .CellFontSize = 8
            .CellForeColor = Color.Gray

            For i = 0 To 12
                .set_MergeCol(i, True)
                .set_ColAlignment(i, 4) '가운데 정렬
                .set_ColWidth(i, 1190)
            Next

            .set_ColWidth(0, 1000)
            .set_ColWidth(12, 1250)

            .set_RowHeight(0, 250)
            .set_RowHeight(1, 250)
            .set_RowHeight(2, 250)
            .set_RowHeight(3, 250)
            .set_RowHeight(4, 250)

            .AllowBigSelection = True

            .set_MergeRow(0, True)
            .set_MergeRow(1, True)
            .set_MergeRow(2, True)
            .set_MergeRow(3, True)
            .set_MergeRow(4, True)

            .set_TextMatrix(0, 0, "Euro" & vbCrLf & "NCAP" & vbCrLf & vbCrLf & "Frontal")
            .set_TextMatrix(1, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(2, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(3, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(4, 0, .get_TextMatrix(0, 0))

            .set_TextMatrix(0, 1, "") '"구분")
            .set_TextMatrix(1, 1, "Upr")
            .set_TextMatrix(2, 1, "Lwr")
            .set_TextMatrix(3, 1, "Injury")
            .set_TextMatrix(4, 1, .get_TextMatrix(3, 1))

            .set_TextMatrix(0, 2, "Head")
            .set_TextMatrix(1, 2, "80")
            .set_TextMatrix(2, 2, "80")
            .set_TextMatrix(3, 2, "Peak" & vbCrLf & "G")
            .set_TextMatrix(4, 2, .get_TextMatrix(3, 2))

            .set_TextMatrix(0, 3, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 3, "80.0")
            .set_TextMatrix(2, 3, "72")
            .set_TextMatrix(3, 3, "3ms" & vbCrLf & "G")
            .set_TextMatrix(4, 3, .get_TextMatrix(3, 3))

            .set_TextMatrix(0, 4, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 4, "700")
            .set_TextMatrix(2, 4, "500")
            .set_TextMatrix(3, 4, "HIC15")
            .set_TextMatrix(4, 4, .get_TextMatrix(3, 4))

            .set_TextMatrix(0, 5, "Neck")
            .set_TextMatrix(1, 5, "1.95")
            .set_TextMatrix(2, 5, "1.2")
            .set_TextMatrix(3, 5, "Shear" & vbCrLf & "[kN]")
            .set_TextMatrix(4, 5, .get_TextMatrix(3, 5))
            .set_TextMatrix(0, 6, .get_TextMatrix(0, 5))
            .set_TextMatrix(1, 6, "2.62")
            .set_TextMatrix(2, 6, "1.7")
            .set_TextMatrix(3, 6, "Tens." & vbCrLf & "[kN]")
            .set_TextMatrix(4, 6, .get_TextMatrix(3, 6))
            .set_TextMatrix(0, 7, .get_TextMatrix(0, 5))
            .set_TextMatrix(1, 7, "49")
            .set_TextMatrix(2, 7, "36")
            .set_TextMatrix(3, 7, "Exten." & vbCrLf & "[Nm]")
            .set_TextMatrix(4, 7, .get_TextMatrix(3, 7))

            .set_TextMatrix(0, 8, "Chest")
            .set_TextMatrix(1, 8, "42")
            .set_TextMatrix(2, 8, "18")
            .set_TextMatrix(3, 8, "Disp" & vbCrLf & "[mm]")
            .set_TextMatrix(4, 8, .get_TextMatrix(3, 8))
            .set_TextMatrix(0, 9, .get_TextMatrix(0, 8))
            .set_TextMatrix(1, 9, "1.0")
            .set_TextMatrix(2, 9, "0.5")
            .set_TextMatrix(3, 9, "VC" & vbCrLf & "[m/s]")
            .set_TextMatrix(4, 9, .get_TextMatrix(3, 9))

            .set_TextMatrix(0, 10, "Femur")
            .set_TextMatrix(1, 10, "6.2")
            .set_TextMatrix(2, 10, "2.6")
            .set_TextMatrix(3, 10, "K.Peak [kN]")
            .set_TextMatrix(4, 10, "LH")
            .set_TextMatrix(0, 11, .get_TextMatrix(0, 10))
            .set_TextMatrix(1, 11, .get_TextMatrix(1, 10))
            .set_TextMatrix(2, 11, .get_TextMatrix(2, 10))
            .set_TextMatrix(3, 11, .get_TextMatrix(3, 10))
            .set_TextMatrix(4, 11, "RH")

            .set_TextMatrix(0, 12, "Score")
            .set_TextMatrix(1, 12, .get_TextMatrix(0, 12))
            .set_TextMatrix(2, 12, .get_TextMatrix(0, 12))
            .set_TextMatrix(3, 12, .get_TextMatrix(0, 12))
            .set_TextMatrix(4, 12, .get_TextMatrix(0, 12))

        End With
    End Sub

    'EuroNCAP Frontal Injury Sheet
    Private Sub Sheet_Euro_F()
        '상해 점수 시트 (유럽 정면)
        Dim i As Integer
        Dim file_tot As Integer
        file_tot = OpenFile.names.Length

        Me.Peak_Injury_Euro_F.Rows = file_tot * 3
        Me.Peak_Injury_Euro_F.Cols = 13

        With Me.Peak_Injury_Euro_F
            .WordWrap = True
            .ScrollBars = MSFlexGridLib.ScrollBarsSettings.flexScrollBarVertical
            .FixedCols = 0
            .FixedRows = 0
            .MergeCells = 1
            .SelectionMode = MSFlexGridLib.SelectionModeSettings.flexSelectionFree
            .AllowBigSelection = True
            .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillRepeat

            For i = 0 To 12
                .set_ColAlignment(i, 4) '가운데 정렬
                .set_ColWidth(i, 1190)
            Next

            .set_MergeCol(0, True)
            .set_MergeCol(1, True)
            .set_MergeCol(12, True)

            .set_ColWidth(0, 900)
            .set_ColWidth(12, 1200)

            .set_ColAlignment(12, 4) '가운데 정렬

            Dim Score_Arr(20) As Single
            Score_Arr(0) = -10

            For i = 1 To OpenFile.names.Length
                .set_RowHeight((i - 1) * 3, 250)
                .set_RowHeight((i - 1) * 3 + 1, 250)
                .set_RowHeight((i - 1) * 3 + 2, 250)

                '.set_MergeRow((i - 1) * 3 + 2, True)

                If DummyType(i - 1) = 0 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[50%]")
                ElseIf DummyType(i - 1) = 1 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[5%]")
                ElseIf DummyType(i - 1) = 2 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[THOR]")
                ElseIf DummyType(i - 1) = 3 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[Q-6]")
                ElseIf DummyType(i - 1) = 4 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[Q-10]")
                End If
                .set_TextMatrix((i - 1) * 3 + 1, 0, .get_TextMatrix((i - 1) * 3, 0))
                .set_TextMatrix((i - 1) * 3 + 2, 0, .get_TextMatrix((i - 1) * 3, 0))

                .set_TextMatrix((i - 1) * 3, 1, "Value")
                .set_TextMatrix((i - 1) * 3 + 1, 1, "Score")
                .set_TextMatrix((i - 1) * 3 + 2, 1, .get_TextMatrix((i - 1) * 3 + 1, 1))

                If DummyType(i - 1) = 0 Or DummyType(i - 1) = 2 Or DummyType(i - 1) = 1 _
                    Or DummyType(i - 1) = 3 Or DummyType(i - 1) = 4 Then '*****************************THOR 포함************
                    '50% 더미============================================================================================
                    .set_TextMatrix((i - 1) * 3, 2, Format(Head_Peak_G(i - 1), "###0.0#"))
                    .set_TextMatrix((i - 1) * 3 + 1, 2, "-")

                    .set_TextMatrix((i - 1) * 3, 3, Format(Head3MSG(i - 1), "###0.0#"))
                    .set_TextMatrix((i - 1) * 3 + 1, 3, Format(DOM_Score(88, 72, Head3MSG(i - 1), 4.0), "0.000"))
                    Score_Arr(2) = Format(DOM_Score(80, 72, Head3MSG(i - 1), 4.0), "0.000")

                    .set_TextMatrix((i - 1) * 3, 4, Format(HIC15(i - 1), "###0"))
                    .set_TextMatrix((i - 1) * 3 + 1, 4, Format(DOM_Score(700, 500, HIC15(i - 1), 4.0), "0.000"))
                    Score_Arr(3) = Format(DOM_Score(700, 500, HIC15(i - 1), 4.0), "0.000")

                    If Head_Peak_G(i - 1) > 80 Then
                        Score_Arr(1) = 0.0

                    Else
                        Score_Arr(1) = 4.0
                        Score_Arr(2) = 4.0
                        Score_Arr(3) = 4.0
                    End If

                    .set_TextMatrix((i - 1) * 3 + 2, 4, Format(DOM_min(Score_Arr, 3), "0.000"))

                    .Row = (i - 1) * 3 + 2
                    .Col = 2
                    .RowSel = (i - 1) * 3 + 2
                    .ColSel = 4

                    If DOM_min(Score_Arr, 3) >= 4.0 Then
                        .CellBackColor = Color.LightGreen
                    ElseIf DOM_min(Score_Arr, 3) >= 2.67 Then
                        .CellBackColor = Color.Yellow
                    ElseIf DOM_min(Score_Arr, 3) >= 1.33 Then
                        .CellBackColor = Color.FromArgb(255, 192, 0)
                    ElseIf DOM_min(Score_Arr, 3) > 0.0 Then
                        .CellBackColor = Color.FromArgb(128, 96, 0)
                    Else
                        .CellBackColor = Color.Red
                    End If

                    'Neck===
                    .set_TextMatrix((i - 1) * 3, 5, Format(Neck_Shear(i - 1), "###0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 5, Format(DOM_Score(1.95, 1.2, Neck_Shear(i - 1), 4.0), "0.000"))
                    Score_Arr(1) = Format(DOM_Score(1.95, 1.2, Neck_Shear(i - 1), 4.0), "0.000")

                    .set_TextMatrix((i - 1) * 3, 6, Format(Neck_Tens(i - 1), "###0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 6, Format(DOM_Score(2.62, 1.7, Neck_Tens(i - 1), 4.0), "0.000"))
                    Score_Arr(2) = Format(DOM_Score(2.62, 1.7, Neck_Tens(i - 1), 4.0), "0.000")

                    .set_TextMatrix((i - 1) * 3, 7, Format(Neck_Exten(i - 1), "###0.0"))
                    .set_TextMatrix((i - 1) * 3 + 1, 7, Format(DOM_Score(49, 36, Neck_Exten(i - 1), 4.0), "0.000"))
                    Score_Arr(3) = Format(DOM_Score(49, 36, Neck_Exten(i - 1), 4.0), "0.000")

                    .set_TextMatrix((i - 1) * 3 + 2, 7, Format(DOM_min(Score_Arr, 3), "0.000"))

                    .Row = (i - 1) * 3 + 2
                    .Col = 5
                    .RowSel = (i - 1) * 3 + 2
                    .ColSel = 7

                    If DOM_min(Score_Arr, 3) >= 4.0 Then
                        .CellBackColor = Color.LightGreen
                    ElseIf DOM_min(Score_Arr, 3) >= 2.67 Then
                        .CellBackColor = Color.Yellow
                    ElseIf DOM_min(Score_Arr, 3) >= 1.33 Then
                        .CellBackColor = Color.FromArgb(255, 192, 0)
                    ElseIf DOM_min(Score_Arr, 3) > 0.0 Then
                        .CellBackColor = Color.FromArgb(128, 96, 0)
                    Else
                        .CellBackColor = Color.Red
                    End If

                    'Chest=============================
                    .set_TextMatrix((i - 1) * 3, 8, Format(Chest_D(i - 1), "###0.0"))
                    ' 2015.12.23 변경 - 자릿수와 점수 동일하게
                    '.set_TextMatrix((i - 1) * 3 + 1, 8, Format(DOM_Score(42, 18, Chest_D(i - 1), 4.0), "0.000"))
                    .set_TextMatrix((i - 1) * 3 + 1, 8, Format(DOM_Score(42, 18, Format(Chest_D(i - 1), "###0.0"), 4.0), "0.000"))
                    'Score_Arr(1) = Format(DOM_Score(42, 18, Chest_D(i - 1), 4.0), "0.000")
                    Score_Arr(1) = Format(DOM_Score(42, 18, Format(Chest_D(i - 1), "###0.0"), 4.0), "0.000")

                    .set_TextMatrix((i - 1) * 3, 9, Format(Chest_VC(i - 1), "###0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 9, Format(DOM_Score(1.0, 0.5, Chest_VC(i - 1), 4.0), "0.000"))
                    Score_Arr(2) = Format(DOM_Score(1.0, 0.5, Chest_VC(i - 1), 4.0), "0.000")

                    .set_TextMatrix((i - 1) * 3 + 2, 9, Format(DOM_min(Score_Arr, 2), "0.000"))

                    .Row = (i - 1) * 3 + 2
                    .Col = 8
                    .RowSel = (i - 1) * 3 + 2
                    .ColSel = 9
                    If DOM_min(Score_Arr, 2) >= 4.0 Then
                        .CellBackColor = Color.LightGreen
                    ElseIf DOM_min(Score_Arr, 2) >= 2.67 Then
                        .CellBackColor = Color.Yellow
                    ElseIf DOM_min(Score_Arr, 2) >= 1.33 Then
                        .CellBackColor = Color.FromArgb(255, 192, 0)
                    ElseIf DOM_min(Score_Arr, 2) > 0.0 Then
                        .CellBackColor = Color.FromArgb(128, 96, 0)
                    Else
                        .CellBackColor = Color.Red
                    End If

                    '.set_TextMatrix((i - 1) * 3, 10, Format(KneeS_L(i - 1), "###0.0"))
                    '.set_TextMatrix((i - 1) * 3 + 1, 10, Format(DOM_Score(15.0, 6.0, KneeS_L(i - 1), 4.0), "0.00"))
                    'Score_Arr(1) = Format(DOM_Score(15.0, 6.0, KneeS_L(i - 1), 4.0), "0.00")

                    '.set_TextMatrix((i - 1) * 3, 11, Format(KneeS_R(i - 1), "###0.0"))
                    '.set_TextMatrix((i - 1) * 3 + 1, 11, Format(DOM_Score(15.0, 6.0, KneeS_R(i - 1), 4.0), "0.00"))
                    'Score_Arr(2) = Format(DOM_Score(15.0, 6.0, KneeS_R(i - 1), 4.0), "0.00")

                    .set_TextMatrix((i - 1) * 3, 10, Format(FemurL(i - 1), "###0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 10, Format(DOM_Score(6.2, 2.6, FemurL(i - 1), 4.0), "0.000"))
                    Score_Arr(1) = Format(DOM_Score(6.2, 2.6, FemurL(i - 1), 4.0), "0.000")

                    .set_TextMatrix((i - 1) * 3, 11, Format(FemurR(i - 1), "###0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 11, Format(DOM_Score(6.2, 2.6, FemurR(i - 1), 4.0), "0.000"))
                    Score_Arr(2) = Format(DOM_Score(6.2, 2.6, FemurR(i - 1), 4.0), "0.000")

                    .set_TextMatrix((i - 1) * 3 + 2, 11, Format(DOM_min(Score_Arr, 2), "0.000"))

                    .Row = (i - 1) * 3 + 2
                    .Col = 10
                    .RowSel = (i - 1) * 3 + 2
                    .ColSel = 11
                    If DOM_min(Score_Arr, 2) >= 4.0 Then
                        .CellBackColor = Color.LightGreen
                    ElseIf DOM_min(Score_Arr, 2) >= 2.67 Then
                        .CellBackColor = Color.Yellow
                    ElseIf DOM_min(Score_Arr, 2) >= 1.33 Then
                        .CellBackColor = Color.FromArgb(255, 192, 0)
                    ElseIf DOM_min(Score_Arr, 2) > 0.0 Then
                        .CellBackColor = Color.FromArgb(128, 96, 0)
                    Else
                        .CellBackColor = Color.Red
                    End If

                    .set_TextMatrix((i - 1) * 3, 12, Format(CSng(.get_TextMatrix((i - 1) * 3 + 2, 4)) _
                                     + CSng(.get_TextMatrix((i - 1) * 3 + 2, 7)) _
                                     + CSng(.get_TextMatrix((i - 1) * 3 + 2, 9)) _
                                     + CSng(.get_TextMatrix((i - 1) * 3 + 2, 11)), "0.000"))
                    .set_TextMatrix((i - 1) * 3 + 1, 12, .get_TextMatrix((i - 1) * 3, 12))
                    .set_TextMatrix((i - 1) * 3 + 2, 12, .get_TextMatrix((i - 1) * 3, 12))

                End If

                .Row = (i - 1) * 3 + 1
                .Col = 2
                .RowSel = (i - 1) * 3 + 1
                .ColSel = 11
                .CellFontBold = False
                .CellForeColor = Color.Gray
            Next

        End With
    End Sub

    'EuroNCAP Frontal Rear Seat
    Private Sub Sheet_Euro_Rear()
        'EuroNCAP Frontal Rear Seat
        '상해 점수 시트 (유럽 정면 - 후석 5%)
        Dim i As Integer
        Dim file_tot As Integer
        file_tot = OpenFile.names.Length

        Me.Peak_Injury_Euro_R.Rows = file_tot * 3
        Me.Peak_Injury_Euro_R.Cols = 13

        With Me.Peak_Injury_Euro_R
            .WordWrap = True
            .ScrollBars = MSFlexGridLib.ScrollBarsSettings.flexScrollBarVertical
            .FixedCols = 0
            .FixedRows = 0
            .MergeCells = 1
            .SelectionMode = MSFlexGridLib.SelectionModeSettings.flexSelectionFree
            .AllowBigSelection = True
            .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillRepeat

            For i = 0 To 12
                .set_ColAlignment(i, 4) '가운데 정렬
                .set_ColWidth(i, 1190)
            Next

            .set_MergeCol(0, True)
            .set_MergeCol(1, True)
            .set_MergeCol(12, True)

            .set_ColWidth(0, 900)
            .set_ColWidth(12, 1200)

            .set_ColAlignment(12, 4) '가운데 정렬

            Dim Score_Arr(20) As Single
            Score_Arr(0) = -10

            For i = 1 To OpenFile.names.Length
                .set_RowHeight((i - 1) * 3, 250)
                .set_RowHeight((i - 1) * 3 + 1, 250)
                .set_RowHeight((i - 1) * 3 + 2, 250)

                '.set_MergeRow((i - 1) * 3 + 2, True)

                If DummyType(i - 1) = 0 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[50%]")
                ElseIf DummyType(i - 1) = 1 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[5%]")
                ElseIf DummyType(i - 1) = 2 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[THOR]")
                ElseIf DummyType(i - 1) = 3 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[Q-6]")
                ElseIf DummyType(i - 1) = 4 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[Q-10]")
                End If
                .set_TextMatrix((i - 1) * 3 + 1, 0, .get_TextMatrix((i - 1) * 3, 0))
                .set_TextMatrix((i - 1) * 3 + 2, 0, .get_TextMatrix((i - 1) * 3, 0))

                .set_TextMatrix((i - 1) * 3, 1, "Value")
                .set_TextMatrix((i - 1) * 3 + 1, 1, "Score")
                .set_TextMatrix((i - 1) * 3 + 2, 1, .get_TextMatrix((i - 1) * 3 + 1, 1))

                If DummyType(i - 1) = 0 Or DummyType(i - 1) = 2 Or DummyType(i - 1) = 1 _
                    Or DummyType(i - 1) = 3 Or DummyType(i - 1) = 4 Then '*****************************THOR 포함************
                    '50% 더미============================================================================================
                    .set_TextMatrix((i - 1) * 3, 2, Format(Head_Peak_G(i - 1), "###0.0#"))
                    .set_TextMatrix((i - 1) * 3 + 1, 2, "-")

                    .set_TextMatrix((i - 1) * 3, 3, Format(Head3MSG(i - 1), "###0.0#"))
                    .set_TextMatrix((i - 1) * 3 + 1, 3, Format(DOM_Score(88, 72, Head3MSG(i - 1), 4.0), "0.000"))
                    Score_Arr(2) = Format(DOM_Score(80, 72, Head3MSG(i - 1), 4.0), "0.000")

                    .set_TextMatrix((i - 1) * 3, 4, Format(HIC15(i - 1), "###0"))
                    .set_TextMatrix((i - 1) * 3 + 1, 4, Format(DOM_Score(700, 500, HIC15(i - 1), 4.0), "0.000"))
                    Score_Arr(3) = Format(DOM_Score(700, 500, HIC15(i - 1), 4.0), "0.000")

                    If Head_Peak_G(i - 1) > 80 Then
                        Score_Arr(1) = 0.0

                    Else
                        Score_Arr(1) = 4.0
                        Score_Arr(2) = 4.0
                        Score_Arr(3) = 4.0
                    End If

                    .set_TextMatrix((i - 1) * 3 + 2, 4, Format(DOM_min(Score_Arr, 3), "0.000"))

                    .Row = (i - 1) * 3 + 2
                    .Col = 2
                    .RowSel = (i - 1) * 3 + 2
                    .ColSel = 4

                    If DOM_min(Score_Arr, 3) >= 4.0 Then
                        .CellBackColor = Color.LightGreen
                    ElseIf DOM_min(Score_Arr, 3) >= 2.67 Then
                        .CellBackColor = Color.Yellow
                    ElseIf DOM_min(Score_Arr, 3) >= 1.33 Then
                        .CellBackColor = Color.FromArgb(255, 192, 0)
                    ElseIf DOM_min(Score_Arr, 3) > 0.0 Then
                        .CellBackColor = Color.FromArgb(128, 96, 0)
                    Else
                        .CellBackColor = Color.Red
                    End If

                    'Neck===
                    .set_TextMatrix((i - 1) * 3, 5, Format(Neck_Shear(i - 1), "###0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 5, Format(DOM_Score(1.95, 1.2, Neck_Shear(i - 1), 1.0), "0.000"))
                    '.set_TextMatrix((i - 1) * 3 + 2, 5, .get_TextMatrix((i - 1) * 3 + 1, 5))
                    Score_Arr(3) = Format(DOM_Score(1.95, 1.2, Neck_Shear(i - 1), 1.0), "0.000")

                    '.Row = (i - 1) * 3 + 2
                    '.Col = 5
                    '.RowSel = (i - 1) * 3 + 2
                    '.ColSel = 5

                    'If DOM_min(Score_Arr, 1) >= 1.0 Then
                    '    .CellBackColor = Color.LightGreen
                    'ElseIf DOM_min(Score_Arr, 1) >= 0.67 Then
                    '    .CellBackColor = Color.Yellow
                    'ElseIf DOM_min(Score_Arr, 1) >= 0.33 Then
                    '    .CellBackColor = Color.FromArgb(255, 192, 0)
                    'ElseIf DOM_min(Score_Arr, 1) > 0.0 Then
                    '    .CellBackColor = Color.FromArgb(128, 96, 0)
                    'Else
                    '    .CellBackColor = Color.Red
                    'End If

                    .set_TextMatrix((i - 1) * 3, 6, Format(Neck_Tens(i - 1), "###0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 6, Format(DOM_Score(2.62, 1.7, Neck_Tens(i - 1), 1.0), "0.000"))
                    '.set_TextMatrix((i - 1) * 3 + 2, 6, .get_TextMatrix((i - 1) * 3 + 1, 6))
                    Score_Arr(2) = Format(DOM_Score(2.62, 1.7, Neck_Tens(i - 1), 1.0), "0.000")

                    '.Row = (i - 1) * 3 + 2
                    '.Col = 6
                    '.RowSel = (i - 1) * 3 + 2
                    '.ColSel = 6

                    'If DOM_min(Score_Arr, 1) >= 1.0 Then
                    '    .CellBackColor = Color.LightGreen
                    'ElseIf DOM_min(Score_Arr, 1) >= 0.67 Then
                    '    .CellBackColor = Color.Yellow
                    'ElseIf DOM_min(Score_Arr, 1) >= 0.33 Then
                    '    .CellBackColor = Color.FromArgb(255, 192, 0)
                    'ElseIf DOM_min(Score_Arr, 1) > 0.0 Then
                    '    .CellBackColor = Color.FromArgb(128, 96, 0)
                    'Else
                    '    .CellBackColor = Color.Red
                    'End If

                    .set_TextMatrix((i - 1) * 3, 7, Format(Neck_Exten(i - 1), "###0.0"))
                    .set_TextMatrix((i - 1) * 3 + 1, 7, Format(DOM_Score(49, 36, Neck_Exten(i - 1), 2.0), "0.000"))
                    '.set_TextMatrix((i - 1) * 3 + 2, 7, .get_TextMatrix((i - 1) * 3 + 1, 7))
                    Score_Arr(1) = Format(DOM_Score(49, 36, Neck_Exten(i - 1), 2.0), "0.000")
                    Score_Arr(1) = Score_Arr(1) + Score_Arr(2) + Score_Arr(3)

                    .set_TextMatrix((i - 1) * 3 + 2, 7, Format(Score_Arr(1), "0.000"))

                    .Row = (i - 1) * 3 + 2
                    .Col = 5
                    .RowSel = (i - 1) * 3 + 2
                    .ColSel = 7

                    If DOM_min(Score_Arr, 1) >= 4.0 Then
                        .CellBackColor = Color.LightGreen
                    ElseIf DOM_min(Score_Arr, 1) >= 2.67 Then
                        .CellBackColor = Color.Yellow
                    ElseIf DOM_min(Score_Arr, 1) >= 1.33 Then
                        .CellBackColor = Color.FromArgb(255, 192, 0)
                    ElseIf DOM_min(Score_Arr, 1) > 0.0 Then
                        .CellBackColor = Color.FromArgb(128, 96, 0)
                    Else
                        .CellBackColor = Color.Red
                    End If

                    'Chest=============================
                    .set_TextMatrix((i - 1) * 3, 8, Format(Chest_D(i - 1), "###0.0"))
                    ' 2015.12.23 변경 - 자릿수와 점수 동일하게
                    '.set_TextMatrix((i - 1) * 3 + 1, 8, Format(DOM_Score(42, 18, Chest_D(i - 1), 4.0), "0.000"))
                    .set_TextMatrix((i - 1) * 3 + 1, 8, Format(DOM_Score(42, 18, Format(Chest_D(i - 1), "###0.0"), 4.0), "0.000"))
                    'Score_Arr(1) = Format(DOM_Score(42, 18, Chest_D(i - 1), 4.0), "0.000")
                    Score_Arr(1) = Format(DOM_Score(42, 18, Format(Chest_D(i - 1), "###0.0"), 4.0), "0.000")

                    .set_TextMatrix((i - 1) * 3, 9, Format(Chest_VC(i - 1), "###0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 9, Format(DOM_Score(1.0, 0.5, Chest_VC(i - 1), 6.0), "0.000"))
                    Score_Arr(2) = Format(DOM_Score(1.0, 0.5, Chest_VC(i - 1), 6.0), "0.000")

                    .set_TextMatrix((i - 1) * 3 + 2, 9, Format(DOM_min(Score_Arr, 2), "0.000"))

                    .Row = (i - 1) * 3 + 2
                    .Col = 8
                    .RowSel = (i - 1) * 3 + 2
                    .ColSel = 9
                    If DOM_min(Score_Arr, 2) >= 4.0 Then
                        .CellBackColor = Color.LightGreen
                    ElseIf DOM_min(Score_Arr, 2) >= 2.67 Then
                        .CellBackColor = Color.Yellow
                    ElseIf DOM_min(Score_Arr, 2) >= 1.33 Then
                        .CellBackColor = Color.FromArgb(255, 192, 0)
                    ElseIf DOM_min(Score_Arr, 2) > 0.0 Then
                        .CellBackColor = Color.FromArgb(128, 96, 0)
                    Else
                        .CellBackColor = Color.Red
                    End If

                    '.set_TextMatrix((i - 1) * 3, 10, Format(KneeS_L(i - 1), "###0.0"))
                    '.set_TextMatrix((i - 1) * 3 + 1, 10, Format(DOM_Score(15.0, 6.0, KneeS_L(i - 1), 4.0), "0.00"))
                    'Score_Arr(1) = Format(DOM_Score(15.0, 6.0, KneeS_L(i - 1), 4.0), "0.00")

                    '.set_TextMatrix((i - 1) * 3, 11, Format(KneeS_R(i - 1), "###0.0"))
                    '.set_TextMatrix((i - 1) * 3 + 1, 11, Format(DOM_Score(15.0, 6.0, KneeS_R(i - 1), 4.0), "0.00"))
                    'Score_Arr(2) = Format(DOM_Score(15.0, 6.0, KneeS_R(i - 1), 4.0), "0.00")

                    .set_TextMatrix((i - 1) * 3, 10, Format(FemurL(i - 1), "###0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 10, Format(DOM_Score(6.2, 2.6, FemurL(i - 1), 4.0), "0.000"))
                    Score_Arr(1) = Format(DOM_Score(6.2, 2.6, FemurL(i - 1), 4.0), "0.000")

                    .set_TextMatrix((i - 1) * 3, 11, Format(FemurR(i - 1), "###0.00"))
                    .set_TextMatrix((i - 1) * 3 + 1, 11, Format(DOM_Score(6.2, 2.6, FemurR(i - 1), 4.0), "0.000"))
                    Score_Arr(2) = Format(DOM_Score(6.2, 2.6, FemurR(i - 1), 4.0), "0.000")

                    .set_TextMatrix((i - 1) * 3 + 2, 11, Format(DOM_min(Score_Arr, 2), "0.000"))

                    .Row = (i - 1) * 3 + 2
                    .Col = 10
                    .RowSel = (i - 1) * 3 + 2
                    .ColSel = 11
                    If DOM_min(Score_Arr, 2) >= 4.0 Then
                        .CellBackColor = Color.LightGreen
                    ElseIf DOM_min(Score_Arr, 2) >= 2.67 Then
                        .CellBackColor = Color.Yellow
                    ElseIf DOM_min(Score_Arr, 2) >= 1.33 Then
                        .CellBackColor = Color.FromArgb(255, 192, 0)
                    ElseIf DOM_min(Score_Arr, 2) > 0.0 Then
                        .CellBackColor = Color.FromArgb(128, 96, 0)
                    Else
                        .CellBackColor = Color.Red
                    End If

                    .set_TextMatrix((i - 1) * 3, 12, Format(CSng(.get_TextMatrix((i - 1) * 3 + 2, 4)) _
                                     + CSng(.get_TextMatrix((i - 1) * 3 + 2, 7)) _
                                     + CSng(.get_TextMatrix((i - 1) * 3 + 2, 9)) _
                                     + CSng(.get_TextMatrix((i - 1) * 3 + 2, 11)), "0.000"))
                    .set_TextMatrix((i - 1) * 3 + 1, 12, .get_TextMatrix((i - 1) * 3, 12))
                    .set_TextMatrix((i - 1) * 3 + 2, 12, .get_TextMatrix((i - 1) * 3, 12))

                End If

                .Row = (i - 1) * 3 + 1
                .Col = 2
                .RowSel = (i - 1) * 3 + 1
                .ColSel = 11
                .CellFontBold = False
                .CellForeColor = Color.Gray
            Next

        End With
    End Sub

    'EuroNCAP Offset Header
    Private Sub Sheet_Lbl8()
        '유럽 옵셋
        Dim i As Integer

        With Me.InjuryLbl8
            .WordWrap = True
            .ScrollBars = MSFlexGridLib.ScrollBarsSettings.flexScrollBarNone

            .Rows = 5 '+ file_tot * 3
            .Cols = 21
            .FixedRows = 0
            .FixedCols = 0

            .FocusRect = MSFlexGridLib.FocusRectSettings.flexFocusNone
            .MergeCells = 1       '셀병합 허용

            .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillRepeat
            .Row = 1
            .Col = 2
            .RowSel = 2
            .ColSel = 19
            .CellFontBold = False
            .CellFontSize = 8
            .CellForeColor = Color.Gray

            For i = 0 To 20
                .set_MergeCol(i, True)
                .set_ColAlignment(i, 4) '가운데 정렬
                .set_ColWidth(i, 730)
            Next
            For i = 4 To 7
                .set_ColWidth(i, 810)
            Next
            For i = 14 To 19
                .set_ColWidth(i, 570)
            Next

            .set_ColWidth(0, 730 + 200)
            .set_ColWidth(20, 1100)

            .set_RowHeight(0, 250)
            .set_RowHeight(1, 250)
            .set_RowHeight(2, 250)
            .set_RowHeight(3, 250)
            .set_RowHeight(4, 250)

            .AllowBigSelection = True

            .set_MergeRow(0, True)
            .set_MergeRow(1, True)
            .set_MergeRow(2, True)
            .set_MergeRow(3, True)
            .set_MergeRow(4, True)

            .set_TextMatrix(0, 0, "Euro" & vbCrLf & "NCAP" & vbCrLf & vbCrLf & "Offset")
            .set_TextMatrix(1, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(2, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(3, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(4, 0, .get_TextMatrix(0, 0))

            .set_TextMatrix(0, 1, "") '"구분")
            .set_TextMatrix(1, 1, "Upr.")
            .set_TextMatrix(2, 1, "Lwr.")
            .set_TextMatrix(3, 1, "Injury")
            .set_TextMatrix(4, 1, .get_TextMatrix(3, 1))

            .set_TextMatrix(0, 2, "Head & Neck")
            .set_TextMatrix(1, 2, "80")
            .set_TextMatrix(2, 2, "80")
            .set_TextMatrix(3, 2, "Peak" & vbCrLf & "G")
            .set_TextMatrix(4, 2, .get_TextMatrix(3, 2))

            .set_TextMatrix(0, 3, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 3, "80.0")
            .set_TextMatrix(2, 3, "72")
            .set_TextMatrix(3, 3, "3ms" & vbCrLf & "G")
            .set_TextMatrix(4, 3, .get_TextMatrix(3, 3))

            .set_TextMatrix(0, 4, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 4, "700")
            .set_TextMatrix(2, 4, "500")
            .set_TextMatrix(3, 4, "HIC15")
            .set_TextMatrix(4, 4, .get_TextMatrix(3, 4))

            .set_TextMatrix(0, 5, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 5, "3.1")
            .set_TextMatrix(2, 5, "1.9")
            .set_TextMatrix(3, 5, "Shear" & vbCrLf & "[kN]")
            .set_TextMatrix(4, 5, .get_TextMatrix(3, 5))
            .set_TextMatrix(0, 6, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 6, "3.3")
            .set_TextMatrix(2, 6, "2.7")
            .set_TextMatrix(3, 6, "Tens." & vbCrLf & "[kN]")
            .set_TextMatrix(4, 6, .get_TextMatrix(3, 6))
            .set_TextMatrix(0, 7, .get_TextMatrix(0, 2))
            .set_TextMatrix(1, 7, "57")
            .set_TextMatrix(2, 7, "42")
            .set_TextMatrix(3, 7, "Exten." & vbCrLf & "[Nm]")
            .set_TextMatrix(4, 7, .get_TextMatrix(3, 7))

            .set_TextMatrix(0, 8, "Chest")
            .set_TextMatrix(1, 8, "42")
            .set_TextMatrix(2, 8, "22")
            .set_TextMatrix(3, 8, "Disp" & vbCrLf & "[mm]")
            .set_TextMatrix(4, 8, .get_TextMatrix(3, 8))
            .set_TextMatrix(0, 9, .get_TextMatrix(0, 8))
            .set_TextMatrix(1, 9, "1.0")
            .set_TextMatrix(2, 9, "0.5")
            .set_TextMatrix(3, 9, "VC" & vbCrLf & "[m/s]")
            .set_TextMatrix(4, 9, .get_TextMatrix(3, 9))

            .set_TextMatrix(0, 10, "Femur/Knee")
            .set_TextMatrix(1, 10, "15")
            .set_TextMatrix(2, 10, "6")
            .set_TextMatrix(3, 10, "K.Slide [mm]")
            .set_TextMatrix(4, 10, "LH")
            .set_TextMatrix(0, 11, .get_TextMatrix(0, 10))
            .set_TextMatrix(1, 11, .get_TextMatrix(1, 10))
            .set_TextMatrix(2, 11, .get_TextMatrix(2, 10))
            .set_TextMatrix(3, 11, .get_TextMatrix(3, 10))
            .set_TextMatrix(4, 11, "RH")

            .set_TextMatrix(0, 12, .get_TextMatrix(0, 10))
            .set_TextMatrix(1, 12, "9.07")
            .set_TextMatrix(2, 12, "3.8")
            .set_TextMatrix(3, 12, "K.Peak [kN]")
            .set_TextMatrix(4, 12, "LH")
            .set_TextMatrix(0, 13, .get_TextMatrix(0, 10))
            .set_TextMatrix(1, 13, .get_TextMatrix(1, 12))
            .set_TextMatrix(2, 13, .get_TextMatrix(2, 12))
            .set_TextMatrix(3, 13, .get_TextMatrix(3, 12))
            .set_TextMatrix(4, 13, "RH")

            .set_TextMatrix(0, 14, "Tibia")
            .set_TextMatrix(1, 14, "1.3")
            .set_TextMatrix(2, 14, "0.4")
            .set_TextMatrix(3, 14, "TI - Upr")
            .set_TextMatrix(4, 14, "LH")
            .set_TextMatrix(0, 15, .get_TextMatrix(0, 14))
            .set_TextMatrix(1, 15, .get_TextMatrix(1, 14))
            .set_TextMatrix(2, 15, .get_TextMatrix(2, 14))
            .set_TextMatrix(3, 15, .get_TextMatrix(3, 14))
            .set_TextMatrix(4, 15, "RH")
            .set_TextMatrix(0, 16, .get_TextMatrix(0, 14))
            .set_TextMatrix(1, 16, .get_TextMatrix(1, 14))
            .set_TextMatrix(2, 16, .get_TextMatrix(2, 14))
            .set_TextMatrix(3, 16, "TI - Lwr")
            .set_TextMatrix(4, 16, "LH")
            .set_TextMatrix(0, 17, .get_TextMatrix(0, 14))
            .set_TextMatrix(1, 17, .get_TextMatrix(1, 14))
            .set_TextMatrix(2, 17, .get_TextMatrix(2, 14))
            .set_TextMatrix(3, 17, .get_TextMatrix(3, 16))
            .set_TextMatrix(4, 17, "RH")
            .set_TextMatrix(0, 18, .get_TextMatrix(0, 14))
            .set_TextMatrix(1, 18, "8")
            .set_TextMatrix(2, 18, "2")
            .set_TextMatrix(3, 18, "Comp.[kN]")
            .set_TextMatrix(4, 18, "LH")
            .set_TextMatrix(0, 19, .get_TextMatrix(0, 14))
            .set_TextMatrix(1, 19, .get_TextMatrix(1, 18))
            .set_TextMatrix(2, 19, .get_TextMatrix(2, 18))
            .set_TextMatrix(3, 19, .get_TextMatrix(3, 18))
            .set_TextMatrix(4, 19, "RH")
            .set_TextMatrix(0, 20, "Score")
            .set_TextMatrix(1, 20, .get_TextMatrix(0, 20))
            .set_TextMatrix(2, 20, .get_TextMatrix(0, 20))
            .set_TextMatrix(3, 20, .get_TextMatrix(0, 20))
            .set_TextMatrix(4, 20, .get_TextMatrix(0, 20))

        End With

    End Sub

    'EuroNCAP Offset Injury Sheet
    Private Sub Sheet_Euro_O()
        '상해 점수 시트 (유럽 옵셋)
        Dim i As Integer
        Dim file_tot As Integer
        file_tot = OpenFile.names.Length

        Me.Peak_Injury_Euro_O.Rows = file_tot * 3
        Me.Peak_Injury_Euro_O.Cols = 21

        With Me.Peak_Injury_Euro_O
            .WordWrap = True
            .ScrollBars = MSFlexGridLib.ScrollBarsSettings.flexScrollBarVertical
            .FixedCols = 0
            .FixedRows = 0
            .MergeCells = 1
            .SelectionMode = MSFlexGridLib.SelectionModeSettings.flexSelectionFree
            .AllowBigSelection = True
            .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillRepeat

            For i = 0 To 20
                .set_ColAlignment(i, 4) '가운데 정렬
                .set_ColWidth(i, 730)
            Next
            For i = 4 To 7
                .set_ColWidth(i, 810)
            Next
            For i = 14 To 19
                '.set_MergeCol(i, True)
                .set_ColWidth(i, 570)
            Next

            .set_ColWidth(0, 730 + 200)
            .set_ColWidth(20, 1100)

            .set_MergeCol(0, True)
            .set_MergeCol(1, True)
            .set_MergeCol(20, True)
            .set_ColAlignment(20, 4) '가운데 정렬

            Dim Score_Arr(20) As Single
            Score_Arr(0) = -10

            For i = 1 To OpenFile.names.Length
                .set_RowHeight((i - 1) * 3, 250)
                .set_RowHeight((i - 1) * 3 + 1, 250)
                .set_RowHeight((i - 1) * 3 + 2, 250)

                '.set_MergeRow((i - 1) * 3 + 2, True)

                If DummyType(i - 1) = 0 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[50%]")
                ElseIf DummyType(i - 1) = 1 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[5%]")
                ElseIf DummyType(i - 1) = 2 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #" & i & vbCrLf & "[THOR]")
                ElseIf DummyType(i - 1) = 3 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[Q-6]")
                ElseIf DummyType(i - 1) = 4 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[Q-10]")
                End If
                .set_TextMatrix((i - 1) * 3 + 1, 0, .get_TextMatrix((i - 1) * 3, 0))
                .set_TextMatrix((i - 1) * 3 + 2, 0, .get_TextMatrix((i - 1) * 3, 0))

                .set_TextMatrix((i - 1) * 3, 1, "Value")
                .set_TextMatrix((i - 1) * 3 + 1, 1, "Score")
                .set_TextMatrix((i - 1) * 3 + 2, 1, .get_TextMatrix((i - 1) * 3 + 1, 1))

                '50% 더미============================================================================================
                .set_TextMatrix((i - 1) * 3, 2, Format(Head_Peak_G(i - 1), "###0.0#"))
                .set_TextMatrix((i - 1) * 3 + 1, 2, "-")

                .set_TextMatrix((i - 1) * 3, 3, Format(Head3MSG(i - 1), "###0.0#"))
                .set_TextMatrix((i - 1) * 3 + 1, 3, "-")

                .set_TextMatrix((i - 1) * 3, 4, Format(HIC15(i - 1), "###0"))
                .set_TextMatrix((i - 1) * 3 + 1, 4, Format(DOM_Score(700, 500, HIC15(i - 1), 4.0), "0.000"))
                Score_Arr(1) = Format(DOM_Score(700, 500, HIC15(i - 1), 4.0), "0.000")

                .set_TextMatrix((i - 1) * 3, 5, Format(Neck_Shear(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 5, Format(DOM_Score(3.1, 1.9, Neck_Shear(i - 1), 4.0), "0.000"))
                Score_Arr(2) = Format(DOM_Score(3.1, 1.9, Neck_Shear(i - 1), 4.0), "0.000")

                .set_TextMatrix((i - 1) * 3, 6, Format(Neck_Tens(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 6, Format(DOM_Score(3.3, 2.7, Neck_Tens(i - 1), 4.0), "0.000"))
                Score_Arr(3) = Format(DOM_Score(3.3, 2.7, Neck_Tens(i - 1), 4.0), "0.000")

                .set_TextMatrix((i - 1) * 3, 7, Format(Neck_Exten(i - 1), "###0.0"))
                .set_TextMatrix((i - 1) * 3 + 1, 7, Format(DOM_Score(57, 42, Neck_Exten(i - 1), 4.0), "0.000"))
                Score_Arr(4) = Format(DOM_Score(57, 42, Neck_Exten(i - 1), 4.0), "0.000")

                .set_TextMatrix((i - 1) * 3 + 2, 7, Format(DOM_min(Score_Arr, 4), "0.000"))

                .Row = (i - 1) * 3 + 2
                .Col = 2
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 7

                If DOM_min(Score_Arr, 4) >= 4.0 Then
                    .CellBackColor = Color.LightGreen
                ElseIf DOM_min(Score_Arr, 4) >= 2.67 Then
                    .CellBackColor = Color.Yellow
                ElseIf DOM_min(Score_Arr, 4) >= 1.33 Then
                    .CellBackColor = Color.FromArgb(255, 192, 0)
                ElseIf DOM_min(Score_Arr, 4) > 0.0 Then
                    .CellBackColor = Color.FromArgb(128, 96, 0)
                Else
                    .CellBackColor = Color.Red
                End If

                .set_TextMatrix((i - 1) * 3, 8, Format(Chest_D(i - 1), "###0.0"))
                ' 2015.12.23 변경 - 자릿수와 점수 동일하게
                '.set_TextMatrix((i - 1) * 3 + 1, 8, Format(DOM_Score(42, 22, Chest_D(i - 1), 4.0), "0.000"))
                .set_TextMatrix((i - 1) * 3 + 1, 8, Format(DOM_Score(42, 22, Format(Chest_D(i - 1), "###0.0"), 4.0), "0.000"))
                'Score_Arr(1) = Format(DOM_Score(42, 22, Chest_D(i - 1), 4.0), "0.000")
                Score_Arr(1) = Format(DOM_Score(42, 22, Format(Chest_D(i - 1), "###0.0"), 4.0), "0.000")

                .set_TextMatrix((i - 1) * 3, 9, Format(Chest_VC(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 9, Format(DOM_Score(1.0, 0.5, Chest_VC(i - 1), 4.0), "0.000"))
                Score_Arr(2) = Format(DOM_Score(1.0, 0.5, Chest_VC(i - 1), 4.0), "0.000")
                .set_TextMatrix((i - 1) * 3 + 2, 9, Format(DOM_min(Score_Arr, 2), "0.000"))

                .Row = (i - 1) * 3 + 2
                .Col = 8
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 9
                If DOM_min(Score_Arr, 2) >= 4.0 Then
                    .CellBackColor = Color.LightGreen
                ElseIf DOM_min(Score_Arr, 2) >= 2.67 Then
                    .CellBackColor = Color.Yellow
                ElseIf DOM_min(Score_Arr, 2) >= 1.33 Then
                    .CellBackColor = Color.FromArgb(255, 192, 0)
                ElseIf DOM_min(Score_Arr, 2) > 0.0 Then
                    .CellBackColor = Color.FromArgb(128, 96, 0)
                Else
                    .CellBackColor = Color.Red
                End If

                .set_TextMatrix((i - 1) * 3, 10, Format(KneeS_L(i - 1), "###0.0"))
                .set_TextMatrix((i - 1) * 3 + 1, 10, Format(DOM_Score(15.0, 6.0, KneeS_L(i - 1), 4.0), "0.000"))
                Score_Arr(1) = Format(DOM_Score(15.0, 6.0, KneeS_L(i - 1), 4.0), "0.000")

                .set_TextMatrix((i - 1) * 3, 11, Format(KneeS_R(i - 1), "###0.0"))
                .set_TextMatrix((i - 1) * 3 + 1, 11, Format(DOM_Score(15.0, 6.0, KneeS_R(i - 1), 4.0), "0.000"))
                Score_Arr(2) = Format(DOM_Score(15.0, 6.0, KneeS_R(i - 1), 4.0), "0.000")

                .set_TextMatrix((i - 1) * 3, 12, Format(FemurL(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 12, Format(DOM_Score(9.07, 3.8, FemurL(i - 1), 4.0), "0.000"))
                Score_Arr(3) = Format(DOM_Score(9.07, 3.8, FemurL(i - 1), 4.0), "0.000")

                .set_TextMatrix((i - 1) * 3, 13, Format(FemurR(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 13, Format(DOM_Score(9.07, 3.8, FemurR(i - 1), 4.0), "0.000"))
                Score_Arr(4) = Format(DOM_Score(9.07, 3.8, FemurR(i - 1), 4.0), "0.000")
                .set_TextMatrix((i - 1) * 3 + 2, 13, Format(DOM_min(Score_Arr, 4), "0.000"))

                .Row = (i - 1) * 3 + 2
                .Col = 10
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 13
                If DOM_min(Score_Arr, 4) >= 4.0 Then
                    .CellBackColor = Color.LightGreen
                ElseIf DOM_min(Score_Arr, 4) >= 2.67 Then
                    .CellBackColor = Color.Yellow
                ElseIf DOM_min(Score_Arr, 4) >= 1.33 Then
                    .CellBackColor = Color.FromArgb(255, 192, 0)
                ElseIf DOM_min(Score_Arr, 4) > 0.0 Then
                    .CellBackColor = Color.FromArgb(128, 96, 0)
                Else
                    .CellBackColor = Color.Red
                End If

                .set_TextMatrix((i - 1) * 3, 14, Format(TI_upr_L(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 14, Format(DOM_Score(1.3, 0.4, TI_upr_L(i - 1), 4.0), "0.000"))
                '.set_TextMatrix((i - 1) * 3 + 2, 14, "-")
                Score_Arr(1) = Format(DOM_Score(1.3, 0.4, TI_upr_L(i - 1), 4.0), "0.000")

                .set_TextMatrix((i - 1) * 3, 15, Format(TI_upr_R(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 15, Format(DOM_Score(1.3, 0.4, TI_upr_R(i - 1), 4.0), "0.000"))
                '.set_TextMatrix((i - 1) * 3 + 2, 15, "-")
                Score_Arr(2) = Format(DOM_Score(1.3, 0.4, TI_upr_R(i - 1), 4.0), "0.000")

                .set_TextMatrix((i - 1) * 3, 16, Format(TI_lwr_L(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 16, Format(DOM_Score(1.3, 0.4, TI_lwr_L(i - 1), 4.0), "0.000"))
                '.set_TextMatrix((i - 1) * 3 + 2, 16, "-")
                Score_Arr(3) = Format(DOM_Score(1.3, 0.4, TI_lwr_L(i - 1), 4.0), "0.000")

                .set_TextMatrix((i - 1) * 3, 17, Format(TI_lwr_R(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 17, Format(DOM_Score(1.3, 0.4, TI_lwr_R(i - 1), 4.0), "0.000"))
                '.set_TextMatrix((i - 1) * 3 + 2, 17, "-")
                Score_Arr(4) = Format(DOM_Score(1.3, 0.4, TI_lwr_R(i - 1), 4.0), "0.000")

                .set_TextMatrix((i - 1) * 3, 18, Format(Tibia_Comp_L(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 18, Format(DOM_Score(8, 2, Tibia_Comp_L(i - 1), 4.0), "0.000"))
                '.set_TextMatrix((i - 1) * 3 + 2, 18, "-")
                Score_Arr(5) = Format(DOM_Score(8, 2, Tibia_Comp_L(i - 1), 4.0), "0.000")

                .set_TextMatrix((i - 1) * 3, 19, Format(Tibia_Comp_R(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 19, Format(DOM_Score(8, 2, Tibia_Comp_R(i - 1), 4.0), "0.000"))
                Score_Arr(6) = Format(DOM_Score(8, 2, Tibia_Comp_R(i - 1), 4.0), "0.000")
                .set_TextMatrix((i - 1) * 3 + 2, 19, Format(DOM_min(Score_Arr, 6), "0.000"))

                .Row = (i - 1) * 3 + 2
                .Col = 14
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 19
                If DOM_min(Score_Arr, 6) >= 4.0 Then
                    .CellBackColor = Color.LightGreen
                ElseIf DOM_min(Score_Arr, 6) >= 2.67 Then
                    .CellBackColor = Color.Yellow
                ElseIf DOM_min(Score_Arr, 6) >= 1.33 Then
                    .CellBackColor = Color.FromArgb(255, 192, 0)
                ElseIf DOM_min(Score_Arr, 6) > 0.0 Then
                    .CellBackColor = Color.FromArgb(128, 96, 0)
                Else
                    .CellBackColor = Color.Red
                End If

                .Row = (i - 1) * 3 + 1
                .Col = 2
                .RowSel = (i - 1) * 3 + 1
                .ColSel = 19
                .CellFontBold = False
                .CellForeColor = Color.Gray

                .set_TextMatrix((i - 1) * 3, 20, Format(CSng(.get_TextMatrix((i - 1) * 3 + 2, 7)) _
                                 + CSng(.get_TextMatrix((i - 1) * 3 + 2, 9)) _
                                 + CSng(.get_TextMatrix((i - 1) * 3 + 2, 13)) _
                                 + CSng(.get_TextMatrix((i - 1) * 3 + 2, 19)), "0.000"))
                .set_TextMatrix((i - 1) * 3 + 1, 20, .get_TextMatrix((i - 1) * 3, 20))
                .set_TextMatrix((i - 1) * 3 + 2, 20, .get_TextMatrix((i - 1) * 3, 20))

                '이상 50% ======================================================================================================
            Next

        End With

    End Sub

#End Region

    Private Sub InjuryDisplayFrm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        'If Me.Width <> 1100 Then
        Me.Width = 1100
        'Me.SplitConMain.Dock = DockStyle.Fill
        'Exit Sub
        'End If
        With Me
            '.Size = New Size(1100, 600)
            '.Width = 1100
            .PathLbl.Location = New Point(10, 20)
            .OpenLst.Location = New Point(12, .PathLbl.Location.Y + .PathLbl.Height + 10)
            'If Me.OpenDlg.FileNames(0) = "" Then
            '    .OpenLst.Size = New Size(.Width - 60, 21 * 4)
            'Else
            '    .OpenLst.Size = New Size(.Width - 60, 21 * UBound(Me.OpenDlg.FileNames) + 21)
            'End If


            .OpenLst.Size = New Size(.SplitConMain.Panel1.ClientRectangle.Width - .OpenLst.Location.X * 2 - 1 - 300, .SplitConMain.Panel1.ClientRectangle.Height - .PathLbl.Height - 30)
            .DescripTxt.Location = New Point(.OpenLst.Location.X + .OpenLst.Width + 5, .OpenLst.Location.Y)
            .DescripTxt.Size = New Size(295, .OpenLst.Height)

            '.ScoreBoard.Location = New Point(20, .OpenLst.Location.Y + .OpenLst.Height + 10)
            .ScoreBoard.Location = New Point(.OpenLst.Location.X, 10)
            .ScoreBoard.Size = New Size(.OpenLst.Width + 300, .SplitConMain.Panel2.ClientRectangle.Height - 60)

            .BtnInjuryGraph.Location = New Point(.SplitConMain.Panel2.ClientRectangle.Width - 205, .ScoreBoard.Location.Y + .ScoreBoard.Height + 10)
            .BtnInjuryGraph.Size = New Size(165, 30)

            .BtnValGraph.Location = New Point(.BtnInjuryGraph.Location.X - 5 - 165, .BtnInjuryGraph.Location.Y)
            .BtnValGraph.Size = New Size(165, 30)

            .PPTBtn.Location = New Point(.BtnValGraph.Location.X - 5 - 165, .BtnValGraph.Location.Y)
            .PPTBtn.Size = New Size(165, 30)

            .ReloadBtn.Location = New Point(.ScoreBoard.Location.X, .PPTBtn.Location.Y)
            .ReloadBtn.Size = New Size(85, 30)

            .Button1.Location = New Point(.ReloadBtn.Location.X + .ReloadBtn.Width + 5, .ReloadBtn.Location.Y)
            .Button1.Size = New Size(85, 30)

            '북미 NCAP
            .InjuryLbl1.Location = New Point(10, 10)
            .InjuryLbl1.Size = New Size(.ScoreBoard.Width - 30, 90)
            .Peak_Injury1.Location = New Point(10, .InjuryLbl1.Height + .InjuryLbl1.Location.Y + 3)
            .Peak_Injury1.Size = New Size(.InjuryLbl1.Width, .ScoreBoard.Height - .InjuryLbl1.Height - 46)

            '내수 정면
            .InjuryLbl2.Location = New Point(10, 10)
            .InjuryLbl2.Size = New Size(.ScoreBoard.Width - 30, 90)
            .Peak_Injury_DOM_F.Location = New Point(10, .InjuryLbl2.Height + .InjuryLbl2.Location.Y + 3)
            .Peak_Injury_DOM_F.Size = New Size(.InjuryLbl2.Width, .ScoreBoard.Height - .InjuryLbl2.Height - 46)

            '내수 옵셋
            .InjuryLbl3.Location = New Point(10, 10)
            .InjuryLbl3.Size = New Size(.ScoreBoard.Width - 30, 90)
            .Peak_Injury_DOM_O.Location = New Point(10, .InjuryLbl3.Height + .InjuryLbl3.Location.Y + 3)
            .Peak_Injury_DOM_O.Size = New Size(.InjuryLbl3.Width, .ScoreBoard.Height - .InjuryLbl3.Height - 46)

            '중국 정면
            .InjuryLbl4.Location = New Point(10, 10)
            .InjuryLbl4.Size = New Size(.ScoreBoard.Width - 30, 90)
            .Peak_Injury_CHINA_F.Location = New Point(10, .InjuryLbl4.Height + .InjuryLbl4.Location.Y + 3)
            .Peak_Injury_CHINA_F.Size = New Size(.InjuryLbl4.Width, .ScoreBoard.Height - .InjuryLbl4.Height - 46)

            '중국 옵셋
            .InjuryLbl5.Location = New Point(10, 10)
            .InjuryLbl5.Size = New Size(.ScoreBoard.Width - 30, 90)
            .Peak_Injury_CHINA_O.Location = New Point(10, .InjuryLbl5.Height + .InjuryLbl5.Location.Y + 3)
            .Peak_Injury_CHINA_O.Size = New Size(.InjuryLbl5.Width, .ScoreBoard.Height - .InjuryLbl5.Height - 46)

            '중국 후석 (정면)
            .InjuryLbl6.Location = New Point(10, 10)
            .InjuryLbl6.Size = New Size(.ScoreBoard.Width - 30, 90)
            '.Peak_Injury_CHINA_F_Rear.Location = New Point(10, .InjuryLbl6.Height + .InjuryLbl6.Location.Y + 3)
            '.Peak_Injury_CHINA_F_Rear.Size = New Size(.InjuryLbl6.Width, .ScoreBoard.Height - .InjuryLbl6.Height - 46)
            .CNCAP_Rear.Location = New Point(10, .InjuryLbl6.Height + .InjuryLbl6.Location.Y + 3)
            .CNCAP_Rear.Size = New Size(.InjuryLbl6.Width, .ScoreBoard.Height - .InjuryLbl6.Height - 46 - 30)
            .BtnFind.Location = New Point(.CNCAP_Rear.Location.X + .CNCAP_Rear.Width - 115, .CNCAP_Rear.Location.Y + .CNCAP_Rear.Height)
            .BtnFind.Size = New Size(115, 30)

            '유럽 정면
            .InjuryLbl7.Location = New Point(10, 10)
            .InjuryLbl7.Size = New Size(.ScoreBoard.Width - 30, 90)
            .EuroFrontal.Location = New Point(10, .InjuryLbl7.Height + .InjuryLbl7.Location.Y + 3)
            .EuroFrontal.Size = New Size(.InjuryLbl7.Width, .ScoreBoard.Height - .InjuryLbl7.Height - 46)

            '유럽 옵셋
            .InjuryLbl8.Location = New Point(10, 10)
            .InjuryLbl8.Size = New Size(.ScoreBoard.Width - 30, 90)
            .Peak_Injury_Euro_O.Location = New Point(10, .InjuryLbl8.Height + .InjuryLbl8.Location.Y + 3)
            .Peak_Injury_Euro_O.Size = New Size(.InjuryLbl8.Width, .ScoreBoard.Height - .InjuryLbl8.Height - 46)
        End With

    End Sub

    Private Sub BtnValGraph_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnValGraph.Click

        Dim InValGraph As New FrmInjuryValGraph(Head3MSG, Chest_G_CUMULATIVE_T3MS_inj, HIC15, HIC36, NTE, NTF, NCE, NCF, Head_Peak_G, _
                                                Chest_D, KneeS_L, KneeS_R, Tibia_Comp_L, Tibia_Comp_R, TI_upr_L, TI_lwr_L, TI_upr_R, TI_lwr_R, _
                                                Chest_VC, FemurL, FemurR, Neck_Comp, Neck_Tens, Neck_Shear, Neck_Exten, Star_Rating, _
                                                ThxIrUpL, ThxIrUpR, ThxIrLowL, ThxIrLowR)

        With InValGraph
            .Owner = Me
            .InjuryLst.Items.Insert(0, "Head 3ms Clip")
            .InjuryLst.Items.Insert(1, "Chest 3ms Clip")
            .InjuryLst.Items.Insert(2, "HIC15")
            .InjuryLst.Items.Insert(3, "HIC36")
            .InjuryLst.Items.Insert(4, "NTE")
            .InjuryLst.Items.Insert(5, "NTF")
            .InjuryLst.Items.Insert(6, "NCE")
            .InjuryLst.Items.Insert(7, "NCF")
            .InjuryLst.Items.Insert(8, "Head Peak G")
            .InjuryLst.Items.Insert(9, "CD")
            .InjuryLst.Items.Insert(10, "Knee Slide (L)")
            .InjuryLst.Items.Insert(11, "Knee Slide (R)")
            .InjuryLst.Items.Insert(12, "Tibia Comp (L)")
            .InjuryLst.Items.Insert(13, "Tibia Comp (R)")
            .InjuryLst.Items.Insert(14, "Tibia Index Upper (L)")
            .InjuryLst.Items.Insert(15, "Tibia Index Lower (L)")
            .InjuryLst.Items.Insert(16, "Tibia Index Upper (R)")
            .InjuryLst.Items.Insert(17, "Tibia Index Lower (R)")
            .InjuryLst.Items.Insert(18, "Chest VC")
            .InjuryLst.Items.Insert(19, "Femur (L)")
            .InjuryLst.Items.Insert(20, "Femur (R)")
            .InjuryLst.Items.Insert(21, "Neck Comp.")
            .InjuryLst.Items.Insert(22, "Neck Tens.")
            .InjuryLst.Items.Insert(23, "Neck Shear")
            .InjuryLst.Items.Insert(24, "Neck Exten.")
            .InjuryLst.Items.Insert(25, "Star Rating")
            .InjuryLst.Items.Insert(26, "Thorax Upper Left")
            .InjuryLst.Items.Insert(27, "Thorax Upper Right")
            .InjuryLst.Items.Insert(28, "Thorax Lower Left")
            .InjuryLst.Items.Insert(29, "Thorax Lower Right")

            .InjuryValChrt.Hide()

            .Show()

        End With
    End Sub

    Private Sub BtnInjuryGraph_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnInjuryGraph.Click
        MainMDI.ProgressBarMain.Value = 0
        MainMDI.ProgressBarMain.Maximum = (UBound(OpenFile.names) + 1) * 310 + 500

        'Dim FrmInjuryGraphing As New FrmInjuryGraph(FilePathGet(Me.OpenDlg.FileNames), OpenFile.names, ParaMeterArr1, ParaMeterArr2, ConversionFactor)
        Dim FrmInjuryGraphing As New FrmInjuryGraph(Tmp_Path, OpenFile.names, ParaMeterArr1, ParaMeterArr2, ConversionFactor, GraphBased, DummyType)

        FrmInjuryGraphing.Owner = Me
        FrmInjuryGraphing.Show()
        If InStr(Me.Text, "Correlation Profile") > 1 Then
            Dim TmpStr() As String
            TmpStr = Split(Me.Text, ":")
            FrmInjuryGraphing.Text = "Plotting - Correlation Profile : " & TmpStr(UBound(TmpStr))
        Else
            FrmInjuryGraphing.Text = Me.Text & " - " & "Plotting"
        End If
    End Sub

#Region "PPT Export"

    Private Sub PPTBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PPTBtn.Click

        Select Case Me.ScoreBoard.SelectedIndex
            Case 0
                PPTexportUSNCAP()
            Case 1
                PPTexportDOMfrontal()
            Case 2
                PPTexportDOMoffset()
            Case 3
                PPTexportChinafrontal()
            Case 4
                PPTexportChinaOffset()
            Case 5
                Select Case Me.CNCAP_Rear.SelectedIndex
                    Case 0
                        PPTexportChinafrontalRear()
                    Case 1
                        PPTexportChinaoffsetRear()
                End Select
            Case 6
                Select Case Me.EuroFrontal.SelectedIndex
                    Case 0
                        PPTexportEuroFrontal_F()
                    Case 1
                        PPTexportEuroFrontal_R()
                End Select
            Case 7
                PPTexportEuroOffset()
            Case Else

        End Select

        '================================================================================================
        Try
            Using NewfileNum As StreamWriter = File.AppendText(RemoteValFolder & System.Environment.UserName)
                NewfileNum.WriteLine("##")
                NewfileNum.WriteLine("## Export Table - Case # " & Me.ScoreBoard.SelectedIndex)
                NewfileNum.Close()
            End Using
        Catch ex As Exception

        End Try
        '================================================================================================

    End Sub

    Private Sub PPTexportUSNCAP()

        '파워포인트를 시작한다. (프로그램만 구동)
        objPPT = New PowerPoint.Application
        objPPT.Visible = MsoTriState.msoTrue
        objPPT.WindowState = PowerPoint.PpWindowState.ppWindowMaximized

        'Presentation을 추가한다.
        objPres = objPPT.Presentations.Add(MsoTriState.msoTrue)

        '슬라이드를 추가한다.=====================================================
        Dim objSlide As PowerPoint.Slide
        Dim objCustomLayout As PowerPoint.CustomLayout

        'EnsurePowerPointIsRunning(True)
        'Create a custom layout based on the first layout in the slide master.
        'This is used simply for creating the slide
        objCustomLayout = objPres.SlideMaster.CustomLayouts.Item(1)
        'Create slide
        objSlide = objPres.Slides.AddSlide(1, objCustomLayout)
        'Set the layout
        'objSlide.Layout = PowerPoint.PpSlideLayout.ppLayoutText
        objSlide.Layout = PowerPoint.PpSlideLayout.ppLayoutBlank
        'Clean up
        objCustomLayout.Delete()
        objCustomLayout = Nothing
        objSlide = Nothing

        '========제목 제거==================================================== (전부 제거하는 방법은 모르겠다;;)
        For i = 1 To objPres.Slides(1).Shapes.Count
            If objPres.Slides(1).Shapes(i).HasTextFrame Then
                objPres.Slides(1).Shapes(i).Delete()
                Exit For
            End If
        Next i
        '=========================================================================

        '테이블을 추가한다.=======================================================
        Dim objShape As PowerPoint.Shape
        Dim objTable As PowerPoint.Table

        '해석파일명을 추가한다.=======================================================================
        Dim strText As String = ""
        objPres.Slides(1).Select()
        objShape = objPres.Slides(1).Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 10, 5, 700, 300)
        objShape.TextFrame.AutoSize = PowerPoint.PpAutoSize.ppAutoSizeShapeToFitText
        For i = 1 To Tot_File
            strText = strText & "MD " & i & ". " & OpenFile.names(i - 1) & vbCrLf
        Next
        objShape.TextFrame.TextRange.Text = strText

        objShape.TextEffect.FontSize = 10
        objShape.TextEffect.FontBold = MsoTriState.msoTrue
        '============================================================================================

        'Add a table into the first slide in the presentation
        Dim Tot_Row As Integer = 5 + 2 * (Tot_File)
        Dim Tot_Col As Integer = 15 '북미 시트
        objPres.Slides(1).Select()

        objShape = objPres.Slides(1).Shapes.AddTable(1, 1, 10, 80, 700)
        objTable = objShape.Table
        With objShape.Table
            .Cell(1, 1).Shape.Fill.ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Bold = MsoTriState.msoTrue
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Size = 9
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            .Cell(1, 1).Shape.TextFrame.VerticalAnchor = MsoVerticalAnchor.msoAnchorMiddle
            .Cell(1, 1).Shape.TextFrame.HorizontalAnchor = MsoHorizontalAnchor.msoAnchorCenter
            '.Cell(nRow, nCol).Shape.TextFrame.TextRange.Text = Me.InjuryLbl1.get_TextMatrix(nRow - 1, nCol - 1)
            .Cell(1, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(1, 1).Shape.TextFrame.MarginTop = 0
            .Cell(1, 1).Shape.TextFrame.MarginLeft = 0
            .Cell(1, 1).Shape.TextFrame.MarginRight = 0
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).Weight = 1
            For i = 1 To 4
                .Rows.Add()
            Next
            .Rows.Add()
            .Rows(1).Height = 12
            .Rows(2).Height = 10
            .Rows(3).Height = 10
            .Rows(4).Height = 10
            .Rows(5).Height = 10
            .Rows(6).Height = 15
            .Cell(6, 1).Shape.Fill.ForeColor.RGB = RGB(Color.White.R, Color.White.G, Color.White.B)
            .Cell(6, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            .Cell(6, 1).Shape.TextFrame.TextRange.Font.Size = 9
            .Cell(6, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            .Cell(6, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(6, 1).Shape.TextFrame.MarginTop = 0
            .Cell(6, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(6, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(6, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(6, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            For i = 1 To 14
                .Columns.Add()
            Next
            For i = 2 To Tot_File * 2
                .Rows.Add()
                .Rows(i + 5).Height = 15
            Next

            '셀병합 및 조절
            .Cell(1, 1).Merge(MergeTo:=.Cell(5, 1))
            .Cell(1, 4).Merge(MergeTo:=.Cell(1, 9))
            .Cell(2, 6).Merge(MergeTo:=.Cell(2, 9))
            .Cell(3, 6).Merge(MergeTo:=.Cell(3, 9))
            .Cell(1, 10).Merge(MergeTo:=.Cell(1, 11))
            .Cell(1, 12).Merge(MergeTo:=.Cell(1, 13))
            .Cell(2, 12).Merge(MergeTo:=.Cell(2, 13))
            .Cell(3, 12).Merge(MergeTo:=.Cell(3, 13))
            .Cell(4, 12).Merge(MergeTo:=.Cell(4, 13))
            .Cell(1, 14).Merge(MergeTo:=.Cell(5, 14))
            .Cell(1, 15).Merge(MergeTo:=.Cell(5, 15))
            .Cell(4, 2).Merge(MergeTo:=.Cell(5, 2))
            .Cell(4, 3).Merge(MergeTo:=.Cell(5, 3))
            .Cell(4, 4).Merge(MergeTo:=.Cell(5, 4))
            .Cell(4, 5).Merge(MergeTo:=.Cell(5, 5))
            .Cell(4, 6).Merge(MergeTo:=.Cell(5, 6))
            .Cell(4, 7).Merge(MergeTo:=.Cell(5, 7))
            .Cell(4, 8).Merge(MergeTo:=.Cell(5, 8))
            .Cell(4, 9).Merge(MergeTo:=.Cell(5, 9))
            .Cell(4, 10).Merge(MergeTo:=.Cell(5, 10))
            .Cell(4, 11).Merge(MergeTo:=.Cell(5, 11))

            '라벨을 쓴다==============================================================================
            .Cell(1, 1).Shape.TextFrame.TextRange.Text = "US-" & vbCrLf & "NCAP"
            .Cell(1, 2).Shape.TextFrame.TextRange.Text = "구분"
            .Cell(2, 2).Shape.TextFrame.TextRange.Text = "법규"
            .Cell(3, 2).Shape.TextFrame.TextRange.Text = "개발 목표"
            .Cell(4, 2).Shape.TextFrame.TextRange.Text = "상해"
            .Cell(1, 3).Shape.TextFrame.TextRange.Text = "머리"
            .Cell(2, 3).Shape.TextFrame.TextRange.Text = "700"
            .Cell(3, 3).Shape.TextFrame.TextRange.Text = "560"
            .Cell(4, 3).Shape.TextFrame.TextRange.Text = "HIC15"
            .Cell(1, 4).Shape.TextFrame.TextRange.Text = "목"
            .Cell(2, 4).Shape.TextFrame.TextRange.Text = "4.17(2.62)"
            .Cell(3, 4).Shape.TextFrame.TextRange.Text = "3.34(2.10)"
            .Cell(4, 4).Shape.TextFrame.TextRange.Text = "Tens." & vbCrLf & "[kN]"
            .Cell(2, 5).Shape.TextFrame.TextRange.Text = "4.0(2.52)"
            .Cell(3, 5).Shape.TextFrame.TextRange.Text = "3.2(2.02)"
            .Cell(4, 5).Shape.TextFrame.TextRange.Text = "Comp." & vbCrLf & "[kN]"
            .Cell(2, 6).Shape.TextFrame.TextRange.Text = "1.0"
            .Cell(3, 6).Shape.TextFrame.TextRange.Text = "0.8"
            .Cell(4, 6).Shape.TextFrame.TextRange.Text = "NTE"
            .Cell(4, 7).Shape.TextFrame.TextRange.Text = "NTF"
            .Cell(4, 8).Shape.TextFrame.TextRange.Text = "NCE"
            .Cell(4, 9).Shape.TextFrame.TextRange.Text = "NCF"

            .Cell(1, 10).Shape.TextFrame.TextRange.Text = "가슴"
            .Cell(2, 10).Shape.TextFrame.TextRange.Text = "63(52)"
            .Cell(3, 10).Shape.TextFrame.TextRange.Text = "50.4(41.6)"
            .Cell(4, 10).Shape.TextFrame.TextRange.Text = "Disp." & vbCrLf & "[mm]"

            .Cell(2, 11).Shape.TextFrame.TextRange.Text = "60.0"
            .Cell(3, 11).Shape.TextFrame.TextRange.Text = "48.0"
            .Cell(4, 11).Shape.TextFrame.TextRange.Text = "3ms" & vbCrLf & "[G]"

            .Cell(1, 12).Shape.TextFrame.TextRange.Text = "무릎"
            .Cell(2, 12).Shape.TextFrame.TextRange.Text = "10.0(6.81)"
            .Cell(3, 12).Shape.TextFrame.TextRange.Text = "8.0(5.44)"
            .Cell(4, 12).Shape.TextFrame.TextRange.Text = "Peak [kN]"
            .Cell(5, 12).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(5, 13).Shape.TextFrame.TextRange.Text = "RH"

            .Cell(1, 14).Shape.TextFrame.TextRange.Text = "P-Aver." & vbCrLf & "RRS"
            .Cell(1, 15).Shape.TextFrame.TextRange.Text = "Rating" & vbCrLf & "[★]"
            '========================================================================================

            For nRow = 6 To Tot_Row Step 2
                'For i = 1 To Tot_File
                '    .Cell(5 + i * 3 - 2, 1).Merge(MergeTo:=.Cell(5 + i * 3 - 1, 1))
                '    .Cell(5 + i * 3 - 2, 15).Merge(MergeTo:=.Cell(5 + i * 3 - 1, 15))
                'Next
                .Cell(nRow, 1).Merge(MergeTo:=.Cell(nRow + 1, 1))
                .Cell(nRow, 15).Merge(MergeTo:=.Cell(nRow + 1, 15))
                For nCol = 1 To Tot_Col
                    .Cell(nRow, nCol).Shape.TextFrame.TextRange.Text = Me.Peak_Injury1.get_TextMatrix(((nRow - 6) / 2) * 3, nCol - 1)
                    .Cell(nRow + 1, nCol).Shape.TextFrame.TextRange.Text = Me.Peak_Injury1.get_TextMatrix(((nRow - 6) / 2) * 3 + 1, nCol - 1)
                    '.Cell(nRow + 1, nCol).Shape.TextFrame.TextRange.Text = Me.Peak_Injury1.get_TextMatrix(((nRow - 6) / 2) * 3 + 2, nCol - 1)
                Next
            Next
        End With

        '
        'Clean up
        objTable = Nothing
        objShape = Nothing
    End Sub

    Private Sub PPTexportDOMfrontal()
        '파워포인트를 시작한다. (프로그램만 구동)
        objPPT = New PowerPoint.Application
        objPPT.Visible = MsoTriState.msoTrue
        objPPT.WindowState = PowerPoint.PpWindowState.ppWindowMaximized

        'Presentation을 추가한다.
        objPres = objPPT.Presentations.Add(MsoTriState.msoTrue)

        '슬라이드를 추가한다.=====================================================
        Dim objSlide As PowerPoint.Slide
        Dim objCustomLayout As PowerPoint.CustomLayout

        'EnsurePowerPointIsRunning(True)
        'Create a custom layout based on the first layout in the slide master.
        'This is used simply for creating the slide
        objCustomLayout = objPres.SlideMaster.CustomLayouts.Item(1)
        'Create slide
        objSlide = objPres.Slides.AddSlide(1, objCustomLayout)
        'Set the layout
        'objSlide.Layout = PowerPoint.PpSlideLayout.ppLayoutText
        objSlide.Layout = PowerPoint.PpSlideLayout.ppLayoutBlank
        'Clean up
        objCustomLayout.Delete()
        objCustomLayout = Nothing
        objSlide = Nothing

        '========제목 제거==================================================== (전부 제거하는 방법은 모르겠다;;)
        For i = 1 To objPres.Slides(1).Shapes.Count
            If objPres.Slides(1).Shapes(i).HasTextFrame Then
                objPres.Slides(1).Shapes(i).Delete()
                'Exit For
            End If
        Next i
        '=========================================================================

        '테이블을 추가한다.=======================================================
        Dim objShape As PowerPoint.Shape
        Dim objTable As PowerPoint.Table

        '해석파일명을 추가한다.=======================================================================
        Dim strText As String = ""
        objPres.Slides(1).Select()
        objShape = objPres.Slides(1).Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 10, 5, 700, 300)
        objShape.TextFrame.AutoSize = PowerPoint.PpAutoSize.ppAutoSizeShapeToFitText
        For i = 1 To Tot_File
            strText = strText & "MD " & i & ". " & OpenFile.names(i - 1) & vbCrLf
        Next
        objShape.TextFrame.TextRange.Text = strText

        objShape.TextEffect.FontSize = 10
        objShape.TextEffect.FontBold = MsoTriState.msoTrue
        '============================================================================================

        'Add a table into the first slide in the presentation
        Dim Tot_Row As Integer = 3 + 3 * (Tot_File)
        Dim Tot_Col As Integer = 11 '내수정면 시트
        objPres.Slides(1).Select()

        objShape = objPres.Slides(1).Shapes.AddTable(1, 1, 10, 50, 700)
        objTable = objShape.Table
        With objShape.Table
            .Cell(1, 1).Shape.Fill.ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Bold = MsoTriState.msoTrue
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Size = 9
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            '.Cell(nRow, nCol).Shape.TextFrame.TextRange.Text = Me.InjuryLbl1.get_TextMatrix(nRow - 1, nCol - 1)
            .Cell(1, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(1, 1).Shape.TextFrame.MarginTop = 0
            .Cell(1, 1).Shape.TextFrame.MarginLeft = 0
            .Cell(1, 1).Shape.TextFrame.MarginRight = 0
            .Cell(1, 1).Shape.TextFrame.VerticalAnchor = MsoVerticalAnchor.msoAnchorMiddle
            .Cell(1, 1).Shape.TextFrame.HorizontalAnchor = MsoHorizontalAnchor.msoAnchorCenter
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).Weight = 1
            For i = 1 To 2
                .Rows.Add()
            Next
            .Rows.Add()
            .Rows(1).Height = 12
            .Rows(2).Height = 10
            .Rows(3).Height = 10
            .Rows(4).Height = 10
            .Cell(4, 1).Shape.Fill.ForeColor.RGB = RGB(Color.White.R, Color.White.G, Color.White.B)
            .Cell(4, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            .Cell(4, 1).Shape.TextFrame.TextRange.Font.Size = 9
            .Cell(4, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            .Cell(4, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(4, 1).Shape.TextFrame.MarginTop = 0
            .Cell(4, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(4, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(4, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(4, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            For i = 1 To 10
                .Columns.Add()
            Next
            For i = 2 To Tot_File * 3
                .Rows.Add()
                .Rows(i + 3).Height = 10
            Next
            '셀병합 및 조절
            .Cell(1, 1).Merge(MergeTo:=.Cell(3, 2))
            .Cell(1, 4).Merge(MergeTo:=.Cell(1, 6))
            .Cell(1, 7).Merge(MergeTo:=.Cell(1, 8))
            .Cell(1, 9).Merge(MergeTo:=.Cell(1, 10))
            .Cell(2, 9).Merge(MergeTo:=.Cell(2, 10))
            .Cell(1, 11).Merge(MergeTo:=.Cell(3, 11))
            '.Cell(1, 13).Merge(MergeTo:=.Cell(3, 13))
            For i = 3 To 8
                .Cell(2, i).Merge(MergeTo:=.Cell(3, i))
            Next
            For i = 1 To Tot_File
                .Cell(3 * i + 1, 1).Merge(MergeTo:=.Cell(3 * i + 3, 1))
                .Cell(3 * i + 2, 2).Merge(MergeTo:=.Cell(3 * i + 3, 2))
                .Cell(3 * i + 1, 11).Merge(MergeTo:=.Cell(3 * i + 3, 11))
            Next

            '라벨을 쓴다==============================================================================
            .Cell(1, 1).Shape.TextFrame.TextRange.Text = "KNCAP" & vbCrLf & "정면"
            .Cell(1, 3).Shape.TextFrame.TextRange.Text = "머리"
            .Cell(2, 3).Shape.TextFrame.TextRange.Text = "HIC36" & vbCrLf & "HIC15"
            .Cell(1, 4).Shape.TextFrame.TextRange.Text = "목"
            .Cell(2, 4).Shape.TextFrame.TextRange.Text = "Shear" & vbCrLf & "[kN]"
            .Cell(2, 5).Shape.TextFrame.TextRange.Text = "Tens." & vbCrLf & "[kN]"
            .Cell(2, 6).Shape.TextFrame.TextRange.Text = "Exten." & vbCrLf & "[Nm]"
            .Cell(1, 7).Shape.TextFrame.TextRange.Text = "가슴"
            .Cell(2, 7).Shape.TextFrame.TextRange.Text = "변위" & vbCrLf & "[mm]"
            .Cell(2, 8).Shape.TextFrame.TextRange.Text = "VC" & vbCrLf & "[m/s]"
            .Cell(1, 9).Shape.TextFrame.TextRange.Text = "무릎"
            '.Cell(2, 9).Shape.TextFrame.TextRange.Text = "변위 [mm]"
            '.Cell(3, 9).Shape.TextFrame.TextRange.Text = "LH"
            '.Cell(3, 10).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(2, 9).Shape.TextFrame.TextRange.Text = "하중 [kN]"
            .Cell(3, 9).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 10).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(1, 11).Shape.TextFrame.TextRange.Text = "Total" & vbCrLf & "Score"

            For nRow = 4 To Tot_Row Step 3

                .Cell(nRow + 2, 3).Merge(MergeTo:=.Cell(nRow + 2, 6))
                .Cell(nRow + 2, 7).Merge(MergeTo:=.Cell(nRow + 2, 8))
                .Cell(nRow + 2, 9).Merge(MergeTo:=.Cell(nRow + 2, 10))
                For nCol = 3 To 8
                    .Cell(nRow, nCol).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_F.get_TextMatrix(nRow - 4, nCol + 1)
                    .Cell(nRow + 1, nCol).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_F.get_TextMatrix(nRow - 3, nCol + 1)
                Next
                For nCol = 9 To 10
                    .Cell(nRow, nCol).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_F.get_TextMatrix(nRow - 4, nCol + 1 + 2)
                    .Cell(nRow + 1, nCol).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_F.get_TextMatrix(nRow - 3, nCol + 1 + 2)
                Next
                '상해쓰기 (위치가 다르다)=================================
                '.Cell(nRow, 3).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_F.get_TextMatrix(nRow - 4, 4)  'HIC
                '.Cell(nRow, 4).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_F.get_TextMatrix(nRow - 4, 5)  'Shear
                '.Cell(nRow, 5).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_F.get_TextMatrix(nRow - 4, 6)  'Tens
                '.Cell(nRow, 6).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_F.get_TextMatrix(nRow - 4, 7)  'Exten.
                '.Cell(nRow, 7).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_F.get_TextMatrix(nRow - 4, 8)  'CD
                '.Cell(nRow, 8).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_F.get_TextMatrix(nRow - 4, 9)  'VC
                '========================================================
                .Cell(nRow, 1).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_F.get_TextMatrix(nRow - 4, 0) 'CASE 이름
                .Cell(nRow, 2).Shape.TextFrame.TextRange.Text = "Value"
                .Cell(nRow + 2, 2).Shape.TextFrame.TextRange.Text = "Score"
                .Cell(nRow, 11).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_F.get_TextMatrix(nRow - 4, 14) '종합점수
                '개별점수
                .Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_F.get_TextMatrix(nRow - 4 + 2, 7)
                .Cell(nRow + 2, 7).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_F.get_TextMatrix(nRow - 4 + 2, 9)
                .Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_F.get_TextMatrix(nRow - 4 + 2, 13)
                '색칠한다.
                If CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) >= 6.0 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) >= 2.0 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If

                If CSng(.Cell(nRow + 2, 7).Shape.TextFrame.TextRange.Text) >= 6.0 Then
                    .Cell(nRow + 2, 7).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 7).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 7).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 7).Shape.TextFrame.TextRange.Text) >= 2.0 Then
                    .Cell(nRow + 2, 7).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 7).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 7).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 7).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If

                If CSng(.Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text) >= 2.67 Then
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text) >= 1.33 Then
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If
            Next

        End With
    End Sub

    Private Sub PPTexportDOMoffset()
        '파워포인트를 시작한다. (프로그램만 구동)
        objPPT = New PowerPoint.Application
        objPPT.Visible = MsoTriState.msoTrue
        objPPT.WindowState = PowerPoint.PpWindowState.ppWindowMaximized

        'Presentation을 추가한다.
        objPres = objPPT.Presentations.Add(MsoTriState.msoTrue)

        '슬라이드를 추가한다.=====================================================
        Dim objSlide As PowerPoint.Slide
        Dim objCustomLayout As PowerPoint.CustomLayout

        'EnsurePowerPointIsRunning(True)
        'Create a custom layout based on the first layout in the slide master.
        'This is used simply for creating the slide
        objCustomLayout = objPres.SlideMaster.CustomLayouts.Item(1)
        'Create slide
        objSlide = objPres.Slides.AddSlide(1, objCustomLayout)
        'Set the layout
        'objSlide.Layout = PowerPoint.PpSlideLayout.ppLayoutText
        objSlide.Layout = PowerPoint.PpSlideLayout.ppLayoutBlank
        'Clean up
        objCustomLayout.Delete()
        objCustomLayout = Nothing
        objSlide = Nothing

        '========제목 제거==================================================== (전부 제거하는 방법은 모르겠다;;)
        For i = 1 To objPres.Slides(1).Shapes.Count
            If objPres.Slides(1).Shapes(i).HasTextFrame Then
                objPres.Slides(1).Shapes(i).Delete()
                Exit For
            End If
        Next i
        '=========================================================================

        '테이블을 추가한다.=======================================================
        Dim objShape As PowerPoint.Shape
        Dim objTable As PowerPoint.Table

        '해석파일명을 추가한다.=======================================================================
        Dim strText As String = ""
        objPres.Slides(1).Select()
        objShape = objPres.Slides(1).Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 10, 5, 700, 300)
        objShape.TextFrame.AutoSize = PowerPoint.PpAutoSize.ppAutoSizeShapeToFitText
        For i = 1 To Tot_File
            strText = strText & "MD " & i & ". " & OpenFile.names(i - 1) & vbCrLf
        Next
        objShape.TextFrame.TextRange.Text = strText

        objShape.TextEffect.FontSize = 10
        objShape.TextEffect.FontBold = MsoTriState.msoTrue
        '============================================================================================

        'Add a table into the first slide in the presentation
        Dim Tot_Row As Integer = 3 + 3 * (Tot_File)
        Dim Tot_Col As Integer = 19 '내수정면 시트
        objPres.Slides(1).Select()

        objShape = objPres.Slides(1).Shapes.AddTable(1, 1, 10, 50, 700)
        objTable = objShape.Table
        With objShape.Table
            .Cell(1, 1).Shape.Fill.ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Bold = MsoTriState.msoTrue
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Size = 9
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            '.Cell(nRow, nCol).Shape.TextFrame.TextRange.Text = Me.InjuryLbl1.get_TextMatrix(nRow - 1, nCol - 1)
            .Cell(1, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(1, 1).Shape.TextFrame.MarginTop = 0
            .Cell(1, 1).Shape.TextFrame.MarginLeft = 0
            .Cell(1, 1).Shape.TextFrame.MarginRight = 0
            .Cell(1, 1).Shape.TextFrame.VerticalAnchor = MsoVerticalAnchor.msoAnchorMiddle
            .Cell(1, 1).Shape.TextFrame.HorizontalAnchor = MsoHorizontalAnchor.msoAnchorCenter
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).Weight = 1
            For i = 1 To 2
                .Rows.Add()
            Next
            .Rows.Add()
            .Rows(1).Height = 12
            .Rows(2).Height = 10
            .Rows(3).Height = 10
            .Rows(4).Height = 10
            .Cell(4, 1).Shape.Fill.ForeColor.RGB = RGB(Color.White.R, Color.White.G, Color.White.B)
            .Cell(4, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            .Cell(4, 1).Shape.TextFrame.TextRange.Font.Size = 9
            .Cell(4, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            .Cell(4, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(4, 1).Shape.TextFrame.MarginTop = 0
            .Cell(4, 1).Shape.TextFrame.MarginLeft = 0
            .Cell(4, 1).Shape.TextFrame.MarginRight = 0
            .Cell(4, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(4, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(4, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(4, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            For i = 1 To 18
                .Columns.Add()
            Next

            For i = 2 To Tot_File * 3
                .Rows.Add()
                .Rows(i + 3).Height = 10
            Next

            '셀병합 및 조절
            .Cell(1, 1).Merge(MergeTo:=.Cell(3, 2))
            .Cell(1, 4).Merge(MergeTo:=.Cell(1, 6))
            .Cell(1, 7).Merge(MergeTo:=.Cell(1, 8))
            .Cell(1, 9).Merge(MergeTo:=.Cell(1, 12))
            .Cell(1, 13).Merge(MergeTo:=.Cell(1, 18))

            For i = 9 To 17 Step 2
                .Cell(2, i).Merge(MergeTo:=.Cell(2, i + 1))
            Next
            .Cell(1, 19).Merge(MergeTo:=.Cell(3, 19))
            For i = 3 To 8
                .Cell(2, i).Merge(MergeTo:=.Cell(3, i))
            Next

            For i = 1 To Tot_File
                .Cell(3 * i + 1, 1).Merge(MergeTo:=.Cell(3 * i + 3, 1))
                .Cell(3 * i + 2, 2).Merge(MergeTo:=.Cell(3 * i + 3, 2))
                .Cell(3 * i + 1, 19).Merge(MergeTo:=.Cell(3 * i + 3, 19))
            Next

            '라벨을 쓴다==============================================================================
            .Cell(1, 1).Shape.TextFrame.TextRange.Text = "KNCAP" & vbCrLf & "옵셋"
            .Cell(1, 3).Shape.TextFrame.TextRange.Text = "머리"
            .Cell(2, 3).Shape.TextFrame.TextRange.Text = "HIC36"
            .Cell(1, 4).Shape.TextFrame.TextRange.Text = "목"
            .Cell(2, 4).Shape.TextFrame.TextRange.Text = "Shear" & vbCrLf & "[kN]"
            .Cell(2, 5).Shape.TextFrame.TextRange.Text = "Tens." & vbCrLf & "[kN]"
            .Cell(2, 6).Shape.TextFrame.TextRange.Text = "Exten." & vbCrLf & "[Nm]"
            .Cell(1, 7).Shape.TextFrame.TextRange.Text = "가슴"
            .Cell(2, 7).Shape.TextFrame.TextRange.Text = "변위" & vbCrLf & "[mm]"
            .Cell(2, 8).Shape.TextFrame.TextRange.Text = "VC" & vbCrLf & "[m/s]"
            .Cell(1, 9).Shape.TextFrame.TextRange.Text = "무릎"
            .Cell(2, 9).Shape.TextFrame.TextRange.Text = "변위 [mm]"
            .Cell(3, 9).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 10).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(2, 11).Shape.TextFrame.TextRange.Text = "하중 [kN]"
            .Cell(3, 11).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 12).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(1, 13).Shape.TextFrame.TextRange.Text = "하지"
            .Cell(2, 13).Shape.TextFrame.TextRange.Text = "TI(Upr)"
            .Cell(3, 13).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 14).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(2, 15).Shape.TextFrame.TextRange.Text = "TI(Lwr)"
            .Cell(3, 15).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 16).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(2, 17).Shape.TextFrame.TextRange.Text = "Comp[kN]"
            .Cell(3, 17).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 18).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(1, 19).Shape.TextFrame.TextRange.Text = "Total" & vbCrLf & "Score"

            For nRow = 4 To Tot_Row Step 3

                .Cell(nRow + 2, 3).Merge(MergeTo:=.Cell(nRow + 2, 6))
                .Cell(nRow + 2, 7).Merge(MergeTo:=.Cell(nRow + 2, 8))
                .Cell(nRow + 2, 9).Merge(MergeTo:=.Cell(nRow + 2, 12))
                .Cell(nRow + 2, 13).Merge(MergeTo:=.Cell(nRow + 2, 18))
                For nCol = 3 To Tot_Col - 1
                    .Cell(nRow, nCol).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_O.get_TextMatrix(nRow - 4, nCol + 1)
                    .Cell(nRow + 1, nCol).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_O.get_TextMatrix(nRow - 3, nCol + 1)
                Next
                '========================================================
                .Cell(nRow, 1).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_O.get_TextMatrix(nRow - 4, 0) 'CASE 이름
                .Cell(nRow, 2).Shape.TextFrame.TextRange.Text = "Value"
                .Cell(nRow + 2, 2).Shape.TextFrame.TextRange.Text = "Score"
                .Cell(nRow, 19).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_O.get_TextMatrix(nRow - 4, 20) '종합점수
                '개별점수
                .Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_O.get_TextMatrix(nRow - 4 + 2, 7)
                .Cell(nRow + 2, 7).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_O.get_TextMatrix(nRow - 4 + 2, 9)
                .Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_O.get_TextMatrix(nRow - 4 + 2, 13)
                .Cell(nRow + 2, 13).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_DOM_O.get_TextMatrix(nRow - 4 + 2, 19)
                '색칠한다.
                If CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) >= 2.67 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) >= 1.33 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If

                If CSng(.Cell(nRow + 2, 7).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 7).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 7).Shape.TextFrame.TextRange.Text) >= 2.67 Then
                    .Cell(nRow + 2, 7).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 7).Shape.TextFrame.TextRange.Text) >= 1.33 Then
                    .Cell(nRow + 2, 7).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 7).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 7).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 7).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If

                If CSng(.Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text) >= 2.67 Then
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text) >= 1.33 Then
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If

                If CSng(.Cell(nRow + 2, 13).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 13).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 13).Shape.TextFrame.TextRange.Text) >= 2.67 Then
                    .Cell(nRow + 2, 13).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 13).Shape.TextFrame.TextRange.Text) >= 1.33 Then
                    .Cell(nRow + 2, 13).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 13).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 13).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 13).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If
            Next

        End With
    End Sub

    Private Sub PPTexportChinafrontal()
        '파워포인트를 시작한다. (프로그램만 구동)
        objPPT = New PowerPoint.Application
        objPPT.Visible = MsoTriState.msoTrue
        objPPT.WindowState = PowerPoint.PpWindowState.ppWindowMaximized

        'Presentation을 추가한다.
        objPres = objPPT.Presentations.Add(MsoTriState.msoTrue)

        '슬라이드를 추가한다.=====================================================
        Dim objSlide As PowerPoint.Slide
        Dim objCustomLayout As PowerPoint.CustomLayout

        'EnsurePowerPointIsRunning(True)
        'Create a custom layout based on the first layout in the slide master.
        'This is used simply for creating the slide
        objCustomLayout = objPres.SlideMaster.CustomLayouts.Item(1)
        'Create slide
        objSlide = objPres.Slides.AddSlide(1, objCustomLayout)
        'Set the layout
        'objSlide.Layout = PowerPoint.PpSlideLayout.ppLayoutText
        objSlide.Layout = PowerPoint.PpSlideLayout.ppLayoutBlank
        'Clean up
        objCustomLayout.Delete()
        objCustomLayout = Nothing
        objSlide = Nothing

        '========제목 제거==================================================== (전부 제거하는 방법은 모르겠다;;)
        For i = 1 To objPres.Slides(1).Shapes.Count
            If objPres.Slides(1).Shapes(i).HasTextFrame Then
                objPres.Slides(1).Shapes(i).Delete()
                Exit For
            End If
        Next i
        '=========================================================================

        '테이블을 추가한다.=======================================================
        Dim objShape As PowerPoint.Shape
        Dim objTable As PowerPoint.Table

        '해석파일명을 추가한다.=======================================================================
        Dim strText As String = ""
        objPres.Slides(1).Select()
        objShape = objPres.Slides(1).Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 10, 5, 700, 300)
        objShape.TextFrame.AutoSize = PowerPoint.PpAutoSize.ppAutoSizeShapeToFitText
        For i = 1 To Tot_File
            strText = strText & "MD " & i & ". " & OpenFile.names(i - 1) & vbCrLf
        Next
        objShape.TextFrame.TextRange.Text = strText

        objShape.TextEffect.FontSize = 10
        objShape.TextEffect.FontBold = MsoTriState.msoTrue
        '============================================================================================

        'Add a table into the first slide in the presentation
        Dim Tot_Row As Integer = 3 + 3 * (Tot_File)
        Dim Tot_Col As Integer = 20 '중국정면 시트 (내수옵셋보다 하나 더 있다)
        objPres.Slides(1).Select()

        objShape = objPres.Slides(1).Shapes.AddTable(1, 1, 10, 50, 700)
        objTable = objShape.Table
        With objShape.Table
            .Cell(1, 1).Shape.Fill.ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Bold = MsoTriState.msoTrue
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Size = 9
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            '.Cell(nRow, nCol).Shape.TextFrame.TextRange.Text = Me.InjuryLbl1.get_TextMatrix(nRow - 1, nCol - 1)
            .Cell(1, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(1, 1).Shape.TextFrame.MarginTop = 0
            .Cell(1, 1).Shape.TextFrame.MarginLeft = 0
            .Cell(1, 1).Shape.TextFrame.MarginRight = 0
            .Cell(1, 1).Shape.TextFrame.VerticalAnchor = MsoVerticalAnchor.msoAnchorMiddle
            .Cell(1, 1).Shape.TextFrame.HorizontalAnchor = MsoHorizontalAnchor.msoAnchorCenter
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).Weight = 1
            For i = 1 To 2
                .Rows.Add()
            Next
            .Rows.Add()
            .Rows(1).Height = 12
            .Rows(2).Height = 10
            .Rows(3).Height = 10
            .Rows(4).Height = 10
            .Cell(4, 1).Shape.Fill.ForeColor.RGB = RGB(Color.White.R, Color.White.G, Color.White.B)
            .Cell(4, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            .Cell(4, 1).Shape.TextFrame.TextRange.Font.Size = 9
            .Cell(4, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            .Cell(4, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(4, 1).Shape.TextFrame.MarginTop = 0
            .Cell(4, 1).Shape.TextFrame.MarginLeft = 0
            .Cell(4, 1).Shape.TextFrame.MarginRight = 0
            .Cell(4, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(4, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(4, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(4, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            For i = 1 To Tot_Col - 1
                .Columns.Add()
            Next
            For i = 2 To Tot_File * 3
                .Rows.Add()
            Next

            '셀병합 및 조절
            .Cell(1, 1).Merge(MergeTo:=.Cell(3, 2))
            .Cell(1, 3).Merge(MergeTo:=.Cell(1, 4))
            .Cell(1, 5).Merge(MergeTo:=.Cell(1, 7))
            .Cell(1, 8).Merge(MergeTo:=.Cell(1, 9))
            .Cell(1, 10).Merge(MergeTo:=.Cell(1, 13))
            .Cell(1, 14).Merge(MergeTo:=.Cell(1, 19))

            For i = 10 To 18 Step 2
                .Cell(2, i).Merge(MergeTo:=.Cell(2, i + 1))
            Next
            .Cell(1, 20).Merge(MergeTo:=.Cell(3, 20))
            For i = 3 To 9
                .Cell(2, i).Merge(MergeTo:=.Cell(3, i))
            Next

            For i = 1 To Tot_File
                .Cell(3 * i + 1, 1).Merge(MergeTo:=.Cell(3 * i + 3, 1))
                .Cell(3 * i + 2, 2).Merge(MergeTo:=.Cell(3 * i + 3, 2))
                .Cell(3 * i + 1, 20).Merge(MergeTo:=.Cell(3 * i + 3, 20))
            Next

            '라벨을 쓴다==============================================================================
            .Cell(1, 1).Shape.TextFrame.TextRange.Text = "CNCAP" & vbCrLf & "정면"
            .Cell(1, 3).Shape.TextFrame.TextRange.Text = "머리"
            .Cell(2, 3).Shape.TextFrame.TextRange.Text = "3ms" & vbCrLf & "[G]"
            .Cell(2, 4).Shape.TextFrame.TextRange.Text = "HIC36"
            .Cell(1, 5).Shape.TextFrame.TextRange.Text = "목"
            .Cell(2, 5).Shape.TextFrame.TextRange.Text = "Shear" & vbCrLf & "[kN]"
            .Cell(2, 6).Shape.TextFrame.TextRange.Text = "Tens." & vbCrLf & "[kN]"
            .Cell(2, 7).Shape.TextFrame.TextRange.Text = "Exten." & vbCrLf & "[Nm]"
            .Cell(1, 8).Shape.TextFrame.TextRange.Text = "가슴"
            .Cell(2, 8).Shape.TextFrame.TextRange.Text = "변위" & vbCrLf & "[mm]"
            .Cell(2, 9).Shape.TextFrame.TextRange.Text = "VC" & vbCrLf & "[m/s]"
            .Cell(1, 10).Shape.TextFrame.TextRange.Text = "무릎"
            .Cell(2, 10).Shape.TextFrame.TextRange.Text = "변위 [mm]"
            .Cell(3, 10).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 11).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(2, 12).Shape.TextFrame.TextRange.Text = "하중 [kN]"
            .Cell(3, 12).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 13).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(1, 14).Shape.TextFrame.TextRange.Text = "하지"
            .Cell(2, 14).Shape.TextFrame.TextRange.Text = "TI(Upr)"
            .Cell(3, 14).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 15).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(2, 16).Shape.TextFrame.TextRange.Text = "TI(Lwr)"
            .Cell(3, 16).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 17).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(2, 18).Shape.TextFrame.TextRange.Text = "Comp[kN]"
            .Cell(3, 18).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 19).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(1, 20).Shape.TextFrame.TextRange.Text = "Total" & vbCrLf & "Score"

            For nRow = 4 To Tot_Row Step 3

                .Cell(nRow + 2, 3).Merge(MergeTo:=.Cell(nRow + 2, 4))
                .Cell(nRow + 2, 5).Merge(MergeTo:=.Cell(nRow + 2, 7))
                .Cell(nRow + 2, 8).Merge(MergeTo:=.Cell(nRow + 2, 9))
                .Cell(nRow + 2, 10).Merge(MergeTo:=.Cell(nRow + 2, 13))
                .Cell(nRow + 2, 14).Merge(MergeTo:=.Cell(nRow + 2, 19))
                For nCol = 3 To Tot_Col - 1
                    .Cell(nRow, nCol).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F.get_TextMatrix(nRow - 4, nCol)
                    .Cell(nRow + 1, nCol).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F.get_TextMatrix(nRow - 3, nCol)
                Next
                '========================================================
                .Cell(nRow, 1).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F.get_TextMatrix(nRow - 4, 0) 'CASE 이름
                .Cell(nRow, 2).Shape.TextFrame.TextRange.Text = "Value"
                .Cell(nRow + 2, 2).Shape.TextFrame.TextRange.Text = "Score"
                .Cell(nRow, 20).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F.get_TextMatrix(nRow - 4, 20) '종합점수
                '개별점수 기입
                .Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F.get_TextMatrix(nRow - 4 + 2, 4)
                .Cell(nRow + 2, 5).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F.get_TextMatrix(nRow - 4 + 2, 7)
                .Cell(nRow + 2, 8).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F.get_TextMatrix(nRow - 4 + 2, 9)
                .Cell(nRow + 2, 10).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F.get_TextMatrix(nRow - 4 + 2, 13)
                .Cell(nRow + 2, 14).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F.get_TextMatrix(nRow - 4 + 2, 19)

                '색칠한다.
                If CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) >= 5.0 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) >= (5 * (2 / 3)) Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) >= (5 * (1 / 3)) Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If

                If CSng(.Cell(nRow + 2, 5).Shape.TextFrame.TextRange.Text) >= 2 Then
                    .Cell(nRow + 2, 5).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 5).Shape.TextFrame.TextRange.Text) >= (2 * (2 / 3)) Then
                    .Cell(nRow + 2, 5).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 5).Shape.TextFrame.TextRange.Text) >= (2 * (1 / 3)) Then
                    .Cell(nRow + 2, 5).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 5).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 5).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 5).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If

                If CSng(.Cell(nRow + 2, 8).Shape.TextFrame.TextRange.Text) >= 5.0 Then
                    .Cell(nRow + 2, 8).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 8).Shape.TextFrame.TextRange.Text) >= (5 * (2 / 3)) Then
                    .Cell(nRow + 2, 8).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 8).Shape.TextFrame.TextRange.Text) >= (5 * (1 / 3)) Then
                    .Cell(nRow + 2, 8).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 8).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 8).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 8).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If

                If CSng(.Cell(nRow + 2, 10).Shape.TextFrame.TextRange.Text) >= 2.0 Then
                    .Cell(nRow + 2, 10).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 10).Shape.TextFrame.TextRange.Text) >= (2 * (2 / 3)) Then
                    .Cell(nRow + 2, 10).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 10).Shape.TextFrame.TextRange.Text) >= (2 * (1 / 3)) Then
                    .Cell(nRow + 2, 10).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 10).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 10).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 10).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If

                If CSng(.Cell(nRow + 2, 14).Shape.TextFrame.TextRange.Text) >= 2.0 Then
                    .Cell(nRow + 2, 14).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 14).Shape.TextFrame.TextRange.Text) >= (2 * (2 / 3)) Then
                    .Cell(nRow + 2, 14).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 14).Shape.TextFrame.TextRange.Text) >= (2 * (1 / 3)) Then
                    .Cell(nRow + 2, 14).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 14).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 14).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 14).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If
            Next

        End With
    End Sub

    Private Sub PPTexportChinaOffset()
        '파워포인트를 시작한다. (프로그램만 구동)
        objPPT = New PowerPoint.Application
        objPPT.Visible = MsoTriState.msoTrue
        objPPT.WindowState = PowerPoint.PpWindowState.ppWindowMaximized

        'Presentation을 추가한다.
        objPres = objPPT.Presentations.Add(MsoTriState.msoTrue)

        '슬라이드를 추가한다.=====================================================
        Dim objSlide As PowerPoint.Slide
        Dim objCustomLayout As PowerPoint.CustomLayout

        'EnsurePowerPointIsRunning(True)
        'Create a custom layout based on the first layout in the slide master.
        'This is used simply for creating the slide
        objCustomLayout = objPres.SlideMaster.CustomLayouts.Item(1)
        'Create slide
        objSlide = objPres.Slides.AddSlide(1, objCustomLayout)
        'Set the layout
        'objSlide.Layout = PowerPoint.PpSlideLayout.ppLayoutText
        objSlide.Layout = PowerPoint.PpSlideLayout.ppLayoutBlank
        'Clean up
        objCustomLayout.Delete()
        objCustomLayout = Nothing
        objSlide = Nothing

        '========제목 제거==================================================== (전부 제거하는 방법은 모르겠다;;)
        For i = 1 To objPres.Slides(1).Shapes.Count
            If objPres.Slides(1).Shapes(i).HasTextFrame Then
                objPres.Slides(1).Shapes(i).Delete()
                Exit For
            End If
        Next i
        '=========================================================================

        '테이블을 추가한다.=======================================================
        Dim objShape As PowerPoint.Shape
        Dim objTable As PowerPoint.Table

        '해석파일명을 추가한다.=======================================================================
        Dim strText As String = ""
        objPres.Slides(1).Select()
        objShape = objPres.Slides(1).Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 10, 5, 700, 300)
        objShape.TextFrame.AutoSize = PowerPoint.PpAutoSize.ppAutoSizeShapeToFitText
        For i = 1 To Tot_File
            strText = strText & "MD " & i & ". " & OpenFile.names(i - 1) & vbCrLf
        Next
        objShape.TextFrame.TextRange.Text = strText

        objShape.TextEffect.FontSize = 10
        objShape.TextEffect.FontBold = MsoTriState.msoTrue
        '============================================================================================

        'Add a table into the first slide in the presentation
        Dim Tot_Row As Integer = 3 + 3 * (Tot_File)
        Dim Tot_Col As Integer = 20 '중국옵셋 시트 
        objPres.Slides(1).Select()

        objShape = objPres.Slides(1).Shapes.AddTable(1, 1, 10, 50, 700)
        objTable = objShape.Table
        With objShape.Table
            .Cell(1, 1).Shape.Fill.ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Bold = MsoTriState.msoTrue
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Size = 9
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            '.Cell(nRow, nCol).Shape.TextFrame.TextRange.Text = Me.InjuryLbl1.get_TextMatrix(nRow - 1, nCol - 1)
            .Cell(1, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(1, 1).Shape.TextFrame.MarginTop = 0
            .Cell(1, 1).Shape.TextFrame.MarginLeft = 0
            .Cell(1, 1).Shape.TextFrame.MarginRight = 0
            .Cell(1, 1).Shape.TextFrame.VerticalAnchor = MsoVerticalAnchor.msoAnchorMiddle
            .Cell(1, 1).Shape.TextFrame.HorizontalAnchor = MsoHorizontalAnchor.msoAnchorCenter
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).Weight = 1
            For i = 1 To 2
                .Rows.Add()
            Next
            .Rows.Add()
            .Rows(1).Height = 12
            .Rows(2).Height = 10
            .Rows(3).Height = 10
            .Rows(4).Height = 10
            .Cell(4, 1).Shape.Fill.ForeColor.RGB = RGB(Color.White.R, Color.White.G, Color.White.B)
            .Cell(4, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            .Cell(4, 1).Shape.TextFrame.TextRange.Font.Size = 9
            .Cell(4, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            .Cell(4, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(4, 1).Shape.TextFrame.MarginTop = 0
            .Cell(4, 1).Shape.TextFrame.MarginLeft = 0
            .Cell(4, 1).Shape.TextFrame.MarginRight = 0
            .Cell(4, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(4, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(4, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(4, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            For i = 1 To Tot_Col - 1
                .Columns.Add()
            Next
            For i = 2 To Tot_File * 3
                .Rows.Add()
            Next

            '셀병합 및 조절
            .Cell(1, 1).Merge(MergeTo:=.Cell(3, 2))
            .Cell(1, 3).Merge(MergeTo:=.Cell(1, 7))
            .Cell(1, 8).Merge(MergeTo:=.Cell(1, 9))
            .Cell(1, 10).Merge(MergeTo:=.Cell(1, 13))
            .Cell(1, 14).Merge(MergeTo:=.Cell(1, 19))

            For i = 10 To 18 Step 2
                .Cell(2, i).Merge(MergeTo:=.Cell(2, i + 1))
            Next
            .Cell(1, 20).Merge(MergeTo:=.Cell(3, 20))
            For i = 3 To 9
                .Cell(2, i).Merge(MergeTo:=.Cell(3, i))
            Next

            For i = 1 To Tot_File
                .Cell(3 * i + 1, 1).Merge(MergeTo:=.Cell(3 * i + 3, 1))
                .Cell(3 * i + 2, 2).Merge(MergeTo:=.Cell(3 * i + 3, 2))
                .Cell(3 * i + 1, 20).Merge(MergeTo:=.Cell(3 * i + 3, 20))
            Next

            '라벨을 쓴다==============================================================================
            .Cell(1, 1).Shape.TextFrame.TextRange.Text = "CNCAP" & vbCrLf & "옵셋"
            .Cell(1, 3).Shape.TextFrame.TextRange.Text = "머리 / 목"
            .Cell(2, 3).Shape.TextFrame.TextRange.Text = "3ms" & vbCrLf & "[G]"
            .Cell(2, 4).Shape.TextFrame.TextRange.Text = "HIC36"
            .Cell(2, 5).Shape.TextFrame.TextRange.Text = "Shear" & vbCrLf & "[kN]"
            .Cell(2, 6).Shape.TextFrame.TextRange.Text = "Tens." & vbCrLf & "[kN]"
            .Cell(2, 7).Shape.TextFrame.TextRange.Text = "Exten." & vbCrLf & "[Nm]"
            .Cell(1, 8).Shape.TextFrame.TextRange.Text = "가슴"
            .Cell(2, 8).Shape.TextFrame.TextRange.Text = "변위" & vbCrLf & "[mm]"
            .Cell(2, 9).Shape.TextFrame.TextRange.Text = "VC" & vbCrLf & "[m/s]"
            .Cell(1, 10).Shape.TextFrame.TextRange.Text = "무릎"
            .Cell(2, 10).Shape.TextFrame.TextRange.Text = "변위 [mm]"
            .Cell(3, 10).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 11).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(2, 12).Shape.TextFrame.TextRange.Text = "하중 [kN]"
            .Cell(3, 12).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 13).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(1, 14).Shape.TextFrame.TextRange.Text = "하지"
            .Cell(2, 14).Shape.TextFrame.TextRange.Text = "TI(Upr)"
            .Cell(3, 14).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 15).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(2, 16).Shape.TextFrame.TextRange.Text = "TI(Lwr)"
            .Cell(3, 16).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 17).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(2, 18).Shape.TextFrame.TextRange.Text = "Comp[kN]"
            .Cell(3, 18).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 19).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(1, 20).Shape.TextFrame.TextRange.Text = "Total" & vbCrLf & "Score"

            For nRow = 4 To Tot_Row Step 3

                .Cell(nRow + 2, 3).Merge(MergeTo:=.Cell(nRow + 2, 7))
                .Cell(nRow + 2, 8).Merge(MergeTo:=.Cell(nRow + 2, 9))
                .Cell(nRow + 2, 10).Merge(MergeTo:=.Cell(nRow + 2, 13))
                .Cell(nRow + 2, 14).Merge(MergeTo:=.Cell(nRow + 2, 19))
                For nCol = 3 To Tot_Col - 1
                    .Cell(nRow, nCol).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O.get_TextMatrix(nRow - 4, nCol)
                    .Cell(nRow + 1, nCol).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O.get_TextMatrix(nRow - 3, nCol)
                Next
                '========================================================
                .Cell(nRow, 1).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O.get_TextMatrix(nRow - 4, 0) 'CASE 이름
                .Cell(nRow, 2).Shape.TextFrame.TextRange.Text = "Value"
                .Cell(nRow + 2, 2).Shape.TextFrame.TextRange.Text = "Score"
                .Cell(nRow, 20).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O.get_TextMatrix(nRow - 4, 20) '종합점수
                '개별점수 기입
                .Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O.get_TextMatrix(nRow - 4 + 2, 7)
                .Cell(nRow + 2, 8).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O.get_TextMatrix(nRow - 4 + 2, 9)
                .Cell(nRow + 2, 10).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O.get_TextMatrix(nRow - 4 + 2, 13)
                .Cell(nRow + 2, 14).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O.get_TextMatrix(nRow - 4 + 2, 19)

                '색칠한다.
                If CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) >= (4 * (2 / 3)) Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) >= (4 * (1 / 3)) Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If

                If CSng(.Cell(nRow + 2, 8).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 8).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 8).Shape.TextFrame.TextRange.Text) >= (4 * (2 / 3)) Then
                    .Cell(nRow + 2, 8).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 8).Shape.TextFrame.TextRange.Text) >= (4 * (1 / 3)) Then
                    .Cell(nRow + 2, 8).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 8).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 8).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 8).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If

                If CSng(.Cell(nRow + 2, 10).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 10).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 10).Shape.TextFrame.TextRange.Text) >= (4 * (2 / 3)) Then
                    .Cell(nRow + 2, 10).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 10).Shape.TextFrame.TextRange.Text) >= (4 * (1 / 3)) Then
                    .Cell(nRow + 2, 10).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 10).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 10).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 10).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If

                If CSng(.Cell(nRow + 2, 14).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 14).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 14).Shape.TextFrame.TextRange.Text) >= (4 * (2 / 3)) Then
                    .Cell(nRow + 2, 14).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 14).Shape.TextFrame.TextRange.Text) >= (4 * (1 / 3)) Then
                    .Cell(nRow + 2, 14).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 14).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 14).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 14).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If
            Next

        End With
    End Sub

    Private Sub PPTexportChinafrontalRear()
        '파워포인트를 시작한다. (프로그램만 구동)
        objPPT = New PowerPoint.Application
        objPPT.Visible = MsoTriState.msoTrue
        objPPT.WindowState = PowerPoint.PpWindowState.ppWindowMaximized

        'Presentation을 추가한다.
        objPres = objPPT.Presentations.Add(MsoTriState.msoTrue)

        '슬라이드를 추가한다.=====================================================
        Dim objSlide As PowerPoint.Slide
        Dim objCustomLayout As PowerPoint.CustomLayout

        'EnsurePowerPointIsRunning(True)
        'Create a custom layout based on the first layout in the slide master.
        'This is used simply for creating the slide
        objCustomLayout = objPres.SlideMaster.CustomLayouts.Item(1)
        'Create slide
        objSlide = objPres.Slides.AddSlide(1, objCustomLayout)
        'Set the layout
        'objSlide.Layout = PowerPoint.PpSlideLayout.ppLayoutText
        objSlide.Layout = PowerPoint.PpSlideLayout.ppLayoutBlank
        'Clean up
        objCustomLayout.Delete()
        objCustomLayout = Nothing
        objSlide = Nothing

        '========제목 제거==================================================== (전부 제거하는 방법은 모르겠다;;)
        For i = 1 To objPres.Slides(1).Shapes.Count
            If objPres.Slides(1).Shapes(i).HasTextFrame Then
                objPres.Slides(1).Shapes(i).Delete()
                Exit For
            End If
        Next i
        '=========================================================================

        '테이블을 추가한다.=======================================================
        Dim objShape As PowerPoint.Shape
        Dim objTable As PowerPoint.Table

        '해석파일명을 추가한다.=======================================================================
        Dim strText As String = ""
        objPres.Slides(1).Select()
        objShape = objPres.Slides(1).Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 10, 5, 700, 300)
        objShape.TextFrame.AutoSize = PowerPoint.PpAutoSize.ppAutoSizeShapeToFitText
        For i = 1 To Tot_File
            strText = strText & "MD " & i & ". " & OpenFile.names(i - 1) & vbCrLf
        Next
        objShape.TextFrame.TextRange.Text = strText

        objShape.TextEffect.FontSize = 10
        objShape.TextEffect.FontBold = MsoTriState.msoTrue
        '============================================================================================

        'Add a table into the first slide in the presentation
        Dim Tot_Row As Integer = 4 + 3 * (Tot_File)
        Dim Tot_Col As Integer = 9 '중국 후석 정면 시트 
        objPres.Slides(1).Select()

        objShape = objPres.Slides(1).Shapes.AddTable(1, 1, 10, 50, 700)
        objTable = objShape.Table
        With objShape.Table
            .Cell(1, 1).Shape.Fill.ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Bold = MsoTriState.msoTrue
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Size = 9
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            '.Cell(nRow, nCol).Shape.TextFrame.TextRange.Text = Me.InjuryLbl1.get_TextMatrix(nRow - 1, nCol - 1)
            .Cell(1, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(1, 1).Shape.TextFrame.MarginTop = 0
            .Cell(1, 1).Shape.TextFrame.MarginLeft = 0
            .Cell(1, 1).Shape.TextFrame.MarginRight = 0
            .Cell(1, 1).Shape.TextFrame.VerticalAnchor = MsoVerticalAnchor.msoAnchorMiddle
            .Cell(1, 1).Shape.TextFrame.HorizontalAnchor = MsoHorizontalAnchor.msoAnchorCenter
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).Weight = 1
            For i = 1 To 3
                .Rows.Add()
            Next
            .Rows.Add()
            .Rows(1).Height = 12
            .Rows(2).Height = 10
            .Rows(3).Height = 10
            .Rows(4).Height = 10
            .Rows(5).Height = 10
            .Cell(5, 1).Shape.Fill.ForeColor.RGB = RGB(Color.White.R, Color.White.G, Color.White.B)
            .Cell(5, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            .Cell(5, 1).Shape.TextFrame.TextRange.Font.Size = 9
            .Cell(5, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            .Cell(5, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(5, 1).Shape.TextFrame.MarginTop = 0
            .Cell(5, 1).Shape.TextFrame.MarginLeft = 0
            .Cell(5, 1).Shape.TextFrame.MarginRight = 0
            .Cell(5, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(5, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(5, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(5, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)

            For i = 2 To Tot_File * 3
                .Rows.Add()
            Next

            For i = 1 To Tot_Col - 1
                .Columns.Add()
            Next

            '셀병합 및 조절
            .Cell(1, 1).Merge(MergeTo:=.Cell(4, 1))
            .Cell(1, 4).Merge(MergeTo:=.Cell(1, 6))
            .Cell(1, 8).Merge(MergeTo:=.Cell(4, 8))
            .Cell(1, 9).Merge(MergeTo:=.Cell(4, 9))
            For i = 1 To Tot_File
                .Cell(3 * i + 2, 1).Merge(MergeTo:=.Cell(3 * i + 4, 1))
                .Cell(3 * i + 2, 8).Merge(MergeTo:=.Cell(3 * i + 4, 8))
                .Cell(3 * i + 2, 9).Merge(MergeTo:=.Cell(3 * i + 4, 9))
                .Cell(3 * i + 4, 4).Merge(MergeTo:=.Cell(3 * i + 4, 6))
            Next

            '라벨을 쓴다==============================================================================
            .Cell(1, 1).Shape.TextFrame.TextRange.Text = "CNCAP" & vbCrLf & "정면" & "후석"
            .Cell(1, 2).Shape.TextFrame.TextRange.Text = "Articles"
            .Cell(2, 2).Shape.TextFrame.TextRange.Text = "Upr."
            .Cell(3, 2).Shape.TextFrame.TextRange.Text = "Lwr."
            .Cell(4, 2).Shape.TextFrame.TextRange.Text = "Injury"
            .Cell(1, 3).Shape.TextFrame.TextRange.Text = "Head"
            .Cell(2, 3).Shape.TextFrame.TextRange.Text = "700"
            .Cell(3, 3).Shape.TextFrame.TextRange.Text = "500"
            .Cell(4, 3).Shape.TextFrame.TextRange.Text = "HIC15"
            .Cell(1, 4).Shape.TextFrame.TextRange.Text = "Neck"
            .Cell(2, 4).Shape.TextFrame.TextRange.Text = "2.62"
            .Cell(3, 4).Shape.TextFrame.TextRange.Text = "1.7"
            .Cell(4, 4).Shape.TextFrame.TextRange.Text = "Tens.[kN]"
            .Cell(2, 5).Shape.TextFrame.TextRange.Text = "1.95"
            .Cell(3, 5).Shape.TextFrame.TextRange.Text = "1.2"
            .Cell(4, 5).Shape.TextFrame.TextRange.Text = "Shear.[kN]"
            .Cell(2, 6).Shape.TextFrame.TextRange.Text = "49"
            .Cell(3, 6).Shape.TextFrame.TextRange.Text = "36"
            .Cell(4, 6).Shape.TextFrame.TextRange.Text = "Exten.[Nm]"
            .Cell(1, 7).Shape.TextFrame.TextRange.Text = "Chest"
            .Cell(2, 7).Shape.TextFrame.TextRange.Text = "48"
            .Cell(3, 7).Shape.TextFrame.TextRange.Text = "23"
            .Cell(4, 7).Shape.TextFrame.TextRange.Text = "Disp.[mm]"
            .Cell(1, 8).Shape.TextFrame.TextRange.Text = "No Secondary" & vbCrLf & "Impact"
            .Cell(1, 9).Shape.TextFrame.TextRange.Text = "Secondary" & vbCrLf & "Impact"

            For nRow = 5 To Tot_Row Step 3
                .Cell(nRow, 1).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F_Rear.get_TextMatrix(nRow - 5, 0) 'CASE 이름
                .Cell(nRow, 2).Shape.TextFrame.TextRange.Text = "Value"
                .Cell(nRow + 1, 2).Shape.TextFrame.TextRange.Text = "Rating"
                .Cell(nRow + 2, 2).Shape.TextFrame.TextRange.Text = "Total"
                'HIC
                .Cell(nRow, 3).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F_Rear.get_TextMatrix(nRow - 5, 2)
                .Cell(nRow + 1, 3).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F_Rear.get_TextMatrix(nRow - 5 + 1, 2)
                .Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F_Rear.get_TextMatrix(nRow - 5 + 2, 2)

                'Neck
                .Cell(nRow, 4).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F_Rear.get_TextMatrix(nRow - 5, 3)
                .Cell(nRow + 1, 4).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F_Rear.get_TextMatrix(nRow - 5 + 1, 3)
                .Cell(nRow + 2, 4).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F_Rear.get_TextMatrix(nRow - 5 + 2, 3)

                .Cell(nRow, 5).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F_Rear.get_TextMatrix(nRow - 5, 4)
                .Cell(nRow + 1, 5).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F_Rear.get_TextMatrix(nRow - 5 + 1, 4)

                .Cell(nRow, 6).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F_Rear.get_TextMatrix(nRow - 5, 5)
                .Cell(nRow + 1, 6).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F_Rear.get_TextMatrix(nRow - 5 + 1, 5)

                .Cell(nRow, 5).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
                .Cell(nRow, 6).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
                .Cell(nRow + 1, 5).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
                .Cell(nRow + 1, 6).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)

                .Cell(nRow, 7).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F_Rear.get_TextMatrix(nRow - 5, 6)
                .Cell(nRow + 1, 7).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F_Rear.get_TextMatrix(nRow - 5 + 1, 6)
                .Cell(nRow + 2, 7).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F_Rear.get_TextMatrix(nRow - 5 + 2, 6)

                .Cell(nRow, 8).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F_Rear.get_TextMatrix(nRow - 5, 7)
                .Cell(nRow, 9).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_F_Rear.get_TextMatrix(nRow - 5, 8)
                .Cell(nRow, 9).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            Next
        End With
    End Sub

    Private Sub PPTexportChinaoffsetRear()
        '파워포인트를 시작한다. (프로그램만 구동)
        objPPT = New PowerPoint.Application
        objPPT.Visible = MsoTriState.msoTrue
        objPPT.WindowState = PowerPoint.PpWindowState.ppWindowMaximized

        'Presentation을 추가한다.
        objPres = objPPT.Presentations.Add(MsoTriState.msoTrue)

        '슬라이드를 추가한다.=====================================================
        Dim objSlide As PowerPoint.Slide
        Dim objCustomLayout As PowerPoint.CustomLayout

        'EnsurePowerPointIsRunning(True)
        'Create a custom layout based on the first layout in the slide master.
        'This is used simply for creating the slide
        objCustomLayout = objPres.SlideMaster.CustomLayouts.Item(1)
        'Create slide
        objSlide = objPres.Slides.AddSlide(1, objCustomLayout)
        'Set the layout
        'objSlide.Layout = PowerPoint.PpSlideLayout.ppLayoutText
        objSlide.Layout = PowerPoint.PpSlideLayout.ppLayoutBlank
        'Clean up
        objCustomLayout.Delete()
        objCustomLayout = Nothing
        objSlide = Nothing

        '========제목 제거==================================================== (전부 제거하는 방법은 모르겠다;;)
        For i = 1 To objPres.Slides(1).Shapes.Count
            If objPres.Slides(1).Shapes(i).HasTextFrame Then
                objPres.Slides(1).Shapes(i).Delete()
                Exit For
            End If
        Next i
        '=========================================================================

        '테이블을 추가한다.=======================================================
        Dim objShape As PowerPoint.Shape
        Dim objTable As PowerPoint.Table

        '해석파일명을 추가한다.=======================================================================
        Dim strText As String = ""
        objPres.Slides(1).Select()
        objShape = objPres.Slides(1).Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 10, 5, 700, 300)
        objShape.TextFrame.AutoSize = PowerPoint.PpAutoSize.ppAutoSizeShapeToFitText
        For i = 1 To Tot_File
            strText = strText & "MD " & i & ". " & OpenFile.names(i - 1) & vbCrLf
        Next
        objShape.TextFrame.TextRange.Text = strText

        objShape.TextEffect.FontSize = 10
        objShape.TextEffect.FontBold = MsoTriState.msoTrue
        '============================================================================================

        'Add a table into the first slide in the presentation
        Dim Tot_Row As Integer = 4 + 3 * (Tot_File)
        Dim Tot_Col As Integer = 9 '중국 후석 정면 시트 
        objPres.Slides(1).Select()

        objShape = objPres.Slides(1).Shapes.AddTable(1, 1, 10, 50, 700)
        objTable = objShape.Table
        With objShape.Table
            .Cell(1, 1).Shape.Fill.ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Bold = MsoTriState.msoTrue
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Size = 9
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            '.Cell(nRow, nCol).Shape.TextFrame.TextRange.Text = Me.InjuryLbl1.get_TextMatrix(nRow - 1, nCol - 1)
            .Cell(1, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(1, 1).Shape.TextFrame.MarginTop = 0
            .Cell(1, 1).Shape.TextFrame.MarginLeft = 0
            .Cell(1, 1).Shape.TextFrame.MarginRight = 0
            .Cell(1, 1).Shape.TextFrame.VerticalAnchor = MsoVerticalAnchor.msoAnchorMiddle
            .Cell(1, 1).Shape.TextFrame.HorizontalAnchor = MsoHorizontalAnchor.msoAnchorCenter
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).Weight = 1
            For i = 1 To 3
                .Rows.Add()
            Next
            .Rows.Add()
            .Rows(1).Height = 12
            .Rows(2).Height = 10
            .Rows(3).Height = 10
            .Rows(4).Height = 10
            .Rows(5).Height = 10
            .Cell(5, 1).Shape.Fill.ForeColor.RGB = RGB(Color.White.R, Color.White.G, Color.White.B)
            .Cell(5, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            .Cell(5, 1).Shape.TextFrame.TextRange.Font.Size = 9
            .Cell(5, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            .Cell(5, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(5, 1).Shape.TextFrame.MarginTop = 0
            .Cell(5, 1).Shape.TextFrame.MarginLeft = 0
            .Cell(5, 1).Shape.TextFrame.MarginRight = 0
            .Cell(5, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(5, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(5, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(5, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)

            For i = 2 To Tot_File * 3
                .Rows.Add()
            Next

            For i = 1 To Tot_Col - 1
                .Columns.Add()
            Next

            '셀병합 및 조절
            .Cell(1, 1).Merge(MergeTo:=.Cell(4, 1))
            .Cell(1, 4).Merge(MergeTo:=.Cell(1, 6))
            .Cell(1, 8).Merge(MergeTo:=.Cell(4, 8))
            .Cell(1, 9).Merge(MergeTo:=.Cell(4, 9))
            For i = 1 To Tot_File
                .Cell(3 * i + 2, 1).Merge(MergeTo:=.Cell(3 * i + 4, 1))
                .Cell(3 * i + 2, 8).Merge(MergeTo:=.Cell(3 * i + 4, 8))
                .Cell(3 * i + 2, 9).Merge(MergeTo:=.Cell(3 * i + 4, 9))
                .Cell(3 * i + 4, 4).Merge(MergeTo:=.Cell(3 * i + 4, 6))
            Next

            '라벨을 쓴다==============================================================================
            .Cell(1, 1).Shape.TextFrame.TextRange.Text = "CNCAP" & vbCrLf & "옵셋" & "후석"
            .Cell(1, 2).Shape.TextFrame.TextRange.Text = "Articles"
            .Cell(2, 2).Shape.TextFrame.TextRange.Text = "Upr."
            .Cell(3, 2).Shape.TextFrame.TextRange.Text = "Lwr."
            .Cell(4, 2).Shape.TextFrame.TextRange.Text = "Injury"
            .Cell(1, 3).Shape.TextFrame.TextRange.Text = "Head"
            .Cell(2, 3).Shape.TextFrame.TextRange.Text = "700"
            .Cell(3, 3).Shape.TextFrame.TextRange.Text = "500"
            .Cell(4, 3).Shape.TextFrame.TextRange.Text = "HIC15"
            .Cell(1, 4).Shape.TextFrame.TextRange.Text = "Neck"
            .Cell(2, 4).Shape.TextFrame.TextRange.Text = "2.62"
            .Cell(3, 4).Shape.TextFrame.TextRange.Text = "1.7"
            .Cell(4, 4).Shape.TextFrame.TextRange.Text = "Tens.[kN]"
            .Cell(2, 5).Shape.TextFrame.TextRange.Text = "1.95"
            .Cell(3, 5).Shape.TextFrame.TextRange.Text = "1.2"
            .Cell(4, 5).Shape.TextFrame.TextRange.Text = "Shear.[kN]"
            .Cell(2, 6).Shape.TextFrame.TextRange.Text = "49"
            .Cell(3, 6).Shape.TextFrame.TextRange.Text = "36"
            .Cell(4, 6).Shape.TextFrame.TextRange.Text = "Exten.[Nm]"
            .Cell(1, 7).Shape.TextFrame.TextRange.Text = "Chest"
            .Cell(2, 7).Shape.TextFrame.TextRange.Text = "48"
            .Cell(3, 7).Shape.TextFrame.TextRange.Text = "23"
            .Cell(4, 7).Shape.TextFrame.TextRange.Text = "Disp.[mm]"
            .Cell(1, 8).Shape.TextFrame.TextRange.Text = "No Secondary" & vbCrLf & "Impact"
            .Cell(1, 9).Shape.TextFrame.TextRange.Text = "Secondary" & vbCrLf & "Impact"

            For nRow = 5 To Tot_Row Step 3
                .Cell(nRow, 1).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O_Rear.get_TextMatrix(nRow - 5, 0) 'CASE 이름
                .Cell(nRow, 2).Shape.TextFrame.TextRange.Text = "Value"
                .Cell(nRow + 1, 2).Shape.TextFrame.TextRange.Text = "Rating"
                .Cell(nRow + 2, 2).Shape.TextFrame.TextRange.Text = "Total"
                'HIC
                .Cell(nRow, 3).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O_Rear.get_TextMatrix(nRow - 5, 2)
                .Cell(nRow + 1, 3).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O_Rear.get_TextMatrix(nRow - 5 + 1, 2)
                .Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O_Rear.get_TextMatrix(nRow - 5 + 2, 2)

                'Neck
                .Cell(nRow, 4).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O_Rear.get_TextMatrix(nRow - 5, 3)
                .Cell(nRow + 1, 4).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O_Rear.get_TextMatrix(nRow - 5 + 1, 3)
                .Cell(nRow + 2, 4).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O_Rear.get_TextMatrix(nRow - 5 + 2, 3)

                .Cell(nRow, 5).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O_Rear.get_TextMatrix(nRow - 5, 4)
                .Cell(nRow + 1, 5).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O_Rear.get_TextMatrix(nRow - 5 + 1, 4)

                .Cell(nRow, 6).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O_Rear.get_TextMatrix(nRow - 5, 5)
                .Cell(nRow + 1, 6).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O_Rear.get_TextMatrix(nRow - 5 + 1, 5)

                .Cell(nRow, 5).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
                .Cell(nRow, 6).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
                .Cell(nRow + 1, 5).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
                .Cell(nRow + 1, 6).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)

                .Cell(nRow, 7).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O_Rear.get_TextMatrix(nRow - 5, 6)
                .Cell(nRow + 1, 7).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O_Rear.get_TextMatrix(nRow - 5 + 1, 6)
                .Cell(nRow + 2, 7).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O_Rear.get_TextMatrix(nRow - 5 + 2, 6)

                .Cell(nRow, 8).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O_Rear.get_TextMatrix(nRow - 5, 7)
                .Cell(nRow, 9).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_CHINA_O_Rear.get_TextMatrix(nRow - 5, 8)
                .Cell(nRow, 9).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            Next
        End With
    End Sub

    Private Sub PPTexportEuroFrontal_F()
        '파워포인트를 시작한다. (프로그램만 구동)
        objPPT = New PowerPoint.Application
        objPPT.Visible = MsoTriState.msoTrue
        objPPT.WindowState = PowerPoint.PpWindowState.ppWindowMaximized

        'Presentation을 추가한다.
        objPres = objPPT.Presentations.Add(MsoTriState.msoTrue)

        '슬라이드를 추가한다.=====================================================
        Dim objSlide As PowerPoint.Slide
        Dim objCustomLayout As PowerPoint.CustomLayout

        'EnsurePowerPointIsRunning(True)
        'Create a custom layout based on the first layout in the slide master.
        'This is used simply for creating the slide
        objCustomLayout = objPres.SlideMaster.CustomLayouts.Item(1)
        'Create slide
        objSlide = objPres.Slides.AddSlide(1, objCustomLayout)
        'Set the layout
        'objSlide.Layout = PowerPoint.PpSlideLayout.ppLayoutText
        objSlide.Layout = PowerPoint.PpSlideLayout.ppLayoutBlank  '이 마스터레이아웃은 아무것도 없음
        'Clean up
        objCustomLayout.Delete()
        objCustomLayout = Nothing
        objSlide = Nothing

        '========제목 제거==================================================== (전부 제거하는 방법은 모르겠다;; 위에 있음)
        For i = 1 To objPres.Slides(1).Shapes.Count
            If objPres.Slides(1).Shapes(i).HasTextFrame Then
                objPres.Slides(1).Shapes(i).Delete()
                Exit For
            End If
        Next i
        '=========================================================================

        '테이블을 추가한다.=======================================================
        Dim objShape As PowerPoint.Shape
        Dim objTable As PowerPoint.Table

        '해석파일명을 추가한다.=======================================================================
        Dim strText As String = ""
        objPres.Slides(1).Select()
        objShape = objPres.Slides(1).Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 10, 5, 700, 300)
        objShape.TextFrame.AutoSize = PowerPoint.PpAutoSize.ppAutoSizeShapeToFitText
        For i = 1 To Tot_File
            strText = strText & "MD " & i & ". " & OpenFile.names(i - 1) & vbCrLf
        Next
        objShape.TextFrame.TextRange.Text = strText

        objShape.TextEffect.FontSize = 10
        objShape.TextEffect.FontBold = MsoTriState.msoTrue
        '============================================================================================

        'Add a table into the first slide in the presentation
        Dim Tot_Row As Integer = 5 + 3 * (Tot_File)
        Dim Tot_Col As Integer = 13 '유럽옵셋 시트
        objPres.Slides(1).Select()

        objShape = objPres.Slides(1).Shapes.AddTable(1, 1, 10, 50, 700)
        objTable = objShape.Table
        With objShape.Table
            .Cell(1, 1).Shape.Fill.ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Bold = MsoTriState.msoTrue
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Size = 9
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            '.Cell(nRow, nCol).Shape.TextFrame.TextRange.Text = Me.InjuryLbl1.get_TextMatrix(nRow - 1, nCol - 1)
            .Cell(1, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(1, 1).Shape.TextFrame.MarginTop = 0
            .Cell(1, 1).Shape.TextFrame.MarginLeft = 0
            .Cell(1, 1).Shape.TextFrame.MarginRight = 0
            .Cell(1, 1).Shape.TextFrame.VerticalAnchor = MsoVerticalAnchor.msoAnchorMiddle
            .Cell(1, 1).Shape.TextFrame.HorizontalAnchor = MsoHorizontalAnchor.msoAnchorCenter
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).Weight = 1
            For i = 1 To 4
                .Rows.Add()
            Next
            .Rows.Add()
            .Rows(1).Height = 12
            .Rows(2).Height = 10
            .Rows(3).Height = 10
            .Rows(4).Height = 10
            .Rows(5).Height = 10
            .Rows(6).Height = 10
            .Cell(6, 1).Shape.Fill.ForeColor.RGB = RGB(Color.White.R, Color.White.G, Color.White.B)
            .Cell(6, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            .Cell(6, 1).Shape.TextFrame.TextRange.Font.Size = 9
            .Cell(6, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            .Cell(6, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(6, 1).Shape.TextFrame.MarginTop = 0
            .Cell(6, 1).Shape.TextFrame.MarginLeft = 0
            .Cell(6, 1).Shape.TextFrame.MarginRight = 0
            .Cell(6, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(6, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(6, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(6, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            For i = 1 To 12
                .Columns.Add()
            Next

            For i = 2 To Tot_File * 3
                .Rows.Add()
                .Rows(i + 3).Height = 10
            Next

            '셀병합 및 조절
            .Cell(1, 1).Merge(MergeTo:=.Cell(5, 2))
            .Cell(1, 3).Merge(MergeTo:=.Cell(1, 5))
            .Cell(1, 6).Merge(MergeTo:=.Cell(1, 8))
            .Cell(1, 9).Merge(MergeTo:=.Cell(1, 10))
            .Cell(1, 11).Merge(MergeTo:=.Cell(1, 12))
            .Cell(2, 11).Merge(MergeTo:=.Cell(2, 12))
            .Cell(3, 11).Merge(MergeTo:=.Cell(3, 12))
            .Cell(4, 11).Merge(MergeTo:=.Cell(4, 12))
            .Cell(1, 13).Merge(MergeTo:=.Cell(5, 13))
            .Cell(2, 3).Merge(MergeTo:=.Cell(3, 3))
            For i = 3 To 10
                .Cell(4, i).Merge(MergeTo:=.Cell(5, i))
            Next

            For i = 1 To Tot_File
                .Cell(5 + 3 * (i - 1) + 1, 1).Merge(MergeTo:=.Cell(5 + 3 * (i - 1) + 1 + 2, 1))
                .Cell(5 + 3 * (i - 1) + 1 + 1, 2).Merge(MergeTo:=.Cell(5 + 3 * (i - 1) + 1 + 2, 2))

                .Cell(5 + 3 * (i - 1) + 1 + 2, 3).Merge(MergeTo:=.Cell(5 + 3 * (i - 1) + 1 + 2, 5))
                .Cell(5 + 3 * (i - 1) + 1 + 2, 6).Merge(MergeTo:=.Cell(5 + 3 * (i - 1) + 1 + 2, 8))
                .Cell(5 + 3 * (i - 1) + 1 + 2, 9).Merge(MergeTo:=.Cell(5 + 3 * (i - 1) + 1 + 2, 10))
                .Cell(5 + 3 * (i - 1) + 1 + 2, 11).Merge(MergeTo:=.Cell(5 + 3 * (i - 1) + 1 + 2, 12))

                .Cell(5 + 3 * (i - 1) + 1, 13).Merge(MergeTo:=.Cell(5 + 3 * (i - 1) + 1 + 2, 13))
            Next

            '라벨을 쓴다==============================================================================
            .Cell(1, 1).Shape.TextFrame.TextRange.Text = "Euro-NCAP" & vbCrLf & "Frontal" & vbCrLf & "[Front Passenger]"
            .Cell(1, 3).Shape.TextFrame.TextRange.Text = "Head"
            .Cell(2, 3).Shape.TextFrame.TextRange.Text = "80"
            .Cell(4, 3).Shape.TextFrame.TextRange.Text = "Peak G"

            .Cell(2, 4).Shape.TextFrame.TextRange.Text = "80"
            .Cell(3, 4).Shape.TextFrame.TextRange.Text = "72"
            .Cell(4, 4).Shape.TextFrame.TextRange.Text = "3ms G"

            .Cell(2, 5).Shape.TextFrame.TextRange.Text = "700"
            .Cell(3, 5).Shape.TextFrame.TextRange.Text = "500"
            .Cell(4, 5).Shape.TextFrame.TextRange.Text = "HIC15"

            .Cell(1, 6).Shape.TextFrame.TextRange.Text = "Neck"
            .Cell(2, 6).Shape.TextFrame.TextRange.Text = "1.95"
            .Cell(3, 6).Shape.TextFrame.TextRange.Text = "1.2"
            .Cell(4, 6).Shape.TextFrame.TextRange.Text = "Shear" & vbCrLf & "[kN]"

            .Cell(2, 7).Shape.TextFrame.TextRange.Text = "2.62"
            .Cell(3, 7).Shape.TextFrame.TextRange.Text = "1.7"
            .Cell(4, 7).Shape.TextFrame.TextRange.Text = "Tens." & vbCrLf & "[kN]"

            .Cell(2, 8).Shape.TextFrame.TextRange.Text = "49"
            .Cell(3, 8).Shape.TextFrame.TextRange.Text = "36"
            .Cell(4, 8).Shape.TextFrame.TextRange.Text = "Exten." & vbCrLf & "[Nm]"

            .Cell(1, 9).Shape.TextFrame.TextRange.Text = "Chest"
            .Cell(2, 9).Shape.TextFrame.TextRange.Text = "42"
            .Cell(3, 9).Shape.TextFrame.TextRange.Text = "18"
            .Cell(4, 9).Shape.TextFrame.TextRange.Text = "Disp." & vbCrLf & "[mm]"

            .Cell(2, 10).Shape.TextFrame.TextRange.Text = "1.0"
            .Cell(3, 10).Shape.TextFrame.TextRange.Text = "0.5"
            .Cell(4, 10).Shape.TextFrame.TextRange.Text = "V/C" & vbCrLf & "[m/s]"

            .Cell(1, 11).Shape.TextFrame.TextRange.Text = "Femur"
            .Cell(2, 11).Shape.TextFrame.TextRange.Text = "6.2"
            .Cell(3, 11).Shape.TextFrame.TextRange.Text = "2.6"
            .Cell(4, 11).Shape.TextFrame.TextRange.Text = "Force [kN]"

            .Cell(5, 11).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(5, 12).Shape.TextFrame.TextRange.Text = "RH"

            .Cell(1, 13).Shape.TextFrame.TextRange.Text = "Total" & vbCrLf & "Score"

            For nRow = 6 To Tot_Row Step 3

                For nCol = 3 To Tot_Col - 1
                    .Cell(nRow, nCol).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_F.get_TextMatrix(nRow - 6, nCol - 1)
                    .Cell(nRow + 1, nCol).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_F.get_TextMatrix(nRow - 5, nCol - 1)
                Next
                '========================================================
                .Cell(nRow, 1).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_F.get_TextMatrix(nRow - 6, 0) 'CASE 이름
                .Cell(nRow, 2).Shape.TextFrame.TextRange.Text = "Value"
                .Cell(nRow + 1, 2).Shape.TextFrame.TextRange.Text = "Score"
                .Cell(nRow, 13).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_F.get_TextMatrix(nRow - 6, 12) '종합점수

                '개별점수
                .Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_F.get_TextMatrix(nRow - 6 + 2, 4)
                .Cell(nRow + 2, 6).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_F.get_TextMatrix(nRow - 6 + 2, 7)
                .Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_F.get_TextMatrix(nRow - 6 + 2, 9)
                .Cell(nRow + 2, 11).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_F.get_TextMatrix(nRow - 6 + 2, 11)
                '색칠한다.
                If CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) >= 2.67 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) >= 1.33 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If

                If CSng(.Cell(nRow + 2, 6).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 6).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 6).Shape.TextFrame.TextRange.Text) >= 2.67 Then
                    .Cell(nRow + 2, 6).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 6).Shape.TextFrame.TextRange.Text) >= 1.33 Then
                    .Cell(nRow + 2, 6).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 6).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 6).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 6).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If

                If CSng(.Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text) >= 2.67 Then
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text) >= 1.33 Then
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If

                If CSng(.Cell(nRow + 2, 11).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 11).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 11).Shape.TextFrame.TextRange.Text) >= 2.67 Then
                    .Cell(nRow + 2, 11).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 11).Shape.TextFrame.TextRange.Text) >= 1.33 Then
                    .Cell(nRow + 2, 11).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 11).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 11).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 11).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If
            Next

        End With
    End Sub

    Private Sub PPTexportEuroFrontal_R()
        '파워포인트를 시작한다. (프로그램만 구동)
        objPPT = New PowerPoint.Application
        objPPT.Visible = MsoTriState.msoTrue
        objPPT.WindowState = PowerPoint.PpWindowState.ppWindowMaximized

        'Presentation을 추가한다.
        objPres = objPPT.Presentations.Add(MsoTriState.msoTrue)

        '슬라이드를 추가한다.=====================================================
        Dim objSlide As PowerPoint.Slide
        Dim objCustomLayout As PowerPoint.CustomLayout

        'EnsurePowerPointIsRunning(True)
        'Create a custom layout based on the first layout in the slide master.
        'This is used simply for creating the slide
        objCustomLayout = objPres.SlideMaster.CustomLayouts.Item(1)
        'Create slide
        objSlide = objPres.Slides.AddSlide(1, objCustomLayout)
        'Set the layout
        'objSlide.Layout = PowerPoint.PpSlideLayout.ppLayoutText
        objSlide.Layout = PowerPoint.PpSlideLayout.ppLayoutBlank  '이 마스터레이아웃은 아무것도 없음
        'Clean up
        objCustomLayout.Delete()
        objCustomLayout = Nothing
        objSlide = Nothing

        '========제목 제거==================================================== (전부 제거하는 방법은 모르겠다;; 위에 있음)
        For i = 1 To objPres.Slides(1).Shapes.Count
            If objPres.Slides(1).Shapes(i).HasTextFrame Then
                objPres.Slides(1).Shapes(i).Delete()
                Exit For
            End If
        Next i
        '=========================================================================

        '테이블을 추가한다.=======================================================
        Dim objShape As PowerPoint.Shape
        Dim objTable As PowerPoint.Table

        '해석파일명을 추가한다.=======================================================================
        Dim strText As String = ""
        objPres.Slides(1).Select()
        objShape = objPres.Slides(1).Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 10, 5, 700, 300)
        objShape.TextFrame.AutoSize = PowerPoint.PpAutoSize.ppAutoSizeShapeToFitText
        For i = 1 To Tot_File
            strText = strText & "MD " & i & ". " & OpenFile.names(i - 1) & vbCrLf
        Next
        objShape.TextFrame.TextRange.Text = strText

        objShape.TextEffect.FontSize = 10
        objShape.TextEffect.FontBold = MsoTriState.msoTrue
        '============================================================================================

        'Add a table into the first slide in the presentation
        Dim Tot_Row As Integer = 5 + 3 * (Tot_File)
        Dim Tot_Col As Integer = 13 '유럽옵셋 시트
        objPres.Slides(1).Select()

        objShape = objPres.Slides(1).Shapes.AddTable(1, 1, 10, 50, 700)
        objTable = objShape.Table
        With objShape.Table
            .Cell(1, 1).Shape.Fill.ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Bold = MsoTriState.msoTrue
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Size = 9
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            '.Cell(nRow, nCol).Shape.TextFrame.TextRange.Text = Me.InjuryLbl1.get_TextMatrix(nRow - 1, nCol - 1)
            .Cell(1, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(1, 1).Shape.TextFrame.MarginTop = 0
            .Cell(1, 1).Shape.TextFrame.MarginLeft = 0
            .Cell(1, 1).Shape.TextFrame.MarginRight = 0
            .Cell(1, 1).Shape.TextFrame.VerticalAnchor = MsoVerticalAnchor.msoAnchorMiddle
            .Cell(1, 1).Shape.TextFrame.HorizontalAnchor = MsoHorizontalAnchor.msoAnchorCenter
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).Weight = 1
            For i = 1 To 4
                .Rows.Add()
            Next
            .Rows.Add()
            .Rows(1).Height = 12
            .Rows(2).Height = 10
            .Rows(3).Height = 10
            .Rows(4).Height = 10
            .Rows(5).Height = 10
            .Rows(6).Height = 10
            .Cell(6, 1).Shape.Fill.ForeColor.RGB = RGB(Color.White.R, Color.White.G, Color.White.B)
            .Cell(6, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            .Cell(6, 1).Shape.TextFrame.TextRange.Font.Size = 9
            .Cell(6, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            .Cell(6, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(6, 1).Shape.TextFrame.MarginTop = 0
            .Cell(6, 1).Shape.TextFrame.MarginLeft = 0
            .Cell(6, 1).Shape.TextFrame.MarginRight = 0
            .Cell(6, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(6, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(6, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(6, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            For i = 1 To 12
                .Columns.Add()
            Next

            For i = 2 To Tot_File * 3
                .Rows.Add()
                .Rows(i + 3).Height = 10
            Next

            '셀병합 및 조절
            .Cell(1, 1).Merge(MergeTo:=.Cell(5, 2))
            .Cell(1, 3).Merge(MergeTo:=.Cell(1, 5))
            .Cell(1, 6).Merge(MergeTo:=.Cell(1, 8))
            .Cell(1, 9).Merge(MergeTo:=.Cell(1, 10))
            .Cell(1, 11).Merge(MergeTo:=.Cell(1, 12))
            .Cell(2, 11).Merge(MergeTo:=.Cell(2, 12))
            .Cell(3, 11).Merge(MergeTo:=.Cell(3, 12))
            .Cell(4, 11).Merge(MergeTo:=.Cell(4, 12))
            .Cell(1, 13).Merge(MergeTo:=.Cell(5, 13))
            .Cell(2, 3).Merge(MergeTo:=.Cell(3, 3))
            For i = 3 To 10
                .Cell(4, i).Merge(MergeTo:=.Cell(5, i))
            Next

            For i = 1 To Tot_File
                .Cell(5 + 3 * (i - 1) + 1, 1).Merge(MergeTo:=.Cell(5 + 3 * (i - 1) + 1 + 2, 1))
                .Cell(5 + 3 * (i - 1) + 1 + 1, 2).Merge(MergeTo:=.Cell(5 + 3 * (i - 1) + 1 + 2, 2))

                .Cell(5 + 3 * (i - 1) + 1 + 2, 3).Merge(MergeTo:=.Cell(5 + 3 * (i - 1) + 1 + 2, 5))
                .Cell(5 + 3 * (i - 1) + 1 + 2, 6).Merge(MergeTo:=.Cell(5 + 3 * (i - 1) + 1 + 2, 8))
                .Cell(5 + 3 * (i - 1) + 1 + 2, 9).Merge(MergeTo:=.Cell(5 + 3 * (i - 1) + 1 + 2, 10))
                .Cell(5 + 3 * (i - 1) + 1 + 2, 11).Merge(MergeTo:=.Cell(5 + 3 * (i - 1) + 1 + 2, 12))

                .Cell(5 + 3 * (i - 1) + 1, 13).Merge(MergeTo:=.Cell(5 + 3 * (i - 1) + 1 + 2, 13))
            Next

            '라벨을 쓴다==============================================================================
            .Cell(1, 1).Shape.TextFrame.TextRange.Text = "Euro-NCAP" & vbCrLf & "Frontal" & vbCrLf & "[Rear Passenger]"
            .Cell(1, 3).Shape.TextFrame.TextRange.Text = "Head"
            .Cell(2, 3).Shape.TextFrame.TextRange.Text = "80"
            .Cell(4, 3).Shape.TextFrame.TextRange.Text = "Peak G"

            .Cell(2, 4).Shape.TextFrame.TextRange.Text = "80"
            .Cell(3, 4).Shape.TextFrame.TextRange.Text = "72"
            .Cell(4, 4).Shape.TextFrame.TextRange.Text = "3ms G"

            .Cell(2, 5).Shape.TextFrame.TextRange.Text = "700"
            .Cell(3, 5).Shape.TextFrame.TextRange.Text = "500"
            .Cell(4, 5).Shape.TextFrame.TextRange.Text = "HIC15"

            .Cell(1, 6).Shape.TextFrame.TextRange.Text = "Neck"
            .Cell(2, 6).Shape.TextFrame.TextRange.Text = "1.95"
            .Cell(3, 6).Shape.TextFrame.TextRange.Text = "1.2"
            .Cell(4, 6).Shape.TextFrame.TextRange.Text = "Shear" & vbCrLf & "[kN]"

            .Cell(2, 7).Shape.TextFrame.TextRange.Text = "2.62"
            .Cell(3, 7).Shape.TextFrame.TextRange.Text = "1.7"
            .Cell(4, 7).Shape.TextFrame.TextRange.Text = "Tens." & vbCrLf & "[kN]"

            .Cell(2, 8).Shape.TextFrame.TextRange.Text = "49"
            .Cell(3, 8).Shape.TextFrame.TextRange.Text = "36"
            .Cell(4, 8).Shape.TextFrame.TextRange.Text = "Exten." & vbCrLf & "[Nm]"

            .Cell(1, 9).Shape.TextFrame.TextRange.Text = "Chest"
            .Cell(2, 9).Shape.TextFrame.TextRange.Text = "42"
            .Cell(3, 9).Shape.TextFrame.TextRange.Text = "18"
            .Cell(4, 9).Shape.TextFrame.TextRange.Text = "Disp." & vbCrLf & "[mm]"

            .Cell(2, 10).Shape.TextFrame.TextRange.Text = "1.0"
            .Cell(3, 10).Shape.TextFrame.TextRange.Text = "0.5"
            .Cell(4, 10).Shape.TextFrame.TextRange.Text = "V/C" & vbCrLf & "[m/s]"

            .Cell(1, 11).Shape.TextFrame.TextRange.Text = "Femur"
            .Cell(2, 11).Shape.TextFrame.TextRange.Text = "6.2"
            .Cell(3, 11).Shape.TextFrame.TextRange.Text = "2.6"
            .Cell(4, 11).Shape.TextFrame.TextRange.Text = "Force [kN]"

            .Cell(5, 11).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(5, 12).Shape.TextFrame.TextRange.Text = "RH"

            .Cell(1, 13).Shape.TextFrame.TextRange.Text = "Total" & vbCrLf & "Score"

            For nRow = 6 To Tot_Row Step 3

                For nCol = 3 To Tot_Col - 1
                    .Cell(nRow, nCol).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_R.get_TextMatrix(nRow - 6, nCol - 1)
                    .Cell(nRow + 1, nCol).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_R.get_TextMatrix(nRow - 5, nCol - 1)
                Next
                '========================================================
                .Cell(nRow, 1).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_R.get_TextMatrix(nRow - 6, 0) 'CASE 이름
                .Cell(nRow, 2).Shape.TextFrame.TextRange.Text = "Value"
                .Cell(nRow + 1, 2).Shape.TextFrame.TextRange.Text = "Score"
                .Cell(nRow, 13).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_R.get_TextMatrix(nRow - 6, 12) '종합점수

                '개별점수
                .Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_R.get_TextMatrix(nRow - 6 + 2, 4)
                .Cell(nRow + 2, 6).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_R.get_TextMatrix(nRow - 6 + 2, 7)
                .Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_R.get_TextMatrix(nRow - 6 + 2, 9)
                .Cell(nRow + 2, 11).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_R.get_TextMatrix(nRow - 6 + 2, 11)
                '색칠한다.
                If CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) >= 2.67 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) >= 1.33 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If

                If CSng(.Cell(nRow + 2, 6).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 6).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 6).Shape.TextFrame.TextRange.Text) >= 2.67 Then
                    .Cell(nRow + 2, 6).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 6).Shape.TextFrame.TextRange.Text) >= 1.33 Then
                    .Cell(nRow + 2, 6).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 6).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 6).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 6).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If

                If CSng(.Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text) >= 2.67 Then
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text) >= 1.33 Then
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If

                If CSng(.Cell(nRow + 2, 11).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 11).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 11).Shape.TextFrame.TextRange.Text) >= 2.67 Then
                    .Cell(nRow + 2, 11).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 11).Shape.TextFrame.TextRange.Text) >= 1.33 Then
                    .Cell(nRow + 2, 11).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 11).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 11).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 11).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If
            Next

        End With
    End Sub

    Private Sub PPTexportEuroOffset()
        '파워포인트를 시작한다. (프로그램만 구동)
        objPPT = New PowerPoint.Application
        objPPT.Visible = MsoTriState.msoTrue
        objPPT.WindowState = PowerPoint.PpWindowState.ppWindowMaximized

        'Presentation을 추가한다.
        objPres = objPPT.Presentations.Add(MsoTriState.msoTrue)

        '슬라이드를 추가한다.=====================================================
        Dim objSlide As PowerPoint.Slide
        Dim objCustomLayout As PowerPoint.CustomLayout

        'EnsurePowerPointIsRunning(True)
        'Create a custom layout based on the first layout in the slide master.
        'This is used simply for creating the slide
        objCustomLayout = objPres.SlideMaster.CustomLayouts.Item(1)
        'Create slide
        objSlide = objPres.Slides.AddSlide(1, objCustomLayout)
        'Set the layout
        'objSlide.Layout = PowerPoint.PpSlideLayout.ppLayoutText
        objSlide.Layout = PowerPoint.PpSlideLayout.ppLayoutBlank  '이 마스터레이아웃은 아무것도 없음
        'Clean up
        objCustomLayout.Delete()
        objCustomLayout = Nothing
        objSlide = Nothing

        '========제목 제거==================================================== (전부 제거하는 방법은 모르겠다;; 위에 있음)
        For i = 1 To objPres.Slides(1).Shapes.Count
            If objPres.Slides(1).Shapes(i).HasTextFrame Then
                objPres.Slides(1).Shapes(i).Delete()
                Exit For
            End If
        Next i
        '=========================================================================

        '테이블을 추가한다.=======================================================
        Dim objShape As PowerPoint.Shape
        Dim objTable As PowerPoint.Table

        '해석파일명을 추가한다.=======================================================================
        Dim strText As String = ""
        objPres.Slides(1).Select()
        objShape = objPres.Slides(1).Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 10, 5, 700, 300)
        objShape.TextFrame.AutoSize = PowerPoint.PpAutoSize.ppAutoSizeShapeToFitText
        For i = 1 To Tot_File
            strText = strText & "MD " & i & ". " & OpenFile.names(i - 1) & vbCrLf
        Next
        objShape.TextFrame.TextRange.Text = strText

        objShape.TextEffect.FontSize = 10
        objShape.TextEffect.FontBold = MsoTriState.msoTrue
        '============================================================================================

        'Add a table into the first slide in the presentation
        Dim Tot_Row As Integer = 3 + 3 * (Tot_File)
        Dim Tot_Col As Integer = 19 '유럽옵셋 시트
        objPres.Slides(1).Select()

        objShape = objPres.Slides(1).Shapes.AddTable(1, 1, 10, 50, 700)
        objTable = objShape.Table
        With objShape.Table
            .Cell(1, 1).Shape.Fill.ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Bold = MsoTriState.msoTrue
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Size = 9
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            '.Cell(nRow, nCol).Shape.TextFrame.TextRange.Text = Me.InjuryLbl1.get_TextMatrix(nRow - 1, nCol - 1)
            .Cell(1, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(1, 1).Shape.TextFrame.MarginTop = 0
            .Cell(1, 1).Shape.TextFrame.MarginLeft = 0
            .Cell(1, 1).Shape.TextFrame.MarginRight = 0
            .Cell(1, 1).Shape.TextFrame.VerticalAnchor = MsoVerticalAnchor.msoAnchorMiddle
            .Cell(1, 1).Shape.TextFrame.HorizontalAnchor = MsoHorizontalAnchor.msoAnchorCenter
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).Weight = 1
            For i = 1 To 2
                .Rows.Add()
            Next
            .Rows.Add()
            .Rows(1).Height = 12
            .Rows(2).Height = 10
            .Rows(3).Height = 10
            .Rows(4).Height = 10
            .Cell(4, 1).Shape.Fill.ForeColor.RGB = RGB(Color.White.R, Color.White.G, Color.White.B)
            .Cell(4, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            .Cell(4, 1).Shape.TextFrame.TextRange.Font.Size = 9
            .Cell(4, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            .Cell(4, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(4, 1).Shape.TextFrame.MarginTop = 0
            .Cell(4, 1).Shape.TextFrame.MarginLeft = 0
            .Cell(4, 1).Shape.TextFrame.MarginRight = 0
            .Cell(4, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(4, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(4, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            .Cell(4, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).ForeColor.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)
            For i = 1 To 18
                .Columns.Add()
            Next

            For i = 2 To Tot_File * 3
                .Rows.Add()
                .Rows(i + 3).Height = 10
            Next

            '셀병합 및 조절
            .Cell(1, 1).Merge(MergeTo:=.Cell(3, 2))
            .Cell(1, 4).Merge(MergeTo:=.Cell(1, 6))
            .Cell(1, 7).Merge(MergeTo:=.Cell(1, 8))
            .Cell(1, 9).Merge(MergeTo:=.Cell(1, 12))
            .Cell(1, 13).Merge(MergeTo:=.Cell(1, 18))

            For i = 9 To 17 Step 2
                .Cell(2, i).Merge(MergeTo:=.Cell(2, i + 1))
            Next
            .Cell(1, 19).Merge(MergeTo:=.Cell(3, 19))
            For i = 3 To 8
                .Cell(2, i).Merge(MergeTo:=.Cell(3, i))
            Next

            For i = 1 To Tot_File
                .Cell(3 * i + 1, 1).Merge(MergeTo:=.Cell(3 * i + 3, 1))
                .Cell(3 * i + 2, 2).Merge(MergeTo:=.Cell(3 * i + 3, 2))
                .Cell(3 * i + 1, 19).Merge(MergeTo:=.Cell(3 * i + 3, 19))
            Next

            '라벨을 쓴다==============================================================================
            .Cell(1, 1).Shape.TextFrame.TextRange.Text = "EuroNCAP" & vbCrLf & "Offset"
            .Cell(1, 3).Shape.TextFrame.TextRange.Text = "Head"
            .Cell(2, 3).Shape.TextFrame.TextRange.Text = "HIC15"
            .Cell(1, 4).Shape.TextFrame.TextRange.Text = "Neck"
            .Cell(2, 4).Shape.TextFrame.TextRange.Text = "Shear" & vbCrLf & "[kN]"
            .Cell(2, 5).Shape.TextFrame.TextRange.Text = "Tens." & vbCrLf & "[kN]"
            .Cell(2, 6).Shape.TextFrame.TextRange.Text = "Exten." & vbCrLf & "[Nm]"
            .Cell(1, 7).Shape.TextFrame.TextRange.Text = "Chest"
            .Cell(2, 7).Shape.TextFrame.TextRange.Text = "Disp." & vbCrLf & "[mm]"
            .Cell(2, 8).Shape.TextFrame.TextRange.Text = "VC" & vbCrLf & "[m/s]"
            .Cell(1, 9).Shape.TextFrame.TextRange.Text = "Femur"
            .Cell(2, 9).Shape.TextFrame.TextRange.Text = "K.Slide [mm]"
            .Cell(3, 9).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 10).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(2, 11).Shape.TextFrame.TextRange.Text = "Force [kN]"
            .Cell(3, 11).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 12).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(1, 13).Shape.TextFrame.TextRange.Text = "Tibia"
            .Cell(2, 13).Shape.TextFrame.TextRange.Text = "TI(Upr)"
            .Cell(3, 13).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 14).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(2, 15).Shape.TextFrame.TextRange.Text = "TI(Lwr)"
            .Cell(3, 15).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 16).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(2, 17).Shape.TextFrame.TextRange.Text = "Comp[kN]"
            .Cell(3, 17).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 18).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(1, 19).Shape.TextFrame.TextRange.Text = "Total" & vbCrLf & "Score"

            For nRow = 4 To Tot_Row Step 3

                .Cell(nRow + 2, 3).Merge(MergeTo:=.Cell(nRow + 2, 6))
                .Cell(nRow + 2, 7).Merge(MergeTo:=.Cell(nRow + 2, 8))
                .Cell(nRow + 2, 9).Merge(MergeTo:=.Cell(nRow + 2, 12))
                .Cell(nRow + 2, 13).Merge(MergeTo:=.Cell(nRow + 2, 18))
                For nCol = 3 To Tot_Col - 1
                    .Cell(nRow, nCol).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_O.get_TextMatrix(nRow - 4, nCol + 1)
                    .Cell(nRow + 1, nCol).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_O.get_TextMatrix(nRow - 3, nCol + 1)
                Next
                '========================================================
                .Cell(nRow, 1).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_O.get_TextMatrix(nRow - 4, 0) 'CASE 이름
                .Cell(nRow, 2).Shape.TextFrame.TextRange.Text = "Value"
                .Cell(nRow + 2, 2).Shape.TextFrame.TextRange.Text = "Score"
                .Cell(nRow, 19).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_O.get_TextMatrix(nRow - 4, 20) '종합점수
                '개별점수
                .Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_O.get_TextMatrix(nRow - 4 + 2, 7)
                .Cell(nRow + 2, 7).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_O.get_TextMatrix(nRow - 4 + 2, 9)
                .Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_O.get_TextMatrix(nRow - 4 + 2, 13)
                .Cell(nRow + 2, 13).Shape.TextFrame.TextRange.Text = Me.Peak_Injury_Euro_O.get_TextMatrix(nRow - 4 + 2, 19)
                '색칠한다.
                If CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) >= 2.67 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) >= 1.33 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 3).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 3).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If

                If CSng(.Cell(nRow + 2, 7).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 7).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 7).Shape.TextFrame.TextRange.Text) >= 2.67 Then
                    .Cell(nRow + 2, 7).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 7).Shape.TextFrame.TextRange.Text) >= 1.33 Then
                    .Cell(nRow + 2, 7).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 7).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 7).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 7).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If

                If CSng(.Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text) >= 2.67 Then
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text) >= 1.33 Then
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 9).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 9).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If

                If CSng(.Cell(nRow + 2, 13).Shape.TextFrame.TextRange.Text) >= 4.0 Then
                    .Cell(nRow + 2, 13).Shape.Fill.ForeColor.RGB = RGB(Color.LightGreen.R, Color.LightGreen.G, Color.LightGreen.B)
                ElseIf CSng(.Cell(nRow + 2, 13).Shape.TextFrame.TextRange.Text) >= 2.67 Then
                    .Cell(nRow + 2, 13).Shape.Fill.ForeColor.RGB = RGB(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B)
                ElseIf CSng(.Cell(nRow + 2, 13).Shape.TextFrame.TextRange.Text) >= 1.33 Then
                    .Cell(nRow + 2, 13).Shape.Fill.ForeColor.RGB = RGB(Color.Orange.R, Color.Orange.G, Color.Orange.B)
                ElseIf CSng(.Cell(nRow + 2, 13).Shape.TextFrame.TextRange.Text) > 0.0 Then
                    .Cell(nRow + 2, 13).Shape.Fill.ForeColor.RGB = RGB(Color.Brown.R, Color.Brown.G, Color.Brown.B)
                Else
                    .Cell(nRow + 2, 13).Shape.Fill.ForeColor.RGB = RGB(Color.Red.R, Color.Red.G, Color.Red.B)
                End If
            Next

        End With
    End Sub

#End Region

    Private Sub ReloadBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReloadBtn.Click
        MainMDI.ProgressBarMain.Value = 0
        MainMDI.ProgressBarMain.Maximum = 550

        '원격 폴더가 있는지 검사

        For i = 0 To OpenFile.names.Count - 1
            If IsRemotePath(i) <> "" Then
                'mySession.GetFiles(Me.FilePathTxt.Text & "/" & FinalRemote(j) & ".xml", Application.StartupPath & "\TempResults\", False, myTransferOptions).Check()
                'Me.StatusLbl.Text = "Wait...Download Result Files...xml"
                'Application.DoEvents()
                mySession.GetFiles(IsRemotePath(i) & OpenFile.names(i) & ".peak", Application.StartupPath & "\TempResults\", False, myTransferOptions).Check()
                MainMDI.Statuslbl.Text = "Download peak files"
                Application.DoEvents()
                mySession.GetFiles(IsRemotePath(i) & OpenFile.names(i) & ".injury", Application.StartupPath & "\TempResults\", False, myTransferOptions).Check()
                MainMDI.Statuslbl.Text = "Download injury files"
                Application.DoEvents()
                mySession.GetFiles(IsRemotePath(i) & OpenFile.names(i) & ".log", Application.StartupPath & "\TempResults\", False, myTransferOptions).Check()
                MainMDI.Statuslbl.Text = "Download log files"
                Application.DoEvents()
                mySession.GetFiles(IsRemotePath(i) & OpenFile.names(i) & ".lac", Application.StartupPath & "\TempResults\", False, myTransferOptions).Check()
                MainMDI.Statuslbl.Text = "Download lac files"
                Application.DoEvents()
                mySession.GetFiles(IsRemotePath(i) & OpenFile.names(i) & ".frc", Application.StartupPath & "\TempResults\", False, myTransferOptions).Check()
                MainMDI.Statuslbl.Text = "Download frc files"
                Application.DoEvents()
                mySession.GetFiles(IsRemotePath(i) & OpenFile.names(i) & ".rds", Application.StartupPath & "\TempResults\", False, myTransferOptions).Check()
                MainMDI.Statuslbl.Text = "Download rds files"
                Application.DoEvents()
                mySession.GetFiles(IsRemotePath(i) & OpenFile.names(i) & ".jps", Application.StartupPath & "\TempResults\", False, myTransferOptions).Check()
                MainMDI.Statuslbl.Text = "Download jps files"
                Application.DoEvents()
                mySession.GetFiles(IsRemotePath(i) & OpenFile.names(i) & ".fhs", Application.StartupPath & "\TempResults\", False, myTransferOptions).Check()
                MainMDI.Statuslbl.Text = "Download fhs files"
                Application.DoEvents()
                mySession.GetFiles(IsRemotePath(i) & OpenFile.names(i) & ".control", Application.StartupPath & "\TempResults\", False, myTransferOptions).Check()
                MainMDI.Statuslbl.Text = "Download control files"
                Application.DoEvents()
            End If
        Next

        '상해를 읽는다
        Reading_Peak()

        '시트를 그린다.
        MainMDI.Statuslbl.Text = "Drawing Injury Sheets"
        MainMDI.ProgressBarMain.Value = 150
        Sheet_US()

        MainMDI.ProgressBarMain.Value = 250
        Sheet_DOM_F()

        MainMDI.ProgressBarMain.Value = 350
        Sheet_DOM_O()

        MainMDI.ProgressBarMain.Value = 400
        Sheet_China_F_Rear()
        Sheet_China_O_Rear()

        MainMDI.ProgressBarMain.Value = 450
        Sheet_Euro_F()
        Sheet_Euro_Rear()

        MainMDI.ProgressBarMain.Value = 500
        Sheet_Lbl8()
        Sheet_Euro_O()

        With Me.Peak_Injury_CHINA_F_Rear
            .Row = 0
            .Col = 7
            .RowSel = 2
            .ColSel = 8
            .CellBackColor = Color.White
        End With
        With Me.Peak_Injury_CHINA_O_Rear
            .Row = 0
            .Col = 7
            .RowSel = 2
            .ColSel = 8
            .CellBackColor = Color.White
        End With


        MainMDI.ProgressBarMain.Value = 550
        MainMDI.Statuslbl.Text = "Complete"
    End Sub

    Private Sub OpenLst_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles OpenLst.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Me.OpenWithXMADgic.Show(MousePosition)
        End If
    End Sub

    Private Sub OpenWithXMADgicToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenWithXMADgicToolStripMenuItem.Click

        If Me.OpenLst.SelectedIndex < 0 Then
            MainMDI.Statuslbl.Text = "파일을 선택하세요"
            Exit Sub
        End If

        MainMDI.Statuslbl.Text = "MADYMO Input Open.."

        Dim PROC As Integer
        Dim ProgramFolder64 As String
        Dim TmpPath() As String
        Dim TmpFile As String

        Dim NewPathFile As StreamReader
        NewPathFile = New StreamReader(Application.StartupPath & "\DATA\EnvironPATH.dat")

        TmpPath = NewPathFile.ReadLine().Split("=")

        ProgramFolder64 = TmpPath(UBound(TmpPath))

        NewPathFile.Close()

        TmpFile = Mid(Me.OpenDlg.FileNames(Me.OpenLst.SelectedIndex), 1, Len(Me.OpenDlg.FileNames(Me.OpenLst.SelectedIndex)) - 5) & ".xml"

        Try
            'PROC = Shell(Application.StartupPath & "\XMADgic.exe", AppWinStyle.NormalFocus)
            PROC = Shell(ProgramFolder64 & "\em64t-win\bin\XMADgic.exe" & " " & TmpFile, AppWinStyle.NormalFocus)
        Catch ex As Exception
            MainMDI.Statuslbl.Text = "XMADgic " & ex.Message & " 설정 메뉴에서 Workspace Path를 설정하세요."
        End Try

    End Sub

    Private Sub OpenKn3WithHyperViewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenKn3WithHyperViewToolStripMenuItem.Click
        If Me.OpenLst.SelectedIndex < 0 Then
            MainMDI.Statuslbl.Text = "파일을 선택하세요"
            Exit Sub
        End If

        MainMDI.Statuslbl.Text = "MADYMO Animation Result File Open.."

        Dim PROC As Integer
        Dim ProgramFolder64 As String
        Dim TmpPath() As String
        Dim TmpFile As String

        Dim NewPathFile As StreamReader
        NewPathFile = New StreamReader(Application.StartupPath & "\DATA\EnvironPATH_HW.dat")

        TmpPath = NewPathFile.ReadLine().Split("=")

        ProgramFolder64 = TmpPath(UBound(TmpPath))

        NewPathFile.Close()

        TmpFile = Mid(Me.OpenDlg.FileNames(Me.OpenLst.SelectedIndex), 1, Len(Me.OpenDlg.FileNames(Me.OpenLst.SelectedIndex)) - 5) & ".kn3"

        Try
            'PROC = Shell(Application.StartupPath & "\XMADgic.exe", AppWinStyle.NormalFocus)
            PROC = Shell(ProgramFolder64 & "\hw.exe" & " " & TmpFile, AppWinStyle.NormalFocus)
        Catch ex As Exception
            MainMDI.Statuslbl.Text = "HyperView " & ex.Message & " 설정 메뉴에서 HyperView Path를 설정하세요."
        End Try
    End Sub

    Private Sub SplitConMain_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitConMain.SplitterMoved


        'Me.Width = 1100
        Me.SplitConMain.Width = Me.ClientRectangle.Width
        With Me
            '.Size = New Size(1100, 600)
            '.Width = 1100
            .PathLbl.Location = New Point(10, 20)
            .OpenLst.Location = New Point(12, .PathLbl.Location.Y + .PathLbl.Height + 10)
            'If Me.OpenDlg.FileNames(0) = "" Then
            '    .OpenLst.Size = New Size(.Width - 60, 21 * 4)
            'Else
            '    .OpenLst.Size = New Size(.Width - 60, 21 * UBound(Me.OpenDlg.FileNames) + 21)
            'End If
            .OpenLst.Size = New Size(.SplitConMain.Panel1.ClientRectangle.Width - .OpenLst.Location.X * 2 - 1 - 300, .SplitConMain.Panel1.ClientRectangle.Height - .PathLbl.Height - 30)
            .DescripTxt.Location = New Point(.OpenLst.Location.X + .OpenLst.Width + 5, .OpenLst.Location.Y)
            .DescripTxt.Size = New Size(295, .OpenLst.Height)

            '.ScoreBoard.Location = New Point(20, .OpenLst.Location.Y + .OpenLst.Height + 10)
            .ScoreBoard.Location = New Point(.OpenLst.Location.X, 10)
            .ScoreBoard.Size = New Size(.OpenLst.Width + 300, .SplitConMain.Panel2.ClientRectangle.Height - 60)

            .BtnInjuryGraph.Location = New Point(.SplitConMain.Panel2.ClientRectangle.Width - 205, .ScoreBoard.Location.Y + .ScoreBoard.Height + 10)
            .BtnInjuryGraph.Size = New Size(165, 30)

            .BtnValGraph.Location = New Point(.BtnInjuryGraph.Location.X - 5 - 165, .BtnInjuryGraph.Location.Y)
            .BtnValGraph.Size = New Size(165, 30)

            .PPTBtn.Location = New Point(.BtnValGraph.Location.X - 5 - 165, .BtnValGraph.Location.Y)
            .PPTBtn.Size = New Size(165, 30)

            .ReloadBtn.Location = New Point(.ScoreBoard.Location.X, .PPTBtn.Location.Y)
            .ReloadBtn.Size = New Size(85, 30)

            .Button1.Location = New Point(.ReloadBtn.Location.X + .ReloadBtn.Width + 5, .ReloadBtn.Location.Y)
            .Button1.Size = New Size(85, 30)

            '북미 NCAP
            .InjuryLbl1.Location = New Point(10, 10)
            .InjuryLbl1.Size = New Size(.ScoreBoard.Width - 30, 90)
            .Peak_Injury1.Location = New Point(10, .InjuryLbl1.Height + .InjuryLbl1.Location.Y + 3)
            .Peak_Injury1.Size = New Size(.InjuryLbl1.Width, .ScoreBoard.Height - .InjuryLbl1.Height - 46)

            '내수 정면
            .InjuryLbl2.Location = New Point(10, 10)
            .InjuryLbl2.Size = New Size(.ScoreBoard.Width - 30, 90)
            .Peak_Injury_DOM_F.Location = New Point(10, .InjuryLbl2.Height + .InjuryLbl2.Location.Y + 3)
            .Peak_Injury_DOM_F.Size = New Size(.InjuryLbl2.Width, .ScoreBoard.Height - .InjuryLbl2.Height - 46)

            '내수 옵셋
            .InjuryLbl3.Location = New Point(10, 10)
            .InjuryLbl3.Size = New Size(.ScoreBoard.Width - 30, 90)
            .Peak_Injury_DOM_O.Location = New Point(10, .InjuryLbl3.Height + .InjuryLbl3.Location.Y + 3)
            .Peak_Injury_DOM_O.Size = New Size(.InjuryLbl3.Width, .ScoreBoard.Height - .InjuryLbl3.Height - 46)

            '중국 정면
            .InjuryLbl4.Location = New Point(10, 10)
            .InjuryLbl4.Size = New Size(.ScoreBoard.Width - 30, 90)
            .Peak_Injury_CHINA_F.Location = New Point(10, .InjuryLbl4.Height + .InjuryLbl4.Location.Y + 3)
            .Peak_Injury_CHINA_F.Size = New Size(.InjuryLbl4.Width, .ScoreBoard.Height - .InjuryLbl4.Height - 46)

            '중국 옵셋
            .InjuryLbl5.Location = New Point(10, 10)
            .InjuryLbl5.Size = New Size(.ScoreBoard.Width - 30, 90)
            .Peak_Injury_CHINA_O.Location = New Point(10, .InjuryLbl5.Height + .InjuryLbl5.Location.Y + 3)
            .Peak_Injury_CHINA_O.Size = New Size(.InjuryLbl5.Width, .ScoreBoard.Height - .InjuryLbl5.Height - 46)

            '중국 후석 (정면)
            .InjuryLbl6.Location = New Point(10, 10)
            .InjuryLbl6.Size = New Size(.ScoreBoard.Width - 30, 90)
            '.Peak_Injury_CHINA_F_Rear.Location = New Point(10, .InjuryLbl6.Height + .InjuryLbl6.Location.Y + 3)
            '.Peak_Injury_CHINA_F_Rear.Size = New Size(.InjuryLbl6.Width, .ScoreBoard.Height - .InjuryLbl6.Height - 46)
            .CNCAP_Rear.Location = New Point(10, .InjuryLbl6.Height + .InjuryLbl6.Location.Y + 3)
            .CNCAP_Rear.Size = New Size(.InjuryLbl6.Width, .ScoreBoard.Height - .InjuryLbl6.Height - 46 - 30)
            .BtnFind.Location = New Point(.CNCAP_Rear.Location.X + .CNCAP_Rear.Width - 115, .CNCAP_Rear.Location.Y + .CNCAP_Rear.Height)
            .BtnFind.Size = New Size(115, 30)
        End With
        'InjuryDisplayFrm_Resize(sender, SplitterEventArgs.Empty)
    End Sub

    Private Sub BtnFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnFind.Click

        Dim k As Integer = 0

        Sheet_China_F_Rear()
        Sheet_China_O_Rear()

        'For k = 0 To Tot_File - 1
        '    With Me.Peak_Injury_CHINA_F_Rear
        '        .Row = k * 3
        '        .Col = 7
        '        .RowSel = k * 3 + 2
        '        .ColSel = 8
        '        .CellBackColor = Color.White
        '        Dim tmpstr() As String
        '        tmpstr = Split(.get_TextMatrix(k * 3, 7), vbCrLf)
        '        .set_TextMatrix(k * 3, 7, tmpstr(LBound(tmpstr)))
        '        .set_TextMatrix(k * 3, 8, tmpstr(LBound(tmpstr)))
        '        .set_TextMatrix(k * 3 + 1, 7, .get_TextMatrix(k * 3, 7))
        '        .set_TextMatrix(k * 3 + 2, 7, .get_TextMatrix(k * 3, 7))
        '        .set_TextMatrix(k * 3 + 1, 8, .get_TextMatrix(k * 3, 8))
        '        .set_TextMatrix(k * 3 + 2, 8, .get_TextMatrix(k * 3, 8))
        '    End With
        '    With Me.Peak_Injury_CHINA_O_Rear
        '        .Row = k * 3
        '        .Col = 7
        '        .RowSel = k * 3 + 2
        '        .ColSel = 8
        '        .CellBackColor = Color.White
        '        Dim tmpstr() As String
        '        tmpstr = Split(.get_TextMatrix(k * 3, 7), vbCrLf)
        '        .set_TextMatrix(k * 3, 7, tmpstr(LBound(tmpstr)))
        '        .set_TextMatrix(k * 3, 8, tmpstr(LBound(tmpstr)))
        '        .set_TextMatrix(k * 3 + 1, 7, .get_TextMatrix(k * 3, 7))
        '        .set_TextMatrix(k * 3 + 2, 7, .get_TextMatrix(k * 3, 7))
        '        .set_TextMatrix(k * 3 + 1, 8, .get_TextMatrix(k * 3, 8))
        '        .set_TextMatrix(k * 3 + 2, 8, .get_TextMatrix(k * 3, 8))
        '    End With
        'Next

        Dim SelectGraph As New FrmSelGraph


        SelectGraph.ShowDialog(Me)

        'SelGraphDetail
        'Index 0 : 그래프 번호 
        'Index 1 : Drop/Rise
        'Index 2 : 타임윈도우
        'Index 3 : Different값

        '한개씩 데이터를 읽어와서 검사한다.

        '데이터 읽기
        Dim ConversionF() As Decimal
        Dim DATA_Pos(,,) As Integer
        Dim DATA_Tot_Len(,,) As Integer

        DATA_Pos = ParaMeterArr1
        DATA_Tot_Len = ParaMeterArr2
        ConversionF = ConversionFactor

        Dim TimeVals() As Decimal = Nothing
        Dim DataVals() As Double = Nothing


        Dim i As Integer = CInt(SelGraphDetail(0))
        Dim m, n As Integer
        Dim time_interval As Double
        Dim Counter As Integer '매번(한사이클 마다) 확인해야하는 횟수

        Dim IsDrop As Boolean = False
        Dim IsRise As Boolean = False
        Dim MarkerTimeIndex As Integer = 0

        For k = 0 To Tot_File - 1
            '데이터를 읽어온다.
            Analopen.DataReading(Tmp_Path(k), OpenFile.names(k), Graph_Ext(i), _
                    DATA_Pos(i, k, 0), DATA_Pos(i, k, 1), _
                    DATA_Tot_Len(i, k, 0), DATA_Tot_Len(i, k, 1), _
                    TimeVals, DataVals, ConversionF(i))

            time_interval = TimeVals(1) - TimeVals(0)
            Counter = CInt(SelGraphDetail(2) / time_interval)

            Select Case CInt(SelGraphDetail(1))
                Case 1 'Drop
                    For n = 0 To UBound(TimeVals) - Counter
                        '전체 사이클
                        For m = 1 To Counter
                            '하나의 사이클
                            If Math.Abs(DataVals(n) - DataVals(n + m)) >= SelGraphDetail(3) Then
                                If DataVals(n) > DataVals(n + m) Then
                                    IsDrop = True
                                    MarkerTimeIndex = n
                                    Exit For
                                End If
                            End If
                        Next
                        If IsDrop = True Then Exit For
                    Next

                    If IsDrop = True Then
                        With Me.Peak_Injury_CHINA_F_Rear
                            .Row = k * 3
                            .Col = 7
                            .RowSel = k * 3 + 2
                            .ColSel = 8
                            .CellBackColor = Color.Red
                            .set_TextMatrix(k * 3, 7, .get_TextMatrix(k * 3, 7) & vbCrLf & "Drop from " & TimeVals(MarkerTimeIndex) & vbCrLf & Format(DataVals(n) - DataVals(n + m), "##0.0##"))
                            .set_TextMatrix(k * 3, 8, .get_TextMatrix(k * 3, 8) & vbCrLf & "Drop from " & TimeVals(MarkerTimeIndex) & vbCrLf & Format(DataVals(n) - DataVals(n + m), "##0.0##"))
                            .set_TextMatrix(k * 3 + 1, 7, .get_TextMatrix(k * 3, 7))
                            .set_TextMatrix(k * 3 + 2, 7, .get_TextMatrix(k * 3, 7))
                            .set_TextMatrix(k * 3 + 1, 8, .get_TextMatrix(k * 3, 8))
                            .set_TextMatrix(k * 3 + 2, 8, .get_TextMatrix(k * 3, 8))
                        End With
                        With Me.Peak_Injury_CHINA_O_Rear
                            .Row = k * 3
                            .Col = 7
                            .RowSel = k * 3 + 2
                            .ColSel = 8
                            .CellBackColor = Color.Red
                            .set_TextMatrix(k * 3, 7, .get_TextMatrix(k * 3, 7) & vbCrLf & "Drop from " & TimeVals(MarkerTimeIndex) & vbCrLf & Format(DataVals(n) - DataVals(n + m), "##0.0##"))
                            .set_TextMatrix(k * 3, 8, .get_TextMatrix(k * 3, 8) & vbCrLf & "Drop from " & TimeVals(MarkerTimeIndex) & vbCrLf & Format(DataVals(n) - DataVals(n + m), "##0.0##"))
                            .set_TextMatrix(k * 3 + 1, 7, .get_TextMatrix(k * 3, 7))
                            .set_TextMatrix(k * 3 + 2, 7, .get_TextMatrix(k * 3, 7))
                            .set_TextMatrix(k * 3 + 1, 8, .get_TextMatrix(k * 3, 8))
                            .set_TextMatrix(k * 3 + 2, 8, .get_TextMatrix(k * 3, 8))
                        End With
                        IsDrop = False
                    End If
                Case 2 'Rise
                    For n = 0 To UBound(TimeVals) - Counter
                        '전체 사이클
                        For m = 1 To Counter
                            '하나의 사이클
                            If Math.Abs(DataVals(n) - DataVals(n + m)) >= SelGraphDetail(3) Then
                                If DataVals(n) < DataVals(n + m) Then
                                    IsDrop = True
                                    MarkerTimeIndex = n
                                    Exit For
                                End If
                            End If
                        Next
                        If IsDrop = True Then Exit For
                    Next

                    If IsDrop = True Then
                        With Me.Peak_Injury_CHINA_F_Rear
                            .Row = k * 3
                            .Col = 7
                            .RowSel = k * 3 + 2
                            .ColSel = 8
                            .CellBackColor = Color.Red
                            .set_TextMatrix(k * 3, 7, .get_TextMatrix(k * 3, 7) & vbCrLf & "Rise from " & TimeVals(MarkerTimeIndex) & vbCrLf & Format(DataVals(n) - DataVals(n + m), "##0.0##"))
                            .set_TextMatrix(k * 3, 8, .get_TextMatrix(k * 3, 8) & vbCrLf & "Rise from " & TimeVals(MarkerTimeIndex) & vbCrLf & Format(DataVals(n) - DataVals(n + m), "##0.0##"))
                            .set_TextMatrix(k * 3 + 1, 7, .get_TextMatrix(k * 3, 7))
                            .set_TextMatrix(k * 3 + 2, 7, .get_TextMatrix(k * 3, 7))
                            .set_TextMatrix(k * 3 + 1, 8, .get_TextMatrix(k * 3, 8))
                            .set_TextMatrix(k * 3 + 2, 8, .get_TextMatrix(k * 3, 8))
                        End With
                        With Me.Peak_Injury_CHINA_O_Rear
                            .Row = 0
                            .Col = 7
                            .RowSel = 2
                            .ColSel = 8
                            .CellBackColor = Color.Red
                            .set_TextMatrix(k * 3, 7, .get_TextMatrix(k * 3, 7) & vbCrLf & "Rise from " & TimeVals(MarkerTimeIndex) & vbCrLf & Format(DataVals(n) - DataVals(n + m), "##0.0##"))
                            .set_TextMatrix(k * 3, 8, .get_TextMatrix(k * 3, 8) & vbCrLf & "Rise from " & TimeVals(MarkerTimeIndex) & vbCrLf & Format(DataVals(n) - DataVals(n + m), "##0.0##"))
                            .set_TextMatrix(k * 3 + 1, 7, .get_TextMatrix(k * 3, 7))
                            .set_TextMatrix(k * 3 + 2, 7, .get_TextMatrix(k * 3, 7))
                            .set_TextMatrix(k * 3 + 1, 8, .get_TextMatrix(k * 3, 8))
                            .set_TextMatrix(k * 3 + 2, 8, .get_TextMatrix(k * 3, 8))
                        End With
                        IsDrop = False
                    End If
            End Select

        Next

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        If ((Control.ModifierKeys And Keys.Alt) = Keys.Alt) Then IsRemote = True

        MainMDI.ProgressBarMain.Maximum = 1300

        Dim i, formerfileCnt As Integer

        If IsRemote = False Then

            With Me.AddFileDlg
                .Title = "Peak 파일 선택 [Add]"
                .Multiselect = True
                .FileName = ""                   '초기에 표시되는 파일 이름
                .InitialDirectory = Mid(Me.PathLbl.Text, 8)   '초기에 표시되는 폴더 위치 (예약어)
                .Filter = "Peak 파일|*.peak|모든 파일|*.*"
                .ShowDialog()
            End With

            If Me.AddFileDlg.FileNames(0) = "" Then
                MainMDI.Statuslbl.Text = "Cancel"
                MainMDI.ProgressBarMain.Value = MainMDI.ProgressBarMain.Maximum
                'Me.Close()
                Exit Sub
            End If

        End If

        formerfileCnt = Me.OpenFile.names.Count

        If IsRemote = False Then

            ReDim Preserve OpenFile.names(formerfileCnt + Me.AddFileDlg.FileNames.Count - 1)
            ReDim Preserve Tmp_Path((formerfileCnt + Me.AddFileDlg.FileNames.Count - 1))
            ReDim Preserve IsRemotePath(formerfileCnt + Me.AddFileDlg.FileNames.Count - 1)

            EventPass = True

            For i = formerfileCnt To UBound(OpenFile.names)
                OpenFile.names(i) = FileNameGet2(Me.AddFileDlg.SafeFileNames(i - formerfileCnt))
                Tmp_Path(i) = FilePathGet2(Me.AddFileDlg.FileNames(i - formerfileCnt))
                IsRemotePath(i) = ""
            Next

        Else

            If Connected = False Then
                MsgBox("Server connection is required.", MsgBoxStyle.Information, "Try Again")
            End If

            Dim RemoteFolder As New FrmRemoteFileLst("Reading")

            Me.Hide()
            RemoteFolder.ShowDialog(Me)

            If IsNothing(RemoteFilesNames) = False Then

                ReDim Preserve IsRemotePath(formerfileCnt + RemoteFilesNames.Count - 1)
                ReDim Preserve OpenFile.names(formerfileCnt + RemoteFilesNames.Count - 1)
                ReDim Preserve Tmp_Path((formerfileCnt + RemoteFilesNames.Count - 1))

                Me.PathLbl.Text = "Path : " & "[Remote]" & Application.StartupPath & "\TempResults\"
                For i = formerfileCnt To UBound(OpenFile.names)
                    OpenFile.names(i) = RemoteFilesNames(i - formerfileCnt)
                    Tmp_Path(i) = Application.StartupPath & "\TempResults\"
                    IsRemotePath(i) = RemoteFolderName
                Next
            Else
                IsRemote = False
                Exit Sub
            End If

            IsRemote = False

        End If

        '================================================================

        For i = Me.OpenLst.Items.Count - 1 To 0 Step -1
            Me.OpenLst.Items.RemoveAt(i)
        Next

        With Me.OpenLst
            For i = 0 To UBound(OpenFile.names)
                .Items.Insert(i, i + 1 & ". " & OpenFile.names(i))
            Next
            .Height = 105
        End With

        If OpenFile.names.Length > 1 Then Me.BtnValGraph.Enabled = True

        ReDim DummyType(UBound(OpenFile.names))

        MainMDI.ProgressBarMain.Value = 10
        '더미구분
        MainMDI.Statuslbl.Text = "Configure Dummies..."
        Dummy(UBound(OpenFile.names), Tmp_Path)

        MainMDI.ProgressBarMain.Value = 50
        ''상해를 읽는다
        'Reading_Peak()

        If OpenFile.names.Length > 5 Then
            Me.Height = 780
        Else
            Me.Height = 630
        End If

        Peak_Injury1.Rows = 1
        Peak_Injury1.Cols = 1
        Peak_Injury_DOM_F.Rows = 1
        Peak_Injury_DOM_O.Cols = 1
        Peak_Injury_CHINA_O.Rows = 1
        Peak_Injury_CHINA_F.Cols = 1
        Peak_Injury_CHINA_F_Rear.Rows = 1
        Peak_Injury_CHINA_F_Rear.Cols = 1
        Peak_Injury_CHINA_O_Rear.Rows = 1
        Peak_Injury_CHINA_O_Rear.Cols = 1

        '그래프 파라미터를 미리 읽어온다.
        Dim RowCol(1) As Integer

        ReDim ParaMeterArr1(31, UBound(OpenFile.names), 1)
        '{그래프 번호 , Case 번호 , 데이터위치(Row/Col) )
        ReDim ParaMeterArr2(31, UBound(OpenFile.names), 1)
        '{그래프 번호 , Case 번호 , 총채널수(Row/Col) )
        'ReDim Preserve ConversionFactor(31)
        '{그래프 Conversion Factor - 단위변환용}

        MainMDI.Statuslbl.Text = "Loading DATA Location Parameters"
        For i = 0 To UBound(OpenFile.names)
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(0), Graph_01)
            ParaMeterArr1(0, i, 0) = RowCol(0)
            ParaMeterArr1(0, i, 1) = RowCol(1)
            ParaMeterArr2(0, i, 0) = RowCol(2)
            ParaMeterArr2(0, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 520
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(1), Graph_02)
            ParaMeterArr1(1, i, 0) = RowCol(0)
            ParaMeterArr1(1, i, 1) = RowCol(1)
            ParaMeterArr2(1, i, 0) = RowCol(2)
            ParaMeterArr2(1, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 540
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(2), Graph_03)
            ParaMeterArr1(2, i, 0) = RowCol(0)
            ParaMeterArr1(2, i, 1) = RowCol(1)
            ParaMeterArr2(2, i, 0) = RowCol(2)
            ParaMeterArr2(2, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 560
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(3), Graph_04)
            ParaMeterArr1(3, i, 0) = RowCol(0)
            ParaMeterArr1(3, i, 1) = RowCol(1)
            ParaMeterArr2(3, i, 0) = RowCol(2)
            ParaMeterArr2(3, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 580
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(4), Graph_05)
            ParaMeterArr1(4, i, 0) = RowCol(0)
            ParaMeterArr1(4, i, 1) = RowCol(1)
            ParaMeterArr2(4, i, 0) = RowCol(2)
            ParaMeterArr2(4, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 600
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(5), Graph_06)
            ParaMeterArr1(5, i, 0) = RowCol(0)
            ParaMeterArr1(5, i, 1) = RowCol(1)
            ParaMeterArr2(5, i, 0) = RowCol(2)
            ParaMeterArr2(5, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 620
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(6), Graph_07)
            ParaMeterArr1(6, i, 0) = RowCol(0)
            ParaMeterArr1(6, i, 1) = RowCol(1)
            ParaMeterArr2(6, i, 0) = RowCol(2)
            ParaMeterArr2(6, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 640
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(7), Graph_08)
            ParaMeterArr1(7, i, 0) = RowCol(0)
            ParaMeterArr1(7, i, 1) = RowCol(1)
            ParaMeterArr2(7, i, 0) = RowCol(2)
            ParaMeterArr2(7, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 660
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(8), Graph_09)
            ParaMeterArr1(8, i, 0) = RowCol(0)
            ParaMeterArr1(8, i, 1) = RowCol(1)
            ParaMeterArr2(8, i, 0) = RowCol(2)
            ParaMeterArr2(8, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 680
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(9), Graph_10)
            ParaMeterArr1(9, i, 0) = RowCol(0)
            ParaMeterArr1(9, i, 1) = RowCol(1)
            ParaMeterArr2(9, i, 0) = RowCol(2)
            ParaMeterArr2(9, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 700
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(10), Graph_11)
            ParaMeterArr1(10, i, 0) = RowCol(0)
            ParaMeterArr1(10, i, 1) = RowCol(1)
            ParaMeterArr2(10, i, 0) = RowCol(2)
            ParaMeterArr2(10, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 720
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(11), Graph_12)
            ParaMeterArr1(11, i, 0) = RowCol(0)
            ParaMeterArr1(11, i, 1) = RowCol(1)
            ParaMeterArr2(11, i, 0) = RowCol(2)
            ParaMeterArr2(11, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 740
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(12), Graph_13)
            ParaMeterArr1(12, i, 0) = RowCol(0)
            ParaMeterArr1(12, i, 1) = RowCol(1)
            ParaMeterArr2(12, i, 0) = RowCol(2)
            ParaMeterArr2(12, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 760
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(13), Graph_14)
            ParaMeterArr1(13, i, 0) = RowCol(0)
            ParaMeterArr1(13, i, 1) = RowCol(1)
            ParaMeterArr2(13, i, 0) = RowCol(2)
            ParaMeterArr2(13, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 780
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(14), Graph_15)
            ParaMeterArr1(14, i, 0) = RowCol(0)
            ParaMeterArr1(14, i, 1) = RowCol(1)
            ParaMeterArr2(14, i, 0) = RowCol(2)
            ParaMeterArr2(14, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 800
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(15), Graph_16)
            ParaMeterArr1(15, i, 0) = RowCol(0)
            ParaMeterArr1(15, i, 1) = RowCol(1)
            ParaMeterArr2(15, i, 0) = RowCol(2)
            ParaMeterArr2(15, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 820
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(16), Graph_17)
            ParaMeterArr1(16, i, 0) = RowCol(0)
            ParaMeterArr1(16, i, 1) = RowCol(1)
            ParaMeterArr2(16, i, 0) = RowCol(2)
            ParaMeterArr2(16, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 840
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(17), Graph_18)
            ParaMeterArr1(17, i, 0) = RowCol(0)
            ParaMeterArr1(17, i, 1) = RowCol(1)
            ParaMeterArr2(17, i, 0) = RowCol(2)
            ParaMeterArr2(17, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 860
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(18), Graph_19)
            ParaMeterArr1(18, i, 0) = RowCol(0)
            ParaMeterArr1(18, i, 1) = RowCol(1)
            ParaMeterArr2(18, i, 0) = RowCol(2)
            ParaMeterArr2(18, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 880
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(19), Graph_20)
            ParaMeterArr1(19, i, 0) = RowCol(0)
            ParaMeterArr1(19, i, 1) = RowCol(1)
            ParaMeterArr2(19, i, 0) = RowCol(2)
            ParaMeterArr2(19, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 900
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(20), Graph_21)
            ParaMeterArr1(20, i, 0) = RowCol(0)
            ParaMeterArr1(20, i, 1) = RowCol(1)
            ParaMeterArr2(20, i, 0) = RowCol(2)
            ParaMeterArr2(20, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 920
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(21), Graph_22)
            ParaMeterArr1(21, i, 0) = RowCol(0)
            ParaMeterArr1(21, i, 1) = RowCol(1)
            ParaMeterArr2(21, i, 0) = RowCol(2)
            ParaMeterArr2(21, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 940
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(22), Graph_23)
            ParaMeterArr1(22, i, 0) = RowCol(0)
            ParaMeterArr1(22, i, 1) = RowCol(1)
            ParaMeterArr2(22, i, 0) = RowCol(2)
            ParaMeterArr2(22, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 960
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(23), Graph_24)
            ParaMeterArr1(23, i, 0) = RowCol(0)
            ParaMeterArr1(23, i, 1) = RowCol(1)
            ParaMeterArr2(23, i, 0) = RowCol(2)
            ParaMeterArr2(23, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 980
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(24), Graph_25)
            ParaMeterArr1(24, i, 0) = RowCol(0)
            ParaMeterArr1(24, i, 1) = RowCol(1)
            ParaMeterArr2(24, i, 0) = RowCol(2)
            ParaMeterArr2(24, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 1000
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(25), Graph_26)
            ParaMeterArr1(25, i, 0) = RowCol(0)
            ParaMeterArr1(25, i, 1) = RowCol(1)
            ParaMeterArr2(25, i, 0) = RowCol(2)
            ParaMeterArr2(25, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 1020
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(26), Graph_27)
            ParaMeterArr1(26, i, 0) = RowCol(0)
            ParaMeterArr1(26, i, 1) = RowCol(1)
            ParaMeterArr2(26, i, 0) = RowCol(2)
            ParaMeterArr2(26, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 1040
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(27), Graph_28)
            ParaMeterArr1(27, i, 0) = RowCol(0)
            ParaMeterArr1(27, i, 1) = RowCol(1)
            ParaMeterArr2(27, i, 0) = RowCol(2)
            ParaMeterArr2(27, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = MainMDI.ProgressBarMain.Maximum - 80
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(28), Graph_29)
            ParaMeterArr1(28, i, 0) = RowCol(0)
            ParaMeterArr1(28, i, 1) = RowCol(1)
            ParaMeterArr2(28, i, 0) = RowCol(2)
            ParaMeterArr2(28, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = MainMDI.ProgressBarMain.Maximum - 60
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(29), Graph_30)
            ParaMeterArr1(29, i, 0) = RowCol(0)
            ParaMeterArr1(29, i, 1) = RowCol(1)
            ParaMeterArr2(29, i, 0) = RowCol(2)
            ParaMeterArr2(29, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = MainMDI.ProgressBarMain.Maximum - 40
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(30), Graph_31)
            ParaMeterArr1(30, i, 0) = RowCol(0)
            ParaMeterArr1(30, i, 1) = RowCol(1)
            ParaMeterArr2(30, i, 0) = RowCol(2)
            ParaMeterArr2(30, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = MainMDI.ProgressBarMain.Maximum - 20
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext(31), Graph_32)
            ParaMeterArr1(31, i, 0) = RowCol(0)
            ParaMeterArr1(31, i, 1) = RowCol(1)
            ParaMeterArr2(31, i, 0) = RowCol(2)
            ParaMeterArr2(31, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = MainMDI.ProgressBarMain.Maximum
        Next

        'Conversion Factor Load and Correlation Factor Load
        '단위 환산 계수와 상관성 수치를 동시에 로드한다.
        ConversionFactor(0) = CDec(Graph_01(UBound(Graph_01))) * GraphCorrel(0)
        ConversionFactor(1) = CDec(Graph_02(UBound(Graph_02))) * GraphCorrel(1)
        ConversionFactor(2) = CDec(Graph_03(UBound(Graph_03))) * GraphCorrel(2)
        ConversionFactor(3) = CDec(Graph_04(UBound(Graph_04))) * GraphCorrel(3)
        ConversionFactor(4) = CDec(Graph_05(UBound(Graph_05))) * GraphCorrel(4)
        ConversionFactor(5) = CDec(Graph_06(UBound(Graph_06))) * GraphCorrel(5)
        ConversionFactor(6) = CDec(Graph_07(UBound(Graph_07))) * GraphCorrel(6)
        ConversionFactor(7) = CDec(Graph_08(UBound(Graph_08))) * GraphCorrel(7)
        ConversionFactor(8) = CDec(Graph_09(UBound(Graph_09))) * GraphCorrel(8)
        ConversionFactor(9) = CDec(Graph_10(UBound(Graph_10))) * GraphCorrel(9)
        ConversionFactor(10) = CDec(Graph_11(UBound(Graph_11))) * GraphCorrel(10)
        ConversionFactor(11) = CDec(Graph_12(UBound(Graph_12))) * GraphCorrel(11)
        ConversionFactor(12) = CDec(Graph_13(UBound(Graph_13))) * GraphCorrel(12)
        ConversionFactor(13) = CDec(Graph_14(UBound(Graph_14))) * GraphCorrel(13)
        ConversionFactor(14) = CDec(Graph_15(UBound(Graph_15))) * GraphCorrel(14)
        ConversionFactor(15) = CDec(Graph_16(UBound(Graph_16))) * GraphCorrel(15)
        ConversionFactor(16) = CDec(Graph_17(UBound(Graph_17))) * GraphCorrel(16)
        ConversionFactor(17) = CDec(Graph_18(UBound(Graph_18))) * GraphCorrel(17)
        ConversionFactor(18) = CDec(Graph_19(UBound(Graph_19))) * GraphCorrel(18)
        ConversionFactor(19) = CDec(Graph_20(UBound(Graph_20))) * GraphCorrel(19)
        ConversionFactor(20) = CDec(Graph_21(UBound(Graph_21))) * GraphCorrel(20)
        ConversionFactor(21) = CDec(Graph_22(UBound(Graph_22))) * GraphCorrel(21)
        ConversionFactor(22) = CDec(Graph_23(UBound(Graph_23))) * GraphCorrel(22)
        ConversionFactor(23) = CDec(Graph_24(UBound(Graph_24))) * GraphCorrel(23)
        ConversionFactor(24) = CDec(Graph_25(UBound(Graph_25))) * GraphCorrel(24)
        ConversionFactor(25) = CDec(Graph_26(UBound(Graph_26))) * GraphCorrel(25)
        ConversionFactor(26) = CDec(Graph_27(UBound(Graph_27))) * GraphCorrel(26)
        ConversionFactor(27) = CDec(Graph_28(UBound(Graph_28))) * GraphCorrel(27)
        ConversionFactor(28) = CDec(Graph_29(UBound(Graph_29))) * GraphCorrel(28)
        ConversionFactor(29) = CDec(Graph_30(UBound(Graph_30))) * GraphCorrel(29)
        ConversionFactor(30) = CDec(Graph_31(UBound(Graph_31))) * GraphCorrel(30)
        ConversionFactor(31) = CDec(Graph_32(UBound(Graph_32))) * GraphCorrel(31)

        '상해를 읽는다
        Reading_Peak()

        '시트를 그린다.
        MainMDI.Statuslbl.Text = "Drawing Injury Sheets"
        MainMDI.ProgressBarMain.Value = 100
        'Sheet_Lbl()
        Sheet_US()

        MainMDI.ProgressBarMain.Value = 200
        'Sheet_Lbl2()
        Sheet_DOM_F()

        MainMDI.ProgressBarMain.Value = 300
        'Sheet_Lbl3()
        Sheet_DOM_O()

        MainMDI.ProgressBarMain.Value = 400
        'Sheet_Lbl4()
        Sheet_China_F()

        'Sheet_Lbl5()
        Sheet_China_0()

        MainMDI.ProgressBarMain.Value = 500

        'Sheet_Lbl6()
        Sheet_China_F_Rear()
        Sheet_China_O_Rear()

        MainMDI.ProgressBarMain.Value = 550

        Sheet_Euro_F()
        Sheet_Euro_Rear()

        MainMDI.ProgressBarMain.Value = 600

        Sheet_Euro_O()

        MainMDI.ProgressBarMain.Value = MainMDI.ProgressBarMain.Maximum

        Me.Show()

        FileDrop = False
        DragFiles = Nothing

        MainMDI.Statuslbl.Text = "Complete"

        Me.Width = 1100

        EventPass = False

    End Sub

    Private Sub OpenLst_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenLst.SelectedIndexChanged

        If EventPass = True Then
            Exit Sub
        End If

        Me.DescripTxt.Text = ""

        ' 참고 사이트
        ' https://social.msdn.microsoft.com/Forums/en-US/d4f32bef-df95-4f8d-91a9-5305418239ff/vbnet-how-use-file-properties-using-code?forum=vblanguage
        ' 기본적으로 제공되는 FileInfo가 ReadOnly 속성이므로
        ' 이러한 방법을 사용함
        Me.DescripTxt.ForeColor = Color.Black

        Try
            Dim myFile As String = Tmp_Path(Me.OpenLst.SelectedIndex) & OpenFile.names(Me.OpenLst.SelectedIndex) & ".xml"
            Dim myDSO As DSOFile.OleDocumentProperties = New DSOFile.OleDocumentProperties
            myDSO.Open(myFile, True, dsoFileOpenOptions.dsoOptionOpenReadOnlyIfNoWriteAccess)
            Me.DescripTxt.Text = myDSO.SummaryProperties.Comments.ToString
            myDSO.Close()
        Catch ex As Exception
            Me.DescripTxt.Text = "1. Cannot Read Comments." & vbCrLf & "2. Empty String Set"
        End Try

        Try
            Me.PathLbl.Text = "Path : " & Tmp_Path(Me.OpenLst.SelectedIndex)
        Catch ex As Exception
            Me.PathLbl.Text = "Path : "
        End Try
        Try
            MainMDI.Statuslbl.Text = Me.OpenLst.Items(Me.OpenLst.SelectedIndex)
        Catch ex As Exception
            MainMDI.Statuslbl.Text = "Empty"
        End Try

    End Sub

    Private Sub DescripTxt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DescripTxt.KeyDown
        'Dim XMLfile_Info As FileVersionInfo = FileVersionInfo.GetVersionInfo(Tmp_Path(Me.OpenLst.SelectedIndex) & OpenFile.names(Me.OpenLst.SelectedIndex) & ".xml")
        'MsgBox(XMLfile_Info.FileDescription)

        ' 참고 사이트
        ' https://social.msdn.microsoft.com/Forums/en-US/d4f32bef-df95-4f8d-91a9-5305418239ff/vbnet-how-use-file-properties-using-code?forum=vblanguage
        ' 기본적으로 제공되는 FileInfo가 ReadOnly 속성이므로
        ' 이러한 방법을 사용함


        'If (e.KeyCode And Not Keys.Modifiers) = Keys.T AndAlso e.Modifiers = Keys.ControlKey Then
        If e.Modifiers = Keys.Control And e.KeyCode = Keys.S Then
            Try
                Dim myFile As String = Tmp_Path(Me.OpenLst.SelectedIndex) & OpenFile.names(Me.OpenLst.SelectedIndex) & ".xml"
                Dim myDSO As DSOFile.OleDocumentProperties = New DSOFile.OleDocumentProperties
                myDSO.Open(myFile, False, dsoFileOpenOptions.dsoOptionUseMBCStringsForNewSets)
                myDSO.SummaryProperties.Comments = DescripTxt.Text
                myDSO.Save()
                myDSO.Close()
                MainMDI.Statuslbl.Text = "Save Comments Successfully"
            Catch ex As Exception
                MainMDI.Statuslbl.Text = "Fail to Save a comments... Try to Install Microsoft KB224351_x86"
                If Directory.Exists("C:\DsoFile") Then
                Else
                    Dim result As Integer = MessageBox.Show("To Use DSO function " & vbCrLf & "Need to Intall KB224351.....OK??", "Try Again", MessageBoxButtons.YesNo)
                    If result = DialogResult.Yes Then
                        Dim PROC As Integer
                        PROC = Shell(Application.StartupPath & "\DsoFileSetup_KB224351_x86.exe", AppWinStyle.NormalFocus)
                    End If
                End If
            End Try
        End If

    End Sub

End Class