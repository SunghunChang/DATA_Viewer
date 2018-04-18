Imports System.IO

Public Class FrmFEconvertingInXML

    Dim IsXMAgicformat As Boolean = True
    Dim WorkingPath As String = ""

    Private Sub DyOpnBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DyOpnBtn.Click

        Dim DlgResult As DialogResult

        Dim m As Integer = 0

        Me.LstFEsystem.Enabled = False
        Me.BtnConvert.Enabled = False

        For m = Me.LstFEsystem.Items.Count - 1 To 0 Step -1
            Me.LstFEsystem.Items.Remove(m)
        Next

        With Me.XMLFileDlg
            .Title = "Select XML File"
            .Multiselect = False
            .FileName = ""                   '초기에 표시되는 파일 이름
            '.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "XML File|*.xml|모든 파일|*.*"
            DlgResult = .ShowDialog()
        End With

        If DlgResult = DialogResult.Cancel Then
            Me.StatusLbl.Text = "Canceled"
            Exit Sub
        Else
            Me.DyNameTxt.Text = Me.XMLFileDlg.SafeFileName
            Me.LstFEsystem.Enabled = True
            Me.BtnConvert.Enabled = True
            WorkingPath = FilePathGet2(Me.XMLFileDlg.FileName) '\있음
        End If

        Dim fileNum As StreamReader
        Dim Tmp_Txt As String
        Dim SystemName As String = ""
        Dim IsDisabled As Boolean = False
        Dim DisabledCnt As Integer = 0

        'Configure FE-MODELs
        Dim ReadXML As New FileStream(Me.XMLFileDlg.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        fileNum = New StreamReader(ReadXML)

        Do While Not fileNum.EndOfStream
            Tmp_Txt = fileNum.ReadLine

            'XML SPY와 XMAgic 동일
            'Disable 건너뛰기
            If InStr(Tmp_Txt, "<DISABLE>") >= 1 Then
                DisabledCnt = DisabledCnt + 1
                Do While Not fileNum.EndOfStream
                    Tmp_Txt = fileNum.ReadLine
                    If (InStr(Tmp_Txt, "</DISABLE>") >= 1) Then
                        DisabledCnt = DisabledCnt - 1
                        If DisabledCnt = 0 Then Exit Do
                    ElseIf InStr(Tmp_Txt, "<DISABLE>") >= 1 Then
                        DisabledCnt = DisabledCnt + 1
                    End If
                Loop
            End If

            If (InStr(Tmp_Txt, "<SYSTEM.MODEL") > 1) Then
                If InStr(Tmp_Txt, "NAME=") > 1 Then
                    'XML SPY 형식
                    SystemName = FindXMLattributeByName("NAME", Tmp_Txt)
                    IsXMAgicformat = False
                Else
                    'XMAgic 형식
                    Tmp_Txt = fileNum.ReadLine
                    Do While Not (InStr(Tmp_Txt, ">") > 1)
                        If InStr(Tmp_Txt, "NAME=") > 1 Then
                            SystemName = FindXMLattributeByName("NAME", Tmp_Txt)
                            Exit Do
                        End If
                        Tmp_Txt = fileNum.ReadLine
                    Loop
                End If
            End If

            If (InStr(Tmp_Txt, "<FE_MODEL") > 1) Then
                If InStr(Tmp_Txt, "NAME=") > 1 Then
                    'XML SPY 형식
                    Me.LstFEsystem.Items.Add(SystemName & "__" & FindXMLattributeByName("NAME", Tmp_Txt))
                Else
                    'XMAgic 형식
                    Tmp_Txt = fileNum.ReadLine
                    Do While Not (InStr(Tmp_Txt, ">") > 1)
                        If InStr(Tmp_Txt, "NAME=") > 1 Then
                            Me.LstFEsystem.Items.Add(SystemName & "__" & FindXMLattributeByName("NAME", Tmp_Txt))
                            Exit Do
                        End If
                        Tmp_Txt = fileNum.ReadLine
                    Loop
                End If
            End If
        Loop

        Me.StatusLbl.Text = "Complete Configuring FE MODEL SYSTEM."

        fileNum.Close()

    End Sub
    
    Private Sub BtnConvert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnConvert.Click

        Dim DlgResult As DialogResult
        Dim Tmp_Txt As String

        'Reading XML
        Dim fileNum As StreamReader
        Dim ReadXML As New FileStream(Me.XMLFileDlg.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        fileNum = New StreamReader(ReadXML)

        With Me.KeySave
            .Title = "Save File"
            .FileName = Me.LstFEsystem.Items(Me.LstFEsystem.SelectedIndex)                 '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "LS-DYNA Key File|*.key|모든 파일|*.*"
            DlgResult = .ShowDialog()
        End With

        If DlgResult = DialogResult.Cancel Then
            Me.StatusLbl.Text = "Save Cancel"
            Exit Sub
        Else
            Me.StatusLbl.Text = "Converting Start..." 'Me.KeySave.FileName
            Application.DoEvents()
        End If

        'New Key File
        Dim NewKeyFile As StreamWriter
        NewKeyFile = New StreamWriter(Me.KeySave.FileName)

        NewKeyFile.WriteLine("$$ Ls-dyna Input Deck -Mesh Only- Generated by MDV Program")
        NewKeyFile.WriteLine("$$ From XML to KeyFile")
        NewKeyFile.WriteLine("$$ Program is created by C.S.H")
        NewKeyFile.WriteLine("*KEYWORD")

        Dim SystemName As String = ""
        Dim TmpSystemName As String = ""
        Dim FEmodelName As String = ""
        Dim TmpFEmodelName As String = ""
        Dim WritingText As String = ""
        Dim Tmp_str() As String
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim DisabledCnt As Integer = 0
        Dim EndLine() As String

        Tmp_str = Split(Me.LstFEsystem.Items(Me.LstFEsystem.SelectedIndex), "__")
        SystemName = Tmp_str(0)
        FEmodelName = Tmp_str(1)

        'Find FE Model Again...
        If IsXMAgicformat = True Then
            'XMAgic Format ================================================================================================ XMAgic Format
            Do While Not fileNum.EndOfStream
                Tmp_Txt = fileNum.ReadLine

                'XML SPY와 XMAgic 동일
                'Disable 건너뛰기
                If InStr(Tmp_Txt, "<DISABLE>") >= 1 Then

                    Me.StatusLbl.Text = "Skip Disabled.."
                    Application.DoEvents()

                    DisabledCnt = DisabledCnt + 1
                    Do While Not fileNum.EndOfStream
                        Tmp_Txt = fileNum.ReadLine
                        If (InStr(Tmp_Txt, "</DISABLE>") >= 1) Then
                            DisabledCnt = DisabledCnt - 1
                            If DisabledCnt = 0 Then Exit Do
                        ElseIf InStr(Tmp_Txt, "<DISABLE>") >= 1 Then
                            DisabledCnt = DisabledCnt + 1
                        End If
                    Loop
                End If

                If (InStr(Tmp_Txt, "<SYSTEM.MODEL") > 1) Then

                    Me.StatusLbl.Text = "Analyzing SYSTEM.MODEL"
                    Application.DoEvents()

                    Tmp_Txt = fileNum.ReadLine
                    Do While Not (InStr(Tmp_Txt, ">") > 1)
                        If InStr(Tmp_Txt, "NAME=") > 1 Then
                            TmpSystemName = FindXMLattributeByName("NAME", Tmp_Txt)
                            Exit Do
                        End If
                        Tmp_Txt = fileNum.ReadLine
                    Loop
                End If

                If (InStr(Tmp_Txt, "<FE_MODEL") > 1) Then
                    Me.StatusLbl.Text = "Analyzing FE_MODEL"
                    Application.DoEvents()
                    'XMAgic 형식
                    Tmp_Txt = fileNum.ReadLine
                    Do While Not (InStr(Tmp_Txt, ">") > 1)
                        If InStr(Tmp_Txt, "NAME=") > 1 Then
                            TmpFEmodelName = FindXMLattributeByName("NAME", Tmp_Txt)
                            Exit Do
                        End If
                        Tmp_Txt = fileNum.ReadLine
                    Loop

                    '선택한 FE MODEL이 맞는지 확인후 작업 시작
                    If (SystemName = TmpSystemName) And (FEmodelName = TmpFEmodelName) Then

                        Me.StatusLbl.Text = "Selected FE_MODEL Converting Start..."
                        Application.DoEvents()

                        Do While Not fileNum.EndOfStream

                            '변환작업
                            Tmp_Txt = fileNum.ReadLine

                            If InStr(Tmp_Txt, "</FE_MODEL>") >= 1 Then Exit Do

                            If InStr(Tmp_Txt, "COORDINATE.CARTESIAN") >= 1 And (Me.ChkRefCoord.Checked = False) Then 'COORDINATE.CARTESIAN ======================================
                                Tmp_Txt = fileNum.ReadLine
                                Tmp_Txt = fileNum.ReadLine
                                '테이블의 종류별로 써준다. at NewKeyFile
                                If InStr(Tmp_Txt, "<![CDATA[") >= 1 Then
                                    Tmp_Txt = fileNum.ReadLine ' |	 ID 	 X 	 Y 	 Z 	 |
                                    Tmp_Txt = fileNum.ReadLine ' 수의 시작
                                    NewKeyFile.WriteLine("*NODE")
                                    Do While Not fileNum.EndOfStream
                                        If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                                        '숫자를 나눔
                                        WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 16) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 16) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 16)
                                        NewKeyFile.WriteLine(WritingText)

                                        Tmp_Txt = fileNum.ReadLine
                                    Loop

                                    '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                                    EndLine = Tmp_Txt.Split("]]>")
                                    If EndLine(0) = "" Then
                                    Else
                                        If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                                            '숫자를 나눔
                                            WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 16) & _
                                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 16) & _
                                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 16)
                                            NewKeyFile.WriteLine(WritingText)
                                        End If
                                    End If

                                End If

                            ElseIf InStr(Tmp_Txt, "COORDINATE_REF.CARTESIAN") >= 1 And (Me.ChkRefCoord.Checked = True) Then 'COORDINATE_REF.CARTESIAN ======================================
                                Tmp_Txt = fileNum.ReadLine
                                Tmp_Txt = fileNum.ReadLine
                                '테이블의 종류별로 써준다. at NewKeyFile
                                If InStr(Tmp_Txt, "<![CDATA[") >= 1 Then
                                    Tmp_Txt = fileNum.ReadLine ' |	 ID 	 X 	 Y 	 Z 	 |
                                    Tmp_Txt = fileNum.ReadLine ' 수의 시작
                                    NewKeyFile.WriteLine("*NODE")
                                    Do While Not fileNum.EndOfStream
                                        If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                                        '숫자를 나눔
                                        WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 16) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 16) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 16)
                                        NewKeyFile.WriteLine(WritingText)

                                        Tmp_Txt = fileNum.ReadLine
                                    Loop

                                    '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                                    EndLine = Tmp_Txt.Split("]]>")
                                    If EndLine(0) = "" Then
                                    Else
                                        If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                                            '숫자를 나눔
                                            WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 16) & _
                                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 16) & _
                                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 16)
                                            NewKeyFile.WriteLine(WritingText)
                                        End If
                                    End If

                                End If

                            ElseIf InStr(Tmp_Txt, "ELEMENT.TRIAD3") >= 1 And (Me.ChkRefELE.Checked = False) Then 'ELEMENT.TRIAD3 ======================================
                                Tmp_Txt = fileNum.ReadLine
                                Tmp_Txt = fileNum.ReadLine
                                '테이블의 종류별로 써준다. at NewKeyFile
                                If InStr(Tmp_Txt, "<![CDATA[") >= 1 Then
                                    Tmp_Txt = fileNum.ReadLine ' |	 ID 	 X 	 Y 	 Z 	 |
                                    Tmp_Txt = fileNum.ReadLine ' 수의 시작
                                    NewKeyFile.WriteLine("*ELEMENT_SHELL")
                                    Do While Not fileNum.EndOfStream
                                        If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                                        '숫자를 나눔
                                        WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(4), 8)
                                        NewKeyFile.WriteLine(WritingText)

                                        Tmp_Txt = fileNum.ReadLine
                                    Loop

                                    '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                                    EndLine = Tmp_Txt.Split("]]>")
                                    If EndLine(0) = "" Then
                                    Else
                                        If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                                            '숫자를 나눔
                                            WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(4), 8)
                                            NewKeyFile.WriteLine(WritingText)
                                        End If
                                    End If

                                End If

                            ElseIf InStr(Tmp_Txt, "ELEMENT_REF.TRIAD3") >= 1 And (Me.ChkRefELE.Checked = True) Then 'ELEMENT_REF.TRIAD3 ======================================
                                Tmp_Txt = fileNum.ReadLine
                                Tmp_Txt = fileNum.ReadLine
                                '테이블의 종류별로 써준다. at NewKeyFile
                                If InStr(Tmp_Txt, "<![CDATA[") >= 1 Then
                                    Tmp_Txt = fileNum.ReadLine ' |	 ID 	 X 	 Y 	 Z 	 |
                                    Tmp_Txt = fileNum.ReadLine ' 수의 시작
                                    NewKeyFile.WriteLine("*ELEMENT_SHELL")
                                    Do While Not fileNum.EndOfStream
                                        If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                                        '숫자를 나눔
                                        WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(4), 8)
                                        NewKeyFile.WriteLine(WritingText)

                                        Tmp_Txt = fileNum.ReadLine
                                    Loop

                                    '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                                    EndLine = Tmp_Txt.Split("]]>")
                                    If EndLine(0) = "" Then
                                    Else
                                        If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                                            '숫자를 나눔
                                            WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(4), 8)
                                            NewKeyFile.WriteLine(WritingText)
                                        End If
                                    End If

                                End If

                            ElseIf InStr(Tmp_Txt, "ELEMENT.QUAD4") >= 1 And (Me.ChkRefELE.Checked = False) Then 'ELEMENT.QUAD4 ======================================
                                Tmp_Txt = fileNum.ReadLine
                                Tmp_Txt = fileNum.ReadLine
                                If InStr(Tmp_Txt, "<![CDATA[") >= 1 Then
                                    Tmp_Txt = fileNum.ReadLine ' |	 ID 	 X 	 Y 	 Z 	 |
                                    Tmp_Txt = fileNum.ReadLine ' 수의 시작
                                    NewKeyFile.WriteLine("*ELEMENT_SHELL")
                                    Do While Not fileNum.EndOfStream
                                        If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                                        '숫자를 나눔
                                        WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(4), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(5), 8)
                                        NewKeyFile.WriteLine(WritingText)

                                        Tmp_Txt = fileNum.ReadLine
                                    Loop

                                    '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                                    EndLine = Tmp_Txt.Split("]]>")
                                    If EndLine(0) = "" Then
                                    Else
                                        If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                                            '숫자를 나눔
                                            WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(4), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(5), 8)
                                            NewKeyFile.WriteLine(WritingText)
                                        End If
                                    End If

                                End If

                            ElseIf InStr(Tmp_Txt, "ELEMENT_REF.QUAD4") >= 1 And (Me.ChkRefELE.Checked = True) Then 'ELEMENT_REF.QUAD4 ======================================
                                Tmp_Txt = fileNum.ReadLine
                                Tmp_Txt = fileNum.ReadLine
                                If InStr(Tmp_Txt, "<![CDATA[") >= 1 Then
                                    Tmp_Txt = fileNum.ReadLine ' |	 ID 	 X 	 Y 	 Z 	 |
                                    Tmp_Txt = fileNum.ReadLine ' 수의 시작
                                    NewKeyFile.WriteLine("*ELEMENT_SHELL")
                                    Do While Not fileNum.EndOfStream
                                        If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                                        '숫자를 나눔
                                        WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(4), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(5), 8)
                                        NewKeyFile.WriteLine(WritingText)

                                        Tmp_Txt = fileNum.ReadLine
                                    Loop

                                    '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                                    EndLine = Tmp_Txt.Split("]]>")
                                    If EndLine(0) = "" Then
                                    Else
                                        If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                                            '숫자를 나눔
                                            WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(4), 8) & _
                                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(5), 8)
                                            NewKeyFile.WriteLine(WritingText)
                                        End If
                                    End If

                                End If

                            ElseIf InStr(Tmp_Txt, "<PART") >= 1 Then 'PART ============================================================================

                                Dim PID As String = ""
                                Dim Pname As String = ""
                                NewKeyFile.WriteLine("*PART")
                                Do While (1)
                                    Tmp_Txt = fileNum.ReadLine

                                    If (InStr(Tmp_Txt, "/>") >= 1) Then Exit Do

                                    If (InStr(Tmp_Txt, "  >") >= 1) Then
                                        Tmp_Txt = fileNum.ReadLine
                                        If InStr(Tmp_Txt, "COMMENT") >= 1 Then Exit Do
                                    End If

                                    If Tmp_Txt = "" Then MsgBox("")

                                    If AttriGet(Tmp_Txt, 1)(0) = "ID" Then
                                        PID = AttriGet(Tmp_Txt, 1)(1)
                                    ElseIf AttriGet(Tmp_Txt, 1)(0) = "NAME" Then
                                        Pname = AttriGet(Tmp_Txt, 1)(1)
                                    End If
                                Loop
                                NewKeyFile.WriteLine(Pname)
                                NewKeyFile.WriteLine(Add_10_Letters(PID))

                            ElseIf InStr(Tmp_Txt, "<RIGID_ELEMENT") >= 1 Then 'RIGID ELEMENT ============================================================================

                                Dim RID As String = ""
                                Dim GID As String = ""
                                Dim NodeList As String = ""
                                Dim RigidDescription As String = ""
                                Dim RigidName As String = ""

                                Do While (1)

                                    Tmp_Txt = fileNum.ReadLine

                                    If (InStr(Tmp_Txt, "/>") >= 1) Then Exit Do

                                    ' If Tmp_Txt = "" Then MsgBox("")

                                    If AttriGet(Tmp_Txt, 1)(0) = "ID" Then
                                        RID = AttriGet(Tmp_Txt, 1)(1)
                                    ElseIf AttriGet(Tmp_Txt, 1)(0) = "GROUP_LIST" Then
                                        GID = AttriGet(Tmp_Txt, 1)(1)
                                    ElseIf AttriGet(Tmp_Txt, 1)(0) = "NODE_LIST" Then
                                        NodeList = AttriGet(Tmp_Txt, 1)(1)
                                    ElseIf AttriGet(Tmp_Txt, 1)(0) = "DESCRIPTION" Then
                                        RigidDescription = AttriGet(Tmp_Txt, 1)(1)
                                    ElseIf AttriGet(Tmp_Txt, 1)(0) = "NAME" Then
                                        RigidName = AttriGet(Tmp_Txt, 1)(1)
                                    End If

                                Loop

                                If GID <> "" Then '단순 그룹 ID 만 있을 경우

                                    NewKeyFile.WriteLine("*CONSTRAINED_NODAL_RIGID_BODY")
                                    NewKeyFile.WriteLine(Add_10_Letters(GID) & "                   0                             0         0")

                                ElseIf NodeList <> "" Then 'NODE_LIST로 정의된 경우

                                    Dim NODEs As String()
                                    Dim SETLine As String = ""

                                    NewKeyFile.WriteLine("*CONSTRAINED_NODAL_RIGID_BODY")
                                    NewKeyFile.WriteLine(Add_10_Letters(RID) & "                   0                             0         0")
                                    NewKeyFile.WriteLine("*SET_NODE_LIST")
                                    NewKeyFile.WriteLine("$HMSET")
                                    If RigidDescription <> "" Then
                                        NewKeyFile.WriteLine("$HMNAME SETS" & RID & RigidDescription)
                                    ElseIf RigidName <> "" Then
                                        NewKeyFile.WriteLine("$HMNAME SETS" & RID & RigidName)
                                    ElseIf (RigidName = "") And (RigidDescription = "") Then
                                        NewKeyFile.WriteLine("$HMNAME SETS" & RID & "NoNameRigid")
                                    End If
                                    NewKeyFile.WriteLine(Add_10_Letters(RID) & "       0.0       0.0       0.0       0.0")

                                    NODEs = SplitTableLine(NodeList)
                                    For i = 1 To UBound(NODEs) + 1 Step 8
                                        For j = 0 To 7
                                            If i + j <= NODEs.Count Then
                                                SETLine = SETLine & Add_10_Letters(NODEs(i + j - 1))
                                            Else
                                                Exit For
                                            End If
                                        Next
                                        NewKeyFile.WriteLine(SETLine)
                                        SETLine = ""
                                    Next

                                End If

                            ElseIf InStr(Tmp_Txt, "<GROUP_FE") >= 1 Then 'GROUP FE ============================================================================

                                Dim Gname_Descrip As String = ""
                                Dim Gname_NAME As String = ""
                                Dim GID As String = ""
                                Dim NodeList As String = ""
                                Dim NODEs As String()
                                Dim PARTs As String()
                                Dim IsNodes As Boolean = True

                                Do While (1)
                                    Tmp_Txt = fileNum.ReadLine

                                    ' If (InStr(Tmp_Txt, ">") >= 1) Then MsgBox("")
                                    'If Tmp_Txt = "" Then MsgBox("")
                                    If (InStr(Tmp_Txt, "/>") >= 1) Then Exit Do
                                    If AttriGet(Tmp_Txt, 1)(0) = "ID" Then
                                        GID = AttriGet(Tmp_Txt, 1)(1)
                                    ElseIf AttriGet(Tmp_Txt, 1)(0) = "DESCRIPTION" Then
                                        Gname_Descrip = AttriGet(Tmp_Txt, 1)(1)
                                    ElseIf AttriGet(Tmp_Txt, 1)(0) = "NAME" Then
                                        Gname_NAME = AttriGet(Tmp_Txt, 1)(1)
                                    ElseIf AttriGet(Tmp_Txt, 1)(0) = "NODE_LIST" Then
                                        IsNodes = True
                                        NodeList = AttriGet(Tmp_Txt, 1)(1)
                                    ElseIf AttriGet(Tmp_Txt, 1)(0) = "PART_LIST" Then
                                        IsNodes = False
                                        NodeList = AttriGet(Tmp_Txt, 1)(1)
                                    End If
                                Loop

                                If IsNodes = True Then

                                    'NODE_LIST ===============================================================================
                                    NewKeyFile.WriteLine("*SET_NODE_LIST")
                                    NewKeyFile.WriteLine("$HMSET")
                                    NewKeyFile.WriteLine("$HMNAME SETS" & GID & Gname_Descrip)
                                    NewKeyFile.WriteLine(Add_10_Letters(GID) & "       0.0       0.0       0.0       0.0")

                                    If Not NodeList = "" Then
                                        Dim SETLine As String = ""
                                        NODEs = SplitTableLine(NodeList)
                                        For i = 1 To UBound(NODEs) + 1 Step 8
                                            For j = 0 To 7
                                                If i + j <= NODEs.Count Then
                                                    SETLine = SETLine & Add_10_Letters(NODEs(i + j - 1))
                                                Else
                                                    Exit For
                                                End If
                                            Next
                                            NewKeyFile.WriteLine(SETLine)
                                            SETLine = ""
                                        Next
                                    End If
                                    '=========================================================================================

                                ElseIf IsNodes = False Then

                                    'PART_LIST ===============================================================================
                                    If Not NodeList = "" Then
                                        Dim SETLine As String = ""
                                        PARTs = SplitTableLine(NodeList)

                                        Select Case IsNumeric(CInt(PARTs(0)))
                                            Case True
                                                NewKeyFile.WriteLine("*SET_PART_LIST")
                                                NewKeyFile.WriteLine("$HMSET")
                                                NewKeyFile.WriteLine("$HMNAME SETS" & Add_Dyna_Letters(GID, 8) & Gname_NAME)
                                                NewKeyFile.WriteLine(Add_10_Letters(GID))

                                                For i = 1 To UBound(PARTs) + 1 Step 8
                                                    For j = 0 To 7
                                                        If i + j <= PARTs.Count Then
                                                            SETLine = SETLine & Add_10_Letters(PARTs(i + j - 1))
                                                        Else
                                                            Exit For
                                                        End If
                                                    Next
                                                    NewKeyFile.WriteLine(SETLine)
                                                    SETLine = ""
                                                Next
                                            Case False
                                                '아무것도 하지않고 나감
                                                '(파트 이름으로 기록된 그룹은 MADYMO에서만 유효한 PART 그룹임
                                        End Select
                                    End If
                                    '=========================================================================================

                                End If

                            ElseIf InStr(Tmp_Txt, "<INCLUDE") >= 1 Then ' Include 포함 시
                                ' WorkingPath가 해당 xml 파일이 위치한 폴더임
                                ' 현재 쓰고있는 파일 'NewKeyFile과 XML Spy 인지 XMAgic인지 보내야 함
                                Tmp_Txt = fileNum.ReadLine
                                'XML SPY인지 파악한다.
                                Dim XMLspy As Boolean 'false : XML SPY / true : XMAgic
                                Dim XMLindicateString As String = ""
                                Dim TmpInclude As StreamReader
                                Dim ReadXMLinclude As New FileStream(WorkingPath & FindXMLattributeByName("FILE", Tmp_Txt), FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                                TmpInclude = New StreamReader(ReadXMLinclude)

                                XMLindicateString = TmpInclude.ReadLine
                                XMLindicateString = TmpInclude.ReadLine

                                If InStr(XMLindicateString, "XMLSPY") >= 1 Then
                                    XMLspy = False
                                Else
                                    XMLspy = True
                                End If
                                TmpInclude.Close()

                                ParsingINCLUDE(WorkingPath & FindXMLattributeByName("FILE", Tmp_Txt), NewKeyFile, XMLspy)
                            End If

                        Loop

                        NewKeyFile.WriteLine("*END")
                        NewKeyFile.Close()

                        Me.StatusLbl.Text = "Selected FE_MODEL Converting End..."
                        Application.DoEvents()

                    End If

                End If
            Loop

        Else
            'XML SPY Format ================================================================================================ XML SPY Format
            Do While Not fileNum.EndOfStream
                Tmp_Txt = fileNum.ReadLine

                'XML SPY와 XMAgic 동일
                'Disable 건너뛰기
                If InStr(Tmp_Txt, "<DISABLE>") >= 1 Then

                    Me.StatusLbl.Text = "Skip Disabled.."
                    Application.DoEvents()

                    DisabledCnt = DisabledCnt + 1
                    Do While Not fileNum.EndOfStream
                        Tmp_Txt = fileNum.ReadLine
                        If (InStr(Tmp_Txt, "</DISABLE>") >= 1) Then
                            DisabledCnt = DisabledCnt - 1
                            If DisabledCnt = 0 Then Exit Do
                        ElseIf InStr(Tmp_Txt, "<DISABLE>") >= 1 Then
                            DisabledCnt = DisabledCnt + 1
                        End If
                    Loop
                End If

                If (InStr(Tmp_Txt, "<SYSTEM.MODEL") > 1) Then

                    Me.StatusLbl.Text = "Analyzing SYSTEM.MODEL"
                    Application.DoEvents()

                    If InStr(Tmp_Txt, "NAME=") > 1 Then
                        TmpSystemName = FindXMLattributeByName("NAME", Tmp_Txt)
                    End If
                End If

                If (InStr(Tmp_Txt, "<FE_MODEL") > 1) Then

                    Me.StatusLbl.Text = "Analyzing FE_MODEL"
                    Application.DoEvents()

                    'XML SPY 형식
                    If InStr(Tmp_Txt, "NAME=") > 1 Then
                        TmpFEmodelName = FindXMLattributeByName("NAME", Tmp_Txt)
                    End If
                    'Tmp_Txt = fileNum.ReadLine

                    '선택한 FE MODEL이 맞는지 확인후 작업 시작
                    If (SystemName = TmpSystemName) And (FEmodelName = TmpFEmodelName) Then

                        Me.StatusLbl.Text = "Selected FE_MODEL Converting Start..."
                        Application.DoEvents()

                        Do While Not fileNum.EndOfStream

                            '변환작업
                            Tmp_Txt = fileNum.ReadLine

                            If InStr(Tmp_Txt, "</FE_MODEL>") >= 1 Then Exit Do

                            If InStr(Tmp_Txt, "COORDINATE.CARTESIAN") >= 1 And (Me.ChkRefCoord.Checked = False) Then 'COORDINATE.CARTESIAN
                                '테이블의 종류별로 써준다. at NewKeyFile
                                Tmp_Txt = fileNum.ReadLine ' |	 ID 	 X 	 Y 	 Z 	 |
                                Tmp_Txt = fileNum.ReadLine ' 수의 시작
                                NewKeyFile.WriteLine("*NODE")
                                Do While Not fileNum.EndOfStream
                                    If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                                    '숫자를 나눔
                                    WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 16) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 16) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 16)
                                    NewKeyFile.WriteLine(WritingText)

                                    Tmp_Txt = fileNum.ReadLine
                                Loop

                                '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                                EndLine = Tmp_Txt.Split("]]>")
                                If EndLine(0) = "" Then
                                Else
                                    If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                                        '숫자를 나눔
                                        WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 16) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 16) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 16)
                                        NewKeyFile.WriteLine(WritingText)
                                    End If
                                End If

                            ElseIf InStr(Tmp_Txt, "COORDINATE_REF.CARTESIAN") >= 1 And (Me.ChkRefCoord.Checked = True) Then 'COORDINATE_REF.CARTESIAN
                                '테이블의 종류별로 써준다. at NewKeyFile
                                Tmp_Txt = fileNum.ReadLine ' |	 ID 	 X 	 Y 	 Z 	 |
                                Tmp_Txt = fileNum.ReadLine ' 수의 시작
                                NewKeyFile.WriteLine("*NODE")
                                Do While Not fileNum.EndOfStream
                                    If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                                    '숫자를 나눔
                                    WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 16) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 16) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 16)
                                    NewKeyFile.WriteLine(WritingText)

                                    Tmp_Txt = fileNum.ReadLine
                                Loop

                                '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                                EndLine = Tmp_Txt.Split("]]>")
                                If EndLine(0) = "" Then
                                Else
                                    If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                                        '숫자를 나눔
                                        WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 16) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 16) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 16)
                                        NewKeyFile.WriteLine(WritingText)
                                    End If
                                End If

                            ElseIf InStr(Tmp_Txt, "ELEMENT.TRIAD3") >= 1 And (Me.ChkRefELE.Checked = False) Then 'ELEMENT.TRIAD3
                                '테이블의 종류별로 써준다. at NewKeyFile
                                Tmp_Txt = fileNum.ReadLine ' |	 ID 	 PART 	 N1 	 N2 	N3	 |
                                Tmp_Txt = fileNum.ReadLine ' 수의 시작
                                NewKeyFile.WriteLine("*ELEMENT_SHELL")
                                Do While Not fileNum.EndOfStream
                                    If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                                    '숫자를 나눔
                                    WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(4), 8)
                                    NewKeyFile.WriteLine(WritingText)

                                    Tmp_Txt = fileNum.ReadLine
                                Loop

                                '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                                EndLine = Tmp_Txt.Split("]]>")
                                If EndLine(0) = "" Then
                                Else
                                    If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                                        '숫자를 나눔
                                        WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(4), 8)
                                        NewKeyFile.WriteLine(WritingText)
                                    End If
                                End If

                            ElseIf InStr(Tmp_Txt, "ELEMENT_REF.TRIAD3") >= 1 And (Me.ChkRefELE.Checked = True) Then 'ELEMENT_REF.TRIAD3
                                '테이블의 종류별로 써준다. at NewKeyFile
                                Tmp_Txt = fileNum.ReadLine ' |	 ID 	 PART 	 N1 	 N2 	N3	 |
                                Tmp_Txt = fileNum.ReadLine ' 수의 시작
                                NewKeyFile.WriteLine("*ELEMENT_SHELL")
                                Do While Not fileNum.EndOfStream
                                    If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                                    '숫자를 나눔
                                    WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(4), 8)
                                    NewKeyFile.WriteLine(WritingText)

                                    Tmp_Txt = fileNum.ReadLine
                                Loop

                                '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                                EndLine = Tmp_Txt.Split("]]>")
                                If EndLine(0) = "" Then
                                Else
                                    If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                                        '숫자를 나눔
                                        WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(4), 8)
                                        NewKeyFile.WriteLine(WritingText)
                                    End If
                                End If

                            ElseIf InStr(Tmp_Txt, "ELEMENT.QUAD4") >= 1 And (Me.ChkRefELE.Checked = False) Then 'ELEMENT.QUAD4

                                Tmp_Txt = fileNum.ReadLine ' |	 ID 	 PART 	 N1 	 N2 	N3	N4	 |
                                Tmp_Txt = fileNum.ReadLine ' 수의 시작
                                NewKeyFile.WriteLine("*ELEMENT_SHELL")
                                Do While Not fileNum.EndOfStream
                                    If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                                    '숫자를 나눔
                                    WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(4), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(5), 8)
                                    NewKeyFile.WriteLine(WritingText)

                                    Tmp_Txt = fileNum.ReadLine
                                Loop

                                '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                                EndLine = Tmp_Txt.Split("]]>")
                                If EndLine(0) = "" Then
                                Else
                                    If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                                        '숫자를 나눔
                                        WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(4), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(5), 8)
                                        NewKeyFile.WriteLine(WritingText)
                                    End If
                                End If

                            ElseIf InStr(Tmp_Txt, "ELEMENT_REF.QUAD4") >= 1 And (Me.ChkRefELE.Checked = True) Then 'ELEMENT_REF.QUAD4

                                Tmp_Txt = fileNum.ReadLine ' |	 ID 	 PART 	 N1 	 N2 	N3	N4	 |
                                Tmp_Txt = fileNum.ReadLine ' 수의 시작
                                NewKeyFile.WriteLine("*ELEMENT_SHELL")
                                Do While Not fileNum.EndOfStream
                                    If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                                    '숫자를 나눔
                                    WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(4), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(5), 8)
                                    NewKeyFile.WriteLine(WritingText)

                                    Tmp_Txt = fileNum.ReadLine
                                Loop

                                '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                                EndLine = Tmp_Txt.Split("]]>")
                                If EndLine(0) = "" Then
                                Else
                                    If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                                        '숫자를 나눔
                                        WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(4), 8) & _
                                        Add_Dyna_Letters(SplitTableLine(EndLine(0))(5), 8)
                                        NewKeyFile.WriteLine(WritingText)
                                    End If
                                End If

                            ElseIf InStr(Tmp_Txt, "<PART") >= 1 Then

                                Dim PID As String = ""
                                Dim Pname As String = ""
                                NewKeyFile.WriteLine("*PART")
                                If InStr(Tmp_Txt, "NAME") >= 1 Then
                                    NewKeyFile.WriteLine(FindXMLattributeByName("NAME", Tmp_Txt))
                                Else
                                    NewKeyFile.WriteLine(FindXMLattributeByName("DESCRIPTION", Tmp_Txt))
                                End If
                                NewKeyFile.WriteLine(Add_10_Letters(FindXMLattributeByName("ID", Tmp_Txt)))

                            ElseIf InStr(Tmp_Txt, "<RIGID_ELEMENT") >= 1 Then 'RIGID ELEMENT ============================================================================

                                Dim RID As String = ""
                                Dim GID As String = ""

                                RID = FindXMLattributeByName("ID", Tmp_Txt)
                                If InStr(Tmp_Txt, "GROUP_LIST") >= 1 Then
                                    GID = FindXMLattributeByName("GROUP_LIST", Tmp_Txt)
                                    NewKeyFile.WriteLine("*CONSTRAINED_NODAL_RIGID_BODY")
                                    NewKeyFile.WriteLine(Add_10_Letters(GID) & "                   0                             0         0")
                                Else
                                    If InStr(Tmp_Txt, "NODE_LIST") Then

                                        Dim NodeList As String = ""
                                        Dim NODEs As String()

                                        NewKeyFile.WriteLine("*CONSTRAINED_NODAL_RIGID_BODY")
                                        NewKeyFile.WriteLine(Add_10_Letters(RID) & "                   0                             0         0")
                                        NewKeyFile.WriteLine("*SET_NODE_LIST")
                                        NewKeyFile.WriteLine("$HMSET")
                                        If FindXMLattributeByName("DESCRIPTION", Tmp_Txt) <> "" Then
                                            NewKeyFile.WriteLine("$HMNAME SETS" & RID & FindXMLattributeByName("DESCRIPTION", Tmp_Txt))
                                        Else
                                            NewKeyFile.WriteLine("$HMNAME SETS" & RID & FindXMLattributeByName("NAME", Tmp_Txt))
                                        End If
                                        NewKeyFile.WriteLine(Add_10_Letters(RID) & "       0.0       0.0       0.0       0.0")

                                        NodeList = FindXMLattributeByName("NODE_LIST", Tmp_Txt)

                                        If Not NodeList = "" Then
                                            Dim SETLine As String = ""
                                            NODEs = SplitTableLine(NodeList)
                                            For i = 1 To UBound(NODEs) + 1 Step 8
                                                For j = 0 To 7
                                                    If i + j <= NODEs.Count Then
                                                        SETLine = SETLine & Add_10_Letters(NODEs(i + j - 1))
                                                    Else
                                                        Exit For
                                                    End If
                                                Next
                                                NewKeyFile.WriteLine(SETLine)
                                                SETLine = ""
                                            Next
                                        End If

                                    ElseIf InStr(Tmp_Txt, "ELEMENT_LIST") Then

                                        'DYNA에서 Rigid 형태의 Element 대응 되는게 없으므로 패스함

                                    End If

                                End If

                            

                            ElseIf InStr(Tmp_Txt, "<GROUP_FE") >= 1 Then 'GROUP FE ============================================================================

                                Dim Gname_Descrip As String = ""
                                Dim Gname_NAME As String = ""
                                Dim GID As String = ""
                                Dim NodeList As String = ""
                                Dim PartList As String = ""
                                Dim NODEs As String()
                                Dim PARTs As String()
                                Dim IsNodes As Boolean = True

                                GID = FindXMLattributeByName("ID", Tmp_Txt)
                                Gname_Descrip = FindXMLattributeByName("DESCRIPTION", Tmp_Txt)
                                Gname_NAME = FindXMLattributeByName("NAME", Tmp_Txt)

                                If InStr(Tmp_Txt, "NODE_LIST") >= 1 Then
                                    IsNodes = True
                                    NodeList = FindXMLattributeByName("NODE_LIST", Tmp_Txt)
                                Else
                                    IsNodes = False
                                    PartList = FindXMLattributeByName("PART_LIST", Tmp_Txt)
                                End If

                                If IsNodes = True Then

                                    'NODE_LIST ===============================================================================
                                    NewKeyFile.WriteLine("*SET_NODE_LIST")
                                    NewKeyFile.WriteLine("$HMSET")
                                    NewKeyFile.WriteLine("$HMNAME SETS" & GID & Gname_Descrip)
                                    NewKeyFile.WriteLine(Add_10_Letters(GID) & "       0.0       0.0       0.0       0.0")

                                    If Not NodeList = "" Then
                                        Dim SETLine As String = ""
                                        NODEs = SplitTableLine(NodeList)
                                        For i = 1 To UBound(NODEs) + 1 Step 8
                                            For j = 0 To 7
                                                If i + j <= NODEs.Count Then
                                                    SETLine = SETLine & Add_10_Letters(NODEs(i + j - 1))
                                                Else
                                                    Exit For
                                                End If
                                            Next
                                            NewKeyFile.WriteLine(SETLine)
                                            SETLine = ""
                                        Next
                                    End If
                                    '=========================================================================================

                                ElseIf IsNodes = False Then

                                    'PART_LIST ===============================================================================
                                    If Not PartList = "" Then
                                        Dim SETLine As String = ""
                                        PARTs = SplitTableLine(PartList)

                                        Select Case IsNumeric(CInt(PARTs(0)))
                                            Case True
                                                NewKeyFile.WriteLine("*SET_PART_LIST")
                                                NewKeyFile.WriteLine("$HMSET")
                                                NewKeyFile.WriteLine("$HMNAME SETS" & Add_Dyna_Letters(GID, 8) & Gname_NAME)
                                                NewKeyFile.WriteLine(Add_10_Letters(GID))

                                                For i = 1 To UBound(PARTs) + 1 Step 8
                                                    For j = 0 To 7
                                                        If i + j <= PARTs.Count Then
                                                            SETLine = SETLine & Add_10_Letters(PARTs(i + j - 1))
                                                        Else
                                                            Exit For
                                                        End If
                                                    Next
                                                    NewKeyFile.WriteLine(SETLine)
                                                    SETLine = ""
                                                Next
                                            Case False
                                                '아무것도 하지않고 나감
                                                '(파트 이름으로 기록된 그룹은 MADYMO에서만 유효한 PART 그룹임
                                        End Select
                                    End If
                                    '=========================================================================================

                                End If

                            ElseIf InStr(Tmp_Txt, "<INCLUDE FILE=") >= 1 Then 'Include 파일이 있는 경우
                                ' WorkingPath가 해당 xml 파일이 위치한 폴더임
                                ' 현재 쓰고있는 파일 'NewKeyFile과 XML Spy 인지 XMAgic인지 보내야 함
                                'XML SPY인지 파악한다.
                                Dim XMLspy As Boolean 'false : XML SPY / true : XMAgic
                                Dim XMLindicateString As String = ""
                                Dim TmpInclude As StreamReader
                                Dim ReadXMLinclude As New FileStream(WorkingPath & FindXMLattributeByName("FILE", Tmp_Txt), FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                                TmpInclude = New StreamReader(ReadXMLinclude)

                                XMLindicateString = TmpInclude.ReadLine
                                XMLindicateString = TmpInclude.ReadLine

                                If InStr(XMLindicateString, "XMLSPY") >= 1 Then
                                    XMLspy = False
                                Else
                                    XMLspy = True
                                End If
                                TmpInclude.Close()

                                ParsingINCLUDE(WorkingPath & FindXMLattributeByName("FILE", Tmp_Txt), NewKeyFile, XMLspy)
                            End If
                        Loop

                        NewKeyFile.WriteLine("*END")
                        NewKeyFile.Close()


                        Me.StatusLbl.Text = "Selected FE_MODEL Converting End..."
                        Application.DoEvents()

                    End If
                End If
            Loop
            'XML SPY Format =============================================================================================
        End If

        Me.StatusLbl.Text = "Complete"

    End Sub

    Private Sub FrmFEconvertingInXML_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If TempLicExpire = False Then
            '원격로그
            Try
                Using NewfileNum As StreamWriter = File.AppendText(RemoteValFolder & System.Environment.UserName)
                    NewfileNum.WriteLine("## Madymo to LS-Dyna : " & Date.Now.ToString & " ##")
                    NewfileNum.Close()
                End Using
            Catch ex As Exception
                'End
            End Try
        End If

        Me.Size = New Point(500, 800)
        Me.MdiParent = MainMDI
    End Sub

    Private Sub FrmFEconvertingInXML_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        With Me
            .DyNameTxt.Location = New Point(5, 10)
            .DyNameTxt.Size = New Size(.ClientRectangle.Width - 10, 25)
            .DyOpnBtn.Location = New Point(.DyNameTxt.Width - 120 + 5, .DyNameTxt.Location.Y + DyNameTxt.Size.Height + 5)
            .DyOpnBtn.Size = New Size(120, 25)

            .LstFEsystem.Location = New Point(5, .DyOpnBtn.Location.Y + .DyOpnBtn.Height + 5)
            .LstFEsystem.Size = New Size(.DyNameTxt.Width, .ClientRectangle.Height - .DyNameTxt.Height - .DyOpnBtn.Height - .ChkRefCoord.Height - .ChkRefELE.Height - .Label1.Height - 35 - 25)

            .ChkRefCoord.Location = New Point(5, .LstFEsystem.Location.Y + .LstFEsystem.Height + 5)
            .ChkRefELE.Location = New Point(5, .ChkRefCoord.Location.Y + .ChkRefCoord.Height + 5)
            .BtnConvert.Size = New Size(.DyOpnBtn.Width, 34)
            .BtnConvert.Location = New Point(.DyOpnBtn.Location.X, .ChkRefCoord.Location.Y)
            .Label1.Location = New Point(.ChkRefELE.Location.X + 5, .ChkRefELE.Location.Y + .ChkRefELE.Height + 5)

        End With
    End Sub

    Private Sub ParsingINCLUDE(ByVal FileFullPath As String, ByRef KeyFileNum As System.IO.StreamWriter, ByVal IsXML As Boolean)
        Dim IncludeFileNum As StreamReader
        Dim ReadXML As New FileStream(FileFullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        IncludeFileNum = New StreamReader(ReadXML)
        Dim Tmp_Txt As String
        Dim DisabledCnt As Integer = 0
        Dim WritingText As String = ""
        Dim EndLine() As String

        If IsXML = False Then '========================================== XML SPY Format

            Do While Not IncludeFileNum.EndOfStream

                Tmp_Txt = IncludeFileNum.ReadLine

                'XML SPY와 XMAgic 동일
                'Disable 건너뛰기
                If InStr(Tmp_Txt, "<DISABLE>") >= 1 Then

                    Me.StatusLbl.Text = "Skip Disabled.."
                    Application.DoEvents()

                    DisabledCnt = DisabledCnt + 1
                    Do While Not IncludeFileNum.EndOfStream
                        Tmp_Txt = IncludeFileNum.ReadLine
                        If (InStr(Tmp_Txt, "</DISABLE>") >= 1) Then
                            DisabledCnt = DisabledCnt - 1
                            If DisabledCnt = 0 Then Exit Do
                        ElseIf InStr(Tmp_Txt, "<DISABLE>") >= 1 Then
                            DisabledCnt = DisabledCnt + 1
                        End If
                    Loop
                End If

                'Parsing*******************************************************************



                If InStr(Tmp_Txt, "COORDINATE.CARTESIAN") >= 1 And (Me.ChkRefCoord.Checked = False) Then 'COORDINATE.CARTESIAN
                    Me.StatusLbl.Text = "Converting : COORDINATE.CARTESIAN"
                    Application.DoEvents()
                    '테이블의 종류별로 써준다. at KeyFileNum
                    Tmp_Txt = IncludeFileNum.ReadLine ' |	 ID 	 X 	 Y 	 Z 	 |
                    Tmp_Txt = IncludeFileNum.ReadLine ' 수의 시작
                    KeyFileNum.WriteLine("*NODE")
                    Do While Not IncludeFileNum.EndOfStream
                        If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                        '숫자를 나눔
                        WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 16) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 16) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 16)
                        KeyFileNum.WriteLine(WritingText)

                        Tmp_Txt = IncludeFileNum.ReadLine
                    Loop

                    '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                    EndLine = Tmp_Txt.Split("]]>")
                    If EndLine(0) = "" Then
                    Else
                        If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                            '숫자를 나눔
                            WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 16) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 16) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 16)
                            KeyFileNum.WriteLine(WritingText)
                        End If
                    End If

                ElseIf InStr(Tmp_Txt, "COORDINATE_REF.CARTESIAN") >= 1 And (Me.ChkRefCoord.Checked = True) Then 'COORDINATE_REF.CARTESIAN
                    Me.StatusLbl.Text = "Converting : COORDINATE_REF.CARTESIAN"
                    Application.DoEvents()
                    '테이블의 종류별로 써준다. at KeyFileNum
                    Tmp_Txt = IncludeFileNum.ReadLine ' |	 ID 	 X 	 Y 	 Z 	 |
                    Tmp_Txt = IncludeFileNum.ReadLine ' 수의 시작
                    KeyFileNum.WriteLine("*NODE")
                    Do While Not IncludeFileNum.EndOfStream
                        If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                        '숫자를 나눔
                        WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 16) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 16) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 16)
                        KeyFileNum.WriteLine(WritingText)

                        Tmp_Txt = IncludeFileNum.ReadLine
                    Loop

                    '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                    EndLine = Tmp_Txt.Split("]]>")
                    If EndLine(0) = "" Then
                    Else
                        If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                            '숫자를 나눔
                            WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 16) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 16) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 16)
                            KeyFileNum.WriteLine(WritingText)
                        End If
                    End If

                ElseIf InStr(Tmp_Txt, "ELEMENT.TRIAD3") >= 1 And (Me.ChkRefELE.Checked = False) Then 'ELEMENT.TRIAD3
                    Me.StatusLbl.Text = "Converting : ELEMENT.TRIAD3"
                    Application.DoEvents()
                    '테이블의 종류별로 써준다. at KeyFileNum
                    Tmp_Txt = IncludeFileNum.ReadLine ' |	 ID 	 PART 	 N1 	 N2 	N3	 |
                    Tmp_Txt = IncludeFileNum.ReadLine ' 수의 시작
                    KeyFileNum.WriteLine("*ELEMENT_SHELL")
                    Do While Not IncludeFileNum.EndOfStream
                        If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                        '숫자를 나눔
                        WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 8) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 8) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 8) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(4), 8)
                        KeyFileNum.WriteLine(WritingText)

                        Tmp_Txt = IncludeFileNum.ReadLine
                    Loop

                    '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                    EndLine = Tmp_Txt.Split("]]>")
                    If EndLine(0) = "" Then
                    Else
                        If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                            '숫자를 나눔
                            WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 8) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 8) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 8) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(4), 8)
                            KeyFileNum.WriteLine(WritingText)
                        End If
                    End If

                ElseIf InStr(Tmp_Txt, "ELEMENT_REF.TRIAD3") >= 1 And (Me.ChkRefELE.Checked = True) Then 'ELEMENT_REF.TRIAD3
                    Me.StatusLbl.Text = "Converting : ELEMENT_REF.TRIAD3"
                    Application.DoEvents()
                    '테이블의 종류별로 써준다. at KeyFileNum
                    Tmp_Txt = IncludeFileNum.ReadLine ' |	 ID 	 PART 	 N1 	 N2 	N3	 |
                    Tmp_Txt = IncludeFileNum.ReadLine ' 수의 시작
                    KeyFileNum.WriteLine("*ELEMENT_SHELL")
                    Do While Not IncludeFileNum.EndOfStream
                        If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                        '숫자를 나눔
                        WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 8) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 8) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 8) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(4), 8)
                        KeyFileNum.WriteLine(WritingText)

                        Tmp_Txt = IncludeFileNum.ReadLine
                    Loop

                    '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                    EndLine = Tmp_Txt.Split("]]>")
                    If EndLine(0) = "" Then
                    Else
                        If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                            '숫자를 나눔
                            WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 8) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 8) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 8) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(4), 8)
                            KeyFileNum.WriteLine(WritingText)
                        End If
                    End If

                ElseIf InStr(Tmp_Txt, "ELEMENT.QUAD4") >= 1 And (Me.ChkRefELE.Checked = False) Then 'ELEMENT.QUAD4
                    Me.StatusLbl.Text = "Converting : ELEMENT.QUAD4"
                    Application.DoEvents()

                    Tmp_Txt = IncludeFileNum.ReadLine ' |	 ID 	 PART 	 N1 	 N2 	N3	N4	 |
                    Tmp_Txt = IncludeFileNum.ReadLine ' 수의 시작
                    KeyFileNum.WriteLine("*ELEMENT_SHELL")
                    Do While Not IncludeFileNum.EndOfStream
                        If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                        '숫자를 나눔
                        WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 8) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 8) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 8) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(4), 8) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(5), 8)
                        KeyFileNum.WriteLine(WritingText)

                        Tmp_Txt = IncludeFileNum.ReadLine
                    Loop

                    '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                    EndLine = Tmp_Txt.Split("]]>")
                    If EndLine(0) = "" Then
                    Else
                        If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                            '숫자를 나눔
                            WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 8) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 8) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 8) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(4), 8) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(5), 8)
                            KeyFileNum.WriteLine(WritingText)
                        End If
                    End If

                ElseIf InStr(Tmp_Txt, "ELEMENT_REF.QUAD4") >= 1 And (Me.ChkRefELE.Checked = True) Then 'ELEMENT_REF.QUAD4
                    Me.StatusLbl.Text = "Converting : ELEMENT_REF.QUAD4"
                    Application.DoEvents()

                    Tmp_Txt = IncludeFileNum.ReadLine ' |	 ID 	 PART 	 N1 	 N2 	N3	N4	 |
                    Tmp_Txt = IncludeFileNum.ReadLine ' 수의 시작
                    KeyFileNum.WriteLine("*ELEMENT_SHELL")
                    Do While Not IncludeFileNum.EndOfStream
                        If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                        '숫자를 나눔
                        WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 8) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 8) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 8) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(4), 8) & _
                            Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(5), 8)
                        KeyFileNum.WriteLine(WritingText)

                        Tmp_Txt = IncludeFileNum.ReadLine
                    Loop

                    '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                    EndLine = Tmp_Txt.Split("]]>")
                    If EndLine(0) = "" Then
                    Else
                        If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                            '숫자를 나눔
                            WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 8) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 8) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 8) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(4), 8) & _
                            Add_Dyna_Letters(SplitTableLine(EndLine(0))(5), 8)
                            KeyFileNum.WriteLine(WritingText)
                        End If
                    End If

                ElseIf InStr(Tmp_Txt, "<PART") >= 1 Then
                    Me.StatusLbl.Text = "Converting : PART"
                    Application.DoEvents()

                    Dim PID As String = ""
                    Dim Pname As String = ""
                    KeyFileNum.WriteLine("*PART")
                    If InStr(Tmp_Txt, "NAME") >= 1 Then
                        KeyFileNum.WriteLine(FindXMLattributeByName("NAME", Tmp_Txt))
                    Else
                        KeyFileNum.WriteLine(FindXMLattributeByName("DESCRIPTION", Tmp_Txt))
                    End If
                    KeyFileNum.WriteLine(Add_10_Letters(FindXMLattributeByName("ID", Tmp_Txt)))

                ElseIf InStr(Tmp_Txt, "<RIGID_ELEMENT") >= 1 Then 'RIGID ELEMENT ============================================================================
                    Me.StatusLbl.Text = "Converting : RIGID_ELEMENT"
                    Application.DoEvents()

                    Dim RID As String = ""
                    Dim GID As String = ""

                    RID = FindXMLattributeByName("ID", Tmp_Txt)
                    If InStr(Tmp_Txt, "GROUP_LIST") >= 1 Then
                        GID = FindXMLattributeByName("GROUP_LIST", Tmp_Txt)
                        KeyFileNum.WriteLine("*CONSTRAINED_NODAL_RIGID_BODY")
                        KeyFileNum.WriteLine(Add_10_Letters(GID) & "                   0                             0         0")
                    Else
                        If InStr(Tmp_Txt, "NODE_LIST") Then

                            Dim NodeList As String = ""
                            Dim NODEs As String()

                            KeyFileNum.WriteLine("*CONSTRAINED_NODAL_RIGID_BODY")
                            KeyFileNum.WriteLine(Add_10_Letters(RID) & "                   0                             0         0")
                            KeyFileNum.WriteLine("*SET_NODE_LIST")
                            KeyFileNum.WriteLine("$HMSET")
                            If FindXMLattributeByName("DESCRIPTION", Tmp_Txt) <> "" Then
                                KeyFileNum.WriteLine("$HMNAME SETS" & RID & FindXMLattributeByName("DESCRIPTION", Tmp_Txt))
                            Else
                                KeyFileNum.WriteLine("$HMNAME SETS" & RID & FindXMLattributeByName("NAME", Tmp_Txt))
                            End If
                            KeyFileNum.WriteLine(Add_10_Letters(RID) & "       0.0       0.0       0.0       0.0")

                            NodeList = FindXMLattributeByName("NODE_LIST", Tmp_Txt)

                            If Not NodeList = "" Then
                                Dim SETLine As String = ""
                                NODEs = SplitTableLine(NodeList)
                                For i = 1 To UBound(NODEs) + 1 Step 8
                                    For j = 0 To 7
                                        If i + j <= NODEs.Count Then
                                            SETLine = SETLine & Add_10_Letters(NODEs(i + j - 1))
                                        Else
                                            Exit For
                                        End If
                                    Next
                                    KeyFileNum.WriteLine(SETLine)
                                    SETLine = ""
                                Next
                            End If

                        ElseIf InStr(Tmp_Txt, "ELEMENT_LIST") Then

                            'DYNA에서 Rigid 형태의 Element 대응 되는게 없으므로 패스함

                        End If

                    End If



                ElseIf InStr(Tmp_Txt, "<GROUP_FE") >= 1 Then 'GROUP FE ============================================================================
                    Me.StatusLbl.Text = "Converting : GROUP_FE"
                    Application.DoEvents()

                    Dim Gname_Descrip As String = ""
                    Dim Gname_NAME As String = ""
                    Dim GID As String = ""
                    Dim NodeList As String = ""
                    Dim PartList As String = ""
                    Dim NODEs As String()
                    Dim PARTs As String()
                    Dim IsNodes As Boolean = True

                    GID = FindXMLattributeByName("ID", Tmp_Txt)
                    Gname_Descrip = FindXMLattributeByName("DESCRIPTION", Tmp_Txt)
                    Gname_NAME = FindXMLattributeByName("NAME", Tmp_Txt)

                    If InStr(Tmp_Txt, "NODE_LIST") >= 1 Then
                        IsNodes = True
                        NodeList = FindXMLattributeByName("NODE_LIST", Tmp_Txt)
                    Else
                        IsNodes = False
                        PartList = FindXMLattributeByName("PART_LIST", Tmp_Txt)
                    End If

                    If IsNodes = True Then

                        'NODE_LIST ===============================================================================
                        KeyFileNum.WriteLine("*SET_NODE_LIST")
                        KeyFileNum.WriteLine("$HMSET")
                        KeyFileNum.WriteLine("$HMNAME SETS" & GID & Gname_Descrip)
                        KeyFileNum.WriteLine(Add_10_Letters(GID) & "       0.0       0.0       0.0       0.0")

                        If Not NodeList = "" Then
                            Dim SETLine As String = ""
                            NODEs = SplitTableLine(NodeList)
                            For i = 1 To UBound(NODEs) + 1 Step 8
                                For j = 0 To 7
                                    If i + j <= NODEs.Count Then
                                        SETLine = SETLine & Add_10_Letters(NODEs(i + j - 1))
                                    Else
                                        Exit For
                                    End If
                                Next
                                KeyFileNum.WriteLine(SETLine)
                                SETLine = ""
                            Next
                        End If
                        '=========================================================================================

                    ElseIf IsNodes = False Then

                        'PART_LIST ===============================================================================
                        If Not PartList = "" Then
                            Dim SETLine As String = ""
                            PARTs = SplitTableLine(PartList)

                            Select Case IsNumeric(CInt(PARTs(0)))
                                Case True
                                    KeyFileNum.WriteLine("*SET_PART_LIST")
                                    KeyFileNum.WriteLine("$HMSET")
                                    KeyFileNum.WriteLine("$HMNAME SETS" & Add_Dyna_Letters(GID, 8) & Gname_NAME)
                                    KeyFileNum.WriteLine(Add_10_Letters(GID))

                                    For i = 1 To UBound(PARTs) + 1 Step 8
                                        For j = 0 To 7
                                            If i + j <= PARTs.Count Then
                                                SETLine = SETLine & Add_10_Letters(PARTs(i + j - 1))
                                            Else
                                                Exit For
                                            End If
                                        Next
                                        KeyFileNum.WriteLine(SETLine)
                                        SETLine = ""
                                    Next
                                Case False
                                    '아무것도 하지않고 나감
                                    '(파트 이름으로 기록된 그룹은 MADYMO에서만 유효한 PART 그룹임
                            End Select
                        End If
                        '=========================================================================================

                    End If
                End If
                '**************************************************************************

            Loop

        ElseIf IsXML = True Then '========================================== XMAgic Format

            Do While Not IncludeFileNum.EndOfStream

                Tmp_Txt = IncludeFileNum.ReadLine

                'XML SPY와 XMAgic 동일
                'Disable 건너뛰기
                If InStr(Tmp_Txt, "<DISABLE>") >= 1 Then

                    Me.StatusLbl.Text = "Skip Disabled.."
                    Application.DoEvents()

                    DisabledCnt = DisabledCnt + 1
                    Do While Not IncludeFileNum.EndOfStream
                        Tmp_Txt = IncludeFileNum.ReadLine
                        If (InStr(Tmp_Txt, "</DISABLE>") >= 1) Then
                            DisabledCnt = DisabledCnt - 1
                            If DisabledCnt = 0 Then Exit Do
                        ElseIf InStr(Tmp_Txt, "<DISABLE>") >= 1 Then
                            DisabledCnt = DisabledCnt + 1
                        End If
                    Loop
                End If

                'Parsing*******************************************************************
                If InStr(Tmp_Txt, "COORDINATE.CARTESIAN") >= 1 And (Me.ChkRefCoord.Checked = False) Then 'COORDINATE.CARTESIAN ======================================
                    Me.StatusLbl.Text = "Converting : COORDINATE.CARTESIAN"
                    Application.DoEvents()

                    Tmp_Txt = IncludeFileNum.ReadLine
                    Tmp_Txt = IncludeFileNum.ReadLine
                    '테이블의 종류별로 써준다. at KeyFileNum
                    If InStr(Tmp_Txt, "<![CDATA[") >= 1 Then
                        Tmp_Txt = IncludeFileNum.ReadLine ' |	 ID 	 X 	 Y 	 Z 	 |
                        Tmp_Txt = IncludeFileNum.ReadLine ' 수의 시작
                        KeyFileNum.WriteLine("*NODE")
                        Do While Not IncludeFileNum.EndOfStream
                            If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                            '숫자를 나눔
                            WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 16) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 16) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 16)
                            KeyFileNum.WriteLine(WritingText)

                            Tmp_Txt = IncludeFileNum.ReadLine
                        Loop

                        '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                        EndLine = Tmp_Txt.Split("]]>")
                        If EndLine(0) = "" Then
                        Else
                            If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                                '숫자를 나눔
                                WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                                    Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 16) & _
                                    Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 16) & _
                                    Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 16)
                                KeyFileNum.WriteLine(WritingText)
                            End If
                        End If

                    End If

                ElseIf InStr(Tmp_Txt, "COORDINATE_REF.CARTESIAN") >= 1 And (Me.ChkRefCoord.Checked = True) Then 'COORDINATE_REF.CARTESIAN ======================================
                    Me.StatusLbl.Text = "Converting : COORDINATE_REF.CARTESIAN"
                    Application.DoEvents()

                    Tmp_Txt = IncludeFileNum.ReadLine
                    Tmp_Txt = IncludeFileNum.ReadLine
                    '테이블의 종류별로 써준다. at KeyFileNum
                    If InStr(Tmp_Txt, "<![CDATA[") >= 1 Then
                        Tmp_Txt = IncludeFileNum.ReadLine ' |	 ID 	 X 	 Y 	 Z 	 |
                        Tmp_Txt = IncludeFileNum.ReadLine ' 수의 시작
                        KeyFileNum.WriteLine("*NODE")
                        Do While Not IncludeFileNum.EndOfStream
                            If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                            '숫자를 나눔
                            WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 16) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 16) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 16)
                            KeyFileNum.WriteLine(WritingText)

                            Tmp_Txt = IncludeFileNum.ReadLine
                        Loop

                        '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                        EndLine = Tmp_Txt.Split("]]>")
                        If EndLine(0) = "" Then
                        Else
                            If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                                '숫자를 나눔
                                WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                                    Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 16) & _
                                    Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 16) & _
                                    Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 16)
                                KeyFileNum.WriteLine(WritingText)
                            End If
                        End If

                    End If

                ElseIf InStr(Tmp_Txt, "ELEMENT.TRIAD3") >= 1 And (Me.ChkRefELE.Checked = False) Then 'ELEMENT.TRIAD3 ======================================
                    Me.StatusLbl.Text = "Converting : ELEMENT.TRIAD3"
                    Application.DoEvents()

                    Tmp_Txt = IncludeFileNum.ReadLine
                    Tmp_Txt = IncludeFileNum.ReadLine
                    '테이블의 종류별로 써준다. at KeyFileNum
                    If InStr(Tmp_Txt, "<![CDATA[") >= 1 Then
                        Tmp_Txt = IncludeFileNum.ReadLine ' |	 ID 	 X 	 Y 	 Z 	 |
                        Tmp_Txt = IncludeFileNum.ReadLine ' 수의 시작
                        KeyFileNum.WriteLine("*ELEMENT_SHELL")
                        Do While Not IncludeFileNum.EndOfStream
                            If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                            '숫자를 나눔
                            WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 8) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 8) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 8) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(4), 8)
                            KeyFileNum.WriteLine(WritingText)

                            Tmp_Txt = IncludeFileNum.ReadLine
                        Loop

                        '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                        EndLine = Tmp_Txt.Split("]]>")
                        If EndLine(0) = "" Then
                        Else
                            If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                                '숫자를 나눔
                                WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 8) & _
                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 8) & _
                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 8) & _
                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(4), 8)
                                KeyFileNum.WriteLine(WritingText)
                            End If
                        End If

                    End If

                ElseIf InStr(Tmp_Txt, "ELEMENT_REF.TRIAD3") >= 1 And (Me.ChkRefELE.Checked = True) Then 'ELEMENT_REF.TRIAD3 ======================================
                    Me.StatusLbl.Text = "Converting : ELEMENT_REF.TRIAD3"
                    Application.DoEvents()

                    Tmp_Txt = IncludeFileNum.ReadLine
                    Tmp_Txt = IncludeFileNum.ReadLine
                    '테이블의 종류별로 써준다. at KeyFileNum
                    If InStr(Tmp_Txt, "<![CDATA[") >= 1 Then
                        Tmp_Txt = IncludeFileNum.ReadLine ' |	 ID 	 X 	 Y 	 Z 	 |
                        Tmp_Txt = IncludeFileNum.ReadLine ' 수의 시작
                        KeyFileNum.WriteLine("*ELEMENT_SHELL")
                        Do While Not IncludeFileNum.EndOfStream
                            If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                            '숫자를 나눔
                            WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 8) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 8) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 8) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(4), 8)
                            KeyFileNum.WriteLine(WritingText)

                            Tmp_Txt = IncludeFileNum.ReadLine
                        Loop

                        '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                        EndLine = Tmp_Txt.Split("]]>")
                        If EndLine(0) = "" Then
                        Else
                            If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                                '숫자를 나눔
                                WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 8) & _
                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 8) & _
                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 8) & _
                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(4), 8)
                                KeyFileNum.WriteLine(WritingText)
                            End If
                        End If

                    End If

                ElseIf InStr(Tmp_Txt, "ELEMENT.QUAD4") >= 1 And (Me.ChkRefELE.Checked = False) Then 'ELEMENT.QUAD4 ======================================
                    Me.StatusLbl.Text = "Converting : ELEMENT.QUAD4"
                    Application.DoEvents()

                    Tmp_Txt = IncludeFileNum.ReadLine
                    Tmp_Txt = IncludeFileNum.ReadLine
                    If InStr(Tmp_Txt, "<![CDATA[") >= 1 Then
                        Tmp_Txt = IncludeFileNum.ReadLine ' |	 ID 	 X 	 Y 	 Z 	 |
                        Tmp_Txt = IncludeFileNum.ReadLine ' 수의 시작
                        KeyFileNum.WriteLine("*ELEMENT_SHELL")
                        Do While Not IncludeFileNum.EndOfStream
                            If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                            '숫자를 나눔
                            WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 8) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 8) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 8) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(4), 8) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(5), 8)
                            KeyFileNum.WriteLine(WritingText)

                            Tmp_Txt = IncludeFileNum.ReadLine
                        Loop

                        '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                        EndLine = Tmp_Txt.Split("]]>")
                        If EndLine(0) = "" Then
                        Else
                            If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                                '숫자를 나눔
                                WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 8) & _
                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 8) & _
                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 8) & _
                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(4), 8) & _
                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(5), 8)
                                KeyFileNum.WriteLine(WritingText)
                            End If
                        End If

                    End If

                ElseIf InStr(Tmp_Txt, "ELEMENT_REF.QUAD4") >= 1 And (Me.ChkRefELE.Checked = True) Then 'ELEMENT_REF.QUAD4 ======================================
                    Me.StatusLbl.Text = "Converting : ELEMENT_REF.QUAD4"
                    Application.DoEvents()

                    Tmp_Txt = IncludeFileNum.ReadLine
                    Tmp_Txt = IncludeFileNum.ReadLine
                    If InStr(Tmp_Txt, "<![CDATA[") >= 1 Then
                        Tmp_Txt = IncludeFileNum.ReadLine ' |	 ID 	 X 	 Y 	 Z 	 |
                        Tmp_Txt = IncludeFileNum.ReadLine ' 수의 시작
                        KeyFileNum.WriteLine("*ELEMENT_SHELL")
                        Do While Not IncludeFileNum.EndOfStream
                            If InStr(Tmp_Txt, "]]>") >= 1 Then Exit Do
                            '숫자를 나눔
                            WritingText = Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(0), 8) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(1), 8) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(2), 8) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(3), 8) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(4), 8) & _
                                Add_Dyna_Letters(SplitTableLine(Tmp_Txt)(5), 8)
                            KeyFileNum.WriteLine(WritingText)

                            Tmp_Txt = IncludeFileNum.ReadLine
                        Loop

                        '만일 ]]> 표식과 맨 마지막 줄이 같이 있을 수 있으므로 확인
                        EndLine = Tmp_Txt.Split("]]>")
                        If EndLine(0) = "" Then
                        Else
                            If IsNumeric(SplitTableLine(EndLine(0))(0)) Then
                                '숫자를 나눔
                                WritingText = Add_Dyna_Letters(SplitTableLine(EndLine(0))(0), 8) & _
                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(1), 8) & _
                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(2), 8) & _
                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(3), 8) & _
                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(4), 8) & _
                                Add_Dyna_Letters(SplitTableLine(EndLine(0))(5), 8)
                                KeyFileNum.WriteLine(WritingText)
                            End If
                        End If

                    End If

                ElseIf InStr(Tmp_Txt, "<PART") >= 1 Then 'PART ============================================================================
                    Me.StatusLbl.Text = "Converting : PART"
                    Application.DoEvents()

                    Dim PID As String = ""
                    Dim Pname As String = ""
                    KeyFileNum.WriteLine("*PART")
                    Do While (1)
                        Tmp_Txt = IncludeFileNum.ReadLine

                        If (InStr(Tmp_Txt, "/>") >= 1) Then Exit Do

                        'If (InStr(Tmp_Txt, ">") >= 1) Then MsgBox("")

                        If AttriGet(Tmp_Txt, 1)(0) = "ID" Then
                            PID = AttriGet(Tmp_Txt, 1)(1)
                        ElseIf AttriGet(Tmp_Txt, 1)(0) = "NAME" Then
                            Pname = AttriGet(Tmp_Txt, 1)(1)
                        End If
                    Loop
                    KeyFileNum.WriteLine(Pname)
                    KeyFileNum.WriteLine(Add_10_Letters(PID))

                ElseIf InStr(Tmp_Txt, "<RIGID_ELEMENT") >= 1 Then 'RIGID ELEMENT ============================================================================
                    Me.StatusLbl.Text = "Converting : RIGID_ELEMENT"
                    Application.DoEvents()

                    Dim RID As String = ""
                    Dim GID As String = ""
                    Dim NodeList As String = ""
                    Dim RigidDescription As String = ""
                    Dim RigidName As String = ""

                    Do While (1)

                        Tmp_Txt = IncludeFileNum.ReadLine

                        If (InStr(Tmp_Txt, "/>") >= 1) Then Exit Do

                        If AttriGet(Tmp_Txt, 1)(0) = "ID" Then
                            RID = AttriGet(Tmp_Txt, 1)(1)
                        ElseIf AttriGet(Tmp_Txt, 1)(0) = "GROUP_LIST" Then
                            GID = AttriGet(Tmp_Txt, 1)(1)
                        ElseIf AttriGet(Tmp_Txt, 1)(0) = "NODE_LIST" Then
                            NodeList = AttriGet(Tmp_Txt, 1)(1)
                        ElseIf AttriGet(Tmp_Txt, 1)(0) = "DESCRIPTION" Then
                            RigidDescription = AttriGet(Tmp_Txt, 1)(1)
                        ElseIf AttriGet(Tmp_Txt, 1)(0) = "NAME" Then
                            RigidName = AttriGet(Tmp_Txt, 1)(1)
                        End If

                    Loop

                    If GID <> "" Then '단순 그룹 ID 만 있을 경우

                        KeyFileNum.WriteLine("*CONSTRAINED_NODAL_RIGID_BODY")
                        KeyFileNum.WriteLine(Add_10_Letters(GID) & "                   0                             0         0")

                    ElseIf NodeList <> "" Then 'NODE_LIST로 정의된 경우

                        Dim NODEs As String()
                        Dim SETLine As String = ""

                        KeyFileNum.WriteLine("*CONSTRAINED_NODAL_RIGID_BODY")
                        KeyFileNum.WriteLine(Add_10_Letters(RID) & "                   0                             0         0")
                        KeyFileNum.WriteLine("*SET_NODE_LIST")
                        KeyFileNum.WriteLine("$HMSET")
                        If RigidDescription <> "" Then
                            KeyFileNum.WriteLine("$HMNAME SETS" & RID & RigidDescription)
                        ElseIf RigidName <> "" Then
                            KeyFileNum.WriteLine("$HMNAME SETS" & RID & RigidName)
                        ElseIf (RigidName = "") And (RigidDescription = "") Then
                            KeyFileNum.WriteLine("$HMNAME SETS" & RID & "NoNameRigid")
                        End If
                        KeyFileNum.WriteLine(Add_10_Letters(RID) & "       0.0       0.0       0.0       0.0")

                        NODEs = SplitTableLine(NodeList)
                        For i = 1 To UBound(NODEs) + 1 Step 8
                            For j = 0 To 7
                                If i + j <= NODEs.Count Then
                                    SETLine = SETLine & Add_10_Letters(NODEs(i + j - 1))
                                Else
                                    Exit For
                                End If
                            Next
                            KeyFileNum.WriteLine(SETLine)
                            SETLine = ""
                        Next

                    End If

                ElseIf InStr(Tmp_Txt, "<GROUP_FE") >= 1 Then 'GROUP FE ============================================================================
                    Me.StatusLbl.Text = "Converting : GROUP_FE"
                    Application.DoEvents()

                    Dim Gname_Descrip As String = ""
                    Dim Gname_NAME As String = ""
                    Dim GID As String = ""
                    Dim NodeList As String = ""
                    Dim NODEs As String()
                    Dim PARTs As String()
                    Dim IsNodes As Boolean = True

                    Do While (1)
                        Tmp_Txt = IncludeFileNum.ReadLine
                        If (InStr(Tmp_Txt, "/>") >= 1) Then Exit Do
                        If AttriGet(Tmp_Txt, 1)(0) = "ID" Then
                            GID = AttriGet(Tmp_Txt, 1)(1)
                        ElseIf AttriGet(Tmp_Txt, 1)(0) = "DESCRIPTION" Then
                            Gname_Descrip = AttriGet(Tmp_Txt, 1)(1)
                        ElseIf AttriGet(Tmp_Txt, 1)(0) = "NAME" Then
                            Gname_NAME = AttriGet(Tmp_Txt, 1)(1)
                        ElseIf AttriGet(Tmp_Txt, 1)(0) = "NODE_LIST" Then
                            IsNodes = True
                            NodeList = AttriGet(Tmp_Txt, 1)(1)
                        ElseIf AttriGet(Tmp_Txt, 1)(0) = "PART_LIST" Then
                            IsNodes = False
                            NodeList = AttriGet(Tmp_Txt, 1)(1)
                        End If
                    Loop

                    If IsNodes = True Then

                        'NODE_LIST ===============================================================================
                        KeyFileNum.WriteLine("*SET_NODE_LIST")
                        KeyFileNum.WriteLine("$HMSET")
                        KeyFileNum.WriteLine("$HMNAME SETS" & GID & Gname_Descrip)
                        KeyFileNum.WriteLine(Add_10_Letters(GID) & "       0.0       0.0       0.0       0.0")

                        If Not NodeList = "" Then
                            Dim SETLine As String = ""
                            NODEs = SplitTableLine(NodeList)
                            For i = 1 To UBound(NODEs) + 1 Step 8
                                For j = 0 To 7
                                    If i + j <= NODEs.Count Then
                                        SETLine = SETLine & Add_10_Letters(NODEs(i + j - 1))
                                    Else
                                        Exit For
                                    End If
                                Next
                                KeyFileNum.WriteLine(SETLine)
                                SETLine = ""
                            Next
                        End If
                        '=========================================================================================

                    ElseIf IsNodes = False Then

                        'PART_LIST ===============================================================================
                        If Not NodeList = "" Then
                            Dim SETLine As String = ""
                            PARTs = SplitTableLine(NodeList)

                            Select Case IsNumeric(CInt(PARTs(0)))
                                Case True
                                    KeyFileNum.WriteLine("*SET_PART_LIST")
                                    KeyFileNum.WriteLine("$HMSET")
                                    KeyFileNum.WriteLine("$HMNAME SETS" & Add_Dyna_Letters(GID, 8) & Gname_NAME)
                                    KeyFileNum.WriteLine(Add_10_Letters(GID))

                                    For i = 1 To UBound(PARTs) + 1 Step 8
                                        For j = 0 To 7
                                            If i + j <= PARTs.Count Then
                                                SETLine = SETLine & Add_10_Letters(PARTs(i + j - 1))
                                            Else
                                                Exit For
                                            End If
                                        Next
                                        KeyFileNum.WriteLine(SETLine)
                                        SETLine = ""
                                    Next
                                Case False
                                    '아무것도 하지않고 나감
                                    '(파트 이름으로 기록된 그룹은 MADYMO에서만 유효한 PART 그룹임
                            End Select
                        End If
                        '=========================================================================================

                    End If
                End If
                '**************************************************************************

            Loop

        End If

        IncludeFileNum.Close()

    End Sub
End Class