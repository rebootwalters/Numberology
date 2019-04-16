using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class BLLContext : IDisposable
    {
        #region constructors and stuff
        private void inithiearchy()
        {
            _numbers = new NumberHiearchy(this);
            _relatedNumbers = new RelatedNumberHiearchy(this);
            _loadingItems = new LoadingRelatedItemsHiearchy(this);
            _riddles = new RiddlesHiearchy(this);
            _users = new UsersHiearchy(this);
        }
        DataAccessLayer.DALContext DataContext = new DataAccessLayer.DALContext();
        private NumberHiearchy _numbers;
        public NumberHiearchy Numbers { get { return _numbers; } }
        private RelatedNumberHiearchy _relatedNumbers;
        public RelatedNumberHiearchy RelatedNumbers {get {return _relatedNumbers;} }
        private LoadingRelatedItemsHiearchy _loadingItems;
        public LoadingRelatedItemsHiearchy LoadingItems { get { return _loadingItems; } }
        private RiddlesHiearchy _riddles;
        public RiddlesHiearchy Riddles { get { return _riddles; } }

        private UsersHiearchy _users;
        public UsersHiearchy Users { get { return _users; } }


        public BLLContext()
        {
            inithiearchy();
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionString"];
            if (cs == null)
            {
                throw new Exception("The Business Logic Layer expects a DefaultConnectionString entry in your config file ");
            }
            DataContext.ConnectionString = cs.ConnectionString;
        }

        public BLLContext(string ConnectionString)
        {
            inithiearchy();
            DataContext.ConnectionString = ConnectionString;
        }
        #endregion


        #region LoadingRelatedItems
        public class LoadingRelatedItemsHiearchy
        {
            internal LoadingRelatedItemsHiearchy(BLLContext it) { that = it; }
            internal BLLContext that;
            public void LoadRelatedNumbersIntoNumber(NumberBLL n)
            {
                n._relatedNumbers = that.RelatedNumbers.GetAllRelatedNumbersWithSpecificParent(n);
            }
            public void LoadParentIntoRelatedNumber(RelatedNumberBLL n)
            {
                n._parentNumber = that.Numbers.FindNumber(n.ID);
            }
            public void LoadRiddlesIntoNumber(NumberBLL n, bool AlsoBackLoadNumbersIntoRiddles = false)
            {
                n._riddles = that.Riddles.GetAllRiddlesWithSpecificNumber(n, AlsoBackLoadNumbersIntoRiddles);
            }

     

            public void LoadRelatedNumbersIntoRiddle(RiddleBLL r, bool AlsoBackLoadRelatedRiddlesIntoNumbers = false)
            {
                r._numbers = that.Numbers.GetAllNumbersWithSpecificRiddle(r, AlsoBackLoadRelatedRiddlesIntoNumbers);
            }

       



        }
        #endregion


        #region Numbers
        public class NumberHiearchy
        {
            internal NumberHiearchy(BLLContext it) { that = it; }
            internal BLLContext that;
 


            #region Create

            public NumberBLL InsertNewNumber(string name, double doublestuff, float floatstuff)
            {
                int id = that.DataContext.Numbers.SP_CreateNumber(name, doublestuff, floatstuff);
                NumberBLL rv = new NumberBLL();
                rv.ID = id;
                rv.Name = name;
                rv.Doublestuff = doublestuff;
                rv.Floatstuff = floatstuff;
                return rv;
            }
            #endregion Create
            #region Read
            public NumberBLL FindNumber(int id)
            {
                var r = that.DataContext.Numbers.SP_ReadSpecificNumber(id);
                if (r == null) return null;
                DataAccessLayer.NumberDAL wrongtype = that.DataContext.Numbers.SP_ReadSpecificNumber(id);
                // could do something like mapper.BLLFromDAL(wrongType);
                // instead I am using Construtor mapping
                return new NumberBLL(wrongtype);
            }

            public List<NumberBLL> GetAllNumbers()
            {
                List<NumberBLL> rv = new List<NumberBLL>();
                var dalnumbers = that.DataContext.Numbers.SP_ReadAllNumbers();
                foreach (var n in dalnumbers)
                {
                    rv.Add(new NumberBLL(n));
                }

                return rv;
            }

            public List<NumberBLL> GetNumbersStartingWith(string pattern)
            {
                List<NumberBLL> rv = new List<NumberBLL>();
                var dalnumbers = that.DataContext.Numbers.SP_ReadNumbersHavingNameStartingWith(pattern);
                foreach (var n in dalnumbers)
                {
                    rv.Add(new NumberBLL(n));
                }

                return rv;
            }

            public List<NumberBLL> GetNumbersBetween(double min, double max)
            {
                List<NumberBLL> rv = new List<NumberBLL>();
                var dalnumbers = that.DataContext.Numbers.SP_ReadNumbersBetween(min, max);
                foreach (var n in dalnumbers)
                {
                    rv.Add(new NumberBLL(n));
                }

                return rv;
            }

            public List<NumberBLL> GetAllNumbersWithSpecificRiddle(RiddleBLL Parent, bool AlsoBackLoadRiddlesIntoNumbers = false)
            {
                List<NumberBLL> rv = new List<NumberBLL>();
                var dalnumbers = that.DataContext.Numbers.SP_ReadNumbersRelatedtoRiddle(Parent.RiddleID);
                foreach (var number in dalnumbers)
                {
                    NumberBLL numberBLL = new NumberBLL(number);
                    rv.Add(numberBLL);
                    if (AlsoBackLoadRiddlesIntoNumbers)
                    {
                        // ToDo:  Backload related Numbers into Riddles
                        // do not backload another level of riddles through these numbers....
                        that.LoadingItems.LoadRiddlesIntoNumber(numberBLL, false);
                    }
                }

                return rv;
            }

            #endregion Read
            #region Update
            public void OptimisticUpdateOfNumber(int id, NumberBLL OriginalValues, NumberBLL NewValues)
            {

                that.DataContext.Numbers.SP_OptimisticUpdateNumber(id,
                    OriginalValues.Name, OriginalValues.Doublestuff, OriginalValues.Floatstuff,
                    NewValues.Name, NewValues.Doublestuff, NewValues.Floatstuff);

            }

            public void PessimisticUpdateOfNumber(int id, NumberBLL NewValues)
            {
                that.DataContext.Numbers.SP_PessimisticUpdateNumber(id, NewValues.Name, NewValues.Doublestuff, NewValues.Floatstuff);

            }
            #endregion Update
            #region Delete
            public void SafeDeleteOfNumber(int id, NumberBLL NumberToDelete)
            {
                that.DataContext.Numbers.SP_SafeDeleteNumber(id, NumberToDelete.Name, NumberToDelete.Doublestuff, NumberToDelete.Floatstuff);
            }

            public void JustDeleteANumber(int id)
            {
                that.DataContext.Numbers.SP_JustDeleteNumber(id);
            }
            #endregion Delete
        }
        #endregion Numbers

        #region related numbers
        public class RelatedNumberHiearchy
        {
            internal RelatedNumberHiearchy(BLLContext it) { that = it; }
            internal BLLContext that;

            #region Create

            public RelatedNumberBLL InsertNewRelatedNumber(string relatedname, string relatedlanguage, int parentID)
            {
                int id = that.DataContext.RelatedNumbers.SP_CreateRelatedNumber(relatedname, relatedlanguage, parentID);
                RelatedNumberBLL rv = new RelatedNumberBLL();
                rv.ID = id;
                rv.ParentNumberID = parentID;
                rv.RelatedLanguage = relatedlanguage;
                rv.RelatedName = relatedname;
                return rv;

            }

            #endregion Create
            #region Read
            public RelatedNumberBLL FindRelatedNumber(int id)
            {
                var r = that.DataContext.RelatedNumbers.SP_ReadSpecificRelatedNumber(id);
                if (r == null) return null;
                return new RelatedNumberBLL(r);
            }

            public List<RelatedNumberBLL> GetAllRelatedNumbers()
            {
                List<RelatedNumberBLL> rv = new List<RelatedNumberBLL>();
                var dalnumbers = that.DataContext.RelatedNumbers.SP_ReadAllRelatedNumbers();
                foreach (var n in dalnumbers)
                {
                    rv.Add(new RelatedNumberBLL(n));
                }

                return rv;
            }

            public List<RelatedNumberBLL> GetRelatedNumbersHavingRelatedNameStartingWith(string pattern)
            {
                List<RelatedNumberBLL> rv = new List<RelatedNumberBLL>();
                var dalnumbers = that.DataContext.RelatedNumbers.SP_ReadRelatedNumbersHavingRelatedNameStartingWith(pattern);
                foreach (var n in dalnumbers)
                {
                    rv.Add(new RelatedNumberBLL(n));
                }

                return rv;
            }

            public List<RelatedNumberBLL> GetRelatedNumbersHavingLanguageStartingWith(string pattern)
            {
                List<RelatedNumberBLL> rv = new List<RelatedNumberBLL>();
                var dalnumbers = that.DataContext.RelatedNumbers.SP_ReadRelatedNumbersHavingLanguageStartingWith(pattern);
                foreach (var n in dalnumbers)
                {
                    rv.Add(new RelatedNumberBLL(n));
                }

                return rv;
            }

            public List<RelatedNumberBLL> GetAllRelatedNumbersWithSpecificParent(NumberBLL Parent)
            {
                List<RelatedNumberBLL> rv = new List<RelatedNumberBLL>();
                var dalnumbers = that.DataContext.RelatedNumbers.SP_ReadRelatedNumbersHavingSpecificParentID(Parent.ID);
                foreach (var n in dalnumbers)
                {
                    rv.Add(new RelatedNumberBLL(Parent, n));
                }

                return rv;
            }

            // do i really need this one, or should I require a Parent in order to find related children
            //public List<RelatedNumberBLL> FindAllRelatedNumbersWithSpecificParentID(int id)
            //{
            //    List<RelatedNumberBLL> rv = new List<RelatedNumberBLL>();

            //    var dalnumbers = DataContext.SP_ReadRelatedNumbersHavingSpecificParentID(id);
            //    foreach (var n in dalnumbers)
            //    {
            //        rv.Add(new RelatedNumberBLL( n));
            //    }

            //    return rv;
            //}
            #endregion Read
            #region Update
            public void OptimisticUpdateOfRelatedNumber(RelatedNumberBLL OriginalValues, RelatedNumberBLL NewValues)
            {
                if (OriginalValues.ID != NewValues.ID)
                {
                    throw new Exception($"You may not change the ID of a RelatedNumber item during an Update.  The OriginalValues ID is {OriginalValues.ID} and the NewValues ID is {NewValues.ID}.  They must be the same.");
                }
                that.DataContext.RelatedNumbers.SP_OptimisticUpdateRelatedNumber(OriginalValues.ID,
                    OriginalValues.RelatedName, OriginalValues.RelatedLanguage, OriginalValues.ParentNumberID,
                    NewValues.RelatedName, NewValues.RelatedLanguage, NewValues.ParentNumberID);

            }

            public void PessimisticUpdateOfRelatedNumber(RelatedNumberBLL NewValues)
            {
                that.DataContext.RelatedNumbers.SP_PessimisticUpdateRelatedNumber(NewValues.ID, NewValues.RelatedName, NewValues.RelatedLanguage, NewValues.ParentNumberID);

            }
            #endregion Update
            #region Delete
            public void SafeDeleteOfNumber(RelatedNumberBLL NumberToDelete)
            {
                that.DataContext.RelatedNumbers.SP_SafeDeleteRelatedNumber(NumberToDelete.ID, NumberToDelete.RelatedName, NumberToDelete.RelatedLanguage, NumberToDelete.ParentNumberID);
            }

            public void JustDeleteARelatedNumber(int ID)
            {
                that.DataContext.Numbers.SP_JustDeleteNumber(ID);
            }
            #endregion Delete

        }
        #endregion related numebrs

        #region Riddles
        public class RiddlesHiearchy
        {
            internal RiddlesHiearchy(BLLContext it) { that = it; }
            internal BLLContext that;

            public List<RiddleBLL> GetAllRiddlesWithSpecificNumber(NumberBLL Parent, bool AlsoBackLoadNumbersIntoRiddles = false)
            {
                List<RiddleBLL> rv = new List<RiddleBLL>();
                var dalriddles = that.DataContext.Riddles.SP_ReadRiddlesRelatedtoNumber(Parent.ID);
                foreach (var riddle in dalriddles)
                {
                    RiddleBLL riddleBLL = new RiddleBLL(riddle);
                    rv.Add(riddleBLL);
                    if (AlsoBackLoadNumbersIntoRiddles)
                    {
                        // ToDo:  Backload related Numbers into Riddles
                        // do not backload another level of riddles through these numbers....
                        that.LoadingItems.LoadRelatedNumbersIntoRiddle(riddleBLL, false);
                    }
                }

                return rv;
            }

            public RiddleBLL FindRiddle(int RiddleID)
            {
                RiddleDAL r = that.DataContext.Riddles.SP_ReadSpecificRiddle(RiddleID);
                if (r == null) return null;
                return new RiddleBLL(r);

            }

            public List<RiddleBLL > GetAllRiddles()
            {
                List<RiddleBLL> rv = new List<RiddleBLL>();
                foreach (var item in that.DataContext.Riddles.SP_ReadAllRiddles())
                {
                    rv.Add(new RiddleBLL(item));
                }
                return rv;
            }

            public RiddleBLL InsertNewRiddle(string riddle, string answer)
            {
                int id =that.DataContext.Riddles.SP_CreateRiddle(riddle, answer);
                return new RiddleBLL() { RiddleID = id, Answer=answer, Riddle=riddle  };
            }
            public RiddleBLL InsertNewRiddle(RiddleBLL r)
            {
                int id = that.DataContext.Riddles.SP_CreateRiddle(r.Riddle, r.Answer);
                
                return new RiddleBLL() { RiddleID = id, Answer = r.Answer, Riddle = r.Riddle };
            }

            public void UpdateExistingRiddle(int RiddleID, string NewRiddle, string NewAnswer)
            {
                that.DataContext.Riddles.SP_PessimisticUpdateRiddle(RiddleID, NewRiddle, NewAnswer);
            }

            public void UpdateExistingRiddle(RiddleBLL r)
            {
                that.DataContext.Riddles.SP_PessimisticUpdateRiddle(r.RiddleID, r.Riddle, r.Answer);

            }

            public void DeleteExistingRiddle(int RiddleID)
            {
                that.DataContext.Riddles.SP_JustDeleteRiddle(RiddleID);
            }

            public void DeleteExistingRiddle(RiddleBLL r)
            {
                DeleteExistingRiddle(r.RiddleID);
            }



            
        }
        #endregion Riddles

        #region Users

        public class UsersHiearchy
        {
            internal UsersHiearchy(BLLContext it) { that = it; }
            internal BLLContext that;

            public List<UserBLL> GetAllUsers()
            {
                List<UserBLL> rv = new List<UserBLL>();
                var r = that.DataContext.Users.SP_ReadAllUsers();
                foreach (var item in r)
                {
                    UserBLL u = new UserBLL(item);
                    rv.Add(u);
                }
                return rv;
            }

            public UserBLL FindByUserID(int UserID)
            {
                var u = that.DataContext.Users.SP_ReadSpecificUser(UserID);
                if (u == null) return null;
                else return new UserBLL(u);
            }

            public UserBLL FindUserByEmail(string email)
            {
                var u = that.DataContext.Users.SP_ReadSpecificUserByEMail(email);
                if (u == null) return null;
                else return new UserBLL(u);
            }

            public List<UserBLL> GetUsersByEmailStartingWith(string pattern)
            {
                var u = that.DataContext.Users.SP_ReadUsersWithEMailStartingWith(pattern);
                List<UserBLL> rv = new List<UserBLL>();
                foreach (var item in u)
                {
                    rv.Add(new UserBLL(item));
                }
                return rv;
            }


            public List<UserBLL> GetUsersByEmailEndingWith(string pattern)
            {
                var u = that.DataContext.Users.SP_ReadUsersWithEMailEndingWith(pattern);
                List<UserBLL> rv = new List<UserBLL>();
                foreach(var item in u)
                {
                    rv.Add(new UserBLL(item));
                }
                return rv;
            }

            public void UpdateUser(int userId, 
                string eMailAddress,
                string name,
                string password,
                string salt,
                string roles,
                string verified,
                string comments
                )
            {

                that.DataContext.Users.SP_PessimisticUpdateUser(userId, eMailAddress, name, password, salt, roles, verified, comments);


            }

            public void UpdateUser(UserBLL user)
            {
                UpdateUser(user.UserID, user.EMailAddress, user.Name, user.Password, user.Salt, user.Roles, user.Verified, user.Comments);
            }

            public UserBLL CreateUser(
                string eMailAddress,
                string name,
                string password,
                string salt,
                string roles,
                string verified,
                string comments
                )
            {

                int id = that.DataContext.Users.SP_CreateUser( eMailAddress, name, password, salt, roles, verified, comments);
                UserBLL rv = new UserBLL()
                {
                    EMailAddress = eMailAddress,
                    Name = name,
                    Password = password,
                    Salt = salt,
                    Roles = roles,
                    Verified = verified,
                    Comments = comments,
                    UserID = id

                };

                return rv;
            }

            public UserBLL CreateUser(UserBLL user)
            {
                return CreateUser(user.EMailAddress, user.Name, user.Password,
                    user.Salt, user.Roles, user.Verified, user.Comments);
            }

            public void DeleteUser(UserBLL user)
            {
                DeleteUser(user.UserID);
            }

            public void DeleteUser(int UserID)
            {
                that.DataContext.Users.SP_JustDeleteUser(UserID);
            }
        
        }
        #endregion

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
                DataContext.Dispose();
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
         ~BLLContext() {
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
