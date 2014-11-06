namespace Hyper.ComponentModel.Example.Entites
{
    using System;

    public class MyEntity
    {
        // opCount is a marker of work done; make protected (as an exception)
        // to minimise impact
        protected int opCount;
        private string name;
        private EventHandler _nameChanged;

        public event EventHandler NameChanged
        {
            add
            {
                opCount++;
                _nameChanged += value;
            }
            remove
            {
                opCount++;
                _nameChanged -= value;
            }
        }

        public string Name
        {
            get
            {
                opCount++;
                return name;
            }
            set
            {
                opCount++;
                if (value != Name)
                {
                    name = value;
                    EventHandler handler = _nameChanged;
                    if (handler != null)
                    {
                        handler(this, EventArgs.Empty);
                    }
                }
            }
        }

        public int OpCount
        {
            get { return opCount; }
        }
    }
}