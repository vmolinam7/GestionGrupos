using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace capa_datos
{
    public class csConexion
    {
        public SqlConnection con;
        public SqlCommand cmd;
        public string usuario = "sa";
        public string contra = "12345";
        public string bd = "BdGestionGrupos", server = ".";
        public virtual void abrirconexcion()
        {
            con = new SqlConnection();
            con.ConnectionString = "Server=" + server + "; Database=" + bd + "; User id=" + usuario + ";Password=" + contra;
            con.Open();
        }
        public void cerrarconexion()
        {
            con.Close();
        }

        public bool ValidarUsuario(string usuario, string contrasena)
        {
            try
            {
                abrirconexcion();
                string sp = "spValidarUsuario";

                using (SqlCommand cmd = new SqlCommand(sp, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Usuario", usuario);
                    cmd.Parameters.AddWithValue("@Contrasena", contrasena);

                    SqlParameter outputUsuarioID = new SqlParameter("@UsuarioID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputUsuarioID);

                    SqlParameter outputResultado = new SqlParameter("@Resultado", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputResultado);

                    SqlParameter outputNombreVendedor = new SqlParameter("@Nombre", SqlDbType.NVarChar, 50)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputNombreVendedor);

                    SqlParameter outputRol = new SqlParameter("@Rol", SqlDbType.NVarChar, 50)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputRol);

                    SqlParameter outputApellido = new SqlParameter("@Apellido", SqlDbType.NVarChar, 50)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputApellido);

                    cmd.ExecuteNonQuery();

                    int result = (int)outputResultado.Value;
                    if (result == 1)
                    {
                        //CsSesionActiva.NombreVendedor = outputNombreVendedor.Value.ToString();
                        //CsSesionActiva.Rol = outputRol.Value.ToString();
                        return true;
                    }
                    return false; // Si no es válido
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
            finally
            {
                cerrarconexion();
            }
        }

    }
}
