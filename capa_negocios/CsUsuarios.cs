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

namespace capa_negocios
{
    public class CsUsuarios
    {
        csConexion csConexion = new csConexion();
        public virtual bool registrarse(string nombres, string apellidos, string telefono, string email, string nombreUsuario, string contrasenia, int rolID)
        {
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
        public void cargarvalorescombo(string searchText, System.Windows.Forms.ComboBox cbx)
        {
            string sp = "spObtenerUsuario";
            csConexion.abrirconexcion();
            csConexion.cmd = new SqlCommand(sp, csConexion.con);
            csConexion.cmd.CommandType = CommandType.StoredProcedure;
            csConexion.cmd.Parameters.Add(new SqlParameter("@SearchText", searchText));

            SqlDataAdapter adapter = new SqlDataAdapter(csConexion.cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            cbx.DisplayMember = "Nombres";
            cbx.ValueMember = "usuarioid";
            cbx.DataSource = dt;

            csConexion.cerrarconexion();
        }
    }
}
