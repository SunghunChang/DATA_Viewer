Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.Strings
Imports DSOFile
Imports WindowsApplication1.ListViewSortFormNamespace

Public Class FrmFileBrowser

    Private EventPass As Boolean = False

    Private Sub FrmFileBrowser_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        MainMDI.Statuslbl.Text = "File Comments Browser Close."
    End Sub

    Private Sub FrmFileBrowser_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Last_Opened_Folder <> "" Then
            Populate(Last_Opened_Folder)
        Else
            Populate(Me.InputCmb.Text)
        End If

        Me.MdiParent = MainMDI

        Me.Size = New Size(1400, 800)
        MainMDI.Statuslbl.Text = "File Comments Browser Open."
    End Sub

    Private Sub Populate(ByVal Path As String)

        EventPass = True

        Try

            With Me
                .FileLstView.Items.Clear()
                .InputCmb.Text = Path

                Dim lb As New ListBox

                For Each Str As String In My.Computer.FileSystem.GetDirectories(Path)
                    lb.Items.Add(Str)
                Next

                For Each Str As String In My.Computer.FileSystem.GetFiles(Path)
                    lb.Items.Add(Str)
                Next

                .FileLstView.Items.Add("...")
                .FileLstView.Items(0).SubItems.Add(".[Root]")
                .FileLstView.Items(0).SubItems.Add(".[Root]   ")
                .FileLstView.Items(0).SubItems.Add(".[Root]")
                .FileLstView.Items(0).SubItems.Add(".[Root]")
                .FileLstView.Items(0).SubItems.Add(Path)
                'Try
                '    .FileLstView.Items(0).SubItems.Add(My.Computer.FileSystem.GetParentPath(Path))
                'Catch ex As Exception
                '    .FileLstView.Items(0).SubItems.Add(Path)
                'End Try

                Dim Tmp_Comments As String

                For Each item In lb.Items
                    Dim NewItem As New ListViewItem

                    If My.Computer.FileSystem.FileExists(item.ToString) Then
                        Dim info As New System.IO.FileInfo(item.ToString)

                        'Reading Comments
                        Try
                            Dim myFile As String = info.FullName
                            Dim myDSO As DSOFile.OleDocumentProperties = New DSOFile.OleDocumentProperties
                            myDSO.Open(myFile, True, dsoFileOpenOptions.dsoOptionOpenReadOnlyIfNoWriteAccess)
                            Tmp_Comments = myDSO.SummaryProperties.Comments.ToString
                            myDSO.Close()
                        Catch ex As Exception
                            Tmp_Comments = ""
                        End Try

                        With NewItem
                            .Text = info.Name ' item.ToString
                            .SubItems.Add(info.Extension)
                            .SubItems.Add(Tmp_Comments)
                            .SubItems.Add(Format(info.Length * 0.00000095367, "#0.0").ToString & " MB   ")
                            '.SubItems.Add(info.CreationTime)
                            .SubItems.Add(info.LastWriteTime)
                            .SubItems.Add(info.ToString)
                            If .Text.EndsWith("xml") Then
                                .ForeColor = Color.Blue
                                .Font = New Font("맑은 고딕", 9, FontStyle.Bold)
                            ElseIf .Text.EndsWith("ppt") Or .Text.EndsWith("pptx") Then
                                .ForeColor = Color.Red
                                .Font = New Font("맑은 고딕", 9, FontStyle.Bold)
                            ElseIf .Text.EndsWith("xls") Or .Text.EndsWith("xlsx") Then
                                .ForeColor = Color.Green
                                .Font = New Font("맑은 고딕", 9, FontStyle.Bold)
                            ElseIf .Text.EndsWith("pdf") Then
                                .ForeColor = Color.Brown
                                .Font = New Font("맑은 고딕", 9, FontStyle.Bold)
                            End If

                        End With

                    Else

                        Dim info As New System.IO.DirectoryInfo(item.ToString)
                        With NewItem
                            Dim tmp() As String
                            tmp = (item.ToString).Split("\")
                            .Text = tmp(UBound(tmp)) 'item.ToString
                            .SubItems.Add("[Directory]")
                            .SubItems.Add("")
                            .SubItems.Add("")
                            '.SubItems.Add(info.CreationTime)
                            .SubItems.Add(info.LastWriteTime)
                            .SubItems.Add(info.ToString)
                            .Font = New Font("맑은 고딕", 9, FontStyle.Bold)
                            .ForeColor = Color.DarkGoldenrod
                        End With
                    End If
                    .FileLstView.Items.Add(NewItem)
                Next

                .FileLstView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent)
                .FileLstView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent)
                .FileLstView.AutoResizeColumn(2, ColumnHeaderAutoResizeStyle.ColumnContent)
                .FileLstView.AutoResizeColumn(3, ColumnHeaderAutoResizeStyle.ColumnContent)
                .FileLstView.AutoResizeColumn(4, ColumnHeaderAutoResizeStyle.ColumnContent)

            End With

            'Me.ListView1.Sorting  = 1

        Catch ex As Exception

            If Path.EndsWith("내 문서") Then
                Path = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                Populate(Path)
            ElseIf Path.EndsWith("내 그림") Then
                Path = My.Computer.FileSystem.SpecialDirectories.MyPictures
                Populate(Path)
            End If

        End Try

        Last_Opened_Folder = Path

        EventPass = False
    End Sub

    Private Sub FileLstView_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles FileLstView.ColumnClick
        Me.FileLstView.ListViewItemSorter = New ListViewSortFormNamespace.ListViewItemComparer(e.Column)
    End Sub

    Private Sub FileLstView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles FileLstView.DoubleClick

        If My.Computer.FileSystem.FileExists(Me.FileLstView.FocusedItem.SubItems(5).Text) Then
            Process.Start(Me.FileLstView.FocusedItem.SubItems(5).Text) '(Me.FileLstView.FocusedItem.Text)
        Else
            If Me.FileLstView.FocusedItem.Text = "..." Then
                Try
                    Populate(My.Computer.FileSystem.GetParentPath(InputCmb.Text))
                Catch ex As Exception
                    MsgBox("You cannot get any farther back than a drive! [" & Me.InputCmb.Text & "]", MsgBoxStyle.Critical, "Error")
                End Try
            Else
                Populate(Me.FileLstView.FocusedItem.SubItems(5).Text) '(Me.FileLstView.FocusedItem.Text)
            End If
        End If

    End Sub

    Private Sub InputCmb_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles InputCmb.KeyDown
        If e.KeyCode = Keys.Enter Then
            Me.InputCmb.Items.Add(Me.InputCmb.Text)
            Try
                Populate(Me.InputCmb.Text)
            Catch ex As Exception
                MsgBox(Me.InputCmb.Text & " does not exist on this system as a directoy. Check spelling?", MsgBoxStyle.Information, "Error")
            End Try
        End If
    End Sub

    Private Sub FileLstView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles FileLstView.KeyDown
        If e.KeyCode = Keys.Enter Then
            FileLstView_DoubleClick(Nothing, e)
        ElseIf e.KeyCode = Keys.F5 Then
            Populate(Me.InputCmb.Text)
        End If
    End Sub

    Private Sub FileLstView_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FileLstView.SelectedIndexChanged
        If EventPass = False Then
            Me.CommentTxt.Text = ""
            Me.CommentTxt.Text = Me.FileLstView.FocusedItem.SubItems(2).Text
            If Me.FileLstView.FocusedItem.Index = 0 Then
                Me.InputCmb.Text = Me.FileLstView.FocusedItem.SubItems(5).Text
            Else
                Me.InputCmb.Text = FilePathGet2(Me.FileLstView.FocusedItem.SubItems(5).Text)
            End If

            MainMDI.Statuslbl.Text = Me.FileLstView.FocusedItem.SubItems(0).Text
        End If
    End Sub

    Private Sub FrmFileBrowser_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Try
            With Me
                .InputCmb.Location = New Point(5, 5)
                .Button1.Size = New Size(120, 23)
                .InputCmb.Size = New Size(.ClientRectangle.Width - 15 - 120, 23)
                .Button1.Location = New Point(.InputCmb.Location.X + .InputCmb.Width + 5, 5)

                .FileLstView.Location = New Point(5, .InputCmb.Location.Y + .InputCmb.Height + 5)
                .CommentTxt.Size = New Size(.ClientRectangle.Width - 10, 110)
                .FileLstView.Size = New Size(.ClientRectangle.Width - 10, .ClientRectangle.Height - 50 - .CommentTxt.Height)
                .CommentTxt.Location = New Point(5, .FileLstView.Location.Y + .FileLstView.Height + 5)

                .FileLstView.Columns(0).Width = .FileLstView.Width * 0.38
                .FileLstView.Columns(1).Width = .FileLstView.Width * 0.15
                .FileLstView.Columns(2).Width = .FileLstView.Width * 0.25
                .FileLstView.Columns(3).Width = .FileLstView.Width * 0.1
                .FileLstView.Columns(4).Width = .FileLstView.Width * 0.1
                .FileLstView.Columns(5).Width = .FileLstView.Width * 0
            End With
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        With Me.FolderBrowserDialog1
            .Description = "Select Path [Folder]"
            .SelectedPath = Me.InputCmb.Text
            .ShowDialog()
        End With

        Me.InputCmb.Text = Me.FolderBrowserDialog1.SelectedPath
        InputCmb_KeyDown(Nothing, New KeyEventArgs(Keys.Enter))
    End Sub

    Private Sub CommentTxt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CommentTxt.KeyDown
        With Me
            If .Text = ".[Root]" Then Exit Sub

            If e.Modifiers = Keys.Control And e.KeyCode = Keys.S Then
                Try
                    Dim myFile As String = Me.FileLstView.FocusedItem.SubItems(5).Text
                    Dim myDSO As DSOFile.OleDocumentProperties = New DSOFile.OleDocumentProperties
                    myDSO.Open(myFile, False, dsoFileOpenOptions.dsoOptionUseMBCStringsForNewSets)
                    myDSO.SummaryProperties.Comments = CommentTxt.Text
                    myDSO.Save()
                    myDSO.Close()
                    MainMDI.Statuslbl.Text = "Save Comments Successfully"
                    Populate(Me.InputCmb.Text)
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
        End With
    End Sub

    Private Sub RemoteRunToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoteRunToolStripMenuItem.Click

        If Connected = True Then
            Dim TmpFiles(0) As String

            For i = 0 To Me.FileLstView.SelectedItems.Count - 1
                ReDim Preserve TmpFiles(UBound(TmpFiles) + 1)
                TmpFiles(UBound(TmpFiles)) = Me.FileLstView.SelectedItems.Item(i).Text
            Next

            Dim TmpFiles_1(UBound(TmpFiles) - 1) As String
            Dim TmpVersion(UBound(TmpFiles) - 1) As String
            Dim Tmp_txt As String

            For i = 1 To UBound(TmpFiles)
                TmpFiles_1(i - 1) = Last_Opened_Folder & "\" & TmpFiles(i)

                If Microsoft.VisualBasic.Strings.Right(TmpFiles_1(i - 1), 3) <> "xml" Then
                    TmpVersion(i - 1) = "None"
                Else


                    Dim ReadFiles As New FileStream(TmpFiles_1(i - 1), FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                    '맨 마지막 상수를 FileShare.ReadWrite 로 지정해야 해석 도중 접근이 가능함

                    Using fileNum As New StreamReader(ReadFiles)
                        Do While Not fileNum.EndOfStream
                            Tmp_txt = fileNum.ReadLine
                            If InStr(Tmp_txt, "RELEASE=""R7.4""", vbTextCompare) >= 1 Then
                                TmpVersion(i - 1) = "74"
                                Exit Do
                            ElseIf InStr(Tmp_txt, "RELEASE=""R7.4.2""", vbTextCompare) >= 1 Then
                                TmpVersion(i - 1) = "742"
                                Exit Do
                            ElseIf InStr(Tmp_txt, "RELEASE=""R7.5""", vbTextCompare) >= 1 Then
                                TmpVersion(i - 1) = "75"
                                Exit Do
                            End If
                        Loop
                    End Using

                    ReadFiles.Close()

                End If

            Next

            Dim RemoteFolder As New FrmRemoteFileLst("Run", TmpFiles_1, TmpVersion)
            RemoteFolder.ShowDialog(Me)
        Else
            If Connected = False Then
                MsgBox("Server connection is required.", MsgBoxStyle.Information, "Try Again")
            End If
        End If
    End Sub

    Private Sub LocalRunMADYMOToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LocalRunMADYMOToolStripMenuItem.Click

        Dim TmpFiles(0) As String

        For i = 0 To Me.FileLstView.SelectedItems.Count - 1
            ReDim Preserve TmpFiles(UBound(TmpFiles) + 1)
            TmpFiles(UBound(TmpFiles)) = Me.FileLstView.SelectedItems.Item(i).Text
        Next

        Dim TmpFiles_1(UBound(TmpFiles) - 1) As String
        Dim TmpVersion(UBound(TmpFiles) - 1) As String
        Dim Tmp_txt As String

        For i = 1 To UBound(TmpFiles)
            TmpFiles_1(i - 1) = Last_Opened_Folder & "\" & TmpFiles(i)

            If Microsoft.VisualBasic.Strings.Right(TmpFiles_1(i - 1), 3) <> "xml" Then
                TmpVersion(i - 1) = "None"
            Else


                Dim ReadFiles As New FileStream(TmpFiles_1(i - 1), FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                '맨 마지막 상수를 FileShare.ReadWrite 로 지정해야 해석 도중 접근이 가능함

                Using fileNum As New StreamReader(ReadFiles)
                    Do While Not fileNum.EndOfStream
                        Tmp_txt = fileNum.ReadLine
                        If InStr(Tmp_txt, "RELEASE=""R7.4""", vbTextCompare) >= 1 Then
                            TmpVersion(i - 1) = "74"
                            Exit Do
                        ElseIf InStr(Tmp_txt, "RELEASE=""R7.4.2""", vbTextCompare) >= 1 Then
                            TmpVersion(i - 1) = "742"
                            Exit Do
                        ElseIf InStr(Tmp_txt, "RELEASE=""R7.5""", vbTextCompare) >= 1 Then
                            TmpVersion(i - 1) = "75"
                            Exit Do
                        End If
                    Loop
                End Using

                ReadFiles.Close()

                'Job Run
                Dim Executable As String = ""
                Select Case TmpVersion(i - 1)
                    Case "74"
                        Executable = "madymo74.exe"
                    Case "742"
                        Executable = "madymo742.exe"
                    Case "75"
                        Executable = "madymo75.exe"
                End Select
                Dim CommandLine As String = "-3d " & TmpFiles_1(i - 1)
                Dim MyStartInfo As New Diagnostics.ProcessStartInfo(Executable, CommandLine)
                MyStartInfo.WorkingDirectory = Last_Opened_Folder ' 작업 디렉토리를 현재 디랙토리로 변경함 ************************************************ 중요 !!!! *********************
                MyStartInfo.UseShellExecute = True                             ' CMD.EXE 등을 사용하지 않음, 직접실행
                'MyStartInfo.RedirectStandardOutput = True                      ' 프로그램 출력(STDOUT)을 Redirect 함
                'MyStartInfo.RedirectStandardInput = False                      ' STDIN 은 Redirect 하지 않음
                'MyStartInfo.CreateNoWindow = False                              ' 프로그램 실행 윈도우즈를 만들지 않음

                Dim MyProcess As New Diagnostics.Process
                MyProcess.StartInfo = MyStartInfo
                MyProcess.Start()

            End If

        Next

    End Sub
End Class

Namespace ListViewSortFormNamespace

    Public Class ListViewItemComparer
        Implements IComparer

        Private col As Integer

        Public Sub New()
            col = 0
        End Sub

        Public Sub New(ByVal column As Integer)
            col = column
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Return [String].Compare(CType(x, ListViewItem).SubItems(col).Text, CType(y, ListViewItem).SubItems(col).Text)
        End Function

    End Class

End Namespace

Namespace ListViewReverseSortFormNamespace

    Public Class ListViewItemComparer
        Implements IComparer

        Private col As Integer

        Public Sub New()
            col = 0
        End Sub

        Public Sub New(ByVal column As Integer)
            col = column
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Return -[String].Compare(CType(x, ListViewItem).SubItems(col).Text, CType(y, ListViewItem).SubItems(col).Text)
        End Function

    End Class

End Namespace