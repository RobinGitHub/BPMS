using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BPMS.Foundatoin
{
    /// <summary>
    /// 此类是Model的抽象基类
    /// </summary>
    public abstract class BaseModel : IBaseModel, IDataErrorInfo
    {
        public string Error
        {
            get
            {
                foreach (string property in this.ValidatedProperties())
                {
                    string validationError = GetValidationError(property);
                    if (!string.IsNullOrEmpty(validationError))
                        return validationError;
                }

                return null;
            }
        }
        public string this[string propertyName]
        {
            get { return this.GetValidationError(propertyName); }
        }
        public bool IsValid
        {
            get
            {
                return Error != null;
            }
        }

        /// <summary>
        /// 指示对象哪些属性需要验证
        /// </summary>
        /// <returns></returns>
        protected abstract string[] ValidatedProperties();

        /// <summary>
        /// 实现每个属性的验证逻辑
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性验证不通过的错误信息</returns>
        protected abstract string GetValidationError(string propertyName);
    }
}
