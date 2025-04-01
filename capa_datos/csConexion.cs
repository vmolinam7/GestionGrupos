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
        public string bd = "dbGestionGrupos", server = ".";
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

                    SqlParameter outputUser = new SqlParameter("@User", SqlDbType.NVarChar,50)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputUser);

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
                        CsSesionActiva.CreadorID = (int)outputUsuarioID.Value;
                        CsSesionActiva.User= outputUser.Value.ToString();
                        return true;
                    }
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
                cerrarconexion();
            }
        }

    }

    public class CsSesionActiva
    {
        public static string User { get; set; }
        public static string Rol { get; set; }

        public static int CreadorID { get; set; }
    }
}
