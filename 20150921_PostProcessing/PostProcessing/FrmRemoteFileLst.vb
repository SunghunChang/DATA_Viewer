Imports WinSCP
Imports System.IO

Public Class FrmRemoteFileLst

    Dim Sorting As Boolean = True
    Public CurrentPath As String
    Public Mode As String = ""
    Public RunFiles As String() = Nothing
    Public RunFilesVer As String() = Nothing


    Public Sub New(ByVal Type As String, Optional ByVal TmpRunFiles As String() = Nothing, Optional ByVal TmpVer As String() = Nothing)

        ' 이 호출은 디자이너에 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하십시오.
        Mode = Type
        RunFiles = TmpRunFiles
        RunFilesVer = TmpVer
    End Sub

    Private Sub FrmRemoteFileLst_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Width = 1100
        Me.Height = 800

        If RemoteFolderName = "" Then
            Me.FilePathTxt.Text = mySession.HomePath.ToString
            CurrentPath = mySession.HomePath.ToString
            PopulateRemote(Me.RemoteFileLst, Me.FilePathTxt.Text)
        Else
            Me.FilePathTxt.Text = RemoteFolderName
            CurrentPath = RemoteFolderName
            PopulateRemote(Me.RemoteFileLst, RemoteFolderName)
        End If


    End Sub

    Private Sub PopulateRemote(ByRef RemoteFileLstView As Windows.Forms.ListView, ByVal path As String)

        'Populate
        Dim RemoteDir As RemoteDirectoryInfo = mySession.ListDirectory(path)
        Dim fileInfo As RemoteFileInfo

        With RemoteFileLstView
            RemoteFileLstView.Items.Clear()

            For Each fileInfo In RemoteDir.Files

                Dim NewItem As New ListViewItem

                With NewItem
                    '0
                    .Text = fileInfo.Name

                    '1
                    Dim Tmp_Str() As String = .Text.Split(".")

                    If fileInfo.IsDirectory = True Then
                        .SubItems.Add("[Directory]")
                        .ForeColor = Color.Lime
                        .Font = New Font("맑은 고딕", 10, FontStyle.Bold)
                    Else
                        .SubItems.Add(fileInfo.FileType)
                    End If

                    '2
                    .SubItems.Add(fileInfo.FilePermissions.ToString)

                    '3
                    .SubItems.Add(fileInfo.LastWriteTime.ToString)

                    '4

                    'If fileInfo.IsDirectory = False Then

                    Select Case LCase(Tmp_Str(UBound(Tmp_Str)))
                        Case "xml"
                            .SubItems.Add("[MADYMO] Extensible Markup Language")
                            .ForeColor = Color.DarkOrange
                            .Font = New Font("맑은 고딕", 10, FontStyle.Bold)
                        Case "peak"
                            .SubItems.Add("[MADYMO] Peak Injury File")
                            .ForeColor = Color.LightYellow
                            .Font = New Font("맑은 고딕", 9, FontStyle.Bold)
                        Case "k", "dyn", "key", "K", "DYN", "KEY"
                            .SubItems.Add("[LS-DYNA] Keyword")
                            .ForeColor = Color.Yellow
                            .Font = New Font("맑은 고딕", 9, FontStyle.Bold)
                        Case "ppt", "pptx"
                            .SubItems.Add("PowerPoint File")
                            .ForeColor = Color.Red
                            .Font = New Font("맑은 고딕", 9, FontStyle.Bold)
                        Case "xls", "xlsx"
                            .SubItems.Add("Excel File")
                            .ForeColor = Color.LightGreen
                            .Font = New Font("맑은 고딕", 9, FontStyle.Bold)
                        Case "doc", "docx"
                            .SubItems.Add("Word File")
                            .ForeColor = Color.SkyBlue
                            .Font = New Font("맑은 고딕", 9, FontStyle.Bold)
                        Case "pdf"
                            .SubItems.Add("Acrobat File")
                            .ForeColor = Color.Brown
                            .Font = New Font("맑은 고딕", 9, FontStyle.Bold)
                        Case "dll"
                            .SubItems.Add("Dynamic Linking Library")
                        Case "txt"
                            .SubItems.Add("Text File")
                        Case "gif"
                            .SubItems.Add("Graphic Interchange Format")
                        Case "jpg"
                            .SubItems.Add("Joint Photographic Experts Group")
                        Case "avi"
                            .SubItems.Add("Audio Video Interleaving")
                        Case "inlog"
                            .SubItems.Add("Interface Motion Editor Log")
                            .ForeColor = Color.YellowGreen
                            .Font = New Font("맑은 고딕", 10, FontStyle.Bold)
                        Case Else
                            If fileInfo.Name.ToString.StartsWith("d3plot") Then
                                .SubItems.Add("[LS-DYNA] Binary Plot File")
                                .ForeColor = Color.BurlyWood
                                .Font = New Font("맑은 고딕", 9, FontStyle.Bold)
                            ElseIf fileInfo.Name.ToString = "d3hsp" Then
                                .SubItems.Add("[LS-DYNA] Analysis Information")
                                .ForeColor = Color.Cyan
                                .Font = New Font("맑은 고딕", 9, FontStyle.Bold)
                            ElseIf fileInfo.Name.ToString.StartsWith("binout") Then
                                .SubItems.Add("[LS-DYNA] Binary Output File")
                            ElseIf fileInfo.Name.ToString.StartsWith("abstat") Then
                                .SubItems.Add("[LS-DYNA] Airbag Statistics")
                            ElseIf fileInfo.Name.ToString.StartsWith("deforc") Then
                                .SubItems.Add("[LS-DYNA] Discrete Element Force")
                            ElseIf fileInfo.Name.ToString.StartsWith("elout") Then
                                .SubItems.Add("[LS-DYNA] Element Output Data")
                            ElseIf fileInfo.Name.ToString.StartsWith("glstat") Then
                                .SubItems.Add("[LS-DYNA] Global Statistics")
                            ElseIf fileInfo.Name.ToString.StartsWith("jntforc") Then
                                .SubItems.Add("[LS-DYNA] Joint Force")
                            ElseIf fileInfo.Name.ToString.StartsWith("matsum") Then
                                .SubItems.Add("[LS-DYNA] Material Energies")
                            ElseIf fileInfo.Name.ToString.StartsWith("nodout") Then
                                .SubItems.Add("[LS-DYNA] Nodal Output")
                            ElseIf fileInfo.Name.ToString.StartsWith("rcforc") Then
                                .SubItems.Add("[LS-DYNA] Resultant Interface Force")
                            ElseIf fileInfo.Name.ToString.StartsWith("rwforc") Then
                                .SubItems.Add("[LS-DYNA] Rigidwall Force")
                            ElseIf fileInfo.Name.ToString.StartsWith("sbtout") Then
                                .SubItems.Add("[LS-DYNA] Seatbelt Output Data")
                            ElseIf fileInfo.Name.ToString.StartsWith("secforc") Then
                                .SubItems.Add("[LS-DYNA] Cross Section Forces")
                            ElseIf fileInfo.Name.ToString.StartsWith("sleout") Then
                                .SubItems.Add("[LS-DYNA] Sliding Interface Energies")
                            ElseIf fileInfo.Name.ToString.StartsWith("swforc") Then
                                .SubItems.Add("[LS-DYNA] Spotweld Nodal Constraint Reaction Forces")
                            ElseIf fileInfo.IsDirectory = True Then
                                .SubItems.Add("[Directory]")
                            Else
                                .SubItems.Add("")
                            End If

                            If LCase(.Text).StartsWith("all_mes") Or LCase(.Text).StartsWith("glstat") Or LCase(.Text).StartsWith("matsum") _
                                Or LCase(.Text).StartsWith("binout") Or LCase(.Text).StartsWith("abstat") Or LCase(.Text).StartsWith("deforc") _
                                Or LCase(.Text).StartsWith("jntforc") Or LCase(.Text).StartsWith("nodout") Or LCase(.Text).StartsWith("rcforc") _
                                Or LCase(.Text).StartsWith("secforc") Or LCase(.Text).StartsWith("sleout") Or LCase(.Text).StartsWith("elout") _
                                Or LCase(.Text).StartsWith("sbtout") Or LCase(.Text).StartsWith("swforc") Or LCase(.Text).StartsWith("rwforc") Then
                                .ForeColor = Color.Orange
                                .Font = New Font("맑은 고딕", 9, FontStyle.Bold)
                            End If
                    End Select

                    'End If

                    '5 - file size
                    Select Case fileInfo.IsDirectory
                        Case True
                            .SubItems.Add("dir")
                        Case False
                            .SubItems.Add(Format(fileInfo.Length / 1048576, "0.00" & " MB"))
                    End Select

                End With

                Select Case Mode
                    Case "Run"
                        If fileInfo.IsDirectory Then RemoteFileLstView.Items.Add(NewItem)
                    Case Else
                        RemoteFileLstView.Items.Add(NewItem)
                End Select

            Next

        End With

        RemoteFileLstView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent)
        RemoteFileLstView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent)
        RemoteFileLstView.AutoResizeColumn(2, ColumnHeaderAutoResizeStyle.ColumnContent)
        RemoteFileLstView.AutoResizeColumn(3, ColumnHeaderAutoResizeStyle.ColumnContent)
        RemoteFileLstView.AutoResizeColumn(4, ColumnHeaderAutoResizeStyle.ColumnContent)
        RemoteFileLstView.Columns(5).Width = 80

        RemoteFileLstView.ListViewItemSorter = New ListViewSortFormNamespace.ListViewItemComparer(0)

    End Sub

    Private Sub RemoteFileLst_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles RemoteFileLst.DoubleClick
        '기본적인 파일 이동
        With Me
            Dim TargetPath As String = ""
            If RemoteFileLst.SelectedItems.Count = 0 Then Exit Sub
            If RemoteFileLst.SelectedItems(0).Name.ToString = "." Then Exit Sub

            If RemoteFileLst.SelectedItems(0).SubItems(4).Text = "[Directory]" Then
                If RemoteFileLst.SelectedItems(0).Text = ".." Then
                    Dim tmp_str() As String = .FilePathTxt.Text.Split("/")
                    TargetPath = tmp_str(0)
                    For i = 1 To UBound(tmp_str) - 1
                        TargetPath = TargetPath & "/" & tmp_str(i)
                    Next
                    .FilePathTxt.Text = TargetPath
                    RemoteFileLst.Items.Clear()
                    PopulateRemote(RemoteFileLst, TargetPath)
                    CurrentPath = TargetPath
                Else
                    TargetPath = .FilePathTxt.Text & "/" & RemoteFileLst.SelectedItems(0).Text
                    RemoteFileLst.Items.Clear()
                    PopulateRemote(RemoteFileLst, TargetPath)
                    .FilePathTxt.Text = TargetPath
                    CurrentPath = TargetPath
                End If
            End If

        End With

    End Sub

    Private Sub RemoteFileLst_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles RemoteFileLst.KeyDown
        Try
            If e.KeyCode = Keys.F5 Then
                PopulateRemote(RemoteFileLst, CurrentPath)
                Me.FilePathTxt.Text = CurrentPath

            ElseIf e.KeyCode = Keys.Enter And Mode = "Run" Then

                Me.FilePathTxt.Text = CurrentPath
                RemoteFolderName = Me.FilePathTxt.Text & "/"

                myTransferOptions.TransferMode = TransferMode.Automatic

                Dim Job_Result As String = ""
                Dim Job_Cnt As Integer = 0

                For i = 0 To UBound(RunFiles)
                    If RunFilesVer(i) = "None" Then

                    Else
                        mySession.ExecuteCommand("cd " & CurrentPath)

                        Dim tmp_str() As String = RunFiles(i).Split("\")


                        Job_Cnt = Job_Cnt + 1
                        Me.StatusLbl.ForeColor = Color.Red
                        Me.StatusLbl.Text = "Wait...Upload Input Files...(" & Job_Cnt & ") - " & tmp_str(UBound(tmp_str))
                        mySession.PutFiles(RunFiles(i), CurrentPath & "/", False, myTransferOptions)

                        Dim SendCommand As String = "madymo_std " & RunFilesVer(i) & " 16 " & tmp_str(UBound(tmp_str)) & " AUTO"
                        Dim SummitResult As WinSCP.CommandExecutionResult = mySession.ExecuteCommand(SendCommand)

                        '=============================================================================================
                        'Process.Start(Application.StartupPath & "\putty.exe", "-telnet 6002317@10.98.40.102")
                        'SendKeys.SendWait("csh0609" & "{ENTER}")
                        '=============================================================================================

                        '=============================================================================================
                        ' 이 코드는 백그라운드로 실행시켜야 함
                        'Create Process
                        Dim conProcess As New Process
                        Dim conInfo As New System.Diagnostics.ProcessStartInfo()

                        Dim strConn As String = "-telnet"
                        Dim strServ As String = SessionInfor(0) & ":23"
                        Dim strUser As String = SessionInfor(1)
                        Dim strPass As String = SessionInfor(2)

                        conInfo.FileName = Application.StartupPath & "\" & "link.dll" '"plink.exe"
                        conInfo.Arguments = strConn & " " & strServ & " -l " & strUser
                        conInfo.RedirectStandardInput = True
                        conInfo.RedirectStandardOutput = True
                        conInfo.UseShellExecute = False
                        conInfo.CreateNoWindow = True
                        conInfo.WindowStyle = ProcessWindowStyle.Hidden

                        conProcess.StartInfo = conInfo
                        conProcess.Start()

                        Dim Writer As System.IO.StreamWriter = conProcess.StandardInput
                        Dim Reader As System.IO.StreamReader = conProcess.StandardOutput

                        'Execute Commands
                        Writer = conProcess.StandardInput
                        Writer.WriteLine(SessionInfor(2))
                        Writer.WriteLine("cd " & CurrentPath)
                        Writer.WriteLine(SendCommand)
                        Writer.Close()

                        'Read Response
                        'Reader = conProcess.StandardOutput
                        'MsgBox(Reader.ReadToEnd().ToString, MsgBoxStyle.Information, "Passing Result")
                        'Reader.Close()

                        'Disconnect
                        conProcess.Close()


                        Dim result As Integer = MessageBox.Show("Open Teminal Session?", "Confirm", MessageBoxButtons.YesNo)
                        If result = DialogResult.No Then

                        ElseIf result = DialogResult.Yes Then
                            'Process.Start(Application.StartupPath & "\putty.exe", "-telnet " & SessionInfor(1) & "@" & SessionInfor(0))

                            Dim TerminalProcess As New Process
                            Dim TerminalInfo As New System.Diagnostics.ProcessStartInfo()

                            TerminalInfo.FileName = Application.StartupPath & "\" & "putty.exe"
                            TerminalInfo.Arguments = "-telnet " & SessionInfor(1) & "@" & SessionInfor(0)
                            TerminalInfo.RedirectStandardInput = True
                            TerminalInfo.RedirectStandardOutput = True
                            TerminalInfo.UseShellExecute = False
                            TerminalInfo.CreateNoWindow = False
                            TerminalInfo.WindowStyle = ProcessWindowStyle.Maximized

                            TerminalProcess.StartInfo = TerminalInfo
                            TerminalProcess.Start()

                            Dim SendWriter As System.IO.StreamWriter = TerminalProcess.StandardInput
                            SendWriter.WriteLine(SessionInfor(2))
                            SendWriter.WriteLine("cd " & CurrentPath)
                            SendWriter.WriteLine("clear")
                            SendWriter.WriteLine("qm")
                        End If
                        '=============================================================================================


                        'mySession.ExecuteCommand(SendCommand)

                        'If SummitResult.IsSuccess = True Then
                        '    'MsgBox(SummitResult.Output, MsgBoxStyle.Information, "Summit Result")
                        '    Job_Result = Job_Result & vbCrLf & "(" & Job_Cnt & ") " & SummitResult.Output.ToString
                        'Else
                        '    'MsgBox(SummitResult.Output, MsgBoxStyle.Information, "Summit Failed")
                        '    Job_Result = Job_Result & vbCrLf & "(" & Job_Cnt & ") " & SummitResult.Output.ToString
                        'End If

                    End If
                Next

                'MsgBox(Job_Result, MsgBoxStyle.Information, "Summit Result")

                Application.DoEvents()
                Me.Close()

            ElseIf e.KeyCode = Keys.Enter And Mode = "Reading" Then
                '다운로드
                '임시 폴더 생성
                If Not Directory.Exists(Application.StartupPath & "\TempResults") Then
                    Directory.CreateDirectory(Application.StartupPath & "\TempResults")
                End If

                '다운로드
                myTransferOptions.TransferMode = TransferMode.Automatic
                Me.StatusLbl.ForeColor = Color.Blue
                Me.StatusLbl.Text = "Wait...Download Result Files..."
                Application.DoEvents()

                '파일명 분리

                Dim tmp_str() As String = RemoteFileLst.SelectedItems(0).Text.Split(".")
                Dim tmp_file As String = tmp_str(0)
                Dim RemoteFiles(0) As String
                Dim Finalremote(0) As String

                For i = 0 To RemoteFileLst.SelectedItems.Count - 1
                    Dim DuplCnt As Integer = -1
                    tmp_str = RemoteFileLst.SelectedItems(i).Text.Split(".")
                    tmp_file = tmp_str(0)
                    If i = 0 Then
                        ReDim RemoteFiles(0)
                        RemoteFiles(0) = tmp_file
                    Else
                        For k = 0 To UBound(RemoteFiles)
                            If RemoteFiles(k) <> tmp_file Then
                                DuplCnt = DuplCnt + 1
                            End If
                        Next
                        If DuplCnt = UBound(RemoteFiles) Then
                            ReDim Preserve RemoteFiles(UBound(RemoteFiles) + 1)
                            RemoteFiles(UBound(RemoteFiles)) = tmp_file
                        End If
                    End If
                Next

                Dim j As Integer = 0
                For j = 0 To UBound(RemoteFiles)
                    If FileExistCheck(RemoteFiles(j)) = True Then
                        ReDim Preserve Finalremote(UBound(Finalremote) + 1)
                        Finalremote(UBound(Finalremote)) = RemoteFiles(j)
                    End If
                Next

                ReDim RemoteFilesNames(UBound(Finalremote) - 1)
                For j = 1 To UBound(Finalremote)
                    RemoteFilesNames(j - 1) = Finalremote(j)
                Next

                For j = 1 To UBound(Finalremote)
                    'mySession.GetFiles(Me.FilePathTxt.Text & "/" & FinalRemote(j) & ".xml", Application.StartupPath & "\TempResults\", False, myTransferOptions).Check()
                    'Me.StatusLbl.Text = "Wait...Download Result Files...xml"
                    'Application.DoEvents()
                    mySession.GetFiles(Me.FilePathTxt.Text & "/" & Finalremote(j) & ".peak", Application.StartupPath & "\TempResults\", False, myTransferOptions).Check()
                    Me.StatusLbl.Text = "Wait...Download Result Files...peak"
                    Application.DoEvents()
                    mySession.GetFiles(Me.FilePathTxt.Text & "/" & Finalremote(j) & ".injury", Application.StartupPath & "\TempResults\", False, myTransferOptions).Check()
                    Me.StatusLbl.Text = "Wait...Download Result Files...peak"
                    Application.DoEvents()
                    mySession.GetFiles(Me.FilePathTxt.Text & "/" & Finalremote(j) & ".log", Application.StartupPath & "\TempResults\", False, myTransferOptions).Check()
                    Me.StatusLbl.Text = "Wait...Download Result Files...log"
                    Application.DoEvents()
                    mySession.GetFiles(Me.FilePathTxt.Text & "/" & Finalremote(j) & ".lac", Application.StartupPath & "\TempResults\", False, myTransferOptions).Check()
                    Me.StatusLbl.Text = "Wait...Download Result Files...lac"
                    Application.DoEvents()
                    mySession.GetFiles(Me.FilePathTxt.Text & "/" & Finalremote(j) & ".frc", Application.StartupPath & "\TempResults\", False, myTransferOptions).Check()
                    Me.StatusLbl.Text = "Wait...Download Result Files...frc"
                    Application.DoEvents()
                    mySession.GetFiles(Me.FilePathTxt.Text & "/" & Finalremote(j) & ".rds", Application.StartupPath & "\TempResults\", False, myTransferOptions).Check()
                    Me.StatusLbl.Text = "Wait...Download Result Files...rds"
                    Application.DoEvents()
                    mySession.GetFiles(Me.FilePathTxt.Text & "/" & Finalremote(j) & ".jps", Application.StartupPath & "\TempResults\", False, myTransferOptions).Check()
                    Me.StatusLbl.Text = "Wait...Download Result Files...jps"
                    Application.DoEvents()
                    mySession.GetFiles(Me.FilePathTxt.Text & "/" & Finalremote(j) & ".fhs", Application.StartupPath & "\TempResults\", False, myTransferOptions).Check()
                    Me.StatusLbl.Text = "Wait...Download Result Files...fhs"
                    Application.DoEvents()
                    mySession.GetFiles(Me.FilePathTxt.Text & "/" & Finalremote(j) & ".control", Application.StartupPath & "\TempResults\", False, myTransferOptions).Check()
                    Me.StatusLbl.Text = "Wait...Download Result Files...control"
                    Application.DoEvents()
                Next

                Me.StatusLbl.Text = "Download Result. Complete"
                RemoteFolderName = Me.FilePathTxt.Text & "/"
                Application.DoEvents()
                Me.Close()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Function FileExistCheck(ByVal filename As String) As Boolean

        'If mySession.FileExists(Me.FilePathTxt.Text & "/" & filename & ".xml") = False Then
        '    FileExistCheck = False
        '    Exit Function
        'End If

        If mySession.FileExists(Me.FilePathTxt.Text & "/" & filename & ".injury") = False Then
            FileExistCheck = False
            Exit Function
        End If

        If mySession.FileExists(Me.FilePathTxt.Text & "/" & filename & ".peak") = False Then
            FileExistCheck = False
            Exit Function
        End If

        If mySession.FileExists(Me.FilePathTxt.Text & "/" & filename & ".log") = False Then
            FileExistCheck = False
            Exit Function
        End If

        If mySession.FileExists(Me.FilePathTxt.Text & "/" & filename & ".lac") = False Then
            FileExistCheck = False
            Exit Function
        End If

        If mySession.FileExists(Me.FilePathTxt.Text & "/" & filename & ".frc") = False Then
            FileExistCheck = False
            Exit Function
        End If

        If mySession.FileExists(Me.FilePathTxt.Text & "/" & filename & ".rds") = False Then
            FileExistCheck = False
            Exit Function
        End If

        If mySession.FileExists(Me.FilePathTxt.Text & "/" & filename & ".jps") = False Then
            FileExistCheck = False
            Exit Function
        End If

        If mySession.FileExists(Me.FilePathTxt.Text & "/" & filename & ".fhs") = False Then
            FileExistCheck = False
            Exit Function
        End If

        If mySession.FileExists(Me.FilePathTxt.Text & "/" & filename & ".control") = False Then
            FileExistCheck = False
            Exit Function
        End If

        FileExistCheck = True
    End Function

    Private Sub SessionFileTransferProgress(ByVal sender As Object, ByVal e As FileTransferProgressEventArgs)

        With Me
            Select Case e.FileProgress
                Case 0.0 To 0.2
                    .StatusLbl.ForeColor = Color.Red
                Case 0.20000001 To 0.4
                    .StatusLbl.ForeColor = Color.OrangeRed
                Case 0.40000001 To 0.6
                    .StatusLbl.ForeColor = Color.DarkOrange
                Case 0.60000001 To 0.8
                    .StatusLbl.ForeColor = Color.Green
                Case 0.80000001 To 1.0
                    .StatusLbl.ForeColor = Color.Blue
            End Select
            .StatusLbl.Text = "Wait...Download File..." & Format(e.FileProgress, "0.00%")

        End With
    End Sub

    Private Sub RemoteFileLst_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoteFileLst.SelectedIndexChanged
        Me.FilePathTxt.Text = CurrentPath
    End Sub

    Private Sub FrmRemoteFileLst_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        With Me
            .FilePathTxt.Location = New Point(0, 0)
            .FilePathTxt.Size = New Size(.ClientRectangle.Width, 23)
            .RemoteFileLst.Location = New Point(0, 23)
            .RemoteFileLst.Size = New Size(.FilePathTxt.Width, .ClientRectangle.Height - 23 - 25)
        End With
    End Sub

    Private Sub RemoteFileLst_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles RemoteFileLst.ColumnClick
        If Sorting = True Then
            RemoteFileLst.ListViewItemSorter = New ListViewSortFormNamespace.ListViewItemComparer(e.Column)
            Sorting = False
        Else
            RemoteFileLst.ListViewItemSorter = New ListViewReverseSortFormNamespace.ListViewItemComparer(e.Column)
            Sorting = True
        End If
    End Sub

End Class

