Imports ExpireClass

Public NotInheritable Class AboutProgram

    Dim DateChk As New ExpireClass.ExpireCondition

    Private Sub AboutProgram_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        MainMDI.Statuslbl.Text = "Status"
    End Sub

    Private Sub AboutProgram_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MainMDI.Statuslbl.Text = "Program Information"
        ' 폼의 제목을 설정합니다
        Dim ApplicationTitle As String
        If My.Application.Info.Title <> "" Then
            ApplicationTitle = My.Application.Info.Title
        Else
            ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Me.Text = String.Format("정보 {0}", ApplicationTitle)
        ' 정보 상자에 표시되는 모든 텍스트를 초기화합니다.
        ' TODO: "프로젝트" 메뉴에서 선택하여 표시되는 프로젝트 속성 대화 상자의 "응용 프로그램" 창에서 응용 프로그램의 
        '    어셈블리 정보를 사용자 지정합니다.
        Me.LabelProductName.Text = My.Application.Info.ProductName
        Me.LabelVersion.Text = String.Format("Version {0}", My.Application.Info.Version.ToString)
        Me.LabelCopyright.Text = My.Application.Info.Copyright
        Me.LabelCompanyName.Text = My.Application.Info.CompanyName
        Me.TextBoxDescription.Text = My.Application.Info.Description
        Me.TextBoxDescription.Text = Me.TextBoxDescription.Text & vbCrLf & vbCrLf & "The entire original source code are made by SungHun, Chang" & vbCrLf & vbCrLf & _
            "Anyone who is in need of this program can use without any agreement of the developer or owner." & vbCrLf & vbCrLf & _
            "But any modification of source code(or Program) without developer's consent is not allowed, and any question or inquiry about this source code line will be ignored by the developer." & vbCrLf & vbCrLf & _
            "The developer is no Liability to reply any question of user or to modify the source code(or Program) for USER convenience." & vbCrLf & vbCrLf & _
            "1.이 프로그램의 모든 사항에 대한 개발은" & vbCrLf & _
            "  장성훈 연구원에 의해 수행되었습니다." & vbCrLf & _
            "  MADYMO®의 상표를 제외한 프로그램 내부의" & vbCrLf & _
            "  모든 사항에 대한 저작권은 개발자에게 있습니다." & vbCrLf & _
            "  [이 프로그램은 회사의 자산이 아닙니다.]" & vbCrLf & vbCrLf & _
            "2.개인 용도로 개발된 프로그램이지만 " & vbCrLf & _
            "  사용하기 바라는 사람은 사용 가능합니다." & vbCrLf & vbCrLf & _
            "3.개발자의 동의 없는 프로그램 수정은 허용되지 " & vbCrLf & _
            "  않으며, 소스 코드에 대한 질문은 답변하지 " & vbCrLf & _
            "  않습니다." & vbCrLf & vbCrLf & _
            "4.또한 개발자는 사용자 편의를 위한 " & vbCrLf & _
            "  소스코드 수정 요청에 부응할 의무는 없습니다."

        Me.Location = New Point(Screen.PrimaryScreen.Bounds.Width / 2 - Me.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - Me.Height / 2)
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub

    Private Sub TextBoxDescription_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBoxDescription.KeyDown
        If e.Modifiers = Keys.Control And e.KeyCode = Keys.I Then
            MsgBox("Program Expire Information" & vbCrLf & " - " & DateDiff(DateInterval.Day, DateChk.CurrentDate, DateChk.IsExpire) & " days left", MsgBoxStyle.Information, "Limited Version")
        End If
    End Sub
End Class
