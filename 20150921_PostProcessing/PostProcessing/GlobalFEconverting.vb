Imports System.IO

Module GlobalFEconverting

    'Rigid에서 중복 노드를 찾기위한 변수
    Public DuplicationNodes(0) As Integer
    Public EntireNodes(0) As Integer
    Public RigidGroupNum(0) As Integer

    'Null (Facet) 모드
    Public IsNullMode As Boolean = False

    'Null Property의 두께(in meter)
    Public NullShellThickness As Single

    'Element Shell Table을 한 곳에 쓰기위해 하나로 모음 ('16.06.22)
    Public Element3(0) As String
    Public Element4(50000000) As String
    Public Element4_cnt As Integer = 0

    Public Function Con_MAT_NULL(ByRef KeyFile As StreamReader, ByRef XMLFile As StreamWriter, ByRef Pfrm As FrmFEconverting) As String

        '*MAT_NULL
        'MID / RO / PC / MU / TEROD / CEROD / YM / PR

        Dim TmpLine As String
        TmpLine = KeyFile.ReadLine()
        If Mid(TmpLine, 1, 1) = "$" Then
            TmpLine = KeyFile.ReadLine()
        End If

        Dim ID As Integer
        Dim RO As Double
        Dim YM As Double
        Dim PR As Double

        ID = Mid(TmpLine, 1, 10)
        RO = Mid(TmpLine, 11, 10) * 1000000000000.0
        YM = Mid(TmpLine, 61, 10) * 1000000.0
        PR = Mid(TmpLine, 71, 10)

        Do While Not (Mid(TmpLine, 1, 1) = "*" Or Mid(TmpLine, 1, 1) = "$")

            If IsNumeric(Mid(TmpLine, 1, 10)) Then
                XMLFile.Write("<MATERIAL.NULL " & "ID=""" & ID & """ DENSITY_NULL=""" & RO & """ CONTACT_E=""" & YM & """ CONTACT_NU=""" & PR & """ />" & vbCrLf)
                Pfrm.ProStatus.Text = "Writing...<>MATRIAL.NULL : " & ID
                'Pfrm.ResultTxt.SelectionStart = Pfrm.ResultTxt.Text.Length
                'Pfrm.ResultTxt.ScrollToCaret()
            End If

            TmpLine = KeyFile.ReadLine()
        Loop

        Con_MAT_NULL = TmpLine
    End Function

    Public Function Con_PART(ByRef KeyFile As StreamReader, ByRef XMLFile As StreamWriter, ByRef Pfrm As FrmFEconverting) As String

        '*PART
        'PID / SECID / MID

        Dim TmpLine As String
        TmpLine = KeyFile.ReadLine()

        Dim PID As Integer
        Dim SECID As Integer
        Dim MaterialID As Integer
        Dim Pname As String = ""


        Do While Not Mid(TmpLine, 1, 1) = "*"

            Do While Mid(TmpLine, 1, 1) = "$"

                Select Case Mid(TmpLine, 1, 7) = "$HMNAME"
                    Case True
                        Pname = RTrim(Mid(TmpLine, 22, Len(TmpLine)))
                End Select

                TmpLine = KeyFile.ReadLine()

            Loop

            If Pname = "" Then Pname = RTrim(TmpLine)

            TmpLine = KeyFile.ReadLine()

            '주석을 건너뜀======================
            Do While Mid(TmpLine, 1, 1) = "$"
                TmpLine = KeyFile.ReadLine()
            Loop
            '==================================

            PID = Mid(TmpLine, 1, 10)
            If IsNullMode = True Then
                MaterialID = 1
                SECID = 1
            Else
                MaterialID = Mid(TmpLine, 21, 10)
                SECID = Mid(TmpLine, 11, 10)
            End If

            If IsNumeric(Mid(TmpLine, 1, 10)) Then
                XMLFile.Write("<PART " & "DESCRIPTION=""" & Pname & """ ID=""" & PID & """ PROPERTY=""" & SECID & """ MATERIAL=""" & MaterialID & """ />" & vbCrLf)
                Pfrm.ProStatus.Text = "Writing...<>PART : " & PID & "-" & Pname
                'Pfrm.ResultTxt.SelectionStart = Pfrm.ResultTxt.Text.Length
                'Pfrm.ResultTxt.ScrollToCaret()
                Application.DoEvents()
            End If

            Pname = ""

            TmpLine = KeyFile.ReadLine()
        Loop

        Con_PART = TmpLine

    End Function

    Public Function Con_PART_CONTACT(ByRef KeyFile As StreamReader, ByRef XMLFile As StreamWriter, ByRef Pfrm As FrmFEconverting) As String

        '*PART
        'PID / SECID / MID

        Dim TmpLine As String
        TmpLine = KeyFile.ReadLine()

        Dim PID As Integer
        Dim SECID As Integer
        Dim MaterialID As Integer
        Dim Pname As String = ""


        Do While Not Mid(TmpLine, 1, 1) = "*"

            Do While Mid(TmpLine, 1, 1) = "$"

                Select Case Mid(TmpLine, 1, 7) = "$HMNAME"
                    Case True
                        Pname = RTrim(Mid(TmpLine, 22, Len(TmpLine)))
                End Select

                TmpLine = KeyFile.ReadLine()

            Loop

            If Pname = "" Then Pname = RTrim(TmpLine)

            TmpLine = KeyFile.ReadLine()

            '주석을 건너뜀======================
            Do While Mid(TmpLine, 1, 1) = "$"
                TmpLine = KeyFile.ReadLine()
            Loop
            '==================================

            PID = Mid(TmpLine, 1, 10)
            If IsNullMode = True Then
                MaterialID = 1
                SECID = 1
            Else
                MaterialID = Mid(TmpLine, 21, 10)
                SECID = Mid(TmpLine, 11, 10)
            End If


            If IsNumeric(Mid(TmpLine, 1, 10)) Then
                XMLFile.Write("<PART " & "DESCRIPTION=""" & Pname & """ ID=""" & PID & """ PROPERTY=""" & SECID & """ MATERIAL=""" & MaterialID & """ />" & vbCrLf)
                Pfrm.ProStatus.Text = "Writing...<>PART : " & PID & "-" & Pname
                'Pfrm.ResultTxt.SelectionStart = Pfrm.ResultTxt.Text.Length
                'Pfrm.ResultTxt.ScrollToCaret()
                Application.DoEvents()
            End If

            Pname = ""

            TmpLine = KeyFile.ReadLine()
            TmpLine = KeyFile.ReadLine()

            If TmpLine = Nothing Then
                Exit Do
            End If
        Loop

        Con_PART_CONTACT = TmpLine

    End Function

    Public Function Con_SECTION_SHELL(ByRef KeyFile As StreamReader, ByRef XMLFile As StreamWriter, ByRef Pfrm As FrmFEconverting, Optional ByVal TITLE As String = "") As String

        '*SECTION_SHELL
        Dim TmpLine As String
        Dim Pname As String = ""
        TmpLine = KeyFile.ReadLine()

        Do While Mid(TmpLine, 1, 1) = "$"

            Select Case Mid(TmpLine, 1, 7) = "$HMNAME"
                Case True
                    Pname = RTrim(Mid(TmpLine, 22, Len(TmpLine)))
            End Select

            TmpLine = KeyFile.ReadLine()
        Loop

        If TITLE = "TITLE" Then
            TmpLine = KeyFile.ReadLine()
        End If

        '주석을 건너뜀======================
        Do While Mid(TmpLine, 1, 1) = "$"
            TmpLine = KeyFile.ReadLine()
        Loop
        '==================================

        Dim SID As Integer
        SID = Mid(TmpLine, 1, 10)

        TmpLine = KeyFile.ReadLine()

        '주석을 건너뜀======================
        Do While Mid(TmpLine, 1, 1) = "$"
            TmpLine = KeyFile.ReadLine()
        Loop
        '==================================

        If IsNumeric(Mid(TmpLine, 1, 10)) Then

            Dim T1, T2, T3, T4 As Double
            T1 = Mid(TmpLine, 1, 10)
            T2 = Mid(TmpLine, 11, 10)
            T3 = Mid(TmpLine, 21, 10)
            T4 = Mid(TmpLine, 31, 10)

            Dim Thickness As Double
            Thickness = (T1 + T2 + T3 + T4) / 4 * 0.001

            XMLFile.Write("<PROPERTY.SHELL4 " & "DESCRIPTION=""" & Pname & """ ID=""" & SID & """ THICK=""" & Thickness & """ HOURGLASS_PAR=""" & 0.1 & """ HOURGLASS_MTH=""STIFFNESS"" " & "INT_POINT=""3"" " & "UPDATE_THICK=""OFF""" & " />" & vbCrLf)
            Pfrm.ProStatus.Text = "Writing...<>PROPERTY.SHELL4 : " & SID
        End If

        Con_SECTION_SHELL = ""

    End Function

    Public Function Con_SECTION_SOLID8(ByRef KeyFile As StreamReader, ByRef XMLFile As StreamWriter, ByRef Pfrm As FrmFEconverting, ByVal OpString As String) As String

        '*SECTION_SOLID_{Option}
        Dim TmpLine As String
        Dim SID As Integer
        Dim Pname As String = ""

        TmpLine = KeyFile.ReadLine()

        Do While Mid(TmpLine, 1, 1) = "$"

            Select Case Mid(TmpLine, 1, 7) = "$HMNAME"
                Case True
                    Pname = RTrim(Mid(TmpLine, 22, Len(TmpLine)))
            End Select

            TmpLine = KeyFile.ReadLine()
        Loop

        If OpString = "TITLE" Then
            TmpLine = KeyFile.ReadLine()
        End If

        '주석을 건너뜀======================
        Do While Mid(TmpLine, 1, 1) = "$"
            TmpLine = KeyFile.ReadLine()
        Loop
        '==================================

        SID = Mid(TmpLine, 1, 10)

        If IsNumeric(Mid(TmpLine, 1, 10)) Then
            XMLFile.Write("<PROPERTY.SOLID8 " & "DESCRIPTION=""" & Pname & """ ID=""" & SID & """ HOURGLASS_PAR=""" & 0.1 & """ FULL_INT=""" & "OFF" & """ ADV_STRAIN=""ON""" & " />" & vbCrLf)
            Pfrm.ProStatus.Text = "Writing...<>PROPERTY.SOLID8 : " & SID
        End If

        Con_SECTION_SOLID8 = ""

    End Function

    Public Function Con_NODE(ByRef KeyFile As StreamReader, ByRef XMLFile As StreamWriter, ByRef Pfrm As FrmFEconverting) As String

        '*NODE

        Dim TmpLine As String = ""
        Dim NodeID As Integer
        Dim Xcoord As Double
        Dim Ycoord As Double
        Dim Zcoord As Double

        TmpLine = KeyFile.ReadLine()

        Pfrm.ProStatus.Text = "Writing...<>TABLE - NODE Coodinates "

        XMLFile.Write("<TABLE TYPE=""COORDINATE.CARTESIAN"">" & vbCrLf)
        XMLFile.Write("<![CDATA[" & vbCrLf)
        XMLFile.Write("| ID    X     Y      Z       |" & vbCrLf)

        '*NODE
        Do While Not (Mid(TmpLine, 1, 1) = "*" Or Mid(TmpLine, 1, 1) = "$")
            NodeID = Mid(TmpLine, 1, 8)
            Xcoord = Mid(TmpLine, 9, 16) * 0.001
            Ycoord = Mid(TmpLine, 25, 16) * 0.001
            Zcoord = Mid(TmpLine, 41, 16) * 0.001
            XMLFile.Write(NodeID & Chr(9) & Xcoord & Chr(9) & Ycoord & Chr(9) & Zcoord & vbCrLf)
            TmpLine = KeyFile.ReadLine()
        Loop

        XMLFile.Write("]]>" & vbCrLf)
        XMLFile.Write("</TABLE>" & vbCrLf)

        Pfrm.ProStatus.Text = "Complete...<>TABLE - NODE Coodinates "

        Con_NODE = TmpLine

    End Function

    Public Function Con_ELEMENT_SHELL(ByRef KeyFile As StreamReader, ByRef XMLFile As StreamWriter, ByRef Pfrm As FrmFEconverting) As String

        'ELEMENT_SHELL

        Dim TmpLine As String = ""
        Dim ELEMENT_ID As Integer
        Dim PID As Integer
        Dim Node1 As Integer
        Dim Node2 As Integer
        Dim Node3 As Integer
        Dim Node4 As Integer

        'TRIA3 Element → QUAD4 Element
        TmpLine = KeyFile.ReadLine()

        'Pfrm.ProStatus.Text = "Writing...<>TABLE - Elements Connectivity TRIAD3 "
        'Pfrm.ResultTxt.SelectionStart = Pfrm.ResultTxt.Text.Length
        'Pfrm.ResultTxt.ScrollToCaret()


        'XMLFile.Write("<TABLE TYPE=""ELEMENT.TRIAD3"">" & vbCrLf)
        'XMLFile.Write("<![CDATA[" & vbCrLf)
        'XMLFile.Write("| ID    PART      N1      N2      N3      |" & vbCrLf)

        Do While Not (Mid(TmpLine, 1, 1) = "*" Or Mid(TmpLine, 1, 1) = "$")
            ELEMENT_ID = Mid(TmpLine, 1, 8)
            PID = Mid(TmpLine, 9, 8)
            Node1 = Mid(TmpLine, 17, 8)
            Node2 = Mid(TmpLine, 25, 8)
            Node3 = Mid(TmpLine, 33, 8)
            Node4 = Mid(TmpLine, 41, 8)

            If Node3 <> Node4 Then Exit Do

            'XMLFile.Write(ELEMENT_ID & Chr(9) & PID & Chr(9) & Node1 & Chr(9) & Node2 & Chr(9) & Node3 & vbCrLf)

            '=TEMP=
            ReDim Preserve Element3(UBound(Element3) + 1)
            Element3(UBound(Element3)) = ELEMENT_ID & " " & PID & " " & Node1 & " " & Node2 & " " & Node3
            '=TEMP=

            TmpLine = KeyFile.ReadLine()
        Loop

        'XMLFile.Write("]]>" & vbCrLf)
        'XMLFile.Write("</TABLE>" & vbCrLf)

        If Mid(TmpLine, 1, 1) = "*" Or Mid(TmpLine, 1, 1) = "$" Then
            Con_ELEMENT_SHELL = TmpLine
            Exit Function
        End If

        'Pfrm.ProStatus.Text = "Complete...<>TABLE - Elements Connectivity TRIAD3 "
        'Pfrm.ProStatus.Text = "Writing...<>TABLE - Elements Connectivity QUAD4 "
        'Pfrm.ResultTxt.SelectionStart = Pfrm.ResultTxt.Text.Length
        'Pfrm.ResultTxt.ScrollToCaret()



        'XMLFile.Write("<TABLE TYPE=""ELEMENT.QUAD4"">" & vbCrLf)
        'XMLFile.Write("<![CDATA[" & vbCrLf)
        'XMLFile.Write("| ID    PART      N1      N2      N3      N4    |" & vbCrLf)

        Do While Not (Mid(TmpLine, 1, 1) = "*" Or Mid(TmpLine, 1, 1) = "$")
            'ELEMENT_ID = Mid(TmpLine, 1, 8)
            'PID = Mid(TmpLine, 9, 8)
            'Node1 = Mid(TmpLine, 17, 8)
            'Node2 = Mid(TmpLine, 25, 8)
            'Node3 = Mid(TmpLine, 33, 8)
            'Node4 = Mid(TmpLine, 41, 8)

            'XMLFile.Write(ELEMENT_ID & Chr(9) & PID & Chr(9) & Node1 & Chr(9) & Node2 & Chr(9) & Node3 & Chr(9) & Node4 & vbCrLf)

            '=TEMP=
            'ReDim Preserve Element4(UBound(Element4) + 1)
            Element4(Element4_cnt) = TmpLine.Insert(9, " ").Insert(17, " ").Insert(26, " ").Insert(35, " ").Insert(44, " ") 'ELEMENT_ID & " " & PID & " " & Node1 & " " & Node2 & " " & Node3 & " " & Node4
            Element4_cnt = Element4_cnt + 1
            '=TEMP=

            TmpLine = KeyFile.ReadLine()
        Loop

        'XMLFile.Write("]]>" & vbCrLf)
        'XMLFile.Write("</TABLE>" & vbCrLf)

        'Pfrm.ProStatus.Text = "Complete...<>TABLE - Elements Connectivity QUAD4 "

        Con_ELEMENT_SHELL = TmpLine

    End Function

    Public Function Con_ELEMENT_SHELL_THICKNESS_OFFSET(ByRef KeyFile As StreamReader, ByRef XMLFile As StreamWriter, ByRef Pfrm As FrmFEconverting) As String

        'ELEMENT_SHELL

        Dim TmpLine As String = ""
        Dim ELEMENT_ID As Integer
        Dim PID As Integer
        Dim Node1 As Integer
        Dim Node2 As Integer
        Dim Node3 As Integer
        Dim Node4 As Integer

        'TRIA3 Element → QUAD4 Element
        TmpLine = KeyFile.ReadLine()

        'Pfrm.ProStatus.Text = "Writing...<>TABLE - Elements Connectivity TRIAD3 "
        'Pfrm.ResultTxt.SelectionStart = Pfrm.ResultTxt.Text.Length
        'Pfrm.ResultTxt.ScrollToCaret()

        'XMLFile.Write("<TABLE TYPE=""ELEMENT.TRIAD3"">" & vbCrLf)
        'XMLFile.Write("<![CDATA[" & vbCrLf)
        'XMLFile.Write("| ID    PART      N1      N2      N3      |" & vbCrLf)

        Do While Not (Mid(TmpLine, 1, 1) = "*" Or Mid(TmpLine, 1, 1) = "$")
            Try

                ELEMENT_ID = Mid(TmpLine, 1, 8)
                PID = Mid(TmpLine, 9, 8)
                Node1 = Mid(TmpLine, 17, 8)
                Node2 = Mid(TmpLine, 25, 8)
                Node3 = Mid(TmpLine, 33, 8)
                Node4 = Mid(TmpLine, 41, 8)

                If Node3 <> Node4 Then Exit Do

                'XMLFile.Write(ELEMENT_ID & Chr(9) & PID & Chr(9) & Node1 & Chr(9) & Node2 & Chr(9) & Node3 & vbCrLf)

                ReDim Preserve Element3(UBound(Element3) + 1)
                Element3(UBound(Element3)) = ELEMENT_ID & " " & PID & " " & Node1 & " " & Node2 & " " & Node3

            Catch ex As Exception

            End Try
            Do
                TmpLine = KeyFile.ReadLine()
            Loop Until (Mid(TmpLine, 1, 1) <> " ")

        Loop

        'XMLFile.Write("]]>" & vbCrLf)
        'XMLFile.Write("</TABLE>" & vbCrLf)

        If Mid(TmpLine, 1, 1) = "*" Or Mid(TmpLine, 1, 1) = "$" Then
            Con_ELEMENT_SHELL_THICKNESS_OFFSET = TmpLine
            Exit Function
        End If

        'Pfrm.ProStatus.Text = "Complete...<>TABLE - Elements Connectivity TRIAD3 "
        'Pfrm.ProStatus.Text = "Writing...<>TABLE - Elements Connectivity QUAD4 "
        'Pfrm.ResultTxt.SelectionStart = Pfrm.ResultTxt.Text.Length
        'Pfrm.ResultTxt.ScrollToCaret()



        'XMLFile.Write("<TABLE TYPE=""ELEMENT.QUAD4"">" & vbCrLf)
        'XMLFile.Write("<![CDATA[" & vbCrLf)
        'XMLFile.Write("| ID    PART      N1      N2      N3      N4    |" & vbCrLf)

        Do While Not (Mid(TmpLine, 1, 1) = "*" Or Mid(TmpLine, 1, 1) = "$")
            Try
                'ELEMENT_ID = Mid(TmpLine, 1, 8)
                'PID = Mid(TmpLine, 9, 8)
                'Node1 = Mid(TmpLine, 17, 8)
                'Node2 = Mid(TmpLine, 25, 8)
                'Node3 = Mid(TmpLine, 33, 8)
                'Node4 = Mid(TmpLine, 41, 8)

                'XMLFile.Write(ELEMENT_ID & Chr(9) & PID & Chr(9) & Node1 & Chr(9) & Node2 & Chr(9) & Node3 & Chr(9) & Node4 & vbCrLf)

                'ReDim Preserve Element4(UBound(Element4) + 1)
                Element4(Element4_cnt) = TmpLine.Insert(9, " ").Insert(17, " ").Insert(26, " ").Insert(35, " ").Insert(44, " ") 'ELEMENT_ID & " " & PID & " " & Node1 & " " & Node2 & " " & Node3 & " " & Node4
                Element4_cnt = Element4_cnt + 1
            Catch ex As Exception

            End Try

            Do
                TmpLine = KeyFile.ReadLine()
            Loop Until (Mid(TmpLine, 1, 1) <> " ")
        Loop

        'XMLFile.Write("]]>" & vbCrLf)
        'XMLFile.Write("</TABLE>" & vbCrLf)

        'Pfrm.ProStatus.Text = "Complete...<>TABLE - Elements Connectivity QUAD4 "

        Con_ELEMENT_SHELL_THICKNESS_OFFSET = TmpLine

    End Function

    Public Function Con_ELEMENT_SOLID(ByRef KeyFile As StreamReader, ByRef XMLFile As StreamWriter, ByRef Pfrm As FrmFEconverting) As String

        'ELEMENT_SOLID
        Dim TmpLine As String = ""
        Dim ELEMENT_ID As Integer
        Dim PID As Integer
        Dim Node1 As Integer
        Dim Node2 As Integer
        Dim Node3 As Integer
        Dim Node4 As Integer
        Dim Node5 As Integer
        Dim Node6 As Integer
        Dim Node7 As Integer
        Dim Node8 As Integer

        TmpLine = KeyFile.ReadLine()

        Pfrm.ProStatus.Text = "Writing...<>TABLE - Elements Connectivity HEXA8 "

        XMLFile.Write("<TABLE TYPE=""ELEMENT.HEXA8"">" & vbCrLf)
        XMLFile.Write("<![CDATA[" & vbCrLf)
        XMLFile.Write("| ID    PART      N1      N2      N3      N4      N5      N6      N7      N8    |" & vbCrLf)

        '*NODE
        Do While Not (Mid(TmpLine, 1, 1) = "*" Or Mid(TmpLine, 1, 1) = "$")
            ELEMENT_ID = Mid(TmpLine, 1, 8)
            PID = Mid(TmpLine, 9, 8)
            Node1 = Mid(TmpLine, 17, 8)
            Node2 = Mid(TmpLine, 25, 8)
            Node3 = Mid(TmpLine, 33, 8)
            Node4 = Mid(TmpLine, 41, 8)
            Node5 = Mid(TmpLine, 49, 8)
            Node6 = Mid(TmpLine, 57, 8)
            Node7 = Mid(TmpLine, 65, 8)
            Node8 = Mid(TmpLine, 73, 8)
            XMLFile.Write(ELEMENT_ID & Chr(9) & PID & Chr(9) & Node1 & Chr(9) & Node2 & Chr(9) & Node3 & Chr(9) & Node4 & Chr(9) & _
                          Node5 & Chr(9) & Node6 & Chr(9) & Node7 & Chr(9) & Node8 & vbCrLf)
            TmpLine = KeyFile.ReadLine()
        Loop

        XMLFile.Write("]]>" & vbCrLf)
        XMLFile.Write("</TABLE>" & vbCrLf)

        Pfrm.ProStatus.Text = "Compelete...<>TABLE - Elements Connectivity HEXA8 "

        Con_ELEMENT_SOLID = TmpLine

    End Function

    Public Function Con_SET_NODE(ByRef KeyFile As StreamReader, ByRef XMLFile As StreamWriter, ByRef Pfrm As FrmFEconverting) As String

        'SET_NODE_LIST
        Dim TmpLine As String = ""
        Dim GID, i, k As Integer
        Dim GNAME As String = ""
        Dim NODE_str As String = ""

        TmpLine = KeyFile.ReadLine()

        Do While Mid(TmpLine, 1, 1) = "$"
            If Mid(TmpLine, 1, 7) = "$HMNAME" Then
                GNAME = RTrim(Mid(TmpLine, 21, Len(TmpLine)))
            End If
            TmpLine = KeyFile.ReadLine()
        Loop

        Try
            GID = Mid(TmpLine, 1, 10)
        Catch ex As Exception
            Pfrm.ResultTxt.Text = Pfrm.ResultTxt.Text & "Empty *SET_NODE_LIST Found → Ignore" & vbCrLf
        End Try

        TmpLine = KeyFile.ReadLine()

        '주석을 건너뜀======================
        Do While Mid(TmpLine, 1, 1) = "$"
            TmpLine = KeyFile.ReadLine()
        Loop
        '==================================

        Do While Not (Mid(TmpLine, 1, 1) = "*" Or Mid(TmpLine, 1, 1) = "$")

            For i = 0 To 7
                If IsNumeric(Mid(TmpLine, i * 10 + 1, 10)) Then
                    NODE_str = NODE_str & "  " & Mid(TmpLine, i * 10 + 1, 10)

                    'Rigid Group인지 검사
                    For k = UBound(RigidGroupNum) To 1 Step -1
                        If RigidGroupNum(k) = GID Then
                            '전체 Rigid Group 노드 변수에 써준다
                            ReDim Preserve EntireNodes(UBound(EntireNodes) + 1)
                            EntireNodes(UBound(EntireNodes)) = Mid(TmpLine, i * 10 + 1, 10)
                        End If
                    Next

                Else
                    Exit For
                End If
            Next

            TmpLine = KeyFile.ReadLine()

        Loop

        NODE_str = LTrim(RTrim(NODE_str))

        XMLFile.Write("<GROUP_FE ID=""" & GID & """ " & vbCrLf)
        XMLFile.Write("DESCRIPTION=""" & GNAME & """" & vbCrLf)
        XMLFile.Write("NODE_LIST=""" & NODE_str & """" & vbCrLf)
        XMLFile.Write("/>" & vbCrLf)

        Pfrm.ProStatus.Text = "Writing...<>GROUP_FE - NODE : " & GID

        Con_SET_NODE = TmpLine

    End Function

    Public Function Con_SET_PART(ByRef KeyFile As StreamReader, ByRef XMLFile As StreamWriter, ByRef Pfrm As FrmFEconverting) As String

        'SET_PART_LIST
        Dim TmpLine As String = ""
        Dim GID, i As Integer
        Dim GNAME As String = ""
        Dim PART_str As String = ""

        TmpLine = KeyFile.ReadLine()

        Do While Mid(TmpLine, 1, 1) = "$"
            If Mid(TmpLine, 1, 7) = "$HMNAME" Then
                GNAME = RTrim(Mid(TmpLine, 21, Len(TmpLine)))
            End If
            TmpLine = KeyFile.ReadLine()
        Loop

        GID = Mid(TmpLine, 1, 10)

        TmpLine = KeyFile.ReadLine()

        '주석을 건너뜀======================
        Do While Mid(TmpLine, 1, 1) = "$"
            TmpLine = KeyFile.ReadLine()
        Loop
        '==================================

        Do While Not (Mid(TmpLine, 1, 1) = "*" Or Mid(TmpLine, 1, 1) = "$")

            For i = 0 To 7
                If IsNumeric(Mid(TmpLine, i * 10 + 1, 10)) Then
                    PART_str = PART_str & "  " & Mid(TmpLine, i * 10 + 1, 10)
                Else
                    Exit For
                End If
            Next

            TmpLine = KeyFile.ReadLine()

        Loop

        PART_str = LTrim(RTrim(PART_str))

        XMLFile.Write("<GROUP_FE ID=""" & GID & """ " & vbCrLf)
        XMLFile.Write("DESCRIPTION=""" & GNAME & """" & vbCrLf)
        XMLFile.Write("PART_LIST=""" & PART_str & """" & vbCrLf)
        XMLFile.Write("/>" & vbCrLf)

        Pfrm.ProStatus.Text = "Writing...<>GROUP_FE - PART : " & GNAME

        Con_SET_PART = TmpLine

    End Function

    Public Function Con_CONSTRAINED_NODAL_RIGID_BODY(ByRef KeyFile As StreamReader, ByRef XMLFile As StreamWriter, ByRef Pfrm As FrmFEconverting) As String

        'CONSTRAINED_NODAL_RIGID_BODY
        Dim TmpLine As String = ""
        Dim RID, PNID As Integer
        Dim RNAME As String = ""

        TmpLine = KeyFile.ReadLine()

        Do While Mid(TmpLine, 1, 1) = "$"
            'Select Case Mid(TmpLine, 1, 8)
            '    Case "$HMNAME "
            '        RNAME = RTrim(Mid(TmpLine, 22, Len(TmpLine)))
            '    Case "$HWCOLOR"
            'End Select
            TmpLine = KeyFile.ReadLine()
        Loop

        RID = Mid(TmpLine, 1, 10)
        PNID = RID

        Do While Not (Mid(TmpLine, 1, 1) = "*" Or Mid(TmpLine, 1, 1) = "$")
            TmpLine = KeyFile.ReadLine()
        Loop

        XMLFile.Write("<RIGID_ELEMENT ID=""" & RID & """ GROUP_LIST=""" & PNID & """ />" & vbCrLf)

        '그룹명을 기억한다.
        ReDim Preserve RigidGroupNum(UBound(RigidGroupNum) + 1)
        RigidGroupNum(UBound(RigidGroupNum)) = PNID

        Pfrm.ProStatus.Text = "Writing...<>RIGID_ELEMENT : " & RID

        Con_CONSTRAINED_NODAL_RIGID_BODY = TmpLine

    End Function
End Module
