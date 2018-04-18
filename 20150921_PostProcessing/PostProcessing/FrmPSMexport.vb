Imports System.IO

Public Class FrmPSMexport

    Dim Times() As Single
    Dim Elements() As Integer
    Dim INITIAL_NODE_X() As Double
    Dim INITIAL_NODE_Y() As Double
    Dim INITIAL_NODE_Z() As Double
    Dim Undefomed_POS_X_L() As Double
    Dim Undefomed_POS_Y_L() As Double
    Dim Undefomed_POS_Z_L() As Double
    Dim X_Ref_Disp() As Double
    Dim Y_Ref_Disp() As Double
    Dim Z_Ref_Disp() As Double
    Dim X_TEST_Disp() As Double
    Dim Y_TEST_Disp() As Double
    Dim Z_TEST_Disp() As Double
    Dim Yawing() As Double   'in degree
    Dim Pitching() As Double   'in degree
    Dim YawingTEST() As Double   'in degree
    Dim PitchingTEST() As Double   'in degree
    Dim Reference_X_Point As Double
    Dim Reference_Y_Point As Double
    Dim Reference_Z_Point As Double
    Dim MainFileName As String
    Dim YawingFile As String
    Dim PitchingFile As String
    Dim YawingTESTFile As String
    Dim PitchingTESTFile As String
    Dim ModelInitialAngleDeg As Double
    Dim IsTrans As Boolean

    Dim NodeList() As Integer 'Excluding Nodes
    Dim NodesPos() As Integer = Nothing ' Excluding Node Position


    Public Function FilePathGet(ByVal Tmp_Str() As String) As String
        '파일의 경로만 가져오는 글로벌 함수.
        Tmp_Str(0) = StrReverse(Tmp_Str(0))
        Tmp_Str(0) = Mid(Tmp_Str(0), InStr(Tmp_Str(0), "\")).ToString
        Tmp_Str(0) = StrReverse(Tmp_Str(0))
        Return Tmp_Str(0)
    End Function

    Private Sub FrmPSMexport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.StatusLbl.Text = "PSM File Converter Loaded"
        With Me
            .ProgressBar1.Value = 0
            .RefBoxTEST.Enabled = False
            .RefBoxTEST_2.Enabled = False

            .AnalysisTab.TabPages(0).Text = "Reference Point Translation (Analysis)"
            .AnalysisTab.TabPages(1).Text = "Reference Point Rotation (Analysis)"
            .AnalysisTab.TabPages(2).Text = "Excluding Node Set (Optional)"
            .TESTTab.TabPages(0).Text = "Reference Point Translation (TEST)"
            .TESTTab.TabPages(1).Text = "Reference Point Rotation (TEST)"

            .TxtToolTip.SetToolTip(.CmdX, "Select Reference X Coordinate Curve (Analysis)" & vbCrLf & " - Positive X Direction : Backward of Vehicle" & vbCrLf & " - Conventional Right-Hand Rule" & vbCrLf & " - Dimension : meter")
            .TxtToolTip.SetToolTip(.CmdY, "Select Reference Y Coordinate Curve (Analysis)" & vbCrLf & " - Positive Y Direction : From DRV Side To PAS Side" & vbCrLf & " - Conventional Right-Hand Rule" & vbCrLf & " - Dimension : meter")
            .TxtToolTip.SetToolTip(.CmdZ, "Select Reference Z Coordinate Curve (Analysis)" & vbCrLf & " - Positive Z Direction : Upward of Vehicle" & vbCrLf & " - Conventional Right-Hand Rule" & vbCrLf & " - Dimension : meter")
            .TxtToolTip.SetToolTip(.SelMainFile, "Select Target File" & vbCrLf & " → Node Coordinate Time History File") ' (LS-PREPOST Output File)" & vbCrLf & _
            '"ex>" & vbCrLf & _
            '"    *KEYWORD" & vbCrLf & _
            '"    $TIME_VALUE = ##" & vbCrLf & _
            '"    $STATE_NO = #" & vbCrLf & _
            '"    $Output for State # at time = ##" & vbCrLf & _
            '"    *ELEMENT_(Option)" & vbCrLf & _
            '"    ." & vbCrLf & _
            '"    ." & vbCrLf & _
            '"    *NODE" & vbCrLf & _
            '"    ." & vbCrLf & _
            '"    ." & vbCrLf & _
            '"    *END" & vbCrLf & _
            '"      (Repeat)")
            .TxtToolTip.SetToolTip(.CmdXTest, "Select Reference X Coordinate Curve (Test)" & vbCrLf & " - Pulse Applied Model is not neccesary" & vbCrLf & " - Dimension : meter")
            .TxtToolTip.SetToolTip(.CmdYTest, "Select Reference Y Coordinate Curve (Test)" & vbCrLf & " - Dimension : meter")
            .TxtToolTip.SetToolTip(.CmdZTest, "Select Reference Z Coordinate Curve (Test)" & vbCrLf & " - Dimension : meter")
            .TxtToolTip.SetToolTip(.CmdYawing, "Select Yawing Curve (Analysis)" & vbCrLf & " - Dimension : degrees" & vbCrLf & " - Positive Z Direction : Upward of Vehicle" & vbCrLf & " - Conventional Right-Hand Rule")
            .TxtToolTip.SetToolTip(.CmdPitch, "Select Pitching Curve (Analysis)" & vbCrLf & " - Dimension : degrees" & vbCrLf & " - Positive Y Direction : From DRV Side To PAS Side" & vbCrLf & " - Conventional Right-Hand Rule")
            .TxtToolTip.SetToolTip(.ReferencePtX, "Initial Reference X Coordinate" & vbCrLf & " - Dimension : meter")
            .TxtToolTip.SetToolTip(.ReferencePtY, "Initial Reference Y Coordinate" & vbCrLf & " - Dimension : meter")
            .TxtToolTip.SetToolTip(.ReferencePtZ, "Initial Reference Z Coordinate" & vbCrLf & " - Dimension : meter")
            .TxtToolTip.SetToolTip(.CmdYawingTEST, "Select Yawing Curve (Analysis)" & vbCrLf & " - Dimension : degrees" & vbCrLf & " - Positive Z Direction : Upward of Vehicle" & vbCrLf & " - Conventional Right-Hand Rule")
            .TxtToolTip.SetToolTip(.CmdPitchTEST, "Select Pitching Curve (Analysis)" & vbCrLf & " - Dimension : degrees" & vbCrLf & " - Positive Y Direction : From DRV Side To PAS Side" & vbCrLf & " - Conventional Right-Hand Rule")
            .TxtToolTip.SetToolTip(.DispScaleX, "Deformation Scale" & vbCrLf & " - X Direction")
            .TxtToolTip.SetToolTip(.DispScaleY, "Deformation Scale" & vbCrLf & " - Y Direction")
            .TxtToolTip.SetToolTip(.DispScaleZ, "Deformation Scale" & vbCrLf & " - Z Direction")
            .TxtToolTip.SetToolTip(.ModelAngleTxt, "Initial Model Angle" & vbCrLf & " - Yaw Angle (in degrees)" & vbCrLf & " - Positive Z Direction : Upward of Vehicle" & vbCrLf & " - Conventional Right-Hand Rule")
        End With

        MsgBox("If the number which is used in curve file is incorrect or there is no consistency in format," & vbCrLf & _
               "There is a possibility that the error occurs." & vbCrLf & _
               "[Truncation / Round off / CLR CallBack...]" & vbCrLf & _
               "Please note at the time of use.", MsgBoxStyle.Exclamation, "Caution")

    End Sub

    Private Sub CmdY_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdY.Click

        Me.ProgressBar1.Value = 0

        Dim i As Integer
        Dim Tmp_Str As String
        Dim Tmp_Str_Split() As String

        With Me.OpenFileDialog1
            .Title = "Y Reference 파일 선택"
            .Multiselect = False
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "dat파일|*.dat|모든 파일|*.*"
            .ShowDialog()
        End With

        If Me.OpenFileDialog1.FileName = "" Then Exit Sub

        Me.StatusLbl.Text = "Reference Y Curve Loaded"

        Me.TxtYcurve.Text = Me.OpenFileDialog1.FileName

        Dim ReadFiles As New FileStream(Me.OpenFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        Dim FileReading As StreamReader

        FileReading = New StreamReader(ReadFiles)

        i = 0

        Do While Not FileReading.EndOfStream 'InStr(Tmp_Str, "ENDATA") = 1
            Tmp_Str = RTrim(FileReading.ReadLine)
            If InStr(Tmp_Str, " ") >= 1 Then
                Tmp_Str_Split = Tmp_Str.Split(" ")
            Else
                Tmp_Str_Split = Tmp_Str.Split(Chr(9))
            End If

            If InStr(Tmp_Str, "ENDATA") = 0 And Tmp_Str <> "" And _
                IsNumeric(Tmp_Str_Split(UBound(Tmp_Str_Split))) = True Then

                ReDim Preserve Y_Ref_Disp(i)
                Y_Ref_Disp(i) = CDbl(Tmp_Str_Split(UBound(Tmp_Str_Split))) / 1000
                i = i + 1
            End If
        Loop

        ''하이퍼뷰
        ''임시코드 ㅡ,.ㅡ? 이상하게 2개가 더 있다.
        'ReDim Preserve Y_Ref_Disp(33)
        'Y_Ref_Disp(32) = Y_Ref_Disp(31)
        'Y_Ref_Disp(33) = Y_Ref_Disp(31)

        Me.CmdY.ForeColor = Color.Green
        Me.TxtYcurve.Select(Me.TxtYcurve.Text.Length, 0)
        Me.FileToolTip.SetToolTip(Me.TxtYcurve, Me.TxtYcurve.Text)

        FileReading.Close()
        ReadFiles.Close()
    End Sub

    Private Sub CmdZ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdZ.Click

        Me.ProgressBar1.Value = 0

        Dim i As Integer
        Dim Tmp_Str As String
        Dim Tmp_Str_Split() As String

        With Me.OpenFileDialog1
            .Title = "Z Reference 파일 선택"
            .Multiselect = False
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "dat파일|*.dat|모든 파일|*.*"
            .ShowDialog()
        End With

        If Me.OpenFileDialog1.FileName = "" Then Exit Sub

        Me.StatusLbl.Text = "Reference Z Curve Loaded"

        Me.TxtZcurve.Text = Me.OpenFileDialog1.FileName

        Dim ReadFiles As New FileStream(Me.OpenFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        Dim FileReading As StreamReader

        FileReading = New StreamReader(ReadFiles)

        i = 0

        Do While Not FileReading.EndOfStream 'InStr(Tmp_Str, "ENDATA") = 1
            Tmp_Str = RTrim(FileReading.ReadLine)
            If InStr(Tmp_Str, " ") >= 1 Then
                Tmp_Str_Split = Tmp_Str.Split(" ")
            Else
                Tmp_Str_Split = Tmp_Str.Split(Chr(9))
            End If

            If InStr(Tmp_Str, "ENDATA") = 0 And Tmp_Str <> "" And _
                IsNumeric(Tmp_Str_Split(UBound(Tmp_Str_Split))) = True Then

                ReDim Preserve Z_Ref_Disp(i)
                Z_Ref_Disp(i) = CDbl(Tmp_Str_Split(UBound(Tmp_Str_Split))) / 1000
                i = i + 1
            End If
        Loop
        ''하이퍼뷰
        ''임시코드 ㅡ,.ㅡ? 이상하게 2개가 더 있다.
        'ReDim Preserve Z_Ref_Disp(33)
        'Z_Ref_Disp(32) = Z_Ref_Disp(31)
        'Z_Ref_Disp(33) = Z_Ref_Disp(31)

        Me.CmdZ.ForeColor = Color.Green
        Me.TxtZcurve.Select(Me.TxtZcurve.Text.Length, 0)
        Me.FileToolTip.SetToolTip(Me.TxtZcurve, Me.TxtZcurve.Text)

        FileReading.Close()
        ReadFiles.Close()
    End Sub

    Private Sub SelMainFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelMainFile.Click

        Me.ProgressBar1.Value = 0
        Me.TxtXcurveTEST.Text = ""
        Me.TxtYcurveTEST.Text = ""
        Me.TxtZcurveTEST.Text = ""


        Dim i, k As Integer
        Dim Tmp_Str As String
        Dim Tmp_Str_Split() As String

        With Me.OpenFileDialog1
            .Title = "Dyna 파일 선택"
            .Multiselect = False
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "모든 파일|*.*"
            .ShowDialog()
        End With

        If Me.OpenFileDialog1.FileName = "" Then Exit Sub

        Me.Enabled = False

        Me.StatusLbl.Text = "Reading Key File and Configuring Time Index....It may take more than a few minutes for this process..."

        MainFileName = Me.OpenFileDialog1.FileName

        Me.TextBox1.Text = Me.OpenFileDialog1.FileName
        Me.TextBox1.Update()

        Dim ReadFiles As New FileStream(Me.OpenFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        Dim FileReading As StreamReader

        FileReading = New StreamReader(ReadFiles,True)

        ReDim Times(0)
        ReDim Elements(0)

        i = -1
        k = -1

        Dim EventPass As Boolean = False

        Dim StartTime As DateTime

        Do While Not FileReading.EndOfStream
            Tmp_Str = FileReading.ReadLine
            Select Case Mid(Tmp_Str, 1, 4)
                Case "$TIM"  '시간을 따온다.=============================================================================
                    k = k + 1
                    ReDim Preserve Times(k)
                    Tmp_Str_Split = Tmp_Str.Split("=")
                    Times(k) = CSng(Tmp_Str_Split(UBound(Tmp_Str_Split)))
                Case "*NOD"  '=========================================================================================
                    If EventPass = False Then
                        '한번만 읽어오면 된다.
                        Tmp_Str = FileReading.ReadLine

                        StartTime = Now

                        Do While Mid(Tmp_Str, 1, 4) <> "*END"
                            i = i + 1
                            ReDim Preserve Elements(i)
                            'Elements(i) = CInt(Mid$(Tmp_Str, 1, 8))
                            Elements(i) = CInt(Tmp_Str.Substring(0, 8))
                            Tmp_Str = FileReading.ReadLine
                            Me.StatusLbl.Text = "Reading NODEs from Key File : # " & i & " - " & Now.Subtract(StartTime).Minutes & "min " & Now.Subtract(StartTime).Seconds & "sec Elapsed..."
                            Application.DoEvents()
                        Loop

                    End If
                    EventPass = True
            End Select
        Loop

        ReDim INITIAL_NODE_X(UBound(Elements))
        ReDim INITIAL_NODE_Y(UBound(Elements))
        ReDim INITIAL_NODE_Z(UBound(Elements))
        ReDim Undefomed_POS_X_L(UBound(Elements))
        ReDim Undefomed_POS_Y_L(UBound(Elements))

        FileReading.Close()
        ReadFiles.Close()

        ReDim X_TEST_Disp(UBound(Times))
        ReDim Y_TEST_Disp(UBound(Times))
        ReDim Z_TEST_Disp(UBound(Times))

        If UBound(Times) > UBound(X_Ref_Disp) Then
            For i = 0 To UBound(X_Ref_Disp)
                X_TEST_Disp(i) = X_Ref_Disp(i)
                Y_TEST_Disp(i) = Y_Ref_Disp(i)
                Z_TEST_Disp(i) = Z_Ref_Disp(i)
            Next
        Else
            For i = 0 To UBound(Times)
                X_TEST_Disp(i) = X_Ref_Disp(i)
                Y_TEST_Disp(i) = Y_Ref_Disp(i)
                Z_TEST_Disp(i) = Z_Ref_Disp(i)
            Next
        End If

        Me.SelMainFile.ForeColor = Color.Blue
        Me.FileToolTip.SetToolTip(Me.TextBox1, Me.TextBox1.Text)

        Me.StatusLbl.Text = "Configuring Complete.[Total " & Elements.Count & " NODEs] ->" & Now.Subtract(StartTime).Minutes & "min " & Now.Subtract(StartTime).Seconds & "sec Elapsed."

        Me.Enabled = True

    End Sub

    Private Sub Start_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Start.Click

        Me.StatusLbl.Text = "Start converting PSM file..."

        If IsNumeric(ReferencePtX.Text) Or IsNumeric(ReferencePtY.Text) Or IsNumeric(ReferencePtZ.Text) Then
        Else
            MsgBox("Incorrect Reference Coordinate", , "No Reference Pt")
            Me.StatusLbl.Text = "Error"
            Exit Sub
        End If
        If Me.TextBox1.Text = "" Then
            MsgBox("Select a PSM Dyna File", , "No Main File")
            Me.StatusLbl.Text = "Error"
            Exit Sub
        Else
        End If
        If YawingFile = "" Then
            MsgBox("Select Reference Yawing", , "No Yawing")
            Me.StatusLbl.Text = "Error"
            Me.CmdYawing.ForeColor = Color.Red
            Exit Sub
        End If
        If PitchingFile = "" Then
            MsgBox("Select Reference Pitching", , "No  Pitching")
            Me.CmdPitch.ForeColor = Color.Red
            Me.StatusLbl.Text = "Error"
            Exit Sub
        End If
        If IsNumeric(ModelAngleTxt.Text) Then
            ModelInitialAngleDeg = CDbl(Me.ModelAngleTxt.Text)
        Else
            MsgBox("Incorrect Model Initial Angle", , "NaN or Invalid Value in Model Angle")
            Me.Label11.ForeColor = Color.Red
            Me.StatusLbl.Text = "Error"
            Exit Sub
        End If
        Me.Label11.ForeColor = Color.Blue

        Me.Start.Enabled = False
        Application.DoEvents()

        Reference_X_Point = CDbl(Me.ReferencePtX.Text)
        Reference_Y_Point = CDbl(Me.ReferencePtY.Text)
        Reference_Z_Point = CDbl(Me.ReferencePtZ.Text)

        With Me
            If IsNumeric(.UnitConverter.Text) = False Or IsNumeric(.TxtXscale.Text) = False _
                Or IsNumeric(.TxtYscale.Text) = False Or IsNumeric(.TxtZscale.Text) = False Then
                MsgBox("Numeric Value Only..", MsgBoxStyle.Critical, "Warning")
                Exit Sub
            End If
            If .TxtXcurve.Text = "" Or .TxtYcurve.Text = "" Or .TxtZcurve.Text = "" Or .TextBox1.Text = "" Then
                MsgBox("Select File..", MsgBoxStyle.Critical, "Warning")
                Exit Sub
            End If
        End With

        Dim i, k As Integer
        Dim Tmp_Str As String
        Dim Min_Cnt As Integer
        Dim X_ref_correction As Single
        Dim Y_ref_correction As Single
        Dim Z_ref_correction As Single

        If UBound(Times) > UBound(X_Ref_Disp) Then
            Min_Cnt = UBound(X_Ref_Disp)
        Else
            Min_Cnt = UBound(Times)
        End If

        Me.ProgressBar1.Value = 0
        Me.ProgressBar1.Maximum = UBound(Times)

        '파일쓰기시작
        '헤더 및 요소 넘버 써준다. ===============================================================================================================================
        With Me.SaveFileDlg
            .Title = "저장 파일 선택"
            .FileName = ""                    '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "inc 파일|*.inc|모든 파일|*.*"
            .ShowDialog()
        End With

        If Me.SaveFileDlg.FileName = "" Then Exit Sub

        Dim NewPSMFile As StreamWriter
        NewPSMFile = New StreamWriter(Me.SaveFileDlg.FileName)

        Me.StatusLbl.Text = "Process Started Writing the Header Part in PSM"
        Application.DoEvents()

        NewPSMFile.WriteLine("#!<MOT,R5.3>")
        
        'NewPSMFile.WriteLine(Elements.Length & Chr(9) & "3")


        Tmp_Str = ""

        Dim h As Integer = 0
        Dim t As Integer = -1 ' For NodePos
        Dim Elimination As Boolean = False

        Dim NodeHeaderLines(0) As String
        NodeHeaderLines(0) = "Number Lines"

        If Me.ChkExNODE.Checked = False Then
            '노드를 제거하지 않고 PSM을 구성하는 경우
            For k = 0 To CInt(UBound(Elements) / 15) Step 1
                Try
                    For i = 0 To 14
                        Tmp_Str = Tmp_Str & Elements(15 * k + i) & Chr(9)
                    Next
                    '??????
                    If Tmp_Str = "" Then
                    Else
                        ReDim Preserve NodeHeaderLines(UBound(NodeHeaderLines) + 1)
                        NodeHeaderLines(UBound(NodeHeaderLines)) = Tmp_Str
                        'NewPSMFile.WriteLine(Tmp_Str)
                    End If
                    '??????
                Catch ex As Exception
                    If Tmp_Str = "" Then
                    Else
                        ReDim Preserve NodeHeaderLines(UBound(NodeHeaderLines) + 1)
                        NodeHeaderLines(UBound(NodeHeaderLines)) = Tmp_Str
                        'NewPSMFile.WriteLine(Tmp_Str)
                    End If
                End Try

                Tmp_Str = ""
            Next

            t = t + 1
            ReDim Preserve NodesPos(t)
            NodesPos(t) = -1
        Else
            '특정 노드를 제거하는 경우 (ex> CONSTRAINT_NODAL_RIGID 등)===========
            '             ***  Element 변수의 Start Index는 0이다.  ***

            For k = 0 To CInt(UBound(Elements) / 15) Step 1
                Try
                    For i = 0 To 14

                        For h = 0 To UBound(NodeList)
                            Select Case (NodeList(h) = Elements(15 * k + i))
                                Case True
                                    Elimination = True
                                    t = t + 1
                                    ReDim Preserve NodesPos(t)
                                    NodesPos(t) = 15 * k + i
                                    Exit For
                                Case Else

                            End Select
                        Next

                        'If Elements(15 * k + i) = 18001346 Then
                        '    MsgBox("df")
                        'End If

                        If Elimination = False Then
                            Tmp_Str = Tmp_Str & Elements(15 * k + i) & Chr(9)
                        End If
                        Elimination = False
                    Next

                    If Tmp_Str = "" Then
                    Else
                        If Tmp_Str = "" Then
                        Else
                            ReDim Preserve NodeHeaderLines(UBound(NodeHeaderLines) + 1)
                            NodeHeaderLines(UBound(NodeHeaderLines)) = Tmp_Str
                            'NewPSMFile.WriteLine(Tmp_Str)
                        End If
                    End If

                Catch ex As Exception
                    If Tmp_Str = "" Then
                    Else
                        ReDim Preserve NodeHeaderLines(UBound(NodeHeaderLines) + 1)
                        NodeHeaderLines(UBound(NodeHeaderLines)) = Tmp_Str
                        'NewPSMFile.WriteLine(Tmp_Str)
                    End If
                End Try

                Tmp_Str = ""
            Next

            'Array.Sort(NodesPos)
            '====================================================================
        End If
        '======================================================================================================================================================

        NodeHeaderLines(0) = CStr(Elements.Length - NodesPos.Count) & Chr(9) & "3"

        For i = 0 To UBound(NodeHeaderLines)
            NewPSMFile.WriteLine(RTrim(NodeHeaderLines(i)))
        Next

        'NewPSMFile.Close()

        '노드 데이터를 쓴다.==============================================================================================
        'Dim ReadEles As New FileStream(Me.OpenFileDialog1.FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)
        Dim ReadEles As New FileStream(MainFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)
        Dim FileReadingEle As StreamReader

        FileReadingEle = New StreamReader(ReadEles)

        k = 0
        Dim WriteCoordinate As String

        Dim IsFirstStep As Boolean = True

        Dim NODE_X As Double
        Dim NODE_Y As Double
        Dim NODE_Z As Double

        '모델의 이동량 (Rigid Body Motion - Translational)
        Dim TRANS_ANAL_X(UBound(Times)) As Double
        Dim TRANS_ANAL_Y(UBound(Times)) As Double
        Dim TRANS_ANAL_Z(UBound(Times)) As Double

        '초기 위치 (Rigid Body Motion - Translational)
        Dim INITIAL_COORD(3) As Double

        '보정 스케일 Factor
        Dim X_Correction_Scale As Double = CDbl(Me.TxtXscale.Text) / 100
        Dim Y_Correction_Scale As Double = CDbl(Me.TxtYscale.Text) / 100
        Dim Z_Correction_Scale As Double = CDbl(Me.TxtZscale.Text) / 100

        '길이 환산 Factor
        Dim Length_Conversion As Double = CDbl(Me.UnitConverter.Text)

        'Deformation Factor
        Dim X_Deform_Scale As Double = CDbl(Me.DispScaleX.Text)
        Dim Y_Deform_Scale As Double = CDbl(Me.DispScaleY.Text)
        Dim Z_Deform_Scale As Double = CDbl(Me.DispScaleZ.Text)

        Dim UndeformedPOS(3) As Double
        Dim DeformedPOSrotate(3) As Double ' 초기에 역각으로 돌림
        Dim DeformedPOSscaled(3) As Double ' 변형 스케일 완료된 좌표

        Dim NodePosCounter As Integer = 0

        Me.StatusLbl.Text = "Process Started Writing the Node Coordinate Part in PSM"

        Do While Not FileReadingEle.EndOfStream

            Tmp_Str = FileReadingEle.ReadLine
            Select Case Mid(Tmp_Str, 1, 4)
                Case "*NOD"
                    If IsFirstStep = True Then '첫번째 스텝

                        NewPSMFile.WriteLine(Format(Times(k), "0.00e+0"))  '----------------------------------------------Time을 써준다.

                        Me.StatusLbl.Text = "Process Started Writing the Node Coordinate Part in PSM : t = " & Format(Times(k), "0.00e+0")
                        Application.DoEvents()

                        For i = 0 To UBound(Elements)

                            '한 줄은 무조건 읽는다.
                            Tmp_Str = FileReadingEle.ReadLine

                            '카운터가 더 커지면 안되므로 UBOUND를 넣어줌
                            If NodePosCounter > UBound(NodesPos) Then NodePosCounter = UBound(NodesPos)

                            If NodesPos(NodePosCounter) = i Then
                                '쓰지않고 넘긴다
                                NodePosCounter = NodePosCounter + 1
                            Else
                                '쓴다
                                'Mid(Tmp_Str,9,16) 'X 좌표
                                'Mid(Tmp_Str,25,16) 'Y 좌표
                                'Mid(Tmp_Str,41,16) 'Z 좌표

                                '첫번째 노드 좌표들을 저장
                                INITIAL_NODE_X(i) = CDbl(Mid(Tmp_Str, 9, 16)) * 0.001
                                INITIAL_NODE_Y(i) = CDbl(Mid(Tmp_Str, 25, 16)) * 0.001
                                INITIAL_NODE_Z(i) = CDbl(Mid(Tmp_Str, 41, 16)) * 0.001

                                WriteCoordinate = Chr(9) & INITIAL_NODE_X(i) & Chr(9) & INITIAL_NODE_Y(i) & Chr(9) & INITIAL_NODE_Z(i)
                                NewPSMFile.WriteLine(WriteCoordinate)
                            End If
                        Next

                        k = k + 1
                        IsFirstStep = False

                    Else '첫번째 스텝이 아닌 경우

                        'Default 설정일 경우 Correction 계산을 제외한다.(변환이 조금 빨라진다.)
                        If Me.ChkDefault.Checked = False Then

                            '변환 Factor를 계산한다.============================================================================================
                            TRANS_ANAL_X(k) = X_Ref_Disp(0) - X_Ref_Disp(k)
                            TRANS_ANAL_Y(k) = Y_Ref_Disp(0) - Y_Ref_Disp(k)
                            TRANS_ANAL_Z(k) = Z_Ref_Disp(0) - Z_Ref_Disp(k)

                            NewPSMFile.WriteLine(Format(Times(k), "0.00e+0"))  'Time을 써준다. 해당 Time의 인덱스는 K이다.

                            Me.StatusLbl.Text = "Process Started Writing the Node Coordinate Part in PSM : t = " & Format(Times(k), "0.00e+0")
                            Application.DoEvents()

                            If Me.ChkTESTuse.Checked = True Then
                                X_ref_correction = TRANS_ANAL_X(k) * X_Correction_Scale + (X_TEST_Disp(k) - X_Ref_Disp(k))
                                Y_ref_correction = TRANS_ANAL_Y(k) * Y_Correction_Scale + (Y_TEST_Disp(k) - Y_Ref_Disp(k))
                                Z_ref_correction = TRANS_ANAL_Z(k) * Z_Correction_Scale + (Z_TEST_Disp(k) - Z_Ref_Disp(k))

                                'TRANS_ANAL_X(k) = TRANS_ANAL_X(k) * (1.0 - X_Correction_Scale) + (X_TEST_Disp(k) - X_Ref_Disp(k))
                                'TRANS_ANAL_Y(k) = TRANS_ANAL_Y(k) * (1.0 - Y_Correction_Scale) + (Y_TEST_Disp(k) - Y_Ref_Disp(k))
                                'TRANS_ANAL_Z(k) = TRANS_ANAL_Z(k) * (1.0 - Z_Correction_Scale) + (Z_TEST_Disp(k) - Z_Ref_Disp(k))
                            Else
                                X_ref_correction = TRANS_ANAL_X(k) * X_Correction_Scale
                                Y_ref_correction = TRANS_ANAL_Y(k) * Y_Correction_Scale
                                Z_ref_correction = TRANS_ANAL_Z(k) * Z_Correction_Scale

                                'TRANS_ANAL_X(k) = TRANS_ANAL_X(k) * (1.0 - X_Correction_Scale)
                                'TRANS_ANAL_Y(k) = TRANS_ANAL_Y(k) * (1.0 - Y_Correction_Scale)
                                'TRANS_ANAL_Z(k) = TRANS_ANAL_Z(k) * (1.0 - Z_Correction_Scale)
                            End If
                            '변환 Factor를 계산한다.============================================================================================

                            '******************************************************************************************************************
                            'Node Writing *****************************************************************************************************
                            '******************************************************************************************************************
                            NodePosCounter = 0

                            For i = 0 To UBound(Elements)

                                Tmp_Str = FileReadingEle.ReadLine

                                '카운터가 더 커지면 안되므로 UBOUND를 넣어줌
                                If NodePosCounter > UBound(NodesPos) Then NodePosCounter = UBound(NodesPos)

                                '제거할 노드는 넘어간다.
                                If NodesPos(NodePosCounter) = i Then
                                    '쓰지않고 넘긴다
                                    NodePosCounter = NodePosCounter + 1

                                Else

                                    If Mid(Tmp_Str, 1, 4) = "*END" Then Exit For
                                    'Mid(Tmp_Str,9,16) 'X 좌표
                                    'Mid(Tmp_Str,25,16) 'Y 좌표
                                    'Mid(Tmp_Str,41,16) 'Z 좌표

                                    ''============================================================= Angle Correction =========================================================================
                                    ''Yawing에 의한 X 보정을 한다.
                                    ''Undeformed X Position
                                    'Undefomed_POS_X_L(i) = INITIAL_NODE_X(i) + ((Reference_X_Point - INITIAL_NODE_X(i)) - (Reference_X_Point - INITIAL_NODE_X(i)) * Math.Cos(Yawing(k) / (180.0 / Math.PI)))
                                    ''                                           ------------------  L -----------------   --------------------- L * COS(pitching) ------------------------------------------
                                    ''Yawing에 의한 X 변화 → Y 보정을 한다.
                                    'Undefomed_POS_Y_L(i) = INITIAL_NODE_Y(i) + ((CDbl(Mid(Tmp_Str, 9, 16)) - Undefomed_POS_X_L(i)) * Math.Tan(Yawing(k) / (180 / Math.PI)))
                                    ''                                                (X_deformed     -     X_undeformed)         * tan(yawing)
                                    ''Pitching에 의한 X 보정을 한다.
                                    'Undefomed_POS_X_L(i) = Undefomed_POS_X_L(i) + ((Reference_X_Point - Undefomed_POS_X_L(i)) - (Reference_X_Point - Undefomed_POS_X_L(i)) * Math.Cos(Pitching(k) / (180.0 / Math.PI)))
                                    ''                                                ------------------  L -----------------   --------------------- L * COS(pitching) -------------------------------------------

                                    ''============================================================= Angle Correction =========================================================================

                                    'NODE_X = Undefomed_POS_X_L(i) + ((CDbl(Mid(Tmp_Str, 9, 16)) * Length_Conversion + X_ref_correction) - Undefomed_POS_X_L(i)) * X_Deform_Scale
                                    ''                                --------------------------- 이 부분이 X 변형량이다. ----------------------------------------

                                    'NODE_Y = Undefomed_POS_Y_L(i) + ((CDbl(Mid(Tmp_Str, 25, 16)) * Length_Conversion + Y_ref_correction) - Undefomed_POS_Y_L(i)) * Y_Deform_Scale
                                    ''                                --------------------------- 이 부분이 Y 변형량이다. ----------------------------------------

                                    'NODE_Z = INITIAL_NODE_Z(i) + ((CDbl(Mid(Tmp_Str, 41, 16)) * Length_Conversion + Z_ref_correction) - INITIAL_NODE_Z(i)) * Z_Deform_Scale

                                    '============================================================= Angle Correction =========================================================================
                                    ' 현 좌표를 Reference 중심 (-) Yawing +15 (ModelInitialAngleDeg)각으로 돌린다. (x축 상으로 올리는 작업)
                                    DeformedPOSrotate = OrientationZ({0, (Mid(Tmp_Str, 9, 16) * Length_Conversion + X_ref_correction) - (Reference_X_Point), _
                                                                     (Mid(Tmp_Str, 25, 16) * Length_Conversion + Y_ref_correction) - (Reference_Y_Point), _
                                                                     (Mid(Tmp_Str, 41, 16) * Length_Conversion + Z_ref_correction) - (Reference_Z_Point)}, _
                                                                     +Yawing(k) + ModelInitialAngleDeg) ' -15)

                                    ' 현 좌표를 Reference 중심 (-) Pitching 각으로 돌린다. (x축 상으로 올리는 작업)
                                    DeformedPOSrotate = OrientationY({0, DeformedPOSrotate(1), DeformedPOSrotate(2), DeformedPOSrotate(3)}, -Pitching(k)) '→ 이게 변형된 Mesh임

                                    ''Initial 좌표를 15도 (ModelInitialAngleDeg) 돌린다.
                                    INITIAL_COORD = OrientationZ({0, INITIAL_NODE_X(i) - (Reference_X_Point), INITIAL_NODE_Y(i) - (Reference_Y_Point), INITIAL_NODE_Z(i) - (Reference_Z_Point)}, ModelInitialAngleDeg) ' -15)
                                    ''→ 변형되지 않았을 때의 Mesh

                                    ' 변형량에 관련된 계산을 한다.
                                    DeformedPOSrotate(1) = DeformedPOSrotate(1) + (INITIAL_COORD(1) - DeformedPOSrotate(1)) * (1.0 - X_Deform_Scale)
                                    DeformedPOSrotate(2) = DeformedPOSrotate(2) + (INITIAL_COORD(2) - DeformedPOSrotate(2)) * (1.0 - Y_Deform_Scale)
                                    DeformedPOSrotate(3) = DeformedPOSrotate(3) + (INITIAL_COORD(3) - DeformedPOSrotate(3)) * (1.0 - Z_Deform_Scale)

                                    ' 바뀐 좌표를 복귀
                                    ' 복귀 시킬 때 Angle을 조정한다.
                                    ' Angle은 시험 데이터의 Angle로 변환한다.
                                    ' → 시험 데이터를 읽지 않았을 때 YawingTEST와 PitchingTEST변수에는 해석 데이터와 동일한 값이 채워져 있다.
                                    ' → 기본적으로 해석 데이터를 읽을 때 해석 각도는 시험 데이터 변수에도 들어가 있다.
                                    DeformedPOSrotate = OrientationY({0, DeformedPOSrotate(1), DeformedPOSrotate(2), DeformedPOSrotate(3)}, PitchingTEST(k))
                                    DeformedPOSrotate = OrientationZ({0, DeformedPOSrotate(1), DeformedPOSrotate(2), DeformedPOSrotate(3)}, -YawingTEST(k) - ModelInitialAngleDeg) '+ 15)

                                    '최종 변형된 좌표
                                    NODE_X = DeformedPOSrotate(1) + Reference_X_Point
                                    NODE_Y = DeformedPOSrotate(2) + Reference_Y_Point
                                    NODE_Z = DeformedPOSrotate(3) + Reference_Z_Point
                                    '============================================================= Angle Correction =========================================================================

                                    WriteCoordinate = Chr(9) & NODE_X & Chr(9) & NODE_Y & Chr(9) & NODE_Z

                                    NewPSMFile.WriteLine(WriteCoordinate)
                                End If
                            Next
                            '******************************************************************************************************************
                            'Node Writing *****************************************************************************************************
                            '******************************************************************************************************************

                            k = k + 1

                            '============ Time Step의 끝에 도달했는지 검사한다. =======================
                            If k > Min_Cnt Then Exit Do
                            '========================================================================

                        Else 'Default 설정일 경우 Correction 계산을 제외한다.(변환이 조금 빨라진다.)

                            '변환 Factor를 계산한다.============================================================================================
                            NewPSMFile.WriteLine(Format(Times(k), "0.00e+0"))  'Time을 써준다. 해당 Time의 인덱스는 K이다.

                            Me.StatusLbl.Text = "Process Started Writing the Node Coordinate Part in PSM : t = " & Format(Times(k), "0.00e+0")
                            Application.DoEvents()

                            TRANS_ANAL_X(k) = X_Ref_Disp(0) - X_Ref_Disp(k)
                            TRANS_ANAL_Y(k) = Y_Ref_Disp(0) - Y_Ref_Disp(k)
                            TRANS_ANAL_Z(k) = Z_Ref_Disp(0) - Z_Ref_Disp(k)

                            If Me.ChkTESTuse.Checked = True Then
                                X_ref_correction = TRANS_ANAL_X(k) * X_Correction_Scale + (X_TEST_Disp(k) - X_Ref_Disp(k))
                                Y_ref_correction = TRANS_ANAL_Y(k) * Y_Correction_Scale + (Y_TEST_Disp(k) - Y_Ref_Disp(k))
                                Z_ref_correction = TRANS_ANAL_Z(k) * Z_Correction_Scale + (Z_TEST_Disp(k) - Z_Ref_Disp(k))
                            Else
                                X_ref_correction = TRANS_ANAL_X(k) * X_Correction_Scale
                                Y_ref_correction = TRANS_ANAL_Y(k) * Y_Correction_Scale
                                Z_ref_correction = TRANS_ANAL_Z(k) * Z_Correction_Scale
                            End If
                            '변환 Factor를 계산한다.============================================================================================

                            '******************************************************************************************************************
                            'Node Writing *****************************************************************************************************
                            '******************************************************************************************************************

                            NodePosCounter = 0

                            For i = 0 To UBound(Elements)
                                Tmp_Str = FileReadingEle.ReadLine

                                '카운터가 더 커지면 안되므로 UBOUND를 넣어줌
                                If NodePosCounter > UBound(NodesPos) Then NodePosCounter = UBound(NodesPos)

                                '제거할 노드는 넘어간다.
                                If NodesPos(NodePosCounter) = i Then
                                    '쓰지않고 넘긴다
                                    NodePosCounter = NodePosCounter + 1

                                Else

                                    'Mid(Tmp_Str,9,16) 'X 좌표
                                    'Mid(Tmp_Str,25,16) 'Y 좌표
                                    'Mid(Tmp_Str,41,16) 'Z 좌표

                                    '============================================================= Angle Correction =========================================================================
                                    ' 현 좌표를 Reference 중심 (-) Yawing +15 각으로 돌린다. (x축 상으로 올리는 작업)
                                    DeformedPOSrotate = OrientationZ({0, (Mid(Tmp_Str, 9, 16) * Length_Conversion + X_ref_correction) - (Reference_X_Point), _
                                                                     (Mid(Tmp_Str, 25, 16) * Length_Conversion + Y_ref_correction) - (Reference_Y_Point), _
                                                                     (Mid(Tmp_Str, 41, 16) * Length_Conversion + Z_ref_correction) - (Reference_Z_Point)}, _
                                                                     +Yawing(k) + ModelInitialAngleDeg) '- 15)

                                    ' 현 좌표를 Reference 중심 (-) Pitching 각으로 돌린다. (x축 상으로 올리는 작업)
                                    DeformedPOSrotate = OrientationY({0, DeformedPOSrotate(1), DeformedPOSrotate(2), DeformedPOSrotate(3)}, -Pitching(k)) '→ 이게 변형된 Mesh임

                                    ''Initial 좌표를 15도 돌린다.
                                    INITIAL_COORD = OrientationZ({0, INITIAL_NODE_X(i) - (Reference_X_Point), INITIAL_NODE_Y(i) - (Reference_Y_Point), INITIAL_NODE_Z(i) - (Reference_Z_Point)}, ModelInitialAngleDeg) ', -15)
                                    ''→ 변형되지 않았을 때의 Mesh

                                    ' 변형량에 관련된 계산을 한다.
                                    DeformedPOSrotate(1) = DeformedPOSrotate(1) + (INITIAL_COORD(1) - DeformedPOSrotate(1)) * (1.0 - X_Deform_Scale)
                                    DeformedPOSrotate(2) = DeformedPOSrotate(2) + (INITIAL_COORD(2) - DeformedPOSrotate(2)) * (1.0 - Y_Deform_Scale)
                                    DeformedPOSrotate(3) = DeformedPOSrotate(3) + (INITIAL_COORD(3) - DeformedPOSrotate(3)) * (1.0 - Z_Deform_Scale)

                                    ' 바뀐 좌표를 복귀
                                    ' 복귀 시킬 때 Angle을 조정한다.
                                    ' Angle은 시험 데이터의 Angle로 변환한다.
                                    ' → 시험 데이터를 읽지 않았을 때 YawingTEST와 PitchingTEST변수에는 해석 데이터와 동일한 값이 채워져 있다.
                                    DeformedPOSrotate = OrientationY({0, DeformedPOSrotate(1), DeformedPOSrotate(2), DeformedPOSrotate(3)}, PitchingTEST(k))
                                    DeformedPOSrotate = OrientationZ({0, DeformedPOSrotate(1), DeformedPOSrotate(2), DeformedPOSrotate(3)}, -YawingTEST(k) - ModelInitialAngleDeg) '+ 15)

                                    '최종 변형된 좌표
                                    NODE_X = DeformedPOSrotate(1) + Reference_X_Point
                                    NODE_Y = DeformedPOSrotate(2) + Reference_Y_Point
                                    NODE_Z = DeformedPOSrotate(3) + Reference_Z_Point
                                    '============================================================= Angle Correction =========================================================================

                                    WriteCoordinate = Chr(9) & NODE_X & Chr(9) & NODE_Y & Chr(9) & NODE_Z

                                    NewPSMFile.WriteLine(WriteCoordinate)
                                End If
                            Next
                            '******************************************************************************************************************
                            'Node Writing *****************************************************************************************************
                            '******************************************************************************************************************

                            k = k + 1

                            '============ Time Step의 끝에 도달했는지 검사한다. =======================
                            If k > Min_Cnt Then Exit Do
                            '========================================================================
                        End If

                    End If
                    Me.ProgressBar1.Value = k - 1
                    Application.DoEvents()
                Case Else

            End Select
        Loop


        Me.ProgressBar1.Value = Me.ProgressBar1.Maximum
        Application.DoEvents()

        NewPSMFile.Close()
        

        FileReadingEle.Close()
        ReadEles.Close()

        Dim NewLogFile As StreamWriter
        NewLogFile = New StreamWriter(Mid(Me.SaveFileDlg.FileName, 1, Len(Me.SaveFileDlg.FileName) - 3) & "PSM_log")

        NewLogFile.WriteLine(vbCrLf & vbCrLf & "###################################################################################################")
        NewLogFile.WriteLine("  ")
        NewLogFile.WriteLine("                                 MADYMO(R) PSM File Converting Log       ")
        NewLogFile.WriteLine("   ")
        NewLogFile.WriteLine("###################################################################################################" & vbCrLf)

        NewLogFile.WriteLine("###################################################################################################")
        NewLogFile.WriteLine(" ")
        NewLogFile.WriteLine("        PSM Coord. Converting Program By C.S.H (T.0149)  ")
        NewLogFile.WriteLine(" ")
        NewLogFile.WriteLine("        1. This program adopted a coordinate system of HMC STANDARD Vehicle Analysis Model.        ")
        NewLogFile.WriteLine("             -> X-direction : Backward of Vehicle                                                  ")
        NewLogFile.WriteLine("             -> Y-direction : Rightside of Vehicle                                                 ")
        NewLogFile.WriteLine("             -> Z-direction : Upward of Vehicle                                                    ")
        NewLogFile.WriteLine("                                                                                                   ")
        NewLogFile.WriteLine("        2. Target File                                                                             ")
        NewLogFile.WriteLine("             -> LS-DYNA keyfile format (by LS-PREPOST)                                             ")
        NewLogFile.WriteLine("                                                                                                   ")
        NewLogFile.WriteLine("###################################################################################################" & vbCrLf & vbCrLf & vbCrLf)
        NewLogFile.WriteLine("#### Target file : " & TextBox1.Text & vbCrLf)
        NewLogFile.WriteLine("#### Total Nodes : " & UBound(Elements) + 1)
        If IsNothing(NodeList) Then
            NewLogFile.WriteLine("#### Total Nodes in *SET_NODE_LIST : None")
        Else
            NewLogFile.WriteLine("#### Total Nodes in *SET_NODE_LIST : " & UBound(NodeList) + 1)
        End If
        NewLogFile.WriteLine("#### Total Eliminated Nodes : " & UBound(NodesPos) + 1)
        NewLogFile.WriteLine("")
        NewLogFile.WriteLine("#### Total Time Steps : " & UBound(Times) + 1)
        For i = 0 To UBound(Times)
            NewLogFile.WriteLine("   " & Times(i))
        Next

        NewLogFile.WriteLine(vbCrLf & "######## Model Initial Angle ########")
        NewLogFile.WriteLine("   Angle(in degree) : " & CDbl(Me.ModelAngleTxt.Text))

        NewLogFile.WriteLine(vbCrLf & "######## Reference Initial Coordinate ########")
        NewLogFile.WriteLine("   X coord : " & Reference_X_Point)
        NewLogFile.WriteLine("   Y coord : " & Reference_Y_Point)
        NewLogFile.WriteLine("   Z coord : " & Reference_Z_Point)

        NewLogFile.WriteLine(vbCrLf & "######## Deformation Scale ########")
        NewLogFile.WriteLine("   X scale : " & X_Deform_Scale)
        NewLogFile.WriteLine("   Y scale : " & Y_Deform_Scale)
        NewLogFile.WriteLine("   Z scale : " & Z_Deform_Scale)

        NewLogFile.WriteLine(vbCrLf & "######## Reference Point Coordinate ########")
        NewLogFile.WriteLine("   X Reference Curve File : " & TxtXcurve.Text)
        NewLogFile.WriteLine("   Y Reference Curve File : " & TxtYcurve.Text)
        NewLogFile.WriteLine("   Z Reference Curve File : " & TxtZcurve.Text & vbCrLf)

        NewLogFile.WriteLine("######## X Reference Curve DATA ########")
        For i = 0 To UBound(X_Ref_Disp)
            NewLogFile.WriteLine(" " & X_Ref_Disp(i))
        Next
        NewLogFile.WriteLine(vbCrLf & "######## Y Reference Curve DATA ########")
        For i = 0 To UBound(Y_Ref_Disp)
            NewLogFile.WriteLine(" " & Y_Ref_Disp(i))
        Next
        NewLogFile.WriteLine(vbCrLf & "######## Z Reference Curve DATA ########")
        For i = 0 To UBound(Y_Ref_Disp)
            NewLogFile.WriteLine(" " & Z_Ref_Disp(i))
        Next

        NewLogFile.WriteLine(vbCrLf & "######## Reference Point Relative Rotation ########")
        NewLogFile.WriteLine("#### Yawing Curve File : " & YawingFile)
        NewLogFile.WriteLine("#### Pitching Curve File : " & PitchingFile & vbCrLf)
        NewLogFile.WriteLine("######## Yawing Curve DATA ########")
        For i = 0 To UBound(Yawing)
            NewLogFile.WriteLine(" " & Yawing(i))
        Next
        NewLogFile.WriteLine(vbCrLf & "######## Pitching Curve DATA ########")
        For i = 0 To UBound(Pitching)
            NewLogFile.WriteLine(" " & Pitching(i))
        Next

        NewLogFile.WriteLine(vbCrLf & "######## TEST Point Coordinate ########")
        NewLogFile.WriteLine(" X TEST Curve File : " & TxtXcurveTEST.Text)
        NewLogFile.WriteLine(" Y TEST Curve File : " & TxtYcurveTEST.Text)
        NewLogFile.WriteLine(" Z TEST Curve File : " & TxtZcurveTEST.Text & vbCrLf)

        NewLogFile.WriteLine("######## X TEST Curve DATA ########")
        If Me.TxtXcurveTEST.Text = "" Then
            NewLogFile.WriteLine(" NONE ")
        Else
            For i = 0 To UBound(X_TEST_Disp)
                NewLogFile.WriteLine(" " & X_TEST_Disp(i))
            Next
        End If

        NewLogFile.WriteLine(vbCrLf & "######## Y TEST Curve DATA ########")
        If Me.TxtYcurveTEST.Text = "" Then
            NewLogFile.WriteLine(" NONE ")
        Else
            For i = 0 To UBound(Y_TEST_Disp)
                NewLogFile.WriteLine(" " & Y_TEST_Disp(i))
            Next
        End If

        NewLogFile.WriteLine(vbCrLf & "######## Z TEST Curve DATA ########")
        If Me.TxtZcurveTEST.Text = "" Then
            NewLogFile.WriteLine(" NONE ")
        Else
            For i = 0 To UBound(Z_TEST_Disp)
                NewLogFile.WriteLine(" " & Z_TEST_Disp(i))
            Next
        End If

        NewLogFile.WriteLine(vbCrLf & "######## TEST Point Relative Rotation ########")
        If YawingTESTFile = "" Then
            NewLogFile.WriteLine("#### TEST Yawing Curve File : NONE")
        Else
            NewLogFile.WriteLine("#### TEST Yawing Curve File : " & YawingFile)
        End If
        If PitchingTESTFile = "" Then
            NewLogFile.WriteLine("#### TEST Pitching Curve File : NONE")
        Else
            NewLogFile.WriteLine("#### TEST Pitching Curve File : " & PitchingFile & vbCrLf)
        End If

        NewLogFile.WriteLine(vbCrLf & "######## TEST Yawing Curve DATA ########")

        If Me.TxtYawingTEST.Text = "" Then
            NewLogFile.WriteLine(" NONE ")
        Else
            For i = 0 To UBound(YawingTEST)
                NewLogFile.WriteLine(" " & YawingTEST(i))
            Next
        End If

        NewLogFile.WriteLine(vbCrLf & "######## TEST Pitching Curve DATA ########")

        If Me.TxtPitchingTEST.Text = "" Then
            NewLogFile.WriteLine(" NONE ")
        Else
            For i = 0 To UBound(PitchingTEST)
                NewLogFile.WriteLine(" " & PitchingTEST(i))
            Next
        End If

        NewLogFile.Close()

        Me.Start.Enabled = True

        Me.StatusLbl.Text = "PSM File Converting Process Complete"

    End Sub

    Private Sub ChkDefault_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkDefault.CheckedChanged
        With Me
            .TxtXscale.Text = "100.0"
            .TxtYscale.Text = "0.0"
            .TxtZscale.Text = "0.0"

            If Me.ChkDefault.Checked = True Then
                Me.GroupBox1.Enabled = False
            Else
                Me.GroupBox1.Enabled = True
            End If
        End With
    End Sub

    Private Sub CmdXTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdXTest.Click
        Me.ProgressBar1.Value = 0

        Dim i As Integer
        Dim Tmp_Str As String
        Dim Tmp_Str_Split() As String

        With Me.OpenFileDialog1
            .Title = "파일 선택"
            .Multiselect = False
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "dat파일|*.dat|모든 파일|*.*"
            .ShowDialog()
        End With

        If Me.OpenFileDialog1.FileName = "" Then Exit Sub

        Me.StatusLbl.Text = "TEST X Curve Loaded"

        Me.TxtXcurveTEST.Text = Me.OpenFileDialog1.FileName

        Dim ReadFiles As New FileStream(Me.OpenFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        Dim FileReading As StreamReader

        FileReading = New StreamReader(ReadFiles)

        i = 0

        Do While Not FileReading.EndOfStream 'InStr(Tmp_Str, "ENDATA") = 1
            Tmp_Str = RTrim(FileReading.ReadLine)
            If InStr(Tmp_Str, " ") >= 1 Then
                Tmp_Str_Split = Tmp_Str.Split(" ")
            Else
                Tmp_Str_Split = Tmp_Str.Split(Chr(9))
            End If

            If InStr(Tmp_Str, "ENDATA") = 0 And Tmp_Str <> "" And _
                IsNumeric(Tmp_Str_Split(UBound(Tmp_Str_Split))) = True Then

                ReDim Preserve X_TEST_Disp(i)
                X_TEST_Disp(i) = CDbl(Tmp_Str_Split(UBound(Tmp_Str_Split))) / 1000
                i = i + 1
            End If
        Loop

        Me.CmdXTest.ForeColor = Color.Blue
        Me.FileToolTip.SetToolTip(Me.TxtXcurveTEST, Me.TxtXcurveTEST.Text)

        FileReading.Close()
        ReadFiles.Close()
    End Sub

    Private Sub CmdYTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdYTest.Click
        Me.ProgressBar1.Value = 0

        Dim i As Integer
        Dim Tmp_Str As String
        Dim Tmp_Str_Split() As String

        With Me.OpenFileDialog1
            .Title = "TEST Y Curve 파일 선택"
            .Multiselect = False
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "dat파일|*.dat|모든 파일|*.*"
            .ShowDialog()
        End With

        If Me.OpenFileDialog1.FileName = "" Then Exit Sub

        Me.StatusLbl.Text = "TEST Y Curve Loaded"

        Me.TxtYcurveTEST.Text = Me.OpenFileDialog1.FileName

        Dim ReadFiles As New FileStream(Me.OpenFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        Dim FileReading As StreamReader

        FileReading = New StreamReader(ReadFiles)

        i = 0

        Do While Not FileReading.EndOfStream 'InStr(Tmp_Str, "ENDATA") = 1
            Tmp_Str = RTrim(FileReading.ReadLine)
            If InStr(Tmp_Str, " ") >= 1 Then
                Tmp_Str_Split = Tmp_Str.Split(" ")
            Else
                Tmp_Str_Split = Tmp_Str.Split(Chr(9))
            End If

            If InStr(Tmp_Str, "ENDATA") = 0 And Tmp_Str <> "" And _
                IsNumeric(Tmp_Str_Split(UBound(Tmp_Str_Split))) = True Then

                ReDim Preserve Y_TEST_Disp(i)
                Y_TEST_Disp(i) = CDbl(Tmp_Str_Split(UBound(Tmp_Str_Split))) / 1000
                i = i + 1
            End If
        Loop

        Me.CmdYTest.ForeColor = Color.Blue
        Me.FileToolTip.SetToolTip(Me.TxtYcurveTEST, Me.TxtYcurveTEST.Text)

        FileReading.Close()
        ReadFiles.Close()
    End Sub

    Private Sub CmdZTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdZTest.Click
        Me.ProgressBar1.Value = 0

        Dim i As Integer
        Dim Tmp_Str As String
        Dim Tmp_Str_Split() As String

        With Me.OpenFileDialog1
            .Title = "TEST Z Curve파일 선택"
            .Multiselect = False
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "dat파일|*.dat|모든 파일|*.*"
            .ShowDialog()
        End With

        If Me.OpenFileDialog1.FileName = "" Then Exit Sub

        Me.StatusLbl.Text = "TEST Z Curve Loaded"

        Me.TxtZcurveTEST.Text = Me.OpenFileDialog1.FileName

        Dim ReadFiles As New FileStream(Me.OpenFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        Dim FileReading As StreamReader

        FileReading = New StreamReader(ReadFiles)

        i = 0

        Do While Not FileReading.EndOfStream 'InStr(Tmp_Str, "ENDATA") = 1
            Tmp_Str = RTrim(FileReading.ReadLine)
            If InStr(Tmp_Str, " ") >= 1 Then
                Tmp_Str_Split = Tmp_Str.Split(" ")
            Else
                Tmp_Str_Split = Tmp_Str.Split(Chr(9))
            End If

            If InStr(Tmp_Str, "ENDATA") = 0 And Tmp_Str <> "" And _
                IsNumeric(Tmp_Str_Split(UBound(Tmp_Str_Split))) = True Then

                ReDim Preserve Z_TEST_Disp(i)
                Z_TEST_Disp(i) = CDbl(Tmp_Str_Split(UBound(Tmp_Str_Split))) / 1000
                i = i + 1
            End If
        Loop

        Me.CmdZTest.ForeColor = Color.Blue
        Me.FileToolTip.SetToolTip(Me.TxtZcurveTEST, Me.TxtZcurveTEST.Text)

        FileReading.Close()
        ReadFiles.Close()
    End Sub

    Private Sub ChkTESTuse_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkTESTuse.CheckedChanged

        Dim i As Integer = 0

        If Me.ChkTESTuse.Checked = True Then

            Me.RefBoxTEST.Enabled = True
            Me.RefBoxTEST_2.Enabled = True

            If IsNothing(YawingTEST) Or IsNothing(PitchingTEST) Then
            Else
                ReDim YawingTEST(UBound(Yawing))
                ReDim PitchingTEST(UBound(Pitching))
            End If

        Else

            Me.RefBoxTEST.Enabled = False
            Me.RefBoxTEST_2.Enabled = False

            If IsNothing(YawingTEST) Or IsNothing(PitchingTEST) Then
            Else
                For i = 0 To UBound(Yawing)
                    YawingTEST(i) = Yawing(i)
                    PitchingTEST(i) = Pitching(i)
                Next
            End If

        End If

    End Sub

    Private Sub GraphX_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GraphX.Click
        IsTrans = True
        Dim DATAview As New FrmPSMgraph(IsTrans, Times, X_Ref_Disp, Y_Ref_Disp, Z_Ref_Disp, X_TEST_Disp, Y_TEST_Disp, Z_TEST_Disp)
        'Dim DATAview As New FrmPSMgraph({0, 0.01, 0.02, 0.03, 0.04, 0.05, 0.06, 0.07, 0.08, 0.09}, X_Ref_Disp, Y_Ref_Disp, Z_Ref_Disp, X_TEST_Disp, Y_TEST_Disp, Z_TEST_Disp)
        DATAview.Show()
        DATAview.Owner = Me
    End Sub

    Private Sub CmdYawing_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdYawing.Click
        Me.ProgressBar1.Value = 0

        Dim i As Integer
        Dim Tmp_Str As String
        Dim Tmp_Str_Split() As String

        With Me.OpenFileDialog1
            .Title = "Reference Yawing 파일 선택"
            .Multiselect = False
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "dat파일|*.dat|모든 파일|*.*"
            .ShowDialog()
        End With

        If Me.OpenFileDialog1.FileName = "" Then Exit Sub

        Me.StatusLbl.Text = "Reference Yawing Curve Loaded"

        Me.TxtYawing.Text = Me.OpenFileDialog1.FileName

        Dim ReadFiles As New FileStream(Me.OpenFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        YawingFile = Me.OpenFileDialog1.FileName
        Dim FileReading As StreamReader

        FileReading = New StreamReader(ReadFiles)

        i = 0

        Do
            Tmp_Str = FileReading.ReadLine
            If InStr(Tmp_Str, " ") >= 1 Then
                Tmp_Str_Split = Tmp_Str.Split(" ")
            Else
                Tmp_Str_Split = Tmp_Str.Split(Chr(9))
            End If

            If InStr(Tmp_Str, "ENDATA") = 0 And Tmp_Str <> "" And _
                IsNumeric(Tmp_Str_Split(UBound(Tmp_Str_Split))) = True Then

                ReDim Preserve Yawing(i)
                ReDim Preserve YawingTEST(i)
                Yawing(i) = CDbl(Tmp_Str_Split(UBound(Tmp_Str_Split)))
                YawingTEST(i) = CDbl(Tmp_Str_Split(UBound(Tmp_Str_Split)))

                i = i + 1
            End If
        Loop Until FileReading.EndOfStream 'InStr(Tmp_Str, "ENDATA") = 1

        ''하이퍼뷰
        ''임시코드 ㅡ,.ㅡ? 이상하게 2개가 더 있다.
        'ReDim Preserve X_Ref_Disp(33)
        'X_Ref_Disp(32) = X_Ref_Disp(31)
        'X_Ref_Disp(33) = X_Ref_Disp(31)

        Me.TxtYawing.Select(Me.TxtYawing.Text.Length, 0)
        Me.CmdYawing.ForeColor = Color.Green

        FileReading.Close()
        ReadFiles.Close()
    End Sub

    Private Sub CmdPitch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdPitch.Click
        Me.ProgressBar1.Value = 0

        Dim i As Integer
        Dim Tmp_Str As String
        Dim Tmp_Str_Split() As String

        With Me.OpenFileDialog1
            .Title = "Reference Pitching 파일 선택"
            .Multiselect = False
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "dat파일|*.dat|모든 파일|*.*"
            .ShowDialog()
        End With

        If Me.OpenFileDialog1.FileName = "" Then Exit Sub

        Me.StatusLbl.Text = "Reference Pitching Curve Loaded"

        Me.TxtPitching.Text = Me.OpenFileDialog1.FileName

        Dim ReadFiles As New FileStream(Me.OpenFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        PitchingFile = Me.OpenFileDialog1.FileName
        Dim FileReading As StreamReader

        FileReading = New StreamReader(ReadFiles)

        i = 0

        Do While Not FileReading.EndOfStream 'InStr(Tmp_Str, "ENDATA") = 1
            Tmp_Str = FileReading.ReadLine
            If InStr(Tmp_Str, " ") >= 1 Then
                Tmp_Str_Split = Tmp_Str.Split(" ")
            Else
                Tmp_Str_Split = Tmp_Str.Split(Chr(9))
            End If

            If InStr(Tmp_Str, "ENDATA") = 0 And Tmp_Str <> "" And _
                IsNumeric(Tmp_Str_Split(UBound(Tmp_Str_Split))) = True Then

                ReDim Preserve Pitching(i)
                ReDim Preserve PitchingTEST(i)
                Pitching(i) = CDbl(Tmp_Str_Split(UBound(Tmp_Str_Split)))
                PitchingTEST(i) = CDbl(Tmp_Str_Split(UBound(Tmp_Str_Split)))
                i = i + 1
            End If
        Loop

        ''하이퍼뷰
        ''임시코드 ㅡ,.ㅡ? 이상하게 2개가 더 있다.
        'ReDim Preserve X_Ref_Disp(33)
        'X_Ref_Disp(32) = X_Ref_Disp(31)
        'X_Ref_Disp(33) = X_Ref_Disp(31)

        Me.TxtPitching.Select(Me.TxtPitching.Text.Length, 0)
        Me.CmdPitch.ForeColor = Color.Green

        FileReading.Close()
        ReadFiles.Close()
    End Sub

    Private Sub FrmPSMexport_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        With Me

            .Width = 975
            .Height = 680

            .AnalysisTab.Location = New Point(7, 12)
            .AnalysisTab.Size = New Size(952, 165)
            .TextBox1.Location = New Point(.AnalysisTab.Location.X, .AnalysisTab.Location.Y + .AnalysisTab.Height + 7)
            .TextBox1.Size = New Size(.AnalysisTab.Width, 22)
            .ModelAngleTxt.Location = New Point(.AnalysisTab.Location.X, .TextBox1.Location.Y + .TextBox1.Height + 3)
            .SelMainFile.Location = New Point(.AnalysisTab.Location.X + .AnalysisTab.Width - 170, .TextBox1.Location.Y + .TextBox1.Height + 5)
            .DescriptionLbl.Location = New Point(.SelMainFile.Location.X - 120, .ModelAngleTxt.Location.Y)
            .Label11.Location = New Point(.ModelAngleTxt.Location.X + .ModelAngleTxt.Width + 2, .ModelAngleTxt.Location.Y + 5)
            .TESTTab.Location = New Point(.AnalysisTab.Location.X, .ModelAngleTxt.Location.Y + .ModelAngleTxt.Height + 15)
            .TESTTab.Size = New Size(.AnalysisTab.Width, .AnalysisTab.Height)
            .GraphX.Location = New Point(.TESTTab.Location.X + .TESTTab.Width - 340, .TESTTab.Location.Y + .TESTTab.Height + 5)
            .GraphX.Size = New Size(170, 33)
            .GraphAngle.Location = New Point(.GraphX.Location.X + .GraphX.Width + 10, .GraphX.Location.Y)
            .GraphAngle.Size = New Size(160, 33)
            .ChkTESTuse.Location = New Point(.GraphX.Location.X - 170, .GraphX.Location.Y + 7)
            .GroupBox1.Location = New Point(.AnalysisTab.Location.X, .GraphX.Location.Y + .GraphX.Height + 15)
            .GroupBox1.Size = New Size(590, 110)
            .GroupBox2.Location = New Point(.GroupBox1.Location.X + .GroupBox1.Width + 10, .GroupBox1.Location.Y)
            .GroupBox2.Size = New Size(Me.ClientRectangle.Width - .GroupBox1.Location.X - .GroupBox1.Width - 20, .GroupBox1.Height)
            .ChkDefault.Location = New Point(.GroupBox1.Location.X, .GroupBox1.Location.Y + .GroupBox1.Height + 5)
            .Start.Size = New Size(.GraphX.Width, .GraphX.Height)
            .Start.Location = New Point(.GroupBox2.Location.X + .GroupBox2.Width - .Start.Width, .GroupBox2.Location.Y + .GroupBox2.Height + 5)

            .RefBox_2.Location = New Point(.RefBox.Location.X, .RefBox.Location.Y)
            .RefBox_2.Size = New Size(.RefBox.Width, .RefBox.Height)
            .TxtYawing.Location = New Point(.TxtXcurve.Location.X, .TxtXcurve.Location.Y)
            .TxtYawing.Size = New Size(.TxtXcurve.Width, .TxtXcurve.Height)
            .TxtPitching.Location = New Point(.TxtYcurve.Location.X, .TxtYcurve.Location.Y)
            .TxtPitching.Size = New Size(.TxtYcurve.Width, .TxtYcurve.Height)
            .CmdYawing.Location = New Point(.CmdX.Location.X, .CmdX.Location.Y)
            .CmdYawing.Size = New Size(.CmdX.Width, .CmdX.Height)
            .CmdPitch.Location = New Point(.CmdY.Location.X, .CmdY.Location.Y)
            .CmdPitch.Size = New Size(.CmdX.Width, .CmdX.Height)

            .LblExNode.Location = New Point(.TxtPitching.Location.X, .TxtPitching.Location.Y)

            .ExNODE_BOX.Location = New Point(.RefBox.Location.X, .RefBox.Location.Y)
            .ExNODE_BOX.Size = New Size(.RefBox.Width, .RefBox.Height)
            .TxtNodeSET.Location = New Point(.TxtXcurve.Location.X, .TxtXcurve.Location.Y)
            .TxtNodeSET.Size = New Size(.TxtXcurve.Width, .TxtXcurve.Height)
            .CmdSelNODESET.Location = New Point(.CmdX.Location.X, .CmdX.Location.Y)
            .CmdSelNODESET.Size = New Size(.CmdX.Width, .CmdX.Height)
            .ChkExNODE.Location = New Point(.CmdSelNODESET.Location.X, .LblExNode.Location.Y)

            .RefBoxTEST.Location = New Point(.RefBox.Location.X, .RefBox.Location.Y)
            .RefBoxTEST.Size = New Size(.RefBox.Width, .RefBox.Height)
            .RefBoxTEST_2.Location = New Point(.RefBox.Location.X, .RefBox.Location.Y)
            .RefBoxTEST_2.Size = New Size(.RefBox.Width, .RefBox.Height)
            .TxtXcurveTEST.Location = New Point(.TxtXcurve.Location.X, .TxtXcurve.Location.Y)
            .TxtXcurveTEST.Size = New Size(.TxtXcurve.Width, .TxtXcurve.Height)
            .TxtYcurveTEST.Location = New Point(.TxtYcurve.Location.X, .TxtYcurve.Location.Y)
            .TxtYcurveTEST.Size = New Size(.TxtYcurve.Width, .TxtYcurve.Height)
            .TxtZcurveTEST.Location = New Point(.TxtZcurve.Location.X, .TxtZcurve.Location.Y)
            .TxtZcurveTEST.Size = New Size(.TxtZcurve.Width, .TxtZcurve.Height)
            .TxtYawingTEST.Location = New Point(.TxtXcurveTEST.Location.X, .TxtXcurveTEST.Location.Y)
            .TxtYawingTEST.Size = New Size(.TxtXcurveTEST.Width, .TxtXcurveTEST.Height)
            .TxtPitchingTEST.Location = New Point(.TxtYcurveTEST.Location.X, .TxtYcurveTEST.Location.Y)
            .TxtPitchingTEST.Size = New Size(.TxtYcurveTEST.Width, .TxtYcurveTEST.Height)
            .CmdXTest.Location = New Point(.CmdX.Location.X, .CmdX.Location.Y)
            .CmdXTest.Size = New Size(.CmdX.Width, .CmdX.Height)
            .CmdYTest.Location = New Point(.CmdY.Location.X, .CmdY.Location.Y)
            .CmdYTest.Size = New Size(.CmdX.Width, .CmdX.Height)
            .CmdZTest.Location = New Point(.CmdY.Location.X, .CmdZ.Location.Y)
            .CmdZTest.Size = New Size(.CmdX.Width, .CmdX.Height)
            .CmdYawingTEST.Location = New Point(.CmdXTest.Location.X, .CmdXTest.Location.Y)
            .CmdYawingTEST.Size = New Size(.CmdXTest.Width, .CmdXTest.Height)
            .CmdPitchTEST.Location = New Point(.CmdYTest.Location.X, .CmdYTest.Location.Y)
            .CmdPitchTEST.Size = New Size(.CmdXTest.Width, .CmdXTest.Height)

            .UnitConverter.Location = New Point(30, 50)
            .UnitConverter.Size = New Size(120, 25)
            .TxtXscale.Size = New Size(120, 25)
            .TxtYscale.Size = New Size(120, 25)
            .TxtZscale.Size = New Size(120, 25)
            .TxtXscale.Location = New Point(.UnitConverter.Location.X + .UnitConverter.Width + 20, UnitConverter.Location.Y)
            .TxtYscale.Location = New Point(.TxtXscale.Location.X + .TxtXscale.Width + 10, UnitConverter.Location.Y)
            .TxtZscale.Location = New Point(.TxtYscale.Location.X + .TxtYscale.Width + 10, UnitConverter.Location.Y)
            .UnitLbl.Location = New Point(.UnitConverter.Location.X, .UnitConverter.Location.Y - 20)
            .Label2.Location = New Point(.TxtXscale.Location.X, .TxtXscale.Location.Y - 20)
            .Label3.Location = New Point(.TxtYscale.Location.X, .TxtYscale.Location.Y - 20)
            .Label4.Location = New Point(.TxtZscale.Location.X, .TxtZscale.Location.Y - 20)
            .Label5.Location = New Point(.TxtXscale.Location.X, .TxtXscale.Location.Y + .TxtXscale.Height + 5)

            .DispScaleX.Location = New Point(5, 35)
            .DispScaleX.Size = New Size((.GroupBox2.Width - 20) / 3, 22)
            .DispScaleY.Size = New Size(.DispScaleX.Width, 22)
            .DispScaleZ.Size = New Size(.DispScaleX.Width, 22)
            .ReferencePtX.Size = .DispScaleX.Size
            .ReferencePtY.Size = .DispScaleY.Size
            .ReferencePtZ.Size = .DispScaleZ.Size
            .DispScaleY.Location = New Point(.DispScaleX.Location.X + .DispScaleX.Width + 5, .DispScaleX.Location.Y)
            .DispScaleZ.Location = New Point(.DispScaleY.Location.X + .DispScaleY.Width + 5, .DispScaleX.Location.Y)
            .ReferencePtX.Location = New Point(.DispScaleX.Location.X, .DispScaleX.Location.Y + .DispScaleX.Height + 5)
            .ReferencePtY.Location = New Point(.DispScaleY.Location.X, .DispScaleY.Location.Y + .DispScaleY.Height + 5)
            .ReferencePtZ.Location = New Point(.DispScaleZ.Location.X, .DispScaleZ.Location.Y + .DispScaleZ.Height + 5)
            .Label6.Location = New Point(.DispScaleX.Location.X, .DispScaleX.Location.Y - 15)
            .Label7.Location = New Point(.DispScaleY.Location.X, .DispScaleY.Location.Y - 15)
            .Label8.Location = New Point(.DispScaleZ.Location.X, .DispScaleZ.Location.Y - 15)
            .Label9.Location = New Point(.ReferencePtX.Location.X, .ReferencePtX.Location.Y + .ReferencePtX.Height + 5)
        End With
    End Sub

    Private Sub CmdX_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdX.Click

        Me.ProgressBar1.Value = 0

        Dim i As Integer
        Dim Tmp_Str As String
        Dim Tmp_Str_Split() As String

        With Me.OpenFileDialog1
            .Title = "X Reference 파일 선택"
            .Multiselect = False
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "dat파일|*.dat|모든 파일|*.*"
            .ShowDialog()
        End With

        If Me.OpenFileDialog1.FileName = "" Then Exit Sub

        Me.StatusLbl.Text = "Reference X Curve Loaded"

        Me.TxtXcurve.Text = Me.OpenFileDialog1.FileName

        Dim ReadFiles As New FileStream(Me.OpenFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        Dim FileReading As StreamReader

        FileReading = New StreamReader(ReadFiles)

        i = 0

        Do While Not FileReading.EndOfStream 'InStr(Tmp_Str, "ENDATA") = 1
            Tmp_Str = RTrim(FileReading.ReadLine)
            If InStr(Tmp_Str, " ") >= 1 Then
                Tmp_Str_Split = Tmp_Str.Split(" ")
            Else
                Tmp_Str_Split = Tmp_Str.Split(Chr(9))
            End If

            If InStr(Tmp_Str, "ENDATA") = 0 And Tmp_Str <> "" And _
                IsNumeric(Tmp_Str_Split(UBound(Tmp_Str_Split))) = True Then

                ReDim Preserve X_Ref_Disp(i)
                X_Ref_Disp(i) = CDbl(Tmp_Str_Split(UBound(Tmp_Str_Split))) / 1000
                i = i + 1
            End If
        Loop

        ''하이퍼뷰
        ''임시코드 ㅡ,.ㅡ? 이상하게 2개가 더 있다.
        'ReDim Preserve X_Ref_Disp(33)
        'X_Ref_Disp(32) = X_Ref_Disp(31)
        'X_Ref_Disp(33) = X_Ref_Disp(31)

        Me.CmdX.ForeColor = Color.Green

        Me.TxtXcurve.Select(Me.TxtXcurve.Text.Length, 0)
        Me.FileToolTip.SetToolTip(Me.TxtXcurve, Me.TxtXcurve.Text)

        FileReading.Close()
        ReadFiles.Close()
    End Sub

    Private Sub FrmPSMexport_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Me.StatusLbl.Text = "PSM File Converter Closed"
    End Sub

    Private Sub CmdYawingTEST_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdYawingTEST.Click

        Me.ProgressBar1.Value = 0

        Dim i As Integer
        Dim Tmp_Str As String
        Dim Tmp_Str_Split() As String

        With Me.OpenFileDialog1
            .Title = "TEST Yawing 파일 선택"
            .Multiselect = False
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "dat파일|*.dat|모든 파일|*.*"
            .ShowDialog()
        End With

        If Me.OpenFileDialog1.FileName = "" Then Exit Sub

        Me.StatusLbl.Text = "TEST Yawing Curve Loaded"

        Me.TxtYawingTEST.Text = Me.OpenFileDialog1.FileName
        YawingTESTFile = Me.OpenFileDialog1.FileName

        Dim ReadFiles As New FileStream(Me.OpenFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        Dim FileReading As StreamReader

        FileReading = New StreamReader(ReadFiles)

        i = 0

        Do While Not FileReading.EndOfStream 'InStr(Tmp_Str, "ENDATA") = 1
            Tmp_Str = RTrim(FileReading.ReadLine)
            If InStr(Tmp_Str, " ") >= 1 Then
                Tmp_Str_Split = Tmp_Str.Split(" ")
            Else
                Tmp_Str_Split = Tmp_Str.Split(Chr(9))
            End If

            If InStr(Tmp_Str, "ENDATA") = 0 And Tmp_Str <> "" And _
                IsNumeric(Tmp_Str_Split(UBound(Tmp_Str_Split))) = True Then
                ReDim Preserve YawingTEST(i)
                YawingTEST(i) = CDbl(Tmp_Str_Split(UBound(Tmp_Str_Split)))
                i = i + 1
            End If
        Loop

        ''하이퍼뷰
        ''임시코드 ㅡ,.ㅡ? 이상하게 2개가 더 있다.
        'ReDim Preserve X_Ref_Disp(33)
        'X_Ref_Disp(32) = X_Ref_Disp(31)
        'X_Ref_Disp(33) = X_Ref_Disp(31)

        Me.CmdYawingTEST.ForeColor = Color.Green

        Me.TxtYawingTEST.Select(Me.TxtYawingTEST.Text.Length, 0)
        Me.FileToolTip.SetToolTip(Me.TxtYawingTEST, Me.TxtYawingTEST.Text)

        FileReading.Close()
        ReadFiles.Close()
    End Sub

    Private Sub CmdPitchTEST_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdPitchTEST.Click

        Me.ProgressBar1.Value = 0

        Dim i As Integer
        Dim Tmp_Str As String
        Dim Tmp_Str_Split() As String

        With Me.OpenFileDialog1
            .Title = "TEST Pitching 파일 선택"
            .Multiselect = False
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "dat파일|*.dat|모든 파일|*.*"
            .ShowDialog()
        End With

        If Me.OpenFileDialog1.FileName = "" Then Exit Sub

        Me.StatusLbl.Text = "TEST Pitching Curve Loaded"

        Me.TxtPitchingTEST.Text = Me.OpenFileDialog1.FileName
        PitchingTESTFile = Me.OpenFileDialog1.FileName

        Dim ReadFiles As New FileStream(Me.OpenFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        Dim FileReading As StreamReader

        FileReading = New StreamReader(ReadFiles)

        i = 0

        Do While Not FileReading.EndOfStream 'InStr(Tmp_Str, "ENDATA") = 1
            Tmp_Str = RTrim(FileReading.ReadLine)
            If InStr(Tmp_Str, " ") >= 1 Then
                Tmp_Str_Split = Tmp_Str.Split(" ")
            Else
                Tmp_Str_Split = Tmp_Str.Split(Chr(9))
            End If

            If InStr(Tmp_Str, "ENDATA") = 0 And Tmp_Str <> "" And _
                IsNumeric(Tmp_Str_Split(UBound(Tmp_Str_Split))) = True Then
                ReDim Preserve PitchingTEST(i)
                PitchingTEST(i) = CDbl(Tmp_Str_Split(UBound(Tmp_Str_Split)))
                i = i + 1
            End If
        Loop

        ''하이퍼뷰
        ''임시코드 ㅡ,.ㅡ? 이상하게 2개가 더 있다.
        'ReDim Preserve X_Ref_Disp(33)
        'X_Ref_Disp(32) = X_Ref_Disp(31)
        'X_Ref_Disp(33) = X_Ref_Disp(31)

        Me.CmdPitchTEST.ForeColor = Color.Green

        Me.TxtPitchingTEST.Select(Me.TxtPitchingTEST.Text.Length, 0)
        Me.FileToolTip.SetToolTip(Me.TxtPitchingTEST, Me.TxtPitchingTEST.Text)

        FileReading.Close()
        ReadFiles.Close()
    End Sub

    Private Sub GraphAngle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GraphAngle.Click
        IsTrans = False
        Dim DATAview As New FrmPSMgraph(IsTrans, Times, Yawing, Pitching, Z_Ref_Disp, YawingTEST, PitchingTEST, Z_TEST_Disp)
        DATAview.Show()
        DATAview.Owner = Me
    End Sub

    Private Sub CmdSelNODESET_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSelNODESET.Click

        Dim NodeSETfile As String = ""

        '*SET_NODE_LIST 파일을 불러온다.
        With Me.NodeExcludingDlg
            .Title = "Select key File [*SET_NODE_LIST]"
            .Multiselect = False
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)
            .Filter = "DYNA 파일|*.key;*.k;*.dyn|모든 파일|*.*"
            .ShowDialog()
        End With

        If Me.NodeExcludingDlg.FileNames(0) = "" Then
            Exit Sub
        Else
            NodeSETfile = Me.NodeExcludingDlg.FileNames(0)
            Me.TxtNodeSET.Text = NodeSETfile
            Me.NodeExcludingDlg.FileNames(0) = ""
        End If

        Me.StatusLbl.Text = "Key File Selected. (Include *SET_NODE_LIST)"

        'NODE SET을 찾아온다.=================================
        NodeList = ReadingSETNODE(NodeSETfile)

        If IsNothing(NodeList) Then
            MsgBox("There is Any *SET_NODE_LIST." & vbCrLf & "Ex> *CONSTRAINED_NODAL_RIGID_BODY, *SET_NODE_LIST", , "Error")
            Exit Sub
        Else
            MsgBox(UBound(NodeList) + 1 & " nodes are found", , "Complete")
        End If
    End Sub
End Class