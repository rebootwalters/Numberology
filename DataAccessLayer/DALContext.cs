using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    // implementing IDisposable indicates that this class has heavy duty resources that need to be 
    // released 'on-demand'  
    // this class has a database connection which is such a resource
    // implementing this interface also allows the c# 'using' pattern to be used on instances of this class
    public class DALContext : IDisposable
    {
        public string ConnectionString {
            get { return MyConnection.ConnectionString; }
            set { MyConnection.ConnectionString = value; }
        }
        private System.Data.SqlClient.SqlConnection MyConnection= new System.Data.SqlClient.SqlConnection();
       
        private void EnsureConnected()
        {
                  

            if (MyConnection.State == System.Data.ConnectionState.Closed)
            {
                MyConnection.Open();
            }
            else if (MyConnection.State == System.Data.ConnectionState.Broken)
            {
                // if the connection is broke, then throw an exception 
                throw (new Exception("the SqlConnection is broken"));
            }
        }

        #region access functions for the Numbers Table
        // get all items in the Numbers Table
        public List<NumberDAL> GetAllNumbers()
        {
            // Ensure Connected opens the connection to the database if it is not already opened
            EnsureConnected();
            // create an empty List to return
            List<NumberDAL> rv = new List<NumberDAL>();
            // create a SQL Command object using the existing and open connection.  We know it is open 
            // because of EnsureConnected();
            SqlCommand localcommand = MyConnection.CreateCommand();
            // configure the command object.  we are sending literal SQL to execute, so use Text as the type
            // and Select * from Numbers since we want ALL numbers
            localcommand.CommandType = System.Data.CommandType.Text;
            localcommand.CommandText = "Select * from Numbers";
            // start the query
            SqlDataReader reader = localcommand.ExecuteReader();
            // reader.Read positions us to the first/Next record returned by the SQL Query
            // and it returns false when there are no more fecords, and true when the record is positioned
            int IDPosition=0;
            int NamePosition=0;
            int DSPosition=0;
            int FSPosition=0;
            bool first = true;
            while(reader.Read())
            {
                if (first)
                {
                    first = false;
                    IDPosition = reader.GetOrdinal("ID");
                    NamePosition = reader.GetOrdinal("Name");
                    DSPosition = reader.GetOrdinal("Doublestuff");
                    FSPosition = reader.GetOrdinal("Floatstuff");
                }
                NumberDAL r = new NumberDAL();
                // r.ID = (int) reader[IDPosition];  // this involves boxing, unboxing and garbage generation
                r.ID = reader.GetInt32(IDPosition);  // this does not involve boxing, unboxing and garbage
                //r.Name = (string)reader[NamePosition];// no garbage, but casting takes some small time overhead
                r.Name = reader.GetString(NamePosition);// no garbage and no extra time penalty
                // r.Doublestuff = (double)reader[DSPosition];// this involves boxing, unboxing and garbage generation
                r.Doublestuff = reader.GetDouble(DSPosition); // no boxing, unboxing and garbage
                //r.Floatstuff = (float)reader[FSPosition];// this involves boxing, unboxing and garbage generation
                r.Floatstuff = reader.GetFloat(FSPosition); // no boxing, unboxing and garbage
                rv.Add(r);
            }
            reader.Close();
            return rv;
        }
    
        // get a single item from the Numbers Table By ID
        public NumberDAL GetNumberByID(int id)
        {
            return null;
        }
        // get numbers from the Numbers Table meeting certain conditions
        public List<NumberDAL> GetNumbersStartingWith(char c)
        {
            List<NumberDAL> rv = new List<NumberDAL>();
            return rv;
        }
        // update an existing Number from the Numbers Table using new values or a Number record
        public void UpdateNumber(NumberDAL newValues)
        {
            UpdateNumber(newValues.ID, newValues.Name, newValues.Doublestuff, newValues.Floatstuff);
        }

        public void UpdateNumber(int id, string name, double doublestuff, float floatstuff)
        {

        }
        // Delete an existing Number from the Numbers Table by ID

        public void DeleteNumberByID(int id)
        {
        }
        public void DeleteNumberByID(NumberDAL recordToDelete)
        {
            DeleteNumberByID(recordToDelete.ID);
        }
        // Create a new Number in the Numbers Table
        // note:  the Numbers Table has an Identity Column for the ID, so it is auto calculated by the database
        public int CreateNumber(NumberDAL newValues)
        {
            return CreateNumber(newValues.Name, newValues.Doublestuff, newValues.Floatstuff);
        }

        public int CreateNumber(string name, double doublestuff, float floatstuff)
        {
            return 0;
        }

        #endregion

        // this class goes even further than simple implementation of the IDisposable interface
        // it follows the Disposable Design Pattern which is used to catch some sloppy programming
        // where the user does not use the c# 'using' pattern and 'forgets' to call Dispose()
        // the item will actually be cleaned up during 'garbage collection' when following the
        // Disposable Design Pattern
        // there are two additional functions added in addition to the required Dispose()
        // ~DALContext() to catch at the garbage collection time
        // and Dispose(bool) to do the actual work

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                MyConnection.Dispose();
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~DALContext()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
             GC.SuppressFinalize(this);
        }
        #endregion


       

    }
}
