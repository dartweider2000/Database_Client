using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DataBase_Client
{
    class Client
    {
        private Label label;

        private void stateChangeHanlder(object Sender, System.Data.StateChangeEventArgs e)
        {
            this.label.Text += $"{e.CurrentState}, {e.OriginalState} --- ";
        }
        public Client(Label label)
        {
            this.label = label;

            string str = @"Data Source=.\SQLEXPRESS01; Initial Catalog=Enterprise; Integrated Security=True";

            SqlConnection connection = new SqlConnection(str);
            connection.StateChange += this.stateChangeHanlder;

            try
            {
                connection.Open();

                //Console.WriteLine(connection.State);
                ///label.Text = Convert.ToString(connection.State);

            }
            catch(Exception ex)
            {
                //Console.WriteLine(ex.Message);
                //label.Text = ex.Message;
            }
            finally
            {
                connection.Close();
                //label.Text += Convert.ToString(connection.State);
            }
        }
    }
}
