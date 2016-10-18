Imports System.Security.Principal
Imports System.Security.Permissions
Imports System.Runtime.InteropServices

Public Class ImpersonateUser
    Private Shared tokenHandle As IntPtr = IntPtr.Zero
    Private Shared impersonatedUser As WindowsImpersonationContext

    <PermissionSetAttribute(SecurityAction.Demand, Name:="FullTrust")>
    Public Shared Sub Impersonate(domainName As String, userName As String, password As String)
        Const LOGON32_PROVIDER_DEFAULT = 0
        Const LOGON32_LOGON_INTERACTIVE = 2

        tokenHandle = IntPtr.Zero

        Try
            Dim returnValue = LogonUser(
                                userName,
                                domainName,
                                password,
                                LOGON32_LOGON_INTERACTIVE,
                                LOGON32_PROVIDER_DEFAULT,
                                tokenHandle)

            If Not returnValue Then
                Dim ret = Marshal.GetLastWin32Error()
                Debug.WriteLine("LogonUser call failed with error code: " + ret.ToString())
                Throw New System.ComponentModel.Win32Exception(ret)
            End If

            Dim newId As WindowsIdentity = New WindowsIdentity(tokenHandle)
            impersonatedUser = newId.Impersonate()
        Catch ex As Exception
            Debug.WriteLine("Exception occurred. " + ex.Message)
        End Try
    End Sub

    Public Shared ReadOnly Property IsValid As Boolean
        Get
            Return impersonatedUser IsNot Nothing
        End Get
    End Property

    Public Shared Sub Undo()
        impersonatedUser.Undo()
        If (tokenHandle <> IntPtr.Zero) Then CloseHandle(tokenHandle)
    End Sub
End Class

Public Module Native
    Public Const BCM_FIRST = &H1600 'Normal button
    Public Const BCM_SETSHIELD = (BCM_FIRST + &HC) 'Elevated button

    <DllImport("user32")> Public Function SendMessage(hWnd As IntPtr, msg As UInt32, wParam As UInt32, lParam As UInt32) As UInt32
    End Function

    <DllImport("advapi32.dll")> Public Function LogonUser(lpszUserName As String, lpszDomain As String, lpszPassword As String, dwLogonType As Integer, dwLogonProvider As Integer, ByRef phToken As IntPtr) As Boolean
    End Function

    <DllImport("advapi32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> Public Function DuplicateToken(hToken As IntPtr, impersonationLevel As Integer, ByRef hNewToken As IntPtr) As Integer
    End Function

    <DllImport("advapi32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> Public Function RevertToSelf() As Boolean
    End Function

    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> Public Function CloseHandle(handle As IntPtr) As Boolean
    End Function
End Module