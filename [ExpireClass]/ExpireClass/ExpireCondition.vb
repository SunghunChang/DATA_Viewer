Imports System

Public Class ExpireCondition

    ''' <summary>
    ''' #9/15/2015#
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ExpireDate As Date = #9/15/2015#
    Public Property CurrentDate As Date = Now

    ''' <summary>
    ''' 만료일 : 장성훈 '16.12.25 / 이외 '15.10.25
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsExpire() As Date
        Select Case System.Environment.UserName
            Case "6002317" '장성훈
                IsExpire = #12/25/2016#
            Case "5805793", "6304537", "9562745", "6005885" '양현모 박홍익 최성철
                IsExpire = #10/25/2016#
            Case Else
                IsExpire = #10/25/2016#
        End Select
    End Function
End Class
