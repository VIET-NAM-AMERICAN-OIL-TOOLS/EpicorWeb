using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicorWeb.Attributes
{
    /// <summary>
    /// (False) = ẩn
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class ConvertToDataTableColumnAttribute : Attribute
    {
        public bool IsConvert { get; private set; }
		public string HeaderName { get; private set; }
		public bool IsRichtextbox { get; private set; }
		public ConvertToDataTableColumnAttribute(bool isConvert)
        {
            this.IsConvert = isConvert;
        }
		
	}
}
