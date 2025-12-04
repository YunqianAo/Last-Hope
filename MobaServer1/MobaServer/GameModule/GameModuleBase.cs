using System;
using System.Collections.Generic;
using System.Text;

namespace MobaServer.GameModule
{
    public class GameModuleBase<T> where T : new()
    {
        static T instance;
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
        public virtual void Init()
        {
            AddListener();
        }
        public virtual void Release()
        {
            RemoveListener();
        }
        public virtual void AddListener()
        {

        }
        public virtual void RemoveListener()
        {

        }
    }
}
