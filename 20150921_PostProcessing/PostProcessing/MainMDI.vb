Imports System.IO
Imports ExpireClass
Imports WinSCP
Imports ConUser


Public Class MainMDI

    Dim CheckExpire As New ExpireClass.ExpireCondition

    Public Confirm As New ConUser.ConUser

    Public Sub New()

        ' 이 호출은 디자이너에 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하십시오.

        Me.AllowDrop = True

    End Sub

    Private MyMDIClient As New MdiClient

    Private Sub MainMDI_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        Try
            Using NewfileNum As StreamWriter = File.AppendText(RemoteValFolder & System.Environment.UserName)
                NewfileNum.WriteLine("##")
                NewfileNum.WriteLine("## MDV Program Ending Normally : " & Date.Now.ToString & " ##")
                NewfileNum.Close()
            End Using

            If Connected = True Then mySession.Close()

            If Directory.Exists(Application.StartupPath & "\TempResults") Then
                Directory.Delete(Application.StartupPath & "\TempResults", True)
            End If

        Catch ex As Exception
            'End
        End Try
    End Sub

    Private Sub MainMDI_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop

        Dim i, m As Integer
        Dim Tmp_Files() As String
        Dim InitialFiles() As String
        Dim IsSame As Boolean = False
        Dim InPeak As Boolean = False

        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            ' Assign the file names to a string array, in  
            ' case the user has selected multiple files. 
            InitialFiles = CType(e.Data.GetData(DataFormats.FileDrop), String())

            For i = 0 To UBound(InitialFiles)
                Tmp_Files = StrReverse(InitialFiles(i)).Split(".")
                If StrReverse(Tmp_Files(LBound(Tmp_Files))) = "peak" Then InPeak = True
            Next

            If InPeak = False Then
                MsgBox("Cannot found Peak File...", MsgBoxStyle.Information, "파일이 없습니다.")
                Exit Sub
            End If

            For i = 0 To UBound(InitialFiles)
                Tmp_Files = InitialFiles(i).Split(".")
                If System.IO.File.Exists(Mid(InitialFiles(i), 1, Len(InitialFiles(i)) - Len(Tmp_Files(UBound(Tmp_Files)))) & "peak") Then
                    If IsNothing(DragFiles) Then
                        ReDim DragFiles(0)
                        DragFiles(0) = Mid(InitialFiles(i), 1, Len(InitialFiles(i)) - Len(Tmp_Files(UBound(Tmp_Files)))) & "peak"
                    Else
                        For m = 0 To UBound(DragFiles)
                            'For k = 0 To UBound(InitialFiles)
                            If DragFiles(m) = (Mid(InitialFiles(i), 1, Len(InitialFiles(i)) - Len(Tmp_Files(UBound(Tmp_Files)))) & "peak") Then
                                IsSame = True
                                Exit For
                            Else
                                IsSame = False
                            End If
                            'Next
                        Next

                        If IsSame = False Then
                            ReDim Preserve DragFiles(UBound(DragFiles) + 1)
                            DragFiles(UBound(DragFiles)) = Mid(InitialFiles(i), 1, Len(InitialFiles(i)) - Len(Tmp_Files(UBound(Tmp_Files)))) & "peak"
                        Else

                        End If

                        IsSame = False

                    End If
                End If
            Next

            Array.Sort(DragFiles)

            'For i = 0 To UBound(DragFiles)
            '    'If Mid(DragFiles(i), Len(DragFiles(i)) - 3) = "peak" Then
            '    'Else

            '    'End If
            '    FileValidate = New StreamReader(DragFiles(i))
            '    FileValidate.ReadLine()
            '    If InStr(FileValidate.ReadLine, "MADYMO") Then

            '    End If
            '    FileValidate.Close()
            'Next

            FileDrop = True

            Call ToolBtnNCAP_Click(Me, EventArgs.Empty)

        End If
    End Sub

    Private Sub MainMDI_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragOver
        e.Effect = DragDropEffects.All
    End Sub

    Private Sub MainMDI_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        Me.Hide()

        Dim BusinessTripEnd As Date = #5/30/2018#
        If DateDiff(DateInterval.Day, Date.Now, BusinessTripEnd) > 0 Then
            TempLicExpire = True
        Else
            TempLicExpire = False
        End If

        Select Case Confirm.ConfigureWho(Application.StartupPath)
            Case True

            Case False
                'SplashScreenStart.Close()
                'End
        End Select

        Me.ProgressBarMain.Value = 0
        Me.ProgressBarMain.Maximum = 1140
        'Me.Text = "MADYMO Data Viewer" & ' - " & DateDiff(DateInterval.Day, CurrentDate, ExpireDate) & " Days"
        'Me.Text = System.String.Format(Me.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor)
        Me.Text = "MADYMO Data Viewer" & " [" & String.Format("Ver. {0}", My.Application.Info.Version.ToString) & "]" & " - Sunghun, Chang"

        Me.MADYMOPSMFileConverterToolStripMenuItem.ToolTipText = "MADYMO PSM File" & vbCrLf & " - Use Only the LS-DYNA Time History File Format"

        Select Case System.Environment.UserName
            Case "6002317"
                Me.UserConfigure.Text = "Developer"
            Case "5805793"
                Me.UserConfigure.Text = "양현모"
            Case "6304537"
                Me.UserConfigure.Text = "박홍익"
            Case "9562745"
                Me.UserConfigure.Text = "최성철"
            Case "6005885"
                Me.UserConfigure.Text = "한광철"
            Case "5405228"
                Me.UserConfigure.Text = "김시열"
            Case "5502875"
                Me.UserConfigure.Text = "서보필"
            Case "6333467"
                Me.UserConfigure.Text = "조현덕"
            Case "5504584"
                Me.UserConfigure.Text = "송은섭"
            Case "9163467"
                Me.UserConfigure.Text = "김원철"
            Case "5300641"
                Me.UserConfigure.Text = "조성수"
            Case "6427553"
                Me.UserConfigure.Text = "배지예"
            Case Else
                Me.UserConfigure.Text = System.Environment.UserName
        End Select

        Me.UserDomainName.Text = System.Environment.UserDomainName
        Me.StatusStrip.Height = 35
        Me.DeveloperLbl.Text = "장성훈 연구원" & vbCrLf & "T.0149"
        Me.DeveloperLbl.ForeColor = Color.Gray

        With Me
            .기타기능EToolStripMenuItem.Enabled = Confirm.ProgramRefine
            .기타기능EToolStripMenuItem.Visible = Confirm.ProgramRefine

            .FEConverterToolStripMenuItem.Enabled = Confirm.ProgramRefine
            .FEConverterToolStripMenuItem.Visible = Confirm.ProgramRefine

            .MADYMOPSMFileConverterToolStripMenuItem.Enabled = Confirm.ProgramRefine
            .MADYMOPSMFileConverterToolStripMenuItem.Visible = Confirm.ProgramRefine

            .THORRMDBToolStripMenuItem.Enabled = Confirm.ProgramRefine
            .THORRMDBToolStripMenuItem.Visible = Confirm.ProgramRefine
            .OpenTHORTToolStripMenuItem.Enabled = Confirm.ProgramRefine
            .OpenTHORTToolStripMenuItem.Visible = Confirm.ProgramRefine
            .SetDataProfileTHORToolStripMenuItem.Visible = Confirm.ProgramRefine
            .ToolBtnTHOR.Enabled = Confirm.ProgramRefine
            .ToolBtnTHOR.Visible = Confirm.ProgramRefine
            .ToolBtnTEST_THOR.Enabled = Confirm.ProgramRefine
            .ToolBtnTEST_THOR.Visible = Confirm.ProgramRefine
        End With

        Try
            'File.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) & "\장성훈\MADYMO Data Viewer\ReadingAnalysisDATA.DLL")
            If Confirm.ProgramRefine = False And File.Exists("C:\Program Files\장성훈\MADYMO Data Viewer\ReadingAnalysisDATA.DLL") Then
                System.IO.File.Delete("C:\Program Files\장성훈\MADYMO Data Viewer\ReadingAnalysisDATA.DLL")
                System.IO.File.Delete("C:\Program Files\장성훈\MADYMO Data Viewer\BatchRun.exe")
                System.IO.File.Delete("C:\Program Files\장성훈\MADYMO Data Viewer\MSFLXGRD.OCX")
            End If

        Catch ex As Exception

        End Try

        Select Case System.Environment.UserDomainName
            Case "HKMC", "HOME-PC", "성훈미경-PC"
            Case Else
                File.Delete(Application.StartupPath & "\DATA\MADYMO_FE_Materials.xml")
                SplashScreenStart.Close()
                ''종료 경고문구 
                MsgBox("Program End" & vbCrLf & "Unauthenticated Company", , "Running Termination")
                End
        End Select

        '회사 : 6002317 / HKMC
        '집 : 장성훈 / HOME-PC

        'Command Line Arguements Handling Methods
        'Dim s() As String = System.Environment.GetCommandLineArgs()

        'If s.Length >= 2 Then
        '    For i = 1 To s.Length - 1
        '        MsgBox(s(i))
        '    Next
        'End If

        'If TempLicExpire = False Then

        '    '원격 로그 사용자 확인
        '    If File.Exists(RemoteValFolder & "MDV_USER_Lst.txt") = True Then

        '        Dim IsValidUser As Boolean = False
        '        Dim User_Lst As New FileStream(RemoteValFolder & "MDV_USER_Lst.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)

        '        Using ValfileNum As New StreamReader(User_Lst)
        '            Dim Tmp_User As String = ""

        '            Do While Not ValfileNum.EndOfStream
        '                Tmp_User = ValfileNum.ReadLine
        '                If Tmp_User = System.Environment.UserName.ToString Then
        '                    IsValidUser = True
        '                    '원격 로그에 기록을 남김 ***********************************************************************************************************
        If TempLicExpire = False Then
            Try
                Using NewfileNum As StreamWriter = File.AppendText(RemoteValFolder & System.Environment.UserName)
                    NewfileNum.WriteLine("##")
                    NewfileNum.WriteLine("## MDV Program Starting : " & Date.Now.ToString & " ##")
                    NewfileNum.Close()
                End Using
            Catch ex As Exception
                'User_Lst.Close()
                'End
            End Try
        End If

        '                    '원격 로그에 기록을 남김 ***********************************************************************************************************
        '                    User_Lst.Close()
        '                    Exit Do
        '                ElseIf Tmp_User = System.Environment.UserName.ToString & "-Full" Then
        '                    IsValidUser = True
        '                    '원격 로그에 기록을 남김 ***********************************************************************************************************
        '                    Try
        '                        Using NewfileNum As StreamWriter = File.AppendText(RemoteValFolder & System.Environment.UserName)
        '                            NewfileNum.WriteLine("##")
        '                            NewfileNum.WriteLine("## Full Function Enabled ##")
        '                            NewfileNum.WriteLine("## MDV Program Starting : " & Date.Now.ToString & " ##")
        '                            NewfileNum.Close()
        '                        End Using
        '                        Me.기타기능EToolStripMenuItem.Enabled = True
        '                        Me.기타기능EToolStripMenuItem.Visible = True
        '                    Catch ex As Exception
        '                        User_Lst.Close()
        '                        End
        '                    End Try
        '                    '원격 로그에 기록을 남김 ***********************************************************************************************************
        '                    User_Lst.Close()
        '                    Exit Do
        '                End If
        '            Loop

        '        End Using

        '        If IsValidUser = False Then
        '            Application.DoEvents()
        '            Try
        '                Using NewfileNum As StreamWriter = File.AppendText(RemoteValFolder & "UnIdentifiedUser.txt")
        '                    NewfileNum.WriteLine("##")
        '                    NewfileNum.WriteLine("## Attempt to Start Program [Madymo Data Viewer] ##")
        '                    NewfileNum.WriteLine("## Excuted Date      : " & Date.Now.ToString)
        '                    NewfileNum.WriteLine("## User Name         : " & Environment.UserName)
        '                    NewfileNum.WriteLine("## User Interative   : " & Environment.UserInteractive.ToString)
        '                    NewfileNum.WriteLine("## Machine Name      : " & Environment.MachineName)
        '                    NewfileNum.Close()
        '                End Using
        '            Catch ex As Exception

        '            End Try

        '            Threading.Thread.Sleep(600)
        '            User_Lst.Close()
        '            End
        '        End If

        '    Else
        '        Application.DoEvents()
        '        Threading.Thread.Sleep(600)
        '        End
        '    End If

        'End If

        Me.Show()

        ParaReading() '- 해석의 파라미터
        ParaReading_THOR() '- 해석의 파라미터 [THOR]
        ReadingTitles() '- 그래프 타이틀
        ReadingTitles_THOR() '- 그래프 타이틀 [THOR]
        ReadingTESTPara() '- 시험 파라미터
        ReadingTESTPara_THOR() '- 시험 파라미터 [THOR]
        ReadingGeneral_Set() ' - 일반 SetUp항목 읽어오기


    End Sub

    Private Sub 해석데이터ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 해석데이터ToolStripMenuItem.Click
        Dim SetUpfrm As New ProfileSetting
        Me.ProgressBarMain.Value = 0
        'Me.ProgressBarMain.Maximum = 1000
        SetUpfrm.Show()
    End Sub

    Private Sub 종료EToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 종료EToolStripMenuItem.Click
        Try
            Using NewfileNum As StreamWriter = File.AppendText(RemoteValFolder & System.Environment.UserName)
                NewfileNum.WriteLine("##")
                NewfileNum.WriteLine("## MDV Program Ending Normally : " & Date.Now.ToString & " ##")
                NewfileNum.Close()

                If Connected = True Then mySession.Close()

                If Directory.Exists(Application.StartupPath & "\TempResults") Then
                    Directory.Delete(Application.StartupPath & "\TempResults", True)
                End If

            End Using
        Catch ex As Exception
            'End
        End Try
        End
    End Sub

    Private Sub ToolBtnNCAP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolBtnNCAP.Click

        Dim TmpInjuryCorrel(28) As Single
        Dim TmpGraphCorrel(31) As Single
        Dim IsCorrected As Boolean = False
        Dim GraphBasedCorrel As Boolean = False

        If ((Control.ModifierKeys And Keys.Control) = Keys.Control) Then '컨트롤 키를 누른채로 클릭
            'MsgBox("Ctrl키를 눌렀넹~")

            If ((Control.ModifierKeys And Keys.Alt) = Keys.Alt) Then IsRemote = True

            IsCorrected = True
            With Me.CorrelOpenFile
                .Title = "Select Correlation Factor File"
                .FileName = ""                   '초기에 표시되는 파일 이름
                .InitialDirectory = Application.StartupPath & "\Profile"  '초기에 표시되는 폴더 위치 (예약어)
                .Filter = "Correl 파일|*.correl|모든 파일|*.*"
                .ShowDialog()
            End With

            If Me.CorrelOpenFile.FileNames(0) = "" Then
                Me.ProgressBarMain.Value = 0
                Me.Statuslbl.Text = "Cancel"
                Exit Sub
            End If

            CorrelFactorReading(Me.CorrelOpenFile.FileName, TmpInjuryCorrel, TmpGraphCorrel)
            Me.Statuslbl.Text = "Correlation Factor를 적용합니다."

        ElseIf ((Control.ModifierKeys And Keys.Shift) = Keys.Shift) Then

            If ((Control.ModifierKeys And Keys.Alt) = Keys.Alt) Then IsRemote = True

            Try

                'Graph Based Injury            '시프트 키를 누른채로 클릭
                'MsgBox("Shift키를 눌렀넹~")
                IsCorrected = True
                With Me.CorrelOpenFile
                    .Title = "Select Correlation Factor File"
                    .FileName = ""                   '초기에 표시되는 파일 이름
                    .InitialDirectory = Application.StartupPath & "\Profile"  '초기에 표시되는 폴더 위치 (예약어)
                    .Filter = "Correl 파일|*.correl|모든 파일|*.*"
                    .ShowDialog()
                End With

                If Me.CorrelOpenFile.FileNames(0) = "" Then
                    Me.ProgressBarMain.Value = 0
                    Me.Statuslbl.Text = "Cancel"
                    Exit Sub
                End If

                '일단 읽어오고 그래프를 수정
                CorrelFactorReading(Me.CorrelOpenFile.FileName, TmpInjuryCorrel, TmpGraphCorrel)
                Dim k As Integer

                Me.Statuslbl.Text = "Injury Graph-Based Correlation Factor를 적용합니다."
                Dim InjuryFile As StreamReader
                InjuryFile = New StreamReader(Me.CorrelOpenFile.FileName)
                Dim Tmp_Str() As String

                For j = 1 To 29 Step 1
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    TmpInjuryCorrel(j - 1) = 1.0
                Next

                For k = 1 To 32 Step 1
                    Tmp_Str = InjuryFile.ReadLine.Split("\")
                    TmpGraphCorrel(k - 1) = CDbl(Tmp_Str(1))
                Next

                InjuryFile.Close()

                GraphBasedCorrel = True


            Catch ex As Exception

                If ((Control.ModifierKeys And Keys.Alt) = Keys.Alt) Then IsRemote = True

                '아무것도 아닌 경우
                For i = 1 To 29 Step 1
                    TmpInjuryCorrel(i - 1) = 1.0
                Next

                For i = 1 To 32 Step 1
                    TmpGraphCorrel(i - 1) = 1.0
                Next
            End Try
        Else

            If ((Control.ModifierKeys And Keys.Alt) = Keys.Alt) Then IsRemote = True

            '아무것도 아닌 경우
            For i = 1 To 29 Step 1
                TmpInjuryCorrel(i - 1) = 1.0
            Next

            For i = 1 To 32 Step 1
                TmpGraphCorrel(i - 1) = 1.0
            Next
        End If

        Dim InjuryDP As New InjuryDisplayFrm(TmpInjuryCorrel, TmpGraphCorrel, GraphBasedCorrel)
        Me.Statuslbl.Text = "Reading Injury Value from PEAK Files / Loading DATA Location Parameters.."
        InjuryDP.Show()
        If IsCorrected = False Then
            InjuryDP.Text = "Injury Summary"
        Else
            Dim TmpProfileName() As String
            TmpProfileName = FileNameGet(Me.CorrelOpenFile.SafeFileNames)
            InjuryDP.Text = "Injury Summary" & " - " & "Correlation Profile : " & TmpProfileName(LBound(TmpProfileName))
        End If

    End Sub

    Private Sub USNCAPToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles USNCAPToolStripMenuItem.Click
        Call ToolBtnNCAP_Click(ToolBtnNCAP, New KeyEventArgs(Keys.Return))
    End Sub

    Private Sub 불러오기ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 불러오기ToolStripMenuItem.Click
        Dim TESTfrm As New FrmTEST_DATA
        TESTfrm.Owner = Me
        Try
            TESTfrm.Show()
            '================================================================================================
            If TempLicExpire = False Then
                Using NewfileNum As StreamWriter = File.AppendText(RemoteValFolder & System.Environment.UserName)
                    NewfileNum.WriteLine("##")
                    NewfileNum.WriteLine("## TEST DATA Reading Opened")
                    NewfileNum.Close()
                End Using
            End If
            '================================================================================================
        Catch ex As ObjectDisposedException
            Me.Statuslbl.Text = "Canceled"
        End Try
    End Sub

    Private Sub MADYMOPSMFileConverterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MADYMOPSMFileConverterToolStripMenuItem.Click

        If TempLicExpire = False Then
            '원격 로그
            Try
                Using NewfileNum As StreamWriter = File.AppendText(RemoteValFolder & System.Environment.UserName)
                    NewfileNum.WriteLine("## PSM Converter Run : " & Date.Now.ToString & " ##")
                    NewfileNum.Close()
                End Using
            Catch ex As Exception
                'End
            End Try
        End If

        Dim PSMfrm As New FrmPSMexport
        FrmPSMexport.Show()
        FrmPSMexport.Owner = Me
    End Sub

    Private Sub ToolBtnTEST_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolBtnTEST.Click
        Call 불러오기ToolStripMenuItem_Click(불러오기ToolStripMenuItem, New KeyEventArgs(Keys.Return))
    End Sub

    'Private Sub FileFormatConverterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FileFormatConverterToolStripMenuItem.Click
    '    Dim PROC As Integer
    '    PROC = Shell(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "Converter.exe", AppWinStyle.NormalFocus)
    'End Sub

    Private Sub BatchRun설치ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BatchRun설치ToolStripMenuItem.Click

        If TempLicExpire = False Then
            '원격 로그
            Try
                Using NewfileNum As StreamWriter = File.AppendText(RemoteValFolder & System.Environment.UserName)
                    NewfileNum.WriteLine("## Batch Run Started : " & Date.Now.ToString & " ##")
                    NewfileNum.Close()
                End Using
            Catch ex As Exception
                'End
            End Try
        End If

        Dim PROC As Integer
        'Dim i As Integer
        'Dim tmp_path As String
        Try
            PROC = Shell(Application.StartupPath & "\BatchRun.exe", AppWinStyle.NormalFocus)
        Catch ex As Exception
            Me.Statuslbl.Text = ex.Message
        End Try

        Me.Statuslbl.Text = "MADYMO Batch Job 실행. 독립 프로세스로 실행됩니다."

        'If File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\장성훈\MADYMO Data Viewer\BatchRun.exe") Then
        '    PROC = Shell(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\장성훈\MADYMO Data Viewer\BatchRun.exe", AppWinStyle.NormalFocus)
        'Else
        '    tmp_path = FilePathGet2(Application.ExecutablePath)
        '    PROC = Shell(tmp_path & "Batch_Install_131229.exe", AppWinStyle.NormalFocus)
        '    'PROC = Shell("madymo74" & " " & "C:\Users\6002317\Desktop\01_VB_Projects_CSH\TEST_MADYMO\00_YP_DRV_C9ST_L4_MobisBag_USNCAP_VH25_LL40_YP20.xml", AppWinStyle.NormalFocus)
        '    'Dim GP As System.Diagnostics.Process
        '    'MsgBox(GP)
        'End If

    End Sub

    Private Sub MainMDI_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        ''If System.Environment.UserDomainName = "HKMC" Or System.Environment.UserDomainName = "HOME-PC" Or System.Environment.UserDomainName = "성훈미경-PC" Then

        ''Else
        ''    File.Delete(Application.StartupPath & "\DATA\MADYMO_FE_Materials.xml")
        ''    ''종료 경고문구 MsgBox("사용하실 수 없습니다.", MsgBoxStyle.Critical, "Program Terminate")
        ''    Me.Close()
        ''End If
    End Sub

    Private Sub 정보IToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 정보IToolStripMenuItem.Click
        AboutProgram.Show()
    End Sub

    Private Sub LSDYNAToMADYMOToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LSDYNAToMADYMOToolStripMenuItem.Click
        Dim InstFrmFE As New FrmFEconverting
        InstFrmFE.Show()
    End Sub

    Private Sub LSPREPOSTOuputToMADYMONotYetToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LSPREPOSTOuputToMADYMONotYetToolStripMenuItem.Click
        Dim InstFrmFE As New FrmFEconverting_LSPRE
        InstFrmFE.Show()
    End Sub

    Private Sub RunXMADgic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RunXMADgic.Click
        Dim PROC, i As Integer
        Dim ProgramFolder64 As String
        Dim TmpPath() As String
        Dim TmpFile As String = ""

        Me.Statuslbl.Text = "Select XML Files...."

        Dim NewPathFile As StreamReader
        NewPathFile = New StreamReader(Application.StartupPath & "\DATA\EnvironPATH.dat")

        TmpPath = NewPathFile.ReadLine().Split("=")

        ProgramFolder64 = TmpPath(UBound(TmpPath))

        NewPathFile.Close()

        '파일명 불러들임

        With Me.XMADgicOpn
            .Title = "Select XML File(s)"
            .Multiselect = True
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            .Filter = "XML 파일|*.xml|모든 파일|*.*"
            .ShowDialog()
        End With

        If Me.XMADgicOpn.FileNames(0) = "" Then Exit Sub

        For i = 0 To UBound(Me.XMADgicOpn.FileNames)
            TmpFile = TmpFile & " " & Me.XMADgicOpn.FileNames(i)
        Next

        Try
            'PROC = Shell(Application.StartupPath & "\XMADgic.exe", AppWinStyle.NormalFocus)
            PROC = Shell(ProgramFolder64 & "\em64t-win\bin\XMADgic.exe" & " " & TmpFile, AppWinStyle.NormalFocus)
        Catch ex As Exception
            Me.Statuslbl.Text = "XMADgic " & ex.Message & " 설정 메뉴에서 Workspace Path를 설정하세요."
        End Try
    End Sub

    Private Sub RunHyperView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RunHyperView.Click
        Dim PROC, i As Integer
        Dim ProgramFolder64 As String
        Dim TmpPath() As String
        Dim TmpFile As String = ""

        Me.Statuslbl.Text = "Select kn3 Files...."

        Dim NewPathFile As StreamReader
        NewPathFile = New StreamReader(Application.StartupPath & "\DATA\EnvironPATH_HW.dat")

        TmpPath = NewPathFile.ReadLine().Split("=")

        ProgramFolder64 = TmpPath(UBound(TmpPath)) & "\hw.exe"
        'ProgramFolder64 = "C:\Program Files\Altair\12.0\hw\bin\win64\hw.exe"

        NewPathFile.Close()

        '파일명 불러들임


        With Me.XMADgicOpn
            .Title = "Select kn3 File(s)"
            .Multiselect = False
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            .Filter = "kn3 파일|*.kn3|모든 파일|*.*"
            .ShowDialog()
        End With

        If Me.XMADgicOpn.FileNames(0) = "" Then Exit Sub

        For i = 0 To UBound(Me.XMADgicOpn.FileNames)
            TmpFile = TmpFile & " " & Me.XMADgicOpn.FileNames(i)
        Next

        Try
            'PROC = Shell(ProgramFolder64 & "\em64t-win\bin\XMADgic.exe" & " " & TmpFile, AppWinStyle.NormalFocus)
            PROC = Shell(ProgramFolder64 & " " & TmpFile, AppWinStyle.NormalFocus)
        Catch ex As Exception
            Me.Statuslbl.Text = "HyperView " & ex.Message & " 설정 메뉴에서 HyperView Path를 설정하세요."
        End Try
    End Sub

    Private Sub NodeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NodeToolStripMenuItem.Click

        Dim NodeSETfile As String = ""
        Dim OriginalPSMfile As String = ""
        Dim NodeList() As Integer

        '*SET_NODE_LIST 파일을 불러온다.
        With Me.NodeExcludingDlg
            .Title = "Select key File [*SET_NODE_LIST]"
            .Multiselect = False
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            .Filter = "DYNA 파일|*.key;*.k;*.dyn|모든 파일|*.*"
            .ShowDialog()
        End With

        If Me.NodeExcludingDlg.FileNames(0) = "" Then
            Exit Sub
        Else
            NodeSETfile = Me.NodeExcludingDlg.FileNames(0)
            Me.NodeExcludingDlg.FileNames(0) = ""
        End If

        'NODE SET을 찾아온다.=================================
        NodeList = ReadingSETNODE(NodeSETfile)

        If IsNothing(NodeList) Then
            Me.Statuslbl.Text = "제거할 노드가 없습니다. Key파일 내에 *CONSTRAINED_NODAL_RIGID_BODY와 *SET_NODE_LIST 카드를 확인하세요."
            Exit Sub
        End If

        'PSM 파일을 불러온다.
        With Me.NodeExcludingDlg
            .Title = "Select PSM File(s)"
            .Multiselect = False
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = FilePathGet2(NodeSETfile) 'System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            .Filter = "PSM 파일|*.inc|모든 파일|*.*"
            .ShowDialog()
        End With

        If Me.NodeExcludingDlg.FileNames(0) = "" Then
            Exit Sub
        Else
            OriginalPSMfile = Me.NodeExcludingDlg.FileNames(0)
            PSM_Node_Excluding(OriginalPSMfile, NodeList)
        End If
    End Sub

    Private Sub CommandPromptWindowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CommandPromptWindowToolStripMenuItem.Click
        Dim CMDwindow As New FrmCMDprompt
        CMDwindow.Show()
    End Sub

    Private Sub SetDataProfileTHORToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetDataProfileTHORToolStripMenuItem.Click
        Dim SetUpfrm As New FrmProSettingTHOR
        Me.ProgressBarMain.Value = 0
        'Me.ProgressBarMain.Maximum = 1000
        SetUpfrm.Show()
    End Sub

    Private Sub THORRMDBToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles THORRMDBToolStripMenuItem.Click

        Dim TmpInjuryCorrel(29) As Single
        Dim TmpGraphCorrel(47) As Single
        Dim IsCorrected As Boolean = False

        If ((Control.ModifierKeys And Keys.Control) = Keys.Control) Then '컨트롤 키를 누른채로 클릭
            'MsgBox("Ctrl키를 눌렀넹~")

            If ((Control.ModifierKeys And Keys.Alt) = Keys.Alt) Then IsRemote = True

            IsCorrected = True
            With Me.CorrelOpenFile
                .Title = "Select Correlation Factor File for THOR"
                .FileName = ""                   '초기에 표시되는 파일 이름
                .InitialDirectory = Application.StartupPath & "\Profile"  '초기에 표시되는 폴더 위치 (예약어)
                .Filter = "Correl 파일|*.correl_THOR|모든 파일|*.*"
                .ShowDialog()
            End With

            If Me.CorrelOpenFile.FileNames(0) = "" Then
                Me.ProgressBarMain.Value = 0
                Me.Statuslbl.Text = "Cancel"
                Exit Sub
            End If

            CorrelFactorReading_THOR(Me.CorrelOpenFile.FileName, TmpInjuryCorrel, TmpGraphCorrel)
            Me.Statuslbl.Text = "[THOR ATD] Correlation Factor를 적용합니다."
        Else

            If ((Control.ModifierKeys And Keys.Alt) = Keys.Alt) Then IsRemote = True

            For i = 1 To 30 Step 1
                TmpInjuryCorrel(i - 1) = 1.0
            Next

            For i = 1 To 48 Step 1
                TmpGraphCorrel(i - 1) = 1.0
            Next
        End If

        Dim InjuryDP As New InjuryDisplayFrm_THOR(TmpInjuryCorrel, TmpGraphCorrel)
        Me.Statuslbl.Text = "Reading Injury Value from PEAK Files / Loading DATA Location Parameters.."
        InjuryDP.Show()
        If IsCorrected = False Then
            InjuryDP.Text = "Injury Summary"
        Else
            Dim TmpProfileName() As String
            TmpProfileName = FileNameGet(Me.CorrelOpenFile.SafeFileNames)
            InjuryDP.Text = "Injury Summary" & " - " & "Correlation Profile : " & TmpProfileName(LBound(TmpProfileName))
        End If
    End Sub

    Private Sub FileCommentBrowserToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FileCommentBrowserToolStripMenuItem.Click
        Dim BrowseFile As New FrmFileBrowser
        BrowseFile.Show()
    End Sub

    Private Sub ToolBtnTHOR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolBtnTHOR.Click
        Call THORRMDBToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub OpenTHORTToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenTHORTToolStripMenuItem.Click
        Dim TESTfrm As New FrmTHORTEST_DATA
        TESTfrm.Owner = Me
        Try
            TESTfrm.Show()
            '================================================================================================
            If TempLicExpire = False Then
                Using NewfileNum As StreamWriter = File.AppendText(RemoteValFolder & System.Environment.UserName)
                    NewfileNum.WriteLine("##")
                    NewfileNum.WriteLine("## TEST DATA Reading(THOR) Opened")
                    NewfileNum.Close()
                End Using
            End If
            '================================================================================================
        Catch ex As ObjectDisposedException
            Me.Statuslbl.Text = "Canceled"
        End Try
    End Sub

    Private Sub MADYMOToLSDYNAToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MADYMOToLSDYNAToolStripMenuItem.Click
        Dim InstFrmFE As New FrmFEconvertingInXML
        InstFrmFE.Show()
    End Sub

    Private Sub GeneralSetUpToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GeneralSetUpToolStripMenuItem.Click
        Dim GeneralSet As New FrmSetUpGeneral
        GeneralSet.Owner = Me
        GeneralSet.MdiParent = Me
        Try
            GeneralSet.Show()
        Catch ex As ObjectDisposedException
            Me.Statuslbl.Text = "Canceled"
        End Try
    End Sub

    Private Sub ToolBtnTEST_THOR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolBtnTEST_THOR.Click
        'Call OpenTHORTToolStripMenuItem_Click(OpenTHORTToolStripMenuItem, New KeyEventArgs(Keys.Return))
        Call OpenTHORTToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub RemoteConnectionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoteConnectionToolStripMenuItem.Click
        'Temp =============================================

        If Connected = True Then
            mySession.Close()
            RemoteConnectionToolStripMenuItem.ForeColor = Color.Red
            RemoteConnectionToolStripMenuItem.Text = "Remote Connection [Closed]"
            MsgBox("Stop to Connect", MsgBoxStyle.Information, "Close Connection")
            Me.Statuslbl.Text = "Connection Closed."
            Me.RemoteConnectLbl.Text = "Connection Closed"
            Me.RemoteConnectLbl.ForeColor = Color.Red
            Connected = False
            Exit Sub
        End If

        Dim LogIn As New FrmLogIn
        LogIn.ShowDialog(Me)

        Me.Statuslbl.Text = "Now, Connecting Server..."
        Me.RemoteConnectLbl.Text = "Now, Connecting Server..."
        Me.RemoteConnectLbl.ForeColor = Color.Orange

        For i = 0 To 3
            Try
                If SessionInfor(i) = "" Then
                    Exit Sub
                End If
            Catch ex As Exception
                Exit Sub
            End Try
        Next

        Try
            With mySessionOptions
                .Protocol = Protocol.Ftp
                .HostName = SessionInfor(0) '"10.98.40.102"
                .UserName = SessionInfor(1) '"6002317"
                .Password = SessionInfor(2) '"csh0609"
                'If SessionInfor(3) = "Any SSH Key" Then
                '.GiveUpSecurityAndAcceptAnySshHostKey = True
                'Else
                '    .SshHostKeyFingerprint = SessionInfor(3) '"ssh-rsa 2048 8e:75:db:b1:47:05:00:9e:81:35:56:99:13:b3:3e:3a"
                '.SshHostKeyFingerprint = "ssh-rsa 2048 8e:75:db:b1:47:05:00:9e:81:35:56:99:13:b3:3e:3a"
                'End If
                .PortNumber = 21
            End With

            mySession.DisableVersionCheck = True
            mySession.Open(mySessionOptions)
            Connected = True
            RemoteConnectionToolStripMenuItem.ForeColor = Color.Blue
            RemoteConnectionToolStripMenuItem.Text = SessionInfor(1) & "@" & SessionInfor(0)

            Me.Statuslbl.Text = "Connected to " & SessionInfor(0)
            Me.RemoteConnectLbl.Text = SessionInfor(1) & "@" & SessionInfor(0)
            Me.RemoteConnectLbl.ForeColor = Color.DarkGreen
            Application.DoEvents()

        Catch ex As Exception
            Connected = False
            RemoteConnectionToolStripMenuItem.ForeColor = Color.Red
            Me.RemoteConnectLbl.Text = "Failed to Connect"
            Me.RemoteConnectLbl.ForeColor = Color.Red
            MsgBox("Failed to Connect")
        End Try
        'Temp =============================================
    End Sub

    Private Sub 시작ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 시작ToolStripMenuItem.Click

    End Sub
End Class
