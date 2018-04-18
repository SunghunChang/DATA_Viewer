Imports System.IO

Public Class FrmFEconverting_LSPRE

    Public PARTsID(0) As Integer

    Private Sub DyOpnBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DyOpnBtn.Click

        Me.StatusLbl.Text = "Selection"
        Application.DoEvents()

        Dim i, j As Integer
        Dim DlgResult As DialogResult

        With Me.KeyFileDlg
            .Title = "Select DYNA File"
            .Multiselect = False
            .FileName = ""                   '초기에 표시되는 파일 이름
            '.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "DYNA File|*.k;*.key;*.dyn|모든 파일|*.*"
            DlgResult = .ShowDialog()
        End With

        'If Me.KeyFileDlg.FileName = "" Then
        If DlgResult = DialogResult.Cancel Then
            Me.StatusLbl.Text = "Cancel"
            Exit Sub
        Else
            Me.DyNameTxt.Text = Me.KeyFileDlg.SafeFileName
        End If

        Dim Tmp_Txt As String
        Dim fileNum As StreamReader
        Dim NewXMLfile As StreamWriter

        'LS-DYNA
        Dim ReadDYNA As New FileStream(Me.KeyFileDlg.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        fileNum = New StreamReader(ReadDYNA)

        'XML
        PARTsID(0) = -1
        Dim SaveFileName As String()
        With Me.XMLsave
            .Title = "저장 파일 선택"
            SaveFileName = FileNameGet(Me.KeyFileDlg.FileNames)
            .FileName = SaveFileName(0)                  '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "MADYMO Input File|*.xml|모든 파일|*.*"
            DlgResult = .ShowDialog()
        End With

        'If Me.XMLsave.FileName = "" Then
        If DlgResult = DialogResult.Cancel Then
            fileNum.Close()
            ReadDYNA.Close()
            Exit Sub
        End If

        NewXMLfile = New StreamWriter(Me.XMLsave.FileName)

        '파일 헤더 쓰기
        NewXMLfile.WriteLine("<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>")
        NewXMLfile.WriteLine("<!DOCTYPE MADYMO SYSTEM ""mtd_3d.dtd"">")
        NewXMLfile.WriteLine("<MADYMO RELEASE=""R7.5"" >")
        NewXMLfile.WriteLine("<SYSTEM.MODEL ID=""1"" NAME=""FE_SYSTEM"">")
        NewXMLfile.WriteLine("<COMMENT><![CDATA[ Using MADYMO DATA Viewer Converting Tool ]]></COMMENT>")
        NewXMLfile.WriteLine("<COMMENT><![CDATA[" & "Converted : " & Date.Today.ToString & "]]></COMMENT>")
        NewXMLfile.WriteLine("<COMMENT><![CDATA[ The developer is no Liability to reply any question of user or to modify the source code for USER convenience. ]]></COMMENT>")
        NewXMLfile.WriteLine("<FE_MODEL ID=""1"" NAME=""FE_MODEL"">")
        NewXMLfile.WriteLine("<CONTROL_FE_MODEL ALPHA_COEF=""0.0"" MASS_LUMP_MTH=""GEOMETRICAL""/>")
        NewXMLfile.WriteLine("<CONTROL_FE_TIME_STEP REDUCTION_FACTOR=""0.9"" CRITICAL_ELEMENTS=""20"" MIN_STEP=""1e-06"" MAX_STEP=""1e+60"" TIME_INT_MTH=""NORMAL"" NR_OF_CYCLES=""0""/>")

        Tmp_Txt = fileNum.ReadLine

        Me.StatusLbl.Text = "Now Converting....."
        Application.DoEvents()

        Do While Not (Tmp_Txt = "*END") 'fileNum.EndOfStream
            Select Case Tmp_Txt
                Case "*ELEMENT_SHELL"
                    Tmp_Txt = Con_ELEMENT_SHELL_LSPRE(fileNum, NewXMLfile)  'PARTsID 정보도 가져온다
                Case "*ELEMENT_SOLID"
                    Tmp_Txt = Con_ELEMENT_SOLID_LSPRE(fileNum, NewXMLfile)  'PARTsID 정보도 가져온다
                Case "*NODE"
                    Tmp_Txt = Con_NODE_LSPRE(fileNum, NewXMLfile)
                Case Else
                    Tmp_Txt = fileNum.ReadLine
            End Select
        Loop

        NewXMLfile.Write("<MATERIAL.NULL " & "ID=""" & "1" & """ NAME=""MAT_NULL""" & " />" & vbCrLf)
        NewXMLfile.Write("<PROPERTY.SHELL4 " & "NAME=""PRO_NULL_SHELL"" ID=""1"" THICK=""0.005"" />" & vbCrLf)

        For i = 1 To UBound(PARTsID)
            NewXMLfile.Write("<PART " & " ID=""" & PARTsID(i) & """ PROPERTY=""" & "1" & """ MATERIAL=""" & "1" & """ />" & vbCrLf)
        Next

        Me.StatusLbl.Text = "Complete..."
        Application.DoEvents()

        NewXMLfile.WriteLine("</FE_MODEL>")
        NewXMLfile.WriteLine("</SYSTEM.MODEL>")
        NewXMLfile.WriteLine("</MADYMO>")

        If Me.SETchk.Checked = True Then
            'Making *SET_PART_LIST ==============================================================
            Dim NewSETfile As StreamWriter
            Dim SETLine As String = ""

            NewSETfile = New StreamWriter(Me.XMLsave.FileName & "_SET")
            NewSETfile.WriteLine("*SET_PART_LIST")
            NewSETfile.WriteLine(Add_10_Letters(TxtID.Text))

            For i = 1 To UBound(PARTsID) Step 8
                For j = 0 To 7
                    If i + j <= UBound(PARTsID) Then
                        SETLine = SETLine & Add_10_Letters(CStr(PARTsID(i + j)))
                    Else
                        Exit For
                    End If
                Next
                NewSETfile.WriteLine(SETLine)
                SETLine = ""
            Next

            NewSETfile.Close()
            '====================================================================================
        End If

        ReDim PARTsID(0)
        NewXMLfile.Close()
        fileNum.Close()
        ReadDYNA.Close()

    End Sub

    Private Function Con_NODE_LSPRE(ByRef KeyFile As StreamReader, ByRef XMLFile As StreamWriter) As String

        '*NODE

        Dim TmpLine As String = ""
        Dim NodeID As Integer
        Dim Xcoord As Double
        Dim Ycoord As Double
        Dim Zcoord As Double

        TmpLine = KeyFile.ReadLine()

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

        Con_NODE_LSPRE = TmpLine

    End Function

    Private Function Con_ELEMENT_SHELL_LSPRE(ByRef KeyFile As StreamReader, ByRef XMLFile As StreamWriter) As String

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
        If IsNumeric(Mid(TmpLine, 1, 8)) Then
        Else
            Con_ELEMENT_SHELL_LSPRE = TmpLine
            Exit Function
        End If

        XMLFile.Write("<TABLE TYPE=""ELEMENT.TRIAD3"">" & vbCrLf)
        XMLFile.Write("<![CDATA[" & vbCrLf)
        XMLFile.Write("| ID    PART      N1      N2      N3      |" & vbCrLf)

        Do While Not (Mid(TmpLine, 1, 1) = "*" Or Mid(TmpLine, 1, 1) = "$")
            ELEMENT_ID = Mid(TmpLine, 1, 8)
            PID = Mid(TmpLine, 9, 8)
            '================================================================================
            If PARTsID(UBound(PARTsID)) <> PID Then
                ReDim Preserve PARTsID(UBound(PARTsID) + 1)
                PARTsID(UBound(PARTsID)) = PID
            End If
            '================================================================================
            Node1 = Mid(TmpLine, 17, 8)
            Node2 = Mid(TmpLine, 25, 8)
            Node3 = Mid(TmpLine, 33, 8)
            Node4 = Mid(TmpLine, 41, 8)

            If Node3 <> Node4 Then Exit Do

            XMLFile.Write(ELEMENT_ID & Chr(9) & PID & Chr(9) & Node1 & Chr(9) & Node2 & Chr(9) & Node3 & vbCrLf)

            TmpLine = KeyFile.ReadLine()
        Loop

        XMLFile.Write("]]>" & vbCrLf)
        XMLFile.Write("</TABLE>" & vbCrLf)

        If Mid(TmpLine, 1, 1) = "*" Or Mid(TmpLine, 1, 1) = "$" Then
            Con_ELEMENT_SHELL_LSPRE = TmpLine
            Exit Function
        End If

        XMLFile.Write("<TABLE TYPE=""ELEMENT.QUAD4"">" & vbCrLf)
        XMLFile.Write("<![CDATA[" & vbCrLf)
        XMLFile.Write("| ID    PART      N1      N2      N3      N4    |" & vbCrLf)

        Do While Not (Mid(TmpLine, 1, 1) = "*" Or Mid(TmpLine, 1, 1) = "$")
            ELEMENT_ID = Mid(TmpLine, 1, 8)
            PID = Mid(TmpLine, 9, 8)
            '================================================================================
            If PARTsID(UBound(PARTsID)) <> PID Then
                ReDim Preserve PARTsID(UBound(PARTsID) + 1)
                PARTsID(UBound(PARTsID)) = PID
            End If
            '================================================================================
            Node1 = Mid(TmpLine, 17, 8)
            Node2 = Mid(TmpLine, 25, 8)
            Node3 = Mid(TmpLine, 33, 8)
            Node4 = Mid(TmpLine, 41, 8)

            XMLFile.Write(ELEMENT_ID & Chr(9) & PID & Chr(9) & Node1 & Chr(9) & Node2 & Chr(9) & Node3 & Chr(9) & Node4 & vbCrLf)

            TmpLine = KeyFile.ReadLine()
        Loop

        XMLFile.Write("]]>" & vbCrLf)
        XMLFile.Write("</TABLE>" & vbCrLf)

        Con_ELEMENT_SHELL_LSPRE = TmpLine

    End Function

    Private Function Con_ELEMENT_SOLID_LSPRE(ByRef KeyFile As StreamReader, ByRef XMLFile As StreamWriter) As String

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
        If IsNumeric(Mid(TmpLine, 1, 8)) Then
        Else
            Con_ELEMENT_SOLID_LSPRE = TmpLine
            Exit Function
        End If

        XMLFile.Write("<TABLE TYPE=""ELEMENT.HEXA8"">" & vbCrLf)
        XMLFile.Write("<![CDATA[" & vbCrLf)
        XMLFile.Write("| ID    PART      N1      N2      N3      N4      N5      N6      N7      N8    |" & vbCrLf)

        '*NODE
        Do While Not (Mid(TmpLine, 1, 1) = "*" Or Mid(TmpLine, 1, 1) = "$")
            ELEMENT_ID = Mid(TmpLine, 1, 8)
            PID = Mid(TmpLine, 9, 8)
            '================================================================================
            If PARTsID(UBound(PARTsID)) <> PID Then
                ReDim Preserve PARTsID(UBound(PARTsID) + 1)
                PARTsID(UBound(PARTsID)) = PID
            End If
            '================================================================================
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

        Con_ELEMENT_SOLID_LSPRE = TmpLine

    End Function

    Private Sub FrmFEconverting_LSPRE_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        MainMDI.Statuslbl.Text = "Status"
    End Sub

    Private Sub FrmFEconverting_LSPRE_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If TempLicExpire = False Then
            '원격로그
            Try
                Using NewfileNum As StreamWriter = File.AppendText(RemoteValFolder & System.Environment.UserName)
                    NewfileNum.WriteLine("## LS-PrePost to Key File Set : " & Date.Now.ToString & " ##")
                    NewfileNum.Close()
                End Using
            Catch ex As Exception
                'End
            End Try
        End If

        MainMDI.Statuslbl.Text = "LS-PREPOST File Converting to MADYMO FE Model [Null Materials] ※There is a possibility that there is an error."
        Me.MdiParent = MainMDI
        Me.StatusLbl.Text = "Initialized..."
        Me.ToolTip1.SetToolTip(Me.DyOpnBtn, "Select Target File (LS-PREPOST Output File)" & vbCrLf & _
                                   "ex>" & vbCrLf & _
                                   "    *KEYWORD" & vbCrLf & _
                                   "    $TIME_VALUE = ##" & vbCrLf & _
                                   "    $STATE_NO = #" & vbCrLf & _
                                   "    $Output for State # at time = ##" & vbCrLf & _
                                   "    *ELEMENT_(Option)" & vbCrLf & _
                                   "    ." & vbCrLf & _
                                   "    ." & vbCrLf & _
                                   "    *NODE" & vbCrLf & _
                                   "    ." & vbCrLf & _
                                   "    ." & vbCrLf & _
                                   "    *END" & vbCrLf & _
                                   "      (Repeat)")
    End Sub

    Private Function Add_10_Letters(ByVal IDNumber As String) As String

        Dim SpaceNum As Integer
        Dim i As Integer

        '10글자가 넘어가면 앞에서 10개만 잘라버림
        If Len(IDNumber) > 10 Then
            Add_10_Letters = Mid(IDNumber, 1, 10)
            Exit Function
        End If

        SpaceNum = 10 - Len(IDNumber)
        For i = 1 To SpaceNum
            IDNumber = " " & IDNumber
        Next

        Add_10_Letters = IDNumber

    End Function
End Class