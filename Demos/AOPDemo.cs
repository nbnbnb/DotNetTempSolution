using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;

namespace Demos
{
    public class AOPDemo
    {
        /// <summary>
        /// 在属性的 get 方法上植入 AOP
        /// </summary>
        public static void PropertyGetAOP()
        {
            AssemblyDefinition asm = AssemblyDefinition.ReadAssembly("Demos.dll");
            foreach (TypeDefinition type in asm.MainModule.Types)
            {
                if (type.Name == "Person")
                {
                    foreach (PropertyDefinition property in type.Properties)
                    {
                        MethodDefinition method = property.GetMethod;


                        Console.WriteLine(".maxstack {0}", method.Body.MaxStackSize);
                        foreach (Instruction ins in method.Body.Instructions)
                        {
                            Console.WriteLine("L_{0}: {1} {2}", ins.Offset.ToString("x4"),
                              ins.OpCode.Name,
                              ins.Operand is String ? String.Format("\"{0}\"", ins.Operand) : ins.Operand);
                        }


                        foreach (Instruction ins in method.Body.Instructions)
                        {
                            if (ins.OpCode.Name == "ldfld" && ins.Operand is FieldDefinition)
                            {
                                Console.WriteLine(ins.Operand);
                                Console.WriteLine(ins.Operand.GetType());
                            }
                        }
                    }
                }
            }
        }
    }

    public class Person
    {

        private Func<String, String> func = m =>
        {
            Console.WriteLine(m);
            return m;
        };

        private String _name;

        public void sayName()
        {
            Console.WriteLine(this.Name);
        }

        public String Name
        {
            get
            {
                return func(_name);

            }
            set { _name = value; }
        }
    }
}
