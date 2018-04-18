Imports System.IO

Module NodeExcluding

    Public Function ReadingSETNODE(ByVal SETNODEfile As String) As Integer()

        'LS-DYNA
        Dim tmp_txt As String
        Dim NODE_str() As Integer = Nothing
        Dim m As Integer = 0
        Dim fileNum As StreamReader
        Dim ReadDYNA As New FileStream(SETNODEfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        fileNum = New StreamReader(ReadDYNA)

        MainMDI.Statuslbl.Text = "Find Node List.................."
        Application.DoEvents()

        tmp_txt = fileNum.ReadLine

        m = 0

        Do While Not fileNum.EndOfStream


            Select Case tmp_txt
                Case "*SET_NODE_LIST"
                    '=============================================================
                    'SET_NODE_LIST
                    Dim TmpLine As String = ""
                    Dim GID, i As Integer
                    Dim GNAME As String = ""


                    tmp_txt = fileNum.ReadLine()

                    Do While Mid(tmp_txt, 1, 1) = "$"
                        If Mid(tmp_txt, 1, 7) = "$HMNAME" Then
                            GNAME = RTrim(Mid(tmp_txt, 21, Len(tmp_txt)))
                        End If
                        tmp_txt = fileNum.ReadLine()
                    Loop

                    GID = Mid(tmp_txt, 1, 10)

                    tmp_txt = fileNum.ReadLine()

                    '주석을 건너뜀======================
                    Do While Mid(tmp_txt, 1, 1) = "$"
                        tmp_txt = fileNum.ReadLine()
                    Loop
                    '==================================



                    Do While Not (Mid(tmp_txt, 1, 1) = "*" Or Mid(tmp_txt, 1, 1) = "$" Or tmp_txt = Nothing)

                        For i = 0 To 7
                            If IsNumeric(Mid(tmp_txt, i * 10 + 1, 10)) Then
                                ReDim Preserve NODE_str(m)
                                NODE_str(m) = CInt(Mid(tmp_txt, i * 10 + 1, 10))
                                m = m + 1
                            Else
                                Exit For
                            End If
                        Next

                        tmp_txt = fileNum.ReadLine()

                    Loop
                    '=============================================================

                Case Else
                    tmp_txt = fileNum.ReadLine()
            End Select
        Loop

        fileNum.Close()

        ReadingSETNODE = NODE_str

    End Function

    Public Sub PSM_Node_Excluding(ByVal PSMfile As String, ByVal Tmp_Ex_Node() As Integer)

        MainMDI.Statuslbl.Text = "Excluding Node List.................."
        Application.DoEvents()

        Dim i, k As Integer
        Dim Tmp_Str As String
        Dim Tmp_Str_Split() As String
        Dim OldTotNodes As Integer
        '원래 파일의 Node List
        Dim OldNodesLst() As Integer
        '새로운 파일의 Node List
        Dim NewNodeLst(0) As Integer

        Dim ReadPSMFiles As New FileStream(PSMfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        Dim FileReading As StreamReader

        FileReading = New StreamReader(ReadPSMFiles)

        Tmp_Str = FileReading.ReadLine()
        Tmp_Str = FileReading.ReadLine()

        '원래 PSM 파일의 노드 갯수를 가져옴
        Tmp_Str_Split = Tmp_Str.Split(Chr(9))
        OldTotNodes = Tmp_Str_Split(LBound(Tmp_Str_Split))
        ReDim OldNodesLst((OldTotNodes) - 1)

        '일단 원래 파일의 노드 번호를 읽어옴
        For k = 0 To OldTotNodes Step 15 '+ 15 Step 15
            Tmp_Str = FileReading.ReadLine()
            Tmp_Str_Split = Tmp_Str.Split(Chr(9))
            If UBound(Tmp_Str_Split) = 0 Then Exit For

            ''===
            'If k > 2600 Then
            '    MsgBox("")
            'End If
            ''===

            For i = 0 To UBound(Tmp_Str_Split) - 1
                OldNodesLst(i + k) = Tmp_Str_Split(i)
            Next

        Next


        '제거해야 할 노드 번호들 (다른 방법으로 읽어와야함 - 별도 루틴 작성 필요)
        'Dim Tmp_Ex_Node() As Integer = {70011917, 70008869, 70008805, 70007936, 70008651}

        Array.Sort(Tmp_Ex_Node)

        '제거해야 할 노드의 위치 (index) 를 기억하는 변수
        Dim NodesPos() As Integer = Nothing
        Dim m As Integer = 0


        '새로운 파일을 생성(로그 파일)
        Dim NewLogFile = New StreamWriter(Mid(PSMfile, 1, Len(PSMfile) - 4) & "_RigidNodeExclude.log")
        NewLogFile.WriteLine("****************************************************************")
        NewLogFile.WriteLine("                  Excluding NODEs for MADYMO PSM")
        NewLogFile.WriteLine("****************************************************************")
        NewLogFile.WriteLine("")

        'NewNodesLst
        '제거해야 할 노드가 몇번째 위치하는지 파악 (이중 루프를 돌릴 필요가 있다.)
        For k = 0 To UBound(Tmp_Ex_Node) '제거해야할 노드 번호들이 여기 해당함
            For i = 0 To OldNodesLst.Count - 1
                If OldNodesLst(i) = Tmp_Ex_Node(k) Then
                    NewLogFile.WriteLine("NODE : " & Tmp_Ex_Node(k) & " at " & i)
                    ReDim Preserve NodesPos(m)
                    NodesPos(m) = i
                    m = m + 1
                End If
            Next
        Next



        NewLogFile.Close()

        '제거해야할 노드의 위치를 정렬 (작은 수부터)
        Array.Sort(NodesPos)

        Dim n As Integer = 0
        Dim PerLineEleNum As Integer = 14 ' 한 줄에 들어가는 노드 넘버 숫자 : 15개

        Tmp_Str = ""

        '새로운 파일을 생성(노드제거 파일)
        Dim NewPSMFile = New StreamWriter(Mid(PSMfile, 1, Len(PSMfile) - 4) & "_RigidNodeExclude.inc")
        NewPSMFile.WriteLine("#!<MOT,R5.3>")
        NewPSMFile.WriteLine(OldNodesLst.Count - NodesPos.Count & Chr(9) & "3")

        '제거해야할 노드를 빼고 새 배열을 저장
        For k = 0 To UBound(OldNodesLst) '- NodesPos.Count) Step 15

            If k <> NodesPos(n) Then
                'If CInt(OldNodesLst(k + i)) = 9174002 Then MsgBox(" ")
                'Tmp_Str = Tmp_Str & OldNodesLst(k + i) & Chr(9)
                ReDim Preserve NewNodeLst(UBound(NewNodeLst) + 1)
                NewNodeLst(UBound(NewNodeLst)) = OldNodesLst(k)
            Else
                n = n + 1
                If UBound(NodesPos) < n Then n = UBound(NodesPos)
                'PerLineEleNum = PerLineEleNum + 1
            End If

            'For i = 0 To PerLineEleNum
            '    If ((i + k) <> NodesPos(n)) And ((k + i) <= UBound(OldNodesLst)) Then
            '        'If CInt(OldNodesLst(k + i)) = 9174002 Then MsgBox(" ")
            '        'Tmp_Str = Tmp_Str & OldNodesLst(k + i) & Chr(9)
            '        'ReDim Preserve NewNodeLst(UBound(NewNodeLst) + 1)
            '        'NewNodeLst(UBound(NewNodeLst)) = OldNodesLst(k + i)
            '    Else
            '        n = n + 1
            '        If UBound(NodesPos) < n Then n = UBound(NodesPos)
            '        'PerLineEleNum = PerLineEleNum + 1
            '    End If
            'Next
            ''한 줄을 쓴다.
            'If CStr(Tmp_Str) = "" Then

            'Else
            '    NewPSMFile.WriteLine(RTrim(Tmp_Str))
            'End If

            ''지운다.
            'Tmp_Str = ""
            'PerLineEleNum = 14
        Next

        '파일에 노드 기록
        Tmp_Str = ""
        For k = 0 To CInt((UBound(NewNodeLst)) / 15) Step 1
            Try
                For i = 1 To 15
                    Tmp_Str = Tmp_Str & NewNodeLst(15 * k + i) & Chr(9)
                Next
                NewPSMFile.WriteLine(Tmp_Str)
            Catch ex As Exception
                NewPSMFile.WriteLine(Tmp_Str)
            End Try

            Tmp_Str = ""
        Next

        '시간대별 노드 위치를 써줌 (제거할 노드의 좌표는 제외)
        n = 0
        Do While Not FileReading.EndOfStream
            '시간을 쓴다.
            NewPSMFile.WriteLine(FileReading.ReadLine())
            '노드 좌표를 쓴다.
            For i = 0 To UBound(OldNodesLst) Step 1
                Tmp_Str = FileReading.ReadLine
                Select Case i
                    Case NodesPos(n)
                        n = n + 1
                        If UBound(NodesPos) < n Then n = UBound(NodesPos)
                    Case Else
                        If Not Tmp_Str = "" Then
                            NewPSMFile.WriteLine(Tmp_Str)
                        End If
                End Select
            Next
            n = 0
        Loop


        NewPSMFile.Close()
        FileReading.Close()

        MainMDI.Statuslbl.Text = "작업 완료. 동일 폴더내에 생성된 inc 파일과 log 파일을 확인하세요."
    End Sub

End Module
