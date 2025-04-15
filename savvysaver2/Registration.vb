Imports System.Data.SqlClient
Imports Microsoft.Data.SqlClient

Public Class Registration

    ' SQL Connection String (with TrustServerCertificate to avoid SSL error)
    Dim con As New SqlConnection("Data Source=CHIDAMBARAM-LAP\SQLEXPRESS01;Initial Catalog=Comparison;Integrated Security=True;TrustServerCertificate=True")
    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        Dim fullname = Guna2TextBox3.Text.Trim()
        Dim username = Guna2TextBox1.Text.Trim()
        Dim email = Guna2TextBox4.Text.Trim()
        Dim phone = Guna2TextBox2.Text.Trim()
        Dim password = Guna2TextBox5.Text.Trim()
        lblEmailWarning.Visible = False
        ' === BASIC EMPTY CHECK ===
        If fullname = "" Or username = "" Or email = "" Or phone = "" Or password = "" Then
            MessageBox.Show("Please fill in all fields.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' === EMAIL VALIDATION ===
        If Not email.EndsWith("@gmail.com") Then
            MessageBox.Show("Email must end with @gmail.com", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
            lblEmailWarning.Visible = True
        End If

        ' === PHONE NUMBER VALIDATION ===
        If Not IsNumeric(phone) OrElse phone.Length <> 10 Then
            MessageBox.Show("Phone number must be exactly 10 digits.", "Invalid Phone", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' === PASSWORD LENGTH VALIDATION ===
        If password.Length < 8 Then
            MessageBox.Show("Password must be at least 8 characters long.", "Weak Password", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' === DATABASE INSERT ===
        Try
            Dim query = "INSERT INTO Customer1 (FullName, Username, Email, Phone, Password) VALUES (@FullName, @Username, @Email, @Phone, @Password)"
            Dim cmd As New SqlCommand(query, con)

            cmd.Parameters.AddWithValue("@FullName", fullname)
            cmd.Parameters.AddWithValue("@Username", username)
            cmd.Parameters.AddWithValue("@Email", email)
            cmd.Parameters.AddWithValue("@Phone", phone)
            cmd.Parameters.AddWithValue("@Password", password)

            con.Open()
            cmd.ExecuteNonQuery()
            con.Close()

            MessageBox.Show("Registration Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Clear Fields
            Guna2TextBox3.Clear()
            Guna2TextBox1.Clear()
            Guna2TextBox4.Clear()
            Guna2TextBox2.Clear()
            Guna2TextBox5.Clear()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    ' BACK TO LOGIN
    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        Form1.Show()
        Me.Hide()
    End Sub

    ' === CAPS LOCK WARNING FOR PASSWORD ===
    Private Sub Guna2TextBox5_KeyDown(sender As Object, e As KeyEventArgs) Handles Guna2TextBox5.KeyDown
        lblCapsWarningReg.Visible = Control.IsKeyLocked(Keys.CapsLock)
    End Sub

    Private Sub Guna2TextBox5_KeyUp(sender As Object, e As KeyEventArgs) Handles Guna2TextBox5.KeyUp
        lblCapsWarningReg.Visible = Control.IsKeyLocked(Keys.CapsLock)
    End Sub

    Private Sub Registration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblCapsWarningReg.Visible = False
    End Sub
    Private Sub Guna2TextBox4_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox4.TextChanged
        If Not Guna2TextBox4.Text.Trim().EndsWith("@gmail.com") Then
            lblEmailWarning.Visible = True
        Else
            lblEmailWarning.Visible = False
        End If
    End Sub
End Class
