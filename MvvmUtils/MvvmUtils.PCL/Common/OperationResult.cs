using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saguar.MvvmUtils.Common
{
    public class OperationResult<T> where T : class
    {

        #region Properties

        public T Result { get; set; }

        public bool HasException => (Exception != null && SpecificException != null);

        public T SpecificException { get; set; }

        public Exception Exception { get; set; }

        public string ExceptionSummary
        {
            get
            {
                if (Exception == null && SpecificException == null) return String.Empty;
                return $"{Exception.Message}\n{Exception.StackTrace}";
            }
        }


        #endregion

    }
}
