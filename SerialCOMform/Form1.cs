using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace SerialCOMform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            COMcomboBox.Items.AddRange( SerialPort.GetPortNames() );
            if( 0 < COMcomboBox.Items.Count)
            {
                COMcomboBox.Text = COMcomboBox.Items[0].ToString();
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            sendSerial();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                if( serialPort1.IsOpen)
                {
                    serialPort1.Close();
                    ConnectButton.Text = "Connect";
                    SendButton.Enabled = false;
                    toolStripStatusLabel1.Text = "Not connected";
                }
                else
                {
                    serialPort1.PortName = COMcomboBox.Text;
                    serialPort1.BaudRate = Int32.Parse( BaudComboBox.Text );
                    serialPort1.Open();
                    ConnectButton.Text = "Disconnect";
                    SendButton.Enabled = true;
                    toolStripStatusLabel1.Text = "Connected (" + COMcomboBox.Text + ")";
                }
            }
            catch {
                toolStripStatusLabel1.Text = "Error";
            }
        }

        private void COMcomboBox_DropDown(object sender, EventArgs e)
        {
            COMcomboBox.Items.Clear();
            COMcomboBox.Items.AddRange(SerialPort.GetPortNames());
        }

        private delegate void printRecievedStrDelegate(string data);

        private void printRecievedStr(string data)
        {
            ReceiveTextBox.Text += data;

            // 自動スクロール
            ReceiveTextBox.SelectionStart = ReceiveTextBox.Text.Length;
            ReceiveTextBox.Focus();
            ReceiveTextBox.ScrollToCaret();
            SendText.Focus();
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string received_str = serialPort1.ReadExisting();
            printRecievedStrDelegate print = new printRecievedStrDelegate(printRecievedStr);
            ReceiveTextBox.Invoke(print, received_str);
        }

        private void SendText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                sendSerial();
            }
        }

        private void sendSerial()
        {
            if (serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Write(SendText.Text);
                    SendText.Text = "";
                }
                catch
                {
                    toolStripStatusLabel1.Text = "Send Error";
                }
            }
        }

        private void SendText_KeyPress(object sender, KeyPressEventArgs e)
        {
            // BEEP抑制
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
        }
    }
}
