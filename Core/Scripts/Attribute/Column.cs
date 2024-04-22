using System;

namespace Roguelike.Core
{
    public class Column : Attribute
    {
        public string Name { get; set; }
        public Column()
        {
        }

        public Column(string name)
        {
            this.Name = name;
        }
    }
}
