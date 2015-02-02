using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Moq;
using NUnit.Framework;

namespace PboBuilderTests
{
    public static class StaticUtilities
    {
        public static void AssertExceptionConstructorsExist(Type type)
        {
            Assert.IsTrue(type.IsSubclassOf(typeof (Exception)), "Provided type doesn't inherit from exception.");

            var constructors = type.GetConstructors();
            Assert.AreEqual(4, constructors.Length);

            // We have the correct number of constructors. Now we must verify their signatures.
            ConstructorInfo parameterlessConstructor = null;
            foreach (var constructorInfo in constructors)
            {
                Exception instance = null;
                var parameters = constructorInfo.GetParameters();
                var parameterCount = parameters.Length;
                switch (parameterCount)
                {
                    case 0:
                        instance = (Exception)constructorInfo.Invoke(null);
                        parameterlessConstructor = constructorInfo;
                        break;
                    case 1:
                        instance = (Exception)constructorInfo.Invoke(new object[] { "message" });
                        break;
                    case 2:
                        if (parameters[0].ParameterType == typeof(string))
                        {
                            instance = (Exception)constructorInfo.Invoke(new object[] { "message", new Exception() });
                        }

                        if (parameters[0].ParameterType == typeof(SerializationInfo) && parameterlessConstructor  != null)
                        {
                            // Round-trip the exception: Serialize and de-serialize with a BinaryFormatter
                            var bf = new BinaryFormatter();
                            using (var ms = new MemoryStream())
                            {
                                // "Save" object state
                                var ex = (Exception)parameterlessConstructor.Invoke(null);
                                bf.Serialize(ms, ex);

                                // Re-use the same stream for de-serialization
                                ms.Seek(0, 0);

                                // Replace the original exception with de-serialized one
                                instance = (Exception)bf.Deserialize(ms);
                            }
                        }
                        break;
                }

                Assert.IsNotNull(instance);
            }
        }
    }
}