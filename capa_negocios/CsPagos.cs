using capa_datos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace capa_negocios
{
    public class CsPagos
    {
        csConexion csConexion=new csConexion();
        public DataTable ObtenerMetodosPago()
        {
            DataTable dt = new DataTable();
            try
            {
                csConexion.abrirconexcion();

                using (SqlCommand cmd = new SqlCommand("spObtenerMetodosPago", csConexion.con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar los métodos de pago: " + ex.Message);
            }
            finally
            {
                csConexion.cerrarconexion();
            }

            return dt;
        }

        public void RegistrarPagoPayPal(int pagoID, int metodoID, string correo,double cantidad)
        {
            try
            {
                csConexion.abrirconexcion();

                using (SqlCommand cmd = new SqlCommand("spRegistrarPagoPayPal", csConexion.con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PagoID", pagoID);
                    cmd.Parameters.AddWithValue("@MetodoID", metodoID);
                    cmd.Parameters.AddWithValue("@Correo", correo);
                    cmd.Parameters.AddWithValue("@Cantidad", cantidad);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el pago con PayPal: " + ex.Message);
            }
            finally
            {
                csConexion.cerrarconexion();
            }
        }

        public void RegistrarPagoEfectivo(int pagoID, int metodoID, decimal cantidad)
        {
            try
            {
                csConexion.abrirconexcion();

                using (SqlCommand cmd = new SqlCommand("spRegistrarPagoEfectivo", csConexion.con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PagoID", pagoID);
                    cmd.Parameters.AddWithValue("@MetodoID", metodoID);
                    cmd.Parameters.AddWithValue("@Cantidad", cantidad);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el pago en efectivo: " + ex.Message);
            }
            finally
            {
                csConexion.cerrarconexion();
            }
        }

        public void RegistrarPagoTarjeta(string correo,int pagoID, int metodoID, string numeroTarjeta, DateTime fechaVence, string titular, decimal cantidad)
        {
            try
            {
                csConexion.abrirconexcion();

                using (SqlCommand cmd = new SqlCommand("spRegistrarPagoTarjeta", csConexion.con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PagoID", pagoID);
                    cmd.Parameters.AddWithValue("@MetodoID", metodoID);
                    cmd.Parameters.AddWithValue("@NumeroTarjeta", numeroTarjeta);
                    cmd.Parameters.AddWithValue("@FechaVence", fechaVence);
                    cmd.Parameters.AddWithValue("@Titular", titular);
                    cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@Correo", correo);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el pago con tarjeta de crédito: " + ex.Message);
            }
            finally
            {
                csConexion.cerrarconexion();
            }
        }
        public string ObtenerDetallesPago(int pagoID)
        {
            try
            {
                csConexion.abrirconexcion();

                using (SqlCommand cmd = new SqlCommand("spObtenerDetallesPago", csConexion.con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PagoID", pagoID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string metodoPago = reader["NOMBRE"].ToString();
                            string fechaPago = reader["FECHAPAGO"].ToString();
                            //string correoPayPal = reader["CORREO"]?.ToString() ?? "No aplica";
                            //string numeroTarjeta = reader["NUM_TARJETA"]?.ToString() ?? "No aplica";
                            //string titularTarjeta = reader["TITTULAR"]?.ToString() ?? "No aplica";
                            string cantidad = reader["CANT_TRANSACCION"].ToString();

                            return $"Método de Pago: {metodoPago}\n" +
                                   $"Fecha del Pago: {fechaPago}\n" +
                                   //$"Correo de PayPal: {correoPayPal}\n" +
                                   //$"Número de Tarjeta: {numeroTarjeta}\n" +
                                   //$"Titular de la Tarjeta: {titularTarjeta}\n" +
                                   $"Cantidad Pagada: {cantidad}";
                        }
                        else
                        {
                            return "No se encontraron detalles para este pago.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error al obtener los detalles del pago: {ex.Message}";
            }
            finally
            {
                csConexion.cerrarconexion();
            }
        }
    }
}
