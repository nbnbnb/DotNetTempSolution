using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Temp
    {
        public void SayName()
        {
            Console.WriteLine(123);

        }
    }

    public class PropertyRead : IPropertyRead
    {
        public void Read()
        {
            Console.WriteLine("Read");
        }
    }

    public class PropertyWrite : IPropertyWrite
    {
        public void Write()
        {
            Console.WriteLine("Write");
        }
    }

    public class PropertyDelete : IPropertyDelete
    {
        public void Delete()
        {
            Console.WriteLine("Delete");
        }
    }

    public interface IPropertyRead
    {
        void Read();
    }

    public interface IPropertyWrite
    {
        void Write();
    }

    public interface IPropertyDelete
    {
        void Delete();
    }

    public interface IPropertyFactory<IProperty, IPropertyXXX>
    {
        IPropertyXXX Build(IProperty property);
    }

    public class ReadPropertyFactory : IPropertyFactory<PropertyInfo, IPropertyRead>
    {
        public IPropertyRead Build(PropertyInfo property)
        {
            return new PropertyRead();
        }
    }

    public class WritePropertyFactory : IPropertyFactory<PropertyInfo, IPropertyWrite>
    {
        public IPropertyWrite Build(PropertyInfo property)
        {
            return new PropertyWrite();
        }
    }

    public class DeletePropertyFactory : IPropertyFactory<PropertyInfo, IPropertyDelete>
    {
        public IPropertyDelete Build(PropertyInfo property)
        {
            return new PropertyDelete();
        }
    }

}
