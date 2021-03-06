﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server {
    [Serializable]
    public class DataValidationException : Exception {
        public DataValidationException() {
        }

        public DataValidationException(string message) : base(message) {
        }

        public DataValidationException(string message, Exception innerException) : base(message, innerException) {
        }

        protected DataValidationException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
