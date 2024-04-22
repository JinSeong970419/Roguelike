using System;

namespace Roguelike.Core
{
    public class KeyColumn : Attribute
    {
        public string Name { get; set; } 

        public KeyColumn()
        {

        }

        public KeyColumn(string name)
        {
            this.Name = name;
        }
    }
}
