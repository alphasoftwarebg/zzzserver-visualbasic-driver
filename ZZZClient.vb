'
'  ZZZClient.vb
'  
'	Copyright 2017 ZZZ Ltd. - Bulgaria
'
'	Licensed under the Apache License, Version 2.0 (the "License");
'	you may not use this file except in compliance with the License.
'	You may obtain a copy of the License at
'
'	http://www.apache.org/licenses/LICENSE-2.0
'
'	Unless required by applicable law or agreed to in writing, software
'	distributed under the License is distributed on an "AS IS" BASIS,
'	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
'	See the License for the specific language governing permissions and
'	limitations under the License.
'

Imports System.Net.Sockets
Imports System.Text

Module ZZZClientVB

    Function ZZZProgram(ByVal serverHost As String, ByVal serverPort As String, ByVal program As String) As String
        Try
            If serverHost = "localhost" Then
                serverHost = "127.0.0.1"
            End If
            Dim tc As New TcpClient(serverHost, serverPort)
            Dim ns As NetworkStream = tc.GetStream()

            If ns.CanWrite And ns.CanRead Then
                ' Do a simple write.
                Dim sendBytes As [Byte]() = Encoding.UTF8.GetBytes(program & Chr(0))
                ns.Write(sendBytes, 0, sendBytes.Length)

                ' Read the NetworkStream into a byte buffer.
                Dim bytes(tc.ReceiveBufferSize) As Byte
                Dim returndata As StringBuilder = New StringBuilder()
                Dim receivedBytes As Integer = ns.Read(bytes, 0, tc.ReceiveBufferSize)
                returndata.Append(Encoding.UTF8.GetString(bytes, 0, receivedBytes))
                While receivedBytes > 0
                    receivedBytes = ns.Read(bytes, 0, tc.ReceiveBufferSize)
                    returndata.Append(Encoding.UTF8.GetString(bytes, 0, receivedBytes))
                End While

                ' Return the data received from the host.
                Return returndata.ToString()
            Else
                If Not ns.CanRead Then
                    Console.WriteLine("cannot not read data from this stream")
                    tc.Close()
                Else
                    If Not ns.CanWrite Then
                        Console.WriteLine("cannot write data to this stream")
                        tc.Close()
                    End If
                End If
            End If
        Catch e As Exception
            Console.WriteLine(e)
        End Try

        Return ""
    End Function

    Sub Main()
        Console.OutputEncoding = Encoding.UTF8

        Dim startTime As DateTime = DateTime.UtcNow

        Console.WriteLine(ZZZProgram("localhost", 3333, "#[cout;Hello World from ZZZServer!]"))

        Dim stopTime As DateTime = DateTime.UtcNow
        Dim elapsedTime As Long = stopTime.Millisecond - startTime.Millisecond
        Console.WriteLine(elapsedTime.ToString + " milliseconds")

        Console.ReadKey()
    End Sub

End Module
