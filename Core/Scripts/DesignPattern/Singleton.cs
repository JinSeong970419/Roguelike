namespace Roguelike.Core
{
    public abstract class Singleton<T> where T : Singleton<T> , new()
    {
        private static T instance = null;
        public static T Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
        // TODO : 상속받은 클래스 내부에서 인스턴스가 여러개 생성되는 현상이 있음. 수정해야 함.
        protected Singleton() { }
    }
}