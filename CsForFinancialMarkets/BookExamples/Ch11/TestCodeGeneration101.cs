using System;
using System.Reflection.Emit;
using System.Reflection;

namespace CodeGeneration101
{
    public class Test
    {
        delegate int BinaryFunction(int n1, int n2);
        delegate int TernaryFunction(int n1, int n2, int n3);

        static void Main()
        {
            DynamicMethod method = new DynamicMethod("MyFirst", null, null, typeof(Test));
            ILGenerator gen = method.GetILGenerator();

            gen.EmitWriteLine((string)"Hello World");
            gen.Emit(OpCodes.Ret);

            method.Invoke(null, null);

            // Now use delegates
            DynamicMethod method2 = new DynamicMethod("MySecond", typeof(int), new[] {typeof (int), typeof (int)}, typeof(void));
            ILGenerator gen2 = method2.GetILGenerator();

            // Put arguments onto evaluation stack
            gen2.Emit(OpCodes.Ldarg_0);
            gen2.Emit(OpCodes.Ldarg_1);
            gen2.Emit(OpCodes.Add);
            gen2.Emit(OpCodes.Ret);

            BinaryFunction f2 = (BinaryFunction)method2.CreateDelegate(typeof(BinaryFunction));
            Console.WriteLine("Using delegate: {0}",f2(1, 2));   // 3

            int result = (int)method2.Invoke(null, new object[] { 2, 4 });


            // Now invoke the dynamically generated method
            Console.WriteLine("Using delegate: {0}", f2(2, 6));       // 8
            Console.WriteLine("Using delegate: {0}", f2(12, 8));      // 20
            Console.WriteLine("Using delegate: {0}", f2(-12, 8));     // -4

            // Using a delegate with 3 input parameters; compute n1*n2*n3
            DynamicMethod method3 = new DynamicMethod("MyThird", typeof(int), new[] { typeof(int), typeof(int), typeof(int) }, typeof(void));
            ILGenerator gen3 = method3.GetILGenerator();

            // Put arguments onto evaluation stack
            gen3.Emit(OpCodes.Ldarg_0);
            gen3.Emit(OpCodes.Ldarg_1);
            gen3.Emit(OpCodes.Mul);
            gen3.Emit(OpCodes.Ldarg_2);
            gen3.Emit(OpCodes.Mul);
            gen3.Emit(OpCodes.Ret);
            
            TernaryFunction f3 = (TernaryFunction)method3.CreateDelegate(typeof(TernaryFunction));
            Console.WriteLine("Using delegate: {0}", f3(1, 2, 2));   // 3

            int result3 = (int)method3.Invoke(null, new object[] { 2, 4, 6 });


            // Now invoke the dynamically generated method
            Console.WriteLine("Using delegate: {0}", f3(2, 6, 1));       // 12
            Console.WriteLine("Using delegate: {0}", f3(12, 8, 2));      // 192
            Console.WriteLine("Using delegate: {0}", f3(-12, 2, 2));     // -48
            
        }
    }
}
