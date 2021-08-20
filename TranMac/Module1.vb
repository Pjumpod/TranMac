Imports System.Console
Imports System.String
Imports System.Text
Imports System.Text.RegularExpressions


Module Module1
    Private m_DataReceived As New StringBuilder
    Sub Main()
        m_DataReceived.Clear()
        Dim clArgs() As String = Environment.GetCommandLineArgs()

        Dim args As String = String.Empty

        For i As Integer = 1 To clArgs.Count() - 1
            args += " " + clArgs(i)
        Next
        args = args.Replace("'", Convert.ToChar(34)).Replace("|", Convert.ToChar(34)).Replace("@", Convert.ToChar(34))

        Dim PrcStartInfo As New ProcessStartInfo With {.CreateNoWindow = True,
                                                   .RedirectStandardError = True,
                                                   .RedirectStandardOutput = True,
                                                   .UseShellExecute = False,
                                                   .Arguments = String.Format(args),
                                                   .FileName = "SerialConsole.exe"}

        Dim Prc As New Process()
        With Prc.StartInfo
            .FileName = "SerialConsole.exe"
            .CreateNoWindow = True
            .UseShellExecute = False
            .RedirectStandardOutput = True
            '.RedirectStandardError = True
            .Arguments = args
        End With
        Prc.Start()
        Dim std_out As String = Prc.StandardOutput.ReadToEnd()
        Prc.WaitForExit()
        'Dim std_out As String = m_DataReceived.ToString()

        'Dim clArgs() As String = Environment.GetCommandLineArgs()
        'Dim args As String = String.Empty

        'For i As Integer = 1 To clArgs.Count() - 1
        'args += clArgs(i)
        'Next
        std_out = Regex.Replace(std_out, "^\s+$[\r\n]*", "", RegexOptions.Multiline)
        If std_out.Length() > 1 Then

            std_out = std_out.Trim().Replace(">", "").Replace("device:", "").Replace("0x", "").Replace(" ", "")
            std_out = Regex.Replace(std_out, "^\s+$[\r\n]*", "", RegexOptions.Multiline)
        End If
        'System.Console.Clear()
        'System.Console.WriteLine(args)
        System.Console.Write(std_out.Trim())
    End Sub

    Private Sub HandleDataReceived(ByVal sender As Object, ByVal e As DataReceivedEventArgs)

        m_DataReceived.Append(e.Data)

    End Sub

End Module
