using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarshallHousing
{
    public partial class Form1 : Form
    {
        DBConnection db;
        public Form1()
        {
            InitializeComponent();
            db = new DBConnection();
            Populate();
        }

        public void Populate()
        {
            dataGridView1.DataSource = db.getManagers();
            dataGridView2.DataSource = db.getStudentsWithLease();
            dataGridView3.DataSource = db.getLeaseIncludeSummers();

            comboBox1.DataSource = db.populateStudentsForComboBox();
            comboBox1.DisplayMember = "fullName";
            comboBox1.ValueMember = "muID";
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            dataGridView5.DataSource = db.getUnsatisfactoryProperties();

            comboBox2.DataSource = db.populateHallsForComboBox();
            comboBox2.DisplayMember = "name";
            comboBox2.ValueMember = "hallOfResidenceID";
            this.comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;

            dataGridView7.DataSource = db.getStudentsInWaitingList();
            dataGridView8.DataSource = db.getStudentsForEachCategory();
            dataGridView9.DataSource = db.getStudentsWithoutNextOfKin();

            comboBox3.DataSource = db.populateStudentsForComboBox();
            comboBox3.DisplayMember = "fullName";
            comboBox3.ValueMember = "muID";
            this.comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;

            dataGridView11.DataSource = db.getMinMaxAvgRoomRent();
            dataGridView12.DataSource = db.getPlaceCountForHalls();
            dataGridView13.DataSource = db.getStaffOver60();

            comboBox4.DataSource = db.populateVehicleForComboBox();
            comboBox4.DisplayMember = "parkingLotName";
            comboBox4.ValueMember = "parkingLotNumber";
            this.comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = comboBox1.SelectedValue as DataRowView;
            if (drv != null)
            {
                textBox1.Text = db.getTotalRentForStudent((int)drv.Row[0]);
            }
            else
            {
                string result = db.getTotalRentForStudent((int)comboBox1.SelectedValue);
                if (result != null && result != "")
                {
                    textBox1.Text = result;
                }
                else
                {
                    textBox1.Text = "0";
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dataGridView4.DataSource = db.getStudentsNotPaidByDate(dateTimePicker1.Value);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = comboBox2.SelectedValue as DataRowView;
            if (drv != null)
            {
                dataGridView6.DataSource = db.getStudentsForHall((int)drv.Row[0]);
            }
            else
            {
                dataGridView6.DataSource = db.getStudentsForHall((int)comboBox2.SelectedValue);
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = comboBox3.SelectedValue as DataRowView;
            if (drv != null)
            {
                dataGridView10.DataSource = db.getAdvisorForStudent((int)drv.Row[0]);
            }
            else
            {
                dataGridView10.DataSource = db.getAdvisorForStudent((int)comboBox3.SelectedValue);
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = comboBox4.SelectedValue as DataRowView;
            if (drv != null)
            {
                textBox2.Text = db.getRegisteredVehicle((int)drv.Row[0]);
            }
            else
            {
                textBox2.Text = db.getRegisteredVehicle((int)comboBox4.SelectedValue);
            }
        }
    }
}
