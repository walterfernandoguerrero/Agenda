using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;//agregar

namespace Agenda
{
    public partial class Form1 : Form
    {
        string dir_img_key = Path.Combine(Application.StartupPath, "imagenes", "key.png");
        string dir_img_encendido = Path.Combine(Application.StartupPath, "imagenes", "encendido.png");

        //aqui cadena de conexion
        string cadenaConexion = "Server =localhost\\SQLEXPRESS; Database=db_agenda; Trusted_Connection=True ";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pbxLimpiar.Image = Image.FromFile(dir_img_key);
            pbxPrender.Image = Image.FromFile(dir_img_encendido);
            txt_id.Enabled = false;
            btn_agregar.Enabled = false;
            Leer();
        }
        private void Leer() {
            try
            {
                

                SqlConnection obj_conexion = new SqlConnection(cadenaConexion);

                //abrir
                obj_conexion.Open();
                //consulta sql
                string query = "select * from contactos";

                //objeto de tipo
                SqlCommand obj_cmd = new SqlCommand(query, obj_conexion);

               

                //lector leer la base de datos
                SqlDataReader lector;

                obj_cmd.CommandText = query;

                //ejecutar lectura
                lector= obj_cmd.ExecuteReader();

                //datos en memoria
                DataTable tabla = new DataTable();

                tabla.Load(lector);

                //control del formulario
                dgvContactos.DataSource= tabla;

                obj_conexion.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                
            }

            

        }

        private void btn_agregar_Click(object sender, EventArgs e)
        {
            grabar();

        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (rbtNombre.Checked)
            {
                leerNombres();
            }
            if (rdbGrupo.Checked)
            {
                leerGrupos();
            }
        }
        private void leerNombres() {

            string buscar = txtBuscar.Text;
            try
            {


                SqlConnection obj_conexion = new SqlConnection(cadenaConexion);

                //abrir
                obj_conexion.Open();
                //consulta sql
                string query = "select * from contactos where nombre like '%"+buscar+"%'";

                //objeto de tipo
                SqlCommand obj_cmd = new SqlCommand(query, obj_conexion);



                //lector leer la base de datos
                SqlDataReader lector;

                obj_cmd.CommandText = query;

                //ejecutar lectura
                lector = obj_cmd.ExecuteReader();

                //datos en memoria
                DataTable tabla = new DataTable();

                tabla.Load(lector);

                //control del formulario
                dgvContactos.DataSource = tabla;

                obj_conexion.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }


        }
        private void leerGrupos()
        {
            string buscar = txtBuscar.Text;
            try
            {


                SqlConnection obj_conexion = new SqlConnection(cadenaConexion);

                //abrir
                obj_conexion.Open();
                //consulta sql
                string query = "select * from contactos where grupo like '%" + buscar + "%'";

                //objeto de tipo
                SqlCommand obj_cmd = new SqlCommand(query, obj_conexion);



                //lector leer la base de datos
                SqlDataReader lector;

                obj_cmd.CommandText = query;

                //ejecutar lectura
                lector = obj_cmd.ExecuteReader();

                //datos en memoria
                DataTable tabla = new DataTable();

                tabla.Load(lector);

                //control del formulario
                dgvContactos.DataSource = tabla;

                obj_conexion.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }


        }

