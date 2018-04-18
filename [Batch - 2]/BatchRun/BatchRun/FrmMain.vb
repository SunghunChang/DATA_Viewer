Imports System.IO
Imports System
Imports System.Diagnostics
Imports System.ComponentModel
Imports System.Drawing
Imports System.Threading
Imports ExpireClass

Public Class FrmMain

    Dim CheckExpire As New ExpireClass.ExpireCondition

    Dim MADYMO_Ver() As String
    Dim EachCntStart As Boolean = False
    Dim Run_Cnt As Integer
    Dim StatusLineWrite As Boolean = False

    Delegate Sub RunProcess()

    Public RunMADYMO_Job As New RunProcess(AddressOf RunMADYMO)

    Private myThread As Thread

    Private WithEvents MyProcess As Process

    Private Sub CmdOpn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdOpn.Click

        Dim i As Integer
        Dim WorkingDirectory As String
        Dim tmp_txt As String

        With Me.OpenDlg
            .Title = "MADYMO INPUT FILE SELECTION"
            .Multiselect = True
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "MADYMO Input 파일|*.xml|모든 파일|*.*"
            .ShowDialog()
        End With
        'Me.OpenDlg.FileNames : 경로 + 파일명 + 확장자
        'Me.OpenDlg.SafeFileNames : 파일명 + 확장자
        'Filepathget(Me.OpenDlg.FileNames) : 경로명 (\포함)
        'UBound(Me.OpenDlg.FileNames) + 1 : 총 파일 수

        If Me.OpenDlg.FileName = "" Then Exit Sub

        Me.BtnRun.Enabled = True

        Dim FormerLength As Integer

        If Me.MainFileGrid.Rows = 1 Then
            '초기 상태 (최초 선택)
            ReDim MADYMO_Ver(UBound(Me.OpenDlg.FileNames))
        Else

            FormerLength = MADYMO_Ver.Length
            ReDim Preserve MADYMO_Ver(UBound(MADYMO_Ver) + UBound(Me.OpenDlg.FileNames) + 1)
        End If


        'Version Verify======================================================================================================================
        For i = 0 To UBound(Me.OpenDlg.FileNames)

            Dim ReadFiles As New FileStream(Me.OpenDlg.FileNames(i), FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            '맨 마지막 상수를 FileShare.ReadWrite 로 지정해야 해석 도중 접근이 가능함

            Using fileNum As New StreamReader(ReadFiles)
                Do While Not fileNum.EndOfStream
                    tmp_txt = fileNum.ReadLine
                    If InStr(tmp_txt, "RELEASE=""R7.4""", vbTextCompare) >= 1 Then
                        MADYMO_Ver(FormerLength + i) = "madymo74.exe"
                        Exit Do
                    ElseIf InStr(tmp_txt, "RELEASE=""R7.4.2""", vbTextCompare) >= 1 Then
                        MADYMO_Ver(FormerLength + i) = "madymo742.exe"
                        Exit Do
                    ElseIf InStr(tmp_txt, "RELEASE=""R7.5""", vbTextCompare) >= 1 Then
                        MADYMO_Ver(FormerLength + i) = "madymo75.exe"
                        Exit Do
                    ElseIf InStr(tmp_txt, "RELEASE=""R7.6""", vbTextCompare) >= 1 Then
                        MADYMO_Ver(FormerLength + i) = "madymo76.exe"
                        Exit Do
                    ElseIf InStr(tmp_txt, "RELEASE=""R7.7""", vbTextCompare) >= 1 Then
                        MADYMO_Ver(FormerLength + i) = "madymo77.exe"
                        Exit Do
                    End If
                Loop
            End Using

            ReadFiles.Close()
        Next
        '======================================================================================================================================

        WorkingDirectory = Mid(FilePathGet(Me.OpenDlg.FileNames), 1, FilePathGet(Me.OpenDlg.FileNames).Length - 1)

        '======================================================================================================================================
        'Draw Grid
        With Me.MainFileGrid
            .Rows = .Rows + Me.OpenDlg.FileNames.Length
            For i = 0 To UBound(Me.OpenDlg.FileNames)
                .set_TextMatrix(FormerLength + i + 1, 0, i + 1)
                .set_TextMatrix(FormerLength + i + 1, 1, WorkingDirectory)
                Select Case MADYMO_Ver(FormerLength + i)
                    Case "madymo74.exe"
                        .set_TextMatrix(FormerLength + i + 1, 2, "R7.4")
                    Case "madymo742.exe"
                        .set_TextMatrix(FormerLength + i + 1, 2, "R7.4.2")
                    Case "madymo75.exe"
                        .set_TextMatrix(FormerLength + i + 1, 2, "R7.5")
                    Case "madymo76.exe"
                        .set_TextMatrix(FormerLength + i + 1, 2, "R7.6")
                    Case "madymo77.exe"
                        .set_TextMatrix(FormerLength + i + 1, 2, "R7.7")
                    Case Else
                        .set_TextMatrix(FormerLength + i + 1, 2, "None")
                End Select
                .set_TextMatrix(FormerLength + i + 1, 3, Me.OpenDlg.SafeFileNames(i))
            Next
        End With
        Application.DoEvents()
        '======================================================================================================================================

    End Sub

    Public Sub RunMADYMO()
        'Dim Executable As String = "madymo75.exe"
        'Dim CommandLine As String = "-3d C:\Users\6002317\Desktop\01_VB_Projects_CSH\TEST_MADYMO\00_YP_DRV_C9ST_L4_MobisBag_USNCAP_VH25_LL40_YP20.xml"

        Dim i As Integer

        With Me
            .Text = "MADYMO Batch Job Run [Now Running....]"

            .ProgressBarTot.Minimum = 0
            .ProgressBarTot.Maximum = (Me.MainFileGrid.Rows - 1) * 10 + 1

            .BtnIni.Enabled = False
            .BtnRun.Enabled = False
            .CmdOpn.Enabled = False
        End With

        For i = 0 To Me.MainFileGrid.Rows - 2
            Me.TxtStatus.Text = Me.TxtStatus.Text & vbCrLf & vbCrLf & "=========================================================================================================================" & vbCrLf
            Me.TxtStatus.Text = Me.TxtStatus.Text & "========================================================================================================================="
            Me.TxtStatus.Text = Me.TxtStatus.Text & vbCrLf & "===================================================      JOB # " & i + 1 & " START      ===================================================" & vbCrLf
            Me.TxtStatus.Text = Me.TxtStatus.Text & "=========================================================================================================================" & vbCrLf
            Me.TxtStatus.Text = Me.TxtStatus.Text & "========================================================================================================================="
            Me.TxtStatus.Text = Me.TxtStatus.Text & vbCrLf & vbCrLf & Now() & vbCrLf & vbCrLf
            Me.TxtStatus.Text = Me.TxtStatus.Text & "MADYMO BatchJob by C.S.H" & vbCrLf
            Me.TxtStatus.Text = Me.TxtStatus.Text & "Start working. " & vbCrLf
            Me.TxtStatus.Text = Me.TxtStatus.Text & "During this procedure, you will not be able to use the control." & vbCrLf
            Me.TxtStatus.Text = Me.TxtStatus.Text & "Abnormal program termination, may cause unexpected errors," & vbCrLf
            Me.TxtStatus.Text = Me.TxtStatus.Text & "Developers do not assume responsibility for these errors." & vbCrLf & vbCrLf
            Me.TxtStatus.Text = Me.TxtStatus.Text & "Now, Preparing Phase" & vbCrLf & vbCrLf

            Me.TxtStatus.Select(Me.TxtStatus.TextLength, 0)
            Me.TxtStatus.ScrollToCaret()
            If Me.MainFileGrid.get_TextMatrix(i + 1, 2) = "None" Then
                With Me.MainFileGrid
                    .Row = i + 1
                    .Col = 2
                    .ColSel = 3
                    .RowSel = i + 1
                    .CellForeColor = Color.Red
                End With
            Else
                Run_Cnt = i
                ActualJob(Run_Cnt + 1, Me.MainFileGrid.get_TextMatrix(Run_Cnt + 1, 1), MADYMO_Ver(Run_Cnt), "-3d " & Me.MainFileGrid.get_TextMatrix(Run_Cnt + 1, 3))
                Me.ProgressBarTot.Value = Me.ProgressBarTot.Value + 1
            End If
            Me.ProgressBarTot.Value = (i + 1) * 10
            Application.DoEvents()
        Next


        With Me

            .ProgressBarTot.Value = .ProgressBarTot.Maximum

            .Text = "MADYMO Batch Job Run"

            .BtnIni.Enabled = True
            .BtnRun.Enabled = False
            .CmdOpn.Enabled = True
        End With

        '=========================================================================================

    End Sub

    Private Sub ActualJob(ByVal JobID As Integer, ByVal WorkingDir As String, ByVal Executable As String, ByVal CommandLine As String)


        Dim MyStartInfo As New Diagnostics.ProcessStartInfo(Executable, CommandLine)
        MyStartInfo.WorkingDirectory = WorkingDir ' 작업 디렉토리를 현재 디랙토리로 변경함 ************************************************ 중요 !!!! *********************
        MyStartInfo.UseShellExecute = False                            ' CMD.EXE 등을 사용하지 않음, 직접실행
        MyStartInfo.RedirectStandardOutput = True                      ' 프로그램 출력(STDOUT)을 Redirect 함
        MyStartInfo.RedirectStandardInput = False                      ' STDIN 은 Redirect 하지 않음
        MyStartInfo.CreateNoWindow = True                              ' 프로그램 실행 윈도우즈를 만들지 않음

        Dim MyProcess As New Diagnostics.Process
        MyProcess.StartInfo = MyStartInfo
        MyProcess.Start()                                             ' 프로세스를 실행함
        'MyProcess.o()

        Dim STDOUT As New StreamReader(MyProcess.StandardOutput.BaseStream)

        Dim dummy As String
        Dim dummy_2 As String = ""

        While Not (MyProcess.HasExited)

            On Error Resume Next

            dummy = STDOUT.ReadLine                             ' 프로세스의 출력된 값에서, 라인 한개 읽기
            ' ''    '
            '  읽어 들인 라인으로 다른 작업.
            '
            'If InStr(dummy, "Storage allocation:") > 1 Then
            '    StatusLineWrite = True
            'End If

            If (dummy <> dummy_2) Then 'And StatusLineWrite Then
                If InStr(dummy, "MADYMO TERMINATED NORMALLY", CompareMethod.Text) Then
                    With Me.MainFileGrid
                        .Row = JobID
                        .Col = 2
                        .ColSel = 3
                        .RowSel = JobID
                        .CellForeColor = Color.Blue
                    End With
                    Me.ProgressBarTot.Value = Me.ProgressBarTot.Value + 1
                ElseIf InStr(dummy, "MADYMO TERMINATED ABNORMALLY", CompareMethod.Text) Then
                    With Me.MainFileGrid
                        .Row = JobID
                        .Col = 2
                        .ColSel = 3
                        .RowSel = JobID
                        .CellForeColor = Color.Red
                    End With
                    'ElseIf Mid(dummy, 1, 7) = "      %" Then
                    '    EachCntStart = True
                    '    Me.ProgressBarTot.Value = Me.ProgressBarTot.Value + 1
                End If
                'Select Case EachCntStart
                '    Case True
                '        If IsNumeric(Mid(dummy, 1, 7)) = True Then
                '            Me.ProgressBarEach.Value = CDbl(Mid(dummy, 1, 7))
                '            Me.ProgressBarTot.Value = Me.ProgressBarTot.Value + 1
                '        End If
                'End Select
                Me.TxtStatus.AppendText(vbCrLf & dummy)
                Me.TxtStatus.Select(Me.TxtStatus.TextLength, 0)
                Me.TxtStatus.ScrollToCaret()
                dummy = dummy_2
            End If
            Application.DoEvents()                                                             ' 다른 프로세스에 영향을 주지 않도록...

        End While

        Me.TxtStatus.Select(Me.TxtStatus.TextLength, 0)
        Me.TxtStatus.ScrollToCaret()

        EachCntStart = False
        StatusLineWrite = False

    End Sub


    Private Sub FrmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Select Case System.Environment.UserName
            Case "6002317"
            Case Else
                If DateDiff(DateInterval.Day, CheckExpire.CurrentDate, CheckExpire.IsExpire) < 0 Then
                    'MsgBox("Available period has passed." & vbCrLf & "Please contact the developer :)", MsgBoxStyle.Critical, "Program Terminated")
                    MsgBox("프로그램을 종료합니다.", MsgBoxStyle.Critical, "Program Terminated")
                    End
                End If
        End Select


        Select Case System.Environment.UserName
            Case "장성훈", "6002317", "5805793", "6304537", "9562745", "6005885", "5405228", "5502875", "6333467", "5504584", "9163467", "5300641", "6427553"
                '장성훈 : 6002317 / 양현모 : 5805793 / 박홍익 : 6304537
                '최성철 : 9562745
                '김시열 : 5405228
            Case Else
                If System.Net.Dns.GetHostName() = "HMC-NAM-W012050" Then
                Else
                    MsgBox("Program End" & vbCrLf & "Unauthenticated User", , "Running Termination")
                    End
                End If
        End Select

        Select Case System.Environment.UserDomainName
            Case "HKMC", "HOME-PC"
            Case Else
                MsgBox("Program End" & vbCrLf & "Unauthenticated Company", , "Running Termination")
                End
        End Select

        '프로세스 확인======================================================================================
        'Dim MADYMO_Data_Viewer() As Process = Process.GetProcessesByName("PostProcessing")
        'Dim Identifier As Integer = 0
        'For Each Process As Process In MADYMO_Data_Viewer
        '    Identifier += 1
        'Next

        'If Identifier = 0 Then
        '    MsgBox("Run with MADYMO Data Viewer", MsgBoxStyle.Critical, "Error")
        '    End
        'End If
        '==================================================================================================

        With Me

            .VerTooltip.SetToolTip(.BtnRun, "◆ Usable MADYMO Ver. ◆" & vbCrLf & _
                                                      "    R7.4" & vbCrLf & _
                                                      "    R7.4.2" & vbCrLf & _
                                                      "    R7.5")
            .VerTooltip.SetToolTip(.BtnIni, "Initailize")
            .VerTooltip.SetToolTip(.CmdOpn, "Select XML File (MADYMO Input)")



            ReDim MADYMO_Ver(0)

            With .MainFileGrid

                .WordWrap = True
                .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillRepeat
                .MergeCells = 1

                .set_ColWidth(0, 500)
                .set_ColWidth(1, 4900)
                .set_ColWidth(2, 750)
                .set_ColWidth(3, 6250)
                .set_ColAlignment(0, 4)
                .set_ColAlignment(1, 0)
                .set_ColAlignment(2, 4)
                .set_ColAlignment(3, 0)
                .set_MergeCol(1, True)
                .set_MergeCol(2, True)
                .set_MergeCol(3, True)

                .set_TextMatrix(0, 0, "Case #")
                .set_TextMatrix(0, 1, "Working Path")
                .set_TextMatrix(0, 2, "Ver.")
                .set_TextMatrix(0, 3, "MADYMO Input File(s) - XML Format")
            End With

        End With
    End Sub

    Private Sub BtnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRun.Click

        If Me.MainFileGrid.Rows = 1 Then Exit Sub

        '스래드 생성
        myThread = New Thread(New ThreadStart(AddressOf ThreadFunction))
        myThread.Start()
        
    End Sub

    Private Sub ThreadFunction()
        Dim myThreadClassObj As New MyThreadClass(Me)
        myThreadClassObj.Run()
    End Sub

    Private Sub BtnIni_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnIni.Click
        ReDim MADYMO_Ver(0)
        Me.MainFileGrid.Rows = 1
        Me.OpenDlg.Reset()
        Me.ProgressBarTot.Value = 0
        Me.TxtStatus.Text = ""
        EachCntStart = False
        StatusLineWrite = False
    End Sub

End Class

Public Class MyThreadClass

    Private myFormControl1 As FrmMain

    Public Sub New(ByVal myForm As FrmMain)
        myFormControl1 = myForm
    End Sub

    Public Sub Run()
        Try
            myFormControl1.Invoke(myFormControl1.RunMADYMO_Job)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

End Class