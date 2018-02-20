using System;
using System.Windows.Forms;

namespace Clients
{
    enum TabPageIndex
    {
        Clients,
        ContractEdit
    }

    public partial class Clients : Form
    {
        public static readonly ListClients clients = new ListClients();    // Список всех клиентов

        public Clients()
        {
            InitializeComponent();

            InitToolStripMenu();

            InitComboBoxClients();

            InitListBoxContracts();

            InitDataGridViewContract();

            InitTabPageContractEdit();

            InitTabControlClients();
        }

        private void ButtonEditClient_Click(object sender, EventArgs e)
        {
            if (CurrentClient == null)
            {
                return;
            }

            var dialogClient = new DialogBoxClients(CurrentClient);

            dialogClient.Show();
        }
    }
}
