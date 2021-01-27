namespace RetroGame.Services
{
    class UserManager
    {
        #region Singleton

        private static UserManager _instance;
        public static UserManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UserManager();
                return _instance;
            }
        }

        #endregion

        #region Members

        public string Id;
        public string Username;
        public bool Authorized;

        #endregion

        public UserManager() { }

        public void Logout()
        {
            Id = string.Empty;
            Username = string.Empty;
            Authorized = false;
        }
    }
}
