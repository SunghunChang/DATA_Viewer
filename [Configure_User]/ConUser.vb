Imports System.IO
Imports System

Public Class ConUser

    Public Function ConfigureWho(ByVal Path As String)

        ConfigureWho = True

        Select Case System.Environment.UserName

            Case "6005885", "5405228", "5502875", "5300641", "5504584"
                Try
                    '=====================================================날짜 연산==================================================
                    'If DateDiff(DateInterval.Day, CheckExpire.CurrentDate, CheckExpire.ExpireDate) < 0 Then ' '#1/15/2015#
                    If DateDiff(DateInterval.Day, Date.Now, #12/15/2017#) < 0 Then
                        File.Delete(Path & "\DATA\MADYMO_FE_Materials.xml")
                        'SplashScreenStart.Close()
                        ConfigureWho = False  ' End
                    End If

                    'MsgBox(CurrentDate.ToString("yyyy-MM-dd"))
                    'MsgBox(ExpireDate.ToString("yyyy-MM-dd"))
                    'MsgBox(DateDiff(DateInterval.Day, CurrentDate, ExpireDate)) 'Plus
                    'MsgBox(DateDiff(DateInterval.Day, ExpireDate, CurrentDate)) 'Minus

                    '===============================================================================================================

                    If Not Directory.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\IFCCache") Then
                        '디렉토리 생성
                        Directory.CreateDirectory(System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\IFCCache")
                    End If
                    '디렉토리 설정
                    Dim DirInfo As IO.DirectoryInfo = New IO.DirectoryInfo(System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\IFCCache")
                    DirInfo.Attributes = FileAttributes.Hidden

                    If Not System.IO.File.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\IFCCache\IFCDate.val") Then
                        '파일 생성
                        Dim NewParaFile As StreamWriter
                        NewParaFile = New StreamWriter(System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\IFCCache\IFCDate.val")
                        NewParaFile.Close()
                    End If

                    Dim ReadValFiles As New FileStream(System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\IFCCache\IFCDate.val", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                    '맨 마지막 상수를 FileShare.ReadWrite 로 지정해야 도중 접근이 가능함
                    Using fileNum As StreamWriter = File.AppendText(System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\IFCCache\IFCDate.val")
                        fileNum.WriteLine(Date.Now.ToString)
                        fileNum.Close()
                    End Using

                    ReadValFiles.Seek(0, SeekOrigin.Begin)

                    Dim tmp_date_1 As String
                    Dim tmp_date_2 As String
                    Dim TxtLineCnt As Integer = 0
                    Dim ReadFileNum As New StreamReader(System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\IFCCache\IFCDate.val")

                    tmp_date_1 = ReadFileNum.ReadLine

                    'try
                    Do While Not ReadFileNum.EndOfStream
                        tmp_date_2 = ReadFileNum.ReadLine
                        If DateDiff(DateInterval.Hour, CDate(tmp_date_1), CDate(tmp_date_2)) < 0 Then
                            'SplashScreenStart.Close()
                            File.Delete(Path & "\DATA\MADYMO_FE_Materials.xml")
                            MsgBox("Program End." & vbCrLf & "Abnormal system operation has been detected.", MsgBoxStyle.Critical, "Program Terminated")
                            ConfigureWho = False  ' End
                            Exit Select
                        Else
                            tmp_date_1 = tmp_date_2
                        End If

                        TxtLineCnt = TxtLineCnt + 1
                    Loop
                    'Catch ex As Exception
                    '    SplashScreenStart.Close()
                    '    MsgBox("???")
                    '    End
                    'End Try

                    ReadFileNum.Close()
                    ReadValFiles.Close()

                    If TxtLineCnt > 150 Then
                        '기존파일 삭제
                        System.IO.File.Delete(System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\IFCCache\IFCDate.val")
                        '파일 생성
                        Dim NewValFiles As StreamWriter
                        NewValFiles = New StreamWriter(System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\IFCCache\IFCDate.val")
                        NewValFiles.Close()
                        '날짜를 써줌
                        Using NewfileNum As StreamWriter = File.AppendText(System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\IFCCache\IFCDate.val")
                            NewfileNum.WriteLine(Date.Now.ToString)
                            NewfileNum.Close()
                        End Using
                    End If

                    '===============================================================================================================
                Catch ex As Exception
                    'In case of Program Validation Fail
                    'SplashScreenStart.Close()
                    MsgBox("ERROR_IPSEC_MM_POLICY_NOT_FOUND" & vbCrLf & "13004 (0x32CC)", MsgBoxStyle.Critical, "Program Terminated by Validation Error")
                    ConfigureWho = False  ' End 'End
                    Exit Select
                End Try

        End Select

    End Function

    Public Function ProgramRefine() As Boolean
        Select Case System.Environment.UserName
            Case "6002317"
                 ProgramRefine = True
            Case "5805793"
                ProgramRefine = True
            Case "6304537"
                 ProgramRefine = True
            Case "6427553"
                ProgramRefine = True
            Case Else
                ProgramRefine = False
        End Select
    End Function
End Class
