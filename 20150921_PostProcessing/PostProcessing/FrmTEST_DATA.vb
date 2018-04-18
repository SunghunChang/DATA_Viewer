Imports System.IO
Imports System.Windows.Forms.DataVisualization.Charting
Imports ReadingR64

Public Class FrmTEST_DATA

    'Dim OpenFile As New GlobalClass '읽어온 파일명 저장

    Public DAT_Headers(,) As String
    Public DATAPreviewDP As New TEST_Graphing
    Public DATAFilePathList() As String
    Public IsTDMfile() As Boolean
    Public TGraphCollection As New Collection()
    Public AddGraphCollection As New Collection()
    Public CopyGraphCollection As New Collection()

    '메뉴중에서 전체 차트에 옵션을 적용하는 것 (범례 On/Off나 위치 설정)
    Dim WholeChartOption As New TEST_Graphing

    Dim TDMCHselEventPass As Boolean = False
    Dim EventPass As Boolean = False '파일리스트 클릭해서 항목 바뀔때 초기화시 이벤트 호출을 막는다.

    Dim TEST_Para_Num(31, 1) As Integer   '그래프 파라미터 인덱스를 저장한다. Line #805
    Dim DummyDisplayLegends As String = "" 'Line #816

    '차트 데이터 복사를 위한 변수 (복사할 차트 / 대상 차트)
    Dim MovinChrt As Chart
    Dim TargetChrt As Chart

    '총 몇개의 계열을 그렸는지 기록한다.
    Dim Tot_Series As Integer = 0

    Public Sub New()

        ' 이 호출은 디자이너에 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하십시오.
        Dim i As Integer
        With Me
            'Collection의 인덱스는 1부터 시작한다
            TGraphCollection.Add(.Chart00)
            TGraphCollection.Add(.Chart01)
            TGraphCollection.Add(.Chart02)
            TGraphCollection.Add(.Chart03)
            TGraphCollection.Add(.Chart04)
            TGraphCollection.Add(.Chart05)
            TGraphCollection.Add(.Chart06)
            TGraphCollection.Add(.Chart07)
            TGraphCollection.Add(.Chart08)
            TGraphCollection.Add(.Chart09)
            TGraphCollection.Add(.Chart10)
            TGraphCollection.Add(.Chart11)
            TGraphCollection.Add(.Chart12)
            TGraphCollection.Add(.Chart13)
            TGraphCollection.Add(.Chart14)
            TGraphCollection.Add(.Chart15)
            TGraphCollection.Add(.Chart16)
            TGraphCollection.Add(.Chart17)
            TGraphCollection.Add(.Chart18)
            TGraphCollection.Add(.Chart19)
            TGraphCollection.Add(.Chart20)
            TGraphCollection.Add(.Chart21)
            TGraphCollection.Add(.Chart22)
            TGraphCollection.Add(.Chart23)
            TGraphCollection.Add(.Chart24)
            TGraphCollection.Add(.Chart25)
            TGraphCollection.Add(.Chart26)
            TGraphCollection.Add(.Chart27)
            TGraphCollection.Add(.Chart28)
            TGraphCollection.Add(.Chart29)
            TGraphCollection.Add(.Chart30)
            TGraphCollection.Add(.Chart31)
            AddGraphCollection.Add(.Chart32)
            AddGraphCollection.Add(.Chart33)
            AddGraphCollection.Add(.Chart34)
            AddGraphCollection.Add(.Chart35)
            AddGraphCollection.Add(.Chart36)
            AddGraphCollection.Add(.Chart37)
            AddGraphCollection.Add(.Chart38)
            AddGraphCollection.Add(.Chart39)
            AddGraphCollection.Add(.Chart40)
            AddGraphCollection.Add(.Chart41)
            AddGraphCollection.Add(.Chart42)
            AddGraphCollection.Add(.Chart43)
            AddGraphCollection.Add(.Chart44)
            AddGraphCollection.Add(.Chart45)
            AddGraphCollection.Add(.Chart46)
            AddGraphCollection.Add(.Chart47)
            CopyGraphCollection.Add(.Chart00)
            CopyGraphCollection.Add(.Chart01)
            CopyGraphCollection.Add(.Chart02)
            CopyGraphCollection.Add(.Chart03)
            CopyGraphCollection.Add(.Chart04)
            CopyGraphCollection.Add(.Chart05)
            CopyGraphCollection.Add(.Chart06)
            CopyGraphCollection.Add(.Chart07)
            CopyGraphCollection.Add(.Chart08)
            CopyGraphCollection.Add(.Chart09)
            CopyGraphCollection.Add(.Chart10)
            CopyGraphCollection.Add(.Chart11)
            CopyGraphCollection.Add(.Chart12)
            CopyGraphCollection.Add(.Chart13)
            CopyGraphCollection.Add(.Chart14)
            CopyGraphCollection.Add(.Chart15)
            CopyGraphCollection.Add(.Chart16)
            CopyGraphCollection.Add(.Chart17)
            CopyGraphCollection.Add(.Chart18)
            CopyGraphCollection.Add(.Chart19)
            CopyGraphCollection.Add(.Chart20)
            CopyGraphCollection.Add(.Chart21)
            CopyGraphCollection.Add(.Chart22)
            CopyGraphCollection.Add(.Chart23)
            CopyGraphCollection.Add(.Chart24)
            CopyGraphCollection.Add(.Chart25)
            CopyGraphCollection.Add(.Chart26)
            CopyGraphCollection.Add(.Chart27)
            CopyGraphCollection.Add(.Chart28)
            CopyGraphCollection.Add(.Chart29)
            CopyGraphCollection.Add(.Chart30)
            CopyGraphCollection.Add(.Chart31)
            CopyGraphCollection.Add(.Chart32)
            CopyGraphCollection.Add(.Chart33)
            CopyGraphCollection.Add(.Chart34)
            CopyGraphCollection.Add(.Chart35)
            CopyGraphCollection.Add(.Chart36)
            CopyGraphCollection.Add(.Chart37)
            CopyGraphCollection.Add(.Chart38)
            CopyGraphCollection.Add(.Chart39)
            CopyGraphCollection.Add(.Chart40)
            CopyGraphCollection.Add(.Chart41)
            CopyGraphCollection.Add(.Chart42)
            CopyGraphCollection.Add(.Chart43)
            CopyGraphCollection.Add(.Chart44)
            CopyGraphCollection.Add(.Chart45)
            CopyGraphCollection.Add(.Chart46)
            CopyGraphCollection.Add(.Chart47)
        End With

        For i = 1 To 32
            '차트 Title을 추가한다.
            '데이터를 그릴때 추가하면 계속 추가한다.
            AddTitles(TGraphCollection.Item(i), GraphTitle(i - 1))
        Next

        For i = 1 To 48
            Me.ListBox1.Items.Add("Graph #" & Format(i, "00"))
        Next

        With Me.OpenTESTDlg
            .Title = "Header 파일 선택"
            .Multiselect = True
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "Diadem DATA File|*.DAT;*.tdm|모든 파일|*.*"
            .ShowDialog()
        End With

        '파일 선택이 없는 경우
        If Me.OpenTESTDlg.FileNames(0) = "" Then
            Exit Sub
        Else
            'OpenFile.names = FileNameGet(Me.OpenTESTDlg.SafeFileNames)
        End If

        '파일명 추가 및 파일 경로 저장
        ReDim DATAFilePathList(UBound(Me.OpenTESTDlg.FileNames))
        ReDim IsTDMfile(UBound(Me.OpenTESTDlg.FileNames))

        For i = 0 To UBound(Me.OpenTESTDlg.FileNames)
            If LCase(Mid(Me.OpenTESTDlg.SafeFileNames(i), Len(Me.OpenTESTDlg.SafeFileNames(i)) - 3, Len(Me.OpenTESTDlg.SafeFileNames(i)))) = ".tdm" Then
                IsTDMfile(i) = True
                Me.FileListBox.Items.Add(Mid(Me.OpenTESTDlg.SafeFileNames(i), 1, Len(Me.OpenTESTDlg.SafeFileNames(i)) - 3) & "tdm")
            Else
                Me.FileListBox.Items.Add(Me.OpenTESTDlg.SafeFileNames(i))
            End If
            DATAFilePathList(i) = FilePathGet(Me.OpenTESTDlg.FileNames)
        Next
    End Sub

    Private Sub AddBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddBtn.Click
        With Me.OpenTESTDlg
            .Title = "Header 파일 선택"
            .Multiselect = True
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "Diadem DATA File|*.DAT;*.tdm|모든 파일|*.*"
            .ShowDialog()
        End With

        If Me.OpenTESTDlg.FileNames(0) = "" Then
            Exit Sub
        End If

        '파일명 추가 및 파일 경로 저장
        ReDim Preserve DATAFilePathList(Me.FileListBox.Items.Count + UBound(Me.OpenTESTDlg.FileNames))
        ReDim Preserve IsTDMfile(Me.FileListBox.Items.Count + UBound(Me.OpenTESTDlg.FileNames))

        Dim Tmp_Cnt As Integer
        Tmp_Cnt = Me.FileListBox.Items.Count
        'Me.FileListBox.Items.Count 를 For문에 쓰면 리스트가 추가될 때마다 숫자가 늘어난다.
        For i = Me.FileListBox.Items.Count To Tmp_Cnt + UBound(Me.OpenTESTDlg.FileNames)

            If LCase(Mid(Me.OpenTESTDlg.SafeFileNames(i - Tmp_Cnt), Len(Me.OpenTESTDlg.SafeFileNames(i - Tmp_Cnt)) - 3, Len(Me.OpenTESTDlg.SafeFileNames(i - Tmp_Cnt)))) = ".tdm" Then
                IsTDMfile(i) = True
                Me.FileListBox.Items.Add(Mid(Me.OpenTESTDlg.SafeFileNames(i - Tmp_Cnt), 1, Len(Me.OpenTESTDlg.SafeFileNames(i - Tmp_Cnt)) - 3) & "tdm")
            Else
                Me.FileListBox.Items.Add(Me.OpenTESTDlg.SafeFileNames(i - Tmp_Cnt))
            End If
            DATAFilePathList(i) = FilePathGet(Me.OpenTESTDlg.FileNames)
        Next
    End Sub

    Private Sub FrmTEST_DATA_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        MainMDI.Statuslbl.Text = "Status"
    End Sub

    Private Sub FrmTEST_DATA_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If TempLicExpire = False Then
            '원격로그
            Try
                Using NewfileNum As StreamWriter = File.AppendText(RemoteValFolder & System.Environment.UserName)
                    NewfileNum.WriteLine("## TEST Data Display Form Open : " & Date.Now.ToString & " ##")
                    NewfileNum.Close()
                End Using
            Catch ex As Exception
                'End
            End Try
        End If

        MainMDI.Statuslbl.Text = "TEST Data Viewer Load"
        With Me
            '.Width = 1400
            '.Height = 800
            '.MainTESTSplitter.Dock = DockStyle.Fill
            .WindowState = FormWindowState.Maximized
            .MainTESTSplitter.SplitterDistance = 350
            .GraphTab.TabPages(0).Text = "TEST DATA #1"
            .GraphTab.TabPages(1).Text = "TEST DATA #2"
            .GraphTab.TabPages(2).Text = "Additional DATA"
            .DummyCmb.SelectedIndex = 0
            .ToolStripCmbOver.SelectedIndex = 0

            Dim k As Integer
            For k = 1 To TGraphCollection.Count
                AllChartSeriesDEL(TGraphCollection(k))
            Next

            .ToolStripCmbOver.SelectedIndex = 0
            .ToolStripCmbOver.Width = 200
            .ToolStripCmbOver.DropDownWidth = 200
        End With
    End Sub

    Private Sub FileListBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FileListBox.SelectedIndexChanged

        Dim i As Integer

        With Me
            If .FileListBox.SelectedIndex >= 0 Then
                '파일을 선택하면 채널명을 리스트에 쓴다.

                '기존의 리스트 항목 삭제
                EventPass = True
                .CmbXaxis.SelectedIndex = -1
                .ChList.SelectedIndex = -1
                EventPass = False
                For i = .ChList.Items.Count To 1 Step -1
                    .ChList.Items.RemoveAt(i - 1)
                    .CmbXaxis.Items.RemoveAt(i - 1)
                Next
                For i = .TDMCHSel.Items.Count To 1 Step -1
                    .TDMCHSel.Items.RemoveAt(i - 1)
                Next

                '채널명 추가
                '선택을 하면 헤더를 읽어온다.
                For i = 0 To UBound(Me.OpenTESTDlg.FileNames)
                    '헤더를 읽어온다.
                    If IsTDMfile(.FileListBox.SelectedIndex) = False Then
                        DAT_Headers = OepnDATfileHeader(DATAFilePathList(.FileListBox.SelectedIndex) & _
                                                    .FileListBox.SelectedItem) 'Me.OpenTESTDlg.FileNames(i)) 'OpenFile.names(i))
                    ElseIf IsTDMfile(.FileListBox.SelectedIndex) = True Then
                        '채널 그룹을 선택하게 해야한다.==========================================================================
                        Dim HeaderFile As StreamReader
                        Dim Tmp_read As String
                        Dim k As Integer = 0
                        Dim Paths As String = DATAFilePathList(.FileListBox.SelectedIndex) & .FileListBox.SelectedItem
                        Dim Tmp_CH_Group_Name() As String = Nothing

                        HeaderFile = New StreamReader(Paths)

                        Tmp_read = HeaderFile.ReadLine

                        If InStr(Tmp_read, "><") > 0 Then

                            Dim Tmp_read_2 As String
                            Tmp_read_2 = Tmp_read.Replace("><", ">" & vbCrLf & "<")
                            Dim TDM_Lines() As String
                            TDM_Lines = Tmp_read_2.Split(vbCrLf)

                            Dim kk As Integer = 0

                            Do While kk <= UBound(TDM_Lines)

                                Tmp_read = TDM_Lines(kk)

                                If InStr(Tmp_read, "<tdm_channelgroup id=""") > 0 Then
                                    kk = kk + 1
                                    Tmp_read = TDM_Lines(kk)
                                    k = k + 1
                                    ReDim Preserve Tmp_CH_Group_Name(k)
                                    Tmp_CH_Group_Name(k) = BetweenEle(Tmp_read)

                                    Do While Not InStr(Tmp_read, "</tdm_channelgroup>") > 1
                                        kk = kk + 1
                                        Tmp_read = TDM_Lines(kk)
                                        '채널그룹의 이름식별=========================================================================
                                        If InStr(Tmp_read, "<name>") > 0 Then
                                            Tmp_CH_Group_Name(k) = BetweenEle(Tmp_read)
                                        End If
                                    Loop

                                End If

                                kk = kk + 1
                            Loop

                        Else

                            Do While Not HeaderFile.EndOfStream
                                Tmp_read = HeaderFile.ReadLine

                                If InStr(Tmp_read, "<tdm_channelgroup id=""") > 0 Then
                                    Tmp_read = HeaderFile.ReadLine
                                    k = k + 1
                                    ReDim Preserve Tmp_CH_Group_Name(k)
                                    Tmp_CH_Group_Name(k) = BetweenEle(Tmp_read)

                                    Do While Not InStr(Tmp_read, "</tdm_channelgroup>") > 1
                                        Tmp_read = HeaderFile.ReadLine
                                        '채널그룹의 이름식별=========================================================================
                                        If InStr(Tmp_read, "<name>") > 0 Then
                                            Tmp_CH_Group_Name(k) = BetweenEle(Tmp_read)
                                        End If
                                    Loop

                                End If
                            Loop

                        End If


                        For k = 1 To UBound(Tmp_CH_Group_Name)
                            With Me
                                TDMCHselEventPass = True
                                .TDMCHSel.Items.Add(Tmp_CH_Group_Name(k))
                                .TDMCHSel.SelectedIndex = 0
                                '.TDMCHSel.SelectedItem(.TDMCHSel.SelectedIndex) = 0

                                TDMCHselEventPass = False
                            End With
                        Next
                        '======================================================================================================

                        DAT_Headers = OepnTDMfileHeader(DATAFilePathList(.FileListBox.SelectedIndex) & .FileListBox.SelectedItem, .TDMCHSel.SelectedIndex + 1)
                        End If
                Next
                For i = 0 To UBound(DAT_Headers, 1)
                    .ChList.Items.Add(DAT_Headers(i, 0))
                    .CmbXaxis.Items.Add(DAT_Headers(i, 0))
                    'X라벨은 Time을 찾아서 한다. (없음 말고)
                    If UCase(.CmbXaxis.Items(i).ToString) = "TIME" Then
                        .CmbXaxis.SelectedIndex = i
                    End If
                Next
            End If
        End With
    End Sub

    Private Sub ChList_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ChList.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Me.ChlstMenu.Show(MousePosition)
        End If
    End Sub

    Private Sub ChList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChList.SelectedIndexChanged

        If Me.FileListBox.SelectedIndex = -1 Or EventPass = True Then Exit Sub

        Dim i As Integer
        Dim FileNameR64 As String
        Dim Values As Byte()

        If IsTDMfile(Me.FileListBox.SelectedIndex) = False Then
            FileNameR64 = Replace(Me.FileListBox.Items((Me.FileListBox.SelectedIndex)), ".DAT", "")

            If Not System.IO.File.Exists(DATAFilePathList(Me.FileListBox.SelectedIndex) & FileNameR64 & ".R64") Then
                MsgBox("데이터 파일을 찾을 수 없습니다", , "경고")
                Exit Sub
            End If

            Dim R64File As New FileInfo(DATAFilePathList(Me.FileListBox.SelectedIndex) & FileNameR64 & ".R64")
            Dim NumByte As Long = R64File.Length
            Dim Fstream As New FileStream(DATAFilePathList(Me.FileListBox.SelectedIndex) & FileNameR64 & ".R64", FileMode.Open, FileAccess.Read)
            Dim FileRead As New BinaryReader(Fstream)
            'Byte Array를 읽어온다.
            Values = FileRead.ReadBytes(CInt(NumByte))

            FileRead.Close()
            Fstream.Close()

        Else
            FileNameR64 = Replace(Me.FileListBox.Items((Me.FileListBox.SelectedIndex)), ".tdm", "")

            If Not System.IO.File.Exists(DATAFilePathList(Me.FileListBox.SelectedIndex) & FileNameR64 & ".tdx") Then
                MsgBox("데이터 파일을 찾을 수 없습니다", , "경고")
                Exit Sub
            End If

            Dim R64File As New FileInfo(DATAFilePathList(Me.FileListBox.SelectedIndex) & FileNameR64 & ".tdx")
            Dim NumByte As Long = R64File.Length
            Dim Fstream As New FileStream(DATAFilePathList(Me.FileListBox.SelectedIndex) & FileNameR64 & ".tdx", FileMode.Open, FileAccess.Read)
            Dim FileRead As New BinaryReader(Fstream)
            'Byte Array를 읽어온다.
            Values = FileRead.ReadBytes(CInt(NumByte))

            FileRead.Close()
            Fstream.Close()
        End If


        '===================================================================================================

        'Dim Values() As Byte = File.ReadAllBytes(Me.OpenTESTDlg.FileNames(Me.FileListBox.SelectedIndex))

        '===================================================================================================

        'Using reader As New BinaryReader(File.Open(Me.OpenTESTDlg.FileNames(Me.FileListBox.SelectedIndex), FileMode.Open))
        '    Dim pos As Integer = 0
        '    Dim length As Integer = reader.BaseStream.Length
        '    While pos < length
        '        Dim val As Integer = reader.ReadDouble()
        '        Console.Write(val)
        '        pos += 8
        '    End While
        'End Using

        Dim CH_DATA_X() As Double '채널 데이터
        Dim CH_DATA_Y() As Double '채널 데이터

        '데이터를 읽어온다 (X축)
        '헤더(채널명,0-데이터이름/1-시작위치/2-길이(갯수)/3-시작옵셋값/4-증분/5-Implicit여부/6-단위)
        If Not IsNumeric(Me.Xscale.Text) Then Exit Sub
        If Not IsNumeric(Me.Yscale.Text) Then Exit Sub
        If Not IsNumeric(Me.Xoffset.Text) Then Exit Sub
        If Not IsNumeric(Me.Yoffset.Text) Then Exit Sub
        'X,Y 옵셋량과 변환 Factor를 적용한다.
        'Implicit DATA의 경우는 데이터를 만들어 줘야한다. (주로 X축 - 시간만 이런다.)
        If Me.CmbXaxis.SelectedIndex = -1 Then Exit Sub
        If DAT_Headers(Me.CmbXaxis.SelectedIndex, 5) = 1 Then
            ReDim CH_DATA_X(CInt(DAT_Headers(Me.CmbXaxis.SelectedIndex, 2) - 1))
            For i = 0 To UBound(CH_DATA_X)
                CH_DATA_X(i) = DAT_Headers(Me.CmbXaxis.SelectedIndex, 3) + (i * DAT_Headers(Me.CmbXaxis.SelectedIndex, 4))
            Next
        Else
            If DAT_Headers(Me.CmbXaxis.SelectedIndex, 8) = 4 Then
                CH_DATA_X = TESTopen.OpenBinaryR32(Values, DAT_Headers(Me.CmbXaxis.SelectedIndex, 1), DAT_Headers(Me.CmbXaxis.SelectedIndex, 2), CSng(Me.Xscale.Text), CSng(Me.Xoffset.Text))
            Else
                CH_DATA_X = TESTopen.OpenBinaryR64(Values, DAT_Headers(Me.CmbXaxis.SelectedIndex, 1), DAT_Headers(Me.CmbXaxis.SelectedIndex, 2), CSng(Me.Xscale.Text), CSng(Me.Xoffset.Text))
            End If


        End If
        If CH_DATA_X(UBound(CH_DATA_X)) < 2.0 And InStr(UCase(DAT_Headers(Me.CmbXaxis.SelectedIndex, 0)), "TIME") >= 1 Then 'X축이 Time이면 단위변경
            '시험 time 단위가 sec이면 MADYMO와 같은 msec로 바꿔준다.
            For i = 0 To UBound(CH_DATA_X)
                CH_DATA_X(i) = CH_DATA_X(i) * 1000
            Next
        End If

        'Y 데이터도 Implicit 이면 만들어준다.
        If DAT_Headers(Me.ChList.SelectedIndex, 5) = 1 Then
            ReDim CH_DATA_Y(CInt(DAT_Headers(Me.ChList.SelectedIndex, 2) - 1))
            For i = 0 To UBound(CH_DATA_Y)
                CH_DATA_Y(i) = DAT_Headers(Me.ChList.SelectedIndex, 3) + (i * DAT_Headers(Me.ChList.SelectedIndex, 4))
            Next
        Else
            If DAT_Headers(Me.ChList.SelectedIndex, 8) = 4 Then
                CH_DATA_Y = TESTopen.OpenBinaryR32(Values, DAT_Headers(Me.ChList.SelectedIndex, 1), DAT_Headers(Me.ChList.SelectedIndex, 2), CSng(Me.Yscale.Text), CSng(Me.Yoffset.Text))
            Else
                CH_DATA_Y = TESTopen.OpenBinaryR64(Values, DAT_Headers(Me.ChList.SelectedIndex, 1), DAT_Headers(Me.ChList.SelectedIndex, 2), CSng(Me.Yscale.Text), CSng(Me.Yoffset.Text))
            End If
        End If

        'DATA Preview
        With Me.PreviewDATA
            For i = .Series.Count To 1 Step -1
                .Series.RemoveAt(i - 1)
            Next
            If IsTDMfile(Me.FileListBox.SelectedIndex) = True Then
                .Series.Add(DAT_Headers(Me.ChList.SelectedIndex, 0).ToString & " " & Me.TDMCHSel.Items(Me.TDMCHSel.SelectedIndex))
            Else
                .Series.Add(DAT_Headers(Me.ChList.SelectedIndex, 0).ToString)
            End If

            Try
                '.Series(DAT_Headers(Me.ChList.SelectedIndex, 0).ToString).Points.DataBindXY(CH_DATA_X, CH_DATA_Y)
                .Series(.Series.Count - 1).Points.DataBindXY(CH_DATA_X, CH_DATA_Y)
            Catch ex As Exception
                If Math.Abs(UBound(CH_DATA_X) - UBound(CH_DATA_Y)) > 2 Then
                    MsgBox("데이터 크기 오류 (X-Y DATA Miss Matching)", , "경고")
                End If
            Finally
                'X 데이터와 Y 데이터의 크기가 한개정도 차이나면 그냥 무시하고 그래프 그림
                '.Series(DAT_Headers(Me.ChList.SelectedIndex, 0).ToString).ChartType = DataVisualization.Charting.SeriesChartType.Line
                .Series(.Series.Count - 1).ChartType = DataVisualization.Charting.SeriesChartType.Line

                'DATA Preview 꾸미기
                DATAPreviewDP.GraphAxisLine(Me.PreviewDATA)
                DATAPreviewDP.ScrollnZoom(Me.PreviewDATA)
                DATAPreviewDP.SeriesValShow(Me.PreviewDATA)

                If CH_DATA_X(UBound(CH_DATA_X)) < 10 Then
                    '.ChartAreas(0).AxisX.IntervalOffset = 0.0001
                    .ChartAreas(0).AxisX.Interval = 0.02
                    .ChartAreas(0).AxisX.Minimum = 0.0
                    .ChartAreas(0).AxisX.Maximum = 0.2
                Else
                    '.ChartAreas(0).AxisX.IntervalOffset = 0.1
                    .ChartAreas(0).AxisX.Interval = 20
                    .ChartAreas(0).AxisX.Minimum = 0.0
                    .ChartAreas(0).AxisX.Maximum = 200
                End If

            End Try

        End With
    End Sub

    'Add Graph DragDrop ==================================================================================
    Private Sub PreviewDATA_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles PreviewDATA.DragDrop
        PreviewDATA.DoDragDrop(Me.PreviewDATA, DragDropEffects.Copy)
    End Sub
    Private Sub PreviewDATA_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PreviewDATA.MouseDown
        '첫번째 : 이벤트 호출
        If e.Button = Windows.Forms.MouseButtons.Middle Then
            PreviewDATA.DoDragDrop(Me.PreviewDATA, DragDropEffects.Copy)
        End If
    End Sub
    Private Sub DrawingADDgraphs(ByRef Charts As Chart)
        'Dim i As Integer
        With Charts
            'For i = 0 To .Series.Count - 1
            '    .Series(i).ChartType = SeriesChartType.Line
            'Next

            DATAPreviewDP.GraphAxisLine(Charts)

            '차트의 Min/Max/Inteverl 설정
            If .ChartAreas(0).AxisX.Maximum < 10 Then
                .ChartAreas(0).AxisX.Interval = 0.02
                .ChartAreas(0).AxisX.Minimum = 0.0
                .ChartAreas(0).AxisX.Maximum = 0.2
            Else
                .ChartAreas(0).AxisX.Interval = 20
                .ChartAreas(0).AxisX.Minimum = 0.0
                .ChartAreas(0).AxisX.Maximum = 200
            End If
            .ChartAreas(0).AxisY.Minimum = Double.NaN
            .ChartAreas(0).AxisY.Maximum = Double.NaN

            'Label Format (X-Y 그래프 라벨 글꼴)
            .ChartAreas(0).AxisX.LabelStyle.Font = New Font("Arial", 7, FontStyle.Bold)
            .ChartAreas(0).AxisY.LabelStyle.Font = New Font("Arial", 7, FontStyle.Bold)

            '범례 표식
            .Legends(0).Font = New Font("Arial", 7, FontStyle.Bold)

            .Update()
        End With

    End Sub
    '=====================================================================================================
    Private Sub Chart32_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart32.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart32_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart32.DragDrop
        DragDropEventEnd(Chart32)
    End Sub
    Private Sub Chart33_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart33.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart33_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart33.DragDrop
        DragDropEventEnd(Chart33)
    End Sub
    Private Sub Chart34_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart34.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart34_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart34.DragDrop
        DragDropEventEnd(Chart34)
    End Sub
    Private Sub Chart35_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart35.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart35_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart35.DragDrop
        DragDropEventEnd(Chart35)
    End Sub
    Private Sub Chart36_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart36.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart36_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart36.DragDrop
        DragDropEventEnd(Chart36)
    End Sub
    Private Sub Chart37_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart37.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart37_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart37.DragDrop
        DragDropEventEnd(Chart37)
    End Sub
    Private Sub Chart38_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart38.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart38_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart38.DragDrop
        DragDropEventEnd(Chart38)
    End Sub
    Private Sub Chart39_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart39.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart39_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart39.DragDrop
        DragDropEventEnd(Chart39)
    End Sub
    Private Sub Chart40_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart40.DragEnter
        '두번째 : 효과 
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart40_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart40.DragDrop
        DragDropEventEnd(Chart40)
    End Sub
    Private Sub Chart41_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart41.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart41_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart41.DragDrop
        DragDropEventEnd(Chart41)
    End Sub
    Private Sub Chart42_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart42.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart42_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart42.DragDrop
        DragDropEventEnd(Chart42)
    End Sub
    Private Sub Chart43_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart43.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart43_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart43.DragDrop
        DragDropEventEnd(Chart43)
    End Sub
    Private Sub Chart44_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart44.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart44_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart44.DragDrop
        DragDropEventEnd(Chart44)
    End Sub
    Private Sub Chart45_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart45.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart45_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart45.DragDrop
        DragDropEventEnd(Chart45)
    End Sub
    Private Sub Chart46_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart46.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart46_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart46.DragDrop
        DragDropEventEnd(Chart46)
    End Sub
    Private Sub Chart47_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart47.DragEnter
        '두번째 : 효과
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub Chart47_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Chart47.DragDrop
        DragDropEventEnd(Chart47)
    End Sub
    Private Sub DragDropEventEnd(ByRef TChart As Chart)
        '세번째 : 작업의 시작
        Dim i As Integer
        With Me
            Dim Titles As String = .PreviewDATA.Series(0).Name & "-" & .FileListBox.SelectedItem.ToString
            Select Case .ToolStripCmbOver.SelectedIndex
                Case 0
                    'Graph Overlap
                    TChart.Series(.PreviewDATA.Series(0).Name) = .PreviewDATA.Series(0)
                    Try
                        TChart.Series(.PreviewDATA.Series(0).Name).Name = Titles
                    Catch ex As Exception '같은 이름이 있는경우 (스케일해서 또 덮는 경우)
                        TChart.Series(.PreviewDATA.Series(0).Name).Name = Titles & "-Re"
                    End Try
                Case 1
                    'New Graph
                    For i = TChart.Series.Count To 1 Step -1
                        TChart.Series.RemoveAt(i - 1)
                    Next
                    TChart.Series(.PreviewDATA.Series(0).Name) = .PreviewDATA.Series(0)
                    TChart.Series(.PreviewDATA.Series(0).Name).Name = Titles
            End Select
            TChart.Update()
            DrawingADDgraphs(TChart)
        End With
    End Sub
    '=====================================================================================================

    Private Sub PreviewDATA_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PreviewDATA.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Me.PreviewContextMenu.Show(MousePosition)
        End If
    End Sub

    Private Sub ChartOptionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChartOptionToolStripMenuItem.Click
        Dim OptionFrm As New FrmChartOption(Me.PreviewDATA)
        OptionFrm.Owner = Me
        OptionFrm.Show()
    End Sub

    Private Sub CopyToClipBoardBMPToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToClipBoardBMPToolStripMenuItem.Click
        CopyChartImage(Me.PreviewDATA)
    End Sub

    Private Sub FrmTEST_DATA_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        With Me
            If .Width < 500 Or .Height < 300 Then Exit Sub
            .PreviewBox.Location = New Point(5, 25)
            .PreviewBox.Size = New Size(450, .ClientRectangle.Height - 30)
            .PreviewDATA.Location = New Point(5, 20)
            .PreviewDATA.Size = New Size(.PreviewBox.ClientRectangle.Width - 10, 300)
            .XScaleLbl.Location = New Point(5, .PreviewDATA.Location.Y + .PreviewDATA.Height + 10)
            .Xscale.Location = New Point(5, .XScaleLbl.Location.Y + .XScaleLbl.Height + 5)
            .Xscale.Size = New Size(.PreviewDATA.Width / 4, 21)
            .Yscale.Size = .Xscale.Size
            .Xoffset.Size = .Xscale.Size
            .Yoffset.Size = .Xscale.Size
            .Yscale.Location = New Point(.Xscale.Location.X + .Xscale.Width, .Xscale.Location.Y)
            .YScaleLbl.Location = New Point(.Yscale.Location.X, .XScaleLbl.Location.Y)
            .Xoffset.Location = New Point(.Yscale.Location.X + .Yscale.Width, .Yscale.Location.Y)
            .XOffsetLbl.Location = New Point(.Xoffset.Location.X, .XScaleLbl.Location.Y)
            .Yoffset.Location = New Point(.Xoffset.Location.X + .Xoffset.Width, .Xoffset.Location.Y)
            .YOffsetLbl.Location = New Point(.Yoffset.Location.X, .XScaleLbl.Location.Y)
            .XScaleLbl.Width = .Xscale.Width
            .YScaleLbl.Width = .Xscale.Width
            .XOffsetLbl.Width = .Xscale.Width
            .YOffsetLbl.Width = .Xscale.Width

            .FileListBox.Location = New Point(5, .Xscale.Location.Y + .Xscale.Height + 10)
            .FileListBox.Size = New Size(.PreviewDATA.Width - 55, 21 * 3)
            .AddBtn.Location = New Point(.FileListBox.Location.X + .FileListBox.Width + 5, .FileListBox.Location.Y)
            .AddBtn.Size = New Size(50, .FileListBox.Height)
            .CmbXaxis.Location = New Point(5, .FileListBox.Location.Y + .FileListBox.Height + 10)
            .CmbXaxis.Width = .PreviewDATA.Width
            .ChList.Location = New Point(5, .CmbXaxis.Location.Y + .CmbXaxis.Height + 5)
            .ChList.Size = New Size(.PreviewDATA.Width, .PreviewBox.ClientRectangle.Height - 5 - .ChList.Location.Y)

            .GraphBox.Location = New Point(5 + 5 + 10 + .PreviewBox.Width, 25)
            .GraphBox.Size = New Size(.ClientRectangle.Width - .PreviewBox.Location.X - .PreviewBox.Width - 10 - 10, .PreviewBox.Height)
            .GraphTab.Location = New Point(5, 20)
            .GraphTab.Size = New Size(.GraphBox.ClientRectangle.Width - 10, .GraphBox.ClientRectangle.Height - 25)

            .Chart00.Location = New Point(2, 2)
            .Chart00.Size = New Size((.GraphTab.ClientRectangle.Width - 10) / 4 - 1, (.GraphTab.ClientRectangle.Height - 10) / 4 - 6)
            Dim i As Integer
            For i = 1 To 32
                Try
                    SameSizeChart(TGraphCollection.Item(i), .Chart00.Size.Width, .Chart00.Size.Height)
                Catch ex As Exception
                    Exit Sub
                End Try
            Next
            For i = 1 To 16
                Try
                    SameSizeChart(AddGraphCollection.Item(i), .Chart00.Size.Width, .Chart00.Size.Height)
                Catch ex As Exception
                    Exit Sub
                End Try
            Next
            .Chart01.Location = New Point(.Chart00.Width, .Chart00.Location.Y)
            .Chart02.Location = New Point(.Chart00.Width * 2, .Chart01.Location.Y)
            .Chart03.Location = New Point(.Chart00.Width * 3, .Chart02.Location.Y)
            .Chart04.Location = New Point(.Chart00.Location.X, .Chart00.Height + 2)
            .Chart05.Location = New Point(.Chart01.Location.X, .Chart04.Location.Y)
            .Chart06.Location = New Point(.Chart02.Location.X, .Chart04.Location.Y)
            .Chart07.Location = New Point(.Chart03.Location.X, .Chart04.Location.Y)
            .Chart08.Location = New Point(.Chart00.Location.X, .Chart00.Height * 2 + 2)
            .Chart09.Location = New Point(.Chart01.Location.X, .Chart08.Location.Y)
            .Chart10.Location = New Point(.Chart02.Location.X, .Chart08.Location.Y)
            .Chart11.Location = New Point(.Chart03.Location.X, .Chart08.Location.Y)
            .Chart12.Location = New Point(.Chart00.Location.X, .Chart00.Height * 3 + 2)
            .Chart13.Location = New Point(.Chart01.Location.X, .Chart12.Location.Y)
            .Chart14.Location = New Point(.Chart02.Location.X, .Chart12.Location.Y)
            .Chart15.Location = New Point(.Chart03.Location.X, .Chart12.Location.Y)
            .Chart16.Location = .Chart00.Location
            .Chart17.Location = .Chart01.Location
            .Chart18.Location = .Chart02.Location
            .Chart19.Location = .Chart03.Location
            .Chart20.Location = .Chart04.Location
            .Chart21.Location = .Chart05.Location
            .Chart22.Location = .Chart06.Location
            .Chart23.Location = .Chart07.Location
            .Chart24.Location = .Chart08.Location
            .Chart25.Location = .Chart09.Location
            .Chart26.Location = .Chart10.Location
            .Chart27.Location = .Chart11.Location
            .Chart28.Location = .Chart12.Location
            .Chart29.Location = .Chart13.Location
            .Chart30.Location = .Chart14.Location
            .Chart31.Location = .Chart15.Location
            .Chart32.Location = .Chart00.Location
            .Chart33.Location = .Chart01.Location
            .Chart34.Location = .Chart02.Location
            .Chart35.Location = .Chart03.Location
            .Chart36.Location = .Chart04.Location
            .Chart37.Location = .Chart05.Location
            .Chart38.Location = .Chart06.Location
            .Chart39.Location = .Chart07.Location
            .Chart40.Location = .Chart08.Location
            .Chart41.Location = .Chart09.Location
            .Chart42.Location = .Chart10.Location
            .Chart43.Location = .Chart11.Location
            .Chart44.Location = .Chart12.Location
            .Chart45.Location = .Chart13.Location
            .Chart46.Location = .Chart14.Location
            .Chart47.Location = .Chart15.Location

            '.BtnApply.Location = New Point(.GraphBox.Location.X - 14, (.PreviewBox.Location.Y + .PreviewBox.Height / 2) - 40)
            '.BtnApply.Size = New Size(14, 67)
        End With
    End Sub

    Private Sub SameSizeChart(ByRef charts As Chart, ByVal wid As Single, ByVal hei As Single)
        With charts
            .Width = wid
            .Height = hei
        End With
    End Sub

    Private Sub AllChartSeriesDEL(ByRef Charts As Chart)
        Dim i As Integer
        For i = 0 To charts.Series.Count - 1
            charts.Series.RemoveAt(i)
        Next
    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click

        If Me.FileListBox.SelectedIndex = -1 Then Exit Sub

        MainMDI.ProgressBarMain.Value = 0
        MainMDI.Statuslbl.Text = "Load and Start TEST Graph Displayed"
        MainMDI.ProgressBarMain.Maximum = 3100

        '그래프 파라미터 위치를 파악한다.========================================================================
        Dim i, j, k, m, n, t, Marker, p As Integer
        'Dim TEST_Para_Num(31, 1) As Integer   '그래프 파라미터 인덱스를 저장한다.
        Dim Tmp_StrX() As String
        Dim Tmp_StrY() As String
        Dim Tmp_Char() As String
        Dim FindDummy() As String = Nothing

        '범례에 데이터 파일명을 표시함
        '   ex> ==> 운전석을 읽고, 또 동승석을 읽는경우 파일 이름이 같아서 읽지 않는 경우가 생기므로
        '           범례를 쓸 때 아예 표시해준다. (Series 이름으로도 설정한다.)
        '           DrawingTESTgraphs Function에서 추가된다.
        'Dim DummyDisplayLegends As String = ""

        Select Case Me.DummyCmb.SelectedIndex
            Case 0
                ReDim FindDummy(UBound(TEST_DRV_Para))
                FindDummy = TEST_DRV_Para
                DummyDisplayLegends = " - DRV"
            Case 1
                ReDim FindDummy(UBound(TEST_PAS_Para))
                FindDummy = TEST_PAS_Para
                DummyDisplayLegends = " - PAS"
            Case 2
                ReDim FindDummy(UBound(TEST_Etc_Para))
                FindDummy = TEST_Etc_Para
                DummyDisplayLegends = " - Etc"
        End Select

        Dim IsX As Boolean = False
        Dim IsY As Boolean = False

        For k = 0 To 31
            'X 파라미터 파악
            Tmp_StrX = TEST_Para(k, 0).Split("/")
            For m = 0 To UBound(Tmp_StrX)
                Tmp_Char = Tmp_StrX(m).Split(",")
                Marker = -1
                For i = 0 To Me.ChList.Items.Count - 1
                    If InStr(Me.ChList.Items(i), Tmp_Char(LBound(Tmp_Char))) >= 1 Then
                        Marker = UBound(Tmp_Char)
                        For j = 0 To UBound(Tmp_Char)
                            If (UCase(Tmp_Char(j)) = "TIME") AndAlso (UCase(Me.ChList.Items(i))) <> "TIME" Then
                                '채널명에 time 채널이 아닌데 time이 들어가는 것들이 생겨서 예외처리 하기위한 문====
                                Exit For
                            Else
                                If InStr(Me.ChList.Items(i), Tmp_Char(j)) >= 1 Then
                                    Marker = Marker - 1 'j '이게 -1이어야 채널명이 맞는 것이다.
                                    If Marker = -1 Then
                                        TEST_Para_Num(k, 0) = i
                                        IsX = True
                                        Exit For
                                    End If
                                End If
                            End If
                        Next
                    End If
                    If IsX <> True Then
                        TEST_Para_Num(k, 0) = -1
                    Else
                        Exit For
                    End If
                Next
            Next

            IsX = False

            'Y 파라미터
            Tmp_StrY = TEST_Para(k, 1).Split("/")
            For m = 0 To UBound(Tmp_StrY)
                Tmp_Char = Tmp_StrY(m).Split(",")
                For i = 0 To Me.ChList.Items.Count - 1
                    If InStr(Me.ChList.Items(i), Tmp_Char(LBound(Tmp_Char))) >= 1 Then
                        Marker = UBound(Tmp_Char)
                        For j = 0 To UBound(Tmp_Char)
                            If InStr(Me.ChList.Items(i), Tmp_Char(j)) >= 1 Then
                                Marker = Marker - 1 ' - j '이게 -2이어야 Y채널명이 맞는 것이다.
                                If Marker = -1 Then
                                    '여기서 더미 파라미터를 확인한다.=============
                                    For n = 0 To UBound(FindDummy)
                                        If InStr(Me.ChList.Items(i), FindDummy(n)) >= 1 Then
                                            Marker = Marker - 1
                                            If Marker = -2 Then
                                                TEST_Para_Num(k, 1) = i
                                                IsY = True
                                                Exit For
                                            End If
                                        End If
                                    Next
                                    '=============================================
                                End If
                            End If
                        Next
                    End If
                    If IsY <> True Then 'Marker = -2 Then
                        TEST_Para_Num(k, 1) = -1
                    Else
                        Exit For
                    End If
                Next
            Next

            IsY = False
        Next
        '======================================================================================================


        With Me
            '========================== 참고 사항 ==============================================================
            'TEST Parameter Variables : TEST_Para(,)
            '그래프 타이틀 : GraphTitle
            'TEST Graph Collections : TGraphCollection(1-32)
            'TEST DATA Header : DAT_Headers(,) 
            '              - (Index:(From 0~),0-데이터이름/1-시작위치/2-길이(갯수)/3-시작옵셋값/4-증분/5-Implicit여부/6-단위/7-이진데이터파일이름)
            'TEST Graph DP Class : TEST_Graphing
            'R64 파일 경로들 : DATAFilePathList()
            'OpenBinaryR64(ByVal Values As Byte(), ByVal Start_Pos As Long, ByVal Val_Count As Long) As Double()
            '==================================================================================================

            Dim Values As Byte()

            'For j = 0 To .FileListBox.Items.Count - 1  '케이스 넘버???

            If IsTDMfile(.FileListBox.SelectedIndex) = False Then
                'DAT 파일일 경우
                Dim R64File As New FileInfo(DATAFilePathList(.FileListBox.SelectedIndex) & Replace(.FileListBox.Items((.FileListBox.SelectedIndex)), ".DAT", ".R64"))
                Dim NumByte As Long = R64File.Length
                Dim Fstream As New FileStream(DATAFilePathList(.FileListBox.SelectedIndex) & Replace(.FileListBox.Items((.FileListBox.SelectedIndex)), ".DAT", ".R64"), FileMode.Open, FileAccess.Read)
                Dim FileRead As New BinaryReader(Fstream)
                'Byte Array를 읽어온다.
                Values = FileRead.ReadBytes(CInt(NumByte))

                FileRead.Close()
                Fstream.Close()

            Else
                'TDM일 경우
                Dim R64File As New FileInfo(DATAFilePathList(.FileListBox.SelectedIndex) & Replace(.FileListBox.Items((.FileListBox.SelectedIndex)), ".tdm", ".tdx"))
                Dim NumByte As Long = R64File.Length
                Dim Fstream As New FileStream(DATAFilePathList(.FileListBox.SelectedIndex) & Replace(.FileListBox.Items((.FileListBox.SelectedIndex)), ".tdm", ".tdx"), FileMode.Open, FileAccess.Read)
                Dim FileRead As New BinaryReader(Fstream)
                'Byte Array를 읽어온다.
                Values = FileRead.ReadBytes(CInt(NumByte))

                FileRead.Close()
                Fstream.Close()

                If Me.TDMCHSel.SelectedIndex < 0 Then
                    Me.TDMCHSel.SelectedIndex = 0
                End If

            End If

            

            Dim Tmp_X_Vals() As Double = Nothing
            Dim Tmp_Y_Vals() As Double

            'Replace 모드이면 모든 차트의 시리즈를 다 지운다.
            If Me.ToolStripCmbOver.SelectedIndex = 1 Then
                For t = 1 To 32
                    DelAllSeries(TGraphCollection.Item(t))
                Next

                Tot_Series = 0

            End If

            For i = 0 To 31  '그래프 개수
                '그래프 추가 작업의 시작====================================================================
                If TEST_Para_Num(i, 0) <> -1 And TEST_Para_Num(i, 1) <> -1 Then
                    If i > 0 Then
                        If DAT_Headers(TEST_Para_Num(i, 0), 1) = DAT_Headers(TEST_Para_Num(i - 1, 0), 1) _
                        And DAT_Headers(TEST_Para_Num(i, 0), 2) = DAT_Headers(TEST_Para_Num(i - 1, 0), 2) _
                        And DAT_Headers(TEST_Para_Num(i, 0), 5) <> 1 And IsNothing(Tmp_X_Vals) = False Then '앞쪽 데이터와 이름/길이가 같고, EXPLICIT 데이터인 조건
                            '이전 데이터와 X 데이터의 시작위치 및 길이(갯수)가 같으면 데이터를 한번 더 읽어올 필요가 없다.

                            '속도를 조금이라도 빠르게 함 (Y만 읽어온다)
                            'Y데이터 읽기 (Explicit)
                            If DAT_Headers(TEST_Para_Num(i, 1), 8) = 4 Then
                                Tmp_Y_Vals = TESTopen.OpenBinaryR32(Values, DAT_Headers(TEST_Para_Num(i, 1), 1), DAT_Headers(TEST_Para_Num(i, 1), 2))
                            Else
                                Tmp_Y_Vals = TESTopen.OpenBinaryR64(Values, DAT_Headers(TEST_Para_Num(i, 1), 1), DAT_Headers(TEST_Para_Num(i, 1), 2))
                            End If
                        Else
                            If DAT_Headers(TEST_Para_Num(i, 0), 5) = 1 Then
                                'Implicit DATA의 경우는 데이터를 만들어 줘야한다. (주로 X축 - 시간만 이런다.)
                                ReDim Tmp_X_Vals(CInt(DAT_Headers(TEST_Para_Num(i, 0), 2) - 1))
                                For p = 0 To UBound(Tmp_X_Vals)
                                    Tmp_X_Vals(p) = DAT_Headers(TEST_Para_Num(i, 0), 3) + (p * DAT_Headers(TEST_Para_Num(i, 0), 4))
                                Next
                            Else
                                If DAT_Headers(TEST_Para_Num(i, 0), 8) = 4 Then
                                    Tmp_X_Vals = TESTopen.OpenBinaryR32(Values, DAT_Headers(TEST_Para_Num(i, 0), 1), DAT_Headers(TEST_Para_Num(i, 0), 2))
                                Else
                                    Tmp_X_Vals = TESTopen.OpenBinaryR64(Values, DAT_Headers(TEST_Para_Num(i, 0), 1), DAT_Headers(TEST_Para_Num(i, 0), 2))
                                End If
                            End If

                            'Time의 경우 sec를 msec로 바꾼다.
                            If Tmp_X_Vals(UBound(Tmp_X_Vals)) < 2.0 And InStr(UCase(DAT_Headers(TEST_Para_Num(i, 0), 0)), "TIME") >= 1 Then
                                '시험 time 단위가 sec이면 MADYMO와 같은 msec로 바꿔준다.
                                For t = 0 To UBound(Tmp_X_Vals)
                                    Tmp_X_Vals(t) = Tmp_X_Vals(t) * 1000
                                Next
                            End If

                            'Y데이터 읽기 (Explicit)
                            If DAT_Headers(TEST_Para_Num(i, 1), 8) = 4 Then
                                Tmp_Y_Vals = TESTopen.OpenBinaryR32(Values, DAT_Headers(TEST_Para_Num(i, 1), 1), DAT_Headers(TEST_Para_Num(i, 1), 2))
                            Else
                                Tmp_Y_Vals = TESTopen.OpenBinaryR64(Values, DAT_Headers(TEST_Para_Num(i, 1), 1), DAT_Headers(TEST_Para_Num(i, 1), 2))
                            End If

                        End If
                    Else
                        '아니면 둘 다 읽어온다. (맨 처음 그래프)
                        If DAT_Headers(TEST_Para_Num(i, 0), 5) = 1 Then
                            'Implicit DATA의 경우는 데이터를 만들어 줘야한다. (주로 X축 - 시간만 이런다.)
                            ReDim Tmp_X_Vals(CInt(DAT_Headers(TEST_Para_Num(i, 0), 2) - 1))
                            For p = 0 To UBound(Tmp_X_Vals)
                                Tmp_X_Vals(p) = DAT_Headers(TEST_Para_Num(i, 0), 3) + (p * DAT_Headers(TEST_Para_Num(i, 0), 4))
                            Next
                        Else
                            If DAT_Headers(TEST_Para_Num(i, 0), 8) = 4 Then
                                Tmp_X_Vals = TESTopen.OpenBinaryR32(Values, DAT_Headers(TEST_Para_Num(i, 0), 1), DAT_Headers(TEST_Para_Num(i, 0), 2))
                            Else
                                Tmp_X_Vals = TESTopen.OpenBinaryR64(Values, DAT_Headers(TEST_Para_Num(i, 0), 1), DAT_Headers(TEST_Para_Num(i, 0), 2))
                            End If
                        End If

                        'Y데이터 읽기 (Explicit)
                        If DAT_Headers(TEST_Para_Num(i, 1), 8) = 4 Then
                            Tmp_Y_Vals = TESTopen.OpenBinaryR32(Values, DAT_Headers(TEST_Para_Num(i, 1), 1), DAT_Headers(TEST_Para_Num(i, 1), 2))
                        Else
                            Tmp_Y_Vals = TESTopen.OpenBinaryR64(Values, DAT_Headers(TEST_Para_Num(i, 1), 1), DAT_Headers(TEST_Para_Num(i, 1), 2))
                        End If

                        'Time의 경우 sec를 msec로 바꾼다.
                        If Tmp_X_Vals(UBound(Tmp_X_Vals)) < 2.0 And InStr(UCase(DAT_Headers(TEST_Para_Num(i, 0), 0)), "TIME") >= 1 Then
                            '시험 time 단위가 sec이면 MADYMO와 같은 msec로 바꿔준다.
                            For t = 0 To UBound(Tmp_X_Vals)
                                Tmp_X_Vals(t) = Tmp_X_Vals(t) * 1000
                            Next
                        End If
                    End If
                    '데이터를 그린다
                    '데이터를 그리기위한 함수 호출
                    If IsTDMfile(.FileListBox.SelectedIndex) = False Then
                        DrawingTESTgraphs(TGraphCollection.Item(i + 1), Tmp_X_Vals, Tmp_Y_Vals, .FileListBox.Items((.FileListBox.SelectedIndex)) & DummyDisplayLegends, GraphTitle(i))
                    Else
                        DrawingTESTgraphs(TGraphCollection.Item(i + 1), Tmp_X_Vals, Tmp_Y_Vals, .FileListBox.Items((.FileListBox.SelectedIndex)) & DummyDisplayLegends & ":" & _
                                          Me.TDMCHSel.Items(Me.TDMCHSel.SelectedIndex), GraphTitle(i))
                    End If

                Else
                    '데이터가 없는 것들 그리는 함수
                    Dim Zero_Arr(0) As Double
                    'DrawingTESTgraphs(TGraphCollection.Item(i + 1), Zero_Arr, Zero_Arr, .FileListBox.Items((.FileListBox.SelectedIndex)) & DummyDisplayLegends, GraphTitle(i))
                    ZeroDATA(TGraphCollection.Item(i + 1), Zero_Arr, Zero_Arr, .FileListBox.Items((.FileListBox.SelectedIndex)) & DummyDisplayLegends, GraphTitle(i))
                End If

                    '==========================================================================================
                    MainMDI.ProgressBarMain.Value = i * 100
                    MainMDI.Statuslbl.Text = "TEST Graph Displayed"
            Next
            'Next
        End With

        Tot_Series = Tot_Series + 1

    End Sub

    Private Sub DrawingTESTgraphs(ByRef Charts As Chart, ByVal X_data As Double(), ByVal Y_data As Double(), ByVal SeriesTitle As String, _
                                  ByVal GraphName As String)
        '이미 시리즈를 추가한 경우이면 그냥 나간다.
        '동승석 및 기타 채널로 바뀐 경우이면 그린다.
        Dim i As Integer
        For i = 0 To Charts.Series.Count - 1
            If Charts.Series.Item(i).Name = SeriesTitle Then Exit Sub
        Next

        With Charts
            Try
                .Series.Add(SeriesTitle).Points.DataBindXY(X_data, Y_data)
            Catch ex As Exception
                MsgBox(GraphName & "의 X-Y 데이터 크기가 일치하지 않습니다." & vbCrLf & "데이터를 확인하세요.", , "Warning")
            End Try

            .Series(SeriesTitle).ChartType = SeriesChartType.Line

            DATAPreviewDP.GraphAxisLine(Charts)

            '차트의 Min/Max/Inteverl 설정
            If X_data(UBound(X_data)) < 10 Then
                .ChartAreas(0).AxisX.Interval = 0.02
                .ChartAreas(0).AxisX.Minimum = 0.0
                .ChartAreas(0).AxisX.Maximum = 0.2
            Else
                .ChartAreas(0).AxisX.Interval = 20
                .ChartAreas(0).AxisX.Minimum = 0.0
                .ChartAreas(0).AxisX.Maximum = 200
            End If
            .ChartAreas(0).AxisY.Minimum = Double.NaN
            .ChartAreas(0).AxisY.Maximum = Double.NaN

            'Label Format (X-Y 그래프 라벨 글꼴)
            .ChartAreas(0).AxisX.LabelStyle.Font = New Font("Arial", 7, FontStyle.Bold)
            .ChartAreas(0).AxisY.LabelStyle.Font = New Font("Arial", 7, FontStyle.Bold)

            '범례 표식
            .Legends(0).Font = New Font("Arial", 7, FontStyle.Bold)

            .Update()
        End With

    End Sub

    Private Sub ZeroDATA(ByRef Charts As Chart, ByVal X_data As Double(), ByVal Y_data As Double(), ByVal SeriesTitle As String, _
                                  ByVal GraphName As String)
        '없는 데이터를 그리는데 사용하는 함수 (이게 없으면 X 축 최대값이 0.2로 되버린다.)

        '이미 시리즈를 추가한 경우이면 그냥 나간다.
        '동승석 및 기타 채널로 바뀐 경우이면 그린다.
        Dim i As Integer
        For i = 0 To Charts.Series.Count - 1
            If Charts.Series.Item(i).Name = SeriesTitle Then Exit Sub
        Next

        Charts.Series.Add(SeriesTitle).Points.DataBindXY(X_data, Y_data)
        Charts.Series(SeriesTitle).ChartType = SeriesChartType.Line
    End Sub

    Private Sub DelAllSeries(ByRef Charts As Chart)
        Dim i As Integer

        For i = Charts.Series.Count To 1 Step -1
            Charts.Series.RemoveAt(i - 1)
        Next
    End Sub

    Private Sub AddTitles(ByRef Charts As Chart, ByVal GraphName As String)
        'Title 추가 
        Charts.Titles.Add(GraphName)
        Charts.Titles(0).Font = New Font("Calibri", 9, FontStyle.Bold)
    End Sub

    Private Sub Chart00_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart00.DoubleClick
        Dim PopUpForm As New InjuryPopUp(0, Me.Chart00, 0) ', Me.Chart00.Titles(0).Text)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart01_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart01.DoubleClick
        Dim PopUpForm As New InjuryPopUp(1, Me.Chart01, 0) ', Me.Chart01.Titles(0).Text)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart02_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart02.DoubleClick
        Dim PopUpForm As New InjuryPopUp(2, Me.Chart02, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart03_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart03.DoubleClick
        Dim PopUpForm As New InjuryPopUp(3, Me.Chart03, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart04_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart04.DoubleClick
        Dim PopUpForm As New InjuryPopUp(4, Me.Chart04, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart05_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart05.DoubleClick
        Dim PopUpForm As New InjuryPopUp(5, Me.Chart05, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart06_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart06.DoubleClick
        Dim PopUpForm As New InjuryPopUp(6, Me.Chart06, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart07_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart07.DoubleClick
        Dim PopUpForm As New InjuryPopUp(7, Me.Chart07, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart08_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart08.DoubleClick
        Dim PopUpForm As New InjuryPopUp(8, Me.Chart08, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart09_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart09.DoubleClick
        Dim PopUpForm As New InjuryPopUp(9, Me.Chart09, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart10_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart10.DoubleClick
        Dim PopUpForm As New InjuryPopUp(10, Me.Chart10, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart11_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart11.DoubleClick
        Dim PopUpForm As New InjuryPopUp(11, Me.Chart11, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart12_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart12.DoubleClick
        Dim PopUpForm As New InjuryPopUp(12, Me.Chart12, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart13_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart13.DoubleClick
        Dim PopUpForm As New InjuryPopUp(13, Me.Chart13, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart14_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart14.DoubleClick
        Dim PopUpForm As New InjuryPopUp(14, Me.Chart14, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart15_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart15.DoubleClick
        Dim PopUpForm As New InjuryPopUp(15, Me.Chart15, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart16_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart16.DoubleClick
        Dim PopUpForm As New InjuryPopUp(16, Me.Chart16, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart17_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart17.DoubleClick
        Dim PopUpForm As New InjuryPopUp(17, Me.Chart17, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart18_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart18.DoubleClick
        Dim PopUpForm As New InjuryPopUp(18, Me.Chart18, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart19_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart19.DoubleClick
        Dim PopUpForm As New InjuryPopUp(19, Me.Chart19, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart20_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart20.DoubleClick
        Dim PopUpForm As New InjuryPopUp(20, Me.Chart20, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart21_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart21.DoubleClick
        Dim PopUpForm As New InjuryPopUp(21, Me.Chart21, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart22_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart22.DoubleClick
        Dim PopUpForm As New InjuryPopUp(22, Me.Chart22, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart23_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart23.DoubleClick
        Dim PopUpForm As New InjuryPopUp(23, Me.Chart23, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart24_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart24.DoubleClick
        Dim PopUpForm As New InjuryPopUp(24, Me.Chart24, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart25_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart25.DoubleClick
        Dim PopUpForm As New InjuryPopUp(25, Me.Chart25, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart26_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart26.DoubleClick
        Dim PopUpForm As New InjuryPopUp(26, Me.Chart26, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart27_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart27.DoubleClick
        Dim PopUpForm As New InjuryPopUp(27, Me.Chart27, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart28_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart28.DoubleClick
        Dim PopUpForm As New InjuryPopUp(28, Me.Chart28, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart29_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart29.DoubleClick
        Dim PopUpForm As New InjuryPopUp(29, Me.Chart29, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart30_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart30.DoubleClick
        Dim PopUpForm As New InjuryPopUp(30, Me.Chart30, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart31_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart31.DoubleClick
        Dim PopUpForm As New InjuryPopUp(31, Me.Chart31, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart32_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart32.DoubleClick
        Dim PopUpForm As New InjuryPopUp(32, Me.Chart32, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart33_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart33.DoubleClick
        Dim PopUpForm As New InjuryPopUp(32, Me.Chart33, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart34_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart34.DoubleClick
        Dim PopUpForm As New InjuryPopUp(32, Me.Chart34, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart35_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart35.DoubleClick
        Dim PopUpForm As New InjuryPopUp(32, Me.Chart35, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart36_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart36.DoubleClick
        Dim PopUpForm As New InjuryPopUp(32, Me.Chart36, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart37_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart37.DoubleClick
        Dim PopUpForm As New InjuryPopUp(32, Me.Chart37, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart38_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart38.DoubleClick
        Dim PopUpForm As New InjuryPopUp(32, Me.Chart38, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart39_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart39.DoubleClick
        Dim PopUpForm As New InjuryPopUp(32, Me.Chart39, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart40_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart40.DoubleClick
        Dim PopUpForm As New InjuryPopUp(32, Me.Chart40, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart41_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart41.DoubleClick
        Dim PopUpForm As New InjuryPopUp(32, Me.Chart41, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart42_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart42.DoubleClick
        Dim PopUpForm As New InjuryPopUp(32, Me.Chart42, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart43_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart43.DoubleClick
        Dim PopUpForm As New InjuryPopUp(32, Me.Chart43, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart44_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart44.DoubleClick
        Dim PopUpForm As New InjuryPopUp(32, Me.Chart44, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart45_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart45.DoubleClick
        Dim PopUpForm As New InjuryPopUp(32, Me.Chart45, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart46_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart46.DoubleClick
        Dim PopUpForm As New InjuryPopUp(32, Me.Chart46, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub
    Private Sub Chart47_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chart47.DoubleClick
        Dim PopUpForm As New InjuryPopUp(32, Me.Chart47, 0)
        PopUpForm.Owner = Me
        PopUpForm.Show()
    End Sub

    Private Sub LegendOnOffToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LegendOnOffToolStripMenuItem.Click

        Dim i As Integer

        If Me.LegendOnOffToolStripMenuItem.Checked = True Then
            For i = 1 To 32
                DATAPreviewDP.ShowLegends(TGraphCollection.Item(i))
            Next
            For i = 1 To 16
                DATAPreviewDP.ShowLegends(AddGraphCollection.Item(i))
            Next
        ElseIf Me.LegendOnOffToolStripMenuItem.Checked = False Then
            For i = 1 To 32
                DATAPreviewDP.HideLegends(TGraphCollection.Item(i))
            Next
            For i = 1 To 16
                DATAPreviewDP.HideLegends(AddGraphCollection.Item(i))
            Next
        End If
    End Sub

    Private Sub TopToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TopToolStripMenuItem.Click
        Dim i As Integer
        For i = 1 To 32
            WholeChartOption.TopLegends(TGraphCollection.Item(i))
        Next
        For i = 1 To 16
            WholeChartOption.TopLegends(AddGraphCollection.Item(i))
        Next
    End Sub
    Private Sub BottmToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BottmToolStripMenuItem.Click
        Dim i As Integer
        For i = 1 To 32
            WholeChartOption.BottomLegends(TGraphCollection.Item(i))
        Next
        For i = 1 To 16
            WholeChartOption.BottomLegends(AddGraphCollection.Item(i))
        Next
    End Sub
    Private Sub LeftToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LeftToolStripMenuItem.Click
        Dim i As Integer
        For i = 1 To 32
            WholeChartOption.LeftLegends(TGraphCollection.Item(i))
        Next
        For i = 1 To 16
            WholeChartOption.LeftLegends(AddGraphCollection.Item(i))
        Next
    End Sub
    Private Sub RightToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RightToolStripMenuItem.Click
        Dim i As Integer
        For i = 1 To 32
            WholeChartOption.RightLegends(TGraphCollection.Item(i))
        Next
        For i = 1 To 16
            WholeChartOption.RightLegends(AddGraphCollection.Item(i))
        Next
    End Sub


    Private Sub Chart32_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart32.Click
        Me.Chart32.Focus()
    End Sub
    Private Sub Chart33_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart33.Click
        Me.Chart33.Focus()
    End Sub
    Private Sub Chart34_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart34.Click
        Me.Chart34.Focus()
    End Sub
    Private Sub Chart35_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart35.Click
        Me.Chart35.Focus()
    End Sub
    Private Sub Chart36_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart36.Click
        Me.Chart36.Focus()
    End Sub
    Private Sub Chart37_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart37.Click
        Me.Chart37.Focus()
    End Sub
    Private Sub Chart38_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart38.Click
        Me.Chart38.Focus()
    End Sub
    Private Sub Chart39_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart39.Click
        Me.Chart39.Focus()
    End Sub
    Private Sub Chart40_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart40.Click
        Me.Chart40.Focus()
    End Sub
    Private Sub Chart41_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart41.Click
        Me.Chart41.Focus()
    End Sub
    Private Sub Chart42_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart42.Click
        Me.Chart42.Focus()
    End Sub
    Private Sub Chart43_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart43.Click
        Me.Chart43.Focus()
    End Sub
    Private Sub Chart44_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart44.Click
        Me.Chart44.Focus()
    End Sub
    Private Sub Chart45_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart45.Click
        Me.Chart45.Focus()
    End Sub
    Private Sub Chart46_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart46.Click
        Me.Chart46.Focus()
    End Sub
    Private Sub Chart47_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart47.Click
        Me.Chart47.Focus()
    End Sub

    Private Sub Chart32_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart32.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart32.Series.Count To 1 Step -1
                Me.Chart32.Series.RemoveAt(i - 1)
            Next
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.V Then
            Me.ListBox1.SelectedIndex = 32
            Call ListBox1_KeyDown(sender, e)
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            Me.ListBox1.SelectedIndex = 32
            Call ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub chart33_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart33.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart33.Series.Count To 1 Step -1
                Me.Chart33.Series.RemoveAt(i - 1)
            Next
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.V Then
            Me.ListBox1.SelectedIndex = 33
            Call ListBox1_KeyDown(sender, e)
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            Me.ListBox1.SelectedIndex = 33
            Call ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub chart34_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart34.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart34.Series.Count To 1 Step -1
                Me.Chart34.Series.RemoveAt(i - 1)
            Next
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.V Then
            Me.ListBox1.SelectedIndex = 34
            Call ListBox1_KeyDown(sender, e)
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            Me.ListBox1.SelectedIndex = 34
            Call ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub chart35_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart35.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart35.Series.Count To 1 Step -1
                Me.Chart35.Series.RemoveAt(i - 1)
            Next
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.V Then
            Me.ListBox1.SelectedIndex = 35
            Call ListBox1_KeyDown(sender, e)
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            Me.ListBox1.SelectedIndex = 35
            Call ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub chart36_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart36.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart36.Series.Count To 1 Step -1
                Me.Chart36.Series.RemoveAt(i - 1)
            Next
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.V Then
            Me.ListBox1.SelectedIndex = 36
            Call ListBox1_KeyDown(sender, e)
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            Me.ListBox1.SelectedIndex = 36
            Call ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub chart37_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart37.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart37.Series.Count To 1 Step -1
                Me.Chart37.Series.RemoveAt(i - 1)
            Next
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.V Then
            Me.ListBox1.SelectedIndex = 37
            Call ListBox1_KeyDown(sender, e)
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            Me.ListBox1.SelectedIndex = 37
            Call ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub chart38_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart38.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart38.Series.Count To 1 Step -1
                Me.Chart38.Series.RemoveAt(i - 1)
            Next
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.V Then
            Me.ListBox1.SelectedIndex = 38
            Call ListBox1_KeyDown(sender, e)
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            Me.ListBox1.SelectedIndex = 38
            Call ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub chart39_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart39.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart39.Series.Count To 1 Step -1
                Me.Chart39.Series.RemoveAt(i - 1)
            Next
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.V Then
            Me.ListBox1.SelectedIndex = 39
            Call ListBox1_KeyDown(sender, e)
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            Me.ListBox1.SelectedIndex = 39
            Call ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub chart40_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart40.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart40.Series.Count To 1 Step -1
                Me.Chart40.Series.RemoveAt(i - 1)
            Next
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.V Then
            Me.ListBox1.SelectedIndex = 40
            Call ListBox1_KeyDown(sender, e)
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            Me.ListBox1.SelectedIndex = 40
            Call ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub chart41_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart41.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart41.Series.Count To 1 Step -1
                Me.Chart41.Series.RemoveAt(i - 1)
            Next
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.V Then
            Me.ListBox1.SelectedIndex = 41
            Call ListBox1_KeyDown(sender, e)
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            Me.ListBox1.SelectedIndex = 41
            Call ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub chart42_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart42.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart42.Series.Count To 1 Step -1
                Me.Chart42.Series.RemoveAt(i - 1)
            Next
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.V Then
            Me.ListBox1.SelectedIndex = 42
            Call ListBox1_KeyDown(sender, e)
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            Me.ListBox1.SelectedIndex = 42
            Call ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub chart43_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart43.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart43.Series.Count To 1 Step -1
                Me.Chart43.Series.RemoveAt(i - 1)
            Next
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.V Then
            Me.ListBox1.SelectedIndex = 43
            Call ListBox1_KeyDown(sender, e)
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            Me.ListBox1.SelectedIndex = 43
            Call ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub chart44_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart44.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart44.Series.Count To 1 Step -1
                Me.Chart44.Series.RemoveAt(i - 1)
            Next
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.V Then
            Me.ListBox1.SelectedIndex = 44
            Call ListBox1_KeyDown(sender, e)
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            Me.ListBox1.SelectedIndex = 44
            Call ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub chart45_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart45.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart45.Series.Count To 1 Step -1
                Me.Chart45.Series.RemoveAt(i - 1)
            Next
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.V Then
            Me.ListBox1.SelectedIndex = 45
            Call ListBox1_KeyDown(sender, e)
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            Me.ListBox1.SelectedIndex = 45
            Call ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub chart46_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart46.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart46.Series.Count To 1 Step -1
                Me.Chart46.Series.RemoveAt(i - 1)
            Next
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.V Then
            Me.ListBox1.SelectedIndex = 46
            Call ListBox1_KeyDown(sender, e)
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            Me.ListBox1.SelectedIndex = 46
            Call ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub chart47_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart47.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim i As Integer
            For i = Me.Chart47.Series.Count To 1 Step -1
                Me.Chart47.Series.RemoveAt(i - 1)
            Next
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.V Then
            Me.ListBox1.SelectedIndex = 47
            Call ListBox1_KeyDown(sender, e)
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            Me.ListBox1.SelectedIndex = 47
            Call ListBox1_KeyDown(sender, e)
        End If
    End Sub

    Private Sub TDMCHSel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TDMCHSel.SelectedIndexChanged
        Dim i As Integer

        If TDMCHselEventPass = True Then Exit Sub

        With Me
            If .FileListBox.SelectedIndex >= 0 Then
                '파일을 선택하면 채널명을 리스트에 쓴다.

                '기존의 리스트 항목 삭제
                EventPass = True
                .CmbXaxis.SelectedIndex = -1
                .ChList.SelectedIndex = -1
                EventPass = False
                For i = .ChList.Items.Count To 1 Step -1
                    .ChList.Items.RemoveAt(i - 1)
                    .CmbXaxis.Items.RemoveAt(i - 1)
                Next

                '채널명 추가
                '선택을 하면 헤더를 읽어온다.
                For i = 0 To UBound(Me.OpenTESTDlg.FileNames)
                    '헤더를 읽어온다.
                    If IsTDMfile(.FileListBox.SelectedIndex) = False Then
                        DAT_Headers = OepnDATfileHeader(DATAFilePathList(.FileListBox.SelectedIndex) & _
                                                    .FileListBox.SelectedItem) 'Me.OpenTESTDlg.FileNames(i)) 'OpenFile.names(i))
                    ElseIf IsTDMfile(.FileListBox.SelectedIndex) = True Then

                        DAT_Headers = OepnTDMfileHeader(DATAFilePathList(.FileListBox.SelectedIndex) & .FileListBox.SelectedItem, .TDMCHSel.SelectedIndex + 1)
                    End If
                Next
                For i = 0 To UBound(DAT_Headers, 1)
                    .ChList.Items.Add(DAT_Headers(i, 0))
                    .CmbXaxis.Items.Add(DAT_Headers(i, 0))
                    'X라벨은 Time을 찾아서 한다. (없음 말고)
                    If UCase(.CmbXaxis.Items(i).ToString) = "TIME" Then
                        .CmbXaxis.SelectedIndex = i
                    End If
                Next
            End If
        End With
    End Sub

    Private Sub MainTESTSplitter_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles MainTESTSplitter.SplitterMoved
        Call FrmTEST_DATA_Resize(sender, Nothing)
    End Sub

    Private Sub ApplyTESTResult_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApplyTESTResult.Click
        Call BtnApply_Click(sender, Nothing)
    End Sub

    Private Sub ListBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListBox1.KeyDown
        Dim SelectedInList As Integer = Me.ListBox1.SelectedIndex + 1

        Try

            If e.Modifiers = Keys.Control And e.KeyCode = Keys.V Then

                Dim MsgResult As Integer = MessageBox.Show("Maintain Original Axis Setting?" & vbCrLf & "** Yes - Maintain Original Axis" _
                                                           & vbCrLf & "** No - Set All Series to Primary Axis" _
                                                           & vbCrLf & "** Cancel - Set All Series to Secondary Axis", "Select Axis Type", MessageBoxButtons.YesNoCancel)

                If MsgResult = DialogResult.Cancel Then
                    InjuryChart_CopyEventEnd_Secondary(MovinChrt, CopyGraphCollection(SelectedInList))
                ElseIf MsgResult = DialogResult.No Then
                    InjuryChart_CopyEventEnd_Primary(MovinChrt, CopyGraphCollection(SelectedInList))
                ElseIf MsgResult = DialogResult.Yes Then
                    InjuryChart_CopyEventEnd_Maintain(MovinChrt, CopyGraphCollection(SelectedInList))
                End If

            ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then

                '복사할 그래프
                MovinChrt = CopyGraphCollection(SelectedInList)

            End If

        Catch ex As NullReferenceException

            MsgBox("Select and Copy(Ctrl+C) Graph First", MsgBoxStyle.Critical, "Empty Chart")
            Exit Sub

        End Try
    End Sub

    Private Sub InjuryChart_CopyEventEnd_Primary(ByRef MoveChart As Chart, ByRef TChart As Chart)
        Dim i, j As Integer
        Dim tmp As DataPointCollection
        Dim Titles As String
        Dim Temp_title() As String

        With Me
            '데이터 포인트 컬랙션을 복사한다.===============================================================================
            For i = 0 To MoveChart.Series.Count - 1
                Try
                    Titles = MoveChart.Titles(0).Text & " / " & MoveChart.Series(i).Name
                Catch ex As Exception
                    Titles = GraphTitle(32) & " / " & MoveChart.Series(i).Name
                End Try
                Temp_title = Titles.Split("/")
                Titles = Trim(Temp_title(UBound(Temp_title) - 1)) & " / " & Trim(Temp_title(UBound(Temp_title)))
                tmp = MoveChart.Series(i).Points
                Try
                    TChart.Series.Add(Titles)
                Catch ex As Exception '같은 이름이 있는경우
                    'TChart.Series.Add(Titles & "-Re")  --> 뭔가 이상하게 그려진다....???
                    MsgBox("Same series already exists.", MsgBoxStyle.Critical, "Repetition")
                    DrawingADDgraphs(TChart)
                    Exit Sub
                End Try

                With TChart
                    .Series(Titles).YAxisType = AxisType.Primary
                    .Series(Titles).ChartType = SeriesChartType.Line
                End With

                For j = 0 To tmp.Count - 1
                    TChart.Series(Titles).Points.AddXY(tmp(j).XValue, tmp(j).YValues(0))
                Next
            Next
            TChart.Update()
            DrawingADDgraphs(TChart)
        End With
    End Sub

    Private Sub InjuryChart_CopyEventEnd_Secondary(ByRef MoveChart As Chart, ByRef TChart As Chart)
        Dim i, j As Integer
        Dim tmp As DataPointCollection
        Dim Titles As String
        Dim Temp_title() As String

        With Me
            '데이터 포인트 컬랙션을 복사한다.===============================================================================
            For i = 0 To MoveChart.Series.Count - 1
                Try
                    Titles = MoveChart.Titles(0).Text & " / " & MoveChart.Series(i).Name
                Catch ex As Exception
                    Titles = GraphTitle(32) & " / " & MoveChart.Series(i).Name
                End Try
                Temp_title = Titles.Split("/")
                Titles = Trim(Temp_title(UBound(Temp_title) - 1)) & " / " & Trim(Temp_title(UBound(Temp_title)))
                tmp = MoveChart.Series(i).Points
                Try
                    TChart.Series.Add(Titles)
                Catch ex As Exception '같은 이름이 있는경우
                    'TChart.Series.Add(Titles & "-Re")  --> 뭔가 이상하게 그려진다....???
                    MsgBox("Same series already exists.", MsgBoxStyle.Critical, "Repetition")
                    DrawingADDgraphs(TChart)
                    Exit Sub
                End Try

                With TChart
                    .Series(Titles).YAxisType = AxisType.Secondary
                    .Series(.Series.Count - 1).ChartType = SeriesChartType.Line
                    .ChartAreas(0).AxisY2.MinorGrid.Enabled = False
                    .ChartAreas(0).AxisY2.MajorGrid.Enabled = False
                    .ChartAreas(0).AxisY2.MajorTickMark.Enabled = True
                    .ChartAreas(0).AxisY2.LabelStyle.Font = New Font("Arial", .ChartAreas(0).AxisY.LabelStyle.Font.Size - 1, FontStyle.Bold)
                End With

                For j = 0 To tmp.Count - 1
                    TChart.Series(Titles).Points.AddXY(tmp(j).XValue, tmp(j).YValues(0))
                Next
            Next
            TChart.Update()
            DrawingADDgraphs(TChart)
        End With
    End Sub

    Private Sub InjuryChart_CopyEventEnd_Maintain(ByRef MoveChart As Chart, ByRef TChart As Chart)
        Dim i, j As Integer
        Dim tmp As DataPointCollection
        Dim Titles As String
        Dim Temp_title() As String

        With Me
            '데이터 포인트 컬랙션을 복사한다.===============================================================================
            For i = 0 To MoveChart.Series.Count - 1
                Try
                    Titles = MoveChart.Titles(0).Text & " / " & MoveChart.Series(i).Name
                Catch ex As Exception
                    Titles = GraphTitle(32) & " / " & MoveChart.Series(i).Name
                End Try
                Temp_title = Titles.Split("/")
                Titles = Trim(Temp_title(UBound(Temp_title) - 1)) & " / " & Trim(Temp_title(UBound(Temp_title)))
                tmp = MoveChart.Series(i).Points
                Try
                    TChart.Series.Add(Titles)
                Catch ex As Exception '같은 이름이 있는경우
                    'TChart.Series.Add(Titles & "-Re")  --> 뭔가 이상하게 그려진다....???
                    MsgBox("Same series already exists.", MsgBoxStyle.Critical, "Repetition")
                    DrawingADDgraphs(TChart)
                    Exit Sub
                End Try

                With TChart
                    .Series(.Series.Count - 1).ChartType = SeriesChartType.Line
                    Select Case MoveChart.Series(i).YAxisType
                        Case AxisType.Primary
                            .Series(TChart.Series.Count - 1).YAxisType = AxisType.Primary
                        Case AxisType.Secondary
                            .ChartAreas(0).AxisY2.Enabled = AxisEnabled.True
                            .Series(TChart.Series.Count - 1).YAxisType = AxisType.Secondary
                    End Select
                    .ChartAreas(0).AxisY2.MinorGrid.Enabled = False
                    .ChartAreas(0).AxisY2.MajorGrid.Enabled = False
                    .ChartAreas(0).AxisY2.MajorTickMark.Enabled = True
                    .ChartAreas(0).AxisY2.LabelStyle.Font = New Font("Arial", .ChartAreas(0).AxisY.LabelStyle.Font.Size - 1, FontStyle.Bold)
                End With

                '.Series(MoveChart.Series.Count + i).YAxisType = AxisType.Secondary

                For j = 0 To tmp.Count - 1
                    TChart.Series(Titles).Points.AddXY(tmp(j).XValue, tmp(j).YValues(0))
                Next
            Next
            TChart.Update()
            DrawingADDgraphs(TChart)
        End With
    End Sub

    Private Sub Chart00_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart00.Click
        Me.ListBox1.SelectedIndex = 0
        Me.Chart00.Focus()
    End Sub
    Private Sub Chart01_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart01.Click
        Me.ListBox1.SelectedIndex = 1
        Me.Chart01.Focus()
    End Sub
    Private Sub Chart02_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart02.Click
        Me.ListBox1.SelectedIndex = 2
        Me.Chart02.Focus()
    End Sub
    Private Sub Chart03_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart03.Click
        Me.ListBox1.SelectedIndex = 3
        Me.Chart03.Focus()
    End Sub
    Private Sub Chart04_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart04.Click
        Me.ListBox1.SelectedIndex = 4
        Me.Chart04.Focus()
    End Sub
    Private Sub Chart05_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart05.Click
        Me.ListBox1.SelectedIndex = 5
        Me.Chart05.Focus()
    End Sub
    Private Sub Chart06_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart06.Click
        Me.ListBox1.SelectedIndex = 6
        Me.Chart06.Focus()
    End Sub
    Private Sub Chart07_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart07.Click
        Me.ListBox1.SelectedIndex = 7
        Me.Chart07.Focus()
    End Sub
    Private Sub Chart08_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart08.Click
        Me.ListBox1.SelectedIndex = 8
        Me.Chart08.Focus()
    End Sub
    Private Sub Chart09_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart09.Click
        Me.ListBox1.SelectedIndex = 9
        Me.Chart09.Focus()
    End Sub
    Private Sub Chart10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart10.Click
        Me.ListBox1.SelectedIndex = 10
        Me.Chart10.Focus()
    End Sub
    Private Sub Chart11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart11.Click
        Me.ListBox1.SelectedIndex = 11
        Me.Chart11.Focus()
    End Sub
    Private Sub Chart12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart12.Click
        Me.ListBox1.SelectedIndex = 12
        Me.Chart12.Focus()
    End Sub
    Private Sub Chart13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart13.Click
        Me.ListBox1.SelectedIndex = 13
        Me.Chart13.Focus()
    End Sub
    Private Sub Chart14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart14.Click
        Me.ListBox1.SelectedIndex = 14
        Me.Chart14.Focus()
    End Sub
    Private Sub Chart15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart15.Click
        Me.ListBox1.SelectedIndex = 15
        Me.Chart15.Focus()
    End Sub
    Private Sub Chart16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart16.Click
        Me.ListBox1.SelectedIndex = 16
        Me.Chart16.Focus()
    End Sub
    Private Sub Chart17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart17.Click
        Me.ListBox1.SelectedIndex = 17
        Me.Chart17.Focus()
    End Sub
    Private Sub Chart18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart18.Click
        Me.ListBox1.SelectedIndex = 18
        Me.Chart18.Focus()
    End Sub
    Private Sub Chart19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart19.Click
        Me.ListBox1.SelectedIndex = 19
        Me.Chart19.Focus()
    End Sub
    Private Sub Chart20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart20.Click
        Me.ListBox1.SelectedIndex = 20
        Me.Chart20.Focus()
    End Sub
    Private Sub Chart21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart21.Click
        Me.ListBox1.SelectedIndex = 21
        Me.Chart21.Focus()
    End Sub
    Private Sub Chart22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart22.Click
        Me.ListBox1.SelectedIndex = 22
        Me.Chart22.Focus()
    End Sub
    Private Sub Chart23_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart23.Click
        Me.ListBox1.SelectedIndex = 23
        Me.Chart23.Focus()
    End Sub
    Private Sub Chart24_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart24.Click
        Me.ListBox1.SelectedIndex = 24
        Me.Chart24.Focus()
    End Sub
    Private Sub Chart25_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart25.Click
        Me.ListBox1.SelectedIndex = 25
        Me.Chart25.Focus()
    End Sub
    Private Sub Chart26_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart26.Click
        Me.ListBox1.SelectedIndex = 26
        Me.Chart26.Focus()
    End Sub
    Private Sub Chart27_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart27.Click
        Me.ListBox1.SelectedIndex = 27
        Me.Chart27.Focus()
    End Sub
    Private Sub Chart28_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart28.Click
        Me.ListBox1.SelectedIndex = 28
        Me.Chart28.Focus()
    End Sub
    Private Sub Chart29_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart29.Click
        Me.ListBox1.SelectedIndex = 29
        Me.Chart29.Focus()
    End Sub
    Private Sub Chart30_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart30.Click
        Me.ListBox1.SelectedIndex = 30
        Me.Chart30.Focus()
    End Sub
    Private Sub Chart31_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart31.Click
        Me.ListBox1.SelectedIndex = 31
        Me.Chart31.Focus()
    End Sub

    Private Sub Chart00_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart00.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(1))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart01_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart01.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(2))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart02_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart02.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(3))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart03_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart03.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(4))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart04_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart04.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(5))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart05_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart05.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(6))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart06_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart06.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(7))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart07_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart07.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(8))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart08_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart08.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(9))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart09_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart09.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(10))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart10_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart10.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(11))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart11_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart11.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(12))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart12_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart12.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(13))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart13_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart13.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(14))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart14_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart14.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(15))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart15_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart15.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(16))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart16_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart16.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(17))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart17_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart17.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(18))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart18_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart18.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(19))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart19_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart19.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(20))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart20_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart20.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(21))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart21_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart21.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(22))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart22_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart22.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(23))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart23_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart23.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(24))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart24_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart24.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(25))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart25_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart25.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(26))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart26_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart26.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(27))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart27_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart27.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(28))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart28_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart28.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(29))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart29_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart29.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(30))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart30_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart30.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(31))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub
    Private Sub Chart31_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chart31.KeyDown
        If e.KeyCode = Keys.Delete Then
            ResetGraph(TGraphCollection.Item(32))
        Else
            ListBox1_KeyDown(sender, e)
        End If
    End Sub

    Private Sub ResetGraph(ByVal chrt As Chart)
        Dim i As Integer

        With chrt
            If .Series.Count <= Tot_Series Then Exit Sub
            For i = .Series.Count To Tot_Series + 1 Step -1
                .Series.RemoveAt(i - 1)
            Next
        End With
    End Sub

    Private Sub NewChName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles NewChName.KeyPress
        'If Me.ChList.SelectedIndex < 0 Then Exit Sub
        'If Me.FileListBox.SelectedIndex < 0 Then Exit Sub

        'If e.KeyChar = ChrW(Keys.Enter) Then
        '    If IsTDMfile(Me.FileListBox.SelectedIndex) = True Then
        '        Dim HeaderFile As StreamReader
        '        Dim Tmp_read As String
        '        Dim k As Integer = 0
        '        Dim m As Integer = -1
        '        Dim Paths As String = DATAFilePathList(Me.FileListBox.SelectedIndex) & Me.FileListBox.SelectedItem
        '        HeaderFile = New StreamReader(Paths)

        '        Do While Not HeaderFile.EndOfStream
        '            Tmp_read = HeaderFile.ReadLine
        '            If InStr(Tmp_read, "<tdm_channelgroup id=""") > 0 Then
        '                m = m + 1
        '                Select Case m
        '                    Case Me.TDMCHSel.SelectedIndex
        '                        Do While Not HeaderFile.EndOfStream
        '                            Tmp_read = HeaderFile.ReadLine
        '                            If InStr(Tmp_read, "<channels>") > 0 Then
        '                                '여기서 채널의 usiXXX ID를 읽어 판별한다.
        '                                'Exit Do 
        '                            End If
        '                        Loop
        '                End Select
        '            End If
        '            '여기서 채널위치를 찾음 : <tdm_channel id="usi823">
        '            '한줄을 더 읽어서 이름을 바꿈
        '            '<name>Time</name>
        '            '근데 아직 텍스트만 바꾸는 방법을 모르겠음
        '        Loop
        '    Else
        '        Exit Sub
        '    End If
        'End If
    End Sub

End Class