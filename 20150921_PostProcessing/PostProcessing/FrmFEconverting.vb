Imports System.IO

Public Class FrmFEconverting

    Private Sub DyOpnBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DyOpnBtn.Click

        Dim DlgResult As DialogResult

        Me.ResultTxt.Text = "File Selection..." & vbCrLf & vbCrLf
        ProStatus.Text = "Ready..."

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
            Me.ResultTxt.Text = Me.ResultTxt.Text & vbCrLf & "No File...." & vbCrLf & vbCrLf
            Exit Sub
        Else
            Me.DyNameTxt.Text = Me.KeyFileDlg.SafeFileName
        End If

        Dim CardsNum(16) As Integer
        Dim Tmp_Txt As String
        Dim fileNum As StreamReader
        Dim NewXMLfile As StreamWriter

        'LS-DYNA
        Dim ReadDYNA As New FileStream(Me.KeyFileDlg.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        fileNum = New StreamReader(ReadDYNA)

        'XML
        Dim SaveFileName As String()
        With Me.XMLsave
            .Title = "저장 파일 선택"
            SaveFileName = FileNameGet(Me.KeyFileDlg.FileNames)
            .FileName = SaveFileName(0)                  '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "MADYMO Input File|*.xml|모든 파일|*.*"
            DlgResult = .ShowDialog()
        End With

        If DlgResult = DialogResult.Cancel Then
            Me.ResultTxt.Text = Me.ResultTxt.Text & vbCrLf & "Cancel" & vbCrLf & vbCrLf
            Exit Sub
        Else
            Me.DyNameTxt.Text = Me.KeyFileDlg.SafeFileName
        End If

        NewXMLfile = New StreamWriter(Me.XMLsave.FileName)

        ReDim Element3(0)
        'ReDim Element4(0)
        Element4_cnt = 0

        Dim StartTime As Date = Date.Now

        Me.ResultTxt.Text = Me.ResultTxt.Text & "Start at : " & StartTime & vbCrLf & vbCrLf
        Me.ResultTxt.Text = Me.ResultTxt.Text & "▣ Key File Working Path : " & vbCrLf & FilePathGet2(Me.KeyFileDlg.FileName) & vbCrLf & vbCrLf
        Me.ResultTxt.Text = Me.ResultTxt.Text & "▣ Key File : " & vbCrLf & Me.KeyFileDlg.SafeFileNames(0) & vbCrLf & vbCrLf
        Dim Tmp_Xmlfile As String()
        Tmp_Xmlfile = Split(Me.XMLsave.FileNames(0), "\")
        Me.ResultTxt.Text = Me.ResultTxt.Text & "▣ Xml File : " & vbCrLf & Tmp_Xmlfile(UBound(Tmp_Xmlfile)) & vbCrLf & vbCrLf

        'NewXMLfile = New StreamWriter(FilePathGet(Me.KeyFileDlg.FileNames) & "\" & Me.XMLsave.FileName)

        '파일 헤더 쓰기
        NewXMLfile.WriteLine("<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>")
        NewXMLfile.WriteLine("<!DOCTYPE MADYMO SYSTEM ""mtd_3d.dtd"">")
        NewXMLfile.WriteLine("<MADYMO RELEASE=""R7.5"" >")
        NewXMLfile.WriteLine("<TYPEDEFS>")
        NewXMLfile.WriteLine("<INCLUDE FILE=""typedefs.xml""/>")
        NewXMLfile.WriteLine("</TYPEDEFS>")
        NewXMLfile.WriteLine("<RUNID/>")
        NewXMLfile.WriteLine("<CONTROL_ALLOCATION C_SIZE=""100000000"" I_SIZE=""100000000"" R_SIZE=""200000000"" NR_PROC=""4""/>")
        NewXMLfile.WriteLine("<CONTROL_ANALYSIS.TIME RACO=""0.01 0.1"" RAMP=""0.0 0.5"" INT_MTH=""EULER"" TIME_END=""0.12"" TIME_STEP=""1.000000E-05"" TIME_START=""0.0"" ANALYSIS_TYPE=""DYNAMIC"" CONSTRAINT_TOL=""1.000000E-09"">")
        NewXMLfile.WriteLine("</CONTROL_ANALYSIS.TIME>")
        NewXMLfile.WriteLine("<CONTROL_OUTPUT TIME_STEP=""1.000000E-04"" WRITE_DEBUG=""NONE"" PADDING_TIME=""0.01"" SCALE_FACTOR_ANI=""1"" FILTER_IGNORE=""OFF"" TIME_STEP_ANI=""0.005"">")
        NewXMLfile.WriteLine("<TIME_HISTORY_FE FE_MODEL=""/Converted_FE_SYSTEM/FE_MODEL"" AIRBAG_OUTPUT_LIST=""ALL""/>")
        NewXMLfile.WriteLine("</CONTROL_OUTPUT>")
        NewXMLfile.WriteLine("<SYSTEM.REF_SPACE ID=""1"" NAME=""S-INERTIAL_SPACE""/>")
        NewXMLfile.WriteLine("<SYSTEM.MODEL ID=""2"" NAME=""Converted_FE_SYSTEM"">")
        NewXMLfile.WriteLine("<COMMENT><![CDATA[ Using MADYMO DATA Viewer Converting Tool ]]></COMMENT>")
        NewXMLfile.WriteLine("<COMMENT><![CDATA[ by SungHun, Chang ]]></COMMENT>")
        NewXMLfile.WriteLine("<COMMENT><![CDATA[" & "Converted : " & Date.Now.ToString & "]]></COMMENT>")
        NewXMLfile.WriteLine("<COMMENT><![CDATA[ The developer is no Liability to reply any question of user or to modify the source code for USER convenience. ]]></COMMENT>")
        NewXMLfile.WriteLine("<FE_MODEL ID=""2"" NAME=""FE_MODEL"">")
        NewXMLfile.WriteLine("<CONTROL_FE_MODEL ALPHA_COEF=""0.0"" MASS_LUMP_MTH=""GEOMETRICAL""/>")
        NewXMLfile.WriteLine("<CONTROL_FE_TIME_STEP REDUCTION_FACTOR=""0.9"" CRITICAL_ELEMENTS=""20"" MIN_STEP=""1e-06"" MAX_STEP=""1e+60"" TIME_INT_MTH=""NORMAL"" NR_OF_CYCLES=""0""/>")

        Tmp_Txt = fileNum.ReadLine

        Do While Not fileNum.EndOfStream
            'If Mid(Tmp_Txt, 1, 1) = "*" Then
            Select Case Tmp_Txt
                Case "*MAT_NULL"
                    If Me.ChkNull.Checked = False Then
                        CardsNum(1) = CardsNum(1) + 1
                        Tmp_Txt = Con_MAT_NULL(fileNum, NewXMLfile, Me)
                        Application.DoEvents()
                    Else
                        Tmp_Txt = fileNum.ReadLine
                    End If
                Case "*MAT_RIGID"
                    If Me.ChkNull.Checked = False Then
                        CardsNum(2) = CardsNum(2) + 1
                        Tmp_Txt = fileNum.ReadLine
                    Else
                        Tmp_Txt = fileNum.ReadLine
                    End If
                Case "*MAT_PIECEWISE_LINEAR_PLASTICITY"
                    If Me.ChkNull.Checked = False Then
                        CardsNum(3) = CardsNum(3) + 1
                        Tmp_Txt = fileNum.ReadLine
                    Else
                        Tmp_Txt = fileNum.ReadLine
                    End If
                Case "*MAT_LOW_DENSITY_FOAM"
                    If Me.ChkNull.Checked = False Then
                        CardsNum(4) = CardsNum(4) + 1
                        Tmp_Txt = fileNum.ReadLine
                    Else
                        Tmp_Txt = fileNum.ReadLine
                    End If
                Case "*DATABASE_HISTORY_NODE"    '------?
                    CardsNum(5) = CardsNum(5) + 1
                    Tmp_Txt = fileNum.ReadLine
                Case "*NODE"
                    CardsNum(6) = CardsNum(6) + 1
                    Tmp_Txt = Con_NODE(fileNum, NewXMLfile, Me)
                    Application.DoEvents()
                Case "*ELEMENT_SHELL"
                    CardsNum(7) = CardsNum(7) + 1
                    Tmp_Txt = Con_ELEMENT_SHELL(fileNum, NewXMLfile, Me)
                    Application.DoEvents()
                Case "*ELEMENT_SHELL_THICKNESS_OFFSET"
                    CardsNum(7) = CardsNum(7) + 1
                    Tmp_Txt = Con_ELEMENT_SHELL_THICKNESS_OFFSET(fileNum, NewXMLfile, Me)
                    Application.DoEvents()
                Case "*ELEMENT_SOLID"
                    CardsNum(8) = CardsNum(8) + 1
                    Tmp_Txt = Con_ELEMENT_SOLID(fileNum, NewXMLfile, Me)
                    Application.DoEvents()
                Case "*PART", "*PART_CONTACT"
                    CardsNum(9) = CardsNum(9) + 1
                    If Tmp_Txt = "*PART" Then
                        Tmp_Txt = Con_PART(fileNum, NewXMLfile, Me)
                    ElseIf Tmp_Txt = "*PART_CONTACT" Then
                        Tmp_Txt = Con_PART_CONTACT(fileNum, NewXMLfile, Me)
                    End If
                Case "*SECTION_SHELL"
                    If Me.ChkNull.Checked = False Then
                        CardsNum(10) = CardsNum(10) + 1
                        Con_SECTION_SHELL(fileNum, NewXMLfile, Me)
                        Tmp_Txt = fileNum.ReadLine
                        Application.DoEvents()
                    Else
                        Tmp_Txt = fileNum.ReadLine
                    End If
                Case "*SECTION_SHELL_TITLE"
                    If Me.ChkNull.Checked = False Then
                        CardsNum(10) = CardsNum(10) + 1
                        Con_SECTION_SHELL(fileNum, NewXMLfile, Me, "TITLE")
                        Tmp_Txt = fileNum.ReadLine
                        Application.DoEvents()
                    Else
                        Tmp_Txt = fileNum.ReadLine
                    End If
                Case "*SECTION_SOLID", "*SECTION_SOLID_TITLE"
                    If Me.ChkNull.Checked = False Then
                        CardsNum(11) = CardsNum(11) + 1
                        If Tmp_Txt = "*SECTION_SOLID" Then
                            Con_SECTION_SOLID8(fileNum, NewXMLfile, Me, "Default")
                        ElseIf Tmp_Txt = "*SECTION_SOLID_TITLE" Then
                            Con_SECTION_SOLID8(fileNum, NewXMLfile, Me, "TITLE")
                        End If
                        Tmp_Txt = fileNum.ReadLine
                        Application.DoEvents()
                    Else
                        Tmp_Txt = fileNum.ReadLine
                    End If
                Case "*CONSTRAINED_NODAL_RIGID_BODY"
                    CardsNum(12) = CardsNum(12) + 1
                    Tmp_Txt = Con_CONSTRAINED_NODAL_RIGID_BODY(fileNum, NewXMLfile, Me)
                    Application.DoEvents()
                Case "*DEFINE_TABLE"
                    CardsNum(13) = CardsNum(13) + 1
                    Tmp_Txt = fileNum.ReadLine
                    Application.DoEvents()
                Case "*DEFINE_CURVE"
                    CardsNum(14) = CardsNum(14) + 1
                    Tmp_Txt = fileNum.ReadLine
                    Application.DoEvents()
                Case "*SET_NODE_LIST"
                    CardsNum(15) = CardsNum(15) + 1
                    Tmp_Txt = Con_SET_NODE(fileNum, NewXMLfile, Me)
                    Application.DoEvents()
                Case "*SET_PART_LIST"
                    CardsNum(16) = CardsNum(16) + 1
                    Tmp_Txt = Con_SET_PART(fileNum, NewXMLfile, Me)
                    Application.DoEvents()
                Case Else
                    Tmp_Txt = fileNum.ReadLine
            End Select
            ' End If
        Loop

        'Write Element shell============================================================================
        NewXMLfile.Write("<TABLE TYPE=""ELEMENT.TRIAD3"">" & vbCrLf)
        NewXMLfile.Write("<![CDATA[" & vbCrLf)
        NewXMLfile.Write("| ID    PART      N1      N2      N3      |" & vbCrLf)
        For i = 1 To UBound(Element3)
            NewXMLfile.WriteLine(Element3(i))
        Next
        NewXMLfile.Write("]]>" & vbCrLf)
        NewXMLfile.Write("</TABLE>" & vbCrLf)


        NewXMLfile.Write("<TABLE TYPE=""ELEMENT.QUAD4"">" & vbCrLf)
        NewXMLfile.Write("<![CDATA[" & vbCrLf)
        NewXMLfile.Write("| ID    PART      N1      N2      N3      N4    |" & vbCrLf)
        For i = 0 To Element4_cnt - 1
            NewXMLfile.WriteLine(Element4(i))
        Next
        NewXMLfile.Write("]]>" & vbCrLf)
        NewXMLfile.Write("</TABLE>" & vbCrLf)
        '===============================================================================================


        If EntireNodes.Count > 1 Then
            Dim i As Integer = 0
            Dim k As Integer = 0
            Dim j As Integer = 0
            Dim SETLine As String = ""

            For k = 0 To UBound(EntireNodes) - 1
                For i = 1 + k To UBound(EntireNodes)
                    Select Case EntireNodes(k) = EntireNodes(i)
                        Case True
                            ReDim Preserve DuplicationNodes(UBound(DuplicationNodes) + 1)
                            DuplicationNodes(UBound(DuplicationNodes)) = EntireNodes(i)
                            Exit For
                    End Select
                Next
            Next

            Dim DupliNodeSETFile As StreamWriter
            'DupliNodeSETFile = New StreamWriter(FilePathGet(Me.XMLsave.FileNames) & Me.XMLsave.FileName(0) & "DuplicateNodesInMADYMO.k")
            If File.Exists(FileNameGet(Me.XMLsave.FileNames)(0) & "_DuplicateNodesInMADYMO.k") Then
                File.Delete(FileNameGet(Me.XMLsave.FileNames)(0) & "_DuplicateNodesInMADYMO.k")
            End If
            DupliNodeSETFile = New StreamWriter(FileNameGet(Me.XMLsave.FileNames)(0) & "_DuplicateNodesInMADYMO.k")

            DupliNodeSETFile.WriteLine("*SET_NODE_LIST")
            DupliNodeSETFile.WriteLine("$HMSET")
            DupliNodeSETFile.WriteLine("$DuplicateNodesInMADYMO")
            DupliNodeSETFile.WriteLine("  99999998")

            For i = 1 To UBound(DuplicationNodes) Step 8
                For j = 0 To 7
                    If i + j <= UBound(DuplicationNodes) Then
                        SETLine = SETLine & Add_10_Letters(CStr(DuplicationNodes(i + j)))
                    Else
                        Exit For
                    End If
                Next
                DupliNodeSETFile.WriteLine(SETLine)
                SETLine = ""
            Next

            DupliNodeSETFile.Close()
        End If

        ProStatus.Text = "Complete..."
        ResultTxt.Text = vbCrLf & ResultTxt.Text & vbCrLf & "◆ MAT_NULL Cards : " & CardsNum(1) & " --→ Converted"
        ResultTxt.Text = ResultTxt.Text & vbCrLf & "◆ MAT_RIGID Cards : " & CardsNum(2)
        ResultTxt.Text = ResultTxt.Text & vbCrLf & "◆ MAT_PIECEWISE_LINEAR_PLASTICITYCards : " & CardsNum(3)
        ResultTxt.Text = ResultTxt.Text & vbCrLf & "◆ MAT_LOW_DENSITY_FOAM Cards : " & CardsNum(4)
        ResultTxt.Text = ResultTxt.Text & vbCrLf & "◆ DATABASE_HISTORY_NODE Cards : " & CardsNum(5)
        ResultTxt.Text = ResultTxt.Text & vbCrLf & "◆ NODE Cards : " & CardsNum(6)
        ResultTxt.Text = ResultTxt.Text & vbCrLf & "◆ ELEMENT_SHELL Cards : " & CardsNum(7) & " --→ Converted"
        ResultTxt.Text = ResultTxt.Text & vbCrLf & "◆ ELEMENT_SOLID Cards : " & CardsNum(8) & " --→ Converted"
        ResultTxt.Text = ResultTxt.Text & vbCrLf & "◆ PART Cards : " & CardsNum(9) & " --→ Converted"
        ResultTxt.Text = ResultTxt.Text & vbCrLf & "◆ SECTION_SHELL Cards : " & CardsNum(10) & " --→ Converted"
        ResultTxt.Text = ResultTxt.Text & vbCrLf & "◆ SECTION_SOLID Cards : " & CardsNum(11) & " --→ Converted"
        ResultTxt.Text = ResultTxt.Text & vbCrLf & "◆ CONSTRAINED_NODAL_RIGID_BODY Cards : " & CardsNum(12) & " --→ Converted"
        ResultTxt.Text = ResultTxt.Text & vbCrLf & "◆ DEFINE_TABLE Cards : " & CardsNum(13)
        ResultTxt.Text = ResultTxt.Text & vbCrLf & "◆ DEFINE_CURVE Cards : " & CardsNum(14)
        ResultTxt.Text = ResultTxt.Text & vbCrLf & "◆ SET_NODE_LIST Cards : " & CardsNum(15) & " --→ Converted"
        ResultTxt.Text = ResultTxt.Text & vbCrLf & "◆ SET_PART_LIST Cards : " & CardsNum(16) & " --→ Converted"

        'Material INCLUDE
        If Me.ChkMaterials.Checked = True Then
            'Write Material Elements
            NewXMLfile.WriteLine("<INCLUDE FILE=""MADYMO_FE_Materials.xml""/>")
            'FilePathGet(Me.XMLsave.FileNames)
            'Application.StartupPath & "\DATA\MADYMO_FE_Materials.xml"
            If File.Exists(FilePathGet(Me.XMLsave.FileNames) & "MADYMO_FE_Materials.xml") = False Then
                File.Copy(Application.StartupPath & "\DATA\MADYMO_FE_Materials.xml", FilePathGet(Me.XMLsave.FileNames) & "MADYMO_FE_Materials.xml")
            End If
        Else
            ResultTxt.Text = ResultTxt.Text & vbCrLf & vbCrLf & "** Material Cards are NOT included.**"
        End If

        'Write Null Material
        If Me.ChkNull.Checked = True Then
            NewXMLfile.WriteLine("<MATERIAL.NULL")
            NewXMLfile.WriteLine("ID=""1""")
            NewXMLfile.WriteLine("NAME=""MAT-NULL""")
            NewXMLfile.WriteLine("DENSITY_NULL=""0.0""")
            NewXMLfile.WriteLine("/>")

            NewXMLfile.WriteLine("<PROPERTY.SHELL4")
            NewXMLfile.WriteLine("ID=""1""")
            NewXMLfile.WriteLine("NAME=""PROP-NULLSHELL""")
            NewXMLfile.WriteLine("THICK=""" & CStr(NullShellThickness) & """")
            NewXMLfile.WriteLine("/>")
        End If

        NewXMLfile.WriteLine("</FE_MODEL>")
        NewXMLfile.WriteLine("</SYSTEM.MODEL>")
        NewXMLfile.WriteLine("</MADYMO>")

        If DuplicationNodes.Count > 1 Then
            ResultTxt.Text = ResultTxt.Text & vbCrLf & vbCrLf & vbCrLf & "***** Duplication Nodes in RIGID_ELEMENT *****" & vbCrLf
            For k = 1 To UBound(DuplicationNodes)
                ResultTxt.Text = ResultTxt.Text & vbCrLf & Add_10_Letters(DuplicationNodes(k))
            Next
        Else
            File.Delete(FileNameGet(Me.XMLsave.FileNames)(0) & "_DuplicateNodesInMADYMO.k")
        End If

        ReDim DuplicationNodes(0)
        ReDim EntireNodes(0)
        ReDim RigidGroupNum(0)

        ResultTxt.Text = ResultTxt.Text & vbCrLf & vbCrLf & "** Elapsed Time [system] : " & DateDiff(DateInterval.Second, StartTime, Date.Now) & " seconds"

        NewXMLfile.Close()
        fileNum.Close()
        ReadDYNA.Close()
    End Sub

    Private Sub FrmFEconverting_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        MainMDI.Statuslbl.Text = "Status"
    End Sub

    Private Sub FrmFEconverting_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If TempLicExpire = False Then
            '원격로그
            Try
                Using NewfileNum As StreamWriter = File.AppendText(RemoteValFolder & System.Environment.UserName)
                    NewfileNum.WriteLine("## LS-Dyna to Madymo : " & Date.Now.ToString & " ##")
                    NewfileNum.Close()
                End Using
            Catch ex As Exception
                'End
            End Try
        End If

        MainMDI.Statuslbl.Text = "LS-DYNA Key file format to MADYMO XML file. ※ Here is a possibility that there is a portion that does not change."
        Me.DescriptionLbl.Text = "※ FE Materials are NOT converted. Use STANDARD material card."
        Me.Size = New Point(500, 800)
        Me.MdiParent = MainMDI
    End Sub

    Private Sub FrmFEconverting_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        If Me.Width > 200 And Me.Height > 200 Then
            With Me
                .DyNameTxt.Location = New Point(5, 10)
                .DyOpnBtn.Size = New Size(130, 25)
                .DyNameTxt.Size = New Size(.ClientRectangle.Width - .DyOpnBtn.Width - 10 - 10, 25)
                .ChkMaterials.Location = New Point(.DyNameTxt.Location.X, .DyNameTxt.Location.Y + .DyNameTxt.Height + 5)
                .ChkNull.Location = New Point(.ChkMaterials.Location.X, .ChkMaterials.Location.Y + .ChkMaterials.Height + 5)
                .DyOpnBtn.Location = New Point(.DyNameTxt.Width + .DyNameTxt.Location.X + 5, .DyNameTxt.Location.Y)
                .ResultTxt.Location = New Point(.ChkNull.Location.X, .ChkNull.Location.Y + .ChkNull.Height + 5)
                .ResultTxt.Size = New Size(.ClientRectangle.Width - 10, .ClientRectangle.Height - .ChkNull.Height - .ChkMaterials.Height - .CardDescription.Height - .DyNameTxt.Height - .DescriptionLbl.Height - .StatusStrip1.Height - 45)
                .DescriptionLbl.Location = New Point(.DyNameTxt.Location.X, .ResultTxt.Location.Y + .ResultTxt.Height + 5)
                .CardDescription.Location = New Point(.DescriptionLbl.Location.X, .DescriptionLbl.Location.Y + .DescriptionLbl.Height + 5)
            End With
        End If
    End Sub

    Private Sub ChkNull_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkNull.CheckedChanged
        Dim ChildFrmThickness As New ThicknessDlgFEconverting

        If Me.ChkNull.Checked = True Then

            If ChildFrmThickness.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                'MessageBox.Show("Customer ID: " & ChildFrmThickness.TextBox1.Text)
                NullShellThickness = CSng(ChildFrmThickness.TextBox1.Text)
                Me.ChkNull.Text = "Convert All Material to Null Material [Null Property Thinkness : " & ChildFrmThickness.TextBox1.Text & "m ]"
                Me.ChkNull.ForeColor = Color.Red
            End If

            IsNullMode = True
        Else
            IsNullMode = False
            Me.ChkNull.ForeColor = Color.Black
            Me.ChkNull.Text = "Convert All Material to Null Material"
        End If
    End Sub
End Class