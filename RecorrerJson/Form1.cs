using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace RecorrerJson
{
    public partial class Form1 : Form
    {
        
        // Instanciamos la lista de clientes y el índice actual
        private List<Cliente> listaClientes;
        private int indiceActual;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Leemos el archivo JSON
            string json = File.ReadAllText("clientes.json");
            // Deserializamos el JSON a una lista de clientes
            listaClientes = JsonConvert.DeserializeObject<List<Cliente>>(json);

            //Establecemos el índice actual y mostramos el primer cliente
            indiceActual = 0;
            MostrarCliente(indiceActual);
        }

        private void MostrarCliente(int indice)
        {
            //Si el indice es inválido, salimos del método
            if (indice < 0 || indice >= listaClientes.Count) return;

            // Guardamos el cliente actual
            Cliente cliente = listaClientes[indice];

            //Instanciamos las propiedades del cliente en los TextBox y DataGridView
            txtBoxDni.Text = cliente.Dni;
            txtBoxNombre.Text = cliente.Nombre;
            txtBoxTelefono.Text = cliente.Telefono;
            txtBoxEmail.Text = cliente.Email;

            dataGridViewPropiedades.DataSource = cliente.Propiedades;
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            // Si la lista es nula o ya estamos en el último cliente, salimos del método
            if (listaClientes == null || listaClientes.Count - 1 == indiceActual) return;

            indiceActual++;
            MostrarCliente(indiceActual);
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            // Si la lista es nula o ya estamos en el primer cliente, salimos del método
            if (listaClientes == null || indiceActual == 0) return;

            indiceActual--;
            MostrarCliente(indiceActual);
        }

        private void btnAplicarCambios_Click(object sender, EventArgs e)
        {
            // Si hay algún campo vacío, mostramos un mensaje de error y salimos del método
            if (comprobarTextosVacíos())
            {
                MessageBox.Show("No se pueden dejar campos vacíos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Actualizamos el cliente actual con los datos de los TextBox y DataGridView
            Cliente clienteActual = listaClientes[indiceActual];
            clienteActual.Dni = txtBoxDni.Text;
            clienteActual.Nombre = txtBoxNombre.Text;   
            clienteActual.Telefono = txtBoxTelefono.Text;   
            clienteActual.Email = txtBoxEmail.Text;

            foreach (DataGridViewRow fila in dataGridViewPropiedades.Rows)
            {
                if (fila.IsNewRow) continue; // saltamos la fila nueva de DataGridView

                foreach (DataGridViewCell celda in fila.Cells)
                {

                    // Validamos que ninguna celda esté vacía
                    if (celda.Value == null || string.IsNullOrWhiteSpace(celda.Value.ToString()))
                    {
                        MessageBox.Show("No se pueden dejar campos vacíos en las propiedades.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // salimos del método si hay error
                    }

                    // Validamos que la columna "Cuota" contenga un número válido
                    if (celda.ColumnIndex == dataGridViewPropiedades.Columns["Cuota"].Index)
                    {
                        // Intentamos convertir a decimal
                        if (!decimal.TryParse(celda.Value.ToString(), out _))
                        {
                            MessageBox.Show("La cuota debe ser un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; // salimos del método si hay error
                        }
                    }
                }
            }

            clienteActual.Propiedades = (List<Propiedad>)dataGridViewPropiedades.DataSource;

            // Serializamos la lista de clientes a JSON y lo guardamos en el archivo
            string jsonActualizado = JsonConvert.SerializeObject(listaClientes, Formatting.Indented);

            // Guardamos el JSON actualizado en el archivo
            File.WriteAllText("clientes.json", jsonActualizado);

            MessageBox.Show("Cambios aplicados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Método para comprobar si algún TextBox está vacío
        private Boolean comprobarTextosVacíos()
        {
            return string.IsNullOrWhiteSpace(txtBoxDni.Text) ||
                   string.IsNullOrWhiteSpace(txtBoxNombre.Text) ||
                   string.IsNullOrWhiteSpace(txtBoxTelefono.Text) ||
                   string.IsNullOrWhiteSpace(txtBoxEmail.Text);
        }
    }
}