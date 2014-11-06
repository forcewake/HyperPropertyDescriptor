namespace Hyper.ComponentModel.Example
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Security.Permissions;
    using Hyper.ComponentModel.Example.Entites;

    internal class Program
    {
        /// <summary>
        /// The number of standard operations (GetValue(), etc) to perform
        /// </summary>
        private const int Cycles = 5000000;

        /// <summary>
        /// The number of times to invoke GetProperties (slower)
        /// </summary>
        private const int MetaCycles = 100000;

        private static void Main(string[] args)
        {
            // verify that things work OK without reflection access
            var permission = new ReflectionPermission(ReflectionPermissionFlag.AllFlags);
            permission.Deny();

            Console.WriteLine("Direct access");
            TestDirect(1, false); // for JIT etc
            TestDirect(Cycles, true);

            Console.WriteLine();
            string typeName = typeof (HyperTypeDescriptionProvider).Name;
            Console.WriteLine("Without " + typeName);

            RunTests(1, 1, false); // for JIT etc
            RunTests(MetaCycles, Cycles, true);

            HyperTypeDescriptionProvider.Add(typeof (MyEntity));
            Console.WriteLine();
            Console.WriteLine("With " + typeName);

            RunTests(1, 1, false); // for Emit, JIT etc
            RunTests(MetaCycles, Cycles, true);

            Console.ReadLine();
        }

        private static void RunTests(int metaCount, int count, bool report)
        {
            // note: GC.Collect here in timing mode (report==true) to
            // minimise chance of increasing object count triggering GC
            // for later tests. Not recommended for production code, but
            // acceptable for levelling the field in performance tests.
            if (report)
            {
                GC.Collect();
            }
            Test<MyEntity>(metaCount, count, "Name", report);
            if (report)
            {
                GC.Collect();
            }
            Test<MySuperEntity>(metaCount, count, "Name", report);
            if (report)
            {
                GC.Collect();
            }
            Test<MySuperEntity>(metaCount, count, "When", report);
            if (report)
            {
                GC.Collect();
            }
        }

        private static void TestDirect(int count, bool output)
        {
            // initialise
            MyEntity t = new MyEntity();
            string value = "";

            // GetValue
            Stopwatch getValue = new Stopwatch();
            getValue.Start();
            for (int i = 0; i < count; i++)
            {
                value = t.Name;
            }
            getValue.Stop();

            // SetValue
            Stopwatch setValue = new Stopwatch();
            setValue.Start();
            for (int i = 0; i < count; i++)
            {
                t.Name = value;
            }
            setValue.Stop();

            // ValueChanged
            Stopwatch valueChanged = new Stopwatch();
            valueChanged.Start();
            EventHandler handler = ValueChanged;
            for (int i = 0; i < count; i++)
            {
                t.NameChanged += handler;
                t.NameChanged -= handler;
            }
            valueChanged.Stop();

            if (output)
            {
                Report<MyEntity>("Name", "GetValue", getValue);
                Report<MyEntity>("Name", "SetValue", setValue);
                Report<MyEntity>("Name", "ValueChanged", valueChanged);
                Console.WriteLine("OpCount: " + t.OpCount);
            }
        }

        private static void Test<T>(int metaCount, int count, string name, bool output) where T : MyEntity, new()
        {
            // initialise
            T t = new T();
            PropertyDescriptorCollection props = null;
            PropertyDescriptor property = null;
            object value = null;
            bool isReadOnly = true, supportsChangeEvents = false;

            // GetProperties
            Stopwatch getProperties = new Stopwatch();
            getProperties.Start();
            for (int i = 0; i < metaCount; i++)
            {
                props = TypeDescriptor.GetProperties(t);
            }
            getProperties.Stop();
            if (props != null) property = props[name];

            // IsReadOnly
            Stopwatch isReadOnlyWatch = new Stopwatch();
            isReadOnlyWatch.Start();
            for (int i = 0; i < count; i++)
            {
                isReadOnly = property.IsReadOnly;
            }
            isReadOnlyWatch.Stop();

            // SupportsNotification
            Stopwatch supportsChangeEventsWatch = new Stopwatch();
            supportsChangeEventsWatch.Start();
            for (int i = 0; i < count; i++)
            {
                supportsChangeEvents = property.SupportsChangeEvents;
            }
            supportsChangeEventsWatch.Stop();

            // GetValue
            Stopwatch getValue = new Stopwatch();
            getValue.Start();
            for (int i = 0; i < count; i++)
            {
                value = property.GetValue(t);
            }
            getValue.Stop();

            // SetValue
            Stopwatch setValue = new Stopwatch();
            if (!isReadOnly)
            {
                setValue.Start();
                for (int i = 0; i < count; i++)
                {
                    property.SetValue(t, value);
                }
                setValue.Stop();
            }

            // ValueChanged
            Stopwatch valueChanged = new Stopwatch();
            if (supportsChangeEvents)
            {
                EventHandler handler = ValueChanged;
                valueChanged.Start();
                for (int i = 0; i < count; i++)
                {
                    property.AddValueChanged(t, handler);
                    property.RemoveValueChanged(t, handler);
                }
                valueChanged.Stop();
            }

            if (output)
            {
                Report<T>(name, "GetProperties", getProperties);
                Report<T>(name, "IsReadOnly", isReadOnlyWatch);
                Report<T>(name, "SupportsChangeEvents", supportsChangeEventsWatch);
                Report<T>(name, "GetValue", getValue);
                if (!isReadOnly)
                {
                    Report<T>(name, "SetValue", setValue);
                }
                if (supportsChangeEvents)
                {
                    Report<T>(name, "ValueChanged", valueChanged);
                }
                Console.WriteLine("OpCount: " + t.OpCount);
            }
        }

        private static void Report<T>(string propertyname, string test, Stopwatch watch)
        {
            int ms = (int) Math.Round(watch.Elapsed.TotalMilliseconds);
            Console.WriteLine("{0}.{1}\t{2}\t{3}ms", typeof (T).Name, propertyname, test, ms);
        }

        private static void ValueChanged(object sender, EventArgs args)
        {
        }
    }
}