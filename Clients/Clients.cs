﻿using System;
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
    }
}
