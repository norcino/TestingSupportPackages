//using System;
//using System.Reflection;

//namespace Builder
//{
//    public class BuildFrom<TE> where TE : class, new()
//    {
//        private TE SourceToCopyFrom;

//        public BuildFrom(TE source)
//        {
//            SourceToCopyFrom = (TE)Copy(source);
//        }

//        private object Copy(object source)
//        {
//            var copy = Activator.CreateInstance(source.GetType());

//            var properties = copy.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

//            foreach (var item in properties)
//            {
//                if (item.CanWrite)
//                {
//                    if (item.PropertyType.IsValueType || item.PropertyType.IsEnum || item.PropertyType.Equals(typeof(System.String)))
//                    {
//                        item.SetValue(copy, item.GetValue(source, null), null);
//                    }
//                    else
//                    {
//                        object opPropertyValue = item.GetValue(source, null);
//                        if (opPropertyValue == null)
//                        {
//                            item.SetValue(copy, null, null);
//                        }
//                        else
//                        {
//                            item.SetValue(copy, Copy(opPropertyValue), null);
//                        }
//                    }
//                }
//            }

//            var fields = copy.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

//            foreach (var item in fields)
//            {
//                if (item.FieldType.IsValueType || item.FieldType.IsEnum || item.FieldType.Equals(typeof(System.String)))
//                {
//                    item.SetValue(copy, item.GetValue(source, null), null);
//                }
//                else
//                {
//                    object opPropertyValue = item.GetValue(source, null);
//                    if (opPropertyValue == null)
//                    {
//                        item.SetValue(copy, null, null);
//                    }
//                    else
//                    {
//                        item.SetValue(copy, Copy(opPropertyValue), null);
//                    }
//                }
//            }

//            return copy;
//        }

//        public TE Build(Action<TE> entitySetupAction = null)
//        {
//            if (entitySetupAction == null)
//                throw new ArgumentNullException($"{nameof(entitySetupAction)}");

//            if(entitySetupAction != null)
//                entitySetupAction(SourceToCopyFrom);

//            return SourceToCopyFrom;
//        }
//    }
//}
