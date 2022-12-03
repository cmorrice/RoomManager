namespace UnitePlugin.ViewModel.Factory
{
    public static class SingletonViewModelFactory<T> where T : new()
    {
        private static T _instance;

        public static T GetInstance
        {
            get
            {
                if(_instance == null)
                    _instance = Getobject();

                return _instance;
            }
        }

        private static T Getobject()
        {
            return new T();
        }



    }
}
