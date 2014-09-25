using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.CodeDom.Compiler;
using System.Reflection;

namespace BPMS.Common
{
    /// <summary>
    /// 计算公式帮助类
    /// </summary>
    public class FormulaHelper
    {
        /// <summary>
        /// 计算结果,如果表达式出错则抛出异常
        /// </summary>
        /// <param name="statement">表达式,如"1+2+3+4"</param>
        /// <returns>结果</returns>
        public static object Eval(string statement)
        {
            if (_evaluatorType == null || _evaluator == null)
            {
                Evaluator();
            }
            return _evaluatorType.InvokeMember(
                        "Eval",
                        BindingFlags.InvokeMethod,
                        null,
                        _evaluator,
                        new object[] { statement }
                     );
        }

        static void Evaluator()
        {
            //构造JScript的编译驱动代码
            CodeDomProvider provider = CodeDomProvider.CreateProvider("JScript");

            CompilerParameters parameters;
            parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;

            CompilerResults results;
            results = provider.CompileAssemblyFromSource(parameters, _jscriptSource);

            Assembly assembly = results.CompiledAssembly;
            _evaluatorType = assembly.GetType("Evaluator");

            _evaluator = Activator.CreateInstance(_evaluatorType);
        }

        private static object _evaluator = null;
        private static Type _evaluatorType = null;

        /// <summary>
        /// JScript代码
        /// </summary>
        private static readonly string _jscriptSource =
            @"class Evaluator
              {
                  public function Eval(expr : String) : String 
                  { 
                     return eval(expr); 
                  }
              }";
    }
}
