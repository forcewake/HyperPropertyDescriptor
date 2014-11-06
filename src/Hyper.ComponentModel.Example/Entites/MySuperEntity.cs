namespace Hyper.ComponentModel.Example.Entites
{
    using System;

    public class MySuperEntity : MyEntity
    {
        private DateTime when;

        public DateTime When
        {
            get
            {
                opCount++;
                return when;
            }
            set
            {
                opCount++;
                when = value;
            }
        }
    }
}