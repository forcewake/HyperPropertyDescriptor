namespace Hyper.ComponentModel.Example.Descriptors
{
    using System;
    using System.ComponentModel;
    using Hyper.ComponentModel.Example.Entites;

    public sealed class MyEntityNamePropertyDescriptor : ChainingPropertyDescriptor
    {
        public MyEntityNamePropertyDescriptor(PropertyDescriptor parent) : base(parent)
        {
        }

        public override object GetValue(object component)
        {
            return ((MyEntity) component).Name;
        }

        public override void SetValue(object component, object value)
        {
            ((MyEntity) component).Name = (string) value;
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override bool SupportsChangeEvents
        {
            get { return true; }
        }

        public override void AddValueChanged(object component, EventHandler handler)
        {
            ((MyEntity) component).NameChanged += handler;
        }

        public override void RemoveValueChanged(object component, EventHandler handler)
        {
            ((MyEntity) component).NameChanged -= handler;
        }
    }
}