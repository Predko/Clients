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
        public BindingList<Contract> contracts = new BindingList<Contract>();
        protected BindingList<Contract> clientContract = new BindingList<Contract>();


        public Clients()
        {
            InitializeComponent();

            comboBoxClients.DataSource = clients;

            listBoxContracts.DataSource = clientContract;
            // Открываем и загружаем файл данных со списком клиентов и сонтрактов(xml)
            //ClientsXml clientsXml = new ClientsXml(@"d:\Clients.xml");

            //if (clientsXml.Load_Ok)
            //{
            //    comboBoxClients.BeginUpdate();
            //    clientsXml.XmlToClientsAndContracts(ref clients, ref contracts);
            //    comboBoxClients.EndUpdate();
            //}
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
                    clientsXml.XmlToClientsAndContracts(ref clients, ref contracts);
                    comboBoxClients.SelectedIndex = 0;
                    SetClientContracts(clients[0]);
                }
            }
        }

        private void SetClientContracts(Client cl)
        {
            if (cl == null)
                return;

            clientContract.Clear();

            int numbContracts = 0;
            double Summ = 0;

            foreach (Contract c in contracts.Where(c => c.id == cl.id).Select(c => c))
            {
                clientContract.Add(c);
                numbContracts++;
                DateTime d = DateTime.Parse(@"07/01/2016", CultureInfo.CreateSpecificCulture("en-US"));
                Summ += (c.dt >= d) ? c.sum
                                    : c.sum / 10000; // denomination after 07/01/2016
            }

            labelListContractsTotals.Text = String.Format($"Договоров: {numbContracts,-5}  на сумму: {Summ:C}");

            //

            labelContract.Text = String.Format($"Договор № {clientContract[0].numb}");
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

            Contract c = clientContract[i];

            labelContract.Text = String.Format($"Договор № {c.numb}"); 
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
