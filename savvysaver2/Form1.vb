Imports System.Data.SqlClient
Imports Guna.UI2.WinForms
Imports Microsoft.Data.SqlClient

Public Class Form1
    ' SQL Server connection
    Dim con As New SqlConnection("Data Source=CHIDAMBARAM-LAP\SQLEXPRESS01;Initial Catalog=Comparison;Integrated Security=True;TrustServerCertificate=True")

    ' LOGIN BUTTON (Guna2GradientButton1)
    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        Dim userType As String = Guna2ComboBox1.SelectedItem
        Dim username = Guna2TextBox1.Text.Trim
        Dim password = Guna2TextBox2.Text.Trim

        If userType = "" Or username = "" Or password = "" Then
            MessageBox.Show("Please fill all fields!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Password Length Validation
        If password.Length < 8 Then
            MessageBox.Show("Password must be at least 8 characters long.", "Weak Password", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' ==== Admin Login ====
        If userType = "Admin" Then
            If username = "admin" And password = "admin123" Then
                MessageBox.Show("Admin Login Successful!", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Hide()
                dashboard.Show()
                Me.Hide()
            Else
                MessageBox.Show("Invalid Admin credentials.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            ' ==== Customer Login ====
        ElseIf userType = "Customer" Then
            Try
                Dim cmd As New SqlCommand("SELECT COUNT(*) FROM Customer1 WHERE Username = @Username AND Password = @Password", con)
                cmd.Parameters.AddWithValue("@Username", username)
                cmd.Parameters.AddWithValue("@Password", password)

                con.Open()
                Dim count = Convert.ToInt32(cmd.ExecuteScalar)
                con.Close()

                If count > 0 Then
                    MessageBox.Show("Customer Login Successful!", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Hide()
                    COMPARE.Show()
                    Me.Hide()
                Else
                    MessageBox.Show("Invalid Customer credentials.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

            Catch ex As Exception
                MessageBox.Show("Database Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If con.State = ConnectionState.Open Then con.Close()
            End Try
        Else
            MessageBox.Show("Please select a valid user type!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    ' REGISTER BUTTON (Guna2GradientButton2)
    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        Dim reg As New Registration()
        reg.Show()
        Me.Hide()
    End Sub

    ' FORM LOAD
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblCapsWarning.Visible = False ' Hide on load
    End Sub

    ' EXIT LINK
    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Application.Exit()
    End Sub

    ' CAPS LOCK DETECTION
    Private Sub Guna2TextBox2_KeyDown(sender As Object, e As KeyEventArgs) Handles Guna2TextBox2.KeyDown
        lblCapsWarning.Visible = Control.IsKeyLocked(Keys.CapsLock)
    End Sub

    Private Sub Guna2TextBox2_KeyUp(sender As Object, e As KeyEventArgs) Handles Guna2TextBox2.KeyUp
        lblCapsWarning.Visible = Control.IsKeyLocked(Keys.CapsLock)
    End Sub

End Class
