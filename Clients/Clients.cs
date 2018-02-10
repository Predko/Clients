﻿using System;
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
    enum TabPageIndex
    {
        Clients,
        ContractEdit
    }

    public partial class Clients : Form
    {
        public readonly ListClients clients = new ListClients();    // Список всех клиентов

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