        private void dgvContactos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_id.Enabled = false;
            btn_agregar.Enabled = false;
            txt_id.Text = dgvContactos.SelectedCells[0].Value.ToString();
            txtNombre.Text = dgvContactos.SelectedCells[1].Value.ToString();
            txtTelefono.Text= dgvContactos.SelectedCells[2].Value.ToString();
            txtCorreo.Text = dgvContactos.SelectedCells[3].Value.ToString();
            txtGrupo.Text = dgvContactos.SelectedCells[4].Value.ToString();

        }

        private void btn_Elimninar_Click(object sender, EventArgs e)
        {
            borrar();
        }

        private void btnAlta_Click(object sender, EventArgs e)
        {
            grabar();
        }
        private void grabar()
        {
            //si no es true corto el evento
            if(!validarCajas())
            {
                return;
            }

            string nombre = txtNombre.Text;
            string telefono = txtTelefono.Text;
            string correo = txtCorreo.Text;
            string grupo = txtGrupo.Text;
            try
            {
                //abrir conexion
                SqlConnection con = new SqlConnection(cadenaConexion);
                con.Open();

               // string consulta = "insert into contactos (nombre, telefono, correo, grupo) values ('"+nombre+"','"+telefono+"','"+correo+"','"+grupo+"')";

                string consulta = "insert into contactos (nombre, telefono, correo, grupo) values (@nombre,@telefono,@correo,@grupo)";
                // ejecutar
                SqlCommand comando = new SqlCommand(consulta, con);

                comando.Parameters.AddWithValue("@nombre", nombre);
                comando.Parameters.AddWithValue("@telefono", telefono);
                comando.Parameters.AddWithValue("@correo", correo);
                comando.Parameters.AddWithValue("@grupo", grupo);

                comando.CommandText = consulta;

                int filas = comando.ExecuteNonQuery();

                con.Close();

                Leer();

                limpiarCajas();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            
        }
        private void borrar()
        {
            //validacion 
            if (txt_id.Text=="")
            {
                MessageBox.Show("Ingresar un valor en la Caja de texto Id o doble click sobre GRILLA :");
                txt_id.Focus();
                return;
            }

            int id = int.Parse(txt_id.Text);

            // *** 2. Mostrar el MessageBox para pedir confirmación ***
            DialogResult resultado = MessageBox.Show(
                $"¿Estás seguro de que deseas eliminar permanentemente el registro con ID {id}?",
                "Confirmar Eliminación",
                MessageBoxButtons.OKCancel, // Botones Aceptar y Cancelar
                MessageBoxIcon.Exclamation // Un icono de exclamación fuerte
            );
            // *** 3. Verificar la respuesta del usuario ***
            if (resultado == DialogResult.OK)
            {
                //codigo para borrar 

                try
                {
                    //abrir conexion
                    SqlConnection con = new SqlConnection(cadenaConexion);
                    con.Open();

                    string query = "delete contactos where id = @id";

                    SqlCommand comando = new SqlCommand(query, con);

                    comando.Parameters.AddWithValue("@id", id);

                    int filas = comando.ExecuteNonQuery();

                    con.Close();

                    Leer();

                    limpiarCajas();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());

                }

            }
            else
            {
                // El usuario presionó "Cancelar" o cerró el cuadro.
                // No se realiza ninguna acción.
                 MessageBox.Show("Eliminación cancelada."); // Este mensaje suele ser opcional
            }

        }
        private bool validarCajas()
        {
                //se puede validar 
                if (txtNombre.Text == "")
                {
                    MessageBox.Show("el campo Nombre esta vacio ");
                    txtNombre.Focus();
                    return false;
                }
                else
                {
                    if (txtTelefono.Text == "")
                    {
                        MessageBox.Show("el campo Telefono esta vacio ");
                        txtTelefono.Focus();
                        return false;
                    }
                    else
                    {
                        if (txtCorreo.Text == "")
                        {
                            MessageBox.Show("el campo Correo esta vacio ");
                            txtCorreo.Focus();
                            return false;

                        }
                        else
                        {
                            if (txtGrupo.Text == "")
                            {
                                MessageBox.Show("el campo Grupo esta vacio ");
                                txtGrupo.Focus();
                                return false;
                            }
                            else
                            {
                                return true;
                            }

                        }
                    }
                }
        }
        private void limpiarCajas()
        {
            txtNombre.Text = "";
            txtTelefono.Text = "";
            txtCorreo.Text = "";
            txtGrupo.Text = "";
            txt_id.Text = "";
            txtNombre.Focus();
        }

        private void pbxLimpiar_Click(object sender, EventArgs e)
        {
            if (txt_id.Enabled)
            {
                txt_id.Enabled = false;
            }
            else
            {
                txt_id.Enabled = true;
                txt_id.Focus();
            }
        }

        private void pbxPrender_Click(object sender, EventArgs e)
        {
            if (btn_agregar.Enabled)
            {
                btn_agregar.Enabled = false;
                limpiarCajas();
            }
            else
            {
                btn_agregar.Enabled = true;
                limpiarCajas();
                
            }
           
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            editar();
        }
        private void editar()
        {
            //si no es true corto el evento
            if (!validarCajas())
            {
                return;
            }

            //validacion 
            if (txt_id.Text == "")
            {
                MessageBox.Show("Ingresar un valor en la Caja de texto Id o doble click sobre GRILLA :");
                txt_id.Focus();
                return;
            }

            int id = int.Parse(txt_id.Text);
            string nombre = txtNombre.Text;
            string telefono = txtTelefono.Text;
            string correo = txtCorreo.Text;
            string grupo = txtGrupo.Text;

            // *** 2. Mostrar el MessageBox para pedir confirmación *** 
            DialogResult resultado = MessageBox.Show(
                $"¿Estás seguro de que deseas Modificar permanentemente el registro con ID: {id}?",
                "Confirmar Edicion",
                MessageBoxButtons.OKCancel, // Botones Aceptar y Cancelar
                MessageBoxIcon.Exclamation // Un icono de exclamación fuerte
            );
            // *** 3. Verificar la respuesta del usuario ***
            if (resultado == DialogResult.OK)
            {
                try
                {
                    SqlConnection con = new SqlConnection(cadenaConexion);

                    con.Open();

                    string query = "update contactos set nombre = @nombre, telefono = @telefono, correo = @correo, grupo = @grupo where id = @id";

                    SqlCommand comando = new SqlCommand(query, con);

                    comando.Parameters.AddWithValue("@nombre", nombre);
                    comando.Parameters.AddWithValue("@telefono", telefono);
                    comando.Parameters.AddWithValue("@correo", correo);
                    comando.Parameters.AddWithValue("@grupo", grupo);
                    comando.Parameters.AddWithValue("@id", id);

                    comando.CommandText = query;

                    int filas = comando.ExecuteNonQuery();

                    con.Close();

                    Leer();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                
            }
            else {
                MessageBox.Show("Modificacion cancelada."); // Este mensaje suele ser opcional
            }

        }
    }
}
