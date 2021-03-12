using System;
using System.Windows.Forms;
using Microsoft.Win32;


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

        public static string DefaultPathFile = @"H:\Документы"; // Путь к файлам договоров по умолчанию



        public Clients()
        {
            InitializeComponent();

            InitToolStripMenu();

            InitComboBoxClients();

            InitListBoxContracts();

            InitDataGridViewContract();

            InitTabPageContractEdit();

            InitTabControlClients();

 
            ResizeRedraw = true;    // Перерисовка при изменении размера
        }

    }
}
