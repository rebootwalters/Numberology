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
        public DALContext()
        {
            Numbers = new NumberHierarchy(this);
            RelatedNumbers = new RelatedNumberHierarchy(this);
            Riddles = new RiddleHierarchy(this);
            Users = new UserHierarchy(this);
            Numbers2Riddles = new Number2RiddleHierarchy(this);

        }
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

      
        #region IDisposable Support 
        // this class goes even further than simple implementation of the IDisposable interface
        // it follows the Disposable Design Pattern which is used to catch some sloppy programming
        // where the user does not use the c# 'using' pattern and 'forgets' to call Dispose()
        // the item will actually be cleaned up during 'garbage collection' when following the
        // Disposable Design Pattern
        // there are two additional functions added in addition to the required Dispose()
        // ~DALContext() to catch at the garbage collection time
        // and Dispose(bool) to do the actual work


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

               throw new Exception("The DALContext was not Disposed Properly.  You should either use it within a C# Using construct, or call Dispose on the instance before it goes out of scope.");
        
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

        #region Mapper Support

            #region Number Mapping
        static int Number_IDPosition = 0;
        static int Number_NamePosition = 0;
        static int Number_DSPosition = 0;
        static int Number_FSPosition = 0;
        static bool Number_PositionsInitialized = false;

        // place a verification to do a rough check on the reader to insure it is somewhat typed to 
        // a NumberDAL.  The check will insure the field count is 4.  
        static int NumberFieldCount = 0;

        static public NumberDAL NumberDALFromReader(SqlDataReader reader)
        {
            // the following line is not thread safe.  Consider whether thread safety is an issue and use
            // the  System.Threading.Interlocked.CompareExchange functions if needed
           
            if (!Number_PositionsInitialized)
            {
                NumberFieldCount = reader.FieldCount; 
                Number_PositionsInitialized  = true;
                Number_IDPosition = reader.GetOrdinal("ID");
                Number_NamePosition = reader.GetOrdinal("Name");
                Number_DSPosition = reader.GetOrdinal("Doublestuff");
                Number_FSPosition = reader.GetOrdinal("Floatstuff");
               

            }
            else
            {
                // check to make sure the schema is still valid
                if (!NumberFieldCount.Equals(reader.FieldCount))
                {
                    

                    throw new Exception("NumberDALFromReader was called, but the Reader has an invalid Field Count... It is likely that the reader is not actually connected to a NumberDAL query stream");
                }
            }
          
            
            NumberDAL rv = new NumberDAL();
            // rv.ID = (int) reader[IDPosition];  // this involves boxing, unboxing and garbage generation
            rv.ID = reader.GetInt32(Number_IDPosition);  // this does not involve boxing, unboxing and garbage
            //rv.Name = (string)reader[NamePosition];// no garbage, but casting takes some small time overhead
            rv.Name = reader.GetString(Number_NamePosition);// no garbage and no extra time penalty
            // rv.Doublestuff = (double)reader[DSPosition];// this involves boxing, unboxing and garbage generation
            rv.Doublestuff = reader.GetDouble(Number_DSPosition); // no boxing, unboxing and garbage
            //rv.Floatstuff = (float)reader[FSPosition];// this involves boxing, unboxing and garbage generation
            rv.Floatstuff = reader.GetFloat(Number_FSPosition); // no boxing, unboxing and garbage

            return rv;

        }
        #endregion Number mapping

            #region Related Number Mapping

        static int RelatedNumber_IDPosition = 0;
        static int RelatedNumber_RelatedNamePosition = 0;
        static int RelatedNumber_RelatedLanguagePosition = 0;
        static int RelatedNumber_ParentNumberIDPosition = 0;
        static bool RelatedNumber_PositionsInitialized = false;

    
        static int RelatedNumberFieldCount = 0;

        public static RelatedNumberDAL RelatedNumberDALFromReader(SqlDataReader reader)
        {
            // the following line is not thread safe.  Consider whether thread safety is an issue and use
            // the  System.Threading.Interlocked.CompareExchange functions if needed

            if (!RelatedNumber_PositionsInitialized)
            {
                RelatedNumber_PositionsInitialized = true ;
                RelatedNumberFieldCount = reader.FieldCount;
                RelatedNumber_IDPosition = reader.GetOrdinal("ID");
                RelatedNumber_RelatedNamePosition = reader.GetOrdinal("RelatedName");
                RelatedNumber_RelatedLanguagePosition = reader.GetOrdinal("RelatedLanguage");
                RelatedNumber_ParentNumberIDPosition = reader.GetOrdinal("ParentNumber_ID");
            }
            else
            {
                // check to make sure the schema is still valid
                if (!RelatedNumberFieldCount.Equals(reader.FieldCount))
                {
                    throw new Exception("RelatedNumberDALFromReader was called, but the Reader has an invalid Field Count... It is likely that the reader is not actually connected to a RelatedNumberDAL query stream");
                }
            }

            RelatedNumberDAL rv = new RelatedNumberDAL();
            // rv.ID = (int) reader[IDPosition];  // this involves boxing, unboxing and garbage generation
            rv.ID = reader.GetInt32(RelatedNumber_IDPosition);  // this does not involve boxing, unboxing and garbage
            //vr.Name = (string)reader[NamePosition];// no garbage, but casting takes some small time overhead
            rv.RelatedName = reader.GetString(RelatedNumber_RelatedNamePosition);// no garbage and no time penalty
             // r.Doublestuff = (double)reader[DSPosition];// this involves boxing, unboxing and garbage generation
            rv.RelatedLanguage = reader.GetString(RelatedNumber_RelatedLanguagePosition); 
            // previous line has no boxing, unboxing and garbage
            //r.Floatstuff = (float)reader[FSPosition];// this involves boxing, unboxing and garbage generation
            rv.ParentNumberID = reader.GetInt32(RelatedNumber_ParentNumberIDPosition);
            // previous line has no boxing, unboxing and garbage
            return rv;
        }


        #endregion

            #region Riddle Mapping

        static int Riddle_IDPosition=0;
        static int Riddle_RiddlePosition = 0;
        static int Riddle_AnswerPosition = 0;
        static bool Riddle_PositionsInitialized = false;

        static int RiddleFieldCount = 0;

        static public RiddleDAL RiddleDALFromReader(SqlDataReader reader)
        {
            if (!Riddle_PositionsInitialized)
            {
                Riddle_PositionsInitialized = true;
                RiddleFieldCount = reader.FieldCount;
                Riddle_IDPosition = reader.GetOrdinal("RiddleID");
                Riddle_RiddlePosition = reader.GetOrdinal("Riddle");
                Riddle_AnswerPosition = reader.GetOrdinal("Answer");
                
            }
            else
            {
                // check to make sure the schema is still valid
                if (!RiddleFieldCount.Equals(reader.FieldCount))
                {
                    throw new Exception("RiddleDALFromReader was called, but the Reader has an invalid Field Count... It is likely that the reader is not actually connected to a RiddleDAL query stream");
                }
            }

            RiddleDAL rv = new RiddleDAL();
            rv.RiddleID = reader.GetInt32(Riddle_IDPosition);
            rv.Riddle = reader.GetString(Riddle_RiddlePosition);
            rv.Answer = reader.GetString(Riddle_AnswerPosition);
            return rv;
        }
        #endregion

            #region User Mapping

        static int User_IDPosition = 0;
        static int User_EMailPosition = 0;
        static int User_NamePosition = 0;
        static int User_PasswordPosition = 0;
        static int User_SaltPosition = 0;
        static int User_RolesPosition = 0;
        static int User_VerifiedPosition = 0;
        static int User_CommentsPosition = 0;
        static bool User_PositionsInitialized = false;

        static int UserFieldCount = 0;

        public static UserDAL UserDALFromReader(SqlDataReader reader)
        {
            if (!User_PositionsInitialized)
            {
                User_PositionsInitialized = true;
                UserFieldCount = reader.FieldCount;
                User_IDPosition = reader.GetOrdinal("UserID");
              
                User_EMailPosition = reader.GetOrdinal("EMailAddress");
                User_NamePosition = reader.GetOrdinal("Name");
                User_PasswordPosition = reader.GetOrdinal("Password");
                User_SaltPosition = reader.GetOrdinal("Salt");
                User_RolesPosition = reader.GetOrdinal("Roles");
                User_VerifiedPosition = reader.GetOrdinal("Verified");
                User_CommentsPosition = reader.GetOrdinal("Comments");
               

            }
            else
            {
                // check to make sure the schema is still valid
                if (!UserFieldCount.Equals(reader.FieldCount))
                {
                    throw new Exception("UserDALFromReader was called, but the Reader has an invalid Field Count... It is likely that the reader is not actually connected to a UserDAL query stream");
                }
            }
            UserDAL rv = new UserDAL();
            rv.UserID = reader.GetInt32(User_IDPosition);
            rv.EMailAddress = reader.GetString(User_EMailPosition);
            rv.Name = reader.GetString(User_NamePosition);
            rv.Password = reader.GetString(User_PasswordPosition);
            rv.Salt = reader.GetString(User_SaltPosition);
            rv.Roles = reader.GetString(User_RolesPosition);
            rv.Verified = reader.GetString(User_VerifiedPosition);
            rv.Comments = reader.GetString(User_CommentsPosition);
            return rv;
        }

        #endregion

            #region Riddle2NumberMapper
        static int Riddle2NumberNumber_IDPosition = 0;
        static int Riddle2NumberRiddle_IDPosition = 0;
        static bool Riddle2Number_PositionsInitialized = false;

        static int Riddle2NumbersFieldCount = 0;

        public static Riddle2NumberDAL Riddle2NumberFromReader(SqlDataReader reader)
        {
            if (!Riddle2Number_PositionsInitialized)
            {
                Riddle2Number_PositionsInitialized = true;
                Riddle2NumbersFieldCount = reader.FieldCount;
                Riddle2NumberNumber_IDPosition = reader.GetOrdinal("NumberID");

                Riddle2NumberRiddle_IDPosition = reader.GetOrdinal("RiddleID");
               


            }
            else
            {
                // check to make sure the schema is still valid
                if (!RelatedNumberFieldCount.Equals(reader.FieldCount))
                {
                    throw new Exception("Riddle2NumberDALFromReader was called, but the Reader has an invalid Field Count... It is likely that the reader is not actually connected to a Riddle2NumberDAL query stream");
                }
            }

            Riddle2NumberDAL rv = new Riddle2NumberDAL();
            rv.NumberID = reader.GetInt32(Riddle2NumberNumber_IDPosition);
            rv.RiddleID = reader.GetInt32(Riddle2NumberRiddle_IDPosition);
            return rv;
        }
        #endregion

        #endregion

        #region Hiearchy Support
        public NumberHierarchy Numbers;
        public class NumberHierarchy
        {
            DALContext that;
            public NumberHierarchy(DALContext it)
            {
                that = it;
            }
            public int SP_CreateNumber(string Name, double Doublestuff, float Floatstuff)
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();

                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "CreateNumber";
                // load parameters
                localcommand.Parameters.AddWithValue("@Name", Name);
                localcommand.Parameters.AddWithValue("@Doublestuff", Doublestuff);
                localcommand.Parameters.AddWithValue("@Floatstuff", Floatstuff);
                localcommand.Parameters.AddWithValue("@ID", 0);
                localcommand.Parameters["@ID"].Direction = System.Data.ParameterDirection.InputOutput;
                // start the query
                localcommand.ExecuteNonQuery();

                // return the newly created ID
                return Convert.ToInt32(localcommand.Parameters["@ID"].Value);

            }
            public void SP_JustDeleteNumber(int ID)
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();


                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "JustDeleteNumber";
                // load parameters
                localcommand.Parameters.AddWithValue("@ID", ID);
                // start the query
                localcommand.ExecuteNonQuery();
                // return the value 
                // no value to return in a void function
            }
            public void SP_OptimisticUpdateNumber(int ID,
                       string OldName, double OldDoublestuff, float OldFloatstuff,
                       string NewName, double NewDoublestuff, float NewFloatstuff
                       )
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();


                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "OptimisticUpdateNumber";
                // load parameters
                localcommand.Parameters.AddWithValue("@ID", ID);
                localcommand.Parameters.AddWithValue("@OldName", OldName);
                localcommand.Parameters.AddWithValue("@OldDoublestuff", OldDoublestuff);
                localcommand.Parameters.AddWithValue("@OldFloatstuff", OldFloatstuff);
                localcommand.Parameters.AddWithValue("@NewName", NewName);
                localcommand.Parameters.AddWithValue("@NewDoublestuff", NewDoublestuff);
                localcommand.Parameters.AddWithValue("@NewFloatstuff", NewFloatstuff);
                // start the query
                localcommand.ExecuteNonQuery();

            }
            public void SP_PessimisticUpdateNumber(int ID,
                        string NewName, double NewDoublestuff, float NewFloatstuff
                        )
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();
                // create a default return value

                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "PessimisticUpdateNumber";
                // load parameters
                localcommand.Parameters.AddWithValue("@ID", ID);
                localcommand.Parameters.AddWithValue("@NewName", NewName);
                localcommand.Parameters.AddWithValue("@NewDoublestuff", NewDoublestuff);
                localcommand.Parameters.AddWithValue("@NewFloatstuff", NewFloatstuff);
                // start the query
                localcommand.ExecuteNonQuery();
            }

            public List<NumberDAL> SP_ReadAllNumbers()
            {

                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();
                // create a default return value
                List<NumberDAL> rv = new List<NumberDAL>();
                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "ReadAllNumbers";
                // use the 'using' clause to insure the reader is closed in the event an exception is thrown.
                using (SqlDataReader reader = localcommand.ExecuteReader())
                {
                    // reader.Read positions us to the first/Next record returned by the SQL Query
                    // and it returns false when there are no more fecords, and true when the record is positioned

                    while (reader.Read())
                    {
                        rv.Add(DataAccessLayer.DALContext.NumberDALFromReader(reader));
                    }
                    // close of the while loop
                    reader.Close();  // technically this is unecessary because of the using clause
                }// close of the using
                return rv;
            }
            public List<NumberDAL> SP_ReadNumbersBetween(double MinDoublestuff,
                        double MaxDoublestuff)
            {

                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();
                // create a default return value
                List<NumberDAL> rv = new List<NumberDAL>();
                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "ReadNumbersBetween";

                // create parameters
                localcommand.Parameters.AddWithValue("@MinDoublestuff", MinDoublestuff);
                localcommand.Parameters.AddWithValue("@MaxDoublestuff", MaxDoublestuff);
                // use the 'using' clause to insure the reader is closed in the event an exception is thrown.
                using (SqlDataReader reader = localcommand.ExecuteReader())
                {
                    // reader.Read positions us to the first/Next record returned by the SQL Query
                    // and it returns false when there are no more fecords, and true when the record is positioned

                    while (reader.Read())
                    {

                        rv.Add(DALContext.NumberDALFromReader(reader));
                    }  // close of the while loop
                    reader.Close();  // technically this is unecessary because of the using clause
                }// close of the using
                return rv;
            }
            public List<NumberDAL> SP_ReadNumbersHavingNameStartingWith(string Startpattern)
            {

                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();
                // create a default return value
                List<NumberDAL> rv = new List<NumberDAL>();
                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "ReadNumbersHavingNameStartingWith";

                // create parameters
                localcommand.Parameters.AddWithValue("@MStartpattern", Startpattern);
                // use the 'using' clause to insure the reader is closed in the event an exception is thrown.
                using (SqlDataReader reader = localcommand.ExecuteReader())
                {
                    // reader.Read positions us to the first/Next record returned by the SQL Query
                    // and it returns false when there are no more fecords, and true when the record is positioned
                    while (reader.Read())
                    {
                        rv.Add(DALContext.NumberDALFromReader(reader));
                    }  // close of the while loop
                    reader.Close();  // technically this is unecessary because of the using clause
                }// close of the using
                return rv;
            }
            public NumberDAL SP_ReadSpecificNumber(int ID)
            {
                // 1. ensure we are connected to the sql server
                that.EnsureConnected();
                // 2. make an appropriate "default" return value
                NumberDAL rv = null;
                // 3. make a command
                SqlCommand cmd = that.MyConnection.CreateCommand();
                // 4. configure the command
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "ReadSpecificNumber";
                // 5. make parameters if needed (dont forget to add to the command)
                cmd.Parameters.AddWithValue("@ID", ID);
                // 6. execute the command
                using (var reader = cmd.ExecuteReader())
                {
                    // 7. "capture the return value"  [may involve looping and lots of other work to be efficient]
                    if (reader.Read())
                    {
                        rv = DALContext.NumberDALFromReader(reader);
                        if (reader.Read())
                        {
                            throw new Exception($"Found multiple records with id of {ID} in Numbers Database");
                        }
                        reader.Close();  // reader would be closed anyway by using
                        return rv;
                    } // of if statment
                } // of using statement
                return rv;
            }
            public List<NumberDAL> SP_ReadNumbersRelatedtoRiddle(int NumberID)
            {

                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();
                // create a default return value
                List<NumberDAL> rv = new List<NumberDAL>();
                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "ReadNumbersRelatedToRiddle";
                // use the 'using' clause to insure the reader is closed in the event an exception is thrown.
                using (SqlDataReader reader = localcommand.ExecuteReader())
                {
                    // reader.Read positions us to the first/Next record returned by the SQL Query
                    // and it returns false when there are no more fecords, and true when the record is positioned

                    while (reader.Read())
                    {
                        rv.Add(DataAccessLayer.DALContext.NumberDALFromReader(reader));
                    }
                    // close of the while loop
                    reader.Close();  // technically this is unecessary because of the using clause
                }// close of the using
                return rv;
            }
            public void SP_SafeDeleteNumber(int ID,
                       string CurrentName, double CurrentDoublestuff, float CurrentFloatstuff)
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();
                // create a default return value

                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "SafeDeleteNumber";
                // load parameters
                localcommand.Parameters.AddWithValue("@ID", ID);
                localcommand.Parameters.AddWithValue("@CurrentName", CurrentName);
                localcommand.Parameters.AddWithValue("@CurrentDoublestuff", CurrentDoublestuff);
                localcommand.Parameters.AddWithValue("@CurrentFloatstuff", CurrentFloatstuff);

                // start the query
                localcommand.ExecuteNonQuery();


            }

        }

        public RelatedNumberHierarchy RelatedNumbers;
        public class RelatedNumberHierarchy
        {
            DALContext that;
            public RelatedNumberHierarchy(DALContext it)
            {
                that = it;
            }
            public int SP_CreateRelatedNumber(string RelatedName, string RelatedLanguage, int ParentNumber_ID)
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();

                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "CreateRelatedNumber";
                // load parameters
                localcommand.Parameters.AddWithValue("@RelatedName", RelatedName);
                localcommand.Parameters.AddWithValue("@RelatedLanguage", RelatedLanguage);
                localcommand.Parameters.AddWithValue("@ParentNumber_ID", ParentNumber_ID);
                localcommand.Parameters.AddWithValue("@ID", 0);
                localcommand.Parameters["@ID"].Direction = System.Data.ParameterDirection.InputOutput;
                // start the query
                localcommand.ExecuteNonQuery();

                // return the newly created ID
                return Convert.ToInt32(localcommand.Parameters["@ID"].Value);
            }


            public void SP_JustDeleteRelatedNumber(int ID)
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();


                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "JustDeleteRelatedNumber";
                // load parameters
                localcommand.Parameters.AddWithValue("@ID", ID);
                // start the query
                localcommand.ExecuteNonQuery();
            }


            public void SP_OptimisticUpdateRelatedNumber(int ID,
                        string OldRelatedName, string OldRelatedLanguage, int OldParentNumberID,
                        string NewRelatedName, string NewRelatedLanguage, int NewParentNumberID
                        )
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();
                // create a default return value

                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "OptimisticUpdateRelatedNumber";
                // load parameters
                localcommand.Parameters.AddWithValue("@ID", ID);
                localcommand.Parameters.AddWithValue("@OldRelatedName", OldRelatedName);
                localcommand.Parameters.AddWithValue("@OldRelatedLanguage", OldRelatedLanguage);
                localcommand.Parameters.AddWithValue("@OldParentNumberID", OldParentNumberID);
                localcommand.Parameters.AddWithValue("@NewRelatedName", NewRelatedName);
                localcommand.Parameters.AddWithValue("@NewRelatedLanguage", NewRelatedLanguage);
                localcommand.Parameters.AddWithValue("@NewParentNumber_ID", NewParentNumberID);
                // start the query
                localcommand.ExecuteNonQuery();
            }
            public void SP_PessimisticUpdateRelatedNumber(int ID,
                       string NewRelatedName, string NewRelatedLanguage, int NewParentNumberID
                       )
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();
                // create a default return value

                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "PessimisticUpdateRelatedNumber";
                // load parameters
                localcommand.Parameters.AddWithValue("@ID", ID);
                localcommand.Parameters.AddWithValue("@NewRelatedName", NewRelatedName);
                localcommand.Parameters.AddWithValue("@NewRelatedLanguage", NewRelatedLanguage);
                localcommand.Parameters.AddWithValue("@NewParentNumber_ID", NewParentNumberID);
                // start the query
                localcommand.ExecuteNonQuery();
            }

            public List<RelatedNumberDAL> SP_ReadAllRelatedNumbers()
            {


                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();
                List<RelatedNumberDAL> rv = new List<RelatedNumberDAL>();
                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "ReadAllRelatedNumbers";
                // use the 'using' clause to insure the reader is closed in the event an exception is thrown.
                using (SqlDataReader reader = localcommand.ExecuteReader())
                {
                    // reader.Read positions us to the first/Next record returned by the SQL Query
                    // and it returns false when there are no more fecords, and true when the record is positioned
                    while (reader.Read())
                    {
                        rv.Add(DataAccessLayer.DALContext.RelatedNumberDALFromReader(reader));
                    }  // close of the while loop
                    reader.Close();  // technically this is unecessary because of the using clause
                }// close of the using
                return rv;
            }
            public List<RelatedNumberDAL> SP_ReadRelatedNumbersHavingRelatedNameStartingWith(string Startpattern)
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();
                List<RelatedNumberDAL> rv = new List<RelatedNumberDAL>();
                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "ReadRelatedNumbersHavingRelatedNameStartingWith";
                localcommand.Parameters.AddWithValue("@Startpattern", Startpattern);
                // use the 'using' clause to insure the reader is closed in the event an exception is thrown.
                using (SqlDataReader reader = localcommand.ExecuteReader())
                {
                    // reader.Read positions us to the first/Next record returned by the SQL Query
                    // and it returns false when there are no more fecords, and true when the record is positioned
                    while (reader.Read())
                    {
                        rv.Add(DALContext.RelatedNumberDALFromReader(reader));
                    }  // close of the while loop

                    reader.Close();  // technically this is unecessary because of the using clause
                }// close of the using
                return rv;
            }
            public List<RelatedNumberDAL> SP_ReadRelatedNumbersHavingSpecificParentID(int ParentID)
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();
                List<RelatedNumberDAL> rv = new List<RelatedNumberDAL>();
                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "ReadRelatedNumbersHavingSpecificParentID";
                localcommand.Parameters.AddWithValue("@ID", ParentID);
                // use the 'using' clause to insure the reader is closed in the event an exception is thrown.
                using (SqlDataReader reader = localcommand.ExecuteReader())
                {
                    // reader.Read positions us to the first/Next record returned by the SQL Query
                    // and it returns false when there are no more fecords, and true when the record is positioned
                    while (reader.Read())
                    {
                        rv.Add(DALContext.RelatedNumberDALFromReader(reader));
                    }  // close of the while loop

                    // close of the while loop
                    reader.Close();  // technically this is unecessary because of the using clause
                }// close of the using
                return rv;
            }
            public List<RelatedNumberDAL> SP_ReadRelatedNumbersHavingLanguageStartingWith(string Startpattern)
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();
                List<RelatedNumberDAL> rv = new List<RelatedNumberDAL>();
                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "ReadRelatedNumbersHavingLanguageStartingWith";
                localcommand.Parameters.AddWithValue("@Startpattern", Startpattern);
                // use the 'using' clause to insure the reader is closed in the event an exception is thrown.
                using (SqlDataReader reader = localcommand.ExecuteReader())
                {
                    // reader.Read positions us to the first/Next record returned by the SQL Query
                    // and it returns false when there are no more fecords, and true when the record is positioned
                    while (reader.Read())
                    {
                        rv.Add(DALContext.RelatedNumberDALFromReader(reader));
                    }  // close of the while loop

                    reader.Close();  // technically this is unecessary because of the using clause
                }// close of the using
                return rv;
            }
            public RelatedNumberDAL SP_ReadSpecificRelatedNumber(int ID)
            {
                // 1. ensure we are connected to the sql server
                that.EnsureConnected();
                // 2. make an appropriate "default" return value
                RelatedNumberDAL rv = null;
                // 3. make a command
                SqlCommand cmd = that.MyConnection.CreateCommand();
                // 4. configure the command
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "ReadSpecificRelatedNumber";
                // 5. make parameters if needed (dont forget to add to the command)
                cmd.Parameters.AddWithValue("@ID", ID);
                // 6. execute the command
                using (var reader = cmd.ExecuteReader())
                {
                    // 7. "capture the return value"  [may involve looping and lots of other work to be efficient]
                    if (reader.Read())
                    {
                        rv = DALContext.RelatedNumberDALFromReader(reader);
                        if (reader.Read())
                        {
                            throw new Exception($"Found multiple records with id of {ID} in Numbers Database");
                        }
                        reader.Close();  // reader would be closed anyway by using
                        return rv;
                    } // of if statment
                } // of using statement
                return rv;
            }

            public void SP_SafeDeleteRelatedNumber(int ID,
                        string CurrentRelatedName, string CurrentRelatedLanguage, int CurrentParentNumberID)
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();
                // create a default return value

                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "SafeDeleteRelatedNumber";
                // load parameters
                localcommand.Parameters.AddWithValue("@ID", ID);
                localcommand.Parameters.AddWithValue("@CurrentRelatedName", CurrentRelatedName);
                localcommand.Parameters.AddWithValue("@CurrentRelatedLanguage", CurrentRelatedLanguage);
                localcommand.Parameters.AddWithValue("@CurrentParentNumber_ID", CurrentParentNumberID);

                // start the query
                localcommand.ExecuteNonQuery();

            }


        }

        public RiddleHierarchy Riddles;
        public class RiddleHierarchy
        {
            DALContext that;
            public RiddleHierarchy(DALContext it)
            {
                that = it;
            }
            public int SP_CreateRiddle(string riddle, string answer)
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();

                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "CreateRiddle";
                // load parameters
                localcommand.Parameters.AddWithValue("@Riddle", riddle);
 
                localcommand.Parameters.AddWithValue("@Answer", answer);

                localcommand.Parameters.AddWithValue("@RiddleID", 0);
                localcommand.Parameters["@RiddleID"].Direction = System.Data.ParameterDirection.InputOutput;
                // start the query
                localcommand.ExecuteNonQuery();

                // return the newly created ID
                return Convert.ToInt32(localcommand.Parameters["@RiddleID"].Value);

            }
            public void SP_JustDeleteRiddle(int ID)
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();


                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "JustDeleteRiddle";
                // load parameters
                localcommand.Parameters.AddWithValue("@RiddleID", ID);
                // start the query
                localcommand.ExecuteNonQuery();
                // return the value 
                // no value to return in a void function
            }
            public void SP_OptimisticUpdateRiddle(int RiddleID,
                       string OldRiddle, 
                       string NewRiddle,
                       string OldAnswer,
                       string NewAnswer
                )
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();


                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "OptimisticUpdateRiddle";
                // load parameters
                localcommand.Parameters.AddWithValue("@RiddleID", RiddleID);
                localcommand.Parameters.AddWithValue("@OldRiddle", OldRiddle);
                localcommand.Parameters.AddWithValue("@OldAnswer", OldAnswer);

                localcommand.Parameters.AddWithValue("@NewRiddle", NewRiddle);
                localcommand.Parameters.AddWithValue("@NewAnswer", NewAnswer);

                // start the query
                localcommand.ExecuteNonQuery();

            }
            public void SP_PessimisticUpdateRiddle(int RiddleID,
                        string NewRiddle, string NewAnswer
                        )
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();
                // create a default return value

                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "PessimisticUpdateRiddle";
                // load parameters
                localcommand.Parameters.AddWithValue("@RidleID", RiddleID);
                localcommand.Parameters.AddWithValue("@NewRidle", NewRiddle);
                localcommand.Parameters.AddWithValue("@NewRAnswer", NewAnswer);


                // start the query
                localcommand.ExecuteNonQuery();
            }

            public List<RiddleDAL> SP_ReadAllRiddles()
            {

                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();
                // create a default return value
                List<RiddleDAL> rv = new List<RiddleDAL>();
                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "ReadAllRiddles";
                // use the 'using' clause to insure the reader is closed in the event an exception is thrown.
                using (SqlDataReader reader = localcommand.ExecuteReader())
                {
                    // reader.Read positions us to the first/Next record returned by the SQL Query
                    // and it returns false when there are no more fecords, and true when the record is positioned

                    while (reader.Read())
                    {
                        rv.Add(DataAccessLayer.DALContext.RiddleDALFromReader(reader));
                    }
                    // close of the while loop
                    reader.Close();  // technically this is unecessary because of the using clause
                }// close of the using
                return rv;
            }

            public List<RiddleDAL> SP_ReadRiddlesRelatedtoNumber(int NumberID)
            {

                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();
                // create a default return value
                List<RiddleDAL> rv = new List<RiddleDAL>();
                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "ReadRiddlesRelatedToNumber";
                // use the 'using' clause to insure the reader is closed in the event an exception is thrown.
                using (SqlDataReader reader = localcommand.ExecuteReader())
                {
                    // reader.Read positions us to the first/Next record returned by the SQL Query
                    // and it returns false when there are no more fecords, and true when the record is positioned

                    while (reader.Read())
                    {
                        rv.Add(DataAccessLayer.DALContext.RiddleDALFromReader(reader));
                    }
                    // close of the while loop
                    reader.Close();  // technically this is unecessary because of the using clause
                }// close of the using
                return rv;
            }
            public RiddleDAL SP_ReadSpecificRiddle(int RiddleID)
            {
                // 1. ensure we are connected to the sql server
                that.EnsureConnected();
                // 2. make an appropriate "default" return value
                RiddleDAL rv = null;
                // 3. make a command
                SqlCommand cmd = that.MyConnection.CreateCommand();
                // 4. configure the command
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "ReadSpecificRiddle";
                // 5. make parameters if needed (dont forget to add to the command)
                cmd.Parameters.AddWithValue("@RiddleID", RiddleID);
                // 6. execute the command
                using (var reader = cmd.ExecuteReader())
                {
                    // 7. "capture the return value"  [may involve looping and lots of other work to be efficient]
                    if (reader.Read())
                    {
                        rv = DALContext.RiddleDALFromReader(reader);
                        if (reader.Read())
                        {
                            throw new Exception($"Found multiple records with id of {RiddleID} in Riddles Database");
                        }
                        reader.Close();  // reader would be closed anyway by using
                        return rv;
                    } // of if statment
                } // of using statement
                return rv;
            }
            // SaveDeleteRiddle

        }

        public UserHierarchy Users;
        public class UserHierarchy
        {
            DALContext that;
            public UserHierarchy(DALContext it)
            {
                that = it;
            }
            public int SP_CreateUser(string emailaddress, string name, string password, string salt, string roles, string verified, string comments)
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();

                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "CreateUser";
                // load parameters
                localcommand.Parameters.AddWithValue("@EMailAddress", emailaddress);
                localcommand.Parameters.AddWithValue("@Name", name);
                localcommand.Parameters.AddWithValue("@Password", password);
                localcommand.Parameters.AddWithValue("@Salt", salt);
                localcommand.Parameters.AddWithValue("@Roles", roles);
                localcommand.Parameters.AddWithValue("@Verified", verified);
                localcommand.Parameters.AddWithValue("@Comments", comments);






                localcommand.Parameters.AddWithValue("@UserID", 0);
                localcommand.Parameters["@UserID"].Direction = System.Data.ParameterDirection.InputOutput;
                // start the query
                localcommand.ExecuteNonQuery();

                // return the newly created ID
                return Convert.ToInt32(localcommand.Parameters["@UserID"].Value);

            }

            public UserDAL SP_ReadSpecificUser(int UserID)
            {
                // 1. ensure we are connected to the sql server
                that.EnsureConnected();
                // 2. make an appropriate "default" return value
                UserDAL rv = null;
                // 3. make a command
                SqlCommand cmd = that.MyConnection.CreateCommand();
                // 4. configure the command
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "ReadSpecificUser";
                // 5. make parameters if needed (dont forget to add to the command)
                cmd.Parameters.AddWithValue("@UserID", UserID);
                // 6. execute the command
                using (var reader = cmd.ExecuteReader())
                {
                    // 7. "capture the return value"  [may involve looping and lots of other work to be efficient]
                    if (reader.Read())
                    {
                        rv = DALContext.UserDALFromReader(reader);
                        if (reader.Read())
                        {
                            throw new Exception($"Found multiple records with id of {UserID} in Users Database");
                        }
                        reader.Close();  // reader would be closed anyway by using
                        return rv;
                    } // of if statment
                } // of using statement
                return rv;
            }

            public UserDAL SP_ReadSpecificUserByEMail(string EMailAddress)
            {
                // 1. ensure we are connected to the sql server
                that.EnsureConnected();
                // 2. make an appropriate "default" return value
                UserDAL rv = null;
                // 3. make a command
                SqlCommand cmd = that.MyConnection.CreateCommand();
                // 4. configure the command
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "ReadUserWithEMail";
                // 5. make parameters if needed (dont forget to add to the command)
                cmd.Parameters.AddWithValue("@pattern", EMailAddress);
                // 6. execute the command
                using (var reader = cmd.ExecuteReader())
                {
                    // 7. "capture the return value"  [may involve looping and lots of other work to be efficient]
                    if (reader.Read())
                    {
                        rv = DALContext.UserDALFromReader(reader);
                        if (reader.Read())
                        {
                            throw new Exception($"Found multiple records with EMail ending with {EMailAddress} in Numbers Database");
                        }
                        reader.Close();  // reader would be closed anyway by using
                        return rv;
                    } // of if statment
                } // of using statement
                return rv;
            }

            public List<UserDAL> SP_ReadAllUsers()
            {

                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();
                // create a default return value
                List<UserDAL> rv = new List<UserDAL>();
                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "ReadAllUsers";
                // use the 'using' clause to insure the reader is closed in the event an exception is thrown.
                using (SqlDataReader reader = localcommand.ExecuteReader())
                {
                    // reader.Read positions us to the first/Next record returned by the SQL Query
                    // and it returns false when there are no more fecords, and true when the record is positioned

                    while (reader.Read())
                    {
                        rv.Add(DataAccessLayer.DALContext.UserDALFromReader(reader));
                    }
                    // close of the while loop
                    reader.Close();  // technically this is unecessary because of the using clause
                }// close of the using
                return rv;
            }

            public List<UserDAL> SP_ReadUsersWithEMailStartingWith(string EMailAddress)
            {

                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();
                // create a default return value
                List<UserDAL> rv = new List<UserDAL>();
                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "ReadUsersWithEMailStartingWith";

                localcommand.Parameters.AddWithValue("@pattern", EMailAddress);
                // use the 'using' clause to insure the reader is closed in the event an exception is thrown.
                using (SqlDataReader reader = localcommand.ExecuteReader())
                {
                    // reader.Read positions us to the first/Next record returned by the SQL Query
                    // and it returns false when there are no more fecords, and true when the record is positioned

                    while (reader.Read())
                    {
                        rv.Add(DataAccessLayer.DALContext.UserDALFromReader(reader));
                    }
                    // close of the while loop
                    reader.Close();  // technically this is unecessary because of the using clause
                }// close of the using
                return rv;
            }


            public List<UserDAL> SP_ReadUsersWithEMailEndingWith(string EMailAddress)
            {

                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();
                // create a default return value
                List<UserDAL> rv = new List<UserDAL>();
                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "ReadUsersWithEMailEndingWith";

                localcommand.Parameters.AddWithValue("@pattern", EMailAddress);
                // use the 'using' clause to insure the reader is closed in the event an exception is thrown.
                using (SqlDataReader reader = localcommand.ExecuteReader())
                {
                    // reader.Read positions us to the first/Next record returned by the SQL Query
                    // and it returns false when there are no more fecords, and true when the record is positioned

                    while (reader.Read())
                    {
                        rv.Add(DataAccessLayer.DALContext.UserDALFromReader(reader));
                    }
                    // close of the while loop
                    reader.Close();  // technically this is unecessary because of the using clause
                }// close of the using
                return rv;
            }

            public void SP_JustDeleteUser(int UserID)
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();


                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "JustDeleteUser";
                // load parameters
                localcommand.Parameters.AddWithValue("@UserID", UserID);
                // start the query
                localcommand.ExecuteNonQuery();
                // return the value 
                // no value to return in a void function
            }

            public void SP_OptimisticUpdateUser(int UserID,
           string oldEmailaddress, string oldName, string oldPassword, string oldSalt, string oldRoles, string oldVerified, string oldComments,
           string newEmailaddress, string newName, string newPassword, string newSalt, string newRoles, string newVerified, string newComments
           )
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();


                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "OptimisticUpdateUser";
                // load parameters
     localcommand.Parameters.AddWithValue("@ExistingUserID", UserID);
     localcommand.Parameters.AddWithValue("@OldEmailAddress",oldEmailaddress);
     localcommand.Parameters.AddWithValue("@OldName", oldName);
     localcommand.Parameters.AddWithValue("@OldPassword", oldPassword);
     localcommand.Parameters.AddWithValue("@OldSalt", oldSalt);
     localcommand.Parameters.AddWithValue("@OldRoles", oldRoles);
     localcommand.Parameters.AddWithValue("@OldVerified", oldVerified);
     localcommand.Parameters.AddWithValue("@OldComments", oldComments);
    localcommand.Parameters.AddWithValue("@NewEmailAddress", newEmailaddress);
    localcommand.Parameters.AddWithValue("@NewName", newName);
    localcommand.Parameters.AddWithValue("@NewPassword", newPassword);
    localcommand.Parameters.AddWithValue("@NewSalt", newSalt);
    localcommand.Parameters.AddWithValue("@NewRoles", newRoles);
    localcommand.Parameters.AddWithValue("@NewVerified", newVerified);
    localcommand.Parameters.AddWithValue("@NewComments", newComments);


                // start the query
                localcommand.ExecuteNonQuery();

            }
            public void SP_PessimisticUpdateUser(int UserID,
                      string newEmailaddress, string newName, string newPassword, string newSalt, string newRoles, string newVerified, string newComments
                        )
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();
                // create a default return value

                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "PessimisticUpdateUser";
                localcommand.Parameters.AddWithValue("@ExistingUserID", UserID);
              ;
                localcommand.Parameters.AddWithValue("@NewEmailAddress", newEmailaddress);
                localcommand.Parameters.AddWithValue("@NewName", newName);
                localcommand.Parameters.AddWithValue("@NewPassword", newPassword);
                localcommand.Parameters.AddWithValue("@NewSalt", newSalt);
                localcommand.Parameters.AddWithValue("@NewRoles", newRoles);
                localcommand.Parameters.AddWithValue("@NewVerified", newVerified);
                localcommand.Parameters.AddWithValue("@NewComments", newComments);
                // start the query
                localcommand.ExecuteNonQuery();
            }



        }


        public Number2RiddleHierarchy Numbers2Riddles;
        public class Number2RiddleHierarchy
        {
            DALContext that;
            public Number2RiddleHierarchy(DALContext it)
            {
                that = it;
            }
            public void SP_CreateNumber2Riddle(int numberID, int riddleID)
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();

                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "CreateRiddle2Number";
                // load parameters
                localcommand.Parameters.AddWithValue("@NumberID", numberID );
                localcommand.Parameters.AddWithValue("@RiddleID", riddleID);

                
                // start the query
                localcommand.ExecuteNonQuery();

                // return the newly created ID
                return;

            }
            public void SP_JustDeleteNumber2Riddle(int RiddleID, int NumberID)
            {
                // Ensure Connected opens the connection to the database if it is not already opened
                that.EnsureConnected();


                // create a SQL Command object using the existing and open connection.  We know it is open 
                // because of EnsureConnected();
                SqlCommand localcommand = that.MyConnection.CreateCommand();
                // configure the command object.  set the type to Stored Procedure, and set the Text to the 
                // name of the stored procedure

                localcommand.CommandType = System.Data.CommandType.StoredProcedure;
                localcommand.CommandText = "JustDeleteNumber2Riddle";
                // load parameters
                localcommand.Parameters.AddWithValue("@RiddleID", RiddleID);
                localcommand.Parameters.AddWithValue("@NumberID", NumberID);
                // start the query
                localcommand.ExecuteNonQuery();
                // return the value 
                // no value to return in a void function
            }
            // safedelete
            // update

        }

        #endregion




    }
}
