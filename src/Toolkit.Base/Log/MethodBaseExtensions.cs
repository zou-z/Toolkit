using System.Reflection;

namespace Toolkit.Base.Log
{
    public static class MethodBaseExtensions
    {
        /// <summary>
        /// 获取当前方法的名称 MethodBase.GetCurrentMethod()?.GetMethodName()
        /// </summary>
        /// <param name="methodBase">MethodBase</param>
        /// <returns>当前方法的名称</returns>
        public static string GetMethodName(this MethodBase methodBase)
        {
            return methodBase.DeclaringType?.FullName + "." + methodBase.Name + "()";
        }
    }
}
