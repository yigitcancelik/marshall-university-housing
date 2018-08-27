using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace MarshallHousing
{
    public class DBConnection
    {
        public MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public DBConnection()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "marshallhousing";
            uid = "root";
            password = "1234";
            string connectionString;
            connectionString = "SERVER=" + server + ";PORT=3306;" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PWD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        public DataTable getManagers()
        {
            string query = "select staff.staffName, hallofresidence.telephone from staff, hallofresidence where staff.position = " 
                        + (int)Position.Manager + " and staff.staffNumber = hallofresidence.hallManagerID";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable getStudentsWithLease()
        {
            string query = "select student.muID, student.firstName, student.lastName, lease.* from student, lease where student.muID = lease.muID";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable getLeaseIncludeSummers()
        {
            string query = "select * from lease where month(leaveDate) = 08";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable populateStudentsForComboBox()
        {
            string query = "select muID, firstName, LastName from student";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dt.Columns.Add("fullName", typeof(string), "firstName + ' ' + lastName");
            return dt;
        }

        public string getTotalRentForStudent(int muID)
        {
            string query = "SELECT * FROM invoices, lease, room where"
                + " invoices.leaseNumber = lease.leaseNumber and lease.placeNumber = room.placeNumber and invoices.dateOfPayment < now()"
                + " and lease.muID = " + muID;
            MySqlCommand cmd = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader rd = cmd.ExecuteReader();
            int total = 0;
            while (rd.Read())
            {
                Query4Helper q = new Query4Helper();
                q.semester = (Semester)rd.GetInt32("semester");
                q.rent = rd.GetInt32("monthlyRent");
                if (q.semester == Semester.Fall || q.semester == Semester.Spring)
                {
                    total += q.rent * 4;
                }
                else
                {
                    total += q.rent * 3;
                }
            }
            connection.Close();
            return total.ToString();
        }

        public DataTable getStudentsNotPaidByDate(DateTime given)
        {
            string query = "select student.* from student, invoices, lease where "
                + "student.muID = lease.muID and lease.leaseNumber = invoices.leaseNumber "
                + "and (invoices.dateOfPayment > '" + given.ToString("yyyy-MM-dd HH:mm:ss") + "' or invoices.dateOfPayment is null)";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            DataTable cloned = dt.Clone();
            cloned.Columns[9].DataType = typeof(Gender);
            cloned.Columns[10].DataType = typeof(StudentCategory);
            cloned.Columns[14].DataType = typeof(CurrentStatus);
            foreach (DataRow r in dt.Rows)
            {
                cloned.ImportRow(r);
            }
            return cloned;
        }

        public DataTable getUnsatisfactoryProperties()
        {
            string query = "select * from inspections where roomCondition = 1";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            DataTable cloned = dt.Clone();
            cloned.Columns[3].DataType = typeof(RoomCondition);
            foreach (DataRow r in dt.Rows)
            {
                cloned.ImportRow(r);
            }
            return cloned;
        }

        public DataTable populateHallsForComboBox()
        {
            string query = "select hallOfResidenceID ,name from hallofresidence";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable getStudentsForHall(int hallID)
        {
            string query = "select student.muID, student.firstName, student.lastName, room.roomNumber, room.placeNumber"
                           + " from room, lease, hallofresidence, student where lease.muID = student.muID and lease.placeNumber = room.placeNumber"
                           + " and hallofresidence.hallOfResidenceID = room.hallResidenceID and hallOfResidenceID = " + hallID;
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable getStudentsInWaitingList()
        {
            string query = "select * from student where currentStatus = 0";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            DataTable cloned = dt.Clone();
            cloned.Columns[9].DataType = typeof(Gender);
            cloned.Columns[10].DataType = typeof(StudentCategory);
            cloned.Columns[14].DataType = typeof(CurrentStatus);
            foreach (DataRow r in dt.Rows)
            {
                cloned.ImportRow(r);
            }
            return cloned;
        }

        public DataTable getStudentsForEachCategory()
        {
            string query = "select count(muID) as NumberOfStudents, categoryOfStudent from student group by categoryOfStudent";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            DataTable cloned = dt.Clone();
            cloned.Columns[1].DataType = typeof(StudentCategory);
            foreach (DataRow r in dt.Rows)
            {
                cloned.ImportRow(r);
            }
            return cloned;
        }

        public DataTable getStudentsWithoutNextOfKin()
        {
            string query = "select student.muID, student.firstName, student.lastName from student left join nextofkin "
                + " on student.muID = nextOfKin.muID where nextOfKinID is null";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable getAdvisorForStudent(int muID)
        {
            string query = "select advisor.name, advisor.telephoneNumber from advisor, student"
                + " where advisor.advisorID = student.advisorID and student.muID = " + muID;
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable getMinMaxAvgRoomRent()
        {
            string query = "select hallofresidence.name ,min(monthlyRent) as MinumumRent, max(monthlyRent) as MaximumRent, avg(monthlyRent) as AvarageRent"
                 + " from room, hallofresidence where room.hallResidenceID = hallofresidence.hallOfResidenceID group by room.hallResidenceID";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable getPlaceCountForHalls()
        {
            string query = "select hallofresidence.name ,count(placeNumber) as NumberOfPlaces"
                 + " from room, hallofresidence where room.hallResidenceID = hallofresidence.hallOfResidenceID group by room.hallResidenceID";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable getStaffOver60()
        {
            string query = "select staffNumber, staffName, datediff(current_date(), staff.dateOfBirth) / 365.25 as age, location from staff "
                + "where location = 'hall' and datediff(current_date(), staff.dateOfBirth) / 365.25 > 60";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable populateVehicleForComboBox()
        {
            string query = "select parkingLotNumber, parkingLotName from parkinglot";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public string getRegisteredVehicle(int parkingLotNumber)
        {
            string query = "select count(vehicle.vinNumber) as TotalNumberOfVehicles from parkinglot, vehicle"
                + " where parkinglot.parkingLotNumber = vehicle.parkingLotNumber and vehicle.parkingLotNumber = " + parkingLotNumber
                + " group by vehicle.parkingLotNumber";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader rd = cmd.ExecuteReader();
            int total = 0;
            while (rd.Read())
            {
                total = rd.GetInt32("TotalNumberOfVehicles");
            }
            connection.Close();
            return total.ToString();
        }
    }
}
