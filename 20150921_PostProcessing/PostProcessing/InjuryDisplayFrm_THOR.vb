Option Explicit On

Imports System.IO
Imports System.Diagnostics
Imports AxMSFlexGridLib
Imports Microsoft.Office.Interop
Imports Microsoft.Office.Core
Imports DSOFile

Public Class InjuryDisplayFrm_THOR

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
    Dim InjuryCorrel(29) As Single
    Dim GraphCorrel(47) As Single

    '상해저장 변수
    Public Head3MSG() As Double     'H3MS_inj ====================='THOR 없음
    Public Chest_G_CUMULATIVE_T3MS_inj() As Double      'T3MS_inj
    Public HIC15() As Double        'HIC15_inj
    Public HIC36() As Double        'HIC36_inj
    Public NTE() As Double      'NTE_inj
    Public NTF() As Double      'NTF_inj
    Public NCE() As Double      'NCE_inj
    Public NCF() As Double      'NCF_inj
    Public Head_Peak_G() As Double      'HaccRpeak_inj ====================='THOR 없음
    'Public Chest_G() As Double      'TaccRpeak_inj
    Public Chest_D() As Double      'ThCC_inj
    Public KneeS_L() As Double      'kneesliderL_inj
    Public KneeS_R() As Double      'kneesliderR_inj
    Public Tibia_Comp_L() As Double     'TCFCLowL_inj
    Public Tibia_Comp_R() As Double     'TCFCLowR_inj
    Public TI_upr_L() As Double     'TIUpL_inj
    Public TI_lwr_L() As Double     'TILowL_inj
    Public TI_upr_R() As Double     'TIUpR_inj
    Public TI_lwr_R() As Double     'TILowR_inj
    Public Chest_VC() As Double     'VC_inj_CFC180 ====================='THOR 없음
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
    Public AngularVelX() As Double
    Public AngularVelY() As Double
    Public AngularVelZ() As Double
    Public BrIC() As Double
    '=========================================================

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

    Public Sub New(ByVal Temp_Injury_Correl() As Single, ByVal Temp_Graph_Correl() As Single)

        ' 이 호출은 디자이너에 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하십시오.
        '상관성 계수-Correlation Factor 저장 변수
        InjuryCorrel = Temp_Injury_Correl
        GraphCorrel = Temp_Graph_Correl

    End Sub

    Private Sub InjuryDisplayFrm_THOR_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F5 Then
            Call ReloadBtn_Click(e, Nothing)
        End If
    End Sub

    Private Sub InjuryDisplayFrm_THOR_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        MainMDI.ProgressBarMain.Value = 0
        MainMDI.ProgressBarMain.Maximum = 1300

        Me.Hide()
        Me.MdiParent = MainMDI

        Me.CommentsToolTip.SetToolTip(Me.DescripTxt, "To Save a Comments" & vbCrLf & " → Ctrl + S")

        If TempLicExpire = False Then
            '원격로그
            Try
                Using NewfileNum As StreamWriter = File.AppendText(RemoteValFolder & System.Environment.UserName)
                    NewfileNum.WriteLine("## Injury Display Form[THOR] Open : " & Date.Now.ToString & " ##")
                    NewfileNum.Close()
                End Using
            Catch ex As Exception
                'End
            End Try
        End If

        Dim i As Integer

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

        End If

            Me.ScoreBoard.TabPages(0).Text = "US-NCAP [THOR]"

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
            ''**********************  상해를 읽는다  *****************************
            'Reading_Peak()
            ''**********************  상해를 읽는다  *****************************

            If OpenFile.names.Length > 5 Then
                Me.Height = 780
            Else
                Me.Height = 630
            End If

            '시트를 그린다.
            MainMDI.Statuslbl.Text = "Drawing Injury Sheets"
            MainMDI.ProgressBarMain.Value = 100

            ''********************** 여기에서 상해 시트를 그리는 프로시저 호출 **************
            'Sheet_Lbl()
            'Sheet_US()
            ''********************** 여기에서 상해 시트를 그리는 프로시저 호출 **************

            '그래프 파라미터를 미리 읽어온다.
            Dim RowCol(1) As Integer

            ReDim ParaMeterArr1(47, UBound(OpenFile.names), 1)
            '{그래프 번호 , Case 번호 , 데이터위치(Row/Col) )
            ReDim ParaMeterArr2(47, UBound(OpenFile.names), 1)
            '{그래프 번호 , Case 번호 , 총채널수(Row/Col) )
            ReDim ConversionFactor(47)
            '{그래프 Conversion Factor - 단위변환용}

            MainMDI.Statuslbl.Text = "Loading DATA Location Parameters"

            For i = 0 To UBound(OpenFile.names)
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(0), Graph_01_THOR)
                ParaMeterArr1(0, i, 0) = RowCol(0)
                ParaMeterArr1(0, i, 1) = RowCol(1)
                ParaMeterArr2(0, i, 0) = RowCol(2)
                ParaMeterArr2(0, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 520
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(1), Graph_02_THOR)
                ParaMeterArr1(1, i, 0) = RowCol(0)
                ParaMeterArr1(1, i, 1) = RowCol(1)
                ParaMeterArr2(1, i, 0) = RowCol(2)
                ParaMeterArr2(1, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 530
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(2), Graph_03_THOR)
                ParaMeterArr1(2, i, 0) = RowCol(0)
                ParaMeterArr1(2, i, 1) = RowCol(1)
                ParaMeterArr2(2, i, 0) = RowCol(2)
                ParaMeterArr2(2, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 540
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(3), Graph_04_THOR)
                ParaMeterArr1(3, i, 0) = RowCol(0)
                ParaMeterArr1(3, i, 1) = RowCol(1)
                ParaMeterArr2(3, i, 0) = RowCol(2)
                ParaMeterArr2(3, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 550
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(4), Graph_05_THOR)
                ParaMeterArr1(4, i, 0) = RowCol(0)
                ParaMeterArr1(4, i, 1) = RowCol(1)
                ParaMeterArr2(4, i, 0) = RowCol(2)
                ParaMeterArr2(4, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 560
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(5), Graph_06_THOR)
                ParaMeterArr1(5, i, 0) = RowCol(0)
                ParaMeterArr1(5, i, 1) = RowCol(1)
                ParaMeterArr2(5, i, 0) = RowCol(2)
                ParaMeterArr2(5, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 570
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(6), Graph_07_THOR)
                ParaMeterArr1(6, i, 0) = RowCol(0)
                ParaMeterArr1(6, i, 1) = RowCol(1)
                ParaMeterArr2(6, i, 0) = RowCol(2)
                ParaMeterArr2(6, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 580
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(7), Graph_08_THOR)
                ParaMeterArr1(7, i, 0) = RowCol(0)
                ParaMeterArr1(7, i, 1) = RowCol(1)
                ParaMeterArr2(7, i, 0) = RowCol(2)
                ParaMeterArr2(7, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 590
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(8), Graph_09_THOR)
                ParaMeterArr1(8, i, 0) = RowCol(0)
                ParaMeterArr1(8, i, 1) = RowCol(1)
                ParaMeterArr2(8, i, 0) = RowCol(2)
                ParaMeterArr2(8, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 600
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(9), Graph_10_THOR)
                ParaMeterArr1(9, i, 0) = RowCol(0)
                ParaMeterArr1(9, i, 1) = RowCol(1)
                ParaMeterArr2(9, i, 0) = RowCol(2)
                ParaMeterArr2(9, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 610
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(10), Graph_11_THOR)
                ParaMeterArr1(10, i, 0) = RowCol(0)
                ParaMeterArr1(10, i, 1) = RowCol(1)
                ParaMeterArr2(10, i, 0) = RowCol(2)
                ParaMeterArr2(10, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 620
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(11), Graph_12_THOR)
                ParaMeterArr1(11, i, 0) = RowCol(0)
                ParaMeterArr1(11, i, 1) = RowCol(1)
                ParaMeterArr2(11, i, 0) = RowCol(2)
                ParaMeterArr2(11, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 630
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(12), Graph_13_THOR)
                ParaMeterArr1(12, i, 0) = RowCol(0)
                ParaMeterArr1(12, i, 1) = RowCol(1)
                ParaMeterArr2(12, i, 0) = RowCol(2)
                ParaMeterArr2(12, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 640
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(13), Graph_14_THOR)
                ParaMeterArr1(13, i, 0) = RowCol(0)
                ParaMeterArr1(13, i, 1) = RowCol(1)
                ParaMeterArr2(13, i, 0) = RowCol(2)
                ParaMeterArr2(13, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 650
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(14), Graph_15_THOR)
                ParaMeterArr1(14, i, 0) = RowCol(0)
                ParaMeterArr1(14, i, 1) = RowCol(1)
                ParaMeterArr2(14, i, 0) = RowCol(2)
                ParaMeterArr2(14, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 660
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(15), Graph_16_THOR)
                ParaMeterArr1(15, i, 0) = RowCol(0)
                ParaMeterArr1(15, i, 1) = RowCol(1)
                ParaMeterArr2(15, i, 0) = RowCol(2)
                ParaMeterArr2(15, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 670
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(16), Graph_17_THOR)
                ParaMeterArr1(16, i, 0) = RowCol(0)
                ParaMeterArr1(16, i, 1) = RowCol(1)
                ParaMeterArr2(16, i, 0) = RowCol(2)
                ParaMeterArr2(16, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 680
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(17), Graph_18_THOR)
                ParaMeterArr1(17, i, 0) = RowCol(0)
                ParaMeterArr1(17, i, 1) = RowCol(1)
                ParaMeterArr2(17, i, 0) = RowCol(2)
                ParaMeterArr2(17, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 690
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(18), Graph_19_THOR)
                ParaMeterArr1(18, i, 0) = RowCol(0)
                ParaMeterArr1(18, i, 1) = RowCol(1)
                ParaMeterArr2(18, i, 0) = RowCol(2)
                ParaMeterArr2(18, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 700
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(19), Graph_20_THOR)
                ParaMeterArr1(19, i, 0) = RowCol(0)
                ParaMeterArr1(19, i, 1) = RowCol(1)
                ParaMeterArr2(19, i, 0) = RowCol(2)
                ParaMeterArr2(19, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 710
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(20), Graph_21_THOR)
                ParaMeterArr1(20, i, 0) = RowCol(0)
                ParaMeterArr1(20, i, 1) = RowCol(1)
                ParaMeterArr2(20, i, 0) = RowCol(2)
                ParaMeterArr2(20, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 720
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(21), Graph_22_THOR)
                ParaMeterArr1(21, i, 0) = RowCol(0)
                ParaMeterArr1(21, i, 1) = RowCol(1)
                ParaMeterArr2(21, i, 0) = RowCol(2)
                ParaMeterArr2(21, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 730
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(22), Graph_23_THOR)
                ParaMeterArr1(22, i, 0) = RowCol(0)
                ParaMeterArr1(22, i, 1) = RowCol(1)
                ParaMeterArr2(22, i, 0) = RowCol(2)
                ParaMeterArr2(22, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 740
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(23), Graph_24_THOR)
                ParaMeterArr1(23, i, 0) = RowCol(0)
                ParaMeterArr1(23, i, 1) = RowCol(1)
                ParaMeterArr2(23, i, 0) = RowCol(2)
                ParaMeterArr2(23, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 750
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(24), Graph_25_THOR)
                ParaMeterArr1(24, i, 0) = RowCol(0)
                ParaMeterArr1(24, i, 1) = RowCol(1)
                ParaMeterArr2(24, i, 0) = RowCol(2)
                ParaMeterArr2(24, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 760
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(25), Graph_26_THOR)
                ParaMeterArr1(25, i, 0) = RowCol(0)
                ParaMeterArr1(25, i, 1) = RowCol(1)
                ParaMeterArr2(25, i, 0) = RowCol(2)
                ParaMeterArr2(25, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 770
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(26), Graph_27_THOR)
                ParaMeterArr1(26, i, 0) = RowCol(0)
                ParaMeterArr1(26, i, 1) = RowCol(1)
                ParaMeterArr2(26, i, 0) = RowCol(2)
                ParaMeterArr2(26, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 780
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(27), Graph_28_THOR)
                ParaMeterArr1(27, i, 0) = RowCol(0)
                ParaMeterArr1(27, i, 1) = RowCol(1)
                ParaMeterArr2(27, i, 0) = RowCol(2)
                ParaMeterArr2(27, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 790
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(28), Graph_29_THOR)
                ParaMeterArr1(28, i, 0) = RowCol(0)
                ParaMeterArr1(28, i, 1) = RowCol(1)
                ParaMeterArr2(28, i, 0) = RowCol(2)
                ParaMeterArr2(28, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 800
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(29), Graph_30_THOR)
                ParaMeterArr1(29, i, 0) = RowCol(0)
                ParaMeterArr1(29, i, 1) = RowCol(1)
                ParaMeterArr2(29, i, 0) = RowCol(2)
                ParaMeterArr2(29, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 810
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(30), Graph_31_THOR)
                ParaMeterArr1(30, i, 0) = RowCol(0)
                ParaMeterArr1(30, i, 1) = RowCol(1)
                ParaMeterArr2(30, i, 0) = RowCol(2)
                ParaMeterArr2(30, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 820
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(31), Graph_32_THOR)
                ParaMeterArr1(31, i, 0) = RowCol(0)
                ParaMeterArr1(31, i, 1) = RowCol(1)
                ParaMeterArr2(31, i, 0) = RowCol(2)
                ParaMeterArr2(31, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 830
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(32), Graph_33_THOR)
                ParaMeterArr1(32, i, 0) = RowCol(0)
                ParaMeterArr1(32, i, 1) = RowCol(1)
                ParaMeterArr2(32, i, 0) = RowCol(2)
                ParaMeterArr2(32, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 840
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(33), Graph_34_THOR)
                ParaMeterArr1(33, i, 0) = RowCol(0)
                ParaMeterArr1(33, i, 1) = RowCol(1)
                ParaMeterArr2(33, i, 0) = RowCol(2)
                ParaMeterArr2(33, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 850
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(34), Graph_35_THOR)
                ParaMeterArr1(34, i, 0) = RowCol(0)
                ParaMeterArr1(34, i, 1) = RowCol(1)
                ParaMeterArr2(34, i, 0) = RowCol(2)
                ParaMeterArr2(34, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 860
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(35), Graph_36_THOR)
                ParaMeterArr1(35, i, 0) = RowCol(0)
                ParaMeterArr1(35, i, 1) = RowCol(1)
                ParaMeterArr2(35, i, 0) = RowCol(2)
                ParaMeterArr2(35, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 870
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(36), Graph_37_THOR)
                ParaMeterArr1(36, i, 0) = RowCol(0)
                ParaMeterArr1(36, i, 1) = RowCol(1)
                ParaMeterArr2(36, i, 0) = RowCol(2)
                ParaMeterArr2(36, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 880
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(37), Graph_38_THOR)
                ParaMeterArr1(37, i, 0) = RowCol(0)
                ParaMeterArr1(37, i, 1) = RowCol(1)
                ParaMeterArr2(37, i, 0) = RowCol(2)
                ParaMeterArr2(37, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 890
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(38), Graph_39_THOR)
                ParaMeterArr1(38, i, 0) = RowCol(0)
                ParaMeterArr1(38, i, 1) = RowCol(1)
                ParaMeterArr2(38, i, 0) = RowCol(2)
                ParaMeterArr2(38, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 900
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(39), Graph_40_THOR)
                ParaMeterArr1(39, i, 0) = RowCol(0)
                ParaMeterArr1(39, i, 1) = RowCol(1)
                ParaMeterArr2(39, i, 0) = RowCol(2)
                ParaMeterArr2(39, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 910
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(40), Graph_41_THOR)
                ParaMeterArr1(40, i, 0) = RowCol(0)
                ParaMeterArr1(40, i, 1) = RowCol(1)
                ParaMeterArr2(40, i, 0) = RowCol(2)
                ParaMeterArr2(40, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 920
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(41), Graph_42_THOR)
                ParaMeterArr1(41, i, 0) = RowCol(0)
                ParaMeterArr1(41, i, 1) = RowCol(1)
                ParaMeterArr2(41, i, 0) = RowCol(2)
                ParaMeterArr2(41, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 930
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(42), Graph_43_THOR)
                ParaMeterArr1(42, i, 0) = RowCol(0)
                ParaMeterArr1(42, i, 1) = RowCol(1)
                ParaMeterArr2(42, i, 0) = RowCol(2)
                ParaMeterArr2(42, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 940
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(43), Graph_44_THOR)
                ParaMeterArr1(43, i, 0) = RowCol(0)
                ParaMeterArr1(43, i, 1) = RowCol(1)
                ParaMeterArr2(43, i, 0) = RowCol(2)
                ParaMeterArr2(43, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 950
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(44), Graph_45_THOR)
                ParaMeterArr1(44, i, 0) = RowCol(0)
                ParaMeterArr1(44, i, 1) = RowCol(1)
                ParaMeterArr2(44, i, 0) = RowCol(2)
                ParaMeterArr2(44, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 960
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(45), Graph_46_THOR)
                ParaMeterArr1(45, i, 0) = RowCol(0)
                ParaMeterArr1(45, i, 1) = RowCol(1)
                ParaMeterArr2(45, i, 0) = RowCol(2)
                ParaMeterArr2(45, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 970
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(46), Graph_47_THOR)
                ParaMeterArr1(46, i, 0) = RowCol(0)
                ParaMeterArr1(46, i, 1) = RowCol(1)
                ParaMeterArr2(46, i, 0) = RowCol(2)
                ParaMeterArr2(46, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 980
                RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(47), Graph_48_THOR)
                ParaMeterArr1(47, i, 0) = RowCol(0)
                ParaMeterArr1(47, i, 1) = RowCol(1)
                ParaMeterArr2(47, i, 0) = RowCol(2)
                ParaMeterArr2(47, i, 1) = RowCol(3)
                MainMDI.ProgressBarMain.Value = 990

            Next

            'Conversion Factor Load and Correlation Factor Load
            '단위 환산 계수와 상관성 수치를 동시에 로드한다.
            ConversionFactor(0) = CDec(Graph_01_THOR(UBound(Graph_01_THOR))) * GraphCorrel(0)
            ConversionFactor(1) = CDec(Graph_02_THOR(UBound(Graph_02_THOR))) * GraphCorrel(1)
            ConversionFactor(2) = CDec(Graph_03_THOR(UBound(Graph_03_THOR))) * GraphCorrel(2)
            ConversionFactor(3) = CDec(Graph_04_THOR(UBound(Graph_04_THOR))) * GraphCorrel(3)
            ConversionFactor(4) = CDec(Graph_05_THOR(UBound(Graph_05_THOR))) * GraphCorrel(4)
            ConversionFactor(5) = CDec(Graph_06_THOR(UBound(Graph_06_THOR))) * GraphCorrel(5)
            ConversionFactor(6) = CDec(Graph_07_THOR(UBound(Graph_07_THOR))) * GraphCorrel(6)
            ConversionFactor(7) = CDec(Graph_08_THOR(UBound(Graph_08_THOR))) * GraphCorrel(7)
            ConversionFactor(8) = CDec(Graph_09_THOR(UBound(Graph_09_THOR))) * GraphCorrel(8)
            ConversionFactor(9) = CDec(Graph_10_THOR(UBound(Graph_10_THOR))) * GraphCorrel(9)
            ConversionFactor(10) = CDec(Graph_11_THOR(UBound(Graph_11_THOR))) * GraphCorrel(10)
            ConversionFactor(11) = CDec(Graph_12_THOR(UBound(Graph_12_THOR))) * GraphCorrel(11)
            ConversionFactor(12) = CDec(Graph_13_THOR(UBound(Graph_13_THOR))) * GraphCorrel(12)
            ConversionFactor(13) = CDec(Graph_14_THOR(UBound(Graph_14_THOR))) * GraphCorrel(13)
            ConversionFactor(14) = CDec(Graph_15_THOR(UBound(Graph_15_THOR))) * GraphCorrel(14)
            ConversionFactor(15) = CDec(Graph_16_THOR(UBound(Graph_16_THOR))) * GraphCorrel(15)
            ConversionFactor(16) = CDec(Graph_17_THOR(UBound(Graph_17_THOR))) * GraphCorrel(16)
            ConversionFactor(17) = CDec(Graph_18_THOR(UBound(Graph_18_THOR))) * GraphCorrel(17)
            ConversionFactor(18) = CDec(Graph_19_THOR(UBound(Graph_19_THOR))) * GraphCorrel(18)
            ConversionFactor(19) = CDec(Graph_20_THOR(UBound(Graph_20_THOR))) * GraphCorrel(19)
            ConversionFactor(20) = CDec(Graph_21_THOR(UBound(Graph_21_THOR))) * GraphCorrel(20)
            ConversionFactor(21) = CDec(Graph_22_THOR(UBound(Graph_22_THOR))) * GraphCorrel(21)
            ConversionFactor(22) = CDec(Graph_23_THOR(UBound(Graph_23_THOR))) * GraphCorrel(22)
            ConversionFactor(23) = CDec(Graph_24_THOR(UBound(Graph_24_THOR))) * GraphCorrel(23)
            ConversionFactor(24) = CDec(Graph_25_THOR(UBound(Graph_25_THOR))) * GraphCorrel(24)
            ConversionFactor(25) = CDec(Graph_26_THOR(UBound(Graph_26_THOR))) * GraphCorrel(25)
            ConversionFactor(26) = CDec(Graph_27_THOR(UBound(Graph_27_THOR))) * GraphCorrel(26)
            ConversionFactor(27) = CDec(Graph_28_THOR(UBound(Graph_28_THOR))) * GraphCorrel(27)
            ConversionFactor(28) = CDec(Graph_29_THOR(UBound(Graph_29_THOR))) * GraphCorrel(28)
            ConversionFactor(29) = CDec(Graph_30_THOR(UBound(Graph_30_THOR))) * GraphCorrel(29)
            ConversionFactor(30) = CDec(Graph_31_THOR(UBound(Graph_31_THOR))) * GraphCorrel(30)
            ConversionFactor(31) = CDec(Graph_32_THOR(UBound(Graph_32_THOR))) * GraphCorrel(31)
            ConversionFactor(32) = CDec(Graph_33_THOR(UBound(Graph_33_THOR))) * GraphCorrel(32)
            ConversionFactor(33) = CDec(Graph_34_THOR(UBound(Graph_34_THOR))) * GraphCorrel(33)
            ConversionFactor(34) = CDec(Graph_35_THOR(UBound(Graph_35_THOR))) * GraphCorrel(34)
            ConversionFactor(35) = CDec(Graph_36_THOR(UBound(Graph_36_THOR))) * GraphCorrel(35)
            ConversionFactor(36) = CDec(Graph_37_THOR(UBound(Graph_37_THOR))) * GraphCorrel(36)
            ConversionFactor(37) = CDec(Graph_38_THOR(UBound(Graph_38_THOR))) * GraphCorrel(37)
            ConversionFactor(38) = CDec(Graph_39_THOR(UBound(Graph_39_THOR))) * GraphCorrel(38)
            ConversionFactor(39) = CDec(Graph_40_THOR(UBound(Graph_40_THOR))) * GraphCorrel(39)
            ConversionFactor(40) = CDec(Graph_41_THOR(UBound(Graph_41_THOR))) * GraphCorrel(40)
            ConversionFactor(41) = CDec(Graph_42_THOR(UBound(Graph_42_THOR))) * GraphCorrel(41)
            ConversionFactor(42) = CDec(Graph_43_THOR(UBound(Graph_43_THOR))) * GraphCorrel(42)
            ConversionFactor(43) = CDec(Graph_44_THOR(UBound(Graph_44_THOR))) * GraphCorrel(43)
            ConversionFactor(44) = CDec(Graph_45_THOR(UBound(Graph_45_THOR))) * GraphCorrel(44)
            ConversionFactor(45) = CDec(Graph_46_THOR(UBound(Graph_46_THOR))) * GraphCorrel(45)
            ConversionFactor(46) = CDec(Graph_47_THOR(UBound(Graph_47_THOR))) * GraphCorrel(46)
            ConversionFactor(47) = CDec(Graph_48_THOR(UBound(Graph_48_THOR))) * GraphCorrel(47)

            MainMDI.ProgressBarMain.Value = 1010

            '**********************  상해를 읽는다  *****************************
            Reading_Peak()
            '**********************  상해를 읽는다  *****************************

            '시트를 그린다.
            MainMDI.Statuslbl.Text = "Drawing Injury Sheets"
            MainMDI.ProgressBarMain.Value = 1020

            '********************** 여기에서 상해 시트를 그리는 프로시저 호출 **************
            Sheet_Lbl()
            Sheet_US()
            MainMDI.ProgressBarMain.Value = MainMDI.ProgressBarMain.Maximum
            '********************** 여기에서 상해 시트를 그리는 프로시저 호출 **************

            Me.Show()

            FileDrop = False
            DragFiles = Nothing

            Me.ScoreBoard.SelectedIndex = StartUpTab_THOR

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

    Private Sub InjuryDisplayFrm_THOR_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Me.Width = 1100 + 45

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
            .InjuryLbl1.Location = New Point(10 - 2, 10)
            .InjuryLbl1.Size = New Size(.ScoreBoard.Width - 26, 90)
            .Peak_Injury1.Location = New Point(10 - 2, .InjuryLbl1.Height + .InjuryLbl1.Location.Y + 3)
            .Peak_Injury1.Size = New Size(.InjuryLbl1.Width, .ScoreBoard.Height - .InjuryLbl1.Height - 46)


        End With

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
                    If InStr(1, tmp_txt, "d_thorM50el", vbTextCompare) > 0 And (InStr(1, tmp_txt, "Opening file", vbTextCompare) > 0 Or InStr(1, tmp_txt, "Assessing", vbTextCompare) > 0) Then
                        DummyType(i) = 2
                        Exit Do
                    ElseIf InStr(1, tmp_txt, "d_hyb350", vbTextCompare) > 0 And (InStr(1, tmp_txt, "Opening file", vbTextCompare) > 0 Or InStr(1, tmp_txt, "Assessing", vbTextCompare) > 0) Then
                        DummyType(i) = 0
                        Exit Do
                    ElseIf InStr(1, tmp_txt, "d_hyb305", vbTextCompare) > 0 And (InStr(1, tmp_txt, "Opening file", vbTextCompare) > 0 Or InStr(1, tmp_txt, "Assessing", vbTextCompare) > 0) Then
                        DummyType(i) = 1
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
        ReDim BrIC(UBound(OpenFile.names))
        ReDim AngularVelX(UBound(OpenFile.names))
        ReDim AngularVelY(UBound(OpenFile.names))
        ReDim AngularVelZ(UBound(OpenFile.names))
        '============================================================================================================

        '위의 상해를 다 읽어온다. '14.11.04 현재 THOR 관련 4개 상해 아직 없음

        'Dim FilePathTemp As String = Mid(Me.PathLbl.Text, 8) 

        For i = 0 To UBound(OpenFile.names) Step 1

            'Dim ReadFiles As New FileStream(FilePathGet(Me.OpenDlg.FileNames) & OpenFile.names(i) & ".peak" _
            '                                , FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            Dim ReadFiles As New FileStream(Tmp_Path(i) & OpenFile.names(i) & ".peak" _
                                            , FileMode.Open, FileAccess.Read, FileShare.ReadWrite)

            fileNum = New StreamReader(ReadFiles)

            If DummyType(i) = 0 Or DummyType(i) = 1 Then 'Hybrid-Ⅲ 50% / 5%

                MsgBox("Some File Contains a Different ATD Information..or Cannot Configure!!!", MsgBoxStyle.Critical, "ATD Info. Error")
                Me.Close()

                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "H3MS_inj", vbTextCompare) > 0 Then
                '        tmp_txt = fileNum.ReadLine
                '        tmp_txt = fileNum.ReadLine
                '        Head3MSG(i) = (CDbl(Mid(tmp_txt, 30, 15)) / 9.81) * InjuryCorrel(0)
                '        Exit Do
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "T3MS_inj", vbTextCompare) > 0 Then
                '        tmp_txt = fileNum.ReadLine
                '        tmp_txt = fileNum.ReadLine
                '        Chest_G_CUMULATIVE_T3MS_inj(i) = (CDbl(Mid(tmp_txt, 30, 15)) / 9.81) * InjuryCorrel(1)
                '        Exit Do
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "HIC15_inj", vbTextCompare) > 0 Then
                '        tmp_txt = fileNum.ReadLine
                '        tmp_txt = fileNum.ReadLine
                '        HIC15(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(2)
                '        Exit Do
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "HIC36_inj", vbTextCompare) > 0 Then
                '        tmp_txt = fileNum.ReadLine
                '        tmp_txt = fileNum.ReadLine
                '        HIC36(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(3)
                '        Exit Do
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "NTE_inj", vbTextCompare) > 0 Then
                '        tmp_txt = fileNum.ReadLine
                '        tmp_txt = fileNum.ReadLine
                '        NTE(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(4)
                '        Exit Do
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "NTF_inj", vbTextCompare) > 0 Then
                '        tmp_txt = fileNum.ReadLine
                '        tmp_txt = fileNum.ReadLine
                '        NTF(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(5)
                '        Exit Do
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "NCE_inj", vbTextCompare) > 0 Then
                '        tmp_txt = fileNum.ReadLine
                '        tmp_txt = fileNum.ReadLine
                '        NCE(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(6)
                '        Exit Do
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "NCF_inj", vbTextCompare) > 0 Then
                '        tmp_txt = fileNum.ReadLine
                '        tmp_txt = fileNum.ReadLine
                '        NCF(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(7)
                '        Exit Do
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "HaccRpeak_inj", vbTextCompare) > 0 Then
                '        tmp_txt = fileNum.ReadLine
                '        tmp_txt = fileNum.ReadLine
                '        Head_Peak_G(i) = (CDbl(Mid(tmp_txt, 30, 15)) / 9.81) * InjuryCorrel(8)
                '        Exit Do
                '    End If
                'Loop
                ' ''=================================================================================
                ''Do While Not fileNum.EndOfStream
                ''    tmp_txt = fileNum.ReadLine
                ''    If InStr(1, tmp_txt, "TaccRpeak_inj", vbTextCompare) > 0 Then
                ''        tmp_txt = fileNum.ReadLine
                ''        tmp_txt = fileNum.ReadLine
                ''        Chest_G(i) = (CDbl(Mid(tmp_txt, 30, 15)) / 9.81)
                ''        Exit do
                ''    End If
                ''Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "ThCC_inj", vbTextCompare) > 0 Then
                '        tmp_txt = fileNum.ReadLine
                '        tmp_txt = fileNum.ReadLine
                '        Chest_D(i) = Math.Abs(CDbl(Mid(tmp_txt, 30, 15))) * 1000 * InjuryCorrel(9)
                '        Exit Do
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "kneesliderL_inj", vbTextCompare) > 0 Then
                '        tmp_txt = fileNum.ReadLine
                '        tmp_txt = fileNum.ReadLine
                '        KneeS_L(i) = CDbl(Mid(tmp_txt, 30, 15)) * 1000 * InjuryCorrel(10)
                '        Exit Do
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "kneesliderR_inj", vbTextCompare) > 0 Then
                '        tmp_txt = fileNum.ReadLine
                '        tmp_txt = fileNum.ReadLine
                '        KneeS_R(i) = CDbl(Mid(tmp_txt, 30, 15)) * 1000 * InjuryCorrel(11)
                '        Exit Do
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "TCFCLowL_inj", vbTextCompare) > 0 Then
                '        tmp_txt = fileNum.ReadLine
                '        tmp_txt = fileNum.ReadLine
                '        Tibia_Comp_L(i) = (CDbl(Mid(tmp_txt, 30, 15)) / 1000) * InjuryCorrel(12)
                '        Exit Do
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "TCFCLowR_inj", vbTextCompare) > 0 Then
                '        tmp_txt = fileNum.ReadLine
                '        tmp_txt = fileNum.ReadLine
                '        Tibia_Comp_R(i) = (CDbl(Mid(tmp_txt, 30, 15)) / 1000) * InjuryCorrel(13)
                '        Exit Do
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "TIUpL_inj", vbTextCompare) > 0 Then
                '        tmp_txt = fileNum.ReadLine
                '        tmp_txt = fileNum.ReadLine
                '        TI_upr_L(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(14)
                '        Exit Do
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "TILowL_inj", vbTextCompare) > 0 Then
                '        tmp_txt = fileNum.ReadLine
                '        tmp_txt = fileNum.ReadLine
                '        TI_lwr_L(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(15)
                '        Exit Do
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "TIUpR_inj", vbTextCompare) > 0 Then
                '        tmp_txt = fileNum.ReadLine
                '        tmp_txt = fileNum.ReadLine
                '        TI_upr_R(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(16)
                '        Exit Do
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "TILowR_inj", vbTextCompare) > 0 Then
                '        tmp_txt = fileNum.ReadLine
                '        tmp_txt = fileNum.ReadLine
                '        TI_lwr_R(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(17)
                '        Exit Do
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "VC_inj_CFC180", vbTextCompare) > 0 Then
                '        tmp_txt = fileNum.ReadLine
                '        tmp_txt = fileNum.ReadLine
                '        Chest_VC(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(18)
                '        Exit Do
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "FFCL_inj", vbTextCompare) > 0 Then
                '        If InStr(1, tmp_txt, "negative", vbTextCompare) > 0 Then
                '            tmp_txt = fileNum.ReadLine
                '            tmp_txt = fileNum.ReadLine
                '            FemurL(i) = Math.Abs((CDbl(Mid(tmp_txt, 60, 12)) / 1000)) * InjuryCorrel(19)
                '            Exit Do
                '        End If
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "FFCR_inj", vbTextCompare) > 0 Then
                '        If InStr(1, tmp_txt, "negative", vbTextCompare) > 0 Then
                '            tmp_txt = fileNum.ReadLine
                '            tmp_txt = fileNum.ReadLine
                '            FemurR(i) = Math.Abs((CDbl(Mid(tmp_txt, 60, 12)) / 1000)) * InjuryCorrel(20)
                '            Exit Do
                '        End If
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "FNICtension_inj", vbTextCompare) > 0 Then
                '        If InStr(1, tmp_txt, "negative", vbTextCompare) > 0 Then
                '            tmp_txt = fileNum.ReadLine
                '            tmp_txt = fileNum.ReadLine
                '            Neck_Comp(i) = (CDbl(Mid(tmp_txt, 60, 12)) / 1000) * InjuryCorrel(21)
                '            Exit Do
                '        End If
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "FNICtension_inj", vbTextCompare) > 0 Then
                '        If InStr(1, tmp_txt, "positive", vbTextCompare) > 0 Then
                '            tmp_txt = fileNum.ReadLine
                '            tmp_txt = fileNum.ReadLine
                '            Neck_Tens(i) = Math.Abs(CDbl(Mid$(tmp_txt, 60, 12)) / 1000) * InjuryCorrel(22)
                '            Exit Do
                '        End If
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "FNICshear_inj", vbTextCompare) > 0 Then
                '        If InStr(1, tmp_txt, "negative", vbTextCompare) > 0 Then
                '            tmp_txt = fileNum.ReadLine
                '            tmp_txt = fileNum.ReadLine
                '            Neck_Shear(i) = Math.Abs(CDbl(Mid$(tmp_txt, 60, 12)) / 1000) * InjuryCorrel(23)
                '            Exit Do
                '        End If
                '    End If
                'Loop
                ''=================================================================================
                'Do While Not fileNum.EndOfStream
                '    tmp_txt = fileNum.ReadLine
                '    If InStr(1, tmp_txt, "FNICbending_inj", vbTextCompare) > 0 Then
                '        If InStr(1, tmp_txt, "negative", vbTextCompare) > 0 Then
                '            tmp_txt = fileNum.ReadLine
                '            tmp_txt = fileNum.ReadLine
                '            Neck_Exten(i) = Math.Abs(CDbl(Mid$(tmp_txt, 60, 12))) * InjuryCorrel(24)
                '            Exit Do
                '        End If
                '    End If
                'Loop

                'fileNum.Close()
                'ReadFiles.Close()

                ''Probability Calculation
                ''Head P
                'P_Head(i) = CND(((Math.Log(Format(HIC15(i), "#.0")) - 7.45231) / 0.73998))

                'If DummyType(i) = 0 Then
                '    P_Neck_Tens(i) = 1 / (1 + Math.Exp(10.9745 - 2.375 * Format(Neck_Tens(i), "#.000")))
                '    P_Neck_Comp(i) = 1 / (1 + Math.Exp(10.9745 - 2.375 * Format(Neck_Comp(i), "#.000")))
                '    P_FemurL(i) = 1 / (1 + Math.Exp(5.795 - 0.5196 * Format(FemurL(i), "#.000")))
                '    P_FemurR(i) = 1 / (1 + Math.Exp(5.795 - 0.5196 * Format(FemurR(i), "#.000")))
                '    P_CD(i) = 1 / (1 + Math.Exp(10.5456 - 1.568 * (Format(Chest_D(i), "#.0") ^ 0.4612)))
                'ElseIf DummyType(i) = 1 Then
                '    P_Neck_Tens(i) = 1 / (1 + Math.Exp(10.958 - 3.77 * Format(Neck_Tens(i), "#.000")))
                '    P_Neck_Comp(i) = 1 / (1 + Math.Exp(10.958 - 3.77 * Format(Neck_Comp(i), "#.000")))
                '    P_FemurL(i) = 1 / (1 + Math.Exp(5.7949 - 0.7619 * Format(FemurL(i), "#.000")))
                '    P_FemurR(i) = 1 / (1 + Math.Exp(5.7949 - 0.7619 * Format(FemurR(i), "#.000")))
                '    P_CD(i) = 1 / (1 + Math.Exp(10.5456 - 1.7212 * (Format(Chest_D(i), "#.0") ^ 0.4612)))
                'End If
                'P_Neck_NTE(i) = 1 / (1 + Math.Exp(3.2269 - 1.9688 * Format(NTE(i), "#.00")))
                'P_Neck_NTF(i) = 1 / (1 + Math.Exp(3.2269 - 1.9688 * Format(NTF(i), "#.00")))
                'P_Neck_NCE(i) = 1 / (1 + Math.Exp(3.2269 - 1.9688 * Format(NCE(i), "#.00")))
                'P_Neck_NCF(i) = 1 / (1 + Math.Exp(3.2269 - 1.9688 * Format(NCF(i), "#.00")))

                'P_Neck_Max(i) = P_Neck_Max_Judg(P_Neck_Tens(i), P_Neck_Comp(i), P_Neck_NTE(i), P_Neck_NTF(i), P_Neck_NCE(i), P_Neck_NCF(i))
                'If P_FemurL(i) >= P_FemurR(i) Then
                '    P_Femur_Max(i) = P_FemurL(i)
                'Else
                '    P_Femur_Max(i) = P_FemurR(i)
                'End If
                '********************************************************************************************
            ElseIf DummyType(i) = 2 Then '=============================================> For THOR 50% ATD
                '********************************************************************************************

                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "T3MS_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        Chest_G_CUMULATIVE_T3MS_inj(i) = (CDbl(Mid(tmp_txt, 30, 15)) / 9.81) * InjuryCorrel(0)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "HIC15_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        HIC15(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(1)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "HIC36_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        HIC36(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(2)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "NTE_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        NTE(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(3)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "NTF_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        NTF(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(4)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "NCE_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        NCE(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(5)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "NCF_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        NCF(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(6)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "KneeSliderL_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        KneeS_L(i) = CDbl(Mid(tmp_txt, 30, 15)) * 1000 * InjuryCorrel(8)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "KneeSliderR_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        KneeS_R(i) = CDbl(Mid(tmp_txt, 30, 15)) * 1000 * InjuryCorrel(9)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "FemurLcompZpeak_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        FemurL(i) = Math.Abs((CDbl(Mid(tmp_txt, 30, 15)) / 1000)) * InjuryCorrel(16)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "FemurRcompZpeak_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        FemurR(i) = Math.Abs((CDbl(Mid(tmp_txt, 30, 15)) / 1000)) * InjuryCorrel(17)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "TCFCLowL_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        Tibia_Comp_L(i) = (CDbl(Mid(tmp_txt, 30, 15)) / 1000) * InjuryCorrel(10)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "TCFCLowR_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        Tibia_Comp_R(i) = (CDbl(Mid(tmp_txt, 30, 15)) / 1000) * InjuryCorrel(11)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "TIUpL_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        TI_upr_L(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(12)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "TILowL_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        TI_lwr_L(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(13)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "TIUpR_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        TI_upr_R(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(14)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "TILowR_inj", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        TI_lwr_R(i) = CDbl(Mid(tmp_txt, 30, 15)) * InjuryCorrel(15)
                        Exit Do
                    End If
                Loop
                '=================================================================================
                'Angular Velocity === Temp
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "HeadCG_Ang_Vel", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        'InjuryCorrel(23~25)
                        'X Angular
                        If Math.Abs(CDbl(Mid(tmp_txt, 40, 12)) * InjuryCorrel(23)) >= Math.Abs(CDbl(Mid(tmp_txt, 60, 12)) * InjuryCorrel(23)) Then
                            AngularVelX(i) = CDbl(Mid(tmp_txt, 40, 12)) * InjuryCorrel(23)
                        Else
                            AngularVelX(i) = CDbl(Mid(tmp_txt, 60, 12)) * InjuryCorrel(23)
                        End If
                        'Y Angular
                        tmp_txt = fileNum.ReadLine
                        If Math.Abs(CDbl(Mid(tmp_txt, 40, 12)) * InjuryCorrel(24)) >= Math.Abs(CDbl(Mid(tmp_txt, 60, 12)) * InjuryCorrel(24)) Then
                            AngularVelY(i) = CDbl(Mid(tmp_txt, 40, 12)) * InjuryCorrel(24)
                        Else
                            AngularVelY(i) = CDbl(Mid(tmp_txt, 60, 12)) * InjuryCorrel(24)
                        End If
                        'Z Angular
                        tmp_txt = fileNum.ReadLine
                        If Math.Abs(CDbl(Mid(tmp_txt, 40, 12)) * InjuryCorrel(25)) >= Math.Abs(CDbl(Mid(tmp_txt, 60, 12)) * InjuryCorrel(25)) Then
                            AngularVelZ(i) = CDbl(Mid(tmp_txt, 40, 12)) * InjuryCorrel(25)
                        Else
                            AngularVelZ(i) = CDbl(Mid(tmp_txt, 60, 12)) * InjuryCorrel(25)
                        End If
                        BrIC(i) = Math.Sqrt(((AngularVelX(i) / 66.25) ^ 2) + ((AngularVelY(i) / 56.45) ^ 2) + ((AngularVelZ(i) / 42.87) ^ 2))
                    End If
                Loop

                '파일 ThxIrUpL스트림을 처음으로 돌린다.
                ReadFiles.Seek(0, SeekOrigin.Begin)

                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "ThxIrTraccRibL_CFC600_dis", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        ThxIrUpL(i) = (CDbl(Mid(tmp_txt, 40, 12)) * -1000) * InjuryCorrel(26)
                        Exit Do
                    End If
                Loop
                ReadFiles.Seek(0, SeekOrigin.Begin)
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "ThxIrTraccRibR_CFC600_dis", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        ThxIrUpR(i) = (CDbl(Mid(tmp_txt, 40, 12)) * -1000) * InjuryCorrel(27)
                        Exit Do
                    End If
                Loop
                ReadFiles.Seek(0, SeekOrigin.Begin)
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "ThxLowIrTraccRibL_CFC600_dis", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        ThxIrLowL(i) = (CDbl(Mid(tmp_txt, 40, 12)) * -1000) * InjuryCorrel(28)
                        Exit Do
                    End If
                Loop
                ReadFiles.Seek(0, SeekOrigin.Begin)
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "ThxLowIrTraccRibR_CFC600_dis", vbTextCompare) > 0 Then
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        tmp_txt = fileNum.ReadLine
                        ThxIrLowR(i) = (CDbl(Mid(tmp_txt, 40, 12)) * -1000) * InjuryCorrel(29)
                        Exit Do
                    End If
                Loop
                ReadFiles.Seek(0, SeekOrigin.Begin)
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "FNICtension_inj", vbTextCompare) > 0 Then
                        If InStr(1, tmp_txt, "negative", vbTextCompare) > 0 Then
                            tmp_txt = fileNum.ReadLine
                            tmp_txt = fileNum.ReadLine
                            Neck_Comp(i) = (CDbl(Mid(tmp_txt, 60, 12)) / 1000) * InjuryCorrel(18)
                            Exit Do
                        End If
                    End If
                Loop
                ReadFiles.Seek(0, SeekOrigin.Begin)
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "FNICtension_inj", vbTextCompare) > 0 Then
                        If InStr(1, tmp_txt, "positive", vbTextCompare) > 0 Then
                            tmp_txt = fileNum.ReadLine
                            tmp_txt = fileNum.ReadLine
                            Neck_Tens(i) = Math.Abs(CDbl(Mid$(tmp_txt, 60, 12)) / 1000) * InjuryCorrel(19)
                            Exit Do
                        End If
                    End If
                Loop
                ReadFiles.Seek(0, SeekOrigin.Begin)
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "FNICshear_inj", vbTextCompare) > 0 Then
                        If InStr(1, tmp_txt, "negative", vbTextCompare) > 0 Then
                            tmp_txt = fileNum.ReadLine
                            tmp_txt = fileNum.ReadLine
                            Neck_Shear(i) = Math.Abs(CDbl(Mid$(tmp_txt, 60, 12)) / 1000) * InjuryCorrel(20)
                            Exit Do
                        End If
                    End If
                Loop
                ReadFiles.Seek(0, SeekOrigin.Begin)
                '=================================================================================
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(1, tmp_txt, "FNICbending_inj", vbTextCompare) > 0 Then
                        If InStr(1, tmp_txt, "negative", vbTextCompare) > 0 Then
                            tmp_txt = fileNum.ReadLine
                            tmp_txt = fileNum.ReadLine
                            Neck_Exten(i) = Math.Abs(CDbl(Mid$(tmp_txt, 60, 12))) * InjuryCorrel(21)
                            Exit Do
                        End If
                    End If
                Loop

                fileNum.Close()
                ReadFiles.Close()

                P_Head(i) = CND(((Math.Log(Format(HIC15(i), "#.0")) - 7.45231) / 0.73998))

                '임시로 가슴변위의 평균값 취함
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
            End If

        Next

    End Sub

    Private Sub OpenLst_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Me.OpenWithXMADgic.Show(MousePosition)
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        If ((Control.ModifierKeys And Keys.Alt) = Keys.Alt) Then IsRemote = True

        MainMDI.ProgressBarMain.Maximum = 1020

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
                Me.Close()
                Exit Sub
            End If
        End If

        formerfileCnt = Me.OpenFile.names.Count

        If IsRemote = False Then

            EventPass = True

            ReDim Preserve OpenFile.names(formerfileCnt + Me.AddFileDlg.FileNames.Count - 1)
            ReDim Preserve Tmp_Path((formerfileCnt + Me.AddFileDlg.FileNames.Count - 1))
            ReDim Preserve IsRemotePath(formerfileCnt + RemoteFilesNames.Count - 1)

            For i = formerfileCnt To UBound(OpenFile.names)
                OpenFile.names(i) = FileNameGet2(Me.AddFileDlg.SafeFileNames(i - formerfileCnt))
                Tmp_Path(i) = FilePathGet2(Me.AddFileDlg.FileNames(i - formerfileCnt))
                IsRemotePath(i) = ""
            Next

        Else

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
        '상해를 읽는다
        Reading_Peak()

        If OpenFile.names.Length > 5 Then
            Me.Height = 780
        Else
            Me.Height = 630
        End If

        Peak_Injury1.Rows = 1
        Peak_Injury1.Cols = 1

        '시트를 그린다.************************************************* 상해 시트 그리기 *********************
        MainMDI.Statuslbl.Text = "Drawing Injury Sheets"
        MainMDI.ProgressBarMain.Value = 100
        Sheet_US()
        '시트를 그린다.************************************************* 상해 시트 그리기 *********************

        '그래프 파라미터를 미리 읽어온다.
        Dim RowCol(1) As Integer

        ReDim ParaMeterArr1(47, UBound(OpenFile.names), 1)
        '{그래프 번호 , Case 번호 , 데이터위치(Row/Col) )
        ReDim ParaMeterArr2(47, UBound(OpenFile.names), 1)
        '{그래프 번호 , Case 번호 , 총채널수(Row/Col) )
        ReDim ConversionFactor(47)
        '{그래프 Conversion Factor - 단위변환용}

        MainMDI.Statuslbl.Text = "Loading DATA Location Parameters"

        For i = 0 To UBound(OpenFile.names)
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(0), Graph_01_THOR)
            ParaMeterArr1(0, i, 0) = RowCol(0)
            ParaMeterArr1(0, i, 1) = RowCol(1)
            ParaMeterArr2(0, i, 0) = RowCol(2)
            ParaMeterArr2(0, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 520
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(1), Graph_02_THOR)
            ParaMeterArr1(1, i, 0) = RowCol(0)
            ParaMeterArr1(1, i, 1) = RowCol(1)
            ParaMeterArr2(1, i, 0) = RowCol(2)
            ParaMeterArr2(1, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 530
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(2), Graph_03_THOR)
            ParaMeterArr1(2, i, 0) = RowCol(0)
            ParaMeterArr1(2, i, 1) = RowCol(1)
            ParaMeterArr2(2, i, 0) = RowCol(2)
            ParaMeterArr2(2, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 540
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(3), Graph_04_THOR)
            ParaMeterArr1(3, i, 0) = RowCol(0)
            ParaMeterArr1(3, i, 1) = RowCol(1)
            ParaMeterArr2(3, i, 0) = RowCol(2)
            ParaMeterArr2(3, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 550
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(4), Graph_05_THOR)
            ParaMeterArr1(4, i, 0) = RowCol(0)
            ParaMeterArr1(4, i, 1) = RowCol(1)
            ParaMeterArr2(4, i, 0) = RowCol(2)
            ParaMeterArr2(4, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 560
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(5), Graph_06_THOR)
            ParaMeterArr1(5, i, 0) = RowCol(0)
            ParaMeterArr1(5, i, 1) = RowCol(1)
            ParaMeterArr2(5, i, 0) = RowCol(2)
            ParaMeterArr2(5, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 570
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(6), Graph_07_THOR)
            ParaMeterArr1(6, i, 0) = RowCol(0)
            ParaMeterArr1(6, i, 1) = RowCol(1)
            ParaMeterArr2(6, i, 0) = RowCol(2)
            ParaMeterArr2(6, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 580
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(7), Graph_08_THOR)
            ParaMeterArr1(7, i, 0) = RowCol(0)
            ParaMeterArr1(7, i, 1) = RowCol(1)
            ParaMeterArr2(7, i, 0) = RowCol(2)
            ParaMeterArr2(7, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 590
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(8), Graph_09_THOR)
            ParaMeterArr1(8, i, 0) = RowCol(0)
            ParaMeterArr1(8, i, 1) = RowCol(1)
            ParaMeterArr2(8, i, 0) = RowCol(2)
            ParaMeterArr2(8, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 600
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(9), Graph_10_THOR)
            ParaMeterArr1(9, i, 0) = RowCol(0)
            ParaMeterArr1(9, i, 1) = RowCol(1)
            ParaMeterArr2(9, i, 0) = RowCol(2)
            ParaMeterArr2(9, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 610
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(10), Graph_11_THOR)
            ParaMeterArr1(10, i, 0) = RowCol(0)
            ParaMeterArr1(10, i, 1) = RowCol(1)
            ParaMeterArr2(10, i, 0) = RowCol(2)
            ParaMeterArr2(10, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 620
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(11), Graph_12_THOR)
            ParaMeterArr1(11, i, 0) = RowCol(0)
            ParaMeterArr1(11, i, 1) = RowCol(1)
            ParaMeterArr2(11, i, 0) = RowCol(2)
            ParaMeterArr2(11, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 630
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(12), Graph_13_THOR)
            ParaMeterArr1(12, i, 0) = RowCol(0)
            ParaMeterArr1(12, i, 1) = RowCol(1)
            ParaMeterArr2(12, i, 0) = RowCol(2)
            ParaMeterArr2(12, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 640
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(13), Graph_14_THOR)
            ParaMeterArr1(13, i, 0) = RowCol(0)
            ParaMeterArr1(13, i, 1) = RowCol(1)
            ParaMeterArr2(13, i, 0) = RowCol(2)
            ParaMeterArr2(13, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 650
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(14), Graph_15_THOR)
            ParaMeterArr1(14, i, 0) = RowCol(0)
            ParaMeterArr1(14, i, 1) = RowCol(1)
            ParaMeterArr2(14, i, 0) = RowCol(2)
            ParaMeterArr2(14, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 660
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(15), Graph_16_THOR)
            ParaMeterArr1(15, i, 0) = RowCol(0)
            ParaMeterArr1(15, i, 1) = RowCol(1)
            ParaMeterArr2(15, i, 0) = RowCol(2)
            ParaMeterArr2(15, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 670
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(16), Graph_17_THOR)
            ParaMeterArr1(16, i, 0) = RowCol(0)
            ParaMeterArr1(16, i, 1) = RowCol(1)
            ParaMeterArr2(16, i, 0) = RowCol(2)
            ParaMeterArr2(16, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 680
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(17), Graph_18_THOR)
            ParaMeterArr1(17, i, 0) = RowCol(0)
            ParaMeterArr1(17, i, 1) = RowCol(1)
            ParaMeterArr2(17, i, 0) = RowCol(2)
            ParaMeterArr2(17, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 690
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(18), Graph_19_THOR)
            ParaMeterArr1(18, i, 0) = RowCol(0)
            ParaMeterArr1(18, i, 1) = RowCol(1)
            ParaMeterArr2(18, i, 0) = RowCol(2)
            ParaMeterArr2(18, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 700
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(19), Graph_20_THOR)
            ParaMeterArr1(19, i, 0) = RowCol(0)
            ParaMeterArr1(19, i, 1) = RowCol(1)
            ParaMeterArr2(19, i, 0) = RowCol(2)
            ParaMeterArr2(19, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 710
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(20), Graph_21_THOR)
            ParaMeterArr1(20, i, 0) = RowCol(0)
            ParaMeterArr1(20, i, 1) = RowCol(1)
            ParaMeterArr2(20, i, 0) = RowCol(2)
            ParaMeterArr2(20, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 720
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(21), Graph_22_THOR)
            ParaMeterArr1(21, i, 0) = RowCol(0)
            ParaMeterArr1(21, i, 1) = RowCol(1)
            ParaMeterArr2(21, i, 0) = RowCol(2)
            ParaMeterArr2(21, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 730
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(22), Graph_23_THOR)
            ParaMeterArr1(22, i, 0) = RowCol(0)
            ParaMeterArr1(22, i, 1) = RowCol(1)
            ParaMeterArr2(22, i, 0) = RowCol(2)
            ParaMeterArr2(22, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 740
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(23), Graph_24_THOR)
            ParaMeterArr1(23, i, 0) = RowCol(0)
            ParaMeterArr1(23, i, 1) = RowCol(1)
            ParaMeterArr2(23, i, 0) = RowCol(2)
            ParaMeterArr2(23, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 750
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(24), Graph_25_THOR)
            ParaMeterArr1(24, i, 0) = RowCol(0)
            ParaMeterArr1(24, i, 1) = RowCol(1)
            ParaMeterArr2(24, i, 0) = RowCol(2)
            ParaMeterArr2(24, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 760
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(25), Graph_26_THOR)
            ParaMeterArr1(25, i, 0) = RowCol(0)
            ParaMeterArr1(25, i, 1) = RowCol(1)
            ParaMeterArr2(25, i, 0) = RowCol(2)
            ParaMeterArr2(25, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 770
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(26), Graph_27_THOR)
            ParaMeterArr1(26, i, 0) = RowCol(0)
            ParaMeterArr1(26, i, 1) = RowCol(1)
            ParaMeterArr2(26, i, 0) = RowCol(2)
            ParaMeterArr2(26, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 780
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(27), Graph_28_THOR)
            ParaMeterArr1(27, i, 0) = RowCol(0)
            ParaMeterArr1(27, i, 1) = RowCol(1)
            ParaMeterArr2(27, i, 0) = RowCol(2)
            ParaMeterArr2(27, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 790
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(28), Graph_29_THOR)
            ParaMeterArr1(28, i, 0) = RowCol(0)
            ParaMeterArr1(28, i, 1) = RowCol(1)
            ParaMeterArr2(28, i, 0) = RowCol(2)
            ParaMeterArr2(28, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 800
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(29), Graph_30_THOR)
            ParaMeterArr1(29, i, 0) = RowCol(0)
            ParaMeterArr1(29, i, 1) = RowCol(1)
            ParaMeterArr2(29, i, 0) = RowCol(2)
            ParaMeterArr2(29, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 810
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(30), Graph_31_THOR)
            ParaMeterArr1(30, i, 0) = RowCol(0)
            ParaMeterArr1(30, i, 1) = RowCol(1)
            ParaMeterArr2(30, i, 0) = RowCol(2)
            ParaMeterArr2(30, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 820
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(31), Graph_32_THOR)
            ParaMeterArr1(31, i, 0) = RowCol(0)
            ParaMeterArr1(31, i, 1) = RowCol(1)
            ParaMeterArr2(31, i, 0) = RowCol(2)
            ParaMeterArr2(31, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 830
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(32), Graph_33_THOR)
            ParaMeterArr1(32, i, 0) = RowCol(0)
            ParaMeterArr1(32, i, 1) = RowCol(1)
            ParaMeterArr2(32, i, 0) = RowCol(2)
            ParaMeterArr2(32, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 840
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(33), Graph_34_THOR)
            ParaMeterArr1(33, i, 0) = RowCol(0)
            ParaMeterArr1(33, i, 1) = RowCol(1)
            ParaMeterArr2(33, i, 0) = RowCol(2)
            ParaMeterArr2(33, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 850
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(34), Graph_35_THOR)
            ParaMeterArr1(34, i, 0) = RowCol(0)
            ParaMeterArr1(34, i, 1) = RowCol(1)
            ParaMeterArr2(34, i, 0) = RowCol(2)
            ParaMeterArr2(34, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 860
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(35), Graph_36_THOR)
            ParaMeterArr1(35, i, 0) = RowCol(0)
            ParaMeterArr1(35, i, 1) = RowCol(1)
            ParaMeterArr2(35, i, 0) = RowCol(2)
            ParaMeterArr2(35, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 870
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(36), Graph_37_THOR)
            ParaMeterArr1(36, i, 0) = RowCol(0)
            ParaMeterArr1(36, i, 1) = RowCol(1)
            ParaMeterArr2(36, i, 0) = RowCol(2)
            ParaMeterArr2(36, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 880
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(37), Graph_38_THOR)
            ParaMeterArr1(37, i, 0) = RowCol(0)
            ParaMeterArr1(37, i, 1) = RowCol(1)
            ParaMeterArr2(37, i, 0) = RowCol(2)
            ParaMeterArr2(37, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 890
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(38), Graph_39_THOR)
            ParaMeterArr1(38, i, 0) = RowCol(0)
            ParaMeterArr1(38, i, 1) = RowCol(1)
            ParaMeterArr2(38, i, 0) = RowCol(2)
            ParaMeterArr2(38, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 900
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(39), Graph_40_THOR)
            ParaMeterArr1(39, i, 0) = RowCol(0)
            ParaMeterArr1(39, i, 1) = RowCol(1)
            ParaMeterArr2(39, i, 0) = RowCol(2)
            ParaMeterArr2(39, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 910
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(40), Graph_41_THOR)
            ParaMeterArr1(40, i, 0) = RowCol(0)
            ParaMeterArr1(40, i, 1) = RowCol(1)
            ParaMeterArr2(40, i, 0) = RowCol(2)
            ParaMeterArr2(40, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 920
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(41), Graph_42_THOR)
            ParaMeterArr1(41, i, 0) = RowCol(0)
            ParaMeterArr1(41, i, 1) = RowCol(1)
            ParaMeterArr2(41, i, 0) = RowCol(2)
            ParaMeterArr2(41, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 930
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(42), Graph_43_THOR)
            ParaMeterArr1(42, i, 0) = RowCol(0)
            ParaMeterArr1(42, i, 1) = RowCol(1)
            ParaMeterArr2(42, i, 0) = RowCol(2)
            ParaMeterArr2(42, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 940
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(43), Graph_44_THOR)
            ParaMeterArr1(43, i, 0) = RowCol(0)
            ParaMeterArr1(43, i, 1) = RowCol(1)
            ParaMeterArr2(43, i, 0) = RowCol(2)
            ParaMeterArr2(43, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 950
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(44), Graph_45_THOR)
            ParaMeterArr1(44, i, 0) = RowCol(0)
            ParaMeterArr1(44, i, 1) = RowCol(1)
            ParaMeterArr2(44, i, 0) = RowCol(2)
            ParaMeterArr2(44, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 960
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(45), Graph_46_THOR)
            ParaMeterArr1(45, i, 0) = RowCol(0)
            ParaMeterArr1(45, i, 1) = RowCol(1)
            ParaMeterArr2(45, i, 0) = RowCol(2)
            ParaMeterArr2(45, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 970
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(46), Graph_47_THOR)
            ParaMeterArr1(46, i, 0) = RowCol(0)
            ParaMeterArr1(46, i, 1) = RowCol(1)
            ParaMeterArr2(46, i, 0) = RowCol(2)
            ParaMeterArr2(46, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 980
            RowCol = Analopen.FileRowCol(Tmp_Path(i), OpenFile.names(i), Graph_Ext_THOR(47), Graph_48_THOR)
            ParaMeterArr1(47, i, 0) = RowCol(0)
            ParaMeterArr1(47, i, 1) = RowCol(1)
            ParaMeterArr2(47, i, 0) = RowCol(2)
            ParaMeterArr2(47, i, 1) = RowCol(3)
            MainMDI.ProgressBarMain.Value = 990

        Next

        'Conversion Factor Load and Correlation Factor Load
        '단위 환산 계수와 상관성 수치를 동시에 로드한다.
        ConversionFactor(0) = CDec(Graph_01_THOR(UBound(Graph_01_THOR))) * GraphCorrel(0)
        ConversionFactor(1) = CDec(Graph_02_THOR(UBound(Graph_02_THOR))) * GraphCorrel(1)
        ConversionFactor(2) = CDec(Graph_03_THOR(UBound(Graph_03_THOR))) * GraphCorrel(2)
        ConversionFactor(3) = CDec(Graph_04_THOR(UBound(Graph_04_THOR))) * GraphCorrel(3)
        ConversionFactor(4) = CDec(Graph_05_THOR(UBound(Graph_05_THOR))) * GraphCorrel(4)
        ConversionFactor(5) = CDec(Graph_06_THOR(UBound(Graph_06_THOR))) * GraphCorrel(5)
        ConversionFactor(6) = CDec(Graph_07_THOR(UBound(Graph_07_THOR))) * GraphCorrel(6)
        ConversionFactor(7) = CDec(Graph_08_THOR(UBound(Graph_08_THOR))) * GraphCorrel(7)
        ConversionFactor(8) = CDec(Graph_09_THOR(UBound(Graph_09_THOR))) * GraphCorrel(8)
        ConversionFactor(9) = CDec(Graph_10_THOR(UBound(Graph_10_THOR))) * GraphCorrel(9)
        ConversionFactor(10) = CDec(Graph_11_THOR(UBound(Graph_11_THOR))) * GraphCorrel(10)
        ConversionFactor(11) = CDec(Graph_12_THOR(UBound(Graph_12_THOR))) * GraphCorrel(11)
        ConversionFactor(12) = CDec(Graph_13_THOR(UBound(Graph_13_THOR))) * GraphCorrel(12)
        ConversionFactor(13) = CDec(Graph_14_THOR(UBound(Graph_14_THOR))) * GraphCorrel(13)
        ConversionFactor(14) = CDec(Graph_15_THOR(UBound(Graph_15_THOR))) * GraphCorrel(14)
        ConversionFactor(15) = CDec(Graph_16_THOR(UBound(Graph_16_THOR))) * GraphCorrel(15)
        ConversionFactor(16) = CDec(Graph_17_THOR(UBound(Graph_17_THOR))) * GraphCorrel(16)
        ConversionFactor(17) = CDec(Graph_18_THOR(UBound(Graph_18_THOR))) * GraphCorrel(17)
        ConversionFactor(18) = CDec(Graph_19_THOR(UBound(Graph_19_THOR))) * GraphCorrel(18)
        ConversionFactor(19) = CDec(Graph_20_THOR(UBound(Graph_20_THOR))) * GraphCorrel(19)
        ConversionFactor(20) = CDec(Graph_21_THOR(UBound(Graph_21_THOR))) * GraphCorrel(20)
        ConversionFactor(21) = CDec(Graph_22_THOR(UBound(Graph_22_THOR))) * GraphCorrel(21)
        ConversionFactor(22) = CDec(Graph_23_THOR(UBound(Graph_23_THOR))) * GraphCorrel(22)
        ConversionFactor(23) = CDec(Graph_24_THOR(UBound(Graph_24_THOR))) * GraphCorrel(23)
        ConversionFactor(24) = CDec(Graph_25_THOR(UBound(Graph_25_THOR))) * GraphCorrel(24)
        ConversionFactor(25) = CDec(Graph_26_THOR(UBound(Graph_26_THOR))) * GraphCorrel(25)
        ConversionFactor(26) = CDec(Graph_27_THOR(UBound(Graph_27_THOR))) * GraphCorrel(26)
        ConversionFactor(27) = CDec(Graph_28_THOR(UBound(Graph_28_THOR))) * GraphCorrel(27)
        ConversionFactor(28) = CDec(Graph_29_THOR(UBound(Graph_29_THOR))) * GraphCorrel(28)
        ConversionFactor(29) = CDec(Graph_30_THOR(UBound(Graph_30_THOR))) * GraphCorrel(29)
        ConversionFactor(30) = CDec(Graph_31_THOR(UBound(Graph_31_THOR))) * GraphCorrel(30)
        ConversionFactor(31) = CDec(Graph_32_THOR(UBound(Graph_32_THOR))) * GraphCorrel(31)
        ConversionFactor(32) = CDec(Graph_33_THOR(UBound(Graph_33_THOR))) * GraphCorrel(32)
        ConversionFactor(33) = CDec(Graph_34_THOR(UBound(Graph_34_THOR))) * GraphCorrel(33)
        ConversionFactor(34) = CDec(Graph_35_THOR(UBound(Graph_35_THOR))) * GraphCorrel(34)
        ConversionFactor(35) = CDec(Graph_36_THOR(UBound(Graph_36_THOR))) * GraphCorrel(35)
        ConversionFactor(36) = CDec(Graph_37_THOR(UBound(Graph_37_THOR))) * GraphCorrel(36)
        ConversionFactor(37) = CDec(Graph_38_THOR(UBound(Graph_38_THOR))) * GraphCorrel(37)
        ConversionFactor(38) = CDec(Graph_39_THOR(UBound(Graph_39_THOR))) * GraphCorrel(38)
        ConversionFactor(39) = CDec(Graph_40_THOR(UBound(Graph_40_THOR))) * GraphCorrel(39)
        ConversionFactor(40) = CDec(Graph_41_THOR(UBound(Graph_41_THOR))) * GraphCorrel(40)
        ConversionFactor(41) = CDec(Graph_42_THOR(UBound(Graph_42_THOR))) * GraphCorrel(41)
        ConversionFactor(42) = CDec(Graph_43_THOR(UBound(Graph_43_THOR))) * GraphCorrel(42)
        ConversionFactor(43) = CDec(Graph_44_THOR(UBound(Graph_44_THOR))) * GraphCorrel(43)
        ConversionFactor(44) = CDec(Graph_45_THOR(UBound(Graph_45_THOR))) * GraphCorrel(44)
        ConversionFactor(45) = CDec(Graph_46_THOR(UBound(Graph_46_THOR))) * GraphCorrel(45)
        ConversionFactor(46) = CDec(Graph_47_THOR(UBound(Graph_47_THOR))) * GraphCorrel(46)
        ConversionFactor(47) = CDec(Graph_48_THOR(UBound(Graph_48_THOR))) * GraphCorrel(47)

        MainMDI.ProgressBarMain.Value = MainMDI.ProgressBarMain.Maximum

        Me.Show()

        FileDrop = False
        DragFiles = Nothing

        MainMDI.Statuslbl.Text = "Complete"

        Me.Width = 1100

        EventPass = False

    End Sub

    Private Sub OpenWithXMADgicToolStripMenuItemToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenWithXMADgicToolStripMenuItemToolStripMenuItem.Click
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

    Private Sub SplitConMain_SplitterMoved(ByVal sender As System.Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitConMain.SplitterMoved

        Me.SplitConMain.Width = Me.ClientRectangle.Width
        With Me
            .PathLbl.Location = New Point(10, 20)
            .OpenLst.Location = New Point(12, .PathLbl.Location.Y + .PathLbl.Height + 10)

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
            .InjuryLbl1.Location = New Point(8, 10)
            .InjuryLbl1.Size = New Size(.ScoreBoard.Width - 26, 90)
            .Peak_Injury1.Location = New Point(8, .InjuryLbl1.Height + .InjuryLbl1.Location.Y + 3)
            .Peak_Injury1.Size = New Size(.InjuryLbl1.Width, .ScoreBoard.Height - .InjuryLbl1.Height - 46)

        End With

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

        If Me.OpenLst.SelectedIndex > 0 Then
            MainMDI.Statuslbl.Text = Me.OpenLst.Items(Me.OpenLst.SelectedIndex)
        End If

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

    Private Sub ReloadBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReloadBtn.Click
        MainMDI.ProgressBarMain.Value = 0
        MainMDI.ProgressBarMain.Maximum = 450

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

        '시트를 그린다.***************************************************************************
        MainMDI.Statuslbl.Text = "Drawing Injury Sheets"
        MainMDI.ProgressBarMain.Value = 150
        Sheet_US()
        '시트를 그린다.***************************************************************************

        MainMDI.ProgressBarMain.Value = MainMDI.ProgressBarMain.Maximum
        MainMDI.Statuslbl.Text = "Complete"
    End Sub

    Private Sub BtnInjuryGraph_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnInjuryGraph.Click
        MainMDI.ProgressBarMain.Value = 0
        MainMDI.ProgressBarMain.Maximum = (UBound(OpenFile.names) + 1) * 310 + 500

        Dim FrmInjuryGraphing As New FrmInjuryGraph_THOR(Tmp_Path, OpenFile.names, ParaMeterArr1, ParaMeterArr2, ConversionFactor)

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

    'US-NCAP Header
    Private Sub Sheet_Lbl()
        Dim i As Integer

        With Me.InjuryLbl1
            .Rows = 3
            .Cols = 24
            .FixedRows = 0
            .FixedCols = 0

            .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillRepeat

            .Row = 0
            .Col = 0
            .RowSel = 2
            .ColSel = 0
            .CellBackColor = Color.GhostWhite

            .Row = 0
            .Col = 3
            .RowSel = 2
            .ColSel = 8
            .CellBackColor = Color.GhostWhite

            .Row = 0
            .Col = 15
            .RowSel = 2
            .ColSel = 22
            .CellBackColor = Color.GhostWhite

            '.MergeCells = flexMergeRestrictAll
            '.SelectionMode = MSFlexGridLib.SelectionModeSettings.flexSelectionFree
            .MergeCells = 1       '셀병합 허용

            .set_MergeRow(0, True)
            .set_MergeRow(1, True)
            .set_MergeRow(2, True)

            .set_ColWidth(0, 630)
            .set_MergeCol(0, True)
            .set_ColAlignment(0, 4)
            For i = 1 To 23
                .set_MergeCol(i, True)
                .set_ColAlignment(i, 4) '가운데 정렬
                .set_ColWidth(i, 660 + 5)
            Next
            For i = 9 To 22
                .set_ColWidth(i, 660 + 9)
            Next

            'Total 1250
            .set_RowHeight(0, 400)
            .set_RowHeight(1, 400)
            .set_RowHeight(2, 450)

            .AllowBigSelection = True

            .set_TextMatrix(0, 0, "US" & vbCrLf & "NCAP")
            .set_TextMatrix(1, 0, .get_TextMatrix(0, 0))
            .set_TextMatrix(2, 0, .get_TextMatrix(0, 0))

            .set_TextMatrix(0, 1, "Head")
            .set_TextMatrix(1, 1, "BrIC")
            .set_TextMatrix(2, 1, .get_TextMatrix(1, 1))

            .set_TextMatrix(0, 2, .get_TextMatrix(0, 1))
            .set_TextMatrix(1, 2, "HIC15")
            .set_TextMatrix(2, 2, .get_TextMatrix(1, 2))

            .set_TextMatrix(0, 3, "Neck")
            .set_TextMatrix(1, 3, "Tens" & vbCrLf & "[kN]")
            .set_TextMatrix(2, 3, .get_TextMatrix(1, 3))

            .set_TextMatrix(0, 4, .get_TextMatrix(0, 3))
            .set_TextMatrix(1, 4, "Comp" & vbCrLf & "[kN]")
            .set_TextMatrix(2, 4, .get_TextMatrix(1, 4))

            .set_TextMatrix(0, 5, .get_TextMatrix(0, 3))
            .set_TextMatrix(1, 5, "NTE")
            .set_TextMatrix(2, 5, .get_TextMatrix(1, 5))

            .set_TextMatrix(0, 6, .get_TextMatrix(0, 3))
            .set_TextMatrix(1, 6, "NTF")
            .set_TextMatrix(2, 6, .get_TextMatrix(1, 6))

            .set_TextMatrix(0, 7, .get_TextMatrix(0, 3))
            .set_TextMatrix(1, 7, "NCE")
            .set_TextMatrix(2, 7, .get_TextMatrix(1, 7))

            .set_TextMatrix(0, 8, .get_TextMatrix(0, 3))
            .set_TextMatrix(1, 8, "NCF")
            .set_TextMatrix(2, 8, .get_TextMatrix(1, 8))

            .set_TextMatrix(0, 9, "Chest")
            .set_TextMatrix(1, 9, "Displacement [mm]")
            .set_TextMatrix(2, 9, "Upr." & vbCrLf & "LH")

            .set_TextMatrix(0, 10, .get_TextMatrix(0, 9))
            .set_TextMatrix(1, 10, .get_TextMatrix(1, 9))
            .set_TextMatrix(2, 10, "Upr." & vbCrLf & "RH")

            .set_TextMatrix(0, 11, .get_TextMatrix(0, 9))
            .set_TextMatrix(1, 11, .get_TextMatrix(1, 9))
            .set_TextMatrix(2, 11, "Lwr." & vbCrLf & "LH")

            .set_TextMatrix(0, 12, .get_TextMatrix(0, 9))
            .set_TextMatrix(1, 12, .get_TextMatrix(1, 9))
            .set_TextMatrix(2, 12, "Lwr." & vbCrLf & "RH")

            .set_TextMatrix(0, 13, .get_TextMatrix(0, 9))
            .set_TextMatrix(1, 13, "Avr.")
            .set_TextMatrix(2, 13, .get_TextMatrix(1, 13))

            .set_TextMatrix(0, 14, .get_TextMatrix(0, 9))
            .set_TextMatrix(1, 14, "3ms" & vbCrLf & "[G]")
            .set_TextMatrix(2, 14, .get_TextMatrix(1, 14))

            .set_TextMatrix(0, 15, "Lower Extremity")
            .set_TextMatrix(1, 15, "Femur [kN]")
            .set_TextMatrix(2, 15, "LH")

            .set_TextMatrix(0, 16, .get_TextMatrix(0, 15))
            .set_TextMatrix(1, 16, .get_TextMatrix(1, 15))
            .set_TextMatrix(2, 16, "RH")

            .set_TextMatrix(0, 17, .get_TextMatrix(0, 15))
            .set_TextMatrix(1, 17, "Tibia Index")
            .set_TextMatrix(2, 17, "Upr." & vbCrLf & "LH")

            .set_TextMatrix(0, 18, .get_TextMatrix(0, 15))
            .set_TextMatrix(1, 18, .get_TextMatrix(1, 17))
            .set_TextMatrix(2, 18, "Upr." & vbCrLf & "RH")

            .set_TextMatrix(0, 19, .get_TextMatrix(0, 15))
            .set_TextMatrix(1, 19, .get_TextMatrix(1, 17))
            .set_TextMatrix(2, 19, "Lwr." & vbCrLf & "LH")

            .set_TextMatrix(0, 20, .get_TextMatrix(0, 15))
            .set_TextMatrix(1, 20, .get_TextMatrix(1, 17))
            .set_TextMatrix(2, 20, "Lwr." & vbCrLf & "RH")

            .set_TextMatrix(0, 21, .get_TextMatrix(0, 15))
            .set_TextMatrix(1, 21, "Tibia Comp.")
            .set_TextMatrix(2, 21, "LH")

            .set_TextMatrix(0, 22, .get_TextMatrix(0, 15))
            .set_TextMatrix(1, 22, .get_TextMatrix(1, 21))
            .set_TextMatrix(2, 22, "RH")

            .set_TextMatrix(0, 23, "Total")
            .set_TextMatrix(1, 23, "Total")
            .set_TextMatrix(2, 23, "Total")
        End With
    End Sub

    'US-NCAP Injury Sheet
    Private Sub Sheet_US()
        '상해 점수 시트 (북미 THOR)
        Dim i As Integer
        Dim file_tot As Integer
        file_tot = OpenFile.names.Length
        Tot_File = file_tot

        Peak_Injury1.Rows = file_tot * 3
        Peak_Injury1.Cols = 24

        With Peak_Injury1
            .MergeCells = 1
            .SelectionMode = MSFlexGridLib.SelectionModeSettings.flexSelectionFree

            .set_ColWidth(0, 630)
            .set_MergeCol(0, True)
            .set_ColAlignment(0, 4)
            For i = 1 To 23
                .set_MergeCol(i, True)
                .set_ColAlignment(i, 4) '가운데 정렬
                .set_ColWidth(i, 665)
            Next
            For i = 9 To 22
                .set_ColWidth(i, 660 + 9)
            Next

            ReDim Star_Rating(OpenFile.names.Length - 1)

            For i = 1 To OpenFile.names.Length

                .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillRepeat
                .Row = (i - 1) * 3 + 1
                .Col = 1
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 22
                .CellFontBold = False
                .CellForeColor = Color.Gray
                '.CellBackColor = Color.LightGray

                .Row = (i - 1) * 3
                .Col = 0
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 0
                .CellBackColor = Color.GhostWhite

                .Row = (i - 1) * 3
                .Col = 3
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 8
                .CellBackColor = Color.GhostWhite

                .Row = (i - 1) * 3
                .Col = 15
                .RowSel = (i - 1) * 3 + 2
                .ColSel = 22
                .CellBackColor = Color.GhostWhite

                .set_RowHeight((i - 1) * 3, 250)
                .set_RowHeight((i - 1) * 3 + 1, 250)
                .set_RowHeight((i - 1) * 3 + 2, 250)

                If DummyType(i - 1) = 0 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[50%]")
                ElseIf DummyType(i - 1) = 1 Then
                    .set_TextMatrix((i - 1) * 3, 0, "CASE #." & i & vbCrLf & "[5%]")
                ElseIf DummyType(i - 1) = 2 Then
                    .set_TextMatrix((i - 1) * 3, 0, "MD" & vbCrLf & "# " & i)
                End If
                .set_TextMatrix((i - 1) * 3 + 1, 0, .get_TextMatrix((i - 1) * 3, 0))
                .set_TextMatrix((i - 1) * 3 + 2, 0, .get_TextMatrix((i - 1) * 3, 0))

                .set_TextMatrix((i - 1) * 3, 1, Format(BrIC(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3 + 1, 1, "Portion")
                .set_TextMatrix((i - 1) * 3 + 2, 1, "P")

                .set_TextMatrix((i - 1) * 3, 2, Format(HIC15(i - 1), "###0.0"))
                .set_TextMatrix((i - 1) * 3 + 1, 2, Format(HIC15(i - 1) / 700, "0.0%"))
                .set_TextMatrix((i - 1) * 3 + 2, 2, Format(P_Head(i - 1), "0.00%"))

                .set_TextMatrix((i - 1) * 3, 3, Format(Neck_Tens(i - 1), "0.000"))
                If DummyType(i - 1) = 0 Or DummyType(i - 1) = 2 Then  'THOR and Hybrid 50%
                    .set_TextMatrix((i - 1) * 3 + 1, 3, Format(Neck_Tens(i - 1) / 4.17, "0.0%"))
                    .set_TextMatrix((i - 1) * 3 + 2, 3, Format(P_Neck_Tens(i - 1), "0.00%"))
                ElseIf DummyType(i - 1) = 1 Then
                    .set_TextMatrix((i - 1) * 3 + 1, 3, Format(Neck_Tens(i - 1) / 2.62, "0.0%"))
                    .set_TextMatrix((i - 1) * 3 + 2, 3, Format(P_Neck_Tens(i - 1), "0.00%"))
                End If

                .set_TextMatrix((i - 1) * 3, 4, Format(Neck_Comp(i - 1), "0.000"))
                If DummyType(i - 1) = 0 Or DummyType(i - 1) = 2 Then  'THOR and Hybrid 50%
                    .set_TextMatrix((i - 1) * 3 + 1, 4, Format(Neck_Comp(i - 1) / 4, "0.0%"))
                    .set_TextMatrix((i - 1) * 3 + 2, 4, Format(P_Neck_Comp(i - 1), "0.00%"))
                ElseIf DummyType(i - 1) = 1 Then
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

                .set_TextMatrix((i - 1) * 3, 9, Format(ThxIrUpL(i - 1), "##.0"))
                If DummyType(i - 1) = 0 Then  'Hybrid 50%
                    .set_TextMatrix((i - 1) * 3 + 1, 9, Format(Chest_D(i - 1) / 63, "0.0%"))
                    .set_TextMatrix((i - 1) * 3 + 2, 9, Format(P_CD(i - 1), "0.00%"))
                ElseIf DummyType(i - 1) = 1 Then
                    .set_TextMatrix((i - 1) * 3 + 1, 9, Format(Chest_D(i - 1) / 52, "0.0%"))
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

                .set_TextMatrix((i - 1) * 3, 10, Format(ThxIrUpR(i - 1), "0.0"))
                .set_TextMatrix((i - 1) * 3 + 1, 10, Format(Chest_G_CUMULATIVE_T3MS_inj(i - 1) / 60.0, "0.0%"))
                .set_TextMatrix((i - 1) * 3 + 2, 10, "-")

                .set_TextMatrix((i - 1) * 3, 11, Format(ThxIrLowL(i - 1), "0.0"))
                If DummyType(i - 1) = 0 Or DummyType(i - 1) = 2 Then  'THOR and Hybrid 50%
                    .set_TextMatrix((i - 1) * 3 + 1, 11, Format(FemurL(i - 1) / 10, "0.0%"))
                    .set_TextMatrix((i - 1) * 3 + 2, 11, Format(P_FemurL(i - 1), "0.00%"))
                ElseIf DummyType(i - 1) = 1 Then
                    .set_TextMatrix((i - 1) * 3 + 1, 11, Format(FemurL(i - 1) / 6.81, "0.0%"))
                    .set_TextMatrix((i - 1) * 3 + 2, 11, Format(P_FemurL(i - 1), "0.00%"))
                End If

                .set_TextMatrix((i - 1) * 3, 12, Format(ThxIrLowR(i - 1), "0.0"))

                .set_TextMatrix((i - 1) * 3, 13, Format((ThxIrUpL(i - 1) + ThxIrUpR(i - 1) + ThxIrLowL(i - 1) + ThxIrLowR(i - 1)) / 4, "0.0"))

                .set_TextMatrix((i - 1) * 3, 14, Format(Chest_G_CUMULATIVE_T3MS_inj(i - 1), "0.0"))
                If DummyType(i - 1) = 0 Or DummyType(i - 1) = 2 Then  'THOR and Hybrid 50%
                    .set_TextMatrix((i - 1) * 3 + 1, 12, Format(FemurR(i - 1) / 10, "0.0%"))
                    .set_TextMatrix((i - 1) * 3 + 2, 12, Format(P_FemurR(i - 1), "0.00%"))
                ElseIf DummyType(i - 1) = 1 Then
                    .set_TextMatrix((i - 1) * 3 + 1, 12, Format(FemurR(i - 1) / 6.81, "0.0%"))
                    .set_TextMatrix((i - 1) * 3 + 2, 12, Format(P_FemurR(i - 1), "0.00%"))
                End If

                .set_TextMatrix((i - 1) * 3, 15, Format(FemurL(i - 1), "0.000"))
                .set_TextMatrix((i - 1) * 3, 16, Format(FemurR(i - 1), "0.000"))

                .set_TextMatrix((i - 1) * 3, 17, Format(TI_upr_L(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3, 18, Format(TI_upr_R(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3, 19, Format(TI_lwr_L(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3, 20, Format(TI_lwr_R(i - 1), "###0.00"))

                .set_TextMatrix((i - 1) * 3, 21, Format(Tibia_Comp_L(i - 1), "###0.00"))
                .set_TextMatrix((i - 1) * 3, 22, Format(Tibia_Comp_R(i - 1), "###0.00"))

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
                .set_TextMatrix((i - 1) * 3, 23, Tmp_Star & vbCrLf & "★")
                .set_TextMatrix((i - 1) * 3 + 1, 23, .get_TextMatrix((i - 1) * 3, 23))
                .set_TextMatrix((i - 1) * 3 + 2, 23, .get_TextMatrix((i - 1) * 3, 23))

            Next

        End With
    End Sub

    Private Sub BtnValGraph_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnValGraph.Click
        Dim InValGraph As New FrmInjuryValGraph_THOR(Head3MSG, Chest_G_CUMULATIVE_T3MS_inj, HIC15, HIC36, NTE, NTF, NCE, NCF, Head_Peak_G, Chest_D, _
                                                     KneeS_L, KneeS_R, Tibia_Comp_L, Tibia_Comp_R, TI_upr_L, TI_lwr_L, TI_upr_R, TI_lwr_R, Chest_VC, _
                                                     FemurL, FemurR, Neck_Comp, Neck_Tens, Neck_Shear, Neck_Exten, Star_Rating, AngularVelX, AngularVelY, _
                                                     AngularVelZ, BrIC, ThxIrUpL, ThxIrUpR, ThxIrLowL, ThxIrLowR)

        With InValGraph
            .Owner = Me
            .InjuryLst.Items.Insert(0, "Head 3ms G")  '===== THOR 없음
            .InjuryLst.Items.Insert(1, "Chest 3ms G")
            .InjuryLst.Items.Insert(2, "BrIC")
            .InjuryLst.Items.Insert(3, "Max. Angular Velocity X")
            .InjuryLst.Items.Insert(4, "Max. Angular Velocity Y")
            .InjuryLst.Items.Insert(5, "Max. Angular Velocity Z")
            .InjuryLst.Items.Insert(6, "HIC15")
            .InjuryLst.Items.Insert(7, "HIC36")
            .InjuryLst.Items.Insert(8, "NTE")
            .InjuryLst.Items.Insert(9, "NTF")
            .InjuryLst.Items.Insert(10, "NCE")
            .InjuryLst.Items.Insert(11, "NCF")
            .InjuryLst.Items.Insert(12, "Head Peak G") '===== THOR 없음
            .InjuryLst.Items.Insert(13, "Average CD")  '===== Average Value
            .InjuryLst.Items.Insert(14, "Chest Upper Left")
            .InjuryLst.Items.Insert(15, "Chest Upper Right")
            .InjuryLst.Items.Insert(16, "Chest Lower Left")
            .InjuryLst.Items.Insert(17, "Chest Lower Right")
            .InjuryLst.Items.Insert(18, "Knee Slide (L)")
            .InjuryLst.Items.Insert(19, "Knee Slide (R)")
            .InjuryLst.Items.Insert(20, "Tibia Comp (L)")
            .InjuryLst.Items.Insert(21, "Tibia Comp (R)")
            .InjuryLst.Items.Insert(22, "Tibia Index Upper (L)")
            .InjuryLst.Items.Insert(23, "Tibia Index Lower (L)")
            .InjuryLst.Items.Insert(24, "Tibia Index Upper (R)")
            .InjuryLst.Items.Insert(25, "Tibia Index Lower (R)")
            .InjuryLst.Items.Insert(26, "Chest VC") '===== THOR 없음
            .InjuryLst.Items.Insert(27, "Femur (L)")
            .InjuryLst.Items.Insert(28, "Femur (R)")
            .InjuryLst.Items.Insert(29, "Neck Comp.")
            .InjuryLst.Items.Insert(30, "Neck Tens.")
            .InjuryLst.Items.Insert(31, "Neck Shear")
            .InjuryLst.Items.Insert(32, "Neck Exten.")
            .InjuryLst.Items.Insert(33, "Star Rating")

            .InjuryValChrt.Hide()

            .Show()

        End With
    End Sub

    Private Sub PPTBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PPTBtn.Click

        Select Case Me.ScoreBoard.SelectedIndex
            Case 0
                PPTexportUSNCAP_THOR()
        End Select

        '================================================================================================
        Try
            Using NewfileNum As StreamWriter = File.AppendText(RemoteValFolder & System.Environment.UserName)
                NewfileNum.WriteLine("##")
                NewfileNum.WriteLine("## Export Table - THOR")
                NewfileNum.Close()
            End Using
        Catch ex As Exception

        End Try
        '================================================================================================

    End Sub

    Private Sub PPTexportUSNCAP_THOR()

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
        objCustomLayout = objPres.SlideMaster.CustomLayouts.Item(1)  '마스터를 잘 설정해야한다.
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
        Dim Tot_Row As Integer = 3 + 2 * (Tot_File)
        Dim Tot_Col As Integer = 25 'THOR 시트
        objPres.Slides(1).Select()

        objShape = objPres.Slides(1).Shapes.AddTable(1, 1, 10, 80, 700)
        objTable = objShape.Table

        With objShape.Table
            .Cell(1, 1).Shape.Fill.ForeColor.RGB = RGB(Color.White.R, Color.White.G, Color.White.B)  ' 여기서 지정하는 색이 전체 색을 결정
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Bold = MsoTriState.msoTrue
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Size = 8
            .Cell(1, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            .Cell(1, 1).Shape.TextFrame.VerticalAnchor = MsoVerticalAnchor.msoAnchorMiddle
            .Cell(1, 1).Shape.TextFrame.HorizontalAnchor = MsoHorizontalAnchor.msoAnchorCenter
            .Cell(1, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(1, 1).Shape.TextFrame.MarginTop = 0
            .Cell(1, 1).Shape.TextFrame.MarginLeft = 0
            .Cell(1, 1).Shape.TextFrame.MarginRight = 0
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).Weight = 1
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderBottom).ForeColor.RGB = RGB(Color.DarkGray.R, Color.DarkGray.G, Color.DarkGray.B)
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderRight).ForeColor.RGB = RGB(Color.DarkGray.R, Color.DarkGray.G, Color.DarkGray.B)
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderLeft).ForeColor.RGB = RGB(Color.DarkGray.R, Color.DarkGray.G, Color.DarkGray.B)
            .Cell(1, 1).Borders(PowerPoint.PpBorderType.ppBorderTop).ForeColor.RGB = RGB(Color.DarkGray.R, Color.DarkGray.G, Color.DarkGray.B)

            For i = 1 To Tot_Col - 1
                .Columns.Add()
            Next
            .Cell(1, 2).Borders(PowerPoint.PpBorderType.ppBorderRight).Weight = 1.5
            .Cell(1, 4).Borders(PowerPoint.PpBorderType.ppBorderRight).Weight = 1.5
            .Cell(1, 10).Borders(PowerPoint.PpBorderType.ppBorderRight).Weight = 1.5
            .Cell(1, 16).Borders(PowerPoint.PpBorderType.ppBorderRight).Weight = 1.5
            .Cell(1, 24).Borders(PowerPoint.PpBorderType.ppBorderRight).Weight = 1.5
            .Cell(1, 1).Shape.Fill.ForeColor.RGB = RGB(Color.GhostWhite.R, Color.GhostWhite.G, Color.GhostWhite.B)
            .Cell(1, 2).Shape.Fill.ForeColor.RGB = RGB(Color.GhostWhite.R, Color.GhostWhite.G, Color.GhostWhite.B)
            For i = 5 To 10
                .Cell(1, i).Shape.Fill.ForeColor.RGB = RGB(Color.GhostWhite.R, Color.GhostWhite.G, Color.GhostWhite.B)
            Next
            For i = 17 To 24
                .Cell(1, i).Shape.Fill.ForeColor.RGB = RGB(Color.GhostWhite.R, Color.GhostWhite.G, Color.GhostWhite.B)
            Next

            'Header
            For i = 1 To 2
                .Rows.Add()
            Next

            'Injury Tables
            'Potion은 Export 하지 않는다.
            .Rows.Add()
            .Rows(1).Height = 12
            .Rows(2).Height = 10
            .Rows(3).Height = 10
            .Rows(4).Height = 15
            '.Cell(4, 1).Shape.Fill.ForeColor.RGB = RGB(Color.White.R, Color.White.G, Color.White.B)
            '.Cell(4, 1).Shape.TextFrame.TextRange.Font.Name = "맑은 고딕"
            '.Cell(4, 1).Shape.TextFrame.TextRange.Font.Size = 8
            .Cell(4, 1).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
            .Cell(4, 1).Shape.TextFrame.MarginBottom = 0
            .Cell(4, 1).Shape.TextFrame.MarginTop = 0

            '.Cell(4, 2).Shape.TextFrame.TextRange.Font.Color.RGB = RGB(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B)

            For i = 2 To Tot_File * 2
                .Rows.Add()
                .Rows(i + 3).Height = 15
            Next

            'Cell Merge and Adjust
            .Cell(1, 1).Merge(MergeTo:=.Cell(3, 1))   'US-NCAP
            .Cell(2, 2).Merge(MergeTo:=.Cell(3, 2))   '상해
            .Cell(1, 3).Merge(MergeTo:=.Cell(1, 4))   'Head
            .Cell(1, 5).Merge(MergeTo:=.Cell(1, 10))   'Neck
            .Cell(1, 11).Merge(MergeTo:=.Cell(1, 16)) 'Chest
            .Cell(1, 17).Merge(MergeTo:=.Cell(1, 24)) 'Lower Extremity
            .Cell(1, 25).Merge(MergeTo:=.Cell(3, 25)) 'Total Rating

            .Cell(2, 3).Merge(MergeTo:=.Cell(3, 3)) 'BrIC
            .Cell(2, 4).Merge(MergeTo:=.Cell(3, 4)) 'HIC15
            .Cell(2, 5).Merge(MergeTo:=.Cell(3, 5)) 'Tens.[kN]
            .Cell(2, 6).Merge(MergeTo:=.Cell(3, 6)) 'Comps.[kN]
            .Cell(2, 7).Merge(MergeTo:=.Cell(3, 7)) 'NTE
            .Cell(2, 8).Merge(MergeTo:=.Cell(3, 8)) 'NTF
            .Cell(2, 9).Merge(MergeTo:=.Cell(3, 9)) 'NCE
            .Cell(2, 10).Merge(MergeTo:=.Cell(3, 10)) 'NCF
            .Cell(2, 11).Merge(MergeTo:=.Cell(2, 14)) 'Displacement [mm]
            .Cell(2, 15).Merge(MergeTo:=.Cell(3, 15)) 'Avr. [mm]
            .Cell(2, 16).Merge(MergeTo:=.Cell(3, 16)) '3ms [G]

            .Cell(2, 17).Merge(MergeTo:=.Cell(2, 18)) 'Femur [kN]
            .Cell(2, 19).Merge(MergeTo:=.Cell(2, 22)) 'Tibia Index
            .Cell(2, 23).Merge(MergeTo:=.Cell(2, 24)) 'Tibia Comp.

            'Writing Labels
            .Cell(1, 1).Shape.TextFrame.TextRange.Text = "US-" & vbCrLf & "NCAP" & vbCrLf & "THOR"
            .Cell(1, 2).Shape.TextFrame.TextRange.Text = "구분"
            .Cell(2, 2).Shape.TextFrame.TextRange.Text = "상해"
            .Cell(1, 3).Shape.TextFrame.TextRange.Text = "Head"
            .Cell(1, 5).Shape.TextFrame.TextRange.Text = "Neck"
            .Cell(1, 11).Shape.TextFrame.TextRange.Text = "Chest"
            .Cell(1, 17).Shape.TextFrame.TextRange.Text = "Lower Extremity"
            .Cell(1, 25).Shape.TextFrame.TextRange.Text = "Total"

            .Cell(2, 3).Shape.TextFrame.TextRange.Text = "BrIC"
            .Cell(2, 4).Shape.TextFrame.TextRange.Text = "HIC15"
            .Cell(2, 5).Shape.TextFrame.TextRange.Text = "Tens." & vbCrLf & "[kN]"
            .Cell(2, 6).Shape.TextFrame.TextRange.Text = "Comps." & vbCrLf & "[kN]"
            .Cell(2, 7).Shape.TextFrame.TextRange.Text = "NTE"
            .Cell(2, 8).Shape.TextFrame.TextRange.Text = "NTF"
            .Cell(2, 9).Shape.TextFrame.TextRange.Text = "NCE"
            .Cell(2, 10).Shape.TextFrame.TextRange.Text = "NCF"
            .Cell(2, 11).Shape.TextFrame.TextRange.Text = "Displacement [mm]"
            .Cell(3, 11).Shape.TextFrame.TextRange.Text = "Upr." & vbCrLf & "LH"
            .Cell(3, 12).Shape.TextFrame.TextRange.Text = "Upr." & vbCrLf & "RH"
            .Cell(3, 13).Shape.TextFrame.TextRange.Text = "Lwr." & vbCrLf & "LH"
            .Cell(3, 14).Shape.TextFrame.TextRange.Text = "Lwr." & vbCrLf & "RH"
            .Cell(2, 15).Shape.TextFrame.TextRange.Text = "Avr." & vbCrLf & "[mm]"
            .Cell(2, 16).Shape.TextFrame.TextRange.Text = "3ms" & vbCrLf & "[G]"

            .Cell(2, 17).Shape.TextFrame.TextRange.Text = "Femur [kN]"
            .Cell(3, 17).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 18).Shape.TextFrame.TextRange.Text = "RH"
            .Cell(2, 19).Shape.TextFrame.TextRange.Text = "Tibia Index"
            .Cell(3, 19).Shape.TextFrame.TextRange.Text = "Upr." & vbCrLf & "LH"
            .Cell(3, 20).Shape.TextFrame.TextRange.Text = "Upr." & vbCrLf & "RH"
            .Cell(3, 21).Shape.TextFrame.TextRange.Text = "Lwr." & vbCrLf & "LH"
            .Cell(3, 22).Shape.TextFrame.TextRange.Text = "Lwr." & vbCrLf & "RH"
            .Cell(2, 23).Shape.TextFrame.TextRange.Text = "Tibia Comp."
            .Cell(3, 23).Shape.TextFrame.TextRange.Text = "LH"
            .Cell(3, 24).Shape.TextFrame.TextRange.Text = "RH"

            For nRow = 4 To Tot_Row Step 2
                .Cell(nRow, 1).Merge(MergeTo:=.Cell(nRow + 1, 1))
                .Cell(nRow, Tot_Col).Merge(MergeTo:=.Cell(nRow + 1, Tot_Col))

                'Writing Injuries
                .Cell(nRow, 1).Shape.TextFrame.TextRange.Text = Me.Peak_Injury1.get_TextMatrix(((nRow - 4) / 2) * 3, 0)
                .Cell(nRow, 2).Shape.TextFrame.TextRange.Text = "Val."
                .Cell(nRow + 1, 2).Shape.TextFrame.TextRange.Text = "P"
                For nCol = 3 To Tot_Col
                    .Cell(nRow, nCol).Shape.TextFrame.TextRange.Text = Replace(Me.Peak_Injury1.get_TextMatrix(((nRow - 4) / 2) * 3, nCol - 2), vbCrLf, "")
                    .Cell(nRow + 1, nCol).Shape.TextFrame.TextRange.Text = Replace(Me.Peak_Injury1.get_TextMatrix(((nRow - 4) / 2) * 3 + 1, nCol - 2), vbCrLf, "")
                Next
            Next

            ''Line
            For i = 1 To 25
                '    .Cell(1, i).Borders(PowerPoint.PpBorderType.ppBorderTop).ForeColor.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
                .Cell(3, i).Borders(PowerPoint.PpBorderType.ppBorderBottom).ForeColor.RGB = RGB(Color.Black.R, Color.Black.G, Color.Black.B)
                .Cell(3, i).Borders(PowerPoint.PpBorderType.ppBorderBottom).Weight = 1.5
            Next

            '
            'Clean up PowerPoint Object
            objTable = Nothing
            objShape = Nothing

        End With
    End Sub

End Class