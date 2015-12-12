using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Helper
{
    public class StaticMemberDynamicWrapper : DynamicObject
    {
        private readonly TypeInfo _type;
        public StaticMemberDynamicWrapper(Type type)
        {
            _type = type.GetTypeInfo();
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _type.DeclaredMembers.Select(m => m.Name);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            var field = FindField(binder.Name);
            if (field != null)
            {
                result = field.GetValue(null);
                return true;
            }
            var prop = FindProperty(binder.Name, true);
            if (prop != null)
            {
                result = prop.GetValue(null, null);
                return true;
            }

            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var field = FindField(binder.Name);
            if (field != null)
            {
                field.SetValue(null, value);
                return true;
            }

            var prop = FindProperty(binder.Name, false);
            if (prop != null)
            {
                prop.SetValue(null, value, null);
                return true;
            }

            return false;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            MethodInfo method = FindMethod(binder.Name, args.Select(m => m.GetType()).ToArray());
            if (method == null)
            {
                result = null;
                return false;
            }

            result = method.Invoke(null, args);

            return true;
        }

        private MethodInfo FindMethod(String name, Type[] paramTypes)
        {
            return _type.DeclaredMethods.FirstOrDefault(m => m.IsPublic && m.IsStatic
            && m.Name == name && ParametersMatch(m.GetParameters(), paramTypes));
        }

        private bool ParametersMatch(ParameterInfo[] parameters, Type[] paramTypes)
        {
            if (parameters.Length != paramTypes.Length)
            {
                return false;
            }

            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (parameters[i].ParameterType != paramTypes[i])
                {
                    return false;
                }
            }

            return true;
        }

        private FieldInfo FindField(String name)
        {
            return _type.DeclaredFields.FirstOrDefault(m => m.IsPublic && m.IsStatic && m.Name == name);
        }

        private PropertyInfo FindProperty(string name, bool get)
        {
            if (get)
            {
                return _type.DeclaredProperties.FirstOrDefault(m => m.Name == name && m.GetMethod != null
                && m.GetMethod.IsPublic && m.GetMethod.IsStatic);
            }

            return _type.DeclaredProperties.FirstOrDefault(m => m.Name == name && m.SetMethod != null
               && m.SetMethod.IsPublic && m.SetMethod.IsStatic);
        }
    }
}
