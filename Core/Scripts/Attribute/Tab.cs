using System;

namespace Roguelike.Core
{
    public class Tab : Attribute
    {
        public string Name { get; set; }

        public Tab()
        {
        }
        public Tab(string name)
        {
            this.Name = name;
        }
    }
}
