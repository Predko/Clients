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
        public ListClients clients = new ListClients();

        

        public Clients()
        {
            InitializeComponent();

            clients.ListClientsChanged += ChangeComboBox;
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
                    comboBoxClients.BeginUpdate();              // приостанавливаем изменение ComboBox, отображающего clients

                    clientsXml.XmlToClientsAndContracts(clients);
                    SetClientContracts(clients[0]);

                    comboBoxClients.SelectedIndex = 0;

                    comboBoxClients.EndUpdate();               // обновляем содержимое ComboBox, отображающего clients
                }
            }
        }

        public void ChangeComboBox(Object sender, ChangedListClientsEventArgs e)
        {
            switch (e.changed)
            {
                case Change.Add:                            // добавляем элемент в список
                    comboBoxClients.Items.Add(e.client);
                    break;

                case Change.Clear:                          // очищаем список
                    comboBoxClients.Items.Clear();
                    break;

                case Change.Set:                            // устанавливаем элемент с данным индексом
                    comboBoxClients.Items[e.index] = e.client;
                    break;
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

            String numb = (cl.contracts.Count != 0) ? cl.contracts[0].Numb.ToString() : "";

            labelContract.Text = String.Format($"Договор № {numb}");

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

            Contract c = (Contract)listBoxContracts.SelectedItem;

            labelContract.Text = String.Format($"Договор № {c.Numb}"); 
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
