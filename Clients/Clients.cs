using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Globalization;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections;
using System.IO;


using static System.Console;

namespace Clients
{
    public partial class Clients : Form
    {
        public BindingList<Client> clients = new BindingList<Client>();
        //public BindingList<Contract> contracts = new BindingList<Contract>();
        //protected BindingList<Contract> clientContract = new BindingList<Contract>();


        public Clients()
        {
            InitializeComponent();

            comboBoxClients.DataSource = clients;

            
        }

        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ToolStripMenuItemLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Открываем и загружаем файл данных со списком клиентов и сонтрактов(xml)
                ClientsXml clientsXml = new ClientsXml(openFileDialog.FileName);

                if (clientsXml.Load_Ok)
                {
                    clientsXml.XmlToClientsAndContracts(ref clients);
                    comboBoxClients.SelectedIndex = 0;
                    SetClientContracts(clients[0]);
                }
            }
        }

        private void SetClientContracts(Client cl)
        {
            if (cl == null)
                return;

            decimal Summ = 0;

            DateTime d = DateTime.Parse(@"07/01/2016", CultureInfo.CreateSpecificCulture("en-US"));
            foreach (Contract c in cl.contracts)
                Summ += (c.Dt >= d) ? c.Summ
                                    : c.Summ / 10000; // denomination after 07/01/2016

            //
            labelListContractsTotals.Text = String.Format($"Договоров: {cl.contracts.Count,-5}  на сумму: {Summ:C}");

            labelContract.Text = String.Format($"Договор № {cl.contracts[0].Numb}");

            listBoxContracts.DataSource = cl.contracts;  // binding the contracts to listBoxContracts
        }


//
//      Выбор элемента comboBoxClients
//
        private void comboBoxClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            Client cl = (Client)((ComboBox)sender).SelectedItem;

            SetClientContracts(cl);
        }

//
//      Выбор элемента listBoxContracts
//
        private void listBoxContracts_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = listBoxContracts.SelectedIndex;
            
            if (i == -1)
                return;

            Contract c = (Contract)listBoxContracts.SelectedItem;   //((BindingList<Contract>)listBoxContracts.DataSource)[i];

            labelContract.Text = String.Format($"Договор № {c.Numb}"); 
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
