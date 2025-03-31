using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using capa_datos;
using System.Net.Mail;
using System.Net;

namespace capa_negocios
{
    public class CsUsuarios
    {
        csConexion csConexion;
        public virtual bool registrarse(string nombres, string apellidos, string telefono, string email, string nombreUsuario, string contrasenia, int rolID)
        {
            csConexion = new csConexion();
            string sp = "spRegistrarse";

            try
            {
                csConexion.abrirconexcion();

                SqlCommand cmd = new SqlCommand(sp, csConexion.con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Nombres", nombres);
                cmd.Parameters.AddWithValue("@Apellidos", apellidos);
                cmd.Parameters.AddWithValue("@Telefono", telefono);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                cmd.Parameters.AddWithValue("@Contrasenia", contrasenia);
                cmd.Parameters.AddWithValue("@RolID", rolID);

                SqlParameter outputUsuarioID = new SqlParameter("@UsuarioID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputUsuarioID);

                cmd.ExecuteNonQuery();

                if (outputUsuarioID.Value != DBNull.Value)
                {
                    int usuarioID = (int)outputUsuarioID.Value;
                    return true;
                }
                return false;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error en el registro: " + ex.Message);
                return false;
            }
            finally
            {
                csConexion.cerrarconexion();
            }
        }

        public bool EsEmailValido(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public bool EsTelefonoValido(string telefono)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(telefono, @"^\d{7,10}$");
        }

        public bool EsContraseniaSegura(string contrasenia)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(contrasenia, @"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$");
        }

        public bool recuperarContrasenia(string email)
        {
            csConexion = new csConexion();
            string sp = "spRecuperarContrasenia";

            try
            {
                csConexion.abrirconexcion();
                SqlCommand cmd = new SqlCommand(sp, csConexion.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);

                string contrasenia = cmd.ExecuteScalar()?.ToString();

                if (!string.IsNullOrEmpty(contrasenia))
                {
                    enviarCorreo(email, contrasenia);
                    return true;
                }
                else
                {
                    MessageBox.Show("El correo no está registrado.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
            finally
            {
                csConexion.cerrarconexion();
            }

        }

        private void enviarCorreo(string destinatario, string contrasenia)
        {
            try
            {
                MailMessage mail = new MailMessage();
                //colocar correo aquí
                mail.From = new MailAddress("tucorreo@gmail.com");
                mail.To.Add(destinatario);
                mail.Subject = "Recuperación de contraseña";

                string htmlBody = @"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            line-height: 1.6;
                            color: #333333;
                        }
                        .container {
                            max-width: 600px;
                            margin: 0 auto;
                            padding: 20px;
                            background-color: #f9f9f9;
                            border-radius: 5px;
                        }
                        .header {
                            background-color: #4285f4;
                            color: white;
                            padding: 15px;
                            text-align: center;
                            border-radius: 5px 5px 0 0;
                        }
                        .content {
                            padding: 20px;
                            background-color: white;
                            border-radius: 0 0 5px 5px;
                        }
                        .password {
                            font-size: 18px;
                            font-weight: bold;
                            color: #4285f4;
                            padding: 10px;
                            margin: 15px 0;
                            background-color: #f2f2f2;
                            border-radius: 5px;
                            text-align: center;
                        }
                        .footer {
                            text-align: center;
                            margin-top: 20px;
                            font-size: 12px;
                            color: #666666;
                        }
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h2>Recuperación de Contraseña</h2>
                        </div>
                        <div class='content'>
                            <p>Estimado(a) usuario,</p>
                            <p>Hemos recibido una solicitud para recuperar su contraseña. A continuación, le proporcionamos la información solicitada:</p>
                            <div class='password'>
                                " + contrasenia.Trim() + @"
                            </div>
                            <p>Por motivos de seguridad, le recomendamos cambiar esta contraseña una vez que inicie sesión en su cuenta.</p>
                            <p>Si usted no solicitó esta información, por favor ignore este correo.</p>
                            <p>Saludos cordiales,<br>El Equipo de Soporte</p>
                        </div>
                        <div class='footer'>
                            <p>Este es un correo automático, por favor no responda a este mensaje.</p>
                        </div>
                    </div>
                </body>
                </html>";

                mail.Body = htmlBody;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                //colocar su correo y respectivo codifgo de aplicaciona aqui
                smtp.Credentials = new NetworkCredential("tucorreo@gmail.com", "tu codgo d aplicacion de google");
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
        }
    }
}
