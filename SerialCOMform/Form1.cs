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
                    toolStripStatusLabel1.Text = "Not connected";
                    SendButton.Enabled = false;
                    COMcomboBox.Enabled = true;
                    BaudComboBox.Enabled = true;
                }
                else
                {
                    serialPort1.PortName = COMcomboBox.Text;
                    serialPort1.BaudRate = Int32.Parse( BaudComboBox.Text );
                    serialPort1.Open();
                    ConnectButton.Text = "Disconnect";
                    toolStripStatusLabel1.Text = "Connected (" + COMcomboBox.Text + ")";
                    SendButton.Enabled = true;
                    COMcomboBox.Enabled = false;
                    BaudComboBox.Enabled = false;
                }
            }
            catch {
                toolStripStatusLabel1.Text = "Error";
                SendButton.Enabled = false;
                COMcomboBox.Enabled = true;
                BaudComboBox.Enabled = true;
            }
        }

        private void COMcomboBox_DropDown(object sender, EventArgs e)
        {
            COMcomboBox.Items.Clear();
            COMcomboBox.Items.AddRange(SerialPort.GetPortNames());
        }

        private delegate void printRecievedStrDelegate(byte data);

        private void printRecievedStr(byte data)
        {
            string recv_str = "";

            // 受信値の表示
            if (radioButtonDEC.Checked == true)
            {
                int ch_value = (int)data;
                recv_str += ch_value.ToString() + " ";
            }
            else if (radioButtonHEX.Checked == true)
            {
                    int ch_value = (int)data;
                    recv_str += String.Format("{0:X}", ch_value) + " ";
            }
            else
            {
                recv_str = ((char)data).ToString();
            }

            ReceiveTextBox.Text += recv_str;

            // 自動スクロール
            ReceiveTextBox.SelectionStart = ReceiveTextBox.Text.Length;
            ReceiveTextBox.Focus();
            ReceiveTextBox.ScrollToCaret();
            SendText.Focus();
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            const int BUFF_SIZE = 256;
            byte[] buff = new byte[BUFF_SIZE];
            int read_size = serialPort1.Read( buff, 0, BUFF_SIZE );

            for (int i=0; i<read_size; i++)
            {
                printRecievedStrDelegate print = new printRecievedStrDelegate(printRecievedStr);
                ReceiveTextBox.Invoke(print, buff[i]);
            }
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
